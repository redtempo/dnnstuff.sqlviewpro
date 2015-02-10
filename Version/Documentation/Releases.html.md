# SQLView Pro Release History 

```Minimum configuration DNN 6.0.3+ / DNN 7+ / .NET 3.5```

<!-- insert-newversion -->

## 04.00.15

10/Feb/2015

* Fixes
	* Fixed error that occured in MySql connections with ORDER BY




## 04.00.14

02/May/2014

* Fixes
	* Fusion chart settings were causing another object reference error
	* Sql install script error in 04.00.07 when using non standard database owner


## 04.00.13

03/Apr/2014

* Fixes
	* Fusion chart settings were causing an object reference error
* Enhancements
	* Changed MetaDescription field to 4000 characters from 500

## 04.00.12

03/Mar/2014

* Enhancements
	* Changed how the report meta description field operates. If the field contains a `<meta>` tag then instead of the description meta tag being altered, all the text in the field will be inserted into the `<head>`  
The intended use here is to allow alternate `<meta>` tags for things like twitter card and facebook open graph


## 04.00.11

03/Mar/2014

* Fixes
	* Fixed issue where single selections in a multivalued parameter were not formatted correctly

## 04.00.10

25/Feb/2014

* Enhancements
	* Added two new tokens to grid report, [SORTEXPRESSION] and [SORTDIRECTION].
	> If these appear in your SQL clause then automatic sort clause generation will not happen.  
	The intention here is to pass these to a stored proc to allow sorting there instead.

* Fixes
	* Fixed issue where tables in dataset using connection other than portal default where named incorrectly (ie. not Table, Table1, ..., TableN)

## 04.00.09

20/Jan/2014

* Fixes

	* Set timeout of SSRS (SQL Reporting Services Report) viewer to infinity

	* Removed unnecessary stored procedure (DNNStuff_SqlViewPro_DeleteConnectionOK)

	* Increased timeout on full screen dialog

## 04.00.08

9/Oct/2013

-   Updates
    -   Updated Excel template to use Flexcel 6.1.0.0

## 04.00.06

30/Jul/2013

-   Additions
    -   Added Excel Export Position to Grid Report settings to allow
        placement either top or bottom

## 04.00.05

10/Jul/2013

-   Additions
    -   Added a new report level field named 'META Description' which
        will populate the page META description tag if specified

-   Bug Fixes
    -   XLSTemplate settings was not maintaining state when one of the
        xls template folders changed

## 04.00.03

3/Apr/2013

-   Additions
    -   Added two new report level tokens: [REPORTTYPE] and
        [REPORTTYPENAME] (ex. FUSIONCHART, Chart (Fusion))
        -   REPORTTYPE will be a unique text identifier of the report
            type, REPORTTYPENAME will be a user friendly name (same as
            the report type dropdown)
        -   To get a list of all values run SELECT \* FROM
            {databaseOwner}{objectQualifier}DNNStuff\_SqlViewPro\_ReportType
            from the Host|SQL menu

    -   Added a new rendermode on reportset advanced section to show
        report in a new window

-   Fixes
    -   Exported reports missing cachetimeout setting could not be
        imported, also affected Browse Reports

-   Enhancements
    -   Changed reportset fullscreen setting to rendermode
    -   Chart properties can now use available tokens

## 04.00.01

13/Feb/2013

-   Fixes
    -   Fixed an 'object reference' error when adding new parameters

## 04.00.00

11/Feb/2013

-   Important
    -   This release is the first for only DNN6 (6.0.3+) and DNN7
        (requires .NET 3.5+)
    -   This release includes a fair number of refactorings to the code
        base and although I've done everything I can think of to reduce
        the possibility of introducing new bugs, please go through a
        sample of your reports and check to make sure

-   Additions
    -   Added new SSRS Report Type (SQL Server Reporting Services)
        -   Supports local and server based SQL Server Reporting
            Services Reports

    -   Added new Excel Template Report Type
        -   Dumps data into a specified page while maintaining the rest
            of the spreadsheet so you can create predefined excel pivot
            tables

    -   Added new Mobiscroll Date Time Parameter Type (Date Time picker
        for mobile reports)

-   Fixes
    -   Fixed issue when dropdown list parameter was referenced in
        different order than in the parameter list

-   Enhancements
    -   Template report type now supports drilldown to other reports
        -   Surround drilldown field with drilldown tag, details here
            [SQLViewPro\_Template|Template report]

    -   Ability to specify a cache timeout for all queries, defaults to
        60 seconds
    -   If cached query is accessed within the timeout, the cached query
        results will be used and the timer is reset back to the timeout
        value (sliding expiration)

```Minimum configuration DNN 5.2.3+ / DNN 6+ / .NET 3.5```

## 03.08.07

5/Nov/2012

-   Additions
    -   AutoRun - this new feature found in the Reportset | Advanced
        section enables the report to run automatically using the
        default parameter values

## 03.08.06

9/Oct/2012

-   Fixes
    -   Paged drilldown grid was providing wrong row of data to next
        report
    -   Chart settings showPercentageInLabel not saving or working

-   Enhancements
    -   Queries are intelligently cached for better performance

