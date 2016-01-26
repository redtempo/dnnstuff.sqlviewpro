

using System;

using System.Xml.Serialization;
using System.Collections.Generic;


namespace DNNStuff.SQLViewPro
{
	
	public enum PropertyType
	{
		@String = 0,
		Choice = 1,
		@Boolean = 2,
		Directory = 3,
		Files = 4
	}
	
	[Serializable(), XmlRoot("settings")]public class Settings
	{
		private List<Section> _sections;
		
		[XmlArray("sections"), XmlArrayItem("section", typeof(Section))]public List<Section> Sections
		{
			get
			{
				return _sections;
			}
			set
			{
				_sections = value;
			}
		}
		
		static new public Settings Load(string filename)
		{
			var serializer = new XmlSerializer(typeof(Settings));
			using (var reader = new System.IO.StreamReader(filename))
			{
				return ((Settings) (serializer.Deserialize(reader)));
			}
			
			return new Settings();
		}
		
		public List<CustomProperty> GetAllProperties()
		{
			var all = new List<CustomProperty>();
			foreach (var section in Sections)
			{
				foreach (var group in section.PropertyGroups)
				{
					foreach (var prop in group.Properties)
					{
						all.Add(prop);
					}
				}
				foreach (var prop in section.Properties)
				{
					all.Add(prop);
				}
			}
			return all;
		}
	}
	
	[Serializable()]public class Section
	{
		private string _name = "";
		[XmlAttribute("name")]public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		
		private string _filter = "";
		[XmlAttribute("filter")]public string Filter
		{
			get
			{
				return _filter;
			}
			set
			{
				_filter = value;
			}
		}
		
		private List<PropertyGroup> _propertyGroups = null;
		[XmlElement("propertygroup", typeof(PropertyGroup))]public List<PropertyGroup> PropertyGroups
		{
			get
			{
				return _propertyGroups;
			}
			set
			{
				_propertyGroups = value;
			}
		}
		
		private List<CustomProperty> _properties = null;
		[XmlElement("property", typeof(CustomProperty))]public List<CustomProperty> Properties
		{
			get
			{
				return _properties;
			}
			set
			{
				_properties = value;
			}
		}
	}
	
	[Serializable()]public class PropertyGroup
	{
		private string _name = "";
		[XmlAttribute("name")]public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		
		private string _layout = "TopToBottom";
		[XmlAttribute("layout")]public string Layout
		{
			get
			{
				return _layout;
			}
			set
			{
				_layout = value;
			}
		}
		
		private string _width = "100%";
		[XmlAttribute("width")]public string Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
			}
		}
		
		private List<CustomProperty> _properties = null;
		[XmlElement("property", typeof(CustomProperty))]public List<CustomProperty> Properties
		{
			get
			{
				return _properties;
			}
			set
			{
				_properties = value;
			}
		}
	}
	
	[Serializable()]public class CustomProperty
	{
		
		private string _value = "";
		[XmlIgnore(), XmlAttribute("value")]public string Value
		{
			get
			{
				if (_value != null)
				{
					if (_value.Length > 0)
					{
						return _value;
					}
				}
				return _default;
			}
			set
			{
				
				_value = value;
			}
		}
		
		private string _name = "";
		[XmlAttribute("name")]public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		
		private PropertyType _type = PropertyType.String;
		[XmlAttribute("type")]public PropertyType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}
		
		private string _default = "";
		[XmlAttribute("default")]public string Default
		{
			get
			{
				return _default;
			}
			set
			{
				_default = value;
			}
		}
		
		private string _filter = "";
		[XmlAttribute("filter")]public string Filter
		{
			get
			{
				return _filter;
			}
			set
			{
				_filter = value;
			}
		}
		
		private bool _required = false;
		[XmlAttribute("required")]public bool Required
		{
			get
			{
				return _required;
			}
			set
			{
				_required = value;
			}
		}
		
		private string _validationExpression;
		[XmlAttribute("validationexpression")]public string ValidationExpression
		{
			get
			{
				return _validationExpression;
			}
			set
			{
				_validationExpression = value;
			}
		}
		
		private string _validationMessage;
		[XmlAttribute("validationmessage")]public string ValidationMessage
		{
			get
			{
				return _validationMessage;
			}
			set
			{
				_validationMessage = value;
			}
		}
		
		private string _directory = "";
		[XmlAttribute("directory")]public string Directory
		{
			get
			{
				return _directory;
			}
			set
			{
				_directory = value;
			}
		}
		
		private int _columns = 40;
		[XmlAttribute("columns")]public int Columns
		{
			get
			{
				return _columns;
			}
			set
			{
				_columns = value;
			}
		}
		
		private int _rows = 1;
		[XmlAttribute("rows")]public int Rows
		{
			get
			{
				return _rows;
			}
			set
			{
				_rows = value;
			}
		}
		
		private List<CustomPropertyChoice> _choices = null;
		[XmlArray("choices"), XmlArrayItem("choice", typeof(CustomPropertyChoice))]public List<CustomPropertyChoice> Choices
		{
			get
			{
				return _choices;
			}
			set
			{
				_choices = value;
			}
		}
	}
	
	[Serializable()]public class CustomPropertyChoice
	{
		public CustomPropertyChoice(string caption, string value)
		{
			_caption = caption;
			_value = value;
		}
		public CustomPropertyChoice()
		{
			
		}
		private string _value = "";
		[XmlAttribute("value")]public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
		
		private string _caption = "";
		[XmlAttribute("caption")]public string Caption
		{
			get
			{
				if (_caption.Length > 0)
				{
					return _caption;
				}
				return _value;
			}
			set
			{
				_caption = value;
			}
		}
		
	}
	
}

