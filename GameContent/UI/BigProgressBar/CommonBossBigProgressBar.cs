using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x0200038C RID: 908
	public class CommonBossBigProgressBar : IBigProgressBar
	{
		// Token: 0x060029C8 RID: 10696 RVA: 0x0057EB60 File Offset: 0x0057CD60
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
			this._cache.SetLife((float)npc.life, (float)npc.lifeMax);
			this._headIndex = bossHeadTextureIndex;
			return true;
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x0057EBCC File Offset: 0x0057CDCC
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			Texture2D value = TextureAssets.NpcHeadBoss[this._headIndex].Value;
			Rectangle barIconFrame = value.Frame(1, 1, 0, 0, 0, 0);
			BigProgressBarHelper.DrawFancyBar(spriteBatch, this._cache.LifeCurrent, this._cache.LifeMax, value, barIconFrame);
		}

		// Token: 0x040052A4 RID: 21156
		private BigProgressBarCache _cache;

		// Token: 0x040052A5 RID: 21157
		private int _headIndex;
	}
}
