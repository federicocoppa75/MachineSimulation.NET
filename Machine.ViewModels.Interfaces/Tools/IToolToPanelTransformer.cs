using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.Interfaces.Tools
{
    public struct ToolPosition
    {
        public Point Point { get; set; }
        public Vector Direction { get; set; }
        public double Radius { get; set; }
        public double Length { get; set; }
    }

    public interface IToolToPanelTransformer
    {
        IList<ToolPosition> Transform();
        Task<IList<ToolPosition>> TransformAsync();
        void TransformAndApplay();
        Task<int> TransformAndApplayAsync();
    }
}
