using System;

namespace Terraria.GameContent.Personalities
{
	// Token: 0x02000421 RID: 1057
	public class PersonalityDatabasePopulator
	{
		// Token: 0x06003061 RID: 12385 RVA: 0x005B92A9 File Offset: 0x005B74A9
		public void Populate(PersonalityDatabase database)
		{
			this._currentDatabase = database;
			this.Populate_BiomePreferences(database);
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x005B92BC File Offset: 0x005B74BC
		private void Populate_BiomePreferences(PersonalityDatabase database)
		{
			OceanBiome biome = new OceanBiome();
			ForestBiome biome2 = new ForestBiome();
			SnowBiome biome3 = new SnowBiome();
			DesertBiome biome4 = new DesertBiome();
			JungleBiome biome5 = new JungleBiome();
			UndergroundBiome biome6 = new UndergroundBiome();
			HallowBiome biome7 = new HallowBiome();
			MushroomBiome biome8 = new MushroomBiome();
			AffectionLevel level = AffectionLevel.Love;
			AffectionLevel level2 = AffectionLevel.Like;
			AffectionLevel level3 = AffectionLevel.Dislike;
			AffectionLevel level4 = AffectionLevel.Hate;
			database.Register(22, new BiomePreferenceListTrait
			{
				{
					level2,
					biome2
				},
				{
					level3,
					biome
				}
			});
			database.Register(17, new BiomePreferenceListTrait
			{
				{
					level2,
					biome2
				},
				{
					level3,
					biome4
				}
			});
			database.Register(588, new BiomePreferenceListTrait
			{
				{
					level2,
					biome2
				},
				{
					level3,
					biome6
				}
			});
			database.Register(633, new BiomePreferenceListTrait
			{
				{
					level2,
					biome2
				},
				{
					level3,
					biome4
				}
			});
			database.Register(441, new BiomePreferenceListTrait
			{
				{
					level2,
					biome3
				},
				{
					level3,
					biome7
				}
			});
			database.Register(124, new BiomePreferenceListTrait
			{
				{
					level2,
					biome3
				},
				{
					level3,
					biome6
				}
			});
			database.Register(209, new BiomePreferenceListTrait
			{
				{
					level2,
					biome3
				},
				{
					level3,
					biome5
				}
			});
			database.Register(142, new BiomePreferenceListTrait
			{
				{
					level,
					biome3
				},
				{
					level4,
					biome4
				}
			});
			database.Register(207, new BiomePreferenceListTrait
			{
				{
					level2,
					biome4
				},
				{
					level3,
					biome2
				}
			});
			database.Register(19, new BiomePreferenceListTrait
			{
				{
					level2,
					biome4
				},
				{
					level3,
					biome3
				}
			});
			database.Register(178, new BiomePreferenceListTrait
			{
				{
					level2,
					biome4
				},
				{
					level3,
					biome5
				}
			});
			database.Register(20, new BiomePreferenceListTrait
			{
				{
					level2,
					biome5
				},
				{
					level3,
					biome4
				}
			});
			database.Register(228, new BiomePreferenceListTrait
			{
				{
					level2,
					biome5
				},
				{
					level3,
					biome7
				}
			});
			database.Register(227, new BiomePreferenceListTrait
			{
				{
					level2,
					biome5
				},
				{
					level3,
					biome2
				}
			});
			database.Register(369, new BiomePreferenceListTrait
			{
				{
					level2,
					biome
				},
				{
					level3,
					biome4
				}
			});
			database.Register(229, new BiomePreferenceListTrait
			{
				{
					level2,
					biome
				},
				{
					level3,
					biome6
				}
			});
			database.Register(353, new BiomePreferenceListTrait
			{
				{
					level2,
					biome
				},
				{
					level3,
					biome3
				}
			});
			database.Register(38, new BiomePreferenceListTrait
			{
				{
					level2,
					biome6
				},
				{
					level3,
					biome
				}
			});
			database.Register(107, new BiomePreferenceListTrait
			{
				{
					level2,
					biome6
				},
				{
					level3,
					biome5
				}
			});
			database.Register(54, new BiomePreferenceListTrait
			{
				{
					level2,
					biome6
				},
				{
					level3,
					biome7
				}
			});
			database.Register(108, new BiomePreferenceListTrait
			{
				{
					level2,
					biome7
				},
				{
					level3,
					biome
				}
			});
			database.Register(18, new BiomePreferenceListTrait
			{
				{
					level2,
					biome7
				},
				{
					level3,
					biome3
				}
			});
			database.Register(208, new BiomePreferenceListTrait
			{
				{
					level2,
					biome7
				},
				{
					level3,
					biome6
				}
			});
			database.Register(550, new BiomePreferenceListTrait
			{
				{
					level2,
					biome7
				},
				{
					level3,
					biome3
				}
			});
			database.Register(160, new BiomePreferenceListTrait
			{
				{
					level2,
					biome8
				}
			});
		}

		// Token: 0x040056B5 RID: 22197
		private PersonalityDatabase _currentDatabase;
	}
}
