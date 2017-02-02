using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBSharding
{
    public class ShardingConnUtils
    {
        private static ThreadLocal<string> local = new ThreadLocal<string>();

        private static Dictionary<string, IDbConnection> _connectionDic = null;

        public static IDbConnection GetConnection()
        {
            IDbConnection conn = _connectionDic[local.Value];
            conn.Open();
            return conn;
        }

        public static void SetConnectionIndex(string connectionKey)
        {
            local.Value = connectionKey;
        }

        public static void RegisConnGroup(Dictionary<string, IDbConnection> connectionDic)
        {
            _connectionDic = connectionDic;
        }

    }
}
