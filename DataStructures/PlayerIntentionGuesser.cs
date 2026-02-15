using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria.DataStructures
{
	// Token: 0x02000535 RID: 1333
	public class PlayerIntentionGuesser
	{
		// Token: 0x0600372C RID: 14124 RVA: 0x0062D564 File Offset: 0x0062B764
		public void Track(Player player, int x, int y, GuessedPlayerIntention intention)
		{
			if (this.PlayerActiveActionTimeLeft == 0)
			{
				return;
			}
			this.LastX = x;
			this.LastY = y;
			this.Intention = intention;
			this.LastPosition = player.position;
			this.LastCenter = player.Center;
			this.LastDirection = player.direction;
			this.LastWidth = player.width;
			this.LastMouse = Main.MouseWorld;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x0062D5CB File Offset: 0x0062B7CB
		public void AllowTracking(int time = 60)
		{
			this.PlayerActiveActionTimeLeft = time;
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x0062D5D4 File Offset: 0x0062B7D4
		public void Update(Player player)
		{
			if (player.whoAmI != Main.myPlayer)
			{
				return;
			}
			this.TimeWithIntention++;
			if (this.PlayerActiveActionTimeLeft > 0)
			{
				this.PlayerActiveActionTimeLeft--;
			}
			if (this.Intention == GuessedPlayerIntention.None)
			{
				return;
			}
			float num = player.Center.Distance(this.LastCenter);
			bool flag = false;
			if (num > 80f)
			{
				flag = true;
			}
			if (player.controlJump)
			{
				flag = true;
			}
			bool usingOrReusingItem = player.UsingOrReusingItem;
			if (usingOrReusingItem && this.Intention == GuessedPlayerIntention.HarvestTreasure && player.HeldItem.pick <= 0)
			{
				flag = true;
			}
			if (usingOrReusingItem && this.Intention == GuessedPlayerIntention.HarvestTrees && player.HeldItem.axe <= 0)
			{
				flag = true;
			}
			if (this.TimeWithIntention >= 480)
			{
				flag = true;
			}
			if (player.dead)
			{
				flag = true;
			}
			if (flag)
			{
				this.Intention = GuessedPlayerIntention.None;
				this.TimeWithIntention = 0;
				return;
			}
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x0062D6AC File Offset: 0x0062B8AC
		public void PrepareUsageProxy(Player player, int itemType, int areaInflateWidth, int areaInflateHeight)
		{
			this.UsageProxy.player = player;
			if (this.UsageProxy.item == null)
			{
				this.UsageProxy.item = new Item();
			}
			this.UsageProxy.item.SetDefaults(itemType, null);
			this.UsageProxy.position = this.LastPosition;
			this.UsageProxy.Center = this.LastCenter;
			this.UsageProxy.mouse = this.LastMouse;
			this.UsageProxy.screenTargetX = this.LastX;
			this.UsageProxy.screenTargetY = this.LastY;
			this.UsageProxy.screenTargetX = Utils.Clamp<int>(this.UsageProxy.screenTargetX, 10, Main.maxTilesX - 10);
			this.UsageProxy.screenTargetY = Utils.Clamp<int>(this.UsageProxy.screenTargetY, 10, Main.maxTilesY - 10);
			Rectangle value = new Rectangle(this.LastX, this.LastY, 1, 1);
			value.Inflate(areaInflateWidth, areaInflateHeight);
			Rectangle value2 = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
			value2.Inflate(-10, -10);
			Rectangle rectangle = default(Rectangle);
			rectangle = Rectangle.Intersect(value, value2);
			this.UsageProxy.reachableStartX = rectangle.Left;
			this.UsageProxy.reachableStartY = rectangle.Top;
			this.UsageProxy.reachableEndX = rectangle.Right;
			this.UsageProxy.reachableEndY = rectangle.Bottom;
		}

		// Token: 0x04005B46 RID: 23366
		public int LastX;

		// Token: 0x04005B47 RID: 23367
		public int LastY;

		// Token: 0x04005B48 RID: 23368
		public Vector2 LastPosition;

		// Token: 0x04005B49 RID: 23369
		public Vector2 LastCenter;

		// Token: 0x04005B4A RID: 23370
		public Vector2 LastMouse;

		// Token: 0x04005B4B RID: 23371
		public int LastDirection;

		// Token: 0x04005B4C RID: 23372
		public int LastWidth;

		// Token: 0x04005B4D RID: 23373
		public GuessedPlayerIntention Intention;

		// Token: 0x04005B4E RID: 23374
		public SmartCursorHelper.SmartCursorUsageInfo UsageProxy = new SmartCursorHelper.SmartCursorUsageInfo();

		// Token: 0x04005B4F RID: 23375
		public int TimeWithIntention;

		// Token: 0x04005B50 RID: 23376
		public int PlayerActiveActionTimeLeft;
	}
}
