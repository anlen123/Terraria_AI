using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000390 RID: 912
	public class GolemHeadProgressBar : IBigProgressBar
	{
		// Token: 0x060029D5 RID: 10709 RVA: 0x0057EF99 File Offset: 0x0057D199
		public GolemHeadProgressBar()
		{
			this._referenceDummy = new NPC();
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x0057EFD0 File Offset: 0x0057D1D0
		public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
		{
			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > Main.maxNPCs)
			{
				return false;
			}
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active && !this.TryFindingAnotherGolemPiece(ref info))
			{
				return false;
			}
			int num = 0;
			this._referenceDummy.SetDefaults(245, npc.GetMatchingSpawnParams());
			num += this._referenceDummy.lifeMax;
			this._referenceDummy.SetDefaults(246, npc.GetMatchingSpawnParams());
			num += this._referenceDummy.lifeMax;
			float num2 = 0f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc2 = Main.npc[i];
				if (npc2.active && this.ValidIds.Contains(npc2.type))
				{
					num2 += (float)npc2.life;
				}
			}
			this._cache.SetLife(num2, (float)num);
			return true;
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x0057F0B8 File Offset: 0x0057D2B8
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[246];
			Texture2D value = TextureAssets.NpcHeadBoss[num].Value;
			Rectangle barIconFrame = value.Frame(1, 1, 0, 0, 0, 0);
			BigProgressBarHelper.DrawFancyBar(spriteBatch, this._cache.LifeCurrent, this._cache.LifeMax, value, barIconFrame);
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x0057F10C File Offset: 0x0057D30C
		private bool TryFindingAnotherGolemPiece(ref BigProgressBarInfo info)
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

		// Token: 0x040052AC RID: 21164
		private BigProgressBarCache _cache;

		// Token: 0x040052AD RID: 21165
		private NPC _referenceDummy;

		// Token: 0x040052AE RID: 21166
		private HashSet<int> ValidIds = new HashSet<int>
		{
			246,
			245
		};
	}
}
