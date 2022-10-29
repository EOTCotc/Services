/*
  ###CSharp Code Generate###
  UserLogout
  Create by User(EMAIL) 2022/9/2 16:38:32
  用户注销表

UserLogout(用户注销表)
-------------------------------------------------
UserLogoutId(编号)              PKString(50)
DIDUserId(用户编号)             FKString(50) //<<关联:DIDUser.DIDUserId>>
LogoutDate(注销时间)            Date
Reason(注销原因)                String(255)
SubmitDate(提交注销时间)        Date
IsCancel(是否取消)              Enum         //0 未取消 1 取消

*/



using NPoco;

namespace DID.Entitys
{
    /* 用户注销表 */
    [PrimaryKey("UserLogoutId", AutoIncrement = false)]
    public class UserLogout
    {

        //编号
        public string UserLogoutId
        {
            get; set; 
        }
        //用户编号
        public string DIDUserId
        {
            get; set;
        }
        //注销时间
        public DateTime? LogoutDate
        {
            get; set;
        }
        //注销原因
        public string Reason
        {
            get; set;
        }
        //提交注销时间
        public DateTime? SubmitDate
        {
            get; set;
        }
        //是否取消 0 未取消 1 取消
        public IsEnum IsCancel
        {
            get; set;
        }
        
    }

}

