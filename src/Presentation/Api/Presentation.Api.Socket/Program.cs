using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Share.Model;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});
var corsAllowAll = "CorsAllowAll";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsAllowAll,
        builder =>
        {
            builder
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseResponseCompression();

app.MapHub<ChatHub>("/chathub");

app.UseCors(corsAllowAll);

app.Run();


public class ChatHub : Hub
{
    [HubMethodName(Configuration.SenderMethodName)]
    public async Task SendMessage(UserMessage message)
    {
        await Clients.AllExcept(Context.ConnectionId).SendAsync(Configuration.ReceiveMethodName, message);
    }
}
