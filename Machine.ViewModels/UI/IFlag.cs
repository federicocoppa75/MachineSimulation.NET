namespace Machine.ViewModels.UI
{
    public interface IFlag
    {
        string Name { get; set; }
        bool Value { get; set; }

        bool TryToParse(string value);
    }
}