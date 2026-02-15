using System;
using Terraria.IO;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000AC RID: 172
	public abstract class GenPass : GenBase
	{
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x004DD3F7 File Offset: 0x004DB5F7
		// (set) Token: 0x06001746 RID: 5958 RVA: 0x004DD3FF File Offset: 0x004DB5FF
		public bool Enabled { get; private set; }

		// Token: 0x06001747 RID: 5959 RVA: 0x004DD408 File Offset: 0x004DB608
		public void Disable()
		{
			this.Enabled = false;
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x004DD411 File Offset: 0x004DB611
		internal void Enable()
		{
			this.Enabled = true;
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x004DD41A File Offset: 0x004DB61A
		public GenPass(string name, double loadWeight)
		{
			this.Name = name;
			this.Weight = loadWeight;
			this.Enabled = true;
		}

		// Token: 0x0600174A RID: 5962
		protected abstract void ApplyPass(GenerationProgress progress, GameConfiguration configuration);

		// Token: 0x0600174B RID: 5963 RVA: 0x004DD437 File Offset: 0x004DB637
		public void Apply(GenerationProgress progress, GameConfiguration configuration)
		{
			this.ApplyPass(progress, configuration);
		}

		// Token: 0x040011C8 RID: 4552
		public string Name;

		// Token: 0x040011C9 RID: 4553
		public double Weight;
	}
}
