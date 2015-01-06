
Imports DotNetNuke.Common.Utilities

Namespace DNNStuff.SQLViewPro.Services
    Public Class File
        Public Shared Function GetFilenameFromFileId(ByVal fileId As String) As String
            Dim fi As DotNetNuke.Services.FileSystem.IFileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(UrlUtils.GetParameterValue(fileId)))
            If fi IsNot Nothing Then Return fi.PhysicalPath
            Return ""
        End Function
    End Class
End Namespace

