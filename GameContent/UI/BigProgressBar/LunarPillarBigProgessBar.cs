using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000395 RID: 917
	public abstract class LunarPillarBigProgessBar : IBigProgressBar
	{
		// Token: 0x060029E6 RID: 10726 RVA: 0x0057F520 File Offset: 0x0057D720
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
			if (!this.IsPlayerInCombatArea())
			{
				return false;
			}
			if (npc.ai[2] == 1f)
			{
				return false;
			}
			Utils.Clamp<float>((float)npc.life / (float)npc.lifeMax, 0f, 1f);
			float num = (float)((int)MathHelper.Clamp(this.GetCurrentShieldValue(), 0f, this.GetMaxShieldValue())) / this.GetMaxShieldValue();
			this._cache.SetLife((float)npc.life, (float)npc.lifeMax);
			this._cache.SetShield(this.GetCurrentShieldValue(), this.GetMaxShieldValue());
			this._headIndex = bossHeadTextureIndex;
			return true;
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x0057F5FC File Offset: 0x0057D7FC
		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			Texture2D value = TextureAssets.NpcHeadBoss[this._headIndex].Value;
			Rectangle barIconFrame = value.Frame(1, 1, 0, 0, 0, 0);
			BigProgressBarHelper.DrawFancyBar(spriteBatch, this._cache.LifeCurrent, this._cache.LifeMax, value, barIconFrame, this._cache.ShieldCurrent, this._cache.ShieldMax);
		}

		// Token: 0x060029E8 RID: 10728
		internal abstract float GetCurrentShieldValue();

		// Token: 0x060029E9 RID: 10729
		internal abstract float GetMaxShieldValue();

		// Token: 0x060029EA RID: 10730
		internal abstract bool IsPlayerInCombatArea();

		// Token: 0x040052B8 RID: 21176
		private BigProgressBarCache _cache;

		// Token: 0x040052B9 RID: 21177
		private int _headIndex;
	}
}
