using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSharding
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ShardingModeAttribute : Attribute
    {
        public ShardingMode ShardingMode { get; set; }

        public int TableCount { get; set; }
    }
}
