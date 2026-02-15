using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x0200039B RID: 923
	public class MartianSaucerBigProgressBar : IBigProgressBar
	{
		// Token: 0x06002A00 RID: 10752 RVA: 0x0057F878 File Offset: 0x0057DA78
		public MartianSaucerBigProgressBar()
		{
			this._referenceDummy = new NPC();
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x0057F8DC File Offset: 0x0057DADC
		public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
		{
			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > Main.maxNPCs)
			{
				return false;
			}
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active || npc.type != 395)
			{
				if (!this.TryFindingAnotherMartianSaucerPiece(ref info))
				{
					return false;
				}
				npc = Main.npc[info.npcIndexToAimAt];
			}
			int num = 0;
			if (Main.expertMode)
			{
				this._referenceDummy.SetDefaults(395, npc.GetMatchingSpawnParams());
				num += this._referenceDummy.lifeMax;
			}
			this._referenceDummy.SetDefaults(394, npc.GetMatchingSpawnParams());
			num += this._referenceDummy.lifeMax * 2;
			this._referenceDummy.SetDefaults(393, npc.GetMatchingSpawnParams());
			num += this._referenceDummy.lifeMax * 2;
			float num2 = 0f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc2 = Main.npc[i];
				if (npc2.active && this.ValidIdsToScanHp.Contains(npc2.type) && (Main.expertMode || npc2.type != 395))
				{
					num2 += (float)npc2.life;
				}
			}
			this._cache.SetLife(num2, (float)num);
			return true;
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x0057FA20 File Offset: 0x0057DC20
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[395];
			Texture2D value = TextureAssets.NpcHeadBoss[num].Value;
			Rectangle barIconFrame = value.Frame(1, 1, 0, 0, 0, 0);
			BigProgressBarHelper.DrawFancyBar(spriteBatch, this._cache.LifeCurrent, this._cache.LifeMax, value, barIconFrame);
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x0057FA74 File Offset: 0x0057DC74
		private bool TryFindingAnotherMartianSaucerPiece(ref BigProgressBarInfo info)
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

		// Token: 0x040052BD RID: 21181
		private BigProgressBarCache _cache;

		// Token: 0x040052BE RID: 21182
		private NPC _referenceDummy;

		// Token: 0x040052BF RID: 21183
		private HashSet<int> ValidIds = new HashSet<int>
		{
			395
		};

		// Token: 0x040052C0 RID: 21184
		private HashSet<int> ValidIdsToScanHp = new HashSet<int>
		{
			395,
			393,
			394
		};
	}
}
