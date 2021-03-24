using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MDT = Machine.Data.Tools;

namespace Machine._3D.Views.Converters
{
    class ToolToGeometryConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is MDT.Tool tool)
            {
                if (tool is MDT.CountersinkTool cst) return Build(cst);
                else if (tool is MDT.DiskOnConeTool dct) return Build(dct);
                else if (tool is MDT.DiskTool dt) return Build(dt);
                else if (tool is MDT.PointedTool pt) return Build(pt);
                else if (tool is MDT.SimpleTool st) return Build(st);
                else if (tool is MDT.TwoSectionTool tst) return Build(tst);
                else throw new NotImplementedException();
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private MeshGeometry3D Build(MDT.CountersinkTool t)
        {
            const double hSvasatore = 10.0;
            var builder = new MeshBuilder();
            var p1 = new Vector3(0.0f, 0.0f, -(float)(t.Length1 - hSvasatore));
            var p12 = new Vector3(0.0f, 0.0f, -(float)t.Length1);
            var p2 = new Vector3(0.0f, 0.0f,  -(float)(t.Length1 + t.Length2));
            var p3 = new Vector3(0.0f, 0.0f, -(float)(t.Length1 + t.Length2 + t.Length3));

            builder.AddCylinder(new Vector3(),
                                p1,
                                t.Diameter1 / 2.0);
            builder.AddCylinder(p1,
                                p12,
                                t.Diameter2 / 2.0);
            builder.AddCone(p12,
                            new Vector3(0.0f, 0.0f, -1.0f),
                            t.Diameter2 / 2.0,
                            t.Diameter1 / 2.0,
                            t.Length2,
                            false,
                            false,
                            20);
            builder.AddCylinder(p2,
                                p3,
                                t.Diameter1 / 2.0);

            return builder.ToMesh();
        }

        private MeshGeometry3D Build(MDT.DiskTool t)
        {
            var builder = new MeshBuilder();
            var d = Math.Abs(t.BodyThickness - t.CuttingThickness) / 2.0;
            var r1 = t.Diameter / 2.0 - t.CuttingRadialThickness;
            var profile = new[]
            {
                new SharpDX.Vector2(0.0f, 10.0f),
                new SharpDX.Vector2(0.0f, (float)r1),
                new SharpDX.Vector2((float)(- d), (float)r1),
                new SharpDX.Vector2((float)(- d), (float)(t.Diameter / 2.0)),
                new SharpDX.Vector2((float)(t.BodyThickness + d), (float)(t.Diameter / 2.0)),
                new SharpDX.Vector2((float)(t.BodyThickness + d), (float)r1),
                new SharpDX.Vector2((float)t.BodyThickness, (float)r1),
                new SharpDX.Vector2((float)t.BodyThickness, 10.0f)
            };

            builder.AddRevolvedGeometry(profile.ToList(),
                                        null,
                                        new Vector3(),
                                        new Vector3(0.0f, 0.0f, -1.0f),
                                        100);

            return builder.ToMesh();
        }

        private MeshGeometry3D Build(MDT.DiskOnConeTool t)
        {
            var builder = new MeshBuilder();
            var d = Math.Abs(t.BodyThickness - t.CuttingThickness) / 2.0;
            var r1 = t.Diameter / 2.0 - t.CuttingRadialThickness;
            var p1 = new Vector3(0.0f, 0.0f, -(float)t.PostponemntLength);
            var profile = new SharpDX.Vector2[]
            {
                new SharpDX.Vector2(0.0f, (float)(t.PostponemntDiameter / 2.0)),
                new SharpDX.Vector2(0.0f, (float)r1),
                new SharpDX.Vector2((float)(- d), (float)r1),
                new SharpDX.Vector2((float)(- d), (float)(t.Diameter / 2.0)),
                new SharpDX.Vector2((float)(t.BodyThickness + d), (float)(t.Diameter / 2.0)),
                new SharpDX.Vector2((float)(t.BodyThickness + d), (float)r1),
                new SharpDX.Vector2((float)t.BodyThickness, (float)r1),
                new SharpDX.Vector2((float)t.BodyThickness, (float)(t.PostponemntDiameter / 2.0)),

            };

            builder.AddRevolvedGeometry(profile.ToList(),
                                        null,
                                        p1,
                                        new Vector3(0.0f, 0.0f, -1.0f),
                                        100);
            builder.AddCylinder(new Vector3(),
                                p1,
                                t.PostponemntDiameter,
                                20);

            return builder.ToMesh();
        }

        private MeshGeometry3D Build(MDT.PointedTool t)
        {
            var builder = new MeshBuilder();

            builder.AddCylinder(new Vector3(),
                                new Vector3(0.0f, 0.0f, -(float)t.StraightLength),
                                t.Diameter / 2.0);
            builder.AddCone(new Vector3(0.0f, 0.0f, -(float)t.StraightLength),
                            new Vector3(0.0f, 0.0f, -(float)(t.StraightLength + t.ConeHeight)),
                            t.Diameter / 2.0,
                            false,
                            20);

            return builder.ToMesh();
        }

        private MeshGeometry3D Build(MDT.SimpleTool t)
        {
            var builder = new MeshBuilder();

            builder.AddCylinder(new Vector3(),
                                new Vector3(0.0f, 0.0f, -(float)t.Length),
                                t.Diameter / 2.0);

            return builder.ToMesh();
        }

        private MeshGeometry3D Build(MDT.TwoSectionTool t)
        {
            var builder = new MeshBuilder();

            builder.AddCylinder(new Vector3(),
                                new Vector3(0.0f, 0.0f, -(float)t.Length1),
                                t.Diameter1 / 2.0);
            builder.AddCylinder(new Vector3(0.0f, 0.0f, -(float)t.Length1),
                                new Vector3(0.0f, 0.0f, -(float)(t.Length1 + t.Length2)),
                                t.Diameter2 / 2.0);

            return builder.ToMesh();
        }

    }
}
