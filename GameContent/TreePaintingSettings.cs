using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
	// Token: 0x02000258 RID: 600
	public class TreePaintingSettings
	{
		// Token: 0x06002338 RID: 9016 RVA: 0x0053C6A4 File Offset: 0x0053A8A4
		public void ApplyShader(int paintColor, Effect shader)
		{
			shader.Parameters["leafHueTestOffset"].SetValue(this.HueTestOffset);
			shader.Parameters["leafMinHue"].SetValue(this.SpecialGroupMinimalHueValue);
			shader.Parameters["leafMaxHue"].SetValue(this.SpecialGroupMaximumHueValue);
			shader.Parameters["leafMinSat"].SetValue(this.SpecialGroupMinimumSaturationValue);
			shader.Parameters["leafMaxSat"].SetValue(this.SpecialGroupMaximumSaturationValue);
			shader.Parameters["invertSpecialGroupResult"].SetValue(this.InvertSpecialGroupResult);
			int index = Main.ConvertPaintIdToTileShaderIndex(paintColor, this.UseSpecialGroups, this.UseWallShaderHacks);
			shader.CurrentTechnique.Passes[index].Apply();
		}

		// Token: 0x04004D3A RID: 19770
		public float SpecialGroupMinimalHueValue;

		// Token: 0x04004D3B RID: 19771
		public float SpecialGroupMaximumHueValue;

		// Token: 0x04004D3C RID: 19772
		public float SpecialGroupMinimumSaturationValue;

		// Token: 0x04004D3D RID: 19773
		public float SpecialGroupMaximumSaturationValue;

		// Token: 0x04004D3E RID: 19774
		public float HueTestOffset;

		// Token: 0x04004D3F RID: 19775
		public bool UseSpecialGroups;

		// Token: 0x04004D40 RID: 19776
		public bool UseWallShaderHacks;

		// Token: 0x04004D41 RID: 19777
		public bool InvertSpecialGroupResult;
	}
}
