using System;

namespace Terraria.GameContent.LootSimulation
{
	// Token: 0x020002EB RID: 747
	public interface ISimulationConditionSetter
	{
		// Token: 0x0600264C RID: 9804
		int GetTimesToRunMultiplier(SimulatorInfo info);

		// Token: 0x0600264D RID: 9805
		void Setup(SimulatorInfo info);

		// Token: 0x0600264E RID: 9806
		void TearDown(SimulatorInfo info);
	}
}
