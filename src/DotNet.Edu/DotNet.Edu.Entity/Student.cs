// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;
using DotNet.Helper;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 学员信息
    /// </summary>
    [Table("学员信息")]
    public class Student
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        [Column("简拼")]
        public string Spell { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Column("性别")]
        public string Sex { get; set; }

        /// <summary>
        /// 准驾车型
        /// </summary>
        [Column("准驾车型")]
        public string DrivingModel { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [Column("身份证号码")]
        public string IDCardNo { get; set; }

        /// <summary>
        /// 企业电话
        /// </summary>
        [Column("企业电话")]
        public string CompanyTelPhone { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        [Column("住址")]
        public string Address { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Column("手机号码")]
        public string MobilePhone { get; set; }

        /// <summary>
        /// 从业类别主键
        /// </summary>
        [Column("从业类别主键", Exported = false)]
        public string WorkCategoryId { get; set; }

        /// <summary>
        /// 从业类别名称
        /// </summary>
        [Column("从业类别")]
        public string WorkCategoryName { get; set; }

        /// <summary>
        /// 培训周期
        /// </summary>
        [Column("培训周期")]
        public int? TrainCycle { get; set; }

        /// <summary>
        /// 从业资格证号
        /// </summary>
        [Column("从业资格证号")]
        public string Certificate { get; set; }

        /// <summary>
        /// 获得资格证时间
        /// </summary>
        [Column("获得资格证日期")]
        public DateTime? CertificateDate { get; set; }

        /// <summary>
        /// 证件开始日期
        /// </summary>
        [Column("证件开始日期")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 证件结束日期
        /// </summary>
        [Column("证件结束日期")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        [Column("照片",Exported = false)]
        public string Photo { get; set; }

        /// <summary>
        /// 培训原因
        /// </summary>
        [Column("培训原因")]
        public string TrainReason { get; set; }

        /// <summary>
        /// 公司主键
        /// </summary>
        [Column("公司主键", Exported = false)]
        public string CompanyId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [Column("企业")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 培训机构主键
        /// </summary>
        [Column("学校主键", Exported = false)]
        public string SchoolId { get; set; }

        /// <summary>
        /// 培训机构
        /// </summary>
        [Column("培训机构")]
        public string SchoolName { get; set; }

        /// <summary>
        /// 班级主键
        /// </summary>
        [Column("班级主键", Exported = false)]
        public string TrainGroupId { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        [Column("班级")]
        public string TrainGroupName { get; set; }

        /// <summary>
        /// 报名时间
        /// </summary>
        [Column("报名时间")]
        public DateTime? RegDateTime { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Column("添加时间")]
        public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        [Column("预约时间")]
        public DateTime? ReservationDateTime { get; set; }

        /// <summary>
        /// 学时
        /// </summary>
        [Column("学时",Exported = false)]
        public int TotalPeriod { get; set; }

        /// <summary>
        /// 学时字符串
        /// </summary>
        [Ignore]
        [Column("学时", Exported = false)]
        public string TotalPeriodName
        {
            get { return DateTimeHelper.GetTimeStringHMS(TotalPeriod); }
        }

        /// <summary>
        /// 状态 0 预报名 1 报名中 2 学习中 3 准备考试 4 考试中 5 完成
        /// </summary>
        [Column("状态", Exported = false)]
        public int Status { get; set; }

        /// <summary> 
        /// 状态名称 0 预报名 1 报名中 2 学习中 3 准备考试 4 考试中 5 完成
        /// </summary>
        [Ignore]
        [Column("状态名称", Exported = false)]
        public string StatusName
        {
            get { return Auth.Service.AuthService.DicDetail.GetNameByValue("StudentStatus", Status.ToString(), "未知"); }
        }
        
        /// <summary>
        /// 备注
        /// </summary>
        [Column("备注")]
        public string Note { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Student Clone()
        {
            return (Student)MemberwiseClone();
        }
    }
}