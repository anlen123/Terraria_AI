using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.IO;

namespace Terraria.Graphics.Effects
{
	// Token: 0x020001EF RID: 495
	public class FilterManager : EffectManager<Filter>
	{
		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06002099 RID: 8345 RVA: 0x005222E8 File Offset: 0x005204E8
		// (remove) Token: 0x0600209A RID: 8346 RVA: 0x00522320 File Offset: 0x00520520
		public event Action OnPostDraw;

		// Token: 0x0600209C RID: 8348 RVA: 0x00522370 File Offset: 0x00520570
		public void BindTo(Preferences preferences)
		{
			preferences.OnSave += this.Configuration_OnSave;
			preferences.OnLoad += this.Configuration_OnLoad;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00522396 File Offset: 0x00520596
		private void Configuration_OnSave(Preferences preferences)
		{
			preferences.Put("FilterLimit", this._filterLimit);
			preferences.Put("FilterPriorityThreshold", Enum.GetName(typeof(EffectPriority), this._priorityThreshold));
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x005223D4 File Offset: 0x005205D4
		private void Configuration_OnLoad(Preferences preferences)
		{
			this._filterLimit = preferences.Get<int>("FilterLimit", 16);
			EffectPriority priorityThreshold;
			if (Enum.TryParse<EffectPriority>(preferences.Get<string>("FilterPriorityThreshold", "VeryLow"), out priorityThreshold))
			{
				this._priorityThreshold = priorityThreshold;
			}
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x00522414 File Offset: 0x00520614
		public override void OnActivate(Filter effect, Vector2 position)
		{
			if (this._activeFilters.Contains(effect))
			{
				if (effect.Active)
				{
					return;
				}
				if (effect.Priority >= this._priorityThreshold)
				{
					this._activeFilterCount--;
				}
				this._activeFilters.Remove(effect);
			}
			else
			{
				effect.Opacity = 0f;
			}
			if (effect.Priority >= this._priorityThreshold)
			{
				this._activeFilterCount++;
			}
			if (this._activeFilters.Count == 0)
			{
				this._activeFilters.AddLast(effect);
				return;
			}
			for (LinkedListNode<Filter> linkedListNode = this._activeFilters.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				Filter value = linkedListNode.Value;
				if (effect.Priority <= value.Priority)
				{
					this._activeFilters.AddAfter(linkedListNode, effect);
					return;
				}
			}
			this._activeFilters.AddLast(effect);
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x005224F0 File Offset: 0x005206F0
		public void BeginCapture(RenderTarget2D screenTarget1)
		{
			if (this._activeFilterCount == 0 && this.OnPostDraw == null)
			{
				this._captureThisFrame = false;
				return;
			}
			this._captureThisFrame = true;
			Main.instance.GraphicsDevice.SetRenderTarget(screenTarget1);
			Main.instance.GraphicsDevice.Clear(Color.Transparent);
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x00522540 File Offset: 0x00520740
		public void Update(GameTime gameTime)
		{
			LinkedListNode<Filter> linkedListNode = this._activeFilters.First;
			int count = this._activeFilters.Count;
			int num = 0;
			while (linkedListNode != null)
			{
				Filter value = linkedListNode.Value;
				LinkedListNode<Filter> next = linkedListNode.Next;
				bool flag = false;
				if (value.Priority >= this._priorityThreshold)
				{
					num++;
					if (num > this._activeFilterCount - this._filterLimit)
					{
						value.Update(gameTime);
						flag = true;
					}
				}
				if (value.Active && flag)
				{
					value.Opacity = Math.Min(value.Opacity + (float)gameTime.ElapsedGameTime.TotalSeconds * 1f, 1f);
				}
				else
				{
					value.Opacity = Math.Max(value.Opacity - (float)gameTime.ElapsedGameTime.TotalSeconds * 1f, 0f);
				}
				if (!value.Active && value.Opacity == 0f)
				{
					if (value.Priority >= this._priorityThreshold)
					{
						this._activeFilterCount--;
					}
					this._activeFilters.Remove(linkedListNode);
				}
				linkedListNode = next;
			}
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x00522650 File Offset: 0x00520850
		public void EndCapture(RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2)
		{
			this.EndCapture(finalTexture, screenTarget1, screenTarget2, screenTarget1.Size(), screenTarget1.Size(), Vector2.Zero);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x0052266C File Offset: 0x0052086C
		public void EndCapture(RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Vector2 screenSize, Vector2 sceneSize, Vector2 sceneOffset)
		{
			if (!this._captureThisFrame)
			{
				return;
			}
			Rectangle value = new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y);
			RenderTarget2D renderTarget2D = screenTarget1;
			RenderTarget2D renderTarget = screenTarget2;
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			graphicsDevice.SetRenderTarget(renderTarget);
			graphicsDevice.Clear(Color.Transparent);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			SpriteEffects effects = Main.GameViewMatrix.Effects;
			Main.spriteBatch.Draw(Main.skyTarget, Vector2.Zero, new Rectangle?(value), Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
			Main.spriteBatch.Draw(renderTarget2D, Vector2.Zero, new Rectangle?(value), Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
			Main.spriteBatch.End();
			Utils.Swap<RenderTarget2D>(ref renderTarget, ref renderTarget2D);
			int num = 0;
			LinkedListNode<Filter> linkedListNode = this._activeFilters.First;
			Filter filter = null;
			while (linkedListNode != null)
			{
				Filter value2 = linkedListNode.Value;
				LinkedListNode<Filter> next = linkedListNode.Next;
				if (value2.Priority >= this._priorityThreshold)
				{
					num++;
					if (num > this._activeFilterCount - this._filterLimit && value2.IsVisible())
					{
						if (filter != null)
						{
							graphicsDevice.SetRenderTarget(renderTarget);
							graphicsDevice.Clear(Color.Transparent);
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
							filter.Apply(renderTarget2D.Size(), sceneSize, sceneOffset);
							Main.spriteBatch.Draw(renderTarget2D, Vector2.Zero, new Rectangle?(value), Main.ColorOfTheSkies);
							Main.spriteBatch.End();
							Utils.Swap<RenderTarget2D>(ref renderTarget, ref renderTarget2D);
						}
						filter = value2;
					}
				}
				linkedListNode = next;
			}
			graphicsDevice.SetRenderTarget(finalTexture);
			graphicsDevice.Clear(Color.Transparent);
			if (Main.player[Main.myPlayer].gravDir == -1f)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
			}
			else
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			}
			if (filter != null)
			{
				filter.Apply(renderTarget2D.Size(), sceneSize, sceneOffset);
				Main.spriteBatch.Draw(renderTarget2D, Vector2.Zero, new Rectangle?(value), Main.ColorOfTheSkies);
			}
			else
			{
				Main.spriteBatch.Draw(renderTarget2D, Vector2.Zero, new Rectangle?(value), Color.White);
			}
			Main.spriteBatch.End();
			for (int i = 0; i < 8; i++)
			{
				graphicsDevice.Textures[i] = null;
			}
			if (this.OnPostDraw != null)
			{
				this.OnPostDraw();
			}
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x0052290A File Offset: 0x00520B0A
		public bool HasActiveFilter()
		{
			return this._activeFilters.Count != 0;
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x0052291A File Offset: 0x00520B1A
		public bool CanCapture()
		{
			return this.HasActiveFilter() || this.OnPostDraw != null;
		}

		// Token: 0x04004B00 RID: 19200
		private const float OPACITY_RATE = 1f;

		// Token: 0x04004B02 RID: 19202
		private LinkedList<Filter> _activeFilters = new LinkedList<Filter>();

		// Token: 0x04004B03 RID: 19203
		private int _filterLimit = 16;

		// Token: 0x04004B04 RID: 19204
		private EffectPriority _priorityThreshold;

		// Token: 0x04004B05 RID: 19205
		private int _activeFilterCount;

		// Token: 0x04004B06 RID: 19206
		private bool _captureThisFrame;
	}
}
