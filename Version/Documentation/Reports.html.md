# SQLView Pro Reports 

## Edit Report

The following settings are common to all report types. 

![EditReport](images\EditReport.png)

### General Options

-   Update – saves the report information and returns you to the
    previous screen
-   Cancel – returns you to the previous screen without saving changes

### Common Report Fields

-   Report Type – select the type of report you wish to show. Currently
    this includes three options, Grid, XML/XSL and Template. SQLView Pro
    is designed to be extensible and additional parameter types will be
    provided as the product matures. Also, a Software Developer Kit
    (SDK) is planned for the future that will allows you to develop your
    own report types or parameter types. You are also welcome to contact
    DNNStuff for quote on customized report types and parameters.

-   Report Name – the name you wish to refer to this report

-   Report Theme – the theme prefix that will be used to generate css
    selectors for the report

-   Report Query – the query used to generate the report

-   Database Connection – the connection used for this report.

-   No Items Text - the text which appears when no data is returned from
    the query

-   Header Text – the text which appears before the report is rendered

-   Footer Text – the text which appears after the report is rendered

-	Page Title Text - if specified, this replaces the current page title

-	Meta Description/ Meta Tags - if specified this replaces the meta description value for the page  
	If the value contains a `<meta>` tag then it is treated as a whole and inserted into the `<head>` section

-   Drilldown from report - the report that this report will drilldown
    from

-   Drilldown from fieldname - the fieldname on the **Drilldown from
    report** that will link to this report
    -   In the example shown, when a user views the **Yearly Budgets**
        report and clicks on the **Year** field in any row, it will
        drilldown to the current report passing in the selected year. In
        this case you can see how that parameter is used with the
        [PARAMETER:Year] in the report query.



## Grid Report

![Grid Report Settings](images\EditReport_GridSettings.png)

### Grid Report Fields

-   Sorting
    -   Allow Sorting – enables sorting
    -   Order By – if sorting is enabled and you specify a sort order
        here (ex. LastName ASC), then the grid will initially be sorted
        this way

-   Paging
    -   Allow Paging – enables paging
    -   Rows Per Page – the number of rows displayed per page
    -   Navigation – the type of navigation used for paging, either
        Numeric or Prev/Next
    -   Prev/Next Wording – if Prev/Next navigation is selected, then
        these words will be used in place of Prev and Next
    -   Position – the position of the paging controls, either Top,
        Bottom or Both (top and bottom)

### Miscellaneous

-   Enable Excel Export – enables the user to export the contents of the
    data to Excel
-   Excel Export Button Caption – the caption shown for the Excel export
    button
-   Hide Column Headers - hides all column headers

## XSL Report

![Xsl Report Settings](images\EditReport_XSLSettings.png)

### XML/XSL Report Fields

-   Xsl File – the url path to the xsl file used to transform the xml
    data

### XML Data

The data generated from your ‘Report Query’ will be transformed into
valid XML and then passed through your xsl file which will result in the
html output for your report.

For instance, if you were using the following query to grab data from
the DNN tabs table,

```SELECT TabId, TabName FROM {objectQualifier}Tabs```

The way the XML is generated is as follows:

	<SQLData>

	<Table>
	   <TabId>7</TabId>\
	   <TabName>Host</TabName>\
	</Table>
	...

	<Table>
	   <TabId>17</TabId>\
	   <TabName>Portals</TabName>\
	</Table>
	</SQLData>

A valid xsl file that could be used to transform this into html would
look like the following:

	<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="SQLData"> <b>Report</b>

	<table cellpadding="1" cellspacing="1" border="1">
		<tr>
			<th>Tab Id</th>
			<th>Tab Name</th>
		</tr>
		<xsl:for-each select="Table">
		<tr>
			<td><xsl:value-of select="TabId"/></td>
			<td><xsl:value-of select="TabName"/></td>
		</tr>
		</xsl:for-each>
	</table>
	</xsl:template>
	</xsl:stylesheet>

## Template Report

A template report is a simplified version of the XML/XSL report type.
For many, this is a much more accessible report type because the syntax
is easier to understand and you can generate a very function report with
this in a relatively short period of time without having to learn xsl.

### Template Report Fields

-   Template Text – the text template that will be used to transform the
    data into html

### Template Data

The data generated from your ‘Report Query’ will be passed into the
template which will result in the html output for your report.

For instance, if you were using the following query to grab data from
the DNN tabs table,

```SELECT TabId, TabName FROM {objectQualifier}Tabs```

A valid template that could be used to transform this into html would
look like the following:

	<table cellpadding="1" cellspacing="1" border="1">
		<tr>
			<th>Tab Id</th>
			<th>Tab Name</th>
		</tr>
		[EACHROW]
		<tr>
			<td>[TabId]</td>
			<td>[TabName]</td>
		</tr>
		[/EACHROW]
	</table>


For more information about the available data tokens, please visit
[Data Tokens](pages/tokens/tokens_data)
