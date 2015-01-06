Imports System

Namespace DNNStuff.SQLViewPro
    Public Class Common
        ' constants
        Public Const CompanyName As String = "DNNStuff"
        Public Const ProductName As String = "SQLViewPro"
        Public Const CompanyUrl As String = "http://www.dnnstuff.com"
        Public Const TrialStyle As String = "display:block;visibility:visible;color:black;position:relative;left:0;top:0;margin:0;padding:0;font:1.0em;line-height:1;"

        ' standard menus
        Public Const ViewOptions As String = "ViewOptions"

        ''' <summary>
        ''' TrialWarning - builds up the Trial warning inserted when using the Trial version of the module
        ''' </summary>
        Public Shared Function TrialWarning() As String

            Dim sb As New Text.StringBuilder
            sb.AppendFormat("<p>Thank you for evaluating <a style=""text-decoration:underline"" target=""_blank"" ")
            sb.AppendFormat("title=""{0}"" ", ProductName)
            sb.AppendFormat("href=""{0}/{2}.aspx?utm_source={1}&utm_medium=trial&utm_campaign={1}"">{2}</a>. ", CompanyUrl, CompanyName, ProductName)
            sb.AppendFormat("If after your evaluation you wish to support great DotNetNuke software and obtain a licensed copy of all DNNStuff modules, ")
            sb.AppendFormat("please visit the store to <a style=""text-decoration:underline"" target=""_blank"" ")
            sb.AppendFormat("title=""{0}"" ", CompanyName)
            sb.AppendFormat("href=""{0}/store.aspx?utm_source={1}&utm_medium=trial&utm_campaign={2}", CompanyUrl, CompanyName, ProductName)
            sb.AppendFormat(""">purchase a membership</a>. Use discount code <strong>'TRIAL'</strong> at checkout for 10% ")
            sb.AppendFormat("off!</p><hr />")

            Return sb.ToString
        End Function

        ''' <summary>
        ''' AddTrialNotice - returns a control containing the trial warning
        ''' </summary>
        Public Shared Sub AddTrialNotice(ByVal ParentControl As Control)

            Dim ctrl As New HtmlControls.HtmlGenericControl("div")
            With ctrl
                .InnerHtml = TrialWarning()
                .Attributes.Add("style", Common.TrialStyle)
            End With

            ParentControl.Controls.Add(ctrl)

        End Sub
    End Class

End Namespace
