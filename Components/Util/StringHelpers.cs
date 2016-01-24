

using Microsoft.VisualBasic;


using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;


public class StringHelpers
{
#region  Strings
	public static string Wordify(string pascalCaseString)
	{
		Regex r = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
		return r.Replace(pascalCaseString, " ${x}");
	}
	
	public static string EscapeSingleQuotes(string value)
	{
		return value.Replace("\'", "\'\'");
	}
	
	public static string XmlEncode(string buf)
	{
		StringBuilder textOut = new StringBuilder();
		char c = default(char);
		if (buf.Trim() == null || buf == string.Empty)
		{
			return string.Empty;
		}
		for (int i = 0; i <= buf.Length - 1; i++)
		{
			c = buf[i];
			if (Entities.ContainsKey(c))
			{
				textOut.Append(Entities[c]);
			}
			else if ((Strings.AscW(c) == 0x9 || Strings.AscW(c) == 0xA || Strings.AscW(c) == 0xD) || ((Strings.AscW(c) >= 0x20) && (Strings.AscW(c) <= 0xD7FF)) || ((Strings.AscW(c) >= 0xE000) && (Strings.AscW(c) <= 0xFFFD)) || ((Strings.AscW(c) >= 0x10000) && (Strings.AscW(c) <= 0x10FFFF)))
			{
				textOut.Append(c);
			}
		}
		return textOut.ToString();
		
	}
	
	static readonly Dictionary<char, string> Entities = new Dictionary<char, string>();
	
	public static string RemoveLastCharacter(string value)
	{
		if (value.Length < 1)
		{
			return value;
		}
		return value.Substring(0, value.Length - 1);
	}
	
	public static int DefaultInt32FromString(string s, int @default)
	{
		int result = default(int);
		if (int.TryParse(s, out result))
		{
			return result;
		}
		return @default;
	}
	
	public static string CleanName(string name)
	{
		
		const string strBadChars = ". ~`!@#$%^&*()-_+={[}]|\\:;<,>?/" + "\u0022" + "\u0027";
		
		int intCounter = default(int);
		for (intCounter = 0; intCounter <= strBadChars.Length - 1; intCounter++)
		{
			name = name.Replace(strBadChars.Substring(intCounter, 1), "");
		}
		
		return name;
		
	}
	
	public static string FindNthField(string s, char separator, int position)
	{
		string[] splits = s.Split(separator);
		if (splits.Length < position)
		{
			return null;
		}
		return splits[position - 1];
	}
	
	public static string FindLastField(string s, char separator)
	{
		string[] splits = s.Split(separator);
		return FindNthField(s, separator, splits.Length);
	}
	
	public static Dictionary<string, string> ToDictionary(string s, char separator)
	{
		Dictionary<string, string> d = new Dictionary<string, string>();
		foreach (string temp in s.Split(separator))
		{
			int index = temp.IndexOf('=');
			if (index > -1)
			{
				d.Add(temp.Substring(0, index), temp.Substring(index + 1));
			}
		}
		return d;
	}
#endregion
}

