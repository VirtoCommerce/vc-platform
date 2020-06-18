---
title: インポート
description: インポート
layout: docs
date: 2016-06-03T10:21:23.333Z
priority: 4
---
## はじめに

カタログデータは、手動で作成できますが、カタログが大量のデータで構成されている場合は、手動でのデータ作成が不適切かつ非合理的になります。Virto Commerce Manager は、ストア管理者がCSV(comma-separated value)ファイルからインポートマネージャーツールを利用し、カタログデータをインポートする機能を提供しています。

データをインポートする前に、データのインポート方法を説明(項目とデータのマップイング方法)する為のカタログインポートジョブを作成する必要があります。

## カテゴリー、価格リスト、アイテムのインポート

カテゴリー・価格リスト・アイテムのインポートは、インポートジョブを利用するとかんたんに行うことができます。"Catalogs" ブロックの Import タブに移動します。以前に作成されたインポートジョブが表示されています。

* Category Import Job
* Price Import Job
* Product Import Job

<img src="../../../../assets/images/docs/017-list-of-import-jobs.PNG" />

リストから必要なインポートジョブを選択し、"Run" ボタンをクリックします。CSVファイルを割り当てて "OK" ボタンをクリックします。

<img src="../../../../assets/images/docs/018-run-import-job.PNG" />

インポートは、インポートデータのサイズにより数分ほどかかります。

インポートジョブを表示・編集するには、リストをダブルクリックします。修正が必要な場合は、内容を修正し、"OK" ボタンをクリックし修正内容を保存します。

<img src="../../../../assets/images/docs/image2013-10-24_10_51_41.png" />

## インポートジョブの作成

Commerce Manager の Catalogs ブロックの Inport タブを開き、インポートジョブを作成する為に "Add" ボタンをクリックします。

<img src="../../../../assets/images/docs/012-add-button.PNG" />

"Create Import Job definition" ウィザードが表示されます。最初のステップは、次の項目を入力します。

* **Importing item data type** - インポートデータのタイプを選択します。(製品、カテゴリー、パッケージなど)
* **Catalog** - インポートするデータのカタログを選択
* **Property set** - インポートデータのプロパティセットを選択
* **CSV Template File** - 以前に作成した CSV テンプレートのパスを定義
* **Name** - インポートジョブのタイトル(デフォルトでは、このプロパティを入力する必要がなく、選択されたデータタイプ・カタログ・ぽうろパティ値により自動的に入力されます）
* **Column delimiter** - 行区切り文字を選択します。Auto は、デフォルトで設定されており、ファイルを選択すると、システムは区切り文字を推測します。
* **Errors allowed during import** - インポート処理中に発生するエラー最大数を入力

アスタリスクでマークされた項目を全て入力後、"Next" ボタンをクリックします。

<img src="../../../../assets/images/docs/image2013-10-24_10_52_31.png" />

## インポートジョブの実行

"Catalogs" ブロックのインポートジョブを開きます。作成されたインポートジョブリストが表示されています。

<img src="../../../../assets/images/docs/014-import-jobs.PNG" />

実行するインポートジョブを選択し、"Run" ボタンをクリックします。

<img src="../../../../assets/images/docs/015-import-jobs.png" />

CSVファイルを選択し、"OK"ボタンクリックしインポート処理を開始します。

<img src="../../../../assets/images/docs/016-select-csv-file.PNG" />