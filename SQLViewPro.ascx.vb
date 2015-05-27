'***************************************************************************/
'* SQLViewPro.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Portal Module for displaying SQLViewPro
'*************/

Imports DotNetNuke
Imports DotNetNuke.Services.Localization
Imports System.Text
Imports DNNStuff.SQLViewPro.Services.Data
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro

    Partial  Class SQLViewPro
        Inherits Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable
        Implements Entities.Modules.IPortable

        Private Const CTRL_ACTION_BACK As String = "cmdBack"

        Private Const RenderMode_Default As String = "Default"
        Private Const RenderMode_Popup As String = "Popup"
        Private Const RenderMode_NewWindow As String = "NewWindow"


#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            Try
#If TRIAL Then

#End If
#If CONFIG = "Trial" Then
                Common.AddTrialNotice(Me)
#End If

                ' PortalModuleControl base class settings for this module
                MyBase.HelpURL = "http://www.dnnstuff.com" ' a URL for support on the module

                InitReportSet()

                If ReportSet IsNot Nothing Then
                    ' initialize report sets and parameters
                    InitParameters()
                    RenderParameterArea()
                Else
                    HideAllAreas()
                    Controls.Add(New LiteralControl("Module is not configured. Please create a new reportset or import an existing one from the repository."))
                End If

            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try

        End Sub

        Protected WithEvents cmdAction As System.Web.UI.WebControls.LinkButton
        Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
#End Region

#Region " DNN Menus"

        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim addedActions As New Entities.Modules.Actions.ModuleActionCollection
                Dim action As Entities.Modules.Actions.ModuleAction

                ' add/edit report set
                If ReportSet Is Nothing Then
                    ' new report set
                    action = New Entities.Modules.Actions.ModuleAction(GetNextActionID)
                    With action
                        .Title = Localization.GetString("Menu.NewReportSet", LocalResourceFile)
                        .CommandName = Entities.Modules.Actions.ModuleActionType.ContentOptions
                        .Url = EditUrl("ReportSetId", "-1", "EditReportSet")
                        .Secure = Security.SecurityAccessLevel.Edit
                    End With
                    addedActions.Add(action)
                Else
                    action = New Entities.Modules.Actions.ModuleAction(GetNextActionID)
                    With action
                        .Title = String.Format(Localization.GetString("Menu.EditCurrentReportSet", LocalResourceFile), ReportSet.ReportSetName)
                        .CommandName = Entities.Modules.Actions.ModuleActionType.ContentOptions
                        .Url = EditUrl("ReportSetId", ReportSet.ReportSetId.ToString, "EditReportSet")
                        .Secure = Security.SecurityAccessLevel.Edit
                    End With
                    addedActions.Add(action)

                    For Each r As ReportInfo In Reports
                        action = New Entities.Modules.Actions.ModuleAction(GetNextActionID)
                        With action
                            .Title = String.Format(Localization.GetString("Menu.EditCurrentReport", LocalResourceFile), r.ReportName)
                            .CommandName = Entities.Modules.Actions.ModuleActionType.ContentOptions
                            .Url = EditUrl("ReportSetId", ReportSet.ReportSetId.ToString, "EditReport", "ReportId=" & r.ReportId)
                            .Secure = Security.SecurityAccessLevel.Edit
                        End With
                        addedActions.Add(action)

                    Next

                    For Each p As ParameterInfo In Parameters
                        action = New Entities.Modules.Actions.ModuleAction(GetNextActionID)
                        With action
                            .Title = String.Format(Localization.GetString("Menu.EditCurrentParameter", LocalResourceFile), p.ParameterName)
                            .CommandName = Entities.Modules.Actions.ModuleActionType.ContentOptions
                            .Url = EditUrl("ParameterId", p.ParameterId.ToString, "EditParameter", String.Format("ReportSetId={0}", ReportSet.ReportSetId.ToString))
                            .Secure = Security.SecurityAccessLevel.Edit
                        End With
                        addedActions.Add(action)
                    Next

                End If

                ' add break
                addedActions.Add(New DotNetNuke.Entities.Modules.Actions.ModuleAction(GetNextActionID(), "~", ""))

                ' edit connections
                action = New Entities.Modules.Actions.ModuleAction(GetNextActionID)
                With action
                    .Title = Localization.GetString("Menu.EditConnections", LocalResourceFile)
                    .CommandName = Entities.Modules.Actions.ModuleActionType.ContentOptions
                    .Url = EditUrl("EditConnections")
                    .Secure = Security.SecurityAccessLevel.Edit
                End With
                addedActions.Add(action)

                ' repository
                action = New Entities.Modules.Actions.ModuleAction(GetNextActionID)
                With action
                    .Title = Localization.GetString("Menu.BrowseRepository", LocalResourceFile)
                    .CommandName = Entities.Modules.Actions.ModuleActionType.ContentOptions
                    .Url = EditUrl("BrowseRepository")
                    .Secure = Security.SecurityAccessLevel.Edit
                End With
                addedActions.Add(action)

                ' add break
                addedActions.Add(New DotNetNuke.Entities.Modules.Actions.ModuleAction(GetNextActionID(), "~", ""))

                Return addedActions
            End Get
        End Property

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements Entities.Modules.IPortable.ExportModule
            ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
            Return ""
        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements Entities.Modules.IPortable.ImportModule
            ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
        End Sub
