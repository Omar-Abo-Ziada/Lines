using Lines.Domain.Enums.Notifications;
using Lines.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Domain.Models.Notifications
{
    // ✅ يعرّف نوع الجهاز/المنصّة علشان تقدر تحلل الأعطال والإرسال بناءً على المنصّة
 

    // ✅ الكيان الأساسي لتخزين توكنات FCM المرتبطة بالمستخدمين
    public class FCMUserToken:BaseModel
    {
        public int Id { get; set; }

        // ✅ معرّف المستخدم (IdentityUser أو ApplicationUser) — استخدم نفس النوع عندك (string/Guid)
        public Guid UserId { get; set; } = default!;
        //public string UserId { get; set; } = default!;

        // ✅ توكن FCM الفعلي (Registration Token) — نضع عليه فهرس Unique
        public string Token { get; set; } = default!;

        // ✅ بصمة أو معرف للجهاز لتمييز الأجهزة المتعددة لنفس المستخدم (اختياري لكنه مفيد)
        public string? DeviceId { get; set; }

        // ✅ نوع المنصّة (Android/iOS/Web)
        public DevicePlatform Platform { get; set; } = DevicePlatform.Unknown;

        //// ✅ إصدار التطبيق عند تسجيل/تحديث التوكن — يساعدك في دعم الإصدارات
        //public string? AppVersion { get; set; }

        // ✅ لغة واجهة المستخدم وقت التسجيل (قد تفيدك في تخصيص الإشعارات)
        public string? Locale { get; set; }

        //// ✅ هل الإشعارات مفعّلة على الجهاز (من إعدادات المستخدم داخل التطبيق)
        //public bool NotificationsEnabled { get; set; } = true;

        // ✅ معلومات تشغيلية
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;  // أوّل مرّة اتسجل فيها التوكن
        public DateTime? LastUpdatedOnUtc { get; set; }                // آخر تحديث للتوكن/بياناته
        public DateTime? LastSeenUtc { get; set; }                     // آخر استخدام (مثلاً عند استقبال إشعار بنجاح)

        //// ✅ آخر IP لو حابب تسجّله وقت التسجيل (اختياري)
        //public string? LastIpAddress { get; set; }

        //// ✅ فلَاج للتعطيل بدل الحذف (عشان ماتخسرش التاريخ)
        public bool IsActive { get; set; } = true;

        // 🔗 علاقة اختيارية مع الـ ApplicationUser (لو عندك Navigation)
        // public virtual ApplicationUser User { get; set; } = default!;
    }

}
