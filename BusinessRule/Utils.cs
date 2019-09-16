using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marr.BusinessRule
{
    public class Utils
    {
        public static IEnumerable<T> ExecLTS<T>(IQueryable<T> lts)
        {
            //return lts;
            List<T> ret = new List<T>();
            foreach (T item in lts)
            {
                ret.Add(item);
            }
            return ret;
        }
    }
}
