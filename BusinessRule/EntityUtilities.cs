using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;

namespace marr.BusinessRule
{
    public static class EntityUtilities
    {
        public static void CloneEntity<T>(T src, T dest)
        {
            if (typeof(T).GetInterface("ICustomEntity") == null)
            {
                foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
                {
                    ColumnAttribute ca = (ColumnAttribute)pd.Attributes[typeof(ColumnAttribute)];
                    if (ca != null && !ca.IsDbGenerated)
                    {
                        pd.SetValue(dest, pd.GetValue(src));
                    }
                }
            }
            else
            {
                ((ICustomEntity)src).CloneTo(dest);
            }
        }
    }
}
