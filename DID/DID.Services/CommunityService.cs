using Dao.Entity;
using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using DID.Models.Request;
using DID.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NPoco;

namespace DID.Services
{
    /// <summary>
    /// 社区接口
    /// </summary>
    public interface ICommunityService
    {
        /// <summary>
        /// 设置用户社区选择（未填邀请码）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response> SetComSelect(ComSelectReq req);

        /// <summary>
        /// 获取用户社区选择
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<ComSelectRespon>> GetComSelect(string userId);

        /// <summary>
        /// 获取已有社区位置
        /// </summary>
        /// <returns> </returns>
        Task<Response<ComAddrRespon>> GetComAddr();

        /// <summary>
        /// 获取当前位置社区数量
        /// </summary>
        /// <param name="country"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        Task<Response<int>> GetComNum(string country, string province, string city, string area);

        /// <summary>
        /// 获取当前位置社区信息
        /// </summary>
        /// <param name="country"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        Task<Response<List<ComRespon>>> GetComList(string country, string province, string city, string area, long page, long itemsPerPage);

        /// <summary>
        /// 获取打回信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        Task<Response<List<ComAuthRespon>>> GetBackCom(string userId, IsEnum isDao, long page, long itemsPerPage);

        /// <summary>
        /// 获取未审核信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        Task<Response<List<ComAuthRespon>>> GetUnauditedCom(string userId, IsEnum isDao, long page, long itemsPerPage);

        /// <summary>
        /// 获取已审核审核信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        Task<Response<List<ComAuthRespon>>> GetAuditedCom(string userId, IsEnum isDao, long page, long itemsPerPage);

        /// <summary>
        /// 社区申请审核
        /// </summary>
        /// <returns> </returns>
        Task<Response> AuditCommunity(string communityId, string userId, AuditTypeEnum auditType, string? remark,bool isDao = false);

        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <returns> </returns>
        Task<Response<GetCommunityInfoRespon>> GetCommunityInfo(string userId);

        /// <summary>
        /// 添加社区信息
        /// </summary>
        /// <returns> </returns>
        Task<Response> AddCommunityInfo(Community item);

        /// <summary>
        /// 社区申请
        /// </summary>
        /// <returns> </returns>
        Task<Response<string>> ApplyCommunity(Community item);
        /// <summary>
        /// 社区图片上传 1 请上传文件! 2 文件类型错误!
        /// </summary>
        /// <returns></returns>
        Task<Response> UploadImage(IFormFile file);
        /// <summary>
        /// 获取社区审核失败信息
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        Task<Response<List<AuthInfo>>> GetComAuthFail(string communityId);

        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <returns> </returns>
        Task<Response<GetCommunityInfoRespon>> GetCommunityInfoById(string communityId);

        /// <summary>
        /// 社区名是否重复
        /// </summary>
        /// <returns> </returns>
        Task<Response<bool>> HasComName(string comName);

    }
    /// <summary>
    /// 社区服务
    /// </summary>
    public class CommunityService : ICommunityService
    {
        private readonly ILogger<CommunityService> _logger;

        private readonly IRewardService _reservice;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CommunityService(ILogger<CommunityService> logger, IRewardService reservice)
        {
            _logger = logger;
            _reservice = reservice;
        }

        /// <summary>
        /// 获取用户社区选择
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<ComSelectRespon>> GetComSelect(string userId)
        {
            using var db = new NDatabase();
            var item = await db.SingleOrDefaultAsync<ComSelect>("select * from ComSelect where DIDUserId = @0", userId);
            if (null == item)
                return InvokeResult.Success(new ComSelectRespon());

            var country = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = 'COUNTRIES'", item.Country);

            var province = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = @1", item.Province, item.Country);

            var city = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = @1", item.City, item.Province);

            var area = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = @1", item.Area, item.City);


            var respon = new ComSelectRespon()
            {
                country = new Area() { code = item.Country, name = country },
                province = new Area() { code = item.Province, name = province },
                city = new Area() { code = item.City, name = city },
                county = new Area() { code = item.Area, name = area }
            };
            return InvokeResult.Success(respon);
        }

        /// <summary>
        /// 设置用户社区选择（未填邀请码）
        /// </summary>
        /// <param name="req"></param>
        /// <returns> </returns>
        public async Task<Response> SetComSelect(ComSelectReq req)
        {
            using var db = new NDatabase();
            var id = await db.SingleOrDefaultAsync<string>("select ComSelectId from ComSelect where DIDUserId = @0", req.UserId);
            if (!string.IsNullOrEmpty(id))
                return InvokeResult.Fail("请勿重复设置!"); //请勿重复设置!
            var country = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = 'COUNTRIES'", req.Country);
            var province = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = @1", req.Province, req.Country);
            if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(province))
                return InvokeResult.Fail("位置信息错误!"); //位置信息错误!

