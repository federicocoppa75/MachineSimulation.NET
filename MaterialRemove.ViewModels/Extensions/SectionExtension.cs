using g3;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Enums;
using MaterialRemove.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVMIoc = Machine.ViewModels.Ioc;

namespace MaterialRemove.ViewModels.Extensions
{
    static class SectionExtension
    {
        static IRemovalParameters _removalParameters;

        public static IList<ISectionFace> CreateFaces(this IPanelSection section, SectionPosition position, IRemovalParameters removalParameters)
        {
            var list = new List<ISectionFace>();

            _removalParameters = removalParameters;

            switch (position)
            {
                case SectionPosition.Center:
                    list.AddFaceUpDown(section, position);
                    break;

                case SectionPosition.SideTop:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceTop(section, position);
                    break;

                case SectionPosition.SideRigth:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceRight(section, position);
                    break;

                case SectionPosition.SideBottom:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceBottom(section, position);
                    break;

                case SectionPosition.SideLeft:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceLeft(section, position);
                    break;

                case SectionPosition.CornerTopRight:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceTop(section, position);
                    list.AddFaceRight(section, position);
                    break;

                case SectionPosition.CornerTopLeft:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceTop(section, position);
                    list.AddFaceLeft(section, position);
                    break;

                case SectionPosition.CornerBottomLeft:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceBottom(section, position);
                    list.AddFaceLeft(section, position);
                    break;

                case SectionPosition.CornerBottomRight:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceRight(section, position);
                    list.AddFaceBottom(section, position);
                    break;

                case SectionPosition.EndBottom:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceBottom(section, position);
                    list.AddFaceLeft(section, position);
                    list.AddFaceRight(section, position);
                    break;

                case SectionPosition.EndLeft:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceBottom(section, position);
                    list.AddFaceLeft(section, position);
                    list.AddFaceTop(section, position);
                    break;

                case SectionPosition.EndRight:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceBottom(section, position);
                    list.AddFaceTop(section, position);
                    list.AddFaceRight(section, position);
                    break;

                case SectionPosition.EndTop:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceTop(section, position);
                    list.AddFaceLeft(section, position);
                    list.AddFaceRight(section, position);
                    break;

                case SectionPosition.CenterAlongX:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceBottom(section, position);
                    list.AddFaceTop(section, position);
                    break;

                case SectionPosition.CenterAlongY:
                    list.AddFaceUpDown(section, position);
                    list.AddFaceLeft(section, position);
                    list.AddFaceRight(section, position);
                    break;

                default:
                    throw new ArgumentException();
            }

            _removalParameters = null;

            return list;
        }

        internal static AxisAlignedBox3d GetBound(this IPanelSection section) => new AxisAlignedBox3d(new Vector3d(section.CenterX, section.CenterY, section.CenterZ), section.SizeX / 2.0, section.SizeY / 2.0, section.SizeZ / 2.0);


        private static ISectionFace CreateFace(double centerX, double centerY, double centerZ, double sizeX, double sizeY, Orientation orientation)
        {
            var vm = MVMIoc.SimpleIoc<IElementViewModelFactory>.GetInstance().CreateSectionFaceViewModel();

            vm.CenterX = centerX;
            vm.CenterY = centerY;
            vm.CenterZ = centerZ;
            vm.SizeX = sizeX;
            vm.SizeY = sizeY;
            vm.Orientation = orientation;
            vm.RemovalParameters = _removalParameters;

            return vm;
        }

        private static void AddFaceUpDown(this IList<ISectionFace> list, IPanelSection section, SectionPosition position)
        {
            list.Add(CreateFace(section.CenterX, section.CenterY, section.GetCenterZFaceUp(), section.SizeX, section.SizeY, Orientation.ZPos));
            list.Add(CreateFace(section.CenterX, section.CenterY, section.GetCenterZFaceDown(), section.SizeX, section.SizeY, Orientation.ZNeg));
        }
        private static void AddFaceTop(this IList<ISectionFace> list, IPanelSection section, SectionPosition position)
        {
            list.Add(CreateFace(section.CenterX, section.GetCenterYFaceTop(), section.CenterZ, section.SizeX, section.SizeZ, Orientation.YPos));
        }
        private static void AddFaceBottom(this IList<ISectionFace> list, IPanelSection section, SectionPosition position)
        {
            list.Add(CreateFace(section.CenterX, section.GetCenterYFaceBottom(), section.CenterZ, section.SizeX, section.SizeZ, Orientation.YNeg));
        }
        private static void AddFaceRight(this IList<ISectionFace> list, IPanelSection section, SectionPosition position)
        {
            list.Add(CreateFace(section.GetCenterXFaceRigth(), section.CenterY, section.CenterZ, section.SizeY, section.SizeZ, Orientation.XPos));
        }
        private static void AddFaceLeft(this IList<ISectionFace> list, IPanelSection section, SectionPosition position)
        {
            list.Add(CreateFace(section.GetCenterXFaceLeft(), section.CenterY, section.CenterZ, section.SizeY, section.SizeZ, Orientation.XNeg));
        }
        private static double GetCenterXFaceRigth(this IPanelSection section) => section.CenterX + section.SizeX / 2.0;
        private static double GetCenterXFaceLeft(this IPanelSection section) => section.CenterX - section.SizeX / 2.0;
        private static double GetCenterYFaceTop(this IPanelSection section) => section.CenterY + section.SizeY / 2.0;
        private static double GetCenterYFaceBottom(this IPanelSection section) => section.CenterY - section.SizeY / 2.0;
        private static double GetCenterZFaceUp(this IPanelSection section) => section.CenterZ + section.SizeZ / 2.0;
        private static double GetCenterZFaceDown(this IPanelSection section) => section.CenterZ - section.SizeZ / 2.0;

        internal static void RemoveAction(this IPanelSection section, int index)
        {
            foreach (var face in section.Faces)
            {
                face.RemoveAction(index);
            }

            if (section.Volume is SectionVolumeViewModel svvm)
            {
                svvm.RemoveAction(index);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        internal static Task RemoveActionAsync(this IPanelSection section, int index)
        {
            Task[] tasks = {
                RemoveActionFromFacesAsync(section, index),
                RemoveActionFromVolumeAsync(section, index)
            };

            return Task.WhenAll(tasks);
        }

        private static Task RemoveActionFromVolumeAsync(this IPanelSection section, int index)
        {
            return Task.Run(() =>
            {
                if(section.Volume != null) 
                {
                    if (section.Volume is SectionVolumeViewModel svvm)
                    {
                        svvm.RemoveAction(index);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            });
        }

        private static Task RemoveActionFromFacesAsync(this IPanelSection section, int index)
        {
            var tasks = new List<Task>();

            foreach (var face in section.Faces)
            {
                tasks.Add(Task.Run(() =>
                {
                    face.RemoveAction(index);
                }));
            }

            return Task.WhenAll(tasks);
        }
    }
}
