using System;

namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
	// Token: 0x020002EF RID: 751
	public class SlimeStaffConditionSetter : ISimulationConditionSetter
	{
		// Token: 0x06002659 RID: 9817 RVA: 0x0055E2FE File Offset: 0x0055C4FE
		public SlimeStaffConditionSetter(int timesToRunMultiplier)
		{
			this._timesToRun = timesToRunMultiplier;
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x0055E310 File Offset: 0x0055C510
		public int GetTimesToRunMultiplier(SimulatorInfo info)
		{
			int netID = info.npcVictim.netID;
			if (netID <= 147)
			{
				if (netID <= 1)
				{
					if (netID - -33 <= 1 || netID - -10 <= 7 || netID == 1)
					{
						goto IL_C3;
					}
				}
				else if (netID <= 138)
				{
					if (netID == 16 || netID == 138)
					{
						goto IL_C3;
					}
				}
				else if (netID == 141 || netID == 147)
				{
					goto IL_C3;
				}
			}
			else if (netID <= 302)
			{
				if (netID <= 187)
				{
					if (netID == 184 || netID == 187)
					{
						goto IL_C3;
					}
				}
				else if (netID == 204 || netID == 302)
				{
					goto IL_C3;
				}
			}
			else if (netID <= 433)
			{
				if (netID - 333 <= 3 || netID == 433)
				{
					goto IL_C3;
				}
			}
			else if (netID == 535 || netID == 537)
			{
				goto IL_C3;
			}
			return 0;
			IL_C3:
			return this._timesToRun;
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x00009E06 File Offset: 0x00008006
		public void Setup(SimulatorInfo info)
		{
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x00009E06 File Offset: 0x00008006
		public void TearDown(SimulatorInfo info)
		{
		}

		// Token: 0x04005061 RID: 20577
		private int _timesToRun;
	}
}
