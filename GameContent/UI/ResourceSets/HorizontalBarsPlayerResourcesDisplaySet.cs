using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets
{
	// Token: 0x020003BB RID: 955
	public class HorizontalBarsPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
	{
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06002CE6 RID: 11494 RVA: 0x005A070C File Offset: 0x0059E90C
		// (set) Token: 0x06002CE7 RID: 11495 RVA: 0x005A0714 File Offset: 0x0059E914
		public string NameKey { get; private set; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06002CE8 RID: 11496 RVA: 0x005A071D File Offset: 0x0059E91D
		// (set) Token: 0x06002CE9 RID: 11497 RVA: 0x005A0725 File Offset: 0x0059E925
		public string ConfigKey { get; private set; }

		// Token: 0x06002CEA RID: 11498 RVA: 0x005A0730 File Offset: 0x0059E930
		public HorizontalBarsPlayerResourcesDisplaySet(string nameKey, string configKey, string resourceFolderName, AssetRequestMode mode)
		{
			this.NameKey = nameKey;
			this.ConfigKey = configKey;
			if (configKey == "HorizontalBarsWithFullText")
			{
				this._drawTextStyle = 2;
			}
			else if (configKey == "HorizontalBarsWithText")
			{
				this._drawTextStyle = 1;
			}
			else
			{
				this._drawTextStyle = 0;
			}
			string str = "Images\\UI\\PlayerResourceSets\\" + resourceFolderName;
			this._hpFill = Main.Assets.Request<Texture2D>(str + "\\HP_Fill", mode);
			this._hpFillHoney = Main.Assets.Request<Texture2D>(str + "\\HP_Fill_Honey", mode);
			this._mpFill = Main.Assets.Request<Texture2D>(str + "\\MP_Fill", mode);
			this._panelLeft = Main.Assets.Request<Texture2D>(str + "\\Panel_Left", mode);
			this._panelMiddleHP = Main.Assets.Request<Texture2D>(str + "\\HP_Panel_Middle", mode);
			this._panelRightHP = Main.Assets.Request<Texture2D>(str + "\\HP_Panel_Right", mode);
			this._panelMiddleMP = Main.Assets.Request<Texture2D>(str + "\\MP_Panel_Middle", mode);
			this._panelRightMP = Main.Assets.Request<Texture2D>(str + "\\MP_Panel_Right", mode);
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x005A0878 File Offset: 0x0059EA78
		public void Draw()
		{
			this.PrepareFields(Main.LocalPlayer);
			SpriteBatch spriteBatch = Main.spriteBatch;
			int num = 16;
			int num2 = 18;
			int num3 = Main.screenWidth - 300 - 22 + num;
			if (this._drawTextStyle == 2)
			{
				num2 += 2;
				HorizontalBarsPlayerResourcesDisplaySet.DrawLifeBarText(spriteBatch, new Vector2((float)num3, (float)num2));
				HorizontalBarsPlayerResourcesDisplaySet.DrawManaText(spriteBatch);
			}
			else if (this._drawTextStyle == 1)
			{
				num2 += 4;
				HorizontalBarsPlayerResourcesDisplaySet.DrawLifeBarText(spriteBatch, new Vector2((float)num3, (float)num2));
			}
			Vector2 vector = new Vector2((float)num3, (float)num2);
			vector.X += (float)((this._maxSegmentCount - this._hpSegmentsCount) * this._panelMiddleHP.Width());
			bool flag = false;
			ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._hpSegmentsCount + 2;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector;
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.LifePanelDrawer);
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref flag);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._hpSegmentsCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(6f, 6f);
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.LifeFillingDrawer);
			resourceDrawSettings.OffsetPerDraw = new Vector2((float)this._hpFill.Width(), 0f);
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref flag);
			this._hpHovered = flag;
			flag = false;
			Vector2 vector2 = new Vector2((float)(num3 - 10), (float)(num2 + 24));
			vector2.X += (float)((this._maxSegmentCount - this._mpSegmentsCount) * this._panelMiddleMP.Width());
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._mpSegmentsCount + 2;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector2;
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.ManaPanelDrawer);
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref flag);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = this._mpSegmentsCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector2 + new Vector2(6f, 6f);
			resourceDrawSettings.GetTextureMethod = new ResourceDrawSettings.TextureGetter(this.ManaFillingDrawer);
			resourceDrawSettings.OffsetPerDraw = new Vector2((float)this._mpFill.Width(), 0f);
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref flag);
			this._mpHovered = flag;
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x005A0B84 File Offset: 0x0059ED84
		private static void DrawManaText(SpriteBatch spriteBatch)
		{
			Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
			int num = 180;
			Player localPlayer = Main.LocalPlayer;
			string text = Lang.inter[2].Value + ":";
			string text2 = localPlayer.statMana + "/" + localPlayer.statManaMax2;
			Vector2 value = new Vector2((float)(Main.screenWidth - num), 65f);
			string text3 = text + " " + text2;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text3);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, value + new Vector2(-vector.X * 0.5f, 0f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text2, value + new Vector2(vector.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(text2).X, 0f), 1f, SpriteEffects.None, 0f, null, null);
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x005A0CD8 File Offset: 0x0059EED8
		private static void DrawLifeBarText(SpriteBatch spriteBatch, Vector2 topLeftAnchor)
		{
			Vector2 value = topLeftAnchor + new Vector2(130f, -20f);
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

		// Token: 0x06002CEE RID: 11502 RVA: 0x005A0E6C File Offset: 0x0059F06C
		private void PrepareFields(Player player)
		{
			PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(player);
			this._hpSegmentsCount = (int)((float)playerStatsSnapshot.LifeMax / playerStatsSnapshot.LifePerSegment);
			this._mpSegmentsCount = (int)((float)playerStatsSnapshot.ManaMax / playerStatsSnapshot.ManaPerSegment);
			this._maxSegmentCount = 20;
			this._hpFruitCount = playerStatsSnapshot.LifeFruitCount;
			this._hpPercent = (float)playerStatsSnapshot.Life / (float)playerStatsSnapshot.LifeMax;
			this._mpPercent = (float)playerStatsSnapshot.Mana / (float)playerStatsSnapshot.ManaMax;
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x005A0EEC File Offset: 0x0059F0EC
		private void LifePanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = this._panelLeft;
			drawScale = 1f;
			if (elementIndex == lastElementIndex)
			{
				sprite = this._panelRightHP;
				offset = new Vector2(-16f, -10f);
				return;
			}
			if (elementIndex != firstElementIndex)
			{
				sprite = this._panelMiddleHP;
			}
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x005A0F50 File Offset: 0x0059F150
		private void ManaPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = this._panelLeft;
			drawScale = 1f;
			if (elementIndex == lastElementIndex)
			{
				sprite = this._panelRightMP;
				offset = new Vector2(-16f, -6f);
				return;
			}
			if (elementIndex != firstElementIndex)
			{
				sprite = this._panelMiddleMP;
			}
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x005A0FB3 File Offset: 0x0059F1B3
		private void LifeFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sprite = this._hpFill;
			if (elementIndex >= this._hpSegmentsCount - this._hpFruitCount)
			{
				sprite = this._hpFillHoney;
			}
			HorizontalBarsPlayerResourcesDisplaySet.FillBarByValues(elementIndex, sprite, this._hpSegmentsCount, this._hpPercent, out offset, out drawScale, out sourceRect);
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x005A0FF4 File Offset: 0x0059F1F4
		private static void FillBarByValues(int elementIndex, Asset<Texture2D> sprite, int segmentsCount, float fillPercent, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			float num = 1f / (float)segmentsCount;
			float t = 1f - fillPercent;
			float lerpValue = Utils.GetLerpValue(num * (float)elementIndex, num * (float)(elementIndex + 1), t, true);
			float num2 = 1f - lerpValue;
			drawScale = 1f;
			Rectangle rectangle = sprite.Frame(1, 1, 0, 0, 0, 0);
			int num3 = (int)((float)rectangle.Width * (1f - num2));
			offset.X += (float)num3;
			rectangle.X += num3;
			rectangle.Width -= num3;
			sourceRect = new Rectangle?(rectangle);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x005A10A9 File Offset: 0x0059F2A9
		private void ManaFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sprite = this._mpFill;
			HorizontalBarsPlayerResourcesDisplaySet.FillBarByValues(elementIndex, sprite, this._mpSegmentsCount, this._mpPercent, out offset, out drawScale, out sourceRect);
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x005A10CF File Offset: 0x0059F2CF
		public void TryToHover()
		{
			if (this._hpHovered)
			{
				CommonResourceBarMethods.DrawLifeMouseOver();
			}
			if (this._mpHovered)
			{
				CommonResourceBarMethods.DrawManaMouseOver();
			}
		}

		// Token: 0x0400544A RID: 21578
		private int _maxSegmentCount;

		// Token: 0x0400544B RID: 21579
		private int _hpSegmentsCount;

		// Token: 0x0400544C RID: 21580
		private int _mpSegmentsCount;

		// Token: 0x0400544D RID: 21581
		private int _hpFruitCount;

		// Token: 0x0400544E RID: 21582
		private float _hpPercent;

		// Token: 0x0400544F RID: 21583
		private float _mpPercent;

		// Token: 0x04005450 RID: 21584
		private byte _drawTextStyle;

		// Token: 0x04005451 RID: 21585
		private bool _hpHovered;

		// Token: 0x04005452 RID: 21586
		private bool _mpHovered;

		// Token: 0x04005453 RID: 21587
		private Asset<Texture2D> _hpFill;

		// Token: 0x04005454 RID: 21588
		private Asset<Texture2D> _hpFillHoney;

		// Token: 0x04005455 RID: 21589
		private Asset<Texture2D> _mpFill;

		// Token: 0x04005456 RID: 21590
		private Asset<Texture2D> _panelLeft;

		// Token: 0x04005457 RID: 21591
		private Asset<Texture2D> _panelMiddleHP;

		// Token: 0x04005458 RID: 21592
		private Asset<Texture2D> _panelRightHP;

		// Token: 0x04005459 RID: 21593
		private Asset<Texture2D> _panelMiddleMP;

		// Token: 0x0400545A RID: 21594
		private Asset<Texture2D> _panelRightMP;
	}
}
