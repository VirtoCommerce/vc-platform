---
title: Google Analytics
description: Google Analytics
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 3
---
## Introduction

This document describes how Google Analytics field can be configured and customized for web application

## Configuration

To configure Google Analytics you need to open commerce manager and go to settings. Select Application/settings tab. The image below highlight system setting that is used for Google Analytics:

<img src="../../../../assets/images/Settings.png" />

To create your google analytics account and more information can be found here <a href="http://www.google.com/analytics/learn/" rel="nofollow">http://www.google.com/analytics/learn/</a>

When you have created an account and have registered your service with google analytics you should obtain a piece of javascript code which has to be pasted into the GoogleAnalytics setting value. A sample is shown below:

```
<script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
    (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
    m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-42754882-1', 'cloudapp.net');
  ga('send', 'pageview');
</script>
```

To add google analytics script to your web pages you could use code similar to this:

```
@{
  var firstOrDefault = SettingsHelper.GetSettings("GoogleAnalytics").FirstOrDefault();
  if (firstOrDefault != null)
  {
    @Html.Raw(firstOrDefault.ToString())
  }
}
```
