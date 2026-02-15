using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Achievements;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Social.Base;

namespace Terraria.UI
{
	// Token: 0x020000F7 RID: 247
	public class InGamePopups
	{
		// Token: 0x02000705 RID: 1797
		public class AchievementUnlockedPopup : IInGameNotification
		{
			// Token: 0x17000503 RID: 1283
			// (get) Token: 0x06003FD7 RID: 16343 RVA: 0x0069B2FD File Offset: 0x006994FD
			// (set) Token: 0x06003FD8 RID: 16344 RVA: 0x0069B305 File Offset: 0x00699505
			public bool ShouldBeRemoved { get; private set; }

			// Token: 0x17000504 RID: 1284
			// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x0069B30E File Offset: 0x0069950E
			// (set) Token: 0x06003FDA RID: 16346 RVA: 0x0069B316 File Offset: 0x00699516
			public object CreationObject { get; private set; }

			// Token: 0x06003FDB RID: 16347 RVA: 0x0069B320 File Offset: 0x00699520
			public AchievementUnlockedPopup(Achievement achievement)
			{
				this.CreationObject = achievement;
				this._ingameDisplayTimeLeft = 300;
				this._theAchievement = achievement;
				this._title = achievement.FriendlyName.Value;
				int iconIndex = Main.Achievements.GetIconIndex(achievement.Name);
				this._iconIndex = iconIndex;
				this._achievementIconFrame = new Rectangle(iconIndex % 8 * 66, iconIndex / 8 * 66, 64, 64);
				this._achievementTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievements", 2);
				this._achievementBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", 2);
			}

			// Token: 0x06003FDC RID: 16348 RVA: 0x0069B3BC File Offset: 0x006995BC
			public void Update()
			{
				this._ingameDisplayTimeLeft--;
				if (this._ingameDisplayTimeLeft < 0)
				{
					this._ingameDisplayTimeLeft = 0;
				}
			}

			// Token: 0x17000505 RID: 1285
			// (get) Token: 0x06003FDD RID: 16349 RVA: 0x0069B3DC File Offset: 0x006995DC
			private float Scale
			{
				get
				{
					if (this._ingameDisplayTimeLeft < 30)
					{
						return MathHelper.Lerp(0f, 1f, (float)this._ingameDisplayTimeLeft / 30f);
					}
					if (this._ingameDisplayTimeLeft > 285)
					{
						return MathHelper.Lerp(1f, 0f, ((float)this._ingameDisplayTimeLeft - 285f) / 15f);
					}
					return 1f;
				}
			}

			// Token: 0x17000506 RID: 1286
			// (get) Token: 0x06003FDE RID: 16350 RVA: 0x0069B448 File Offset: 0x00699648
			private float Opacity
			{
				get
				{
					float scale = this.Scale;
					if (scale <= 0.5f)
					{
						return 0f;
					}
					return (scale - 0.5f) / 0.5f;
				}
			}

			// Token: 0x06003FDF RID: 16351 RVA: 0x0069B478 File Offset: 0x00699678
			public void PushAnchor(ref Vector2 anchorPosition)
			{
				float num = 50f * this.Opacity;
				anchorPosition.Y -= num;
			}

