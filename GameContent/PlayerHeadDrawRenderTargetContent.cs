using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent
{
	// Token: 0x02000253 RID: 595
	public class PlayerHeadDrawRenderTargetContent : AnOutlinedDrawRenderTargetContent
	{
		// Token: 0x0600232B RID: 9003 RVA: 0x0053C2A6 File Offset: 0x0053A4A6
		public void UsePlayer(Player player)
		{
			this._player = player;
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0053C2B0 File Offset: 0x0053A4B0
		internal override void DrawTheContent(SpriteBatch spriteBatch)
		{
			if (this._player == null)
			{
				return;
			}
			if (this._player.ShouldNotDraw)
			{
				return;
			}
			this._drawData.Clear();
			this._dust.Clear();
			this._gore.Clear();
			PlayerDrawHeadSet playerDrawHeadSet = default(PlayerDrawHeadSet);
			playerDrawHeadSet.BoringSetup(this._player, this._drawData, this._dust, this._gore, (float)(this.width / 2), (float)(this.height / 2), 1f, 1f);
			PlayerDrawHeadLayers.DrawPlayer_00_BackHelmet(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_01_FaceSkin(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_02_DrawArmorWithFullHair(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_03_HelmetHair(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_04_HatsWithFullHair(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_05_TallHats(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_06_NormalHats(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_07_JustHair(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_08_FaceAcc(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_RenderAllLayers(ref playerDrawHeadSet);
		}

		// Token: 0x04004D34 RID: 19764
		private Player _player;

		// Token: 0x04004D35 RID: 19765
		private readonly List<DrawData> _drawData = new List<DrawData>();

		// Token: 0x04004D36 RID: 19766
		private readonly List<int> _dust = new List<int>();

		// Token: 0x04004D37 RID: 19767
		private readonly List<int> _gore = new List<int>();
	}
}
