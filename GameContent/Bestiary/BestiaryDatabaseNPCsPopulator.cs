using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200032E RID: 814
	public class BestiaryDatabaseNPCsPopulator
	{
		// Token: 0x060027E4 RID: 10212 RVA: 0x00568DFA File Offset: 0x00566FFA
		private BestiaryEntry FindEntryByNPCID(int npcNetId)
		{
			return this._currentDatabase.FindEntryByNPCID(npcNetId);
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x00568E08 File Offset: 0x00567008
		private BestiaryEntry Register(BestiaryEntry entry)
		{
			return this._currentDatabase.Register(entry);
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x00568E16 File Offset: 0x00567016
		private IBestiaryEntryFilter Register(IBestiaryEntryFilter filter)
		{
			return this._currentDatabase.Register(filter);
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x00568E24 File Offset: 0x00567024
		public void Populate(BestiaryDatabase database)
		{
			this._currentDatabase = database;
			this.AddEmptyEntries_CrittersAndEnemies_Automated();
			this.AddTownNPCs_Manual();
			this.AddNPCBiomeRelationships_Automated();
			this.AddNPCBiomeRelationships_Manual();
			this.AddNPCBiomeRelationships_AddDecorations_Automated();
			this.ModifyEntriesThatNeedIt();
			this.RegisterFilters();
			this.RegisterSortSteps();
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x00568E5D File Offset: 0x0056705D
		private void RegisterTestEntries()
		{
			this.Register(BestiaryEntry.Biome("Bestiary_Biomes.Hallow", "Images/UI/Bestiary/Biome_Hallow", new Func<bool>(BestiaryDatabaseNPCsPopulator.Conditions.ReachHardMode)));
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x00568E84 File Offset: 0x00567084
		private void RegisterSortSteps()
		{
			foreach (IBestiarySortStep sortStep in new List<IBestiarySortStep>
			{
				new SortingSteps.ByUnlockState(),
				new SortingSteps.ByBestiarySortingId(),
				new SortingSteps.Alphabetical(),
				new SortingSteps.ByNetId(),
				new SortingSteps.ByAttack(),
				new SortingSteps.ByDefense(),
				new SortingSteps.ByCoins(),
				new SortingSteps.ByHP(),
				new SortingSteps.ByBestiaryRarity()
			})
			{
				this._currentDatabase.Register(sortStep);
			}
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x00568F40 File Offset: 0x00567140
		private void RegisterFilters()
		{
			this.Register(new Filters.ByUnlockState());
			this.Register(new Filters.ByBoss());
			this.Register(new Filters.ByRareCreature());
			List<IBestiaryInfoElement> commonInfoElementsForFilters = BestiaryDatabaseNPCsPopulator.CommonTags.GetCommonInfoElementsForFilters();
			for (int i = 0; i < commonInfoElementsForFilters.Count; i++)
			{
				this.Register(new Filters.ByInfoElement(commonInfoElementsForFilters[i]));
			}
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x00568F9C File Offset: 0x0056719C
		private void ModifyEntriesThatNeedIt_NameOverride(int npcID, string newNameKey)
		{
			BestiaryEntry bestiaryEntry = this.FindEntryByNPCID(npcID);
			bestiaryEntry.Info.RemoveAll((IBestiaryInfoElement x) => x is NamePlateInfoElement);
			bestiaryEntry.Info.Add(new NamePlateInfoElement(newNameKey, npcID));
			bestiaryEntry.Icon = new UnlockableNPCEntryIcon(npcID, 0f, 0f, 0f, 0f, newNameKey);
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x00569010 File Offset: 0x00567210
		private void ModifyEntriesThatNeedIt()
		{
			this.FindEntryByNPCID(258).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom)
			});
			this.FindEntryByNPCID(-1).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption)
			});
			this.FindEntryByNPCID(81).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption)
			});
			this.FindEntryByNPCID(121).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption)
			});
			this.FindEntryByNPCID(7).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption)
			});
			this.FindEntryByNPCID(98).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption)
			});
			this.FindEntryByNPCID(6).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption)
			});
			this.FindEntryByNPCID(94).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption)
			});
			this.FindEntryByNPCID(173).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson)
			});
			this.FindEntryByNPCID(181).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson)
			});
			this.FindEntryByNPCID(183).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson)
			});
			this.FindEntryByNPCID(242).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson)
			});
			this.FindEntryByNPCID(241).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson)
			});
			this.FindEntryByNPCID(174).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson)
			});
			this.FindEntryByNPCID(240).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson)
			});
			this.FindEntryByNPCID(175).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle)
			});
			this.FindEntryByNPCID(153).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle)
			});
			this.FindEntryByNPCID(52).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle)
			});
			this.FindEntryByNPCID(58).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle)
			});
			this.FindEntryByNPCID(102).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns)
			});
			this.FindEntryByNPCID(157).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle)
			});
			this.FindEntryByNPCID(51).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle)
			});
			this.FindEntryByNPCID(169).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow)
			});
			this.FindEntryByNPCID(510).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert)
			});
			this.FindEntryByNPCID(69).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert)
			});
			this.FindEntryByNPCID(580).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert)
			});
			this.FindEntryByNPCID(581).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert)
			});
			this.FindEntryByNPCID(78).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert)
			});
			this.FindEntryByNPCID(79).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptDesert)
			});
			this.FindEntryByNPCID(630).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonDesert)
			});
			this.FindEntryByNPCID(80).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowDesert)
			});
			this.FindEntryByNPCID(533).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundBasedOnWorldEvilProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert)
			});
			this.FindEntryByNPCID(528).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert)
			});
			this.FindEntryByNPCID(529).AddTags(new IBestiaryInfoElement[]
			{
				new BestiaryPortraitBackgroundBasedOnWorldEvilProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert)
			});
			this._currentDatabase.ApplyPass(new BestiaryDatabase.BestiaryEntriesPass(this.TryGivingEntryFlavorTextIfItIsMissing));
			BestiaryEntry bestiaryEntry = this.FindEntryByNPCID(398);
			bestiaryEntry.Info.Add(new MoonLordPortraitBackgroundProviderBestiaryInfoElement());
			bestiaryEntry.Info.RemoveAll((IBestiaryInfoElement x) => x is NamePlateInfoElement);
			bestiaryEntry.Info.Add(new NamePlateInfoElement("Enemies.MoonLord", 398));
			bestiaryEntry.Icon = new UnlockableNPCEntryIcon(398, 0f, 0f, 0f, 0f, "Enemies.MoonLord");
			BestiaryEntry bestiaryEntry2 = this.FindEntryByNPCID(664);
			bestiaryEntry2.Info.RemoveAll((IBestiaryInfoElement x) => x is NPCKillCounterInfoElement);
			this.FindEntryByNPCID(687).Info.RemoveAll((IBestiaryInfoElement x) => x is NPCKillCounterInfoElement);
			this.ModifyEntriesThatNeedIt_NameOverride(637, "Friends.TownCat");
			this.ModifyEntriesThatNeedIt_NameOverride(638, "Friends.TownDog");
			this.ModifyEntriesThatNeedIt_NameOverride(656, "Friends.TownBunny");
			for (int i = 494; i <= 506; i++)
			{
				this.FindEntryByNPCID(i).UIInfoProvider = new SalamanderShellyDadUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[i]);
			}
			this.FindEntryByNPCID(534).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new IBestiaryUICollectionInfoProvider[]
			{
				new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[534], false),
				new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[441])
			});
			foreach (NPCStatsReportInfoElement npcstatsReportInfoElement in from x in this.FindEntryByNPCID(13).Info
			select x as NPCStatsReportInfoElement into x
			where x != null
			select x)
			{
				npcstatsReportInfoElement.OnRefreshStats += this.AdjustEaterOfWorldStats;
			}
			foreach (NPCStatsReportInfoElement npcstatsReportInfoElement2 in from x in this.FindEntryByNPCID(491).Info
			select x as NPCStatsReportInfoElement into x
			where x != null
			select x)
			{
				npcstatsReportInfoElement2.OnRefreshStats += this.AdjustPirateShipStats;
			}
			this.FindEntryByNPCID(395).Info.RemoveAll((IBestiaryInfoElement x) => x is BossBestiaryInfoElement);
			foreach (NPCStatsReportInfoElement npcstatsReportInfoElement3 in from x in bestiaryEntry2.Info
			select x as NPCStatsReportInfoElement into x
			where x != null
			select x)
			{
				npcstatsReportInfoElement3.OnRefreshStats += this.HideStats;
			}
			this.FindEntryByNPCID(68).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new IBestiaryUICollectionInfoProvider[]
			{
				new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[68], true),
				new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[35], true),
				new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[54])
			});
			this.FindEntryByNPCID(35).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new IBestiaryUICollectionInfoProvider[]
			{
				new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[35], true),
				new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[54])
			});
			this.FindEntryByNPCID(37).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new IBestiaryUICollectionInfoProvider[]
			{
				new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[37]),
				new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[54]),
				new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[35], true)
			});
			this.FindEntryByNPCID(565).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[565], true);
			this.FindEntryByNPCID(577).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[577], true);
			this.FindEntryByNPCID(551).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[551], true);
			this.FindEntryByNPCID(491).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[491], true);
			foreach (KeyValuePair<int, int> keyValuePair in new Dictionary<int, int>
			{
				{
					5,
					4
				},
				{
					267,
					266
				},
				{
					115,
					113
				},
				{
					116,
					113
				},
				{
					117,
					113
				},
				{
					139,
					134
				},
				{
					372,
					370
				},
				{
					658,
					657
				},
				{
					659,
					657
				},
				{
					660,
					657
				},
				{
					454,
					439
				},
				{
					521,
					439
				}
			})
			{
				int key = keyValuePair.Key;
				int value = keyValuePair.Value;
				this.FindEntryByNPCID(key).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[value], true);
			}
			foreach (KeyValuePair<int, int[]> keyValuePair2 in new Dictionary<int, int[]>
			{
				{
					443,
					new int[]
					{
						46
					}
				},
				{
					442,
					new int[]
					{
						74
					}
				},
				{
					592,
					new int[]
					{
						55
					}
				},
				{
					444,
					new int[]
					{
						356
					}
				},
				{
					601,
					new int[]
					{
						599
					}
				},
				{
					445,
					new int[]
					{
						361
					}
				},
				{
					446,
					new int[]
					{
						377
					}
				},
				{
					605,
					new int[]
					{
						604
					}
				},
				{
					447,
					new int[]
					{
						300
					}
				},
				{
					627,
					new int[]
					{
						626
					}
				},
				{
					613,
					new int[]
					{
						612
					}
				},
				{
					448,
					new int[]
					{
						357
					}
				},
				{
					539,
					new int[]
					{
						299,
						538
					}
				}
			})
			{
				this.FindEntryByNPCID(keyValuePair2.Key).UIInfoProvider = new GoldCritterUICollectionInfoProvider(keyValuePair2.Value, ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[keyValuePair2.Key]);
			}
			foreach (KeyValuePair<int, int> keyValuePair3 in new Dictionary<int, int>
			{
				{
					362,
					363
				},
				{
					364,
					365
				},
				{
					602,
					603
				},
				{
					608,
					609
				}
			})
			{
				this.FindEntryByNPCID(keyValuePair3.Key).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new IBestiaryUICollectionInfoProvider[]
				{
					new CritterUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[keyValuePair3.Key]),
					new CritterUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[keyValuePair3.Value])
				});
			}
			this.FindEntryByNPCID(4).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("eoc")
			});
			this.FindEntryByNPCID(13).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("eow")
			});
			this.FindEntryByNPCID(266).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("boc")
			});
			this.FindEntryByNPCID(113).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("wof")
			});
			this.FindEntryByNPCID(50).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("slime king")
			});
			this.FindEntryByNPCID(125).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("the twins")
			});
			this.FindEntryByNPCID(126).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("the twins")
			});
			this.FindEntryByNPCID(222).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("qb")
			});
			this.FindEntryByNPCID(222).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("bee queen")
			});
			this.FindEntryByNPCID(398).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("moonlord")
			});
			this.FindEntryByNPCID(398).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("cthulhu")
			});
			this.FindEntryByNPCID(398).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("ml")
			});
			this.FindEntryByNPCID(125).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("mech boss")
			});
			this.FindEntryByNPCID(126).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("mech boss")
			});
			this.FindEntryByNPCID(127).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("mech boss")
			});
			this.FindEntryByNPCID(134).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("mech boss")
			});
			this.FindEntryByNPCID(657).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("slime queen")
			});
			this.FindEntryByNPCID(636).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("eol")
			});
			this.FindEntryByNPCID(636).AddTags(new IBestiaryInfoElement[]
			{
				new SearchAliasInfoElement("fairy")
			});
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x00569FDC File Offset: 0x005681DC
		private void HideStats(NPCStatsReportInfoElement element)
		{
			element.HideStats = true;
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x00569FE5 File Offset: 0x005681E5
		private void AdjustEaterOfWorldStats(NPCStatsReportInfoElement element)
		{
			element.LifeMax *= NPC.GetEaterOfWorldsSegmentsCount();
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x00569FFC File Offset: 0x005681FC
		private void AdjustPirateShipStats(NPCStatsReportInfoElement element)
		{
			NPC npc = new NPC();
			int num = 4;
			npc.SetDefaults(492, new NPCSpawnParams
			{
				playerCountForMultiplayerDifficultyOverride = new int?(1)
			});
			element.LifeMax = num * npc.lifeMax;
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x0056A040 File Offset: 0x00568240
		private void TryGivingEntryFlavorTextIfItIsMissing(BestiaryEntry entry)
		{
			if (entry.Info.Any((IBestiaryInfoElement x) => x is FlavorTextBestiaryInfoElement))
			{
				return;
			}
			SpawnConditionBestiaryInfoElement spawnConditionBestiaryInfoElement = null;
			int? num = null;
			foreach (IBestiaryInfoElement bestiaryInfoElement in entry.Info)
			{
				BestiaryPortraitBackgroundProviderPreferenceInfoElement bestiaryPortraitBackgroundProviderPreferenceInfoElement = bestiaryInfoElement as BestiaryPortraitBackgroundProviderPreferenceInfoElement;
				if (bestiaryPortraitBackgroundProviderPreferenceInfoElement != null)
				{
					SpawnConditionBestiaryInfoElement spawnConditionBestiaryInfoElement2 = bestiaryPortraitBackgroundProviderPreferenceInfoElement.GetPreferredProvider() as SpawnConditionBestiaryInfoElement;
					if (spawnConditionBestiaryInfoElement2 != null)
					{
						spawnConditionBestiaryInfoElement = spawnConditionBestiaryInfoElement2;
						break;
					}
				}
				SpawnConditionBestiaryInfoElement spawnConditionBestiaryInfoElement3 = bestiaryInfoElement as SpawnConditionBestiaryInfoElement;
				if (spawnConditionBestiaryInfoElement3 != null)
				{
					int displayTextPriority = spawnConditionBestiaryInfoElement3.DisplayTextPriority;
					if (num != null)
					{
						int num2 = displayTextPriority;
						int? num3 = num;
						if (!(num2 >= num3.GetValueOrDefault() & num3 != null))
						{
							continue;
						}
					}
					spawnConditionBestiaryInfoElement = spawnConditionBestiaryInfoElement3;
					num = new int?(displayTextPriority);
				}
			}
			if (spawnConditionBestiaryInfoElement == null)
			{
				return;
			}
			string displayNameKey = spawnConditionBestiaryInfoElement.GetDisplayNameKey();
			string text = "Bestiary_BiomeText.biome_";
			string str = displayNameKey.Substring(displayNameKey.IndexOf('.') + 1);
			text += str;
			entry.Info.Add(new FlavorTextBestiaryInfoElement(text));
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x0056A168 File Offset: 0x00568368
		private void AddTownNPCs_Manual()
		{
			this.Register(BestiaryEntry.TownNPC(22));
			this.Register(BestiaryEntry.TownNPC(17));
			this.Register(BestiaryEntry.TownNPC(18));
			this.Register(BestiaryEntry.TownNPC(19));
			this.Register(BestiaryEntry.TownNPC(20));
			this.Register(BestiaryEntry.TownNPC(37));
			this.Register(BestiaryEntry.TownNPC(54));
			this.Register(BestiaryEntry.TownNPC(38));
			this.Register(BestiaryEntry.TownNPC(107));
			this.Register(BestiaryEntry.TownNPC(108));
			this.Register(BestiaryEntry.TownNPC(124));
			this.Register(BestiaryEntry.TownNPC(142));
			this.Register(BestiaryEntry.TownNPC(160));
			this.Register(BestiaryEntry.TownNPC(178));
			this.Register(BestiaryEntry.TownNPC(207));
			this.Register(BestiaryEntry.TownNPC(208));
			this.Register(BestiaryEntry.TownNPC(209));
			this.Register(BestiaryEntry.TownNPC(227));
			this.Register(BestiaryEntry.TownNPC(228));
			this.Register(BestiaryEntry.TownNPC(229));
			this.Register(BestiaryEntry.TownNPC(353));
			this.Register(BestiaryEntry.TownNPC(369));
			this.Register(BestiaryEntry.TownNPC(441));
			this.Register(BestiaryEntry.TownNPC(550));
			this.Register(BestiaryEntry.TownNPC(588));
			this.Register(BestiaryEntry.TownNPC(368));
			this.Register(BestiaryEntry.TownNPC(453));
			this.Register(BestiaryEntry.TownNPC(633));
			this.Register(BestiaryEntry.TownNPC(663));
			this.Register(BestiaryEntry.TownNPC(638));
			this.Register(BestiaryEntry.TownNPC(637));
			this.Register(BestiaryEntry.TownNPC(656));
			this.Register(BestiaryEntry.TownNPC(670));
			this.Register(BestiaryEntry.TownNPC(678));
			this.Register(BestiaryEntry.TownNPC(679));
			this.Register(BestiaryEntry.TownNPC(680));
			this.Register(BestiaryEntry.TownNPC(681));
			this.Register(BestiaryEntry.TownNPC(682));
			this.Register(BestiaryEntry.TownNPC(683));
			this.Register(BestiaryEntry.TownNPC(684));
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x0056A3FC File Offset: 0x005685FC
		private void AddMultiEntryNPCS_Manual()
		{
			this.Register(BestiaryEntry.Enemy(85)).Icon = new UnlockableNPCEntryIcon(85, 0f, 0f, 0f, 3f, null);
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x0056A42C File Offset: 0x0056862C
		private void AddEmptyEntries_CrittersAndEnemies_Automated()
		{
			HashSet<int> exclusions = BestiaryDatabaseNPCsPopulator.GetExclusions();
			foreach (KeyValuePair<int, NPC> keyValuePair in ContentSamples.NpcsByNetId)
			{
				if (!exclusions.Contains(keyValuePair.Key) && !keyValuePair.Value.isLikeATownNPC)
				{
					if (keyValuePair.Value.CountsAsACritter)
					{
						this.Register(BestiaryEntry.Critter(keyValuePair.Key));
					}
					else
					{
						this.Register(BestiaryEntry.Enemy(keyValuePair.Key));
					}
				}
			}
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x0056A4D0 File Offset: 0x005686D0
		private static HashSet<int> GetExclusions()
		{
			HashSet<int> hashSet = new HashSet<int>();
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, NPCID.Sets.NPCBestiaryDrawModifiers> keyValuePair in NPCID.Sets.NPCBestiaryDrawOffset)
			{
				if (keyValuePair.Value.Hide)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (int item in list)
			{
				hashSet.Add(item);
			}
			return hashSet;
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x0056A584 File Offset: 0x00568784
		private void AddNPCBiomeRelationships_Automated()
		{
			this.FindEntryByNPCID(357).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain
			});
			this.FindEntryByNPCID(448).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain
			});
			this.FindEntryByNPCID(606).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard
			});
			this.FindEntryByNPCID(211).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(377).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(446).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(595).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(596).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(597).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(598).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(599).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(600).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(601).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(612).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(613).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(25).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(30).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins
			});
			this.FindEntryByNPCID(665).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(33).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(112).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(666).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(300).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(355).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(358).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(447).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(610).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard
			});
			this.FindEntryByNPCID(210).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(261).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(402).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(403).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(485).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(486).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(487).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(359).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(410).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(604).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.WindyDay
			});
			this.FindEntryByNPCID(605).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.WindyDay
			});
			this.FindEntryByNPCID(218).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(361).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(404).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(445).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(626).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(627).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(2).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(74).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(190).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(191).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(192).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(193).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(194).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(217).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(297).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(298).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(671).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(672).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(673).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(674).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(675).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(356).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(360).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(655).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(653).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(654).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(442).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(444).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(669).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(677).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(676).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(582).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(583).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(584).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(585).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(1).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(59).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(138).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundHallow
			});
			this.FindEntryByNPCID(147).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(265).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(367).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(616).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(617).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(23).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor
			});
			this.FindEntryByNPCID(55).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(57).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(58).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(102).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(157).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(219).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(220).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(236).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(302).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween
			});
			this.FindEntryByNPCID(366).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(465).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(537).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(592).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(607).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert
			});
			this.FindEntryByNPCID(10).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(11).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(12).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(34).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(117).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(118).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(119).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(163).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SpiderNest
			});
			this.FindEntryByNPCID(164).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SpiderNest
			});
			this.FindEntryByNPCID(230).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain
			});
			this.FindEntryByNPCID(241).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(406).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(496).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(497).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(519).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(593).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain
			});
			this.FindEntryByNPCID(625).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(49).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(51).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(60).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(93).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(137).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundHallow
			});
			this.FindEntryByNPCID(184).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(204).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(224).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain
			});
			this.FindEntryByNPCID(259).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(299).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(317).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(318).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(378).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(393).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(494).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(495).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(513).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(514).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(515).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(538).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(539).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(580).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(587).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(16).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(71).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(81).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(183).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(67).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(70).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(75).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(239).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(267).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(288).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(394).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(408).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(428).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar
			});
			this.FindEntryByNPCID(43).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(56).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(72).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(141).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(185).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(374).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(375).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(661).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(388).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(602).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(603).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(115).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(232).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(258).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(409).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(462).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(516).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(42).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(46).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(47).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(69).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert
			});
			this.FindEntryByNPCID(231).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(235).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(247).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple
			});
			this.FindEntryByNPCID(248).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple
			});
			this.FindEntryByNPCID(303).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(304).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard
			});
			this.FindEntryByNPCID(337).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Christmas,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(354).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SpiderNest
			});
			this.FindEntryByNPCID(362).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(363).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(364).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(365).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(395).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(443).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(464).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(508).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(532).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(540).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Party,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(578).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(608).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert
			});
			this.FindEntryByNPCID(609).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert
			});
			this.FindEntryByNPCID(611).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(689).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(264).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(101).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(121).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(122).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(132).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(148).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow
			});
			this.FindEntryByNPCID(149).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow
			});
			this.FindEntryByNPCID(168).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(234).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(250).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain
			});
			this.FindEntryByNPCID(257).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(421).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.NebulaPillar
			});
			this.FindEntryByNPCID(470).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(472).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins
			});
			this.FindEntryByNPCID(478).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(546).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(581).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(615).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(256).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(133).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(221).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(252).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Pirates
			});
			this.FindEntryByNPCID(329).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(385).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(427).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar
			});
			this.FindEntryByNPCID(490).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(548).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(63).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(64).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(85).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(629).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(103).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(152).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(174).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(195).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(254).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom
			});
			this.FindEntryByNPCID(260).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(382).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(383).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(386).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(389).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(466).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(467).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(489).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(530).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(175).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(176).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(188).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(3).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(7).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(8).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(9).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(95).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(96).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(97).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(98).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(99).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(100).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(120).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundHallow
			});
			this.FindEntryByNPCID(150).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(151).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(153).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(154).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(158).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(161).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow
			});
			this.FindEntryByNPCID(186).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(187).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(189).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(223).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(233).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(251).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(319).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(320).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(321).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(331).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(332).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(338).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(339).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(340).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(341).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(342).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(350).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(381).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(492).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Pirates
			});
			this.FindEntryByNPCID(510).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(511).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(512).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(552).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(553).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(554).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(590).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(82).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(116).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(166).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(199).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple
			});
			this.FindEntryByNPCID(263).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(371).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(461).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(463).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(523).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(52).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(200).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(244).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain
			});
			this.FindEntryByNPCID(255).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom
			});
			this.FindEntryByNPCID(384).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(387).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(390).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(418).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(420).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.NebulaPillar
			});
			this.FindEntryByNPCID(460).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(468).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(524).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(525).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert
			});
			this.FindEntryByNPCID(526).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert
			});
			this.FindEntryByNPCID(527).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowUndergroundDesert
			});
			this.FindEntryByNPCID(536).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(566).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(567).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(53).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(169).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(301).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard
			});
			this.FindEntryByNPCID(690).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard
			});
			this.FindEntryByNPCID(391).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(405).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(423).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.NebulaPillar
			});
			this.FindEntryByNPCID(438).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(498).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(499).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(500).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(501).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(502).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(503).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(504).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(505).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(506).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(534).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(568).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(569).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(21).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(24).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(26).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins
			});
			this.FindEntryByNPCID(27).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins
			});
			this.FindEntryByNPCID(28).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins
			});
			this.FindEntryByNPCID(29).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins
			});
			this.FindEntryByNPCID(31).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(32).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(44).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(73).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(77).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(78).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert
			});
			this.FindEntryByNPCID(79).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptDesert
			});
			this.FindEntryByNPCID(630).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonDesert
			});
			this.FindEntryByNPCID(80).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowDesert
			});
			this.FindEntryByNPCID(104).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(111).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins
			});
			this.FindEntryByNPCID(140).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(159).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(162).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(196).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(198).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple
			});
			this.FindEntryByNPCID(201).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(202).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(203).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(212).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Pirates
			});
			this.FindEntryByNPCID(213).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Pirates
			});
			this.FindEntryByNPCID(242).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(269).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(270).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(272).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(273).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(275).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(276).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(277).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(278).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(279).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(280).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(281).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(282).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(283).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(284).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(285).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(286).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(287).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(294).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(295).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(296).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(310).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(311).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(312).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(313).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(316).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard
			});
			this.FindEntryByNPCID(326).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(415).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(449).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(450).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(451).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(452).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(471).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins
			});
			this.FindEntryByNPCID(482).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite
			});
			this.FindEntryByNPCID(572).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(573).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(143).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion
			});
			this.FindEntryByNPCID(144).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion
			});
			this.FindEntryByNPCID(145).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion
			});
			this.FindEntryByNPCID(155).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow
			});
			this.FindEntryByNPCID(271).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(274).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(314).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(352).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(379).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(509).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(555).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(556).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(557).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(61).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert
			});
			this.FindEntryByNPCID(110).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(206).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(214).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Pirates
			});
			this.FindEntryByNPCID(215).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Pirates
			});
			this.FindEntryByNPCID(216).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Pirates
			});
			this.FindEntryByNPCID(225).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain
			});
			this.FindEntryByNPCID(291).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(292).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(293).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(347).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(412).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(413).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(414).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(469).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(473).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(474).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(475).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundHallow
			});
			this.FindEntryByNPCID(476).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(483).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite
			});
			this.FindEntryByNPCID(586).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(62).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(131).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(165).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SpiderNest
			});
			this.FindEntryByNPCID(167).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(197).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(226).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple
			});
			this.FindEntryByNPCID(237).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(238).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SpiderNest
			});
			this.FindEntryByNPCID(480).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble
			});
			this.FindEntryByNPCID(528).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(529).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(289).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(693).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(694).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(439).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(440).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(533).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(170).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptIce
			});
			this.FindEntryByNPCID(171).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowIce
			});
			this.FindEntryByNPCID(179).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(180).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonIce
			});
			this.FindEntryByNPCID(181).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(205).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(411).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(424).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.NebulaPillar
			});
			this.FindEntryByNPCID(429).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar
			});
			this.FindEntryByNPCID(481).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble
			});
			this.FindEntryByNPCID(240).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(290).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(430).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(431).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(432).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(433).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(434).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(435).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(436).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(479).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(518).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(591).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(691).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard
			});
			this.FindEntryByNPCID(45).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(130).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(172).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(305).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(306).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(307).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(308).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(309).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(425).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar
			});
			this.FindEntryByNPCID(426).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar
			});
			this.FindEntryByNPCID(570).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(571).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(417).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(419).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(65).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(692).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(372).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(373).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(407).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(542).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(543).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(544).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(545).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(619).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(621).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(622).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(623).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(128).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(177).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(561).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(562).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(563).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(594).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.WindyDay
			});
			this.FindEntryByNPCID(253).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(129).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(6).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(173).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(399).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky
			});
			this.FindEntryByNPCID(416).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(531).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(83).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(84).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundHallow
			});
			this.FindEntryByNPCID(86).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(330).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(620).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(48).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky
			});
			this.FindEntryByNPCID(268).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(328).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(66).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(182).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(13).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(14).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(15).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(39).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(40).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(41).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(315).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(343).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(94).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(392).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(558).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(559).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(560).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(348).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(349).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(156).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(35).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(68).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(134).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(136).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(135).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(454).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(455).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(456).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(457).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(458).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(459).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(113).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(114).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
			this.FindEntryByNPCID(564).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(565).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(327).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(520).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian
			});
			this.FindEntryByNPCID(574).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(575).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(246).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple
			});
			this.FindEntryByNPCID(50).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(477).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse
			});
			this.FindEntryByNPCID(541).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(109).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(243).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Blizzard
			});
			this.FindEntryByNPCID(618).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon
			});
			this.FindEntryByNPCID(351).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(249).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple
			});
			this.FindEntryByNPCID(222).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(262).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(87).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky
			});
			this.FindEntryByNPCID(88).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky
			});
			this.FindEntryByNPCID(89).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky
			});
			this.FindEntryByNPCID(90).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky
			});
			this.FindEntryByNPCID(91).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky
			});
			this.FindEntryByNPCID(92).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky
			});
			this.FindEntryByNPCID(127).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(346).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(370).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(4).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(551).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(245).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple
			});
			this.FindEntryByNPCID(576).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(577).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(266).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
			});
			this.FindEntryByNPCID(325).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon
			});
			this.FindEntryByNPCID(344).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(125).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(126).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(549).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy
			});
			this.FindEntryByNPCID(345).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon
			});
			this.FindEntryByNPCID(668).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Blizzard
			});
			this.FindEntryByNPCID(422).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar
			});
			this.FindEntryByNPCID(493).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
			});
			this.FindEntryByNPCID(507).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.NebulaPillar
			});
			this.FindEntryByNPCID(517).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar
			});
			this.FindEntryByNPCID(491).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Pirates
			});
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x0056F71C File Offset: 0x0056D91C
		private void AddNPCBiomeRelationships_Manual()
		{
			this.FindEntryByNPCID(628).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.WindyDay
			});
			this.FindEntryByNPCID(-4).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(-3).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(-7).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(1).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(-10).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(-8).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(-9).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(-6).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(-5).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(-2).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			});
			this.FindEntryByNPCID(-1).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(81).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(121).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(7).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(8).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(9).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(98).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(99).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(100).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(6).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(94).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			this.FindEntryByNPCID(173).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(181).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(183).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(242).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(241).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(174).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(240).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson
			});
			this.FindEntryByNPCID(175).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle);
			this.FindEntryByNPCID(175).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(153).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(52).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime);
			this.FindEntryByNPCID(52).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(58).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(102).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(157).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(51).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			this.FindEntryByNPCID(169).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow);
			this.FindEntryByNPCID(169).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			this.FindEntryByNPCID(510).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			this.FindEntryByNPCID(510).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(511).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			this.FindEntryByNPCID(511).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(512).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			this.FindEntryByNPCID(512).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(69).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(580).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			this.FindEntryByNPCID(580).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(581).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			this.FindEntryByNPCID(581).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm
			});
			this.FindEntryByNPCID(78).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert
			});
			this.FindEntryByNPCID(79).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert
			});
			this.FindEntryByNPCID(630).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert
			});
			this.FindEntryByNPCID(80).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowUndergroundDesert
			});
			this.FindEntryByNPCID(533).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			this.FindEntryByNPCID(533).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert
			});
			this.FindEntryByNPCID(528).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			this.FindEntryByNPCID(528).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowUndergroundDesert
			});
			this.FindEntryByNPCID(529).Info.Remove(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			this.FindEntryByNPCID(529).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert
			});
			this.FindEntryByNPCID(624).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(5).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(139).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(484).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
			this.FindEntryByNPCID(317).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween
			});
			this.FindEntryByNPCID(318).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween
			});
			this.FindEntryByNPCID(320).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween
			});
			this.FindEntryByNPCID(321).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween
			});
			this.FindEntryByNPCID(319).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween
			});
			this.FindEntryByNPCID(324).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(322).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(323).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(302).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(521).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(332).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Christmas
			});
			this.FindEntryByNPCID(331).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Christmas
			});
			this.FindEntryByNPCID(335).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Christmas,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(336).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Christmas,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(333).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Christmas,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(334).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Christmas,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(535).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime
			});
			this.FindEntryByNPCID(614).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(225).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(224).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(250).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(632).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard
			});
			this.FindEntryByNPCID(631).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(634).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(635).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			this.FindEntryByNPCID(636).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(639).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(640).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(641).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(642).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(643).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(644).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(645).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(646).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(647).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(648).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(649).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(650).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(651).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(652).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(657).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(658).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(660).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(659).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(22).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(17).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(588).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(441).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow
			});
			this.FindEntryByNPCID(124).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow
			});
			this.FindEntryByNPCID(209).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow
			});
			this.FindEntryByNPCID(142).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Christmas
			});
			this.FindEntryByNPCID(207).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert
			});
			this.FindEntryByNPCID(19).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert
			});
			this.FindEntryByNPCID(178).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert
			});
			this.FindEntryByNPCID(20).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(228).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(227).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(369).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(229).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(353).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
			this.FindEntryByNPCID(38).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(107).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(54).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(108).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(18).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(208).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(550).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(633).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(663).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow
			});
			this.FindEntryByNPCID(160).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom
			});
			this.FindEntryByNPCID(637).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(638).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(656).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(670).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(678).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(679).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(680).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(681).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(682).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(683).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(684).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(687).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle
			});
			this.FindEntryByNPCID(368).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
			this.FindEntryByNPCID(37).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon
			});
			this.FindEntryByNPCID(453).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground
			});
			this.FindEntryByNPCID(664).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
			this.FindEntryByNPCID(688).Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
			});
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x00570C48 File Offset: 0x0056EE48
		private void AddNPCBiomeRelationships_AddDecorations_Automated()
		{
			foreach (KeyValuePair<int, NPC> keyValuePair in ContentSamples.NpcsByNetId)
			{
				BestiaryEntry bestiaryEntry = this.FindEntryByNPCID(keyValuePair.Key);
				if (bestiaryEntry.Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain))
				{
					if (bestiaryEntry.Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow))
					{
						bestiaryEntry.AddTags(new IBestiaryInfoElement[]
						{
							BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Blizzard
						});
					}
					else
					{
						bestiaryEntry.AddTags(new IBestiaryInfoElement[]
						{
							BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Rain
						});
					}
				}
				else
				{
					if (bestiaryEntry.Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse))
					{
						bestiaryEntry.AddTags(new IBestiaryInfoElement[]
						{
							BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.EclipseSun
						});
					}
					if (bestiaryEntry.Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime))
					{
						bestiaryEntry.AddTags(new IBestiaryInfoElement[]
						{
							BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon
						});
					}
					if (bestiaryEntry.Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime))
					{
						bestiaryEntry.AddTags(new IBestiaryInfoElement[]
						{
							BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Sun
						});
					}
					if (bestiaryEntry.Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon))
					{
						bestiaryEntry.AddTags(new IBestiaryInfoElement[]
						{
							BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.PumpkinMoon
						});
					}
					if (bestiaryEntry.Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon))
					{
						bestiaryEntry.AddTags(new IBestiaryInfoElement[]
						{
							BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.FrostMoon
						});
					}
					if (bestiaryEntry.Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor))
					{
						bestiaryEntry.AddTags(new IBestiaryInfoElement[]
						{
							BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon
						});
						bestiaryEntry.AddTags(new IBestiaryInfoElement[]
						{
							BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Meteor
						});
					}
				}
			}
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x00570E04 File Offset: 0x0056F004
		public void AddDropOverrides(BestiaryDatabase bestiaryDatabase)
		{
			this.FindEntryByNPCID(121).Info.AddRange(this.FindEntryByNPCID(81).Info.OfType<ItemDropBestiaryInfoElement>());
		}

		// Token: 0x040050F2 RID: 20722
		private BestiaryDatabase _currentDatabase;

		// Token: 0x020008AB RID: 2219
		public static class CommonTags
		{
			// Token: 0x060045C2 RID: 17858 RVA: 0x006C4588 File Offset: 0x006C2788
			public static List<IBestiaryInfoElement> GetCommonInfoElementsForFilters()
			{
				return new List<IBestiaryInfoElement>
				{
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Party,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.WindyDay,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SpiderNest,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptIce,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptDesert,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonIce,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonDesert,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundHallow,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowIce,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowDesert,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowUndergroundDesert,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.OldOnesArmy,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Pirates,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Martian,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.PumpkinMoon,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostMoon,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Halloween,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Christmas,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.NebulaPillar,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar
				};
			}

			// Token: 0x02000AD9 RID: 2777
			public static class SpawnConditions
			{
				// Token: 0x02000B17 RID: 2839
				public static class Invasions
				{
					// Token: 0x040078E4 RID: 30948
					public static SpawnConditionBestiaryInfoElement Goblins = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.Goblins", 49, "Images/MapBG1", null);

					// Token: 0x040078E5 RID: 30949
					public static SpawnConditionBestiaryInfoElement Pirates = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.Pirates", 50, "Images/MapBG11", null);

					// Token: 0x040078E6 RID: 30950
					public static SpawnConditionBestiaryInfoElement Martian = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.Martian", 53, "Images/MapBG1", new Color?(new Color(35, 40, 40)));

					// Token: 0x040078E7 RID: 30951
					public static SpawnConditionBestiaryInfoElement OldOnesArmy = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.OldOnesArmy", 55, "Images/MapBG1", null);

					// Token: 0x040078E8 RID: 30952
					public static SpawnConditionBestiaryInfoElement PumpkinMoon = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.PumpkinMoon", 51, "Images/MapBG1", new Color?(new Color(35, 40, 40)));

					// Token: 0x040078E9 RID: 30953
					public static SpawnConditionBestiaryInfoElement FrostMoon = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.FrostMoon", 52, "Images/MapBG12", new Color?(new Color(35, 40, 40)));

					// Token: 0x040078EA RID: 30954
					public static SpawnConditionBestiaryInfoElement FrostLegion = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.FrostLegion", 54, "Images/MapBG12", null);
				}

				// Token: 0x02000B18 RID: 2840
				public static class Events
				{
					// Token: 0x040078EB RID: 30955
					public static SpawnConditionBestiaryInfoElement SlimeRain = new SpawnConditionBestiaryInfoElement("Bestiary_Events.SlimeRain", 47, "Images/MapBG1", null)
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					// Token: 0x040078EC RID: 30956
					public static SpawnConditionBestiaryInfoElement WindyDay = new SpawnConditionBestiaryInfoElement("Bestiary_Events.WindyDay", 41, "Images/MapBG1", null)
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					// Token: 0x040078ED RID: 30957
					public static SpawnConditionBestiaryInfoElement BloodMoon = new SpawnConditionBestiaryInfoElement("Bestiary_Events.BloodMoon", 38, "Images/MapBG26", new Color?(new Color(200, 190, 180)))
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					// Token: 0x040078EE RID: 30958
					public static SpawnConditionBestiaryInfoElement Halloween = new SpawnConditionBestiaryInfoElement("Bestiary_Events.Halloween", 45, "Images/MapBG1", null)
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					// Token: 0x040078EF RID: 30959
					public static SpawnConditionBestiaryOverlayInfoElement Rain = new SpawnConditionBestiaryOverlayInfoElement("Bestiary_Events.Rain", 40, null, null)
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					// Token: 0x040078F0 RID: 30960
					public static SpawnConditionBestiaryInfoElement Christmas = new SpawnConditionBestiaryInfoElement("Bestiary_Events.Christmas", 46, "Images/MapBG12", null)
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					// Token: 0x040078F1 RID: 30961
					public static SpawnConditionBestiaryInfoElement Eclipse = new SpawnConditionBestiaryInfoElement("Bestiary_Events.Eclipse", 39, "Images/MapBG1", new Color?(new Color(60, 30, 0)))
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					// Token: 0x040078F2 RID: 30962
					public static SpawnConditionBestiaryInfoElement Party = new SpawnConditionBestiaryInfoElement("Bestiary_Events.Party", 48, "Images/MapBG1", null)
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					// Token: 0x040078F3 RID: 30963
					public static SpawnConditionBestiaryOverlayInfoElement Blizzard = new SpawnConditionBestiaryOverlayInfoElement("Bestiary_Events.Blizzard", 42, null, null)
					{
						DisplayTextPriority = 1,
						HideInPortraitInfo = true,
						OrderPriority = -2f
					};

					// Token: 0x040078F4 RID: 30964
					public static SpawnConditionBestiaryOverlayInfoElement Sandstorm = new SpawnConditionBestiaryOverlayInfoElement("Bestiary_Events.Sandstorm", 43, "Images/MapBGOverlay1", new Color?(Color.White))
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};
				}

				// Token: 0x02000B19 RID: 2841
				public static class Biomes
				{
					// Token: 0x040078F5 RID: 30965
					public static SpawnConditionBestiaryInfoElement TheCorruption = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheCorruption", 7, "Images/MapBG6", new Color?(new Color(200, 200, 200)));

					// Token: 0x040078F6 RID: 30966
					public static SpawnConditionBestiaryInfoElement TheCrimson = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Crimson", 12, "Images/MapBG7", new Color?(new Color(200, 200, 200)));

					// Token: 0x040078F7 RID: 30967
					public static SpawnConditionBestiaryInfoElement Surface = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Surface", 0, "Images/MapBG1", null);

					// Token: 0x040078F8 RID: 30968
					public static SpawnConditionBestiaryInfoElement Graveyard = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Graveyard", 35, "Images/MapBG27", null);

					// Token: 0x040078F9 RID: 30969
					public static SpawnConditionBestiaryInfoElement UndergroundJungle = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundJungle", 23, "Images/MapBG13", null);

					// Token: 0x040078FA RID: 30970
					public static SpawnConditionBestiaryInfoElement TheUnderworld = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheUnderworld", 33, "Images/MapBG3", null);

					// Token: 0x040078FB RID: 30971
					public static SpawnConditionBestiaryInfoElement TheDungeon = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheDungeon", 32, "Images/MapBG5", null);

					// Token: 0x040078FC RID: 30972
					public static SpawnConditionBestiaryInfoElement Underground = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Underground", 1, "Images/MapBG2", null);

					// Token: 0x040078FD RID: 30973
					public static SpawnConditionBestiaryInfoElement TheHallow = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheHallow", 17, "Images/MapBG8", null);

					// Token: 0x040078FE RID: 30974
					public static SpawnConditionBestiaryInfoElement UndergroundMushroom = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundMushroom", 25, "Images/MapBG21", null);

					// Token: 0x040078FF RID: 30975
					public static SpawnConditionBestiaryInfoElement Jungle = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Jungle", 22, "Images/MapBG9", null);

					// Token: 0x04007900 RID: 30976
					public static SpawnConditionBestiaryInfoElement Caverns = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Caverns", 2, "Images/MapBG32", null);

					// Token: 0x04007901 RID: 30977
					public static SpawnConditionBestiaryInfoElement UndergroundSnow = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundSnow", 6, "Images/MapBG4", null);

					// Token: 0x04007902 RID: 30978
					public static SpawnConditionBestiaryInfoElement Ocean = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Ocean", 28, "Images/MapBG11", null);

					// Token: 0x04007903 RID: 30979
					public static SpawnConditionBestiaryInfoElement SurfaceMushroom = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.SurfaceMushroom", 24, "Images/MapBG20", null);

					// Token: 0x04007904 RID: 30980
					public static SpawnConditionBestiaryInfoElement UndergroundDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundDesert", 4, "Images/MapBG15", null);

					// Token: 0x04007905 RID: 30981
					public static SpawnConditionBestiaryInfoElement Snow = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Snow", 5, "Images/MapBG12", null);

					// Token: 0x04007906 RID: 30982
					public static SpawnConditionBestiaryInfoElement Desert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Desert", 3, "Images/MapBG10", null);

					// Token: 0x04007907 RID: 30983
					public static SpawnConditionBestiaryInfoElement Meteor = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Meteor", 44, "Images/MapBG1", new Color?(new Color(35, 40, 40)));

					// Token: 0x04007908 RID: 30984
					public static SpawnConditionBestiaryInfoElement Oasis = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Oasis", 27, "Images/MapBG10", null);

					// Token: 0x04007909 RID: 30985
					public static SpawnConditionBestiaryInfoElement SpiderNest = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.SpiderNest", 34, "Images/MapBG19", null);

					// Token: 0x0400790A RID: 30986
					public static SpawnConditionBestiaryInfoElement TheTemple = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheTemple", 31, "Images/MapBG14", null);

					// Token: 0x0400790B RID: 30987
					public static SpawnConditionBestiaryInfoElement CorruptUndergroundDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CorruptUndergroundDesert", 10, "Images/MapBG40", null);

					// Token: 0x0400790C RID: 30988
					public static SpawnConditionBestiaryInfoElement CrimsonUndergroundDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CrimsonUndergroundDesert", 15, "Images/MapBG41", null);

					// Token: 0x0400790D RID: 30989
					public static SpawnConditionBestiaryInfoElement HallowUndergroundDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.HallowUndergroundDesert", 20, "Images/MapBG42", null);

					// Token: 0x0400790E RID: 30990
					public static SpawnConditionBestiaryInfoElement CorruptDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CorruptDesert", 9, "Images/MapBG37", null);

					// Token: 0x0400790F RID: 30991
					public static SpawnConditionBestiaryInfoElement CrimsonDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CrimsonDesert", 14, "Images/MapBG38", null);

					// Token: 0x04007910 RID: 30992
					public static SpawnConditionBestiaryInfoElement HallowDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.HallowDesert", 19, "Images/MapBG39", null);

					// Token: 0x04007911 RID: 30993
					public static SpawnConditionBestiaryInfoElement Granite = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Granite", 30, "Images/MapBG17", new Color?(new Color(100, 100, 100)));

					// Token: 0x04007912 RID: 30994
					public static SpawnConditionBestiaryInfoElement UndergroundCorruption = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundCorruption", 8, "Images/MapBG23", null);

					// Token: 0x04007913 RID: 30995
					public static SpawnConditionBestiaryInfoElement UndergroundCrimson = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundCrimson", 13, "Images/MapBG24", null);

					// Token: 0x04007914 RID: 30996
					public static SpawnConditionBestiaryInfoElement UndergroundHallow = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundHallow", 18, "Images/MapBG22", null);

					// Token: 0x04007915 RID: 30997
					public static SpawnConditionBestiaryInfoElement Marble = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Marble", 29, "Images/MapBG18", null);

					// Token: 0x04007916 RID: 30998
					public static SpawnConditionBestiaryInfoElement CorruptIce = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CorruptIce", 11, "Images/MapBG34", new Color?(new Color(200, 200, 200)));

					// Token: 0x04007917 RID: 30999
					public static SpawnConditionBestiaryInfoElement HallowIce = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.HallowIce", 21, "Images/MapBG36", new Color?(new Color(200, 200, 200)));

					// Token: 0x04007918 RID: 31000
					public static SpawnConditionBestiaryInfoElement CrimsonIce = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CrimsonIce", 16, "Images/MapBG35", new Color?(new Color(200, 200, 200)));

					// Token: 0x04007919 RID: 31001
					public static SpawnConditionBestiaryInfoElement Sky = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Sky", 26, "Images/MapBG33", null);

					// Token: 0x0400791A RID: 31002
					public static SpawnConditionBestiaryInfoElement NebulaPillar = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.NebulaPillar", 58, "Images/MapBG28", null);

					// Token: 0x0400791B RID: 31003
					public static SpawnConditionBestiaryInfoElement SolarPillar = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.SolarPillar", 56, "Images/MapBG29", null);

					// Token: 0x0400791C RID: 31004
					public static SpawnConditionBestiaryInfoElement VortexPillar = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.VortexPillar", 57, "Images/MapBG30", null);

					// Token: 0x0400791D RID: 31005
					public static SpawnConditionBestiaryInfoElement StardustPillar = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.StardustPillar", 59, "Images/MapBG31", null);
				}

				// Token: 0x02000B1A RID: 2842
				public static class Times
				{
					// Token: 0x0400791E RID: 31006
					public static SpawnConditionBestiaryInfoElement DayTime = new SpawnConditionBestiaryInfoElement("Bestiary_Times.DayTime", 36, null, null)
					{
						DisplayTextPriority = -1,
						OrderPriority = -1f
					};

					// Token: 0x0400791F RID: 31007
					public static SpawnConditionBestiaryInfoElement NightTime = new SpawnConditionBestiaryInfoElement("Bestiary_Times.NightTime", 37, "Images/MapBG1", new Color?(new Color(35, 40, 40)))
					{
						DisplayTextPriority = -1,
						OrderPriority = -1f
					};
				}

				// Token: 0x02000B1B RID: 2843
				public static class Visuals
				{
					// Token: 0x04007920 RID: 31008
					public static SpawnConditionDecorativeOverlayInfoElement Sun = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay3", new Color?(Color.White))
					{
						DisplayPriority = 1f
					};

					// Token: 0x04007921 RID: 31009
					public static SpawnConditionDecorativeOverlayInfoElement Moon = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay4", new Color?(Color.White))
					{
						DisplayPriority = 1f
					};

					// Token: 0x04007922 RID: 31010
					public static SpawnConditionDecorativeOverlayInfoElement EclipseSun = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay5", new Color?(Color.White))
					{
						DisplayPriority = 1f
					};

					// Token: 0x04007923 RID: 31011
					public static SpawnConditionDecorativeOverlayInfoElement PumpkinMoon = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay8", new Color?(Color.White))
					{
						DisplayPriority = 1f
					};

					// Token: 0x04007924 RID: 31012
					public static SpawnConditionDecorativeOverlayInfoElement FrostMoon = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay9", new Color?(Color.White))
					{
						DisplayPriority = 1f
					};

					// Token: 0x04007925 RID: 31013
					public static SpawnConditionDecorativeOverlayInfoElement Meteor = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay7", new Color?(Color.White))
					{
						DisplayPriority = 1f
					};

					// Token: 0x04007926 RID: 31014
					public static SpawnConditionDecorativeOverlayInfoElement Rain = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay2", new Color?(new Color(200, 200, 200)))
					{
						DisplayPriority = 1f
					};

					// Token: 0x04007927 RID: 31015
					public static SpawnConditionDecorativeOverlayInfoElement Blizzard = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay6", new Color?(Color.White))
					{
						DisplayPriority = 1f
					};
				}
			}
		}

		// Token: 0x020008AC RID: 2220
		public static class Conditions
		{
			// Token: 0x060045C3 RID: 17859 RVA: 0x004DD56B File Offset: 0x004DB76B
			public static bool ReachHardMode()
			{
				return Main.hardMode;
			}
		}

		// Token: 0x020008AD RID: 2221
		public static class CrownosIconIndexes
		{
			// Token: 0x040072AA RID: 29354
			public const int Surface = 0;

			// Token: 0x040072AB RID: 29355
			public const int Underground = 1;

			// Token: 0x040072AC RID: 29356
			public const int Cave = 2;

			// Token: 0x040072AD RID: 29357
			public const int Desert = 3;

			// Token: 0x040072AE RID: 29358
			public const int UndergroundDesert = 4;

			// Token: 0x040072AF RID: 29359
			public const int Snow = 5;

			// Token: 0x040072B0 RID: 29360
			public const int UndergroundIce = 6;

			// Token: 0x040072B1 RID: 29361
			public const int Corruption = 7;

			// Token: 0x040072B2 RID: 29362
			public const int CorruptionUnderground = 8;

			// Token: 0x040072B3 RID: 29363
			public const int CorruptionDesert = 9;

			// Token: 0x040072B4 RID: 29364
			public const int CorruptionUndergroundDesert = 10;

			// Token: 0x040072B5 RID: 29365
			public const int CorruptionIce = 11;

			// Token: 0x040072B6 RID: 29366
			public const int Crimson = 12;

			// Token: 0x040072B7 RID: 29367
			public const int CrimsonUnderground = 13;

			// Token: 0x040072B8 RID: 29368
			public const int CrimsonDesert = 14;

			// Token: 0x040072B9 RID: 29369
			public const int CrimsonUndergroundDesert = 15;

			// Token: 0x040072BA RID: 29370
			public const int CrimsonIce = 16;

			// Token: 0x040072BB RID: 29371
			public const int Hallow = 17;

			// Token: 0x040072BC RID: 29372
			public const int HallowUnderground = 18;

			// Token: 0x040072BD RID: 29373
			public const int HallowDesert = 19;

			// Token: 0x040072BE RID: 29374
			public const int HallowUndergroundDesert = 20;

			// Token: 0x040072BF RID: 29375
			public const int HallowIce = 21;

			// Token: 0x040072C0 RID: 29376
			public const int Jungle = 22;

			// Token: 0x040072C1 RID: 29377
			public const int UndergroundJungle = 23;

			// Token: 0x040072C2 RID: 29378
			public const int SurfaceMushroom = 24;

			// Token: 0x040072C3 RID: 29379
			public const int UndergroundMushroom = 25;

			// Token: 0x040072C4 RID: 29380
			public const int Sky = 26;

			// Token: 0x040072C5 RID: 29381
			public const int Oasis = 27;

			// Token: 0x040072C6 RID: 29382
			public const int Ocean = 28;

			// Token: 0x040072C7 RID: 29383
			public const int Marble = 29;

			// Token: 0x040072C8 RID: 29384
			public const int Granite = 30;

			// Token: 0x040072C9 RID: 29385
			public const int JungleTemple = 31;

			// Token: 0x040072CA RID: 29386
			public const int Dungeon = 32;

			// Token: 0x040072CB RID: 29387
			public const int Underworld = 33;

			// Token: 0x040072CC RID: 29388
			public const int SpiderNest = 34;

			// Token: 0x040072CD RID: 29389
			public const int Graveyard = 35;

			// Token: 0x040072CE RID: 29390
			public const int Day = 36;

			// Token: 0x040072CF RID: 29391
			public const int Night = 37;

			// Token: 0x040072D0 RID: 29392
			public const int BloodMoon = 38;

			// Token: 0x040072D1 RID: 29393
			public const int Eclipse = 39;

			// Token: 0x040072D2 RID: 29394
			public const int Rain = 40;

			// Token: 0x040072D3 RID: 29395
			public const int WindyDay = 41;

			// Token: 0x040072D4 RID: 29396
			public const int Blizzard = 42;

			// Token: 0x040072D5 RID: 29397
			public const int Sandstorm = 43;

			// Token: 0x040072D6 RID: 29398
			public const int Meteor = 44;

			// Token: 0x040072D7 RID: 29399
			public const int Halloween = 45;

			// Token: 0x040072D8 RID: 29400
			public const int Christmas = 46;

			// Token: 0x040072D9 RID: 29401
			public const int SlimeRain = 47;

			// Token: 0x040072DA RID: 29402
			public const int Party = 48;

			// Token: 0x040072DB RID: 29403
			public const int GoblinInvasion = 49;

			// Token: 0x040072DC RID: 29404
			public const int PirateInvasion = 50;

			// Token: 0x040072DD RID: 29405
			public const int PumpkinMoon = 51;

			// Token: 0x040072DE RID: 29406
			public const int FrostMoon = 52;

			// Token: 0x040072DF RID: 29407
			public const int AlienInvasion = 53;

			// Token: 0x040072E0 RID: 29408
			public const int FrostLegion = 54;

			// Token: 0x040072E1 RID: 29409
			public const int OldOnesArmy = 55;

			// Token: 0x040072E2 RID: 29410
			public const int SolarTower = 56;

			// Token: 0x040072E3 RID: 29411
			public const int VortexTower = 57;

			// Token: 0x040072E4 RID: 29412
			public const int NebulaTower = 58;

			// Token: 0x040072E5 RID: 29413
			public const int StardustTower = 59;

			// Token: 0x040072E6 RID: 29414
			public const int Hardmode = 60;

			// Token: 0x040072E7 RID: 29415
			public const int ItemSpawn = 61;
		}
	}
}
