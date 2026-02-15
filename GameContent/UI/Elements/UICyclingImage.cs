using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003C4 RID: 964
	public class UICyclingImage : UIImage
	{
		// Token: 0x06002D2D RID: 11565 RVA: 0x005A1DFA File Offset: 0x0059FFFA
		public UICyclingImage(List<Asset<Texture2D>> textureAssets)
		{
			this.FramesPerCycle = 45;
			this._textureAssets = textureAssets;
			base.SetImage(this._textureAssets[this._currentTextureIndex]);
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x005A1E28 File Offset: 0x005A0028
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			int num = this._framesCounted + 1;
			this._framesCounted = num;
			if (num < this.FramesPerCycle)
			{
				return;
			}
			this._framesCounted = 0;
			num = this._currentTextureIndex + 1;
			this._currentTextureIndex = num;
			if (num >= this._textureAssets.Count)
			{
				this._currentTextureIndex = 0;
			}
			base.SetImage(this._textureAssets[this._currentTextureIndex]);
		}

		// Token: 0x04005481 RID: 21633
		public int FramesPerCycle;

		// Token: 0x04005482 RID: 21634
		private List<Asset<Texture2D>> _textureAssets;

		// Token: 0x04005483 RID: 21635
		private int _currentTextureIndex;

		// Token: 0x04005484 RID: 21636
		private int _framesCounted;
	}
}
