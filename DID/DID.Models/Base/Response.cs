using Newtonsoft.Json;
using System.Net;
using System.Runtime.Serialization;

namespace DID.Models.Base
{
    /// <summary>
    /// Class Response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonObject("result")]
    [DataContract]
    public class Response<T> : Response
    {
        /// <summary>
        /// 返回的数据
        /// </summary>
        [JsonProperty("items")]
        [DataMember]
        public T Items { get; set; }
    }

    /// <summary>
    /// Class Response.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 返回: 0=成功 1=失败,401=未登录，404=未找到，500=系统内部错误，其它=错误码
        /// </summary>
        [JsonProperty("error")]
        [DataMember]
        public int Code { get; set; }
        /// <summary>
        /// 失败原因说明
        /// </summary>
        [JsonProperty("msg")]
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// 数据总条数
        /// </summary>
        //[JsonProperty("total")]
        //[DataMember]
        //public long Total { get; set; }
    }
    /// <summary>
    /// 调用接口结果帮助类
    /// </summary>
    public static class InvokeResult
    {
        private const int FAIL = 1;
        private const int SUCCESS = 0;
        /// <summary>
        /// 返回失败及失败原因
        /// </summary>
        /// <param name="message">失败原因</param>
        /// <returns></returns>
        public static Response Fail(string message = "FAIL")
        {
            return Create(FAIL, message);
        }

        /// <summary>
        /// 返回失败及失败原因
        /// </summary>
        /// <param name="message">失败原因</param>
        /// <returns></returns>
        public static Response<T> Fail<T>(string message = "FAIL")
        {
            return Create<T>(FAIL, message, default);
        }

        /// <summary>
        /// 返回失败及失败原因
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="message">失败原因</param>
        /// <returns></returns>
        public static Response Error(int code, string message = "FAIL")
        {
            return Create(code, message);
        }

        /// <summary>
        /// 返回失败及失败原因
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="message">失败原因</param>
        /// <returns></returns>
        public static Response Error(HttpStatusCode code, string message = "FAIL")
        {
            return Create((int)code, message);
        }

        /// <summary>
        /// 返回失败及失败原因
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="message">失败原因</param>
        /// <returns></returns>
        public static Response<T> Error<T>(int code, string message = "FAIL")
        {
            return Create<T>(code, message);
        }

        /// <summary>
        /// 返回成功及成功描述
        /// </summary>
        /// <param name="message">失败原因</param>
        /// <returns></returns>
        public static Response Success(string message = "SUCCESS")
        {
            return Create(SUCCESS, message);
        }

        /// <summary>
        /// 返回成功及成功描述
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <param name="message">失败原因</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static Response<T> Success<T>(T data/*, long total = 0*/, string message = "SUCCESS")
        {
            return Create(SUCCESS, message, data/*, total*/);
        }

        /// <summary>
        /// Creates the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <returns>Response.</returns>
        private static Response Create(int code, string message)
        {
            return new Response { Code = code, Message = message };
        }

        /// <summary>
        /// Creates the specified code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <param name="total"></param>
        /// <returns>Response&lt;T&gt;.</returns>
        private static Response<T> Create<T>(int code, string message, T data = default/*, long total = 0*/)
        {
            return new Response<T> { Code = code, Message = message, Items = data/*, Total = total*/ };
        }
    }
}
