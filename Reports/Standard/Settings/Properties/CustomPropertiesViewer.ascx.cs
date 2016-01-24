
using System.Web.UI.HtmlControls;
using System;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Web.UI;

using DotNetNuke.Services.Localization;
using DotNetNuke.UI.WebControls;


namespace DNNStuff.SQLViewPro
{
	
	public partial class CustomPropertiesViewer : UserControl
	{
		
#region  Public Properties
		public Settings Settings {get; set;}
	    public string LocalResourceFile { get; set; } = "";

	    public object InitialValues {get; set;}
	    public string Filter { get; set; } = "";

	    #endregion
		
#region  Public Methods
		public void InitializeValues()
		{
			foreach (var prop in InitialValues.GetType().GetProperties())
			{
				foreach (var customprop in Settings.GetAllProperties())
				{
					if (customprop.Name.ToUpper() == prop.Name.ToUpper())
					{
						customprop.Value = (string) (prop.GetValue(InitialValues, null));
					}
				}
			}
		}
#endregion
		
		protected void Page_Load(object sender, EventArgs e)
		{
			//RenderProperties()
		}
		
		/// <summary>
		/// Capture settings from controls and save to properties dictionary
		/// </summary>
		/// <remarks></remarks>
		public void SetProperties()
		{
			foreach (var customprop in Settings.GetAllProperties())
			{
				switch (customprop.Type)
				{
					case PropertyType.String:
						var tb = (TextBox) (FindControl("tb" + customprop.Name));
						if (tb != null)
						{
							customprop.Value = tb.Text;
						}
						break;
					case PropertyType.Boolean:
						var cb = (CheckBox) (FindControl("cb" + customprop.Name));
						if (cb != null)
						{
							customprop.Value = cb.Checked.ToString().ToLower();
						}
						break;
					case PropertyType.Choice:
					case PropertyType.Directory:
					case PropertyType.Files:
						var ddl = (DropDownList) (FindControl("ddl" + customprop.Name));
						if (ddl != null)
						{
							customprop.Value = ddl.SelectedValue;
						}
						break;
				}
			}
		}
		
		public void SetProperties(object obj)
		{
			SetProperties();
			
			foreach (var prop in obj.GetType().GetProperties())
			{
				foreach (var customprop in Settings.GetAllProperties())
				{
					if (customprop.Name == prop.Name)
					{
						var value = Convert.ChangeType(customprop.Value, prop.PropertyType);
						prop.SetValue(obj, value, null);
					}
				}
			}
		}
		
		public object GetInitialValue(string name)
		{
			foreach (var prop in InitialValues.GetType().GetProperties())
			{
				if (prop.CanRead && prop.Name == name)
				{
					return prop.GetValue(InitialValues, null);
				}
			}
			return null;
		}
		
		private PropertyLabelControl RenderPropertyLabel(CustomProperty prop)
		{
			// add label
			var l = new PropertyLabelControl();
			l.ID = prop.Name + "_Help";
			
			var caption = (string) (Localization.GetString("lbl" + prop.Name, LocalResourceFile));
			if (caption == "")
			{
				caption = StringHelpers.Wordify(prop.Name);
			}
			l.Caption = caption + " :";
			l.Font.Bold = true;
			
			var help = (string) (Localization.GetString("lbl" + prop.Name + ".Help", LocalResourceFile));
			if (help == "")
			{
				help = string.Format("Enter a {0} ({1})", caption, prop.Type.ToString());
			}
			l.HelpText = "<div class=\"Help\">" + help + "</div>";
			
			return l;
		}
		
