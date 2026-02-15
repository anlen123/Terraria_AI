using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.UI
{
	// Token: 0x0200036E RID: 878
	public class IssueReportsIndicator
	{
		// Token: 0x06002926 RID: 10534 RVA: 0x00578582 File Offset: 0x00576782
		public void AttemptLettingPlayerKnow()
		{
			this.Setup();
			this._shouldBeShowing = true;
			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, -1, -1, 0f, 1f);
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x005785A8 File Offset: 0x005767A8
		public void Hide()
		{
			this._shouldBeShowing = false;
			this._displayUpPercent = 0f;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x005785BC File Offset: 0x005767BC
		private void OpenUI()
		{
			this.Setup();
			Main.OpenReportsMenu();
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x005785C9 File Offset: 0x005767C9
		private void Setup()
		{
			this._buttonTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/IssueButton", 1);
			this._buttonOutlineTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/IssueButton_Outline", 1);
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x005785F8 File Offset: 0x005767F8
		public void Draw(SpriteBatch spriteBatch)
		{
			bool shouldBeShowing = this._shouldBeShowing;
			this._displayUpPercent = MathHelper.Clamp(this._displayUpPercent + (float)shouldBeShowing.ToDirectionInt(), 0f, 1f);
			if (this._displayUpPercent == 0f)
			{
				return;
			}
			Texture2D value = this._buttonTexture.Value;
			Vector2 value2 = Main.ScreenSize.ToVector2() + new Vector2(40f, -80f);
			Vector2 value3 = value2 + new Vector2(-80f, 0f);
			Vector2 vector = Vector2.Lerp(value2, value3, this._displayUpPercent);
			Rectangle rectangle = value.Frame(1, 1, 0, 0, 0, 0);
			Vector2 origin = rectangle.Size() / 2f;
			bool flag = false;
			if (Utils.CenteredRectangle(vector, rectangle.Size()).Contains(Main.MouseScreen.ToPoint()))
			{
				flag = true;
				string textValue = Language.GetTextValue("UI.IssueReporterHasThingsToShow");
				Main.instance.MouseText(textValue, 0, 0, -1, -1, -1, -1, 0);
				if (Main.mouseLeft)
				{
					this.OpenUI();
					this.Hide();
					return;
				}
			}
			float scale = 1f;
			spriteBatch.Draw(value, vector, new Rectangle?(rectangle), Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
			if (flag)
			{
				Texture2D value4 = this._buttonOutlineTexture.Value;
				Rectangle rectangle2 = value4.Frame(1, 1, 0, 0, 0, 0);
				spriteBatch.Draw(value4, vector, new Rectangle?(rectangle2), Color.White, 0f, rectangle2.Size() / 2f, scale, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x040051B3 RID: 20915
		private float _displayUpPercent;

		// Token: 0x040051B4 RID: 20916
		private bool _shouldBeShowing;

		// Token: 0x040051B5 RID: 20917
		private Asset<Texture2D> _buttonTexture;

		// Token: 0x040051B6 RID: 20918
		private Asset<Texture2D> _buttonOutlineTexture;
	}
}
