using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003D8 RID: 984
	public class UIBestiaryInfoItemLine : UIPanel, IManuallyOrderedUIElement
	{
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06002DCC RID: 11724 RVA: 0x005A6145 File Offset: 0x005A4345
		// (set) Token: 0x06002DCD RID: 11725 RVA: 0x005A614D File Offset: 0x005A434D
		public int OrderInUIList { get; set; }

		// Token: 0x06002DCE RID: 11726 RVA: 0x005A6158 File Offset: 0x005A4358
		public UIBestiaryInfoItemLine(DropRateInfo info, BestiaryUICollectionInfo uiinfo, float textScale = 1f)
		{
			this._infoDisplayItem = new Item();
			this._infoDisplayItem.SetDefaults(info.itemId, null);
			this.SetBestiaryNotesOnItemCache(info);
			base.SetPadding(0f);
			this.PaddingLeft = 10f;
			this.PaddingRight = 10f;
			this.Width.Set(-14f, 1f);
			this.Height.Set(32f, 0f);
			this.Left.Set(5f, 0f);
			base.OnMouseOver += this.MouseOver;
			base.OnMouseOut += this.MouseOut;
			this.BorderColor = new Color(89, 116, 213, 255);
			string text;
			string text2;
			this.GetDropInfo(info, uiinfo, out text, out text2);
			if (uiinfo.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3)
			{
				this._hideMouseOver = true;
				Asset<Texture2D> texture = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Locked", 1);
				UIElement uielement = new UIElement
				{
					Height = new StyleDimension(0f, 1f),
					Width = new StyleDimension(0f, 1f),
					HAlign = 0.5f,
					VAlign = 0.5f
				};
				uielement.SetPadding(0f);
				UIImage element = new UIImage(texture)
				{
					ImageScale = 0.55f,
					HAlign = 0.5f,
					VAlign = 0.5f
				};
				uielement.Append(element);
				base.Append(uielement);
				return;
			}
			UIItemIcon element2 = new UIItemIcon(this._infoDisplayItem, uiinfo.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3)
			{
				IgnoresMouseInteraction = true,
				HAlign = 0f,
				Left = new StyleDimension(4f, 0f)
			};
			base.Append(element2);
			if (!string.IsNullOrEmpty(text))
			{
				text2 = text + " " + text2;
			}
			UITextPanel<string> element3 = new UITextPanel<string>(text2, textScale, false)
			{
				IgnoresMouseInteraction = true,
				DrawPanel = false,
				HAlign = 1f,
				Top = new StyleDimension(-4f, 0f)
			};
			base.Append(element3);
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x005A637C File Offset: 0x005A457C
		protected void GetDropInfo(DropRateInfo dropRateInfo, BestiaryUICollectionInfo uiinfo, out string stackRange, out string droprate)
		{
			if (dropRateInfo.stackMin != dropRateInfo.stackMax)
			{
				stackRange = string.Format(" ({0}-{1})", dropRateInfo.stackMin, dropRateInfo.stackMax);
			}
			else if (dropRateInfo.stackMin == 1)
			{
				stackRange = "";
			}
			else
			{
				stackRange = " (" + dropRateInfo.stackMin + ")";
			}
			string originalFormat = "P";
			if ((double)dropRateInfo.dropRate < 0.001)
			{
				originalFormat = "P4";
			}
			if (dropRateInfo.dropRate != 1f)
			{
				droprate = Utils.PrettifyPercentDisplay(dropRateInfo.dropRate, originalFormat);
			}
			else
			{
				droprate = "100%";
			}
			if (uiinfo.UnlockState != BestiaryEntryUnlockState.CanShowDropsWithDropRates_4)
			{
				droprate = "???";
				stackRange = "";
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x005A6445 File Offset: 0x005A4645
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			if (base.IsMouseHovering && !this._hideMouseOver)
			{
				this.DrawMouseOver();
			}
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x005A6464 File Offset: 0x005A4664
		private void DrawMouseOver()
		{
			Main.HoverItem = this._infoDisplayItem;
			Main.instance.MouseText("", 0, 0, -1, -1, -1, -1, 0);
			Main.mouseText = true;
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x005A6498 File Offset: 0x005A4698
		public override int CompareTo(object obj)
		{
			IManuallyOrderedUIElement manuallyOrderedUIElement = obj as IManuallyOrderedUIElement;
			if (manuallyOrderedUIElement != null)
			{
				return this.OrderInUIList.CompareTo(manuallyOrderedUIElement.OrderInUIList);
			}
			return base.CompareTo(obj);
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x005A64CC File Offset: 0x005A46CC
		private void SetBestiaryNotesOnItemCache(DropRateInfo info)
		{
			List<string> list = new List<string>();
			if (info.conditions == null)
			{
				return;
			}
			foreach (IProvideItemConditionDescription provideItemConditionDescription in info.conditions)
			{
				if (provideItemConditionDescription != null)
				{
					string conditionDescription = provideItemConditionDescription.GetConditionDescription();
					if (!string.IsNullOrWhiteSpace(conditionDescription))
					{
						list.Add(conditionDescription);
					}
				}
			}
			this._infoDisplayItem.BestiaryNotes = string.Join("\n", list);
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x005A6558 File Offset: 0x005A4758
		private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this.BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x005A657A File Offset: 0x005A477A
		private void MouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			this.BorderColor = new Color(89, 116, 213, 255);
		}

		// Token: 0x040054E7 RID: 21735
		private Item _infoDisplayItem;

		// Token: 0x040054E8 RID: 21736
		private bool _hideMouseOver;
	}
}
