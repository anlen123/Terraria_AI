using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200034E RID: 846
	public class FilterProviderInfoElement : IFilterInfoProvider, IProvideSearchFilterString, IBestiaryInfoElement
	{
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06002869 RID: 10345 RVA: 0x00572861 File Offset: 0x00570A61
		// (set) Token: 0x0600286A RID: 10346 RVA: 0x00572869 File Offset: 0x00570A69
		public int DisplayTextPriority { get; set; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600286B RID: 10347 RVA: 0x00572872 File Offset: 0x00570A72
		// (set) Token: 0x0600286C RID: 10348 RVA: 0x0057287A File Offset: 0x00570A7A
		public bool HideInPortraitInfo { get; set; }

		// Token: 0x0600286D RID: 10349 RVA: 0x00572883 File Offset: 0x00570A83
		public FilterProviderInfoElement(string nameLanguageKey, int filterIconFrame)
		{
			this._key = nameLanguageKey;
			this._filterIconFrame.X = filterIconFrame % 16;
			this._filterIconFrame.Y = filterIconFrame / 16;
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x005728B0 File Offset: 0x00570AB0
		public UIElement GetFilterImage()
		{
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", 1);
			return new UIImageFramed(asset, asset.Frame(16, 5, this._filterIconFrame.X, this._filterIconFrame.Y, 0, 0))
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x00572909 File Offset: 0x00570B09
		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			return Language.GetText(this._key).Value;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x00572925 File Offset: 0x00570B25
		public string GetDisplayNameKey()
		{
			return this._key;
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x00572930 File Offset: 0x00570B30
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			if (this.HideInPortraitInfo)
			{
				return null;
			}
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			UIElement uielement = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", 1), null, 12, 7)
			{
				Width = new StyleDimension(-14f, 1f),
				Height = new StyleDimension(34f, 0f),
				BackgroundColor = new Color(43, 56, 101),
				BorderColor = Color.Transparent,
				Left = new StyleDimension(5f, 0f)
			};
			uielement.SetPadding(0f);
			uielement.PaddingRight = 5f;
			UIElement filterImage = this.GetFilterImage();
			filterImage.HAlign = 0f;
			filterImage.Left = new StyleDimension(5f, 0f);
			UIText element = new UIText(Language.GetText(this.GetDisplayNameKey()), 0.8f, false)
			{
				HAlign = 0f,
				PaddingLeft = 38f,
				Width = StyleDimension.FromPercent(1f),
				TextOriginX = 0f,
				TextOriginY = 0f,
				VAlign = 0.5f,
				DynamicallyScaleDownToWidth = true
			};
			if (filterImage != null)
			{
				uielement.Append(filterImage);
			}
			uielement.Append(element);
			this.AddOnHover(uielement);
			return uielement;
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x00572A81 File Offset: 0x00570C81
		private void AddOnHover(UIElement button)
		{
			button.OnUpdate += delegate(UIElement e)
			{
				this.ShowButtonName(e);
			};
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x00572A98 File Offset: 0x00570C98
		private void ShowButtonName(UIElement element)
		{
			if (!element.IsMouseHovering)
			{
				return;
			}
			string textValue = Language.GetTextValue(this.GetDisplayNameKey());
			Main.instance.MouseText(textValue, 0, 0, -1, -1, -1, -1, 0);
		}

		// Token: 0x04005123 RID: 20771
		private const int framesPerRow = 16;

		// Token: 0x04005124 RID: 20772
		private const int framesPerColumn = 5;

		// Token: 0x04005125 RID: 20773
		private Point _filterIconFrame;

		// Token: 0x04005126 RID: 20774
		private string _key;
	}
}
