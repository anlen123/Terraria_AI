using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.Light
{
	// Token: 0x020001FB RID: 507
	public interface ILightingEngine
	{
		// Token: 0x060020CD RID: 8397
		void Rebuild();

		// Token: 0x060020CE RID: 8398
		void AddLight(int x, int y, Vector3 color);

		// Token: 0x060020CF RID: 8399
		void ProcessArea(Rectangle area);

		// Token: 0x060020D0 RID: 8400
		Vector3 GetColor(int x, int y);

		// Token: 0x060020D1 RID: 8401
		void Clear();
	}
}
