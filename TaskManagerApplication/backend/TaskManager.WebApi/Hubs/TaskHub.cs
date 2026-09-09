using Microsoft.AspNetCore.SignalR;

namespace TaskManager.WebApi.Hubs;

// Clients only listen for broadcasts; no server-invokable methods needed yet.
public class TaskHub : Hub
{
}
