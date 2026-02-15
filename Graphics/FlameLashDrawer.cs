using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics
{
	// Token: 0x020001D0 RID: 464
	public struct FlameLashDrawer
	{
		// Token: 0x06001F72 RID: 8050 RVA: 0x0051B59C File Offset: 0x0051979C
		public void Draw(Projectile proj)
		{
			this.transitToDark = Utils.GetLerpValue(0f, 6f, proj.localAI[0], true);
			MiscShaderData miscShaderData = GameShaders.Misc["FlameLash"];
			miscShaderData.UseSaturation(-2f);
			miscShaderData.UseOpacity(MathHelper.Lerp(4f, 8f, this.transitToDark));
			miscShaderData.Apply(null);
			FlameLashDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, false, true);
			FlameLashDrawer._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x0051B694 File Offset: 0x00519894
		private Color StripColors(float progressOnStrip)
		{
			float lerpValue = Utils.GetLerpValue(0f - 0.1f * this.transitToDark, 0.7f - 0.2f * this.transitToDark, progressOnStrip, true);
			Color result = Color.Lerp(Color.Lerp(Color.White, Color.Orange, this.transitToDark * 0.5f), Color.Red, lerpValue) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, false));
			result.A /= 8;
			return result;
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0051B724 File Offset: 0x00519924
		private float StripWidth(float progressOnStrip)
		{
			float num = Utils.GetLerpValue(0f, 0.06f + this.transitToDark * 0.01f, progressOnStrip, true);
			num = 1f - (1f - num) * (1f - num);
			return MathHelper.Lerp(24f + this.transitToDark * 16f, 8f, Utils.GetLerpValue(0f, 1f, progressOnStrip, true)) * num;
		}

		// Token: 0x040049FD RID: 18941
		private static VertexStrip _vertexStrip = new VertexStrip();

		// Token: 0x040049FE RID: 18942
		private float transitToDark;
	}
}