#End Region

#Region " Page Properties"
        ' will have to set this dynamically
        Private _ReportSetId As Integer = -1
        Private _Parameters As ArrayList
        Private _ReportSet As ReportSetInfo
        Private _Reports As ArrayList
        Private _DrilldownStack As New ArrayListStack

        ' token settings
        Private _sqlviewproTokens As Hashtable

        ' effective module id
        Private _EffectiveModuleId As Integer = -1
        Public ReadOnly Property EffectiveModuleId() As Integer
            Get
                If _EffectiveModuleId = -1 Then
                    If Request.QueryString("ModuleId") IsNot Nothing Then
                        _EffectiveModuleId = CInt(Request.QueryString("ModuleId"))
                    Else
                        _EffectiveModuleId = MyBase.ModuleId
                    End If
                End If
                Return _EffectiveModuleId
            End Get
        End Property
        Private _EffectiveTabId As Integer = -1
        Public ReadOnly Property EffectiveTabId() As Integer
            Get
                If _EffectiveTabId = -1 Then
                    If Request.QueryString("TabId") IsNot Nothing Then
                        _EffectiveTabId = CInt(Request.QueryString("TabId"))
                    Else
                        _EffectiveTabId = MyBase.TabId
                    End If
                End If
                Return _EffectiveTabId
            End Get
        End Property

        Public ReadOnly Property DrilldownStackKey() As String
            Get
                Return "Drilldown$" & EffectiveModuleId.ToString
            End Get
        End Property

        Public Property ReportSet() As ReportSetInfo
            Get
                Return _ReportSet
            End Get
            Set(ByVal Value As ReportSetInfo)
                _ReportSet = Value
            End Set
        End Property

        Public Property Reports() As ArrayList
            Get
                Return _Reports
            End Get
            Set(ByVal value As ArrayList)
                _Reports = value
            End Set
        End Property

        Public Property Parameters() As ArrayList
            Get
                Return _Parameters
            End Get
            Set(ByVal Value As ArrayList)
                _Parameters = Value
            End Set
        End Property

#End Region

