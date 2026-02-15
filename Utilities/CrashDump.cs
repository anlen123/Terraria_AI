using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using ReLogic.OS;

namespace Terraria.Utilities
{
	// Token: 0x020000D6 RID: 214
	public static class CrashDump
	{
		// Token: 0x06001855 RID: 6229 RVA: 0x004E17B5 File Offset: 0x004DF9B5
		public static bool WriteException(CrashDump.Options options, string outputDirectory = ".")
		{
			return CrashDump.Write(options, CrashDump.ExceptionInfo.Present, outputDirectory);
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x004E17BF File Offset: 0x004DF9BF
		public static bool Write(CrashDump.Options options, string outputDirectory = ".")
		{
			return CrashDump.Write(options, CrashDump.ExceptionInfo.None, outputDirectory);
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x004E17CC File Offset: 0x004DF9CC
		private static string CreateDumpName()
		{
			DateTime dateTime = DateTime.Now.ToLocalTime();
			return string.Format("{0}_{1}_{2}_{3}.dmp", new object[]
			{
				Main.dedServ ? "TerrariaServer" : "Terraria",
				Main.versionNumber,
				dateTime.ToString("MM-dd-yy_HH-mm-ss-ffff", CultureInfo.InvariantCulture),
				Thread.CurrentThread.ManagedThreadId
			});
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x004E183C File Offset: 0x004DFA3C
		private static bool Write(CrashDump.Options options, CrashDump.ExceptionInfo exceptionInfo, string outputDirectory)
		{
			if (!Platform.IsWindows)
			{
				return false;
			}
			string path = Path.Combine(outputDirectory, CrashDump.CreateDumpName());
			if (!Utils.TryCreatingDirectory(outputDirectory))
			{
				return false;
			}
			bool result;
			using (FileStream fileStream = File.Create(path))
			{
				result = CrashDump.Write(fileStream.SafeFileHandle, options, exceptionInfo);
			}
			return result;
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x004E189C File Offset: 0x004DFA9C
		private static bool Write(SafeHandle fileHandle, CrashDump.Options options, CrashDump.ExceptionInfo exceptionInfo)
		{
			if (!Platform.IsWindows)
			{
				return false;
			}
			Process currentProcess = Process.GetCurrentProcess();
			IntPtr handle = currentProcess.Handle;
			uint id = (uint)currentProcess.Id;
			CrashDump.MiniDumpExceptionInformation miniDumpExceptionInformation;
			miniDumpExceptionInformation.ThreadId = CrashDump.GetCurrentThreadId();
			miniDumpExceptionInformation.ClientPointers = false;
			miniDumpExceptionInformation.ExceptionPointers = IntPtr.Zero;
			if (exceptionInfo == CrashDump.ExceptionInfo.Present)
			{
				miniDumpExceptionInformation.ExceptionPointers = Marshal.GetExceptionPointers();
			}
			bool result;
			if (miniDumpExceptionInformation.ExceptionPointers == IntPtr.Zero)
			{
				result = CrashDump.MiniDumpWriteDump(handle, id, fileHandle, (uint)options, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			}
			else
			{
				result = CrashDump.MiniDumpWriteDump(handle, id, fileHandle, (uint)options, ref miniDumpExceptionInformation, IntPtr.Zero, IntPtr.Zero);
			}
			return result;
		}

		// Token: 0x0600185A RID: 6234
		[DllImport("dbghelp.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, ref CrashDump.MiniDumpExceptionInformation expParam, IntPtr userStreamParam, IntPtr callbackParam);

		// Token: 0x0600185B RID: 6235
		[DllImport("dbghelp.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

		// Token: 0x0600185C RID: 6236
		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern uint GetCurrentThreadId();

		// Token: 0x020006F8 RID: 1784
		[Flags]
		public enum Options : uint
		{
			// Token: 0x040067FA RID: 26618
			Normal = 0U,
			// Token: 0x040067FB RID: 26619
			WithDataSegs = 1U,
			// Token: 0x040067FC RID: 26620
			WithFullMemory = 2U,
			// Token: 0x040067FD RID: 26621
			WithHandleData = 4U,
			// Token: 0x040067FE RID: 26622
			FilterMemory = 8U,
			// Token: 0x040067FF RID: 26623
			ScanMemory = 16U,
			// Token: 0x04006800 RID: 26624
			WithUnloadedModules = 32U,
			// Token: 0x04006801 RID: 26625
			WithIndirectlyReferencedMemory = 64U,
			// Token: 0x04006802 RID: 26626
			FilterModulePaths = 128U,
			// Token: 0x04006803 RID: 26627
			WithProcessThreadData = 256U,
			// Token: 0x04006804 RID: 26628
			WithPrivateReadWriteMemory = 512U,
			// Token: 0x04006805 RID: 26629
			WithoutOptionalData = 1024U,
			// Token: 0x04006806 RID: 26630
			WithFullMemoryInfo = 2048U,
			// Token: 0x04006807 RID: 26631
			WithThreadInfo = 4096U,
			// Token: 0x04006808 RID: 26632
			WithCodeSegs = 8192U,
			// Token: 0x04006809 RID: 26633
			WithoutAuxiliaryState = 16384U,
			// Token: 0x0400680A RID: 26634
			WithFullAuxiliaryState = 32768U,
			// Token: 0x0400680B RID: 26635
			WithPrivateWriteCopyMemory = 65536U,
			// Token: 0x0400680C RID: 26636
			IgnoreInaccessibleMemory = 131072U,
			// Token: 0x0400680D RID: 26637
			ValidTypeFlags = 262143U
		}

		// Token: 0x020006F9 RID: 1785
		private enum ExceptionInfo
		{
			// Token: 0x0400680F RID: 26639
			None,
			// Token: 0x04006810 RID: 26640
			Present
		}

		// Token: 0x020006FA RID: 1786
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct MiniDumpExceptionInformation
		{
			// Token: 0x04006811 RID: 26641
			public uint ThreadId;

			// Token: 0x04006812 RID: 26642
			public IntPtr ExceptionPointers;

			// Token: 0x04006813 RID: 26643
			[MarshalAs(UnmanagedType.Bool)]
			public bool ClientPointers;
		}
	}
}
