using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Probing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Probing
{
    public class ProbeFactory : IProbeFactory
    {
        private static int _probeCount = 0;

        public IProbe Create(IProbableElement parent, Point point)
        {
            var me = parent as IMachineElement;
            var id = ++_probeCount;
            var factory = Ioc.SimpleIoc<IProbePointTransformerFactory>.GetInstance();
            var probe = new PointProbeViewModel()
            {
                ProbeId = id,
                Name = $"Point probe ({id})",
                Parent = me,
                X = point.X,
                Y = point.Y,
                Z = point.Z,
                IsVisible = true
            };

            var t = factory.GetTransformer(probe);
            var p = t.Transform(point, true);

            probe.RelativeX = p.X;
            probe.RelativeY = p.Y;
            probe.RelativeZ = p.Z;

            if(t is IProbePointChangableTransformer ct) probe.Transformer = ct;

            me.Children.Add(probe);

            return probe;
        }

        public IProbe Create(IProbePoint master, IProbePoint slave)
        {
            var id = ++_probeCount;
            var me = master as IMachineElement;
            var probe = new DistanceProbeViewModel()
            {
                ProbeId = id,
                Name = $"Distance probe ({id})",
                Parent = me,
                IsVisible = true,
                Master = master,
                Slave = slave
            };

            me.Children.Add(probe);

            return probe;
        }
    }
}
