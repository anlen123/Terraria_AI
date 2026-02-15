using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.Map
{
	// Token: 0x02000178 RID: 376
	public class BossBagMapLayer : IMapLayer
	{
		// Token: 0x06001E18 RID: 7704 RVA: 0x00502594 File Offset: 0x00500794
		public void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			for (int i = 0; i < 400; i++)
			{
				WorldItem worldItem = Main.item[i];
				if (worldItem != null && worldItem.active && ItemID.Sets.BossBag[worldItem.type])
				{
					Main.instance.LoadItem(worldItem.type);
					RenderTarget2D texture;
					if (Main.ItemMapIconRenderer.RequestAndTryGet(worldItem.type, out texture) && context.Draw(texture, worldItem.Center.ToTileCoordinates().ToVector2() + new Vector2(0.5f, 0.5f), Alignment.Center).IsMouseOver)
					{
						text = worldItem.Name;
					}
				}
			}
		}
	}
}
