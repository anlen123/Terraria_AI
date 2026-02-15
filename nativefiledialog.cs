using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x0200000F RID: 15
public static class nativefiledialog
{
	// Token: 0x0600004C RID: 76 RVA: 0x00002EEC File Offset: 0x000010EC
	private static int Utf8Size(string str)
	{
		return str.Length * 4 + 1;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00002EF8 File Offset: 0x000010F8
	private unsafe static byte* Utf8EncodeNullable(string str)
	{
		if (str == null)
		{
			return null;
		}
		int num = nativefiledialog.Utf8Size(str);
		byte* ptr = (byte*)((void*)Marshal.AllocHGlobal(num));
		fixed (string text = str)
		{
			char* ptr2 = text;
			if (ptr2 != null)
			{
				ptr2 += RuntimeHelpers.OffsetToStringData / 2;
			}
			Encoding.UTF8.GetBytes(ptr2, (str != null) ? (str.Length + 1) : 0, ptr, num);
		}
		return ptr;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00002F50 File Offset: 0x00001150
	private unsafe static string UTF8_ToManaged(IntPtr s, bool freePtr = false)
	{
		if (s == IntPtr.Zero)
		{
			return null;
		}
		byte* ptr = (byte*)((void*)s);
		while (*ptr != 0)
		{
			ptr++;
		}
		int num = (int)((long)((byte*)ptr - (byte*)((void*)s)));
		if (num == 0)
		{
			return string.Empty;
		}
		char* ptr2 = stackalloc char[checked(unchecked((UIntPtr)num) * 2)];
		int chars = Encoding.UTF8.GetChars((byte*)((void*)s), num, ptr2, num);
		string result = new string(ptr2, 0, chars);
		if (freePtr)
		{
			nativefiledialog.free(s);
		}
		return result;
	}

	// Token: 0x0600004F RID: 79
	[DllImport("msvcrt", CallingConvention = CallingConvention.Cdecl)]
	private static extern void free(IntPtr ptr);

	// Token: 0x06000050 RID: 80
	[DllImport("nfd", CallingConvention = CallingConvention.Cdecl, EntryPoint = "NFD_OpenDialog")]
	private unsafe static extern nativefiledialog.nfdresult_t INTERNAL_NFD_OpenDialog(byte* filterList, byte* defaultPath, out IntPtr outPath);

	// Token: 0x06000051 RID: 81 RVA: 0x00002FC0 File Offset: 0x000011C0
	public unsafe static nativefiledialog.nfdresult_t NFD_OpenDialog(string filterList, string defaultPath, out string outPath)
	{
		byte* ptr = nativefiledialog.Utf8EncodeNullable(filterList);
		byte* ptr2 = nativefiledialog.Utf8EncodeNullable(defaultPath);
		IntPtr s;
		nativefiledialog.nfdresult_t result = nativefiledialog.INTERNAL_NFD_OpenDialog(ptr, ptr2, out s);
		Marshal.FreeHGlobal((IntPtr)((void*)ptr));
		Marshal.FreeHGlobal((IntPtr)((void*)ptr2));
		outPath = nativefiledialog.UTF8_ToManaged(s, true);
		return result;
	}

	// Token: 0x06000052 RID: 82
	[DllImport("nfd", CallingConvention = CallingConvention.Cdecl, EntryPoint = "NFD_OpenDialogMultiple")]
	private unsafe static extern nativefiledialog.nfdresult_t INTERNAL_NFD_OpenDialogMultiple(byte* filterList, byte* defaultPath, out nativefiledialog.nfdpathset_t outPaths);

	// Token: 0x06000053 RID: 83 RVA: 0x00003004 File Offset: 0x00001204
	public unsafe static nativefiledialog.nfdresult_t NFD_OpenDialogMultiple(string filterList, string defaultPath, out nativefiledialog.nfdpathset_t outPaths)
	{
		byte* ptr = nativefiledialog.Utf8EncodeNullable(filterList);
		byte* ptr2 = nativefiledialog.Utf8EncodeNullable(defaultPath);
		nativefiledialog.nfdresult_t result = nativefiledialog.INTERNAL_NFD_OpenDialogMultiple(ptr, ptr2, out outPaths);
		Marshal.FreeHGlobal((IntPtr)((void*)ptr));
		Marshal.FreeHGlobal((IntPtr)((void*)ptr2));
		return result;
	}

	// Token: 0x06000054 RID: 84
	[DllImport("nfd", CallingConvention = CallingConvention.Cdecl, EntryPoint = "NFD_SaveDialog")]
	private unsafe static extern nativefiledialog.nfdresult_t INTERNAL_NFD_SaveDialog(byte* filterList, byte* defaultPath, out IntPtr outPath);

	// Token: 0x06000055 RID: 85 RVA: 0x00003040 File Offset: 0x00001240
	public unsafe static nativefiledialog.nfdresult_t NFD_SaveDialog(string filterList, string defaultPath, out string outPath)
	{
		byte* ptr = nativefiledialog.Utf8EncodeNullable(filterList);
		byte* ptr2 = nativefiledialog.Utf8EncodeNullable(defaultPath);
		IntPtr s;
		nativefiledialog.nfdresult_t result = nativefiledialog.INTERNAL_NFD_SaveDialog(ptr, ptr2, out s);
		Marshal.FreeHGlobal((IntPtr)((void*)ptr));
		Marshal.FreeHGlobal((IntPtr)((void*)ptr2));
		outPath = nativefiledialog.UTF8_ToManaged(s, true);
		return result;
	}

	// Token: 0x06000056 RID: 86
	[DllImport("nfd", CallingConvention = CallingConvention.Cdecl, EntryPoint = "NFD_PickFolder")]
	private unsafe static extern nativefiledialog.nfdresult_t INTERNAL_NFD_PickFolder(byte* defaultPath, out IntPtr outPath);

	// Token: 0x06000057 RID: 87 RVA: 0x00003084 File Offset: 0x00001284
	public unsafe static nativefiledialog.nfdresult_t NFD_PickFolder(string defaultPath, out string outPath)
	{
		byte* ptr = nativefiledialog.Utf8EncodeNullable(defaultPath);
		IntPtr s;
		nativefiledialog.nfdresult_t result = nativefiledialog.INTERNAL_NFD_PickFolder(ptr, out s);
		Marshal.FreeHGlobal((IntPtr)((void*)ptr));
		outPath = nativefiledialog.UTF8_ToManaged(s, true);
		return result;
	}

	// Token: 0x06000058 RID: 88
	[DllImport("nfd", CallingConvention = CallingConvention.Cdecl, EntryPoint = "NFD_GetError")]
	private static extern IntPtr INTERNAL_NFD_GetError();

	// Token: 0x06000059 RID: 89 RVA: 0x000030B4 File Offset: 0x000012B4
	public static string NFD_GetError()
	{
		return nativefiledialog.UTF8_ToManaged(nativefiledialog.INTERNAL_NFD_GetError(), false);
	}

	// Token: 0x0600005A RID: 90
	[DllImport("nfd", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr NFD_PathSet_GetCount(ref nativefiledialog.nfdpathset_t pathset);

	// Token: 0x0600005B RID: 91
	[DllImport("nfd", CallingConvention = CallingConvention.Cdecl, EntryPoint = "NFD_PathSet_GetPath")]
	private static extern IntPtr INTERNAL_NFD_PathSet_GetPath(ref nativefiledialog.nfdpathset_t pathset, IntPtr index);

	// Token: 0x0600005C RID: 92 RVA: 0x000030C1 File Offset: 0x000012C1
	public static string NFD_PathSet_GetPath(ref nativefiledialog.nfdpathset_t pathset, IntPtr index)
	{
		return nativefiledialog.UTF8_ToManaged(nativefiledialog.INTERNAL_NFD_PathSet_GetPath(ref pathset, index), false);
	}

	// Token: 0x0600005D RID: 93
	[DllImport("nfd", CallingConvention = CallingConvention.Cdecl)]
	public static extern void NFD_PathSet_Free(ref nativefiledialog.nfdpathset_t pathset);

	// Token: 0x04000018 RID: 24
	private const string nativeLibName = "nfd";

	// Token: 0x020005E8 RID: 1512
	public enum nfdresult_t
	{
		// Token: 0x0400632B RID: 25387
		NFD_ERROR,
		// Token: 0x0400632C RID: 25388
		NFD_OKAY,
		// Token: 0x0400632D RID: 25389
		NFD_CANCEL
	}

	// Token: 0x020005E9 RID: 1513
	public struct nfdpathset_t
	{
		// Token: 0x0400632E RID: 25390
		private IntPtr buf;

		// Token: 0x0400632F RID: 25391
		private IntPtr indices;

		// Token: 0x04006330 RID: 25392
		private IntPtr count;
	}
}
