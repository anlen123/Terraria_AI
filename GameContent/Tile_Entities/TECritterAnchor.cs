using System;
using Terraria.DataStructures;
using Terraria.GameContent.LeashedEntities;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x02000411 RID: 1041
	public class TECritterAnchor : TELeashedEntityAnchorWithItem
	{
		// Token: 0x06002FAE RID: 12206 RVA: 0x005B41E2 File Offset: 0x005B23E2
		public TECritterAnchor()
		{
			this.type = TECritterAnchor._myEntityID;
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x005B41F5 File Offset: 0x005B23F5
		public override void RegisterTileEntityID(int assignedID)
		{
			this.type = (TECritterAnchor._myEntityID = (byte)assignedID);
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x005B4208 File Offset: 0x005B2408
		public override bool IsTileValidForEntity(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile.active() && tile.type == 724;
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x005B4239 File Offset: 0x005B2439
		public override TileEntity GenerateInstance()
		{
			return new TECritterAnchor();
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x005B4240 File Offset: 0x005B2440
		public static void Kill(int x, int y)
		{
			TileEntity.Kill(x, y, (int)TECritterAnchor._myEntityID);
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x005B424E File Offset: 0x005B244E
		public static int Hook_AfterPlacement(int x, int y, int type, int style, int direction, int alternate)
		{
			return TELeashedEntityAnchorWithItem.PlaceFromPlayerPlacementHook(x, y, (int)TECritterAnchor._myEntityID);
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x005B425C File Offset: 0x005B245C
		public override bool FitsItem(int itemType)
		{
			return ContentSamples.ItemsByType[itemType].makeNPC > 0;
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x005B4271 File Offset: 0x005B2471
		public override LeashedEntity CreateLeashedEntity()
		{
			if (this.itemType <= 0)
			{
				return null;
			}
			LeashedCritter leashedCritter = (LeashedCritter)TECritterAnchor.GetLeashedCritterPrototype(this.itemType).NewInstance();
			leashedCritter.SetDefaults(this.itemType);
			return leashedCritter;
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x005B42A0 File Offset: 0x005B24A0
		static TECritterAnchor()
		{
			TECritterAnchor.SetPrototypeCollection(FlyerLeashedCritter.Prototype, new int[]
			{
				444,
				653,
				661
			});
			TECritterAnchor.SetPrototypeCollection(NormalButterflyLeashedCritter.Prototype, new int[]
			{
				356
			});
			TECritterAnchor.SetPrototypeCollection(EmpressButterflyLeashedCritter.Prototype, new int[]
			{
				661
			});
			TECritterAnchor.SetPrototypeCollection(HellButterflyLeashedCritter.Prototype, new int[]
			{
				653
			});
			TECritterAnchor.SetPrototypeCollection(FireflyLeashedCritter.Prototype, new int[]
			{
				355,
				358,
				654
			});
			TECritterAnchor.SetPrototypeCollection(ShimmerFlyLeashedCritter.Prototype, new int[]
			{
				677
			});
			TECritterAnchor.SetPrototypeCollection(DragonflyLeashedCritter.Prototype, new int[]
			{
				595,
				596,
				601,
				597,
				598,
				599,
				600
			});
			TECritterAnchor.SetPrototypeCollection(CrawlingFlyLeashedCritter.Prototype, new int[]
			{
				604,
				605,
				669
			});
			TECritterAnchor.SetPrototypeCollection(FairyLeashedCritter.Prototype, new int[]
			{
				585,
				584,
				583
			});
			TECritterAnchor.SetPrototypeCollection(CrawlerLeashedCritter.Prototype, new int[]
			{
				357,
				448,
				484,
				485,
				486,
				487,
				606,
				616,
				617
			});
			TECritterAnchor.SetPrototypeCollection(SnailLeashedCritter.Prototype, new int[]
			{
				359,
				360,
				655
			});
			TECritterAnchor.SetPrototypeCollection(RunnerLeashedCritter.Prototype, new int[]
			{
				300,
				447,
				610
			});
			TECritterAnchor.SetPrototypeCollection(BirdLeashedCritter.Prototype, new int[]
			{
				74,
				297,
				298,
				442,
				611,
				671,
				672,
				673,
				675,
				674
			});
			TECritterAnchor.SetPrototypeCollection(WaterfowlLeashedCritter.Prototype, new int[]
			{
				362,
				364,
				602,
				608
			});
			TECritterAnchor.SetPrototypeCollection(FishLeashedCritter.Prototype, new int[]
			{
				55,
				592,
				607,
				626,
				627,
				688
			});
			TECritterAnchor.SetPrototypeCollection(JumperLeashedCritter.Prototype, new int[]
			{
				377,
				446
			});
			TECritterAnchor.SetPrototypeCollection(WaterStriderLeashedCritter.Prototype, new int[]
			{
				612,
				613
			});
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x005B4494 File Offset: 0x005B2694
		public static void SetPrototypeCollection(LeashedCritter instance, params int[] targetIds)
		{
			foreach (int num in targetIds)
			{
				TECritterAnchor.CritterPrototypes[num] = instance;
			}
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x005B44BD File Offset: 0x005B26BD
		public static LeashedCritter GetLeashedCritterPrototype(int itemType)
		{
			return TECritterAnchor.CritterPrototypes[(int)ContentSamples.ItemsByType[itemType].makeNPC];
		}

		// Token: 0x0400566D RID: 22125
		private static byte _myEntityID;

		// Token: 0x0400566E RID: 22126
		public static LeashedCritter[] CritterPrototypes = NPCID.Sets.Factory.CreateCustomSet<LeashedCritter>(WalkerLeashedCritter.Prototype, new object[0]);
	}
}
