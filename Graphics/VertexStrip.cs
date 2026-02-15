using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics
{
	// Token: 0x020001D3 RID: 467
	public class VertexStrip
	{
		// Token: 0x06001F7F RID: 8063 RVA: 0x0051BC01 File Offset: 0x00519E01
		public void Reset(int expectedVertexCount = 0)
		{
			this._vertexAmountCurrentlyMaintained = 0;
			this._indicesAmountCurrentlyMaintained = 0;
			if (this._vertices.Length < expectedVertexCount)
			{
				Array.Resize<VertexStrip.CustomVertexInfo>(ref this._vertices, expectedVertexCount);
			}
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0051BC28 File Offset: 0x00519E28
		public void PrepareStrip(Vector2[] positions, float[] rotations, VertexStrip.StripColorFunction colorFunction, VertexStrip.StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), int? expectedVertexPairsAmount = null, bool includeBacksides = false)
		{
			int num = positions.Length;
			this.Reset(num * 2);
			int num2 = num;
			if (expectedVertexPairsAmount != null)
			{
				num2 = expectedVertexPairsAmount.Value;
			}
			int num3 = 0;
			while (num3 < num && !(positions[num3] == Vector2.Zero))
			{
				Vector2 pos = positions[num3] + offsetForAllPositions;
				float rot = MathHelper.WrapAngle(rotations[num3]);
				float progressOnStrip = (float)num3 / (float)(num2 - 1);
				this.AddVertexPair(colorFunction, widthFunction, pos, rot, progressOnStrip);
				num3++;
			}
			this.PrepareIndices(includeBacksides);
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0051BCAC File Offset: 0x00519EAC
		public void PrepareStripWithProceduralPadding(Vector2[] positions, float[] rotations, VertexStrip.StripColorFunction colorFunction, VertexStrip.StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), bool includeBacksides = false, bool tryStoppingOddBug = true)
		{
			this._temporaryPositionsCache.Clear();
			this._temporaryRotationsCache.Clear();
			int num = 0;
			while (num < positions.Length && !(positions[num] == Vector2.Zero))
			{
				Vector2 vector = positions[num];
				float num2 = MathHelper.WrapAngle(rotations[num]);
				this._temporaryPositionsCache.Add(vector);
				this._temporaryRotationsCache.Add(num2);
				if (num + 1 < positions.Length && positions[num + 1] != Vector2.Zero)
				{
					Vector2 vector2 = positions[num + 1];
					float num3 = MathHelper.WrapAngle(rotations[num + 1]);
					int num4 = (int)(Math.Abs(MathHelper.WrapAngle(num3 - num2)) / 0.2617994f);
					if (num4 != 0)
					{
						float num5 = vector.Distance(vector2);
						Vector2 value = vector + num2.ToRotationVector2() * num5;
						Vector2 value2 = vector2 + num3.ToRotationVector2() * -num5;
						int num6 = num4 + 2;
						float num7 = 1f / (float)num6;
						Vector2 target = vector;
						for (float num8 = num7; num8 < 1f; num8 += num7)
						{
							Vector2 vector3 = Vector2.CatmullRom(value, vector, vector2, value2, num8);
							float num9 = MathHelper.WrapAngle(vector3.DirectionTo(target).ToRotation());
							if (float.IsNaN(num9))
							{
								num9 = this._temporaryRotationsCache.Last<float>();
							}
							this._temporaryPositionsCache.Add(vector3);
							this._temporaryRotationsCache.Add(num9);
							target = vector3;
						}
					}
				}
				num++;
			}
			this.Reset(this._temporaryPositionsCache.Count * 2);
			int count = this._temporaryPositionsCache.Count;
			Vector2 zero = Vector2.Zero;
			int num10 = 0;
			while (num10 < count && (!tryStoppingOddBug || !(this._temporaryPositionsCache[num10] == zero)))
			{
				Vector2 pos = this._temporaryPositionsCache[num10] + offsetForAllPositions;
				float rot = this._temporaryRotationsCache[num10];
				float progressOnStrip = (float)num10 / (float)(count - 1);
				this.AddVertexPair(colorFunction, widthFunction, pos, rot, progressOnStrip);
				num10++;
			}
			this.PrepareIndices(includeBacksides);
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0051BECC File Offset: 0x0051A0CC
		public void PrepareIndices(bool includeBacksides)
		{
			int num = this._vertexAmountCurrentlyMaintained / 2 - 1;
			int num2 = 6 + includeBacksides.ToInt() * 6;
			int num3 = num * num2;
			this._indicesAmountCurrentlyMaintained = num3;
			if (this._indices.Length < num3)
			{
				Array.Resize<short>(ref this._indices, num3);
			}
			short num4 = 0;
			while ((int)num4 < num)
			{
				short num5 = (short)((int)num4 * num2);
				int num6 = (int)(num4 * 2);
				this._indices[(int)num5] = (short)num6;
				this._indices[(int)(num5 + 1)] = (short)(num6 + 1);
				this._indices[(int)(num5 + 2)] = (short)(num6 + 2);
				this._indices[(int)(num5 + 3)] = (short)(num6 + 2);
				this._indices[(int)(num5 + 4)] = (short)(num6 + 1);
				this._indices[(int)(num5 + 5)] = (short)(num6 + 3);
				if (includeBacksides)
				{
					this._indices[(int)(num5 + 6)] = (short)(num6 + 2);
					this._indices[(int)(num5 + 7)] = (short)(num6 + 1);
					this._indices[(int)(num5 + 8)] = (short)num6;
					this._indices[(int)(num5 + 9)] = (short)(num6 + 2);
					this._indices[(int)(num5 + 10)] = (short)(num6 + 3);
					this._indices[(int)(num5 + 11)] = (short)(num6 + 1);
				}
				num4 += 1;
			}
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x0051BFF0 File Offset: 0x0051A1F0
		public void AddVertexPair(VertexStrip.StripColorFunction colorFunction, VertexStrip.StripHalfWidthFunction widthFunction, Vector2 pos, float rot, float progressOnStrip)
		{
			Color vertexColor = colorFunction(progressOnStrip);
			float scaleFactor = widthFunction(progressOnStrip);
			Vector2 value = MathHelper.WrapAngle(rot - 1.5707964f).ToRotationVector2() * scaleFactor;
			this.AddVertexPair(pos + value, pos - value, progressOnStrip, vertexColor);
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x0051C040 File Offset: 0x0051A240
		public void AddVertexPair(Vector2 a, Vector2 b, Vector3 uvA, Vector3 uvB, Color vertexColor)
		{
			while (this._vertexAmountCurrentlyMaintained + 1 >= this._vertices.Length)
			{
				Array.Resize<VertexStrip.CustomVertexInfo>(ref this._vertices, this._vertices.Length * 2);
			}
			Vector2.Distance(a, b);
			this._vertices[this._vertexAmountCurrentlyMaintained].Position = a;
			this._vertices[this._vertexAmountCurrentlyMaintained + 1].Position = b;
			this._vertices[this._vertexAmountCurrentlyMaintained].TexCoord = uvA;
			this._vertices[this._vertexAmountCurrentlyMaintained + 1].TexCoord = uvB;
			this._vertices[this._vertexAmountCurrentlyMaintained].Color = vertexColor;
			this._vertices[this._vertexAmountCurrentlyMaintained + 1].Color = vertexColor;
			this._vertexAmountCurrentlyMaintained += 2;
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0051C120 File Offset: 0x0051A320
		public void AddVertexPair(Vector2 a, Vector2 b, float uv_x, Color vertexColor)
		{
			while (this._vertexAmountCurrentlyMaintained + 1 >= this._vertices.Length)
			{
				Array.Resize<VertexStrip.CustomVertexInfo>(ref this._vertices, this._vertices.Length * 2);
			}
			float num = Vector2.Distance(a, b);
			this._vertices[this._vertexAmountCurrentlyMaintained].Position = a;
			this._vertices[this._vertexAmountCurrentlyMaintained + 1].Position = b;
			this._vertices[this._vertexAmountCurrentlyMaintained].TexCoord = new Vector3(uv_x, num, num);
			this._vertices[this._vertexAmountCurrentlyMaintained + 1].TexCoord = new Vector3(uv_x, 0f, num);
			this._vertices[this._vertexAmountCurrentlyMaintained].Color = vertexColor;
			this._vertices[this._vertexAmountCurrentlyMaintained + 1].Color = vertexColor;
			this._vertexAmountCurrentlyMaintained += 2;
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x0051C210 File Offset: 0x0051A410
		public void AddVertexPair(Vector2 v1, Vector2 v2, float uv_x, Color color1, Color color2)
		{
			while (this._vertexAmountCurrentlyMaintained + 1 >= this._vertices.Length)
			{
				Array.Resize<VertexStrip.CustomVertexInfo>(ref this._vertices, this._vertices.Length * 2);
			}
			float num = Vector2.Distance(v1, v2);
			VertexStrip.CustomVertexInfo[] vertices = this._vertices;
			int vertexAmountCurrentlyMaintained = this._vertexAmountCurrentlyMaintained;
			this._vertexAmountCurrentlyMaintained = vertexAmountCurrentlyMaintained + 1;
			vertices[vertexAmountCurrentlyMaintained] = new VertexStrip.CustomVertexInfo(v1, color1, new Vector3(uv_x, num, num));
			VertexStrip.CustomVertexInfo[] vertices2 = this._vertices;
			vertexAmountCurrentlyMaintained = this._vertexAmountCurrentlyMaintained;
			this._vertexAmountCurrentlyMaintained = vertexAmountCurrentlyMaintained + 1;
			vertices2[vertexAmountCurrentlyMaintained] = new VertexStrip.CustomVertexInfo(v2, color2, new Vector3(uv_x, 0f, num));
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x0051C2AC File Offset: 0x0051A4AC
		public void DrawTrail()
		{
			if (this._vertexAmountCurrentlyMaintained < 3)
			{
				return;
			}
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			VertexBufferBinding[] vertexBuffers = graphicsDevice.GetVertexBuffers();
			IndexBuffer indices = graphicsDevice.Indices;
			graphicsDevice.DrawUserIndexedPrimitives<VertexStrip.CustomVertexInfo>(PrimitiveType.TriangleList, this._vertices, 0, this._vertexAmountCurrentlyMaintained, this._indices, 0, this._indicesAmountCurrentlyMaintained / 3);
			graphicsDevice.SetVertexBuffers(vertexBuffers);
			graphicsDevice.Indices = indices;
		}

		// Token: 0x04004A07 RID: 18951
		private VertexStrip.CustomVertexInfo[] _vertices = new VertexStrip.CustomVertexInfo[1];

		// Token: 0x04004A08 RID: 18952
		private int _vertexAmountCurrentlyMaintained;

		// Token: 0x04004A09 RID: 18953
		private short[] _indices = new short[1];

		// Token: 0x04004A0A RID: 18954
		private int _indicesAmountCurrentlyMaintained;

		// Token: 0x04004A0B RID: 18955
		private List<Vector2> _temporaryPositionsCache = new List<Vector2>();

		// Token: 0x04004A0C RID: 18956
		private List<float> _temporaryRotationsCache = new List<float>();

		// Token: 0x0200078C RID: 1932
		// (Invoke) Token: 0x06004162 RID: 16738
		public delegate Color StripColorFunction(float progressOnStrip);

		// Token: 0x0200078D RID: 1933
		// (Invoke) Token: 0x06004166 RID: 16742
		public delegate float StripHalfWidthFunction(float progressOnStrip);

		// Token: 0x0200078E RID: 1934
		private struct CustomVertexInfo : IVertexType
		{
			// Token: 0x06004169 RID: 16745 RVA: 0x006B8F49 File Offset: 0x006B7149
			public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
			{
				this.Position = position;
				this.Color = color;
				this.TexCoord = texCoord;
			}

			// Token: 0x1700052A RID: 1322
			// (get) Token: 0x0600416A RID: 16746 RVA: 0x006B8F60 File Offset: 0x006B7160
			public VertexDeclaration VertexDeclaration
			{
				get
				{
					return VertexStrip.CustomVertexInfo._vertexDeclaration;
				}
			}

			// Token: 0x04006FF4 RID: 28660
			public Vector2 Position;

			// Token: 0x04006FF5 RID: 28661
			public Color Color;

			// Token: 0x04006FF6 RID: 28662
			public Vector3 TexCoord;

			// Token: 0x04006FF7 RID: 28663
			private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[]
			{
				new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
				new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
				new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
			});
		}
	}
}
