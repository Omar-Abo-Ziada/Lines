using Lines.Application.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Interfaces.Notifications
{
    public interface IFCMUserTokenService
    {
        Task RegisterOrUpdateTokenAsync(DeviceTokenRequest req);

    }
}
