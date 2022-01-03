using MDB = Machine.Data.Base;
using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.ToolProxies
{
    internal class AngolarTransmission1ProxyViewModel : ToolProxyViewModel, ISubSpindleObserver
    {
        protected IAngularTransmission AngolarTransmission => GetTool<IAngularTransmission>();

        [Category("General")]
        [PropertyOrder(7)]
        [Editor(typeof(PropertyGridFilePicker), typeof(PropertyGridFilePicker))]
        public string BodyModelFile
        {
            get => AngolarTransmission.BodyModelFile;
            set
            {
                if (Set(AngolarTransmission.BodyModelFile, value, v => AngolarTransmission.BodyModelFile = v, nameof(BodyModelFile))) UpdateTool();
            }
        }

        [Category("Sub spindles")]
        [PropertyOrder(1)]
        [ExpandableObject]
        public ATSubSpindleProxyViewModel SubSpindle1 { get; private set; }

        [Browsable(false)]
        public override bool IsAngolarTransmission => true;

        protected override string GetToolType() => "Angolar transmission (1)";

        public AngolarTransmission1ProxyViewModel(int nSpindles = 1) : base(CreateTool<IAngularTransmission>())
        {
            AngolarTransmission.SetSubSpindlesNumber(nSpindles);
            SubSpindle1 = new ATSubSpindleProxyViewModel(AngolarTransmission.FirstSubSpindle(), this);
        }

        public AngolarTransmission1ProxyViewModel(IAngularTransmission at) : base(at)
        {
            SubSpindle1 = new ATSubSpindleProxyViewModel(at.FirstSubSpindle(), this);
        }

        public AngolarTransmission1ProxyViewModel(AngolarTransmission1ProxyViewModel src) : this()
        {;
            CopyFrom(src);
        }

        public void NotifySubSpindleChanged() => UpdateTool();

        public override ToolProxyViewModel CreateCopy() => new AngolarTransmission1ProxyViewModel(this);

        protected void CopyFrom(AngolarTransmission1ProxyViewModel src)
        {
            base.CopyFrom(src);
            BodyModelFile = src.BodyModelFile;
            SubSpindle1.CopyFrom(src.SubSpindle1);
        }
    }

    internal static class SubSpindleExtension
    {
        public static MDB.Point Position(this ISubspindle ss)
        {
            ss.GetPosition(out double x, out double y, out double z);
            return new MDB.Point() { X = x, Y = y, Z = z };
        }

        public static MDB.Vector Direction(this ISubspindle ss)
        {
            ss.GetDirection(out double x, out double y, out double z);
            return new MDB.Vector() { X = x, Y = y,Z = z };
        }

        public static int GetSubSpindlesCount(this IAngularTransmission at)
        {
            var n = 0;

            foreach (var item in at.GetSubspindles()) n++;

            return n;
        }
        public static ISubspindle FirstSubSpindle(this IAngularTransmission at) => GetSubSpindle(at, 0);
        public static ISubspindle SecondSubSpindle(this IAngularTransmission at) => GetSubSpindle(at, 1);
        public static ISubspindle ThirdSubSpindle(this IAngularTransmission at) => GetSubSpindle(at, 2);

        private static ISubspindle GetSubSpindle(IAngularTransmission at, int index)
        {
            int i = 0;

            foreach (var item in at.GetSubspindles())
            {
                if (index == i) return item;

                i++;
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
}
