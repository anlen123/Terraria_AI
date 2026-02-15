using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.Social;
using Terraria.Social.Base;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000372 RID: 882
	public class WorkshopPublishingIndicator
	{
		// Token: 0x06002934 RID: 10548 RVA: 0x005794F8 File Offset: 0x005776F8
		public void Hide()
		{
			this._displayUpPercent = 0f;
			this._frameCounter = 0;
			this._timesSoundWasPlayed = 0;
			this._shouldPlayEndingSound = false;
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x0057951C File Offset: 0x0057771C
		public void Draw(SpriteBatch spriteBatch)
		{
			WorkshopSocialModule workshop = SocialAPI.Workshop;
			if (workshop == null)
			{
				return;
			}
			AWorkshopProgressReporter progressReporter = workshop.ProgressReporter;
			bool hasOngoingTasks = progressReporter.HasOngoingTasks;
			bool flag = this._displayUpPercent == 1f;
			this._displayUpPercent = MathHelper.Clamp(this._displayUpPercent + (float)hasOngoingTasks.ToDirectionInt() / 60f, 0f, 1f);
			bool flag2 = this._displayUpPercent == 1f;
			if (flag && !flag2)
			{
				this._shouldPlayEndingSound = true;
			}
			if (this._displayUpPercent == 0f)
			{
				return;
			}
			if (this._indicatorTexture == null)
			{
				this._indicatorTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/InProgress", 1);
			}
			Texture2D value = this._indicatorTexture.Value;
			int num = 6;
			this._frameCounter++;
			int num2 = 5;
			int num3 = this._frameCounter / num2 % num;
			Vector2 value2 = Main.ScreenSize.ToVector2() + new Vector2(-40f, 40f);
			Vector2 value3 = value2 + new Vector2(0f, -80f);
			Vector2 vector = Vector2.Lerp(value2, value3, this._displayUpPercent);
			Rectangle rectangle = value.Frame(1, 6, 0, num3, 0, 0);
			Vector2 origin = rectangle.Size() / 2f;
			spriteBatch.Draw(value, vector, new Rectangle?(rectangle), Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			float f;
			if (progressReporter.TryGetProgress(out f) && !float.IsNaN(f))
			{
				string text = f.ToString("P");
				DynamicSpriteFont value4 = FontAssets.ItemStack.Value;
				int num4 = 1;
				Vector2 origin2 = value4.MeasureString(text) * (float)num4 * new Vector2(0.5f, 1f);
				Utils.DrawBorderStringFourWay(spriteBatch, value4, text, vector.X, vector.Y - 10f, Color.White, Color.Black, origin2, (float)num4);
			}
			if (num3 == 3 && this._frameCounter % num2 == 0)
			{
				if (this._shouldPlayEndingSound)
				{
					this._shouldPlayEndingSound = false;
					this._timesSoundWasPlayed = 0;
					SoundEngine.PlaySound(64, -1, -1, 1, 1f, 0f);
				}
				if (hasOngoingTasks)
				{
					float volumeScale = Utils.Remap((float)this._timesSoundWasPlayed, 0f, 10f, 1f, 0f, true);
					SoundEngine.PlaySound(21, -1, -1, 1, volumeScale, 0f);
					this._timesSoundWasPlayed++;
				}
			}
		}

		// Token: 0x040051B9 RID: 20921
		private float _displayUpPercent;

		// Token: 0x040051BA RID: 20922
		private int _frameCounter;

		// Token: 0x040051BB RID: 20923
		private bool _shouldPlayEndingSound;

		// Token: 0x040051BC RID: 20924
		private Asset<Texture2D> _indicatorTexture;

		// Token: 0x040051BD RID: 20925
		private int _timesSoundWasPlayed;
	}
}
