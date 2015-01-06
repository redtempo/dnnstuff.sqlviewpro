# SQLViewPro Parameter Layout 

## What is Parameter Layout

By default, SQLView Pro renders any parameters in standard format namely
top to bottom with captions to the left of the parameter input area.
Parameter Layout allow you to override this default format with any
custom format you wish to use. You merely build your own layout template
using tokens to represent where you want the parameter captions and
parameter prompts (inputs) to go. Parameter Layout can be found on the
Report Set | Advanced tab.

## Parameter Layouts Tokens

Each parameter can be identified as a token in the layout template as
one or more of the following tokens:

-   **[ParameterName]**
    -   inserts the full parameter caption and prompt for the parameter
        named *ParameterName*

-   **[ParameterName\_Prompt]**
    -   inserts the parameter prompt for the parameter named
        *ParameterName*

-   **[ParameterName\_Caption]**
    -   inserts the parameter caption for the parameter named
        *ParameterName*

-   **[ACTIONBUTTON]**
    -   inserts the Run/Back button

## Parameter Layout Format

The following is a sample of a valid parameter layout format and can be
found in the ParameterLayout sample in the repository section of SQLView
Pro.

In this sample, we have two parameters for the report named Department
and MinSeniority. The Department parameter is a dropdown list of
departments and the MinSeniority parameter is a dropdown list of
integers from 1 to 10.

	<table width="100%">
		<tr>
			<td><b>[Department_Caption]</b></td>
			<td><b>[MinSeniority_Caption]</b></td>
		</tr>
		<tr>
			<td>[Department_Prompt]</td>
			<td>[MinSeniority_Prompt]</td>
		</tr>
	</table>

This results in the two parameters rendered side by side in a table
format as shown below:

![Parameter Layout Results](images\Parameter_LayoutResults1.png)
