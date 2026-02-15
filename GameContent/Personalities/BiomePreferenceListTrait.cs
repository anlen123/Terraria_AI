using System;
using System.Collections;
using System.Collections.Generic;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x0200041E RID: 1054
	public class BiomePreferenceListTrait : IShopPersonalityTrait, IEnumerable<BiomePreferenceListTrait.BiomePreference>, IEnumerable
	{
		// Token: 0x06003054 RID: 12372 RVA: 0x005B9084 File Offset: 0x005B7284
		public BiomePreferenceListTrait()
		{
			this._preferences = new List<BiomePreferenceListTrait.BiomePreference>();
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x005B9097 File Offset: 0x005B7297
		public void Add(BiomePreferenceListTrait.BiomePreference preference)
		{
			this._preferences.Add(preference);
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x005B90A5 File Offset: 0x005B72A5
		public void Add(AffectionLevel level, AShoppingBiome biome)
		{
			this._preferences.Add(new BiomePreferenceListTrait.BiomePreference(level, biome));
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x005B90BC File Offset: 0x005B72BC
		public void ModifyShopPrice(HelperInfo info, ShopHelper shopHelperInstance)
		{
			BiomePreferenceListTrait.BiomePreference biomePreference = null;
			for (int i = 0; i < this._preferences.Count; i++)
			{
				BiomePreferenceListTrait.BiomePreference biomePreference2 = this._preferences[i];
				if (biomePreference2.Biome.IsInBiome(info.player) && (biomePreference == null || biomePreference.Affection < biomePreference2.Affection))
				{
					biomePreference = biomePreference2;
				}
			}
			if (biomePreference != null)
			{
				this.ApplyPreference(biomePreference, info, shopHelperInstance);
			}
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x005B9120 File Offset: 0x005B7320
		private void ApplyPreference(BiomePreferenceListTrait.BiomePreference preference, HelperInfo info, ShopHelper shopHelperInstance)
		{
			string nameKey = preference.Biome.NameKey;
			AffectionLevel affection = preference.Affection;
			if (affection <= AffectionLevel.Dislike)
			{
				if (affection != AffectionLevel.Hate)
				{
					if (affection != AffectionLevel.Dislike)
					{
						return;
					}
					shopHelperInstance.DislikeBiome(nameKey);
					return;
				}
				else
				{
					shopHelperInstance.HateBiome(nameKey);
				}
			}
			else
			{
				if (affection == AffectionLevel.Like)
				{
					shopHelperInstance.LikeBiome(nameKey);
					return;
				}
				if (affection == AffectionLevel.Love)
				{
					shopHelperInstance.LoveBiome(nameKey);
					return;
				}
			}
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x005B9179 File Offset: 0x005B7379
		public IEnumerator<BiomePreferenceListTrait.BiomePreference> GetEnumerator()
		{
			return this._preferences.GetEnumerator();
		}

		// Token: 0x0600305A RID: 12378 RVA: 0x005B9179 File Offset: 0x005B7379
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._preferences.GetEnumerator();
		}

		// Token: 0x040056B0 RID: 22192
		private List<BiomePreferenceListTrait.BiomePreference> _preferences;

		// Token: 0x0200093B RID: 2363
		public class BiomePreference
		{
			// Token: 0x0600481E RID: 18462 RVA: 0x006CB516 File Offset: 0x006C9716
			public BiomePreference(AffectionLevel affection, AShoppingBiome biome)
			{
				this.Affection = affection;
				this.Biome = biome;
			}

			// Token: 0x04007506 RID: 29958
			public AffectionLevel Affection;

			// Token: 0x04007507 RID: 29959
			public AShoppingBiome Biome;
		}
	}
}
