using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DBSharding.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //注册数据库连接
            IDbConnection db1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["db1"].ConnectionString);
            IDbConnection db2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["db2"].ConnectionString);
            IDbConnection db3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["db3"].ConnectionString);

            Dictionary<string, IDbConnection> connectionDic = new Dictionary<string, IDbConnection>();
            connectionDic.Add("0", db1);
            connectionDic.Add("1", db2);
            connectionDic.Add("2", db3);

            ShardingConnUtils.RegisConnGroup(connectionDic);


            //测试
            User user = new User();
            user.Id = 3;
            user.UserName = "alan";

            UserRepertory userRepertory = new UserRepertory();
            userRepertory.AddUser(user);

            User user2 = userRepertory.GetUserById(3);
            Console.WriteLine(user2.UserName);

        }
    }
}
