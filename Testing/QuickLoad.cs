using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using ReLogic.OS;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.Testing
{
	// Token: 0x02000114 RID: 276
	public abstract class QuickLoad
	{
		// Token: 0x06001ADD RID: 6877
		protected abstract void Start();

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x004F8060 File Offset: 0x004F6260
		public static bool QuickLoading
		{
			get
			{
				return QuickLoad._loadedConfig != null;
			}
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x004F806C File Offset: 0x004F626C
		public static void Load()
		{
			if (!QuickLoad.TryRead(out QuickLoad._loadedConfig))
			{
				return;
			}
			if (QuickLoad.ShiftHeld())
			{
				QuickLoad._loadedConfig = null;
				Platform.Get<IWindowService>().Activate(Main.instance.Window);
				if (MessageBox.Show("Quick Load skipped. Do you want to delete the configuration?", "", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.Yes)
				{
					QuickLoad.Clear();
				}
			}
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x004F80C0 File Offset: 0x004F62C0
		public static void OnContentLoaded()
		{
			try
			{
				if (QuickLoad._loadedConfig != null)
				{
					QuickLoad._loadedConfig.Start();
				}
			}
			catch (Exception arg)
			{
				if (MessageBox.Show("Do you want to delete the configuration?\n\n" + arg, "Quickload Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
				{
					QuickLoad.Clear();
				}
			}
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x004F8114 File Offset: 0x004F6314
		public static QuickLoad Deserialize(string json)
		{
			return JsonConvert.DeserializeObject<QuickLoad>(json, QuickLoad.SerializerSettings);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x004F8121 File Offset: 0x004F6321
		public static string Serialize(QuickLoad config)
		{
			return JsonConvert.SerializeObject(config, typeof(QuickLoad), QuickLoad.SerializerSettings);
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x004F8138 File Offset: 0x004F6338
		public static bool TryRead(out QuickLoad config)
		{
			config = null;
			bool result;
			try
			{
				if (!File.Exists(QuickLoad.FilePath))
				{
					result = false;
				}
				else
				{
					config = QuickLoad.Deserialize(File.ReadAllText(QuickLoad.FilePath));
					result = true;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x004F8184 File Offset: 0x004F6384
		public static void Set(QuickLoad config)
		{
			File.WriteAllText(QuickLoad.FilePath, QuickLoad.Serialize(config));
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x004F8196 File Offset: 0x004F6396
		public static void Clear()
		{
			if (File.Exists(QuickLoad.FilePath))
			{
				File.Delete(QuickLoad.FilePath);
			}
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x004F81B0 File Offset: 0x004F63B0
		private static bool ShiftHeld()
		{
			if (Keyboard.GetState().PressingShift())
			{
				return true;
			}
			try
			{
				if (Platform.IsWindows)
				{
					return QuickLoad.ShiftHeldWin();
				}
				if (Platform.IsOSX)
				{
					return QuickLoad.ShiftHeldOSX();
				}
				if (Platform.IsLinux)
				{
					return QuickLoad.ShiftHeldX11();
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x06001AE7 RID: 6887
		[DllImport("user32.dll")]
		private static extern short GetAsyncKeyState(int vKey);

		// Token: 0x06001AE8 RID: 6888 RVA: 0x004F8214 File Offset: 0x004F6414
		private static bool ShiftHeldWin()
		{
			return ((int)QuickLoad.GetAsyncKeyState(160) & 32768) != 0 || ((int)QuickLoad.GetAsyncKeyState(161) & 32768) != 0;
		}

		// Token: 0x06001AE9 RID: 6889
		[DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
		private static extern ulong CGEventSourceFlagsState(uint stateID);

		// Token: 0x06001AEA RID: 6890 RVA: 0x004F823D File Offset: 0x004F643D
		private static bool ShiftHeldOSX()
		{
			return (QuickLoad.CGEventSourceFlagsState(1U) & 131072UL) > 0UL;
		}

		// Token: 0x06001AEB RID: 6891
		[DllImport("libX11")]
		private static extern IntPtr XOpenDisplay(IntPtr display);

		// Token: 0x06001AEC RID: 6892
		[DllImport("libX11")]
		private static extern void XCloseDisplay(IntPtr display);

		// Token: 0x06001AED RID: 6893
		[DllImport("libX11")]
		private static extern int XQueryKeymap(IntPtr display, byte[] keys_return);

		// Token: 0x06001AEE RID: 6894
		[DllImport("libX11")]
		private static extern int XKeysymToKeycode(IntPtr display, ulong keysym);

		// Token: 0x06001AEF RID: 6895 RVA: 0x004F8250 File Offset: 0x004F6450
		private static bool ShiftHeldX11()
		{
			IntPtr intPtr = QuickLoad.XOpenDisplay(IntPtr.Zero);
			if (intPtr == IntPtr.Zero)
			{
				return false;
			}
			bool result;
			try
			{
				int num = QuickLoad.XKeysymToKeycode(intPtr, 65505UL);
				int num2 = QuickLoad.XKeysymToKeycode(intPtr, 65506UL);
				byte[] array = new byte[32];
				QuickLoad.XQueryKeymap(intPtr, array);
				bool flag = ((int)array[num / 8] & 1 << num % 8) != 0;
				bool flag2 = ((int)array[num2 / 8] & 1 << num2 % 8) != 0;
				result = (flag || flag2);
			}
			finally
			{
				QuickLoad.XCloseDisplay(intPtr);
			}
			return result;
		}

		// Token: 0x0400151F RID: 5407
		private static readonly string FilePath = Path.Combine(Main.SavePath, "dev-quickload.json");

		// Token: 0x04001520 RID: 5408
		private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
		{
			ContractResolver = new EasyDeserializationJsonContractResolver(),
			TypeNameHandling = 4
		};

		// Token: 0x04001521 RID: 5409
		private static QuickLoad _loadedConfig;

		// Token: 0x02000725 RID: 1829
		public class JoinWorld : QuickLoad
		{
			// Token: 0x0600405F RID: 16479 RVA: 0x0069CAA0 File Offset: 0x0069ACA0
			protected override void Start()
			{
				string s;
				int num;
				if (Program.LaunchParameters.TryGetValue("-quickloadclient", out s) && int.TryParse(s, out num) && num < this.ExtraClients.Count)
				{
					this.ExtraClients[num].Start();
					return;
				}
				if (this.ExtraClients != null)
				{
					this.LaunchExtraClients();
				}
				this.RestoreWindowBounds();
				this.SelectPlayerAndWorld();
				this.PlayWorldOrJoinServer();
			}

			// Token: 0x06004060 RID: 16480 RVA: 0x0069CB0C File Offset: 0x0069AD0C
			private void RestoreWindowBounds()
			{
				if (this.WindowedBounds == null)
				{
					return;
				}
				Rectangle value = this.WindowedBounds.Value;
				if (Platform.Get<IWindowService>().IsSizeable(Main.instance.Window))
				{
					Main.SetResolution(value.Width, value.Height);
					Platform.Get<IWindowService>().SetPosition(Main.instance.Window, value.X, value.Y);
				}
			}

			// Token: 0x06004061 RID: 16481 RVA: 0x0069CB7A File Offset: 0x0069AD7A
			private void SaveWindowBounds()
			{
				if (Platform.Get<IWindowService>().IsSizeable(Main.instance.Window))
				{
					this.WindowedBounds = new Rectangle?(Platform.Get<IWindowService>().GetBounds(Main.instance.Window));
				}
			}

			// Token: 0x06004062 RID: 16482 RVA: 0x0069CBB4 File Offset: 0x0069ADB4
			private void LaunchExtraClients()
			{
				for (int i = 0; i < this.ExtraClients.Count; i++)
				{
					Process.Start(Process.GetCurrentProcess().ProcessName, "-quickloadclient " + i);
				}
			}

			// Token: 0x06004063 RID: 16483 RVA: 0x0069CBF8 File Offset: 0x0069ADF8
			protected void SelectPlayerAndWorld()
			{
				Main.LoadPlayers();
				Main.SelectPlayer(Main.PlayerList.Single((PlayerFileData p) => p.Path == this.PlayerPath));
				if (!string.IsNullOrEmpty(this.WorldPath))
				{
					Main.WorldList.Single((WorldFileData w) => w.Path == this.WorldPath).SetAsActive();
				}
			}

			// Token: 0x06004064 RID: 16484 RVA: 0x0069CC50 File Offset: 0x0069AE50
			protected void PlayWorldOrJoinServer()
			{
				WorldGen.Hooks.OnWorldLoad += this.OnWorldLoad;
				Main.menuMode = 10;
				if (this.ServerIPText == null)
				{
					WorldGen.playWorld();
					return;
				}
				Netplay.ServerPassword = this.ServerPassword;
				if (this.IsHostAndPlay)
				{
					Main.HostAndPlay();
					return;
				}
				Main.autoPass = true;
				Netplay.SetRemoteIP(this.ServerIPText);
				Netplay.StartTcpClient();
				Main.statusText = Language.GetTextValue("Net.ConnectingTo", this.ServerIPText);
			}

			// Token: 0x06004065 RID: 16485 RVA: 0x0069CCC8 File Offset: 0x0069AEC8
			private void OnWorldLoad()
			{
				WorldGen.Hooks.OnWorldLoad -= this.OnWorldLoad;
				if (Main.ActiveWorldFileData.Path != this.WorldPath || Main.ActivePlayerFileData.Path != this.PlayerPath || (this.ServerIPText != null && Main.netMode != 1) || (Main.netMode == 1 && this.ServerIPText != Netplay.ServerIPText))
				{
					return;
				}
				if (this.PlayerPosition != Vector2.Zero)
				{
					Main.LocalPlayer.position = this.PlayerPosition;
				}
				if (this.MountType != 0)
				{
					Main.LocalPlayer.mount.SetMount(this.MountType, Main.LocalPlayer);
				}
				DebugUtils.QuickSPMessage("/onquickload");
			}

			// Token: 0x06004066 RID: 16486 RVA: 0x0069CD90 File Offset: 0x0069AF90
			public virtual QuickLoad.JoinWorld WithCurrentState()
			{
				this.SaveWindowBounds();
				Main.SaveSettings();
				if (!Main.gameMenu)
				{
					Player.SavePlayer(Main.ActivePlayerFileData, false);
				}
				if (Main.WorldFileMetadata == null)
				{
					Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
				}
				if (Main.netMode != 1)
				{
					WorldFile.SaveWorld();
				}
				this.PlayerPath = Main.ActivePlayerFileData.Path;
				this.WorldPath = Main.ActiveWorldFileData.Path;
				if (Main.netMode == 1)
				{
					this.ServerIPText = Netplay.ServerIPText;
					this.ServerPassword = Netplay.ServerPassword;
					this.IsHostAndPlay = Netplay.IsHostAndPlay;
					if (this.IsHostAndPlay)
					{
						NetMessage.SendData(94, -1, -1, NetworkText.FromLiteral("/quickload-clientprobe"), 0, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				if (!Main.gameMenu)
				{
					this.PlayerPosition = Main.LocalPlayer.position;
					this.MountType = Main.LocalPlayer.mount.Type;
				}
				this.InDebugRegenUI = (UIWorldGenDebug.ActiveInstance != null);
				if (this.InDebugRegenUI)
				{
					if (UIWorldGenDebug.CurrentTargetOrLatestPass != null)
					{
						this.RegenTargetPassName = UIWorldGenDebug.CurrentTargetOrLatestPass.Name;
					}
					this.RegenSnapshotFrequency = WorldGenerator.CurrentController.SnapshotFrequency;
					this.RegenPauseOnHashMismatch = WorldGenerator.CurrentController.PauseOnHashMismatch;
				}
				return this;
			}

			// Token: 0x06004067 RID: 16487 RVA: 0x0069CECC File Offset: 0x0069B0CC
			protected WorldGenerator.Controller CreateRegenController()
			{
				WorldGen.PrepForRegen();
				return new WorldGenerator.Controller(WorldGen.Manifest)
				{
					Paused = (this.InDebugRegenUI && this.RegenTargetPassName == null),
					SnapshotFrequency = this.RegenSnapshotFrequency,
					PauseOnHashMismatch = this.RegenPauseOnHashMismatch,
					OnPassesLoaded = delegate(WorldGenerator.Controller c)
					{
						c.PauseAfterPass = c.Passes.FirstOrDefault((GenPass p) => p.Name == this.RegenTargetPassName);
						if (c.PauseAfterPass != null)
						{
							c.TryRunToEndOfPass(c.PauseAfterPass, true, true);
						}
					}
				};
			}

			// Token: 0x04006931 RID: 26929
			public Rectangle? WindowedBounds;

			// Token: 0x04006932 RID: 26930
			public string PlayerPath;

			// Token: 0x04006933 RID: 26931
			public string WorldPath;

			// Token: 0x04006934 RID: 26932
			public string ServerIPText;

			// Token: 0x04006935 RID: 26933
			public string ServerPassword;

			// Token: 0x04006936 RID: 26934
			public bool IsHostAndPlay;

			// Token: 0x04006937 RID: 26935
			public List<QuickLoad.JoinWorld> ExtraClients = new List<QuickLoad.JoinWorld>();

			// Token: 0x04006938 RID: 26936
			public Vector2 PlayerPosition;

			// Token: 0x04006939 RID: 26937
			public int MountType;

			// Token: 0x0400693A RID: 26938
			public bool InDebugRegenUI;

			// Token: 0x0400693B RID: 26939
			public string RegenTargetPassName;

			// Token: 0x0400693C RID: 26940
			public WorldGenerator.SnapshotFrequency RegenSnapshotFrequency;

			// Token: 0x0400693D RID: 26941
			public bool RegenPauseOnHashMismatch;
		}

		// Token: 0x02000726 RID: 1830
		public class RegenWorld : QuickLoad.JoinWorld
		{
			// Token: 0x0600406D RID: 16493 RVA: 0x0069CFAE File Offset: 0x0069B1AE
			protected override void Start()
			{
				base.SelectPlayerAndWorld();
				this.GenerateWorld();
			}

			// Token: 0x0600406E RID: 16494 RVA: 0x0069CFBC File Offset: 0x0069B1BC
			private void GenerateWorld()
			{
				WorldGen.CreateNewWorld(null, base.CreateRegenController(), new WorldGen.WorldGenerationFinishCallback(this.OnGenerationFinished));
			}

			// Token: 0x0600406F RID: 16495 RVA: 0x0069CFD7 File Offset: 0x0069B1D7
			private void OnGenerationFinished(bool playable)
			{
				if (!playable)
				{
					return;
				}
				this.InDebugRegenUI = false;
				base.PlayWorldOrJoinServer();
			}
		}
	}
}
