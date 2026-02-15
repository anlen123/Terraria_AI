using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent
{
	// Token: 0x02000238 RID: 568
	public class ExtraSpawnPointManager
	{
		// Token: 0x06002258 RID: 8792 RVA: 0x00536FCC File Offset: 0x005351CC
		public static bool TryGetExtraSpawnPointForTeam(int team, out Point spawnPoint)
		{
			spawnPoint = Point.Zero;
			if (!Main.teamBasedSpawnsSeed)
			{
				return false;
			}
			if (team < 0 || team >= ExtraSpawnPointManager.extraSpawnPoints.Length)
			{
				return false;
			}
			try
			{
				spawnPoint = ExtraSpawnPointManager.extraSpawnPoints[team];
			}
			catch (IndexOutOfRangeException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x0053702C File Offset: 0x0053522C
		public static void GenerateExtraSpawns_Setup()
		{
			if (ExtraSpawnPointManager.settings.skyblock)
			{
				ExtraSpawnPointManager._listOfLandmasses.Clear();
				for (int i = 0; i < GenVars.landmassData.Count; i++)
				{
					LandmassData landmassData = GenVars.landmassData[i];
					if (landmassData.DataType == LandmassDataType.SkyblockIsland && landmassData.Style == 0 && landmassData.Position.Distance(new Vector2((float)Main.spawnTileX, (float)Main.spawnTileY)) >= 300f)
					{
						ExtraSpawnPointManager._listOfLandmasses.Add(landmassData);
					}
				}
				return;
			}
			if (ExtraSpawnPointManager.settings.roundLandmass)
			{
				ExtraSpawnPointManager._listOfLandmasses.Clear();
				for (int j = 0; j < GenVars.landmassData.Count; j++)
				{
					LandmassData landmassData2 = GenVars.landmassData[j];
					if (landmassData2.DataType == LandmassDataType.RoundLandmass && landmassData2.Position.Distance(new Vector2((float)Main.spawnTileX, (float)Main.spawnTileY)) >= 300f)
					{
						ExtraSpawnPointManager._listOfLandmasses.Add(landmassData2);
					}
				}
				return;
			}
			if (ExtraSpawnPointManager.settings.extraLiquid)
			{
				ExtraSpawnPointManager._listOfLandmasses.Clear();
				for (int k = 0; k < GenVars.landmassData.Count; k++)
				{
					LandmassData landmassData3 = GenVars.landmassData[k];
					if (landmassData3.DataType == LandmassDataType.ExtraLiquidBubbleSquare && landmassData3.Position.Distance(new Vector2((float)Main.spawnTileX, (float)Main.spawnTileY)) >= 300f)
					{
						ExtraSpawnPointManager._listOfLandmasses.Add(landmassData3);
					}
				}
			}
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00537195 File Offset: 0x00535395
		public static void ResetExtraSpawns()
		{
			ExtraSpawnPointManager._listOfLandmasses.Clear();
			ExtraSpawnPointManager.extraSpawnPoints = new Point[0];
			ExtraSpawnPointManager.settings = default(ExtraSpawnSettings);
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x005371B8 File Offset: 0x005353B8
		public static void GenerateExtraSpawns()
		{
			ExtraSpawnPointManager.GenerateExtraSpawns_Setup();
			ExtraSpawnType spawnType = ExtraSpawnPointManager.settings.spawnType;
			if (spawnType == ExtraSpawnType.None || spawnType != ExtraSpawnType.TeamBased)
			{
				ExtraSpawnPointManager.extraSpawnPoints = new Point[0];
				return;
			}
			ExtraSpawnPointManager.extraSpawnPoints = new Point[(int)PlayerTeamID.Count];
			ExtraSpawnPointManager.extraSpawnPoints[0] = new Point(Main.spawnTileX, Main.spawnTileY);
			List<Point> list = new List<Point>();
			for (int i = 1; i < (int)PlayerTeamID.Count; i++)
			{
				ExtraSpawnPointManager.GenerateExtraSpawns_TryFindSpawnRandomly(list, ExtraSpawnPointManager.GenerateExtraSpawns_GetFallbackSpawn(i, (int)PlayerTeamID.Count));
			}
			for (int j = 1; j < (int)PlayerTeamID.Count; j++)
			{
				Point point = list[WorldGen.genRand.Next(list.Count)];
				ExtraSpawnPointManager.extraSpawnPoints[j] = point;
				list.Remove(point);
			}
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x00537278 File Offset: 0x00535478
		private static bool GenerateExtraSpawns_TryFindSpawnRandomly(List<Point> spawnPoints, Point fallbackSpawn)
		{
			int num = 500;
			int num2 = 60;
			int num3 = 60;
			bool flag = true;
			LandmassData item = default(LandmassData);
			for (int i = 0; i < num; i++)
			{
				int x = 0;
				int num4 = 0;
				int num5 = (int)Main.worldSurface;
				if (flag)
				{
					x = WorldGen.genRand.Next(num2, Main.maxTilesX / 2);
				}
				else
				{
					x = WorldGen.genRand.Next(Main.maxTilesX / 2, Main.maxTilesX - num2);
				}
				if (!ExtraSpawnPointManager.settings.surface)
				{
					num5 = Main.UnderworldLayer - 50 - num3;
					num4 = WorldGen.genRand.Next((int)Main.worldSurface + 200 + num3, num5);
				}
				if (ExtraSpawnPointManager.settings.remix)
				{
					num5 = GenVars.remixMushroomLayerHigh - num3;
					num4 = GenVars.remixSurfaceLayerLow + 50 + num3;
				}
				if (ExtraSpawnPointManager.settings.skyblock)
				{
					LandmassData landmassData = default(LandmassData);
					int j = 500;
					while (j > 0)
					{
						j--;
						if (ExtraSpawnPointManager._listOfLandmasses.Count <= 0)
						{
							break;
						}
						landmassData = ExtraSpawnPointManager._listOfLandmasses[WorldGen.genRand.Next(ExtraSpawnPointManager._listOfLandmasses.Count)];
						if ((!ExtraSpawnPointManager.settings.surface || (double)landmassData.Position.Y <= Main.worldSurface) && (!ExtraSpawnPointManager.settings.remix || (landmassData.Position.Y >= (float)GenVars.remixSurfaceLayerLow && landmassData.Position.Y <= (float)GenVars.remixMushroomLayerHigh)))
						{
							break;
						}
					}
					x = (int)MathHelper.Clamp(landmassData.Top.X, (float)num2, (float)(Main.maxTilesX - num2));
					num4 = (int)MathHelper.Clamp(landmassData.Top.Y, (float)num3, (float)(Main.maxTilesY - num3));
					num5 = (int)MathHelper.Clamp((float)(num4 + landmassData.RadiusOrHalfSize / 2), (float)num3, (float)(Main.maxTilesY - num3));
					item = landmassData;
				}
				else if (ExtraSpawnPointManager.settings.roundLandmass || ExtraSpawnPointManager.settings.extraLiquid)
				{
					LandmassData landmassData2 = default(LandmassData);
					Vector2 vector = Vector2.Zero;
					int k = 500;
					while (k > 0)
					{
						k--;
						if (ExtraSpawnPointManager._listOfLandmasses.Count <= 0)
						{
							break;
						}
						landmassData2 = ExtraSpawnPointManager._listOfLandmasses[WorldGen.genRand.Next(ExtraSpawnPointManager._listOfLandmasses.Count)];
						vector = landmassData2.Position;
						if (ExtraSpawnPointManager.settings.roundLandmass)
						{
							vector = landmassData2.Top;
						}
						if ((!ExtraSpawnPointManager.settings.surface || (double)vector.Y <= Main.worldSurface) && (!ExtraSpawnPointManager.settings.remix || (vector.Y >= (float)GenVars.remixSurfaceLayerLow && vector.Y <= (float)GenVars.remixMushroomLayerHigh)) && (!ExtraSpawnPointManager.settings.roundLandmass || k <= 250 || landmassData2.RadiusOrHalfSize >= 40) && (!ExtraSpawnPointManager.settings.extraLiquid || k <= 250 || landmassData2.RadiusOrHalfSize >= 10))
						{
							break;
						}
					}
					x = (int)MathHelper.Clamp(vector.X, (float)num2, (float)(Main.maxTilesX - num2));
					num4 = (int)MathHelper.Clamp(vector.Y, (float)num3, (float)(Main.maxTilesY - num3));
					num5 = (int)MathHelper.Clamp((float)(num4 + landmassData2.RadiusOrHalfSize / 2), (float)num3, (float)(Main.maxTilesY - num3));
					item = landmassData2;
				}
				flag = !flag;
				if (ExtraSpawnPointManager.GenerateExtraSpawns_TryFindSpawnAt(spawnPoints, ref x, ref num4, num5))
				{
					spawnPoints.Add(new Point(x, num4));
					if (!item.Equals(default(LandmassData)))
					{
						ExtraSpawnPointManager._listOfLandmasses.Remove(item);
					}
					return true;
				}
			}
			spawnPoints.Add(fallbackSpawn);
			return false;
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00537620 File Offset: 0x00535820
		private static bool GenerateExtraSpawns_TryFindSpawnAt(List<Point> spawnPoints, ref int spawnX, ref int spawnY, int maxY)
		{
			spawnY = ExtraSpawnPointManager.GenerateExtraSpawns_IterateDownToFloor(spawnX, spawnY, maxY);
			int num = 50;
			if (ExtraSpawnPointManager.settings.skyblock)
			{
				num = 15;
			}
			if (ExtraSpawnPointManager.settings.extraLiquid)
			{
				num = 30;
			}
			bool flag = false;
			int teleportStartX = Math.Max(0, spawnX - num);
			int teleportRangeX = num;
			int teleportStartY = Math.Max(0, spawnY - num);
			int teleportRangeY = num;
			int[] tilesToAvoidForSpawn_TeamBasedSpawns = WorldGen.GetTilesToAvoidForSpawn_TeamBasedSpawns();
			int tilesToAvoidRange = 50;
			int maximumFallDistanceFromOrignalPoint = 100;
			Func<Tile, int, int, bool> specializedConditions = delegate(Tile tile, int x, int y)
			{
				Point point = new Point(x, y);
				int num2 = 250;
				if (ExtraSpawnPointManager.settings.extraLiquid && tile.type == 379)
				{
					return false;
				}
				if (ExtraSpawnPointManager.settings.skyblock && tile.type != 0)
				{
					return false;
				}
				if (ExtraSpawnPointManager.settings.remix)
				{
					if (WorldGen.GetWorldSize() == 0)
					{
						num2 = 150;
					}
					if (y < GenVars.remixSurfaceLayerLow || y > GenVars.remixMushroomLayerHigh)
					{
						return false;
					}
				}
				if (ExtraSpawnPointManager.settings.roundLandmass && WorldGen.GetWorldSize() == 0)
				{
					num2 = 150;
				}
				for (int i = 0; i < spawnPoints.Count; i++)
				{
					Point point2 = spawnPoints[i];
					int num3 = Math.Abs(point2.X - point.X);
					int num4 = Math.Abs(point2.Y - point.Y);
					if (num3 < num2 && num4 < num2)
					{
						return false;
					}
				}
				return true;
			};
			Vector2 vector = Utils.CheckForGoodTeleportationSpot(ref flag, teleportStartX, teleportRangeX, teleportStartY, teleportRangeY, new Utils.RandomTeleportationAttemptSettings
			{
				teleporteeSize = new Vector2(20f, 42f),
				teleporteeVelocity = Vector2.Zero,
				teleporteeGravityDirection = 1f,
				avoidLava = true,
				avoidAnyLiquid = true,
				avoidHurtTiles = true,
				avoidWalls = true,
				mostlySolidFloor = true,
				strictRange = true,
				maximumFallDistanceFromOrignalPoint = maximumFallDistanceFromOrignalPoint,
				attemptsBeforeGivingUp = 250,
				tilesToAvoid = tilesToAvoidForSpawn_TeamBasedSpawns,
				tilesToAvoidRange = tilesToAvoidRange,
				specializedConditions = specializedConditions
			});
			if (flag)
			{
				spawnX = (int)(vector.X / 16f);
				spawnY = (int)(vector.Y / 16f);
				return true;
			}
			return false;
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x0053775C File Offset: 0x0053595C
		private static Point GenerateExtraSpawns_GetFallbackSpawn(int iteration, int iterationMax)
		{
			float num = ExtraSpawnPointManager.GenerateExtraSpawns_WorldPercentileAvoidWorldSpawnIfNeeded((float)iteration / (float)iterationMax);
			int num2 = (int)((float)Main.maxTilesX * num);
			int num3 = 0;
			double worldSurface = Main.worldSurface;
			if (!ExtraSpawnPointManager.settings.surface)
			{
				num3 = (int)((float)Main.maxTilesY * 0.5f);
				int underworldLayer = Main.UnderworldLayer;
			}
			if (ExtraSpawnPointManager.settings.roundLandmass)
			{
				if (ExtraSpawnPointManager.settings.surface)
				{
					num3 = 0;
					double worldSurface2 = Main.worldSurface;
				}
				else
				{
					num3 = (int)Main.worldSurface + 100;
					int underworldLayer2 = Main.UnderworldLayer;
				}
			}
			if (ExtraSpawnPointManager.settings.remix)
			{
				num3 = (int)MathHelper.Lerp((float)GenVars.remixSurfaceLayerLow, (float)GenVars.remixMushroomLayerHigh, 0.5f);
				int remixMushroomLayerHigh = GenVars.remixMushroomLayerHigh;
			}
			num3 = ExtraSpawnPointManager.GenerateExtraSpawns_IterateDownToFloor(num2, num3, (int)Main.worldSurface);
			return new Point(num2, num3);
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x00537818 File Offset: 0x00535A18
		private static float GenerateExtraSpawns_WorldPercentileAvoidWorldSpawnIfNeeded(float currentPercentile)
		{
			if (!ExtraSpawnPointManager.settings.surface && !ExtraSpawnPointManager.settings.remix)
			{
				return currentPercentile;
			}
			float num = 0.1f;
			if (currentPercentile < 0.5f)
			{
				return Utils.Remap(currentPercentile, 0f, 0.5f, 0f, 0.5f - num, true);
			}
			return Utils.Remap(currentPercentile, 0.5f, 1f, 0.5f + num, 1f, true);
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x00537888 File Offset: 0x00535A88
		private static int GenerateExtraSpawns_IterateDownToFloor(int spawnX, int spawnY, int maxY)
		{
			if (spawnY > Main.maxTilesY - 5)
			{
				spawnY = Main.maxTilesY - 5;
			}
			else if (spawnY < 5)
			{
				spawnY = 5;
			}
			if (maxY <= spawnY)
			{
				return spawnY;
			}
			bool extraLiquid = ExtraSpawnPointManager.settings.extraLiquid;
			int num = spawnY;
			while (num < maxY && num < Main.maxTilesY)
			{
				Tile tile = Main.tile[spawnX, num];
				if (tile.active() && (extraLiquid || tile.liquid <= 0) && (tile.type < 0 || Main.tileSolid[(int)tile.type]))
				{
					return num;
				}
				num++;
			}
			return spawnY;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x00537914 File Offset: 0x00535B14
		public static void PrepareExtraSpawns()
		{
			ExtraSpawnPointManager.GenerateExtraSpawns_Setup();
			for (int i = 1; i < ExtraSpawnPointManager.extraSpawnPoints.Length; i++)
			{
				Point point = ExtraSpawnPointManager.extraSpawnPoints[i];
				if ((double)point.Y >= Main.worldSurface && point.Y < Main.UnderworldLayer)
				{
					WorldGen.PlaceTorchesAroundSpawn(point.X, point.Y);
				}
			}
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x00537970 File Offset: 0x00535B70
		public static void Clear()
		{
			ExtraSpawnPointManager.extraSpawnPoints = new Point[0];
			ExtraSpawnPointManager.settings = default(ExtraSpawnSettings);
			ExtraSpawnPointManager._listOfLandmasses.Clear();
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x00537994 File Offset: 0x00535B94
		public static void Read(BinaryReader reader, bool networking = false)
		{
			byte b = reader.ReadByte();
			ExtraSpawnPointManager.extraSpawnPoints = new Point[(int)b];
			for (int i = 0; i < (int)b; i++)
			{
				int x = (int)reader.ReadInt16();
				int y = (int)reader.ReadInt16();
				ExtraSpawnPointManager.extraSpawnPoints[i] = new Point(x, y);
			}
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x005379E0 File Offset: 0x00535BE0
		public static void Write(BinaryWriter writer, bool networking = false)
		{
			writer.Write((byte)ExtraSpawnPointManager.extraSpawnPoints.Length);
			for (int i = 0; i < ExtraSpawnPointManager.extraSpawnPoints.Length; i++)
			{
				writer.Write((short)ExtraSpawnPointManager.extraSpawnPoints[i].X);
				writer.Write((short)ExtraSpawnPointManager.extraSpawnPoints[i].Y);
			}
		}

		// Token: 0x04004CCF RID: 19663
		public static Point[] extraSpawnPoints = new Point[0];

		// Token: 0x04004CD0 RID: 19664
		public static ExtraSpawnSettings settings = default(ExtraSpawnSettings);

		// Token: 0x04004CD1 RID: 19665
		private static List<LandmassData> _listOfLandmasses = new List<LandmassData>();
	}
}
