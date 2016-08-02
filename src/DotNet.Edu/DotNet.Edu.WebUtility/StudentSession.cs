using DotNet.Edu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Edu.WebUtility
{
    /// <summary>
    /// 学员会话用户
    /// </summary>
    public class StudentSession
    {
        /// <summary>
        /// 学员主键
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// 学员姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 登陆IP地址
        /// </summary>
        public string LoginIPAddr { get; set; }

        /// <summary>
        /// 登陆设备
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// 登陆时间
        /// </summary>
        public DateTime LoginDateTime { get; set; }

        /// <summary>
        /// 学员实体对象
        /// </summary>
        public Student Student { get; set; }

        /// <summary>
        /// 学习对象
        /// </summary>
        public PeriodDetails Learning { get; set; }

        /// <summary>
        /// 出题方式 1.顺序 2.随机
        /// </summary>
        public int ExerciseType { get; set; }

        /// <summary>
        /// 学员练习题
        /// </summary>
        public List<ExcerciseQuestion> ExerciseQuestions { get; set; }

        /// <summary>
        /// 学员收藏
        /// </summary>
        public List<ExcerciseQuestion> FavoriteQuestions { get; set; }

    }
}
