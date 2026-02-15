using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x0200044B RID: 1099
	public class BlizzardSky : CustomSky
	{
		// Token: 0x060031E1 RID: 12769 RVA: 0x00009E06 File Offset: 0x00008006
		public override void OnLoad()
		{
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x005E437C File Offset: 0x005E257C
		public override void Update(GameTime gameTime)
		{
			if (FocusHelper.PauseSkies)
			{
				return;
			}
			if (this._isLeaving)
			{
				this._opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this._opacity < 0f)
				{
					this._isActive = false;
					this._opacity = 0f;
					return;
				}
			}
			else
			{
				this._opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this._opacity > 1f)
				{
					this._opacity = 1f;
				}
			}
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x005E440C File Offset: 0x005E260C
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (minDepth < 1f || maxDepth == 3.4028235E+38f)
			{
				float scale = Math.Min(1f, Main.cloudAlpha * 2f);
				Color color = new Color(new Vector4(1f) * Main.ColorOfTheSkies.ToVector4()) * this._opacity * 0.7f * scale;
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);
			}
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x005E449B File Offset: 0x005E269B
		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
			this._isLeaving = false;
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x005E44AB File Offset: 0x005E26AB
		public override void Deactivate(params object[] args)
		{
			this._isLeaving = true;
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x005E44B4 File Offset: 0x005E26B4
		public override void Reset()
		{
			this._opacity = 0f;
			this._isActive = false;
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x005E44C8 File Offset: 0x005E26C8
		public override bool IsActive()
		{
			return this._isActive;
		}

		// Token: 0x040057A8 RID: 22440
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x040057A9 RID: 22441
		private bool _isActive;

		// Token: 0x040057AA RID: 22442
		private bool _isLeaving;

		// Token: 0x040057AB RID: 22443
		private float _opacity;
	}
}
