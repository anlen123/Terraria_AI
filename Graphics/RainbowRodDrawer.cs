using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics
{
	// Token: 0x020001D2 RID: 466
	public struct RainbowRodDrawer
	{
		// Token: 0x06001F7B RID: 8059 RVA: 0x0051BA64 File Offset: 0x00519C64
		public void Draw(Projectile proj)
		{
			MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
			miscShaderData.UseSaturation(-2.8f);
			miscShaderData.UseOpacity(4f);
			miscShaderData.Apply(null);
			RainbowRodDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, false, true);
			RainbowRodDrawer._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0051BB30 File Offset: 0x00519D30
		private Color StripColors(float progressOnStrip)
		{
			Color value = Main.hslToRgb((progressOnStrip * 1.6f - Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.5f, byte.MaxValue);
			Color result = Color.Lerp(Color.White, value, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, false));
			result.A = 0;
			return result;
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x0051BBA8 File Offset: 0x00519DA8
		private float StripWidth(float progressOnStrip)
		{
			float num = 1f;
			float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 32f, num);
		}

		// Token: 0x04004A06 RID: 18950
		private static VertexStrip _vertexStrip = new VertexStrip();
	}
}
