using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x0200055E RID: 1374
	public struct TileReachCheckSettings
	{
		// Token: 0x060037B1 RID: 14257 RVA: 0x0062EE48 File Offset: 0x0062D048
		public void GetRanges(out int x, out int y)
		{
			x = Player.tileRangeX * this.TileRangeMultiplier;
			y = Player.tileRangeY * this.TileRangeMultiplier;
			if (this.TileReachLimit != null)
			{
				if (x > this.TileReachLimit.Value)
				{
					x = this.TileReachLimit.Value;
				}
				if (y > this.TileReachLimit.Value)
				{
					y = this.TileReachLimit.Value;
				}
			}
			if (this.OverrideXReach != null)
			{
				x = this.OverrideXReach.Value;
			}
			if (this.OverrideYReach != null)
			{
				y = this.OverrideYReach.Value;
			}
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x0062EEEC File Offset: 0x0062D0EC
		public void GetTileRegion(Player player, out int LX, out int LY, out int HX, out int HY, int TB = 0)
		{
			int num;
			int num2;
			this.GetRanges(out num, out num2);
			num += TB;
			num2 += TB;
			LX = (int)(player.position.X / 16f) - num + 1;
			HX = (int)Math.Ceiling((double)((player.position.X + (float)player.width) / 16f)) + num - 2;
			LY = (int)(player.position.Y / 16f) - num2 + 1;
			HY = (int)Math.Ceiling((double)((player.position.Y + (float)player.height) / 16f)) + num2 - 2;
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x0062EF8C File Offset: 0x0062D18C
		public Rectangle GetTileRegion(Player player, int TB = 0)
		{
			int num;
			int num2;
			int num3;
			int num4;
			this.GetTileRegion(player, out num, out num2, out num3, out num4, TB);
			return new Rectangle(num, num2, num3 - num, num4 - num2);
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x0062EFB8 File Offset: 0x0062D1B8
		public void GetWorldRegion(Player player, out int LX, out int LY, out int HX, out int HY, int TB = 0)
		{
			this.GetTileRegion(player, out LX, out LY, out HX, out HY, TB);
			LX *= 16;
			LY *= 16;
			HX *= 16;
			HY *= 16;
			HX += 15;
			HY += 15;
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x0062F008 File Offset: 0x0062D208
		public Rectangle GetWorldRegion(Player player, int TB = 0)
		{
			int num;
			int num2;
			int num3;
			int num4;
			this.GetWorldRegion(player, out num, out num2, out num3, out num4, TB);
			return new Rectangle(num, num2, num3 - num, num4 - num2);
		}

		// Token: 0x04005BB2 RID: 23474
		public int TileRangeMultiplier;

		// Token: 0x04005BB3 RID: 23475
		public int? TileReachLimit;

		// Token: 0x04005BB4 RID: 23476
		public int? OverrideXReach;

		// Token: 0x04005BB5 RID: 23477
		public int? OverrideYReach;

		// Token: 0x04005BB6 RID: 23478
		public static readonly TileReachCheckSettings Simple = new TileReachCheckSettings
		{
			TileRangeMultiplier = 1,
			TileReachLimit = new int?(20)
		};

		// Token: 0x04005BB7 RID: 23479
		public static readonly TileReachCheckSettings Pylons = new TileReachCheckSettings
		{
			OverrideXReach = new int?(60),
			OverrideYReach = new int?(60)
		};
	}
}
