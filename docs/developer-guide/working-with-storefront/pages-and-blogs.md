---
title: Pages and Blogs
description: Virto Commerce implements a variation of so called NO-CMS approach (similar to Jekyll) for pages and blogs (and themes to some extent)
layout: docs
date: 2015-09-17T22:38:48.690Z
priority: 2
---
## Introduction

Virto Commerce implements a variation of so called NO-CMS approach (similar to <a href="http://jekyllrb.com/" rel="nofollow">Jekyll</a>) for pages and blogs (and themes to some extent). That means there is no hard dependency on where CMS content like pages, articles and templates are stored. They can be stored in database, github or local file system. This can be configured using CMS config. The content is downloaded from the remote location at runtime and is saved in the local website folder structure under App_Data folder from where local runtime generator picks it up and renders as html.

Pages and blog articles are created using templating engine that supports both <a href="http://daringfireball.net/projects/markdown/" rel="nofollow">markdown</a> and <a href="https://github.com/Shopify/liquid/wiki" rel="nofollow">liquid</a>. During runtime those templates are converted to html and persisted in memory until files are changed at which point content is regenerated.

## Front Matter

The front matter is where NO CMS approach starts to get really cool. Any file that contains a <a href="http://yaml.org/" rel="nofollow">YAML</a> front matter block will be processed as a special file. The front matter must be the first thing in the file and must take the form of valid YAML set between triple-dashed lines. Here is a basic example:

```
---
layout: post
title: Blogging Like a Hacker
---
```

Between these triple-dashed lines, you can set predefined variables (see below for a reference) or even create custom ones of your own. These variables will then be available to you to access using Liquid tags both further down in the file and also in any layouts or includes that the page or post in question relies on.

## Predefined Global Variables

There are a number of predefined global variables that you can set in the front matter of a page or post.

|Variable|Description|
|--------|-----------|
|layout|If set, this specifies the layout file to use. Use the layout file name without the file extension. Layout files must be placed in the layouts directory of the current theme.|
|permalink|If you need your processed blog post URLs to be something other than the site-wide style, then you can set this variable and it will be used as the final URL.|
|published|Set to false if you donвЂ™t want a specific post to show up when the site is generated.|
|template|By default equals to "page" and will use "page.liquid" to render current page.|
|tags|One or multiple tags can be added to a post. Tags can be specified as a <a href="http://en.wikipedia.org/wiki/YAML#Lists" rel="nofollow">YAML list</a> or a comma-separated string.|

## Custom Variables

Any variables in the front matter that are not predefined are mixed into the data that is sent to the Liquid templating engine during the conversion. For instance, if you set a title, you can use that in your layout to set the page title:

```
...
  <body>
    <h1>{{page.title}}</title>
...
```
