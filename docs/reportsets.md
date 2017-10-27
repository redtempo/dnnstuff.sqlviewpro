# SQLView Pro Report Sets 

## Report Sets

The report sets screen is where you define your report sets (groups of
reports along with their parameters). This screen, along with the
connections screen, is global to all SQLView Pro modules.

![Edit Report Sets](images\EditReportSets.png)

### Report Sets Options

-   Del - deletes the report set. As a warning, this option will also
    delete all reports and parameters defined within the report set.
-   Edit – edits the report set allowing you to change report set
    parameters, add/remove reports and parameters.

### General Options

-   Close – returns you to the previous screen

## Edit Report Set

![Edit Report Set](images\EditReportSet.png)

### General Options

-   Update – saves the report set information and returns you to the
    previous screen
-   Cancel – returns you to the previous screen without saving changes
-   Apply – saves new report set information and activates the Add
    Report and Add Parameter options without leaving the screen

### Report Set Fields

-   Report Set Name – the name you wish to refer to this report set
-   Report Set Theme – the theme prefix that will be used to generate
    css selectors for the report set
-   Database Connection – the default connection used for this report
    set. For instance, you could set the default connection to
    ‘Northwind’ for this report set, then in each report and parameter
    select ‘Reportset Default’ for their database connections so you
    don’t have to specify each one individually. Then, if you need to
    change the database connection for the entire reportset, you change
    it once for the report set and don’t need to change it for the
    individual reports or parameters.
-   Report Set Header Text – the text which appears before the report
    set is rendered
-   Report Set Footer Text – the text which appears after the report set
    is rendered

## Reports

![Report Set Reports](images\EditReportSet_Reports.png)

### Reports Options

-   Del - deletes the report.
-   Edit – edits the report allowing you to change report parameters.

### General Options

-   Add Report – allows the addition of another report

## Parameters

![Report Set Parameters](images\EditReportSet_Parameters.png)

### Parameters Options

-   Del - deletes the parameter.
-   Edit – edits the parameter allowing you to change parameter
    settings.

### General Options

-   Add Parameter – allows the addition of another parameter