            if (!string.IsNullOrEmpty(req.City))
            {
                var city = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = @1", req.City, req.Province);
                if (string.IsNullOrEmpty(city))
                    return InvokeResult.Fail("位置信息错误!"); //位置信息错误!
            }

            if (!string.IsNullOrEmpty(req.Area))
            {
                var area = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = @1", req.Area, req.City);
                if (string.IsNullOrEmpty(area))
                    return InvokeResult.Fail("位置信息错误!"); //位置信息错误!
            }
            
            
            var item = new ComSelect()
            {
                ComSelectId = Guid.NewGuid().ToString(),
                DIDUserId = req.UserId,
                Country = req.Country,
                Province = req.Province,
                City = req.City,
                Area = req.Area,
                CreateDate = DateTime.Now
            };
            await db.InsertAsync(item);
            return InvokeResult.Success("插入成功!");
        }

        /// <summary>
        /// 获取已有社区位置
        /// </summary>
        /// <returns> </returns>
        public async Task<Response<ComAddrRespon>> GetComAddr()
        {
            using var db = new NDatabase();
            var country_list = await db.FetchAsync<Area>("select Distinct a.Country code, b.Name from Community a left join Area b on a.Country = b.Code where a.AuthType = 2 and a.Country is not null");
            var province_list = await db.FetchAsync<Area>("select Distinct a.Province code, b.Name from Community a left join Area b on a.Province = b.Code where a.AuthType = 2 and a.Province is not null");
            var city_list = await db.FetchAsync<Area>("select Distinct a.City code, b.Name from Community a left join Area b on a.City = b.Code where a.AuthType = 2 and a.City is not null");
            var county_list = await db.FetchAsync<Area>("select Distinct a.Area code, b.Name from Community a left join Area b on a.Area = b.Code where a.AuthType = 2 and a.Area is not null");

            var country = new Dictionary<string, string>();
            country_list.ForEach(a => country.Add(a.code, a.name));

            var provinces = new Dictionary<string, string>();
            province_list.ForEach(a => provinces.Add(a.code, a.name));

            var citys = new Dictionary<string, string>();
            city_list.ForEach(a => citys.Add(a.code, a.name));

            var countys = new Dictionary<string, string>();
            county_list.ForEach(a => countys.Add(a.code, a.name));

            var item = new ComAddrRespon()
            {
                country_list = country,
                province_list = provinces,
                city_list = citys,
                county_list = countys
            };

            return InvokeResult.Success(item);
        }

        /// <summary>
        /// 获取当前位置社区数量
        /// </summary>
        /// <returns> </returns>
        public async Task<Response<int>> GetComNum(string country, string province, string city, string area)
        {
            using var db = new NDatabase();
            var num = await db.SingleOrDefaultAsync<int>("select count(1) from Community where Country = @0 and Province = @1 and City = @2 and Area = @3 and AuthType = @4",
                country, province, city, area, AuthTypeEnum.审核成功);
            return InvokeResult.Success(num);
        }

        /// <summary>
        /// 获取当前位置社区信息
        /// </summary>
        /// <returns> </returns>
        public async Task<Response<List<ComRespon>>> GetComList(string country, string province, string city, string area, long page, long itemsPerPage)
        {
            using var db = new NDatabase();
            //var list = await db.FetchAsync<Community>("select * from Community where Country = @0 and Province = @1 and City = @2 and Area = @3 and AuthType = @4",
            //    country, province, city, area, AuthTypeEnum.审核成功);
            var list = (await db.PageAsync<Community>(page, itemsPerPage, "select * from Community where Country = @0 and Province = @1 and City = @2 and Area = @3 and AuthType = @4",
                country, province, city, area, AuthTypeEnum.审核成功)).Items;

            var result = new List<ComRespon>();
            foreach (var item in list)
            {
                result.Add(new ComRespon()
                {
                    Name = item.ComName,
                    Describe = item.Describe,
                    Image = item.Image,
                    Telegram = item.Telegram
                });
            }

            return InvokeResult.Success(result);
        }

