using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200053A RID: 1338
	public class BackgroundVariant
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06003741 RID: 14145 RVA: 0x0062DB6F File Offset: 0x0062BD6F
		public int[] Backgrounds
		{
			get
			{
				return this._backgrounds;
			}
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x0062DB77 File Offset: 0x0062BD77
		public void Set(int far, int middle, int near)
		{
			this._backgrounds[0] = far;
			this._backgrounds[1] = middle;
			this._backgrounds[2] = near;
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x0062DB94 File Offset: 0x0062BD94
		public void Clear()
		{
			this.Set(-1, -1, -1);
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06003744 RID: 14148 RVA: 0x0062DB9F File Offset: 0x0062BD9F
		public bool HasAny
		{
			get
			{
				return this._backgrounds[0] != -1 || this._backgrounds[1] != -1 || this._backgrounds[2] != -1;
			}
		}

		// Token: 0x04005B60 RID: 23392
		private readonly int[] _backgrounds = new int[]
		{
			-1,
			-1,
			-1
		};
	}
}
