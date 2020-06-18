---
title: 支払ゲートウェイ設定
description: 支払ゲートウェイ設定
layout: docs
date: 2016-06-03T10:21:23.333Z
priority: 1
---
## はじめに

支払ゲートウェイは、指定されたコンテキストに基づいて支払を完了する為に外部プロバイダーに実際の支払を転送するのに利用される注文プロセスの一部です。全ての支払方法は、支払ゲートウェイとその構成に基づいて関連づけられていなければなりません。Virto Commerce は、iCharge Payment integrator モジュールを利用し、既に多くのゲートウェイがサポートされています。詳細については、<a href="http://www.nsoftware.com/kb/help/BPN5-A/intro.rst" rel="nofollow">http://www.nsoftware.com/kb/help/BPN5-A/intro.rst</a> をご参照ください。

## サポートされているゲートウェイ

最初に行わなければならないことは、多くのゲートウェイから一つ（それ以上）を選択し、ゲートウェイベンダーにアカウントを設定します。既にアカウントを持っていいた場合、ログオンIDとパスワードを管理ツールで支払手順に従って、利用することができます。サポートされているゲートウェイは、以下の通りです。

| | |
|-|-|
|Authorize.Net AIM(1)|<a href="http://www.authorize.net" rel="nofollow">http://www.authorize.net</a>|
|eProcessing Transparent Databse Engine(3)|<a href="http://www.eProcessingNetwork.com" rel="nofollow">http://www.eProcessingNetwork.com</a>|
|GoRealTime (Full-pass) (4)|<a href="http://www.gorealtime.com" rel="nofollow">http://www.gorealtime.com</a>|
|Intellipay ExpertLink (6)|<a href="http://www.intellipay.com" rel="nofollow">http://www.intellipay.com</a>|
|iTransact RediCharge HTML (8)|<a href="http://www.itransact.com" rel="nofollow">http://www.itransact.com</a>|
|NetBilling DirectMode(9)|<a href="http://www.netbilling.com" rel="nofollow">http://www.netbilling.com</a>|
|Verisign PayFlow Pro (10)|<a href="https://www.paypal.com/cgi-bin/webscr?cmd=_payflow-pro-overview-outside" rel="nofollow">https://www.paypal.com/cgi-bin/webscr?cmd=_payflow-pro-overview-outside</a>|
|USA ePay CGI Transaction Gateway(13)|<a href="http://www.usaepay.com" rel="nofollow">http://www.usaepay.com</a>|
|Plug 'n Pay (14)|<a href="http://www.plugnpay.com" rel="nofollow">http://www.plugnpay.com</a>|
|Planet Payment iPay(15)|<a href="http://planetpayment.com/" rel="nofollow">http://planetpayment.com/</a>|
|MPCS (16)|<a href="http://merchantcommerce.net/" rel="nofollow">http://merchantcommerce.net/</a>|
|RTWare (17)|<a href="http://www.rtware.net/" rel="nofollow">http://www.rtware.net/</a>|
|ECX (18)|<a href="http://www.ecx.com" rel="nofollow">http://www.ecx.com</a>|
|Bank of America eStores (Form Post)(19)|<a href="http://bankofamerica.com/merchantservices" rel="nofollow">http://bankofamerica.com/merchantservices</a>|
|Innovative Gateway (PHP)(20)|<a href="http://www.innovativegateway.com" rel="nofollow">http://www.innovativegateway.com</a>|
|Merchant Anywhere (Transaction Central) (21)|<a href="http://www.merchantanywhere.com/" rel="nofollow">http://www.merchantanywhere.com/</a>|
|SkipJack (22)|<a href="http://www.skipjack.com" rel="nofollow">http://www.skipjack.com</a>|
|ECHOnline NVP API(23)|<a href="http://www.echo-inc.com" rel="nofollow">http://www.echo-inc.com</a>|
|3 Delta Systems (3DSI) EC-Linx(24)|<a href="http://www.3dsi.com" rel="nofollow">http://www.3dsi.com</a>|
|TrustCommerce API(25)|<a href="http://www.trustcommerce.com" rel="nofollow">http://www.trustcommerce.com</a>|
|PSIGate XML(26)|<a href="http://www.psigate.com" rel="nofollow">http://www.psigate.com</a>|
|PayFuse XML(27)|<a href="http://www.firstnationalmerchants.com/" rel="nofollow">http://www.firstnationalmerchants.com/</a>|
|PayFlowLink (28)|<a href="http://www.verisign.com" rel="nofollow">http://www.verisign.com</a>|
|Paymentech Orbital Gateway V4.3(29)|<a href="http://www.paymentech.com" rel="nofollow">http://www.paymentech.com</a>|
|LinkPoint (30)|<a href="http://www.linkpoint.com" rel="nofollow">http://www.linkpoint.com</a>|
|Moneris eSelect Plus Canada(31)|<a href="http://www.moneris.com" rel="nofollow">http://www.moneris.com</a>|
|uSight Gateway Post-Auth(32)|<a href="http://gateway.usight.com" rel="nofollow">http://gateway.usight.com</a>|
|Fast Transact VeloCT (Direct Mode)(33)|<a href="http://www.fasttransact.com/" rel="nofollow">http://www.fasttransact.com/</a>|
|NetworkMerchants Direct-Post API(34)|<a href="http://www.nmi.com/" rel="nofollow">http://www.nmi.com/</a>|
|Ogone DirectLink(35)|<a href="http://www.ogone.be" rel="nofollow">http://www.ogone.be</a>|
|Concord EFSNet (36) (Depreciated, use LinkPoint)|<a href="https://secure.todaysebiz.com" rel="nofollow">https://secure.todaysebiz.com</a>|
|TransFirst Transaction Central Classic (formerly PRIGate) (37)|<a href="www.transfirst.com" rel="nofollow">www.transfirst.com</a>|
|Protx (38) (Depreciated, use SagePay (67) instead)|<a href="http://www.sagepay.com" rel="nofollow">http://www.sagepay.com</a>|
|Optimal Payments / FirePay Direct Payment Protocol (39)|<a href="http://www.optimalpayments.com/" rel="nofollow">http://www.optimalpayments.com/</a>|
|Merchant Partners (Transaction Engine) (40)|<a href="http://www.merchantpartners.com/" rel="nofollow">http://www.merchantpartners.com/</a>|
|CyberCash (41)|<a href="http://www.cybercash.net/" rel="nofollow">http://www.cybercash.net/</a>|
|First Data Global Gateway (Linkpoint) (42)|<a href="http://www.firstdata.com" rel="nofollow">http://www.firstdata.com</a>|
|YourPay (43) (Depreciated, use Linkpoint (42) instead)|<a href="http://www.yourpay.com" rel="nofollow">http://www.yourpay.com</a>|
|ACH Payments AGI (44)|<a href="http://www.ach-payments.com" rel="nofollow">http://www.ach-payments.com</a>|
|Payments Gateway AGI(45)|<a href="https://www.paymentsgateway.net/" rel="nofollow">https://www.paymentsgateway.net/</a>|
|Cyber Source SOAP API (46)|<a href="http://www.cybersource.com" rel="nofollow">http://www.cybersource.com</a>|
|eWay XML API (Australia) (47)|<a href="http://www.eway.com.au/" rel="nofollow">http://www.eway.com.au/</a>|
|goEmerchant XML(48)|<a href="http://www.goemerchant.com/" rel="nofollow">http://www.goemerchant.com/</a>|
|TransFirst eLink(50)|<a href="http://www.transfirst.com" rel="nofollow">http://www.transfirst.com</a>|
|Chase Merchant Services (Linkpoint) (51)|<a href="http://www.chase.com" rel="nofollow">http://www.chase.com</a>|
|PSIGate XML Interface (52)|<a href="http://www.psigate.com" rel="nofollow">http://www.psigate.com</a>|
|Thompson Merchant Services NexCommerce (iTransact mode) (53)|<a href="http://www.thompsonmerchant.com" rel="nofollow">http://www.thompsonmerchant.com</a>|
|WorldPay Select Junior Invisible (54)|<a href="http://www.worldpay.com" rel="nofollow">http://www.worldpay.com</a>|
|TransFirst Transaction Central (55)|<a href="http://www.transfirst.com" rel="nofollow">http://www.transfirst.com</a> (This is different from TransFirst eLink, supported above. The TransactionCentral gateway is also used by MerchantAnywhere and PRIGate)|
|Paygea (56)|<a href="http://www.paygea.com/" rel="nofollow">http://www.paygea.com/</a>|
|Sterling SPOT API (HTTPS POST)(57)|<a href="http://www.sterlingpayment.com" rel="nofollow">http://www.sterlingpayment.com</a>|
|PayJunction Trinity Gateway (58)|<a href="http://www.payjunction.com" rel="nofollow">http://www.payjunction.com</a>|
|SECPay (United Kingdom) API Solution(59)|<a href="http://www.secpay.com" rel="nofollow">http://www.secpay.com</a>|
|Payment Express PXPost (60)|<a href="http://www.paymentexpress.com" rel="nofollow">http://www.paymentexpress.com</a>|
|Elavon/NOVA/My Virtual Merchant (61)|<a href="http://www.myvirtualmerchant.com" rel="nofollow">http://www.myvirtualmerchant.com</a>|
|Sage Payment Solutions (Bankcard HTTPS Post protocol)(62)|<a href="http://www.sagepayments.com" rel="nofollow">http://www.sagepayments.com</a>|
|SecurePay (Script API/COM Object Interface) (63)|<a href="http://securepay.com" rel="nofollow">http://securepay.com</a>|
|Moneris eSelect Plus USA (64)|<a href="http://www.moneris.com" rel="nofollow">http://www.moneris.com</a>|
|Beanstream Process Transaction API(65)|<a href="http://beanstream.com" rel="nofollow">http://beanstream.com</a>|
|Verifi Direct-Post API(66)|<a href="http://www.verifi.com" rel="nofollow">http://www.verifi.com</a>|
|SagePay Direct (Previously Protx) (67)|<a href="http://www.sagepay.com" rel="nofollow">http://www.sagepay.com</a>|
|Merchant E-Solutions Payment Gateway (Trident API)(68)|<a href="http://merchante-solutions.com/" rel="nofollow">http://merchante-solutions.com/</a>|
|PayLeap Web Services API (69)|<a href="http://www.payleap.com" rel="nofollow">http://www.payleap.com</a>|
|PayPoint.net (Previously SECPay) API Solution (70)|<a href="http://paypoint.net" rel="nofollow">http://paypoint.net</a>|
|Worldpay XML (Direct/Invisible) (71)|<a href="http://www.worldpay.com" rel="nofollow">http://www.worldpay.com</a>|
|ProPay Merchant Services API (72)|<a href="http://www.propay.com" rel="nofollow">http://www.propay.com</a>|
|Intuit QuickBooks Merchant Services (QBMS) (73)|<a href="http://payments.intuit.com/" rel="nofollow">http://payments.intuit.com/</a>|
|Heartland POS Gateway (74)|<a href="http://www.heartlandpaymentsystems.com/" rel="nofollow">http://www.heartlandpaymentsystems.com/</a>|
|Litle Online Gateway (75)|<a href="http://www.litle.com/" rel="nofollow">http://www.litle.com/</a>|
|BrainTree DirectPost (Server-to-Server) Gateway (76)|<a href="http://www.braintreepaymentsolutions.com/" rel="nofollow">http://www.braintreepaymentsolutions.com/</a>|
|JetPay Gateway (77)|<a href="http://www.jetpay.com/" rel="nofollow">http://www.jetpay.com/</a>|
|Sterling XML Gateway (78)|<a href="http://www.securenet.com" rel="nofollow">http://www.securenet.com</a>|
|Landmark Flat File HTTPS Post (79)|<a href="http://landmarkclearing.com/" rel="nofollow">http://landmarkclearing.com/</a>|
|HSBC XML API (80)|<a href="http://www.business.hsbc.co.uk/1/2/business-banking/business-payment-processing/business-debit-and-credit-card-processing" rel="nofollow">http://www.business.hsbc.co.uk/1/2/business-banking/business-payment-processing/business-debit-and-credit-card-processing</a>|
|BluePay 2.0 Post (81)|<a href="http://www.bluepay.com" rel="nofollow">http://www.bluepay.com</a>|
|Adyen API Payments (82)|<a href="http://www.adyen.com" rel="nofollow">http://www.adyen.com</a>|
|Barclay XML API (83)|<a href="http://www.barclaycard.co.uk/business/" rel="nofollow">http://www.barclaycard.co.uk/business/</a>|
|PayTrace Payment Gateway (84)|<a href="http://www.paytrace.com/" rel="nofollow">http://www.paytrace.com/</a>|
|YKC Gateway (85)|<a href="http://www.ykc-bos.co.jp/" rel="nofollow">http://www.ykc-bos.co.jp/</a>|
|Cyberbit Gateway (86)|<a href="http://www.cyberbit.eu/" rel="nofollow">http://www.cyberbit.eu/</a>|
|GoToBilling Gateway (87)|<a href="http://www.gotobilling.com/" rel="nofollow">http://www.gotobilling.com/</a>|
|TransNational Bankcard (88)|<a href="http://www.tnbci.com/" rel="nofollow">http://www.tnbci.com/</a>|
|Netbanx (89)|<a href="http://www.netbanx.com/" rel="nofollow">http://www.netbanx.com/</a>|
|MIT (90)|<a href="http://www.centrodepagos.com.mx" rel="nofollow">http://www.centrodepagos.com.mx</a>|
|DataCash (91)|<a href="http://www.datacash.com/" rel="nofollow">http://www.datacash.com/</a>|
|ACH Federal (92)|<a href="http://www.achfederal.com/" rel="nofollow">http://www.achfederal.com/</a>|
|Global Iris (HSBC) (93)|<a href="http://www.globalpaymentsinc.com/UK/customerSupport/globaliris.html" rel="nofollow">http://www.globalpaymentsinc.com/UK/customerSupport/globaliris.html</a>|

