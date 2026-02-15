using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Testing;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003B7 RID: 951
	public class UIWorldLoad : UIState
	{
		// Token: 0x06002CC3 RID: 11459 RVA: 0x0059ED8C File Offset: 0x0059CF8C
		public UIWorldLoad()
		{
			this._progressBar.Top.Pixels = 270f;
			this._progressBar.HAlign = 0.5f;
			this._progressBar.VAlign = 0f;
			this._progressBar.Recalculate();
			this._progressMessage.CopyStyle(this._progressBar);
			UIHeader progressMessage = this._progressMessage;
			progressMessage.Top.Pixels = progressMessage.Top.Pixels - 70f;
			this._progressMessage.Recalculate();
			base.Append(this._progressBar);
			base.Append(this._progressMessage);
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x0059EE42 File Offset: 0x0059D042
		public override void OnActivate()
		{
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.Points[3000].Unlink();
				UILinkPointNavigator.ChangePoint(3000);
			}
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x0059EE6C File Offset: 0x0059D06C
		public override void Update(GameTime gameTime)
		{
			if (WorldGenerator.CurrentController != null)
			{
				if (DebugOptions.enableDebugCommands && Main.keyState.IsKeyDown(Keys.F5))
				{
					UIWorldGenDebug.Open();
				}
				if (PlayerInput.Triggers.Current.Inventory && !WorldGenerator.CurrentController.QueuedAbort)
				{
					WorldGenerator.CurrentController.QueuedAbort = true;
					SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				}
			}
			this._progressBar.Top.Pixels = MathHelper.Lerp(270f, 370f, Utils.GetLerpValue(600f, 700f, (float)Main.screenHeight, true));
			this._progressMessage.Top.Pixels = this._progressBar.Top.Pixels - 70f;
			this._progressBar.Recalculate();
			this._progressMessage.Recalculate();
			base.Update(gameTime);
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x0059EF4F File Offset: 0x0059D14F
		public override void Draw(SpriteBatch spriteBatch)
		{
			this._progress = WorldGenerator.CurrentGenerationProgress;
			if (this._progress == null)
			{
				return;
			}
			base.Draw(spriteBatch);
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x0059EF6C File Offset: 0x0059D16C
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float overallProgress = 0f;
			float currentProgress = 0f;
			string text = string.Empty;
			if (this._progress != null)
			{
				overallProgress = (float)this._progress.TotalProgress;
				currentProgress = (float)this._progress.Value;
				text = this._progress.Message;
				if (WorldGenerator.CurrentController.QueuedAbort)
				{
					text = Language.GetTextValue("UI.Canceling");
				}
			}
			this._progressBar.SetProgress(overallProgress, currentProgress);
			this._progressMessage.Text = text;
			if (WorldGen.drunkWorldGenText && !WorldGen.placingTraps && !WorldGen.getGoodWorldGen)
			{
				this._progressMessage.Text = string.Concat(Main.rand.Next(999999999));
				for (int i = 0; i < 3; i++)
				{
					if (Main.rand.Next(2) == 0)
					{
						this._progressMessage.Text = this._progressMessage.Text + Main.rand.Next(999999999);
					}
				}
			}
			if (WorldGen.notTheBees && !Main.zenithWorld)
			{
				this._progressMessage.Text = Language.GetTextValue("UI.WorldGenEasterEgg_GeneratingBees");
			}
			if (WorldGen.getGoodWorldGen && (!WorldGen.noTrapsWorldGen || !WorldGen.placingTraps))
			{
				string text2 = "";
				for (int j = this._progressMessage.Text.Length - 1; j >= 0; j--)
				{
					text2 += this._progressMessage.Text.Substring(j, 1);
				}
				this._progressMessage.Text = text2;
			}
			Main.gameTips.Update();
			Main.gameTips.Draw();
			this.UpdateGamepadSquiggle();
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x0059F110 File Offset: 0x0059D310
		private void UpdateGamepadSquiggle()
		{
			Vector2 value = new Vector2((float)Math.Cos((double)(Main.GlobalTimeWrappedHourly * 6.2831855f)), (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.2831855f * 2f))) * new Vector2(30f, 15f) + Vector2.UnitY * 20f;
			UILinkPointNavigator.Points[3000].Unlink();
			UILinkPointNavigator.SetPosition(3000, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f + value);
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x0059F1B4 File Offset: 0x0059D3B4
		public string GetStatusText()
		{
			if (this._progress == null)
			{
				return string.Format("{0:0.0%} - ... - {1:0.0%}", 0, 0);
			}
			return string.Format("{0:0.0%} - " + this._progress.Message + " - {1:0.0%}", this._progress.TotalProgress, this._progress.Value);
		}

		// Token: 0x04005426 RID: 21542
		private UIGenProgressBar _progressBar = new UIGenProgressBar();

		// Token: 0x04005427 RID: 21543
		private UIHeader _progressMessage = new UIHeader();

		// Token: 0x04005428 RID: 21544
		private GenerationProgress _progress;
	}
}
