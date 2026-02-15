using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002C6 RID: 710
	public class DrowningShader : ChromaShader
	{
		// Token: 0x060025C4 RID: 9668 RVA: 0x00559100 File Offset: 0x00557300
		public override void Update(float elapsedTime)
		{
			Player player = Main.player[Main.myPlayer];
			this._breath = (float)(player.breath * player.breathCDMax - player.breathCD) / (float)(player.breathMax * player.breathCDMax);
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x00559144 File Offset: 0x00557344
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		}, IsTransparent = true)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = new Vector4(0f, 0f, 1f, 1f - this._breath);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x00559194 File Offset: 0x00557394
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		}, IsTransparent = true)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			float num = this._breath * 1.2f - 0.1f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 zero = Vector4.Zero;
				if (canvasPositionOfIndex.Y > num)
				{
					zero = new Vector4(0f, 0f, 1f, MathHelper.Clamp((canvasPositionOfIndex.Y - num) * 5f, 0f, 1f));
				}
				fragment.SetColor(i, zero);
			}
		}

		// Token: 0x04005012 RID: 20498
		private float _breath = 1f;
	}
}
