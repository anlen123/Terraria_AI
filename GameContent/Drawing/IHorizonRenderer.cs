using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000434 RID: 1076
	public interface IHorizonRenderer
	{
		// Token: 0x0600308B RID: 12427
		void DrawHorizon();

		// Token: 0x0600308C RID: 12428
		void ModifyHorizonLight(ref Color color);

		// Token: 0x0600308D RID: 12429
		void DrawSun(Vector2 sunPosition);

		// Token: 0x0600308E RID: 12430
		void CloudsStart();

		// Token: 0x0600308F RID: 12431
		void DrawCloud(float globalCloudAlpha, Cloud theCloud, int cloudPass, float cY);

		// Token: 0x06003090 RID: 12432
		void CloudsEnd();

		// Token: 0x06003091 RID: 12433
		void DrawSurfaceLayer(int layerIndex);

		// Token: 0x06003092 RID: 12434
		void DrawLensFlare();
	}
}
