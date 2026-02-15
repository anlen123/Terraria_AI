using System;
using System.Collections.Generic;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000420 RID: 1056
	public class PersonalityDatabase
	{
		// Token: 0x0600305D RID: 12381 RVA: 0x005B91FD File Offset: 0x005B73FD
		public PersonalityDatabase()
		{
			this._personalityProfiles = new Dictionary<int, PersonalityProfile>();
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x005B921B File Offset: 0x005B741B
		public void Register(int npcId, IShopPersonalityTrait trait)
		{
			if (!this._personalityProfiles.ContainsKey(npcId))
			{
				this._personalityProfiles[npcId] = new PersonalityProfile();
			}
			this._personalityProfiles[npcId].ShopModifiers.Add(trait);
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x005B9254 File Offset: 0x005B7454
		public void Register(IShopPersonalityTrait trait, params int[] npcIds)
		{
			for (int i = 0; i < npcIds.Length; i++)
			{
				this.Register(trait, new int[]
				{
					npcIds[i]
				});
			}
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x005B9284 File Offset: 0x005B7484
		public PersonalityProfile GetByNPCID(int npcId)
		{
			PersonalityProfile result;
			if (this._personalityProfiles.TryGetValue(npcId, out result))
			{
				return result;
			}
			return this._trashEntry;
		}

		// Token: 0x040056B3 RID: 22195
		private Dictionary<int, PersonalityProfile> _personalityProfiles;

		// Token: 0x040056B4 RID: 22196
		private PersonalityProfile _trashEntry = new PersonalityProfile();
	}
}
