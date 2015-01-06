Imports System.Xml
Imports System.Collections.Generic
Imports DotNetNuke.Services.Localization
Imports System.Reflection

Namespace DNNStuff.SQLViewPro

    Partial Public Class CustomPropertiesViewer
        Inherits System.Web.UI.UserControl

#Region " Public Properties"
        Public Property Settings() As Settings
        Public Property LocalResourceFile() As String = ""
        Public Property InitialValues() As Object
        Public Property Filter() As String = ""
#End Region

#Region " Public Methods"
        Public Sub InitializeValues()
            For Each prop As PropertyInfo In InitialValues.GetType.GetProperties()
                For Each customprop As CustomProperty In Settings.GetAllProperties()
                    If customprop.Name.ToUpper = prop.Name.ToUpper Then
                        customprop.Value = prop.GetValue(InitialValues, Nothing)
                    End If
                Next
            Next
        End Sub
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'RenderProperties()
        End Sub

        ''' <summary>
        ''' Capture settings from controls and save to properties dictionary
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetProperties()
            For Each customprop As CustomProperty In Settings.GetAllProperties()
                Select Case customprop.Type
                    Case PropertyType.String
                        Dim tb As TextBox = DirectCast(FindControl("tb" & customprop.Name), TextBox)
                        If tb IsNot Nothing Then
                            customprop.Value = tb.Text
                        End If
                    Case PropertyType.Boolean
                        Dim cb As CheckBox = DirectCast(FindControl("cb" & customprop.Name), CheckBox)
                        If cb IsNot Nothing Then
                            customprop.Value = cb.Checked.ToString.ToLower
                        End If
                    Case PropertyType.Choice, PropertyType.Directory, PropertyType.Files
                        Dim ddl As DropDownList = DirectCast(FindControl("ddl" & customprop.Name), DropDownList)
                        If ddl IsNot Nothing Then
                            customprop.Value = ddl.SelectedValue
                        End If
                End Select
            Next
        End Sub

        Public Sub SetProperties(ByRef obj As Object)
            SetProperties()

            For Each prop As PropertyInfo In obj.GetType.GetProperties()
                For Each customprop As CustomProperty In Settings.GetAllProperties()
                    If customprop.Name = prop.Name Then
                        Dim value As Object = Convert.ChangeType(customprop.Value, prop.PropertyType)
                        prop.SetValue(obj, value, Nothing)
                    End If
                Next
            Next
        End Sub

        Public Function GetInitialValue(ByVal name As String) As Object
            For Each prop As PropertyInfo In InitialValues.GetType.GetProperties()
                If prop.CanRead And prop.Name = name Then
                    Return prop.GetValue(InitialValues, Nothing)
                End If
            Next
            Return Nothing
        End Function

        Private Function RenderPropertyLabel(ByVal prop As CustomProperty) As DotNetNuke.UI.WebControls.PropertyLabelControl
            ' add label
            Dim l As New DotNetNuke.UI.WebControls.PropertyLabelControl
            l.ID = prop.Name & "_Help"

            Dim caption As String = Localization.GetString("lbl" & prop.Name, LocalResourceFile)
            If caption = "" Then caption = StringHelpers.Wordify(prop.Name)
            l.Caption = caption & " :"
            l.Font.Bold = True

            Dim help As String = Localization.GetString("lbl" & prop.Name & ".Help", LocalResourceFile)
            If help = "" Then help = String.Format("Enter a {0} ({1})", caption, prop.Type.ToString)
            l.HelpText = "<div class=""Help"">" & help & "</div>"

            Return l
        End Function

        Private Function RenderPropertyPrompt(ByVal prop As CustomProperty) As Control
            Dim container As New HtmlControls.HtmlGenericControl

            Dim prompt As Control = Nothing
            ' prompt
            Select Case prop.Type
                Case PropertyType.String
                    Dim tb As New TextBox
                    tb.ID = "tb" & prop.Name
                    tb.Columns = prop.Columns
                    tb.Rows = prop.Rows
                    If tb.Rows > 1 Then tb.TextMode = TextBoxMode.MultiLine
                    tb.Text = prop.Value
                    prompt = tb
                Case PropertyType.Boolean
                    Dim cb As New CheckBox
                    cb.ID = "cb" & prop.Name
                    Boolean.TryParse(prop.Value, cb.Checked)
                    prompt = cb
                Case PropertyType.Choice
                    Dim ddl As New DropDownList
                    ddl.ID = "ddl" & prop.Name
                    Dim li As ListItem
                    For Each ch As CustomPropertyChoice In prop.Choices
                        li = New ListItem(ch.Caption, ch.Value)
                        li.Selected = (li.Value = prop.Value)
                        ddl.Items.Add(li)
                    Next
                    prompt = ddl
                Case PropertyType.Directory
                    Dim ddl As New DropDownList
                    ddl.ID = "ddl" & prop.Name
                    Dim dir As New IO.DirectoryInfo(MapPath(prop.Directory))
                    Dim li As ListItem
                    For Each subdir As IO.DirectoryInfo In dir.GetDirectories
                        If Not subdir.Name.StartsWith("_") Then
                            li = New ListItem(StrConv(subdir.Name.Replace("-", " "), VbStrConv.ProperCase), subdir.Name)
                            li.Selected = (li.Value = prop.Value)
                            ddl.Items.Add(li)
                        End If
                    Next
                    prompt = ddl
                Case PropertyType.Files
                    Dim ddl As New DropDownList
                    ddl.ID = "ddl" & prop.Name
                    Dim dir As New IO.DirectoryInfo(MapPath(prop.Directory))
                    Dim li As ListItem
                    For Each subfile As IO.FileInfo In dir.GetFiles
                        If Not subfile.Name.StartsWith("_") Then
                            li = New ListItem(subfile.Name, subfile.Name)
                            li.Selected = (li.Value = prop.Value)
                            ddl.Items.Add(li)
                        End If
                    Next
                    prompt = ddl
            End Select

            container.Controls.Add(prompt)

            Dim addedBreak As Boolean = False
            If prop.ValidationExpression <> "" Then
                Dim validate As New RegularExpressionValidator
                validate.ValidationExpression = ValidationHelpers.CommonValidator(prop.ValidationExpression)
                validate.ErrorMessage = prop.ValidationMessage
                validate.ControlToValidate = prompt.ID
                validate.Display = ValidatorDisplay.Dynamic
                If Not addedBreak Then container.Controls.Add(New LiteralControl("<br />"))
                container.Controls.Add(validate)
                addedBreak = True
            End If

            If prop.Required Then
                Dim validate As New RequiredFieldValidator
                validate.ErrorMessage = String.Format("{0} is required", StringHelpers.Wordify(prop.Name))
                validate.ControlToValidate = prompt.ID
                validate.Display = ValidatorDisplay.Dynamic
                If Not addedBreak Then container.Controls.Add(New LiteralControl("<br />"))
                container.Controls.Add(validate)
                addedBreak = True
            End If

            Return container
        End Function

        Private Function RenderProperty(ByVal prop As CustomProperty) As HtmlTableRow

            ' prompt
            Dim prompt As Control = RenderPropertyPrompt(prop)

            ' label
            Dim label As DotNetNuke.UI.WebControls.PropertyLabelControl
            label = RenderPropertyLabel(prop)

            ' caption cell
            Dim tdCaption As HtmlTableCell = New HtmlTableCell
            tdCaption.VAlign = "Top"
            tdCaption.Width = "30%"
            tdCaption.Controls.Add(label)

            ' prompt cell
            Dim tdPrompt As HtmlTableCell = New HtmlTableCell
            tdPrompt.VAlign = "Top"
            tdPrompt.Align = "Left"
            tdPrompt.Controls.Add(prompt)

            ' add row
            Dim trProp As HtmlTableRow = New HtmlTableRow
            trProp.VAlign = "Top"
            trProp.Cells.Add(tdCaption)
            trProp.Cells.Add(tdPrompt)

            Return trProp
        End Function

        Private Function RenderGroupCellsTopToBottom(ByVal group As PropertyGroup) As HtmlTableRow
            Dim trGroupRow As HtmlTableRow = New HtmlTableRow

            For Each prop As CustomProperty In group.Properties
                If prop.Filter = "" Or prop.Filter.Contains(Filter) Then
                    prop.Value = GetInitialValue(prop.Name).ToString()

                    Dim propCell As New HtmlTableCell
                    propCell.VAlign = "Top"
                    propCell.Controls.Add(RenderPropertyLabel(prop))

                    propCell.Controls.Add(RenderPropertyPrompt(prop))

                    trGroupRow.Cells.Add(propCell)
                End If
            Next
            Return trGroupRow
        End Function

        Private Function RenderGroupCellsLeftToRight(ByVal group As PropertyGroup) As HtmlTableRow
            Dim trGroupRow As HtmlTableRow = New HtmlTableRow

            For Each prop As CustomProperty In group.Properties
                If prop.Filter = "" Or prop.Filter.Contains(Filter) Then
                    prop.Value = GetInitialValue(prop.Name).ToString()

                    Dim labelCell As New HtmlTableCell
                    labelCell.VAlign = "Top"
                    labelCell.Controls.Add(RenderPropertyLabel(prop))
                    trGroupRow.Cells.Add(labelCell)

                    Dim promptCell As New HtmlTableCell
                    promptCell.VAlign = "Top"
                    promptCell.Controls.Add(RenderPropertyPrompt(prop))

                    trGroupRow.Cells.Add(promptCell)
                End If
            Next
            Return trGroupRow
        End Function

        Public Sub RenderProperties()
            tblProperties.Rows.Clear()

            If Settings IsNot Nothing Then
                If Settings.Sections.Count > 0 Then
                    For Each section As Section In Settings.Sections
                        If section.Filter = "" Or section.Filter.Contains(Filter) Then
                            If PropertiesToShow(section) > 0 Then
                                Dim trSection As HtmlTableRow = New HtmlTableRow
                                Dim tdName As HtmlTableCell = New HtmlTableCell
                                tdName.ColSpan = 2
                                tdName.Controls.Add(New LiteralControl("<h3>" & section.Name & "</h3>"))
                                trSection.Cells.Add(tdName)

                                tblProperties.Rows.Add(trSection)

                                For Each group As PropertyGroup In section.PropertyGroups
                                    Dim tbGroup As HtmlTable = New HtmlTable
                                    tbGroup.Width = group.Width
                                    tbGroup.CellPadding = 1
                                    tbGroup.CellSpacing = 1
                                    Select Case group.Layout
                                        Case "TopToBottom"
                                            tbGroup.Rows.Add(RenderGroupCellsTopToBottom(group))
                                        Case "LeftToRight", ""
                                            tbGroup.Rows.Add(RenderGroupCellsLeftToRight(group))
                                    End Select

                                    Dim tcGroup As New HtmlTableCell
                                    tcGroup.Controls.Add(tbGroup)
                                    tcGroup.ColSpan = 2

                                    Dim trGroup As New HtmlTableRow
                                    trGroup.Cells.Add(tcGroup)
                                    tblProperties.Rows.Add(trGroup)
                                Next
                                Dim value As Object
                                For Each prop As CustomProperty In section.Properties
                                    If prop.Filter = "" Or prop.Filter.Contains(Filter) Then
                                        value = GetInitialValue(prop.Name)
                                        If Not value Is Nothing Then
                                            prop.Value = GetInitialValue(prop.Name).ToString()
                                        End If
                                        tblProperties.Rows.Add(RenderProperty(prop))
                                    End If
                                Next
                            End If
                        End If
                    Next
                Else
                    RenderNoProperties()
                End If
            Else
                RenderNoProperties()
            End If

        End Sub

        Private Function PropertiesToShow(section As Section) As Integer
            Dim count As Integer = 0
            For Each prop As CustomProperty In section.Properties
                If prop.Filter = "" Or prop.Filter.Contains(Filter) Then
                    count += 1
                End If
            Next
            For Each group As PropertyGroup In section.PropertyGroups
                For Each prop As CustomProperty In group.Properties
                    If prop.Filter = "" Or prop.Filter.Contains(Filter) Then
                        count += 1
                    End If
                Next
            Next

            Return count
        End Function

        Private Sub RenderNoProperties()
            Dim trProp As HtmlTableRow = New HtmlTableRow
            Dim tdCaption As HtmlTableCell = New HtmlTableCell

            Dim l As New LiteralControl(DotNetNuke.Services.Localization.Localization.GetString("NoCustomSettings", ResolveUrl("App_LocalResources/SQLViewPro")))
            tdCaption.Controls.Add(l)

            trProp.Cells.Add(tdCaption)
            tblProperties.Rows.Add(trProp)

        End Sub
    End Class
End Namespace
