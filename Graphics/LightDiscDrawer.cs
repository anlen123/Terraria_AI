using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics
{
	// Token: 0x020001CF RID: 463
	public struct LightDiscDrawer
	{
		// Token: 0x06001F6E RID: 8046 RVA: 0x0051B478 File Offset: 0x00519678
		public void Draw(Projectile proj)
		{
			MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
			miscShaderData.UseSaturation(-2.8f);
			miscShaderData.UseOpacity(2f);
			miscShaderData.Apply(null);
			LightDiscDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, false, true);
			LightDiscDrawer._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0051B544 File Offset: 0x00519744
		private Color StripColors(float progressOnStrip)
		{
			float num = 1f - progressOnStrip;
			Color result = new Color(48, 63, 150) * (num * num * num * num) * 0.5f;
			result.A = 0;
			return result;
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0051B587 File Offset: 0x00519787
		private float StripWidth(float progressOnStrip)
		{
			return 16f;
		}

		// Token: 0x040049FC RID: 18940
		private static VertexStrip _vertexStrip = new VertexStrip();
	}
}
