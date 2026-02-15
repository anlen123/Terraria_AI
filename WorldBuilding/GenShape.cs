using System;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000B6 RID: 182
	public abstract class GenShape : GenBase
	{
		// Token: 0x06001773 RID: 6003
		public abstract bool Perform(Point origin, GenAction action);

		// Token: 0x06001774 RID: 6004 RVA: 0x004DD7F1 File Offset: 0x004DB9F1
		protected bool UnitApply(GenAction action, Point origin, int x, int y, params object[] args)
		{
			if (this._outputData != null)
			{
				this._outputData.Add(x - origin.X, y - origin.Y);
			}
			return action.Apply(origin, x, y, args);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x004DD823 File Offset: 0x004DBA23
		public GenShape Output(ShapeData outputData)
		{
			this._outputData = outputData;
			return this;
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x004DD82D File Offset: 0x004DBA2D
		public GenShape QuitOnFail(bool value = true)
		{
			this._quitOnFail = value;
			return this;
		}

		// Token: 0x04001261 RID: 4705
		private ShapeData _outputData;

		// Token: 0x04001262 RID: 4706
		protected bool _quitOnFail;
	}
}
