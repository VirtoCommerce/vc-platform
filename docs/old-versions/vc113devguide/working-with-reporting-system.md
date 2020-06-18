---
title: Working with Reporting system
description: Working with Reporting system
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 9
---
## Introduction

Reporting Module works directly with Sql server reporting service (SSRS) reports files (RDL format - <a href="http://technet.microsoft.com/en-us/library/ms155062.aspx" rel="nofollow">http://technet.microsoft.com/en-us/library/ms155062.aspx</a>). That means reports can be created with SSRS Reports Builder and uploaded directly to VC manager. However it is not using SSRS for generating reports data. VC manager (application works on client) does not access database directly, but usesВ VC services which are responsible for preparing datasets defined in report.

## Creating report file

VC supports RDL files created with MS SQL 2012 ReportBuilder v.3.0 (<a href="http://www.microsoft.com/en-us/download/details.aspx?id=29072" rel="nofollow">http://www.microsoft.com/en-us/download/details.aspx?id=29072</a>)

All main features of RDL is supported, like page layout, report and datasets parameters, values expression.

However there are few things, that VC treats differently than SSRS:

1. Data source definition. VC uses data source name to resolve its connection string to database by looking to its own settings: {FrontEnd root folder}/App_Data/Virto/Configuration/connectionStrings.local.config. If such exists, then VC will use own defined database. If not then connection defined in report will be used. So it is similar to SSRS shared connection feature which is not supported by VC.
2. Report parameters. Before generating report, VC like SSRS will ask to fill defined parameters with appropriate values. VC has own realization of UI which is very similar to SSRS. Still not all feature are implemented yet, like multi value selection and providing available values from datasets.
