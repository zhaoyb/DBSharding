using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SheepAspect.Framework;

namespace DBSharding2
{
    public class Class2 : Class1
    {
        [SelectMethods("Name: 'GetUserById'")]
        public void PublicMethods() { }

    }
}