#Region " Page Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                DotNetNuke.Framework.jQuery.RequestRegistration()
                If ReportSet IsNot Nothing Then
                    If ReportSet.RenderMode = RenderMode_Popup Or ReportSet.RenderMode = RenderMode_NewWindow Then
                        Page.ClientScript.RegisterClientScriptInclude(Me.GetType, "jquery.fancybox", ResolveUrl("resources/jQuery/FancyBox/jquery.fancybox-1.3.4.pack.js"))
                        Page.ClientScript.RegisterClientScriptInclude(Me.GetType, "jquery.sqlviewpro.fullscreen", ResolveUrl("resources/jQuery/jquery.sqlviewpro.fullscreen.js"))
                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "sqlviewpro.object", String.Format("<script type=""text/javascript"">sqlviewpro.fullScreenUrl = '{0}';</script>", FullscreenUrl))
                        DNNUtilities.InjectCSS(Me.Page, ResolveUrl("resources/jQuery/fancybox/jquery.fancybox-1.3.4.css"))
                    End If
                    RenderModule()
                End If
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            If ReportSet IsNot Nothing Then
                If Session(DrilldownStackKey) Is Nothing Then
                    Session.Add(DrilldownStackKey, _DrilldownStack)
                Else
                    Session(DrilldownStackKey) = _DrilldownStack
                End If
            End If
        End Sub

#End Region

#Region " Action"

        Public Sub RefreshReport()
            ' going forward from parameters to first level report
            Dim state As New DrilldownState(DirectCast(Parameters.Clone, ArrayList))
            ' clear panel
            pnlReportSet.Controls.Clear()
            _DrilldownStack.Clear()
            _DrilldownStack.Push(state)
            RenderPage()
        End Sub

        Private Sub PreviousReport()
            ' clear panel
            pnlReportSet.Controls.Clear()
            ' going back a report
            _DrilldownStack.Pop()
            RenderPage()
        End Sub

        Private Sub cmdAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAction.Click
            RefreshReport()
        End Sub

        Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
            PreviousReport()
        End Sub

        Private Sub cmdBack_Command(sender As Object, e As System.Web.UI.WebControls.CommandEventArgs) Handles cmdBack.Command

            'Select Case e.CommandName
            '    Case "Goto"
            '        Dim level As Integer = CInt(e.CommandArgument)
            '        While _DrilldownStack.Count > level
            '            '                        _DrilldownStack.Pop()
            '            _DrilldownStack.RemoveAt(_DrilldownStack.Count - 1)

            '        End While
            'End Select

            '' clear panel
            'pnlReportSet.Controls.Clear()
            'RenderPage()
        End Sub

        Private Sub RenderModule()
            If ReportSet Is Nothing Then Exit Sub

            Try
                RetrieveParameterValuesFromControls()

                If Not Page.IsPostBack Then
                    If CurrentlyInFullscreen() Then
                        RetrieveParameterValuesFromQueryString()
                    End If

                    ' first time shown
                    _DrilldownStack.Clear()
                    If (NoParameters() And Not ReportSet.RenderMode = RenderMode_Popup) Or (NoParameters() And Not ReportSet.RenderMode = RenderMode_NewWindow) Or CurrentlyInFullscreen() Or (ReportSet.AutoRun And Not (ReportSet.RenderMode = RenderMode_Popup Or ReportSet.RenderMode = RenderMode_NewWindow)) Then
                        Dim state As New DrilldownState(DirectCast(Parameters.Clone, ArrayList))
                        _DrilldownStack.Push(state)    ' initial reports that aren't drilldowns
                    End If
                End If

                RenderPage()

            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub RenderPage()
            If _DrilldownStack.IsEmpty() Then
                ShowParameterArea()
            Else
                RenderReportSetArea()
                ShowReportSetArea()
            End If
        End Sub

#End Region

