using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.Map
{
	// Token: 0x0200017F RID: 383
	public class SpawnMapLayer : IMapLayer
	{
		// Token: 0x06001E32 RID: 7730 RVA: 0x005034A4 File Offset: 0x005016A4
		public void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			Player localPlayer = Main.LocalPlayer;
			Vector2 position = new Vector2((float)localPlayer.SpawnX + 0.5f, (float)localPlayer.SpawnY);
			Vector2 position2 = new Vector2((float)Main.spawnTileX + 0.5f, (float)Main.spawnTileY);
			if (!Main.teamBasedSpawnsSeed && context.Draw(TextureAssets.SpawnPoint.Value, position2, Alignment.Bottom).IsMouseOver)
			{
				text = Language.GetTextValue("UI.SpawnPoint");
			}
			if (localPlayer.SpawnX == -1)
			{
				return;
			}
			if (context.Draw(TextureAssets.SpawnBed.Value, position, Alignment.Bottom).IsMouseOver)
			{
				text = Language.GetTextValue("UI.SpawnBed");
			}
		}
	}
}
