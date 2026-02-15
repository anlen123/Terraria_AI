using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.UI.Minimap
{
	// Token: 0x020003C2 RID: 962
	public class MinimapFrameTemplate
	{
		// Token: 0x06002D13 RID: 11539 RVA: 0x005A19E9 File Offset: 0x0059FBE9
		public MinimapFrameTemplate(string name, Vector2 frameOffset, Vector2 resetPosition, Vector2 zoomInPosition, Vector2 zoomOutPosition)
		{
			this.name = name;
			this.frameOffset = frameOffset;
			this.resetPosition = resetPosition;
			this.zoomInPosition = zoomInPosition;
			this.zoomOutPosition = zoomOutPosition;
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x005A1A18 File Offset: 0x0059FC18
		public MinimapFrame CreateInstance(AssetRequestMode mode)
		{
			MinimapFrame minimapFrame = new MinimapFrame(MinimapFrameTemplate.LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + this.name + "\\MinimapFrame", mode), this.frameOffset);
			minimapFrame.NameKey = this.name;
			minimapFrame.ConfigKey = this.name;
			minimapFrame.SetResetButton(MinimapFrameTemplate.LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + this.name + "\\MinimapButton_Reset", mode), this.resetPosition);
			minimapFrame.SetZoomOutButton(MinimapFrameTemplate.LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + this.name + "\\MinimapButton_ZoomOut", mode), this.zoomOutPosition);
			minimapFrame.SetZoomInButton(MinimapFrameTemplate.LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + this.name + "\\MinimapButton_ZoomIn", mode), this.zoomInPosition);
			return minimapFrame;
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x005A1AD8 File Offset: 0x0059FCD8
		private static Asset<T> LoadAsset<T>(string assetName, AssetRequestMode mode) where T : class
		{
			return Main.Assets.Request<T>(assetName, mode);
		}

		// Token: 0x04005471 RID: 21617
		private string name;

		// Token: 0x04005472 RID: 21618
		private Vector2 frameOffset;

		// Token: 0x04005473 RID: 21619
		private Vector2 resetPosition;

		// Token: 0x04005474 RID: 21620
		private Vector2 zoomInPosition;

		// Token: 0x04005475 RID: 21621
		private Vector2 zoomOutPosition;
	}
}
