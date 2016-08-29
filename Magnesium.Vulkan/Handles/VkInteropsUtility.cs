﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Magnesium.Vulkan
{
	internal static class VkInteropsUtility
	{
		// Using Hans Passant's answer
		// http://stackoverflow.com/questions/10773440/conversion-in-net-native-utf-8-managed-string
		internal static IntPtr NativeUtf8FromString(string managedString)
		{
			int len = System.Text.Encoding.UTF8.GetByteCount(managedString);
			byte[] buffer = new byte[len + 1];
			System.Text.Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
			IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
			return nativeUtf8;
		}

		// Using Hans Passant's answer
		// http://stackoverflow.com/questions/10773440/conversion-in-net-native-utf-8-managed-string
		public static string StringFromNativeUtf8(IntPtr nativeUtf8)
		{
			int len = 0;
			while (Marshal.ReadByte(nativeUtf8, len) != 0) ++len;
			byte[] buffer = new byte[len];
			Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
			return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
		}

		internal static IntPtr ExtractUInt64HandleArray<TVkType>(TVkType[] arrayItems, Func<TVkType, UInt64> extractHandle)
		{
			return ExtractHandleArray<TVkType, UInt64>(arrayItems, extractHandle);
		}

		internal static IntPtr ExtractIntPtrHandleArray<TVkType>(TVkType[] arrayItems, Func<TVkType, IntPtr> extractHandle)
		{
			return ExtractHandleArray<TVkType, IntPtr>(arrayItems, extractHandle);
		}

		// REMEMBER TO FREE ALLOCATED MEMORY i.e. Marshal.FreeHGlobal
		internal static IntPtr AllocateUInt32Array(UInt32[] values)
		{
			Debug.Assert(values != null);

			var arrayLength = values.Length;
			var arraySizeInBytes = (int)(arrayLength * sizeof(UInt32));
			var arrayPtr = Marshal.AllocHGlobal(arraySizeInBytes);

			var tempBuffer = new byte[arraySizeInBytes];
			Buffer.BlockCopy(values, 0, tempBuffer, 0, arraySizeInBytes);
			Marshal.Copy(tempBuffer, 0, arrayPtr, arraySizeInBytes);

			return arrayPtr;
		}

		/// DON'T FORGET TO Marshal.FreeHGlobal the returned value
		private static IntPtr ExtractHandleArray<TVkType, TVkInternalHandle>(TVkType[] arrayItems, Func<TVkType, TVkInternalHandle> extractHandle)
		{
			Debug.Assert(arrayItems != null);
			Debug.Assert(extractHandle != null);

			var arrayLength = arrayItems.Length;

			var arrayStride = Marshal.SizeOf(typeof(TVkInternalHandle));
			var arraySizeInBytes = arrayLength * arrayStride;
			var array = Marshal.AllocHGlobal(arraySizeInBytes);

			var vkHandles = new TVkInternalHandle[arrayLength];
			for (var j = 0; j < arrayLength; ++j)
			{
				vkHandles[j] = extractHandle(arrayItems[j]);
			}

			var tempBuffer = new byte[arraySizeInBytes];
			Buffer.BlockCopy(vkHandles, 0, tempBuffer, 0, arraySizeInBytes);
			Marshal.Copy(tempBuffer, 0, array, arraySizeInBytes);

			return array;
		}

		internal static IntPtr AllocateNestedHGlobalArray<TMgReference, TVkData>(List<IntPtr> allocatedItems, TMgReference[] references, Func<List<IntPtr>, TMgReference, TVkData> initializeData)
		{
			var stride = Marshal.SizeOf(typeof(TVkData));
			var dependencyCount = references.Length;
			var dstArray = Marshal.AllocHGlobal(stride * dependencyCount);

			var offset = 0;
			for (var i = 0; i < dependencyCount; ++i)
			{
				var temp = initializeData(allocatedItems, references[i]);
				var localDest = IntPtr.Add(dstArray, offset);
				Marshal.StructureToPtr(temp, localDest, false);
				offset += stride;
			}
			return dstArray;
		}

		internal static IntPtr AllocateHGlobalArray<TMgReference, TVkData>(TMgReference[] references, Func<TMgReference, TVkData> initializeData)
		{
			var stride = Marshal.SizeOf(typeof(TVkData));
			var dependencyCount = references.Length;
			var dstArray = Marshal.AllocHGlobal(stride * dependencyCount);

			var offset = 0;
			for (var i = 0; i < dependencyCount; ++i)
			{
				var temp = initializeData(references[i]);
				var localDest = IntPtr.Add(dstArray, offset);
				Marshal.StructureToPtr(temp, localDest, false);
				offset += stride;
			}

			return dstArray;
		}
	}
}
