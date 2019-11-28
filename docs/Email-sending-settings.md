# Email sending settings documentation

### Introduction
This article describes how to tune email sending  in the Platform v.3

### How to customize email sending via SMTP
1. If use **_gmail.com_** then need to do:
    * Turn on `Allow less secure apps` here: https://www.google.com/settings/security/lesssecureapps
    * Set Gateway is `Smtp` in **_Notifications_** options in **_Appsettings.json_** or **_Azure_**
    * Then customize **_Smtp options_**:
        1. Fill **_SmtpServer_** `http://smtp.gmail.com` and **_Port_** `587`
        2. Turn `ON` SSL: **_EnableSsl_** set **_true_**
        3. Set **_Login_** and **_Password_**
        like this:
            ```json
            "Smtp": {
                "SmtpServer": "http://smtp.gmail.com",
                "Port": 587,
                "EnableSsl": true,
                "Login": "test@test.com",
                "Password": "qwerty"
            }
            ```
2. If use another SMTP server like **_sendgrid.net_** then need to do:
    * Set Gateway is `Smtp` in **_Notifications_** options in **_Appsettings.json_** or **_Azure_**
    * Then customize **_Smtp options_**:
        1. Fill SmtpServer `http://smtp.gmail.com` and Port `587`
        2. Turn `OFF` SSL: **_EnableSsl_** set **_false_** 
        > NOTE: should read prerequiments in the SMTP server site
        3. Set **_Login_** and **_Password_**
        like this:
            ```json
            "Smtp": {
                "SmtpServer": "http://smtp.sendgrid.net",
                "Port": 25,
                "EnableSsl": false,
                "Login": "test@sendgrid.net",
                "Password": "qwerty"
            }
            ```

### How to customize email sending via SendGrid's API
1. Set Gateway is `SendGrid` **_Notifications_** options in **_Appsettings.json_** or **_Azure_**
2. Then fill apikey in **_SendGrid options_**
 looks like this:
    ```json
        "SendGrid": {
            "ApiKey": "testapikey"
        }
    ```

> NOTE: Also you could check the emailing-services in IntegrationTests [here](https://github.com/VirtoCommerce/vc-module-notification/blob/release/3.0.0/tests/VirtoCommerce.NotificationsModule.Tests/IntegrationTests/NotificationSenderIntegrationTests.cs)
