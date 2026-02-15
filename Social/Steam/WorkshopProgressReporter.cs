using System;
using System.Collections.Generic;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	// Token: 0x02000141 RID: 321
	public class WorkshopProgressReporter : AWorkshopProgressReporter
	{
		// Token: 0x06001C7B RID: 7291 RVA: 0x004FDF77 File Offset: 0x004FC177
		public WorkshopProgressReporter(List<WorkshopHelper.UGCBased.APublisherInstance> publisherInstances)
		{
			this._publisherInstances = publisherInstances;
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x004FDF86 File Offset: 0x004FC186
		public override bool HasOngoingTasks
		{
			get
			{
				return this._publisherInstances.Count > 0;
			}
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x004FDF98 File Offset: 0x004FC198
		public override bool TryGetProgress(out float progress)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < this._publisherInstances.Count; i++)
			{
				float num3;
				if (this._publisherInstances[i].TryGetProgress(out num3))
				{
					num += num3;
					num2 += 1f;
				}
			}
			progress = 0f;
			if (num2 == 0f)
			{
				return false;
			}
			progress = num / num2;
			return true;
		}

		// Token: 0x040015C5 RID: 5573
		private List<WorkshopHelper.UGCBased.APublisherInstance> _publisherInstances;
	}
}
