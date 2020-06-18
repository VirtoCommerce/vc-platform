---
title: ユーザー管理、ロール、ロール割り当て
description: ユーザー管理、ロール、ロール割り当て
layout: docs
date: 2016-06-03T10:21:23.333Z
priority: 5
---
## はじめに

ユーザーは、Vitro Commerce Management アプリケーションとサービスを利用する個人です。ユーザーは、組織内部に所属し、お客様と混同しないでください。

Vitro Commerce 機能を利用するには、少なくとも１つのロールに割り当てられている必要があります。各ロールは、特定の権限がユーザーに与えられます。これらの権限の許可は、Virto Commerce アプリケーションの各機能へのアクセスを制限します。例として、ユーザーがカタログを表示する場合、カタログ以外の操作ができなく制限することができます。カタログ管理権限の無い倉庫管理ユーザーには、カタログを表示することができないように設定することができます。

一般的なロールは、Virto Commerce インストール時にデフォルトで設定されます。Vitro Commerce 管理アプリケーションで各ロールを編集することができます。

## ユーザー管理と権限割り当て

このセクションでは、Virto Commerce Management アプリケーションとアクセス許可が適切にユーザーに割り当てられるように必要な手順と影響を与える様々なセキュリティ上の考慮すべき事項について説明します。

アプリケーションのデータや機能に適切にアクセス権を設定する為に Virto Commerce 管理者を支援することを意図しています。

設定を行うには、ユーザー作成とユーザーロール設定できる為に Virto Commerce 管理ツールを利用できることが必要であるため、Virto Commerce 管理権限を持っている必要があります。

## アクセス許可

アクセス許可は、ユーザーにロールを割り当てることによってコントロールされます。ロールは、アクセス権限の集合になります。ロールは、複数のユーザーに割り当てることができ、各ユーザーは、複数の割り当てられたロールを持つことができます。

割り当てられたロールの組み合わせにより、ユーザーは、自分が必要な情報と機能へのアクセス権を持つことができます。

このモデルは、柔軟性の高い機能を提供しますが、複数の店舗やカタログ、ユーザの独自の個別セットを持つそれぞれに複雑な環境では、アクセス権を管理するためのベストプラクティスを確立することが重要です。

注意：利用可能な権限とそれらの説明については、[利用可能な権限](docs/old-versions/vc111userguide-jp/users-management-roles-and-role-assignment/available-permissions) ページをご参照ください。

## ユーザー

Virto Commerce 管理ツールを利用する組織内の個々人は、個人のユーザーアカウントを持っている必要があります。

各ユーザーは、プロフィール作成時に指定されたロールによって定義された特定の権限を持っています。Virto Commerce ユーザーは、それぞれの業務を行う為だけに必要なデータにアクセスする権限を持つよう構成することが必要です。

## 管理者ユーザー

インストール後、スーパーユーザー (管理者) 権限を持つユーザーアカウントがあります。管理者だけげユーザーとロールを管理する権限持ちます。

注意：スーパーユーザー権限を持つ第二のユーザーを作成することを強く推奨します。管理者のユーザーアカウントのいずれかがロックされている場合、他の管理者ユーザーは、ロックを解除することができます。

## ユーザー作成

ユーザーを作成するには、適切な権限を持っている必要があります。

ユーザー作成手順:

1. モジュールリストから **Users** モジュールを選択します。
2. Users モジュールの **Accounts** タブを選択します。
3. ユーザーリストの右上にある **Add** ボタンをクリックします。
  <img src="../../../assets/images/docs/image2013-5-27_13_14_58.png" />
4. ポップアップウィザードの最初のステップは、必須項目の入力です。(ログイン/パスワード):
  <img src="../../../assets/images/docs/image2013-5-27_13_19_32.png" />
5. ポップアップウィザードの次のステップは、事前に作成済のロールリストからロールを割り当てます。
  <img src="../../../assets/images/docs/image2013-5-27_13_24_9.png" />
6. 全ての必須項目入力後、Finish ボタンをクリックします。

作成されたユーザーは、ユーザーリストに表示されます。

## ユーザー編集

1. **Users** モジュールを開きます。
2. **Accounts** タブをクリックします。
3. ユーザー選択後、ダブルクリックします。
4. 必要な項目を編集します。
5. Save ボタンクリックで保存します。

## ユーザーの無効化/有効化

1. **Users** モジュールを開きます。
2. **Accounts** タブをクリックします。
3. ユーザー選択後、ダブルクリックします。
4. **Reject user/Approve user** ボタンをクリックします。
5. Save ボタンクリックで保存します。

