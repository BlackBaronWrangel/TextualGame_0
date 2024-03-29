namespace GlobalServices.Interfaces
{
    public interface ILogger
    {
        string Logs { get;}
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
