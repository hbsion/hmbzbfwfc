using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marr.BusinessRule
{
    public class ListContext<T>
    {
        private ListContext()
        {
            _state = ListContextState.View;
            _items = new T[0];
            _loadedFirstPostion = 0;
            _count = 0;
            _innerPosition = -1;
        }

        public ListContext(IBrObject<T> br) : this()
        {
            _br = br;
            _count = _br.Count(_filter);
        }

        private IBrObject<T> _br;
        private T[] _items;
        private T[] _appendedItems;
        private int _loadedFirstPostion;
        private int _count;
        private int _innerPosition;
        private T _editingEntity;
        private ListContextState _state;
        private Filter _filter;
        private Sorter _sorter;
        private int _pageSize;

        private void EnsureCurrentLoaded()
        {
            if (_items != null && _items.Length != 0)
            {
                if (_innerPosition >= 0 && _innerPosition < _items.Length)
                    return;
            }
            if (_innerPosition < 0 && _appendedItems != null)
            {
                if (-1 - _innerPosition < _appendedItems.Length)
                    return;
            }

            this.RefreshLoaded();
        }

        private void AppendItem(T item)
        {
            if (_appendedItems != null)
            {
                T[] temp = new T[_appendedItems.Length + 1];
                Array.Copy(_appendedItems, temp, _appendedItems.Length);
                temp[_appendedItems.Length] = item;
                GC.SuppressFinalize(_appendedItems);
                _appendedItems = temp;
            }
            else
                _appendedItems = new T[] { item };
        }

        private void RemoveCurrent()
        {
            if (_innerPosition >= 0)
            {
                T[] temp = new T[_items.Length - 1];
                int index = _innerPosition;

                if (index > 0) Array.Copy(_items, temp, index);
                if (index < _items.Length - 1) Array.Copy(_items, index + 1, temp, index, _items.Length - index - 1);
                GC.SuppressFinalize(_items);
                _items = temp;

                _count--;
            }
            else if (_innerPosition < 0 && _appendedItems != null && _appendedItems.Length > -1 - _innerPosition)
            {
                T[] temp = new T[_appendedItems.Length - 1];
                int index = -1 - _innerPosition;

                if (index > 0) Array.Copy(_appendedItems, temp, index);
                if (index < _appendedItems.Length - 1) Array.Copy(_appendedItems, index + 1, temp, index, _appendedItems.Length - index - 1);
                GC.SuppressFinalize(_appendedItems);
                _appendedItems = temp;
            }
        }

        public void RefreshLoaded()
        {
            if (_pageSize == 0) return;
            if (_innerPosition < 0) return;

            this.Refresh();
        }

        public void Refresh()
        {
            _count = _br.Count(_filter);

            if (_pageSize > 0)
            {
                int fp = _innerPosition + _loadedFirstPostion;
                if (fp >= _count) fp = _count - 1;

                int pageIndex = (int)(fp / _pageSize);
                _items = _br.Query(_filter, _sorter, pageIndex, _pageSize).ToArray();
                _loadedFirstPostion = pageIndex * _pageSize;
                _innerPosition = fp - _loadedFirstPostion;
            }
            else
            {
                _items = new T[0];
                _loadedFirstPostion = 0;
                _innerPosition = -1;
            }

            if (_appendedItems != null)
            {
                GC.SuppressFinalize(_appendedItems);
                _appendedItems = null;
            }
        }

        public Filter CurrentFilter 
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                _count = _br.Count(_filter);
                this.RefreshLoaded();
            }
        }

        public Sorter CurrentSorter 
        {
            get
            {
                return _sorter;
            }
            set
            {
                _sorter = value;
                this.RefreshLoaded();
            }
        }

        public int PageSize 
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                this.RefreshLoaded();
            }
        }

        public int Position
        {
            get
            {
                if (_innerPosition >= 0)
                    return _innerPosition + _loadedFirstPostion;
                else if (_appendedItems != null && _appendedItems.Length > -1 - _innerPosition)
                    return _count + (-1 - _innerPosition);
                else
                    return -1;
            }
            set
            {
                if (value >= _loadedFirstPostion && value < _loadedFirstPostion + _items.Length)
                    _innerPosition = value - _loadedFirstPostion;
                else if (value >= _count && _appendedItems != null && _appendedItems.Length > value - _count)
                    _innerPosition = _count - value - 1;
                else
                {
                    _loadedFirstPostion = 0;
                    _innerPosition = value;
                    GC.SuppressFinalize(_items);
                    _items = new T[0];
                    this.EnsureCurrentLoaded();
                }

            }
        }

        public int Count
        {
            get
            {
                if (_appendedItems != null)
                    return _count + _appendedItems.Length;
                else
                    return _count;
            }
        }

        public T Current
        {
            get
            {
                if (this.State == ListContextState.View)
                {
                    this.EnsureCurrentLoaded();
                    if (_innerPosition >= 0)
                        return _items[_innerPosition];
                    else if (_appendedItems != null && _appendedItems.Length > -1 - _innerPosition)
                        return _appendedItems[-1 - _innerPosition];
                    else
                        return default(T);
                }
                else
                {
                    return _editingEntity;
                }
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                if (this.Position >= 0)
                    return this.Position / this.PageSize;
                else
                    return 0;
            }
        }

        public T[] CurrentPageItems
        {
            get
            {
                this.EnsureCurrentLoaded();

                T[] ret;
                if (_items != null && _items.Length > 0)
                {
                    ret = new T[_items.Length];
                    Array.Copy(_items, ret, _items.Length);
                }
                else
                {
                    ret = new T[1];
                    ret[0] = _br.NewEntity();
                }

                return ret;
            }
        }

        public ListContextState State
        {
            get
            {
                return _state;
            }
        }

        public bool MoveNext()
        {
            if (this.Position < this.Count - 1 && this.State == ListContextState.View)
            {
                this.Position++;
                return true;
            }
            else
                return false;
        }

        public bool MovePrev()
        {
            if (this.Position > 0 && this.State == ListContextState.View)
            {
                this.Position--;
                return true;
            }
            else
                return false;
        }

        public void BeginEdit()
        {
            if (this.State == ListContextState.View)
            {
                _editingEntity = _br.NewEntity();
                EntityUtilities.CloneEntity<T>(this.Current, _editingEntity);
                _state = ListContextState.Editing;
            }
        }

        public void EndEdit()
        {
            switch (this.State)
            {
                case ListContextState.Editing:
                    _br.CheckAction(_editingEntity, ActionEnum.Modify);
                    _br.Update(_editingEntity);
                    _state = ListContextState.View;
                    EntityUtilities.CloneEntity<T>(_editingEntity, this.Current);
                    break;
                case ListContextState.AddNew:
                    _br.CheckAction(_editingEntity, ActionEnum.Add);
                    _br.Insert(_editingEntity);
                    _state = ListContextState.View;
                    this.AppendItem(_editingEntity);
                    this.Position = this.Count - 1;
                    break;
            }
        }

        public void CancelEdit()
        {
            if (this.State != ListContextState.View)
            {
                _state = ListContextState.View;
            }
        }

        public void AddNew()
        {
            if (this.State == ListContextState.View)
            {
                _editingEntity = _br.NewEntity();
                _state = ListContextState.AddNew;
            }
        }

        public void Delete()
        {
            if (this.State == ListContextState.View && this.Position != -1)
            {
                _br.Delete(this.Current);
                this.RemoveCurrent();

                if (this.Position == -1) this.Position = this.Count - 1;
            }
        }
    }
}
