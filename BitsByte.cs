using System;
using System.Collections.Generic;
using System.IO;

namespace Terraria
{
	// Token: 0x02000036 RID: 54
	public struct BitsByte
	{
		// Token: 0x06000329 RID: 809 RVA: 0x00045140 File Offset: 0x00043340
		public BitsByte(bool b1 = false, bool b2 = false, bool b3 = false, bool b4 = false, bool b5 = false, bool b6 = false, bool b7 = false, bool b8 = false)
		{
			this.value = 0;
			this[0] = b1;
			this[1] = b2;
			this[2] = b3;
			this[3] = b4;
			this[4] = b5;
			this[5] = b6;
			this[6] = b7;
			this[7] = b8;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00045199 File Offset: 0x00043399
		public void ClearAll()
		{
			this.value = 0;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x000451A2 File Offset: 0x000433A2
		public void SetAll()
		{
			this.value = byte.MaxValue;
		}

		// Token: 0x170000B0 RID: 176
		public bool this[int key]
		{
			get
			{
				return ((int)this.value & 1 << key) != 0;
			}
			set
			{
				if (value)
				{
					this.value |= (byte)(1 << key);
					return;
				}
				this.value &= (byte)(~(byte)(1 << key));
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000451F4 File Offset: 0x000433F4
		public void Retrieve(ref bool b0)
		{
			this.Retrieve(ref b0, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0004522C File Offset: 0x0004342C
		public void Retrieve(ref bool b0, ref bool b1)
		{
			this.Retrieve(ref b0, ref b1, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00045260 File Offset: 0x00043460
		public void Retrieve(ref bool b0, ref bool b1, ref bool b2)
		{
			this.Retrieve(ref b0, ref b1, ref b2, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00045290 File Offset: 0x00043490
		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3)
		{
			this.Retrieve(ref b0, ref b1, ref b2, ref b3, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x000452BC File Offset: 0x000434BC
		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4)
		{
			this.Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref BitsByte.Null, ref BitsByte.Null, ref BitsByte.Null);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x000452E8 File Offset: 0x000434E8
		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5)
		{
			this.Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref b5, ref BitsByte.Null, ref BitsByte.Null);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00045310 File Offset: 0x00043510
		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5, ref bool b6)
		{
			this.Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref b5, ref b6, ref BitsByte.Null);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00045334 File Offset: 0x00043534
		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5, ref bool b6, ref bool b7)
		{
			b0 = this[0];
			b1 = this[1];
			b2 = this[2];
			b3 = this[3];
			b4 = this[4];
			b5 = this[5];
			b6 = this[6];
			b7 = this[7];
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0004538E File Offset: 0x0004358E
		public static implicit operator byte(BitsByte bb)
		{
			return bb.value;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00045398 File Offset: 0x00043598
		public static implicit operator BitsByte(byte b)
		{
			return new BitsByte
			{
				value = b
			};
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000453B8 File Offset: 0x000435B8
		public static BitsByte[] ComposeBitsBytesChain(bool optimizeLength, params bool[] flags)
		{
			int i = flags.Length;
			int num = 0;
			while (i > 0)
			{
				num++;
				i -= 7;
			}
			BitsByte[] array = new BitsByte[num];
			int num2 = 0;
			int num3 = 0;
			for (int j = 0; j < flags.Length; j++)
			{
				array[num3][num2] = flags[j];
				num2++;
				if (num2 == 7 && num3 < num - 1)
				{
					array[num3][num2] = true;
					num2 = 0;
					num3++;
				}
			}
			if (optimizeLength)
			{
				int num4 = array.Length - 1;
				while (array[num4] == 0 && num4 > 0)
				{
					array[num4 - 1][7] = false;
					num4--;
				}
				Array.Resize<BitsByte>(ref array, num4 + 1);
			}
			return array;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00045474 File Offset: 0x00043674
		public static BitsByte[] DecomposeBitsBytesChain(BinaryReader reader)
		{
			List<BitsByte> list = new List<BitsByte>();
			BitsByte item;
			do
			{
				item = reader.ReadByte();
				list.Add(item);
			}
			while (item[7]);
			return list.ToArray();
		}

		// Token: 0x0400025F RID: 607
		private static bool Null;

		// Token: 0x04000260 RID: 608
		private byte value;
	}
}
