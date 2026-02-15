using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.Utilities.Terraria.Utilities;

namespace Terraria.Graphics
{
	// Token: 0x020001D1 RID: 465
	public struct StormLightningDrawer
	{
		// Token: 0x06001F76 RID: 8054 RVA: 0x0051B7A4 File Offset: 0x005199A4
		public void Draw(Vector2[] positions, float[] rotations, float width, Color color, float progress, bool isMainBolt, FloatRange progressRange, float intensity)
		{
			this._width = width;
			this._color = color;
			this._isMainBolt = isMainBolt;
			this._progress = progress;
			this._progressRange = progressRange;
			this._taperEnd = (this._progressRange.Maximum < 1f);
			MiscShaderData miscShaderData = GameShaders.Misc["StormLightning"];
			miscShaderData.UseSaturation(intensity);
			miscShaderData.UseOpacity(Utils.Remap(this._progress, 0.1f, 0.25f, 0.5f, 1f, true) * Utils.Remap(this._progress, 0.25f, 0.75f, 1f, 0f, true));
			miscShaderData.Apply(null);
			StormLightningDrawer._vertexStrip.PrepareStrip(positions, rotations, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition, null, false);
			StormLightningDrawer._vertexStrip.DrawTrail();
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0051B8E8 File Offset: 0x00519AE8
		private static float WaveTransition(float progressOnStrip, float waveProgress, float transitionLength, float from, float to)
		{
			return Utils.Remap(progressOnStrip, MathHelper.Lerp(-transitionLength, 1f, waveProgress), MathHelper.Lerp(0f, 1f + transitionLength, waveProgress), to, from, true);
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x0051B914 File Offset: 0x00519B14
		private Color StripColors(float progressOnStrip)
		{
			progressOnStrip = this._progressRange.Lerp(progressOnStrip);
			float waveProgress = Utils.Remap(this._progress, 0f, 0.15f, 0f, 1f, true);
			float num = StormLightningDrawer.WaveTransition(progressOnStrip, waveProgress, 0.5f, 0f, 1f);
			float waveProgress2 = Utils.Remap(this._progress, 0.25f, 1f, 0f, 1f, true);
			float num2 = StormLightningDrawer.WaveTransition(progressOnStrip, waveProgress2, 0.5f, 1f, 0f);
			float scale = num * num2;
			return this._color * scale;
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x0051B9B0 File Offset: 0x00519BB0
		private float StripWidth(float progressOnStrip)
		{
			progressOnStrip = this._progressRange.Lerp(progressOnStrip);
			float num = this._width;
			num *= Utils.Remap(progressOnStrip, 0.5f, 1f, 1f, 0.5f, true);
			num *= Utils.Remap(this._progress, 0.5f, 1f, 1f, 0.5f, true);
			if (this._taperEnd)
			{
				num *= Utils.Remap(this._progressRange.Maximum - progressOnStrip, 0.1f, 0f, 1f, this._isMainBolt ? 0.5f : 0f, true);
			}
			return num;
		}

		// Token: 0x040049FF RID: 18943
		private static VertexStrip _vertexStrip = new VertexStrip();

		// Token: 0x04004A00 RID: 18944
		private float _width;

		// Token: 0x04004A01 RID: 18945
		private Color _color;

		// Token: 0x04004A02 RID: 18946
		private bool _isMainBolt;

		// Token: 0x04004A03 RID: 18947
		private float _progress;

		// Token: 0x04004A04 RID: 18948
		private FloatRange _progressRange;

		// Token: 0x04004A05 RID: 18949
		private bool _taperEnd;
	}
}
