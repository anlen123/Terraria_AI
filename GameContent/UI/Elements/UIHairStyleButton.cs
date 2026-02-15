using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003EC RID: 1004
	public class UIHairStyleButton : UIImageButton
	{
		// Token: 0x06002E6F RID: 11887 RVA: 0x005AA75C File Offset: 0x005A895C
		public UIHairStyleButton(Player player, int hairStyleId) : base(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", 1), null)
		{
			this._player = player;
			this.HairStyleId = hairStyleId;
			this.Width = StyleDimension.FromPixels(44f);
			this.Height = StyleDimension.FromPixels(44f);
			this._selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1);
			this._hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", 1);
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x005AA7E2 File Offset: 0x005A89E2
		public void SkipRenderingContent(int timeInFrames)
		{
			this._framesToSkip = timeInFrames;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x005AA7EC File Offset: 0x005A89EC
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this._hovered)
			{
				if (!this._soundedHover)
				{
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
				this._soundedHover = true;
			}
			else
			{
				this._soundedHover = false;
			}
			Vector2 value = new Vector2(-5f, -5f);
			base.DrawSelf(spriteBatch);
			if (this._player.hair == this.HairStyleId)
			{
				spriteBatch.Draw(this._selectedBorderTexture.Value, base.GetDimensions().Center() - this._selectedBorderTexture.Size() / 2f, Color.White);
			}
			if (this._hovered)
			{
				spriteBatch.Draw(this._hoveredBorderTexture.Value, base.GetDimensions().Center() - this._hoveredBorderTexture.Size() / 2f, Color.White);
			}
			if (this._framesToSkip > 0)
			{
				this._framesToSkip--;
				return;
			}
			int head = this._player.head;
			this._player.head = -1;
			int hair = this._player.hair;
			this._player.hair = this.HairStyleId;
			Main.PlayerRenderer.DrawPlayerHead(Main.Camera, this._player, base.GetDimensions().Center() + value, 1f, 1f, default(Color));
			this._player.hair = hair;
			this._player.head = head;
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x005AA97F File Offset: 0x005A8B7F
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			this._player.hair = this.HairStyleId;
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			base.LeftMouseDown(evt);
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x005AA9AE File Offset: 0x005A8BAE
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this._hovered = true;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x005AA9BE File Offset: 0x005A8BBE
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this._hovered = false;
		}

		// Token: 0x04005579 RID: 21881
		private readonly Player _player;

		// Token: 0x0400557A RID: 21882
		public readonly int HairStyleId;

		// Token: 0x0400557B RID: 21883
		private readonly Asset<Texture2D> _selectedBorderTexture;

		// Token: 0x0400557C RID: 21884
		private readonly Asset<Texture2D> _hoveredBorderTexture;

		// Token: 0x0400557D RID: 21885
		private bool _hovered;

		// Token: 0x0400557E RID: 21886
		private bool _soundedHover;

		// Token: 0x0400557F RID: 21887
		private int _framesToSkip;
	}
}
