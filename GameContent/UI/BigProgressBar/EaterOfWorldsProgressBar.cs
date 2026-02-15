using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x0200038E RID: 910
	public class EaterOfWorldsProgressBar : IBigProgressBar
	{
		// Token: 0x060029CE RID: 10702 RVA: 0x0057ECD6 File Offset: 0x0057CED6
		public EaterOfWorldsProgressBar()
		{
			this._segmentForReference = new NPC();
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x0057ECEC File Offset: 0x0057CEEC
		public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
		{
			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > Main.maxNPCs)
			{
				return false;
			}
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active && !this.TryFindingAnotherEOWPiece(ref info))
			{
				return false;
			}
			int num = 2;
			int num2 = NPC.GetEaterOfWorldsSegmentsCount() + num;
			this._segmentForReference.SetDefaults(14, npc.GetMatchingSpawnParams());
			int num3 = 0;
			int num4 = this._segmentForReference.lifeMax * num2;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc2 = Main.npc[i];
				if (npc2.active && npc2.type >= 13 && npc2.type <= 15)
				{
					num3 += npc2.life;
				}
			}
			int num5 = num3;
			int num6 = num4;
			this._cache.SetLife((float)num5, (float)num6);
			return true;
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x0057EDC0 File Offset: 0x0057CFC0
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[13];
			Texture2D value = TextureAssets.NpcHeadBoss[num].Value;
			Rectangle barIconFrame = value.Frame(1, 1, 0, 0, 0, 0);
			BigProgressBarHelper.DrawFancyBar(spriteBatch, this._cache.LifeCurrent, this._cache.LifeMax, value, barIconFrame);
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x0057EE10 File Offset: 0x0057D010
		private bool TryFindingAnotherEOWPiece(ref BigProgressBarInfo info)
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && npc.type >= 13 && npc.type <= 15)
				{
					info.npcIndexToAimAt = i;
					return true;
				}
			}
			return false;
		}

		// Token: 0x040052A8 RID: 21160
		private BigProgressBarCache _cache;

		// Token: 0x040052A9 RID: 21161
		private NPC _segmentForReference;
	}
}
