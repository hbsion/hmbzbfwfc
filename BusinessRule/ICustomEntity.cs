using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marr.BusinessRule
{
    internal interface ICustomEntity
    {
        void CloneTo(object dest);
    }
}
