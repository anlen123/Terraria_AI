using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
	// Token: 0x0200028F RID: 655
	public class TwilightHairDyeShaderData : HairShaderData
	{
		// Token: 0x0600251A RID: 9498 RVA: 0x00552AB3 File Offset: 0x00550CB3
		public TwilightHairDyeShaderData(Asset<Effect> shader, string passName) : base(shader, passName)
		{
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x00552ABD File Offset: 0x00550CBD
		public override void Apply(Player player, DrawData? drawData = null)
		{
			if (drawData != null)
			{
				base.UseTargetPosition(Main.screenPosition + drawData.Value.position);
			}
			base.Apply(player, drawData);
		}
	}
}
