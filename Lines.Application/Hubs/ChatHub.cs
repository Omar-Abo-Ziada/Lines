using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Lines.Application.Hubs
{
    [Authorize]   
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IMemoryCache _cache;

        private const string CONNECTION_USERS_KEY = "chat_connection_users"; // connectionId -> userId
        private const string USER_TRIP_GROUPS_KEY = "chat_user_trip_groups"; // userId -> tripId

        public ChatHub(ILogger<ChatHub> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task JoinTripChatGroup(string tripId)  
        {
            try
            {
                if (string.IsNullOrEmpty(tripId))
                {
                    _logger.LogWarning("Attempted to join trip chat group with null or empty tripId");
                    return;
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User not authenticated when trying to join trip chat group");
                    return;
                }

                var userTripGroups = GetUserTripGroups();

                // Remove user from any existing trip chat group
                if (userTripGroups.ContainsKey(userId))
                {
                    var currentTripId = userTripGroups[userId];
                    var oldGroupName = $"trip_{currentTripId}";
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, oldGroupName);
                    _logger.LogInformation("User {UserId} left previous trip chat group {GroupName}",
                        userId, oldGroupName);
                }

                // Join new trip chat group
                var groupName = $"trip_{tripId}";
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                // Track current trip group for user
                userTripGroups[userId] = tripId;
                SaveUserTripGroups(userTripGroups);

                _logger.LogInformation("User {UserId} joined trip chat group {GroupName}",
                    userId, groupName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining trip chat group {TripId} for connection {ConnectionId}",
                    tripId, Context.ConnectionId);
                throw;
            }
        }

        public async Task LeaveTripChatGroup(string tripId)
        {
            try
            {
                if (string.IsNullOrEmpty(tripId))
                {
                    _logger.LogWarning("Attempted to leave trip chat group with null or empty tripId");
                    return;
                }

                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User not authenticated when trying to leave trip chat group");
                    return;
                }

                var userTripGroups = GetUserTripGroups();

                var groupName = $"trip_{tripId}";
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

                // Remove from tracking if this is the current trip group
                if (userTripGroups.ContainsKey(userId) && userTripGroups[userId] == tripId)
                {
                    userTripGroups.Remove(userId);
                    SaveUserTripGroups(userTripGroups);
                }

                _logger.LogInformation("User {UserId} left trip chat group {GroupName}",
                    userId, groupName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error leaving trip chat group {TripId} for connection {ConnectionId}",
                    tripId, Context.ConnectionId);
                throw;
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = GetCurrentUserId();
                var connectionUsers = GetConnectionUsers();
                var userTripGroups = GetUserTripGroups();

                if (!string.IsNullOrEmpty(userId))
                {
                    // Check if user already has an active connection
                    var existingConnectionId = connectionUsers.FirstOrDefault(x => x.Value == userId).Key;

                    if (!string.IsNullOrEmpty(existingConnectionId))
                    {
                        // Remove old connection from any trip groups
                        if (userTripGroups.ContainsKey(userId))
                        {
                            var currentTripId = userTripGroups[userId];
                            var groupName = $"trip_{currentTripId}";
                            await Groups.RemoveFromGroupAsync(existingConnectionId, groupName);
                        }

                        // Clean up old connection tracking
                        connectionUsers.Remove(existingConnectionId);
                        SaveConnectionUsers(connectionUsers);

                        _logger.LogInformation("Removed old connection {OldConnectionId} for user {UserId} due to new connection {NewConnectionId}",
                            existingConnectionId, userId, Context.ConnectionId);
                    }

                    // Register new connection
                    connectionUsers[Context.ConnectionId] = userId;
                    SaveConnectionUsers(connectionUsers);

                    _logger.LogInformation("User {UserId} connected with connection {ConnectionId}",
                        userId, Context.ConnectionId);
                }
                else
                {
                    _logger.LogWarning("User connected without valid user ID. Connection: {ConnectionId}",
                        Context.ConnectionId);
                }

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync for connection {ConnectionId}",
                    Context.ConnectionId);
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var connectionUsers = GetConnectionUsers();
                var userTripGroups = GetUserTripGroups();

                var userId = connectionUsers.ContainsKey(Context.ConnectionId)
                    ? connectionUsers[Context.ConnectionId]
                    : "Unknown";

                if (connectionUsers.ContainsKey(Context.ConnectionId))
                {
                    var user = connectionUsers[Context.ConnectionId];
                    connectionUsers.Remove(Context.ConnectionId);
                    SaveConnectionUsers(connectionUsers);

                    // Remove from trip group tracking
                    userTripGroups.Remove(user);
                    SaveUserTripGroups(userTripGroups);
                }

                if (exception != null)
                {
                    _logger.LogError(exception, "User {UserId} disconnected with error. Connection: {ConnectionId}",
                        userId, Context.ConnectionId);
                }
                else
                {
                    _logger.LogInformation("User {UserId} disconnected normally. Connection: {ConnectionId}",
                        userId, Context.ConnectionId);
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnDisconnectedAsync for connection {ConnectionId}",
                    Context.ConnectionId);
            }
        }

        private string? GetCurrentUserId()
        {
            return Context.UserIdentifier ?? Context.User?.FindFirst("sub")?.Value;
        }

        // ===== Cache Helpers =====

        private Dictionary<string, string> GetConnectionUsers()
        {
            return _cache.GetOrCreate(CONNECTION_USERS_KEY, _ => new Dictionary<string, string>())!;
        }

        private void SaveConnectionUsers(Dictionary<string, string> data)
        {
            _cache.Set(CONNECTION_USERS_KEY, data, TimeSpan.FromHours(24));
        }

        private Dictionary<string, string> GetUserTripGroups()
        {
            return _cache.GetOrCreate(USER_TRIP_GROUPS_KEY, _ => new Dictionary<string, string>())!;
        }

        private void SaveUserTripGroups(Dictionary<string, string> data)
        {
            _cache.Set(USER_TRIP_GROUPS_KEY, data, TimeSpan.FromHours(24));
        }
    }
}
