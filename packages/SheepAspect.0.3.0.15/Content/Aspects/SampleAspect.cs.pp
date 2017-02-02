using System;
using System.Diagnostics;
using SheepAspect.Framework;
using SheepAspect.Runtime;

namespace $rootNamespace$.Aspects
{
    [Aspect]
    public class SampleAspect
    {
        [SelectMethods("Public & !InType:ThisAspect")]
        public void PublicMethods() { }

        [Around("PublicMethods")]
        public object LogAroundMethod(MethodJointPoint jp)
        {
            Trace.TraceInformation("Entering {0} on {1}. Args:{2}", jp.Method, jp.This, string.Join(",", jp.Args));
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
                Trace.TraceInformation("Exitting {0} with exception: '{1}'", jp.Method, e);
                throw;
            }
        }
    }
}