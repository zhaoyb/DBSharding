using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DBSharding
{
    public class ShardingCore
    {
        public static void Process(MethodInfo methodInfo, object[] args)
        {
            ShardingModeAttribute shardingMode = methodInfo.GetCustomAttribute<ShardingModeAttribute>();

            if (shardingMode != null)
            {
                switch (shardingMode.ShardingMode)
                {
                    case ShardingMode.Mod:
                        int talbeCount = shardingMode.TableCount;
                        ModShardingProcess(methodInfo, args, talbeCount);
                        break;
                    case ShardingMode.Day:
                        break;
                    case ShardingMode.Month:
                        break;
                    case ShardingMode.Yeae:
                        break;
                }
            }
        }

        private static void ModShardingProcess(MethodInfo methodInfo, object[] args, int tableCount)
        {
            ParameterInfo[] paramseteInfos = methodInfo.GetParameters();  //获取方法调用参数

            if (paramseteInfos != null && paramseteInfos.Length > 0)
            {
                foreach (ParameterInfo parameterInfo in paramseteInfos)
                {
                    ShardingKeyAttribute shardingKeyAttribute = parameterInfo.GetCustomAttribute<ShardingKeyAttribute>();
                    if (shardingKeyAttribute != null)
                    {
                        int shardingValue = 0;
                        if (string.IsNullOrEmpty(shardingKeyAttribute.Keys))
                        {
                            object shardingObjectValue = args[parameterInfo.Position];

                            shardingValue = Convert.ToInt32(shardingObjectValue);

                        }
                        else
                        {
                            string shardingKey = shardingKeyAttribute.Keys;
                            object targetObject = args[parameterInfo.Position];
                            object shardingObjectValue = targetObject.GetType().GetProperty(shardingKey).GetValue(targetObject);

                            shardingValue = Convert.ToInt32(shardingObjectValue);

                        }
                        int connectionIndex = shardingValue % tableCount;
                        ShardingConnUtils.SetConnectionIndex(connectionIndex.ToString());
                    }
                }
            }
        }
    }
}
