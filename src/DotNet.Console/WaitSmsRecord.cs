// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using DotNet.Entity;

namespace DotNet
{
    [Table(Schema = "Sms", Name = "WaitSmsRecordTemp")]
    public class WaitSmsRecord
    {
        [PrimaryKey(true)]
        public int RecordID { get; set; }

        public string SendClientNum { get; set; }
        public string SendContent { get; set; }
        public int AdderID { get; set; }
        public string AdderName { get; set; }
        public DateTime AddTime { get; set; }

        public override string ToString()
        {
            return $"RecordID: {RecordID}, SendClientNum: {SendClientNum}, SendContent: {SendContent}, AdderID: {AdderID}, AdderName: {AdderName}, AddTime: {AddTime}";
        }
    }
}