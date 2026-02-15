using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent
{
	// Token: 0x0200025E RID: 606
	public static class LucyAxeMessage
	{
		// Token: 0x06002357 RID: 9047 RVA: 0x0053D768 File Offset: 0x0053B968
		private static string GetCategoryName(LucyAxeMessage.MessageSource source)
		{
			switch (source)
			{
			default:
				return "LucyTheAxe_Idle";
			case LucyAxeMessage.MessageSource.Storage:
				return "LucyTheAxe_Storage";
			case LucyAxeMessage.MessageSource.ThrownAway:
				return "LucyTheAxe_ThrownAway";
			case LucyAxeMessage.MessageSource.PickedUp:
				return "LucyTheAxe_PickedUp";
			case LucyAxeMessage.MessageSource.ChoppedTree:
				return "LucyTheAxe_ChoppedTree";
			case LucyAxeMessage.MessageSource.ChoppedGemTree:
				return "LucyTheAxe_GemTree";
			case LucyAxeMessage.MessageSource.ChoppedCactus:
				return "LucyTheAxe_ChoppedCactus";
			}
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0053D7C0 File Offset: 0x0053B9C0
		public static void Initialize()
		{
			ItemSlot.OnItemTransferred += LucyAxeMessage.ItemSlot_OnItemTransferred;
			Player.Hooks.OnEnterWorld += LucyAxeMessage.Hooks_OnEnterWorld;
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0053D7E4 File Offset: 0x0053B9E4
		private static void Hooks_OnEnterWorld(Player player)
		{
			if (player == Main.LocalPlayer)
			{
				LucyAxeMessage.GiveIdleMessageCooldown();
			}
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0053D7F4 File Offset: 0x0053B9F4
		public static void UpdateMessageCooldowns()
		{
			for (int i = 0; i < LucyAxeMessage._messageCooldownsByType.Length; i++)
			{
				if (LucyAxeMessage._messageCooldownsByType[i] > 0)
				{
					LucyAxeMessage._messageCooldownsByType[i]--;
				}
			}
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0053D830 File Offset: 0x0053BA30
		public static void TryPlayingIdleMessage()
		{
			LucyAxeMessage.MessageSource messageSource = LucyAxeMessage.MessageSource.Idle;
			if (LucyAxeMessage._messageCooldownsByType[(int)messageSource] > 0)
			{
				return;
			}
			Player localPlayer = Main.LocalPlayer;
			LucyAxeMessage.Create(messageSource, localPlayer.Top, new Vector2(Main.rand.NextFloatDirection() * 7f, -2f + Main.rand.NextFloat() * -2f));
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x0053D888 File Offset: 0x0053BA88
		private static void ItemSlot_OnItemTransferred(ItemSlot.ItemTransferInfo info)
		{
			if (info.ItemType != 5095)
			{
				return;
			}
			bool flag = LucyAxeMessage.CountsAsStorage(info.FromContenxt);
			bool flag2 = LucyAxeMessage.CountsAsStorage(info.ToContext);
			if (flag == flag2)
			{
				return;
			}
			LucyAxeMessage.MessageSource messageSource = flag ? LucyAxeMessage.MessageSource.PickedUp : LucyAxeMessage.MessageSource.Storage;
			if (LucyAxeMessage._messageCooldownsByType[(int)messageSource] > 0)
			{
				return;
			}
			LucyAxeMessage.PutMessageTypeOnCooldown(messageSource, 420);
			Player localPlayer = Main.LocalPlayer;
			LucyAxeMessage.Create(messageSource, localPlayer.Top, new Vector2((float)(localPlayer.direction * 7), -2f));
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0053D903 File Offset: 0x0053BB03
		private static void GiveIdleMessageCooldown()
		{
			LucyAxeMessage.PutMessageTypeOnCooldown(LucyAxeMessage.MessageSource.Idle, Main.rand.Next(7200, 14400));
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0053D91F File Offset: 0x0053BB1F
		public static void PutMessageTypeOnCooldown(LucyAxeMessage.MessageSource source, int timeInFrames)
		{
			LucyAxeMessage._messageCooldownsByType[(int)source] = timeInFrames;
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0053D929 File Offset: 0x0053BB29
		private static bool CountsAsStorage(int itemSlotContext)
		{
			return itemSlotContext == 3 || itemSlotContext == 6 || itemSlotContext == 15;
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0053D93B File Offset: 0x0053BB3B
		public static void TryCreatingMessageWithCooldown(LucyAxeMessage.MessageSource messageSource, Vector2 position, Vector2 velocity, int cooldownTimeInTicks)
		{
			if (Main.netMode == 2)
			{
				return;
			}
			if (LucyAxeMessage._messageCooldownsByType[(int)messageSource] > 0)
			{
				return;
			}
			LucyAxeMessage.PutMessageTypeOnCooldown(messageSource, cooldownTimeInTicks);
			LucyAxeMessage.Create(messageSource, position, velocity);
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0053D960 File Offset: 0x0053BB60
		public static void Create(LucyAxeMessage.MessageSource source, Vector2 position, Vector2 velocity)
		{
			if (Main.netMode == 2)
			{
				return;
			}
			LucyAxeMessage.GiveIdleMessageCooldown();
			LucyAxeMessage.SpawnPopupText(source, (int)LucyAxeMessage._variation, position, velocity);
			LucyAxeMessage.PlaySound(source, position);
			LucyAxeMessage.SpawnEmoteBubble();
			if (Main.netMode == 1)
			{
				NetMessage.SendData(141, -1, -1, null, (int)source, (float)LucyAxeMessage._variation, velocity.X, velocity.Y, (int)position.X, (int)position.Y, 0);
			}
			LucyAxeMessage._variation += 1;
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0053D9D8 File Offset: 0x0053BBD8
		private static void SpawnEmoteBubble()
		{
			EmoteBubble.MakeLocalPlayerEmote(149);
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0053D9E4 File Offset: 0x0053BBE4
		public static void CreateFromNet(LucyAxeMessage.MessageSource source, byte variation, Vector2 position, Vector2 velocity)
		{
			LucyAxeMessage.SpawnPopupText(source, (int)variation, position, velocity);
			LucyAxeMessage.PlaySound(source, position);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0053D9F6 File Offset: 0x0053BBF6
		private static void PlaySound(LucyAxeMessage.MessageSource source, Vector2 position)
		{
			SoundEngine.PlaySound(SoundID.LucyTheAxeTalk, position, 0f, 1f);
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0053DA10 File Offset: 0x0053BC10
		private static void SpawnPopupText(LucyAxeMessage.MessageSource source, int variationUnwrapped, Vector2 position, Vector2 velocity)
		{
			string textForVariation = LucyAxeMessage.GetTextForVariation(source, variationUnwrapped);
			PopupText.NewText(new AdvancedPopupRequest
			{
				Text = textForVariation,
				DurationInFrames = 420,
				Velocity = velocity,
				Color = new Color(184, 96, 98) * 1.15f
			}, position);
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0053DA70 File Offset: 0x0053BC70
		private static string GetTextForVariation(LucyAxeMessage.MessageSource source, int variationUnwrapped)
		{
			string categoryName = LucyAxeMessage.GetCategoryName(source);
			return LanguageManager.Instance.IndexedFromCategory(categoryName, variationUnwrapped).Value;
		}

		// Token: 0x04004D62 RID: 19810
		private static byte _variation;

		// Token: 0x04004D63 RID: 19811
		private static int[] _messageCooldownsByType = new int[7];

		// Token: 0x020007E6 RID: 2022
		public enum MessageSource
		{
			// Token: 0x040070F9 RID: 28921
			Idle,
			// Token: 0x040070FA RID: 28922
			Storage,
			// Token: 0x040070FB RID: 28923
			ThrownAway,
			// Token: 0x040070FC RID: 28924
			PickedUp,
			// Token: 0x040070FD RID: 28925
			ChoppedTree,
			// Token: 0x040070FE RID: 28926
			ChoppedGemTree,
			// Token: 0x040070FF RID: 28927
			ChoppedCactus,
			// Token: 0x04007100 RID: 28928
			Count
		}
	}
}
