using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Achievements;
using Terraria.GameInput;
using Terraria.Social;
using Terraria.Social.Base;

namespace Terraria.UI
{
	// Token: 0x020000F5 RID: 245
	public class InGameNotificationsTracker
	{
		// Token: 0x0600192F RID: 6447 RVA: 0x004E6EA4 File Offset: 0x004E50A4
		public static void Initialize()
		{
			Main.Achievements.OnAchievementCompleted += InGameNotificationsTracker.AddCompleted;
			SocialAPI.JoinRequests.OnRequestAdded += InGameNotificationsTracker.JoinRequests_OnRequestAdded;
			SocialAPI.JoinRequests.OnRequestRemoved += InGameNotificationsTracker.JoinRequests_OnRequestRemoved;
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x004E6EF3 File Offset: 0x004E50F3
		private static void JoinRequests_OnRequestAdded(UserJoinToServerRequest request)
		{
			InGameNotificationsTracker.AddJoinRequest(request);
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x004E6EFC File Offset: 0x004E50FC
		private static void JoinRequests_OnRequestRemoved(UserJoinToServerRequest request)
		{
			for (int i = InGameNotificationsTracker._notifications.Count - 1; i >= 0; i--)
			{
				if (InGameNotificationsTracker._notifications[i].CreationObject == request)
				{
					InGameNotificationsTracker._notifications.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x004E6F40 File Offset: 0x004E5140
		public static void DrawInGame(SpriteBatch sb)
		{
			float num = (float)(Main.screenHeight - 40);
			if (PlayerInput.UsingGamepad)
			{
				num -= 25f;
			}
			Vector2 vector = new Vector2((float)(Main.screenWidth / 2), num);
			foreach (IInGameNotification inGameNotification in InGameNotificationsTracker._notifications)
			{
				inGameNotification.DrawInGame(sb, vector);
				inGameNotification.PushAnchor(ref vector);
				if (vector.Y < -100f)
				{
					break;
				}
			}
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x004E6FD4 File Offset: 0x004E51D4
		public static void DrawInIngameOptions(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointIdLocalIndexToUse)
		{
			int num = 4;
			int num2 = area.Height / 5 - num;
			Rectangle area2 = new Rectangle(area.X, area.Y, area.Width - 6, num2);
			int num3 = 0;
			foreach (IInGameNotification inGameNotification in InGameNotificationsTracker._notifications)
			{
				inGameNotification.DrawInNotificationsArea(spriteBatch, area2, ref gamepadPointIdLocalIndexToUse);
				area2.Y += num2 + num;
				num3++;
				if (num3 >= 5)
				{
					break;
				}
			}
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x004E706C File Offset: 0x004E526C
		public static void AddCompleted(Achievement achievement)
		{
			if (Main.netMode == 2)
			{
				return;
			}
			InGameNotificationsTracker._notifications.Add(new InGamePopups.AchievementUnlockedPopup(achievement));
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x004E7087 File Offset: 0x004E5287
		public static void AddJoinRequest(UserJoinToServerRequest request)
		{
			if (Main.netMode == 2)
			{
				return;
			}
			InGameNotificationsTracker._notifications.Add(new InGamePopups.PlayerWantsToJoinGamePopup(request));
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x004E70A2 File Offset: 0x004E52A2
		public static void Clear()
		{
			InGameNotificationsTracker._notifications.Clear();
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x004E70B0 File Offset: 0x004E52B0
		public static void Update()
		{
			for (int i = 0; i < InGameNotificationsTracker._notifications.Count; i++)
			{
				InGameNotificationsTracker._notifications[i].Update();
				if (InGameNotificationsTracker._notifications[i].ShouldBeRemoved)
				{
					InGameNotificationsTracker._notifications.Remove(InGameNotificationsTracker._notifications[i]);
					i--;
				}
			}
		}

		// Token: 0x0400132D RID: 4909
		private static List<IInGameNotification> _notifications = new List<IInGameNotification>();
	}
}
