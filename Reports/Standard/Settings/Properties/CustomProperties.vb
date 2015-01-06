Imports System.Xml
Imports System.Xml.Serialization
Imports System.Collections.Generic

Namespace DNNStuff.SQLViewPro

    Public Enum PropertyType As Integer
        [String] = 0
        Choice = 1
        [Boolean] = 2
        Directory = 3
        Files = 4
    End Enum

    <Serializable(), XmlRoot("settings")> _
    Public Class Settings
        Private _sections As List(Of Section)

        <XmlArray("sections"), XmlArrayItem("section", GetType(Section))> _
        Public Property Sections() As List(Of Section)
            Get
                Return _sections
            End Get
            Set(ByVal value As List(Of Section))
                _sections = value
            End Set
        End Property

        Shared Shadows Function Load(ByVal filename As String) As Settings
            Dim serializer As XmlSerializer = New XmlSerializer(GetType(Settings))
            Using reader As New IO.StreamReader(filename)
                Return DirectCast(serializer.Deserialize(reader), Settings)
            End Using
            Return New Settings
        End Function

        Public Function GetAllProperties() As List(Of CustomProperty)
            Dim all As List(Of CustomProperty) = New List(Of CustomProperty)
            For Each Section As Section In Sections
                For Each group As PropertyGroup In Section.PropertyGroups
                    For Each prop As CustomProperty In group.Properties
                        all.Add(prop)
                    Next
                Next
                For Each prop As CustomProperty In Section.Properties
                    all.Add(prop)
                Next
            Next
            Return all
        End Function
    End Class

    <Serializable()> _
    Public Class Section
        Private _name As String = ""
        <XmlAttribute("name")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _filter As String = ""
        <XmlAttribute("filter")> _
        Public Property Filter() As String
            Get
                Return _filter
            End Get
            Set(ByVal value As String)
                _filter = value
            End Set
        End Property

        Private _propertyGroups As List(Of PropertyGroup) = Nothing
        <XmlElement("propertygroup", GetType(PropertyGroup))> _
        Public Property PropertyGroups() As List(Of PropertyGroup)
            Get
                Return _propertyGroups
            End Get
            Set(ByVal value As List(Of PropertyGroup))
                _propertyGroups = value
            End Set
        End Property

        Private _properties As List(Of CustomProperty) = Nothing
        <XmlElement("property", GetType(CustomProperty))> _
        Public Property Properties() As List(Of CustomProperty)
            Get
                Return _properties
            End Get
            Set(ByVal value As List(Of CustomProperty))
                _properties = value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public Class PropertyGroup
        Private _name As String = ""
        <XmlAttribute("name")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _layout As String = "TopToBottom"
        <XmlAttribute("layout")> _
        Public Property Layout() As String
            Get
                Return _layout
            End Get
            Set(ByVal value As String)
                _layout = value
            End Set
        End Property

        Private _width As String = "100%"
        <XmlAttribute("width")> _
        Public Property Width() As String
            Get
                Return _width
            End Get
            Set(ByVal value As String)
                _width = value
            End Set
        End Property

        Private _properties As List(Of CustomProperty) = Nothing
        <XmlElement("property", GetType(CustomProperty))> _
        Public Property Properties() As List(Of CustomProperty)
            Get
                Return _properties
            End Get
            Set(ByVal value As List(Of CustomProperty))
                _properties = value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public Class CustomProperty

        Private _value As String = ""
        <XmlIgnore(), XmlAttribute("value")> _
        Public Property Value() As String
            Get
                If _value IsNot Nothing Then
                    If _value.Length > 0 Then
                        Return _value
                    End If
                End If
                Return _default
            End Get
            Set(ByVal value As String)

                _value = value
            End Set
        End Property

        Private _name As String = ""
        <XmlAttribute("name")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _type As PropertyType = PropertyType.String
        <XmlAttribute("type")> _
        Public Property Type() As PropertyType
            Get
                Return _type
            End Get
            Set(ByVal value As PropertyType)
                _type = value
            End Set
        End Property

        Private _default As String = ""
        <XmlAttribute("default")> _
        Public Property [Default]() As String
            Get
                Return _default
            End Get
            Set(ByVal value As String)
                _default = value
            End Set
        End Property

        Private _filter As String = ""
        <XmlAttribute("filter")> _
        Public Property Filter() As String
            Get
                Return _filter
            End Get
            Set(ByVal value As String)
                _filter = value
            End Set
        End Property

        Private _required As Boolean = False
        <XmlAttribute("required")> _
        Public Property Required() As Boolean
            Get
                Return _required
            End Get
            Set(ByVal value As Boolean)
                _required = value
            End Set
        End Property

        Private _validationExpression As String
        <XmlAttribute("validationexpression")> _
        Public Property ValidationExpression() As String
            Get
                Return _validationExpression
            End Get
            Set(ByVal value As String)
                _validationExpression = value
            End Set
        End Property

        Private _validationMessage As String
        <XmlAttribute("validationmessage")> _
        Public Property ValidationMessage() As String
            Get
                Return _validationMessage
            End Get
            Set(ByVal value As String)
                _validationMessage = value
            End Set
        End Property

        Private _directory As String = ""
        <XmlAttribute("directory")> _
        Public Property Directory() As String
            Get
                Return _directory
            End Get
            Set(ByVal value As String)
                _directory = value
            End Set
        End Property

        Private _columns As Integer = 40
        <XmlAttribute("columns")> _
        Public Property Columns() As Integer
            Get
                Return _columns
            End Get
            Set(ByVal value As Integer)
                _columns = value
            End Set
        End Property

        Private _rows As Integer = 1
        <XmlAttribute("rows")> _
        Public Property Rows() As Integer
            Get
                Return _rows
            End Get
            Set(ByVal value As Integer)
                _rows = value
            End Set
        End Property

        Private _choices As List(Of CustomPropertyChoice) = Nothing
        <XmlArray("choices"), XmlArrayItem("choice", GetType(CustomPropertyChoice))> _
        Public Property Choices() As List(Of CustomPropertyChoice)
            Get
                Return _choices
            End Get
            Set(ByVal value As List(Of CustomPropertyChoice))
                _choices = value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public Class CustomPropertyChoice
        Public Sub New(ByVal caption As String, ByVal value As String)
            _caption = caption
            _value = value
        End Sub
        Public Sub New()

        End Sub
        Private _value As String = ""
        <XmlAttribute("value")> _
        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property

        Private _caption As String = ""
        <XmlAttribute("caption")> _
        Public Property Caption() As String
            Get
                If _caption.Length > 0 Then
                    Return _caption
                End If
                Return _value
            End Get
            Set(ByVal value As String)
                _caption = value
            End Set
        End Property

    End Class

End Namespace
