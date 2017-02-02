using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSharding
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ShardingKeyAttribute : Attribute
    {
        public string Keys { get; set; }
    }
}
