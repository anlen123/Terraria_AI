using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
	// Token: 0x02000291 RID: 657
	public class LegacyHairShaderData : HairShaderData
	{
		// Token: 0x0600251E RID: 9502 RVA: 0x00552B89 File Offset: 0x00550D89
		public LegacyHairShaderData() : base(null, null)
		{
			this._shaderDisabled = true;
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x00552B9C File Offset: 0x00550D9C
		public override Color GetColor(Player player, Color lightColor)
		{
			bool flag = true;
			Color result = this._colorProcessor(player, player.hairColor, ref flag);
			if (flag)
			{
				return new Color(result.ToVector4() * lightColor.ToVector4());
			}
			return result;
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x00552BDD File Offset: 0x00550DDD
		public LegacyHairShaderData UseLegacyMethod(LegacyHairShaderData.ColorProcessingMethod colorProcessor)
		{
			this._colorProcessor = colorProcessor;
			return this;
		}

		// Token: 0x04004F67 RID: 20327
		private LegacyHairShaderData.ColorProcessingMethod _colorProcessor;

		// Token: 0x0200080E RID: 2062
		// (Invoke) Token: 0x060042DE RID: 17118
		public delegate Color ColorProcessingMethod(Player player, Color color, ref bool lighting);
	}
}
