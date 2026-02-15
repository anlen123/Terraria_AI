using System;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000AE RID: 174
	public abstract class GenSearch : GenBase
	{
		// Token: 0x0600174C RID: 5964 RVA: 0x004DD441 File Offset: 0x004DB641
		public GenSearch Conditions(params GenCondition[] conditions)
		{
			this._conditions = conditions;
			return this;
		}

		// Token: 0x0600174D RID: 5965
		public abstract Point Find(Point origin);

		// Token: 0x0600174E RID: 5966 RVA: 0x004DD44C File Offset: 0x004DB64C
		protected bool Check(int x, int y)
		{
			for (int i = 0; i < this._conditions.Length; i++)
			{
				if (this._requireAll ^ this._conditions[i].IsValid(x, y))
				{
					return !this._requireAll;
				}
			}
			return this._requireAll;
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x004DD494 File Offset: 0x004DB694
		public GenSearch RequireAll(bool mode)
		{
			this._requireAll = mode;
			return this;
		}

		// Token: 0x040011CA RID: 4554
		public static Point NOT_FOUND = new Point(int.MaxValue, int.MaxValue);

		// Token: 0x040011CB RID: 4555
		private bool _requireAll = true;

		// Token: 0x040011CC RID: 4556
		private GenCondition[] _conditions;
	}
}
