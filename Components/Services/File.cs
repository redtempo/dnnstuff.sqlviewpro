

using System;


using DotNetNuke.Common.Utilities;
	
	
	
	namespace DNNStuff.SQLViewPro.Services
	{
		public class File
		{
			public static string GetFilenameFromFileId(string fileId)
			{
				DotNetNuke.Services.FileSystem.IFileInfo fi = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(UrlUtils.GetParameterValue(fileId)));
			if (fi != null)
			{
				return fi.PhysicalPath;
			}
			return "";
		}
	}
}


