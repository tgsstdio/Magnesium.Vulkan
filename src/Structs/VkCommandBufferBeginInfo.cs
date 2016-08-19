using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkCommandBufferBeginInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkCommandBufferUsageFlags flags { get; set; }
		public VkCommandBufferInheritanceInfo pInheritanceInfo { get; set; }
	}
}