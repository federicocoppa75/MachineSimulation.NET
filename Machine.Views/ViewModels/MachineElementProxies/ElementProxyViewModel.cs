using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.UI.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using MDB = Machine.Data.Base;
using SWM = System.Windows.Media;

namespace Machine.Views.ViewModels.MachineElementProxies
{
    //public class ElementProxyViewModel2
    //{
    //    public string Name { get; set; } = "pippo";
    //    public string ModelFile { get; set; } = "pluto";
    //    public SWM.Color Color { get; set; } = SWM.Colors.AliceBlue;
    //}

    public struct Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override string ToString() => $"{X}; {Y}; {Z}";
    }

    [CategoryOrder("General", 0)]
    [CategoryOrder("Positioning", 1)]
    public class ElementProxyViewModel : INotifyPropertyChanged, IDisposable
    {
        protected IMachineElement _element;

        [Category("General")]
        [PropertyOrder(0)]
        public string Type { get; private set; }

        [Category("General")]
        [PropertyOrder(1)]
        public string Name 
        {
            get => _element.Name;
            set => _element.Name = value; 
        }

        [Category("General")]
        [PropertyOrder(2)]
        [Editor(typeof(PropertyGridFilePicker), typeof(PropertyGridFilePicker))]
        public string ModelFile 
        {
            get => _element.ModelFile; 
            set => _element.ModelFile = value; 
        }

        [Category("General")]
        [PropertyOrder(3)]
        public SWM.Color Color 
        { 
            get => _element.Color.Convert(); 
            set => _element.Color = value.Convert(); 
        }

        [Category("Positioning")]
        [PropertyOrder(1)]
        [ExpandableObject]
        public Vector Placement 
        { 
            get => ToPosition(_element.Transformation); 
            set => _element.Transformation = UpdatePosition(_element.Transformation, value); 
        }

        [Category("Positioning")]
        [PropertyOrder(2)]
        [ExpandableObject]
        public Vector Rotation 
        { 
            get => ToRotation(_element.Transformation); 
            set => _element.Transformation = UpdatetRotation(_element.Transformation, value); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ElementProxyViewModel(IMachineElement element)
        {
            _element = element;
            Type = GetTypeDescription(element);

            if(_element is INotifyPropertyChanged npc) npc.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        #region implementation
        private static Vector ToPosition(MDB.Matrix matrix) => (matrix != null) ? new Vector() { X = matrix.OffsetX, Y = matrix.OffsetY, Z = matrix.OffsetZ } : new Vector();
        private static MDB.Matrix UpdatePosition(MDB.Matrix matrix, Vector position)
        {
            var m = CreateCopy(matrix);

            m.OffsetX = position.X;
            m.OffsetY = position.Y;
            m.OffsetZ = position.Z;

            return m;
        }

        private static MDB.Matrix CreateCopy(MDB.Matrix matrix)
        {
            return new MDB.Matrix()
            {
                M11 = matrix.M11,
                M12 = matrix.M12,
                M13 = matrix.M13,
                M21 = matrix.M21,
                M22 = matrix.M22,
                M23 = matrix.M23,
                M31 = matrix.M31,
                M32 = matrix.M32,
                M33 = matrix.M33,
                OffsetX = matrix.OffsetX,
                OffsetY = matrix.OffsetY,
                OffsetZ = matrix.OffsetZ
            };
        }

        /// <summary>
        /// Utilizzo la notazione degli angoli aereonautici
        /// Yaw - imbardata - rotazione attorno all'asse Z
        /// Pitch - rollio - rotazione attorno all'asse Y
        /// Roll - beccheggio - rotazione attorno all'asse X
        /// vedi https://en.wikipedia.org/wiki/Rotation_matrix#Decompositions
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static Vector ToRotation(MDB.Matrix matrix)
        {
            if (matrix == null) return new Vector();

            var beta = Math.Asin(-matrix.M31);

            if (Compare(beta, Math.PI / 2.0, 0.0001) || Compare(beta, -Math.PI / 2.0, 0.0001))
            {
                // alfa = 0
                var alfa = Math.Atan2(matrix.M12, matrix.M13);

                return new Vector() { X = 0.0, Y = -beta * 180.0 / Math.PI, Z = -alfa * 180.0 / Math.PI };
            }
            else
            {
                var alfa = Math.Atan2(matrix.M21, matrix.M11);
                var gamma = Math.Atan2(matrix.M32, matrix.M33);

                return new Vector() { X = -gamma * 180.0 / Math.PI, Y = -beta * 180.0 / Math.PI, Z = -alfa * 180.0 / Math.PI };
            }
        }

        private static bool Compare(double v1, double v2, double tolerance)
        {
            var v = v1 - v2;

            return (v < tolerance) && (v > -tolerance);
        }

        private static MDB.Matrix UpdatetRotation(MDB.Matrix matrix, Vector rotation)
        {
            var m = CreateCopy(matrix);
            var cosA = Math.Cos(-rotation.Z * Math.PI / 180.0);
            var sinA = Math.Sin(-rotation.Z * Math.PI / 180.0);
            var cosB = Math.Cos(-rotation.Y * Math.PI / 180.0);
            var sinB = Math.Sin(-rotation.Y * Math.PI / 180.0);
            var cosG = Math.Cos(-rotation.X * Math.PI / 180.0);
            var sinG = Math.Sin(-rotation.X * Math.PI / 180.0);

            m.M11 = cosA * cosB;
            m.M12 = cosA * sinB * sinG - sinA * cosG;
            m.M13 = cosA * sinB * cosG + sinA * sinG;
            m.M21 = sinA * cosB;
            m.M22 = sinA * sinB * sinG + cosA * cosG;
            m.M23 = sinA * sinB * cosG - cosA * sinG;
            m.M31 = -sinB;
            m.M32 = cosB * sinG;
            m.M33 = cosB * cosG;

            return m;
        }

        private static string GetTypeDescription(IMachineElement element)
        {
            var type = element.GetType();
            var a = GetAttribute<MachineStructAttribute>(type);

            return a.Name;
        }

        private static T GetAttribute<T>(Type t) where T : MachineStructAttribute
        {
            return t.GetCustomAttributes(typeof(T), false)
                     .Select(o => o as T)
                     .FirstOrDefault();
        }

        #endregion

        #region IDisposable
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if (disposing)
                {
                    if (_element is INotifyPropertyChanged npc) npc.PropertyChanged -= OnPropertyChanged;
                    _element = null;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
