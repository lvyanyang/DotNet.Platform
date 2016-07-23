// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Utility;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Data;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 班级信息服务
    /// </summary>
    public class TrainGroupService
    {
        private static readonly Cache<string, TrainGroup> Cache = new Cache<string, TrainGroup>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal TrainGroupService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            Cache.Clear().Set(new EduRepository<TrainGroup>().Query().ToDictionary(p => p.Id, p => p));
        }

        /// <summary>
        /// 是否存在指定名称的对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="name">名称</param>
        /// <param name="categoryId">业务类型主键</param>
        /// <param name="schoolId">培训机构主键</param>
        /// <returns>如果存在返回false</returns>
        public BoolMessage ExistsByName(string id, string name, string schoolId, string categoryId)
        {
            var has = Cache.ValueList()
                .Contains(p => p.Name.Equals(name) && p.CategoryId.Equals(categoryId)
                    && p.SchoolId.Equals(schoolId) && !p.Id.Equals(id));
            return has ? new BoolMessage(false, "指定的班级名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(TrainGroup entity)
        {
            var repos = new EduRepository<TrainGroup>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(TrainGroup entity)
        {
            #region 更新列
            var cols = new[]
            {
                nameof(entity.Name), //名称
                nameof(entity.Spell), //简拼
                nameof(entity.CategoryId), //业务类型主键
                nameof(entity.CategoryName), //业务类型名称
                nameof(entity.SchoolId), //培训学校主键
                nameof(entity.SchoolName), //培训学校名称
                nameof(entity.StartDate), //开始日期
                nameof(entity.EndDate), //结束日期
                nameof(entity.Note) //备注
            };
            #endregion

            var repos = new EduRepository<TrainGroup>();
            repos.Update(entity, cols);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(TrainGroup entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 学员预约
        /// </summary>
        /// <param name="trainGroupId">班级主键</param>
        /// <param name="studentIds">学员主键数组</param>
        public List<BoolMessage> Reservation(string trainGroupId, string[] studentIds)
        {
            return ReservationCore(trainGroupId, studentIds, false);
        }

        /// <summary>
        /// 取消学员预约
        /// </summary>
        /// <param name="trainGroupId">班级主键</param>
        /// <param name="studentIds">学员主键数组</param>
        public List<BoolMessage> UnReservation(string trainGroupId, string[] studentIds)
        {
            return ReservationCore(trainGroupId, studentIds, true);
        }

        private List<BoolMessage> ReservationCore(string trainGroupId, string[] studentIds, bool isCancel)
        {
            var results = new List<BoolMessage>();
            try
            {
                if (!Cache.Contains(trainGroupId))
                {
                    results.Add(new BoolMessage(false, "无效的班级主键"));
                    return results;
                }
                DbSession.Begin(new EduDatabase());
                foreach (var id in studentIds)
                {
                    //检测学员是否已经预约
                    var studentEntity = EduService.Student.Get(id);
                    if (studentEntity == null)
                    {
                        results.Add(new BoolMessage(false, "无效的学员主键"));
                        continue;
                    }

                    var checkStatus = isCancel ? 2 : 1;
                    var checkMessage = isCancel ? $"学员 { studentEntity.Name} 状态是 { studentEntity.StatusName}; 必须是学习中状态的学员才能取消预约" :
                                                $"学员 {studentEntity.Name} 状态是 {studentEntity.StatusName};必须是报名状态的学员才能预约";
                    if (studentEntity.Status != checkStatus)
                    {
                        results.Add(new BoolMessage(false, checkMessage));
                        continue;
                    }

                    var studentStatus = isCancel ? 1 : 2;
                    DateTime? reservationDateTime = null;
                    string reservationTrainGroupId = null;
                    string reservationTrainGroupName = null;
                    if (!isCancel)
                    {
                        var groupEntity = Get(trainGroupId);
                        reservationDateTime = DateTime.Now;
                        reservationTrainGroupId = groupEntity.Id;
                        reservationTrainGroupName = groupEntity.Name;
                    }

                    //1.更新学员表中的班级信息、状态信息
                    
                    EduService.Student.UpdateGroupStatus(studentEntity.Id, reservationTrainGroupId,
                        reservationTrainGroupName, studentStatus, reservationDateTime);

                    //2.更新班级表中的状态、人数
                    var num = EduService.Student.GetGroupCount(trainGroupId);
                    var status = num > 0 ? 1 : 0;
                    UpdateStatus(trainGroupId, num, status);
                    EduService.StudentLog.Create(studentEntity.Id, studentEntity.Name, isCancel ? "学员取消预约" : "学员预约");
                    results.Add(BoolMessage.True);
                }
                DbSession.Commit();
                return results;
            }
            catch
            {
                DbSession.Rollback();
                throw;
            }
        }

        public BoolMessage UpdateStatus(string trainGroupId, int num, int status)
        {
            #region 更新列
            var cols = new[]
            {
                nameof(TrainGroup.Num), //人数
                nameof(TrainGroup.Status) //状态
            };
            #endregion

            var entity = Get(trainGroupId);
            entity.Num = num;
            entity.Status = status;
            var repos = new EduRepository<TrainGroup>();
            repos.Update(entity, cols);
            return BoolMessage.True;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new EduRepository<TrainGroup>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public TrainGroup Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取启用的简单对象集合(已排序)
        /// </summary>
        public List<Simple> GetSimpleList()
        {
            var list = Cache.ValueList();
            var user = AuthHelper.GetSessionUser();
            if (user.IsSchool)
            {
                list = Cache.ValueList().Where(p => p.SchoolId.Equals(user.User.SchoolId)).ToList();
            }
            return list.OrderByAsc(p => p.CreateDateTime).Select(p => new Simple(p.Id, p.Name, p.Spell)).ToList();
        }

        /// <summary>
        /// 获取启用的对象集合(已排序)
        /// </summary>
        public List<TrainGroup> GetList(string schoolId)
        {
            return Cache.ValueList()
                .Where(p => p.SchoolId.Equals(schoolId)).ToList()
                .OrderByAsc(p => p.CreateDateTime);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">名称关键字</param>
        /// <param name="schoolId"></param>
        /// <param name="categoryId"></param>
        /// <param name="endDate">开始时间</param>
        /// <param name="startDate">结束时间</param>
        /// <param name="status">状态 0 未开班 1 已开班</param>
        public PageList<TrainGroup> GetPageList(PaginationCondition pageCondition,
            string name, string schoolId, string categoryId, DateTime? startDate, DateTime? endDate, int? status)
        {
            pageCondition.SetDefaultOrder(nameof(TrainGroup.CreateDateTime));
            var repos = new EduRepository<TrainGroup>();
            var query = repos.PageQuery(pageCondition);
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Name.Contains(name) || p.Spell.Contains(name));
            }
            if (schoolId.IsNotEmpty())
            {
                query.Where(p => p.SchoolId == schoolId);
            }
            if (categoryId.IsNotEmpty())
            {
                categoryId = categoryId.Trim();
                query.Where(p => p.CategoryId == categoryId);
            }
            if (status.HasValue)
            {
                query.Where(p => p.Status == status.Value);
            }
            if (startDate.HasValue)
            {
                query.Where(p => p.StartDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query.Where(p => p.EndDate <= endDate.Value);
            }

            return repos.Page(query);
        }
    }
}
