using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	// Token: 0x02000295 RID: 661
	public class SandstormShaderData : ScreenShaderData
	{
		// Token: 0x0600252A RID: 9514 RVA: 0x00553277 File Offset: 0x00551477
		public SandstormShaderData(string passName) : base(passName)
		{
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x0055328C File Offset: 0x0055148C
		public override void Update(GameTime gameTime)
		{
			Vector2 vector = new Vector2(-Main.windSpeedCurrent, -1f) * new Vector2(20f, 0.1f);
			vector.Normalize();
			vector *= new Vector2(2f, 0.2f);
			if (FocusHelper.UpdateVisualEffects)
			{
				this._texturePosition += vector * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			this._texturePosition.X = this._texturePosition.X % 10f;
			this._texturePosition.Y = this._texturePosition.Y % 10f;
			base.UseDirection(vector);
			base.Update(gameTime);
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x0055333F File Offset: 0x0055153F
		public override void Apply()
		{
			base.UseTargetPosition(this._texturePosition);
			base.Apply();
		}

		// Token: 0x04004F6C RID: 20332
		private Vector2 _texturePosition = Vector2.Zero;
	}
}
