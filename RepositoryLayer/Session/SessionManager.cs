using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Session
{
    using System.Collections.Concurrent;

    public class SessionManager
    {
        private readonly ConcurrentDictionary<string, string> _sessions = new ConcurrentDictionary<string, string>();

        public string CreateSession(string email)
        {
            var sessionId = Guid.NewGuid().ToString();
            _sessions.TryAdd(email, sessionId);
            return sessionId;
        }

        public bool RemoveSession(string email)
        {
            return _sessions.TryRemove(email, out _);
        }

        public bool IsSessionValid(string email, string sessionId)
        {
            return _sessions.TryGetValue(email, out var storedSessionId) && storedSessionId == sessionId;
        }
    }

}
