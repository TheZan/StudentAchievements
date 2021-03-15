using System.Linq;
using System.Collections.Generic;

namespace StudentAchievements.Areas.Message.Infrastructure
{
    public class ConnectionProvider
    {
        private Dictionary<string, string> connections;

        public ConnectionProvider()
        {
            connections = new Dictionary<string, string>();
        }

        public string GetConnection(string userId)
        {
            if(connections.ContainsKey(userId))
            {
                return connections[userId];
            }

            return null;
        }

        public void AddConnection(string userId, string connectionId) => connections.Add(userId, connectionId);

        public void RemoveConnection(string userId) => connections.Remove(userId);        
    }
}