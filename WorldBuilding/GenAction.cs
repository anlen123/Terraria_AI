using System;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A3 RID: 163
	public abstract class GenAction : GenBase
	{
		// Token: 0x06001724 RID: 5924
		public abstract bool Apply(Point origin, int x, int y, params object[] args);

		// Token: 0x06001725 RID: 5925 RVA: 0x004DD0F2 File Offset: 0x004DB2F2
		protected bool UnitApply(Point origin, int x, int y, params object[] args)
		{
			if (this.OutputData != null)
			{
				this.OutputData.Add(x - origin.X, y - origin.Y);
			}
			return this.NextAction == null || this.NextAction.Apply(origin, x, y, args);
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x004DD131 File Offset: 0x004DB331
		public GenAction IgnoreFailures()
		{
			this._returnFalseOnFailure = false;
			return this;
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x004DD13B File Offset: 0x004DB33B
		protected bool Fail()
		{
			return !this._returnFalseOnFailure;
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x004DD146 File Offset: 0x004DB346
		public GenAction Output(ShapeData data)
		{
			this.OutputData = data;
			return this;
		}

		// Token: 0x040011BA RID: 4538
		public GenAction NextAction;

		// Token: 0x040011BB RID: 4539
		public ShapeData OutputData;

		// Token: 0x040011BC RID: 4540
		private bool _returnFalseOnFailure = true;
	}
}
