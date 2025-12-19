using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Lines.Presentation.Firebase
{


    public static class FirebaseSetupExtensions
    {
        public static IServiceCollection AddFirebaseAdmin(this IServiceCollection services, IConfiguration config)
        {
            // 1) اقرأ من appsettings
            var pathFromConfig = config["Firebase:CredentialsPath"];

            // 2) أو من ENV VAR
            var pathFromEnv = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_PATH");

            var credPath = !string.IsNullOrWhiteSpace(pathFromConfig) ? pathFromConfig : pathFromEnv;

            if (string.IsNullOrWhiteSpace(credPath) || !File.Exists(credPath))
            {
                Console.WriteLine("⚠️ Firebase credentials not found. Set Firebase:CredentialsPath or FIREBASE_CREDENTIALS_PATH.");
                return services; // نكمل بدون تهيئة (عشان بيئة CI مثلاً)
            }

            // امنع إنشاء أكتر من App
            if (FirebaseApp.DefaultInstance is null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(credPath)
                });
                Console.WriteLine("✅ Firebase Admin initialized.");
            }

            // لو حابب تستخدمه عبر DI:
            services.AddSingleton(FirebaseApp.DefaultInstance);

            return services;
        }
    }
}

