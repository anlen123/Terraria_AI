using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	// Token: 0x02000484 RID: 1156
	public class ActionStalagtite : GenAction
	{
		// Token: 0x0600333F RID: 13119 RVA: 0x005F48CC File Offset: 0x005F2ACC
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			WorldGen.PlaceTight(x, y, false);
			return base.UnitApply(origin, x, y, args);
		}
	}
}
