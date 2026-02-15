using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.LayoutProviders
{
	// Token: 0x020004B8 RID: 1208
	public class DualDungeonLayoutProvider : DungeonLayoutProvider
	{
		// Token: 0x06003472 RID: 13426 RVA: 0x00603FE9 File Offset: 0x006021E9
		public DualDungeonLayoutProvider(DungeonLayoutProviderSettings settings) : base(settings)
		{
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x00603FF4 File Offset: 0x006021F4
		public override void ProvideLayout(DungeonData data, GenerationProgress progress, UnifiedRandom genRand, ref int roomDelay)
		{
			DungeonRoom entranceRoom = DualDungeonLayoutProvider.CalculateEntranceRoom(data);
			DungeonRoom dungeonRoom = this.CalculateStartingRoom(data);
			this.CalculateEntranceHall(data, entranceRoom, dungeonRoom);
			List<DungeonRoom> first = this.CalculateBiomeRooms(data);
			List<DungeonRoom> list = this.CalculateRooms(data);
			this.ConvertSpecializedRooms(data, list);
			List<DungeonHall> halls = new DualDungeonLayoutProvider.HallwayCalculator(data, first.Concat(list).Concat(new DungeonRoom[]
			{
				dungeonRoom
			}).ToList<DungeonRoom>()).Generate();
			this.ConvertSpecializedHalls(halls);
			DualDungeonLayoutProvider.CalculateDungeonBounds(data, list.Concat(new DungeonRoom[]
			{
				dungeonRoom
			}), halls);
			for (int i = 0; i < data.dungeonRooms.Count; i++)
			{
				double num = Math.Round((double)((float)(i + 1) / (float)data.dungeonRooms.Count * 100f));
				DungeonUtils.UpdateDungeonProgress(progress, Utils.Remap((float)i, 0f, (float)data.dungeonRooms.Count, 0.35f, 0.4f, true), Language.GetTextValue("WorldGeneration.DungeonRooms", num), true);
				data.dungeonRooms[i].GenerateRoom(data);
			}
			for (int j = 0; j < data.dungeonRooms.Count; j++)
			{
				data.dungeonRooms[j].GeneratePreHallwaysDungeonFeaturesInRoom(data);
			}
			List<DungeonHall> list2 = data.dungeonHalls.FindAll(new Predicate<DungeonHall>(DualDungeonLayoutProvider.HallwayCheck_IsCrackedBrickHall));
			List<DungeonHall> list3 = data.dungeonHalls.FindAll(new Predicate<DungeonHall>(DualDungeonLayoutProvider.HallwayCheck_IsSpiderHall));
			int num2 = 0;
			for (int k = 0; k < list2.Count; k++)
			{
				DungeonHall dungeonHall = list2[k];
				double num3 = Math.Round((double)((float)(num2 + 1) / (float)data.dungeonHalls.Count * 100f));
				DungeonUtils.UpdateDungeonProgress(progress, Utils.Remap((float)num2, 0f, (float)data.dungeonHalls.Count, 0.4f, 0.6f, true), Language.GetTextValue("WorldGeneration.DungeonHalls", num3), true);
				if (dungeonHall.calculated)
				{
					dungeonHall.GenerateHall(data);
					num2++;
				}
			}
			for (int l = 0; l < list3.Count; l++)
			{
				DungeonHall dungeonHall2 = list3[l];
				double num4 = Math.Round((double)((float)(num2 + 1) / (float)data.dungeonHalls.Count * 100f));
				DungeonUtils.UpdateDungeonProgress(progress, Utils.Remap((float)num2, 0f, (float)data.dungeonHalls.Count, 0.4f, 0.6f, true), Language.GetTextValue("WorldGeneration.DungeonHalls", num4), true);
				if (dungeonHall2.calculated)
				{
					dungeonHall2.GenerateHall(data);
					num2++;
				}
			}
			for (int m = 0; m < data.dungeonHalls.Count; m++)
			{
				DungeonHall dungeonHall3 = data.dungeonHalls[m];
				if (!DualDungeonLayoutProvider.HallwayCheck_IsCrackedBrickHall(dungeonHall3) && !DualDungeonLayoutProvider.HallwayCheck_IsSpiderHall(dungeonHall3))
				{
					double num5 = Math.Round((double)((float)(num2 + 1) / (float)data.dungeonHalls.Count * 100f));
					DungeonUtils.UpdateDungeonProgress(progress, Utils.Remap((float)num2, 0f, (float)data.dungeonHalls.Count, 0.4f, 0.6f, true), Language.GetTextValue("WorldGeneration.DungeonHalls", num5), true);
					if (dungeonHall3.calculated)
					{
						dungeonHall3.GenerateHall(data);
						num2++;
					}
				}
			}
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x00604351 File Offset: 0x00602551
		private static bool HallwayCheck_IsSpiderHall(DungeonHall hall)
		{
			return hall.settings.StyleData.Style == 12;
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x00604367 File Offset: 0x00602567
		private static bool HallwayCheck_IsCrackedBrickHall(DungeonHall hall)
		{
			return hall.CrackedBrick;
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x00604370 File Offset: 0x00602570
		private DungeonRoom CalculateStartingRoom(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			DungeonControlLine dungeonControlLine = data.genVars.dungeonDitherSnake[0];
			Vector2D start = dungeonControlLine.Start;
			DungeonRoom dungeonRoom = DungeonCrawler.MakeDungeon_GetRoom(new RegularDungeonRoomSettings
			{
				RoomType = DungeonRoomType.Regular,
				RoomPosition = new Point((int)start.X, (int)start.Y),
				RandomSeed = genRand.Next(),
				ProgressionStage = dungeonControlLine.ProgressionStage,
				StyleData = dungeonControlLine.Style,
				OverrideOuterBoundsSize = 8,
				OverrideInnerBoundsSize = (int)(20.0 * data.roomStrengthScalar)
			}, true);
			dungeonRoom.CalculateRoom(data);
			return dungeonRoom;
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x00604410 File Offset: 0x00602610
		private DungeonHall CalculateEntranceHall(DungeonData data, DungeonRoom entranceRoom, DungeonRoom startingRoom)
		{
			StairwellDungeonHallSettings stairwellDungeonHallSettings = (StairwellDungeonHallSettings)DungeonCrawler.MakeDungeon_GetHallSettings(DungeonHallType.Stairwell, data, Vector2.Zero, Vector2.Zero, startingRoom.settings.StyleData);
			stairwellDungeonHallSettings.IsEntranceHall = true;
			DungeonHall dungeonHall = DungeonCrawler.MakeDungeon_GetHall(stairwellDungeonHallSettings, true);
			Vector2D startPoint;
			Vector2D endPoint;
			DualDungeonLayoutProvider.GetHallwayConnectionPoints(entranceRoom, startingRoom, out startPoint, out endPoint);
			dungeonHall.CalculateHall(data, startPoint, endPoint);
			return dungeonHall;
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x00604460 File Offset: 0x00602660
		private List<DungeonRoom> CalculateBiomeRooms(DungeonData data)
		{
			List<DungeonRoom> list = new List<DungeonRoom>();
			foreach (DungeonControlLine dungeonControlLine in data.genVars.dungeonDitherSnake)
			{
				if (dungeonControlLine.Next == null || dungeonControlLine.Next.ProgressionStage != dungeonControlLine.ProgressionStage)
				{
					Vector2D vector2D = (dungeonControlLine.Next != null) ? dungeonControlLine.Start : dungeonControlLine.End;
					DungeonRoomSettings dungeonRoomSettings = DungeonCrawler.MakeDungeon_GetRoomSettings(dungeonControlLine.Style.BiomeRoomType, data, dungeonControlLine);
					dungeonRoomSettings.RoomPosition = new Point((int)vector2D.X, (int)vector2D.Y);
					DungeonRoom dungeonRoom = DungeonCrawler.MakeDungeon_GetRoom(dungeonRoomSettings, true);
					dungeonRoom.CalculateRoom(data);
					list.Add(dungeonRoom);
				}
			}
			return list;
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x00604530 File Offset: 0x00602730
		private List<DungeonRoom> CalculateRooms(DungeonData data)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			IEnumerable<DungeonControlLine> dungeonDitherSnake = data.genVars.dungeonDitherSnake;
			List<DungeonRoom> list = new List<DungeonRoom>();
			foreach (DungeonControlLine dungeonControlLine in dungeonDitherSnake.Skip(1))
			{
				if (dungeonControlLine.ProgressionStage != dungeonControlLine.Prev.ProgressionStage)
				{
					DungeonRoom item;
					if (this.TryMakeRegularRoomOnLine(data, dungeonControlLine, 0.8, genRand.NextDouble() - 0.5, out item))
					{
						list.Add(item);
					}
				}
				else
				{
					for (int i = 0; i < 20; i++)
					{
						double num = genRand.NextDouble() * 2.0 - 1.0;
						num = (double)Math.Sign(num) * Math.Pow(Math.Abs(num), 0.5);
						DungeonRoom item;
						if (this.TryMakeRegularRoomOnLine(data, dungeonControlLine, genRand.NextDouble(), num, out item))
						{
							list.Add(item);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x00604644 File Offset: 0x00602844
		private bool TryMakeRegularRoomOnLine(DungeonData data, DungeonControlLine line, double normalizedDistanceAlong, double normalizedDistanceFrom, out DungeonRoom room)
		{
			int num = 10;
			DungeonRoomSettings dungeonRoomSettings = DungeonCrawler.MakeDungeon_GetRoomSettings(DualDungeonLayoutProvider.DualDungeonLayout_GetGeneralRoomType(WorldGen.genRand), data, line);
			int boundingRadius = dungeonRoomSettings.GetBoundingRadius();
			SnakeOrientation orientation = dungeonRoomSettings.Orientation;
			Point point = data.genVars.dungeonDitherSnake.GetRoomPositionInsideBorder(line, normalizedDistanceAlong, normalizedDistanceFrom, boundingRadius, out orientation).ToPoint();
			dungeonRoomSettings.Orientation = orientation;
			dungeonRoomSettings.RoomPosition = new Point(point.X, point.Y);
			room = DungeonCrawler.MakeDungeon_TryRoom(data, point.X, point.Y, dungeonRoomSettings, true, boundingRadius + num, true);
			if (room == null)
			{
				return false;
			}
			room.CalculateRoom(data);
			return true;
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x006046E0 File Offset: 0x006028E0
		private static void CalculateDungeonBounds(DungeonData data, IEnumerable<DungeonRoom> rooms, IEnumerable<DungeonHall> halls)
		{
			DungeonBounds outerPotentialDungeonBounds = data.genVars.outerPotentialDungeonBounds;
			int count = data.genVars.dungeonGenerationStyles.Count;
			data.outerProgressionBounds = new DungeonBounds[count];
			for (int i = 0; i < count; i++)
			{
				DungeonBounds dungeonBounds = data.outerProgressionBounds[i] = new DungeonBounds();
				foreach (DungeonRoom dungeonRoom in rooms)
				{
					dungeonBounds.UpdateBounds(dungeonRoom.OuterBounds);
				}
				foreach (DungeonHall dungeonHall in halls)
				{
					dungeonBounds.UpdateBounds(dungeonHall.Bounds);
				}
				if (dungeonBounds.Top < outerPotentialDungeonBounds.Top)
				{
					dungeonBounds.Top = outerPotentialDungeonBounds.Top;
				}
				if (dungeonBounds.Bottom > outerPotentialDungeonBounds.Bottom)
				{
					dungeonBounds.Bottom = outerPotentialDungeonBounds.Bottom;
				}
				dungeonBounds.CalculateHitbox();
			}
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x00604800 File Offset: 0x00602A00
		private static DungeonRoom CalculateEntranceRoom(DungeonData data)
		{
			DungeonBounds outerPotentialDungeonBounds = data.genVars.outerPotentialDungeonBounds;
			if (data.genVars.generatingDungeonPositionY > outerPotentialDungeonBounds.Top - 10)
			{
				data.genVars.generatingDungeonPositionY = outerPotentialDungeonBounds.Top - 10;
			}
			if (data.genVars.preGenDungeonEntranceSettings.PrecalculateEntrancePosition)
			{
				data.genVars.generatingDungeonPositionX = -10 + (int)data.genVars.dungeonEntrancePosition.X + WorldGen.genRand.Next(20);
				data.genVars.generatingDungeonPositionY = (int)data.genVars.dungeonEntrancePosition.Y + 30;
			}
			if (SpecialSeedFeatures.DungeonEntranceIsBuried)
			{
				data.genVars.generatingDungeonPositionY = (int)Main.worldSurface + 15;
			}
			if (SpecialSeedFeatures.DungeonEntranceIsUnderground)
			{
				data.genVars.generatingDungeonPositionY = (int)GenVars.worldSurfaceHigh + 15;
			}
			DungeonRoom dungeonRoom = DungeonCrawler.MakeDungeon_GetRoom(new LegacyDungeonRoomSettings
			{
				StyleData = data.genVars.dungeonStyle,
				RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY),
				IsEntranceRoom = true
			}, true);
			dungeonRoom.CalculateRoom(data);
			return dungeonRoom;
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x00604924 File Offset: 0x00602B24
		public static DungeonRoomType DualDungeonLayout_GetGeneralRoomType(UnifiedRandom genRand)
		{
			switch (genRand.Next(8))
			{
			default:
				return DungeonRoomType.Legacy;
			case 1:
				return DungeonRoomType.Regular;
			case 2:
				return DungeonRoomType.Wormlike;
			case 3:
				return DungeonRoomType.GenShapeCircle;
			case 4:
				return DungeonRoomType.GenShapeMound;
			case 5:
				return DungeonRoomType.GenShapeHourglass;
			case 6:
				return DungeonRoomType.GenShapeDoughnut;
			case 7:
				return DungeonRoomType.GenShapeQuadCircle;
			}
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x00604974 File Offset: 0x00602B74
		public static DungeonHallType DualDungeonLayout_GetGeneralHallType(UnifiedRandom genRand)
		{
			switch (genRand.Next(3))
			{
			default:
				return DungeonHallType.Legacy;
			case 1:
				return DungeonHallType.Regular;
			case 2:
				return DungeonHallType.Sine;
			}
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x006049A0 File Offset: 0x00602BA0
		public void ConvertSpecializedRooms(DungeonData data, List<DungeonRoom> rooms)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			DitherSnake dungeonDitherSnake = data.genVars.dungeonDitherSnake;
			int num = 2;
			int num2 = 2;
			int num3 = 2;
			int num4 = 2;
			int num5 = 5;
			int num6 = 6;
			switch (WorldGen.GetWorldSize())
			{
			case 0:
				num = 2;
				num2 = 2;
				num3 = 2;
				num4 = 2;
				num5 = 5;
				num6 = 6;
				break;
			case 1:
				num = 4;
				num2 = 6;
				num3 = 6;
				num4 = 6;
				num5 = 8;
				num6 = 10;
				break;
			case 2:
				num = 6;
				num2 = 10;
				num3 = 10;
				num4 = 8;
				num5 = 11;
				num6 = 14;
				break;
			}
			List<DungeonRoom> list = new List<DungeonRoom>();
			List<DungeonRoom> list2 = new List<DungeonRoom>();
			List<DungeonRoom> list3 = new List<DungeonRoom>();
			foreach (DungeonRoom dungeonRoom in rooms)
			{
				byte style = dungeonRoom.settings.StyleData.Style;
				if (style != 1)
				{
					if (style != 6)
					{
						if (style == 8)
						{
							list2.Add(dungeonRoom);
						}
					}
					else
					{
						list3.Add(dungeonRoom);
					}
				}
				else
				{
					list.Add(dungeonRoom);
				}
			}
			List<DungeonRoom> list4 = list.ToList<DungeonRoom>();
			int count = list4.Count;
			int num7 = 2000;
			while (num7 > 0 && list4.Count > 0 && num > 0)
			{
				num7--;
				if (num7 <= 0)
				{
					break;
				}
				DungeonRoom dungeonRoom2 = list4[genRand.Next(list4.Count)];
				if (dungeonRoom2.settings.OnCurvedLine || dungeonRoom2.settings.Orientation != SnakeOrientation.Bottom)
				{
					list4.Remove(dungeonRoom2);
				}
				else if (dungeonRoom2.settings is GenShapeDungeonRoomSettings && ((GenShapeDungeonRoomSettings)dungeonRoom2.settings).ShapeType == GenShapeType.Doughnut)
				{
					list4.Remove(dungeonRoom2);
				}
				else if (dungeonRoom2.GetFloodedRoomTileCount() < SceneMetrics.ShimmerTileThreshold)
				{
					list4.Remove(dungeonRoom2);
				}
				else
				{
					dungeonRoom2.settings.StyleData = DungeonGenerationStyles.Shimmer;
					dungeonRoom2.settings.HallwayConnectionPointOverride = new DungeonUtils.GetHallwayConnectionPoint(DualDungeonLayoutProvider.ConnectToTopOfRoomOnly);
					num--;
					list.Remove(dungeonRoom2);
					list4.Remove(dungeonRoom2);
				}
			}
			if (num > 0)
			{
				list4 = list.ToList<DungeonRoom>();
				int count2 = list4.Count;
				num7 = 2000;
				while (num7 > 0 && list4.Count > 0 && num > 0)
				{
					num7--;
					if (num7 <= 0)
					{
						break;
					}
					DungeonRoom dungeonRoom3 = list4[genRand.Next(list4.Count)];
					if (dungeonRoom3.settings.OnCurvedLine || dungeonRoom3.settings.Orientation != SnakeOrientation.Bottom)
					{
						list4.Remove(dungeonRoom3);
					}
					else if (dungeonRoom3.settings is GenShapeDungeonRoomSettings && ((GenShapeDungeonRoomSettings)dungeonRoom3.settings).ShapeType == GenShapeType.Doughnut)
					{
						list4.Remove(dungeonRoom3);
					}
					else
					{
						dungeonRoom3.settings.StyleData = DungeonGenerationStyles.Shimmer;
						dungeonRoom3.settings.HallwayConnectionPointOverride = new DungeonUtils.GetHallwayConnectionPoint(DualDungeonLayoutProvider.ConnectToTopOfRoomOnly);
						num--;
						list.Remove(dungeonRoom3);
						list4.Remove(dungeonRoom3);
					}
				}
			}
			list4 = list.ToList<DungeonRoom>();
			num7 = 2000;
			while (num7 > 0 && list4.Count > 0 && num2 > 0)
			{
				num7--;
				if (num7 <= 0)
				{
					break;
				}
				DungeonRoom dungeonRoom4 = list4[genRand.Next(list4.Count)];
				if (dungeonRoom4.settings.OnCurvedLine)
				{
					list4.Remove(dungeonRoom4);
				}
				else
				{
					DungeonControlLine controlLine = dungeonRoom4.settings.ControlLine;
					DungeonRoomSettings dungeonRoomSettings = DungeonCrawler.MakeDungeon_GetRoomSettings(DungeonRoomType.LivingTree, data, controlLine);
					dungeonRoomSettings.RoomPosition = new Point(dungeonRoom4.settings.RoomPosition.X, dungeonRoom4.settings.RoomPosition.Y);
					if (dungeonDitherSnake.IsCircleInsideBorderWithMatchingStyle(controlLine, dungeonRoomSettings.RoomPosition, dungeonRoomSettings.GetBoundingRadius()))
					{
						int num8 = data.dungeonRooms.IndexOf(dungeonRoom4);
						int num9 = rooms.IndexOf(dungeonRoom4);
						list.Remove(dungeonRoom4);
						list4.Remove(dungeonRoom4);
						rooms.Remove(dungeonRoom4);
						data.dungeonRooms.Remove(dungeonRoom4);
						dungeonRoom4 = DungeonCrawler.MakeDungeon_GetRoom(dungeonRoomSettings, false);
						dungeonRoom4.CalculateRoom(data);
						if (num8 > -1)
						{
							data.dungeonRooms.Insert(num8, dungeonRoom4);
						}
						else
						{
							data.dungeonRooms.Add(dungeonRoom4);
						}
						if (num9 > -1)
						{
							rooms.Insert(num9, dungeonRoom4);
						}
						else
						{
							rooms.Add(dungeonRoom4);
						}
						dungeonRoom4.settings.StyleData = DungeonGenerationStyles.LivingWood;
						dungeonRoom4.settings.HallwayConnectionPointOverride = new DungeonUtils.GetHallwayConnectionPoint(DualDungeonLayoutProvider.ConnectToBottomOfRoomOnly);
						num2--;
					}
				}
			}
			list4 = list.ToList<DungeonRoom>();
			num7 = 2000;
			while (num7 > 0 && list4.Count > 0 && num4 > 0)
			{
				num7--;
				if (num7 <= 0)
				{
					break;
				}
				DungeonRoom dungeonRoom5 = list4[genRand.Next(list4.Count)];
				if (dungeonRoom5.settings.OnCurvedLine)
				{
					list4.Remove(dungeonRoom5);
				}
				else
				{
					dungeonRoom5.settings.StyleData = DungeonGenerationStyles.Spider;
					num4--;
					list.Remove(dungeonRoom5);
					list4.Remove(dungeonRoom5);
				}
			}
			list4 = list2.ToList<DungeonRoom>();
			num7 = 2000;
			while (num7 > 0 && list4.Count > 0 && num5 > 0)
			{
				num7--;
				if (num7 <= 0)
				{
					break;
				}
				DungeonRoom dungeonRoom6 = list4[genRand.Next(list4.Count)];
				if (dungeonRoom6.settings.OnCurvedLine || dungeonRoom6.settings.Orientation != SnakeOrientation.Bottom)
				{
					list4.Remove(dungeonRoom6);
				}
				else if (dungeonRoom6.settings is GenShapeDungeonRoomSettings && ((GenShapeDungeonRoomSettings)dungeonRoom6.settings).ShapeType == GenShapeType.Doughnut)
				{
					list4.Remove(dungeonRoom6);
				}
				else
				{
					dungeonRoom6.settings.StyleData = DungeonGenerationStyles.Beehive;
					dungeonRoom6.settings.HallwayConnectionPointOverride = new DungeonUtils.GetHallwayConnectionPoint(DualDungeonLayoutProvider.ConnectToTopOfRoomOnly);
					num5--;
					list2.Remove(dungeonRoom6);
					list4.Remove(dungeonRoom6);
				}
			}
			list4 = list2.ToList<DungeonRoom>();
			num7 = 2000;
			while (num7 > 0 && list4.Count > 0 && num3 > 0)
			{
				num7--;
				if (num7 <= 0)
				{
					break;
				}
				DungeonRoom dungeonRoom7 = list4[genRand.Next(list4.Count)];
				if (dungeonRoom7.settings.OnCurvedLine)
				{
					list4.Remove(dungeonRoom7);
				}
				else
				{
					DungeonControlLine controlLine2 = dungeonRoom7.settings.ControlLine;
					DungeonRoomSettings dungeonRoomSettings2 = DungeonCrawler.MakeDungeon_GetRoomSettings(DungeonRoomType.LivingTree, data, controlLine2);
					dungeonRoomSettings2.RoomPosition = new Point(dungeonRoom7.settings.RoomPosition.X, dungeonRoom7.settings.RoomPosition.Y);
					if (dungeonDitherSnake.IsCircleInsideBorderWithMatchingStyle(controlLine2, dungeonRoomSettings2.RoomPosition, dungeonRoomSettings2.GetBoundingRadius()))
					{
						int num10 = data.dungeonRooms.IndexOf(dungeonRoom7);
						int num11 = rooms.IndexOf(dungeonRoom7);
						list.Remove(dungeonRoom7);
						list4.Remove(dungeonRoom7);
						rooms.Remove(dungeonRoom7);
						data.dungeonRooms.Remove(dungeonRoom7);
						dungeonRoom7 = DungeonCrawler.MakeDungeon_GetRoom(dungeonRoomSettings2, false);
						dungeonRoom7.CalculateRoom(data);
						if (num10 > -1)
						{
							data.dungeonRooms.Insert(num10, dungeonRoom7);
						}
						else
						{
							data.dungeonRooms.Add(dungeonRoom7);
						}
						if (num11 > -1)
						{
							rooms.Insert(num11, dungeonRoom7);
						}
						else
						{
							rooms.Add(dungeonRoom7);
						}
						dungeonRoom7.settings.StyleData = DungeonGenerationStyles.LivingMahogany;
						dungeonRoom7.settings.HallwayConnectionPointOverride = new DungeonUtils.GetHallwayConnectionPoint(DualDungeonLayoutProvider.ConnectToBottomOfRoomOnly);
						num3--;
					}
				}
			}
			list4 = list3.ToList<DungeonRoom>();
			num7 = 2000;
			while (num7 > 0 && list4.Count > 0 && num6 > 0)
			{
				num7--;
				if (num7 <= 0)
				{
					break;
				}
				DungeonRoom dungeonRoom8 = list4[genRand.Next(list4.Count)];
				if (dungeonRoom8.settings.OnCurvedLine)
				{
					list4.Remove(dungeonRoom8);
				}
				else
				{
					dungeonRoom8.settings.StyleData = DungeonGenerationStyles.Crystal;
					num6--;
					list3.Remove(dungeonRoom8);
					list4.Remove(dungeonRoom8);
				}
			}
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x006051EC File Offset: 0x006033EC
		public void ConvertSpecializedHalls(List<DungeonHall> halls)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			int num = 3;
			switch (WorldGen.GetWorldSize())
			{
			case 0:
				num = 3;
				break;
			case 1:
				num = 4;
				break;
			case 2:
				num = 5;
				break;
			}
			List<DungeonHall> list = new List<DungeonHall>();
			foreach (DungeonHall dungeonHall in halls)
			{
				byte style = dungeonHall.settings.StyleData.Style;
				if (style == 1)
				{
					list.Add(dungeonHall);
				}
			}
			List<DungeonHall> list2 = list.ToList<DungeonHall>();
			int num2 = 2000;
			while (num2 > 0 && list2.Count > 0 && num > 0)
			{
				num2--;
				if (num2 <= 0)
				{
					break;
				}
				DungeonHall dungeonHall2 = list2[genRand.Next(list2.Count)];
				if (dungeonHall2.CrackedBrick)
				{
					list2.Remove(dungeonHall2);
				}
				else
				{
					dungeonHall2.settings.StyleData = DungeonGenerationStyles.Spider;
					num--;
					list.Remove(dungeonHall2);
					list2.Remove(dungeonHall2);
				}
			}
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x0060530C File Offset: 0x0060350C
		public static ConnectionPointQuality ConnectToTopOfRoomOnly(DungeonRoom room, Vector2D otherRoomPos, out Vector2D connectionPoint)
		{
			connectionPoint = room.GetRoomCenterForHallway(otherRoomPos);
			while (room.IsInsideRoom(connectionPoint.ToPoint()))
			{
				connectionPoint.Y -= 1.0;
			}
			connectionPoint.Y += 3.0;
			Vector2D vector2D = (otherRoomPos - connectionPoint).SafeNormalize(default(Vector2D));
			if (vector2D.Y < 0.0)
			{
				return ConnectionPointQuality.Good;
			}
			if (vector2D.Y >= 0.3)
			{
				return ConnectionPointQuality.Bad;
			}
			return ConnectionPointQuality.Okay;
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x006053A8 File Offset: 0x006035A8
		public static ConnectionPointQuality ConnectToBottomOfRoomOnly(DungeonRoom room, Vector2D otherRoomPos, out Vector2D connectionPoint)
		{
			connectionPoint = room.GetRoomCenterForHallway(otherRoomPos);
			while (room.IsInsideRoom(connectionPoint.ToPoint()))
			{
				connectionPoint.Y += 1.0;
			}
			connectionPoint.Y -= 3.0;
			Vector2D vector2D = (otherRoomPos - connectionPoint).SafeNormalize(default(Vector2D));
			if (vector2D.Y > 0.3)
			{
				return ConnectionPointQuality.Bad;
			}
			if (vector2D.Y <= 0.0)
			{
				return ConnectionPointQuality.Good;
			}
			return ConnectionPointQuality.Okay;
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x00605444 File Offset: 0x00603644
		public static ConnectionPointQuality GetHallwayConnectionPoints(DungeonRoom room1, DungeonRoom room2, out Vector2D point1, out Vector2D point2)
		{
			int hallwayConnectionPoint = (int)room1.GetHallwayConnectionPoint(room2.Center, out point1);
			ConnectionPointQuality hallwayConnectionPoint2 = room2.GetHallwayConnectionPoint(room1.Center, out point2);
			return (ConnectionPointQuality)Math.Max(hallwayConnectionPoint, (int)hallwayConnectionPoint2);
		}

		// Token: 0x02000987 RID: 2439
		private class HallwayCalculator
		{
			// Token: 0x0600494F RID: 18767 RVA: 0x006CFB50 File Offset: 0x006CDD50
			public HallwayCalculator(DungeonData data, List<DungeonRoom> rooms)
			{
				this.data = data;
				this.rooms = (from r in rooms
				select new DualDungeonLayoutProvider.HallwayCalculator.RoomEntry
				{
					room = r,
					progressAlongSnake = data.genVars.dungeonDitherSnake.GetPositionAlongSnake(r.Center)
				} into r
				orderby r.progressAlongSnake
				select r).ToList<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>();
				this.controlLines = data.genVars.dungeonDitherSnake;
				this.avgLineLength = this.controlLines.Average((DungeonControlLine l) => l.LineLength);
				this.maxProgressDelta = 300.0 / this.avgLineLength;
			}

			// Token: 0x06004950 RID: 18768 RVA: 0x006CFC40 File Offset: 0x006CDE40
			public List<DungeonHall> Generate()
			{
				int hallRadius = 25;
				int hallRadius2 = 8;
				foreach (DualDungeonLayoutProvider.HallwayCalculator.RoomEntry source in this.rooms.Skip(2))
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						double num;
						DualDungeonLayoutProvider.HallwayCalculator.HallLine hallLine = this.SelectGoodRoomForHallway(source, out num, new Func<DualDungeonLayoutProvider.HallwayCalculator.HallLine, double>(this.ScoreStairwell), hallRadius);
						if (hallLine != null && num > 0.0)
						{
							this.MakeHall(hallLine, DungeonHallType.Stairwell);
						}
					}
				}
				foreach (DualDungeonLayoutProvider.HallwayCalculator.RoomEntry roomEntry in this.rooms.Skip(1))
				{
					if (!roomEntry.backLinks.Any<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>())
					{
						int num2 = 0;
						DualDungeonLayoutProvider.HallwayCalculator.HallLine hallLine2;
						for (;;)
						{
							double num3;
							hallLine2 = this.SelectGoodRoomForHallway(roomEntry, out num3, new Func<DualDungeonLayoutProvider.HallwayCalculator.HallLine, double>(this.ScoreHallway), hallRadius2);
							if (hallLine2 != null)
							{
								break;
							}
							if (num2 == 1000)
							{
								goto IL_E5;
							}
							num2++;
						}
						this.MakeHall(hallLine2, DualDungeonLayoutProvider.DualDungeonLayout_GetGeneralHallType(WorldGen.genRand));
					}
					IL_E5:
					if (WorldGen.genRand.Next(2) == 0)
					{
						double num4;
						DualDungeonLayoutProvider.HallwayCalculator.HallLine hallLine3 = this.SelectGoodRoomForHallway(roomEntry, out num4, new Func<DualDungeonLayoutProvider.HallwayCalculator.HallLine, double>(this.ScoreHallway), hallRadius2);
						if (hallLine3 != null && num4 > -2.0)
						{
							this.MakeHall(hallLine3, DualDungeonLayoutProvider.DualDungeonLayout_GetGeneralHallType(WorldGen.genRand));
						}
					}
				}
				return this.halls;
			}

			// Token: 0x06004951 RID: 18769 RVA: 0x006CFDB4 File Offset: 0x006CDFB4
			private void MakeHall(DualDungeonLayoutProvider.HallwayCalculator.HallLine line, DungeonHallType hallType)
			{
				DungeonGenerationStyleData style = this.data.genVars.dungeonDitherSnake.GetLineContaining(line.sourcePoint, null, 0).Style;
				DungeonHallSettings dungeonHallSettings = DungeonCrawler.MakeDungeon_GetHallSettings(hallType, this.data, line.sourcePoint.ToVector2(), line.targetPoint.ToVector2(), style);
				if (DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(line.target))
				{
					dungeonHallSettings.CarveOnly = true;
				}
				DungeonHall dungeonHall = DungeonCrawler.MakeDungeon_GetHall(dungeonHallSettings, true);
				dungeonHall.CalculateHall(this.data, line.sourcePoint, line.targetPoint);
				this.halls.Add(dungeonHall);
				line.source.backLinks.Add(line.target);
				line.target.forwardLinks.Add(line.source);
				if (hallType == DungeonHallType.Stairwell)
				{
					this.stairwells.Add(line);
				}
			}

			// Token: 0x06004952 RID: 18770 RVA: 0x006CFE84 File Offset: 0x006CE084
			private double ScoreHallway(DualDungeonLayoutProvider.HallwayCalculator.HallLine line)
			{
				double num = 0.0;
				double fromMin = Math.Sin(0.6108652381980153);
				Vector2D v = line.targetPoint - line.sourcePoint;
				Vector2D vector2D = v.SafeNormalize(Vector2D.UnitX);
				num -= Utils.Remap(Math.Abs(vector2D.Y), fromMin, 0.0, 0.0, 1.0, true);
				num -= Utils.Remap(Math.Abs(vector2D.Y), fromMin, 1.0, 0.0, 5.0, true);
				num -= Utils.Remap(v.Length(), this.avgLineLength * 1.5, this.avgLineLength * 3.0, 0.0, 3.0, true);
				num -= (double)Utils.Remap((float)line.target.forwardLinks.Count, 1f, 3f, 0f, 2f, true);
				num -= (double)Utils.Remap((float)line.source.backLinks.Count, 1f, 3f, 0f, 2f, true);
				num += (double)Utils.Remap((float)this.DistanceFromCommonAncestor(line.source, line.target), 3f, 6f, 0f, 1f, true);
				if ((DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(line.target) && line.target.forwardLinks.Any<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>()) || (DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(line.source) && line.source.backLinks.Any<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>()))
				{
					num -= 5.0;
				}
				if (DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(line.target) || DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(line.source))
				{
					num -= Utils.Remap(Math.Abs(vector2D.Y), 0.6, 0.8, 0.0, 5.0, true);
				}
				return num;
			}

			// Token: 0x06004953 RID: 18771 RVA: 0x006D0098 File Offset: 0x006CE298
			private double ScoreStairwell(DualDungeonLayoutProvider.HallwayCalculator.HallLine line)
			{
				Vector2D v = line.targetPoint - line.sourcePoint;
				Vector2D vector2D = v.SafeNormalize(Vector2D.UnitX);
				double num = 0.0;
				num -= Utils.Remap(Math.Abs(vector2D.Y), 0.6, 1.0, 1.0, 0.0, true);
				num -= Utils.Remap(v.Length(), this.avgLineLength * 0.5, this.avgLineLength * 1.5, 1.0, 0.0, true);
				if (line.target.backLinks.Any<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>() || line.target.forwardLinks.Any<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>())
				{
					num -= 5.0;
				}
				if (DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(line.source) || DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(line.target))
				{
					num -= 5.0;
				}
				return num;
			}

			// Token: 0x06004954 RID: 18772 RVA: 0x006D01A0 File Offset: 0x006CE3A0
			private DualDungeonLayoutProvider.HallwayCalculator.HallLine SelectGoodRoomForHallway(DualDungeonLayoutProvider.HallwayCalculator.RoomEntry source, out double bestScore, Func<DualDungeonLayoutProvider.HallwayCalculator.HallLine, double> scoreFunc, int hallRadius)
			{
				UnifiedRandom genRand = WorldGen.genRand;
				int num = this.rooms.FindIndex((DualDungeonLayoutProvider.HallwayCalculator.RoomEntry r) => r.progressAlongSnake >= source.progressAlongSnake - this.maxProgressDelta);
				num = Math.Min(num, Math.Max(0, this.rooms.IndexOf(source) - 2));
				IEnumerable<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry> enumerable = this.rooms.Skip(num).TakeWhile((DualDungeonLayoutProvider.HallwayCalculator.RoomEntry r) => r != source);
				double nearbyRoomSearchRadius = (from r in enumerable
				select Vector2D.Distance(r.Center, source.Center)).Max() + this.extraNearbyRoomSearchRadius;
				List<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry> list = (from r in this.rooms
				where r != source && Vector2D.Distance(r.Center, source.Center) <= nearbyRoomSearchRadius
				select r).ToList<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>();
				DualDungeonLayoutProvider.HallwayCalculator.HallLine result = null;
				bestScore = double.MinValue;
				foreach (DualDungeonLayoutProvider.HallwayCalculator.RoomEntry roomEntry in enumerable)
				{
					if (this.CanConnect(source, roomEntry))
					{
						double num2 = genRand.NextDouble();
						DualDungeonLayoutProvider.HallwayCalculator.HallLine hallLine = new DualDungeonLayoutProvider.HallwayCalculator.HallLine
						{
							source = source,
							target = roomEntry
						};
						ConnectionPointQuality hallwayConnectionPoints = DualDungeonLayoutProvider.GetHallwayConnectionPoints(source.room, roomEntry.room, out hallLine.sourcePoint, out hallLine.targetPoint);
						num2 -= (double)((hallwayConnectionPoints == ConnectionPointQuality.Bad) ? 10 : ((hallwayConnectionPoints == ConnectionPointQuality.Okay) ? 2 : 0));
						num2 += scoreFunc(hallLine);
						if (num2 > bestScore)
						{
							foreach (DualDungeonLayoutProvider.HallwayCalculator.RoomEntry roomEntry2 in list)
							{
								if (roomEntry2 != roomEntry)
								{
									Vector2D vector2D = roomEntry2.Center.ClosestPointOnLine(hallLine.sourcePoint, hallLine.targetPoint);
									double num3 = vector2D.Distance(roomEntry2.Center) - (double)roomEntry2.room.settings.GetBoundingRadius() - (double)hallRadius;
									if (num3 < 0.0)
									{
										num2 += num3 / 4.0 - 3.0;
									}
									if (roomEntry2.room.OuterBounds.Contains(vector2D))
									{
										num2 -= 1000.0;
									}
								}
							}
							foreach (DualDungeonLayoutProvider.HallwayCalculator.HallLine hallLine2 in this.stairwells)
							{
								if (Utils.LineSegmentsIntersect(hallLine.sourcePoint, hallLine.targetPoint, hallLine2.sourcePoint, hallLine2.targetPoint))
								{
									num2 -= 3.0;
								}
							}
							if (num2 > bestScore && this.controlLines.IsLineInsideBorder(hallLine.sourcePoint, hallLine.targetPoint, hallRadius))
							{
								result = hallLine;
								bestScore = num2;
							}
						}
					}
				}
				return result;
			}

			// Token: 0x06004955 RID: 18773 RVA: 0x006D04D8 File Offset: 0x006CE6D8
			private int DistanceFromCommonAncestor(DualDungeonLayoutProvider.HallwayCalculator.RoomEntry a, DualDungeonLayoutProvider.HallwayCalculator.RoomEntry b)
			{
				HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry> hashSet = new HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>
				{
					a
				};
				HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry> hashSet2 = new HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>
				{
					b
				};
				HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry> hashSet3 = new HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>
				{
					a
				};
				HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry> hashSet4 = new HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>
				{
					b
				};
				for (int i = 0; i < 8; i++)
				{
					if (hashSet.Any(new Func<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry, bool>(hashSet2.Contains)))
					{
						return i;
					}
					hashSet3 = new HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>(hashSet3.SelectMany((DualDungeonLayoutProvider.HallwayCalculator.RoomEntry e) => e.backLinks));
					foreach (DualDungeonLayoutProvider.HallwayCalculator.RoomEntry item in hashSet3)
					{
						hashSet.Add(item);
					}
					hashSet4 = new HashSet<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>(hashSet4.SelectMany((DualDungeonLayoutProvider.HallwayCalculator.RoomEntry e) => e.backLinks));
					foreach (DualDungeonLayoutProvider.HallwayCalculator.RoomEntry item2 in hashSet4)
					{
						hashSet2.Add(item2);
					}
				}
				return 8;
			}

			// Token: 0x06004956 RID: 18774 RVA: 0x006D0628 File Offset: 0x006CE828
			private bool CanConnect(DualDungeonLayoutProvider.HallwayCalculator.RoomEntry a, DualDungeonLayoutProvider.HallwayCalculator.RoomEntry b)
			{
				return !a.backLinks.Contains(b) && !b.backLinks.Contains(a) && (a.room.settings.ProgressionStage == b.room.settings.ProgressionStage || DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(a) || DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(b));
			}

			// Token: 0x06004957 RID: 18775 RVA: 0x006D0685 File Offset: 0x006CE885
			private static bool IsBiomeRoom(DualDungeonLayoutProvider.HallwayCalculator.RoomEntry entry)
			{
				return DualDungeonLayoutProvider.HallwayCalculator.IsBiomeRoom(entry.room.settings.RoomType);
			}

			// Token: 0x06004958 RID: 18776 RVA: 0x006D069C File Offset: 0x006CE89C
			private static bool IsBiomeRoom(DungeonRoomType roomType)
			{
				return roomType - DungeonRoomType.BiomeSquare <= 2;
			}

			// Token: 0x040075FF RID: 30207
			private readonly DungeonData data;

			// Token: 0x04007600 RID: 30208
			private readonly List<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry> rooms;

			// Token: 0x04007601 RID: 30209
			private readonly List<DungeonHall> halls = new List<DungeonHall>();

			// Token: 0x04007602 RID: 30210
			private readonly List<DualDungeonLayoutProvider.HallwayCalculator.HallLine> stairwells = new List<DualDungeonLayoutProvider.HallwayCalculator.HallLine>();

			// Token: 0x04007603 RID: 30211
			private readonly DitherSnake controlLines;

			// Token: 0x04007604 RID: 30212
			private readonly double maxProgressDelta;

			// Token: 0x04007605 RID: 30213
			private readonly double avgLineLength;

			// Token: 0x04007606 RID: 30214
			private readonly double extraNearbyRoomSearchRadius = 50.0;

			// Token: 0x02000AE9 RID: 2793
			private class RoomEntry
			{
				// Token: 0x170005C4 RID: 1476
				// (get) Token: 0x06004D04 RID: 19716 RVA: 0x006DA13E File Offset: 0x006D833E
				public Point Center
				{
					get
					{
						return this.room.Center;
					}
				}

				// Token: 0x0400787C RID: 30844
				public DungeonRoom room;

				// Token: 0x0400787D RID: 30845
				public double progressAlongSnake;

				// Token: 0x0400787E RID: 30846
				public List<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry> backLinks = new List<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>();

				// Token: 0x0400787F RID: 30847
				public List<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry> forwardLinks = new List<DualDungeonLayoutProvider.HallwayCalculator.RoomEntry>();
			}

			// Token: 0x02000AEA RID: 2794
			private class HallLine
			{
				// Token: 0x04007880 RID: 30848
				public DualDungeonLayoutProvider.HallwayCalculator.RoomEntry source;

				// Token: 0x04007881 RID: 30849
				public DualDungeonLayoutProvider.HallwayCalculator.RoomEntry target;

				// Token: 0x04007882 RID: 30850
				public Vector2D sourcePoint;

				// Token: 0x04007883 RID: 30851
				public Vector2D targetPoint;
			}
		}
	}
}
