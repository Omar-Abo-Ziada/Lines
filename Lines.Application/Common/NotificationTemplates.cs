using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Common
{
    public static class NotificationTemplates
    {
        public const string TripCreatedTitle = "طلب رحلة جديد";
        public const string TripCreatedBody = "تم إنشاء طلب رحلتك وجاري البحث عن سائق قريب.";

        public const string NoDriversTitle = "لا يوجد سائقون متاحون";
        public const string NoDriversBody = "لا يوجد سائقون بالقرب منك الآن، حاول مرة أخرى بعد قليل.";

        public const string NewTripForDriverTitle = "رحلة جديدة";
        public const string NewTripForDriverBody = "لديك طلب رحلة جديد بالقرب منك.";

        public const string OfferAcceptedTitle = "تم قبول العرض";
        public const string OfferAcceptedBodyDriver = "الراكب قبل عرضك وتم إنشاء الرحلة بنجاح.";
        public const string TripConfirmedBodyPassenger = "تم تأكيد رحلتك والسائق في الطريق إليك.";


        public const string TripAcceptedTitle = "تم قبول الرحلة";

        public const string TripAcceptedBodyForDriver = "تم قبول طلب الرحلة من الراكب. الرجاء التوجه إلى موقعه الآن.";

        public const string TripAcceptedBodyForPassenger =  "السائق قبل طلب رحلتك وهو في طريقه إليك.";


        public const string TripCancelledTitle = "تم إلغاء الرحلة";

        public const string TripCancelledBodyDriver ="تم إلغاء طلب الرحلة من قبل الراكب.";

        public const string TripCancelledBodyPassenger = "تم إلغاء رحلتك بنجاح.";


        public const string NewOfferTitle = "عرض جديد";
        public const string NewOfferBody = "لديك عرض جديد على طلب رحلتك.";

        public const string NewChatMessageTitle = "رسالة جديدة";
        public const string NewChatMessageBody = "لديك رسالة جديدة في المحادثة.";


    }

}
