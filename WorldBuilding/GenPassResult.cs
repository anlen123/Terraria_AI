using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x02000095 RID: 149
	public class GenPassResult
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060016CB RID: 5835 RVA: 0x004DC63B File Offset: 0x004DA83B
		// (set) Token: 0x060016CC RID: 5836 RVA: 0x004DC643 File Offset: 0x004DA843
		public string Name { get; set; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060016CD RID: 5837 RVA: 0x004DC64C File Offset: 0x004DA84C
		// (set) Token: 0x060016CE RID: 5838 RVA: 0x004DC654 File Offset: 0x004DA854
		public int DurationMs { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060016CF RID: 5839 RVA: 0x004DC65D File Offset: 0x004DA85D
		// (set) Token: 0x060016D0 RID: 5840 RVA: 0x004DC665 File Offset: 0x004DA865
		public int RandNext { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060016D1 RID: 5841 RVA: 0x004DC66E File Offset: 0x004DA86E
		// (set) Token: 0x060016D2 RID: 5842 RVA: 0x004DC676 File Offset: 0x004DA876
		public uint? Hash { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060016D3 RID: 5843 RVA: 0x004DC67F File Offset: 0x004DA87F
		// (set) Token: 0x060016D4 RID: 5844 RVA: 0x004DC687 File Offset: 0x004DA887
		public bool Skipped { get; set; }

		// Token: 0x060016D5 RID: 5845 RVA: 0x004DC690 File Offset: 0x004DA890
		public override string ToString()
		{
			if (this.Skipped)
			{
				return string.Format("Pass - {0}: Skipped", this.Name);
			}
			return string.Format("Pass - {0}: {1}ms, rand: {2:X8}, hash: {3:X8}", new object[]
			{
				this.Name,
				this.DurationMs,
				this.RandNext,
				this.Hash
			});
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x004DC6FC File Offset: 0x004DA8FC
		public bool Matches(GenPassResult other)
		{
			if (!(this.Name == other.Name) || this.RandNext != other.RandNext || this.Skipped != other.Skipped)
			{
				return false;
			}
			if (this.Hash != null && other.Hash != null)
			{
				uint? hash = this.Hash;
				uint? hash2 = other.Hash;
				return hash.GetValueOrDefault() == hash2.GetValueOrDefault() & hash != null == (hash2 != null);
			}
			return true;
		}
	}
}
