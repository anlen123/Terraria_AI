using System;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000B0 RID: 176
	public abstract class GenStructure : GenBase
	{
		// Token: 0x06001753 RID: 5971 RVA: 0x004DD4CC File Offset: 0x004DB6CC
		public virtual bool Place(Point origin, StructureMap structures)
		{
			return this.Place(origin, structures, null);
		}

		// Token: 0x06001754 RID: 5972
		public abstract bool Place(Point origin, StructureMap structures, GenerationProgress progress);
	}
}
