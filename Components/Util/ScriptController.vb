#Region "licensing - please include"
' Bruce Chapman Copyright (c) 2011
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions: 
' 
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software. 
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE. 
#End Region

#Region "change control"
' name    date     comment
' * brc     dec 11   Created
' 

#End Region

Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Reflection

Namespace DNNStuff.SQLViewPro
    ''' <summary>
    ''' Used to inject script into pages independent of DotNetNuke run-time version
    ''' </summary>
    Public NotInheritable Class ScriptController
        Private Sub New()
        End Sub
        ''' <summary>
        ''' This enum provides a location for where in the output page a script should be located.
        ''' </summary>
        Public Enum ScriptInjectOrder
            a_BeforejQuery
            b_BeforeDnnXml
            c_BeforeDomPositioning
            d_BeforeDnnControls
            e_Default
        End Enum
        ''' <summary>
        ''' This enum provides a location for where in the output page a css file should be referenced.
        ''' </summary>
        Public Enum CssInjectOrder
            a_BeforeDefault
            b_BeforeModule
            c_BeforeSkin
            d_BeforeContainer
            e_BeforePortal
            f_Last
        End Enum
#Region "jquery/css injection"
        ''' <summary>
        ''' Includes the jQuery libraries onto the page
        ''' </summary>
        ''' <param name="page">Page object from calling page/control</param>
        ''' <param name="includejQueryUI">if true, includes the jQuery UI libraries</param>
        ''' <param name="debug">if true, includes the uncompressed libraries</param>
        Public Shared Sub InjectjQueryLibary(page As System.Web.UI.Page, includejQueryUI As Boolean, debug As Boolean)
            Dim major As Integer, minor As Integer, build As Integer, revision As Integer
            Dim injectjQueryLib As Boolean = False
            Dim injectjQueryUiLib As Boolean = False
            If DNNUtilities.SafeDNNVersion(major, minor, revision, build) Then
                Select Case major
                    Case 4
                        injectjQueryLib = True
                        injectjQueryUiLib = True
                        Exit Select
                    Case 5
                        injectjQueryLib = False
                        injectjQueryUiLib = True
                        Exit Select
                    Case Else
                        '6.0 and above
                        injectjQueryLib = False
                        injectjQueryUiLib = False
                        Exit Select
                End Select
            Else
                injectjQueryLib = True
            End If

            If injectjQueryLib Then
                'no in-built jQuery libraries into the framework, so include the google version
                Dim [lib] As String = Nothing
                If debug Then
                    [lib] = "http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.js"
                Else
                    [lib] = "http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js"
                End If

                If page.Header.FindControl("jquery") Is Nothing Then
                    Dim jQueryLib As New System.Web.UI.HtmlControls.HtmlGenericControl("script")
                    jQueryLib.Attributes.Add("src", [lib])
                    jQueryLib.Attributes.Add("type", "text/javascript")
                    jQueryLib.ID = "jquery"
                    page.Header.Controls.Add(jQueryLib)

                    ' use the noConflict (stops use of $) due to the use of prototype with a standard DNN distro
                    Dim noConflictScript As New System.Web.UI.HtmlControls.HtmlGenericControl("script")
                    noConflictScript.InnerText = " jQuery.noConflict(); "
                    noConflictScript.Attributes.Add("type", "text/javascript")
                    page.Header.Controls.Add(noConflictScript)
                End If
            Else
                'call DotNetNuke.Framework.jQuery.RequestRegistration();
                Dim jQueryType As Type = Type.[GetType]("DotNetNuke.Framework.jQuery, DotNetNuke")
                If jQueryType IsNot Nothing Then
                    'run the DNN 5.0 specific jQuery registration code
                    jQueryType.InvokeMember("RequestRegistration", System.Reflection.BindingFlags.InvokeMethod Or System.Reflection.BindingFlags.[Public] Or System.Reflection.BindingFlags.[Static], Nothing, jQueryType, Nothing)
                End If
            End If

            'include the UI libraries??
            If includejQueryUI Then
                If injectjQueryUiLib Then
                    ' if you want to use a local version (ie, not CDN) here's where you would change it)

                    Dim [lib] As String = Nothing
                    If debug Then
                        [lib] = "http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/jquery-ui.js"
                    Else

                        [lib] = "http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/jquery-ui.min.js"
                    End If
                    page.ClientScript.RegisterClientScriptInclude("jqueryUI", [lib])
                Else
                    'use late bound call to request registration of jquery
                    Dim jQueryType As Type = Type.[GetType]("DotNetNuke.Framework.jQuery, DotNetNuke")
                    If jQueryType IsNot Nothing Then
                        'dnn 6.0 and later, allow jquery ui to be loaded from the settings.

                        jQueryType.InvokeMember("RequestUIRegistration", System.Reflection.BindingFlags.InvokeMethod Or System.Reflection.BindingFlags.[Public] Or System.Reflection.BindingFlags.[Static], Nothing, jQueryType, Nothing)
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' Inject a js Library reference into the page
        ''' </summary>
        ''' <param name="page">The page object of the page to add the script reference to</param>
        ''' <param name="name">Unique name for the script</param>
        ''' <param name="lib">Url to the script library (can be relative/absolute)</param>
        ''' <param name="inHeader">True if to go in the page header, false if to go into the page body</param>
        ''' <param name="scriptPosition">Enumerated position for calculating where to place the script.  Works for DNN 6.1 and later only, ignored in earlier versions</param>
        Public Shared Sub InjectJsLibrary(page As System.Web.UI.Page, name As String, [lib] As String, inHeader As Boolean, scriptPosition As ScriptInjectOrder)
            Dim major As Integer, minor As Integer, build As Integer, revision As Integer
            Dim allowInHeader As Boolean = False
            Dim useDotNetNukeWebClient As Boolean = False
            Dim dnnWebClientOk As Boolean = False
            If DNNUtilities.SafeDNNVersion(major, minor, revision, build) Then
                Select Case major
                    Case 4, 5
                        allowInHeader = True
                        Exit Select
                    Case Else
                        '6.0 and above
                        If minor >= 1 Then
                            '6.1 and abpve
                            If revision < 1 Then
                                '6.1.0 - work with change in order that means no placement of scripts in header
                                allowInHeader = False
                                useDotNetNukeWebClient = True
                            Else
                                '6.1.1 and above - use client dependency framework
                                useDotNetNukeWebClient = True
                            End If
                        Else
                            '6.0  
                            allowInHeader = True
                        End If
                        Exit Select
                End Select
            End If

            If useDotNetNukeWebClient Then
                'use the dotnetnuke web client methods
                Dim priority As Integer = GetScriptPriority(scriptPosition)
                'get the imbibe type
                Dim imbibe As Type = Type.[GetType]("DotNetNuke.Web.Client.ClientResourceManagement.ClientResourceManager, DotNetNuke.Web.Client")
                If imbibe IsNot Nothing Then
                    'create arrays of both types and values for the parameters, in readiness for the reflection call
                    Dim paramTypes As Type() = New Type(3) {}
                    Dim paramValues As Object() = New Object(3) {}
                    paramTypes(0) = GetType(System.Web.UI.Page)
                    paramValues(0) = page
                    paramTypes(1) = GetType(String)
                    paramValues(1) = [lib]
                    paramTypes(2) = GetType(Integer)
                    paramValues(2) = priority
                    paramTypes(3) = GetType(String)
                    If inHeader AndAlso allowInHeader Then
                        paramValues(3) = "PageHeaderProvider"
                    Else
                        paramValues(3) = "DnnBodyProvider"
                    End If
                    'call the method to register the script via reflection
                    Dim registerScriptMethod As MethodInfo = imbibe.GetMethod("RegisterScript", paramTypes)
                    If registerScriptMethod IsNot Nothing Then
                        registerScriptMethod.Invoke(Nothing, paramValues)
                        'worked OK
                        dnnWebClientOk = True
                    End If
                End If
            End If

            If Not useDotNetNukeWebClient OrElse dnnWebClientOk = False Then
                'earlier versions or failed with reflection call, inject manually
                If inHeader AndAlso allowInHeader Then
                    If page.Header.FindControl(name) Is Nothing Then
                        Dim jsLib As New System.Web.UI.HtmlControls.HtmlGenericControl("script")
                        jsLib.Attributes.Add("src", [lib])
                        jsLib.Attributes.Add("type", "text/javascript")
                        jsLib.ID = name
                        page.Header.Controls.Add(jsLib)
                    End If
                Else
                    'register a script block - doesn't go in the header
                    If page.ClientScript IsNot Nothing Then
                        page.ClientScript.RegisterClientScriptInclude(name, [lib])
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Inject a reference to a CSS file into the page
        ''' </summary>
        ''' <param name="page">The current page object</param>
        ''' <param name="name">the name of the css file - should be unique</param>
        ''' <param name="file">the css file location - can be absolute or relative.</param>
        ''' <param name="inHeader">true if css to be included in header, false if not</param>
        ''' <param name="cssOrder">Where to include the css file in relation to the DNN css files - applies to DNN 6.1 installs only</param>
        Public Shared Sub InjectCssReference(page As System.Web.UI.Page, name As String, file As String, inHeader As Boolean, cssOrder As CssInjectOrder)
            Dim major As Integer, minor As Integer, build As Integer, revision As Integer
            Dim useDotNetNukeWebClient As Boolean = False, dnnWebClientOk As Boolean = False
            If DNNUtilities.SafeDNNVersion(major, minor, revision, build) Then
                If major >= 6 Then
                    If major = 6 AndAlso minor < 1 Then
                        useDotNetNukeWebClient = False
                    Else
                        useDotNetNukeWebClient = True
                    End If
                End If
            End If
            If useDotNetNukeWebClient Then
                'use reflection to inject the css reference
                Dim priority As Integer = GetCssPriority(cssOrder)
                'get the imbibe type
                Dim imbibe As Type = Type.[GetType]("DotNetNuke.Web.Client.ClientResourceManagement.ClientResourceManager, DotNetNuke.Web.Client")
                If imbibe IsNot Nothing Then
                    'reflection call
                    'ClientResourceManager.RegisterScript(Page page, string filePath, int priority) // default provider
                    Dim paramTypes As Type() = New Type(3) {}
                    Dim paramValues As Object() = New Object(3) {}
                    paramTypes(0) = GetType(System.Web.UI.Page)
                    paramValues(0) = page
                    paramTypes(1) = GetType(String)
                    paramValues(1) = file
                    paramTypes(2) = GetType(Integer)
                    paramValues(2) = priority
                    paramTypes(3) = GetType(String)
                    If inHeader AndAlso inHeader Then
                        paramValues(3) = "PageHeaderProvider"
                    Else
                        paramValues(3) = "DnnBodyProvider"
                    End If
                    'call the method to register the script via reflection
                    Dim registerStyleSheetMethod As MethodInfo = imbibe.GetMethod("RegisterStyleSheet", paramTypes)
                    If registerStyleSheetMethod IsNot Nothing Then
                        registerStyleSheetMethod.Invoke(Nothing, paramValues)
                        'worked OK
                        dnnWebClientOk = True
                    End If
                End If
            End If
            'not on DNN 6.1, so use direct method to inject the header / body.
            'note that outcome position is pot luck based on calling code.
            If Not useDotNetNukeWebClient OrElse dnnWebClientOk = False Then
                If page.Header.FindControl(name) Is Nothing Then
                    '764 : xhtml compliance by using html link control which closes tag without separate closing tag
                    Dim cssFile As New System.Web.UI.HtmlControls.HtmlLink()
                    cssFile.Attributes.Add("rel", "stylesheet")
                    cssFile.Attributes.Add("href", file)
                    cssFile.Attributes.Add("type", "text/css")
                    cssFile.ID = name
                    page.Header.Controls.Add(cssFile)
                Else
                    If page.FindControl(name) Is Nothing Then
                        Dim cssFile As New System.Web.UI.HtmlControls.HtmlLink()
                        cssFile.Attributes.Add("rel", "stylesheet")
                        cssFile.Attributes.Add("href", file)
                        cssFile.Attributes.Add("type", "text/css")
                        cssFile.ID = name
                        page.Controls.Add(cssFile)
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' Injects Css for jQuery tabs Ui
        ''' </summary>
        ''' <param name="page"></param>
        ''' <param name="preDnn6CssFile"></param>
        ''' <param name="postDnn6CssFile"></param>
        ''' <remarks>This method only gets run in pre-dnn6 installations.  Otherwise it uses the pre-defined DNN 6 Css declarations to style 
        ''' the UI Tabs</remarks>
        Public Shared Sub InjectjQueryTabsCss(page As System.Web.UI.Page, preDnn6CssFile As String, postDnn6CssFile As String)
            Dim major As Integer, minor As Integer, build As Integer, revision As Integer
            DNNUtilities.SafeDNNVersion(major, minor, revision, build)
            If major < 6 Then
                If preDnn6CssFile IsNot Nothing AndAlso preDnn6CssFile <> "" Then
                    InjectCssReference(page, "moduleJqCss", preDnn6CssFile, True, CssInjectOrder.f_Last)
                End If
                InjectCssReference(page, "jqueryUiTheme", "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css", True, CssInjectOrder.f_Last)
            End If
            If major >= 6 AndAlso postDnn6CssFile IsNot Nothing AndAlso postDnn6CssFile <> "" Then
                InjectCssReference(page, "moduleJqCss", postDnn6CssFile, True, CssInjectOrder.c_BeforeSkin)
            End If

        End Sub
#End Region

#Region "calculate priority from enum types"
        Private Shared Function GetScriptPriority(scriptPosition As ScriptInjectOrder) As Integer
            Select Case scriptPosition
                Case ScriptInjectOrder.a_BeforejQuery
                    Return 4
                Case ScriptInjectOrder.b_BeforeDnnXml
                    Return 14
                Case ScriptInjectOrder.c_BeforeDomPositioning
                    Return 29
                Case ScriptInjectOrder.d_BeforeDnnControls
                    Return 39
                Case Else
                    Return 100
            End Select
        End Function
        Private Shared Function GetCssPriority(cssPosition As CssInjectOrder) As Integer
            Select Case cssPosition
                Case CssInjectOrder.a_BeforeDefault
                    Return 4
                Case CssInjectOrder.b_BeforeModule
                    Return 9
                Case CssInjectOrder.c_BeforeSkin
                    Return 14
                Case CssInjectOrder.d_BeforeContainer
                    Return 24
                Case CssInjectOrder.e_BeforePortal
                    Return 34
                Case Else
                    Return 50
            End Select
        End Function
#End Region

    End Class
End Namespace
