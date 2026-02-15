using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x02000267 RID: 615
	public class TeleportHelpers
	{
		// Token: 0x060023D5 RID: 9173 RVA: 0x00547C74 File Offset: 0x00545E74
		public static bool FindClosestTeleportSpotNoSpace(Player player, out Vector2 resultPosition)
		{
			bool result = false;
			resultPosition = player.position;
			player.velocity = Vector2.Zero;
			Vector2 value = new Vector2((float)player.width * 0.5f, (float)player.height);
			Vector2 bottom = player.Bottom;
			Point point = bottom.ToTileCoordinates();
			int num = point.X - 25;
			int num2 = point.X + 25;
			int num3 = point.Y - 25;
			int num4 = point.Y + 25;
			num = Utils.Clamp<int>(num, 40, Main.maxTilesX - 40);
			num2 = Utils.Clamp<int>(num2, 40, Main.maxTilesX - 40);
			num3 = Utils.Clamp<int>(num3, 40, Main.maxTilesY - 40);
			num4 = Utils.Clamp<int>(num4, 40, Main.maxTilesY - 40);
			float num5 = float.MaxValue;
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					Vector2 vector = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 15)) - value;
					Tile tile = Main.tile[i, j];
					Tile tile2 = Main.tile[i, j + 1];
					bool flag = WorldGen.SolidOrSlopedTile(tile) || tile.liquid > 0;
					bool flag2 = WorldGen.SolidOrSlopedTile(tile2) && tile2.liquid == 0;
					if (!TeleportHelpers.TileIsDangerous(i, j) && !flag && flag2 && !Collision.LavaCollision(vector, player.width, player.height) && !Collision.AnyHurtingTiles(vector, player.width, player.height) && !Collision.SolidCollision(vector, player.width, player.height))
					{
						float num6 = (vector - bottom).Length();
						if (num6 < num5)
						{
							resultPosition = vector;
							num5 = num6;
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x00547E5C File Offset: 0x0054605C
		public static bool RequestMagicConchTeleportPosition(Player player, int crawlOffsetX, int startX, out Point landingPoint)
		{
			landingPoint = default(Point);
			Point point = new Point(startX, 50);
			int num = 1;
			int num2 = -1;
			int num3 = 1;
			int num4 = 0;
			int num5 = 5000;
			Vector2 value = new Vector2((float)player.width * 0.5f, (float)player.height);
			int num6 = 40;
			bool flag = WorldGen.SolidOrSlopedTile(Main.tile[point.X, point.Y]);
			int num7 = 0;
			int num8 = 400;
			while (num4 < num5 && num7 < num8)
			{
				num4++;
				Tile tile = Main.tile[point.X, point.Y];
				Tile tile2 = Main.tile[point.X, point.Y + num3];
				bool flag2 = WorldGen.SolidOrSlopedTile(tile) || tile.liquid > 0;
				bool flag3 = WorldGen.SolidOrSlopedTile(tile2) || tile2.liquid > 0;
				if (TeleportHelpers.IsInSolidTilesExtended(new Vector2((float)(point.X * 16 + 8), (float)(point.Y * 16 + 15)) - value, player.velocity, player.width, player.height, (int)player.gravDir))
				{
					if (flag)
					{
						point.Y += num;
					}
					else
					{
						point.Y += num2;
					}
				}
				else if (flag2)
				{
					if (flag)
					{
						point.Y += num;
					}
					else
					{
						point.Y += num2;
					}
				}
				else
				{
					flag = false;
					if (!TeleportHelpers.IsInSolidTilesExtended(new Vector2((float)(point.X * 16 + 8), (float)(point.Y * 16 + 15 + 16)) - value, player.velocity, player.width, player.height, (int)player.gravDir) && !flag3 && (double)point.Y < Main.worldSurface)
					{
						point.Y += num;
					}
					else if (tile2.liquid > 0)
					{
						point.X += crawlOffsetX;
						num7++;
					}
					else if (TeleportHelpers.TileIsDangerous(point.X, point.Y))
					{
						point.X += crawlOffsetX;
						num7++;
					}
					else if (TeleportHelpers.TileIsDangerous(point.X, point.Y + num3))
					{
						point.X += crawlOffsetX;
						num7++;
					}
					else
					{
						if (point.Y >= num6)
						{
							break;
						}
						point.Y += num;
					}
				}
			}
			if (num4 == num5 || num7 >= num8)
			{
				return false;
			}
			if (!WorldGen.InWorld(point.X, point.Y, 40))
			{
				return false;
			}
			bool flag4 = false;
			for (int i = 0; i < 10; i++)
			{
				int num9 = point.Y + i;
				Tile tile3 = Main.tile[point.X, num9];
				if (WorldGen.SolidOrSlopedTile(tile3) || tile3.liquid > 0)
				{
					flag4 = true;
					break;
				}
			}
			if (!flag4)
			{
				return false;
			}
			landingPoint = point;
			return true;
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x00548154 File Offset: 0x00546354
		private static bool TileIsDangerous(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return (tile.liquid > 0 && tile.lava()) || (tile.wall == 87 && (double)y > Main.worldSurface && !NPC.downedPlantBoss) || (Main.wallDungeon[(int)tile.wall] && (double)y > Main.worldSurface && !NPC.downedBoss3);
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x005481C0 File Offset: 0x005463C0
		private static bool IsInSolidTilesExtended(Vector2 testPosition, Vector2 playerVelocity, int width, int height, int gravDir)
		{
			if (Collision.LavaCollision(testPosition, width, height))
			{
				return true;
			}
			if (Collision.AnyHurtingTiles(testPosition, width, height))
			{
				return true;
			}
			if (Collision.SolidCollision(testPosition, width, height))
			{
				return true;
			}
			Vector2 vector = Vector2.UnitX * 16f;
			if (Collision.TileCollision(testPosition - vector, vector, width, height, true, true, gravDir, false, false, true) != vector)
			{
				return true;
			}
			vector = -Vector2.UnitX * 16f;
			if (Collision.TileCollision(testPosition - vector, vector, width, height, true, true, gravDir, false, false, true) != vector)
			{
				return true;
			}
			vector = Vector2.UnitY * 16f;
			if (Collision.TileCollision(testPosition - vector, vector, width, height, true, true, gravDir, false, false, true) != vector)
			{
				return true;
			}
			vector = -Vector2.UnitY * 16f;
			return Collision.TileCollision(testPosition - vector, vector, width, height, true, true, gravDir, false, false, true) != vector;
		}
	}
}
