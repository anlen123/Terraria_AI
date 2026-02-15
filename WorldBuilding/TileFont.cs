using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000BC RID: 188
	public class TileFont
	{
		// Token: 0x06001795 RID: 6037 RVA: 0x004DE0F8 File Offset: 0x004DC2F8
		public static void DrawString(Point start, string text, TileFont.DrawMode mode)
		{
			Point position = start;
			foreach (char c in text)
			{
				if (c == '\n')
				{
					position.X = start.X;
					position.Y += 6;
				}
				byte[] charData;
				if (TileFont.MicroFont.TryGetValue(c, out charData))
				{
					TileFont.DrawChar(position, charData, mode);
					position.X += 6;
				}
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x004DE164 File Offset: 0x004DC364
		private static void DrawChar(Point position, byte[] charData, TileFont.DrawMode mode)
		{
			if (mode.HasBackground)
			{
				for (int i = -1; i < charData.Length + 1; i++)
				{
					for (int j = -1; j < 6; j++)
					{
						Main.tile[position.X + j, position.Y + i].ResetToType(mode.BackgroundTile);
						WorldGen.TileFrame(position.X + j, position.Y + i, false, false);
					}
				}
			}
			for (int k = 0; k < charData.Length; k++)
			{
				int num = (int)charData[k] << 1;
				for (int l = 0; l < 5; l++)
				{
					if ((num & 128) == 128)
					{
						Main.tile[position.X + l, position.Y + k].ResetToType(mode.ForegroundTile);
						WorldGen.TileFrame(position.X + l, position.Y + k, false, false);
					}
					num <<= 1;
				}
			}
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x004DE248 File Offset: 0x004DC448
		public static Point MeasureString(string text)
		{
			Point zero = Point.Zero;
			Point point = zero;
			Point point2 = new Point(0, 5);
			foreach (char c in text)
			{
				if (c == '\n')
				{
					point.X = zero.X;
					point.Y += 6;
					point2.Y = point.Y + 5;
				}
				byte[] array;
				if (TileFont.MicroFont.TryGetValue(c, out array))
				{
					point.X += 6;
					point2.X = Math.Max(point2.X, point.X - 1);
				}
			}
			return point2;
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x004DE2EC File Offset: 0x004DC4EC
		public static void HLineLabel(Point start, int width, string text, TileFont.DrawMode mode, bool rightSideText = false)
		{
			Point point = TileFont.MeasureString(text);
			for (int i = start.X; i < start.X + width; i++)
			{
				Main.tile[i, start.Y].ResetToType(mode.ForegroundTile);
				WorldGen.TileFrame(i, start.Y, false, false);
			}
			TileFont.DrawString(new Point(rightSideText ? (start.X + width + 1) : (start.X - point.X - 1), start.Y - point.Y / 2), text, mode);
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x004DE37C File Offset: 0x004DC57C
		public static void VLineLabel(Point start, int height, string text, TileFont.DrawMode mode, bool bottomText = false)
		{
			Point point = TileFont.MeasureString(text);
			for (int i = start.Y; i < start.Y + height; i++)
			{
				Main.tile[start.X, i].ResetToType(mode.ForegroundTile);
				WorldGen.TileFrame(start.X, i, false, false);
			}
			TileFont.DrawString(new Point(start.X - point.X / 2, bottomText ? (start.Y + height + 1) : (start.Y - point.Y - 1)), text, mode);
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x004DE40C File Offset: 0x004DC60C
		// Note: this type is marked as 'beforefieldinit'.
		static TileFont()
		{
			Dictionary<char, byte[]> dictionary = new Dictionary<char, byte[]>();
			dictionary.Add('A', new byte[]
			{
				124,
				68,
				68,
				124,
				68
			});
			dictionary.Add('B', new byte[]
			{
				124,
				68,
				120,
				68,
				124
			});
			dictionary.Add('C', new byte[]
			{
				124,
				64,
				64,
				64,
				124
			});
			dictionary.Add('D', new byte[]
			{
				120,
				68,
				68,
				68,
				120
			});
			dictionary.Add('E', new byte[]
			{
				124,
				64,
				120,
				64,
				124
			});
			dictionary.Add('F', new byte[]
			{
				124,
				64,
				112,
				64,
				64
			});
			dictionary.Add('G', new byte[]
			{
				124,
				64,
				76,
				68,
				124
			});
			dictionary.Add('H', new byte[]
			{
				68,
				68,
				124,
				68,
				68
			});
			dictionary.Add('I', new byte[]
			{
				124,
				16,
				16,
				16,
				124
			});
			dictionary.Add('J', new byte[]
			{
				12,
				4,
				4,
				68,
				124
			});
			dictionary.Add('K', new byte[]
			{
				68,
				72,
				112,
				72,
				68
			});
			dictionary.Add('L', new byte[]
			{
				64,
				64,
				64,
				64,
				124
			});
			dictionary.Add('M', new byte[]
			{
				68,
				108,
				84,
				68,
				68
			});
			dictionary.Add('N', new byte[]
			{
				68,
				100,
				84,
				76,
				68
			});
			dictionary.Add('O', new byte[]
			{
				124,
				68,
				68,
				68,
				124
			});
			dictionary.Add('P', new byte[]
			{
				120,
				68,
				120,
				64,
				64
			});
			dictionary.Add('Q', new byte[]
			{
				124,
				68,
				68,
				124,
				16
			});
			dictionary.Add('R', new byte[]
			{
				120,
				68,
				120,
				68,
				68
			});
			dictionary.Add('S', new byte[]
			{
				124,
				64,
				124,
				4,
				124
			});
			dictionary.Add('T', new byte[]
			{
				124,
				16,
				16,
				16,
				16
			});
			dictionary.Add('U', new byte[]
			{
				68,
				68,
				68,
				68,
				124
			});
			dictionary.Add('V', new byte[]
			{
				68,
				68,
				40,
				40,
				16
			});
			dictionary.Add('W', new byte[]
			{
				68,
				68,
				84,
				84,
				40
			});
			dictionary.Add('X', new byte[]
			{
				68,
				40,
				16,
				40,
				68
			});
			dictionary.Add('Y', new byte[]
			{
				68,
				68,
				40,
				16,
				16
			});
			dictionary.Add('Z', new byte[]
			{
				124,
				8,
				16,
				32,
				124
			});
			dictionary.Add('a', new byte[]
			{
				56,
				4,
				60,
				68,
				60
			});
			dictionary.Add('b', new byte[]
			{
				64,
				120,
				68,
				68,
				120
			});
			dictionary.Add('c', new byte[]
			{
				56,
				68,
				64,
				68,
				56
			});
			dictionary.Add('d', new byte[]
			{
				4,
				60,
				68,
				68,
				60
			});
			dictionary.Add('e', new byte[]
			{
				56,
				68,
				124,
				64,
				60
			});
			dictionary.Add('f', new byte[]
			{
				28,
				32,
				120,
				32,
				32
			});
			dictionary.Add('g', new byte[]
			{
				56,
				68,
				60,
				4,
				120
			});
			dictionary.Add('h', new byte[]
			{
				64,
				64,
				120,
				68,
				68
			});
			dictionary.Add('i', new byte[]
			{
				16,
				0,
				16,
				16,
				16
			});
			dictionary.Add('j', new byte[]
			{
				4,
				4,
				4,
				4,
				120
			});
			dictionary.Add('k', new byte[]
			{
				64,
				72,
				112,
				72,
				68
			});
			dictionary.Add('l', new byte[]
			{
				64,
				64,
				64,
				64,
				60
			});
			dictionary.Add('m', new byte[]
			{
				40,
				84,
				84,
				84,
				84
			});
			dictionary.Add('n', new byte[]
			{
				120,
				68,
				68,
				68,
				68
			});
			dictionary.Add('o', new byte[]
			{
				56,
				68,
				68,
				68,
				56
			});
			dictionary.Add('p', new byte[]
			{
				56,
				68,
				68,
				120,
				64
			});
			dictionary.Add('q', new byte[]
			{
				56,
				68,
				68,
				60,
				4
			});
			dictionary.Add('r', new byte[]
			{
				88,
				100,
				64,
				64,
				64
			});
			dictionary.Add('s', new byte[]
			{
				60,
				64,
				56,
				4,
				120
			});
			dictionary.Add('t', new byte[]
			{
				64,
				112,
				64,
				68,
				56
			});
			dictionary.Add('u', new byte[]
			{
				0,
				68,
				68,
				68,
				56
			});
			dictionary.Add('v', new byte[]
			{
				0,
				68,
				68,
				40,
				16
			});
			dictionary.Add('w', new byte[]
			{
				84,
				84,
				84,
				84,
				40
			});
			dictionary.Add('x', new byte[]
			{
				68,
				68,
				56,
				68,
				68
			});
			dictionary.Add('y', new byte[]
			{
				68,
				68,
				60,
				4,
				120
			});
			dictionary.Add('z', new byte[]
			{
				124,
				4,
				56,
				64,
				124
			});
			dictionary.Add('0', new byte[]
			{
				124,
				76,
				84,
				100,
				124
			});
			dictionary.Add('1', new byte[]
			{
				16,
				48,
				16,
				16,
				56
			});
			dictionary.Add('2', new byte[]
			{
				120,
				4,
				56,
				64,
				124
			});
			dictionary.Add('3', new byte[]
			{
				124,
				4,
				56,
				4,
				124
			});
			dictionary.Add('4', new byte[]
			{
				64,
				64,
				80,
				124,
				16
			});
			dictionary.Add('5', new byte[]
			{
				124,
				64,
				120,
				4,
				120
			});
			dictionary.Add('6', new byte[]
			{
				124,
				64,
				124,
				68,
				124
			});
			dictionary.Add('7', new byte[]
			{
				124,
				4,
				8,
				16,
				16
			});
			dictionary.Add('8', new byte[]
			{
				124,
				68,
				124,
				68,
				124
			});
			dictionary.Add('9', new byte[]
			{
				124,
				68,
				124,
				4,
				124
			});
			Dictionary<char, byte[]> dictionary2 = dictionary;
			char key = '-';
			byte[] array = new byte[5];
			array[2] = 124;
			dictionary2.Add(key, array);
			dictionary.Add(' ', new byte[5]);
			TileFont.MicroFont = dictionary;
		}

		// Token: 0x0400126F RID: 4719
		private static readonly Dictionary<char, byte[]> MicroFont;

		// Token: 0x020006E5 RID: 1765
		public struct DrawMode
		{
			// Token: 0x06003F44 RID: 16196 RVA: 0x00698CF7 File Offset: 0x00696EF7
			public DrawMode(ushort foregroundTile)
			{
				this.ForegroundTile = foregroundTile;
				this.HasBackground = false;
				this.BackgroundTile = 0;
			}

			// Token: 0x06003F45 RID: 16197 RVA: 0x00698D0E File Offset: 0x00696F0E
			public DrawMode(ushort foregroundTile, ushort backgroundTile)
			{
				this.ForegroundTile = foregroundTile;
				this.BackgroundTile = backgroundTile;
				this.HasBackground = true;
			}

			// Token: 0x040067AA RID: 26538
			public readonly ushort ForegroundTile;

			// Token: 0x040067AB RID: 26539
			public readonly ushort BackgroundTile;

			// Token: 0x040067AC RID: 26540
			public readonly bool HasBackground;
		}
	}
}
