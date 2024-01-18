# Email notifications
Virto Commerce allows to send email notifications. To use this functionality the notification settings should be configured propertly. 

## How to configure email notifications

### Smtp
If use **_gmail.com_** then need to do:

1. Turn on `Allow less secure apps` here: https://www.google.com/settings/security/lesssecureapps
1. Set Gateway is `Smtp` in **_Notifications_** options in **_Appsettings.json_** or **_Notifications:Gateway_** in **_Azure_**
1. Then customize **_Smtp options_**:
    1. Fill **_SmtpServer_** `smtp.gmail.com` and **_Port_** `587`
    2. Set **_Login_** and **_Password_**
    * like this in **_appsttings.json_**:
        ```json
        "Gateway": "Smtp",
        "DefaultSender": "noreply@gmail.com",
        "Smtp": {
          "SmtpServer": "smtp.gmail.com",
          "Port": 587,
          "Login": "noreply@gmail.com",
          "Password": "**** **** **** ****",
          "ForceSslTls": false
        }
        ```
    * or in **_Azure_**:    
        * **_Notifications:Smtp:SmtpServer_** set `smtp.gmail.com`
        * **_Notifications:Smtp:Port_** set `587`
        * **_Notifications:Smtp:EnableSsl_** set `true`
        * **_Notifications:Smtp:Login_** set `noreply@gmail.com`
        * **_Notifications:Smtp:Password_** set `**** **** **** ****`

### SendGrid's API
1. Set Gateway is `SendGrid` **_Notifications_** options in **_Appsettings.json_** or **_Azure_**
2. Then fill apikey in **_SendGrid options_**
    * looks like this in **_appsetting.json_**:
        ```json
            "Gateway": "Smtp",
            "DefaultSender": "noreply@gmail.com",
            "SendGrid": {
                "ApiKey": "testapikey"
            }
        ```
    * or in **_Azure_**:    
            * **_Notifications:SendGrid:ApiKey_** set `testapikey`    

!!! note
    Also you could check the emailing-services in IntegrationTests [here](https://github.com/VirtoCommerce/vc-module-notification/blob/master/tests/VirtoCommerce.NotificationsModule.Tests/IntegrationTests/NotificationSenderIntegrationTests.cs)
