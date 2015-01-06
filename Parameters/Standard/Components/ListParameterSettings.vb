Imports System.Xml.Serialization
Imports System.Web.UI.WebControls

Namespace DNNStuff.SQLViewPro.StandardParameters

    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class ListParameterSettings
        Public Property ConnectionId() As Integer
        Public Property Command() As String
        Public Property List() As String
        Public Property [Default]() As String
        Public Property CommandCacheTimeout() As Integer = 60
    End Class

    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class DropDownListParameterSettings
        Inherits ListParameterSettings
        Public Property AutoPostback() As Boolean = False
    End Class

    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class ListBoxParameterSettings
        Inherits ListParameterSettings
        Public Property AutoPostback() As Boolean = False
        Public Property MultiSelect() As Boolean = False
        Public Property MultiSelectSize() As Integer = 5
    End Class

    <XmlRootAttribute(ElementName:="Settings", IsNullable:=False)> _
    Public Class FlowListParameterSettings
        Inherits ListParameterSettings

        Public Property RepeatLayout() As RepeatLayout
        Public Property RepeatDirection() As RepeatDirection
        Public Property RepeatColumns() As Integer = 2
    End Class


End Namespace
