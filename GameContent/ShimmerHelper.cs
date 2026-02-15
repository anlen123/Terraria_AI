using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x02000248 RID: 584
	public class ShimmerHelper
	{
		// Token: 0x060022E6 RID: 8934 RVA: 0x0053AEE8 File Offset: 0x005390E8
		public static Vector2? FindSpotWithoutShimmer(Entity entity, int startX, int startY, int expand, bool allowSolidTop)
		{
			Vector2 value = new Vector2((float)(-(float)entity.width / 2), (float)(-(float)entity.height));
			for (int i = 0; i < expand; i++)
			{
				float num = (float)(startX - i);
				int num2 = startY - expand;
				Vector2 vector = new Vector2(num * (float)16, (float)(num2 * 16)) + value;
				if (ShimmerHelper.IsSpotShimmerFree(entity, vector, allowSolidTop))
				{
					return new Vector2?(vector);
				}
				vector = new Vector2((float)((startX + i) * 16), (float)(num2 * 16)) + value;
				if (ShimmerHelper.IsSpotShimmerFree(entity, vector, allowSolidTop))
				{
					return new Vector2?(vector);
				}
				float num3 = (float)(startX - i);
				num2 = startY + expand;
				vector = new Vector2(num3 * (float)16, (float)(num2 * 16)) + value;
				if (ShimmerHelper.IsSpotShimmerFree(entity, vector, allowSolidTop))
				{
					return new Vector2?(vector);
				}
				vector = new Vector2((float)((startX + i) * 16), (float)(num2 * 16)) + value;
				if (ShimmerHelper.IsSpotShimmerFree(entity, vector, allowSolidTop))
				{
					return new Vector2?(vector);
				}
			}
			for (int j = 0; j < expand; j++)
			{
				float num4 = (float)(startX - expand);
				int num5 = startY - j;
				Vector2 vector2 = new Vector2(num4 * (float)16, (float)(num5 * 16)) + value;
				if (ShimmerHelper.IsSpotShimmerFree(entity, vector2, allowSolidTop))
				{
					return new Vector2?(vector2);
				}
				vector2 = new Vector2((float)((startX + expand) * 16), (float)(num5 * 16)) + value;
				if (ShimmerHelper.IsSpotShimmerFree(entity, vector2, allowSolidTop))
				{
					return new Vector2?(vector2);
				}
				float num6 = (float)(startX - expand);
				num5 = startY + j;
				vector2 = new Vector2(num6 * (float)16, (float)(num5 * 16)) + value;
				if (ShimmerHelper.IsSpotShimmerFree(entity, vector2, allowSolidTop))
				{
					return new Vector2?(vector2);
				}
				vector2 = new Vector2((float)((startX + expand) * 16), (float)(num5 * 16)) + value;
				if (ShimmerHelper.IsSpotShimmerFree(entity, vector2, allowSolidTop))
				{
					return new Vector2?(vector2);
				}
			}
			return null;
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x0053B0B4 File Offset: 0x005392B4
		private static bool IsSpotShimmerFree(Entity entity, Vector2 landingPosition, bool allowSolidTop)
		{
			return !Collision.SolidCollision(landingPosition, entity.width, entity.height) && Collision.SolidCollision(landingPosition + new Vector2(0f, (float)entity.height), entity.width, 100, allowSolidTop) && (!Collision.WetCollision(landingPosition, entity.width, entity.height + 100) || !Collision.shimmer);
		}
	}
}
