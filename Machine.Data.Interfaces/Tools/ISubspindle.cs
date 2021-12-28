

namespace Machine.Data.Interfaces.Tools
{
    public interface ISubspindle
    {
        string ToolName { get; set; }

        void GetPosition(out double x, out double y, out double z);
        void SetPosition(double x, double y, double z);

        void GetDirection(out double x, out double y, out double z);
        void SetDirection(double x, double y, double z);
    }
}