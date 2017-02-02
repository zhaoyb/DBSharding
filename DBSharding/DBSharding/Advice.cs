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

namespace DBSharding
{
    [Aspect]
    public class Advice
    {
        [SelectMethods("HasCustomAttributeType:'DBSharding.ShardingModeAttribute'")]
        public void PublicMethods() { }

        [Around("PublicMethods")]
        public object LogAroundMethod(MethodJointPoint jp)
        {
            try
            {
                ShardingCore.Process(jp.Method,jp.Args);

                var result = jp.Execute();

                if (jp.Method.ReturnType == typeof(void))
                    result = "{void}";

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
