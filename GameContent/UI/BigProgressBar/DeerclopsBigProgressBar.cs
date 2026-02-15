using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x0200038D RID: 909
	public class DeerclopsBigProgressBar : IBigProgressBar
	{
		// Token: 0x060029CB RID: 10699 RVA: 0x0057EC18 File Offset: 0x0057CE18
		public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
		{
			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > Main.maxNPCs)
			{
				return false;
			}
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active)
			{
				return false;
			}
			int bossHeadTextureIndex = npc.GetBossHeadTextureIndex();
			if (bossHeadTextureIndex == -1)
			{
				return false;
			}
			if (!NPC.IsDeerclopsHostile())
			{
				return false;
			}
			this._cache.SetLife((float)npc.life, (float)npc.lifeMax);
			this._headIndex = bossHeadTextureIndex;
			return true;
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x0057EC8C File Offset: 0x0057CE8C
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			Texture2D value = TextureAssets.NpcHeadBoss[this._headIndex].Value;
			Rectangle barIconFrame = value.Frame(1, 1, 0, 0, 0, 0);
			BigProgressBarHelper.DrawFancyBar(spriteBatch, this._cache.LifeCurrent, this._cache.LifeMax, value, barIconFrame);
		}

		// Token: 0x040052A6 RID: 21158
		private BigProgressBarCache _cache;

		// Token: 0x040052A7 RID: 21159
		private int _headIndex;
	}
}
