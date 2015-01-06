# SQLView Pro Parameters 

## Edit Parameter

The following settings are common to all parameter types.

![Edit Parameter](images\EditParameter.png)

### General Options

-   Update – saves the parameter information and returns you to the
    previous screen
-   Cancel – returns you to the previous screen without saving changes

### Common Parameter Fields

-   Parameter Type – select the type of parameter you wish to show.
    Currently this includes seven options; Calendar, Checkbox,
    CheckBoxList, DropDownList, ListBox, RadioButtonList, and TextBox.
    -   SQLView Pro is designed to be extensible and additional
        parameter types will be provided as the product matures. Also, a
        Software Developer Kit (SDK) is planned for the future that will
        allows you to develop your own report types or parameter types.
        You are also welcome to contact DNNStuff for quote on customized
        report types and parameters.

-   Parameter Name – the name you wish to refer to this report
-   Parameter Caption – the caption which displays next to the parameter
-   Parameter Default – the value to initially set the parameter to
    (optional)

## Parameter Types

### Calendar

![Calendar](images\Parameter_Calendar.png)

### Checkbox

![Checkbox](images\Parameter_Checkbox.png)

### CheckBoxList

![CheckBoxList](images\Parameter_CheckBoxList.png)

### DropDownList

![DropDownList](images\Parameter_DropDownList.png)

### ListBox

![ListBox](images\Parameter_ListBox.png)

### RadioButtonList

![RadioButtonList](images\Parameter_RadioButtonList.png)

### TextBox

![TextBox](images\Parameter_TextBox.png)

## List Values

All parameter types that involve presenting some kind of list data use a
common way of specifying the list data. You can add list data manually
by specifying each list items value and name pair, or you can use a sql
query to generate the list data. If you use both options, the list data
from each method will be appended to each other before display. This
dual method is helpful if you want to drive the selection list from a
table but also provide a catchall selection to specify an **all items**
choice.

### Parameter List

If you use the manual way you need to specify each value/name pair on
it's own line separating the value and name with a pipe character |
(Ascii code 124). In the example screen below I've included a value/name
pair (-1|All) in my selection list. If this option is selected, the
value of -1 will be passed to the report for this particular parameter
example.

### Parameter Query

If you want to drive your parameter selection list with a database query
you'll need to pick the proper database connection to use and then
specify your query. The query must return a table of values that has a
minimum of two fields. The first field will be used as the selection
value (ie. the value passed to the report if selected) and the second
field will be used as the selection name. In the example screen below
I've included a parameter query that results in a list of departments.
The example is from the included sample report named **Sample -
Employees by Department**

![List Values](images\Parameter_ListValues.png)

The sample above results in the following dropdown for the parameter

![List Values Sample Results](images\Parameter_ListValues_Sample.png)

### Parameter List Defaults

Parameter list items can be preselected by specifying either the value
or name. Multiple values can be selected by specifying values or names
or a combination of both in a comma separated list.

If the list contains the following items:

1 - Apple 2 - Pear 3 - Orange 4 - Bananna

then you can select Orange by specifying either 3 or Orange in the
default field or you could select Orange and Pear in any of the following ways:

2,3

2,Orange 

Pear,Orange 

Pear,3
