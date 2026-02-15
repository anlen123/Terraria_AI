using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameInput
{
	// Token: 0x0200008A RID: 138
	public class SmartSelectGamepadPointer
	{
		// Token: 0x060015BB RID: 5563 RVA: 0x004D28BF File Offset: 0x004D0ABF
		public bool ShouldBeUsed()
		{
			return PlayerInput.UsingGamepad && Main.LocalPlayer.controlTorch && Main.SmartCursorIsUsed;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x004D28DC File Offset: 0x004D0ADC
		public void SmartSelectLookup_GetTargetTile(Player player, out int tX, out int tY)
		{
			tX = (int)(((float)Main.mouseX + Main.screenPosition.X) / 16f);
			tY = (int)(((float)Main.mouseY + Main.screenPosition.Y) / 16f);
			if (player.gravDir == -1f)
			{
				tY = (int)((Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY) / 16f);
			}
			if (!this.ShouldBeUsed())
			{
				return;
			}
			Point point = this.GetPointerPosition().ToPoint();
			tX = (int)(((float)point.X + Main.screenPosition.X) / 16f);
			tY = (int)(((float)point.Y + Main.screenPosition.Y) / 16f);
			if (player.gravDir == -1f)
			{
				tY = (int)((Main.screenPosition.Y + (float)Main.screenHeight - (float)point.Y) / 16f);
			}
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x004D29C5 File Offset: 0x004D0BC5
		public void UpdateSize(Vector2 size)
		{
			this._size = size;
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x004D29CE File Offset: 0x004D0BCE
		public void UpdateCenter(Vector2 center)
		{
			this._center = center;
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x004D29D8 File Offset: 0x004D0BD8
		public Vector2 GetPointerPosition()
		{
			Vector2 vector = (new Vector2((float)Main.mouseX, (float)Main.mouseY) - this._center) / this._size;
			float num = Math.Abs(vector.X);
			if (num < Math.Abs(vector.Y))
			{
				num = Math.Abs(vector.Y);
			}
			if (num > 1f)
			{
				vector /= num;
			}
			vector *= Main.GameViewMatrix.RenderZoom.X;
			return vector * this._distUniform + this._center;
		}

		// Token: 0x040010F9 RID: 4345
		private Vector2 _size;

		// Token: 0x040010FA RID: 4346
		private Vector2 _center;

		// Token: 0x040010FB RID: 4347
		private Vector2 _distUniform = new Vector2(80f, 64f);
	}
}
