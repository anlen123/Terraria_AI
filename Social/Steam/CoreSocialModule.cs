using System;
using System.Threading;
using ReLogic.OS;
using Steamworks;
using Terraria.Localization;
using Terraria.Utilities;

namespace Terraria.Social.Steam
{
	// Token: 0x02000146 RID: 326
	public class CoreSocialModule : ISocialModule
	{
		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06001CB2 RID: 7346 RVA: 0x004FE9FC File Offset: 0x004FCBFC
		// (remove) Token: 0x06001CB3 RID: 7347 RVA: 0x004FEA30 File Offset: 0x004FCC30
		public static event Action OnTick;

		// Token: 0x06001CB4 RID: 7348 RVA: 0x00009E06 File Offset: 0x00008006
		public static void SetSkipPulsing(bool shouldSkipPausing)
		{
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x004FEA64 File Offset: 0x004FCC64
		public void Initialize()
		{
			CoreSocialModule._instance = this;
			if (!Main.dedServ && SteamAPI.RestartAppIfNecessary(new AppId_t(105600U)))
			{
				Environment.Exit(1);
				return;
			}
			if (!SteamAPI.Init())
			{
				MessageBox.Show(Language.GetTextValue("Error.LaunchFromSteam"), Language.GetTextValue("Error.Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(1);
			}
			SteamInput.Init(false);
			this.IsSteamValid = true;
			new Thread(new ParameterizedThreadStart(this.SteamCallbackLoop))
			{
				IsBackground = true
			}.Start();
			new Thread(new ParameterizedThreadStart(this.SteamTickLoop))
			{
				IsBackground = true
			}.Start();
			Main.OnTickForThirdPartySoftwareOnly += this.PulseSteamTick;
			Main.OnTickForThirdPartySoftwareOnly += this.PulseSteamCallback;
			if (Platform.IsOSX && !Main.dedServ)
			{
				this._onOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnOverlayActivated));
			}
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x004FEB4F File Offset: 0x004FCD4F
		public void PulseSteamTick()
		{
			if (Monitor.TryEnter(this._steamTickLock))
			{
				Monitor.Pulse(this._steamTickLock);
				Monitor.Exit(this._steamTickLock);
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x004FEB74 File Offset: 0x004FCD74
		public void PulseSteamCallback()
		{
			if (Monitor.TryEnter(this._steamCallbackLock))
			{
				Monitor.Pulse(this._steamCallbackLock);
				Monitor.Exit(this._steamCallbackLock);
			}
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x004FEB99 File Offset: 0x004FCD99
		public static void Pulse()
		{
			CoreSocialModule._instance.PulseSteamTick();
			CoreSocialModule._instance.PulseSteamCallback();
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x004FEBB0 File Offset: 0x004FCDB0
		private void SteamTickLoop(object context)
		{
			Monitor.Enter(this._steamTickLock);
			while (this.IsSteamValid)
			{
				if (this._skipPulsing)
				{
					Monitor.Wait(this._steamCallbackLock);
				}
				else
				{
					if (CoreSocialModule.OnTick != null)
					{
						CoreSocialModule.OnTick();
					}
					Monitor.Wait(this._steamTickLock);
				}
			}
			Monitor.Exit(this._steamTickLock);
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x004FEC10 File Offset: 0x004FCE10
		private void SteamCallbackLoop(object context)
		{
			Monitor.Enter(this._steamCallbackLock);
			while (this.IsSteamValid)
			{
				if (this._skipPulsing)
				{
					Monitor.Wait(this._steamCallbackLock);
				}
				else
				{
					SteamAPI.RunCallbacks();
					Monitor.Wait(this._steamCallbackLock);
				}
			}
			Monitor.Exit(this._steamCallbackLock);
			SteamAPI.Shutdown();
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x004FEC69 File Offset: 0x004FCE69
		public void Shutdown()
		{
			this.IsSteamValid = false;
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x004FEC72 File Offset: 0x004FCE72
		public void OnOverlayActivated(GameOverlayActivated_t result)
		{
			Main.instance.IsMouseVisible = (result.m_bActive == 1);
		}

		// Token: 0x040015D4 RID: 5588
		private static CoreSocialModule _instance;

		// Token: 0x040015D5 RID: 5589
		public const int SteamAppId = 105600;

		// Token: 0x040015D6 RID: 5590
		private bool IsSteamValid;

		// Token: 0x040015D8 RID: 5592
		private object _steamTickLock = new object();

		// Token: 0x040015D9 RID: 5593
		private object _steamCallbackLock = new object();

		// Token: 0x040015DA RID: 5594
		private Callback<GameOverlayActivated_t> _onOverlayActivated;

		// Token: 0x040015DB RID: 5595
		private bool _skipPulsing;
	}
}
