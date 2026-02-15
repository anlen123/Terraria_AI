using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x0200040A RID: 1034
	public class UIGenProgressBar : UIElement
	{
		// Token: 0x06002F7A RID: 12154 RVA: 0x005B3184 File Offset: 0x005B1384
		public UIGenProgressBar()
		{
			if (Main.netMode != 2)
			{
				this._texOuterCorrupt = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Corrupt", 1);
				this._texOuterCrimson = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Crimson", 1);
				this._texOuterRandom = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Random", 1);
				this._texOuterLower = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Lower", 1);
			}
			this.Recalculate();
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x005B3214 File Offset: 0x005B1414
		public override void Recalculate()
		{
			this.Width.Precent = 0f;
			this.Height.Precent = 0f;
			this.Width.Pixels = 612f;
			this.Height.Pixels = 70f;
			base.Recalculate();
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x005B3267 File Offset: 0x005B1467
		public void SetProgress(float overallProgress, float currentProgress)
		{
			this._targetCurrentProgress = currentProgress;
			this._targetOverallProgress = overallProgress;
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x005B3278 File Offset: 0x005B1478
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (!this._texOuterCorrupt.IsLoaded || !this._texOuterCrimson.IsLoaded || !this._texOuterRandom.IsLoaded || !this._texOuterLower.IsLoaded)
			{
				return;
			}
			bool flag = WorldGen.crimson;
			bool flag2 = WorldGen.generatingRandomEvil;
			if (WorldGen.drunkWorldGen && Main.rand.Next(2) == 0)
			{
				flag = (Main.rand.Next(2) == 0);
				flag2 = (Main.rand.Next(4) == 0);
			}
			this._visualOverallProgress = this._targetOverallProgress;
			this._visualCurrentProgress = this._targetCurrentProgress;
			CalculatedStyle dimensions = base.GetDimensions();
			int completedWidth = (int)(this._visualOverallProgress * (float)this._longBarWidth);
			int completedWidth2 = (int)(this._visualCurrentProgress * (float)this._smallBarWidth);
			Vector2 value = new Vector2(dimensions.X, dimensions.Y);
			Color color = default(Color);
			color.PackedValue = (flag2 ? 4292696893U : (flag ? 4286836223U : 4283888223U));
			this.DrawFilling2(spriteBatch, value + new Vector2(20f, 40f), 16, completedWidth, this._longBarWidth, color, Color.Lerp(color, Color.Black, 0.5f), new Color(48, 48, 48));
			color.PackedValue = 4290947159U;
			this.DrawFilling2(spriteBatch, value + new Vector2(50f, 60f), 8, completedWidth2, this._smallBarWidth, color, Color.Lerp(color, Color.Black, 0.5f), new Color(33, 33, 33));
			Rectangle r = base.GetDimensions().ToRectangle();
			r.X -= 8;
			Texture2D texture = flag2 ? this._texOuterRandom.Value : (flag ? this._texOuterCrimson.Value : this._texOuterCorrupt.Value);
			spriteBatch.Draw(texture, r.TopLeft(), Color.White);
			spriteBatch.Draw(this._texOuterLower.Value, r.TopLeft() + new Vector2(44f, 60f), Color.White);
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x005B3498 File Offset: 0x005B1698
		private void DrawFilling(SpriteBatch spritebatch, Texture2D tex, Texture2D texShadow, Vector2 topLeft, int completedWidth, int totalWidth, Color separator, Color empty)
		{
			if (completedWidth % 2 != 0)
			{
				completedWidth--;
			}
			Vector2 position = topLeft + (float)completedWidth * Vector2.UnitX;
			int i = completedWidth;
			Rectangle rectangle = tex.Frame(1, 1, 0, 0, 0, 0);
			while (i > 0)
			{
				if (rectangle.Width > i)
				{
					rectangle.X += rectangle.Width - i;
					rectangle.Width = i;
				}
				spritebatch.Draw(tex, position, new Rectangle?(rectangle), Color.White, 0f, new Vector2((float)rectangle.Width, 0f), 1f, SpriteEffects.None, 0f);
				position.X -= (float)rectangle.Width;
				i -= rectangle.Width;
			}
			if (texShadow != null)
			{
				spritebatch.Draw(texShadow, topLeft, new Rectangle?(new Rectangle(0, 0, completedWidth, texShadow.Height)), Color.White);
			}
			spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth, (int)topLeft.Y, totalWidth - completedWidth, tex.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), empty);
			spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth - 2, (int)topLeft.Y, 2, tex.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), separator);
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x005B35F8 File Offset: 0x005B17F8
		private void DrawFilling2(SpriteBatch spritebatch, Vector2 topLeft, int height, int completedWidth, int totalWidth, Color filled, Color separator, Color empty)
		{
			if (completedWidth % 2 != 0)
			{
				completedWidth--;
			}
			spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X, (int)topLeft.Y, completedWidth, height), new Rectangle?(new Rectangle(0, 0, 1, 1)), filled);
			spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth, (int)topLeft.Y, totalWidth - completedWidth, height), new Rectangle?(new Rectangle(0, 0, 1, 1)), empty);
			spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth - 2, (int)topLeft.Y, 2, height), new Rectangle?(new Rectangle(0, 0, 1, 1)), separator);
		}

		// Token: 0x0400565B RID: 22107
		private Asset<Texture2D> _texOuterCrimson;

		// Token: 0x0400565C RID: 22108
		private Asset<Texture2D> _texOuterCorrupt;

		// Token: 0x0400565D RID: 22109
		private Asset<Texture2D> _texOuterRandom;

		// Token: 0x0400565E RID: 22110
		private Asset<Texture2D> _texOuterLower;

		// Token: 0x0400565F RID: 22111
		private float _visualOverallProgress;

		// Token: 0x04005660 RID: 22112
		private float _targetOverallProgress;

		// Token: 0x04005661 RID: 22113
		private float _visualCurrentProgress;

		// Token: 0x04005662 RID: 22114
		private float _targetCurrentProgress;

		// Token: 0x04005663 RID: 22115
		private int _smallBarWidth = 508;

		// Token: 0x04005664 RID: 22116
		private int _longBarWidth = 570;
	}
}
