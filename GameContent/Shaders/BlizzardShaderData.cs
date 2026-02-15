using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	// Token: 0x02000296 RID: 662
	public class BlizzardShaderData : ScreenShaderData
	{
		// Token: 0x0600252D RID: 9517 RVA: 0x00553354 File Offset: 0x00551554
		public BlizzardShaderData(string passName) : base(passName)
		{
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x00553374 File Offset: 0x00551574
		public override void Update(GameTime gameTime)
		{
			float num = Main.windSpeedCurrent;
			if (num >= 0f && num <= 0.1f)
			{
				num = 0.1f;
			}
			else if (num <= 0f && num >= -0.1f)
			{
				num = -0.1f;
			}
			this.windSpeed = num * 0.05f + this.windSpeed * 0.95f;
			Vector2 vector = new Vector2(-this.windSpeed, -1f) * new Vector2(10f, 2f);
			vector.Normalize();
			vector *= new Vector2(0.8f, 0.6f);
			if (FocusHelper.UpdateVisualEffects)
			{
				this._texturePosition += vector * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			this._texturePosition.X = this._texturePosition.X % 10f;
			this._texturePosition.Y = this._texturePosition.Y % 10f;
			base.UseDirection(vector);
			base.UseTargetPosition(this._texturePosition);
			base.Update(gameTime);
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x00553483 File Offset: 0x00551683
		public override void Apply()
		{
			base.UseTargetPosition(this._texturePosition);
			base.Apply();
		}

		// Token: 0x04004F6D RID: 20333
		private Vector2 _texturePosition = Vector2.Zero;

		// Token: 0x04004F6E RID: 20334
		private float windSpeed = 0.1f;
	}
}
