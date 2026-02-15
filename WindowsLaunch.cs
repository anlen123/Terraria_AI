using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Terraria.Social;

namespace Terraria
{
	// Token: 0x02000024 RID: 36
	public static class WindowsLaunch
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00011D70 File Offset: 0x0000FF70
		private static bool ConsoleCtrlCheck(WindowsLaunch.CtrlTypes ctrlType)
		{
			bool flag = false;
			switch (ctrlType)
			{
			case WindowsLaunch.CtrlTypes.CTRL_C_EVENT:
				flag = true;
				break;
			case WindowsLaunch.CtrlTypes.CTRL_BREAK_EVENT:
				flag = true;
				break;
			case WindowsLaunch.CtrlTypes.CTRL_CLOSE_EVENT:
				flag = true;
				break;
			case WindowsLaunch.CtrlTypes.CTRL_LOGOFF_EVENT:
			case WindowsLaunch.CtrlTypes.CTRL_SHUTDOWN_EVENT:
				flag = true;
				break;
			}
			if (flag)
			{
				SocialAPI.Shutdown();
			}
			return true;
		}

		// Token: 0x0600019A RID: 410
		[DllImport("Kernel32")]
		public static extern bool SetConsoleCtrlHandler(WindowsLaunch.HandlerRoutine handler, bool add);

		// Token: 0x0600019B RID: 411 RVA: 0x00011DBA File Offset: 0x0000FFBA
		[STAThread]
		private static void Main(string[] args)
		{
			AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs sargs)
			{
				string resourceName = new AssemblyName(sargs.Name).Name + ".dll";
				string text = Array.Find<string>(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.EndsWith(resourceName));
				if (text == null)
				{
					return null;
				}
				Assembly result;
				using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(text))
				{
					byte[] array = new byte[manifestResourceStream.Length];
					manifestResourceStream.Read(array, 0, array.Length);
					result = Assembly.Load(array);
				}
				return result;
			};
			Program.LaunchGame(args, false);
		}

		// Token: 0x0400012D RID: 301
		private static WindowsLaunch.HandlerRoutine _handleRoutine;

		// Token: 0x020005ED RID: 1517
		// (Invoke) Token: 0x06003B3E RID: 15166
		public delegate bool HandlerRoutine(WindowsLaunch.CtrlTypes ctrlType);

		// Token: 0x020005EE RID: 1518
		public enum CtrlTypes
		{
			// Token: 0x04006338 RID: 25400
			CTRL_C_EVENT,
			// Token: 0x04006339 RID: 25401
			CTRL_BREAK_EVENT,
			// Token: 0x0400633A RID: 25402
			CTRL_CLOSE_EVENT,
			// Token: 0x0400633B RID: 25403
			CTRL_LOGOFF_EVENT = 5,
			// Token: 0x0400633C RID: 25404
			CTRL_SHUTDOWN_EVENT
		}
	}
}
