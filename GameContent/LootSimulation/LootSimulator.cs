using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ReLogic.OS;
using Terraria.ID;

namespace Terraria.GameContent.LootSimulation
{
	// Token: 0x020002E8 RID: 744
	public class LootSimulator
	{
		// Token: 0x0600263D RID: 9789 RVA: 0x0055D9B2 File Offset: 0x0055BBB2
		public LootSimulator()
		{
			this.FillDesiredTestConditions();
			this.FillItemExclusions();
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x0055D9E0 File Offset: 0x0055BBE0
		private void FillItemExclusions()
		{
			List<int> list = new List<int>();
			list.AddRange(from tuple in ItemID.Sets.IsAPickup.Select((bool state, int index) => new
			{
				index,
				state
			})
			where tuple.state
			select tuple.index);
			list.AddRange(from tuple in ItemID.Sets.CommonCoin.Select((bool state, int index) => new
			{
				index,
				state
			})
			where tuple.state
			select tuple.index);
			this._excludedItemIds = list.ToArray();
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x0055DAF0 File Offset: 0x0055BCF0
		private void FillDesiredTestConditions()
		{
			this._neededTestConditions.AddRange(new List<ISimulationConditionSetter>
			{
				SimulationConditionSetters.MidDay,
				SimulationConditionSetters.MidNight,
				SimulationConditionSetters.HardMode,
				SimulationConditionSetters.ExpertMode,
				SimulationConditionSetters.ExpertAndHardMode,
				SimulationConditionSetters.WindyExpertHardmodeEndgameBloodMoonNight,
				SimulationConditionSetters.WindyExpertHardmodeEndgameEclipseMorning,
				SimulationConditionSetters.SlimeStaffTest,
				SimulationConditionSetters.LuckyCoinTest
			});
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x0055DB70 File Offset: 0x0055BD70
		public void Run()
		{
			int timesMultiplier = 10000;
			this.SetCleanSlateWorldConditions();
			string text = "";
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int i = -65; i < (int)NPCID.Count; i++)
			{
				string str;
				if (this.TryGettingLootFor(i, timesMultiplier, out str))
				{
					text = text + str + "\n\n";
				}
			}
			stopwatch.Stop();
			text += string.Format("\nSimulation Took {0} seconds to complete.\n", (float)stopwatch.ElapsedMilliseconds / 1000f);
			Platform.Get<IClipboard>().Value = text;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x0055DC00 File Offset: 0x0055BE00
		private void SetCleanSlateWorldConditions()
		{
			Main.dayTime = true;
			Main.time = 27000.0;
			Main.hardMode = false;
			Main.GameMode = 0;
			NPC.downedMechBoss1 = false;
			NPC.downedMechBoss2 = false;
			NPC.downedMechBoss3 = false;
			NPC.downedMechBossAny = false;
			NPC.downedPlantBoss = false;
			Main._shouldUseWindyDayMusic = false;
			Main._shouldUseStormMusic = false;
			Main.eclipse = false;
			Main.bloodMoon = false;
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x0055DC64 File Offset: 0x0055BE64
		private bool TryGettingLootFor(int npcNetId, int timesMultiplier, out string outputText)
		{
			SimulatorInfo simulatorInfo = new SimulatorInfo();
			NPC npc = new NPC();
			npc.SetDefaults(npcNetId, default(NPCSpawnParams));
			simulatorInfo.npcVictim = npc;
			LootSimulationItemCounter lootSimulationItemCounter = new LootSimulationItemCounter();
			simulatorInfo.itemCounter = lootSimulationItemCounter;
			foreach (ISimulationConditionSetter simulationConditionSetter in this._neededTestConditions)
			{
				simulationConditionSetter.Setup(simulatorInfo);
				int num = simulationConditionSetter.GetTimesToRunMultiplier(simulatorInfo) * timesMultiplier;
				for (int i = 0; i < num; i++)
				{
					npc.NPCLoot();
				}
				lootSimulationItemCounter.IncreaseTimesAttempted(num, simulatorInfo.runningExpertMode);
				simulationConditionSetter.TearDown(simulatorInfo);
				this.SetCleanSlateWorldConditions();
			}
			lootSimulationItemCounter.Exclude(this._excludedItemIds.ToArray<int>());
			string text = lootSimulationItemCounter.PrintCollectedItems(false);
			string text2 = lootSimulationItemCounter.PrintCollectedItems(true);
			string name = NPCID.Search.GetName(npcNetId);
			string text3 = string.Format("FindEntryByNPCID(NPCID.{0})", name);
			if (text.Length > 0)
			{
				text3 = string.Format("{0}\n.AddDropsNormalMode({1})", text3, text);
			}
			if (text2.Length > 0)
			{
				text3 = string.Format("{0}\n.AddDropsExpertMode({1})", text3, text2);
			}
			text3 += ";";
			outputText = text3;
			return text.Length > 0 || text2.Length > 0;
		}

		// Token: 0x04005042 RID: 20546
		private List<ISimulationConditionSetter> _neededTestConditions = new List<ISimulationConditionSetter>();

		// Token: 0x04005043 RID: 20547
		private int[] _excludedItemIds = new int[0];
	}
}
