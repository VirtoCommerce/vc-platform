INSERT INTO [EmailTemplateLanguage] ([EmailTemplateLanguageId],[LanguageCode],[Subject],[Body],[Priority],[EmailTemplateId],[LastModified],[Created],[Discriminator]) VALUES (N'228e0b1a-5dd2-4383-a58e-32f8196e0da1',N'fr-FR',N'Notification',N'<?xml version="1.0" encoding="utf-8"?>
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
      Sub Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(SubTotal, ''###,###.00'')"/><br/>
      Handling Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(HandlingTotal, ''###,###.00'')"/><br/>
      Shipping Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(ShippingTotal, ''###,###.00'')"/><br/>
      Total Tax: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(TaxTotal, ''###,###.00'')"/><br/>
      TOTAL: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(Total, ''###,###.00'')"/><br/>
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
        <!--
    Sub Total: <xsl:value-of select="SubTotal"/><br/>
    Handling Total: <xsl:value-of select="HandlingTotal"/><br/>
    Shipping Total: <xsl:value-of select="ShippingTotal"/><br/>
    Total Tax: <xsl:value-of select="TaxTotal"/><br/>
    Discount: <xsl:value-of select="DiscoutnAmount"/><br/>
    TOTAL: <xsl:value-of select="Total"/><br/>
    -->
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
      Amount: <xsl:value-of select="//BillingCurrency"/>&#160;<xsl:value-of select="format-number(Amount, ''###,###.00'')"/>
    </div>
  </xsl:template>
</xsl:stylesheet>',0,N'7ba07b86-95e1-4e83-9d6a-365f41cdbd48',N'20130425 08:06:14.107',N'20130425 08:06:14.107',N'EmailTemplateLanguage');
INSERT INTO [EmailTemplateLanguage] ([EmailTemplateLanguageId],[LanguageCode],[Subject],[Body],[Priority],[EmailTemplateId],[LastModified],[Created],[Discriminator]) VALUES (N'd9d063fc-6e38-4020-88be-836a0c17ae9b',N'fr-FR',N'Notification',N'<?xml version="1.0" encoding="utf-8"?>
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
      Sub Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(SubTotal, ''###,###.00'')"/><br/>
      Handling Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(HandlingTotal, ''###,###.00'')"/><br/>
      Shipping Total: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(ShippingTotal, ''###,###.00'')"/><br/>
      Total Tax: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(TaxTotal, ''###,###.00'')"/><br/>
      TOTAL: <xsl:value-of select="BillingCurrency"/>&#160;<xsl:value-of select="format-number(Total, ''###,###.00'')"/><br/>
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
        <!--
    Sub Total: <xsl:value-of select="SubTotal"/><br/>
    Handling Total: <xsl:value-of select="HandlingTotal"/><br/>
    Shipping Total: <xsl:value-of select="ShippingTotal"/><br/>
    Total Tax: <xsl:value-of select="TaxTotal"/><br/>
    Discount: <xsl:value-of select="DiscoutnAmount"/><br/>
    TOTAL: <xsl:value-of select="Total"/><br/>
    -->
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
      Amount: <xsl:value-of select="//BillingCurrency"/>&#160;<xsl:value-of select="format-number(Amount, ''###,###.00'')"/>
    </div>
  </xsl:template>
</xsl:stylesheet>',0,N'29556bd5-dc56-4485-af80-e80267e004ee',N'20130425 08:05:48.513',N'20130425 08:05:28.003',N'EmailTemplateLanguage');
