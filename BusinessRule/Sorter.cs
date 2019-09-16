using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marr.BusinessRule
{
    public struct Sorter
    {
        public string Field { get; set; }
        public bool ASC { get; set; }

        public static readonly Sorter Empty = new Sorter();
    }
}
