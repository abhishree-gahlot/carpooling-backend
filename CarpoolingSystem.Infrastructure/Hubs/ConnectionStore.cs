using System.Collections.Concurrent;

namespace CarpoolingSystem.Infrastructure.Hubs;

public static class ConnectionStore
{
    private static readonly ConcurrentDictionary<Guid, string> _connections = new();
    public static void Add(Guid userId, string connectionId) => _connections[userId] = connectionId;
    public static void Remove(Guid userId) => _connections.TryRemove(userId, out _);
    public static string? GetConnectionId(Guid userId) => _connections.TryGetValue(userId, out var connId) ? connId : null;
    public static string GetAllConnections() =>
        _connections.Any()
            ? string.Join(", ", _connections.Select(x => $"{x.Key}={x.Value[..8]}"))
            : "none";
}