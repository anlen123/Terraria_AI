using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003F8 RID: 1016
	public class UIResourcePack : UIPanel
	{
		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06002EA8 RID: 11944 RVA: 0x005AD148 File Offset: 0x005AB348
		// (set) Token: 0x06002EA9 RID: 11945 RVA: 0x005AD150 File Offset: 0x005AB350
		public int Order { get; set; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06002EAA RID: 11946 RVA: 0x005AD159 File Offset: 0x005AB359
		// (set) Token: 0x06002EAB RID: 11947 RVA: 0x005AD161 File Offset: 0x005AB361
		public UIElement ContentPanel { get; private set; }

		// Token: 0x06002EAC RID: 11948 RVA: 0x005AD16C File Offset: 0x005AB36C
		public UIResourcePack(ResourcePack pack, int order)
		{
			this.ResourcePack = pack;
			this.Order = order;
			this.BackgroundColor = UIResourcePack.DefaultBackgroundColor;
			this.BorderColor = UIResourcePack.DefaultBorderColor;
			this.Height = StyleDimension.FromPixels(102f);
			this.MinHeight = this.Height;
			this.MaxHeight = this.Height;
			this.MinWidth = StyleDimension.FromPixels(102f);
			this.Width = StyleDimension.FromPercent(1f);
			base.SetPadding(5f);
			this._iconBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", 1);
			this.OverflowHidden = true;
			this.BuildChildren();
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x005AD21C File Offset: 0x005AB41C
		private void BuildChildren()
		{
			StyleDimension styleDimension = StyleDimension.FromPixels(77f);
			StyleDimension styleDimension2 = StyleDimension.FromPixels(4f);
			UIText uitext = new UIText(this.ResourcePack.Name, 1f, false)
			{
				Left = styleDimension,
				Top = styleDimension2
			};
			base.Append(uitext);
			styleDimension2.Pixels += uitext.GetOuterDimensions().Height + 6f;
			UIText uitext2 = new UIText(Language.GetTextValue("UI.Author", this.ResourcePack.Author), 0.7f, false)
			{
				Left = styleDimension,
				Top = styleDimension2
			};
			base.Append(uitext2);
			styleDimension2.Pixels += uitext2.GetOuterDimensions().Height + 10f;
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Divider", 1);
			UIImage uiimage = new UIImage(asset)
			{
				Left = StyleDimension.FromPixels(72f),
				Top = styleDimension2,
				Height = StyleDimension.FromPixels((float)asset.Height()),
				Width = StyleDimension.FromPixelsAndPercent(-80f, 1f),
				ScaleToFit = true
			};
			this.Recalculate();
			base.Append(uiimage);
			styleDimension2.Pixels += uiimage.GetOuterDimensions().Height + 5f;
			UIElement uielement = new UIElement
			{
				Left = styleDimension,
				Top = styleDimension2,
				Height = StyleDimension.FromPixels(92f - styleDimension2.Pixels),
				Width = StyleDimension.FromPixelsAndPercent(-styleDimension.Pixels, 1f)
			};
			base.Append(uielement);
			this.ContentPanel = uielement;
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x005AD3B8 File Offset: 0x005AB5B8
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			this.DrawIcon(spriteBatch);
			if (this.ResourcePack.Branding == ResourcePack.BrandingType.SteamWorkshop)
			{
				Asset<Texture2D> asset = TextureAssets.Extra[243];
				spriteBatch.Draw(asset.Value, new Vector2(base.GetDimensions().X + base.GetDimensions().Width - (float)asset.Width() - 3f, base.GetDimensions().Y + 2f), new Rectangle?(asset.Frame(1, 1, 0, 0, 0, 0)), Color.White);
			}
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x005AD44C File Offset: 0x005AB64C
		private void DrawIcon(SpriteBatch spriteBatch)
		{
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			spriteBatch.Draw(this.ResourcePack.Icon, new Rectangle((int)innerDimensions.X + 4, (int)innerDimensions.Y + 4 + 10, 64, 64), Color.White);
			spriteBatch.Draw(this._iconBorderTexture.Value, new Rectangle((int)innerDimensions.X, (int)innerDimensions.Y + 10, 72, 72), Color.White);
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x005AD4C4 File Offset: 0x005AB6C4
		public override int CompareTo(object obj)
		{
			return this.Order.CompareTo(((UIResourcePack)obj).Order);
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x005AD4EA File Offset: 0x005AB6EA
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this.BackgroundColor = UIResourcePack.HoverBackgroundColor;
			this.BorderColor = UIResourcePack.HoverBorderColor;
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x005AD509 File Offset: 0x005AB709
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this.BackgroundColor = UIResourcePack.DefaultBackgroundColor;
			this.BorderColor = UIResourcePack.DefaultBorderColor;
		}

		// Token: 0x040055BB RID: 21947
		private const int PANEL_PADDING = 5;

		// Token: 0x040055BC RID: 21948
		private const int ICON_SIZE = 64;

		// Token: 0x040055BD RID: 21949
		private const int ICON_BORDER_PADDING = 4;

		// Token: 0x040055BE RID: 21950
		private const int HEIGHT_FLUFF = 10;

		// Token: 0x040055BF RID: 21951
		private const float HEIGHT = 102f;

		// Token: 0x040055C0 RID: 21952
		private const float MIN_WIDTH = 102f;

		// Token: 0x040055C1 RID: 21953
		private static readonly Color DefaultBackgroundColor = new Color(26, 40, 89) * 0.8f;

		// Token: 0x040055C2 RID: 21954
		private static readonly Color DefaultBorderColor = new Color(13, 20, 44) * 0.8f;

		// Token: 0x040055C3 RID: 21955
		private static readonly Color HoverBackgroundColor = new Color(46, 60, 119);

		// Token: 0x040055C4 RID: 21956
		private static readonly Color HoverBorderColor = new Color(20, 30, 56);

		// Token: 0x040055C5 RID: 21957
		public readonly ResourcePack ResourcePack;

		// Token: 0x040055C7 RID: 21959
		private readonly Asset<Texture2D> _iconBorderTexture;
	}
}
