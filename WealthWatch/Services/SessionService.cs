using System;

namespace WealthWatch.Services
{
    public class SessionService
    {
        private Guid? _currentUserId;

        public void StartSession(Guid userId)
        {
            _currentUserId = userId;
            Console.WriteLine($"Session started for user {userId}");
        }

        public void EndSession()
        {
            Console.WriteLine($"Session ended for user {_currentUserId}");
            _currentUserId = null;
        }

        public Guid? GetCurrentUserId()
        {
            return _currentUserId;
        }

        public bool IsUserLoggedIn()
        {
            return _currentUserId.HasValue;
        }
    }
}
