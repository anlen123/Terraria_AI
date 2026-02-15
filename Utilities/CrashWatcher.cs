using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace Terraria.Utilities
{
	// Token: 0x020000CD RID: 205
	public static class CrashWatcher
	{
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060017FB RID: 6139 RVA: 0x004E0361 File Offset: 0x004DE561
		// (set) Token: 0x060017FC RID: 6140 RVA: 0x004E0368 File Offset: 0x004DE568
		public static bool LogAllExceptions { get; set; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x004E0370 File Offset: 0x004DE570
		// (set) Token: 0x060017FE RID: 6142 RVA: 0x004E0377 File Offset: 0x004DE577
		public static bool DumpOnException { get; set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x004E037F File Offset: 0x004DE57F
		// (set) Token: 0x06001800 RID: 6144 RVA: 0x004E0386 File Offset: 0x004DE586
		public static bool DumpOnCrash { get; private set; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001801 RID: 6145 RVA: 0x004E038E File Offset: 0x004DE58E
		// (set) Token: 0x06001802 RID: 6146 RVA: 0x004E0395 File Offset: 0x004DE595
		public static CrashDump.Options CrashDumpOptions { get; private set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001803 RID: 6147 RVA: 0x004E039D File Offset: 0x004DE59D
		private static string DumpPath
		{
			get
			{
				return Path.Combine(Main.SavePath, "Dumps");
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x004E03B0 File Offset: 0x004DE5B0
		public static void Inititialize()
		{
			Console.WriteLine("Error Logging Enabled.");
			AppDomain.CurrentDomain.FirstChanceException += delegate(object sender, FirstChanceExceptionEventArgs exceptionArgs)
			{
				if (Main.IsFullScreenThatWouldBeStuckOnCrashMessage())
				{
					return;
				}
				if (CrashWatcher.LogAllExceptions && !false)
				{
					string text = CrashWatcher.PrintException(exceptionArgs.Exception);
					Console.Write("================\r\n" + string.Format("{0}: First-Chance Exception\r\nThread: {1} [{2}]\r\nCulture: {3}\r\nException: {4}\r\n", new object[]
					{
						DateTime.Now,
						Thread.CurrentThread.ManagedThreadId,
						Thread.CurrentThread.Name,
						Thread.CurrentThread.CurrentCulture.Name,
						text
					}) + "================\r\n\r\n");
				}
			};
			AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs exceptionArgs)
			{
				if (Main.IsFullScreenThatWouldBeStuckOnCrashMessage())
				{
					return;
				}
				string text = CrashWatcher.PrintException((Exception)exceptionArgs.ExceptionObject);
				Console.Write("================\r\n" + string.Format("{0}: Unhandled Exception\r\nThread: {1} [{2}]\r\nCulture: {3}\r\nException: {4}\r\n", new object[]
				{
					DateTime.Now,
					Thread.CurrentThread.ManagedThreadId,
					Thread.CurrentThread.Name,
					Thread.CurrentThread.CurrentCulture.Name,
					text
				}) + "================\r\n");
				if (CrashWatcher.DumpOnCrash)
				{
					CrashDump.WriteException(CrashWatcher.CrashDumpOptions, CrashWatcher.DumpPath);
				}
			};
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x004E041C File Offset: 0x004DE61C
		private static string PrintException(Exception ex)
		{
			string text = ex.ToString();
			try
			{
				int num = (int)typeof(Exception).GetProperty("HResult", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetGetMethod(true).Invoke(ex, null);
				if (num != 0)
				{
					text = text + "\nHResult: " + num;
				}
			}
			catch
			{
			}
			if (ex is ReflectionTypeLoadException)
			{
				foreach (Exception ex2 in ((ReflectionTypeLoadException)ex).LoaderExceptions)
				{
					text = text + "\n+--> " + CrashWatcher.PrintException(ex2);
				}
			}
			return text;
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x004E04C0 File Offset: 0x004DE6C0
		public static void EnableCrashDumps(CrashDump.Options options)
		{
			CrashWatcher.DumpOnCrash = true;
			CrashWatcher.CrashDumpOptions = options;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x004E04CE File Offset: 0x004DE6CE
		public static void DisableCrashDumps()
		{
			CrashWatcher.DumpOnCrash = false;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00009E06 File Offset: 0x00008006
		[Conditional("DEBUG")]
		private static void HookDebugExceptionDialog()
		{
		}
	}
}
