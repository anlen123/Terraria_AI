using System;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics
{
	// Token: 0x020001C9 RID: 457
	public static class GraphicsUtils
	{
		// Token: 0x06001F52 RID: 8018 RVA: 0x0051A6F4 File Offset: 0x005188F4
		public static int PendingDrawCallCount(this SpriteBatch spriteBatch)
		{
			if (GraphicsUtils.SpriteBatch_spriteQueueCount == null)
			{
				bool flag = typeof(SpriteBatch).Assembly.GetName().Name == "Microsoft.Xna.Framework.Graphics";
				GraphicsUtils.SpriteBatch_spriteQueueCount = typeof(SpriteBatch).GetField(flag ? "spriteQueueCount" : "numSprites", BindingFlags.Instance | BindingFlags.NonPublic);
				GraphicsUtils.SpriteBatch_spriteTextures = typeof(SpriteBatch).GetField(flag ? "spriteTextures" : "textureInfo", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			int n = (int)GraphicsUtils.SpriteBatch_spriteQueueCount.GetValue(spriteBatch);
			Texture[] textures = (Texture2D[])GraphicsUtils.SpriteBatch_spriteTextures.GetValue(spriteBatch);
			return GraphicsUtils.DrawCallCount(textures, n);
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0051A7A8 File Offset: 0x005189A8
		private static int DrawCallCount(Texture[] textures, int n)
		{
			int num = 0;
			if (Program.IsXna)
			{
				Texture texture = null;
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < n; i++)
				{
					Texture texture2 = textures[i];
					if (texture2 != texture)
					{
						if (i > num2)
						{
							num += GraphicsUtils.DrawCallCountXNA(i - num2, ref num3);
						}
						num2 = i;
						texture = texture2;
					}
				}
				num += GraphicsUtils.DrawCallCountXNA(n - num2, ref num3);
			}
			else
			{
				int num4 = 0;
				for (;;)
				{
					int num5 = Math.Min(n, 2048);
					Texture texture3 = textures[num4];
					for (int j = 1; j < num5; j++)
					{
						Texture texture4 = textures[num4 + j];
						if (texture4 != texture3)
						{
							num++;
							texture3 = texture4;
						}
					}
					num++;
					if (n <= 2048)
					{
						break;
					}
					n -= 2048;
					num4 += 2048;
				}
			}
			return num;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0051A868 File Offset: 0x00518A68
		private static int DrawCallCountXNA(int count, ref int vbPos)
		{
			int num = 0;
			while (count > 0)
			{
				int num2 = count;
				if (num2 > 2048 - vbPos)
				{
					num2 = 2048 - vbPos;
					if (num2 < 256)
					{
						vbPos = 0;
						num2 = count;
						if (num2 > 2048)
						{
							num2 = 2048;
						}
					}
				}
				vbPos += num2;
				count -= num2;
				num++;
			}
			return num;
		}

		// Token: 0x040049ED RID: 18925
		private static FieldInfo SpriteBatch_spriteQueueCount;

		// Token: 0x040049EE RID: 18926
		private static FieldInfo SpriteBatch_spriteTextures;
	}
}
