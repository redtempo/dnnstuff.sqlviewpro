```
title: Decoding Html in a template report 
layout: page
tags: ['intro','page']
```

The current SQLView Pro data token library doesn't include the facility
to decode html data fields but here is a technique you can use to do
this. By the way, this technique assumes you have the jQuery library
ready to go on your page which is probably the case for most DNN sites
these days.

In your template report, add a small piece of script at the top that
looks like this:

``` js
<script type="text/javascript">
	function decode(val) {
		document.write(\$('<div/>').html(val).text());
	}
</script>
```

Then in the rest of your template, when ever you have an html field you
want to decode, just use the function call like so:

``` js
<script>
	decode('[MYFIELDNAME]');
</script>
```

For example, if I had a field named **Description** in my db query, I
could use this template.

``` js
<script type="text/javascript">
	function decode(val) {
		document.write(\$('<div/>').html(val).text());
	}
</script>
[EACHROW]The description is :

<script>
decode('[Description]');

</script>
[/EACHROW]
```

