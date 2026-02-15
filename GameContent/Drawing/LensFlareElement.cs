using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x0200043B RID: 1083
	public struct LensFlareElement
	{
		// Token: 0x060030C3 RID: 12483 RVA: 0x005BCF44 File Offset: 0x005BB144
		public void Draw(SpriteBatch spriteBatch, Vector2 sunPosition, Vector2 screenCenterPosition, float intensity)
		{
			if (intensity == 0f)
			{
				return;
			}
			Player localPlayer = Main.LocalPlayer;
			int availableAdvancedShadowsCount = localPlayer.availableAdvancedShadowsCount;
			Vector2 v = localPlayer.GetAdvancedShadow(0).Position - localPlayer.GetAdvancedShadow(Math.Min(4, availableAdvancedShadowsCount - 1)).Position;
			float num = Vector2.Dot(v.SafeNormalize(Vector2.UnitX), (sunPosition - screenCenterPosition).SafeNormalize(-Vector2.UnitY)) * v.Length();
			for (int i = 0; i < this.RepeatTimes; i++)
			{
				float scale = this.ScaleStart + this.ScaleOverIndex * (float)i;
				Color color = this.Color * (1f + this.IntensityOverIndex * (float)i) * intensity;
				float num2 = this.DistanceStart + this.DistanceAlongIndex * (float)i;
				num2 += num * -0.0002f;
				num2 %= 1f;
				Vector2 position = Vector2.Lerp(sunPosition, screenCenterPosition, num2 * 2f);
				float num3 = (screenCenterPosition - sunPosition).ToRotation() + this.Rotation;
				if (this.Rotation == 0f)
				{
					num3 += Main.screenPosition.Y * 0.001f;
				}
				spriteBatch.Draw(this.Texture.Value, position, null, color, num3, this.Texture.Size() / 2f, scale, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x040056E4 RID: 22244
		public Asset<Texture2D> Texture;

		// Token: 0x040056E5 RID: 22245
		public int RepeatTimes;

		// Token: 0x040056E6 RID: 22246
		public float ScaleStart;

		// Token: 0x040056E7 RID: 22247
		public float ScaleOverIndex;

		// Token: 0x040056E8 RID: 22248
		public float DistanceStart;

		// Token: 0x040056E9 RID: 22249
		public float DistanceAlongIndex;

		// Token: 0x040056EA RID: 22250
		public Color Color;

		// Token: 0x040056EB RID: 22251
		public float IntensityOverIndex;

		// Token: 0x040056EC RID: 22252
		public float Rotation;
	}
}
