using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x02000264 RID: 612
	public class ShopHelper
	{
		// Token: 0x0600238E RID: 9102 RVA: 0x0053F154 File Offset: 0x0053D354
		public ShopHelper()
		{
			this._database = new PersonalityDatabase();
			new PersonalityDatabasePopulator().Populate(this._database);
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x0053F1A8 File Offset: 0x0053D3A8
		public ShoppingSettings GetShoppingSettings(Player player, NPC npc)
		{
			ShoppingSettings result = new ShoppingSettings
			{
				PriceAdjustment = 1f,
				HappinessReport = ""
			};
			this._currentNPCBeingTalkedTo = npc;
			this._currentPlayerTalking = player;
			this.ProcessMood(player, npc);
			result.PriceAdjustment = this._currentPriceAdjustment;
			result.HappinessReport = this._currentHappiness;
			return result;
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x0053F208 File Offset: 0x0053D408
		private float GetSkeletonMerchantPrices(NPC npc)
		{
			float num = 1f;
			if (Main.moonPhase == 1 || Main.moonPhase == 7)
			{
				num = 1.1f;
			}
			if (Main.moonPhase == 2 || Main.moonPhase == 6)
			{
				num = 1.2f;
			}
			if (Main.moonPhase == 3 || Main.moonPhase == 5)
			{
				num = 1.3f;
			}
			if (Main.moonPhase == 4)
			{
				num = 1.4f;
			}
			if (Main.dayTime)
			{
				num += 0.1f;
			}
			return num;
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x0053F27C File Offset: 0x0053D47C
		private float GetTravelingMerchantPrices(NPC npc)
		{
			Vector2 value = npc.Center / 16f;
			Vector2 value2 = new Vector2((float)Main.spawnTileX, (float)Main.spawnTileY);
			float num = Vector2.Distance(value, value2) / (float)(Main.maxTilesX / 2);
			num = 1.5f - num;
			return (2f + num) / 3f;
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x0053F2D4 File Offset: 0x0053D4D4
		private void ProcessMood(Player player, NPC npc)
		{
			this._currentHappiness = "";
			this._currentPriceAdjustment = 1f;
			if (npc.loveStruck)
			{
				this._currentPriceAdjustment *= 0.9f;
			}
			if (Main.remixWorld)
			{
				return;
			}
			if (npc.type == 368)
			{
				return;
			}
			if (npc.type == 453)
			{
				return;
			}
			if (NPCID.Sets.IsTownPet[npc.type])
			{
				return;
			}
			if (this.IsNotReallyTownNPC(npc))
			{
				return;
			}
			if (this.RuinMoodIfHomeless(npc))
			{
				this._currentPriceAdjustment = 1000f;
			}
			else if (this.IsFarFromHome(npc))
			{
				this._currentPriceAdjustment = 1000f;
			}
			if (this.IsPlayerInEvilBiomes(player))
			{
				this._currentPriceAdjustment = 1000f;
			}
			int num;
			int num2;
			List<NPC> nearbyResidentNPCs = this.GetNearbyResidentNPCs(npc, out num, out num2);
			bool flag = true;
			bool flag2 = true;
			float num3 = 1.05f;
			if (npc.type == 663)
			{
				flag = false;
				num3 = 1f;
				if (num < 2 && num2 < 2)
				{
					this.AddHappinessReportText("HateLonely", null);
					this._currentPriceAdjustment = 1000f;
				}
			}
			if (flag2 && num > 3)
			{
				for (int i = 3; i < num; i++)
				{
					this._currentPriceAdjustment *= num3;
				}
				if (num > 6)
				{
					this.AddHappinessReportText("HateCrowded", null);
				}
				else
				{
					this.AddHappinessReportText("DislikeCrowded", null);
				}
			}
			if (flag && num <= 2 && num2 < 4)
			{
				this.AddHappinessReportText("LoveSpace", null);
				this._currentPriceAdjustment *= 0.95f;
			}
			bool[] array = new bool[(int)NPCID.Count];
			foreach (NPC npc2 in nearbyResidentNPCs)
			{
				array[npc2.type] = true;
			}
			HelperInfo info = new HelperInfo
			{
				player = player,
				npc = npc,
				NearbyNPCs = nearbyResidentNPCs,
				nearbyNPCsByType = array
			};
			foreach (IShopPersonalityTrait shopPersonalityTrait in this._database.GetByNPCID(npc.type).ShopModifiers)
			{
				shopPersonalityTrait.ModifyShopPrice(info, this);
			}
			new AllPersonalitiesModifier().ModifyShopPrice(info, this);
			if (this._currentHappiness == "")
			{
				this.AddHappinessReportText("Content", null);
			}
			this._currentPriceAdjustment = this.LimitAndRoundMultiplier(this._currentPriceAdjustment);
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x0053F554 File Offset: 0x0053D754
		private float LimitAndRoundMultiplier(float priceAdjustment)
		{
			priceAdjustment = MathHelper.Clamp(priceAdjustment, 0.75f, 1.5f);
			priceAdjustment = (float)Math.Round((double)(priceAdjustment * 100f)) / 100f;
			return priceAdjustment;
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x0053F57F File Offset: 0x0053D77F
		private static string BiomeNameByKey(string biomeNameKey)
		{
			return Language.GetTextValue("TownNPCMoodBiomes." + biomeNameKey);
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x0053F594 File Offset: 0x0053D794
		private void AddHappinessReportText(string textKeyInCategory, object substitutes = null)
		{
			string str = "TownNPCMood_" + NPCID.Search.GetName(this._currentNPCBeingTalkedTo.netID);
			if (this._currentNPCBeingTalkedTo.type == 633 && this._currentNPCBeingTalkedTo.altTexture == 2)
			{
				str += "Transformed";
			}
			string textValueWith = Language.GetTextValueWith(str + "." + textKeyInCategory, substitutes);
			this._currentHappiness = this._currentHappiness + textValueWith + " ";
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0053F617 File Offset: 0x0053D817
		public void LikeBiome(string nameKey)
		{
			this.AddHappinessReportText("LikeBiome", new
			{
				BiomeName = ShopHelper.BiomeNameByKey(nameKey)
			});
			this._currentPriceAdjustment *= 0.94f;
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0053F641 File Offset: 0x0053D841
		public void LoveBiome(string nameKey)
		{
			this.AddHappinessReportText("LoveBiome", new
			{
				BiomeName = ShopHelper.BiomeNameByKey(nameKey)
			});
			this._currentPriceAdjustment *= 0.88f;
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x0053F66B File Offset: 0x0053D86B
		public void DislikeBiome(string nameKey)
		{
			this.AddHappinessReportText("DislikeBiome", new
			{
				BiomeName = ShopHelper.BiomeNameByKey(nameKey)
			});
			this._currentPriceAdjustment *= 1.06f;
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0053F695 File Offset: 0x0053D895
		public void HateBiome(string nameKey)
		{
			this.AddHappinessReportText("HateBiome", new
			{
				BiomeName = ShopHelper.BiomeNameByKey(nameKey)
			});
			this._currentPriceAdjustment *= 1.12f;
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0053F6BF File Offset: 0x0053D8BF
		public void LikeNPC(int npcType)
		{
			this.AddHappinessReportText("LikeNPC", new
			{
				NPCName = NPC.GetFullnameByID(npcType)
			});
			this._currentPriceAdjustment *= 0.94f;
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0053F6E9 File Offset: 0x0053D8E9
		public void LoveNPCByTypeName(int npcType)
		{
			this.AddHappinessReportText("LoveNPC_" + NPCID.Search.GetName(npcType), new
			{
				NPCName = NPC.GetFullnameByID(npcType)
			});
			this._currentPriceAdjustment *= 0.88f;
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0053F723 File Offset: 0x0053D923
		public void LikePrincess()
		{
			this.AddHappinessReportText("LikeNPC_Princess", new
			{
				NPCName = NPC.GetFullnameByID(663)
			});
			this._currentPriceAdjustment *= 0.94f;
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0053F751 File Offset: 0x0053D951
		public void LoveNPC(int npcType)
		{
			this.AddHappinessReportText("LoveNPC", new
			{
				NPCName = NPC.GetFullnameByID(npcType)
			});
			this._currentPriceAdjustment *= 0.88f;
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0053F77B File Offset: 0x0053D97B
		public void DislikeNPC(int npcType)
		{
			this.AddHappinessReportText("DislikeNPC", new
			{
				NPCName = NPC.GetFullnameByID(npcType)
			});
			this._currentPriceAdjustment *= 1.06f;
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x0053F7A5 File Offset: 0x0053D9A5
		public void HateNPC(int npcType)
		{
			this.AddHappinessReportText("HateNPC", new
			{
				NPCName = NPC.GetFullnameByID(npcType)
			});
			this._currentPriceAdjustment *= 1.12f;
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0053F7D0 File Offset: 0x0053D9D0
		private List<NPC> GetNearbyResidentNPCs(NPC npc, out int npcsWithinHouse, out int npcsWithinVillage)
		{
			List<NPC> list = new List<NPC>();
			npcsWithinHouse = 0;
			npcsWithinVillage = 0;
			Vector2 value = new Vector2((float)npc.homeTileX, (float)npc.homeTileY);
			if (npc.homeless)
			{
				value = new Vector2(npc.Center.X / 16f, npc.Center.Y / 16f);
			}
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (i != npc.whoAmI)
				{
					NPC npc2 = Main.npc[i];
					if (npc2.active && npc2.townNPC && !this.IsNotReallyTownNPC(npc2) && !WorldGen.TownManager.CanNPCsLiveWithEachOther_ShopHelper(npc, npc2))
					{
						Vector2 value2 = new Vector2((float)npc2.homeTileX, (float)npc2.homeTileY);
						if (npc2.homeless)
						{
							value2 = npc2.Center / 16f;
						}
						float num = Vector2.Distance(value, value2);
						if (num < 25f)
						{
							list.Add(npc2);
							npcsWithinHouse++;
						}
						else if (num < 120f)
						{
							npcsWithinVillage++;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x0053F8E1 File Offset: 0x0053DAE1
		private bool RuinMoodIfHomeless(NPC npc)
		{
			if (npc.homeless)
			{
				this.AddHappinessReportText("NoHome", null);
			}
			return npc.homeless;
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0053F900 File Offset: 0x0053DB00
		private bool IsFarFromHome(NPC npc)
		{
			Vector2 value = new Vector2((float)npc.homeTileX, (float)npc.homeTileY);
			Vector2 value2 = new Vector2(npc.Center.X / 16f, npc.Center.Y / 16f);
			if (Vector2.Distance(value, value2) > 120f)
			{
				this.AddHappinessReportText("FarFromHome", null);
				return true;
			}
			return false;
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0053F968 File Offset: 0x0053DB68
		private bool IsPlayerInEvilBiomes(Player player)
		{
			for (int i = 0; i < this._dangerousBiomes.Length; i++)
			{
				AShoppingBiome ashoppingBiome = this._dangerousBiomes[i];
				if (ashoppingBiome.IsInBiome(player))
				{
					this.AddHappinessReportText("HateBiome", new
					{
						BiomeName = ShopHelper.BiomeNameByKey(ashoppingBiome.NameKey)
					});
					return true;
				}
			}
			return false;
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0053F9B8 File Offset: 0x0053DBB8
		private bool IsNotReallyTownNPC(NPC npc)
		{
			int type = npc.type;
			return type == 37 || type == 368 || type == 453;
		}

		// Token: 0x04004D75 RID: 19829
		public const float LowestPossiblePriceMultiplier = 0.75f;

		// Token: 0x04004D76 RID: 19830
		public const float MaxHappinessAchievementPriceMultiplier = 0.82f;

		// Token: 0x04004D77 RID: 19831
		public const float HighestPossiblePriceMultiplier = 1.5f;

		// Token: 0x04004D78 RID: 19832
		private string _currentHappiness;

		// Token: 0x04004D79 RID: 19833
		private float _currentPriceAdjustment;

		// Token: 0x04004D7A RID: 19834
		private NPC _currentNPCBeingTalkedTo;

		// Token: 0x04004D7B RID: 19835
		private Player _currentPlayerTalking;

		// Token: 0x04004D7C RID: 19836
		private PersonalityDatabase _database;

		// Token: 0x04004D7D RID: 19837
		private AShoppingBiome[] _dangerousBiomes = new AShoppingBiome[]
		{
			new CorruptionBiome(),
			new CrimsonBiome(),
			new DungeonBiome()
		};

		// Token: 0x04004D7E RID: 19838
		private const float likeValue = 0.94f;

		// Token: 0x04004D7F RID: 19839
		private const float dislikeValue = 1.06f;

		// Token: 0x04004D80 RID: 19840
		private const float loveValue = 0.88f;

		// Token: 0x04004D81 RID: 19841
		private const float hateValue = 1.12f;
	}
}
