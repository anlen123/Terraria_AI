using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	// Token: 0x02000483 RID: 1155
	public class ActionPlaceStatue : GenAction
	{
		// Token: 0x0600333D RID: 13117 RVA: 0x005F4853 File Offset: 0x005F2A53
		public ActionPlaceStatue(int index = -1)
		{
			this._statueIndex = index;
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x005F4864 File Offset: 0x005F2A64
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			Point16 point;
			if (this._statueIndex == -1)
			{
				point = GenVars.statueList[GenBase._random.Next(2, GenVars.statueList.Length)];
			}
			else
			{
				point = GenVars.statueList[this._statueIndex];
			}
			WorldGen.PlaceTile(x, y, (int)point.X, true, false, -1, (int)point.Y);
			return base.UnitApply(origin, x, y, args);
		}

		// Token: 0x0400589D RID: 22685
		private int _statueIndex;
	}
}
