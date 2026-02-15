using System;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x0200046A RID: 1130
	public class RunnerLeashedCritter : WalkerLeashedCritter
	{
		// Token: 0x060032C9 RID: 13001 RVA: 0x005F1BBC File Offset: 0x005EFDBC
		public RunnerLeashedCritter()
		{
			this.anchorStyle = 1;
			this.walkingPace = 1.5f;
		}

		// Token: 0x04005842 RID: 22594
		public new static RunnerLeashedCritter Prototype = new RunnerLeashedCritter();
	}
}
