using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x0200026F RID: 623
	public class BackgroundChangeFlashInfo
	{
		// Token: 0x06002404 RID: 9220 RVA: 0x005498A0 File Offset: 0x00547AA0
		public void UpdateCache()
		{
			this.UpdateVariation(0, WorldGen.treeBG1);
			this.UpdateVariation(1, WorldGen.treeBG2);
			this.UpdateVariation(2, WorldGen.treeBG3);
			this.UpdateVariation(3, WorldGen.treeBG4);
			this.UpdateVariation(4, WorldGen.corruptBG);
			this.UpdateVariation(5, WorldGen.jungleBG);
			this.UpdateVariation(6, WorldGen.snowBG);
			this.UpdateVariation(7, WorldGen.hallowBG);
			this.UpdateVariation(8, WorldGen.crimsonBG);
			this.UpdateVariation(9, WorldGen.desertBG);
			this.UpdateVariation(10, WorldGen.oceanBG);
			this.UpdateVariation(11, WorldGen.mushroomBG);
			this.UpdateVariation(12, WorldGen.underworldBG);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x0054994D File Offset: 0x00547B4D
		private void UpdateVariation(int areaId, int newVariationValue)
		{
			int num = this._variations[areaId];
			this._variations[areaId] = newVariationValue;
			if (num != newVariationValue)
			{
				this.ValueChanged(areaId);
			}
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0054996A File Offset: 0x00547B6A
		private void ValueChanged(int areaId)
		{
			if (Main.gameMenu)
			{
				return;
			}
			this._flashPower[areaId] = 1f;
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x00549984 File Offset: 0x00547B84
		public void UpdateFlashValues()
		{
			for (int i = 0; i < this._flashPower.Length; i++)
			{
				this._flashPower[i] = MathHelper.Clamp(this._flashPower[i] - 0.05f, 0f, 1f);
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x005499C9 File Offset: 0x00547BC9
		public float GetFlashPower(int areaId)
		{
			return this._flashPower[areaId];
		}

		// Token: 0x04004DAB RID: 19883
		private int[] _variations = new int[TreeTopsInfo.AreaId.Count];

		// Token: 0x04004DAC RID: 19884
		private float[] _flashPower = new float[TreeTopsInfo.AreaId.Count];
	}
}
