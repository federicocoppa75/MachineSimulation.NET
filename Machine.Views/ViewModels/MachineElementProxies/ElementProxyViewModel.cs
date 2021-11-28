using Machine.ViewModels.Interfaces.MachineElements;
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
    }

    public class ElementProxyViewModel : INotifyPropertyChanged, IDisposable
    {
        private IMachineElement _element;

        [Category("General")]
        [PropertyOrder(1)]
        public string Name 
        {
            get => _element.Name;
            set => _element.Name = value; 
        }

        [Category("General")]
        [PropertyOrder(2)]
        public string ModelFile 
        {
            get => _element.ModelFile; 
            set => _element.ModelFile = value; 
        }

        [Category("General")]
        [PropertyOrder(3)]
        public SWM.Color Color 
        { 
            get => Convert(_element.Color); 
            set => _element.Color = Convert(value); 
        }

        [Category("Positioning")]
        [PropertyOrder(1)]
        [ExpandableObject]
        public Vector Position 
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

            if(_element is INotifyPropertyChanged npc) npc.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        #region implementation
        private static SWM.Color Convert(MDB.Color color) => SWM.Color.FromArgb(color.A, color.R, color.G, color.B);

        private static MDB.Color Convert(SWM.Color color) => new MDB.Color() { A = color.A, R = color.R, G = color.G, B = color.B };

        private static Vector ToPosition(MDB.Matrix matrix) => new Vector() { X = matrix.OffsetX, Y = matrix.OffsetY, Z = matrix.OffsetZ };
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
