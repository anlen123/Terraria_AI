using System;

namespace Terraria.Utilities
{
	// Token: 0x020000C8 RID: 200
	public struct Bits64
	{
		// Token: 0x170002A1 RID: 673
		public bool this[int i]
		{
			get
			{
				return (this.v & 1UL << i) > 0UL;
			}
			set
			{
				if (value)
				{
					this.v |= 1UL << i;
					return;
				}
				this.v &= ~(1UL << i);
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x004DFEE9 File Offset: 0x004DE0E9
		public bool IsEmpty
		{
			get
			{
				return this.v == 0UL;
			}
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x004DFEF5 File Offset: 0x004DE0F5
		public static implicit operator ulong(Bits64 b)
		{
			return b.v;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x004DFF00 File Offset: 0x004DE100
		public static implicit operator Bits64(ulong v)
		{
			return new Bits64
			{
				v = v
			};
		}

		// Token: 0x0400129C RID: 4764
		private ulong v;
	}
}
