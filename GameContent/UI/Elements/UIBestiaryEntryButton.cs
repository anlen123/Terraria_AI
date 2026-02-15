using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003D2 RID: 978
	public class UIBestiaryEntryButton : UIElement
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06002D9A RID: 11674 RVA: 0x005A4516 File Offset: 0x005A2716
		// (set) Token: 0x06002D9B RID: 11675 RVA: 0x005A451E File Offset: 0x005A271E
		public BestiaryEntry Entry { get; private set; }

		// Token: 0x06002D9C RID: 11676 RVA: 0x005A4528 File Offset: 0x005A2728
		public UIBestiaryEntryButton(BestiaryEntry entry, bool isAPrettyPortrait)
		{
			this.Entry = entry;
			this.Height.Set(72f, 0f);
			this.Width.Set(72f, 0f);
			base.SetPadding(0f);
			UIElement uielement = new UIElement
			{
				Width = new StyleDimension(-4f, 1f),
				Height = new StyleDimension(-4f, 1f),
				IgnoresMouseInteraction = true,
				OverflowHidden = true,
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			uielement.SetPadding(0f);
			uielement.Append(new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Back", 1))
			{
				VAlign = 0.5f,
				HAlign = 0.5f
			});
			if (isAPrettyPortrait)
			{
				Asset<Texture2D> asset = this.TryGettingBackgroundImageProvider(entry);
				if (asset != null)
				{
					uielement.Append(new UIImage(asset)
					{
						HAlign = 0.5f,
						VAlign = 0.5f
					});
				}
			}
			UIBestiaryEntryIcon uibestiaryEntryIcon = new UIBestiaryEntryIcon(entry, isAPrettyPortrait);
			uielement.Append(uibestiaryEntryIcon);
			base.Append(uielement);
			this._icon = uibestiaryEntryIcon;
			int? num = this.TryGettingDisplayIndex(entry);
			if (num != null)
			{
				UIText element = new UIText(num.Value.ToString(), 0.9f, false)
				{
					Top = new StyleDimension(10f, 0f),
					Left = new StyleDimension(10f, 0f),
					IgnoresMouseInteraction = true
				};
				base.Append(element);
			}
			this._bordersGlow = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Selection", 1))
			{
				VAlign = 0.5f,
				HAlign = 0.5f,
				IgnoresMouseInteraction = true
			};
			this._bordersOverlay = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Overlay", 1))
			{
				VAlign = 0.5f,
				HAlign = 0.5f,
				IgnoresMouseInteraction = true,
				Color = Color.White * 0.6f
			};
			base.Append(this._bordersOverlay);
			UIImage uiimage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Front", 1));
			uiimage.VAlign = 0.5f;
			uiimage.HAlign = 0.5f;
			uiimage.IgnoresMouseInteraction = true;
			base.Append(uiimage);
			this._borders = uiimage;
			if (isAPrettyPortrait)
			{
				base.RemoveChild(this._bordersOverlay);
			}
			if (!isAPrettyPortrait)
			{
				base.OnMouseOver += this.MouseOver;
				base.OnMouseOut += this.MouseOut;
			}
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x005A47E0 File Offset: 0x005A29E0
		private Asset<Texture2D> TryGettingBackgroundImageProvider(BestiaryEntry entry)
		{
			IEnumerable<IBestiaryBackgroundImagePathAndColorProvider> enumerable = from x in entry.Info
			where x is IBestiaryBackgroundImagePathAndColorProvider
			select x as IBestiaryBackgroundImagePathAndColorProvider;
			IEnumerable<IPreferenceProviderElement> preferences = entry.Info.OfType<IPreferenceProviderElement>();
			foreach (IBestiaryBackgroundImagePathAndColorProvider bestiaryBackgroundImagePathAndColorProvider in from provider in enumerable
			where preferences.Any((IPreferenceProviderElement preference) => preference.Matches(provider))
			select provider)
			{
				Asset<Texture2D> backgroundImage = bestiaryBackgroundImagePathAndColorProvider.GetBackgroundImage();
				if (backgroundImage != null)
				{
					return backgroundImage;
				}
			}
			foreach (IBestiaryBackgroundImagePathAndColorProvider bestiaryBackgroundImagePathAndColorProvider2 in enumerable)
			{
				Asset<Texture2D> backgroundImage = bestiaryBackgroundImagePathAndColorProvider2.GetBackgroundImage();
				if (backgroundImage != null)
				{
					return backgroundImage;
				}
			}
			return null;
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x005A48E8 File Offset: 0x005A2AE8
		private int? TryGettingDisplayIndex(BestiaryEntry entry)
		{
			int? result = null;
			IBestiaryInfoElement bestiaryInfoElement = entry.Info.FirstOrDefault((IBestiaryInfoElement x) => x is IBestiaryEntryDisplayIndex);
			if (bestiaryInfoElement != null)
			{
				result = new int?((bestiaryInfoElement as IBestiaryEntryDisplayIndex).BestiaryDisplayIndex);
			}
			return result;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x005A4940 File Offset: 0x005A2B40
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (base.IsMouseHovering)
			{
				Main.instance.MouseText(this._icon.GetHoverText(), 0, 0, -1, -1, -1, -1, 0);
			}
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x005A4974 File Offset: 0x005A2B74
		private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			base.RemoveChild(this._borders);
			base.RemoveChild(this._bordersGlow);
			base.RemoveChild(this._bordersOverlay);
			base.Append(this._borders);
			base.Append(this._bordersGlow);
			this._icon.ForceHover = true;
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x005A49E0 File Offset: 0x005A2BE0
		private void MouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			base.RemoveChild(this._borders);
			base.RemoveChild(this._bordersGlow);
			base.RemoveChild(this._bordersOverlay);
			base.Append(this._bordersOverlay);
			base.Append(this._borders);
			this._icon.ForceHover = false;
		}

		// Token: 0x040054CA RID: 21706
		private UIImage _bordersGlow;

		// Token: 0x040054CB RID: 21707
		private UIImage _bordersOverlay;

		// Token: 0x040054CC RID: 21708
		private UIImage _borders;

		// Token: 0x040054CD RID: 21709
		private UIBestiaryEntryIcon _icon;
	}
}
