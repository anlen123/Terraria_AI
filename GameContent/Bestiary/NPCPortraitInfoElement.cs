using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200035E RID: 862
	public class NPCPortraitInfoElement : IBestiaryInfoElement
	{
		// Token: 0x060028AD RID: 10413 RVA: 0x00573235 File Offset: 0x00571435
		public NPCPortraitInfoElement(int? rarityStars = null)
		{
			this._filledStarsCount = rarityStars;
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x00573244 File Offset: 0x00571444
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			UIElement uielement = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(112f, 0f)
			};
			uielement.SetPadding(0f);
			BestiaryEntry bestiaryEntry = new BestiaryEntry();
			Asset<Texture2D> portraitBackgroundAsset = null;
			Color portraitColor = Color.White;
			bestiaryEntry.Icon = info.OwnerEntry.Icon.CreateClone();
			bestiaryEntry.UIInfoProvider = info.OwnerEntry.UIInfoProvider;
			List<IBestiaryBackgroundOverlayAndColorProvider> list = new List<IBestiaryBackgroundOverlayAndColorProvider>();
			bool flag = info.UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
			if (flag)
			{
				List<IBestiaryInfoElement> list2 = new List<IBestiaryInfoElement>();
				IEnumerable<IBestiaryBackgroundImagePathAndColorProvider> source = info.OwnerEntry.Info.OfType<IBestiaryBackgroundImagePathAndColorProvider>();
				IEnumerable<IPreferenceProviderElement> preferences = info.OwnerEntry.Info.OfType<IPreferenceProviderElement>();
				IEnumerable<IBestiaryBackgroundImagePathAndColorProvider> enumerable = from provider in source
				where preferences.Any((IPreferenceProviderElement preference) => preference.Matches(provider))
				select provider;
				bool flag2 = false;
				foreach (IBestiaryBackgroundImagePathAndColorProvider bestiaryBackgroundImagePathAndColorProvider in enumerable)
				{
					Asset<Texture2D> backgroundImage = bestiaryBackgroundImagePathAndColorProvider.GetBackgroundImage();
					if (backgroundImage != null)
					{
						portraitBackgroundAsset = backgroundImage;
						flag2 = true;
						Color? backgroundColor = bestiaryBackgroundImagePathAndColorProvider.GetBackgroundColor();
						if (backgroundColor != null)
						{
							portraitColor = backgroundColor.Value;
							break;
						}
						break;
					}
				}
				foreach (IBestiaryInfoElement bestiaryInfoElement in info.OwnerEntry.Info)
				{
					IBestiaryBackgroundImagePathAndColorProvider bestiaryBackgroundImagePathAndColorProvider2 = bestiaryInfoElement as IBestiaryBackgroundImagePathAndColorProvider;
					if (bestiaryBackgroundImagePathAndColorProvider2 != null)
					{
						Asset<Texture2D> backgroundImage2 = bestiaryBackgroundImagePathAndColorProvider2.GetBackgroundImage();
						if (backgroundImage2 == null)
						{
							continue;
						}
						if (!flag2)
						{
							portraitBackgroundAsset = backgroundImage2;
						}
						Color? backgroundColor2 = bestiaryBackgroundImagePathAndColorProvider2.GetBackgroundColor();
						if (backgroundColor2 != null)
						{
							portraitColor = backgroundColor2.Value;
						}
					}
					if (!flag2)
					{
						IBestiaryBackgroundOverlayAndColorProvider bestiaryBackgroundOverlayAndColorProvider = bestiaryInfoElement as IBestiaryBackgroundOverlayAndColorProvider;
						if (bestiaryBackgroundOverlayAndColorProvider != null && bestiaryBackgroundOverlayAndColorProvider.GetBackgroundOverlayImage() != null)
						{
							list2.Add(bestiaryInfoElement);
						}
					}
				}
				list.AddRange(from x in list2.OrderBy(new Func<IBestiaryInfoElement, float>(this.GetSortingValueForElement))
				select x as IBestiaryBackgroundOverlayAndColorProvider);
			}
			UIBestiaryNPCEntryPortrait element = new UIBestiaryNPCEntryPortrait(bestiaryEntry, portraitBackgroundAsset, portraitColor, list)
			{
				Left = new StyleDimension(4f, 0f),
				HAlign = 0f
			};
			uielement.Append(element);
			if (flag && this._filledStarsCount != null)
			{
				UIElement element2 = this.CreateStarsContainer();
				uielement.Append(element2);
			}
			return uielement;
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x005734C0 File Offset: 0x005716C0
		private float GetSortingValueForElement(IBestiaryInfoElement element)
		{
			IBestiaryBackgroundOverlayAndColorProvider bestiaryBackgroundOverlayAndColorProvider = element as IBestiaryBackgroundOverlayAndColorProvider;
			if (bestiaryBackgroundOverlayAndColorProvider != null)
			{
				return bestiaryBackgroundOverlayAndColorProvider.DisplayPriority;
			}
			return 0f;
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x005734E4 File Offset: 0x005716E4
		private UIElement CreateStarsContainer()
		{
			int num = 14;
			int num2 = 14;
			int num3 = -4;
			int num4 = num + num3;
			int num5 = 5;
			int num6 = 5;
			int value = this._filledStarsCount.Value;
			float num7 = 1f;
			int num8 = num4 * Math.Min(num6, num5) - num3;
			double num9 = (double)num4 * Math.Ceiling((double)num5 / (double)num6) - (double)num3;
			UIElement uielement = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", 1), null, 5, 21)
			{
				Width = new StyleDimension((float)num8 + num7 * 2f, 0f),
				Height = new StyleDimension((float)num9 + num7 * 2f, 0f),
				BackgroundColor = Color.Gray * 0f,
				BorderColor = Color.Transparent,
				Left = new StyleDimension(10f, 0f),
				Top = new StyleDimension(6f, 0f),
				VAlign = 0f
			};
			uielement.SetPadding(0f);
			for (int i = num5 - 1; i >= 0; i--)
			{
				string text = "Images/UI/Bestiary/Icon_Rank_Light";
				if (i >= value)
				{
					text = "Images/UI/Bestiary/Icon_Rank_Dim";
				}
				UIImage element = new UIImage(Main.Assets.Request<Texture2D>(text, 1))
				{
					Left = new StyleDimension((float)(num4 * (i % num6)) - (float)num8 * 0.5f + (float)num * 0.5f, 0f),
					Top = new StyleDimension((float)(num4 * (i / num6)) - (float)num9 * 0.5f + (float)num2 * 0.5f, 0f),
					HAlign = 0.5f,
					VAlign = 0.5f
				};
				uielement.Append(element);
			}
			return uielement;
		}

		// Token: 0x0400513F RID: 20799
		private int? _filledStarsCount;
	}
}
