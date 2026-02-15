using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000435 RID: 1077
	public class EmptyHorizonRenderer : IHorizonRenderer
	{
		// Token: 0x06003093 RID: 12435 RVA: 0x005BA728 File Offset: 0x005B8928
		public void DrawHorizon()
		{
			if (!Main.ShouldDrawSurfaceBackground())
			{
				return;
			}
			foreach (BackgroundGradientDrawer backgroundGradientDrawer in SunGradients.BackgroundDrawers)
			{
				backgroundGradientDrawer.Draw();
			}
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x00009E06 File Offset: 0x00008006
		public void DrawLensFlare()
		{
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x00009E06 File Offset: 0x00008006
		public void ModifyHorizonLight(ref Color color)
		{
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x00009E06 File Offset: 0x00008006
		public void DrawSun(Vector2 sunPosition)
		{
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x00009E06 File Offset: 0x00008006
		public void DrawSurfaceLayer(int layerIndex)
		{
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x00009E06 File Offset: 0x00008006
		public void CloudsStart()
		{
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x005BA780 File Offset: 0x005B8980
		public void DrawCloud(float globalCloudAlpha, Cloud theCloud, int cloudPass, float cY)
		{
			Asset<Texture2D> asset = TextureAssets.Cloud[theCloud.type];
			Color value = theCloud.cloudColor(Main.ColorOfTheSkies);
			if (cloudPass == 1)
			{
				float num = theCloud.scale * 0.8f;
				float num2 = (theCloud.scale + 1f) / 2f * 0.9f;
				value.R = (byte)((float)value.R * num);
				value.G = (byte)((float)value.G * num2);
			}
			if (Main.atmo < 1f)
			{
				value *= Main.atmo;
			}
			Main.spriteBatch.Draw(asset.Value, new Vector2(theCloud.position.X, cY) + asset.Size() / 2f, null, value * globalCloudAlpha, theCloud.rotation, asset.Size() / 2f, theCloud.scale, theCloud.spriteDir, 0f);
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x00009E06 File Offset: 0x00008006
		public void CloudsEnd()
		{
		}
	}
}