#Region " ReportSet"
        Private Sub InitReportSet()

            ' init stack for drilldown
            If Session.Item(DrilldownStackKey) Is Nothing Then
                _DrilldownStack = New ArrayListStack
            Else
                _DrilldownStack = DirectCast(Session.Item(DrilldownStackKey), ArrayListStack)
            End If

            Dim objReportSetController As ReportSetController = New ReportSetController
            _ReportSet = objReportSetController.GetReportSetByModule(EffectiveModuleId)
            If _ReportSet IsNot Nothing Then
                If _ReportSet.ReportSetTheme <> "None" Then
                    DNNUtilities.InjectCSS(Me.Page, ResolveUrl(String.Format("skins/{0}/styles.css", _ReportSet.ReportSetTheme)))
                End If
                ' get reports
                _Reports = objReportSetController.GetReportSetReport(ReportSet.ReportSetId)
            End If


        End Sub

        Private Sub ShowReportSetArea()
            If (NoParameters() And FirstLevel()) Or (ReportSet.AlwaysShowParameters And FirstLevel()) Then
                If cmdBack IsNot Nothing Then cmdBack.Visible = False
            Else
                If cmdBack IsNot Nothing Then cmdBack.Visible = True
            End If
            pnlParameter.Visible = ReportSet.AlwaysShowParameters And Not NoParameters()
            pnlReportSet.Visible = True
        End Sub

        Private Sub RenderReportSetArea()
            ' set class of report set panel
            pnlReportSet.CssClass = _ReportSet.ReportSetTheme & "_ReportSet"

            Dim c As Control = Nothing

            c = ApplyReportLayout(BuildDefaultReportSetLayout())
            pnlReportSet.Controls.Add(c)

            ' set action buttons
            Dim obj As Control = FindControlRecursive(pnlReportSet, CTRL_ACTION_BACK)

            If obj IsNot Nothing Then
                cmdBack = DirectCast(obj, LinkButton)
            End If

        End Sub

        Private Function BuildDefaultReportSetLayout() As String
            ' build default reportset layout
            Dim sb As New StringBuilder
            sb.Append(RenderBackButtonAsText())
            sb.Append(RenderReportContainerAsText())

            Return sb.ToString
        End Function

        Private Function RenderReportSetHeaderAsText() As String
            Dim s As New StringBuilder
            If _ReportSet.ReportSetHeaderText.Length > 0 Then
                s.AppendFormat("<div class=""{0}_Header"">{1}</div>", _ReportSet.ReportSetTheme, ReplaceTokens(_ReportSet.ReportSetHeaderText))
            End If
            Return s.ToString
        End Function

        Private Function RenderReportSetFooterAsText() As String
            Dim s As New StringBuilder
            If _ReportSet.ReportSetFooterText.Length > 0 Then
                s.AppendFormat("<div class=""{0}_Footer"">{1}</div>", _ReportSet.ReportSetTheme, ReplaceTokens(_ReportSet.ReportSetFooterText))
            End If
            Return s.ToString
        End Function

        Private Function RenderReportContainerAsText() As String
            Dim s As New StringBuilder
            s.AppendFormat("<div id=""pnlReportContainer"" runat=""server"" />")
            Return s.ToString
        End Function

        Private Function RenderBackButtonAsText() As String
            ' button
            If HideBackButton() And FirstLevel() Then Return ""
            Return String.Format("<asp:LinkButton id=""{1}"" runat=""server"" CommandName=""Goto"" CommandArgument=""1"" OnClick=""{1}_Click"" cssClass=""CommandButton SQLViewProButton"">{0}</asp:LinkButton>", BackCaption, CTRL_ACTION_BACK)
        End Function

        'Private Function RenderBreadCrumbAsText() As String
        '    Dim s As New StringBuilder

        '    Dim level As Integer = 1
        '    For Each x As DrilldownState In _DrilldownStack
        '        If level = _DrilldownStack.Count Then
        '            s.AppendFormat("<span cssClass=""CommandButton"">{0}</asp:LinkButton>", x.)
        '        Else
        '            s.AppendFormat("<asp:LinkButton id=""cmdGoto_{0}"" runat=""server"" CommandName=""Goto"" CommandArgument=""1"" OnCommand=""cmdBack_Command"" cssClass=""CommandButton""><- Back</asp:LinkButton>", level)
        '        End If
        '        level += 1
        '    Next
        '    Return s.ToString
        'End Function

