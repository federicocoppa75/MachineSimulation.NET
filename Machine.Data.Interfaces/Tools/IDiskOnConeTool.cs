namespace Machine.Data.Interfaces.Tools
{
    public interface IDiskOnConeTool : IDiskTool
    {
        double PostponemntDiameter { get; set; }
        double PostponemntLength { get; set; }
    }
}