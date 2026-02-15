using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x0200038F RID: 911
	public class BrainOfCthuluBigProgressBar : IBigProgressBar
	{
		// Token: 0x060029D2 RID: 10706 RVA: 0x0057EE5B File Offset: 0x0057D05B
		public BrainOfCthuluBigProgressBar()
		{
			this._creeperForReference = new NPC();
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x0057EE70 File Offset: 0x0057D070
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
			int brainOfCthuluCreepersCount = NPC.GetBrainOfCthuluCreepersCount();
			this._creeperForReference.SetDefaults(267, npc.GetMatchingSpawnParams());
			int num = this._creeperForReference.lifeMax * brainOfCthuluCreepersCount;
			float num2 = 0f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc2 = Main.npc[i];
				if (npc2.active && npc2.type == this._creeperForReference.type)
				{
					num2 += (float)npc2.life;
				}
			}
			float current = (float)npc.life + num2;
			int num3 = npc.lifeMax + num;
			this._cache.SetLife(current, (float)num3);
			return true;
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x0057EF48 File Offset: 0x0057D148
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[266];
			Texture2D value = TextureAssets.NpcHeadBoss[num].Value;
			Rectangle barIconFrame = value.Frame(1, 1, 0, 0, 0, 0);
			BigProgressBarHelper.DrawFancyBar(spriteBatch, this._cache.LifeCurrent, this._cache.LifeMax, value, barIconFrame);
		}

		// Token: 0x040052AA RID: 21162
		private BigProgressBarCache _cache;

		// Token: 0x040052AB RID: 21163
		private NPC _creeperForReference;
	}
}
