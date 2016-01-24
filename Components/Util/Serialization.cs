

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
			UTF8Encoding encoding = new UTF8Encoding();
			string constructedString = encoding.GetString(characters);
			return constructedString;
		} //UTF8ByteArrayToString
		private static byte[] StringToUTF8ByteArray(string pXmlString)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			byte[] byteArray = encoding.GetBytes(pXmlString);
			return byteArray;
		} //StringToUTF8ByteArray
		
		public static string SerializeObjectOld(object o, Type t)
		{
			try
			{
				MemoryStream ms = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(t);
				XmlTextWriter writer = new XmlTextWriter(ms, new UTF8Encoding());
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
				UTF8StringWriter sw = new UTF8StringWriter();
				XmlSerializer xs = new XmlSerializer(t);
				
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.NewLineHandling = NewLineHandling.Entitize;
				settings.Indent = true;
				settings.Encoding = new UTF8Encoding();
				settings.IndentChars = " ";
				
				XmlWriter writer = XmlWriter.Create(sw, settings);
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
			XmlSerializer xs = new XmlSerializer(t);
			MemoryStream ms = new MemoryStream(StringToUTF8ByteArray(s));
			return xs.Deserialize(ms);
		} //DeserializeObject
		
		public static object DeserializeObject(string s, Type t)
		{
			StringReader sr = new StringReader(s);
			XmlSerializer xs = new XmlSerializer(t);
			
			XmlReaderSettings settings = new XmlReaderSettings();
			XmlReader xr = XmlReader.Create(sr, settings);
			return xs.Deserialize(xr);
		} //DeserializeObject
		
		internal class UTF8StringWriter : StringWriter
		{
			public override Encoding Encoding
			{
				get
				{
					return Encoding.UTF8;
				}
			}
		}
		
#endregion
		
	}
}

