using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000394 RID: 916
	public class TwinsBigProgressBar : IBigProgressBar
	{
		// Token: 0x060029E3 RID: 10723 RVA: 0x0057F414 File Offset: 0x0057D614
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
			int num = (npc.type == 126) ? 125 : 126;
			int num2 = npc.lifeMax;
			int num3 = npc.life;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc2 = Main.npc[i];
				if (npc2.active && npc2.type == num)
				{
					num2 += npc2.lifeMax;
					num3 += npc2.life;
					break;
				}
			}
			this._cache.SetLife((float)num3, (float)num2);
			this._headIndex = npc.GetBossHeadTextureIndex();
			return true;
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x0057F4D4 File Offset: 0x0057D6D4
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			Texture2D value = TextureAssets.NpcHeadBoss[this._headIndex].Value;
			Rectangle barIconFrame = value.Frame(1, 1, 0, 0, 0, 0);
			BigProgressBarHelper.DrawFancyBar(spriteBatch, this._cache.LifeCurrent, this._cache.LifeMax, value, barIconFrame);
		}

		// Token: 0x040052B6 RID: 21174
		private BigProgressBarCache _cache;

		// Token: 0x040052B7 RID: 21175
		private int _headIndex;
	}
}
