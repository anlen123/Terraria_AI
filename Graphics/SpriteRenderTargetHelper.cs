using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Graphics
{
	// Token: 0x020001CB RID: 459
	public struct SpriteRenderTargetHelper
	{
		// Token: 0x06001F5C RID: 8028 RVA: 0x0051AAC4 File Offset: 0x00518CC4
		public static void GetDrawBoundary(List<DrawData> playerDrawData, out Vector2 lowest, out Vector2 highest)
		{
			lowest = Vector2.Zero;
			highest = Vector2.Zero;
			for (int i = 0; i <= playerDrawData.Count; i++)
			{
				if (i != playerDrawData.Count)
				{
					DrawData drawData = playerDrawData[i];
					if (i == 0)
					{
						lowest = drawData.position;
						highest = drawData.position;
					}
					SpriteRenderTargetHelper.GetHighsAndLowsOf(ref lowest, ref highest, ref drawData);
				}
			}
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0051AB30 File Offset: 0x00518D30
		public static void GetHighsAndLowsOf(ref Vector2 lowest, ref Vector2 highest, ref DrawData cdd)
		{
			Vector2 origin = cdd.origin;
			Rectangle rectangle = cdd.destinationRectangle;
			if (cdd.sourceRect != null)
			{
				rectangle = cdd.sourceRect.Value;
			}
			if (cdd.sourceRect == null)
			{
				rectangle = cdd.texture.Frame(1, 1, 0, 0, 0, 0);
			}
			rectangle.X = 0;
			rectangle.Y = 0;
			Vector2 position = cdd.position;
			SpriteRenderTargetHelper.GetHighsAndLowsOf(ref lowest, ref highest, ref cdd, ref position, ref origin, new Vector2(0f, 0f));
			SpriteRenderTargetHelper.GetHighsAndLowsOf(ref lowest, ref highest, ref cdd, ref position, ref origin, new Vector2((float)rectangle.Width, 0f));
			SpriteRenderTargetHelper.GetHighsAndLowsOf(ref lowest, ref highest, ref cdd, ref position, ref origin, new Vector2(0f, (float)rectangle.Height));
			SpriteRenderTargetHelper.GetHighsAndLowsOf(ref lowest, ref highest, ref cdd, ref position, ref origin, new Vector2((float)rectangle.Width, (float)rectangle.Height));
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0051AC10 File Offset: 0x00518E10
		public static void GetHighsAndLowsOf(ref Vector2 lowest, ref Vector2 highest, ref DrawData cdd, ref Vector2 pos, ref Vector2 origin, Vector2 corner)
		{
			Vector2 corner2 = SpriteRenderTargetHelper.GetCorner(ref cdd, ref pos, ref origin, corner);
			lowest = Vector2.Min(lowest, corner2);
			highest = Vector2.Max(highest, corner2);
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0051AC50 File Offset: 0x00518E50
		public static Vector2 GetCorner(ref DrawData cdd, ref Vector2 pos, ref Vector2 origin, Vector2 corner)
		{
			Vector2 spinningpoint = corner - origin;
			return pos + spinningpoint.RotatedBy((double)cdd.rotation, default(Vector2)) * cdd.scale;
		}
	}
}
