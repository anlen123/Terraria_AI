using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Initializers
{
	// Token: 0x0200007E RID: 126
	public class LinkButtonsInitializer
	{
		// Token: 0x0600155D RID: 5469 RVA: 0x004C3D30 File Offset: 0x004C1F30
		public static void Load()
		{
			List<TitleLinkButton> titleLinks = Main.TitleLinks;
			titleLinks.Add(LinkButtonsInitializer.MakeSimpleButton("TitleLinks.Discord", "https://discord.gg/terraria", 0));
			titleLinks.Add(LinkButtonsInitializer.MakeSimpleButton("TitleLinks.Instagram", "https://www.instagram.com/terraria_logic/", 1));
			titleLinks.Add(LinkButtonsInitializer.MakeSimpleButton("TitleLinks.Reddit", "https://www.reddit.com/r/Terraria/", 2));
			titleLinks.Add(LinkButtonsInitializer.MakeSimpleButton("TitleLinks.Twitter", "https://twitter.com/Terraria_Logic", 3));
			titleLinks.Add(LinkButtonsInitializer.MakeSimpleButton("TitleLinks.Bluesky", "https://bsky.app/profile/terraria.bsky.social", 4));
			titleLinks.Add(LinkButtonsInitializer.MakeSimpleButton("TitleLinks.Forums", "https://forums.terraria.org/index.php", 5));
			titleLinks.Add(LinkButtonsInitializer.MakeSimpleButton("TitleLinks.Merch", "https://terraria.org/store", 6));
			titleLinks.Add(LinkButtonsInitializer.MakeSimpleButton("TitleLinks.Wiki", "https://terraria.wiki.gg/", 7));
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x004C3DF4 File Offset: 0x004C1FF4
		private static TitleLinkButton MakeSimpleButton(string textKey, string linkUrl, int horizontalFrameIndex)
		{
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/TitleLinkButtons", 1);
			Rectangle value = asset.Frame(8, 2, horizontalFrameIndex, 0, 0, 0);
			Rectangle value2 = asset.Frame(8, 2, horizontalFrameIndex, 1, 0, 0);
			value.Width--;
			value.Height--;
			value2.Width--;
			value2.Height--;
			return new TitleLinkButton
			{
				TooltipTextKey = textKey,
				LinkUrl = linkUrl,
				FrameWehnSelected = new Rectangle?(value2),
				FrameWhenNotSelected = new Rectangle?(value),
				Image = asset
			};
		}
	}
}
