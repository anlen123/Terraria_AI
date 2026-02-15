using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics
{
	// Token: 0x020001CD RID: 461
	public struct EmpressBladeDrawer
	{
		// Token: 0x06001F66 RID: 8038 RVA: 0x0051B1B8 File Offset: 0x005193B8
		public void Draw(Projectile proj)
		{
			float num = proj.ai[1];
			MiscShaderData miscShaderData = GameShaders.Misc["EmpressBlade"];
			int num2 = 1;
			int num3 = 0;
			int num4 = 0;
			float w = 0.6f;
			miscShaderData.UseShaderSpecificData(new Vector4((float)num2, (float)num3, (float)num4, w));
			miscShaderData.Apply(null);
			EmpressBladeDrawer._vertexStrip.PrepareStrip(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, new int?(proj.oldPos.Length), true);
			EmpressBladeDrawer._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0051B2A0 File Offset: 0x005194A0
		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(this.ColorStart, this.ColorEnd, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, true));
			result.A /= 2;
			return result;
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0051B2FD File Offset: 0x005194FD
		private float StripWidth(float progressOnStrip)
		{
			return 36f;
		}

		// Token: 0x040049F6 RID: 18934
		public const int TotalIllusions = 1;

		// Token: 0x040049F7 RID: 18935
		public const int FramesPerImportantTrail = 60;

		// Token: 0x040049F8 RID: 18936
		private static VertexStrip _vertexStrip = new VertexStrip();

		// Token: 0x040049F9 RID: 18937
		public Color ColorStart;

		// Token: 0x040049FA RID: 18938
		public Color ColorEnd;
	}
}