        /// <summary>
        /// 社区申请
        /// </summary>
        /// <returns> </returns>
        public async Task<Response<string>> ApplyCommunity(Community item)
        {
            using var db = new NDatabase();
            item.CommunityId = Guid.NewGuid().ToString();

            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", item.DIDUserId);
            if(null == user)
                return InvokeResult.Fail<string>("用户信息未找到!");
            //质押5000EOTC
            var eotc = CurrentUser.GetEUModel(user)?.StakeEotc??0;
            if (eotc < 5000)
                return InvokeResult.Fail<string>("质押EOTC数量不足!");
            if (!string.IsNullOrEmpty(user.ApplyCommunityId))
            {
                var authType = await db.SingleOrDefaultAsync<AuthTypeEnum>("select AuthType from Community where CommunityId = @0", user.ApplyCommunityId);
                if (authType != AuthTypeEnum.未审核 && authType != AuthTypeEnum.审核失败)
                    return InvokeResult.Fail<string>("请勿重复申请!");
            }
            user.ApplyCommunityId = item.CommunityId;

            var refUserId = await db.FirstOrDefaultAsync<string>("select RefUserId from DIDUser where DIDUserId = @0", item.DIDUserId);//邀请人
            if (string.IsNullOrEmpty(refUserId))
                return InvokeResult.Fail<string>("邀请人未找到!");

            item.RefDIDUserId = refUserId;
            item.RefCommunityId = await db.FirstOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", refUserId);//邀请人社区
            item.CreateDate = DateTime.Now;
            item.AuthType = AuthTypeEnum.审核中;

            //邀请人审核
            var auth = new ComAuth
            {
                ComAuthId = Guid.NewGuid().ToString(),
                CommunityId = item.CommunityId,
                AuditUserId = refUserId,//推荐人审核
                CreateDate = DateTime.Now,
                AuditType = AuditTypeEnum.未审核,
                AuditStep = AuditStepEnum.初审
            };

            db.BeginTransaction();
            await db.InsertAsync(item);
            await db.InsertAsync(auth);
            await db.UpdateAsync(user);
            db.CompleteTransaction();

            //Dao审核
            //ToDaoAuth(auth, item.DIDUserId!);

            //默认2小时去Dao
            var hours = Convert.ToInt32(_reservice.GetRewardValue("ComAuthHours").Result.Items);
            var timer = new Timers { 
                TimersId = Guid.NewGuid().ToString(), 
                Rid = auth.ComAuthId, 
                CreateDate = DateTime.Now, 
                StartTime = DateTime.Now, 
                EndTime = DateTime.Now.AddHours(hours), 
                TimerType = TimerTypeEnum.社区审核 
            };
            await db.InsertAsync(timer);
            TimersHelp.ComAuthTimer(timer);

            return InvokeResult.Success<string>(item.CommunityId);
        }

        /// <summary>
        /// 社区名是否重复
        /// </summary>
        /// <returns> </returns>
        public async Task<Response<bool>> HasComName(string comName)
        {
            using var db = new NDatabase();
            var id = await db.SingleOrDefaultAsync<string>("select CommunityId from Community where ComName = @0", comName);
            if (string.IsNullOrEmpty(id))
            {
                return InvokeResult.Success(false);
            }
            else
                return InvokeResult.Success(true);
        }


        /// <summary>
        /// 添加社区信息
        /// </summary>
        /// <returns> </returns>
        public async Task<Response> AddCommunityInfo(Community item)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<Community>(item.CommunityId);
            if(null == model)
                return InvokeResult.Fail("社区信息未找到!");
            if(item.DIDUserId != model.DIDUserId)
                return InvokeResult.Fail("社区信息修改错误!");

            //社区名不能重复
            var id = await db.SingleOrDefaultAsync<string>("select CommunityId from Community where ComName = @0", item.ComName);
            if (model.ComName != item.ComName && string.IsNullOrEmpty(id) && model.IsUpdateComName == IsEnum.否)
            {
                model.ComName = item.ComName;
                model.IsUpdateComName = IsEnum.是;
            }
            //else
            //    return InvokeResult.Fail("社区名称重复或已修改!");

            //一年只能修改两次社区位置
            if (!string.IsNullOrEmpty(item.AddressName) &&item.AddressName != model.AddressName  && model.UpdateNum == 2 && model.UpdateAddressDate?.Year == DateTime.Now.Year)
                return InvokeResult.Fail("社区位置一年只能修改2次!");
            model.Image = item.Image;
            model.Describe = item.Describe;
            model.Telegram = item.Telegram;
            model.Discord = item.Discord;
            model.QQ = item.QQ;
            
            if (!string.IsNullOrEmpty(item.AddressName))
            {
                model.Country = item.Country;
                model.Province = item.Province;
                model.City = item.City;
                model.Area = item.Area;
                model.UpdateDate = DateTime.Now;
            }
            if (item.AddressName != model.AddressName)
            {
                model.AddressName = item.AddressName;
                if ( null != model.UpdateAddressDate && DateTime.Now.Year -  model.UpdateAddressDate?.Year > 0)
                    model.UpdateNum = 1;
                else
                    model.UpdateNum += 1;
                model.UpdateAddressDate = DateTime.Now;
            }

