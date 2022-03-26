using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.Handles;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using MDE = Machine.Data.Enums;

namespace Machine.ViewModels.Factories
{
    public class HandleFactory : IHandleFactory
    {
        public IElementHandle Create(IMachineElement element, MDE.ElementHandle type = MDE.ElementHandle.Position)
        {
            IElementHandle handle = null;

            if ((element != null) && !string.IsNullOrEmpty(element.ModelFile))
            {
                switch (type)
                {
                    case MDE.ElementHandle.Position:
                        handle = CreatePositionElementHandle(element);
                        break;
                    case MDE.ElementHandle.Rotation:
                        handle = CreaterRotationElementHandle(element);
                        break;
                    default:
                        break;
                }
            }

            return handle;
        }

        private IElementHandle CreatePositionElementHandle(IMachineElement element)
        {
            var bbProvider = Ioc.SimpleIoc<IElementBoundingBoxProvider>.GetInstance();

            if (bbProvider.GetBoundingBox(element, out double minX, out double minY, out double minZ, out double maxX, out double maxY, out double maxZ))
            {
                var handle = new Handles.ElementHandleViewModel()
                {
                    IsVisible = true,
                    MinX = minX,
                    MinY = minY,
                    MinZ = minZ,
                    MaxX = maxX,
                    MaxY = maxY,
                    MaxZ = maxZ
                };

                handle.Children.Add(new Handles.PositionHandleViewModel() { IsVisible = true, Parent = handle, Type = Interfaces.Handles.Type.X });
                handle.Children.Add(new Handles.PositionHandleViewModel() { IsVisible = true, Parent = handle, Type = Interfaces.Handles.Type.Y });
                handle.Children.Add(new Handles.PositionHandleViewModel() { IsVisible = true, Parent = handle, Type = Interfaces.Handles.Type.Z });

                return handle;
            }
            else
            {
                return null;
            }
        }

        private IElementHandle CreaterRotationElementHandle(IMachineElement element)
        {
            var bbProvider = Ioc.SimpleIoc<IElementBoundingBoxProvider>.GetInstance();

            if (bbProvider.GetBoundingBox(element, out double minX, out double minY, out double minZ, out double maxX, out double maxY, out double maxZ))
            {
                var handle = new Handles.ElementHandleViewModel()
                {
                    IsVisible = true,
                    MinX = minX,
                    MinY = minY,
                    MinZ = minZ,
                    MaxX = maxX,
                    MaxY = maxY,
                    MaxZ = maxZ
                };

                handle.Children.Add(new Handles.RotationHandleViewModel() { IsVisible = true, Parent = handle, Type = Interfaces.Handles.Type.X });
                handle.Children.Add(new Handles.RotationHandleViewModel() { IsVisible = true, Parent = handle, Type = Interfaces.Handles.Type.Y });
                handle.Children.Add(new Handles.RotationHandleViewModel() { IsVisible = true, Parent = handle, Type = Interfaces.Handles.Type.Z });

                return handle;
            }
            else
            {
                return null;
            }
        }
    }
}
