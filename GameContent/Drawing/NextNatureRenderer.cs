using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x0200043F RID: 1087
	public class NextNatureRenderer : INatureRenderer
	{
		// Token: 0x060030CB RID: 12491 RVA: 0x005BD114 File Offset: 0x005BB314
		public void DrawNature(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, SideFlags seams = SideFlags.None)
		{
			seams |= NextNatureRenderer.GetOriginSides(sourceRectangle, origin);
			NextNatureRenderer.Entry item = new NextNatureRenderer.Entry
			{
				Data = new DrawData(texture, position, new Rectangle?(sourceRectangle), color, rotation, origin, scale, effects, 0f),
				IsGlowMask = false,
				Seams = seams
			};
			this._entries.Add(item);
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x005BD178 File Offset: 0x005BB378
		private static SideFlags GetOriginSides(Rectangle sourceRectangle, Vector2 origin)
		{
			float num = origin.X / (float)sourceRectangle.Width;
			double num2 = (double)(1f - num);
			float num3 = origin.Y / (float)sourceRectangle.Height;
			float num4 = 1f - num3;
			SideFlags sideFlags = SideFlags.None;
			if ((double)num < 0.25)
			{
				sideFlags |= SideFlags.Left;
			}
			if (num2 < 0.25)
			{
				sideFlags |= SideFlags.Right;
			}
			if ((double)num3 < 0.25)
			{
				sideFlags |= SideFlags.Top;
			}
			if ((double)num4 < 0.25)
			{
				sideFlags |= SideFlags.Bottom;
			}
			return sideFlags;
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x005BD1F8 File Offset: 0x005BB3F8
		public void DrawGlowmask(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			NextNatureRenderer.Entry item = new NextNatureRenderer.Entry
			{
				Data = new DrawData(texture, position, sourceRectangle, color, rotation, origin, scale, effects, 0f),
				IsGlowMask = true
			};
			this._entries.Add(item);
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x005BD244 File Offset: 0x005BB444
		public void DrawAfterAllObjects(SpriteBatchBeginner beginner)
		{
			if (this._entries.Count == 0)
			{
				return;
			}
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			float num = 0f;
			if (Main.dayTime)
			{
				float fromValue = (float)Main.time;
				float num2 = 54000f;
				float val = Utils.Remap(fromValue, 1200f, 5400f, 0f, 1f, true) * Utils.Remap(fromValue, 1200f, 7200f, 1f, 0f, true) * 0.3f;
				float num3 = Utils.Remap(fromValue, num2 - 10800f, num2 - 4200f, 0f, 1f, true) * Utils.Remap(fromValue, num2 - 1800f, num2 - 600f, 1f, 0f, true) * 0.4f;
				num3 *= num3;
				float val2 = Utils.Remap(fromValue, 0f, 7200f, 0f, 1f, true) * Utils.Remap(fromValue, num2 - 7200f, num2, 1f, 0f, true) * 0f;
				num = Math.Max(Math.Max(val, num3), val2);
				if (Main.eclipse)
				{
					num = 0f;
				}
			}
			num *= 0.4f;
			Vector2 lastCelestialBodyPosition = Main.LastCelestialBodyPosition;
			float num4 = Utils.Remap(Math.Min(lastCelestialBodyPosition.X, 1f - lastCelestialBodyPosition.X), 0f, 0.010416667f, 0f, 1f, true);
			num *= num4;
			if (!Main.ShouldDrawSurfaceBackground() || !Main.ForegroundSunlightEffects)
			{
				num = 0f;
			}
			if (num == 0f)
			{
				this.DrawWithoutShader(beginner, Main.spriteBatch);
			}
			else
			{
				this.DrawWithLitNatureShader(beginner, num, lastCelestialBodyPosition);
			}
			this._entries.Clear();
			TimeLogger.Nature.AddTime(fromTimestamp);
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x005BD404 File Offset: 0x005BB604
		private void DrawWithoutShader(SpriteBatchBeginner beginner, SpriteBatch spriteBatch)
		{
			beginner.Begin(spriteBatch);
			foreach (NextNatureRenderer.Entry entry in this._entries)
			{
				DrawData data = entry.Data;
				data.Draw(spriteBatch);
			}
			spriteBatch.End();
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x005BD470 File Offset: 0x005BB670
		private void DrawWithLitNatureShader(SpriteBatchBeginner beginner, float visibility, Vector2 sunPosition)
		{
			SpriteDrawBuffer spriteBuffer = Main.spriteBuffer;
			foreach (NextNatureRenderer.Entry entry in this._entries)
			{
				DrawData data = entry.Data;
				data.Draw(spriteBuffer);
			}
			MiscShaderData miscShaderData = GameShaders.Misc["LitNature"];
			Vector2 vector = Vector2.Transform(Main.ReverseGravitySupport(sunPosition * Main.ScreenSize.ToVector2(), 0f), Matrix.Invert(Main.Transform));
			Vector4 specificData = new Vector4(vector.X, vector.Y, visibility, 0f);
			miscShaderData.UseImage1(Main.HorizonHelper.SunVisibilityPixelTexture);
			miscShaderData.UseSpriteTransformMatrix(new Matrix?(beginner.transformMatrix));
			Color color;
			Color color2;
			HorizonHelper.GetCelestialBodyColors(out color, out color2);
			Color color3 = Main.dayTime ? color : color2;
			Vector3 vector2 = Main.rgbToHsl(color3);
			color3 = Main.hslToRgb(vector2.X, Utils.Clamp<float>(vector2.Y * 8f, 0f, 1f), vector2.Z * 1f, byte.MaxValue) * 0.5f;
			miscShaderData.UseColor(Color.Lerp(color3, new Color(255, 200, 0), 0.8f));
			int num = 0;
			foreach (NextNatureRenderer.Entry entry2 in this._entries)
			{
				specificData.W = (float)(entry2.IsGlowMask ? ((SideFlags)(-1)) : entry2.Seams);
				miscShaderData.UseShaderSpecificData(specificData);
				miscShaderData.Apply(new DrawData?(entry2.Data));
				spriteBuffer.DrawSingle(num++);
			}
			spriteBuffer.Unbind();
		}

		// Token: 0x040056F3 RID: 22259
		private readonly List<NextNatureRenderer.Entry> _entries = new List<NextNatureRenderer.Entry>();

		// Token: 0x0200093E RID: 2366
		private struct Entry
		{
			// Token: 0x0400750C RID: 29964
			public DrawData Data;

			// Token: 0x0400750D RID: 29965
			public SideFlags Seams;

			// Token: 0x0400750E RID: 29966
			public bool IsGlowMask;
		}
	}
}
