// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;
using DotNet.Configuration;
using DotNet.Doc;
using DotNet.Entity;
using DotNet.Helper;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Mvc
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public class JsonController : Controller
    {
        ///// <summary>
        ///// 服务器内部500错误消息
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //public JsonNetResult InternalError(string msg)
        //{
        //    return new JsonNetResult(new JsonMessage(false, msg));
        //}

        /// <summary>
        /// 是否是新建记录
        /// </summary>
        public bool IsCreate => WebHelper.GetFormString(MvcHelper.RecordCreate).Equals("1");

        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 System.Web.Mvc.JsonResult 对象。
        /// </summary>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 JSON 结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        protected new JsonNetResult Json(object data)
        {
            return new JsonNetResult(data);
        }

        /// <summary>
        /// 创建 JsonResult 对象
        /// </summary>
        /// <param name="success">是否执行成功</param>
        /// <param name="message">执行消息</param>
        /// <returns></returns>
        protected JsonNetResult Json(bool success, string message)
        {
            return new JsonNetResult(new JsonMessage(success, message));
        }

        /// <summary>
        /// 创建 JsonResult 对象
        /// </summary>
        /// <param name="success">是否执行成功</param>
        /// <returns></returns>
        protected JsonNetResult Json(bool success)
        {
            return new JsonNetResult(new JsonMessage(success));
        }

        /// <summary>
        /// 创建 JsonResult 对象
        /// </summary>
        /// <param name="boolMessage">Bool消息</param>
        /// <returns></returns>
        protected JsonNetResult Json(BoolMessage boolMessage)
        {
            return new JsonNetResult(new JsonMessage(boolMessage));
        }


        /// <summary>
        /// 创建 System.Web.Mvc.JsonResult 对象，该对象使用内容类型、内容编码和 JSON 请求行为将指定对象序列化为 JavaScript 对象表示法 (JSON) 格式。
        /// </summary>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="contentType">内容类型（MIME 类型）。</param>
        /// <returns>将指定对象序列化为 JSON 格式的结果对象。</returns>
        protected new JsonNetResult Json(object data, string contentType)
        {
            return new JsonNetResult(data, contentType);
        }

        /// <summary>
        /// 创建 System.Web.Mvc.JsonResult 对象，该对象使用内容类型、内容编码和 JSON 请求行为将指定对象序列化为 JavaScript 对象表示法 (JSON) 格式。
        /// </summary>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="contentType">内容类型（MIME 类型）。</param>
        /// <param name="contentEncoding">内容编码。</param>
        /// <returns>将指定对象序列化为 JSON 格式的结果对象。</returns>
        protected new JsonNetResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return new JsonNetResult(data, contentType, contentEncoding);
        }

        /// <summary>
        /// 创建 System.Web.Mvc.JsonResult 对象，该对象使用内容类型、内容编码和 JSON 请求行为将指定对象序列化为 JavaScript 对象表示法 (JSON) 格式。
        /// </summary>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="contentType">内容类型（MIME 类型）。</param>
        /// <param name="contentEncoding">内容编码。</param>
        /// <param name="behavior">JSON 请求行为</param>
        /// <returns>将指定对象序列化为 JSON 格式的结果对象。</returns>
        protected new JsonNetResult Json(object data, string contentType, Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            return new JsonNetResult(data, contentType, contentEncoding, behavior);
        }

        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 System.Web.Mvc.JsonResult 对象。响应为text/html
        /// </summary>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 JSON 结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        protected JsonNetResult JsonText(object data)
        {
            return new JsonNetResult(data, "text/html", Encoding.UTF8);
        }

        /// <summary>
        /// 服务器404错误消息
        /// </summary>
        /// <param name="isModal">是否模式对话框</param>
        /// <param name="model">对象</param>
        /// <param name="title">错误标题</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public ViewResult NotFound(string title,string message, bool isModal=true,object model = null)
        {
            ViewBag.title = title;
            ViewBag.message = message;
            ViewBag.isModal = isModal;
            return View("NotFound", model);
            //return new JsonNetResult(new JsonMessage(false, "找不到网页:" + msg));
        }

        /// <summary>
        /// 服务器操作提示
        /// </summary>
        /// <param name="results">结构列表</param>
        /// <param name="title">标题</param>
        public ViewResult AjaxServerResult(IEnumerable<BoolMessage> results,string title="应用程序执行结果")
        {
            ViewBag.title = title;
            return View("AjaxServerResult", results);
        }

        /// <summary>
        /// 标记新建记录
        /// </summary>
        public void MarkCreate()
        {
            ViewData[MvcHelper.RecordCreate] = "1";
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dataList">数据</param>
        /// <param name="fileName">导出文件名称(不太后缀名)</param>
        /// <returns></returns>
        protected FileContentResult Export<T>(IEnumerable<T> dataList, string fileName = null) where T : class, new()
        {
            var metadata = EntityMetadata.ForType(typeof(T));
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = metadata.TableInfo.Caption;
            }
            byte[] fileContents = ExcelHelper.Export(dataList);
            return File(fileContents, "application/vnd.ms-excel", $"{fileName}.xlsx");
        }

    }
}
