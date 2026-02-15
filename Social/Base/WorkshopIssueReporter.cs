using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Terraria.Social.Base
{
	// Token: 0x02000159 RID: 345
	public class WorkshopIssueReporter : IProvideReports
	{
		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06001D38 RID: 7480 RVA: 0x005009D8 File Offset: 0x004FEBD8
		// (remove) Token: 0x06001D39 RID: 7481 RVA: 0x00500A10 File Offset: 0x004FEC10
		public event Action OnNeedToOpenUI;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06001D3A RID: 7482 RVA: 0x00500A48 File Offset: 0x004FEC48
		// (remove) Token: 0x06001D3B RID: 7483 RVA: 0x00500A80 File Offset: 0x004FEC80
		public event Action OnNeedToNotifyUI;

		// Token: 0x06001D3C RID: 7484 RVA: 0x00500AB8 File Offset: 0x004FECB8
		private void AddReport(string reportText)
		{
			IssueReport item = new IssueReport(reportText);
			this._reports.Add(item);
			while (this._reports.Count > this._maxReports)
			{
				this._reports.RemoveAt(0);
			}
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x00500AF9 File Offset: 0x004FECF9
		public List<IssueReport> GetReports()
		{
			return this._reports;
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x00500B01 File Offset: 0x004FED01
		private void OpenReportsScreen()
		{
			if (this.OnNeedToOpenUI != null)
			{
				this.OnNeedToOpenUI();
			}
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x00500B16 File Offset: 0x004FED16
		private void NotifyReportsScreen()
		{
			if (this.OnNeedToNotifyUI != null)
			{
				this.OnNeedToNotifyUI();
			}
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x00500B2C File Offset: 0x004FED2C
		public void ReportInstantUploadProblem(string textKey)
		{
			string textValue = Language.GetTextValue(textKey);
			this.AddReport(textValue);
			this.OpenReportsScreen();
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x00500B4D File Offset: 0x004FED4D
		public void ReportInstantUploadProblemFromValue(string text)
		{
			this.AddReport(text);
			this.OpenReportsScreen();
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x00500B5C File Offset: 0x004FED5C
		public void ReportDelayedUploadProblem(string textKey)
		{
			string textValue = Language.GetTextValue(textKey);
			this.AddReport(textValue);
			this.NotifyReportsScreen();
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x00500B80 File Offset: 0x004FED80
		public void ReportDelayedUploadProblemWithoutKnownReason(string textKey, string reasonValue)
		{
			object obj = new
			{
				Reason = reasonValue
			};
			string textValueWith = Language.GetTextValueWith(textKey, obj);
			this.AddReport(textValueWith);
			this.NotifyReportsScreen();
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x00500BAC File Offset: 0x004FEDAC
		public void ReportDownloadProblem(string textKey, string path, Exception exception)
		{
			object obj = new
			{
				FilePath = path,
				Reason = exception.ToString()
			};
			string textValueWith = Language.GetTextValueWith(textKey, obj);
			this.AddReport(textValueWith);
			this.NotifyReportsScreen();
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x00500BDC File Offset: 0x004FEDDC
		public void ReportManifestCreationProblem(string textKey, Exception exception)
		{
			object obj = new
			{
				Reason = exception.ToString()
			};
			string textValueWith = Language.GetTextValueWith(textKey, obj);
			this.AddReport(textValueWith);
			this.NotifyReportsScreen();
		}

		// Token: 0x0400162B RID: 5675
		private int _maxReports = 1000;

		// Token: 0x0400162C RID: 5676
		private List<IssueReport> _reports = new List<IssueReport>();
	}
}
