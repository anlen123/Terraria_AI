using System;
using System.Diagnostics;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.Initializers
{
	// Token: 0x02000083 RID: 131
	public static class LaunchInitializer
	{
		// Token: 0x06001582 RID: 5506 RVA: 0x004CAF61 File Offset: 0x004C9161
		public static void LoadParameters(Main game)
		{
			LaunchInitializer.LoadSharedParameters(game);
			if (Main.dedServ)
			{
				LaunchInitializer.LoadServerParameters(game);
			}
			LaunchInitializer.LoadClientParameters(game);
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x004CAF7C File Offset: 0x004C917C
		private static void LoadSharedParameters(Main game)
		{
			string path;
			if ((path = LaunchInitializer.TryParameter(new string[]
			{
				"-loadlib"
			})) != null)
			{
				game.loadLib(path);
			}
			string s;
			int listenPort;
			if ((s = LaunchInitializer.TryParameter(new string[]
			{
				"-p",
				"-port"
			})) != null && int.TryParse(s, out listenPort))
			{
				Netplay.ListenPort = listenPort;
			}
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x004CAFD8 File Offset: 0x004C91D8
		private static void LoadClientParameters(Main game)
		{
			string ip;
			if ((ip = LaunchInitializer.TryParameter(new string[]
			{
				"-j",
				"-join"
			})) != null)
			{
				game.AutoJoin(ip);
			}
			string arg;
			if ((arg = LaunchInitializer.TryParameter(new string[]
			{
				"-pass",
				"-password"
			})) != null)
			{
				Netplay.ServerPassword = Main.ConvertFromSafeArgument(arg);
				game.AutoPass();
			}
			if (LaunchInitializer.HasParameter(new string[]
			{
				"-host"
			}))
			{
				game.AutoHost();
			}
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x004CB058 File Offset: 0x004C9258
		private static void LoadServerParameters(Main game)
		{
			try
			{
				string s;
				if ((s = LaunchInitializer.TryParameter(new string[]
				{
					"-forcepriority"
				})) != null)
				{
					Process currentProcess = Process.GetCurrentProcess();
					int num;
					if (int.TryParse(s, out num))
					{
						switch (num)
						{
						case 0:
							currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
							break;
						case 1:
							currentProcess.PriorityClass = ProcessPriorityClass.High;
							break;
						case 2:
							currentProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
							break;
						case 3:
							currentProcess.PriorityClass = ProcessPriorityClass.Normal;
							break;
						case 4:
							currentProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
							break;
						case 5:
							currentProcess.PriorityClass = ProcessPriorityClass.Idle;
							break;
						default:
							currentProcess.PriorityClass = ProcessPriorityClass.High;
							break;
						}
					}
					else
					{
						currentProcess.PriorityClass = ProcessPriorityClass.High;
					}
				}
				else
				{
					Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
				}
			}
			catch
			{
			}
			string value;
			if ((value = LaunchInitializer.TryParameter(new string[]
			{
				"-maxplayers",
				"-players"
			})) != null)
			{
				int num2 = Convert.ToInt32(value);
				if (num2 <= 255 && num2 >= 1)
				{
					game.SetNetPlayers(num2);
				}
			}
			string arg;
			if ((arg = LaunchInitializer.TryParameter(new string[]
			{
				"-pass",
				"-password"
			})) != null)
			{
				Netplay.ServerPassword = Main.ConvertFromSafeArgument(arg);
			}
			string text;
			int language;
			if ((text = LaunchInitializer.TryParameter(new string[]
			{
				"-lang"
			})) != null && int.TryParse(text, out language))
			{
				LanguageManager.Instance.SetLanguage(language);
			}
			if ((text = LaunchInitializer.TryParameter(new string[]
			{
				"-language"
			})) != null)
			{
				LanguageManager.Instance.SetLanguage(text);
			}
			string worldName;
			if ((worldName = LaunchInitializer.TryParameter(new string[]
			{
				"-worldname"
			})) != null)
			{
				game.SetWorldName(worldName);
			}
			string newMOTD;
			if ((newMOTD = LaunchInitializer.TryParameter(new string[]
			{
				"-motd"
			})) != null)
			{
				game.NewMOTD(newMOTD);
			}
			string banFilePath;
			if ((banFilePath = LaunchInitializer.TryParameter(new string[]
			{
				"-banlist"
			})) != null)
			{
				Netplay.BanFilePath = banFilePath;
			}
			if (LaunchInitializer.HasParameter(new string[]
			{
				"-autoshutdown"
			}))
			{
				game.EnableAutoShutdown();
			}
			string hostToken;
			if ((hostToken = LaunchInitializer.TryParameter(new string[]
			{
				"-hosttoken"
			})) != null)
			{
				Netplay.HostToken = hostToken;
			}
			if (LaunchInitializer.HasParameter(new string[]
			{
				"-secure"
			}))
			{
				Netplay.SpamCheck = true;
			}
			string serverWorldRollbacks;
			if ((serverWorldRollbacks = LaunchInitializer.TryParameter(new string[]
			{
				"-worldrollbackstokeep"
			})) != null)
			{
				game.setServerWorldRollbacks(serverWorldRollbacks);
			}
			string worldSize;
			if ((worldSize = LaunchInitializer.TryParameter(new string[]
			{
				"-autocreate"
			})) != null)
			{
				game.autoCreate(worldSize);
			}
			if (LaunchInitializer.HasParameter(new string[]
			{
				"-noupnp"
			}))
			{
				Netplay.UseUPNP = false;
			}
			if (LaunchInitializer.HasParameter(new string[]
			{
				"-experimental"
			}))
			{
				Main.UseExperimentalFeatures = true;
			}
			string text2;
			if ((text2 = LaunchInitializer.TryParameter(new string[]
			{
				"-world"
			})) != null)
			{
				if (FileUtilities.Exists(text2, false) || !Main.autoGen)
				{
					game.SetWorld(text2, false);
				}
				else
				{
					new WorldFileData(text2, false).SetAsActive();
					Main.autoGenFileLocation = text2;
				}
			}
			else if (SocialAPI.Mode == SocialMode.Steam && (text2 = LaunchInitializer.TryParameter(new string[]
			{
				"-cloudworld"
			})) != null)
			{
				if (FileUtilities.Exists(text2, true) || !Main.autoGen)
				{
					game.SetWorld(text2, true);
				}
				else
				{
					new WorldFileData(text2, true).SetAsActive();
					Main.autoGenFileLocation = text2;
				}
			}
			string configPath;
			if ((configPath = LaunchInitializer.TryParameter(new string[]
			{
				"-config"
			})) != null)
			{
				game.LoadDedConfig(configPath);
			}
			string autogenSeedName;
			if ((autogenSeedName = LaunchInitializer.TryParameter(new string[]
			{
				"-seed"
			})) != null)
			{
				Main.AutogenSeedName = autogenSeedName;
			}
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x004CB3F8 File Offset: 0x004C95F8
		private static bool HasParameter(params string[] keys)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				if (Program.LaunchParameters.ContainsKey(keys[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x004CB428 File Offset: 0x004C9628
		private static string TryParameter(params string[] keys)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				string text;
				if (Program.LaunchParameters.TryGetValue(keys[i], out text))
				{
					if (text == null)
					{
						text = "";
					}
					return text;
				}
			}
			return null;
		}
	}
}
