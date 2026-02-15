using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets
{
	// Token: 0x020003BA RID: 954
	public class FancyClassicPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
	{
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x0059FBC8 File Offset: 0x0059DDC8
		// (set) Token: 0x06002CD7 RID: 11479 RVA: 0x0059FBD0 File Offset: 0x0059DDD0
		public string NameKey { get; private set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06002CD8 RID: 11480 RVA: 0x0059FBD9 File Offset: 0x0059DDD9
		// (set) Token: 0x06002CD9 RID: 11481 RVA: 0x0059FBE1 File Offset: 0x0059DDE1
		public string ConfigKey { get; private set; }

		// Token: 0x06002CDA RID: 11482 RVA: 0x0059FBEC File Offset: 0x0059DDEC
		public FancyClassicPlayerResourcesDisplaySet(string nameKey, string configKey, string resourceFolderName, AssetRequestMode mode)
		{
			this.NameKey = nameKey;
			this.ConfigKey = configKey;
			if (configKey == "NewWithText")
			{
				this._drawText = true;
			}
			else
			{
				this._drawText = false;
			}
			string str = "Images\\UI\\PlayerResourceSets\\" + resourceFolderName;
			this._heartLeft = Main.Assets.Request<Texture2D>(str + "\\Heart_Left", mode);
			this._heartMiddle = Main.Assets.Request<Texture2D>(str + "\\Heart_Middle", mode);
			this._heartRight = Main.Assets.Request<Texture2D>(str + "\\Heart_Right", mode);
			this._heartRightFancy = Main.Assets.Request<Texture2D>(str + "\\Heart_Right_Fancy", mode);
			this._heartFill = Main.Assets.Request<Texture2D>(str + "\\Heart_Fill", mode);
			this._heartFillHoney = Main.Assets.Request<Texture2D>(str + "\\Heart_Fill_B", mode);
			this._heartSingleFancy = Main.Assets.Request<Texture2D>(str + "\\Heart_Single_Fancy", mode);
			this._starTop = Main.Assets.Request<Texture2D>(str + "\\Star_A", mode);
			this._starMiddle = Main.Assets.Request<Texture2D>(str + "\\Star_B", mode);
			this._starBottom = Main.Assets.Request<Texture2D>(str + "\\Star_C", mode);
			this._starSingle = Main.Assets.Request<Texture2D>(str + "\\Star_Single", mode);
			this._starFill = Main.Assets.Request<Texture2D>(str + "\\Star_Fill", mode);
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x0059FD94 File Offset: 0x0059DF94
		public void Draw()
		{
			Player localPlayer = Main.LocalPlayer;
			SpriteBatch spriteBatch = Main.spriteBatch;
			this.PrepareFields(localPlayer);
			this.DrawLifeBar(spriteBatch);
			this.DrawManaBar(spriteBatch);
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x0059FDC4 File Offset: 0x0059DFC4
		private void DrawLifeBar(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2((float)(Main.screenWidth - 300 + 4), 15f);
			if (this._drawText)
			{
				vector.Y += 6f;
				FancyClassicPlayerResourcesDisplaySet.DrawLifeBarText(spriteBatch, vector + new Vector2(-4f, 3f));
			}
			bool hoverLife = false;
			ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._heartCountRow1;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector;
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.HeartPanelDrawer);
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref hoverLife);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._heartCountRow2;
			resourceDrawSettings.ElementIndexOffset = 10;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(0f, 28f);
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.HeartPanelDrawer);
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref hoverLife);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._heartCountRow1;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 15f);
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.HeartFillingDrawer);
			resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
			resourceDrawSettings.Draw(spriteBatch, ref hoverLife);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._heartCountRow2;
			resourceDrawSettings.ElementIndexOffset = 10;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 15f) + new Vector2(0f, 28f);
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.HeartFillingDrawer);
			resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
			resourceDrawSettings.Draw(spriteBatch, ref hoverLife);
			this._hoverLife = hoverLife;
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x005A006C File Offset: 0x0059E26C
		private static void DrawLifeBarText(SpriteBatch spriteBatch, Vector2 topLeftAnchor)
		{
			Vector2 value = topLeftAnchor + new Vector2(130f, -24f);
			Player localPlayer = Main.LocalPlayer;
			Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
			string text = string.Concat(new object[]
			{
				Lang.inter[0].Value,
				" ",
				localPlayer.statLifeMax2,
				"/",
				localPlayer.statLifeMax2
			});
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[0].Value, value + new Vector2(-vector.X * 0.5f, 0f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, localPlayer.statLife + "/" + localPlayer.statLifeMax2, value + new Vector2(vector.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f, null, null);
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x005A0200 File Offset: 0x0059E400
		private void DrawManaBar(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2((float)(Main.screenWidth - 40), 22f);
			int starCount = this._starCount;
			bool hoverMana = false;
			ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._starCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector;
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.StarPanelDrawer);
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitY;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref hoverMana);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._starCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 16f);
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.StarFillingDrawer);
			resourceDrawSettings.OffsetPerDraw = Vector2.UnitY * -2f;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitY;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
			resourceDrawSettings.Draw(spriteBatch, ref hoverMana);
			this._hoverMana = hoverMana;
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x005A0340 File Offset: 0x0059E540
		private static void DrawManaText(SpriteBatch spriteBatch)
		{
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(Lang.inter[2].Value);
			Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
			int num = 50;
			if (vector.X >= 45f)
			{
				num = (int)vector.X + 5;
			}
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[2].Value, new Vector2((float)(Main.screenWidth - num), 6f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x005A03E8 File Offset: 0x0059E5E8
		private void HeartPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = this._heartLeft;
			drawScale = 1f;
			if (elementIndex == lastElementIndex && elementIndex == firstElementIndex)
			{
				sprite = this._heartSingleFancy;
				offset = new Vector2(-4f, -4f);
				return;
			}
			if (elementIndex == lastElementIndex && lastElementIndex == this._lastHeartPanelIndex)
			{
				sprite = this._heartRightFancy;
				offset = new Vector2(-8f, -4f);
				return;
			}
			if (elementIndex == lastElementIndex)
			{
				sprite = this._heartRight;
				return;
			}
			if (elementIndex != firstElementIndex)
			{
				sprite = this._heartMiddle;
			}
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x005A048C File Offset: 0x0059E68C
		private void HeartFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = this._heartLeft;
			if (elementIndex < this._playerLifeFruitCount)
			{
				sprite = this._heartFillHoney;
			}
			else
			{
				sprite = this._heartFill;
			}
			float lerpValue = Utils.GetLerpValue(this._lifePerHeart * (float)elementIndex, this._lifePerHeart * (float)(elementIndex + 1), this._currentPlayerLife, true);
			drawScale = lerpValue;
			if (elementIndex == this._lastHeartFillingIndex && lerpValue > 0f)
			{
				drawScale += Main.cursorScale - 1f;
			}
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x005A051C File Offset: 0x0059E71C
		private void StarPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = this._starTop;
			drawScale = 1f;
			if (elementIndex == lastElementIndex && elementIndex == firstElementIndex)
			{
				sprite = this._starSingle;
				return;
			}
			if (elementIndex == lastElementIndex)
			{
				sprite = this._starBottom;
				offset = new Vector2(0f, 0f);
				return;
			}
			if (elementIndex != firstElementIndex)
			{
				sprite = this._starMiddle;
			}
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x005A0594 File Offset: 0x0059E794
		private void StarFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = this._starFill;
			float lerpValue = Utils.GetLerpValue(this._manaPerStar * (float)elementIndex, this._manaPerStar * (float)(elementIndex + 1), this._currentPlayerMana, true);
			drawScale = lerpValue;
			if (elementIndex == this._lastStarFillingIndex && lerpValue > 0f)
			{
				drawScale += Main.cursorScale - 1f;
			}
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x005A0608 File Offset: 0x0059E808
		private void PrepareFields(Player player)
		{
			PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(player);
			this._playerLifeFruitCount = playerStatsSnapshot.LifeFruitCount;
			this._lifePerHeart = playerStatsSnapshot.LifePerSegment;
			this._currentPlayerLife = (float)playerStatsSnapshot.Life;
			this._manaPerStar = playerStatsSnapshot.ManaPerSegment;
			this._heartCountRow1 = Utils.Clamp<int>((int)((float)playerStatsSnapshot.LifeMax / this._lifePerHeart), 0, 10);
			this._heartCountRow2 = Utils.Clamp<int>((int)((float)(playerStatsSnapshot.LifeMax - 200) / this._lifePerHeart), 0, 10);
			int lastHeartFillingIndex = (int)((float)playerStatsSnapshot.Life / this._lifePerHeart);
			this._lastHeartFillingIndex = lastHeartFillingIndex;
			this._lastHeartPanelIndex = this._heartCountRow1 + this._heartCountRow2 - 1;
			this._starCount = (int)((float)playerStatsSnapshot.ManaMax / this._manaPerStar);
			this._currentPlayerMana = (float)playerStatsSnapshot.Mana;
			this._lastStarFillingIndex = (int)(this._currentPlayerMana / this._manaPerStar);
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x005A06F0 File Offset: 0x0059E8F0
		public void TryToHover()
		{
			if (this._hoverLife)
			{
				CommonResourceBarMethods.DrawLifeMouseOver();
			}
			if (this._hoverMana)
			{
				CommonResourceBarMethods.DrawManaMouseOver();
			}
		}

		// Token: 0x0400542E RID: 21550
		private float _currentPlayerLife;

		// Token: 0x0400542F RID: 21551
		private float _lifePerHeart;

		// Token: 0x04005430 RID: 21552
		private int _playerLifeFruitCount;

		// Token: 0x04005431 RID: 21553
		private int _lastHeartFillingIndex;

		// Token: 0x04005432 RID: 21554
		private int _lastHeartPanelIndex;

		// Token: 0x04005433 RID: 21555
		private int _heartCountRow1;

		// Token: 0x04005434 RID: 21556
		private int _heartCountRow2;

		// Token: 0x04005435 RID: 21557
		private int _starCount;

		// Token: 0x04005436 RID: 21558
		private int _lastStarFillingIndex;

		// Token: 0x04005437 RID: 21559
		private float _manaPerStar;

		// Token: 0x04005438 RID: 21560
		private float _currentPlayerMana;

		// Token: 0x04005439 RID: 21561
		private Asset<Texture2D> _heartLeft;

		// Token: 0x0400543A RID: 21562
		private Asset<Texture2D> _heartMiddle;

		// Token: 0x0400543B RID: 21563
		private Asset<Texture2D> _heartRight;

		// Token: 0x0400543C RID: 21564
		private Asset<Texture2D> _heartRightFancy;

		// Token: 0x0400543D RID: 21565
		private Asset<Texture2D> _heartFill;

		// Token: 0x0400543E RID: 21566
		private Asset<Texture2D> _heartFillHoney;

		// Token: 0x0400543F RID: 21567
		private Asset<Texture2D> _heartSingleFancy;

		// Token: 0x04005440 RID: 21568
		private Asset<Texture2D> _starTop;

		// Token: 0x04005441 RID: 21569
		private Asset<Texture2D> _starMiddle;

		// Token: 0x04005442 RID: 21570
		private Asset<Texture2D> _starBottom;

		// Token: 0x04005443 RID: 21571
		private Asset<Texture2D> _starSingle;

		// Token: 0x04005444 RID: 21572
		private Asset<Texture2D> _starFill;

		// Token: 0x04005445 RID: 21573
		private bool _hoverLife;

		// Token: 0x04005446 RID: 21574
		private bool _hoverMana;

		// Token: 0x04005447 RID: 21575
		private bool _drawText;
	}
}
