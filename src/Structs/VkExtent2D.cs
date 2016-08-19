using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkExtent2D
	{
		public UInt32 width { get; set; }
		public UInt32 height { get; set; }
	}
}