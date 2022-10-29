using Dao.Models.Base;
using DID.Common;

namespace Dao.Common
{
    public class WalletHelp
    {
        /// <summary>
        /// 获取钱包地址ID
        /// </summary>
        /// <param name="walletAddress"></param>
        /// <param name="otype"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string GetWalletId(DaoBaseReq req)
        {
            using var db = new NDatabase();
            var walletId =  db.SingleOrDefault<string>("select WalletId from Wallet where WalletAddress = @0 and Otype = @1 and Sign = @2 and IsLogout = 0 and IsDelete = 0",
                                                                req.WalletAddress, req.Otype, req.Sign);
            return walletId;
        }

        /// <summary>
        /// 获取用户编号
        /// </summary>
        /// <param name="walletAddress"></param>
        /// <param name="otype"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string GetUserId(DaoBaseReq req)
        {
            using var db = new NDatabase();
            var walletId = db.SingleOrDefault<string>("select DIDUserId from Wallet where WalletAddress = @0 and Otype = @1 and Sign = @2 and IsLogout = 0 and IsDelete = 0",
                                                                req.WalletAddress, req.Otype, req.Sign);
            return walletId;
        }

        /// <summary>
        /// 获取用户钱包地址ID
        /// </summary>
        /// <param name="walletAddress"></param>
        /// <param name="otype"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string GetWalletIds(DaoBaseReq req)
        {
            using var db = new NDatabase();
            var walletId = db.SingleOrDefault<string>("select WalletId from Wallet where  DIDUserId = " +
                                                    "(select DIDUserId from Wallet where WalletAddress = @0 and Otype = @1 and Sign = @2 and IsLogout = 0 and IsDelete = 0) and IsLogout = 0 and IsDelete = 0",
                                                    req.WalletAddress, req.Otype, req.Sign);
            return walletId;
        }

        /// <summary>
        /// 获取提交人
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public static string GetSubmitter(string walletId)
        {
            using var db = new NDatabase();
            var uid = db.SingleOrDefault<string>("select b.Uid from Wallet a left join DIDUser b on a.DIDUserId = b.DIDUserId " +
            "where a.WalletId = @0 and a.IsLogout = 0 and a.IsDelete = 0", walletId);
            var name = db.SingleOrDefault<string>("select c.Name from DIDUser a left join Wallet b on a.DIDUserId = b.DIDUserId left join UserAuthInfo c on a.UserAuthInfoId = c.UserAuthInfoId " +
                "where b.WalletId = @0 and b.IsLogout = 0 and b.IsDelete = 0", walletId);

            return name + "(" + uid + ")";
        }

        /// <summary>
        /// 获取UId + Name
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetUidName(string userId)
        {
            using var db = new NDatabase();
            var uid = db.SingleOrDefault<string>("select Uid from DIDUser where DIDUserId = @0", userId);
            var name = db.SingleOrDefault<string>("select b.Name from DIDUser a left join UserAuthInfo b on a.UserAuthInfoId = b.UserAuthInfoId " +
                "where a.DIDUserId = @0", userId);

            return name??"未认证" + "(" + uid + ")";
        }

        /// <summary>
        /// 获取Name
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetName(string userId)
        {
            using var db = new NDatabase();
            var name = db.SingleOrDefault<string>("select b.Name from UserAuthInfo b left join DIDUser a  on a.UserAuthInfoId = b.UserAuthInfoId " +
                "where a.DIDUserId = @0 and a.AuthType = 2", userId);

            return name ?? "未认证";
        }

        /// <summary>
        /// 获取Phone
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetPhone(string userId)
        {
            using var db = new NDatabase();
            var phone = db.SingleOrDefault<string>("select b.PhoneNum from UserAuthInfo b left join DIDUser a  on a.UserAuthInfoId = b.UserAuthInfoId " +
                "where a.DIDUserId = @0 and a.AuthType = 2", userId);

            return phone;
        }
    }
}