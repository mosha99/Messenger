namespace Presentation.Client.Share;

public interface ISignalRProvider
{
    Task SendAsync<T>(string methodName, T item);
    IDisposable On<T1>(string methodName, Func<T1, Task> handler);
    void Build(string url);
    Task StartAsync();
}