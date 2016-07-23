// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 培训机构
    /// </summary>
    [Table("培训机构")]    
    public class School
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 编号
        /// </summary>
		[Column("编号")]
        public string Code { get; set; }

		/// <summary>
        /// 名称
        /// </summary>
		[Column("名称")]
        public string Name { get; set; }

		/// <summary>
        /// 简拼
        /// </summary>
		[Column("简拼")]
        public string Spell { get; set; }

		/// <summary>
        /// 上级主键
        /// </summary>
		[Column("上级主键")]
        public string DepartmentId { get; set; }

		/// <summary>
        /// 上级名称
        /// </summary>
		[Column("上级名称")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 所属区域
        /// </summary>
        [Ignore]
        public string Region => ProvinceName + CityName + AreaName;

        /// <summary>
        /// 省主键
        /// </summary>
        [Column("省主键")]
        public string ProvinceId { get; set; }

		/// <summary>
        /// 省名称
        /// </summary>
		[Column("省名称")]
        public string ProvinceName { get; set; }

		/// <summary>
        /// 市主键
        /// </summary>
		[Column("市主键")]
        public string CityId { get; set; }

		/// <summary>
        /// 市名称
        /// </summary>
		[Column("市名称")]
        public string CityName { get; set; }

		/// <summary>
        /// 区主键
        /// </summary>
		[Column("区主键")]
        public string AreaId { get; set; }

		/// <summary>
        /// 区名称
        /// </summary>
		[Column("区名称")]
        public string AreaName { get; set; }

		/// <summary>
        /// 核发日期
        /// </summary>
		[Column("核发日期")]
        public DateTime? HFDate { get; set; }

		/// <summary>
        /// 生效日期
        /// </summary>
		[Column("生效日期")]
        public DateTime? StartDate { get; set; }

		/// <summary>
        /// 失效日期
        /// </summary>
		[Column("失效日期")]
        public DateTime? EndDate { get; set; }

		/// <summary>
        /// 法人代表
        /// </summary>
		[Column("法人代表")]
        public string Frdb { get; set; }

		/// <summary>
        /// 联系电话
        /// </summary>
		[Column("联系电话")]
        public string Telephone { get; set; }

		/// <summary>
        /// 许可证号
        /// </summary>
		[Column("许可证号")]
        public string Xkzh { get; set; }

		/// <summary>
        /// 工商注册号
        /// </summary>
		[Column("工商注册号")]
        public string GSCode { get; set; }

		/// <summary>
        /// 批准文号
        /// </summary>
		[Column("批准文号")]
        public string PZCode { get; set; }

		/// <summary>
        /// 档案号
        /// </summary>
		[Column("档案号")]
        public string DACode { get; set; }

		/// <summary>
        /// 工商注册地址
        /// </summary>
		[Column("工商注册地址")]
        public string Gsaddress { get; set; }

		/// <summary>
        /// 实际地址
        /// </summary>
		[Column("实际地址")]
        public string Address { get; set; }

		/// <summary>
        /// 经营范围
        /// </summary>
		[Column("经营范围")]
        public string Jyfw { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		[Column("创建时间")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [Column("启用")]
        public bool IsEnabled { get; set; } = true;

		/// <summary>
        /// 备注
        /// </summary>
		[Column("备注")]
        public string Note { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public School Clone()
        {
            return (School)MemberwiseClone();
        }
    }
}