using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200035A RID: 858
	public class UnlockProgressDisplayBestiaryInfoElement : IBestiaryInfoElement
	{
		// Token: 0x060028A1 RID: 10401 RVA: 0x00572F08 File Offset: 0x00571108
		public UnlockProgressDisplayBestiaryInfoElement(BestiaryUnlockProgressReport progressReport)
		{
			this._progressReport = progressReport;
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x00572F18 File Offset: 0x00571118
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			UIElement uielement = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", 1), null, 12, 7)
			{
				Width = new StyleDimension(-11f, 1f),
				Height = new StyleDimension(109f, 0f),
				BackgroundColor = new Color(43, 56, 101),
				BorderColor = Color.Transparent,
				Left = new StyleDimension(3f, 0f)
			};
			uielement.PaddingLeft = 4f;
			uielement.PaddingRight = 4f;
			string arg = Utils.PrettifyPercentDisplay((float)info.UnlockState / 4f, "P2");
			string text = string.Format("{0} Entry Collected", arg);
			string arg2 = Utils.PrettifyPercentDisplay(this._progressReport.CompletionPercent, "P2");
			string text2 = string.Format("{0} Bestiary Collected", arg2);
			int num = 8;
			UIText uitext = new UIText(text, 0.8f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				IsWrapped = true,
				PaddingTop = (float)(-(float)num),
				PaddingBottom = (float)(-(float)num)
			};
			UIText uitext2 = new UIText(text2, 0.8f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				IsWrapped = true,
				PaddingTop = (float)(-(float)num),
				PaddingBottom = (float)(-(float)num)
			};
			this._text1 = uitext;
			this._text2 = uitext2;
			this.AddDynamicResize(uielement, uitext);
			uielement.Append(uitext);
			uielement.Append(uitext2);
			return uielement;
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x005730F4 File Offset: 0x005712F4
		private void AddDynamicResize(UIElement container, UIText text)
		{
			text.OnInternalTextChange += delegate()
			{
				container.Height = new StyleDimension(this._text1.MinHeight.Pixels + 4f + this._text2.MinHeight.Pixels, 0f);
				this._text2.Top = new StyleDimension(this._text1.MinHeight.Pixels + 4f, 0f);
			};
		}

		// Token: 0x04005139 RID: 20793
		private BestiaryUnlockProgressReport _progressReport;

		// Token: 0x0400513A RID: 20794
		private UIElement _text1;

		// Token: 0x0400513B RID: 20795
		private UIElement _text2;
	}
}
