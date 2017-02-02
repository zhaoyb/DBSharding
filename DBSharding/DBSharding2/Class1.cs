using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DBSharding;
using SheepAspect.Framework;
using SheepAspect.Runtime;

namespace DBSharding2
{
    [Aspect]
    public class Class1
    {

        [Around("PublicMethods")]
        public object LogAroundMethod(MethodJointPoint jp)
        {
            Trace.TraceInformation("Entering {0} on {1}. Args:{2}", jp.Method, jp.This, string.Join(",", jp.Args));
            //msg参数包含调用方法的信息，这里通过封装，使信息更丰富一些
            MethodInfo methodInfo = jp.Method;


            ShardingModeAttribute shardingMode = methodInfo.GetCustomAttribute<ShardingModeAttribute>();

            if (shardingMode != null)
            {
                switch (shardingMode.shardingMode)
                {
                    case ShardingMode.MOD:
                        MODShardingProcess(methodInfo, jp.Args);
                        break;
                    case ShardingMode.DAY:
                        break;
                    case ShardingMode.MONTH:
                        break;
                    case ShardingMode.YEAE:
                        break;
                }
            }



            try
            {
                var result = jp.Execute();

                if (jp.Method.ReturnType == typeof(void))
                    result = "{void}";

                Trace.TraceInformation("Exitting {0}. Result: {1}", jp.Method, result);
                return result;


            }
            catch (Exception e)
            {

                throw;
            }
        }

        private void MODShardingProcess(MethodInfo methodInfo, object[] args)
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
                        if (string.IsNullOrEmpty(shardingKeyAttribute.keys))
                        {
                            object shardingObjectValue = args[parameterInfo.Position];

                            shardingValue = Convert.ToInt32(shardingObjectValue);

                        }
                        else
                        {
                            string shardingKey = shardingKeyAttribute.keys;
                            object targetObject = args[parameterInfo.Position];
                            object shardingObjectValue = targetObject.GetType().GetProperty(shardingKey).GetValue(targetObject);

                            shardingValue = Convert.ToInt32(shardingObjectValue);

                        }

                        int connectionIndex = shardingValue % 3;
                        ConnUtils.SetConnectionIndex(connectionIndex);
                    }
                }
            }
        }
    }
}
