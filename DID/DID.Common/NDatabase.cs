using Microsoft.Data.SqlClient;
using NPoco;
using System.Data.Common;

namespace DID.Common
{
    public class NDatabase : Database
    {
        private static object locker = new();
        public NDatabase() : base(AppSettings.GetValue("ConnectionString"), DatabaseType.SqlServer2012, SqlClientFactory.Instance)
        {

        }
        /// <summary>
        /// sql日志
        /// </summary>
        /// <param name="cmd"></param>
        protected override void OnExecutingCommand(DbCommand cmd)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/Sql/");
            string fn = string.Format(@"{0}\{1}.trc", path, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            lock (locker)
            {
                File.AppendAllText(fn,
                    string.Format("{0}:{1}\n", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), FormatCommand(cmd)));
            }
        }
    }
}
