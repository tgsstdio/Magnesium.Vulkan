using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSparseBufferMemoryBindInfo
	{
		public UInt64 buffer { get; set; }
		public UInt32 bindCount { get; set; }
		public IntPtr pBinds { get; set; } // VkSparseMemoryBind
	}
}
