using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000359 RID: 857
	public class FlavorTextBestiaryInfoElement : IBestiaryInfoElement
	{
		// Token: 0x0600289E RID: 10398 RVA: 0x00572DB1 File Offset: 0x00570FB1
		public FlavorTextBestiaryInfoElement(string languageKey)
		{
			this._key = languageKey;
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x00572DC0 File Offset: 0x00570FC0
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2)
			{
				return null;
			}
			UIPanel uipanel = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", 1), null, 12, 7);
			uipanel.Width = new StyleDimension(-11f, 1f);
			uipanel.Height = new StyleDimension(109f, 0f);
			uipanel.BackgroundColor = new Color(43, 56, 101);
			uipanel.BorderColor = Color.Transparent;
			uipanel.Left = new StyleDimension(3f, 0f);
			uipanel.PaddingLeft = 4f;
			uipanel.PaddingRight = 4f;
			UIText uitext = new UIText(Language.GetText(this._key), 0.8f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				IsWrapped = true
			};
			FlavorTextBestiaryInfoElement.AddDynamicResize(uipanel, uitext);
			uipanel.Append(uitext);
			return uipanel;
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x00572ED0 File Offset: 0x005710D0
		private static void AddDynamicResize(UIElement container, UIText text)
		{
			text.OnInternalTextChange += delegate()
			{
				container.Height = new StyleDimension(text.MinHeight.Pixels, 0f);
			};
		}

		// Token: 0x04005138 RID: 20792
		private string _key;
	}
}
