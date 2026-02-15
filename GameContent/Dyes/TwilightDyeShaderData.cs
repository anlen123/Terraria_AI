using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
	// Token: 0x02000290 RID: 656
	public class TwilightDyeShaderData : ArmorShaderData
	{
		// Token: 0x0600251C RID: 9500 RVA: 0x00552AED File Offset: 0x00550CED
		public TwilightDyeShaderData(Asset<Effect> shader, string passName) : base(shader, passName)
		{
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x00552AF8 File Offset: 0x00550CF8
		public override void Apply(Entity entity, DrawData? drawData)
		{
			if (drawData != null)
			{
				Player player = entity as Player;
				if (player != null && !player.isDisplayDollOrInanimate && !player.isHatRackDoll)
				{
					base.UseTargetPosition(Main.screenPosition + drawData.Value.position);
				}
				else if (entity is Projectile)
				{
					base.UseTargetPosition(Main.screenPosition + drawData.Value.position);
				}
				else
				{
					base.UseTargetPosition(drawData.Value.position);
				}
			}
			base.Apply(entity, drawData);
		}
	}
}