## 支払方法を構成

既にゲートウェイ・プロバイダーのアカウントを持っているものとして手順を説明します。例として、 authorize.net を利用します。

新しい支払方法を作成するには、以下の手順になります。

* 支払を管理する権限を持つユーザーでログオンし、管理ツールを開きます。
* Settings/Payments に移動し、Addボタンをクリックします。
* 名前・説明を入力し、支払に使用するゲートウェイを選択します。
* Active をチェックし、次へボタンをクリックします。
  ![](../../../../../assets/images/docs/paymentCreate.png)
* 必要な場合は、支払方法の表示名としてローカリゼーションを追加します。
* 最終ページでは、選択されたゲートウェイの情報を設定します。(Authorize.Net を選択したものとして仮定する)
  * Authorize.Net の AIM(3.1) プロトコルのエクストラセキュリティキーを入力します。Authorize.net で構成していない場合は、未入力
    * このゲートウェイは、ユニークなセキュリティ機能です。利用するには、Autorize.net で提供されるセキュリティハッシュを追加しなけらばならない。構成手順でハッシュセキュリティサポートされていない場合、サーバーでチェックされません。
  * マーチャントゲートウェイログオン、パスワードを入力します。
  * "Default URL for a specific Gateway" 項目は、iCharge のデフォルトゲートウェイで上書きされます。 例として、authorize.net テスト用に利用される "<a href="https://test.authorize.net/gateway/transact.dll" rel="nofollow">https://test.authorize.net/gateway/transact.dll</a>"　に変更されます。
  * テスト用ゲートウェイを利用する場合は、"Identifies if transaction is in test mode" をチェックします。それ以外の場合は、チェックしないでください。テストモードでは、実際の支払が行われません。
  ![](../../../../../assets/images/docs/paymentCreate3.png)

特定のゲートウェイの全ての情報は、こちらをご参照ください。<a href="http://www.nsoftware.com/kb/help/BPN5-A/pg_icgatewaysetup.rst" rel="nofollow">http://www.nsoftware.com/kb/help/BPN5-A/pg_icgatewaysetup.rst</a>