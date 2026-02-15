using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	// Token: 0x02000482 RID: 1154
	public class ActionGrass : GenAction
	{
		// Token: 0x0600333B RID: 13115 RVA: 0x005F47E4 File Offset: 0x005F29E4
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			if (GenBase._tiles[x, y].active() || GenBase._tiles[x, y - 1].active())
			{
				return false;
			}
			WorldGen.PlaceTile(x, y, (int)Utils.SelectRandom<ushort>(GenBase._random, new ushort[]
			{
				3,
				73
			}), true, false, -1, 0);
			return base.UnitApply(origin, x, y, args);
		}
	}
}
