using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.Map
{
	// Token: 0x0200017E RID: 382
	public class TeamBasedSpawnMapLayer : IMapLayer
	{
		// Token: 0x06001E30 RID: 7728 RVA: 0x00503434 File Offset: 0x00501634
		public void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			if (!Main.teamBasedSpawnsSeed)
			{
				return;
			}
			int team = Main.LocalPlayer.team;
			Point zero = Point.Zero;
			if (!ExtraSpawnPointManager.TryGetExtraSpawnPointForTeam(team, out zero))
			{
				return;
			}
			if (context.Draw(TextureAssets.Extra[282].Value, zero.ToVector2(), new SpriteFrame(6, 1, (byte)team, 0), Alignment.Bottom).IsMouseOver)
			{
				text = Language.GetTextValue("UI.TeamSpawnPoint");
			}
		}
	}
}
