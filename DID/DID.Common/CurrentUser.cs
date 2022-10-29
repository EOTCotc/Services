using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using DID.Entitys;
using DID.Models.Request;
using DID.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace DID.Common;

public class CurrentUser : ICurrentUser
{
    private readonly HttpContext _httpContext;

    private static ILogger<CurrentUser> _logger;

    public CurrentUser(IHttpContextAccessor httpContextAccessor, ILogger<CurrentUser> logger)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _logger = logger;
    }

    /// <summary>
    /// 当前登录用户名称
    /// </summary>
    public string Name => _httpContext.User.Identity?.Name;

    /// <summary>
    /// 当前登录用户ID
    /// </summary>
    public string UserId => GetClaimValueByType("UserId").FirstOrDefault().ToString();

    /// <summary>
    /// 请求携带的Token
    /// </summary>
    /// <returns></returns>
    public string GetToken()
    {
        return _httpContext.Request.Headers["Authorization"].ToString()
            .Replace("Bearer ", "");
    }

    /// <summary>
    /// 是否已认证
    /// </summary>
    /// <returns></returns>
    public bool IsAuthenticated()
    {
        return _httpContext.User.Identity is { IsAuthenticated: true };
    }

    /// <summary>
    /// 获取用户身份权限
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Claim> GetClaimsIdentity()
    {
        return _httpContext.User.Claims;
    }

    /// <summary>
    /// 根据权限类型获取详细权限
    /// </summary>
    /// <param name="claimType"></param>
    /// <returns></returns>
    public List<string> GetClaimValueByType(string claimType)
    {
        return (from item in GetClaimsIdentity()
            where item.Type == claimType
            select item.Value).ToList();
    }

    public List<string> GetUserInfoFromToken(string claimType)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        if (!string.IsNullOrEmpty(GetToken()))
        {
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(GetToken());

            return (from item in jwtToken.Claims
                where item.Type == claimType
                select item.Value).ToList();
        }

        return new List<string>();
    }

    /// <summary>
    /// 获取EOTC数量
    /// </summary>
    /// <returns></returns>
    public static double GetEotc(string userId)
    {
        try
        {
            using var db = new NDatabase();
            var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            if (null == user)
                return 0;
            var client = new RestClient();
            var request = new RestRequest(string.Format("https://api.eotcyu.club/api/OTC/QueryPoints?uid={0}&pwd={1}", user.Uid, user.PassWord), Method.Post);
            var response = client.Execute(request);
            var model = JsonExtensions.DeserializeFromJson<EUModel>(response.Content);
            return model.EOTC;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// 用户注册EOTC
    /// </summary>
    /// <returns></returns>
    public static int RegisterEotc(string mail,string ads, string sign, string net, string uid, string pid)
    {
        try
        {
            using var db = new NDatabase();

            var client = new RestClient();
            var request = new RestRequest(string.Format("https://api.eotcyu.club/api/OTC/RegisterEotc?mail={0}&ads={1}&sign={2}&net={3}&uid={4}&pid={5}",
                                        mail, ads, sign, net, uid, pid), Method.Post);
            var response = client.Execute(request);
            var model = JsonExtensions.DeserializeFromJson<CodeModel>(response.Content);
            //Console.WriteLine(response.Content);
            _logger.LogDebug(response.Content);
            return model.Code;
        }
        catch
        {
            return -1;
        }
    }
    /// <summary>
    /// 获取空投
    /// </summary>
    /// <returns></returns>
    public static double GetAirdrop(string userId)
    {
        try
        {
            using var db = new NDatabase();
            var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            if (null == user)
                return 0;
            var client = new RestClient();
            var request = new RestRequest(string.Format("https://api.eotcyu.club/api/OTC/QueryPoints?uid={0}&pwd={1}", user.Uid, user.PassWord), Method.Post);
            var response = client.Execute(request);
            var model = JsonExtensions.DeserializeFromJson<EUModel>(response.Content);
            return model.Airdrop;
        }
        catch
        {
            return 0;
        }
    }
}