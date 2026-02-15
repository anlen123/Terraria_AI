using System;
using System.Text;

namespace Terraria.Utilities
{
	// Token: 0x020000C9 RID: 201
	public struct Vertical64BitStrips
	{
		// Token: 0x060017E3 RID: 6115 RVA: 0x004DFF1E File Offset: 0x004DE11E
		public Vertical64BitStrips(int len)
		{
			this.arr = new Bits64[len];
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x004DFF2C File Offset: 0x004DE12C
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
		}

		// Token: 0x170002A3 RID: 675
		public Bits64 this[int x]
		{
			get
			{
				return this.arr[x];
			}
			set
			{
				this.arr[x] = value;
			}
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x004DFF60 File Offset: 0x004DE160
		public void Expand3x3()
		{
			for (int i = 0; i < this.arr.Length - 1; i++)
			{
				Bits64[] array = this.arr;
				int num = i;
				array[num] |= this.arr[i + 1];
			}
			for (int j = this.arr.Length - 1; j > 0; j--)
			{
				Bits64[] array2 = this.arr;
				int num2 = j;
				array2[num2] |= this.arr[j - 1];
			}
			for (int k = 0; k < this.arr.Length; k++)
			{
				Bits64 b = this.arr[k];
				this.arr[k] = (b << 1 | b | b >> 1);
			}
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x004E004C File Offset: 0x004DE24C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(this.arr.Length * 65);
			for (int i = 0; i < 64; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append('\n');
				}
				for (int j = 0; j < this.arr.Length; j++)
				{
					stringBuilder.Append(this[j][i] ? 'x' : ' ');
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400129D RID: 4765
		private Bits64[] arr;
	}
}
