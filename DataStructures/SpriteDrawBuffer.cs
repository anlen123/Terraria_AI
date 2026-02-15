using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;

namespace Terraria.DataStructures
{
	// Token: 0x0200058B RID: 1419
	public class SpriteDrawBuffer
	{
		// Token: 0x0600380A RID: 14346 RVA: 0x0062FA18 File Offset: 0x0062DC18
		public SpriteDrawBuffer(GraphicsDevice graphicsDevice, int bufferSize = 2048)
		{
			this.graphicsDevice = graphicsDevice;
			this.bufferSize = bufferSize;
			this.ResizeArrays(bufferSize);
			this.spriteBatch = new SpriteBatch(graphicsDevice);
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x0062FA48 File Offset: 0x0062DC48
		public void ResizeArrays(int count)
		{
			Array.Resize<VertexPositionColorTexture>(ref this.vertices, count * 4);
			Array.Resize<Texture>(ref this.textures, count);
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x0062FA64 File Offset: 0x0062DC64
		public void ApplyDefaultSpriteEffect(RasterizerState rasterizer, Matrix transformation)
		{
			this.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, rasterizer, null, transformation);
			this.spriteBatch.End();
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x0062FA83 File Offset: 0x0062DC83
		public void ApplyDefaultSpriteEffect()
		{
			this.spriteBatch.Begin();
			this.spriteBatch.End();
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x0062FA9C File Offset: 0x0062DC9C
		private void CheckBuffers()
		{
			if (this.vertexBuffer == null || this.vertexBuffer.IsDisposed)
			{
				if (this.vertexBuffer != null)
				{
					this.vertexBuffer.Dispose();
				}
				this.vertexBuffer = new DynamicVertexBuffer(this.graphicsDevice, typeof(VertexPositionColorTexture), this.bufferSize * 4, BufferUsage.WriteOnly);
			}
			if (this.indexBuffer == null || this.indexBuffer.IsDisposed)
			{
				if (this.indexBuffer != null)
				{
					this.indexBuffer.Dispose();
				}
				this.indexBuffer = new IndexBuffer(this.graphicsDevice, typeof(ushort), this.bufferSize * 6, BufferUsage.WriteOnly);
				this.indexBuffer.SetData<ushort>(SpriteDrawBuffer.GenIndexBuffer(this.bufferSize));
			}
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x0062FB58 File Offset: 0x0062DD58
		private static ushort[] GenIndexBuffer(int maxSprites)
		{
			ushort[] array = new ushort[maxSprites * 6];
			int i = 0;
			ushort num = 0;
			while (i < maxSprites)
			{
				array[i++] = num;
				array[i++] = num + 1;
				array[i++] = num + 2;
				array[i++] = num + 3;
				array[i++] = num + 2;
				array[i++] = num + 1;
				num += 4;
			}
			return array;
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x0062FBC0 File Offset: 0x0062DDC0
		private void Bind()
		{
			if (this.preBindVertexBuffers != null)
			{
				return;
			}
			this.preBindVertexBuffers = this.graphicsDevice.GetVertexBuffers();
			this.preBindIndexBuffer = this.graphicsDevice.Indices;
			this.graphicsDevice.SetVertexBuffer(this.vertexBuffer);
			this.graphicsDevice.Indices = this.indexBuffer;
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x0062FC1A File Offset: 0x0062DE1A
		public void Unbind()
		{
			if (this.preBindVertexBuffers == null)
			{
				return;
			}
			this.graphicsDevice.SetVertexBuffers(this.preBindVertexBuffers);
			this.graphicsDevice.Indices = this.preBindIndexBuffer;
			this.preBindVertexBuffers = null;
			this.preBindIndexBuffer = null;
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x0062FC58 File Offset: 0x0062DE58
		public int DrawRange(int index, int count)
		{
			this.vertexCount = 0;
			this.CheckBuffers();
			this.Bind();
			this.graphicsDevice.Textures[0] = this.textures[index];
			int num = 0;
			while (count > 0)
			{
				if (this.uploadedSpriteIndex < 0 || index < this.uploadedSpriteIndex || index + count > this.uploadedSpriteIndex + this.bufferSize)
				{
					this.vertexBuffer.SetData<VertexPositionColorTexture>(this.vertices, index * 4, Math.Min(this.vertices.Length - index * 4, this.bufferSize * 4), SetDataOptions.Discard);
					this.uploadedSpriteIndex = index;
				}
				int num2 = Math.Min(count, this.bufferSize);
				int num3 = index - this.uploadedSpriteIndex;
				this.graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, num3 * 4, 0, num2 * 4, 0, num2 * 2);
				count -= num2;
				index += num2;
				num++;
			}
			return num;
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x0062FD34 File Offset: 0x0062DF34
		public void DrawSingle(int index)
		{
			this.DrawRange(index, 1);
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x0062FD40 File Offset: 0x0062DF40
		public int DrawAll()
		{
			if (this.vertexCount == 0)
			{
				return 0;
			}
			int num = this.vertexCount / 4;
			Texture texture = this.textures[0];
			int num2 = 0;
			int num3 = 0;
			for (int i = 1; i < num; i++)
			{
				Texture texture2 = this.textures[i];
				if (texture2 != texture)
				{
					num3 += this.DrawRange(num2, i - num2);
					num2 = i;
					texture = texture2;
				}
			}
			num3 += this.DrawRange(num2, num - num2);
			this.Unbind();
			return num3;
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x0062FDB8 File Offset: 0x0062DFB8
		public void Draw(Texture2D texture, Vector2 position, VertexColors colors)
		{
			this.Draw(texture, position, null, colors, 0f, Vector2.Zero, 1f, SpriteEffects.None);
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x0062FDE8 File Offset: 0x0062DFE8
		public void Draw(Texture2D texture, Rectangle destination, VertexColors colors)
		{
			this.Draw(texture, destination, null, colors);
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x0062FE07 File Offset: 0x0062E007
		public void Draw(Texture2D texture, Rectangle destination, Rectangle? sourceRectangle, VertexColors colors)
		{
			this.Draw(texture, destination, sourceRectangle, colors, 0f, Vector2.Zero, SpriteEffects.None);
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x0062FE20 File Offset: 0x0062E020
		public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors color, float rotation, Vector2 origin, float scale, SpriteEffects effects)
		{
			this.Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale, scale), effects);
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x0062FE48 File Offset: 0x0062E048
		public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
		{
			float z;
			float w;
			if (sourceRectangle != null)
			{
				z = (float)sourceRectangle.Value.Width * scale.X;
				w = (float)sourceRectangle.Value.Height * scale.Y;
			}
			else
			{
				z = (float)texture.Width * scale.X;
				w = (float)texture.Height * scale.Y;
			}
			this.Draw(texture, new Vector4(position.X, position.Y, z, w), sourceRectangle, colors, rotation, origin, effects, 0f);
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x0062FED4 File Offset: 0x0062E0D4
		public void Draw(Texture2D texture, Rectangle destination, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, SpriteEffects effects)
		{
			this.Draw(texture, new Vector4((float)destination.X, (float)destination.Y, (float)destination.Width, (float)destination.Height), sourceRectangle, colors, rotation, origin, effects, 0f);
		}

		// Token: 0x0600381B RID: 14363 RVA: 0x0062FF18 File Offset: 0x0062E118
		public void Draw(Texture2D texture, Vector4 destination, VertexColors colors, float rotation = 0f, Vector2 origin = default(Vector2), SpriteEffects effects = SpriteEffects.None)
		{
			this.Draw(texture, destination, null, colors, rotation, origin, effects, 0f);
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x0062FF44 File Offset: 0x0062E144
		public void Draw(Texture2D texture, Vector4 destinationRectangle, Rectangle? sourceRectangle, VertexColors colors, float rotation = 0f, Vector2 origin = default(Vector2), SpriteEffects effect = SpriteEffects.None, float depth = 0f)
		{
			Vector4 vector;
			if (sourceRectangle != null)
			{
				vector.X = (float)sourceRectangle.Value.X;
				vector.Y = (float)sourceRectangle.Value.Y;
				vector.Z = (float)sourceRectangle.Value.Width;
				vector.W = (float)sourceRectangle.Value.Height;
			}
			else
			{
				vector.X = 0f;
				vector.Y = 0f;
				vector.Z = (float)texture.Width;
				vector.W = (float)texture.Height;
			}
			Vector2 vector2;
			vector2.X = vector.X / (float)texture.Width;
			vector2.Y = vector.Y / (float)texture.Height;
			Vector2 vector3;
			vector3.X = (vector.X + vector.Z) / (float)texture.Width;
			vector3.Y = (vector.Y + vector.W) / (float)texture.Height;
			if ((effect & SpriteEffects.FlipVertically) != SpriteEffects.None)
			{
				float y = vector3.Y;
				vector3.Y = vector2.Y;
				vector2.Y = y;
			}
			if ((effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
			{
				float x = vector3.X;
				vector3.X = vector2.X;
				vector2.X = x;
			}
			this.QueueSprite(destinationRectangle, -origin, colors, vector, vector2, vector3, texture, depth, rotation);
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x006300A0 File Offset: 0x0062E2A0
		private void QueueSprite(Vector4 destinationRect, Vector2 origin, VertexColors colors, Vector4 sourceRectangle, Vector2 texCoordTL, Vector2 texCoordBR, Texture2D texture, float depth, float rotation)
		{
			this.uploadedSpriteIndex = -1;
			float num = origin.X / sourceRectangle.Z;
			float num2 = origin.Y / sourceRectangle.W;
			float x = destinationRect.X;
			float y = destinationRect.Y;
			float z = destinationRect.Z;
			float w = destinationRect.W;
			float num3 = num * z;
			float num4 = num2 * w;
			float num5;
			float num6;
			if (rotation != 0f)
			{
				num5 = (float)Math.Cos((double)rotation);
				num6 = (float)Math.Sin((double)rotation);
			}
			else
			{
				num5 = 1f;
				num6 = 0f;
			}
			int num7 = this.vertexCount / 4;
			if (num7 >= this.textures.Length)
			{
				this.ResizeArrays(this.textures.Length * 2);
			}
			this.textures[num7] = texture;
			this.PushVertex(new Vector3(x + num3 * num5 - num4 * num6, y + num3 * num6 + num4 * num5, depth), colors.TopLeftColor, texCoordTL);
			this.PushVertex(new Vector3(x + (num3 + z) * num5 - num4 * num6, y + (num3 + z) * num6 + num4 * num5, depth), colors.TopRightColor, new Vector2(texCoordBR.X, texCoordTL.Y));
			this.PushVertex(new Vector3(x + num3 * num5 - (num4 + w) * num6, y + num3 * num6 + (num4 + w) * num5, depth), colors.BottomLeftColor, new Vector2(texCoordTL.X, texCoordBR.Y));
			this.PushVertex(new Vector3(x + (num3 + z) * num5 - (num4 + w) * num6, y + (num3 + z) * num6 + (num4 + w) * num5, depth), colors.BottomRightColor, texCoordBR);
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x0063024C File Offset: 0x0062E44C
		private void PushVertex(Vector3 pos, Color color, Vector2 texCoord)
		{
			VertexPositionColorTexture[] array = this.vertices;
			int num = this.vertexCount;
			this.vertexCount = num + 1;
			SpriteDrawBuffer.SetVertex(ref array[num], pos, color, texCoord);
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x0063027D File Offset: 0x0062E47D
		private static void SetVertex(ref VertexPositionColorTexture vertex, Vector3 pos, Color color, Vector2 texCoord)
		{
			vertex.Position = pos;
			vertex.Color = color;
			vertex.TextureCoordinate = texCoord;
		}

		// Token: 0x04005C20 RID: 23584
		private readonly GraphicsDevice graphicsDevice;

		// Token: 0x04005C21 RID: 23585
		private readonly SpriteBatch spriteBatch;

		// Token: 0x04005C22 RID: 23586
		private readonly int bufferSize;

		// Token: 0x04005C23 RID: 23587
		private DynamicVertexBuffer vertexBuffer;

		// Token: 0x04005C24 RID: 23588
		private IndexBuffer indexBuffer;

		// Token: 0x04005C25 RID: 23589
		private int vertexCount;

		// Token: 0x04005C26 RID: 23590
		private VertexPositionColorTexture[] vertices;

		// Token: 0x04005C27 RID: 23591
		private Texture[] textures;

		// Token: 0x04005C28 RID: 23592
		private int uploadedSpriteIndex = -1;

		// Token: 0x04005C29 RID: 23593
		private VertexBufferBinding[] preBindVertexBuffers;

		// Token: 0x04005C2A RID: 23594
		private IndexBuffer preBindIndexBuffer;
	}
}
