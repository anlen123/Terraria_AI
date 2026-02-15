using System;

namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
	// Token: 0x020002EE RID: 750
	public class StackedConditionSetter : ISimulationConditionSetter
	{
		// Token: 0x06002655 RID: 9813 RVA: 0x0055E28F File Offset: 0x0055C48F
		public StackedConditionSetter(params ISimulationConditionSetter[] setters)
		{
			this._setters = setters;
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x0055E2A0 File Offset: 0x0055C4A0
		public void Setup(SimulatorInfo info)
		{
			for (int i = 0; i < this._setters.Length; i++)
			{
				this._setters[i].Setup(info);
			}
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x0055E2D0 File Offset: 0x0055C4D0
		public void TearDown(SimulatorInfo info)
		{
			for (int i = 0; i < this._setters.Length; i++)
			{
				this._setters[i].TearDown(info);
			}
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000379F1 File Offset: 0x00035BF1
		public int GetTimesToRunMultiplier(SimulatorInfo info)
		{
			return 1;
		}

		// Token: 0x04005060 RID: 20576
		private ISimulationConditionSetter[] _setters;
	}
}
