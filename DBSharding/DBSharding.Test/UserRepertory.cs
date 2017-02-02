using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DBSharding;

namespace DBSharding.Test
{
    public class UserRepertory
    {

        [ShardingMode(ShardingMode = ShardingMode.Mod, TableCount = 3)]
        public bool AddUser([ShardingKey(Keys = "Id")]User user)
        {
            IDbConnection connection = ShardingConnUtils.GetConnection(); //侵入代码，但也仅此一行，无论怎样，数据库操作都是要获取数据库连接

            int count = connection.Execute("insert into user value(@Id,@UserName)", user);
            return count > 0;
        }

        [ShardingMode(ShardingMode = ShardingMode.Mod, TableCount = 3)]
        public User GetUserById([ShardingKey]int Id)
        {
            IDbConnection connection = ShardingConnUtils.GetConnection();

            List<User> users = connection.Query<User>("select * from user where Id=@Id", new { Id = Id }).ToList();

            if (users.Count > 0)
            {
                return users[0];
            }
            return null;
        }
    }
}
