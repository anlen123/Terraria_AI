using System;

namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
	// Token: 0x020002ED RID: 749
	public class FastConditionSetter : ISimulationConditionSetter
	{
		// Token: 0x06002651 RID: 9809 RVA: 0x0055E24D File Offset: 0x0055C44D
		public FastConditionSetter(Action<SimulatorInfo> setup, Action<SimulatorInfo> tearDown)
		{
			this._setup = setup;
			this._tearDown = tearDown;
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x0055E263 File Offset: 0x0055C463
		public void Setup(SimulatorInfo info)
		{
			if (this._setup != null)
			{
				this._setup(info);
			}
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x0055E279 File Offset: 0x0055C479
		public void TearDown(SimulatorInfo info)
		{
			if (this._tearDown != null)
			{
				this._tearDown(info);
			}
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000379F1 File Offset: 0x00035BF1
		public int GetTimesToRunMultiplier(SimulatorInfo info)
		{
			return 1;
		}

		// Token: 0x0400505E RID: 20574
		private Action<SimulatorInfo> _setup;

		// Token: 0x0400505F RID: 20575
		private Action<SimulatorInfo> _tearDown;
	}
}
