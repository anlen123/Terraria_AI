using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.DataStructures
{
	// Token: 0x02000599 RID: 1433
	public class PlayerDrawHelper
	{
		// Token: 0x06003864 RID: 14436 RVA: 0x006319A0 File Offset: 0x0062FBA0
		public static int PackShader(int localShaderIndex, PlayerDrawHelper.ShaderConfiguration shaderType)
		{
			return (int)(localShaderIndex + shaderType * (PlayerDrawHelper.ShaderConfiguration)1000);
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x006319AB File Offset: 0x0062FBAB
		public static void UnpackShader(int packedShaderIndex, out int localShaderIndex, out PlayerDrawHelper.ShaderConfiguration shaderType)
		{
			shaderType = (PlayerDrawHelper.ShaderConfiguration)(packedShaderIndex / 1000);
			localShaderIndex = packedShaderIndex % 1000;
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x006319C0 File Offset: 0x0062FBC0
		public static void SetShaderForData(Player player, int cHead, ref DrawData cdd)
		{
			int num;
			PlayerDrawHelper.ShaderConfiguration shaderConfiguration;
			PlayerDrawHelper.UnpackShader(cdd.shader, out num, out shaderConfiguration);
			switch (shaderConfiguration)
			{
			case PlayerDrawHelper.ShaderConfiguration.ArmorShader:
				GameShaders.Hair.Apply(0, player, new DrawData?(cdd));
				GameShaders.Armor.Apply(num, player, new DrawData?(cdd));
				return;
			case PlayerDrawHelper.ShaderConfiguration.HairShader:
				if (player.head == 0)
				{
					GameShaders.Hair.Apply(0, player, new DrawData?(cdd));
					GameShaders.Armor.Apply(cHead, player, new DrawData?(cdd));
					return;
				}
				GameShaders.Armor.Apply(0, player, new DrawData?(cdd));
				GameShaders.Hair.Apply((short)num, player, new DrawData?(cdd));
				return;
			case PlayerDrawHelper.ShaderConfiguration.TileShader:
				Main.tileShader.CurrentTechnique.Passes[num].Apply();
				return;
			case PlayerDrawHelper.ShaderConfiguration.TilePaintID:
			{
				int index = Main.ConvertPaintIdToTileShaderIndex(num, false, false);
				Main.tileShader.CurrentTechnique.Passes[index].Apply();
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x04005C71 RID: 23665
		public static Color DISPLAY_DOLL_DEFAULT_SKIN_COLOR = new Color(163, 121, 92);

		// Token: 0x020009C0 RID: 2496
		public enum ShaderConfiguration
		{
			// Token: 0x040076A2 RID: 30370
			ArmorShader,
			// Token: 0x040076A3 RID: 30371
			HairShader,
			// Token: 0x040076A4 RID: 30372
			TileShader,
			// Token: 0x040076A5 RID: 30373
			TilePaintID
		}
	}
}
