using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marr.BusinessRule
{
    public struct Filter
    {
        public string Field { get; set; }
        public string SearchText { get; set; }

        public static readonly Filter Empty = new Filter();
    }
}
