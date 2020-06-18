---
title: 在庫
description: 在庫
layout: docs
date: 2016-06-03T10:21:23.333Z
priority: 1
---
## はじめに

既存商品が到着すると、倉庫スタッフは、新しい在庫を反映するようにシステムを更新します。在庫を登録することを許可されたユーザーは、Commerce Managerで既存商品の在庫数を増やすことができます。

## 在庫の受け入れ

1. **Fulfillment** モジュールを開きます。
2. **Inventory** タブを開きます。
3. **Add** ボタンをクリックします。
  <img src="../../../../assets/images/docs/image2013-5-29_17_38_44.png" />
4. ポップアップダイアログで、適切なフルフィルメントセンターを選択します。
  <img src="../../../../assets/images/docs/image2013-6-14_14_55_14.png" />
5. 空のSKUを入力するか、テキストボックスの右側
  <img src="../../../../assets/images/docs/image2013-6-14_14_56_55.png" />
   ボタンをクリックし、SKU番号を選択します。
6. 数量を編集し、エンターキーを押します。
  備考: 在庫/Skuが、利用可能なリストに存在しない場合は、以下のような警告メッセージが表示されます。
  <img src="../../../../assets/images/docs/image2013-6-14_15_12_17.png" />
7. 全ての在庫と数量を入力後、OK ボタンをクリックします。

既に利用可能な在庫登録済の場合数量が増加し、未登録の場合は、リストに追加されます。

## 在庫の編集

1. **Fulfillment** モジュールを開きます。
2. **Inventory** タブを開きます。
3. 修正する在庫を選択し、ダブルクリックします。
4. 必要項目を編集します。
  <img src="../../../../assets/images/docs/image2013-6-14_15_38_48.png" />

|プロパティ|説明|
|---------|----|
|In Stock Quantity|在庫の数量|
|Reserved Quantity|予約注文数(注文できない数)|
|Reorder Min. Quantity|サプライヤーに注文する必要がある数量|
|Allow preorder|先行予約可能な商品の場合設定|
|Preorder Quantity|先行予約することができた数量|
|Preorder available|予約が利用可能になる日付|
|Allow backorder|入荷待ちを設定できる場合設定|
|Backorder Quantity|入荷待ち可能数量|
|Backorder available|予約が利用可能になる日付|

- 数量を編集します。
  <img src="../../../../assets/images/docs/image2013-6-14_16_4_3.png" />
  数量変更ダイアログでは、数量の増減を入力し編集理由を入力します。

5. 修正内容を保存します。

## 在庫の検索

リストの在庫を検索するには、Fulfillment ブロックに移動し、右側検索でフィルターを利用します。

<img src="../../../../assets/images/docs/search_inventory.PNG" />