#End Region

#Region " Report"
        Private Function ApplyReportLayout(ByVal reportLayout As String) As Control

            Dim reportControls As Control = ParseControl(reportLayout)

            Dim reportContainer As Control = FindControlRecursive(reportControls, "pnlReportContainer")

            ' state
            Dim state As DrilldownState = DirectCast(_DrilldownStack.Peek, DrilldownState)
            With state
                .PortalId = PortalId
                .ModuleId = EffectiveModuleId
                .TabId = TabId
                .UserId = UserId
                .ReportSet = ReportSet
            End With

            Dim objReportBase As Controls.ReportControlBase
            For Each objReport As ReportInfo In Reports

                If objReport.ReportDrilldownReportId = state.FromReportId And objReport.ReportDrilldownFieldname = state.FromReportColumn Then ' show only if proper drilldown level

                    ' override theme if nothing
                    If objReport.ReportTheme = "" Then objReport.ReportTheme = ReportSet.ReportSetTheme

                    If objReport.ReportTheme <> "None" Then
                        DNNUtilities.InjectCSS(Me.Page, ResolveUrl(String.Format("skins/{0}/styles.css", objReport.ReportTheme)))
                    End If

                    ' override connection string if necessary
                    If objReport.ReportConnectionId = ConnectionType.ReportSetDefault Then
                        objReport.ReportConnectionId = ReportSet.ReportSetConnectionId
                        objReport.ReportConnectionString = ReportSet.ReportSetConnectionString
                    End If

                    ' add to page
                    objReportBase = CType(LoadControl(Me.ResolveUrl(objReport.ReportTypeControlSrc)), Controls.ReportControlBase)
                    objReportBase.ID = "ReportBase" & objReport.ReportId
                    objReportBase.FullScreenUrl = FullscreenUrl()
                    objReportBase.LoadRuntimeSettings(objReport)

                    ' handlers
                    AddHandler objReportBase.OnDrillDown, AddressOf OnDrilldown

                    objReportBase.State = state
                    objReportBase.Report = objReport

                    ' add report wrapper and then add this to report set wrapper
                    Dim ctrl As New HtmlControls.HtmlGenericControl("div")
                    With ctrl
                        .Attributes.Add("class", objReport.ReportTheme & "_Report")
                    End With

                    Dim headerMarkup As String = objReportBase.RenderHeaderAsText
                    Dim footerMarkup As String = objReportBase.RenderFooterAsText

                    ' header
                    If headerMarkup <> "" Then ctrl.Controls.Add(ParseControl(headerMarkup))
                    ' report
                    ctrl.Controls.Add(objReportBase)
                    ' footer
                    If footerMarkup <> "" Then ctrl.Controls.Add(ParseControl(footerMarkup))


                    reportContainer.Controls.Add(ctrl)
                End If

            Next

            Return reportControls
        End Function

#End Region

#Region " Convenience"
        Private Function NoParameters() As Boolean
            Return _Parameters.Count = 0
        End Function
        Private Function HasExcelReport() As Boolean
            For Each objReport As ReportInfo In Reports
                If objReport.ReportTypeId = "EXCEL" Then Return True
            Next
            Return False
        End Function
        Private Function FirstLevel() As Boolean
            Return _DrilldownStack.Count = 1
        End Function
        Private Function ActionCaption() As String
            Dim caption As String = ReportSet.RunCaption
            If caption = "" Then
                caption = Localization.GetString("ActionRun.Text", LocalResourceFile)
            End If
            If caption = "" Then caption = "Run"
            Return caption
        End Function
        Private Function BackCaption() As String
            Dim caption As String = ReportSet.BackCaption
            If caption = "" Then
                caption = Localization.GetString("ActionBack.Text", LocalResourceFile)
            End If
            If caption = "" Then caption = "<- Back"
            Return caption
        End Function

        Private Function CurrentlyInFullscreen() As Boolean
            Return Request.Url.AbsolutePath.EndsWith("Report.aspx")
        End Function

        Private Function HideBackButton() As Boolean
            Dim value As String = Request.QueryString("hidebackbutton")
            If value <> Nothing Then
                If value = "0" Then Return False Else Return True
            Else
                Return False
            End If
        End Function

        Private Function FullscreenUrl() As String
            Return ResolveUrl("Report.aspx") & "?moduleid=" & EffectiveModuleId.ToString() & "&tabid=" & EffectiveTabId.ToString()
        End Function

