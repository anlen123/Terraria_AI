using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.Biomes.CaveHouse;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x0200050C RID: 1292
	public class CaveHouseBiome : MicroBiome
	{
		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x0600362D RID: 13869 RVA: 0x006239D8 File Offset: 0x00621BD8
		// (set) Token: 0x0600362E RID: 13870 RVA: 0x006239E0 File Offset: 0x00621BE0
		[JsonProperty]
		public double IceChestChance { get; set; }

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x0600362F RID: 13871 RVA: 0x006239E9 File Offset: 0x00621BE9
		// (set) Token: 0x06003630 RID: 13872 RVA: 0x006239F1 File Offset: 0x00621BF1
		[JsonProperty]
		public double JungleChestChance { get; set; }

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06003631 RID: 13873 RVA: 0x006239FA File Offset: 0x00621BFA
		// (set) Token: 0x06003632 RID: 13874 RVA: 0x00623A02 File Offset: 0x00621C02
		[JsonProperty]
		public double GoldChestChance { get; set; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06003633 RID: 13875 RVA: 0x00623A0B File Offset: 0x00621C0B
		// (set) Token: 0x06003634 RID: 13876 RVA: 0x00623A13 File Offset: 0x00621C13
		[JsonProperty]
		public double GraniteChestChance { get; set; }

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06003635 RID: 13877 RVA: 0x00623A1C File Offset: 0x00621C1C
		// (set) Token: 0x06003636 RID: 13878 RVA: 0x00623A24 File Offset: 0x00621C24
		[JsonProperty]
		public double MarbleChestChance { get; set; }

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06003637 RID: 13879 RVA: 0x00623A2D File Offset: 0x00621C2D
		// (set) Token: 0x06003638 RID: 13880 RVA: 0x00623A35 File Offset: 0x00621C35
		[JsonProperty]
		public double MushroomChestChance { get; set; }

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06003639 RID: 13881 RVA: 0x00623A3E File Offset: 0x00621C3E
		// (set) Token: 0x0600363A RID: 13882 RVA: 0x00623A46 File Offset: 0x00621C46
		[JsonProperty]
		public double DesertChestChance { get; set; }

		// Token: 0x0600363B RID: 13883 RVA: 0x00623A50 File Offset: 0x00621C50
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			if (!WorldGen.InWorld(origin.X, origin.Y, 30))
			{
				return false;
			}
			int num = 25;
			for (int i = origin.X - num; i <= origin.X + num; i++)
			{
				for (int j = origin.Y - num; j <= origin.Y + num; j++)
				{
					if (Main.tile[i, j].wire())
					{
						return false;
					}
					if (TileID.Sets.BasicChest[(int)Main.tile[i, j].type])
					{
						return false;
					}
				}
			}
			HouseBuilder houseBuilder = HouseUtils.CreateBuilder(origin, structures);
			if (!houseBuilder.IsValid)
			{
				return false;
			}
			this.ApplyConfigurationToBuilder(houseBuilder);
			houseBuilder.Place(this._builderContext, structures);
			return true;
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x00623B04 File Offset: 0x00621D04
		private void ApplyConfigurationToBuilder(HouseBuilder builder)
		{
			switch (builder.Type)
			{
			case HouseType.Wood:
				builder.ChestChance = this.GoldChestChance;
				return;
			case HouseType.Ice:
				builder.ChestChance = this.IceChestChance;
				return;
			case HouseType.Desert:
				builder.ChestChance = this.DesertChestChance;
				return;
			case HouseType.Jungle:
				builder.ChestChance = this.JungleChestChance;
				return;
			case HouseType.Mushroom:
				builder.ChestChance = this.MushroomChestChance;
				return;
			case HouseType.Granite:
				builder.ChestChance = this.GraniteChestChance;
				return;
			case HouseType.Marble:
				builder.ChestChance = this.MarbleChestChance;
				return;
			default:
				return;
			}
		}

		// Token: 0x04005AF1 RID: 23281
		private readonly HouseBuilderContext _builderContext = new HouseBuilderContext();
	}
}
