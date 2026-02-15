using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000B8 RID: 184
	public struct GenShapeActionPair
	{
		// Token: 0x06001778 RID: 6008 RVA: 0x004DD837 File Offset: 0x004DBA37
		public GenShapeActionPair(GenShape shape, GenAction action)
		{
			this.Shape = shape;
			this.Action = action;
		}

		// Token: 0x04001263 RID: 4707
		public readonly GenShape Shape;

		// Token: 0x04001264 RID: 4708
		public readonly GenAction Action;
	}
}
