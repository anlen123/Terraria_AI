using System;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004AF RID: 1199
	public class RegularDungeonRoom : DungeonRoom
	{
		// Token: 0x06003447 RID: 13383 RVA: 0x005FF5F7 File Offset: 0x005FD7F7
		public RegularDungeonRoom(DungeonRoomSettings settings) : base(settings)
		{
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x00602A14 File Offset: 0x00600C14
		public override void CalculateRoom(DungeonData data)
		{
			this.calculated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.RegularRoom(data, x, y, false);
			this.calculated = true;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x00602A5C File Offset: 0x00600C5C
		public override bool GenerateRoom(DungeonData data)
		{
			this.generated = false;
			int x = this.settings.RoomPosition.X;
			int y = this.settings.RoomPosition.Y;
			this.RegularRoom(data, x, y, true);
			this.generated = true;
			return true;
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x00602AA4 File Offset: 0x00600CA4
		public void RegularRoom(DungeonData data, int i, int j, bool generating)
		{
			UnifiedRandom unifiedRandom = new UnifiedRandom(this.settings.RandomSeed);
			RegularDungeonRoomSettings regularDungeonRoomSettings = (RegularDungeonRoomSettings)this.settings;
			ushort brickTileType = this.settings.StyleData.BrickTileType;
			ushort brickWallType = this.settings.StyleData.BrickWallType;
			Point center = new Point(i, j);
			if (base.Processed)
			{
				center = this.InnerBounds.Center;
			}
			int num = 6 + unifiedRandom.Next(7);
			int num2 = 8;
			if (regularDungeonRoomSettings.OverrideInnerBoundsSize > 0)
			{
				num = regularDungeonRoomSettings.OverrideInnerBoundsSize;
			}
			if (regularDungeonRoomSettings.OverrideOuterBoundsSize > 0)
			{
				num2 = regularDungeonRoomSettings.OverrideOuterBoundsSize;
			}
			if (base.Processed)
			{
				num = this._innerBoundsSize;
			}
			int num3 = num + num2;
			this.InnerBounds.SetBounds(center.X, center.Y, center.X, center.Y);
			this.OuterBounds.SetBounds(center.X, center.Y, center.X, center.Y);
			this.OuterBounds.UpdateBounds(center.X - num3, center.Y - num3, center.X + num3, center.Y + num3);
			this.InnerBounds.UpdateBounds(this.OuterBounds.Left + num2, this.OuterBounds.Top + num2, this.OuterBounds.Right - num2, this.OuterBounds.Bottom - num2);
			base.GenerateDungeonSquareRoom(data, this.InnerBounds, this.OuterBounds, center, brickTileType, brickWallType, num, num3, generating, generating);
			this._innerBoundsSize = num;
			this.InnerBounds.CalculateHitbox();
			this.OuterBounds.CalculateHitbox();
		}

		// Token: 0x040059D9 RID: 23001
		public int _innerBoundsSize;
	}
}
