namespace Lines.Application.Common.Email
{
    public sealed record MailFormat(string Html)
    {
        public static readonly MailFormat Welcome = new(@"
        <html>
            <body>
                <h2>Welcome to Lines!</h2>
                <p>Hi {0},</p>
                <p>Thank you for joining <strong>Lines</strong>. We're excited to have you on board.</p>
                <p>Start exploring our collection and enjoy reading!</p>
                <br />
                <p>Best regards,<br/>Lines Team</p>
            </body>
        </html>
    ");

        public static readonly MailFormat Otp = new(@"
<html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f9f9f9;
                margin: 0;
                padding: 20px;
                color: #333;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                padding: 30px;
                border-radius: 8px;
                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            }}
            h2 {{
                color: #2e6c80;
                margin-bottom: 20px;
            }}
            .otp-code {{
                font-size: 24px;
                font-weight: bold;
                color: #2e6c80;
                background-color: #f0f8ff;
                padding: 10px 20px;
                border-radius: 6px;
                display: inline-block;
                margin: 15px 0;
            }}
            p {{
                font-size: 16px;
                line-height: 1.6;
            }}
            .footer {{
                margin-top: 30px;
                font-size: 14px;
                color: #888;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h2>Your One-Time Password (OTP)</h2>
            <p>Hello {0},</p>
            <p>Please use the OTP below to complete your action:</p>
            <div class='otp-code'>{1}</div>
            <p>This code is valid for <strong>10 minutes</strong>.</p>
            <p>If you did not request this OTP, please ignore this email or contact support.</p>
            <div class='footer'>
                <p>Thanks,<br/>The Lines Security Team</p>
            </div>
        </div>
    </body>
</html>
");

        public static readonly MailFormat DriverEmailVerification = new(@"
<html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f9f9f9;
                margin: 0;
                padding: 20px;
                color: #333;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                padding: 30px;
                border-radius: 8px;
                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            }}
            h2 {{
                color: #2e6c80;
                margin-bottom: 20px;
            }}
            .otp-code {{
                font-size: 24px;
                font-weight: bold;
                color: #2e6c80;
                background-color: #f0f8ff;
                padding: 10px 20px;
                border-radius: 6px;
                display: inline-block;
                margin: 15px 0;
            }}
            p {{
                font-size: 16px;
                line-height: 1.6;
            }}
            .footer {{
                margin-top: 30px;
                font-size: 14px;
                color: #888;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h2>Verify Your Email</h2>
            <p>Hello {0},</p>
            <p>Thank you for registering as a driver with Lines. Please verify your email using the code below:</p>
            <div class='otp-code'>{1}</div>
            <p>This code is valid for <strong>10 minutes</strong>.</p>
            <p>After verification, you can continue with your driver registration.</p>
            <div class='footer'>
                <p>Thanks,<br/>The Lines Driver Registration Team</p>
            </div>
        </div>
    </body>
</html>
");


    }
}
