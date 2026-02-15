using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x0200022D RID: 557
	public abstract class NPCDamageTracker
	{
		// Token: 0x060021CE RID: 8654 RVA: 0x00531FB4 File Offset: 0x005301B4
		public static NPCDamageTracker.CustomDefinition RegisterCompositeTypeBoss(params int[] types)
		{
			NPCDamageTracker.CustomDefinition customDefinition = new NPCDamageTracker.CustomDefinition
			{
				NPCTypes = types.ToList<int>()
			};
			foreach (int num in types)
			{
				NPCDamageTracker.CustomBossDefinitions[num] = customDefinition;
			}
			return customDefinition;
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00531FF0 File Offset: 0x005301F0
		public static void RegisterMobsForBoss(int bossType, params int[] mobTypes)
		{
			foreach (int num in mobTypes)
			{
				NPCDamageTracker.BossTypeForMob[num] = bossType;
			}
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x0053201C File Offset: 0x0053021C
		static NPCDamageTracker()
		{
			NPCDamageTracker.RegisterMobsForBoss(50, new int[]
			{
				1,
				535
			});
			NPCDamageTracker.RegisterMobsForBoss(4, new int[]
			{
				5
			});
			NPCDamageTracker.RegisterMobsForBoss(222, new int[]
			{
				210,
				211
			});
			NPCDamageTracker.RegisterCompositeTypeBoss(new int[]
			{
				13,
				14,
				15
			});
			NPCDamageTracker.RegisterCompositeTypeBoss(new int[]
			{
				266,
				267
			});
			NPCDamageTracker.RegisterCompositeTypeBoss(new int[]
			{
				35,
				36
			});
			NPCDamageTracker.RegisterMobsForBoss(113, new int[]
			{
				115,
				116,
				117,
				118,
				119
			});
			NPCDamageTracker.RegisterMobsForBoss(657, new int[]
			{
				658,
				659,
				660
			});
			NPCDamageTracker.RegisterCompositeTypeBoss(new int[]
			{
				126,
				125
			}).Name = Language.GetText("Enemies.TheTwins");
			NPCDamageTracker.RegisterCompositeTypeBoss(new int[]
			{
				127,
				128,
				129,
				130,
				131
			});
			NPCDamageTracker.RegisterMobsForBoss(134, new int[]
			{
				139
			});
			NPCDamageTracker.RegisterMobsForBoss(262, new int[]
			{
				264
			});
			NPCDamageTracker.RegisterCompositeTypeBoss(new int[]
			{
				245,
				246,
				247,
				248
			});
			NPCDamageTracker.RegisterMobsForBoss(370, new int[]
			{
				372,
				373
			});
			NPCDamageTracker.RegisterMobsForBoss(439, new int[]
			{
				454,
				455,
				456,
				457,
				458,
				459
			});
			NPCDamageTracker.RegisterCompositeTypeBoss(new int[]
			{
				398,
				396,
				397
			}).Name = Language.GetText("Enemies.MoonLord");
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x0053221F File Offset: 0x0053041F
		private static bool GetRealActiveNPC(ref NPC npc)
		{
			if (npc.realLife >= 0)
			{
				npc = Main.npc[npc.realLife];
			}
			return npc.active;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x00532248 File Offset: 0x00530448
		private static bool TryGetTrackerFor(NPC npc, out NPCDamageTracker tracker)
		{
			tracker = null;
			if (!NPCDamageTracker.GetRealActiveNPC(ref npc))
			{
				return false;
			}
			foreach (NPCDamageTracker npcdamageTracker in NPCDamageTracker._activeTrackers)
			{
				if (npcdamageTracker.IncludeDamageFor(npc))
				{
					tracker = npcdamageTracker;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x005322B4 File Offset: 0x005304B4
		private static bool CreateTrackerFor(NPC npc, out NPCDamageTracker tracker)
		{
			tracker = null;
			NPCDamageTracker.CustomDefinition customDefinition = NPCDamageTracker.CustomBossDefinitions[npc.type];
			if (customDefinition == null && !npc.boss)
			{
				return false;
			}
			tracker = new BossDamageTracker(npc.type, customDefinition);
			return true;
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x005322F0 File Offset: 0x005304F0
		public static void AddDamage(NPC npc, int owner, int damage)
		{
			if (!NPCDamageTracker.GetRealActiveNPC(ref npc) || npc.life <= 0)
			{
				return;
			}
			NPCDamageTracker npcdamageTracker;
			if (!NPCDamageTracker.TryGetTrackerFor(npc, out npcdamageTracker))
			{
				if (!NPCDamageTracker.CreateTrackerFor(npc, out npcdamageTracker))
				{
					return;
				}
				NPCDamageTracker.Start(npcdamageTracker);
			}
			npcdamageTracker.AddDamage(owner, Math.Min(damage, npc.life));
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00532340 File Offset: 0x00530540
		public static void AddDamageToLastAttack(NPC npc, int damage)
		{
			if (!NPCDamageTracker.GetRealActiveNPC(ref npc) || npc.life <= 0)
			{
				return;
			}
			NPCDamageTracker npcdamageTracker;
			if (!NPCDamageTracker.TryGetTrackerFor(npc, out npcdamageTracker))
			{
				return;
			}
			npcdamageTracker.AddDamageToLastAttack(Math.Min(damage, npc.life));
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00532380 File Offset: 0x00530580
		public static void BossKilled(NPC npc)
		{
			NPCDamageTracker npcdamageTracker;
			if (NPCDamageTracker.TryGetTrackerFor(npc, out npcdamageTracker))
			{
				npcdamageTracker.OnBossKilled(npc);
			}
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x0053239E File Offset: 0x0053059E
		public static void Start(NPCDamageTracker tracker)
		{
			NPCDamageTracker._activeTrackers.Add(tracker);
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x005323AB File Offset: 0x005305AB
		public static void Reset()
		{
			NPCDamageTracker._activeTrackers.Clear();
			NPCDamageTracker._recentFinishedTrackers.Clear();
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x005323C1 File Offset: 0x005305C1
		public static IEnumerable<NPCDamageTracker> RecentAttempts()
		{
			return NPCDamageTracker._recentFinishedTrackers;
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x005323C8 File Offset: 0x005305C8
		public static void Update()
		{
			foreach (NPCDamageTracker npcdamageTracker in NPCDamageTracker._activeTrackers)
			{
				npcdamageTracker.Tick();
			}
			foreach (NPCDamageTracker npcdamageTracker2 in NPCDamageTracker._recentFinishedTrackers)
			{
				npcdamageTracker2.Tick();
			}
			for (int i = NPCDamageTracker._activeTrackers.Count - 1; i >= 0; i--)
			{
				NPCDamageTracker._activeTrackers[i].CheckActive();
			}
			while (NPCDamageTracker._recentFinishedTrackers.Count > 1 && NPCDamageTracker._recentFinishedTrackers[0].TimeSinceLastHit > NPCDamageTracker.EXTRA_RECENT_TRACKER_EXPIRY_TIME)
			{
				NPCDamageTracker._recentFinishedTrackers.RemoveAt(0);
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x005324B0 File Offset: 0x005306B0
		private static void StopTracking(NPCDamageTracker tracker)
		{
			if (!NPCDamageTracker._activeTrackers.Remove(tracker) || tracker.IsEmpty)
			{
				return;
			}
			NPCDamageTracker._recentFinishedTrackers.Add(tracker);
			if (NPCDamageTracker._recentFinishedTrackers.Count > NPCDamageTracker.MAX_RECENT_TRACKERS)
			{
				NPCDamageTracker._recentFinishedTrackers.RemoveAt(0);
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x005324EF File Offset: 0x005306EF
		public bool IsEmpty
		{
			get
			{
				return this._list.Count == 0;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x005324FF File Offset: 0x005306FF
		public int Duration
		{
			get
			{
				return this._lastHitTime;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x00532507 File Offset: 0x00530707
		public int TimeSinceLastHit
		{
			get
			{
				return this._ticks - this._lastHitTime;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060021DF RID: 8671
		public abstract LocalizedText Name { get; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060021E0 RID: 8672
		public abstract LocalizedText KillTimeMessage { get; }

		// Token: 0x060021E1 RID: 8673
		protected abstract bool IncludeDamageFor(NPC npc);

		// Token: 0x060021E2 RID: 8674 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void CheckActive()
		{
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void OnBossKilled(NPC npc)
		{
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x00532516 File Offset: 0x00530716
		private void Tick()
		{
			this._ticks++;
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x00532526 File Offset: 0x00530726
		protected void Stop()
		{
			NPCDamageTracker.StopTracking(this);
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x00532530 File Offset: 0x00530730
		public void AddDamage(int owner, int damage)
		{
			this._lastHitTime = this._ticks;
			NPCDamageTracker.CreditEntry orAddEntry = this.GetOrAddEntry(owner);
			orAddEntry.Damage += damage;
			this._lastAttacker = orAddEntry;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x00532566 File Offset: 0x00530766
		public void AddDamageToLastAttack(int damage)
		{
			this._lastAttacker.Damage += damage;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x0053257C File Offset: 0x0053077C
		private NPCDamageTracker.CreditEntry GetOrAddEntry(int owner)
		{
			if (owner < 0 || owner >= 255)
			{
				if (this._worldCredit == null)
				{
					this._worldCredit = new NPCDamageTracker.WorldCreditEntry();
					this._list.Add(this._worldCredit);
				}
				return this._worldCredit;
			}
			string name = Main.player[owner].name;
			foreach (NPCDamageTracker.CreditEntry creditEntry in this._list)
			{
				NPCDamageTracker.PlayerCreditEntry playerCreditEntry = creditEntry as NPCDamageTracker.PlayerCreditEntry;
				if (playerCreditEntry != null && playerCreditEntry.PlayerName == name)
				{
					return playerCreditEntry;
				}
			}
			NPCDamageTracker.PlayerCreditEntry playerCreditEntry2 = new NPCDamageTracker.PlayerCreditEntry(name);
			this._list.Add(playerCreditEntry2);
			return playerCreditEntry2;
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x00532640 File Offset: 0x00530840
		public NetworkText GetReport(Player forPlayer = null)
		{
			this._list.Sort();
			int[] array = (from x in this._list
			select x.Damage).ToArray<int>();
			int[] array2 = NPCDamageTracker.CalculatePercentages(array);
			int length = array.Max().ToString().Length;
			List<NetworkText> list = new List<NetworkText>(this._list.Count + 2);
			StringBuilder stringBuilder = new StringBuilder("{0}");
			list.Add(NetworkText.FromKey("BossDamageCommand.Title", new object[]
			{
				this.Name.ToNetworkText()
			}));
			LocalizedText killTimeMessage = this.KillTimeMessage;
			if (killTimeMessage != null)
			{
				stringBuilder.Append("\n{1}");
				TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.Duration / 60.0);
				string text = string.Format("{0}:{1:00}", (int)timeSpan.TotalMinutes, timeSpan.Seconds);
				list.Add(killTimeMessage.ToNetworkText(new object[]
				{
					text
				}));
			}
			for (int i = 0; i < this._list.Count; i++)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(array2[i]).Append('%');
				while (stringBuilder2.Length < 6)
				{
					stringBuilder2.Append(' ');
				}
				stringBuilder2.Append(array[i]);
				while (stringBuilder2.Length < 8 + length)
				{
					stringBuilder2.Append(' ');
				}
				NPCDamageTracker.CreditEntry creditEntry = this._list[i];
				stringBuilder2.Append('{').Append(list.Count).Append('}');
				list.Add(creditEntry.Name);
				string text2 = stringBuilder2.ToString();
				if (forPlayer != null && creditEntry is NPCDamageTracker.PlayerCreditEntry && ((NPCDamageTracker.PlayerCreditEntry)creditEntry).PlayerName == forPlayer.name)
				{
					text2 = "[c/FFAF00:" + text2 + "]";
				}
				stringBuilder.Append('\n').Append(text2);
			}
			string text3 = stringBuilder.ToString();
			object[] substitutions = list.ToArray();
			return NetworkText.FromFormattable(text3, substitutions);
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x00532870 File Offset: 0x00530A70
		private static int[] CalculatePercentages(int[] damages)
		{
			int num = damages.Sum();
			int[] array = new int[damages.Length];
			double[] array2 = new double[damages.Length];
			int i = 0;
			for (int j = 0; j < damages.Length; j++)
			{
				double num2 = (double)(damages[j] * 100) / (double)num;
				int num3 = (int)num2;
				array[j] = num3;
				array2[j] = num2 - (double)num3;
				i += num3;
			}
			while (i < 100)
			{
				int num4 = 0;
				double num5 = 0.0;
				for (int k = 0; k < damages.Length; k++)
				{
					if (array2[k] > num5)
					{
						num5 = array2[k];
						num4 = k;
					}
				}
				array2[num4] = 0.0;
				array[num4]++;
				i++;
			}
			return array;
		}

		// Token: 0x04004C94 RID: 19604
		public static NPCDamageTracker.CustomDefinition[] CustomBossDefinitions = NPCID.Sets.Factory.CreateCustomSet<NPCDamageTracker.CustomDefinition>(null, new object[0]);

		// Token: 0x04004C95 RID: 19605
		public static int[] BossTypeForMob = NPCID.Sets.Factory.CreateIntSet(new int[0]);

		// Token: 0x04004C96 RID: 19606
		private static List<NPCDamageTracker> _activeTrackers = new List<NPCDamageTracker>();

		// Token: 0x04004C97 RID: 19607
		private static List<NPCDamageTracker> _recentFinishedTrackers = new List<NPCDamageTracker>();

		// Token: 0x04004C98 RID: 19608
		private static readonly int MAX_RECENT_TRACKERS = 3;

		// Token: 0x04004C99 RID: 19609
		private static readonly int EXTRA_RECENT_TRACKER_EXPIRY_TIME = 54000;

		// Token: 0x04004C9A RID: 19610
		private readonly List<NPCDamageTracker.CreditEntry> _list = new List<NPCDamageTracker.CreditEntry>(255);

		// Token: 0x04004C9B RID: 19611
		private NPCDamageTracker.WorldCreditEntry _worldCredit;

		// Token: 0x04004C9C RID: 19612
		private NPCDamageTracker.CreditEntry _lastAttacker;

		// Token: 0x04004C9D RID: 19613
		private int _ticks;

		// Token: 0x04004C9E RID: 19614
		private int _lastHitTime;

		// Token: 0x020007AC RID: 1964
		public class CustomDefinition
		{
			// Token: 0x04007073 RID: 28787
			public List<int> NPCTypes;

			// Token: 0x04007074 RID: 28788
			public LocalizedText Name;
		}

		// Token: 0x020007AD RID: 1965
		private abstract class CreditEntry : IComparable<NPCDamageTracker.CreditEntry>
		{
			// Token: 0x1700052D RID: 1325
			// (get) Token: 0x060041B2 RID: 16818 RVA: 0x006BB327 File Offset: 0x006B9527
			// (set) Token: 0x060041B3 RID: 16819 RVA: 0x006BB32F File Offset: 0x006B952F
			public int Damage { get; set; }

			// Token: 0x1700052E RID: 1326
			// (get) Token: 0x060041B4 RID: 16820
			public abstract NetworkText Name { get; }

			// Token: 0x060041B5 RID: 16821 RVA: 0x006BB338 File Offset: 0x006B9538
			public int CompareTo(NPCDamageTracker.CreditEntry other)
			{
				return -this.Damage.CompareTo(other.Damage);
			}
		}

		// Token: 0x020007AE RID: 1966
		private class PlayerCreditEntry : NPCDamageTracker.CreditEntry
		{
			// Token: 0x060041B7 RID: 16823 RVA: 0x006BB35A File Offset: 0x006B955A
			public PlayerCreditEntry(string name)
			{
				this.PlayerName = name;
			}

			// Token: 0x1700052F RID: 1327
			// (get) Token: 0x060041B8 RID: 16824 RVA: 0x006BB369 File Offset: 0x006B9569
			public override NetworkText Name
			{
				get
				{
					return NetworkText.FromLiteral(this.PlayerName);
				}
			}

			// Token: 0x04007076 RID: 28790
			public readonly string PlayerName;
		}

		// Token: 0x020007AF RID: 1967
		private class WorldCreditEntry : NPCDamageTracker.CreditEntry
		{
			// Token: 0x17000530 RID: 1328
			// (get) Token: 0x060041B9 RID: 16825 RVA: 0x006BB376 File Offset: 0x006B9576
			public override NetworkText Name
			{
				get
				{
					return NetworkText.FromKey("BossDamageCommand.WorldCreditName", new object[0]);
				}
			}
		}
	}
}
