using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000218 RID: 536
	public class MapHeadRenderer : INeedRenderTargetContent
	{
		// Token: 0x060021A5 RID: 8613 RVA: 0x00531428 File Offset: 0x0052F628
		public MapHeadRenderer()
		{
			for (int i = 0; i < this._playerRenders.Length; i++)
			{
				this._playerRenders[i] = new PlayerHeadDrawRenderTargetContent();
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00531478 File Offset: 0x0052F678
		public void Reset()
		{
			this._anyDirty = false;
			this._drawData.Clear();
			for (int i = 0; i < this._playerRenders.Length; i++)
			{
				this._playerRenders[i].Reset();
			}
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x005314B8 File Offset: 0x0052F6B8
		public void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color))
		{
			PlayerHeadDrawRenderTargetContent playerHeadDrawRenderTargetContent = this._playerRenders[drawPlayer.whoAmI];
			playerHeadDrawRenderTargetContent.UsePlayer(drawPlayer);
			playerHeadDrawRenderTargetContent.UseColor(borderColor);
			playerHeadDrawRenderTargetContent.Request();
			this._anyDirty = true;
			this._drawData.Clear();
			if (playerHeadDrawRenderTargetContent.IsReady)
			{
				RenderTarget2D target = playerHeadDrawRenderTargetContent.GetTarget();
				this._drawData.Add(new DrawData(target, position, null, Color.White, 0f, target.Size() / 2f, scale, SpriteEffects.None, 0f));
				this.RenderDrawData(drawPlayer);
			}
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x00531550 File Offset: 0x0052F750
		private void RenderDrawData(Player drawPlayer)
		{
			Effect pixelShader = Main.pixelShader;
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i < this._drawData.Count; i++)
			{
				DrawData drawData = this._drawData[i];
				if (drawData.sourceRect == null)
				{
					drawData.sourceRect = new Rectangle?(drawData.texture.Frame(1, 1, 0, 0, 0, 0));
				}
				PlayerDrawHelper.SetShaderForData(drawPlayer, drawPlayer.cHead, ref drawData);
				if (drawData.texture != null)
				{
					drawData.Draw(spriteBatch);
				}
			}
			pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x005315EB File Offset: 0x0052F7EB
		public bool IsReady
		{
			get
			{
				return !this._anyDirty;
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x005315F8 File Offset: 0x0052F7F8
		public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			if (!this._anyDirty)
			{
				return;
			}
			for (int i = 0; i < this._playerRenders.Length; i++)
			{
				this._playerRenders[i].PrepareRenderTarget(device, spriteBatch);
			}
			this._anyDirty = false;
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x00531638 File Offset: 0x0052F838
		private void CreateOutlines(float alpha, float scale, Color borderColor)
		{
			if (borderColor != Color.Transparent)
			{
				List<DrawData> collection = new List<DrawData>(this._drawData);
				List<DrawData> list = new List<DrawData>(this._drawData);
				this._drawData.Clear();
				float num = 2f * scale;
				Color color = borderColor * (alpha * alpha);
				Color color2 = Color.Black;
				color2 *= alpha * alpha;
				int colorOnlyShaderIndex = ContentSamples.DyeShaderIDs.ColorOnlyShaderIndex;
				for (int i = 0; i < list.Count; i++)
				{
					DrawData value = list[i];
					value.shader = colorOnlyShaderIndex;
					value.color = color2;
					list[i] = value;
				}
				int num2 = 2;
				Vector2 zero;
				for (int j = -num2; j <= num2; j++)
				{
					for (int k = -num2; k <= num2; k++)
					{
						if (Math.Abs(j) + Math.Abs(k) == num2)
						{
							zero = new Vector2((float)j * num, (float)k * num);
							for (int l = 0; l < list.Count; l++)
							{
								DrawData item = list[l];
								item.position += zero;
								this._drawData.Add(item);
							}
						}
					}
				}
				for (int m = 0; m < list.Count; m++)
				{
					DrawData value2 = list[m];
					value2.shader = colorOnlyShaderIndex;
					value2.color = color;
					list[m] = value2;
				}
				zero = Vector2.Zero;
				num2 = 1;
				for (int n = -num2; n <= num2; n++)
				{
					for (int num3 = -num2; num3 <= num2; num3++)
					{
						if (Math.Abs(n) + Math.Abs(num3) == num2)
						{
							zero = new Vector2((float)n * num, (float)num3 * num);
							for (int num4 = 0; num4 < list.Count; num4++)
							{
								DrawData item2 = list[num4];
								item2.position += zero;
								this._drawData.Add(item2);
							}
						}
					}
				}
				this._drawData.AddRange(collection);
			}
		}

		// Token: 0x04004C14 RID: 19476
		private bool _anyDirty;

		// Token: 0x04004C15 RID: 19477
		private PlayerHeadDrawRenderTargetContent[] _playerRenders = new PlayerHeadDrawRenderTargetContent[255];

		// Token: 0x04004C16 RID: 19478
		private readonly List<DrawData> _drawData = new List<DrawData>();
	}
}
