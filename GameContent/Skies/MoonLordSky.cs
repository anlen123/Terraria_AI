using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x0200044E RID: 1102
	public class MoonLordSky : CustomSky
	{
		// Token: 0x060031FB RID: 12795 RVA: 0x005E4DDD File Offset: 0x005E2FDD
		public MoonLordSky(bool forPlayer)
		{
			this._forPlayer = forPlayer;
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x00009E06 File Offset: 0x00008006
		public override void OnLoad()
		{
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x005E4DF8 File Offset: 0x005E2FF8
		public override void Update(GameTime gameTime)
		{
			if (this._forPlayer)
			{
				if (this._isActive)
				{
					this._fadeOpacity = Math.Min(1f, 0.01f + this._fadeOpacity);
					return;
				}
				this._fadeOpacity = Math.Max(0f, this._fadeOpacity - 0.01f);
			}
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x005E4E50 File Offset: 0x005E3050
		private float GetIntensity()
		{
			if (this._forPlayer)
			{
				return this._fadeOpacity;
			}
			float? moonLordSkyIntensity = Main.SceneMetrics.MoonLordSkyIntensity;
			if (moonLordSkyIntensity != null)
			{
				return moonLordSkyIntensity.Value;
			}
			return 0f;
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x005E4E90 File Offset: 0x005E3090
		public override Color OnTileColor(Color inColor)
		{
			float intensity = this.GetIntensity();
			return new Color(Vector4.Lerp(new Vector4(0.5f, 0.8f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x005E4ED8 File Offset: 0x005E30D8
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 0f && minDepth < 0f)
			{
				float intensity = this.GetIntensity();
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
			}
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x005E4F28 File Offset: 0x005E3128
		public override float GetCloudAlpha()
		{
			return 1f - this._fadeOpacity;
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x005E4F36 File Offset: 0x005E3136
		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
			if (this._forPlayer)
			{
				this._fadeOpacity = 0.002f;
				return;
			}
			this._fadeOpacity = 1f;
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x005E4F5E File Offset: 0x005E315E
		public override void Deactivate(params object[] args)
		{
			this._isActive = false;
			if (!this._forPlayer)
			{
				this._fadeOpacity = 0f;
			}
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x005E4F7A File Offset: 0x005E317A
		public override void Reset()
		{
			this._isActive = false;
			this._fadeOpacity = 0f;
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x005E4F8E File Offset: 0x005E318E
		public override bool IsActive()
		{
			return this._isActive || this._fadeOpacity > 0.001f;
		}

		// Token: 0x040057B7 RID: 22455
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x040057B8 RID: 22456
		private bool _isActive;

		// Token: 0x040057B9 RID: 22457
		private bool _forPlayer;

		// Token: 0x040057BA RID: 22458
		private float _fadeOpacity;
	}
}
