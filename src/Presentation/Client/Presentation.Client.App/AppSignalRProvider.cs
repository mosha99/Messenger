using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Presentation.Client.Share;

namespace Presentation.Client.App;

public class AppSignalRProvider(NavigationManager Navigation) : ISignalRProvider, IAsyncDisposable
{
    private HubConnection? hubConnection;

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null) await hubConnection.DisposeAsync();
    }

    public Task SendAsync<T>(string methodName, T item)
    {
        return hubConnection!.SendAsync(methodName, item);
    }

    public IDisposable On<T1>(string methodName, Func<T1, Task> handler)
    {
        return hubConnection!.On(methodName, handler);
    }

    public void Build(string url)
    {

        hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();
    }

    public Task StartAsync()
    {
        return hubConnection!.StartAsync();
    }
}