		private Control RenderPropertyPrompt(CustomProperty prop)
		{
			var container = new HtmlGenericControl();
			
			Control prompt = null;
			// prompt
			switch (prop.Type)
			{
				case PropertyType.String:
					var tb = new TextBox();
					tb.ID = "tb" + prop.Name;
					tb.Columns = prop.Columns;
					tb.Rows = prop.Rows;
					if (tb.Rows > 1)
					{
						tb.TextMode = TextBoxMode.MultiLine;
					}
					tb.Text = prop.Value;
					prompt = tb;
					break;
				case PropertyType.Boolean:
					var cb = new CheckBox();
					cb.ID = "cb" + prop.Name;
					var temp_result = cb.Checked;
					bool.TryParse(prop.Value, out temp_result);
					cb.Checked = temp_result;
					prompt = cb;
					break;
				case PropertyType.Choice:
					var ddl_1 = new DropDownList();
					ddl_1.ID = "ddl" + prop.Name;
					var li_1 = default(ListItem);
					foreach (var ch in prop.Choices)
					{
						li_1 = new ListItem(ch.Caption, ch.Value);
						li_1.Selected = li_1.Value == prop.Value;
						ddl_1.Items.Add(li_1);
					}
					prompt = ddl_1;
					break;
				case PropertyType.Directory:
					var ddl_2 = new DropDownList();
					ddl_2.ID = "ddl" + prop.Name;
					var dir_1 = new System.IO.DirectoryInfo(MapPath(prop.Directory));
					var li_2 = default(ListItem);
					foreach (var subdir in dir_1.GetDirectories())
					{
						if (!subdir.Name.StartsWith("_"))
						{
							li_2 = new ListItem(Strings.StrConv(subdir.Name.Replace("-", " "), VbStrConv.ProperCase, 0), subdir.Name);
							li_2.Selected = li_2.Value == prop.Value;
							ddl_2.Items.Add(li_2);
						}
					}
					prompt = ddl_2;
					break;
				case PropertyType.Files:
					var ddl = new DropDownList();
					ddl.ID = "ddl" + prop.Name;
					var dir = new System.IO.DirectoryInfo(MapPath(prop.Directory));
					var li = default(ListItem);
					foreach (var subfile in dir.GetFiles())
					{
						if (!subfile.Name.StartsWith("_"))
						{
							li = new ListItem(subfile.Name, subfile.Name);
							li.Selected = li.Value == prop.Value;
							ddl.Items.Add(li);
						}
					}
					prompt = ddl;
					break;
			}
			
			container.Controls.Add(prompt);
			
			var addedBreak = false;
			if (prop.ValidationExpression != "")
			{
				var validate = new RegularExpressionValidator();
				validate.ValidationExpression = ValidationHelpers.CommonValidator(prop.ValidationExpression);
				validate.ErrorMessage = prop.ValidationMessage;
				validate.ControlToValidate = prompt.ID;
				validate.Display = ValidatorDisplay.Dynamic;
				if (!addedBreak)
				{
					container.Controls.Add(new LiteralControl("<br />"));
				}
				container.Controls.Add(validate);
				addedBreak = true;
			}
			
			if (prop.Required)
			{
				var validate = new RequiredFieldValidator();
				validate.ErrorMessage = string.Format("{0} is required", StringHelpers.Wordify(prop.Name));
				validate.ControlToValidate = prompt.ID;
				validate.Display = ValidatorDisplay.Dynamic;
				if (!addedBreak)
				{
					container.Controls.Add(new LiteralControl("<br />"));
				}
				container.Controls.Add(validate);
				addedBreak = true;
			}
			
			return container;
		}
		
		private HtmlTableRow RenderProperty(CustomProperty prop)
		{
			
			// prompt
			var prompt = RenderPropertyPrompt(prop);
			
			// label
			var label = default(PropertyLabelControl);
			label = RenderPropertyLabel(prop);
			
			// caption cell
			var tdCaption = new HtmlTableCell();
			tdCaption.VAlign = "Top";
			tdCaption.Width = "30%";
			tdCaption.Controls.Add(label);
			
			// prompt cell
			var tdPrompt = new HtmlTableCell();
			tdPrompt.VAlign = "Top";
			tdPrompt.Align = "Left";
			tdPrompt.Controls.Add(prompt);
			
			// add row
			var trProp = new HtmlTableRow();
			trProp.VAlign = "Top";
			trProp.Cells.Add(tdCaption);
			trProp.Cells.Add(tdPrompt);
			
			return trProp;
		}
		
