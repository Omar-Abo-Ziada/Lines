using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Lines.Application.DTOs.Notifications;
using Lines.Application.Interfaces;
 
using Lines.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Infrastructure.Services
{

    public class FcmNotifier : IFcmNotifier
    {
        private readonly ApplicationDBContext _db;

        public FcmNotifier(ApplicationDBContext db) { _db = db; }

        public async Task<NotificationResponse> SendToUserAsync(Guid userId, string title, string body, object data = null)
        {
            var tokens = await _db.FCMUserTokens
                .Where(t => t.UserId == userId && t.IsActive /*&& t.NotificationsEnabled*/)
                .Select(t => t.Token)
                .ToListAsync();

            return await SendToManyAsync(tokens, title, body, data);
        }

        public async Task<NotificationResponse> SendToManyAsync(IEnumerable<string> tokens, string title, string body, object data = null)
        {
            var tokenList = tokens.Distinct().Take(500).ToList();
            if (!tokenList.Any())
                return new NotificationResponse();

            var message = new MulticastMessage
            {
                Notification = string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(body)
                    ? null
                    : new Notification { Title = title, Body = body },
                Data = data?.ToDictionary() ?? new Dictionary<string, string>(),
                Tokens = tokenList
            };

            var resp = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);

            // نظّف فقط التوكنات الـ invalid/unregistered
            var invalidPairs = resp.Responses
                .Select((r, i) => new { r, token = tokenList[i] })
                .Where(x => !x.r.IsSuccess && IsInvalidTokenError(x.r.Exception))
                .ToList();

            if (invalidPairs.Any())
            {
                var badTokens = invalidPairs.Select(x => x.token).ToList();
                var rows = await _db.FCMUserTokens.Where(t => badTokens.Contains(t.Token)).ToListAsync();
                foreach (var r in rows)
                {
                    r.IsActive = false; // أو RemoveRange لو تحب تحذف
                    r.LastUpdatedOnUtc = DateTime.UtcNow;
                }
                await _db.SaveChangesAsync();
            }

            return new NotificationResponse
            {
                SuccessCount = resp.SuccessCount,
                FailureCount = resp.FailureCount,
                InvalidTokens = invalidPairs.Select(x => x.token).ToList()
            };
        }

        private static bool IsInvalidTokenError(FirebaseMessagingException? ex)
        {
            if (ex == null) return false;
            //return ex.ErrorCode == ErrorCode.NotFound || ex.ErrorCode == ErrorCode.InvalidArgument;
            return ex.MessagingErrorCode == MessagingErrorCode.Unregistered
                    || ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument;
        }
    }

    static class ObjExt
    {
        public static Dictionary<string, string> ToDictionary(this object anon)
            => anon?.GetType().GetProperties()
               .ToDictionary(p => p.Name, p => Convert.ToString(p.GetValue(anon)) ?? "")
               ?? new Dictionary<string, string>();
    }


    //public class FcmNotifier : INotifier
    //{
    //    private readonly ApplicationDBContext _db;
    //    public FcmNotifier(ApplicationDBContext db) { _db = db; }

    //    public async Task<NotificationResponse> SendToUserAsync(string userId, string title, string body, object data = null)
    //    {
    //        var tokens = await _db.FCMUserTokens
    //            .Where(t => t.UserId == userId)
    //            .Select(t => t.Token)
    //            .ToListAsync();

    //        return await SendToManyAsync(tokens, title, body, data);
    //    }

    //    public async Task<NotificationResponse> SendToManyAsync(IEnumerable<string> tokens, string title, string body, object data = null)
    //    {
    //        var tokenList = tokens.Distinct().Take(500).ToList();
    //        if (!tokenList.Any()) return new NotificationResponse();

    //        var message = new MulticastMessage
    //        {
    //            Notification = string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(body)
    //                ? null
    //                : new Notification { Title = title, Body = body },
    //            Data = data?.ToDictionary() ?? new Dictionary<string, string>(),
    //            Tokens = tokenList
    //        };

    //        var resp = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
    //        var invalid = resp.Responses
    //            .Select((r, i) => new { r, token = tokenList[i] })
    //            .Where(x => !x.r.IsSuccess)
    //            .Select(x => x.token)
    //            .ToList();

    //        // نظّف التوكنات الباطلة
    //        if (invalid.Any())
    //        {
    //            var bad = await _db.FCMUserTokens.Where(t => invalid.Contains(t.Token)).ToListAsync();
    //            _db.FCMUserTokens.RemoveRange(bad);
    //            await _db.SaveChangesAsync();
    //        }

    //        return new NotificationResponse { SuccessCount = resp.SuccessCount, FailureCount = resp.FailureCount, InvalidTokens = invalid };
    //    }
    //}

    //static class ObjExt
    //{
    //    public static Dictionary<string, string> ToDictionary(this object anon)
    //        => anon?.GetType().GetProperties().ToDictionary(p => p.Name, p => Convert.ToString(p.GetValue(anon)) ?? "")
    //           ?? new Dictionary<string, string>();
    //}

}
