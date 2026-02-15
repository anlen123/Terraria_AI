using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x0200039A RID: 922
	public class PirateShipBigProgressBar : IBigProgressBar
	{
		// Token: 0x060029FC RID: 10748 RVA: 0x0057F6BC File Offset: 0x0057D8BC
		public PirateShipBigProgressBar()
		{
			this._referenceDummy = new NPC();
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x0057F6E8 File Offset: 0x0057D8E8
		public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
		{
			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > Main.maxNPCs)
			{
				return false;
			}
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active || npc.type != 491)
			{
				if (!this.TryFindingAnotherPirateShipPiece(ref info))
				{
					return false;
				}
				npc = Main.npc[info.npcIndexToAimAt];
			}
			int num = 0;
			this._referenceDummy.SetDefaults(492, npc.GetMatchingSpawnParams());
			num += this._referenceDummy.lifeMax * 4;
			float num2 = 0f;
			for (int i = 0; i < 4; i++)
			{
				int num3 = (int)npc.ai[i];
				if (Main.npc.IndexInRange(num3))
				{
					NPC npc2 = Main.npc[num3];
					if (npc2.active && npc2.type == 492)
					{
						num2 += (float)npc2.life;
					}
				}
			}
			this._cache.SetLife(num2, (float)num);
			return true;
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x0057F7D8 File Offset: 0x0057D9D8
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[491];
			Texture2D value = TextureAssets.NpcHeadBoss[num].Value;
			Rectangle barIconFrame = value.Frame(1, 1, 0, 0, 0, 0);
			BigProgressBarHelper.DrawFancyBar(spriteBatch, this._cache.LifeCurrent, this._cache.LifeMax, value, barIconFrame);
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x0057F82C File Offset: 0x0057DA2C
		private bool TryFindingAnotherPirateShipPiece(ref BigProgressBarInfo info)
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && this.ValidIds.Contains(npc.type))
				{
					info.npcIndexToAimAt = i;
					return true;
				}
			}
			return false;
		}

		// Token: 0x040052BA RID: 21178
		private BigProgressBarCache _cache;

		// Token: 0x040052BB RID: 21179
		private NPC _referenceDummy;

		// Token: 0x040052BC RID: 21180
		private HashSet<int> ValidIds = new HashSet<int>
		{
			491
		};
	}
}
