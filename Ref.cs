using System;

namespace Terraria
{
	// Token: 0x02000025 RID: 37
	public class Ref<T>
	{
		// Token: 0x0600019C RID: 412 RVA: 0x0000357B File Offset: 0x0000177B
		public Ref()
		{
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00011DEC File Offset: 0x0000FFEC
		public Ref(T value)
		{
			this.Value = value;
		}

		// Token: 0x0400012E RID: 302
		public T Value;
	}
}
