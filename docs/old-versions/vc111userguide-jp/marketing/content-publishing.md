---
title: コンテンツ配信
description: コンテンツ配信
layout: docs
date: 2016-06-03T10:21:23.333Z
priority: 2
---
## はじめに

Marketing モジュールの "Content Publishing" タブは、コンテンツブックを管理することを意図しており、どこに、いつ、どのようにサイトに配信(表示)させるかの定義を行います。

[Dynamic Content](docs/old-versions/vc111userguide-jp/marketing/dynamic-content) ブロックでは、この種類のコンテンツがどのように利用されるかを Virto Commerce Manager で調整することについて説明します。コンテンツ配信は、コマースユーザーが、開発者のサポートを受けることなくオンラインショップのマーケティング戦略を選択しながら継続的に調整することができるようになっています。

ブロックで利用可能なアクション:

1. コンテンツグループの作成・修正配信
2. コンテンツ配信アイテムの複製
3. コンテンツ配信アイテム検索のフィルターの適用
4. コンテンツ配信プロパティの定義(コンテンツ配信の条件、コンテンツプレースなど)
5. コンテンツ配信グループの削除

## コンテンツ配信グループの作成

Virto Commerce Manager で Marketing ブロック の "Content Publishing" タブに移動し、"Add" ボタンを利用し、コンテンツ配信アイテムを作成します。

![](../../../../assets/images/docs/001-add-content-publishing.PNG)

作成の4ステップで表示される項目を入力し、新しいアイテムを保存します。

* Name - 新しいコンテンツ配信の名前を記述
* Description - コンテンツ配信の概要と詳細を記述
* Priority - アイテムの優先順位を定義
* Check the box - サイトでこのアイテムを利用可能にする場合、"Is active" をチェック
* アイテムが利用可能になる日付を開始・終了で定義(未入力の場合は、直ぐに利用可能になります)
* Content places - コンテンツ配信が利用可能になった時、インスタンス化され、カート又はホームページに表示(リストから選択);
* Dynamic Content - 利用可能オプションのリストから選択、コンテンツ配信アイテムに関連したダイナミックコンテンツをの種類を定義
* Availability conditions - コンテンツ配信の条件は、サイト上に利用可能になるように設定

## コンテンツ配信グループの複製

既に作成されたアイテムと似たコンテンツ配信グループを作成する必要が発生した場合、複製オプションを料できます。これは、コンテンツ配信グループをクローン化し、クローンの定義を編集することができます。

Marketing ブロックに移動し、コンテンツ配信グループタブを開き、"Duplicate" ボタンを利用しクローンを作成します。

![](../../../../assets/images/docs/003-duplicate-content-publishing.PNG)

## コンテンツ配信の編集

コンテンツ配信の詳細を編集する際には、Marketing モジュールの Content Publishing ブロックに移動し、アイテムをダブルクリックし、必要な項目を編集します。この手順は、[Create Content Publishing Group](docs/old-versions/vc111userguide-jp/marketing/content-publishing/create-content-publishing-group) のパラメーター変更説明で確認できます。

![](../../../../assets/images/docs/002-edit-content-publishing.PNG)

コンテンツ配信の名前・説明変更・コンテンツプレースの変更・その他条件を変更が必要な場合があります。変更終了時には、Save ボタンをクリックし、内容を保存します。

## ダイナミックコンテンツ配信の削除

ダイナミックコンテンツの削除が必要になった際には、Marketing ブロックに移動し、"Content Publishing" タブを開きます。削除したアイテムをダブルクリックし、"Remove" ボタンをクリックします。

![](../../../../assets/images/docs/004-remove-content-publishing.PNG)

コンテンツ配置の削除確認画面が表示されますので、Finish をクリックします。

## コンテンツ配信グループのフィルター

コンテンツ配信グループリストが多くなった場合、ブロックの右側のフィルターを利用することができます。

Marketing モジュールの "Content Publishing" タブに移動します。検索するアイテムの期間を定義します。(Active from, Active to又はActive from-toのいずれかを指定)

![](../../../../assets/images/docs/005-filtering-content-publishing.PNG)