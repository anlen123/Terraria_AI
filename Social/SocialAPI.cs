using System;
using System.Collections.Generic;
using ReLogic.OS;
using Terraria.Social.Base;
using Terraria.Social.Steam;
using Terraria.Social.WeGame;

namespace Terraria.Social
{
	// Token: 0x02000123 RID: 291
	public static class SocialAPI
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001B60 RID: 7008 RVA: 0x004FA04F File Offset: 0x004F824F
		public static SocialMode Mode
		{
			get
			{
				return SocialAPI._mode;
			}
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x004FA058 File Offset: 0x004F8258
		public static void Initialize(SocialMode? mode = null)
		{
			if (mode == null)
			{
				mode = new SocialMode?(SocialMode.None);
				if (Main.dedServ)
				{
					if (Program.LaunchParameters.ContainsKey("-steam"))
					{
						mode = new SocialMode?(SocialMode.Steam);
					}
				}
				else
				{
					mode = new SocialMode?(SocialMode.Steam);
				}
			}
			SocialAPI._mode = mode.Value;
			SocialAPI._modules = new List<ISocialModule>();
			SocialAPI.JoinRequests = new ServerJoinRequestsManager();
			Main.OnTickForInternalCodeOnly += SocialAPI.JoinRequests.Update;
			SocialMode mode2 = SocialAPI.Mode;
			if (mode2 != SocialMode.Steam)
			{
				if (mode2 == SocialMode.WeGame)
				{
					SocialAPI.LoadWeGame();
				}
			}
			else
			{
				SocialAPI.LoadSteam();
			}
			foreach (ISocialModule socialModule in SocialAPI._modules)
			{
				socialModule.Initialize();
			}
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x004FA134 File Offset: 0x004F8334
		public static void Shutdown()
		{
			SocialAPI._modules.Reverse();
			foreach (ISocialModule socialModule in SocialAPI._modules)
			{
				socialModule.Shutdown();
			}
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x004FA190 File Offset: 0x004F8390
		private static T LoadModule<T>() where T : ISocialModule, new()
		{
			T t = Activator.CreateInstance<T>();
			SocialAPI._modules.Add(t);
			return t;
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x004FA1B4 File Offset: 0x004F83B4
		private static T LoadModule<T>(T module) where T : ISocialModule
		{
			SocialAPI._modules.Add(module);
			return module;
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x004FA1C7 File Offset: 0x004F83C7
		private static void LoadDiscord()
		{
			if (Main.dedServ)
			{
				return;
			}
			if (ReLogic.OS.Platform.IsWindows || Environment.Is64BitOperatingSystem)
			{
				bool is64BitProcess = Environment.Is64BitProcess;
			}
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x004FA1E8 File Offset: 0x004F83E8
		private static void LoadSteam()
		{
			SocialAPI.LoadModule<Terraria.Social.Steam.CoreSocialModule>();
			SocialAPI.Friends = SocialAPI.LoadModule<Terraria.Social.Steam.FriendsSocialModule>();
			SocialAPI.Achievements = SocialAPI.LoadModule<Terraria.Social.Steam.AchievementsSocialModule>();
			SocialAPI.Cloud = SocialAPI.LoadModule<Terraria.Social.Steam.CloudSocialModule>();
			SocialAPI.Overlay = SocialAPI.LoadModule<Terraria.Social.Steam.OverlaySocialModule>();
			SocialAPI.Workshop = SocialAPI.LoadModule<Terraria.Social.Steam.WorkshopSocialModule>();
			SocialAPI.Platform = SocialAPI.LoadModule<Terraria.Social.Steam.PlatformSocialModule>();
			if (Main.dedServ)
			{
				SocialAPI.Network = SocialAPI.LoadModule<Terraria.Social.Steam.NetServerSocialModule>();
			}
			else
			{
				SocialAPI.Network = SocialAPI.LoadModule<Terraria.Social.Steam.NetClientSocialModule>();
			}
			WeGameHelper.WriteDebugString("LoadSteam modules", new object[0]);
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x004FA264 File Offset: 0x004F8464
		private static void LoadWeGame()
		{
			SocialAPI.LoadModule<Terraria.Social.WeGame.CoreSocialModule>();
			SocialAPI.Cloud = SocialAPI.LoadModule<Terraria.Social.WeGame.CloudSocialModule>();
			SocialAPI.Friends = SocialAPI.LoadModule<Terraria.Social.WeGame.FriendsSocialModule>();
			SocialAPI.Overlay = SocialAPI.LoadModule<Terraria.Social.WeGame.OverlaySocialModule>();
			if (Main.dedServ)
			{
				SocialAPI.Network = SocialAPI.LoadModule<Terraria.Social.WeGame.NetServerSocialModule>();
			}
			else
			{
				SocialAPI.Network = SocialAPI.LoadModule<Terraria.Social.WeGame.NetClientSocialModule>();
			}
			WeGameHelper.WriteDebugString("LoadWeGame modules", new object[0]);
		}

		// Token: 0x0400155F RID: 5471
		private static SocialMode _mode;

		// Token: 0x04001560 RID: 5472
		public static Terraria.Social.Base.FriendsSocialModule Friends;

		// Token: 0x04001561 RID: 5473
		public static Terraria.Social.Base.AchievementsSocialModule Achievements;

		// Token: 0x04001562 RID: 5474
		public static Terraria.Social.Base.CloudSocialModule Cloud;

		// Token: 0x04001563 RID: 5475
		public static Terraria.Social.Base.NetSocialModule Network;

		// Token: 0x04001564 RID: 5476
		public static Terraria.Social.Base.OverlaySocialModule Overlay;

		// Token: 0x04001565 RID: 5477
		public static Terraria.Social.Base.WorkshopSocialModule Workshop;

		// Token: 0x04001566 RID: 5478
		public static ServerJoinRequestsManager JoinRequests;

		// Token: 0x04001567 RID: 5479
		public static Terraria.Social.Base.PlatformSocialModule Platform;

		// Token: 0x04001568 RID: 5480
		private static List<ISocialModule> _modules;
	}
}
