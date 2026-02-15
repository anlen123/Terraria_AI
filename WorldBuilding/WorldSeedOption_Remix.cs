using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x0200009F RID: 159
	public class WorldSeedOption_Remix : AWorldGenerationOption
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x004DCE33 File Offset: 0x004DB033
		protected override string KeyName
		{
			get
			{
				return "Seed_Remix";
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x004DCE3A File Offset: 0x004DB03A
		public override string ServerConfigName
		{
			get
			{
				return "remix";
			}
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x004DCE41 File Offset: 0x004DB041
		public WorldSeedOption_Remix()
		{
			base.SpecialSeedNames = new string[]
			{
				"dontdigup"
			};
			base.SpecialSeedValues = new int[0];
		}
	}
}
