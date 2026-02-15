using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LootSimulation
{
	// Token: 0x020002E9 RID: 745
	public class SimulatorInfo
	{
		// Token: 0x06002643 RID: 9795 RVA: 0x0055DDC0 File Offset: 0x0055BFC0
		public SimulatorInfo()
		{
			this.player = new Player();
			this._originalDayTimeCounter = Main.time;
			this._originalDayTimeFlag = Main.dayTime;
			this._originalPlayerPosition = this.player.position;
			this.runningExpertMode = false;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x0055DE0C File Offset: 0x0055C00C
		public void ReturnToOriginalDaytime()
		{
			Main.dayTime = this._originalDayTimeFlag;
			Main.time = this._originalDayTimeCounter;
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x0055DE24 File Offset: 0x0055C024
		public void AddItem(int itemId, int amount)
		{
			this.itemCounter.AddItem(itemId, amount, this.runningExpertMode);
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x0055DE39 File Offset: 0x0055C039
		public void ReturnToOriginalPlayerPosition()
		{
			this.player.position = this._originalPlayerPosition;
		}

		// Token: 0x04005044 RID: 20548
		public Player player;

		// Token: 0x04005045 RID: 20549
		private double _originalDayTimeCounter;

		// Token: 0x04005046 RID: 20550
		private bool _originalDayTimeFlag;

		// Token: 0x04005047 RID: 20551
		private Vector2 _originalPlayerPosition;

		// Token: 0x04005048 RID: 20552
		public bool runningExpertMode;

		// Token: 0x04005049 RID: 20553
		public LootSimulationItemCounter itemCounter;

		// Token: 0x0400504A RID: 20554
		public NPC npcVictim;
	}
}
