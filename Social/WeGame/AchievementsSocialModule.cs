using System;
using System.Threading;
using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000125 RID: 293
	public class AchievementsSocialModule : AchievementsSocialModule
	{
		// Token: 0x06001B73 RID: 7027 RVA: 0x004FA560 File Offset: 0x004F8760
		public override void Initialize()
		{
			this._callbackHelper.RegisterCallback(2001, new RailEventCallBackHandler(this.RailEventCallBack));
			this._callbackHelper.RegisterCallback(2101, new RailEventCallBackHandler(this.RailEventCallBack));
			IRailPlayerStats myPlayerStats = this.GetMyPlayerStats();
			IRailPlayerAchievement myPlayerAchievement = this.GetMyPlayerAchievement();
			if (myPlayerStats != null && myPlayerAchievement != null)
			{
				myPlayerStats.AsyncRequestStats("");
				myPlayerAchievement.AsyncRequestAchievement("");
				while (!this._areStatsReceived && !this._areAchievementReceived)
				{
					CoreSocialModule.RailEventTick();
					Thread.Sleep(10);
				}
			}
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x004FA5EF File Offset: 0x004F87EF
		public override void Shutdown()
		{
			this.StoreStats();
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x004FA5F8 File Offset: 0x004F87F8
		private IRailPlayerStats GetMyPlayerStats()
		{
			if (this._playerStats == null)
			{
				IRailStatisticHelper railStatisticHelper = rail_api.RailFactory().RailStatisticHelper();
				if (railStatisticHelper != null)
				{
					this._playerStats = railStatisticHelper.CreatePlayerStats(new RailID
					{
						id_ = 0UL
					});
				}
			}
			return this._playerStats;
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x004FA63C File Offset: 0x004F883C
		private IRailPlayerAchievement GetMyPlayerAchievement()
		{
			if (this._playerAchievement == null)
			{
				IRailAchievementHelper railAchievementHelper = rail_api.RailFactory().RailAchievementHelper();
				if (railAchievementHelper != null)
				{
					this._playerAchievement = railAchievementHelper.CreatePlayerAchievement(new RailID
					{
						id_ = 0UL
					});
				}
			}
			return this._playerAchievement;
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x004FA680 File Offset: 0x004F8880
		public void RailEventCallBack(RAILEventID eventId, EventBase data)
		{
			if (eventId == 2001)
			{
				this._areStatsReceived = true;
				return;
			}
			if (eventId != 2101)
			{
				return;
			}
			this._areAchievementReceived = true;
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x004FA6A4 File Offset: 0x004F88A4
		public override bool IsAchievementCompleted(string name)
		{
			bool flag = false;
			RailResult railResult = 1;
			IRailPlayerAchievement myPlayerAchievement = this.GetMyPlayerAchievement();
			if (myPlayerAchievement != null)
			{
				railResult = myPlayerAchievement.HasAchieved(name, ref flag);
			}
			return flag && railResult == 0;
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x004FA6D4 File Offset: 0x004F88D4
		public override byte[] GetEncryptionKey()
		{
			RailComparableID railID = rail_api.RailFactory().RailPlayer().GetRailID();
			byte[] array = new byte[16];
			byte[] bytes = BitConverter.GetBytes(railID.id_);
			Array.Copy(bytes, array, 8);
			Array.Copy(bytes, 0, array, 8, 8);
			return array;
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x004FA714 File Offset: 0x004F8914
		public override string GetSavePath()
		{
			return "/achievements-wegame.dat";
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x004FA71C File Offset: 0x004F891C
		private int GetIntStat(string name)
		{
			int result = 0;
			IRailPlayerStats myPlayerStats = this.GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				myPlayerStats.GetStatValue(name, ref result);
			}
			return result;
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x004FA740 File Offset: 0x004F8940
		private float GetFloatStat(string name)
		{
			double num = 0.0;
			IRailPlayerStats myPlayerStats = this.GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				myPlayerStats.GetStatValue(name, ref num);
			}
			return (float)num;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x004FA770 File Offset: 0x004F8970
		private bool SetFloatStat(string name, float value)
		{
			IRailPlayerStats myPlayerStats = this.GetMyPlayerStats();
			RailResult railResult = 1;
			if (myPlayerStats != null)
			{
				railResult = myPlayerStats.SetStatValue(name, (double)value);
			}
			return railResult == 0;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x004FA798 File Offset: 0x004F8998
		public override void UpdateIntStat(string name, int value)
		{
			IRailPlayerStats myPlayerStats = this.GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				int num = 0;
				if (myPlayerStats.GetStatValue(name, ref num) == null && num < value)
				{
					myPlayerStats.SetStatValue(name, value);
				}
			}
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x004FA7CC File Offset: 0x004F89CC
		private bool SetIntStat(string name, int value)
		{
			IRailPlayerStats myPlayerStats = this.GetMyPlayerStats();
			RailResult railResult = 1;
			if (myPlayerStats != null)
			{
				railResult = myPlayerStats.SetStatValue(name, value);
			}
			return railResult == 0;
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x004FA7F4 File Offset: 0x004F89F4
		public override void UpdateFloatStat(string name, float value)
		{
			IRailPlayerStats myPlayerStats = this.GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				double num = 0.0;
				if (myPlayerStats.GetStatValue(name, ref num) == null && (float)num < value)
				{
					myPlayerStats.SetStatValue(name, (double)value);
				}
			}
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x004FA82F File Offset: 0x004F8A2F
		public override void StoreStats()
		{
			this.SaveStats();
			this.SaveAchievement();
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x004FA840 File Offset: 0x004F8A40
		private void SaveStats()
		{
			IRailPlayerStats myPlayerStats = this.GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				myPlayerStats.AsyncStoreStats("");
			}
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x004FA864 File Offset: 0x004F8A64
		private void SaveAchievement()
		{
			IRailPlayerAchievement myPlayerAchievement = this.GetMyPlayerAchievement();
			if (myPlayerAchievement != null)
			{
				myPlayerAchievement.AsyncStoreAchievement("");
			}
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x004FA888 File Offset: 0x004F8A88
		public override void CompleteAchievement(string name)
		{
			IRailPlayerAchievement myPlayerAchievement = this.GetMyPlayerAchievement();
			if (myPlayerAchievement != null)
			{
				myPlayerAchievement.MakeAchievement(name);
			}
		}

		// Token: 0x0400156A RID: 5482
		private const string FILE_NAME = "/achievements-wegame.dat";

		// Token: 0x0400156B RID: 5483
		private bool _areStatsReceived;

		// Token: 0x0400156C RID: 5484
		private bool _areAchievementReceived;

		// Token: 0x0400156D RID: 5485
		private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

		// Token: 0x0400156E RID: 5486
		private IRailPlayerAchievement _playerAchievement;

		// Token: 0x0400156F RID: 5487
		private IRailPlayerStats _playerStats;
	}
}
