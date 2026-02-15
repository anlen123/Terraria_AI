using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x0200023B RID: 571
	public struct PositionedChest
	{
		// Token: 0x06002274 RID: 8820 RVA: 0x00538265 File Offset: 0x00536465
		public PositionedChest(Chest chest, Vector2 position)
		{
			this.chest = chest;
			this.position = position;
		}

		// Token: 0x04004CE1 RID: 19681
		public Chest chest;

		// Token: 0x04004CE2 RID: 19682
		public Vector2 position;
	}
}
