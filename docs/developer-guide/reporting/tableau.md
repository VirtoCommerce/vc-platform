---
title: Tableau
description: The article describes Virto Commerce reporting with Tableau
layout: docs
date: 2015-10-21T12:55:39.160Z
priority: 2
---
Product owner website: <a href="http://www.tableau.com/" rel="nofollow">Tableau Software</a>

## Tableau desktop

Tableau desktop is a tool for data analysis, reporting and dashboards composing. The application has an intuitive user-interface with many features and provides a connection to popular data sources.

To install Tableau desktop go to <a href="https://www.tableau.com/products/desktop" rel="nofollow">https://www.tableau.com/products/desktop</a>

![](../../assets/images/docs/worddavbf6f8eedfe0851d3ccd278b1ef996e3a.png)
Tableau desktop

## Connect Tableau desktop to Virto Commerce database.

VirtoCommerce uses Microsoft Sql Server as the database server. To connect to a data source, click "Connect to Data".

![](../../assets/images/docs/worddavb057ad38186ca11341d9fbefa19d9188.png)  
Tableau desktop
  
To connect to VirtoCommerce database use a standard connector for Microsoft Sql Server. Connection requires to enter Sql server address, username and password in the setup window.

![](../../assets/images/docs/worddavb171d7b5fd3e20f277bba54eb5ef6adc.png)  
Connect to Microsoft SQL server
  
Setup Virto Commerce database connection

![](../../assets/images/docs/worddavfabc50127e4932d3eb4ddac329042caf.png)  
Virto Commerce database

The Virto Commerce database is connected. The system is ready to get data and analyze by provided definition.

## Create a sample chart

Let's create an orders pie chart. Orders data is available in the OrderOperation table. Add data required for analysis:

Drag table OrderOperation to top placeholder.Set Extract data.Click Update data.

![](../../assets/images/docs/worddavabf6719414dc402027068f0c13b59095.png)  
Add data for analysis
  
To design chart go to Sheet tab.

![](../../assets/images/docs/worddav052bf27f28438c367937344bf3858133.png)  
Switch to Sheet
  
To define dimension drag CreatedDate field to Color widget. Default dimension is year. It is possible to change the dimension in the context menu.

![](../../assets/images/docs/worddav0cf07b4312ca7be3053315cbbaa4537e.png)  
Define dimensions

To define measure drag Sum to the pie chart

![](../../assets/images/docs/worddav17cc61bbe9d36f1211c3a590ae4de621.png)  
Define measure

We have created a simple pie chart. Learn more about other features provided by Tableau desktop at the <a href="http://www.tableau.com/" rel="nofollow">official site</a>.

![](../../assets/images/docs/worddav259f315ec7039d3516f3cb260c76cb18.png)  
Sample pie chart

## Tableau online

Tableau Online or Tableau Server are available for collaborative work. Go to Tableau online to register <a href="https://www.tableau.com/products/cloud-bi" rel="nofollow">https://www.tableau.com/products/cloud-bi</a>В and follow instructions.

## Publishing on Tableau online dashboard

If you have access to Tableau Online or Tableau Server you can publish Workbook created in the Tableau desktop application. To do this, click Server from the main menu and select Publish workbook from submenu in the dropdown. In the opened window enter Tableau Online or Tableau Server address and click Connect.

![](../../assets/images/docs/worddav7d06b38e4d53d69b741d9416f283d548.png)  
Connect to Tableau online

In opened window set Project, Workbook Name and Description values and click Publish. You can configure permissions here or edit them later.

![](../../assets/images/docs/worddav6126c53f9ea6074159b1fe107659b60d.png)  
Sample chart published

After the publication Workbook will be available online.

## Embedding Tableau chart to Virto Commerce admin

Reports and graphs that are published on Tableau Online or Tableau Server can be embedded to a Website, Wordpress, Excel, SharePoint etc. You just need to add a script to a web page. To generate a script of Tableau Online, click Share.   
  
![](../../assets/images/docs/worddav8c9c836e13eb131ee8bacaa8dabbd6e3.png)  
Tableau online

In the "Share View" view you can set Display options and get the generated script.

![](../../assets/images/docs/worddavb0e9b556af15bb336af8f37c66aad148.png)  
Embed script

## Conclusion

In the document we showed how easy it is to setup Tableau to connect to VirtoCommerce and compose charts and reports based on the accumulated data. The resulting charts and reports can be shared online or emebedded to the VirtoCommerce admin dashboard.
