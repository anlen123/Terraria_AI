using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using ReLogic.IO;
using ReLogic.OS;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria
{
	// Token: 0x0200004E RID: 78
	public static class Program
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x00355DDF File Offset: 0x00353FDF
		public static float LoadedPercentage
		{
			get
			{
				if (Program.ThingsToLoad == 0)
				{
					return 1f;
				}
				return (float)Program.ThingsLoaded / (float)Program.ThingsToLoad;
			}
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00355DFB File Offset: 0x00353FFB
		public static void StartForceLoad()
		{
			if (!Main.SkipAssemblyLoad)
			{
				new Thread(new ParameterizedThreadStart(Program.ForceLoadThread))
				{
					IsBackground = true
				}.Start();
				return;
			}
			Program.LoadedEverything = true;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00355E28 File Offset: 0x00354028
		public static void ForceLoadThread(object threadContext)
		{
			Program.ForceLoadAssembly(Assembly.GetExecutingAssembly(), true);
			Program.LoadedEverything = true;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00355E3C File Offset: 0x0035403C
		private static void ForceJITOnAssembly(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypes())
			{
				foreach (MethodInfo methodInfo in Program.IsMono ? type.GetMethods() : type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (!methodInfo.IsAbstract && !methodInfo.ContainsGenericParameters && methodInfo.GetMethodBody() != null)
					{
						if (Program.IsMono)
						{
							Program.JitForcedMethodCache = methodInfo.MethodHandle.GetFunctionPointer();
						}
						else
						{
							RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);
						}
					}
				}
				Program.ThingsLoaded++;
			}
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00355EEC File Offset: 0x003540EC
		private static void ForceStaticInitializers(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (!type.IsGenericType)
				{
					RuntimeHelpers.RunClassConstructor(type.TypeHandle);
				}
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00355F25 File Offset: 0x00354125
		private static void ForceLoadAssembly(Assembly assembly, bool initializeStaticMembers)
		{
			Program.ThingsToLoad = assembly.GetTypes().Length;
			Program.ForceJITOnAssembly(assembly);
			if (initializeStaticMembers)
			{
				Program.ForceStaticInitializers(assembly);
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00355F44 File Offset: 0x00354144
		private static void ForceLoadAssembly(string name, bool initializeStaticMembers)
		{
			Assembly assembly = null;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				if (assemblies[i].GetName().Name.Equals(name))
				{
					assembly = assemblies[i];
					break;
				}
			}
			if (assembly == null)
			{
				assembly = Assembly.Load(name);
			}
			Program.ForceLoadAssembly(assembly, initializeStaticMembers);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00355FA0 File Offset: 0x003541A0
		private static void SetupLogging()
		{
			if (Program.LaunchParameters.ContainsKey("-logfile"))
			{
				string text = Program.LaunchParameters["-logfile"];
				if (text == null || text.Trim() == "")
				{
					text = Path.Combine(Program.SavePath, "Logs", string.Format("Log_{0:yyyyMMddHHmmssfff}.log", DateTime.Now));
				}
				else
				{
					text = Path.Combine(text, string.Format("Log_{0:yyyyMMddHHmmssfff}.log", DateTime.Now));
				}
				ConsoleOutputMirror.ToFile(text);
			}
			CrashWatcher.Inititialize();
			CrashWatcher.DumpOnException = Program.LaunchParameters.ContainsKey("-minidump");
			CrashWatcher.LogAllExceptions = Program.LaunchParameters.ContainsKey("-logerrors");
			if (Program.LaunchParameters.ContainsKey("-fulldump"))
			{
				CrashDump.Options options = CrashDump.Options.WithFullMemory;
				Console.WriteLine("Full Dump logs enabled.");
				CrashWatcher.EnableCrashDumps(options);
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00356078 File Offset: 0x00354278
		private static void InitializeConsoleOutput()
		{
			if (Debugger.IsAttached)
			{
				return;
			}
			try
			{
				Console.OutputEncoding = Encoding.UTF8;
				if (Platform.IsWindows)
				{
					Console.InputEncoding = Encoding.Unicode;
				}
				else
				{
					Console.InputEncoding = Encoding.UTF8;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x003560CC File Offset: 0x003542CC
		public static void LaunchGame(string[] args, bool monoArgs = false)
		{
			Thread.CurrentThread.Name = "Main Thread";
			if (monoArgs)
			{
				args = Utils.ConvertMonoArgsToDotNet(args);
			}
			Program.LogFNANativeLibVersions();
			Program.LaunchParameters = Utils.ParseArguements(args);
			Program.SavePath = (Program.LaunchParameters.ContainsKey("-savedirectory") ? Program.LaunchParameters["-savedirectory"] : Platform.Get<IPathService>().GetStoragePath("Terraria"));
			ThreadPool.SetMinThreads(8, 8);
			Program.InitializeConsoleOutput();
			Program.SetupLogging();
			Platform.Get<IWindowService>().SetQuickEditEnabled(false);
			Program.RunGame();
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0035615C File Offset: 0x0035435C
		public static void RunGame()
		{
			LanguageManager.Instance.SetLanguage(GameCulture.DefaultCulture);
			if (Platform.IsOSX)
			{
				Main.OnEngineLoad += delegate()
				{
					Main.instance.IsMouseVisible = false;
				};
			}
			else if (Platform.IsWindows)
			{
				Main.OnEngineLoad += delegate()
				{
					IMouseNotifier mouseNotifier = Platform.Get<IMouseNotifier>();
					if (mouseNotifier != null)
					{
						mouseNotifier.AddMouseHandler(delegate(bool connected)
						{
							if (connected)
							{
								Main.instance.IsMouseVisible = true;
								Main.instance.ReHideCursor = true;
							}
						});
					}
				};
			}
			using (Main main = new Main())
			{
				try
				{
					Lang.InitializeLegacyLocalization();
					SocialAPI.Initialize(null);
					LaunchInitializer.LoadParameters(main);
					Main.OnEnginePreload += Program.StartForceLoad;
					if (Main.dedServ)
					{
						main.DedServ();
					}
					main.Run();
				}
				catch (Exception e)
				{
					Program.DisplayException(e);
				}
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00009E06 File Offset: 0x00008006
		private static void LogFNANativeLibVersions()
		{
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00356244 File Offset: 0x00354444
		private static void DisplayException(Exception e)
		{
			try
			{
				string text = e.ToString();
				if (WorldGen.isGeneratingOrLoadingWorld)
				{
					try
					{
						text = string.Format("Creating world - Seed: {0} Width: {1}, Height: {2}, Evil: {3}, IsExpert: {4}\n{5}", new object[]
						{
							Main.ActiveWorldFileData.SeedText,
							Main.maxTilesX,
							Main.maxTilesY,
							WorldGen.WorldGenParam_Evil,
							Main.expertMode,
							text
						});
					}
					catch
					{
					}
				}
				using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", true))
				{
					streamWriter.WriteLine(DateTime.Now);
					streamWriter.WriteLine(text);
					streamWriter.WriteLine("");
				}
				if (Main.dedServ)
				{
					Console.WriteLine(Language.GetTextValue("Error.ServerCrash"), DateTime.Now, text);
				}
				MessageBox.Show(text, "Terraria: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch
			{
			}
		}

		// Token: 0x040009D3 RID: 2515
		public static bool IsXna = true;

		// Token: 0x040009D4 RID: 2516
		public static bool IsFna = false;

		// Token: 0x040009D5 RID: 2517
		public static bool IsMono = Type.GetType("Mono.Runtime") != null;

		// Token: 0x040009D6 RID: 2518
		public const bool IsDebug = false;

		// Token: 0x040009D7 RID: 2519
		public static Dictionary<string, string> LaunchParameters = new Dictionary<string, string>();

		// Token: 0x040009D8 RID: 2520
		public static string SavePath;

		// Token: 0x040009D9 RID: 2521
		public const string TerrariaSaveFolderPath = "Terraria";

		// Token: 0x040009DA RID: 2522
		private static int ThingsToLoad;

		// Token: 0x040009DB RID: 2523
		private static int ThingsLoaded;

		// Token: 0x040009DC RID: 2524
		public static bool LoadedEverything;

		// Token: 0x040009DD RID: 2525
		public static IntPtr JitForcedMethodCache;
	}
}