#End Region

#Region " Parameter"
        Private Sub InitParameters()
            ' get report set
            Dim objReportSetController As ReportSetController = New ReportSetController
            ' store parameters
            _Parameters = objReportSetController.GetReportSetParameter(ReportSet.ReportSetId)

        End Sub

        Private Sub HideAllAreas()
            ' used when report not configured at all
            pnlParameter.Visible = False
            pnlReportSet.Visible = False
        End Sub

        Private Sub ShowParameterArea()
            pnlParameter.Visible = True
            pnlReportSet.Visible = False
        End Sub

        Private Sub RetrieveParameterValuesFromControls()
            Dim ctrl As Controls.ParameterControlBase
            For Each objParameter As ParameterInfo In Parameters

                ctrl = DirectCast(pnlParameter.FindControl(objParameter.ParameterIdentifier), Controls.ParameterControlBase)
                If Not ctrl Is Nothing Then
                    objParameter.Values = ctrl.Values
                    objParameter.ExtraValues = ctrl.ExtraValues
                    objParameter.MultiValued = ctrl.MultiValued
                End If
            Next
        End Sub

        Private Sub RetrieveParameterValuesFromQueryString()
            Dim ctrl As Controls.ParameterControlBase
            For Each objParameter As ParameterInfo In Parameters
                If Request.QueryString(objParameter.ParameterIdentifier) IsNot Nothing Then
                    ctrl = DirectCast(pnlParameter.FindControl(objParameter.ParameterIdentifier), Controls.ParameterControlBase)
                    If Not ctrl Is Nothing Then
                        Dim val As String = Request.QueryString(objParameter.ParameterIdentifier)
                        Dim values As List(Of String) = New List(Of String)
                        If val.StartsWith("'") Then 'multivalue list
                            ' remove opening and closing single quote, convert ',' to ^, then split on ^
                            values.AddRange(val.TrimStart(New Char() {"'"c}).TrimEnd(New Char() {"'"c}).Replace("','", "^").Split(New Char() {"^"c}, StringSplitOptions.None))
                        Else
                            values.Add(val)
                        End If
                        ctrl.Values = values
                        objParameter.Values = values
                    End If
                End If
            Next

        End Sub

        Private Sub RenderParameterArea()
            pnlParameter.CssClass = ReportSet.ReportSetTheme & "_Parameter"

            Dim c As Control = Nothing

            If ReportSet.ParameterLayout <> "" Then
                c = ApplyParameterLayout(ReportSet.ParameterLayout)
            Else
                c = ApplyParameterLayout(BuildDefaultParameterLayout())
            End If

            pnlParameter.Controls.Add(c)

            ' set action buttons (can be nothing if using full screen mode)
            cmdAction = DirectCast(FindControlRecursive(pnlParameter, "cmdAction"), LinkButton)

        End Sub

        Private Function BuildDefaultParameterLayout() As String
            ' build default parameter layout
            Dim sb As New StringBuilder
            For Each objParameter As ParameterInfo In Parameters
                sb.Append(RenderParameterAsText(objParameter))
            Next
            Return sb.ToString
        End Function

        Private Function ApplyParameterLayout(ByVal parameterLayout As String) As Control

            ' add reportset header
            parameterLayout = RenderReportSetHeaderAsText() & parameterLayout

            ' add action button if user hasn't
            If Not parameterLayout.Contains("[ACTIONBUTTON]") Then
                parameterLayout = parameterLayout & "[ACTIONBUTTON]"
            End If

            parameterLayout = parameterLayout & RenderReportSetFooterAsText()

            ' build parameter controls
            For Each objParameter As ParameterInfo In Parameters
                ' caption
                If parameterLayout.Contains("[" & objParameter.ParameterIdentifier & "_Caption]") Then
                    parameterLayout = parameterLayout.Replace("[" & objParameter.ParameterIdentifier & "_Caption]", RenderLabelAsText(objParameter.ParameterCaption, ""))
                End If

                ' prompt
                If parameterLayout.Contains("[" & objParameter.ParameterIdentifier & "_Prompt]") Then
                    parameterLayout = parameterLayout.Replace("[" & objParameter.ParameterIdentifier & "_Prompt]", RenderPromptAsText(objParameter))
                End If

                ' caption & prompt
                If parameterLayout.Contains("[" & objParameter.ParameterIdentifier & "]") Then
                    parameterLayout = parameterLayout.Replace("[" & objParameter.ParameterIdentifier & "]", RenderParameterAsText(objParameter))
                End If

            Next
            ' action button
            parameterLayout = parameterLayout.Replace("[ACTIONBUTTON]", RenderActionButtonAsText())

            ' other tokens
            parameterLayout = ReplaceTokens(parameterLayout)

            ' build parameter registration types
            Dim parameterRegistrations As String = ""
            For Each registrationValue As String In registeredParameterTypes.Values
                parameterRegistrations = parameterRegistrations & registrationValue
            Next
            ' create controls
            Dim parameterControls As Control = ParseControl(parameterRegistrations & parameterLayout)

            ' load settings for each parameter
            For Each objParameter As ParameterInfo In Parameters
                Dim c As Control
                c = FindControlRecursive(parameterControls, objParameter.ParameterIdentifier)
                If c IsNot Nothing Then
                    Dim baseParameter As Controls.ParameterControlBase = DirectCast(c, Controls.ParameterControlBase)
                    baseParameter.Settings = objParameter
                    baseParameter.PortalSettings = PortalSettings
                    baseParameter.LoadRuntimeSettings()

                    ' handlers
                    AddHandler baseParameter.OnRun, AddressOf OnRun

                End If
            Next

            Return parameterControls
        End Function

        Dim registeredParameterTypes As Specialized.StringDictionary = New Specialized.StringDictionary()
        Private Function RenderPromptAsText(ByVal objParameter As ParameterInfo) As String
            Dim registerControl As String = ""
            Dim parameterControl As String = String.Format("<svp:{0} id=""{1}"" runat=""server"" />", objParameter.ParameterTypeId.ToLower(), objParameter.ParameterIdentifier)
            If Not registeredParameterTypes.ContainsKey(objParameter.ParameterTypeName) Then
                registerControl = String.Format("<%@ register tagprefix=""svp"" tagname=""{0}"" src=""{1}"" %>", objParameter.ParameterTypeId.ToLower, ResolveUrl(objParameter.ParameterTypeControlSrc))
                registeredParameterTypes.Add(objParameter.ParameterTypeName, registerControl)
            End If
            Return parameterControl
        End Function

        Private Function RenderParameterAsText(ByVal objParameter As ParameterInfo) As String

            Dim sb As New StringBuilder

            ' label
            If objParameter.ParameterCaption.Length > 0 Then
                Dim lbl As String = RenderLabelAsText(objParameter.ParameterCaption)
                sb.Append(lbl)
                sb.Append("<br />")
            End If

            ' input
            Dim prompt As String = RenderPromptAsText(objParameter)
            sb.Append(prompt)
            sb.Append("<br />")

            Return sb.ToString

        End Function

        Private Function RenderLabelAsText(ByVal caption As String, Optional ByVal suffix As String = " :") As String
            ' label
            Return "<asp:label CssClass=""Caption"">" & caption & suffix & "</asp:label>"
        End Function

        Private Function RenderActionButtonAsText() As String
            If ReportSet.RenderMode = RenderMode_Popup And Not CurrentlyInFullscreen() Then
                Return RenderFullScreenActionButtonAsText()
            End If
            If ReportSet.RenderMode = RenderMode_NewWindow And Not CurrentlyInFullscreen() Then
                Return RenderNewWindowActionButtonAsText()
            End If
            Return String.Format("<asp:LinkButton id=""cmdAction"" runat=""server"" OnClick=""cmdAction_Click"" cssClass=""CommandButton SQLViewProButton"">{0}</asp:LinkButton>", ActionCaption)
        End Function

        Private Function RenderFullScreenActionButtonAsText() As String
            Return String.Format("<a class=""commandbutton SQLViewProButton fullscreen"" href=""{1}"">{0}</a>", ActionCaption, FullscreenUrl)
        End Function

        Private Function RenderNewWindowActionButtonAsText() As String
            Return String.Format("<a class=""commandbutton SQLViewProButton newwindow"" href=""{1}"">{0}</a>", ActionCaption, FullscreenUrl)
        End Function

        Private Function FindControlRecursive(root As Control, searchId As String) As Control

            If root.ID = searchId Then
                Return root
            End If

            For Each ctl As Control In root.Controls
                Dim foundCtl As Control = FindControlRecursive(ctl, searchId)

                If foundCtl IsNot Nothing Then
                    Return foundCtl
                End If
            Next
            Return Nothing
        End Function

