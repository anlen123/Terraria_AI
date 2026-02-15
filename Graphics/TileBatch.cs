using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics
{
	// Token: 0x020001D8 RID: 472
	public class TileBatch
	{
		// Token: 0x06001FAF RID: 8111 RVA: 0x0051C964 File Offset: 0x0051AB64
		public TileBatch(GraphicsDevice graphicsDevice)
		{
			this._graphicsDevice = graphicsDevice;
			this._spriteBatch = new SpriteBatch(graphicsDevice);
			this.Allocate();
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0051CA18 File Offset: 0x0051AC18
		private void Allocate()
		{
			if (this._vertexBuffer == null || this._vertexBuffer.IsDisposed)
			{
				this._vertexBuffer = new DynamicVertexBuffer(this._graphicsDevice, typeof(VertexPositionColorTexture), 8192, BufferUsage.WriteOnly);
				this._vertexBufferPosition = 0;
				this._vertexBuffer.ContentLost += delegate(object <p0>, EventArgs <p1>)
				{
					this._vertexBufferPosition = 0;
				};
			}
			if (this._indexBuffer == null || this._indexBuffer.IsDisposed)
			{
				if (this._fallbackIndexData == null)
				{
					this._fallbackIndexData = new short[12288];
					for (int i = 0; i < 2048; i++)
					{
						this._fallbackIndexData[i * 6] = (short)(i * 4);
						this._fallbackIndexData[i * 6 + 1] = (short)(i * 4 + 1);
						this._fallbackIndexData[i * 6 + 2] = (short)(i * 4 + 2);
						this._fallbackIndexData[i * 6 + 3] = (short)(i * 4);
						this._fallbackIndexData[i * 6 + 4] = (short)(i * 4 + 2);
						this._fallbackIndexData[i * 6 + 5] = (short)(i * 4 + 3);
					}
				}
				this._indexBuffer = new DynamicIndexBuffer(this._graphicsDevice, typeof(short), 12288, BufferUsage.WriteOnly);
				this._indexBuffer.SetData<short>(this._fallbackIndexData);
				this._indexBuffer.ContentLost += delegate(object <p0>, EventArgs <p1>)
				{
					this._indexBuffer.SetData<short>(this._fallbackIndexData);
				};
			}
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x0051CB6C File Offset: 0x0051AD6C
		private void FlushRenderState()
		{
			this.Allocate();
			this._graphicsDevice.SetVertexBuffer(this._vertexBuffer);
			this._graphicsDevice.Indices = this._indexBuffer;
			this._graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			this._drawCalls = 0;
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x0051CBBE File Offset: 0x0051ADBE
		public void Dispose()
		{
			if (this._vertexBuffer != null)
			{
				this._vertexBuffer.Dispose();
			}
			if (this._indexBuffer != null)
			{
				this._indexBuffer.Dispose();
			}
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x0051CBE6 File Offset: 0x0051ADE6
		public void Begin(RasterizerState rasterizer, Matrix transformation)
		{
			this._spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, rasterizer, null, transformation);
			this._spriteBatch.End();
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x0051CC05 File Offset: 0x0051AE05
		public void Begin()
		{
			this.Begin(RasterizerState.CullCounterClockwise, Matrix.Identity);
			if (this._queuedSpriteCount > 0)
			{
				throw new InvalidOperationException("Sprites have already been added before calling Begin");
			}
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x0051CC2B File Offset: 0x0051AE2B
		public int Restart()
		{
			return this.End();
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x0051CC34 File Offset: 0x0051AE34
		public void SetLayer(uint layer, ushort stack = 0)
		{
			if (layer >= 16777216U)
			{
				throw new ArgumentOutOfRangeException("Max Layer Exceeded");
			}
			if (!this._layeredSortingEnabled)
			{
				if (this._queuedSpriteCount > 0)
				{
					throw new InvalidOperationException("Sprites have already been added before setting the first layer");
				}
				this._layeredSortingEnabled = true;
			}
			this._nextLayerStack = new uint?(layer << 16 | (uint)stack);
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0051CC88 File Offset: 0x0051AE88
		public void Draw(Texture2D texture, Vector2 position, VertexColors colors)
		{
			Vector4 vector = default(Vector4);
			vector.X = position.X;
			vector.Y = position.Y;
			vector.Z = 1f;
			vector.W = 1f;
			this.InternalDraw(texture, ref vector, true, ref TileBatch._nullRectangle, ref colors, ref TileBatch._vector2Zero, SpriteEffects.None, 0f);
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0051CCEC File Offset: 0x0051AEEC
		public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, float scale, SpriteEffects effects)
		{
			Vector4 vector = default(Vector4);
			vector.X = position.X;
			vector.Y = position.Y;
			vector.Z = scale;
			vector.W = scale;
			this.InternalDraw(texture, ref vector, true, ref sourceRectangle, ref colors, ref origin, effects, 0f);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0051CD44 File Offset: 0x0051AF44
		public void Draw(Texture2D texture, Vector4 destination, VertexColors colors)
		{
			this.InternalDraw(texture, ref destination, false, ref TileBatch._nullRectangle, ref colors, ref TileBatch._vector2Zero, SpriteEffects.None, 0f);
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0051CD70 File Offset: 0x0051AF70
		public void Draw(Texture2D texture, Vector2 position, VertexColors colors, Vector2 scale)
		{
			Vector4 vector = default(Vector4);
			vector.X = position.X;
			vector.Y = position.Y;
			vector.Z = scale.X;
			vector.W = scale.Y;
			this.InternalDraw(texture, ref vector, true, ref TileBatch._nullRectangle, ref colors, ref TileBatch._vector2Zero, SpriteEffects.None, 0f);
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0051CDD8 File Offset: 0x0051AFD8
		public void Draw(Texture2D texture, Vector4 destination, Rectangle? sourceRectangle, VertexColors colors)
		{
			this.InternalDraw(texture, ref destination, false, ref sourceRectangle, ref colors, ref TileBatch._vector2Zero, SpriteEffects.None, 0f);
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0051CE00 File Offset: 0x0051B000
		public void Draw(Texture2D texture, Vector4 destination, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, SpriteEffects effects, float rotation)
		{
			this.InternalDraw(texture, ref destination, false, ref sourceRectangle, ref colors, ref origin, effects, rotation);
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0051CE24 File Offset: 0x0051B024
		public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, VertexColors colors)
		{
			Vector4 vector = default(Vector4);
			vector.X = (float)destinationRectangle.X;
			vector.Y = (float)destinationRectangle.Y;
			vector.Z = (float)destinationRectangle.Width;
			vector.W = (float)destinationRectangle.Height;
			this.InternalDraw(texture, ref vector, false, ref sourceRectangle, ref colors, ref TileBatch._vector2Zero, SpriteEffects.None, 0f);
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0051CE8C File Offset: 0x0051B08C
		private static short[] CreateIndexData()
		{
			short[] array = new short[12288];
			for (int i = 0; i < 2048; i++)
			{
				array[i * 6] = (short)(i * 4);
				array[i * 6 + 1] = (short)(i * 4 + 1);
				array[i * 6 + 2] = (short)(i * 4 + 2);
				array[i * 6 + 3] = (short)(i * 4);
				array[i * 6 + 4] = (short)(i * 4 + 2);
				array[i * 6 + 5] = (short)(i * 4 + 3);
			}
			return array;
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0051CF00 File Offset: 0x0051B100
		private unsafe void InternalDraw(Texture2D texture, ref Vector4 destination, bool scaleDestination, ref Rectangle? sourceRectangle, ref VertexColors colors, ref Vector2 origin, SpriteEffects effects, float rotation)
		{
			int num;
			if (this._layeredSortingEnabled)
			{
				if (this._nextLayerStack != null)
				{
					uint value = this._nextLayerStack.Value;
					if (texture != this._currentBatchKey.Texture || value != this._currentBatchKey.LayerStack)
					{
						this.SwitchBatch(texture, value);
					}
				}
				else if (texture != this._currentBatchKey.Texture)
				{
					this.SwitchBatch(texture, this._currentBatchKey.LayerStack + 1U);
				}
				this._nextLayerStack = null;
				num = this.GetNextSpriteIndex(ref this._batches[this._currentBatchIndex]);
			}
			else
			{
				if (this._queuedSpriteCount >= this._spriteDataQueue.Length)
				{
					Array.Resize<TileBatch.SpriteData>(ref this._spriteDataQueue, this._spriteDataQueue.Length << 1);
				}
				if (this._queuedSpriteCount >= this._spriteTextures.Length)
				{
					Array.Resize<Texture2D>(ref this._spriteTextures, this._spriteTextures.Length << 1);
				}
				this._spriteTextures[this._queuedSpriteCount] = texture;
				int queuedSpriteCount = this._queuedSpriteCount;
				this._queuedSpriteCount = queuedSpriteCount + 1;
				num = queuedSpriteCount;
			}
			fixed (TileBatch.SpriteData* ptr = &this._spriteDataQueue[num])
			{
				TileBatch.SpriteData* ptr2 = ptr;
				float num2 = destination.Z;
				float num3 = destination.W;
				if (sourceRectangle != null)
				{
					Rectangle value2 = sourceRectangle.Value;
					ptr2->Source.X = (float)value2.X;
					ptr2->Source.Y = (float)value2.Y;
					ptr2->Source.Z = (float)value2.Width;
					ptr2->Source.W = (float)value2.Height;
					if (scaleDestination)
					{
						num2 *= (float)value2.Width;
						num3 *= (float)value2.Height;
					}
				}
				else
				{
					float num4 = (float)texture.Width;
					float num5 = (float)texture.Height;
					ptr2->Source.X = 0f;
					ptr2->Source.Y = 0f;
					ptr2->Source.Z = num4;
					ptr2->Source.W = num5;
					if (scaleDestination)
					{
						num2 *= num4;
						num3 *= num5;
					}
				}
				ptr2->Destination.X = destination.X;
				ptr2->Destination.Y = destination.Y;
				ptr2->Destination.Z = num2;
				ptr2->Destination.W = num3;
				ptr2->Origin.X = origin.X;
				ptr2->Origin.Y = origin.Y;
				ptr2->Effects = effects;
				ptr2->Colors = colors;
				ptr2->Rotation = rotation;
			}
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0051D188 File Offset: 0x0051B388
		private int GetNextSpriteIndex(ref TileBatch.LayerBatch layerBatchState)
		{
			if (layerBatchState.CurrentSliceIsFull)
			{
				int newSpriteBufferSlice = this.GetNewSpriteBufferSlice(layerBatchState.Length);
				this._batchData[layerBatchState.Tail].Next = newSpriteBufferSlice;
				layerBatchState.Tail = newSpriteBufferSlice;
				layerBatchState.NextSprite = this._batchData[newSpriteBufferSlice].Start;
			}
			layerBatchState.Length++;
			int nextSprite = layerBatchState.NextSprite;
			layerBatchState.NextSprite = nextSprite + 1;
			return nextSprite;
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0051D1F8 File Offset: 0x0051B3F8
		private int GetNewSpriteBufferSlice(int length)
		{
			if (this._batchDataCount == this._batchData.Length)
			{
				Array.Resize<TileBatch.DataSlice>(ref this._batchData, this._batchData.Length * 2);
			}
			int batchDataCount = this._batchDataCount;
			this._batchDataCount = batchDataCount + 1;
			int num = batchDataCount;
			this._batchData[num] = new TileBatch.DataSlice
			{
				Start = this._queuedSpriteCount,
				Length = length
			};
			this._queuedSpriteCount += length;
			while (this._queuedSpriteCount > this._spriteDataQueue.Length)
			{
				Array.Resize<TileBatch.SpriteData>(ref this._spriteDataQueue, this._spriteDataQueue.Length * 2);
			}
			return num;
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0051D29C File Offset: 0x0051B49C
		private void SwitchBatch(Texture2D texture, uint layerStack)
		{
			TileBatch.LayerBatchKey currentBatchKey = this._currentBatchKey;
			int currentBatchIndex = this._currentBatchIndex;
			this._currentBatchKey = new TileBatch.LayerBatchKey
			{
				LayerStack = layerStack,
				Texture = texture
			};
			uint num = layerStack >> 14 | (layerStack & 65535U);
			if ((ulong)num < (ulong)((long)this._batchLookupCache.Length) && this._batchLookupCache[(int)num].Texture == texture)
			{
				this._currentBatchIndex = this._batchLookupCache[(int)num].BatchIndex;
			}
			else if (!this._batchLookup.TryGetValue(this._currentBatchKey, out this._currentBatchIndex))
			{
				this.CreateBatch();
			}
			uint num2 = currentBatchKey.LayerStack >> 14 | (currentBatchKey.LayerStack & 65535U);
			if ((ulong)num2 < (ulong)((long)this._batchLookupCache.Length))
			{
				this._batchLookupCache[(int)num2] = new TileBatch.RecentLayerCacheEntry(currentBatchKey.Texture, currentBatchIndex);
			}
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0051D37C File Offset: 0x0051B57C
		private void CreateBatch()
		{
			Texture2D texture = this._currentBatchKey.Texture;
			ushort texture2;
			int num;
			if (!this._textureIdLookup.TryGetValue(texture, out texture2))
			{
				texture2 = (this._textureIdLookup[texture] = (ushort)this._passTextureCount);
				if (this._passTextureCount == this._passTextures.Length)
				{
					Array.Resize<Texture2D>(ref this._passTextures, this._passTextures.Length * 2);
				}
				Texture2D[] passTextures = this._passTextures;
				num = this._passTextureCount;
				this._passTextureCount = num + 1;
				passTextures[num] = texture;
			}
			if (this._batchCount == this._batches.Length)
			{
				Array.Resize<TileBatch.LayerBatch>(ref this._batches, this._batches.Length * 2);
			}
			int newSpriteBufferSlice = this.GetNewSpriteBufferSlice(2);
			TileBatch.LayerBatch[] batches = this._batches;
			num = this._batchCount;
			this._batchCount = num + 1;
			batches[this._currentBatchIndex = num] = new TileBatch.LayerBatch
			{
				LayerStack = this._currentBatchKey.LayerStack,
				Texture = texture2,
				Head = newSpriteBufferSlice,
				Tail = newSpriteBufferSlice,
				NextSprite = this._batchData[newSpriteBufferSlice].Start
			};
			this._batchLookup[this._currentBatchKey] = this._currentBatchIndex;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0051D4AD File Offset: 0x0051B6AD
		public int End()
		{
			this._layeredSortingEnabled = false;
			if (this._queuedSpriteCount == 0)
			{
				return 0;
			}
			this.FlushRenderState();
			if (this._passTextureCount > 0)
			{
				this.FlushLayered();
			}
			else
			{
				this.Flush();
			}
			return this._drawCalls;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0051D4E4 File Offset: 0x0051B6E4
		private void Flush()
		{
			Texture2D texture2D = null;
			int num = 0;
			for (int i = 0; i < this._queuedSpriteCount; i++)
			{
				if (this._spriteTextures[i] != texture2D)
				{
					if (i > num)
					{
						this.RenderBatch(texture2D, this._spriteDataQueue, num, i - num);
					}
					num = i;
					texture2D = this._spriteTextures[i];
				}
			}
			this.RenderBatch(texture2D, this._spriteDataQueue, num, this._queuedSpriteCount - num);
			Array.Clear(this._spriteTextures, 0, this._queuedSpriteCount);
			this._queuedSpriteCount = 0;
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0051D560 File Offset: 0x0051B760
		private void RenderBatch(Texture2D texture, TileBatch.SpriteData[] sprites, int offset, int count)
		{
			this._graphicsDevice.Textures[0] = texture;
			while (count > 0)
			{
				SetDataOptions options = SetDataOptions.NoOverwrite;
				int num = count;
				if (num > 2048 - this._vertexBufferPosition)
				{
					num = 2048 - this._vertexBufferPosition;
					if (num < 256)
					{
						this._vertexBufferPosition = 0;
						options = SetDataOptions.Discard;
						num = count;
						if (num > 2048)
						{
							num = 2048;
						}
					}
				}
				this.FillVertexBuffer(texture, sprites, offset, num, 0);
				int offsetInBytes = this._vertexBufferPosition * sizeof(VertexPositionColorTexture) * 4;
				this._vertexBuffer.SetData<VertexPositionColorTexture>(offsetInBytes, this._vertices, 0, num * 4, sizeof(VertexPositionColorTexture), options);
				int minVertexIndex = this._vertexBufferPosition * 4;
				int numVertices = num * 4;
				int startIndex = this._vertexBufferPosition * 6;
				int primitiveCount = num * 2;
				this._graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, minVertexIndex, numVertices, startIndex, primitiveCount);
				this._vertexBufferPosition += num;
				offset += num;
				count -= num;
				this._drawCalls++;
			}
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x0051D660 File Offset: 0x0051B860
		private unsafe void FillVertexBuffer(Texture2D texture, TileBatch.SpriteData[] sprites, int offset, int count, int vbSpriteOffset)
		{
			float num = 1f / (float)texture.Width;
			float num2 = 1f / (float)texture.Height;
			fixed (TileBatch.SpriteData* ptr = &sprites[offset])
			{
				TileBatch.SpriteData* ptr2 = ptr;
				fixed (VertexPositionColorTexture* ptr3 = &this._vertices[vbSpriteOffset * 4])
				{
					VertexPositionColorTexture* ptr4 = ptr3;
					TileBatch.SpriteData* ptr5 = ptr2;
					VertexPositionColorTexture* ptr6 = ptr4;
					for (int i = 0; i < count; i++)
					{
						float num3;
						float num4;
						if (ptr5->Rotation != 0f)
						{
							num3 = (float)Math.Cos((double)ptr5->Rotation);
							num4 = (float)Math.Sin((double)ptr5->Rotation);
						}
						else
						{
							num3 = 1f;
							num4 = 0f;
						}
						float num5 = ptr5->Origin.X / ptr5->Source.Z;
						float num6 = ptr5->Origin.Y / ptr5->Source.W;
						ptr6->Color = ptr5->Colors.TopLeftColor;
						ptr6[1].Color = ptr5->Colors.TopRightColor;
						ptr6[2].Color = ptr5->Colors.BottomRightColor;
						ptr6[3].Color = ptr5->Colors.BottomLeftColor;
						for (int j = 0; j < 4; j++)
						{
							float num7 = TileBatch.CORNER_OFFSET_X[j];
							float num8 = TileBatch.CORNER_OFFSET_Y[j];
							float num9 = (num7 - num5) * ptr5->Destination.Z;
							float num10 = (num8 - num6) * ptr5->Destination.W;
							float x = ptr5->Destination.X + num9 * num3 - num10 * num4;
							float y = ptr5->Destination.Y + num9 * num4 + num10 * num3;
							if ((ptr5->Effects & SpriteEffects.FlipVertically) != SpriteEffects.None)
							{
								num8 = 1f - num8;
							}
							if ((ptr5->Effects & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
							{
								num7 = 1f - num7;
							}
							ptr6->Position.X = x;
							ptr6->Position.Y = y;
							ptr6->Position.Z = 0f;
							ptr6->TextureCoordinate.X = (ptr5->Source.X + num7 * ptr5->Source.Z) * num;
							ptr6->TextureCoordinate.Y = (ptr5->Source.Y + num8 * ptr5->Source.W) * num2;
							ptr6++;
						}
						ptr5++;
					}
				}
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0051D8F0 File Offset: 0x0051BAF0
		private void FlushLayered()
		{
			Array.Sort<TileBatch.LayerBatch>(this._batches, 0, this._batchCount);
			int i = 0;
			this._vertexBufferPosition = 0;
			for (int j = 0; j < this._batchCount; j++)
			{
				TileBatch.LayerBatch layerBatch = this._batches[j];
				Texture2D value = this._passTextures[(int)layerBatch.Texture];
				this._graphicsDevice.Textures[0] = value;
				int num = layerBatch.Length;
				int num2 = j;
				int num3 = 0;
				TileBatch.DataSlice dataSlice = default(TileBatch.DataSlice);
				do
				{
					if (this._vertexBufferPosition == i)
					{
						i = 0;
						this._vertexBufferPosition = 0;
						while (i < num)
						{
							if (!this.FillVertexBuffer(this._batches[num2], ref dataSlice, ref num3, ref i))
							{
								break;
							}
							num2++;
							num3 = 0;
						}
						while (i < 2048 && num2 < this._batchCount)
						{
							layerBatch = this._batches[num2];
							if (i + layerBatch.Length > 2048)
							{
								break;
							}
							this.FillVertexBuffer(layerBatch, ref i);
							num2++;
						}
						this._vertexBuffer.SetData<VertexPositionColorTexture>(this._vertices, 0, i * 4, SetDataOptions.Discard);
					}
					int num4 = Math.Min(num, i - this._vertexBufferPosition);
					this._graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, this._vertexBufferPosition * 4, 0, num4 * 4, 0, num4 * 2);
					this._vertexBufferPosition += num4;
					num -= num4;
					this._drawCalls++;
				}
				while (num > 0);
			}
			this._queuedSpriteCount = 0;
			this._batchDataCount = 0;
			this._batchCount = 0;
			this._batchLookup.Clear();
			Array.Clear(this._batchLookupCache, 0, this._batchLookupCache.Length);
			this._passTextureCount = 0;
			this._textureIdLookup.Clear();
			this._currentBatchKey = default(TileBatch.LayerBatchKey);
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0051DAB8 File Offset: 0x0051BCB8
		private void FillVertexBuffer(TileBatch.LayerBatch batch, ref int vbCount)
		{
			TileBatch.DataSlice dataSlice = default(TileBatch.DataSlice);
			int num = 0;
			this.FillVertexBuffer(batch, ref dataSlice, ref num, ref vbCount);
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x0051DADC File Offset: 0x0051BCDC
		private bool FillVertexBuffer(TileBatch.LayerBatch batch, ref TileBatch.DataSlice currentSlice, ref int batchOffset, ref int vbCount)
		{
			if (batchOffset == 0)
			{
				currentSlice = this._batchData[batch.Head];
			}
			Texture2D texture = this._passTextures[(int)batch.Texture];
			while (batchOffset < batch.Length)
			{
				if (currentSlice.Length == 0)
				{
					currentSlice = this._batchData[currentSlice.Next];
				}
				int num = Math.Min(Math.Min(batch.Length - batchOffset, currentSlice.Length), 2048 - vbCount);
				if (num == 0)
				{
					return false;
				}
				this.FillVertexBuffer(texture, this._spriteDataQueue, currentSlice.Start, num, vbCount);
				vbCount += num;
				batchOffset += num;
				currentSlice.Start += num;
				currentSlice.Length -= num;
			}
			return true;
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x0051DBA7 File Offset: 0x0051BDA7
		// Note: this type is marked as 'beforefieldinit'.
		static TileBatch()
		{
			float[] array = new float[4];
			array[1] = 1f;
			array[2] = 1f;
			TileBatch.CORNER_OFFSET_X = array;
			TileBatch.CORNER_OFFSET_Y = new float[]
			{
				0f,
				0f,
				1f,
				1f
			};
		}

		// Token: 0x04004A1B RID: 18971
		private const int MinSliceLength = 2;

		// Token: 0x04004A1C RID: 18972
		private static readonly float[] CORNER_OFFSET_X;

		// Token: 0x04004A1D RID: 18973
		private static readonly float[] CORNER_OFFSET_Y;

		// Token: 0x04004A1E RID: 18974
		private GraphicsDevice _graphicsDevice;

		// Token: 0x04004A1F RID: 18975
		private TileBatch.SpriteData[] _spriteDataQueue = new TileBatch.SpriteData[2048];

		// Token: 0x04004A20 RID: 18976
		private Texture2D[] _spriteTextures = new Texture2D[2048];

		// Token: 0x04004A21 RID: 18977
		private int _queuedSpriteCount;

		// Token: 0x04004A22 RID: 18978
		private bool _layeredSortingEnabled;

		// Token: 0x04004A23 RID: 18979
		private TileBatch.DataSlice[] _batchData = new TileBatch.DataSlice[2048];

		// Token: 0x04004A24 RID: 18980
		private int _batchDataCount;

		// Token: 0x04004A25 RID: 18981
		private TileBatch.LayerBatch[] _batches = new TileBatch.LayerBatch[2048];

		// Token: 0x04004A26 RID: 18982
		private int _batchCount;

		// Token: 0x04004A27 RID: 18983
		private uint? _nextLayerStack;

		// Token: 0x04004A28 RID: 18984
		private int _currentBatchIndex;

		// Token: 0x04004A29 RID: 18985
		private TileBatch.LayerBatchKey _currentBatchKey;

		// Token: 0x04004A2A RID: 18986
		private Dictionary<TileBatch.LayerBatchKey, int> _batchLookup = new Dictionary<TileBatch.LayerBatchKey, int>();

		// Token: 0x04004A2B RID: 18987
		private readonly TileBatch.RecentLayerCacheEntry[] _batchLookupCache = new TileBatch.RecentLayerCacheEntry[2048];

		// Token: 0x04004A2C RID: 18988
		private Texture2D[] _passTextures = new Texture2D[512];

		// Token: 0x04004A2D RID: 18989
		private int _passTextureCount;

		// Token: 0x04004A2E RID: 18990
		private Dictionary<Texture2D, ushort> _textureIdLookup = new Dictionary<Texture2D, ushort>();

		// Token: 0x04004A2F RID: 18991
		private SpriteBatch _spriteBatch;

		// Token: 0x04004A30 RID: 18992
		private static Vector2 _vector2Zero;

		// Token: 0x04004A31 RID: 18993
		private static Rectangle? _nullRectangle;

		// Token: 0x04004A32 RID: 18994
		private DynamicVertexBuffer _vertexBuffer;

		// Token: 0x04004A33 RID: 18995
		private DynamicIndexBuffer _indexBuffer;

		// Token: 0x04004A34 RID: 18996
		private short[] _fallbackIndexData;

		// Token: 0x04004A35 RID: 18997
		private VertexPositionColorTexture[] _vertices = new VertexPositionColorTexture[8192];

		// Token: 0x04004A36 RID: 18998
		private int _vertexBufferPosition;

		// Token: 0x04004A37 RID: 18999
		private int _drawCalls;

		// Token: 0x0200078F RID: 1935
		private struct SpriteData
		{
			// Token: 0x04006FF8 RID: 28664
			public Vector4 Source;

			// Token: 0x04006FF9 RID: 28665
			public Vector4 Destination;

			// Token: 0x04006FFA RID: 28666
			public Vector2 Origin;

			// Token: 0x04006FFB RID: 28667
			public SpriteEffects Effects;

			// Token: 0x04006FFC RID: 28668
			public VertexColors Colors;

			// Token: 0x04006FFD RID: 28669
			public float Rotation;
		}

		// Token: 0x02000790 RID: 1936
		private struct DataSlice
		{
			// Token: 0x04006FFE RID: 28670
			public int Start;

			// Token: 0x04006FFF RID: 28671
			public int Length;

			// Token: 0x04007000 RID: 28672
			public int Next;
		}

		// Token: 0x02000791 RID: 1937
		private struct LayerBatch : IComparable<TileBatch.LayerBatch>
		{
			// Token: 0x1700052B RID: 1323
			// (get) Token: 0x0600416C RID: 16748 RVA: 0x006B8FB6 File Offset: 0x006B71B6
			public ulong SortKey
			{
				get
				{
					return (ulong)this.LayerStack << 16 | (ulong)this.Texture;
				}
			}

			// Token: 0x1700052C RID: 1324
			// (get) Token: 0x0600416D RID: 16749 RVA: 0x006B8FCA File Offset: 0x006B71CA
			public bool CurrentSliceIsFull
			{
				get
				{
					return this.Length >= 2 && (this.Length & this.Length - 1) == 0;
				}
			}

			// Token: 0x0600416E RID: 16750 RVA: 0x006B8FEC File Offset: 0x006B71EC
			public int CompareTo(TileBatch.LayerBatch other)
			{
				return this.SortKey.CompareTo(other.SortKey);
			}

			// Token: 0x04007001 RID: 28673
			public uint LayerStack;

			// Token: 0x04007002 RID: 28674
			public ushort Texture;

			// Token: 0x04007003 RID: 28675
			public int Head;

			// Token: 0x04007004 RID: 28676
			public int Tail;

			// Token: 0x04007005 RID: 28677
			public int Length;

			// Token: 0x04007006 RID: 28678
			public int NextSprite;
		}

		// Token: 0x02000792 RID: 1938
		private struct LayerBatchKey : IEquatable<TileBatch.LayerBatchKey>
		{
			// Token: 0x0600416F RID: 16751 RVA: 0x006B900E File Offset: 0x006B720E
			public bool Equals(TileBatch.LayerBatchKey other)
			{
				return this.LayerStack == other.LayerStack;
			}

			// Token: 0x06004170 RID: 16752 RVA: 0x006B901E File Offset: 0x006B721E
			public override int GetHashCode()
			{
				return (int)(this.LayerStack ^ (uint)this.Texture.GetHashCode());
			}

			// Token: 0x04007007 RID: 28679
			public uint LayerStack;

			// Token: 0x04007008 RID: 28680
			public Texture2D Texture;
		}

		// Token: 0x02000793 RID: 1939
		private struct RecentLayerCacheEntry
		{
			// Token: 0x06004171 RID: 16753 RVA: 0x006B9032 File Offset: 0x006B7232
			public RecentLayerCacheEntry(Texture texture, int batchIndex)
			{
				this.Texture = texture;
				this.BatchIndex = batchIndex;
			}

			// Token: 0x04007009 RID: 28681
			public readonly Texture Texture;

			// Token: 0x0400700A RID: 28682
			public readonly int BatchIndex;
		}
	}
}