            await db.UpdateAsync(model);
            return InvokeResult.Success("添加成功!");
        }

        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <returns> </returns>
        public async Task<Response<GetCommunityInfoRespon>> GetCommunityInfo(string userId)
        {
            using var db = new NDatabase();
            var item = await db.SingleOrDefaultAsync<GetCommunityInfoRespon>("select * from Community a left join UserCommunity b on a.CommunityId = b.CommunityId where b.DIDUserId = @0", userId);
            if (null == item)
                return InvokeResult.Fail<GetCommunityInfoRespon>("社区信息未找到!");

            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", item.DIDUserId);

            item.CreateName = db.SingleOrDefault<string>("select b.Name from UserAuthInfo b left join DIDUser a  on a.UserAuthInfoId = b.UserAuthInfoId " +
                                                    "where a.DIDUserId = @0 and a.AuthType = 2", item.DIDUserId!);

            item.RefComName = await db.SingleOrDefaultAsync<string>("select ComName from Community where CommunityId = (select CommunityId from UserCommunity where DIDUserId = @0)", user.RefUserId);

            item.RefCommunityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", user.RefUserId);

            return InvokeResult.Success(item);
        }

        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <returns> </returns>
        public async Task<Response<GetCommunityInfoRespon>> GetCommunityInfoById(string communityId)
        {
            using var db = new NDatabase();
            var item = await db.SingleOrDefaultAsync<GetCommunityInfoRespon>("select * from Community where CommunityId = @0", communityId);
            if (null == item)
                return InvokeResult.Fail<GetCommunityInfoRespon>("社区信息未找到!");

            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", item.DIDUserId);

            item.CreateName = db.SingleOrDefault<string>("select b.Name from UserAuthInfo b left join DIDUser a  on a.UserAuthInfoId = b.UserAuthInfoId " +
                                                   "where a.DIDUserId = @0 and a.AuthType = 2", item.DIDUserId!);

            item.RefComName = await db.SingleOrDefaultAsync<string>("select ComName from Community where CommunityId = (select CommunityId from UserCommunity where DIDUserId = @0)", user.RefUserId);

            item.RefCommunityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", user.RefUserId);

            return InvokeResult.Success(item);
        }

        /// <summary>
        /// 社区申请审核
        /// </summary>
        /// <returns> </returns>
        public async Task<Response> AuditCommunity(string communityId, string userId, AuditTypeEnum auditType, string? remark ,bool isDao)
        {
            using var db = new NDatabase();
            var authinfo = await db.SingleOrDefaultByIdAsync<Community>(communityId);
            var auth = await db.SingleOrDefaultAsync<ComAuth>("select * from ComAuth where CommunityId = @0 and AuditUserId = @1 ", communityId, userId);
            if(null == authinfo || null == auth)
                return InvokeResult.Fail("审核信息未找到!");
            //不会出现重复的记录 每个用户只审核一次
            if (auth.AuditType != AuditTypeEnum.未审核)
                return InvokeResult.Fail("已审核!");

            auth.AuditType = auditType;
            auth.Remark = remark; 
            auth.AuditDate = DateTime.Now;

            db.BeginTransaction();
            await db.UpdateAsync(auth);

            //修改审核状态
            if (auth.AuditStep == AuditStepEnum.抽审 && auth.AuditType == AuditTypeEnum.审核通过)
            {
                //await db.ExecuteAsync("update Community set AuthType = @1 where CommunityId = @0", communityId, AuthTypeEnum.审核成功);

                ////更改用户社区信息为自己的社区 (下级用户自动加入)
                ////await db.ExecuteAsync("update UserCommunity set CommunityId = @0, CreateDate = @2  where DIDUserId = @1", communityId, authinfo.DIDUserId, DateTime.Now);

                //var list = await db.FetchAsync<Models.Models.UserCom>(";with temp as \n" +
                //      "(select DIDUserId,ApplyCommunityId from DIDUser where DIDUserId = @0 and IsLogout = 0\n" +
                //      "union all \n" +
                //      "select a.DIDUserId,a.ApplyCommunityId from DIDUser a inner join temp on a.RefUserId = temp.DIDUserId and a.IsLogout = 0) \n" +
                //      "select * from temp", authinfo.DIDUserId);
                //for (var i = 0; i < list.Count; i++)
                //{
                //    if (!string.IsNullOrEmpty(list[i].ApplyCommunityId)&& i > 0)//除自己到下个社区之间的用户
                //        break;
                //    await db.ExecuteAsync("update UserCommunity set CommunityId = @0, CreateDate = @2  where DIDUserId = @1", communityId, list[i].DIDUserId, DateTime.Now);
                //}
            }
            else if (auth.AuditType != AuditTypeEnum.审核通过)
            {
                //修改审核状态
                await db.ExecuteAsync("update Community set AuthType = @1 where CommunityId = @0", communityId, AuthTypeEnum.审核失败);
                //todo: 审核失败 扣分
            }

            //下一步审核
            if (auth.AuditStep == AuditStepEnum.初审 && auth.AuditType == AuditTypeEnum.审核通过)
            {
                //上级节点审核
                var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", authinfo.DIDUserId);
                var auditUserIds = await db.FetchAsync<string>("select AuditUserId from ComAuth where CommunityId = @0 and IsDelete = 0 order by AuditStep", communityId);
                auditUserIds.Add(authinfo.DIDUserId!);
                //var auths = await db.FetchAsync<DIDUser>("select * from DIDUser where UserNode = @0 and IsLogout = 0 and DIDUserId not in (@1)", user.UserNode == UserNodeEnum.高级节点 ? UserNodeEnum.高级节点 : ++user.UserNode, auditUserIds);
                //var random = new Random().Next(auths.Count);
                //var authUserId = auths[random].DIDUserId;
                var authUserId = await db.SingleOrDefaultAsync<string>(
                                                                        ";WITH temp AS (\n" +
                                                        "	SELECT\n" +
                                                        "		DIDUserId,RefUserId, UserNode\n" +
                                                        "	FROM\n" +
                                                        "		DIDUser \n" +
                                                        "	WHERE\n" +
                                                        "		DIDUserId = @0\n" +
                                                        "		AND IsLogout = 0 UNION ALL\n" +
                                                        "	SELECT\n" +
                                                        "		a.DIDUserId,a.RefUserId , a.UserNode\n" +
                                                        "	FROM\n" +
                                                        "		DIDUser a\n" +
                                                        "		INNER JOIN temp ON a.DIDUserId = temp.RefUserId\n" +
                                                        "		AND a.IsLogout = 0 \n" +
                                                        "	) SELECT top 1 * FROM temp where UserNode > 1 and DIDUserId not in (@1)\n" +
                                                        "	", authinfo.DIDUserId, auditUserIds);
                if (string.IsNullOrEmpty(authUserId))
                    return InvokeResult.Fail("审核失败,未找到上级节点!");

                var nextAuth = new ComAuth()
                {
                    ComAuthId = Guid.NewGuid().ToString(),
                    CommunityId = communityId,
                    AuditUserId = authUserId,
                    CreateDate = DateTime.Now,
                    AuditType = AuditTypeEnum.未审核,
                    AuditStep = AuditStepEnum.二审
                };

                await db.InsertAsync(nextAuth);

                //Dao审核
                //ToDaoAuth(nextAuth, authinfo.DIDUserId!);

                var hours = Convert.ToInt32(_reservice.GetRewardValue("ComAuthHours").Result.Items);
                var timer = new Timers { 
                    TimersId = Guid.NewGuid().ToString(),
                    Rid = nextAuth.ComAuthId,
                    CreateDate = DateTime.Now, 
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(hours), 
                    TimerType = TimerTypeEnum.社区审核
                };
                await db.InsertAsync(timer);
                TimersHelp.ComAuthTimer(timer);
            }
            else if (auth.AuditStep == AuditStepEnum.二审 && auth.AuditType == AuditTypeEnum.审核通过)
            {
                await db.ExecuteAsync("update Community set AuthType = @1 where CommunityId = @0", communityId, AuthTypeEnum.审核成功);

                //更改用户社区信息为自己的社区 (下级用户自动加入)
                //await db.ExecuteAsync("update UserCommunity set CommunityId = @0, CreateDate = @2  where DIDUserId = @1", communityId, authinfo.DIDUserId, DateTime.Now);

                var list = await db.FetchAsync<Models.Models.UserCom>(";with temp as \n" +
                      "(select DIDUserId,ApplyCommunityId from DIDUser where DIDUserId = @0 and IsLogout = 0\n" +
                      "union all \n" +
                      "select a.DIDUserId,a.ApplyCommunityId from DIDUser a inner join temp on a.RefUserId = temp.DIDUserId and a.IsLogout = 0) \n" +
                      "select * from temp", authinfo.DIDUserId);
                var users = new List<string>();
                for (var i = 0; i < list.Count; i++)
                {
                    if (!string.IsNullOrEmpty(list[i].ApplyCommunityId) && i > 0)//除自己到下个社区之间的用户
                        break;
                    users.Add(list[i].DIDUserId);


                    //await db.ExecuteAsync("update UserCommunity set CommunityId = @0, CreateDate = @2  where DIDUserId = @1", communityId, list[i].DIDUserId, DateTime.Now);
                }
                var usercom = db.FetchAsync<UserCommunity>("select * from UserCommunity where DIDUserId in (@0)", users);

                var updates = new List<UpdateBatch<UserCommunity>>();
                foreach (var item in usercom.Result)
                {
                    var data = new UpdateBatch<UserCommunity>();
                    data.Poco = item;
                    data.Snapshot = db.StartSnapshot(item);
                    item.CommunityId = communityId;
                    item.CreateDate = DateTime.Now;
                    updates.Add(data);
                }
                
                await db.UpdateBatchAsync(updates);

                //中高级节点审核
                var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", authinfo.DIDUserId);
                var auditUserIds = await db.FetchAsync<string>("select AuditUserId from ComAuth where CommunityId = @0 and IsDelete = 0 order by AuditStep", communityId);
                auditUserIds.Add(authinfo.DIDUserId!);
                var auths = await db.FetchAsync<DIDUser>("select * from DIDUser where (UserNode = 4 or UserNode = 5)  and IsLogout = 0 and DIDUserId not in (@0)", auditUserIds);
                if (auths.Count <= 0)
                    _logger.LogInformation("社区审核失败,未找到中高级节点!");
                else
                {
                    var random = new Random().Next(auths.Count);
                    var authUserId = auths[random].DIDUserId;

                    var nextAuth = new ComAuth()
                    {
                        ComAuthId = Guid.NewGuid().ToString(),
                        CommunityId = communityId,
                        AuditUserId = authUserId,
                        CreateDate = DateTime.Now,
                        AuditType = AuditTypeEnum.未审核,
                        AuditStep = AuditStepEnum.抽审
                    };

                    await db.InsertAsync(nextAuth);
                }
            }

            db.CompleteTransaction();

            //奖励EOTC 10
            if (isDao)
            {
                var eotc = Convert.ToDouble(_reservice.GetRewardValue("ComAudit").Result.Items);//奖励eotc数量
                var detail = new IncomeDetails()
                {
                    IncomeDetailsId = Guid.NewGuid().ToString(),
                    CreateDate = DateTime.Now,
                    EOTC = eotc,
                    Remarks = "处理社区审核",
                    Type = IDTypeEnum.处理审核,
                    DIDUserId = userId
                };
                var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);

                db.BeginTransaction();
                db.Insert(detail);
                user.DaoEOTC += eotc;
                db.Update(user);
                var userExamine = db.SingleOrDefault<UserExamine>("select * from UserExamine where DIDUserId = @0 and IsDelete = 0", userId);
                userExamine.EOTC += eotc;
                userExamine.ExamineNum += 1;
                db.Update(userExamine);
                db.CompleteTransaction();
            }

            return InvokeResult.Success("审核成功!");
        }

        /// <summary>
        /// 获取已审核审核信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public async Task<Response<List<ComAuthRespon>>> GetAuditedCom(string userId,IsEnum isDao, long page, long itemsPerPage)
        {
            var result = new List<ComAuthRespon>();
            using var db = new NDatabase();
            //var items = await db.FetchAsync<ComAuth>("select * from ComAuth where AuditUserId = @0 and AuditType != 0", userId);
            var items = (await db.PageAsync<ComAuth>(page, itemsPerPage, "select * from ComAuth where AuditUserId = @0 and AuditType != 0 and IsDelete = 0 and IsDao = @1 order by CreateDate Desc", userId, isDao)).Items;
            foreach (var item in items)
            {
                var community = await db.SingleOrDefaultAsync<Community>("select * from Community where CommunityId = @0", item.CommunityId);
                var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", community.DIDUserId);
                var authinfo = new ComAuthRespon()
                {
                    CommunityId = community.CommunityId,
                    DIDUser = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", community.DIDUserId),
                    RefUId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", community.RefDIDUserId),
                    RefUserId = community.RefDIDUserId,
                    RefName = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", community.RefDIDUserId),
                    Address = community.Address,
                    ComName = community.ComName,
                    Country = community.Country,
                    Province = community.Province,
                    City = community.City,
                    Area = community.Area,
                    CreateDate = community.CreateDate,
                    Describe = community.Describe,
                    Discord = community.Discord,
                    HasGroup = community.HasGroup,
                    HasOffice = community.HasOffice,
                    Image = community.Image,
                    Mail = await db.SingleOrDefaultAsync<string>("select Mail from DIDUser where DIDUserId = @0", community.DIDUserId),
                    Phone = community.Phone,
                    QQ = community.QQ,
                    AddressName = community.AddressName,
                    RefCommunityName = await db.SingleOrDefaultAsync<string>("select a.ComName from Community a left join UserCommunity b on a.CommunityId = b.CommunityId where b.DIDUserId = @0", community.RefDIDUserId),
                    Telegram = community.Telegram,
                    Eotc = CurrentUser.GetEUModel(user)?.StakeEotc ?? 0//调接口查eotc总数
                };

                var auths = await db.FetchAsync<ComAuth>("select * from ComAuth where CommunityId = @0 and IsDelete = 0 order by AuditStep", item.CommunityId);
                var list = new List<AuthInfo>();
                foreach (var auth in auths)
                {
                    list.Add(new AuthInfo()
                    {
                        UId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", auth.AuditUserId),
                        AuditStep = auth.AuditStep,
                        AuthDate = auth.AuditDate,
                        Name = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", auth.AuditUserId),
                        AuditType = auth.AuditType,
                        Remark = auth.Remark,
                        IsDao = auth.IsDao
                    });
                }
                authinfo.Auths = list;
                result.Add(authinfo);
            }
            return InvokeResult.Success(result);
        }

        /// <summary>
        /// 获取未审核信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public async Task<Response<List<ComAuthRespon>>> GetUnauditedCom(string userId, IsEnum isDao, long page, long itemsPerPage)
        {
            var result = new List<ComAuthRespon>();
            using var db = new NDatabase();
            //var items = await db.FetchAsync<ComAuth>("select * from ComAuth where AuditUserId = @0 and AuditType = 0", userId);
            var items = (await db.PageAsync<ComAuth>(page, itemsPerPage, "select * from ComAuth where AuditUserId = @0 and AuditType = 0 and IsDelete = 0 and IsDao = @1", userId, isDao)).Items;
            foreach (var item in items)
            {
                var community = await db.SingleOrDefaultAsync<Community>("select * from Community where CommunityId = @0", item.CommunityId);
                var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", community.DIDUserId);
                var authinfo = new ComAuthRespon()
                {
                    CommunityId = community.CommunityId,
                    DIDUser = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", community.DIDUserId),
                    RefUId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", community.RefDIDUserId),
                    RefUserId = community.RefDIDUserId,
                    RefName = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", community.RefDIDUserId),
                    Address = community.Address,
                    ComName = community.ComName,
                    Country = community.Country,
                    Province = community.Province,
                    City = community.City,
                    Area = community.Area,
                    CreateDate = community.CreateDate,
                    Describe = community.Describe,
                    Discord = community.Discord,
                    HasGroup = community.HasGroup,
                    HasOffice = community.HasOffice,
                    Image = community.Image,
                    Mail = await db.SingleOrDefaultAsync<string>("select Mail from DIDUser where DIDUserId = @0", community.DIDUserId),
                    Phone = community.Phone,
                    QQ = community.QQ,
                    AddressName = community.AddressName,
                    RefCommunityName = await db.SingleOrDefaultAsync<string>("select a.ComName from Community a left join UserCommunity b on a.CommunityId = b.CommunityId where b.DIDUserId = @0", community.RefDIDUserId),
                    Telegram = community.Telegram,
                    Eotc = CurrentUser.GetEUModel(user)?.StakeEotc ?? 0//调接口查eotc总数
                };

                var auths = await db.FetchAsync<ComAuth>("select * from ComAuth where CommunityId = @0 and IsDelete = 0 order by AuditStep", item.CommunityId);
                var list = new List<AuthInfo>();
                foreach (var auth in auths)
                {
                    list.Add(new AuthInfo()
                    {
                        UId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", auth.AuditUserId),
                        AuditStep = auth.AuditStep,
                        AuthDate = auth.AuditDate,
                        Name = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", auth.AuditUserId),
                        AuditType = auth.AuditType,
                        Remark = auth.Remark,
                        IsDao = auth.IsDao
                    });
                }
                authinfo.Auths = list;
                result.Add(authinfo);
            }
            return InvokeResult.Success(result);
        }

        /// <summary>
        /// 获取打回信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public async Task<Response<List<ComAuthRespon>>> GetBackCom(string userId,IsEnum isDao, long page, long itemsPerPage)
        {
            var result = new List<ComAuthRespon>();
            using var db = new NDatabase();
            //var items = await db.FetchAsync<ComAuth>("select * from ComAuth where AuditUserId = @0", userId);
            var items = (await db.PageAsync<ComAuth>(page, itemsPerPage, "select * from ComAuth where AuditUserId = @0 and IsDelete = 0 and IsDao = @1", userId, isDao)).Items;
            foreach (var item in items)
            {
                var community = await db.SingleOrDefaultAsync<Community>("select * from Community where CommunityId = @0", item.CommunityId);
                var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", community.DIDUserId);
                var authinfo = new ComAuthRespon()
                {
                    CommunityId = community.CommunityId,
                    DIDUser = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", community.DIDUserId),
                    RefUId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", community.RefDIDUserId),
                    RefUserId = community.RefDIDUserId,
                    RefName = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", community.RefDIDUserId),
                    Address = community.Address,
                    ComName = community.ComName,
                    Country = community.Country,
                    Province = community.Province,
                    City = community.City,
                    Area = community.Area,
                    CreateDate = community.CreateDate,
                    Describe = community.Describe,
                    Discord = community.Discord,
                    HasGroup = community.HasGroup,
                    HasOffice = community.HasOffice,
                    Image = community.Image,
                    Mail = await db.SingleOrDefaultAsync<string>("select Mail from DIDUser where DIDUserId = @0", community.DIDUserId),
                    Phone = community.Phone,
                    QQ = community.QQ,
                    AddressName = community.AddressName,
                    RefCommunityName = await db.SingleOrDefaultAsync<string>("select a.ComName from Community a left join UserCommunity b on a.CommunityId = b.CommunityId where b.DIDUserId = @0", community.RefDIDUserId),
                    Telegram = community.Telegram,
                    Eotc = CurrentUser.GetEUModel(user)?.StakeEotc ?? 0//调接口查eotc总数
                };

                var auths = await db.FetchAsync<ComAuth>("select * from ComAuth where CommunityId = @0 and IsDelete = 0 order by AuditStep", item.CommunityId);
                var list = new List<AuthInfo>();
                foreach (var auth in auths)
                {
                    list.Add(new AuthInfo()
                    {
                        UId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", auth.AuditUserId),
                        AuditStep = auth.AuditStep,
                        AuthDate = auth.AuditDate,
                        Name = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", auth.AuditUserId),
                        AuditType = auth.AuditType,
                        Remark = auth.Remark,
                        IsDao = auth.IsDao
                    });
                }
                authinfo.Auths = list;
                var next = auths.Where(a => a.AuditStep == item.AuditStep + 1).ToList();
                if (next.Count > 0 && (next[0].AuditType != AuditTypeEnum.未审核 && next[0].AuditType != AuditTypeEnum.审核通过))
                    result.Add(authinfo);
            }
            return InvokeResult.Success(result);
        }

        /// <summary>
        /// 社区图片上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response> UploadImage(IFormFile file)
        {
            try
            {
                var dir = new DirectoryInfo(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "Images/ComImges/"));

                //保存目录不存在就创建这个目录
                if (!dir.Exists)
                {
                    Directory.CreateDirectory(dir.FullName);
                }
                //var filename = upload.UserId + "_" + upload.Type + ".jpg";
                var filename = Guid.NewGuid().ToString() + ".jpg";
                using (var stream = new FileStream(dir.FullName + filename, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    //file.CopyTo(stream);
                }
                //return InvokeResult.Success("Images/AuthImges/" + upload.UId + "/" + filename);
                return InvokeResult.Success($"Images/ComImges/{filename}");
            }
            catch (Exception e)
            {
                _logger.LogError("UploadImage", e);
                return InvokeResult.Fail("Fail");
            }
        }

        /// <summary>
        /// 获取社区审核失败信息
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        public async Task<Response<List<AuthInfo>>> GetComAuthFail(string communityId)
        {
            using var db = new NDatabase();
            var auths = await db.FetchAsync<ComAuth>("select * from ComAuth where CommunityId = @0 and IsDelete = 0 order by AuditStep", communityId);
            var list = new List<AuthInfo>();
            foreach (var auth in auths)
            {
                list.Add(new AuthInfo()
                {
                    UId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", auth.AuditUserId),
                    AuditStep = auth.AuditStep,
                    AuthDate = auth.AuditDate,
                    Name = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", auth.AuditUserId),
                    AuditType = auth.AuditType,
                    Remark = auth.Remark
                });
            }
            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 两小时未审核去Dao审核
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userId"></param>
        public void ToDaoAuth(ComAuth item, string userId)
        {
            //两小时没人审核 自动到Dao审核
            //var t = new System.Timers.Timer(60000);//实例化Timer类，设置间隔时间为10000毫秒；
            var t = new System.Timers.Timer(2 * 60 * 60 * 1000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(async (object? source, System.Timers.ElapsedEventArgs e) =>
            {
                t.Stop(); //先关闭定时器
                          //todo: Dao审核
                using var db = new NDatabase();

                //随机Dao审核员审核
                var userIds = await db.FetchAsync<DIDUser>("select * from DIDUser where DIDUserId != @0 and IsExamine = 1 and IsLogout = 0 and IsEnable = 1", userId);
                if (userIds.Count == 0)
                    _logger.LogError("身份认证审核(没有找到审核员)");
                var random = new Random().Next(userIds.Count);
                var auditUserId = userIds[random].DIDUserId;

                if (item.AuditType == AuditTypeEnum.未审核)
                {
                    item.IsDelete = IsEnum.是;
                    await db.UpdateAsync(item);

                    item.ComAuthId = Guid.NewGuid().ToString();
                    item.IsDao = IsEnum.是;
                    item.AuditUserId = auditUserId;//Dao在线节点用户编号
                    item.CreateDate = DateTime.Now;
                    item.IsDelete = IsEnum.否;
                    await db.InsertAsync(item);
                }
            });//到达时间的时候执行事件；
            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            t.Start(); //启动定时器
        }
    }
}
