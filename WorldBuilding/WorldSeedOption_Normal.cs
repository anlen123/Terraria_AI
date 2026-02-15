using System;
using System.Linq;

namespace Terraria.WorldBuilding
{
	// Token: 0x02000099 RID: 153
	public class WorldSeedOption_Normal : AWorldGenerationOption
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x004DCC37 File Offset: 0x004DAE37
		protected override string KeyName
		{
			get
			{
				return "Seed_Normal";
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06001700 RID: 5888 RVA: 0x000762F3 File Offset: 0x000744F3
		public override string ServerConfigName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x004DCC3E File Offset: 0x004DAE3E
		public WorldSeedOption_Normal()
		{
			base.SpecialSeedNames = new string[0];
			base.SpecialSeedValues = new int[0];
			AWorldGenerationOption.OnOptionStateChanged += this.UpdateDependentState;
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x004DCC6F File Offset: 0x004DAE6F
		private void UpdateDependentState(AWorldGenerationOption changed)
		{
			base.Enabled = WorldGenerationOptions.Options.All((AWorldGenerationOption x) => x == this || !x.Enabled);
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x004DCC90 File Offset: 0x004DAE90
		protected override void OnEnabledStateChanged()
		{
			if (!base.Enabled)
			{
				return;
			}
			foreach (AWorldGenerationOption aworldGenerationOption in WorldGenerationOptions.Options)
			{
				if (aworldGenerationOption != this)
				{
					aworldGenerationOption.Enabled = false;
				}
			}
		}
	}
}
