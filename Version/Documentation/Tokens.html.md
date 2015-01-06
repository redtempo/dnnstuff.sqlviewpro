# SQLViewPro Tokens 

## What are tokens?

See [Tokens](/pages/tokens) for a general overview of tokens and the
standard token support across most DNNStuff modules

SQLView Pro Specific Tokens
---------------------------

-   [REPORTNAME]
    -   inserts the name of the current report

-   [PAGEURL]
    -   inserts the full url to the current page.

-   [FULLSCREENURL]
    -   inserts the full url to the current report without the chrome
        (same as the full screen view of a report), useful for passing
        the url to a printing app etc.

-   [PARAMETER:ParameterName]
    -   inserts the value of the parameter, where the name of the
        parameter is ParameterName

-   [QUERYSTRING:KeyName] or [QS:keyname]
    -   inserts the value of the querystring parameter, where the name
        of the querystring key is *KeyName*

-   [SERVERVAR:KeyName] or [SV:keyname]
    -   inserts the value of the server variable, where the name of the
        server variable key is *KeyName*

Tokens specific to SQLView Pro Parameter Layouts
------------------------------------------------

-   [ParameterName]
    -   inserts the full parameter caption and prompt for the parameter
        named *ParameterName*

-   [ParameterName\_Prompt]
    -   inserts the parameter prompt for the parameter named
        *ParameterName*

-   [ParameterName\_Caption]
    -   inserts the parameter caption for the parameter named
        *ParameterName*


