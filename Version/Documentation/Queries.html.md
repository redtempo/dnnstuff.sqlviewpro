# SQLView Pro Queries

All data that is displayed in a report must first come from a query. A
query is simply a SQL statement that the data provider uses to grab the
data from the database. Teaching you SQL is beyond the scope of the
SQLView Pro documentation but there are many good resources to guide you
along. One I found recently that provides an interactive online
experience is [sqlzoo.net](http://www.sqlzoo.net)

## Query Tokens

Query tokens are special keywords surrounded with square brackets that
are substituted at query time with dynamic data. There are various forms
of query tokens depending on the context of the data.

## databaseOwner and objectQualifier

There are two DNN specific tokens available which DNN module developers
and power users are sure to recognize. The two tokens I'm referring to
are the databaseOwner and objectQualifier tokens. These two pieces of
information are specific to your DNN installation and are necessary if
you want to create truly portable queries across DNN installations. In
the majority of cases the databaseOwner equates to **dbo** and the
objectQualifer is an empty string but not always. For this reason it's
important that when referring to intrinsic DNN tables or tables related
to 3rd party DNN modules you include both of these tokens before table,
view and stored procedure names.

The format of these tokens is the same as in the standard DNN sql page:

> **{databaseOwner}** and **{objectQualifier}**
or you can use the shorted forms, **{dO}** and **{oQ}**

### Example

If you were going to select all users from the DNN database for a query
    you would use

	SELECT \* FROM {databaseOwner}{objectQualifier}User

or the shorted form

	SELECT \* FROM {dO}{oQ}User


## Parameter Tokens

If you are using parameters to drive your report, you'll undoubtedly
want to include the corresponding parameter token for that parameter
into your query to filter your data or there is no point in having the
parameter in the first place. To do this you'll need a special parameter
token.

The format of the parameter token is as follows:

> **[PARAMETER:{parameter name}]** where {parameter name} is the name of
your parameter

### Example

If I have a parameter named **Department** then I would use	**[PARAMETER:Department]**

    SELECT * FROM MyTable WHERE Department = '[PARAMETER:Department]'

## Handling Multiple Values

If you are using a checkbox list parameter, or the listbox in
multiselect mode then the parameter value will result in a comma
separated list of selected values. You will not be able to use a
**WHERE** clause in your query for this type of data so you must instead
use an **IN** clause. If no value is selected then an empty string is
sent.

### Example

	SELECT * FROM MyTable WHERE Department IN ([PARAMETER:Department])

Notice the lack of single quotes around the parameter value in this case
since they are automatically added by the module.

## Querystring Tokens

Querystring tokens are useful if you wish to drive your report based on
the value of one or more query strings of the current page.

The format of the querystring token is as follows:

> **[QUERYSTRING:{querystring name}]** where {querystring name} is the
query string key. You can also use the shortform [QS:{querystring name}]

### Example

If I have a query string key named **ShowAll** then I would use **[QUERYSTRING:ShowAll]** in my query.

Let's assume the page url is ```http://www.something.com/default.aspx?ShowAll=1``` and my query is

	SELECT * FROM Employees WHERE State = 'NY' OR [QUERYSTRING:ShowAll]

The resulting query will be

	SELECT * FROM Employees WHERE State = 'NY' OR 1

## Server Variable Tokens

Server variable tokens are useful if you wish to drive your report based
on the value of one or more available server variables.

The format of the server variable token is as follows:

> **[SERVERVAR:{key}]** where {key} is the server variable name. You can
also use the shortform SV:{key}

## Form Variable Tokens

Form tokens allow you to use any form values posted to the report.

The format of the form variable token is as follows:

> **[FORMVAR:{key}]** where {key} is the form variable name. You can also
use the shortform **[FV:{key}]**

Warning: It is recommended that every time you use
querystring parameters or any parameters that you cannot fully
authenticate the data for, you create a stored procedure and pass the
query string parameters into the stored procedure in order to combat
against sql inject attacks.
