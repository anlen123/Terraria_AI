using System;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.Utilities;

namespace Terraria.Graphics.Light
{
	// Token: 0x020001FE RID: 510
	public class LightMap
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060020F3 RID: 8435 RVA: 0x0052B487 File Offset: 0x00529687
		// (set) Token: 0x060020F4 RID: 8436 RVA: 0x0052B48F File Offset: 0x0052968F
		public int NonVisiblePadding { get; set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060020F5 RID: 8437 RVA: 0x0052B498 File Offset: 0x00529698
		// (set) Token: 0x060020F6 RID: 8438 RVA: 0x0052B4A0 File Offset: 0x005296A0
		public int Width { get; private set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060020F7 RID: 8439 RVA: 0x0052B4A9 File Offset: 0x005296A9
		// (set) Token: 0x060020F8 RID: 8440 RVA: 0x0052B4B1 File Offset: 0x005296B1
		public int Height { get; private set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x0052B4BA File Offset: 0x005296BA
		// (set) Token: 0x060020FA RID: 8442 RVA: 0x0052B4C2 File Offset: 0x005296C2
		public float LightDecayThroughAir { get; set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060020FB RID: 8443 RVA: 0x0052B4CB File Offset: 0x005296CB
		// (set) Token: 0x060020FC RID: 8444 RVA: 0x0052B4D3 File Offset: 0x005296D3
		public float LightDecayThroughSolid { get; set; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060020FD RID: 8445 RVA: 0x0052B4DC File Offset: 0x005296DC
		// (set) Token: 0x060020FE RID: 8446 RVA: 0x0052B4E4 File Offset: 0x005296E4
		public float LightDecayThroughCrackedBrick { get; set; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x0052B4ED File Offset: 0x005296ED
		// (set) Token: 0x06002100 RID: 8448 RVA: 0x0052B4F5 File Offset: 0x005296F5
		public Vector3 LightDecayThroughWater { get; set; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06002101 RID: 8449 RVA: 0x0052B4FE File Offset: 0x005296FE
		// (set) Token: 0x06002102 RID: 8450 RVA: 0x0052B506 File Offset: 0x00529706
		public Vector3 LightDecayThroughHoney { get; set; }

		// Token: 0x17000338 RID: 824
		public Vector3 this[int x, int y]
		{
			get
			{
				return this._colors[this.IndexOf(x, y)];
			}
			set
			{
				this._colors[this.IndexOf(x, y)] = value;
			}
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x0052B53C File Offset: 0x0052973C
		public LightMap()
		{
			this.LightDecayThroughAir = 0.91f;
			this.LightDecayThroughSolid = 0.56f;
			this.LightDecayThroughCrackedBrick = 0.8f;
			this.LightDecayThroughWater = new Vector3(0.88f, 0.96f, 1.015f) * 0.91f;
			this.LightDecayThroughHoney = new Vector3(0.75f, 0.7f, 0.6f) * 0.91f;
			this.Width = 203;
			this.Height = 203;
			this._colors = new Vector3[41209];
			this._mask = new LightMaskMode[41209];
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x0052B5F9 File Offset: 0x005297F9
		public void GetLight(int x, int y, out Vector3 color)
		{
			color = this._colors[this.IndexOf(x, y)];
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x0052B614 File Offset: 0x00529814
		public LightMaskMode GetMask(int x, int y)
		{
			return this._mask[this.IndexOf(x, y)];
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x0052B628 File Offset: 0x00529828
		public void Clear()
		{
			for (int i = 0; i < this._colors.Length; i++)
			{
				this._colors[i].X = 0f;
				this._colors[i].Y = 0f;
				this._colors[i].Z = 0f;
				this._mask[i] = LightMaskMode.None;
			}
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x0052B693 File Offset: 0x00529893
		public void SetMaskAt(int x, int y, LightMaskMode mode)
		{
			this._mask[this.IndexOf(x, y)] = mode;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x0052B6A5 File Offset: 0x005298A5
		public void Blur()
		{
			this.BlurPass();
			this.BlurPass();
			this._random.NextSeed();
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x0052B6BE File Offset: 0x005298BE
		private void BlurPass()
		{
			FastParallel.For(0, this.Width, delegate(int start, int end, object context)
			{
				for (int i = start; i < end; i++)
				{
					this.BlurLine(this.IndexOf(i, 0), this.IndexOf(i, this.Height - 1 - this.NonVisiblePadding), 1);
					this.BlurLine(this.IndexOf(i, this.Height - 1), this.IndexOf(i, this.NonVisiblePadding), -1);
				}
			}, null);
			FastParallel.For(0, this.Height, delegate(int start, int end, object context)
			{
				for (int i = start; i < end; i++)
				{
					this.BlurLine(this.IndexOf(0, i), this.IndexOf(this.Width - 1 - this.NonVisiblePadding, i), this.Height);
					this.BlurLine(this.IndexOf(this.Width - 1, i), this.IndexOf(this.NonVisiblePadding, i), -this.Height);
				}
			}, null);
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x0052B6F4 File Offset: 0x005298F4
		private void BlurLine(int startIndex, int endIndex, int stride)
		{
			Vector3 zero = Vector3.Zero;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int num = startIndex; num != endIndex + stride; num += stride)
			{
				if (zero.X < this._colors[num].X)
				{
					zero.X = this._colors[num].X;
					flag = false;
				}
				else if (!flag)
				{
					if (zero.X < 0.0185f)
					{
						flag = true;
					}
					else
					{
						this._colors[num].X = zero.X;
					}
				}
				if (zero.Y < this._colors[num].Y)
				{
					zero.Y = this._colors[num].Y;
					flag2 = false;
				}
				else if (!flag2)
				{
					if (zero.Y < 0.0185f)
					{
						flag2 = true;
					}
					else
					{
						this._colors[num].Y = zero.Y;
					}
				}
				if (zero.Z < this._colors[num].Z)
				{
					zero.Z = this._colors[num].Z;
					flag3 = false;
				}
				else if (!flag3)
				{
					if (zero.Z < 0.0185f)
					{
						flag3 = true;
					}
					else
					{
						this._colors[num].Z = zero.Z;
					}
				}
				if (!flag || !flag3 || !flag2)
				{
					switch (this._mask[num])
					{
					case LightMaskMode.None:
						if (!flag)
						{
							zero.X *= this.LightDecayThroughAir;
						}
						if (!flag2)
						{
							zero.Y *= this.LightDecayThroughAir;
						}
						if (!flag3)
						{
							zero.Z *= this.LightDecayThroughAir;
						}
						break;
					case LightMaskMode.Solid:
						if (!flag)
						{
							zero.X *= this.LightDecayThroughSolid;
						}
						if (!flag2)
						{
							zero.Y *= this.LightDecayThroughSolid;
						}
						if (!flag3)
						{
							zero.Z *= this.LightDecayThroughSolid;
						}
						break;
					case LightMaskMode.Water:
					{
						float num2 = (float)this._random.WithModifier((ulong)((long)num)).Next(98, 100) / 100f;
						if (!flag)
						{
							zero.X *= this.LightDecayThroughWater.X * num2;
						}
						if (!flag2)
						{
							zero.Y *= this.LightDecayThroughWater.Y * num2;
						}
						if (!flag3)
						{
							zero.Z *= this.LightDecayThroughWater.Z * num2;
						}
						break;
					}
					case LightMaskMode.Honey:
						if (!flag)
						{
							zero.X *= this.LightDecayThroughHoney.X;
						}
						if (!flag2)
						{
							zero.Y *= this.LightDecayThroughHoney.Y;
						}
						if (!flag3)
						{
							zero.Z *= this.LightDecayThroughHoney.Z;
						}
						break;
					case LightMaskMode.CrackedBricks:
						if (!flag)
						{
							zero.X *= this.LightDecayThroughCrackedBrick;
						}
						if (!flag2)
						{
							zero.Y *= this.LightDecayThroughCrackedBrick;
						}
						if (!flag3)
						{
							zero.Z *= this.LightDecayThroughCrackedBrick;
						}
						break;
					}
				}
			}
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x0052BA14 File Offset: 0x00529C14
		private int IndexOf(int x, int y)
		{
			return x * this.Height + y;
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0052BA20 File Offset: 0x00529C20
		public void SetSize(int width, int height)
		{
			if (width * height > this._colors.Length)
			{
				this._colors = new Vector3[width * height];
				this._mask = new LightMaskMode[width * height];
			}
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x04004B62 RID: 19298
		private Vector3[] _colors;

		// Token: 0x04004B63 RID: 19299
		private LightMaskMode[] _mask;

		// Token: 0x04004B69 RID: 19305
		private FastRandom _random = FastRandom.CreateWithRandomSeed();

		// Token: 0x04004B6A RID: 19306
		private const int DEFAULT_WIDTH = 203;

		// Token: 0x04004B6B RID: 19307
		private const int DEFAULT_HEIGHT = 203;
	}
}
