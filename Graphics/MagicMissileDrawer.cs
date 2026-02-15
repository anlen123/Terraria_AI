using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics
{
	// Token: 0x020001CE RID: 462
	public struct MagicMissileDrawer
	{
		// Token: 0x06001F6A RID: 8042 RVA: 0x0051B310 File Offset: 0x00519510
		public void Draw(Projectile proj)
		{
			MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
			miscShaderData.UseSaturation(-2.8f);
			miscShaderData.UseOpacity(2f);
			miscShaderData.Apply(null);
			MagicMissileDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, false, true);
			MagicMissileDrawer._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0051B3DC File Offset: 0x005195DC
		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(Color.White, Color.Violet, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, false));
			result.A /= 2;
			return result;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0051B437 File Offset: 0x00519637
		private float StripWidth(float progressOnStrip)
		{
			return MathHelper.Lerp(26f, 32f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, true);
		}

		// Token: 0x040049FB RID: 18939
		private static VertexStrip _vertexStrip = new VertexStrip();
	}
}
