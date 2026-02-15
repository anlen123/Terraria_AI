using System;

namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
	// Token: 0x020002F0 RID: 752
	public class LuckyCoinConditionSetter : ISimulationConditionSetter
	{
		// Token: 0x0600265D RID: 9821 RVA: 0x0055E3E6 File Offset: 0x0055C5E6
		public LuckyCoinConditionSetter(int timesToRunMultiplier)
		{
			this._timesToRun = timesToRunMultiplier;
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x0055E3F8 File Offset: 0x0055C5F8
		public int GetTimesToRunMultiplier(SimulatorInfo info)
		{
			int netID = info.npcVictim.netID;
			if (netID != 216 && netID != 491)
			{
				return 0;
			}
			return this._timesToRun;
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x00009E06 File Offset: 0x00008006
		public void Setup(SimulatorInfo info)
		{
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x00009E06 File Offset: 0x00008006
		public void TearDown(SimulatorInfo info)
		{
		}

		// Token: 0x04005062 RID: 20578
		private int _timesToRun;
	}
}
