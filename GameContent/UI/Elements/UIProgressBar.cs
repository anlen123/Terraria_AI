using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x0200040B RID: 1035
	public class UIProgressBar : UIElement
	{
		// Token: 0x06002F80 RID: 12160 RVA: 0x005B36BD File Offset: 0x005B18BD
		public UIProgressBar()
		{
			this._progressBar.Height.Precent = 1f;
			this._progressBar.Recalculate();
			base.Append(this._progressBar);
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x005B36FC File Offset: 0x005B18FC
		public void SetProgress(float value)
		{
			this._targetProgress = value;
			if (value < this._visualProgress)
			{
				this._visualProgress = value;
			}
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x005B3718 File Offset: 0x005B1918
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			this._visualProgress = this._visualProgress * 0.95f + 0.05f * this._targetProgress;
			this._progressBar.Width.Precent = this._visualProgress;
			this._progressBar.Recalculate();
		}

		// Token: 0x04005665 RID: 22117
		private UIProgressBar.UIInnerProgressBar _progressBar = new UIProgressBar.UIInnerProgressBar();

		// Token: 0x04005666 RID: 22118
		private float _visualProgress;

		// Token: 0x04005667 RID: 22119
		private float _targetProgress;

		// Token: 0x02000936 RID: 2358
		private class UIInnerProgressBar : UIElement
		{
			// Token: 0x06004818 RID: 18456 RVA: 0x006CB2E0 File Offset: 0x006C94E0
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				CalculatedStyle dimensions = base.GetDimensions();
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Vector2(dimensions.X, dimensions.Y), null, Color.Blue, 0f, Vector2.Zero, new Vector2(dimensions.Width, dimensions.Height / 1000f), SpriteEffects.None, 0f);
			}
		}
	}
}
