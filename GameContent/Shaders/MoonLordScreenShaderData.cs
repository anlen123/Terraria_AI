using System;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	// Token: 0x0200029A RID: 666
	public class MoonLordScreenShaderData : ScreenShaderData
	{
		// Token: 0x06002540 RID: 9536 RVA: 0x005548E4 File Offset: 0x00552AE4
		public MoonLordScreenShaderData(string passName, bool aimAtPlayer) : base(passName)
		{
			this._aimAtPlayer = aimAtPlayer;
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x005548FC File Offset: 0x00552AFC
		private void UpdateMoonLordIndex()
		{
			if (this._aimAtPlayer)
			{
				return;
			}
			if (this._moonLordIndex >= 0 && Main.npc[this._moonLordIndex].active && Main.npc[this._moonLordIndex].type == 398)
			{
				return;
			}
			int moonLordIndex = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == 398)
				{
					moonLordIndex = i;
					break;
				}
			}
			this._moonLordIndex = moonLordIndex;
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x00554988 File Offset: 0x00552B88
		public override void Apply()
		{
			this.UpdateMoonLordIndex();
			if (this._aimAtPlayer)
			{
				base.UseTargetPosition(Main.SceneMetrics.Center);
			}
			else if (this._moonLordIndex != -1)
			{
				base.UseTargetPosition(Main.npc[this._moonLordIndex].Center);
			}
			base.Apply();
		}

		// Token: 0x04004F8F RID: 20367
		private int _moonLordIndex = -1;

		// Token: 0x04004F90 RID: 20368
		private bool _aimAtPlayer;
	}
}
