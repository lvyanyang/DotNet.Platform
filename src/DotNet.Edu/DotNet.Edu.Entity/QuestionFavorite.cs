// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 题库收藏
    /// </summary>
    [Table("题库收藏")]    
    public class QuestionFavorite
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 学员主键
        /// </summary>
		[Column("学员主键")]
        public string StudentId { get; set; }

		/// <summary>
        /// 题库主键
        /// </summary>
		[Column("题库主键")]
        public string QuestionId { get; set; }

		/// <summary>
        /// 题库收藏:1错题收藏2题库收藏
        /// </summary>
		[Column("题库收藏:1错题收藏2题库收藏")]
        public int FavoriteType { get; set; }

		/// <summary>
        /// 添加时间
        /// </summary>
		[Column("添加时间")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public QuestionFavorite Clone()
        {
            return (QuestionFavorite)MemberwiseClone();
        }
    }
}