using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	// Token: 0x02000485 RID: 1157
	public class ActionVines : GenAction
	{
		// Token: 0x06003341 RID: 13121 RVA: 0x005F48E1 File Offset: 0x005F2AE1
		public ActionVines(int minLength = 6, int maxLength = 10, int vineId = 52)
		{
			this._minLength = minLength;
			this._maxLength = maxLength;
			this._vineId = vineId;
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x005F4900 File Offset: 0x005F2B00
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			int num = GenBase._random.Next(this._minLength, this._maxLength + 1);
			int num2 = 0;
			while (num2 < num && !GenBase._tiles[x, y + num2].active())
			{
				GenBase._tiles[x, y + num2].type = (ushort)this._vineId;
				GenBase._tiles[x, y + num2].active(true);
				num2++;
			}
			return num2 > 0 && base.UnitApply(origin, x, y, args);
		}

		// Token: 0x0400589E RID: 22686
		private int _minLength;

		// Token: 0x0400589F RID: 22687
		private int _maxLength;

		// Token: 0x040058A0 RID: 22688
		private int _vineId;
	}
}
