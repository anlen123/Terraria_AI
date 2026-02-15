using System;
using Terraria.Enums;

namespace Terraria.DataStructures
{
	// Token: 0x02000538 RID: 1336
	public struct AnchorData
	{
		// Token: 0x06003739 RID: 14137 RVA: 0x0062DA16 File Offset: 0x0062BC16
		public AnchorData(AnchorType type, int count, int start)
		{
			this.type = type;
			this.tileCount = count;
			this.checkStart = start;
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x0062DA2D File Offset: 0x0062BC2D
		public static bool operator ==(AnchorData data1, AnchorData data2)
		{
			return data1.type == data2.type && data1.tileCount == data2.tileCount && data1.checkStart == data2.checkStart;
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x0062DA5B File Offset: 0x0062BC5B
		public static bool operator !=(AnchorData data1, AnchorData data2)
		{
			return data1.type != data2.type || data1.tileCount != data2.tileCount || data1.checkStart != data2.checkStart;
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x0062DA8C File Offset: 0x0062BC8C
		public override bool Equals(object obj)
		{
			return obj is AnchorData && (this.type == ((AnchorData)obj).type && this.tileCount == ((AnchorData)obj).tileCount) && this.checkStart == ((AnchorData)obj).checkStart;
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x0062DAE0 File Offset: 0x0062BCE0
		public override int GetHashCode()
		{
			byte b = (byte)this.checkStart;
			byte b2 = (byte)this.tileCount;
			return (int)((ushort)this.type) << 16 | (int)b2 << 8 | (int)b;
		}

		// Token: 0x04005B58 RID: 23384
		public AnchorType type;

		// Token: 0x04005B59 RID: 23385
		public int tileCount;

		// Token: 0x04005B5A RID: 23386
		public int checkStart;

		// Token: 0x04005B5B RID: 23387
		public static AnchorData Empty;
	}
}
