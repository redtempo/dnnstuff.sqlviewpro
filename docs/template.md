# SQLViewPro Template Report 

## Drilldown

-   to enable drilldown on a field, surround the field with a drilldown
    tag where columnName is the name of the column being drilled down
-   the inner field does not have to match the name attribute of the
    drilldown tag, it can be a composite of other fields or static text

> ```<drilldown name="columnName">[columnName]</drilldown>```
