// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统消息服务
    /// </summary>
    public class MessageService
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        internal MessageService()
        {
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Message entity)
        {
            var repos = new AuthRepository<Message>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Message entity)
        {
            var repos = new AuthRepository<Message>();
            repos.Update(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Message entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new AuthRepository<Message>();
            repos.Delete(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Message Get(string id)
        {
            return new AuthRepository<Message>().Get(id);
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="receiveUserId">接收人主键</param>
        /// <param name="isRead">状态</param>
        public List<Message> GetList(string receiveUserId, bool? isRead)
        {
            var repos = new AuthRepository<Message>();
            var query = repos.SQL.Where(p => p.ReceiveUserId == receiveUserId).OrderByDesc(p => p.SendDateTime);
            if (isRead.HasValue)
            {
                query.Where(p => p.IsRead == isRead);
            }
            return repos.Query(query).ToList();
        }

        /// <summary>
        /// 消息广播
        /// </summary>
        /// <param name="receiveUserList">接收用户列表</param>
        /// <param name="title">标题</param>
        /// <param name="message">广播消息内容</param>
        /// <param name="sendUserId">发送人主键</param>
        /// <param name="sendUserName">发送人姓名</param>
        public void Send(List<User> receiveUserList, string title, string message, string sendUserId, string sendUserName)
        {
            foreach (var user in receiveUserList)
            {
                var entity = new Message();
                entity.Title = title;
                entity.MessageContent = message;
                entity.SendUserId = sendUserId;
                entity.SendUserName = sendUserName;
                entity.SendDateTime = DateTime.Now;
                entity.ReceiveUserId = user.Id;
                entity.ReceiveUserName = user.Name;
                entity.ReadDateTime = null;
                Create(entity);
            }
        }

        /// <summary>
        /// 读取未读消息数量
        /// </summary>
        /// <param name="receiveUserId">接收用户主键</param>
        public int GetUnReadCount(string receiveUserId)
        {
            var repos = new AuthRepository<Message>();
            return repos.Count(p => p.ReceiveUserId == receiveUserId && p.IsRead == false);
        }

        /// <summary>
        /// 获取最后一条未读消息
        /// </summary>
        /// <param name="receiveUserId">接收用户主键</param>
        public Message GetLastUnRead(string receiveUserId)
        {
            var repos = new AuthRepository<Message>();
            var query = repos.SQL.Where(p => p.ReceiveUserId == receiveUserId && p.IsRead == false)
                .OrderByDesc(p => p.SendDateTime);
            return repos.Query(query).FirstOrDefault();
        }

        /// <summary>
        /// 读消息
        /// </summary>
        /// <param name="id">系统消息主键</param>
        /// <returns>执行成功返回BoolMessage.True</returns>
        public BoolMessage ReadMessage(string id)
        {
            var entity = Get(id);
            entity.IsRead = true;
            entity.ReadDateTime = DateTime.Now;
            var repos = new AuthRepository<Message>();
            repos.Update(entity, p => p.IsRead, p => p.ReadDateTime);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="title">标题关键字</param>
        /// <param name="sendUserName">发送人姓名关键字</param>
        /// <param name="receiveUserId">接收人主键</param>
        /// <param name="sendStartDate">发送开始日期</param>
        /// <param name="sendEndDate">发送结束日期</param>
        /// <param name="isRead">状态</param>
        public PageList<Message> GetPageList(PaginationCondition pageCondition,
            string receiveUserId, bool? isRead, DateTime? sendStartDate, DateTime? sendEndDate,
            string title, string sendUserName)
        {
            pageCondition.SetDefaultOrder(nameof(Message.SendDateTime));
            var repos = new AuthRepository<Message>();
            var query = repos.PageQuery(pageCondition).Where(p => p.ReceiveUserId == receiveUserId);

            if (isRead.HasValue)
            {
                query.Where(p => p.IsRead == isRead);
            }
            if (sendStartDate.HasValue)
            {
                query.Where(p => p.SendDateTime >= sendStartDate.Value);
            }
            if (sendEndDate.HasValue)
            {
                query.Where(p => p.SendDateTime <= sendEndDate.Value.AddDays(1));
            }
            if (title.IsNotEmpty())
            {
                title = title.Trim();
                query.Where(p => p.Title.Contains(title));
            }
            if (sendUserName.IsNotEmpty())
            {
                sendUserName = sendUserName.Trim();
                query.Where(p => p.SendUserName.Contains(sendUserName));
            }
            return repos.Page(query);
        }
    }
}
