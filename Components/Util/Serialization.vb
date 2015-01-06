Imports System.Xml.Serialization
Imports System.Text
Imports System.Xml
Imports System.IO

Namespace DNNStuff.SQLViewPro

    Public Class Serialization

#Region " XML Serialization Support"
        Private Shared Function UTF8ByteArrayToString(ByVal characters() As [Byte]) As [String]
            Dim encoding As New UTF8Encoding
            Dim constructedString As [String] = encoding.GetString(characters)
            Return constructedString
        End Function 'UTF8ByteArrayToString
        Private Shared Function StringToUTF8ByteArray(ByVal pXmlString As [String]) As [Byte]()
            Dim encoding As New UTF8Encoding
            Dim byteArray As [Byte]() = encoding.GetBytes(pXmlString)
            Return byteArray
        End Function 'StringToUTF8ByteArray

        Public Shared Function SerializeObjectOld(ByVal o As Object, ByVal t As Type) As String
            Try
                Dim ms As New MemoryStream
                Dim xs As New XmlSerializer(t)
                Dim writer As New XmlTextWriter(ms, New System.Text.UTF8Encoding)
                writer.Formatting = Formatting.Indented

                xs.Serialize(writer, o)
                ms = CType(writer.BaseStream, MemoryStream)
                Return UTF8ByteArrayToString(ms.ToArray())
            Catch e As Exception
                Return Nothing
            End Try
        End Function 'SerializeObject

        Public Shared Function SerializeObject(ByVal o As Object, ByVal t As Type) As String
            Try
                '                Dim ms As New MemoryStream
                Dim sw As New UTF8StringWriter()
                Dim xs As New XmlSerializer(t)

                Dim settings As New XmlWriterSettings()
                settings.NewLineHandling = NewLineHandling.Entitize
                settings.Indent = True
                settings.Encoding = New UTF8Encoding()
                settings.IndentChars = " "

                Dim writer As XmlWriter = XmlWriter.Create(sw, settings)
                xs.Serialize(writer, o)

                Return sw.ToString()

                'ms = CType(writer.BaseStream, MemoryStream)
                'Return UTF8ByteArrayToString(ms.ToArray())
            Catch e As Exception
                Return Nothing
            End Try
        End Function 'SerializeObject

        Public Shared Function DeserializeObjectOld(ByVal s As String, ByVal t As Type) As Object
            Dim xs As New XmlSerializer(t)
            Dim ms As New MemoryStream(StringToUTF8ByteArray(s))
            Return xs.Deserialize(ms)
        End Function 'DeserializeObject

        Public Shared Function DeserializeObject(ByVal s As String, ByVal t As Type) As Object
            Dim sr As New StringReader(s)
            Dim xs As New XmlSerializer(t)

            Dim settings As New XmlReaderSettings()
            Dim xr As XmlReader = XmlReader.Create(sr, settings)
            Return xs.Deserialize(xr)
        End Function 'DeserializeObject

        Friend Class UTF8StringWriter
            Inherits StringWriter
            Public Overrides ReadOnly Property Encoding As System.Text.Encoding
                Get
                    Return Encoding.UTF8
                End Get
            End Property
        End Class

#End Region

    End Class
End Namespace
