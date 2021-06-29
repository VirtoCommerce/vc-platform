---
title: Social buttons
description: Social buttons
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 8
---
## Introduction

This document describes how social buttons can be configured and customized for web application

## Configuration

To configure social buttons you need to open commerce manager and go to settings. Select Application/settings tab. The image below highlight system settings that are used for social buttons:

<img src="../../../../assets/images/settings.png" />

There are two settings for this. One is called ShareButtonsHtml which contains plain html for social share buttons. The sample above was generated using http://www.addThis.com API. So you can easily customize it and simply paste the generated html. To access the code in front end using mvc razor view engine you can use the code below:

```
@{
  var firstOrDefault = SettingsHelper.GetSettings("ShareButtonsHtml").FirstOrDefault();
  if (firstOrDefault != null)
  {
    @Html.Raw(firstOrDefault.ToString())
  }
}
```

The other setting is for Follow us links. It is a collection of settings containing serviceName:serviceId as can be seen in image.

<img src="../../../../assets/images/FollowLinks.png" />

The follow us links can be generated using addThis smart layers as shown in sample below: (More information for customization at <a href="http://support.addthis.com/" rel="nofollow">http://support.addthis.com/</a>)

```
<script type="text/javascript">
  // this will be called once all Smart Layers have been rendered
  var callback = function (smartlayer) {
    // remove all layers currently on the page
    smartlayer.destroy();

    var layers = {
      'theme': 'transparent',
      'follow': {
        'services': [],
        //'title': 'Follow',
        'postFollowTitle': 'Thanks for following!',
        'postFollowRecommendedMsg': 'Recommended for you',
        'mobile': true,
        'theme': 'transparent'
      }
    };
 
    @foreach (var follow in @SettingsHelper.GetSettings("FollowServices"))
    {
      var followStrSplit = follow.ToString().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
      <text>
      layers.follow.services.push({
        @if (followStrSplit.Length > 0)
        {
          @:'service': '@followStrSplit[0]'
        }
        @if (followStrSplit.Length > 1)
        {
          @:, 'id': '@followStrSplit[1]'
        }
        @if (followStrSplit.Length > 2)
        {
          @:, 'usertype': '@followStrSplit[2]'
        }
      });
      </text>
    }
             
    // now render Recommended and What's Next Layers
    window.addthis.layers(layers);
  };

  addthis.layers({
    'theme': 'transparent',
    'share': {}
  }, callback);
</script>
```
