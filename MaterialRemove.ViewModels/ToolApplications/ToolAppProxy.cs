using g3;
using Machine.ViewModels.Interfaces;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.ToolApplications
{
    struct ToolAppProxy : g3.BoundedImplicitFunction3d, IIndexed//, IIntersector
    {
        readonly int _coll;
        readonly int _collIdx;
        readonly int _index;

        public ToolAppProxy(int coll, int collIdx, int index)
        {
            _coll = coll;
            _collIdx = collIdx;
            _index = index;
        }

        #region BoundedImplicitFunction3d
        public AxisAlignedBox3d Bounds() => ToolApplications.GetAt(_coll, _collIdx).Bounds();

        public double Value(ref Vector3d pt) => ToolApplications.GetAt(_coll, _collIdx).Value(ref pt);
        #endregion

        #region IIndexed
        public int Index => _index;
        #endregion

        //#region IIntersector
        //public bool Intersect(IPanel panel)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Intersect(IPanelSection section)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Intersect(ISectionFace face)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
    }
}
