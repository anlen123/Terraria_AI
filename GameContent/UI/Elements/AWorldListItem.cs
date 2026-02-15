using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x02000408 RID: 1032
	public abstract class AWorldListItem : UIPanel
	{
		// Token: 0x06002F55 RID: 12117 RVA: 0x005B1840 File Offset: 0x005AFA40
		private void UpdateGlitchAnimation(UIElement affectedElement)
		{
			int glitchFrame = this._glitchFrame;
			int minValue = 3;
			int num = 3;
			if (this._glitchFrame == 0)
			{
				minValue = 15;
				num = 120;
			}
			int num2 = this._glitchFrameCounter + 1;
			this._glitchFrameCounter = num2;
			if (num2 >= Main.rand.Next(minValue, num + 1))
			{
				this._glitchFrameCounter = 0;
				this._glitchFrame = (this._glitchFrame + 1) % 16;
				if ((this._glitchFrame == 4 || this._glitchFrame == 8 || this._glitchFrame == 12) && Main.rand.Next(3) == 0)
				{
					this._glitchVariation = Main.rand.Next(7);
				}
			}
			(affectedElement as UIImageFramed).SetFrame(7, 16, this._glitchVariation, this._glitchFrame, 0, 0);
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x005B18FC File Offset: 0x005AFAFC
		protected void GetDifficulty(out string expertText, out Color gameModeColor)
		{
			expertText = "";
			gameModeColor = Color.White;
			if (this._data.GameMode == 3)
			{
				expertText = Language.GetTextValue("UI.Creative");
				gameModeColor = Main.creativeModeColor;
				return;
			}
			int num = 1;
			int gameMode = this._data.GameMode;
			if (gameMode != 1)
			{
				if (gameMode == 2)
				{
					num = 3;
				}
			}
			else
			{
				num = 2;
			}
			if (this._data.ForTheWorthy)
			{
				num++;
			}
			switch (num)
			{
			case 2:
				expertText = Language.GetTextValue("UI.Expert");
				gameModeColor = Main.mcColor;
				return;
			case 3:
				expertText = Language.GetTextValue("UI.Master");
				gameModeColor = Main.hcColor;
				return;
			case 4:
				expertText = Language.GetTextValue("UI.Legendary");
				gameModeColor = Main.legendaryModeColor;
				return;
			default:
				expertText = Language.GetTextValue("UI.Normal");
				return;
			}
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x005B19D8 File Offset: 0x005AFBD8
		protected Asset<Texture2D> GetIcon()
		{
			if (this._data.ZenithWorld)
			{
				return this.GetSeedIcon("Everything", true, false);
			}
			if (this._data.DrunkWorld)
			{
				return this.GetSeedIcon("CorruptionCrimson", true, false);
			}
			if (this._data.ForTheWorthy)
			{
				return this.GetSeedIcon("FTW", true, true);
			}
			if (this._data.NotTheBees)
			{
				return this.GetSeedIcon("NotTheBees", true, true);
			}
			if (this._data.Anniversary)
			{
				return this.GetSeedIcon("Anniversary", true, true);
			}
			if (this._data.DontStarve)
			{
				return this.GetSeedIcon("DontStarve", true, true);
			}
			if (this._data.RemixWorld)
			{
				return this.GetSeedIcon("Remix", true, true);
			}
			if (this._data.NoTrapsWorld)
			{
				return this.GetSeedIcon("Traps", true, true);
			}
			if (this._data.SkyblockWorld)
			{
				return this.GetSeedIcon("Skyblock", false, false);
			}
			return this.GetSeedIcon("", true, true);
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x005B1AE8 File Offset: 0x005AFCE8
		protected List<Asset<Texture2D>> GetIcons()
		{
			List<Asset<Texture2D>> list = new List<Asset<Texture2D>>();
			if (this._data.DrunkWorld)
			{
				list.Add(this.GetSeedIcon("CorruptionCrimson", true, false));
			}
			if (this._data.ForTheWorthy)
			{
				list.Add(this.GetSeedIcon("FTW", true, true));
			}
			if (this._data.NotTheBees)
			{
				list.Add(this.GetSeedIcon("NotTheBees", true, true));
			}
			if (this._data.Anniversary)
			{
				list.Add(this.GetSeedIcon("Anniversary", true, true));
			}
			if (this._data.DontStarve)
			{
				list.Add(this.GetSeedIcon("DontStarve", true, true));
			}
			if (this._data.RemixWorld)
			{
				list.Add(this.GetSeedIcon("Remix", true, true));
			}
			if (this._data.NoTrapsWorld)
			{
				list.Add(this.GetSeedIcon("Traps", true, true));
			}
			if (this._data.SkyblockWorld)
			{
				list.Add(this.GetSeedIcon("Skyblock", false, false));
			}
			if (list.Count > 0)
			{
				return list;
			}
			list.Add(this.GetSeedIcon("", true, true));
			return list;
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x005B1C1C File Offset: 0x005AFE1C
		protected UIElement GetIconElement()
		{
			if (this._data.ZenithWorld)
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/IconEverythingAnimated", 1);
				UIImageFramed uiimageFramed = new UIImageFramed(asset, asset.Frame(7, 16, 0, 0, 0, 0));
				uiimageFramed.Left = new StyleDimension(4f, 0f);
				uiimageFramed.OnUpdate += this.UpdateGlitchAnimation;
				return uiimageFramed;
			}
			return new UICyclingImage(this.GetIcons())
			{
				Left = new StyleDimension(4f, 0f)
			};
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x005B1CA0 File Offset: 0x005AFEA0
		private Asset<Texture2D> GetSeedIcon(string seed, bool hardmodeVariants = true, bool evilVariants = true)
		{
			string text = "Images/UI/Icon";
			if (hardmodeVariants)
			{
				text += (this._data.IsHardMode ? "Hallow" : "");
			}
			if (evilVariants)
			{
				text += (this._data.HasCorruption ? "Corruption" : "Crimson");
			}
			text += seed;
			return Main.Assets.Request<Texture2D>(text, 1);
		}

		// Token: 0x04005643 RID: 22083
		protected WorldFileData _data;

		// Token: 0x04005644 RID: 22084
		protected int _glitchFrameCounter;

		// Token: 0x04005645 RID: 22085
		protected int _glitchFrame;

		// Token: 0x04005646 RID: 22086
		protected int _glitchVariation;
	}
}
