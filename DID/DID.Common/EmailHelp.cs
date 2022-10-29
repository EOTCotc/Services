using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DID.Common
{
    /// <summary>
    /// 邮件发送
    /// </summary>
    public class EmailHelp
    {
        /// <summary>
        /// Postmark Token 
        /// </summary>
        public const string PostmarkToken = "1d9ba211-7523-4dad-a869-99a90e119d8c";

        /// <summary>
        /// 注册邮件
        /// </summary>
        public const string Register = "<div style=\"text-align:left\">欢迎您注册EOTC会员：<br/><br/>您的注册验证码是：<b>{0}</b><br/>验证码10分钟内有效,如非本人操作请忽略。<br/>如果您有任何问题，欢迎联系我们：coin@eotc.me<br/></div><div style=\"text-align:right\">{1}</div>";

        /// <summary>
        /// 验证码
        /// </summary>
        public const string Verify = "您的验证码是：<b>{0}</b><br/>验证码10分钟内有效,如非本人操作请忽略。<br/>如果您有任何问题，欢迎联系我们：coin@eotc.me<br/></div><div style=\"text-align:right\">{1}</div>";

        /// <summary>
        /// 团队信息
        /// </summary>
        public const string Team = "<b>团队人员展示<b><br/><table border=\"1\"><tr><th>姓名</th><th>邮箱</th><th>层级</th></tr>{0}</table>";

        /// <summary>
        /// 注册验证码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="code"></param>
        public static void SendRegister(string mail, string code)
        {
            var client = new RestClient();
            var request = new RestRequest("https://api.postmarkapp.com/email",Method.Post);
            request.AddHeader("X-Postmark-Server-Token", PostmarkToken);
            request.AddHeader("Content-Type", "application/json");
            var body = new
            {
                From = "vip@eotcvip.live",
                To = mail,
                Subject = "EOTC注册",
                HtmlBody = string.Format(Register, code, DateTime.Now.ToString("D", DateTimeFormatInfo.InvariantInfo))
            };
            request.AddBody(body);
            var response = client.Execute(request);
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="code"></param>
        public static void SendVerify(string mail, string code)
        {
            var client = new RestClient();
            var request = new RestRequest("https://api.postmarkapp.com/email", Method.Post);
            request.AddHeader("X-Postmark-Server-Token", PostmarkToken);
            request.AddHeader("Content-Type", "application/json");
            var body = new
            {
                From = "vip@eotcvip.live",
                To = mail,
                Subject = "EOTC验证码",
                HtmlBody = string.Format(Verify, code, DateTime.Now.ToString("D", DateTimeFormatInfo.InvariantInfo))
            };
            request.AddBody(body);
            var response = client.Execute(request);
        }

        /// <summary>
        /// 团队信息
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="team"></param>
        public static void SendTeamInfo(string mail, string team)
        {
            var client = new RestClient();
            var request = new RestRequest("https://api.postmarkapp.com/email", Method.Post);
            request.AddHeader("X-Postmark-Server-Token", PostmarkToken);
            request.AddHeader("Content-Type", "application/json");
            var body = new
            {
                From = "vip@eotcvip.live",
                To = mail,
                Subject = "EOTC团队信息",
                HtmlBody = string.Format(Team, team)
            };
            request.AddBody(body);
            var response = client.Execute(request);
        }

    }
}
