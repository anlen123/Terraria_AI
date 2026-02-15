using System;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B6 RID: 694
	internal class DebugKeyboard : RgbDevice
	{
		// Token: 0x06002592 RID: 9618 RVA: 0x0055791A File Offset: 0x00555B1A
		private DebugKeyboard(Fragment fragment) : base(4, 6, fragment, new DeviceColorProfile())
		{
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x0055792C File Offset: 0x00555B2C
		public static DebugKeyboard Create()
		{
			int num = 400;
			int num2 = 100;
			Point[] array = new Point[num * num2];
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					array[i * num + j] = new Point(j / 10, i / 10);
				}
			}
			Vector2[] array2 = new Vector2[num * num2];
			for (int k = 0; k < num2; k++)
			{
				for (int l = 0; l < num; l++)
				{
					array2[k * num + l] = new Vector2((float)l / (float)num2, (float)k / (float)num2);
				}
			}
			return new DebugKeyboard(Fragment.FromCustom(array, array2));
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Present()
		{
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x005579DC File Offset: 0x00555BDC
		public override void DebugDraw(IDebugDrawer drawer, Vector2 position, float scale)
		{
			for (int i = 0; i < base.LedCount; i++)
			{
				Vector2 ledCanvasPosition = base.GetLedCanvasPosition(i);
				drawer.DrawSquare(new Vector4(ledCanvasPosition * scale + position, scale / 100f, scale / 100f), new Color(base.GetUnprocessedLedColor(i)));
			}
		}
	}
}
