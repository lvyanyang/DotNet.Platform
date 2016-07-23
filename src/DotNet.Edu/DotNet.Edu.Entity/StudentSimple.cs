// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    public class StudentSimple
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
        /// 身份证号码
        /// </summary>
        [Column("身份证号码")]
        public string IDCardNo { get; set; }
    }
}