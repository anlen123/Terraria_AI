using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Map
{
	// Token: 0x0200017B RID: 379
	public class MapIconOverlay
	{
		// Token: 0x06001E22 RID: 7714 RVA: 0x005028FB File Offset: 0x00500AFB
		public MapIconOverlay AddLayer(IMapLayer layer)
		{
			this._layers.Add(layer);
			return this;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0050290C File Offset: 0x00500B0C
		public void Draw(Vector2 mapPosition, Vector2 mapOffset, Rectangle? clippingRect, float mapScale, float drawScale, int alpha, ref string text)
		{
			MapOverlayDrawContext mapOverlayDrawContext = new MapOverlayDrawContext(mapPosition, mapOffset, clippingRect, mapScale, drawScale, (float)alpha / 255f);
			foreach (IMapLayer mapLayer in this._layers)
			{
				mapLayer.Draw(ref mapOverlayDrawContext, ref text);
			}
		}

		// Token: 0x0400167C RID: 5756
		private readonly List<IMapLayer> _layers = new List<IMapLayer>();
	}
}
