




public class ValidationHelpers
{
	public static string CommonValidator(string key)
	{
		switch (key)
		{
			case "Integer":
				return "[-|+]?\\b\\d+\\b";
			case "PositiveInteger":
				return "\\b\\d+\\b";
			case "Color":
			case "Colour":
				return "^([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$";
		}
		return key;
	}
}

