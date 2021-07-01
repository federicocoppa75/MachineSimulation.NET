using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using MDB = Machine.Data.Base;

namespace Machine._3D.Views.Helpers
{
    class InserterToSinkTransformer : IInserterToSinkTransformer
    {
        IInsertionsSink _sink;
        IInjectorElement _injector;

        public InserterToSinkTransformer(IInsertionsSink sink, IInjectorElement injector)
        {
            _sink = sink;
            _injector = injector;
        }

        public InsertPosition Transform()
        {
            var matrix1 = _sink.GetChainTransformation();

            matrix1.Invert();

            GetInsertPositionAndDirection(matrix1, out Point3D p, out Vector3D d);

            return new InsertPosition()
            {
                Position = new MDB.Point() { X = p.X, Y = p.Y, Z = p.Z },
                Direction = new MDB.Vector() { X = d.X, Y = d.Y, Z = d.Z },
            };
        }

        private void GetInsertPositionAndDirection(Matrix3D panelMatrix, out Point3D position, out Vector3D direction)
        {
            var matrix = _injector.GetChainTransformation();
            var p0 = new Point3D(_injector.Position.X, _injector.Position.Y, _injector.Position.Z);
            var d0 = new Vector3D(_injector.Direction.X, _injector.Direction.Y, _injector.Direction.Z);
            var p1 = matrix.Transform(p0);
            var d1 = matrix.Transform(d0);
            var p2 = panelMatrix.Transform(p1);
            var d2 = panelMatrix.Transform(d1);

            position = p2;
            direction = d2;
        }
    }
}
