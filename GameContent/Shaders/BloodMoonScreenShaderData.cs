using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	// Token: 0x02000299 RID: 665
	public class BloodMoonScreenShaderData : ScreenShaderData
	{
		// Token: 0x0600253E RID: 9534 RVA: 0x00552F74 File Offset: 0x00551174
		public BloodMoonScreenShaderData(string passName) : base(passName)
		{
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x00554834 File Offset: 0x00552A34
		public override void Update(GameTime gameTime)
		{
			float num = 1f - Utils.SmoothStep((float)Main.worldSurface + 50f, (float)Main.rockLayer + 100f, (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f);
			if (Main.remixWorld)
			{
				num = Utils.SmoothStep((float)(Main.rockLayer + Main.worldSurface) / 2f, (float)Main.rockLayer, (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f);
			}
			if (Main.shimmerAlpha > 0f)
			{
				num *= 1f - Main.shimmerAlpha;
			}
			base.UseOpacity(num * 0.75f);
		}
	}
}
