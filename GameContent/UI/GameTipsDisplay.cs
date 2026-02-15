using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000379 RID: 889
	public class GameTipsDisplay
	{
		// Token: 0x06002958 RID: 10584 RVA: 0x0057AB30 File Offset: 0x00578D30
		public GameTipsDisplay(ITipProvider tipProvider)
		{
			this._tipProvider = tipProvider;
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x0057AB58 File Offset: 0x00578D58
		public void Update()
		{
			double time = Main.gameTimeCache.TotalGameTime.TotalSeconds;
			this._currentTips.RemoveAll((GameTipsDisplay.GameTip x) => x.IsExpired(time));
			bool flag = true;
			using (List<GameTipsDisplay.GameTip>.Enumerator enumerator = this._currentTips.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsExpiring(time))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				this.AddNewTip(time);
			}
			foreach (GameTipsDisplay.GameTip gameTip in this._currentTips)
			{
				gameTip.Update(time);
			}
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x0057AC40 File Offset: 0x00578E40
		public void ClearTips()
		{
			this._currentTips.Clear();
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x0057AC50 File Offset: 0x00578E50
		public void Draw()
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			float num = (float)Main.screenWidth;
			float y = (float)Main.screenHeight + this.TipOffsetY;
			float num2 = (float)Main.screenWidth * 0.5f;
			foreach (GameTipsDisplay.GameTip gameTip in this._currentTips)
			{
				if (gameTip.ScreenAnchorX >= -0.5f && gameTip.ScreenAnchorX <= 1.5f)
				{
					DynamicSpriteFont value = FontAssets.MouseText.Value;
					string text = value.CreateWrappedText(gameTip.Text, num2, Language.ActiveCulture.CultureInfo);
					if (text.Split(new char[]
					{
						'\n'
					}).Length > 2)
					{
						text = value.CreateWrappedText(gameTip.Text, num2 * 1.5f - 50f, Language.ActiveCulture.CultureInfo);
					}
					if (Main.vampireSeed)
					{
						text = Language.GetTextValue("Misc.Vampirism");
					}
					else if (WorldGen.getGoodWorldGen)
					{
						string text2 = "";
						for (int i = text.Length - 1; i >= 0; i--)
						{
							text2 += text.Substring(i, 1);
						}
						text = text2;
					}
					else if (WorldGen.drunkWorldGenText)
					{
						text = string.Concat(Main.rand.Next(999999999));
						for (int j = 0; j < 14; j++)
						{
							if (Main.rand.Next(2) == 0)
							{
								text += Main.rand.Next(999999999);
							}
						}
					}
					Vector2 vector = value.MeasureString(text);
					float num3 = 1.1f;
					float num4 = 110f;
					if (vector.Y > num4)
					{
						num3 = num4 / vector.Y;
					}
					Vector2 vector2 = new Vector2(num * gameTip.ScreenAnchorX, y);
					vector2 -= vector * num3 * 0.5f;
					if (WorldGen.tenthAnniversaryWorldGen && !Main.zenithWorld)
					{
						ChatManager.DrawColorCodedStringWithShadow(spriteBatch, value, text, vector2, Color.HotPink, 0f, Vector2.Zero, new Vector2(num3, num3), -1f, 2f);
					}
					else
					{
						ChatManager.DrawColorCodedStringWithShadow(spriteBatch, value, text, vector2, Color.White, 0f, Vector2.Zero, new Vector2(num3, num3), -1f, 2f);
					}
				}
			}
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0057AEDC File Offset: 0x005790DC
		private void AddNewTip(double currentTime)
		{
			string textKey = "UI.Back";
			string key = this._tipProvider.RollAvailableTip().Key;
			if (Language.Exists(key))
			{
				textKey = key;
			}
			this._currentTips.Add(new GameTipsDisplay.GameTip(textKey, currentTime));
		}

		// Token: 0x040051C9 RID: 20937
		private readonly List<GameTipsDisplay.GameTip> _currentTips = new List<GameTipsDisplay.GameTip>();

		// Token: 0x040051CA RID: 20938
		private ITipProvider _tipProvider;

		// Token: 0x040051CB RID: 20939
		public float TipOffsetY = -150f;

		// Token: 0x020008D3 RID: 2259
		private class GameTip
		{
			// Token: 0x17000569 RID: 1385
			// (get) Token: 0x0600466A RID: 18026 RVA: 0x006C5998 File Offset: 0x006C3B98
			public string Text
			{
				get
				{
					if (this._textKey == null)
					{
						return "What?!";
					}
					return this._formattedText;
				}
			}

			// Token: 0x0600466B RID: 18027 RVA: 0x006C59AE File Offset: 0x006C3BAE
			public bool IsExpired(double currentTime)
			{
				return currentTime >= this.SpawnTime + (double)this.Duration;
			}

			// Token: 0x0600466C RID: 18028 RVA: 0x006C59C4 File Offset: 0x006C3BC4
			public bool IsExpiring(double currentTime)
			{
				return currentTime >= this.SpawnTime + (double)this.Duration - 1.0;
			}

			// Token: 0x0600466D RID: 18029 RVA: 0x006C59E4 File Offset: 0x006C3BE4
			public GameTip(string textKey, double spawnTime)
			{
				this._textKey = Language.GetText(textKey);
				this.SpawnTime = spawnTime;
				this.ScreenAnchorX = 2.5f;
				this.Duration = 11.5f;
				this._formattedText = this._textKey.Value;
			}

			// Token: 0x0600466E RID: 18030 RVA: 0x006C5A34 File Offset: 0x006C3C34
			public void Update(double currentTime)
			{
				double num = currentTime - this.SpawnTime;
				if (num < 0.5)
				{
					this.ScreenAnchorX = MathHelper.SmoothStep(2.5f, 0.5f, (float)Utils.GetLerpValue(0.0, 0.5, num, true));
					return;
				}
				if (num >= (double)(this.Duration - 1f))
				{
					this.ScreenAnchorX = MathHelper.SmoothStep(0.5f, -1.5f, (float)Utils.GetLerpValue((double)(this.Duration - 1f), (double)this.Duration, num, true));
					return;
				}
				this.ScreenAnchorX = 0.5f;
			}

			// Token: 0x0400733D RID: 29501
			private const float APPEAR_FROM = 2.5f;

			// Token: 0x0400733E RID: 29502
			private const float APPEAR_TO = 0.5f;

			// Token: 0x0400733F RID: 29503
			private const float DISAPPEAR_TO = -1.5f;

			// Token: 0x04007340 RID: 29504
			private const float APPEAR_TIME = 0.5f;

			// Token: 0x04007341 RID: 29505
			private const float DISAPPEAR_TIME = 1f;

			// Token: 0x04007342 RID: 29506
			private const float DURATION = 11.5f;

			// Token: 0x04007343 RID: 29507
			private LocalizedText _textKey;

			// Token: 0x04007344 RID: 29508
			private string _formattedText;

			// Token: 0x04007345 RID: 29509
			public float ScreenAnchorX;

			// Token: 0x04007346 RID: 29510
			public readonly float Duration;

			// Token: 0x04007347 RID: 29511
			public readonly double SpawnTime;
		}
	}
}
