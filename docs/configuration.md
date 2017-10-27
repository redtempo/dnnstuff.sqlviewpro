# SQLView Pro Configuration 

SQLView Pro is a very easy module to configure once you understand the
underlying configuration that is required. This page and it's sub pages
will attempt to explain what each configuration option is but won't go
into details about the technology itself. For instance, topics such as
[html](http://en.wikipedia.org/wiki/Html), xsl, sql query language
(SQL) etc. will be mentioned and are required to use SQLView Pro but
it's not the aim of this documentation to teach them. Where possible,
I'll try and direct the reader to more suitable reference pages.

Module Settings
---------------

The module settings for SQLView Pro are quite simple. You merely select
the report set you want to show. Before you do this though, you’ll need
to define at least one report set with a single report. Connections and
parameters are optional.

![Module Settings](/images/ModuleSettings.png)

Connections
-----------

Connections are used as a way of determining where the data for reports,
parameter lists etc. are sourced from. There is a built in **Portal
Default** connection that maps to the current DotNetNuke database so you
will only need to create a connection if you with to grab data from a
database other than the DNN database. All connections should be
specified as OLEDB data sources. If you are unsure of the format of the
connection string for your particular database, take a look at
[www.connectionstrings.com](http://www.connectionstrings.com/) for
more information.

See [Connections](connections)

Report Sets
-----------

A report set is a grouping of reports and parameters. The simplest
report set contains just a single report and no parameters. You can
however, create more dynamic reports by specifying multiple reports and
parameters in a single report set. For instance if you wanted to do a
report that let the user select a country and then show two reports, a
grid of cities in that country and a graph that showed a breakdown of
sales by region in that country, you would have a reportset that
contained two reports and a single parameter.

See [Report Sets](reportsets)

Reports
-------

A report is a single displayable piece of information, such as a
listing, graph, table etc. that is driven by a SQL query.

See [Reports](reports)

Parameters
----------

Parameters are used to enable the user to filter the results of a
particular report set. For instance, you can create a parameter that
displays a drop down list of countries.

See [Parameters](parameters)
