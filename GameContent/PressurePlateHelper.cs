using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x0200027F RID: 639
	public class PressurePlateHelper
	{
		// Token: 0x0600246D RID: 9325 RVA: 0x0054D068 File Offset: 0x0054B268
		public static void Update()
		{
			if (!PressurePlateHelper.NeedsFirstUpdate)
			{
				return;
			}
			foreach (Point location in PressurePlateHelper.PressurePlatesPressed.Keys)
			{
				PressurePlateHelper.PokeLocation(location);
			}
			PressurePlateHelper.PressurePlatesPressed.Clear();
			PressurePlateHelper.NeedsFirstUpdate = false;
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0054D0D4 File Offset: 0x0054B2D4
		public static void Reset()
		{
			PressurePlateHelper.PressurePlatesPressed.Clear();
			for (int i = 0; i < PressurePlateHelper.PlayerLastPosition.Length; i++)
			{
				PressurePlateHelper.PlayerLastPosition[i] = Vector2.Zero;
			}
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x0054D110 File Offset: 0x0054B310
		public static void ResetPlayer(int player)
		{
			Point[] array = PressurePlateHelper.PressurePlatesPressed.Keys.ToArray<Point>();
			for (int i = 0; i < array.Length; i++)
			{
				PressurePlateHelper.MoveAwayFrom(array[i], player);
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x0054D148 File Offset: 0x0054B348
		public static void UpdatePlayerPosition(Player player)
		{
			Point point = new Point(1, 1);
			Vector2 value = point.ToVector2();
			List<Point> tilesIn = Collision.GetTilesIn(PressurePlateHelper.PlayerLastPosition[player.whoAmI] + value, PressurePlateHelper.PlayerLastPosition[player.whoAmI] + player.Size - value);
			List<Point> tilesIn2 = Collision.GetTilesIn(player.TopLeft + value, player.BottomRight - value);
			Rectangle hitbox = player.Hitbox;
			hitbox.Inflate(-point.X, -point.Y);
			Rectangle hitbox2 = player.Hitbox;
			hitbox2.X = (int)PressurePlateHelper.PlayerLastPosition[player.whoAmI].X;
			hitbox2.Y = (int)PressurePlateHelper.PlayerLastPosition[player.whoAmI].Y;
			hitbox2.Inflate(-point.X, -point.Y);
			for (int i = 0; i < tilesIn.Count; i++)
			{
				Point point2 = tilesIn[i];
				Tile tile = Main.tile[point2.X, point2.Y];
				if (tile.active() && tile.type == 428)
				{
					PressurePlateHelper.pressurePlateBounds.X = point2.X * 16;
					PressurePlateHelper.pressurePlateBounds.Y = point2.Y * 16 + 16 - PressurePlateHelper.pressurePlateBounds.Height;
					if (!hitbox.Intersects(PressurePlateHelper.pressurePlateBounds) && !tilesIn2.Contains(point2))
					{
						PressurePlateHelper.MoveAwayFrom(point2, player.whoAmI);
					}
				}
			}
			for (int j = 0; j < tilesIn2.Count; j++)
			{
				Point point3 = tilesIn2[j];
				Tile tile2 = Main.tile[point3.X, point3.Y];
				if (tile2.active() && tile2.type == 428)
				{
					PressurePlateHelper.pressurePlateBounds.X = point3.X * 16;
					PressurePlateHelper.pressurePlateBounds.Y = point3.Y * 16 + 16 - PressurePlateHelper.pressurePlateBounds.Height;
					if (hitbox.Intersects(PressurePlateHelper.pressurePlateBounds) && (!tilesIn.Contains(point3) || !hitbox2.Intersects(PressurePlateHelper.pressurePlateBounds)))
					{
						PressurePlateHelper.MoveInto(point3, player.whoAmI);
					}
				}
			}
			PressurePlateHelper.PlayerLastPosition[player.whoAmI] = player.position;
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x0054D3BC File Offset: 0x0054B5BC
		public static void DestroyPlate(Point location)
		{
			bool[] array;
			if (PressurePlateHelper.PressurePlatesPressed.TryGetValue(location, out array))
			{
				PressurePlateHelper.PressurePlatesPressed.Remove(location);
				PressurePlateHelper.PokeLocation(location);
			}
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x0054D3EA File Offset: 0x0054B5EA
		private static void UpdatePlatePosition(Point location, int player, bool onIt)
		{
			if (onIt)
			{
				PressurePlateHelper.MoveInto(location, player);
				return;
			}
			PressurePlateHelper.MoveAwayFrom(location, player);
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x0054D400 File Offset: 0x0054B600
		private static void MoveInto(Point location, int player)
		{
			bool[] array;
			if (PressurePlateHelper.PressurePlatesPressed.TryGetValue(location, out array))
			{
				array[player] = true;
				return;
			}
			object entityCreationLock = PressurePlateHelper.EntityCreationLock;
			lock (entityCreationLock)
			{
				PressurePlateHelper.PressurePlatesPressed[location] = new bool[255];
			}
			PressurePlateHelper.PressurePlatesPressed[location][player] = true;
			PressurePlateHelper.PokeLocation(location);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x0054D478 File Offset: 0x0054B678
		private static void MoveAwayFrom(Point location, int player)
		{
			bool[] array;
			if (PressurePlateHelper.PressurePlatesPressed.TryGetValue(location, out array))
			{
				array[player] = false;
				bool flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					object entityCreationLock = PressurePlateHelper.EntityCreationLock;
					lock (entityCreationLock)
					{
						PressurePlateHelper.PressurePlatesPressed.Remove(location);
					}
					PressurePlateHelper.PokeLocation(location);
				}
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x0054D4F4 File Offset: 0x0054B6F4
		private static void PokeLocation(Point location)
		{
			if (Main.netMode != 1)
			{
				Wiring.blockPlayerTeleportationForOneIteration = true;
				Wiring.HitSwitch(location.X, location.Y);
				NetMessage.SendData(59, -1, -1, null, location.X, (float)location.Y, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x04004F15 RID: 20245
		public static object EntityCreationLock = new object();

		// Token: 0x04004F16 RID: 20246
		public static Dictionary<Point, bool[]> PressurePlatesPressed = new Dictionary<Point, bool[]>();

		// Token: 0x04004F17 RID: 20247
		public static bool NeedsFirstUpdate;

		// Token: 0x04004F18 RID: 20248
		private static Vector2[] PlayerLastPosition = new Vector2[255];

		// Token: 0x04004F19 RID: 20249
		private static Rectangle pressurePlateBounds = new Rectangle(0, 0, 16, 10);
	}
}
