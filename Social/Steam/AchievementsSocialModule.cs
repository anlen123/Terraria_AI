using System;
using System.Collections.Generic;
using System.Threading;
using Steamworks;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	// Token: 0x02000144 RID: 324
	public class AchievementsSocialModule : AchievementsSocialModule
	{
		// Token: 0x06001C99 RID: 7321 RVA: 0x004FE5A6 File Offset: 0x004FC7A6
		public override void Initialize()
		{
			this._userStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
			SteamUserStats.RequestCurrentStats();
			while (!this._areStatsReceived)
			{
				CoreSocialModule.Pulse();
				Thread.Sleep(10);
			}
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x004FE5DB File Offset: 0x004FC7DB
		public override void Shutdown()
		{
			this._userStatsReceived.Unregister();
			this.StoreStats();
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x004FE5F0 File Offset: 0x004FC7F0
		public override bool IsAchievementCompleted(string name)
		{
			bool flag;
			return SteamUserStats.GetAchievement(name, ref flag) && flag;
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x004FE608 File Offset: 0x004FC808
		public override byte[] GetEncryptionKey()
		{
			byte[] array = new byte[16];
			byte[] bytes = BitConverter.GetBytes(SteamUser.GetSteamID().m_SteamID);
			Array.Copy(bytes, array, 8);
			Array.Copy(bytes, 0, array, 8, 8);
			return array;
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x004FE63E File Offset: 0x004FC83E
		public override string GetSavePath()
		{
			return "/achievements-steam.dat";
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x004FE648 File Offset: 0x004FC848
		private int GetIntStat(string name)
		{
			int num;
			if (this._intStatCache.TryGetValue(name, out num))
			{
				return num;
			}
			if (SteamUserStats.GetStat(name, ref num))
			{
				this._intStatCache.Add(name, num);
			}
			return num;
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x004FE680 File Offset: 0x004FC880
		private float GetFloatStat(string name)
		{
			float num;
			if (this._floatStatCache.TryGetValue(name, out num))
			{
				return num;
			}
			if (SteamUserStats.GetStat(name, ref num))
			{
				this._floatStatCache.Add(name, num);
			}
			return num;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x004FE6B7 File Offset: 0x004FC8B7
		private bool SetFloatStat(string name, float value)
		{
			this._floatStatCache[name] = value;
			return SteamUserStats.SetStat(name, value);
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x004FE6CD File Offset: 0x004FC8CD
		public override void UpdateIntStat(string name, int value)
		{
			if (this.GetIntStat(name) < value)
			{
				this.SetIntStat(name, value);
			}
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x004FE6E2 File Offset: 0x004FC8E2
		private bool SetIntStat(string name, int value)
		{
			this._intStatCache[name] = value;
			return SteamUserStats.SetStat(name, value);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x004FE6F8 File Offset: 0x004FC8F8
		public override void UpdateFloatStat(string name, float value)
		{
			if (this.GetFloatStat(name) < value)
			{
				this.SetFloatStat(name, value);
			}
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x004FE70D File Offset: 0x004FC90D
		public override void StoreStats()
		{
			SteamUserStats.StoreStats();
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x004FE715 File Offset: 0x004FC915
		public override void CompleteAchievement(string name)
		{
			SteamUserStats.SetAchievement(name);
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x004FE71E File Offset: 0x004FC91E
		private void OnUserStatsReceived(UserStatsReceived_t results)
		{
			if (results.m_nGameID == 105600UL && results.m_steamIDUser == SteamUser.GetSteamID())
			{
				this._areStatsReceived = true;
			}
		}

		// Token: 0x040015CC RID: 5580
		private const string FILE_NAME = "/achievements-steam.dat";

		// Token: 0x040015CD RID: 5581
		private Callback<UserStatsReceived_t> _userStatsReceived;

		// Token: 0x040015CE RID: 5582
		private bool _areStatsReceived;

		// Token: 0x040015CF RID: 5583
		private Dictionary<string, int> _intStatCache = new Dictionary<string, int>();

		// Token: 0x040015D0 RID: 5584
		private Dictionary<string, float> _floatStatCache = new Dictionary<string, float>();
	}
}