## 03.08.05

2/Aug/2012

-   Updates
    -   For any parameters that can result in multiple values passed to
        the report, if no values are selected an empty string is passed
        ie. ''

-   Additions
    -   Added Multiselect option to listbox parameter
    -   Added ability to change the number of rows displayed for a
        listbox parameter
    -   Added support for Form values access
        -   Use [FORMVAR:var] or [FV:var] for Form values access

## 03.08.04

1/Aug/2012

-   Updates
    -   Updated to support Azure deployment

## 03.08.03

7/Jun/2012

-   Additions
    -   Added GeoLocation parameter type - this will provide latitude
        and longitude of the user in latitude,longitude format
        -   Alternately you can access the values separately using the
            subvalue format, [PARAMETER:MyLocation:Latitude] or
            [PARAMETER:MyLocation:Longitude]

-   Fixes
    -   Default report type is now GRID
    -   Default parameter type is now TEXTBOX
    -   Fixed FusionChart data values and labels by XmlEncoding entities
        properly - single quote in labels were causing issues
    -   Fixed FusionChart report footer - wasn't visible

## 03.08.02

11/Apr/2012

-   Updates
    -   Modifications to all settings screens to better conform to new
        DNN6 standards

## 03.08.01

26/Mar/2012

-   Additions
    -   Calendar parameter includes a server date format string - used
        to convert localized dates to a standard database date format

-   Updates
    -   Parameter settings screens are better localized with full help

-   Bug fixes
    -   Drilldown reports now only show if their specific column is
        selected

## 03.08.00

22/Mar/2012

-   Minimum supported install is now DNN 5.2.3 and DNN 6.0.0, both have
    to be .NET Framework 3.5 minimum
-   Change to excel exporting - now exports are performed with a hidden
    iframe
-   Fix report footer render error
-   Update to license
-   Update to install for DNN5/DNN6

```Minimum configuration DNN 5.1.0+ / DNN 6+```

## 03.07.03

18/Jan/2012

-   Changed data retrieval timeout to infinity

## 03.07.02

06/Jan/2012

-   Fixed a bug in text parameters row, column property handling

## 03.07.01

15/Dec/2011

-   Added a new report setting named 'Page Title'
    -   If 'Page Title' is specified, the browser page title will be
        replaced with this value.
    -   You can use tokens within this new setting

## 03.06.09

13/Dec/2011

-   Added a new token [FULLSCREENURL] for reports which results in the
    url to the report without any chrome
    -   It's effectively the same url that is sent to the jQuery
        Fancybox plugin when showing reports in the full screen popup
    -   Useful if you want to send someone the url to the report by
        itself with selected parameters or to a printing application

-   Fixed a grid paging bug
-   Fixed an out of memory issue on large exports

## 03.06.08

30/Nov/2011

-   Fixed bug when single or double quotes in chart titles or other
    properties
-   Fixed bug were some chart properties were not being saved the first
    time
-   Added row and column sizing for textbox parameter
-   Added Auto Run for dropdown parameters

## 03.06.07

15/Nov/2011

-   Fixed a bug in Fusion Chart custom color set
-   Fixed a bug in Fusion Chart rendering when 'Always Show Parameters'
    option was selected
-   Fixed a bug when parameter not included in Custom Parameters layout
    - now assigned with empty value

## 03.06.06

03/Nov/2011

-   Fixed bug in full screen reports - Only presents itself in later
    DNN5 implementations
-   Fixed bug in checkboxlist and listbox parameters - Single quotes
    around list items were getting doubled up

## 03.06.04

17/Aug/2011

-   Fixed bug with drilldown parameters passing wrong row of data when
    multiple pages

## 03.06.02

09/Aug/2011

-   Fixed DNN6 compatibility issues

## 03.06.01

26/Jul/2011

-   Fixed bug with [PAGEURL] when using custom pararmeter layouts
-   Fixed bug with full screen module permissions
-   Added token replacement into list-type parameter option queries
-   Fixed single quote bug in queries .. single quotes now being doubled
    up before passing to query
-   Fixed bug with report ordering

## 03.06.00

21/Jul/2011

-   Added Fusion Chart report type
-   Fixed unicode issue with report command and report options

## 03.05.02

20/Apr/2011

-   Fixed Full Screen security
-   Added new token [PAGEURL] that evaluates to the current page. Useful
    for full screen report links

## 03.05.01

14/Apr/2011

-   First DNN5 only release
-   Added Excel report type - exports directly to excel without showing
    a report surface
-   Changed excel report filename to reflect the name of the report it
    is coming from
-   Replaced calendar parameter caption with image
-   Added [ACTIONBUTTON] token to custom parameter layout to render the
    action button (Run button)
-   Added new reportset setting, Always Show Parameters. Parameters will
    not automatically hide after running the report.
-   Added new reportset feature, Full Screen. Clicking on the run link
    will show report up in a fullscreen popup. ***(Requires jQuery
    1.3+)***

```Minimum configuration DNN 4.6.2+```

## 03.04.00

25/Feb/2011

-   Fixed bug in Grid Report that sometimes required two clicks to
    drilldown to next report
