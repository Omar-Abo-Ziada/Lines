using Lines.Application.DTOs.Notifications;
using Lines.Application.Interfaces.Notifications;
using Lines.Domain.Models.Notifications;
using Lines.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Infrastructure.Services.Notifications
{
    public class FCMUserTokenService: IFCMUserTokenService
    {
        private readonly ApplicationDBContext _db;

        public FCMUserTokenService(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task RegisterOrUpdateTokenAsync(DeviceTokenRequest req)
        {

            var token = await _db.FCMUserTokens.FirstOrDefaultAsync(t => t.Token == req.Token);

            if (token is null)
            {
                token = new FCMUserToken
                {
                    UserId = req.UserId,
                    Token = req.Token,
                    DeviceId = req.DeviceId,
                    Platform = req.Platform,
                    Locale = req.Locale,
                    //NotificationsEnabled = req.NotificationsEnabled ?? true,
                    CreatedOnUtc = DateTime.UtcNow,
                    //IsActive = true
                };
                _db.FCMUserTokens.Add(token);
            }
            else
            {
                token.UserId = req.UserId; // في حال نقل التوكن لحساب آخر بعد تسجيل خروج/دخول
                token.DeviceId = req.DeviceId ?? token.DeviceId;
                token.Platform = req.Platform;
                token.Locale = req.Locale ?? token.Locale;
                //token.NotificationsEnabled = req.NotificationsEnabled ?? token.NotificationsEnabled;
                //token.IsActive = true;
                token.LastUpdatedOnUtc = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
        }

    }
}