## リセットユーザーパスワード

1. **Users** モジュールを開きます。
2. **Accounts** タブをクリックします。
3. ユーザー選択後、ダブルクリックします。
4. **Reset password** ボタンをクリックします。
5. 新しいパスワードと繰り返しパスワードを入力します。
6. OK ボタンクリックし、パスワードを変更します。
  ![](../../../assets/images/docs/resetPassword.png)

## ロール

Virto Commerce 管理アプリケーション機能を利用するには、最初にユーザーにアクセス権を割り当てる必要があります。Virto Commerce 管理アプリケーションでロールを割り当てることで、間接的にユーザーに権限を割り当てることになります。
ユーザーは、複数のロールを割り当てることができます。新しいロールの作成は、アクセス権限の指定が含まれており、アクセス権限については、Virto Commerce 管理ツールの機能をご参照ください。

## ビルトインロール

Virto Commerce インストール後には、次のユーザーロールが作成されます。

* Super User (管理者ロールと呼ばれる), Virto Commerce 管理ツールの全ての機能にアクセス権限を持ちます。全ての機能・全てのアプリケーションにアクセスする必要がない場合は、ユーザーにロールを割り当て無いようにしてください。
* Catalog manager (カタログ・カテゴリー・商品・価格の作成・編集ができます。)
* Store marketing (プロモーション・ダイナミックコンテンツ・価格リスト・ストア設定を管理できます。)
* Customer service agent (顧客とケースを管理できます。)
* Shipping receiving (フルフィルメント・配送完了・返品・交換を管理できます。)
* Configuration (設定を管理できます。)
* Private shopper (ストアフロントの店舗制御を行うことができます。)

## アクセス権限構成

アクセス権限は、階層構造になっており、カタログ権限が与えられている場合、サブ権限のロールも割り当てられたことになります。

## ロール作成

ユーザーを作成するには、適切な権限を持つように設定する必要があります。

ユーザー作成手順:

1. モジュールリストから、**Users** モジュールを選択します。
2. **Users** モジュールの **Roles** タブ をクリックします。
3. ロールリストの右上の **Add** ボタンをクリックします。
  <img src="../../../assets/images/docs/image2013-5-27_13_14_58.png" />
4. ポップアップウィザードでは、必須項目を入力します。(Name, Permissions)
5. **Finish** ボタンをクリックします。
  <img src="../../../assets/images/docs/image2013-5-27_17_25_54.png" />

## ロール編集

1. **Users** モジュールを開きます。
2. **Roles** タブをクリックします。
3. ロール選択後、ダブルクリックします。
4. 必須項目を入力します。
5. Save ボタンクリックで保存します。

## ユーザーに複数のロールを割り当てる

ユーザーが複数の権限を持ち、一つのロール以上のアクセス権が必要な場合、Virto Commerce システムでは、複数のロールを割り当てる設定ができます。

## ロール作成

1. **Users** モジュールを開きます。
2. **Roles** タブをクリックします。
3. Role を選択します。
4. ロールリスト上部の **Remove** ボタンをクリックします。
5. 削除の確認画面で処理を実行します。

## モジュール

モジュールは、Virto Commerce 管理ツールのトップレベルのメニュー項目になります。それぞれのモジュールは、機能を持ち、機能毎にアクセス権があります。ユーザーは、各機能へのアクセス権が無い場合、モジュールを利用することができません。

## 一般的なユーザーロールと権限割り当ての例

次の表は、組織が必要とするロールのサンプルで、ユーザーが、カタログ・ストア・配送に割り当てられている例を表しています。

|ロール|モジュール|カタログへの割り当て|ストアへの割り当て|フルフィルメントへの割り当て|
|-----|---------|-------------------|----------------|--------------------------|
|Shipper|Fulfillment|No|No|Yes|
|Receiver|Fulfillment|No|No|Yes|
|Merchandiser|Maketing, Catalog, Reporting|Yes|Yes|Yes|
|Marketer|Marketing|Yes|Yes|No|
|Customer service agent|Customer service|Yes|Yes|Yes|
|Catalog manager|Catalog|Yes|No|Yes|
|Administrator|Settings|No|No|No|

## 利用可能なアクセス権限

