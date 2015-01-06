'***************************************************************************/
'* GridReportSettings.ascx.vb
'*
'* Copyright (c) 2004 by DNNStuff.
'* All rights reserved.
'*
'* Date:        August 9, 2004
'* Author:      Richard Edwards
'* Description: Grid Report Settings Handler
'*************/

Imports System.Configuration
Imports System.Xml.Serialization
Imports DNNStuff.SQLViewPro.Controls
Imports DNNStuff.SQLViewPro.Serialization

Namespace DNNStuff.SQLViewPro.StandardReports

    Partial Class GridReportSettingsControl
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
                Return ResolveUrl("App_LocalResources/GridReportSettingsControl")
            End Get
        End Property

        Public Overrides Function UpdateSettings() As String
            Dim PageSize As Integer = 5

            Dim obj As GridReportSettings = New GridReportSettings
            With obj
                .OrderBy = txtOrderBy.Text
                .AllowSorting = chkAllowSorting.Checked
                .AllowPaging = chkAllowPaging.Checked
                If Integer.TryParse(txtPageSize.Text, PageSize) Then
                    .PageSize = PageSize
                Else
                    .PageSize = 5
                End If
                .PagerMode = ddPagerMode.SelectedValue
                .PagerPosition = ddPagerPosition.SelectedValue
                .PrevPageText = txtPrevPageText.Text
                .NextPageText = txtNextPageText.Text
                .EnableExcelExport = chkEnableExcelExport.Checked
                .ExcelExportButtonCaption = txtExcelExportButtonCaption.Text
                .ExcelExportPosition = ddExcelExportPosition.SelectedValue
                .HideColumnHeaders = chkHideColumnHeaders.Checked
                .HideColumns = txtHideColumns.Text
            End With

            Return SerializeObject(obj, GetType(GridReportSettings))

        End Function

        Public Overrides Sub LoadSettings(ByVal settings As String)
            Dim obj As GridReportSettings = New GridReportSettings()
            If Not String.IsNullOrEmpty(settings) Then
                obj = DirectCast(DeserializeObject(settings, GetType(GridReportSettings)), GridReportSettings)
            End If

            With obj
                txtOrderBy.Text = .OrderBy
                chkAllowSorting.Checked = .AllowSorting
                chkAllowPaging.Checked = .AllowPaging
                txtPageSize.Text = .PageSize.ToString
                ddPagerMode.SelectedValue = .PagerMode
                ddPagerPosition.SelectedValue = .PagerPosition
                txtPrevPageText.Text = .PrevPageText
                txtNextPageText.Text = .NextPageText
                chkEnableExcelExport.Checked = .EnableExcelExport
                txtExcelExportButtonCaption.Text = .ExcelExportButtonCaption
                ddExcelExportPosition.SelectedValue = .ExcelExportPosition
                chkHideColumnHeaders.Checked = .HideColumnHeaders
                txtHideColumns.Text = .HideColumns
            End With
        End Sub


#End Region

    End Class

#Region " Settings"
    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class GridReportSettings
        Public Property AllowSorting() As Boolean = False
        Public Property OrderBy() As String = ""
        Public Property AllowPaging() As Boolean = False
        Public Property PageSize() As Integer = 10
        Public Property PagerMode() As String = ""
        Public Property PrevPageText() As String = "Prev"
        Public Property NextPageText() As String = "Next"
        Public Property PagerPosition() As String = "Bottom"
        Public Property EnableExcelExport() As Boolean = False
        Public Property ExcelExportButtonCaption() As String = "Excel"
        Public Property ExcelExportPosition() As String = "Bottom"
        Public Property HideColumnHeaders() As Boolean = False
        Public Property HideColumns() As String = ""
    End Class
#End Region

End Namespace
