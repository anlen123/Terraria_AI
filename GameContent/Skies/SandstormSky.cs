using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Events;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x0200044D RID: 1101
	public class SandstormSky : CustomSky
	{
		// Token: 0x060031F3 RID: 12787 RVA: 0x00009E06 File Offset: 0x00008006
		public override void OnLoad()
		{
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x005E4C64 File Offset: 0x005E2E64
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

		// Token: 0x060031F5 RID: 12789 RVA: 0x005E4CF4 File Offset: 0x005E2EF4
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (minDepth < 1f || maxDepth == 3.4028235E+38f)
			{
				float scale = Math.Min(1f, Sandstorm.Severity * 1.5f);
				Color color = new Color(new Vector4(0.85f, 0.66f, 0.33f, 1f) * 0.8f * Main.ColorOfTheSkies.ToVector4()) * this._opacity * scale;
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);
			}
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x005E4D95 File Offset: 0x005E2F95
		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
			this._isLeaving = false;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x005E4DA5 File Offset: 0x005E2FA5
		public override void Deactivate(params object[] args)
		{
			this._isLeaving = true;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x005E4DAE File Offset: 0x005E2FAE
		public override void Reset()
		{
			this._opacity = 0f;
			this._isActive = false;
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x005E4DC2 File Offset: 0x005E2FC2
		public override bool IsActive()
		{
			return this._isActive;
		}

		// Token: 0x040057B3 RID: 22451
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x040057B4 RID: 22452
		private bool _isActive;

		// Token: 0x040057B5 RID: 22453
		private bool _isLeaving;

		// Token: 0x040057B6 RID: 22454
		private float _opacity;
	}
}
