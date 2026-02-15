using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	// Token: 0x0200054B RID: 1355
	public class GeneralIssueReporter : IProvideReports
	{
		// Token: 0x0600376B RID: 14187 RVA: 0x0062E4C9 File Offset: 0x0062C6C9
		public void AddReport(string textToShow)
		{
			this._reports.Add(new IssueReport(textToShow));
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x0062E4DC File Offset: 0x0062C6DC
		public List<IssueReport> GetReports()
		{
			return this._reports;
		}

		// Token: 0x04005B88 RID: 23432
		private List<IssueReport> _reports = new List<IssueReport>();
	}
}
