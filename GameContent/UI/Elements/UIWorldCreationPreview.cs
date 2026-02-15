using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003E4 RID: 996
	public class UIWorldCreationPreview : UIElement
	{
		// Token: 0x06002E37 RID: 11831 RVA: 0x005A8ED0 File Offset: 0x005A70D0
		public UIWorldCreationPreview()
		{
			this._BorderTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewBorder", 1);
			this._BackgroundNormalTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyNormal1", 1);
			this._BackgroundExpertTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyExpert1", 1);
			this._BackgroundMasterTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyMaster1", 1);
			this._BunnyNormalTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyNormal2", 1);
			this._BunnyExpertTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyExpert2", 1);
			this._BunnyCreativeTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyCreative2", 1);
			this._BunnyMasterTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyMaster2", 1);
			this._EvilRandomTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewEvilRandom", 1);
			this._EvilCorruptionTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewEvilCorruption", 1);
			this._EvilCrimsonTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewEvilCrimson", 1);
			this._SizeSmallTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeSmall", 1);
			this._SizeMediumTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeMedium", 1);
			this._SizeLargeTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeLarge", 1);
			this.Width.Set((float)this._BackgroundExpertTexture.Width(), 0f);
			this.Height.Set((float)this._BackgroundExpertTexture.Height(), 0f);
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x005A904F File Offset: 0x005A724F
		public void UpdateOption(byte difficulty, byte evil, byte size)
		{
			this._difficulty = difficulty;
			this._evil = evil;
			this._size = size;
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x005A9068 File Offset: 0x005A7268
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Vector2 position = new Vector2(dimensions.X + 4f, dimensions.Y + 4f);
			Color color = Color.White;
			switch (this._difficulty)
			{
			case 0:
			case 3:
				spriteBatch.Draw(this._BackgroundNormalTexture.Value, position, Color.White);
				color = Color.White;
				break;
			case 1:
				spriteBatch.Draw(this._BackgroundExpertTexture.Value, position, Color.White);
				color = Color.DarkGray;
				break;
			case 2:
				spriteBatch.Draw(this._BackgroundMasterTexture.Value, position, Color.White);
				color = Color.DarkGray;
				break;
			}
			switch (this._size)
			{
			case 0:
				spriteBatch.Draw(this._SizeSmallTexture.Value, position, color);
				break;
			case 1:
				spriteBatch.Draw(this._SizeMediumTexture.Value, position, color);
				break;
			case 2:
				spriteBatch.Draw(this._SizeLargeTexture.Value, position, color);
				break;
			}
			switch (this._evil)
			{
			case 0:
				spriteBatch.Draw(this._EvilRandomTexture.Value, position, color);
				break;
			case 1:
				spriteBatch.Draw(this._EvilCorruptionTexture.Value, position, color);
				break;
			case 2:
				spriteBatch.Draw(this._EvilCrimsonTexture.Value, position, color);
				break;
			}
			switch (this._difficulty)
			{
			case 0:
				spriteBatch.Draw(this._BunnyNormalTexture.Value, position, color);
				break;
			case 1:
				spriteBatch.Draw(this._BunnyExpertTexture.Value, position, color);
				break;
			case 2:
				spriteBatch.Draw(this._BunnyMasterTexture.Value, position, color * 1.2f);
				break;
			case 3:
				spriteBatch.Draw(this._BunnyCreativeTexture.Value, position, color);
				break;
			}
			spriteBatch.Draw(this._BorderTexture.Value, new Vector2(dimensions.X, dimensions.Y), Color.White);
		}

		// Token: 0x04005526 RID: 21798
		private readonly Asset<Texture2D> _BorderTexture;

		// Token: 0x04005527 RID: 21799
		private readonly Asset<Texture2D> _BackgroundExpertTexture;

		// Token: 0x04005528 RID: 21800
		private readonly Asset<Texture2D> _BackgroundNormalTexture;

		// Token: 0x04005529 RID: 21801
		private readonly Asset<Texture2D> _BackgroundMasterTexture;

		// Token: 0x0400552A RID: 21802
		private readonly Asset<Texture2D> _BunnyExpertTexture;

		// Token: 0x0400552B RID: 21803
		private readonly Asset<Texture2D> _BunnyNormalTexture;

		// Token: 0x0400552C RID: 21804
		private readonly Asset<Texture2D> _BunnyCreativeTexture;

		// Token: 0x0400552D RID: 21805
		private readonly Asset<Texture2D> _BunnyMasterTexture;

		// Token: 0x0400552E RID: 21806
		private readonly Asset<Texture2D> _EvilRandomTexture;

		// Token: 0x0400552F RID: 21807
		private readonly Asset<Texture2D> _EvilCorruptionTexture;

		// Token: 0x04005530 RID: 21808
		private readonly Asset<Texture2D> _EvilCrimsonTexture;

		// Token: 0x04005531 RID: 21809
		private readonly Asset<Texture2D> _SizeSmallTexture;

		// Token: 0x04005532 RID: 21810
		private readonly Asset<Texture2D> _SizeMediumTexture;

		// Token: 0x04005533 RID: 21811
		private readonly Asset<Texture2D> _SizeLargeTexture;

		// Token: 0x04005534 RID: 21812
		private byte _difficulty;

		// Token: 0x04005535 RID: 21813
		private byte _evil;

		// Token: 0x04005536 RID: 21814
		private byte _size;
	}
}
