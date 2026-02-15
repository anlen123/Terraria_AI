using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x0200009A RID: 154
	public class WorldSeedOption_Anniversary : AWorldGenerationOption
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x004DCCFD File Offset: 0x004DAEFD
		protected override string KeyName
		{
			get
			{
				return "Seed_Celebration";
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x004DCD04 File Offset: 0x004DAF04
		public override string ServerConfigName
		{
			get
			{
				return "celebration";
			}
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x004DCD0B File Offset: 0x004DAF0B
		public WorldSeedOption_Anniversary()
		{
			base.SpecialSeedNames = new string[]
			{
				"celebrationmk10"
			};
			base.SpecialSeedValues = new int[]
			{
				5162021,
				5162011
			};
		}
	}
}
