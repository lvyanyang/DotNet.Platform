// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Service;
using DotNet.Auth.Utility;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Data;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 报名学员服务
    /// </summary>
    public class StudentService
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        internal StudentService()
        {
        }

        /// <summary>
        /// 是否存在指定名称的对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="idCardNo">身份证号码</param>
        /// <returns>如果存在返回false</returns>
        public BoolMessage ExistsByIDCardNo(string id, string idCardNo)
        {
            var repos = new EduRepository<Student>();
            var has = repos.Exists(p => p.IDCardNo == idCardNo && p.Id != id && p.Status != 5);
            return has ? new BoolMessage(false, "输入学员身份证号码已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Student entity)
        {
            if (entity.IDCardNo.IsEmpty()) throw new ArgumentException("请输入学员身份证号码");
            if (entity.IDCardNo.Length < 15) throw new ArgumentException("身份证号码必须是15或者18位");

            var user = AuthHelper.GetSessionUser();
            var repos = new EduRepository<Student>();
            if (user.IsCompany)
            {
                entity.CompanyId = user.User.CompanyId;
                entity.CompanyName = user.User.CompanyName;
            }
            entity.Password = StringHelper.EncryptString(entity.IDCardNo.Substring(entity.IDCardNo.Length - 6));
            entity.CreateDateTime = DateTime.Now;
            entity.Status = user.IsCompany ? 0 : 1; //如果是企业状态为预报名状态,如果是培训机构为报名状态
            entity.TrainCycle = GetStudentTrainCycle(entity.IDCardNo);
            repos.Insert(entity);
            EduService.StudentLog.Create(entity.Id, entity.Name, user.IsCompany ? "企业录入学员信息" : "录入学员信息");
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Student entity)
        {
            #region 更新列
            var cols = new[]
            {
                nameof(entity.Name), //姓名
                nameof(entity.Spell), //简拼
                nameof(entity.Sex), //性别
                nameof(entity.DrivingModel), //准驾车型
                nameof(entity.Address), //住址
                nameof(entity.MobilePhone), //手机号码
                nameof(entity.Certificate), //从业资格证号
                nameof(entity.CertificateDate), //获得资格证时间
                nameof(entity.StartDate), //证件有效开始日期
                nameof(entity.EndDate), //证件有效结束日期
                nameof(entity.Photo), //照片
                nameof(entity.TrainReason), //培训原因
                nameof(entity.Note), //备注
                nameof(entity.WorkCategoryId),
                nameof(entity.WorkCategoryName),
                nameof(entity.CompanyId),
                nameof(entity.CompanyName)//公司名称
            };
            #endregion
            var user = AuthHelper.GetSessionUser();
            if (user.IsCompany)
            {
                entity.CompanyId = user.User.CompanyId;
                entity.CompanyName = user.User.CompanyName;
            }
            var repos = new EduRepository<Student>();
            repos.Update(entity, cols);
            EduService.StudentLog.Create(entity.Id, entity.Name, user.IsCompany ? "企业修改学员信息" : "修改学员信息");
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新学员的班级信息、状态信息
        /// </summary>
        /// <param name="studentId">学员主键</param>
        /// <param name="trainGroupId">班级主键</param>
        /// <param name="trainGroupName">班级名称</param>
        /// <param name="status">状态</param>
        /// <param name="reservationDateTime">预约时间</param>
        public BoolMessage UpdateGroupStatus(string studentId, string trainGroupId,
            string trainGroupName, int status, DateTime? reservationDateTime)
        {
            #region 更新列
            var cols = new[]
            {
                nameof(Student.TrainGroupId), //班级主键
                nameof(Student.TrainGroupName), //班级名称
                nameof(Student.ReservationDateTime), //预约时间
                nameof(Student.Status) //状态
            };
            #endregion

            var entity = new Student();
            entity.Id = studentId;
            entity.TrainGroupId = trainGroupId;
            entity.TrainGroupName = trainGroupName;
            entity.ReservationDateTime = reservationDateTime;
            entity.Status = status;
            var repos = new EduRepository<Student>();
            repos.Update(entity, cols);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取班级人数
        /// </summary>
        /// <param name="trainGroupId">班级主键</param>
        public int GetGroupCount(string trainGroupId)
        {
            return new EduRepository<Student>().Count(p => p.TrainGroupId == trainGroupId && p.Status == 2);
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Student entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        // <summary>
        /// 学员登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码(明文)</param>
        /// <returns>操作成功返回True</returns>
        public BoolMessage Login(string account, string password)
        {
            account = account.Trim();
            var entity = GetByIDCardNoOrMobile(account);
            if (entity == null)
            {
                return new BoolMessage(false, "无效的账号");
            }

            #region 检测条件

            var encryptedPwd = StringHelper.EncryptString(password);
            if (string.IsNullOrEmpty(entity.Password) || !entity.Password.Equals(encryptedPwd))
            {
                return new BoolMessage(false, "账号密码错误");
            }

            if (entity.Status != 2)
            {
                return new BoolMessage(false, "此学员还没有开通学习权限,请联系学校管理员");
            }

            #endregion

            return new BoolMessage(true);
        }


        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Student Get(string id)
        {
            return new EduRepository<Student>().Get(id);
        }

        /// <summary>
        /// 获取学员对象使用身份证或者手机号码
        /// </summary>
        /// <param name="studentAccount">学员账号</param>
        public Student GetByIDCardNoOrMobile(string studentAccount)
        {
            var repos = new EduRepository<Student>();
            if (studentAccount.Length == 11)
            {
                return repos.Get(p => p.MobilePhone == studentAccount);
            }
            return repos.Get(p => p.IDCardNo == studentAccount);
        }

        /// <summary>
        /// 获取学员状态选项
        /// </summary>
        public List<Auth.Entity.DicDetail> GetStudentStatus()
        {
            return AuthService.DicDetail.GetListByDicCode("StudentStatus");
        }

        /// <summary>
        /// 获取启用的对象集合(已排序)
        /// </summary>
        public List<Student> GetList()
        {
            var repos = new EduRepository<Student>();
            return repos.Fetch();
        }

        /// <summary>
        /// 填充主键
        /// </summary>
        /// <param name="list"></param>
        public List<BoolMessage> FillSimpleId(List<StudentSimple> list)
        {
            var results = new List<BoolMessage>();
            var repos = new EduRepository<Student>();
            foreach (StudentSimple simple in list)
            {
                var entity = repos.Get(p => p.IDCardNo == simple.IDCardNo, p => p.Id);
                if (entity != null)
                {
                    simple.Id = entity.Id;
                }
                else
                {
                    results.Add(new BoolMessage(false, $"身份证号码:{simple.IDCardNo}无效"));
                }
            }
            return results;
        }

        /// <summary>
        /// 学员导入
        /// </summary>
        /// <param name="students">学员列表</param>
        public List<BoolMessage> Import(List<Student> students)
        {
            if (students == null) return new List<BoolMessage>();
            var results = new List<BoolMessage>();

            var user = AuthHelper.GetSessionUser();
            var repos = new EduRepository<Student>();
            foreach (var entity in students)
            {
                #region check

                if (entity.Name.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定姓名"));
                    continue;
                }
                if (entity.IDCardNo.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定身份证号码"));
                    continue;
                }
                if (entity.IDCardNo.Length < 15)
                {
                    results.Add(new BoolMessage(false, "身份证号码必须是15或者18位"));
                    continue;
                }
                if (entity.SchoolName.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定培训机构"));
                }
                if (entity.WorkCategoryName.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定从业类别"));
                }
                if (entity.MobilePhone.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定手机号码"));
                }

                #endregion

                //1.检测学员是否已经报名
                var has = repos.Exists(p => p.IDCardNo == entity.IDCardNo && p.Status != 5);
                //2.如果已经报名返回错误信息
                if (has)
                {
                    results.Add(new BoolMessage(false, $"学员 {entity.Name} 已经报名,所在企业 {entity.CompanyName}"));
                    continue;
                }
                //3.新增学员

                #region check
                entity.WorkCategoryId = AuthService.DicDetail.GetValueByName("TrainCategory",
                    entity.WorkCategoryName);
                if (entity.WorkCategoryId.IsEmpty())
                {
                    results.Add(new BoolMessage(false, $"学员 {entity.Name} 从业类别输入错误"));
                    continue;
                }

                entity.SchoolId = EduService.School.GetIdByName(entity.SchoolName);
                if (entity.SchoolId.IsEmpty())
                {
                    results.Add(new BoolMessage(false, $"学员 {entity.Name} 培训机构输入错误"));
                    continue;
                }

                entity.CompanyId = EduService.Company.GetIdByName(entity.CompanyName);
                if (entity.CompanyId.IsEmpty())
                {
                    results.Add(new BoolMessage(false, $"学员 {entity.Name} 企业输入错误"));
                    continue;
                }
                #endregion

                if (user.IsCompany)
                {
                    entity.CompanyId = user.User.CompanyId;
                    entity.CompanyName = user.User.CompanyName;
                }
                entity.Id = StringHelper.Guid();
                entity.Password = StringHelper.EncryptString(entity.IDCardNo.Substring(entity.IDCardNo.Length - 6));
                entity.Spell = entity.Name.Spell();
                entity.CreateDateTime = DateTime.Now;
                entity.Status = user.IsCompany ? 0 : 1; //如果是企业状态为预报名状态,如果是培训机构为报名状态
                entity.TrainCycle = GetStudentTrainCycle(entity.IDCardNo);
                repos.Insert(entity);
                EduService.StudentLog.Create(entity.Id, entity.Name, user.IsCompany ? "企业导入学员信息" : "导入学员信息");
                results.Add(BoolMessage.True);
            }
            return results;
        }

        /// <summary>
        /// 学员审核入库
        /// </summary>
        /// <param name="studentIds">学员主键数组</param>
        /// <returns></returns>
        public List<BoolMessage> StockIn(string[] studentIds)
        {
            if (studentIds == null || studentIds.Length == 0) return new List<BoolMessage>();
            var results = new List<BoolMessage>();

            var repos = new EduRepository<Student>();
            foreach (var id in studentIds)
            {
                var entity = repos.Get(p => p.Id == id);

                if (entity.Status != 0)
                {
                    results.Add(new BoolMessage(false, $"学员 {entity.Name} 已经入库"));
                    continue;
                }
                entity.Status = 1;
                entity.RegDateTime = DateTime.Now;
                repos.Update(entity, new[] { nameof(entity.Status), nameof(entity.RegDateTime) });
                EduService.StudentLog.Create(entity.Id, entity.Name, "学员入库");
                results.Add(BoolMessage.True);
            }
            return results;
        }

        /// <summary>
        /// 企业删除学员
        /// </summary>
        /// <param name="studentIds">学员主键数组</param>
        public List<BoolMessage> CompanyStudentDelete(string[] studentIds)
        {
            if (studentIds == null || studentIds.Length == 0) return new List<BoolMessage>();
            var results = new List<BoolMessage>();

            var repos = new EduRepository<Student>();
            foreach (var id in studentIds)
            {
                var entity = repos.Get(p => p.Id == id, p => p.Id, p => p.Name, p => p.Status);
                if (entity == null)
                {
                    results.Add(new BoolMessage(false, $"学员{id}已经删除"));
                    continue;
                }
                if (entity.Status != 0)
                {
                    results.Add(new BoolMessage(false, $"学员 {entity.Name} {entity.StatusName} 无法删除,只有预报名状态才能删除"));
                    continue;
                }
                repos.Delete(id);
                results.Add(BoolMessage.True);
            }
            return results;
        }

        /// <summary>
        /// 学校删除学员
        /// </summary>
        /// <param name="studentIds">学员主键数组</param>
        public List<BoolMessage> SchoolStudentDelete(string[] studentIds)
        {
            if (studentIds == null || studentIds.Length == 0) return new List<BoolMessage>();
            var results = new List<BoolMessage>();

            var repos = new EduRepository<Student>();
            foreach (var id in studentIds)
            {
                var entity = repos.Get(p => p.Id == id, p => p.Id, p => p.Name, p => p.Status);
                if (entity == null)
                {
                    results.Add(new BoolMessage(false, $"学员{id}已经删除"));
                    continue;
                }
                if (entity.Status != 1)
                {
                    results.Add(new BoolMessage(false, $"学员 {entity.Name} {entity.StatusName} 无法删除,只有报名状态才能删除"));
                    continue;
                }
                repos.Delete(id);
                results.Add(BoolMessage.True);
            }
            return results;
        }

        /// <summary>
        /// 获取学员培训周期
        /// </summary>
        /// <param name="idCardNo">学员身份证号码</param>
        public int GetStudentTrainCycle(string idCardNo)
        {
            var repos = new EduRepository<Student>();
            var count = repos.Count(p => p.IDCardNo == idCardNo && p.Status == 5);
            return count + 1;
        }


        /// <summary>
        /// 获取启用的对象集合
        /// </summary>
        public List<Student> GetList(StudentSearchParam searchParam)
        {
            var repos = new EduRepository<Student>();
            var query = repos.SQL.OrderByAsc(p => p.CreateDateTime);
            SetQuery(searchParam, query);
            return repos.Fetch(query);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="searchParam">查询参数</param>
        public PageList<Student> GetPageList(PaginationCondition pageCondition, StudentSearchParam searchParam)
        {
            pageCondition.SetDefaultOrder(nameof(Student.CreateDateTime));
            var repos = new EduRepository<Student>();
            var query = repos.PageQuery(pageCondition);
            SetQuery(searchParam, query);
            return repos.Page(query);
        }

        private void SetQuery(StudentSearchParam searchParam, SQLQuery<Student> query = null)
        {
            if (query == null)
            {
                query = new SQLQuery<Student>();
            }
            if (searchParam.Name.IsNotEmpty()) //姓名
            {
                searchParam.Name = searchParam.Name.Trim();
                query.Where(p => p.Name.Contains(searchParam.Name) || p.Spell.Contains(searchParam.Name));
            }

            if (searchParam.IDCardNo.IsNotEmpty()) //身份证号码
            {
                searchParam.IDCardNo = searchParam.IDCardNo.Trim();
                query.Where(p => p.IDCardNo.Contains(searchParam.IDCardNo));
            }

            if (searchParam.CompanyId.IsNotEmpty()) //工作单位
            {
                query.Where(p => p.CompanyId == searchParam.CompanyId);
            }

            if (searchParam.TrainGroupId.IsNotEmpty()) //所属班级
            {
                query.Where(p => p.TrainGroupId == searchParam.TrainGroupId);
            }

            if (searchParam.SchoolId.IsNotEmpty()) //学校
            {
                query.Where(p => p.SchoolId == searchParam.SchoolId);
            }

            if (searchParam.WorkCategoryId.IsNotEmpty()) //从业类型
            {
                query.Where(p => p.WorkCategoryId == searchParam.WorkCategoryId);
            }

            if (searchParam.RegStartDate.HasValue)//报名开始日期
            {
                query.Where(p => p.RegDateTime >= searchParam.RegStartDate.Value);
            }
            if (searchParam.RegEndDate.HasValue)//报名结束日期
            {
                query.Where(p => p.RegDateTime <= searchParam.RegEndDate.Value.AddDays(1));
            }

            if (searchParam.CreateStartDate.HasValue)//录入开始日期
            {
                query.Where(p => p.CreateDateTime >= searchParam.CreateStartDate.Value);
            }
            if (searchParam.CreateEndDate.HasValue)//录入结束日期
            {
                query.Where(p => p.CreateDateTime <= searchParam.CreateEndDate.Value.AddDays(1));
            }

            if (searchParam.Status.HasValue) //状态
            {
                query.Where(p => p.Status == searchParam.Status.Value);
            }
        }
    }
}
