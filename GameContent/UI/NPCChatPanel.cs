using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using ReLogic.Localization.IME;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000366 RID: 870
	public class NPCChatPanel
	{
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060028F6 RID: 10486 RVA: 0x00538813 File Offset: 0x00536A13
		private Player LocalPlayer
		{
			get
			{
				return Main.LocalPlayer;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x005765C0 File Offset: 0x005747C0
		private byte mouseTextColor
		{
			get
			{
				return Main.mouseTextColor;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060028F8 RID: 10488 RVA: 0x005765C7 File Offset: 0x005747C7
		public bool allowRichText
		{
			get
			{
				return this.LocalPlayer.talkNPC != -1;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060028F9 RID: 10489 RVA: 0x005765DA File Offset: 0x005747DA
		public bool InVirtualKeyboard
		{
			get
			{
				return Main.InGameUI.CurrentState is UIVirtualKeyboard && PlayerInput.UsingGamepad;
			}
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x005765F4 File Offset: 0x005747F4
		public void Draw()
		{
			if (!this.CanHoldConversation())
			{
				this.Close();
				return;
			}
			this.PrepareText();
			this.PrepareInteractions();
			this.PrepareVirtualKeyboard();
			Color chatBack = new Color(200, 200, 200, 200);
			int num = (int)((this.mouseTextColor * 2 + byte.MaxValue) / 3);
			Color color = new Color(num, num, num, num);
			Point point = new Point(500, 500);
			Rectangle rectangle = new Rectangle(Main.screenWidth / 2 - point.X / 2, 100, point.X, 30);
			rectangle.Height += 30 * this._textDisplayCache.AmountOfLines;
			rectangle.Height += 30 * this._neededInteractionLines + Math.Max(0, 2 * (this._neededInteractionLines - 1));
			Utils.DrawInvBG(Main.spriteBatch, rectangle, default(Color));
			this.DrawText(color, rectangle);
			Main.DrawNPCPortrait(chatBack, rectangle.TopLeft());
			Main.DrawNPCChatBottomRightItem(rectangle.BottomRight());
			if (!PlayerInput.IgnoreMouseInterface && rectangle.Contains(new Point(Main.mouseX, Main.mouseY)))
			{
				this.LocalPlayer.mouseInterface = true;
			}
			this.DrawButtons(rectangle, color);
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x00576730 File Offset: 0x00574930
		private void DrawButtons(Rectangle panelArea, Color chatColor)
		{
			UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsNew = true;
			UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsCount = this._interactions.Count;
			DynamicSpriteFont value = FontAssets.MouseText.Value;
			Vector2 vector = panelArea.BottomLeft() + new Vector2(30f, (float)(-22 * this._neededInteractionLines + Math.Max(0, 2 * (this._neededInteractionLines - 1)) - 4));
			int num = -1;
			int num2 = -1;
			float num3 = 0.9f;
			Rectangle rectangle = new Rectangle((int)vector.X, (int)vector.Y, 100, 22);
			foreach (NPCInteraction npcinteraction in this._interactions)
			{
				num++;
				byte mouseTextColor = this.mouseTextColor;
				chatColor = new Color((int)mouseTextColor, (int)((double)mouseTextColor / 1.1), (int)(mouseTextColor / 2), (int)mouseTextColor);
				if (num % 4 == 0)
				{
					rectangle.X = (int)vector.X;
					rectangle.Y = num / 4 * 22 + (int)vector.Y;
				}
				string text = npcinteraction.GetText();
				int num4 = 0;
				bool flag = npcinteraction.TryAddCoins(ref chatColor, out num4);
				float num5 = 1f;
				Vector2 vector2 = ChatManager.GetStringSize(value, text, new Vector2(num3), -1f);
				if (vector2.X > 260f)
				{
					num5 *= 260f / vector2.X;
				}
				rectangle.Width = (int)(vector2.X * num5);
				bool flag2 = rectangle.Contains(new Point(Main.mouseX, Main.mouseY));
				Vector2 value2 = new Vector2(flag2 ? 1.2f : num3);
				Vector2 origin = new Vector2(0f, vector2.Y * 0.5f);
				Color baseColor = flag2 ? Color.Brown : Color.Black;
				Vector2 vector3 = new Vector2((float)rectangle.Left, (float)rectangle.Center.Y);
				if (flag2)
				{
					vector3.X -= (float)((int)((1.2f - num3) * (float)rectangle.Width * 0.5f));
					vector2 *= 1.2f / num3;
				}
				if (flag2)
				{
					num2 = num;
				}
				ChatManager.DrawColorCodedStringShadow(Main.spriteBatch, value, text, vector3, baseColor, 0f, origin, value2 * num5, -1f, 2f);
				ChatManager.DrawColorCodedString(Main.spriteBatch, value, text, vector3, chatColor, 0f, origin, value2 * num5, -1f, false);
				UILinkPointNavigator.SetPosition(2500 + num, rectangle.Center.ToVector2());
				rectangle.X += rectangle.Width + 30;
				if (npcinteraction.ShowExcalmation)
				{
					Utils.DrawNotificationIcon(Main.spriteBatch, vector3 + new Vector2(vector2.X * num5, 0f) + new Vector2(8f, 0f), 0f, false);
				}
				if (flag)
				{
					ItemSlot.DrawMoney(Main.spriteBatch, "", (float)(rectangle.X - 45), (float)(rectangle.Y - 44), Utils.CoinsSplit((long)num4), true, false);
					rectangle.X += 106;
				}
				if (!PlayerInput.IgnoreMouseInterface && flag2)
				{
					this.LocalPlayer.mouseInterface = true;
					this.LocalPlayer.releaseUseItem = false;
					num2 = num;
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						Main.mouseLeftRelease = false;
						npcinteraction.Interact();
					}
				}
			}
			if (this._lastHovered != num2 && (!PlayerInput.UsingGamepad || num2 != -1))
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
			this._lastHovered = num2;
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x00576AE8 File Offset: 0x00574CE8
		private void PrepareInteractions()
		{
			this._interactions.Clear();
			foreach (NPCInteraction npcinteraction in NPCInteractions.All)
			{
				if (npcinteraction.Condition())
				{
					this._interactions.Add(npcinteraction);
				}
			}
			int count = this._interactions.Count;
			this._neededInteractionLines = (int)Math.Ceiling((double)((float)count / 4f));
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x00576B74 File Offset: 0x00574D74
		private void PrepareVirtualKeyboard()
		{
			int num = 120 + this._textDisplayCache.AmountOfLines * 30 + 30;
			num -= 235;
			UIVirtualKeyboard.ShouldHideText = !PlayerInput.SettingsForUI.ShowGamepadHints;
			if (!PlayerInput.UsingGamepad)
			{
				num = 9999;
			}
			UIVirtualKeyboard.OffsetDown = num;
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x00576BC0 File Offset: 0x00574DC0
		private void PrepareText()
		{
			string npcChatText = Main.npcChatText;
			this.OverrideChatTextWithShenanigans(ref npcChatText);
			this._textDisplayCache.PrepareCache(npcChatText);
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x00576BE8 File Offset: 0x00574DE8
		private void OverrideChatTextWithShenanigans(ref string chatTextToShow)
		{
			object obj = this.LocalPlayer.talkNPC != -1 && Main.CanDryadPlayStardewAnimation(this.LocalPlayer, Main.npc[this.LocalPlayer.talkNPC]);
			int num = 24;
			if (this.LocalPlayer.talkNPC != -1 && Main.npc[this.LocalPlayer.talkNPC].ai[0] == (float)num && NPC.RerollDryadText == 2)
			{
				NPC.RerollDryadText = 1;
			}
			object obj2 = obj;
			if (obj2 != null && NPC.RerollDryadText == 1 && Main.npc[this.LocalPlayer.talkNPC].ai[0] != (float)num && this.LocalPlayer.talkNPC != -1 && Main.npc[this.LocalPlayer.talkNPC].active && Main.npc[this.LocalPlayer.talkNPC].type == 20)
			{
				NPC.RerollDryadText = 0;
				chatTextToShow = (Main.npcChatText = Main.npc[this.LocalPlayer.talkNPC].GetChat());
				NPC.PreventJojaColaDialog = true;
			}
			if (obj2 != null && !NPC.PreventJojaColaDialog)
			{
				chatTextToShow = Language.GetTextValue("StardewTalk.PlayerHasColaAndIsHoldingIt");
			}
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x00576D0C File Offset: 0x00574F0C
		private void DrawText(Color textColor, Rectangle textArea)
		{
			Vector2 vector = textArea.TopLeft() + new Vector2(20f, 20f);
			DynamicSpriteFont value = FontAssets.MouseText.Value;
			string[] textLines = this._textDisplayCache.TextLines;
			int amountOfLines = this._textDisplayCache.AmountOfLines;
			for (int i = 0; i < amountOfLines; i++)
			{
				string text = textLines[i];
				if (text != null)
				{
					if (this.allowRichText)
					{
						ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, value, text, vector + new Vector2(0f, (float)(i * 30)), textColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					}
					else
					{
						Utils.DrawBorderStringFourWay(Main.spriteBatch, value, text, vector.X, vector.Y + (float)(i * 30), textColor, Color.Black, Vector2.Zero, 1f);
					}
				}
			}
			if (!Main.editSign || textLines[amountOfLines - 1] == null)
			{
				return;
			}
			Vector2 vector2 = vector + new Vector2(0f, (float)((amountOfLines - 1) * 30));
			vector2.X += value.MeasureString(textLines[amountOfLines - 1]).X;
			string compositionString = Platform.Get<IImeService>().CompositionString;
			if (compositionString != null && compositionString.Length > 0)
			{
				float x = value.MeasureString(compositionString).X;
				if (x + vector2.X - vector.X > 460f)
				{
					vector2 = vector + new Vector2(0f, (float)(amountOfLines * 30));
				}
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, value, compositionString, vector2, Main.imeCompositionStringColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
				Main.instance.SetIMEPanelAnchor(vector2 + new Vector2(0f, 54f), 0f);
				vector2.X += x;
			}
			int num = this.textBlinkerCount + 1;
			this.textBlinkerCount = num;
			if (num >= 20)
			{
				this.textBlinkerState = ((this.textBlinkerState == 0) ? 1 : 0);
				this.textBlinkerCount = 0;
			}
			if (this.textBlinkerState == 1)
			{
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, value, "|", vector2, textColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
			}
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x00576F5A File Offset: 0x0057515A
		public void Close()
		{
			this._lastHovered = -1;
			this.ClearNPCChatText();
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x00576F69 File Offset: 0x00575169
		private void ClearNPCChatText()
		{
			Main.npcChatText = "";
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x00576F75 File Offset: 0x00575175
		public bool CanHoldConversation()
		{
			return this.LocalPlayer.talkNPC >= 0 || this.LocalPlayer.sign != -1;
		}

		// Token: 0x04005180 RID: 20864
		private int textBlinkerCount;

		// Token: 0x04005181 RID: 20865
		private int textBlinkerState;

		// Token: 0x04005182 RID: 20866
		private List<NPCInteraction> _interactions = new List<NPCInteraction>();

		// Token: 0x04005183 RID: 20867
		private TextDisplayCache _textDisplayCache = new TextDisplayCache();

		// Token: 0x04005184 RID: 20868
		private int _neededInteractionLines;

		// Token: 0x04005185 RID: 20869
		public const int AllowedInteractionsPerLine = 4;

		// Token: 0x04005186 RID: 20870
		private int _lastHovered = -1;
	}
}
