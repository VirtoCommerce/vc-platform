INSERT INTO [EmailTemplate] ([EmailTemplateId],[Name],[Type],[Body],[DefaultLanguageCode],[Subject],[LastModified],[Created],[Discriminator]) VALUES (N'29556bd5-dc56-4485-af80-e80267e004ee',N'order-notify',N'Xsl',N'<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head id="Head1">
        <style type="text/css">
          #PurchaseOrder {}
          h1 {font-size: 20px;}
          h2 {font-size: 18px;}
          h3 {font-size: 16px; background-color: #cccccc; padding: 2px 2px 2px 2px}
          .introduction {padding: 5px 0 0 0}
          .footer {padding: 5px 0 0 0}
        </style>
        <title>
          Order Notification
        </title>
      </head>
      <body>
        <xsl:apply-templates select="//Order"></xsl:apply-templates>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="Order">
    <div id="PurchaseOrder">
      <h1>Sale/Order Notification from the Store</h1>
      <h1>**ORDER SUMMARY</h1>
      <xsl:call-template name="OrderHeader"></xsl:call-template>
      <div class="OrderForms">
        <h2>Products Purchased:</h2>
        <xsl:apply-templates select="OrderForms/OrderForm"></xsl:apply-templates>
      </div>
      <xsl:call-template name="OrderFooter"></xsl:call-template>
      <div class="Footer">
        Regards,<br/> your Company.
      </div>
    </div>
  </xsl:template>
  <xsl:template name="PaymentPlanSchedule">
    <div class="schedule">
      Payment Schedule: starting&#160;<xsl:value-of select="ms:format-date(StartDate, ''MMM dd, yyyy'')"/>&#160;every&#160;<xsl:value-of select="CycleLength"/>&#160;<xsl:call-template name="PlanCycle" />&#160;for&#160;<xsl:value-of select="MaxCyclesCount"/>&#160;<xsl:call-template name="PlanCycle" />&#160;till&#160;<xsl:value-of select="ms:format-date(EndDate, ''MMM dd, yyyy'')"/>
    </div>
  </xsl:template>
  <xsl:template name="PlanCycle">
    <xsl:value-of select="CycleMode"/>(s)
  </xsl:template>
  <xsl:template name="OrderHeader">
    Order Number: <xsl:value-of select="TrackingNumber"/><br/>
    Status: <xsl:value-of select="Status"/><br/>
    Name: <xsl:value-of select="CustomerName"/><br/>
    Email: <a>
      <xsl:attribute name="href">
        mailto:<xsl:value-of select="//OrderAddresses/OrderAddress[OrderAddressId=//AddressId]/Email"/>
      </xsl:attribute>
      <xsl:value-of select="//OrderAddresses/OrderAddress[OrderAddressId=//AddressId]/Email"/>
    </a>
  </xsl:template>
  <xsl:template name="OrderFooter">
    <h3>Order Summary</h3>
    <div class="OrderSummary">
      Sub Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(Subtotal, ''###,##0.00'')"/><br/>
      Handling Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(HandlingTotal, ''###,##0.00'')"/><br/>
      Shipping Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(ShippingTotal, ''###,##0.00'')"/><br/>
      Total Tax: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(TaxTotal, ''###,##0.00'')"/><br/>
      TOTAL: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(Total, ''###,##0.00'')"/><br/>
    </div>
  </xsl:template>
  <xsl:template match="OrderForm">
    <div class="OrderForm">
      <div class="OrderForms">
        <h3>Line Items</h3>
        <xsl:apply-templates select="LineItems/LineItem"></xsl:apply-templates>
        <h3>Payments</h3>
        <xsl:apply-templates select="Payments/Payment"></xsl:apply-templates>
      </div>
      <div class="OrderSummary">
      </div>
    </div>
  </xsl:template>
  <xsl:template match="LineItem">
    <div class="LineItem">
      <xsl:value-of select="format-number(Quantity, ''###,###.##'')"/>&#160;<xsl:value-of select="DisplayName"/> - <xsl:value-of select="//BillingCurrency"/>&#160;<xsl:value-of select="format-number(ListPrice, ''###,###.00'')"/> each
    </div>
  </xsl:template>
  <xsl:template match="Payment">
    <div class="Payment">
      Payment Method: <xsl:value-of select="PaymentMethodName"/><br/>
      Amount: <xsl:value-of select="//BillingCurrency"/>&#160;<xsl:value-of select="format-number(Amount, ''###,##0.00'')"/>
    </div>
  </xsl:template>
</xsl:stylesheet>',N'en-US',N'Order notification email template',N'20130514 12:17:28.037',N'20130423 09:49:14.567',N'EmailTemplate');
INSERT INTO [EmailTemplate] ([EmailTemplateId],[Name],[Type],[Body],[DefaultLanguageCode],[Subject],[LastModified],[Created],[Discriminator]) VALUES (N'4789540f-b6c9-4d30-a4db-45c848b2cac0',N'confirm-account',N'Xsl',N'<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head id="Head1">
        <style type="text/css">
          #SendEmailTemplate{
          color: #444444;
          font-family: ''Lucida Grande'',Verdana,Arial,sans-serif;
          font-size: 12px;
          line-height: 18px;
          }
          h1 {font-size: 20px;}
          h2 {font-size: 18px;}
          h3 {font-size: 16px; padding: 2px 2px 2px 2px}
          .footer {padding: 10px 0 0 0}
        </style>
        <title>
          Confirm account
        </title>
      </head>
      <body>
        <xsl:call-template name="SendEmailTemplate"></xsl:call-template>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="SendEmailTemplate">
    <div id="SendEmailTemplate">
      <b>
        <xsl:value-of select="//SendEmailTemplate/Username" />,
      </b>
      <br/>
      <br/>
      <div class="body">
        To confirm your account, click on the following link:
        <br/>
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="//SendEmailTemplate/Url"/>
          </xsl:attribute>
          <xsl:value-of select="//SendEmailTemplate/Url" />
        </a>
      </div>
      <br/>
      <div class="Footer">
        Best Regards,
        <br/><xsl:value-of select="//SendEmailTemplate/AuthorName" />.
      </div>
      <div style="color:#aaaaaa;margin:10px 0 14px 0;padding-top:10px;border-top:1px solid #eeeeee">
        This email is a service from Your Company.
      </div>
    </div>
  </xsl:template>
</xsl:stylesheet>',N'en-US',N'Account confirmation',N'20140618 13:52:13.917',N'20140618 13:23:05.433',N'EmailTemplate');
INSERT INTO [EmailTemplate] ([EmailTemplateId],[Name],[Type],[Body],[DefaultLanguageCode],[Subject],[LastModified],[Created],[Discriminator]) VALUES (N'5c496513-c7a4-47c3-817f-f8ca5c0cf5dd',N'forgot-password',N'Xsl',N'<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head id="Head1">
        <style type="text/css">
          #SendEmailTemplate {
          color: #444444;
          font-family: ''Lucida Grande'',Verdana,Arial,sans-serif;
          font-size: 12px;
          line-height: 18px;
          }
          h1 {font-size: 20px;}
          h2 {font-size: 18px;}
          h3 {font-size: 16px; padding: 2px 2px 2px 2px}
          .footer {padding: 10px 0 0 0}
        </style>
        <title>
          Reset password
        </title>
      </head>
      <body>
        <xsl:call-template name="SendEmailTemplate"></xsl:call-template>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="SendEmailTemplate">
    <div id="SendEmailTemplate">
      <b>
        <xsl:value-of select="//SendEmailTemplate/Username" />,
      </b>
      <br/>
      <br/>
      <div class="body">
        To change your password, click on the following link:
        <br/>
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="//SendEmailTemplate/Url"/>
          </xsl:attribute>
          <xsl:value-of select="//SendEmailTemplate/Url" />
        </a>
      </div>
      <br/>
      <div class="Footer">
        Best Regards,
        <br/><xsl:value-of select="//SendEmailTemplate/AuthorName" />.
      </div>
      <div style="color:#aaaaaa;margin:10px 0 14px 0;padding-top:10px;border-top:1px solid #eeeeee">
        This email is a service from Your Company.
      </div>
    </div>
  </xsl:template>
</xsl:stylesheet>',N'en-US',N'Reset password',N'20140618 13:52:56.287',N'20140122 14:36:31.740',N'EmailTemplate');
INSERT INTO [EmailTemplate] ([EmailTemplateId],[Name],[Type],[Body],[DefaultLanguageCode],[Subject],[LastModified],[Created],[Discriminator]) VALUES (N'78fa9ead-fc4d-4cbd-aba0-456a0ad0b72a',N'case-created-notification',N'Xsl',N'<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head id="Head1">
        <style type="text/css">
          #CaseNotification {
            color: #444444;
            font-family: ''Lucida Grande'',Verdana,Arial,sans-serif;
            font-size: 12px;
            line-height: 18px;
          }
          h1 {font-size: 20px;}
          h2 {font-size: 18px;}
          h3 {font-size: 16px; padding: 2px 2px 2px 2px}
          .footer {padding: 10px 0 0 0}
        </style>
        <title>
          Case created notification
        </title>
      </head>
      <body>
        <xsl:call-template name="CaseCreatedNotificationTemplate"></xsl:call-template>
      </body>
    </html>
  </xsl:template>
  <xsl:template name="CaseCreatedNotificationTemplate">
    <div id="CaseNotification">
      The case <b>
        "<xsl:value-of select="//Case/Title" />"
      </b> has been created
      <xsl:apply-templates select="//Case/Contact[count(child::*) != 0]"></xsl:apply-templates>
      <div class="Footer">
        Regards,
        <br/> <xsl:value-of select="//Case/AgentName" />.
      </div>
      <div style="color:#aaaaaa;margin:10px 0 14px 0;padding-top:10px;border-top:1px solid #eeeeee">
        This email is a service from Your Company.
      </div>
    </div>
  </xsl:template>
  <xsl:template match="Contact">
    <br/>
    <br/>
    Contact:<xsl:value-of select="FullName"/>
    <br/>
    Phones:
    <xsl:for-each select="Phones">
    <xsl:value-of select="Phone/Number"/>
      <span>,&#160;</span>
  </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>',N'en-US',N'Case created notification email',N'20130503 12:18:57.587',N'20130503 10:47:43.207',N'EmailTemplate');
INSERT INTO [EmailTemplate] ([EmailTemplateId],[Name],[Type],[Body],[DefaultLanguageCode],[Subject],[LastModified],[Created],[Discriminator]) VALUES (N'7ba07b86-95e1-4e83-9d6a-365f41cdbd48',N'order-confirmation',N'Xsl',N'<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head id="Head1">
        <style type="text/css">
          #PurchaseOrder {}
          h1 {font-size: 20px;}
          h2 {font-size: 18px;}
          h3 {font-size: 16px; background-color: #cccccc; padding: 2px 2px 2px 2px}
          .introduction {padding: 5px 0 0 0}
        </style>
        <title>
          Order Notification
        </title>
      </head>
      <body>
        <xsl:apply-templates select="//Order"></xsl:apply-templates>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="Order">
    <div id="PurchaseOrder">
      <xsl:value-of select="CustomerName"/>,<br /><br />

      Your order with YOUR COMPANY has been completed.<br /><br />
      We thank you for your business.<br /><br />

      <h1>**Thanks for ordering</h1>
      <div class="introduction">
        We thank you for your business
      </div>

      <h1>**YOUR ORDER SUMMARY</h1>
      <xsl:call-template name="OrderHeader"></xsl:call-template>
      <div class="OrderForms">
        <h2>Products Purchased:</h2>
        <xsl:apply-templates select="OrderForms/OrderForm"></xsl:apply-templates>
      </div>
      <xsl:call-template name="OrderFooter"></xsl:call-template>
      <h1>**SUPPORT AND ASSISTANCE</h1>
      <div class="Footer">
        If you need further assistance or have any other questions, feel free to contact us as follows:<br /><br />
        <strong>Support e-mail:</strong> <a href="mailto:support@yourcompany.com">support@yourcompany.com</a> <br /><br />
        For more information on YourCompany and our products and services, please visit
        <a href="http://www.yourcompany.com">http://www.yourcompany.com</a>. <br /><br />
        Regards,<br/> Your Company.
      </div>
    </div>
  </xsl:template>
  <xsl:template name="PaymentPlanSchedule">
    <div class="schedule">
      Payment Schedule: starting&#160;<xsl:value-of select="ms:format-date(StartDate, ''MMM dd, yyyy'')"/>&#160;every&#160;<xsl:value-of select="CycleLength"/>&#160;<xsl:call-template name="PlanCycle" />&#160;for&#160;<xsl:value-of select="MaxCyclesCount"/>&#160;<xsl:call-template name="PlanCycle" />&#160;till&#160;<xsl:value-of select="ms:format-date(EndDate, ''MMM dd, yyyy'')"/>
    </div>
  </xsl:template>
  <xsl:template name="PlanCycle">
    <xsl:value-of select="CycleMode"/>(s)
  </xsl:template>
  <xsl:template name="OrderHeader">
    Order Number: <xsl:value-of select="TrackingNumber"/><br/>
    Status: <xsl:value-of select="Status"/><br/>
    Name: <xsl:value-of select="CustomerName"/><br/>
    Email: <a>
      <xsl:attribute name="href">
        mailto:<xsl:value-of select="//OrderAddresses/OrderAddress[OrderAddressId=//AddressId]/Email"/>
      </xsl:attribute>
      <xsl:value-of select="//OrderAddresses/OrderAddress[OrderAddressId=//AddressId]/Email"/>
    </a>
  </xsl:template>
  <xsl:template name="OrderFooter">
    <h3>Order Summary</h3>
    <div class="OrderSummary">
      Sub Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(Subtotal, ''###,##0.00'')"/><br/>
      Handling Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(HandlingTotal, ''###,##0.00'')"/><br/>
      Shipping Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(ShippingTotal, ''###,##0.00'')"/><br/>
      Total Tax: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(TaxTotal, ''###,##0.00'')"/><br/>
      TOTAL: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(Total, ''###,##0.00'')"/><br/>
    </div>
  </xsl:template>
  <xsl:template match="OrderForm">
    <div class="OrderForm">
      <div class="OrderForms">
        <h3>Line Items</h3>
        <xsl:apply-templates select="LineItems/LineItem"></xsl:apply-templates>
        <h3>Payments</h3>
        <xsl:apply-templates select="Payments/Payment"></xsl:apply-templates>
      </div>
      <div class="OrderSummary">
      </div>
    </div>
  </xsl:template>
  <xsl:template match="LineItem">
    <div class="LineItem">
      <xsl:value-of select="format-number(Quantity, ''###,###.##'')"/>&#160;<xsl:value-of select="DisplayName"/> - <xsl:value-of select="//BillingCurrency"/>&#160;<xsl:value-of select="format-number(ListPrice, ''###,###.00'')"/> each
    </div>
  </xsl:template>
  <xsl:template match="Payment">
    <div class="Payment">
      Payment Method: <xsl:value-of select="PaymentMethodName"/><br/>
      Amount: <xsl:value-of select="//BillingCurrency"/>&#160;<xsl:value-of select="format-number(Amount, ''###,##0.00'')"/>
    </div>
  </xsl:template>
</xsl:stylesheet>',N'en-US',N'Order confirmation email template',N'20130514 12:22:12.583',N'20130424 12:20:12.767',N'EmailTemplate');
INSERT INTO [EmailTemplate] ([EmailTemplateId],[Name],[Type],[Body],[DefaultLanguageCode],[Subject],[LastModified],[Created],[Discriminator]) VALUES (N'8cd4d85d-9fad-47ad-9689-d179cd29ad52',N'case-public-reply',N'Xsl',N'<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head id="Head1">
        <style type="text/css">
          #PubliReplyItemTemplate {
          color: #444444;
          font-family: ''Lucida Grande'',Verdana,Arial,sans-serif;
          font-size: 12px;
          line-height: 18px;
          }
          h1 {font-size: 20px;}
          h2 {font-size: 18px;}
          h3 {font-size: 16px; padding: 2px 2px 2px 2px}
          .footer {padding: 10px 0 0 0}
        </style>
        <title>
          Public reply
        </title>
      </head>
      <body>
        <xsl:call-template name="PubliReplyItemTemplate"></xsl:call-template>
      </body>
    </html>
  </xsl:template>
  
  <xsl:template name="PubliReplyItemTemplate">
    <div id="PubliReplyItemTemplate">
      Public reply for case #<xsl:value-of select="//PublicReplyItem/CaseId" />
      <br/>
      <h3><xsl:value-of select="//PublicReplyItem/Title" /></h3>
        <div class="body">
          <xsl:value-of select="//PublicReplyItem/Body" />
        </div>
      <div class="Footer">
        Best Regards,
        <br/><xsl:value-of select="//PublicReplyItem/AuthorName" />.
      </div>
      <div style="color:#aaaaaa;margin:10px 0 14px 0;padding-top:10px;border-top:1px solid #eeeeee">
        This email is a service from Your Company.
      </div>
    </div>
  </xsl:template>
</xsl:stylesheet>',N'en-US',N'Reply to case',N'20130503 13:25:20.700',N'20130503 13:15:43.170',N'EmailTemplate');
