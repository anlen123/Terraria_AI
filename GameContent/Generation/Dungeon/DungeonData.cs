using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Entrances;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x02000499 RID: 1177
	public class DungeonData
	{
		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060033AA RID: 13226 RVA: 0x005F8F4F File Offset: 0x005F714F
		public DungeonGenVars genVars
		{
			get
			{
				return GenVars.dungeonGenVars[this.Iteration];
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060033AB RID: 13227 RVA: 0x005F8F61 File Offset: 0x005F7161
		public double HallSizeScalar
		{
			get
			{
				return (this.hallStrengthScalar + this.hallStepScalar) / 2.0;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060033AC RID: 13228 RVA: 0x005F8F7A File Offset: 0x005F717A
		public double RoomSizeScalar
		{
			get
			{
				return (this.roomStrengthScalar + this.roomStepScalar) / 2.0;
			}
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x005F8F94 File Offset: 0x005F7194
		public bool CanGenerateFeatureInArea(IDungeonFeature feature, int x, int y, int fluff)
		{
			DungeonBounds dungeonBounds = new DungeonBounds();
			dungeonBounds.SetBounds(x - fluff, y - fluff, x + fluff, y + fluff);
			dungeonBounds.CalculateHitbox();
			return this.CanGenerateFeatureInArea(feature, dungeonBounds);
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x005F8FCC File Offset: 0x005F71CC
		public bool CanGenerateFeatureInArea(IDungeonFeature feature, DungeonBounds bounds)
		{
			return bounds.HasHitbox() && this.CanGenerateFeatureInArea(feature, bounds.Hitbox);
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x005F8FE8 File Offset: 0x005F71E8
		public bool CanGenerateFeatureInArea(IDungeonFeature feature, Rectangle hitbox)
		{
			for (int i = hitbox.Left; i <= hitbox.Right; i++)
			{
				for (int j = hitbox.Top; j <= hitbox.Bottom; j++)
				{
					if (!this.CanGenerateFeatureAt(feature, i, j))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x005F9034 File Offset: 0x005F7234
		public bool CanGenerateFeatureAt(IDungeonFeature feature, int x, int y)
		{
			if (!WorldGen.InWorld(x, y, 5))
			{
				return false;
			}
			if (Main.tile[x, y].wall == 350)
			{
				return false;
			}
			if (this.dungeonEntrance.Bounds.Contains(x, y) && !this.dungeonEntrance.CanGenerateFeatureAt(this, feature, x, y))
			{
				return false;
			}
			for (int i = 0; i < this.protectedDungeonBounds.Count; i++)
			{
				if (this.protectedDungeonBounds[i].Contains(x, y))
				{
					return false;
				}
			}
			for (int j = 0; j < this.dungeonFeatures.Count; j++)
			{
				IDungeonFeature dungeonFeature = this.dungeonFeatures[j];
				if (dungeonFeature is DungeonFeature)
				{
					DungeonFeature dungeonFeature2 = (DungeonFeature)dungeonFeature;
					if (dungeonFeature2.generated && dungeonFeature2.Bounds.Contains(x, y) && !dungeonFeature2.CanGenerateFeatureAt(this, feature, x, y))
					{
						return false;
					}
				}
			}
			for (int k = 0; k < this.dungeonRooms.Count; k++)
			{
				DungeonRoom dungeonRoom = this.dungeonRooms[k];
				if (dungeonRoom.generated && dungeonRoom.OuterBounds.Contains(x, y) && !dungeonRoom.CanGenerateFeatureAt(this, feature, x, y))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x005F9164 File Offset: 0x005F7364
		public bool IsAnyRoomInSpot(out DungeonRoom roomFound, int x, int y, DungeonRoomSearchSettings settings)
		{
			roomFound = null;
			for (int i = 0; i < this.dungeonRooms.Count; i++)
			{
				DungeonRoom dungeonRoom = this.dungeonRooms[i];
				if (DungeonUtils.RoomCanBeChosen(dungeonRoom, settings) && dungeonRoom.InnerBounds.ContainsWithFluff(x, y, settings.Fluff))
				{
					roomFound = dungeonRoom;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400590D RID: 22797
		public DungeonType Type;

		// Token: 0x0400590E RID: 22798
		public int Iteration;

		// Token: 0x0400590F RID: 22799
		public DungeonEntrance dungeonEntrance;

		// Token: 0x04005910 RID: 22800
		public List<DungeonRoom> dungeonRooms = new List<DungeonRoom>();

		// Token: 0x04005911 RID: 22801
		public List<DungeonHall> dungeonHalls = new List<DungeonHall>();

		// Token: 0x04005912 RID: 22802
		public List<IDungeonFeature> dungeonFeatures = new List<IDungeonFeature>();

		// Token: 0x04005913 RID: 22803
		public List<DungeonDoorData> dungeonDoorData = new List<DungeonDoorData>();

		// Token: 0x04005914 RID: 22804
		public List<DungeonPlatformData> dungeonPlatformData = new List<DungeonPlatformData>();

		// Token: 0x04005915 RID: 22805
		public List<DungeonBounds> protectedDungeonBounds = new List<DungeonBounds>();

		// Token: 0x04005916 RID: 22806
		public bool makeNextPitTrapFlooded;

		// Token: 0x04005917 RID: 22807
		public bool useSkewedDungeonEntranceHalls;

		// Token: 0x04005918 RID: 22808
		public bool createdDungeonEntranceOnSurface;

		// Token: 0x04005919 RID: 22809
		public double dungeonEntranceStrengthX;

		// Token: 0x0400591A RID: 22810
		public double dungeonEntranceStrengthY;

		// Token: 0x0400591B RID: 22811
		public double dungeonEntranceStrengthX2;

		// Token: 0x0400591C RID: 22812
		public double dungeonEntranceStrengthY2;

		// Token: 0x0400591D RID: 22813
		public Vector2D lastDungeonHall = Vector2D.Zero;

		// Token: 0x0400591E RID: 22814
		public DungeonBounds dungeonBounds = new DungeonBounds();

		// Token: 0x0400591F RID: 22815
		public DungeonBounds[] outerProgressionBounds = new DungeonBounds[0];

		// Token: 0x04005920 RID: 22816
		public int[] wallVariants = new int[3];

		// Token: 0x04005921 RID: 22817
		public int chandelierItemType;

		// Token: 0x04005922 RID: 22818
		public int platformItemType;

		// Token: 0x04005923 RID: 22819
		public int doorItemType;

		// Token: 0x04005924 RID: 22820
		public int[] lanternStyles = new int[3];

		// Token: 0x04005925 RID: 22821
		public int[] shelfStyles = new int[3];

		// Token: 0x04005926 RID: 22822
		public int[] bannerStyles = new int[6];

		// Token: 0x04005927 RID: 22823
		public double globalFeatureScalar = 1.0;

		// Token: 0x04005928 RID: 22824
		public double dungeonStepScalar = 1.0;

		// Token: 0x04005929 RID: 22825
		public double hallStrengthScalar = 1.0;

		// Token: 0x0400592A RID: 22826
		public double hallStepScalar = 1.0;

		// Token: 0x0400592B RID: 22827
		public double hallInteriorToExteriorRatio = 0.5;

		// Token: 0x0400592C RID: 22828
		public double hallSlantVariantScalar = 1.0;

		// Token: 0x0400592D RID: 22829
		public double roomStrengthScalar = 1.0;

		// Token: 0x0400592E RID: 22830
		public double roomStepScalar = 1.0;

		// Token: 0x0400592F RID: 22831
		public double roomInteriorToExteriorRatio = 0.5;

		// Token: 0x04005930 RID: 22832
		public double roomSlantVariantScalar = 1.0;
	}
}
