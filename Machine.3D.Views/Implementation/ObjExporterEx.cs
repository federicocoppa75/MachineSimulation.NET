using HelixToolkit.Wpf.SharpDX.Model.Scene;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWMM3D = System.Windows.Media.Media3D;


namespace Machine._3D.Views.Implementation
{
    internal class ObjExporterEx : ObjExporter, IExporter
    {
        public ObjExporterEx(string outputFileName) : this(outputFileName, null)
        {

        }

        public ObjExporterEx(string outputFileName, string comment) : base(outputFileName, comment)
        {

        }

        public new void Export(Viewport3DX viewport)
        {
            this.ExportHeader();
            this.ExportViewport(viewport);

            // Export objects
            foreach (var item in viewport.Items)
            {
                Traverse<MeshNode>(item, SWMM3D.Transform3D.Identity, ExportModel);
            }

            // Export camera
            this.ExportCamera(viewport.Camera);

            // Export lights
            foreach (var item in viewport.Items)
            {
                Traverse<LightNode>(item, SWMM3D.Transform3D.Identity, this.ExportLight);
            }
        }

        private static void Traverse<T>(Element3D element, SWMM3D.Transform3D transform, Action<T, SWMM3D.Transform3D> action) where T : SceneNode
        {
            var childTransform = CombineTransform(element.Transform, transform);

            if ((element.SceneNode is T t))
            {
                action(t, childTransform);
            }

            if (element is CompositeModel3D composite)
            {
                foreach (var item in composite.Children)
                {
                    Traverse<T>(item, childTransform, action);
                }
            }
        }

        private static SWMM3D.Transform3D CombineTransform(SWMM3D.Transform3D t1, SWMM3D.Transform3D t2)
        {
            if (t1 == null && t2 == null)
            {
                return SWMM3D.Transform3D.Identity;
            }

            if (t1 == null && t2 != null)
            {
                return t2;
            }

            if (t1 != null && t2 == null)
            {
                return t1;
            }

            return new SWMM3D.Transform3DGroup
            {
                Children = { t1, t2 }
            };
        }
    }
}
