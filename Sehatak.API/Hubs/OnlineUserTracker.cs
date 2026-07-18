using System.Collections.Concurrent;

namespace Sehatak.API.Hubs
{
    public class OnlineUserTracker
    {
        private readonly ConcurrentDictionary<string, int> _connections = new();
        public bool UserConnected(string userId)
        {
            var count = _connections.AddOrUpdate(userId, 1, (_, old) => old + 1);
            return count == 1;
        }

        public bool UserDisconnected(string userId)
        {
            if(_connections.TryGetValue(userId, out var count))
                if(count<=1)
                {
                    _connections.TryRemove(userId, out _);
                    return true;
                }
            _connections[userId] = count - 1;
            return false;

        }
        public List<string> GetOnlineUsers() => _connections.Keys.ToList();
    }
}
