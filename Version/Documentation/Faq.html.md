```
title: SQLViewPro - Faq
layout: page
tags: ['faq','page']
topNav: false
topNavOrder: 1

```

## Showing Run/Back as Buttons

Each command including Run/Back/Export etc. are all rendered as anchor tags or ```<a />``` tags.

If you would rather they look like buttons you can easily achieve this by defining your own styling to a special class that is attached to each anchor tag, named **SQLViewProButton**

To override this, it is best to do so in your portal.css by either editing it directly in /portals/[yourportalid]/portal.css or going into Admin|Site Settings and choosing the Stylesheet Editor tab.

At the bottom of the css stylesheet, just add a new style such as the following. Feel free to change the colors, sizing etc. to suit your needs.

	.SQLViewProButton {
		display:inline-block;
		background:rgba(240,240,240,1);
		min-width: 50px;
		text-align: center;
		-moz-border-radius: 2px;
		-webkit-border-radius: 2px;
		border-radius: 2px;
		-moz-box-shadow: 0px 0px 2px rgba(0,0,0,0.4);
		-webkit-box-shadow: 0px 0px 2px rgba(0,0,0,0.4);
		box-shadow: 0px 0px 2px rgba(0,0,0,0.4);
		color: rgba(0,0,0,0.9);
		-webkit-text-shadow: 1px 1px 0px rgba(255,255,255,0.8);
		text-shadow: 1px 1px 0px rgba(255,255,255,0.8);
		border: 1px solid rgba(0,0,0,0.5);
		padding: 5px 5px 5px 5px;
		margin-top: 5px;
	}

