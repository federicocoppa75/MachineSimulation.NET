using Machine.Data.Interfaces.Tools;

namespace Machine.Data.Interfaces.Factories
{
    public interface IToolFactory
    {
        T Create<T>() where T : ITool;  
    }
}