-   Added tokens for server variables. You can use either
    [SERVERVAR:key] or [SV:key] where key is the name of the server
    variable
-   Added new css class to div that surrounds the parameter, formatted
    as skin\_Parameter (ie. Default\_Parameter if the skin name is
    Default)
    -   this new css class can be used to target styles for anything
        rendered as a paramter, input, select, etc.

-   Added None option to skin settings which emits no extra css style
    information
-   Modified ReportSet/Report - Header/Footer fields can each be up to
    4000 characters now
-   Added [SQLViewPro\_ParameterLayout|custom parameter layout] to
    ReportSet (located on the Advanced tab in ReportSet settings)
    -   this new feature allows you to specify a custom layout for your
        parameters instead of having the module decide
    -   tokens to represent parameters are formatted as
        [{ParameterName}], [{ParameterName}\_Prompt] and
        [{ParameterName}\_Caption] (where {ParameterName} is the name of
        your parameter)
    -   for example you can use this feature to align your captions to
        the right or left of the prompts instead of having them on top

-   Added new repository sample (SampleParameterLayout) showing the new
    Parameter Layout features
-   Added copy report - creates a copy of an existing report
-   Added copy parameter - creates a copy of an existing parameter

## 03.03.01

18/Jan/2011

-   Added escaped strings for [ and ] with 0x5B and 0x5D respectively

## 03.03.00

19/Oct/2010

-   Added Run Caption to Reportset - allows you to change the 'Run' link
    to whatever you wish
-   Added Back Caption to Reportset - allows you to change the '\< Back'
    link to whatever you wish
-   Added Empty Parameter - empty parameter doens't prompt for anything,
    which allows you to implement a run on demand report

## 03.02.02

16/Sep/2010

-   Fixed a problem with excel exporting international characters - now
    supports UFT-8

## 03.02.01

08/Jun/2010

-   Updated - drilldown to pass the entire row to the next report as
    parameters
    -   Example: If initial report has 3 columns (ID, Name, Sales) then
        [PARAMETER:ID], [PARAMETER:NAME], [PARAMETER:SALES] are
        available to the drill down report

-   Added - Hide Columns settings to Grid Report
    -   Enter a comma seperated list of columns to hide. Useful if you
        require an ID field in a drilldown, but don't want to show it on
        the report

-   Fixed - querystring parameters weren't being replaced properly
-   Added - QueryString parameters can be called with shortform
    [QS:querystringkey]

## 03.02.00

20/May/2010

-   Minimum DNN dependency is now 4.6.2
-   Fixed problem with excel exports in IE
-   Refactored token, parameter replacement
-   Token replacement now supports [\#EVAL VALUE="expression"]
    -   ie. can do things like [\#EVAL
        VALUE="String.Format("{0:c}",[MyColumn]"] to format MyColumn as
        currency
    -   See [Tokens\_Eval|Eval Tokens]

-   Default values can now be set on any list parameters
    -   default value can be either the value or text value of the list
        item
    -   to select more than one default value for CheckBoxList,
        RadioButtonList used a comma delimited list of values

-   Added [REPORTNAME] token
-   Added parameter replacement, logic replacement to template body
-   Added overwrite warning when importing from repository

## 03.01.08

13/Apr/2010

-   Header/Footer areas on ReportSet and Reports are now rendered as
    html not merely text
    -   This means you can use html such as links, bolding etc. inside
        header/footer areas

-   Header/Footer areas on ReportSet now include token parsing for
    standard DNN tokens (same as report header/footer)
-   Changed token parsing to handle nested tokens (tokens within tokens)
    -   For example, you can do something like this [TOKEN1
        LENGTH="[TOKEN2]"]

-   Added \#CALL intrinsic to token handling. This allows you to
    reflectively call functions from another assembly
    -   Syntax - [\#CALL ASSEMBLY="assemblyname" CLASS="classname"
        METHOD="methodname" VALUES="comma separated values"]
    -   Example - [\#CALL ASSEMBLY="MyFunctions" CLASS="Addition"
        METHOD="AddNumbers" VALUES="1,2"]
    -   This will load MyFunctions.dll (from the bin folder), create an
        instance for the Addition class, and invoke the AddNumbers
        function passing 1 and 2 as parameters. See wiki for more info.
    -   This \#CALL feature will be most useful on template reports
    -   See [Tokens\_Call|Call Tokens]

-   Removed the query test when reports are saved. I was having some
    reports of false positives so I'm going to have to rethink how I do the query testing in the future

-   Changed connection string test when a connection is saved

## 03.01.07

29/Mar/2010

-   Updated - Removed need to do an apply when creating new report sets
    -   Previously when starting a new report set, you had to name the
        report set and apply the changes before you could add reports
        and parameters. This condition has been removed.

-   Added - No Items text for reports shows if no data returned from
    query

## 03.01.06

28/Jan/2010

-   Bug fix - Parameter resources not deployed correctly
-   Bug fix - Object reference not set to an instance of an object error
    when saving parameters using connections
-   Bug fix - Parameter lists from queries not intializing if only a
    single column returned

## 03.01.05

17/Jan/2010

-   Initial Release

