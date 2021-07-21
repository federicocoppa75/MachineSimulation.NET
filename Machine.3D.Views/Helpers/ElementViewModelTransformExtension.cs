using Machine._3D.Views.Converters;
using Machine.Data.Enums;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Links;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Machine._3D.Views.Helpers
{
    internal static class ElementViewModelTransformExtension
    {
        public static Matrix3D GetChainTransformation(this IPanelElement panel)
        {
            var matrix = (panel as IMachineElement).GetChainTransformation();

            if (panel is IMovablePanel mp)
            {
                var m = Matrix3D.Identity;

                m.OffsetX = mp.OffsetX;
                matrix.Append(m);
            }

            var mc = Matrix3D.Identity;

            mc.OffsetX = panel.CenterX;
            mc.OffsetY = panel.CenterY;
            mc.OffsetZ = panel.CenterZ;

            matrix.Append(mc);

            return matrix;
        }

        public static Matrix3D GetChainTransformation(this IMachineElement endOfChain)
        {
            IMachineElement p = endOfChain;
            var list = new List<IMachineElement>();
            var matrix = Matrix3D.Identity;

            while (p != null)
            {
                list.Add(p);
                p = p.Parent;
            }

            for (int i = 0; i < list.Count; i++) matrix.Append(GetElementTransformation(list[i]));

            return matrix;
        }

        private static Matrix3D GetElementTransformation(IMachineElement e)
        {
            if ((e == null) || (e.Transformation == null))
            {
                return Matrix3D.Identity;
            }
            else
            {
                var ts = StaticTransformationConverter.Convert(e.Transformation);

                if (e.LinkToParent != null) ts.Append(GetLinkTransformation(e.LinkToParent));

                return ts;
            }
        }

        private static Matrix3D GetLinkTransformation(ILinkViewModel link)
        {
            switch (link.MoveType)
            {
                case LinkMoveType.Linear:
                    return GetLinearLinkTransformation(link as ILinearLinkViewModel);
                case Data.Enums.LinkMoveType.Pneumatic:
                    return GetPenumaticLinkTransformation(link as IPneumaticLinkViewModel);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            throw new NotImplementedException();
        }

        private static Matrix3D GetPenumaticLinkTransformation(IPneumaticLinkViewModel link)
        {
            switch (link.Type)
            {
                case LinkType.Linear:
                    return GetLinearTransformation(link.Direction, link.Value);
                case LinkType.Rotary:
                    return GetRataryTransformation(link.Direction, link.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Matrix3D GetLinearLinkTransformation(ILinearLinkViewModel link)
        {
            switch (link.Type)
            {
                case LinkType.Linear:
                    return GetLinearTransformation(link.Direction, link.Value, link.Pos);
                case LinkType.Rotary:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Matrix3D GetLinearTransformation(LinkDirection direction, double value, double pos = 0.0)
        {
            var matrix = Matrix3D.Identity;
            var v = value - pos;

            switch (direction)
            {
                case LinkDirection.X:
                    matrix.OffsetX = v;
                    break;
                case LinkDirection.Y:
                    matrix.OffsetY = v;
                    break;
                case LinkDirection.Z:
                    matrix.OffsetZ = v;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return matrix;
        }

        private static Matrix3D GetRataryTransformation(LinkDirection direction, double value, double pos = 0.0)
        {
            double x = 0.0, y = 0.0, z = 0.0;
            var matrix = Matrix3D.Identity;
            var v = value - pos;

            switch (direction)
            {
                case LinkDirection.X:
                    x = 1.0;
                    break;
                case LinkDirection.Y:
                    y = 1.0;
                    break;
                case LinkDirection.Z:
                    z = 1.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            matrix.Rotate(new Quaternion(new Vector3D(x, y, z), v));

            return matrix;
        }

    }
}
