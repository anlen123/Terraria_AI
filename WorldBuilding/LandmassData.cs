using System;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000B3 RID: 179
	public struct LandmassData
	{
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x004DD4F3 File Offset: 0x004DB6F3
		// (set) Token: 0x06001759 RID: 5977 RVA: 0x004DD511 File Offset: 0x004DB711
		public Vector2 Top
		{
			get
			{
				return this.Position - new Vector2(0f, (float)this.RadiusOrHalfSize);
			}
			set
			{
				this.Position = value + new Vector2(0f, (float)this.RadiusOrHalfSize);
			}
		}

		// Token: 0x040011D4 RID: 4564
		public LandmassDataType DataType;

		// Token: 0x040011D5 RID: 4565
		public Vector2 Position;

		// Token: 0x040011D6 RID: 4566
		public int RadiusOrHalfSize;

		// Token: 0x040011D7 RID: 4567
		public int Style;
	}
}
