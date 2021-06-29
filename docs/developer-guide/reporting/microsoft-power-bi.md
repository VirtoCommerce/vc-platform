---
title: Microsoft Power BI
description: The article describes reporting in Virto Commerce with Microsoft Power BI
layout: docs
date: 2015-10-21T12:50:50.507Z
priority: 1
---
Product owner <a href="https://powerbi.microsoft.com/" rel="nofollow">Official website</a>

## Power BI desktop

Power BI desktop is a tool for data analysis, report construction and dashboards composing. The application has an intuitive user-interface with many features and provides a connection to popular data sources.

To install Power BI desktop go to <a href="https://powerbi.microsoft.com/en-us/desktop" rel="nofollow">https://powerbi.microsoft.com/en-us/desktop</a>.

![](../../assets/images/docs/worddavf5055a891e93f0f08ed37d42b8bf8ee1.png)  
Power BI desktop

## Connect Power BI desktop to Virto Commerce database

VirtoCommerce uses Microsoft Sql Server as the database server. To connect to a data source click "Get Data".

![](../../assets/images/docs/worddav7c7da7390238dba039e2f65cc4f90a5b.png)  
Power BI desktop

To connect to VirtoCommerce database use a standard connector for Microsoft Sql Server. Connection requires to enter Sql server address, username and password in the connection setup window.   
![](../../assets/images/docs/worddav45a195ab6458d51b1d1898ae916c6e36.png)  
Connect to Microsoft SQL server

Setup VirtoCommerce database connection  
![](../../assets/images/docs/worddavc8913bc221cba585c843e9e9006c2c1c.png)  
Define data source

Once connected, you can use a data source for data analysis.  
![](../../assets/images/docs/worddav263b861867bf982f71525fad3021e89c.png)  
Data source

## Create a sample chart

Let's create an orders pie chart. Orders data is available in the OrderOperation table. To add pie click pie icon.  
![](../../assets/images/docs/worddav53604d1a0db671c97ad0a31064618665.png)  
Add pie chart

Add dimension by month by adding New column.  
![](../../assets/images/docs/worddav319aca1c1099a0be74a1bf29a2eb1688.png)  
Add calculated column

Define CreateMonth dimension.   
![](../../assets/images/docs/worddavf2427811e67d9fd5592ed1b8f5bc602e.png)  
Define calculated column

Drag CreateMonth to Legend and drag Sum to Values   
![](../../assets/images/docs/worddav9893eaf9a1073ca1ae32a0d10f50d37c.png)  
Sample pie chart

We have created a simple pie chart. Learn more about other features provided by Power BI desktop on the <a href="https://powerbi.microsoft.com" rel="nofollow">official website</a>.

## Register Power BI

Use online Power BI for collaborative work. To register it go to <a href="https://powerbi.microsoft.com" rel="nofollow">https://powerbi.microsoft.com</a> and follow instructions.

## Publish to Power BI

If you have access to Power BI you can publish Workbook created in the desktop application. In order to do that click "Home" from the main menu and click submenu "Publish".