		private HtmlTableRow RenderGroupCellsTopToBottom(PropertyGroup group)
		{
			var trGroupRow = new HtmlTableRow();
			
			foreach (var prop in group.Properties)
			{
				if (prop.Filter == "" || prop.Filter.Contains(Filter))
				{
					prop.Value = (string) (GetInitialValue(prop.Name).ToString());
					
					var propCell = new HtmlTableCell();
					propCell.VAlign = "Top";
					propCell.Controls.Add(RenderPropertyLabel(prop));
					
					propCell.Controls.Add(RenderPropertyPrompt(prop));
					
					trGroupRow.Cells.Add(propCell);
				}
			}
			return trGroupRow;
		}
		
		private HtmlTableRow RenderGroupCellsLeftToRight(PropertyGroup group)
		{
			var trGroupRow = new HtmlTableRow();
			
			foreach (var prop in group.Properties)
			{
				if (prop.Filter == "" || prop.Filter.Contains(Filter))
				{
					prop.Value = (string) (GetInitialValue(prop.Name).ToString());
					
					var labelCell = new HtmlTableCell();
					labelCell.VAlign = "Top";
					labelCell.Controls.Add(RenderPropertyLabel(prop));
					trGroupRow.Cells.Add(labelCell);
					
					var promptCell = new HtmlTableCell();
					promptCell.VAlign = "Top";
					promptCell.Controls.Add(RenderPropertyPrompt(prop));
					
					trGroupRow.Cells.Add(promptCell);
				}
			}
			return trGroupRow;
		}
		
		public void RenderProperties()
		{
			tblProperties.Rows.Clear();
			
			if (Settings != null)
			{
				if (Settings.Sections.Count > 0)
				{
					foreach (var section in Settings.Sections)
					{
						if (section.Filter == "" || section.Filter.Contains(Filter))
						{
							if (PropertiesToShow(section) > 0)
							{
								var trSection = new HtmlTableRow();
								var tdName = new HtmlTableCell();
								tdName.ColSpan = 2;
								tdName.Controls.Add(new LiteralControl("<h3>" + section.Name + "</h3>"));
								trSection.Cells.Add(tdName);
								
								tblProperties.Rows.Add(trSection);
								
								foreach (var group in section.PropertyGroups)
								{
									var tbGroup = new HtmlTable();
									tbGroup.Width = group.Width;
									tbGroup.CellPadding = 1;
									tbGroup.CellSpacing = 1;
									switch (group.Layout)
									{
										case "TopToBottom":
											tbGroup.Rows.Add(RenderGroupCellsTopToBottom(group));
											break;
										case "LeftToRight":
										case "":
											tbGroup.Rows.Add(RenderGroupCellsLeftToRight(group));
											break;
									}
									
									var tcGroup = new HtmlTableCell();
									tcGroup.Controls.Add(tbGroup);
									tcGroup.ColSpan = 2;
									
									var trGroup = new HtmlTableRow();
									trGroup.Cells.Add(tcGroup);
									tblProperties.Rows.Add(trGroup);
								}
								var value = default(object);
								foreach (var prop in section.Properties)
								{
									if (prop.Filter == "" || prop.Filter.Contains(Filter))
									{
										value = GetInitialValue(prop.Name);
										if (value != null)
										{
											prop.Value = (string) (GetInitialValue(prop.Name).ToString());
										}
										tblProperties.Rows.Add(RenderProperty(prop));
									}
								}
							}
						}
					}
				}
				else
				{
					RenderNoProperties();
				}
			}
			else
			{
				RenderNoProperties();
			}
			
		}
		
		private int PropertiesToShow(Section section)
		{
			var count = 0;
			foreach (var prop in section.Properties)
			{
				if (prop.Filter == "" || prop.Filter.Contains(Filter))
				{
					count++;
				}
			}
			foreach (var group in section.PropertyGroups)
			{
				foreach (var prop in group.Properties)
				{
					if (prop.Filter == "" || prop.Filter.Contains(Filter))
					{
						count++;
					}
				}
			}
			
			return count;
		}
		
		private void RenderNoProperties()
		{
			var trProp = new HtmlTableRow();
			var tdCaption = new HtmlTableCell();
			
			var l = new LiteralControl((string) (Localization.GetString("NoCustomSettings", ResolveUrl("App_LocalResources/SQLViewPro"))));
			tdCaption.Controls.Add(l);
			
			trProp.Cells.Add(tdCaption);
			tblProperties.Rows.Add(trProp);
			
		}
	}
}

