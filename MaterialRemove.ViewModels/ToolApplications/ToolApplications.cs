using g3;
using Machine.ViewModels.Interfaces;
using MaterialRemove.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.ToolApplications
{
    abstract class ToolApplications
    {
        protected static List<ToolApplications> _collList;
        protected static object _lockObj = new object();

        static ToolApplications()
        {
            _collList = new List<ToolApplications>();
        }

        public static ToolAppProxy Add<T>(ref T item) where T : g3.BoundedImplicitFunction3d, IIndexed, IIntersector
        {
            return ToolApplications<T>.Instance.Add(ref item);
        }

        public static g3.BoundedImplicitFunction3d GetAt(int index1, int index2) => _collList[index1].GetAt(index2);

        protected abstract g3.BoundedImplicitFunction3d GetAt(int index);
    }

    class ToolApplications<T> : ToolApplications where T : g3.BoundedImplicitFunction3d, IIndexed, IIntersector
    {
        private int _collIdx = -1;
        private List<T> _items = new List<T>();
        private object _addLockObj = new object();

        private static ToolApplications<T> _instance;
        public static ToolApplications<T> Instance
        {
            get
            {
                lock (_lockObj)
                {
                    if (_instance == null) _instance = new ToolApplications<T>();
                }

                return _instance;
            }
        }

        public ToolApplications()
        {
            _collIdx = _collList.Count;
            _collList.Add(this);
        }
 
        public ToolAppProxy Add(ref T item)
        {
            int n = 0;

            lock(_addLockObj)
            {
                n = _items.Count;                                
            }

            _items.Add(item);

            return new ToolAppProxy(_collIdx, n, item.Index);
        }

        protected override BoundedImplicitFunction3d GetAt(int index) => _items[index];
    }
}
