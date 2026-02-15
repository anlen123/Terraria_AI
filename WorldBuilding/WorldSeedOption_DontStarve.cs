using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x0200009B RID: 155
	public class WorldSeedOption_DontStarve : AWorldGenerationOption
	{
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001708 RID: 5896 RVA: 0x004DCD43 File Offset: 0x004DAF43
		protected override string KeyName
		{
			get
			{
				return "Seed_TheConstant";
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x004DCD4A File Offset: 0x004DAF4A
		public override string ServerConfigName
		{
			get
			{
				return "theconstant";
			}
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x004DCD51 File Offset: 0x004DAF51
		public WorldSeedOption_DontStarve()
		{
			base.SpecialSeedNames = new string[]
			{
				"constant",
				"theconstant",
				"eye4aneye",
				"eyeforaneye"
			};
			base.SpecialSeedValues = new int[0];
		}
	}
}
