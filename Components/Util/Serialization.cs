

using System;


using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.IO;


namespace DNNStuff.SQLViewPro
{
	
	public class Serialization
	{
		
#region  XML Serialization Support
		private static string UTF8ByteArrayToString(byte[] characters)
		{
			var encoding = new UTF8Encoding();
			var constructedString = encoding.GetString(characters);
			return constructedString;
		} //UTF8ByteArrayToString
		private static byte[] StringToUTF8ByteArray(string pXmlString)
		{
			var encoding = new UTF8Encoding();
			var byteArray = encoding.GetBytes(pXmlString);
			return byteArray;
		} //StringToUTF8ByteArray
		
		public static string SerializeObjectOld(object o, Type t)
		{
			try
			{
				var ms = new MemoryStream();
				var xs = new XmlSerializer(t);
				var writer = new XmlTextWriter(ms, new UTF8Encoding());
				writer.Formatting = Formatting.Indented;
				
				xs.Serialize(writer, o);
				ms = (MemoryStream) writer.BaseStream;
				return UTF8ByteArrayToString(ms.ToArray());
			}
			catch (Exception)
			{
				return null;
			}
		} //SerializeObject
		
		public static string SerializeObject(object o, Type t)
		{
			try
			{
				//                Dim ms As New MemoryStream
				var sw = new UTF8StringWriter();
				var xs = new XmlSerializer(t);
				
				var settings = new XmlWriterSettings();
				settings.NewLineHandling = NewLineHandling.Entitize;
				settings.Indent = true;
				settings.Encoding = new UTF8Encoding();
				settings.IndentChars = " ";
				
				var writer = XmlWriter.Create(sw, settings);
				xs.Serialize(writer, o);
				
				return sw.ToString();
				
				//ms = CType(writer.BaseStream, MemoryStream)
				//Return UTF8ByteArrayToString(ms.ToArray())
			}
			catch (Exception)
			{
				return null;
			}
		} //SerializeObject
		
		public static object DeserializeObjectOld(string s, Type t)
		{
			var xs = new XmlSerializer(t);
			var ms = new MemoryStream(StringToUTF8ByteArray(s));
			return xs.Deserialize(ms);
		} //DeserializeObject
		
		public static object DeserializeObject(string s, Type t)
		{
			var sr = new StringReader(s);
			var xs = new XmlSerializer(t);
			
			var settings = new XmlReaderSettings();
			var xr = XmlReader.Create(sr, settings);
			return xs.Deserialize(xr);
		} //DeserializeObject
		
		internal class UTF8StringWriter : StringWriter
		{
			public override Encoding Encoding => Encoding.UTF8;
		}
		
#endregion
		
	}
}