#End Region

#Region " Parameter Events"
        Private Sub OnRun(ByVal sender As System.Object)
            RefreshReport()
        End Sub
#End Region

#Region " Drilldown"
        Private Sub OnDrilldown(ByVal sender As System.Object, ByVal e As Controls.DrilldownEventArgs)
            Dim state As New DrilldownState(e.ReportId, e.Name, DirectCast(Parameters.Clone, ArrayList))

            ' add datarow as parameters
            Dim columnIndex As Integer = 0
            For columnIndex = 0 To e.Value.ItemArray.Length - 1
                Dim pi As New ParameterInfo
                pi.ParameterName = e.Value.Table.Columns(columnIndex).ColumnName
                pi.Values = New List(Of String)(New String() {e.Value.Item(columnIndex).ToString})

                state.Parameters.Add(pi)

            Next
            _DrilldownStack.Push(state)
            ' clear panel
            pnlReportSet.Controls.Clear()
            ' render again
            RenderPage()
        End Sub

#End Region

#Region " Token Replacement"
        Private Function ReplaceTokens(ByVal text As String) As String

            Return TokenReplacement.ReplaceTokens(text, TokenSettings)

        End Function

        Private Function TokenSettings() As Hashtable

            If _sqlviewproTokens IsNot Nothing Then Return _sqlviewproTokens

            Dim tokens As New Hashtable
            tokens.Add("MODULEID", Me.EffectiveModuleId.ToString)
            tokens.Add("MODULEFOLDER", Me.ControlPath.Remove(Me.ControlPath.Length - 1)) ' remove last / character to be consistent with resolveurl
            tokens.Add("TABMODULEID", Me.TabModuleId.ToString)

            tokens.Add("PAGEURL", Request.Url.AbsoluteUri)
            tokens.Add("IMAGEURL", ResolveUrl("~/images"))

            tokens.Add("CDATASTART", "<![CDATA[")
            tokens.Add("CDATAEND", "]]>")

            _sqlviewproTokens = tokens
            Return _sqlviewproTokens
        End Function

#End Region


    End Class

End Namespace
