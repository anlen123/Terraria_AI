using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Effects
{
	// Token: 0x020001F5 RID: 501
	public class OverlayManager : EffectManager<Overlay>
	{
		// Token: 0x060020B4 RID: 8372 RVA: 0x005229A4 File Offset: 0x00520BA4
		public OverlayManager()
		{
			for (int i = 0; i < this._activeOverlays.Length; i++)
			{
				this._activeOverlays[i] = new LinkedList<Overlay>();
			}
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x005229F4 File Offset: 0x00520BF4
		public override void OnActivate(Overlay overlay, Vector2 position)
		{
			LinkedList<Overlay> linkedList = this._activeOverlays[(int)overlay.Priority];
			if (overlay.Mode == OverlayMode.FadeIn || overlay.Mode == OverlayMode.Active)
			{
				return;
			}
			if (overlay.Mode == OverlayMode.FadeOut)
			{
				linkedList.Remove(overlay);
				this._overlayCount--;
			}
			else
			{
				overlay.Opacity = 0f;
			}
			if (linkedList.Count != 0)
			{
				foreach (Overlay overlay2 in linkedList)
				{
					overlay2.Mode = OverlayMode.FadeOut;
				}
			}
			linkedList.AddLast(overlay);
			this._overlayCount++;
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00522AAC File Offset: 0x00520CAC
		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < this._activeOverlays.Length; i++)
			{
				LinkedListNode<Overlay> next;
				for (LinkedListNode<Overlay> linkedListNode = this._activeOverlays[i].First; linkedListNode != null; linkedListNode = next)
				{
					Overlay value = linkedListNode.Value;
					next = linkedListNode.Next;
					value.Update(gameTime);
					switch (value.Mode)
					{
					case OverlayMode.FadeIn:
						value.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds * 1f;
						if (value.Opacity >= 1f)
						{
							value.Opacity = 1f;
							value.Mode = OverlayMode.Active;
						}
						break;
					case OverlayMode.Active:
						value.Opacity = Math.Min(1f, value.Opacity + (float)gameTime.ElapsedGameTime.TotalSeconds * 1f);
						break;
					case OverlayMode.FadeOut:
						value.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds * 1f;
						if (value.Opacity <= 0f)
						{
							value.Opacity = 0f;
							value.Mode = OverlayMode.Inactive;
							this._activeOverlays[i].Remove(linkedListNode);
							this._overlayCount--;
						}
						break;
					}
				}
			}
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00522BF8 File Offset: 0x00520DF8
		public void Draw(SpriteBatch spriteBatch, RenderLayers layer)
		{
			if (this._overlayCount == 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < this._activeOverlays.Length; i++)
			{
				for (LinkedListNode<Overlay> linkedListNode = this._activeOverlays[i].First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					Overlay value = linkedListNode.Value;
					if (value.Layer == layer && value.IsVisible())
					{
						if (!flag)
						{
							spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
							flag = true;
						}
						value.Draw(spriteBatch);
					}
				}
			}
			if (flag)
			{
				spriteBatch.End();
			}
		}

		// Token: 0x04004B13 RID: 19219
		private const float OPACITY_RATE = 1f;

		// Token: 0x04004B14 RID: 19220
		private LinkedList<Overlay>[] _activeOverlays = new LinkedList<Overlay>[Enum.GetNames(typeof(EffectPriority)).Length];

		// Token: 0x04004B15 RID: 19221
		private int _overlayCount;
	}
}
