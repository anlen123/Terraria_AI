using System;
using System.Linq;

namespace Terraria.GameInput
{
	// Token: 0x0200008F RID: 143
	public class TriggersPack
	{
		// Token: 0x060015F1 RID: 5617 RVA: 0x004D4D28 File Offset: 0x004D2F28
		public void Initialize()
		{
			this.Current.SetupKeys();
			this.Old.SetupKeys();
			this.JustPressed.SetupKeys();
			this.JustReleased.SetupKeys();
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x004D4D56 File Offset: 0x004D2F56
		public void Reset()
		{
			this.Old.CloneFrom(this.Current);
			this.Current.Reset();
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x004D4D74 File Offset: 0x004D2F74
		public void Update()
		{
			this.CompareDiffs(this.JustPressed, this.Old, this.Current);
			this.CompareDiffs(this.JustReleased, this.Current, this.Old);
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x004D4DA8 File Offset: 0x004D2FA8
		public void CompareDiffs(TriggersSet Bearer, TriggersSet oldset, TriggersSet newset)
		{
			Bearer.Reset();
			foreach (string key in Bearer.KeyStatus.Keys.ToList<string>())
			{
				Bearer.KeyStatus[key] = (newset.KeyStatus[key] && !oldset.KeyStatus[key]);
			}
		}

		// Token: 0x04001155 RID: 4437
		public TriggersSet Current = new TriggersSet();

		// Token: 0x04001156 RID: 4438
		public TriggersSet Old = new TriggersSet();

		// Token: 0x04001157 RID: 4439
		public TriggersSet JustPressed = new TriggersSet();

		// Token: 0x04001158 RID: 4440
		public TriggersSet JustReleased = new TriggersSet();
	}
}