|モジュールのよるアクセス権限|説明|影響|
|-------------------------|----|----|
|Users| |アクセス権限が選択されていない場合、メニュー項目は表示されません。|
|Manage Accounts|ユーザーアカウントに作成・削除・編集権限が割り当てられている。|タブが表示|
|Manage Roles|作成・削除・編集ロール権限が割り当てられている。|タブが表示|
| | | |
|Order Management| |アクセス権限が選択されていない場合、注文管理メニュー項目は表示されません。|
|Manage Orders|注文情報の作成・削除権限が割り当てられている。|タブが表示|
|Issue Order Refunds|注文情報の支払権限が割り当てられている。|支払ボタンが利用可能|
|Create Order Exchange|注文情報のアイテム作成・交換変更権限が割り当てられている。|Summaryタブの**Create Exchange** ボタンと注文編集ビューの返品が表示|
|Create Order Returns|注文情報の作成・返品変更権限が割り当てられている。|**Create Return** ボタンが利用可能|
|Complete Order Returns|注文情報の返品完了権限が割り当てられている。|注文情報ビューの返品タブの **Complete** ボタンが利用可能|
|Cancel Order Returns|注文情報の返品キャンセル権限が割り当てられている。|注文情報編集ビューの返品タブの **Cancel** ボタンが利用可能|
| | | |
|Catalog Management| |アクセス権限が選択されていない場合、カタログメニューは表示されません。|
|Manage Catalogs|カタログの作成・編集・削除権限が割り当てられている。|カタログメニューアイテムが利用可能(カタログを開く・削削除)**Add catalog** ボタンが利用可能|
|Manage Categories|カテゴリーの作成・編集・削除権限が割り当てられている。|カテゴリーメニューアイテムが利用可能(**作成・編集・削除**).|
|Manage Catalog Items (Products and SKUs)|カタログのカタログ作成・商品編集・商品削除権限が割り当てられている。|カタログアイテムリストの items management ボタンが利用可能(**作成・編集・削除**).|
|Manage Virtual Catalogs|バーチャルカタログの作成・編集・削除とリンクカテゴリーの作成・編集・削除権限が割り当てられている。|バーチャルカタログ管理が利用可能(**作成・編集・削除**).|
|Manage Linked Categories|リンクカテゴリー作成権限が割り当てられている。|リンクカテゴリー管理が利用可能|
|Manage Catalog Import Jobs|カタログのカタログインポートジョブ作成・編集・削除権限が割り当てられている。|カタログアイテムのインポートジョブ管理が利用可能|
|Run Catalog Import Jobs|カタログのカタログインポートジョブ実行権限が割り当てられている。|インポートのインポート実行ジョブボタンが利用可能|
|Manage Item Associations|カタログの関連商品作成・編集・削除権限が割り当てられている。|商品編集ダイアログの関連タブが表示|
|Create/Edit Editorial Reviews|アイテムのレビュー作成・編集・削除権限が割り当てられている。|レビュータブの管理ボタンが利用可能|
|Publish Editorial Reviews|アイテムのレビュー発行権限が割り当てられている。|レビューをアクティブにする機能が利用可能レビュー発行とドラフト作成ボタンが利用可能|
|Remove Editorial Reviews|レビュー削除権限が割り当てられている。|レビュータブの **Remove** ボタンが利用可能|
|Manage Customer Reviews|顧客レビュー管理権限が割り当てられている。|カタログのレビュータブが表示|
| | | |
|Price List Management| |"Manage Price Lists"・"Manage PriceList Assignments"が選択されていない場合、表示されない。|
|Manage Price Lists|価格の追加・編集・削除権限が割り当てられている。|価格リストタブが表示|
|Manage Item Pricing|アイテムの作成・編集・削除権限が割り当てられている。|編集ビュー詳細タブとウィザードが表示|
|PriceList Import Jobs|価格リストのインポート権限が割り当てられている。|価格リストのインポートタブが表示|
|Run PriceList Import Job| |インポートタブの Run ボタンが表示|
|Manage PriceList Assignments|価格リスト割り当て作成・編集・削除権限が割り当てられている。|価格リスト割り当てタブが表示|
| | | |
|Fulfillment| |"Manage Fulfillment Inventory"・"Manage Fulfillment Picklists"・"Receive Fulfillment Inventory"が選択されていない場合、表示されません。|
|Manage Fulfillment Inventory|在庫アイテム管理権限が割り当てられている。|"Receive Fulfillment Inventory"権限が割り当てられている場合、在庫タブが表示|
|Manage Fulfillment Picklists|ピッキングリスト作成権限が割り当てられている。|タブが表示|
|Receive Fulfillment Inventory|在庫タブの在庫受け入れ権限が割り当てられている。|**Add** ボタンが利用可能|
|Complete Shipment|配送完了権限が割り当てられている。|**Complete shipment** ボタンが利用可能|
|Edit Returns and Exchanges|注文情報の返品・交換編集権限が割り当てられている。|注文タブ編集ビューの "Returns and Exchanges" タブが表示|
| | | |
|Marketing| |権限が選択されていない場合、アイテムメニューは表示されません。|
|Publishing|コンテンツ配信条件の権限が割り当てられている。|タブが表示|
|Manage Dynamic Content|ダイナミックコンテンツの管理権限が割り当てられている。|タブが表示|
|Manage Promotions|ストアとカタログのプロモーション管理権限が割り当てられている。|タブが表示|
| | | |
|Customer Service| |権限が選択されていない場合、表示されません。|
|View all cases|全てのケースを見る権限が割り当てれらている。|全てのユーザーのケースが表示|
|View cases only assigned to a user|ログインしているユーザーにケース参照する権限が割り当てられている。|ログインしているユーザーに割り当てられているケースのみ表示|
|Search cases|全てのケースを検索する権限が割り当てられている。|フィルターが表示|
|Create new case|新しいケースを作成する権限が割り当てられている。|メニューの **Create case** が利用可能|
|Edit case properties|ケースプロパティを編集する権限が割り当てられている。|case edit controlsが利用可能|
|Delete existing case|存在するケースの削除権限が割り当てられている。|**Case Options** commandsが利用可能|
|Add public and private case comments|パブリックとプライベートコメントを作成する権限が割り当てられている。|新しいパブリック・プライベートコメント作成が利用可能|
|Add private comments|プライベートコメントのみ作成権限が割り当てられている。|新しいプライベートコメント作成が利用可能|
|Create customer|新しいコンタクト作成権限が割り当てられている。|**Add customer** ボタンが利用可能|
|Edit customer properties|コンタクトプロパティ管理権限が割り当てられている。|customer edit controlsが利用可能|
|Delete existing customer|コンタクト削除権限が割り当てられている。|お客様編集ビューの"Contact Options" メニューの **Delete customer** ボタンが利用可能|
|Create and reset passwords|パスワード作成・編集権限が割り当てられている。|お客様編集ビューの"Contact Options" メニューの **Create Login/Password** ボタンが利用可能|
|Suspend and restore access for customers|コンタクトの承認・却下権限が割り当てられている。|お客様編集ビューの"Contact Options"メニューの **Suspend Access** と **Restore Access** ボタンが利用可能|
|Create account for a contact|新しいアカウント作成権限が割り当てられている。|"Contact Options" が利用可能|
| | | |
|Configuration| |権限が選択されていない場合、メニュー項目が表示されない。|
|Configure Application Settings|アプリケーション設定管理権限が割り当てられている。|*Application/Settings* タブが表示|
|Configure Application System Jobs|アプリケーションシステムジョブの権限が割り当てられている。|*Application/System jobs* タブが表示|
|Configure Application email templates|アプリケーションメールテンプレート権限が割り当てられている。|*Application/Emal templates* タブが表示|
|Configure Application display templates|アプリケーション表示テンプレート権限が割り当てられている。|*Application/Display templates* タブが表示|
|Configure  Customer Rules|アプリケーションお客様ルール権限が割り当てられている。|*Customers/Rules* タブが表示|
|Configure  Customer Info|アプリケーションお客様情報設定権限が割り当てられている。|*Customers/Info* タブが表示|
|Configure  Customer Case types|アプリケーションお客様ケースタイプ権限が割り当てられている。|*Customers/Case types* タブが表示|
|Configure  Content places|コンテンツ配布権限が割り当てられている。|*Content places* タブが表示|
|Configure Fulfillment|フルフィルメント設定権限が割り当てられている。|*Fulfillment* タブが表示|
|Configure Payments|支払管理権限が割り当てられている。|*Payments* タブが表示|
|Configure Shipping options|配送オプション権限が割り当てられている。|*Shipping/Shipping options* タブが表示|
|Configure Shipping methods|配送方法権限が割り当てられている。|*Shipping/Shipping methods* タブが表示|
|Configure Shipping packages|配送パッケージ管理権限が割り当てられている。|*Shipping/Shipping packages* タブが表示|
|Configure Jurisdictions|配送区域管理権限が割り当てられている。|*Shipping/Jurisdictions* タブが表示|
|Configure Jurisdiction groups|配送区域グループ管理権限が割り当てられている。|*Shipping/Jurisdiction groups* タブが表示|
|Configure Tax categories|税カテゴリー権限が割り当てられている。|*Taxes/Tax categories* タブが表示|
|Configure Taxes|税管理権限が割り当てられている。|*Taxes/Taxes* タブが表示|
|Configure Tax import|税インポート権限が割り当てられている。|*Taxes/Import* タブが表示|