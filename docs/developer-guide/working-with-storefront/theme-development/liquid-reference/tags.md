---
title: Tags
description: The developer guide to Liquid tags
layout: docs
date: 2015-08-31T22:34:13.050Z
priority: 1
---
Liquid tags are the programming logic that tells templates what to do. Tags are wrapped in: {% raw %} {% %} {% endraw %}

Certain tags, such as **for** and **cycle** can take on parameters. Details for each parameter can be found in their respective sections.

Tags can be broken down to four categories:
* Control Flow Tags
* Iteration Tags
* Theme Tags
* Variable Tags

## Creating your own tags

To create a new tag, simply inherit from DotLiquid.Tag and register your tag with DotLiquid.Template inside EngineConfig.cs file.

```
public class Random : DotLiquid.Tag
{
	private int _max;
	public override void Initialize(string tagName, string markup, List<string> tokens)
	{
		base.Initialize(tagName, markup, tokens);
		_max = Convert.ToInt32(markup);
	}
	
	public override void Render(Context context, TextWriter result)
	{
		result.Write(new Random().Next(_max).ToString());
	}
}

Template.RegisterTag<Random>("random");
Template template = Template.Parse(" {% raw %}{% random 5 %}{% endraw %}");
template.Render(); // => "3"
```