			// Token: 0x06003FE0 RID: 16352 RVA: 0x0069B4A0 File Offset: 0x006996A0
			public void DrawInGame(SpriteBatch sb, Vector2 bottomAnchorPosition)
			{
				float opacity = this.Opacity;
				if (opacity > 0f)
				{
					float num = this.Scale * 1.1f;
					Vector2 vector = (FontAssets.ItemStack.Value.MeasureString(this._title) + new Vector2(58f, 10f)) * num;
					Rectangle r = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, -vector.Y * 0.5f), vector);
					Vector2 mouseScreen = Main.MouseScreen;
					bool flag = r.Contains(mouseScreen.ToPoint());
					Color c = flag ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f);
					Utils.DrawInvBG(sb, r, c);
					float num2 = num * 0.3f;
					Vector2 vector2 = r.Right() - Vector2.UnitX * num * (12f + num2 * (float)this._achievementIconFrame.Width);
					sb.Draw(this._achievementTexture.Value, vector2, new Rectangle?(this._achievementIconFrame), Color.White * opacity, 0f, new Vector2(0f, (float)(this._achievementIconFrame.Height / 2)), num2, SpriteEffects.None, 0f);
					sb.Draw(this._achievementBorderTexture.Value, vector2, null, Color.White * opacity, 0f, new Vector2(4f, (float)(this._achievementBorderTexture.Height() / 2)), num2, SpriteEffects.None, 0f);
					Color value = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)(Main.mouseTextColor / 5), (int)Main.mouseTextColor);
					Utils.DrawBorderString(sb, this._title, vector2 - Vector2.UnitX * 10f, value * opacity, num * 0.9f, 1f, 0.4f, -1);
					if (flag)
					{
						this.OnMouseOver();
					}
				}
			}

			// Token: 0x06003FE1 RID: 16353 RVA: 0x0069B6B0 File Offset: 0x006998B0
			private void OnMouseOver()
			{
				if (PlayerInput.IgnoreMouseInterface)
				{
					return;
				}
				Main.player[Main.myPlayer].mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.mouseLeftRelease = false;
					if (Main.gameMenu)
					{
						if (Main.menuMode == 0)
						{
							IngameFancyUI.OpenAchievementsAndGoto(this._theAchievement);
						}
					}
					else
					{
						IngameFancyUI.OpenAchievementsAndGoto(this._theAchievement);
					}
					this._ingameDisplayTimeLeft = 0;
					this.ShouldBeRemoved = true;
				}
			}

			// Token: 0x06003FE2 RID: 16354 RVA: 0x0069B71E File Offset: 0x0069991E
			public void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse)
			{
				Utils.DrawInvBG(spriteBatch, area, Color.Red);
			}

			// Token: 0x0400686A RID: 26730
			private Achievement _theAchievement;

			// Token: 0x0400686B RID: 26731
			private Asset<Texture2D> _achievementTexture;

			// Token: 0x0400686C RID: 26732
			private Asset<Texture2D> _achievementBorderTexture;

			// Token: 0x0400686D RID: 26733
			private const int _iconSize = 64;

			// Token: 0x0400686E RID: 26734
			private const int _iconSizeWithSpace = 66;

			// Token: 0x0400686F RID: 26735
			private const int _iconsPerRow = 8;

			// Token: 0x04006870 RID: 26736
			private int _iconIndex;

			// Token: 0x04006871 RID: 26737
			private Rectangle _achievementIconFrame;

			// Token: 0x04006872 RID: 26738
			private string _title;

			// Token: 0x04006873 RID: 26739
			private int _ingameDisplayTimeLeft;
		}

		// Token: 0x02000706 RID: 1798
		public class PlayerWantsToJoinGamePopup : IInGameNotification
		{
			// Token: 0x17000507 RID: 1287
			// (get) Token: 0x06003FE3 RID: 16355 RVA: 0x0069B72C File Offset: 0x0069992C
			private float Scale
			{
				get
				{
					if (this._timeLeft < 30)
					{
						return MathHelper.Lerp(0f, 1f, (float)this._timeLeft / 30f);
					}
					if (this._timeLeft > 1785)
					{
						return MathHelper.Lerp(1f, 0f, ((float)this._timeLeft - 1785f) / 15f);
					}
					return 1f;
				}
			}

			// Token: 0x17000508 RID: 1288
			// (get) Token: 0x06003FE4 RID: 16356 RVA: 0x0069B798 File Offset: 0x00699998
			private float Opacity
			{
				get
				{
					float scale = this.Scale;
					if (scale <= 0.5f)
					{
						return 0f;
					}
					return (scale - 0.5f) / 0.5f;
				}
			}

			// Token: 0x17000509 RID: 1289
			// (get) Token: 0x06003FE5 RID: 16357 RVA: 0x0069B7C7 File Offset: 0x006999C7
			// (set) Token: 0x06003FE6 RID: 16358 RVA: 0x0069B7CF File Offset: 0x006999CF
			public object CreationObject { get; private set; }

			// Token: 0x06003FE7 RID: 16359 RVA: 0x0069B7D8 File Offset: 0x006999D8
			public PlayerWantsToJoinGamePopup(UserJoinToServerRequest request)
			{
				this._request = request;
				this.CreationObject = request;
				this._timeLeft = 1800;
			}

			// Token: 0x1700050A RID: 1290
			// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x0069B7F9 File Offset: 0x006999F9
			public bool ShouldBeRemoved
			{
				get
				{
					return this._timeLeft <= 0;
				}
			}

			// Token: 0x06003FE9 RID: 16361 RVA: 0x0069B807 File Offset: 0x00699A07
			public void Update()
			{
				this._timeLeft--;
			}

			// Token: 0x06003FEA RID: 16362 RVA: 0x0069B818 File Offset: 0x00699A18
			public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
			{
				float opacity = this.Opacity;
				if (opacity > 0f)
				{
					string text = Utils.FormatWith(this._request.GetUserWrapperText(), new
					{
						DisplayName = this._request.UserDisplayName,
						FullId = this._request.UserFullIdentifier
					});
					float num = this.Scale * 1.1f;
					Vector2 vector = (FontAssets.ItemStack.Value.MeasureString(text) + new Vector2(58f, 10f)) * num;
					Rectangle r = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, -vector.Y * 0.5f), vector);
					Vector2 mouseScreen = Main.MouseScreen;
					Color c = r.Contains(mouseScreen.ToPoint()) ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f);
					Utils.DrawInvBG(spriteBatch, r, c);
					Vector2 vector2 = new Vector2((float)r.Left, (float)r.Center.Y);
					vector2.X += 32f;
					Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", 1).Value;
					Vector2 vector3 = new Vector2((float)(r.Left + 7), MathHelper.Lerp((float)r.Top, (float)r.Bottom, 0.5f) - (float)(value.Height / 2) - 1f);
					bool flag = Utils.CenteredRectangle(vector3 + new Vector2((float)(value.Width / 2), 0f), value.Size()).Contains(mouseScreen.ToPoint());
					spriteBatch.Draw(value, vector3, null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, SpriteEffects.None, 0f);
					if (flag)
					{
						this.OnMouseOver(false);
					}
					value = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", 1).Value;
					vector3 = new Vector2((float)(r.Left + 7), MathHelper.Lerp((float)r.Top, (float)r.Bottom, 0.5f) + (float)(value.Height / 2) + 1f);
					flag = Utils.CenteredRectangle(vector3 + new Vector2((float)(value.Width / 2), 0f), value.Size()).Contains(mouseScreen.ToPoint());
					spriteBatch.Draw(value, vector3, null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, SpriteEffects.None, 0f);
					if (flag)
					{
						this.OnMouseOver(true);
					}
					Color value2 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)(Main.mouseTextColor / 5), (int)Main.mouseTextColor);
					Utils.DrawBorderString(spriteBatch, text, r.Center.ToVector2() + new Vector2(10f, 0f), value2 * opacity, num * 0.9f, 0.5f, 0.4f, -1);
				}
			}

			// Token: 0x06003FEB RID: 16363 RVA: 0x0069BB84 File Offset: 0x00699D84
			private void OnMouseOver(bool reject = false)
			{
				if (PlayerInput.IgnoreMouseInterface)
				{
					return;
				}
				Main.player[Main.myPlayer].mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.mouseLeftRelease = false;
					this._timeLeft = 0;
					if (reject)
					{
						this._request.Reject();
						return;
					}
					this._request.Accept();
				}
			}

			// Token: 0x06003FEC RID: 16364 RVA: 0x0069BBE0 File Offset: 0x00699DE0
			public void PushAnchor(ref Vector2 positionAnchorBottom)
			{
				float num = 70f * this.Opacity;
				positionAnchorBottom.Y -= num;
			}

			// Token: 0x06003FED RID: 16365 RVA: 0x0069BC08 File Offset: 0x00699E08
			public void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse)
			{
				string userWrapperText = this._request.GetUserWrapperText();
				string userDisplayName = this._request.UserDisplayName;
				Utils.TrimTextIfNeeded(ref userDisplayName, FontAssets.MouseText.Value, 0.9f, (float)(area.Width / 4));
				string text = Utils.FormatWith(userWrapperText, new
				{
					DisplayName = userDisplayName,
					FullId = this._request.UserFullIdentifier
				});
				Vector2 mouseScreen = Main.MouseScreen;
				Color c = area.Contains(mouseScreen.ToPoint()) ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f);
				Utils.DrawInvBG(spriteBatch, area, c);
				Vector2 pos = new Vector2((float)area.Left, (float)area.Center.Y);
				pos.X += 32f;
				Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", 1).Value;
				Vector2 vector = new Vector2((float)(area.Left + 7), MathHelper.Lerp((float)area.Top, (float)area.Bottom, 0.5f) - (float)(value.Height / 2) - 1f);
				bool flag = Utils.CenteredRectangle(vector + new Vector2((float)(value.Width / 2), 0f), value.Size()).Contains(mouseScreen.ToPoint());
				spriteBatch.Draw(value, vector, null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, SpriteEffects.None, 0f);
				if (flag)
				{
					this.OnMouseOver(false);
				}
				value = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", 1).Value;
				vector = new Vector2((float)(area.Left + 7), MathHelper.Lerp((float)area.Top, (float)area.Bottom, 0.5f) + (float)(value.Height / 2) + 1f);
				flag = Utils.CenteredRectangle(vector + new Vector2((float)(value.Width / 2), 0f), value.Size()).Contains(mouseScreen.ToPoint());
				spriteBatch.Draw(value, vector, null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, SpriteEffects.None, 0f);
				if (flag)
				{
					this.OnMouseOver(true);
				}
				pos.X += 6f;
				Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)(Main.mouseTextColor / 5), (int)Main.mouseTextColor);
				Utils.DrawBorderString(spriteBatch, text, pos, color, 0.9f, 0f, 0.4f, -1);
			}

			// Token: 0x04006876 RID: 26742
			private int _timeLeft;

			// Token: 0x04006877 RID: 26743
			private const int _timeLeftMax = 1800;

			// Token: 0x04006878 RID: 26744
			private UserJoinToServerRequest _request;
		}
	}
}
