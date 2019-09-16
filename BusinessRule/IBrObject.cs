using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marr.BusinessRule
{
    public interface IBrObject<T> : IDisposable 
    {
        IEnumerable<T> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize);
        int Count(Filter filter);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        T NewEntity();
        void CheckAction(T entity, ActionEnum action);
    }

    public enum ActionEnum
    {
        Add,
        Modify,
    }
}
