'***************************************************************************/
'* XmlReportSettings.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Template Report Settings Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization

Namespace DNNStuff.SQLViewPro.StandardReports

    Public Class TemplateReportSettingsControl
        Inherits ReportSettingsControlBase

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

#Region " Page"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        End Sub

#End Region

#Region " Base Method Implementations"
        Protected Overrides ReadOnly Property LocalResourceFile() As String
            Get
                Return ResolveUrl("App_LocalResources/TemplateReportSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String
            Dim PageSize As Integer = 5
            Dim obj As TemplateReportSettings = New TemplateReportSettings
            With obj
                .TemplateText = txtTemplateText.Text
                .AllowPaging = chkAllowPaging.Checked
                .PagingType = ddPagingType.SelectedValue
                If Integer.TryParse(txtPageSize.Text, PageSize) Then
                    .PageSize = PageSize
                Else
                    .PageSize = 5
                End If
                .PrevPageText = txtPrevPageText.Text
                .NextPageText = txtNextPageText.Text
            End With

            Return SerializeObject(obj, GetType(TemplateReportSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As TemplateReportSettings = New TemplateReportSettings()
            If Not String.IsNullOrEmpty(settings) Then
                obj = DirectCast(DeserializeObject(settings, GetType(TemplateReportSettings)), TemplateReportSettings)
            End If

            With obj
                txtTemplateText.Text = obj.TemplateText
                chkAllowPaging.Checked = .AllowPaging
                ddPagingType.SelectedValue = .PagingType
                txtPageSize.Text = .PageSize.ToString
                txtPrevPageText.Text = .PrevPageText
                txtNextPageText.Text = .NextPageText
            End With
        End Sub

#End Region

    End Class

#Region " Settings"
    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class TemplateReportSettings
        Public Property TemplateText() As String = "[EACHROW][StringCol][/EACHROW]"
        Public Property AllowPaging() As Boolean = False
        Public Property PagingType() As String = "Internal"
        Public Property PageSize() As Integer = 10
        Public Property PrevPageText() As String = "Prev"
        Public Property NextPageText() As String = "Next"
        Public Property PagerPosition() As String = "Bottom"
    End Class
#End Region

End Namespace
