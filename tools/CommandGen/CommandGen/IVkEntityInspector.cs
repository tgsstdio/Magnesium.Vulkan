﻿using System.Collections.Generic;
using System.Xml.Linq;

namespace CommandGen
{
	public interface IVkEntityInspector
	{
		void Inspect(XElement root);
		IDictionary<string, string> Translations { get; }
		IDictionary<string, VkHandleInfo> Handles { get; }
		IDictionary<string, VkEnumInfo> Enums { get; }
		ISet<string> BlittableTypes { get; }
		string GetTypeCsName(string name, string label);
	}
}