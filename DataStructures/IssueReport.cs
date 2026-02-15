using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200054A RID: 1354
	public class IssueReport
	{
		// Token: 0x0600376A RID: 14186 RVA: 0x0062E4AF File Offset: 0x0062C6AF
		public IssueReport(string reportText)
		{
			this.timeReported = DateTime.Now;
			this.reportText = reportText;
		}

		// Token: 0x04005B86 RID: 23430
		public DateTime timeReported;

		// Token: 0x04005B87 RID: 23431
		public string reportText;
	}
}
