using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Localization;
using Terraria.Utilities;

namespace Terraria.Graphics.Capture
{
	// Token: 0x020001DC RID: 476
	internal class CaptureCamera : IDisposable
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06001FEE RID: 8174 RVA: 0x0051F3C9 File Offset: 0x0051D5C9
		public bool IsCapturing
		{
			get
			{
				Monitor.Enter(this._captureLock);
				bool result = this._activeSettings != null;
				Monitor.Exit(this._captureLock);
				return result;
			}
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x0051F3EC File Offset: 0x0051D5EC
		public CaptureCamera(GraphicsDevice graphics)
		{
			CaptureCamera.CameraExists = true;
			this._graphics = graphics;
			this._spriteBatch = new SpriteBatch(graphics);
			try
			{
				this._frameBuffer = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, DepthFormat.None);
				this._filterFrameBuffer1 = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, DepthFormat.None);
				this._filterFrameBuffer2 = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, DepthFormat.None);
				this._waterTarget = new WorldSceneLayerTarget(graphics, 2048, 2048);
			}
			catch
			{
				Main.CaptureModeDisabled = true;
				return;
			}
			this._downscaleSampleState = SamplerState.AnisotropicClamp;
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x0051F4D4 File Offset: 0x0051D6D4
		public void Capture(CaptureSettings settings)
		{
			Main.GlobalTimerPaused = true;
			if (this._activeSettings != null)
			{
				throw new InvalidOperationException("Capture called while another capture was already active.");
			}
			try
			{
				object captureLock = this._captureLock;
				lock (captureLock)
				{
					this._activeSettings = settings;
					this._Capture();
				}
			}
			catch (OutOfMemoryException value)
			{
				Console.WriteLine(value);
				this._renderQueue.Clear();
				this._outputData = null;
				this.FinishCapture();
				Main.NewText(Language.GetTextValue("Error.CaptureOutOfMemory"), byte.MaxValue, 0, 0);
			}
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0051F578 File Offset: 0x0051D778
		private void _Capture()
		{
			Microsoft.Xna.Framework.Rectangle area = this._activeSettings.Area;
			float num = 1f;
			if (this._activeSettings.UseScaling)
			{
				if (area.Width * 16 > 4096)
				{
					num = 4096f / (float)(area.Width * 16);
				}
				if (area.Height * 16 > 4096)
				{
					num = Math.Min(num, 4096f / (float)(area.Height * 16));
				}
				num = Math.Min(1f, num);
				this._outputImageSize = new Size((int)MathHelper.Clamp((float)((int)(num * (float)(area.Width * 16))), 1f, 4096f), (int)MathHelper.Clamp((float)((int)(num * (float)(area.Height * 16))), 1f, 4096f));
				this._outputData = new byte[4 * this._outputImageSize.Width * this._outputImageSize.Height];
				int num2 = (int)Math.Floor((double)(num * 2048f));
				this._scaledFrameData = new byte[4 * num2 * num2];
				this._scaledFrameBuffer = new RenderTarget2D(this._graphics, num2, num2, false, this._graphics.PresentationParameters.BackBufferFormat, DepthFormat.None);
			}
			else
			{
				this._outputData = new byte[16777216];
			}
			this._tilesProcessed = 0f;
			this._totalTiles = (float)(area.Width * area.Height);
			for (int i = area.X; i < area.X + area.Width; i += 126)
			{
				for (int j = area.Y; j < area.Y + area.Height; j += 126)
				{
					int num3 = Math.Min(128, area.X + area.Width - i);
					int num4 = Math.Min(128, area.Y + area.Height - j);
					int width = (int)Math.Floor((double)(num * (float)(num3 * 16)));
					int height = (int)Math.Floor((double)(num * (float)(num4 * 16)));
					int x = (int)Math.Floor((double)(num * (float)((i - area.X) * 16)));
					int y = (int)Math.Floor((double)(num * (float)((j - area.Y) * 16)));
					this._renderQueue.Enqueue(new CaptureCamera.CaptureChunk(new Microsoft.Xna.Framework.Rectangle(i, j, num3, num4), new Microsoft.Xna.Framework.Rectangle(x, y, width, height)));
				}
			}
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0051F7D8 File Offset: 0x0051D9D8
		public void DrawTick()
		{
			Monitor.Enter(this._captureLock);
			if (this._activeSettings == null)
			{
				return;
			}
			if (this._renderQueue.Count > 0)
			{
				CaptureCamera.CaptureChunk captureChunk = this._renderQueue.Dequeue();
				this._graphics.SetRenderTarget(null);
				this._graphics.Clear(Microsoft.Xna.Framework.Color.Transparent);
				Main.instance.TilesRenderer.PrepareForAreaDrawing(captureChunk.Area.Left, captureChunk.Area.Right, captureChunk.Area.Top, captureChunk.Area.Bottom, false);
				Main.instance.TilePaintSystem.PrepareAllRequests();
				this._graphics.SetRenderTarget(this._frameBuffer);
				Main.instance.DrawCapture(captureChunk.Area, this._activeSettings, this);
				if (this._activeSettings.UseScaling)
				{
					this._graphics.SetRenderTarget(this._scaledFrameBuffer);
					this._graphics.Clear(Microsoft.Xna.Framework.Color.Transparent);
					this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, this._downscaleSampleState, DepthStencilState.Default, RasterizerState.CullNone);
					this._spriteBatch.Draw(this._frameBuffer, new Microsoft.Xna.Framework.Rectangle(0, 0, this._scaledFrameBuffer.Width, this._scaledFrameBuffer.Height), Microsoft.Xna.Framework.Color.White);
					this._spriteBatch.End();
					this._graphics.SetRenderTarget(null);
					this._scaledFrameBuffer.GetData<byte>(this._scaledFrameData, 0, this._scaledFrameBuffer.Width * this._scaledFrameBuffer.Height * 4);
					this.DrawBytesToBuffer(this._scaledFrameData, this._outputData, this._scaledFrameBuffer.Width, this._outputImageSize.Width, captureChunk.ScaledArea);
				}
				else
				{
					this._graphics.SetRenderTarget(null);
					this.SaveImage(this._frameBuffer, captureChunk.ScaledArea.Width, captureChunk.ScaledArea.Height, ImageFormat.Png, this._activeSettings.OutputName, string.Concat(new object[]
					{
						captureChunk.Area.X,
						"-",
						captureChunk.Area.Y,
						".png"
					}));
				}
				this._tilesProcessed += (float)(captureChunk.Area.Width * captureChunk.Area.Height);
			}
			if (this._renderQueue.Count == 0)
			{
				this.FinishCapture();
			}
			Monitor.Exit(this._captureLock);
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x0051FA6C File Offset: 0x0051DC6C
		public void BeginDrawCapture()
		{
			if (Lighting.NotRetro)
			{
				Main.instance.GraphicsDevice.SetRenderTarget(Main.skyTarget);
				Main.instance.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);
				Filters.Scene.BeginCapture(this._filterFrameBuffer1);
			}
			Main.waterTarget = this._waterTarget;
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0051FAC3 File Offset: 0x0051DCC3
		public void EndDrawCapture(Vector2 screenSize, Vector2 sceneSize, Vector2 sceneOffset)
		{
			if (Lighting.NotRetro)
			{
				Filters.Scene.EndCapture(this._frameBuffer, this._filterFrameBuffer1, this._filterFrameBuffer2, screenSize, sceneSize, sceneOffset);
			}
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0051FAEC File Offset: 0x0051DCEC
		private unsafe void DrawBytesToBuffer(byte[] sourceBuffer, byte[] destinationBuffer, int sourceBufferWidth, int destinationBufferWidth, Microsoft.Xna.Framework.Rectangle area)
		{
			fixed (byte* ptr = &destinationBuffer[0])
			{
				byte* ptr2 = ptr;
				fixed (byte* ptr3 = &sourceBuffer[0])
				{
					byte* ptr4 = ptr3;
					ptr2 += destinationBufferWidth * area.Y + area.X << 2;
					for (int i = 0; i < area.Height; i++)
					{
						for (int j = 0; j < area.Width; j++)
						{
							if (Program.IsXna)
							{
								ptr2[2] = *ptr4;
								ptr2[1] = ptr4[1];
								*ptr2 = ptr4[2];
								ptr2[3] = ptr4[3];
							}
							else
							{
								*ptr2 = *ptr4;
								ptr2[1] = ptr4[1];
								ptr2[2] = ptr4[2];
								ptr2[3] = ptr4[3];
							}
							ptr4 += 4;
							ptr2 += 4;
						}
						ptr4 += sourceBufferWidth - area.Width << 2;
						ptr2 += destinationBufferWidth - area.Width << 2;
					}
				}
			}
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0051FBC4 File Offset: 0x0051DDC4
		public float GetProgress()
		{
			return this._tilesProcessed / this._totalTiles;
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x0051FBD3 File Offset: 0x0051DDD3
		private bool SaveImage(int width, int height, ImageFormat imageFormat, string filename)
		{
			return CaptureCamera.SaveImage(this._outputData, width, height, imageFormat, filename);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x0051FBE8 File Offset: 0x0051DDE8
		public static bool SaveImage(byte[] data, int width, int height, ImageFormat imageFormat, string filename)
		{
			if (!Utils.TryCreatingDirectory(Main.SavePath + Path.DirectorySeparatorChar.ToString() + "Captures" + Path.DirectorySeparatorChar.ToString()))
			{
				return false;
			}
			bool result;
			try
			{
				if (Program.IsFna)
				{
					PlatformUtilities.SavePng(filename, width, height, data);
				}
				else
				{
					using (Bitmap bitmap = new Bitmap(width, height))
					{
						System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);
						BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
						IntPtr scan = bitmapData.Scan0;
						Marshal.Copy(data, 0, scan, width * height * 4);
						bitmap.UnlockBits(bitmapData);
						bitmap.Save(filename, imageFormat);
						bitmap.Dispose();
					}
				}
				result = true;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				result = false;
			}
			return result;
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x0051FCC4 File Offset: 0x0051DEC4
		private void SaveImage(Texture2D texture, int width, int height, ImageFormat imageFormat, string foldername, string filename)
		{
			string text = string.Concat(new string[]
			{
				Main.SavePath,
				Path.DirectorySeparatorChar.ToString(),
				"Captures",
				Path.DirectorySeparatorChar.ToString(),
				foldername
			});
			string text2 = Path.Combine(text, filename);
			if (!Utils.TryCreatingDirectory(text))
			{
				return;
			}
			if (Program.IsFna)
			{
				int elementCount = texture.Width * texture.Height * 4;
				texture.GetData<byte>(this._outputData, 0, elementCount);
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						this._outputData[num2] = this._outputData[num];
						this._outputData[num2 + 1] = this._outputData[num + 1];
						this._outputData[num2 + 2] = this._outputData[num + 2];
						this._outputData[num2 + 3] = this._outputData[num + 3];
						num += 4;
						num2 += 4;
					}
					num += texture.Width - width << 2;
				}
				PlatformUtilities.SavePng(text2, width, height, this._outputData);
				return;
			}
			using (Bitmap bitmap = new Bitmap(width, height))
			{
				System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);
				int elementCount2 = texture.Width * texture.Height * 4;
				texture.GetData<byte>(this._outputData, 0, elementCount2);
				int num3 = 0;
				int num4 = 0;
				for (int k = 0; k < height; k++)
				{
					for (int l = 0; l < width; l++)
					{
						byte b = this._outputData[num3 + 2];
						this._outputData[num4 + 2] = this._outputData[num3];
						this._outputData[num4] = b;
						this._outputData[num4 + 1] = this._outputData[num3 + 1];
						this._outputData[num4 + 3] = this._outputData[num3 + 3];
						num3 += 4;
						num4 += 4;
					}
					num3 += texture.Width - width << 2;
				}
				BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
				IntPtr scan = bitmapData.Scan0;
				Marshal.Copy(this._outputData, 0, scan, width * height * 4);
				bitmap.UnlockBits(bitmapData);
				bitmap.Save(text2, imageFormat);
			}
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x0051FF28 File Offset: 0x0051E128
		private void FinishCapture()
		{
			if (this._activeSettings.UseScaling && this._outputData != null)
			{
				int num = 0;
				while (!this.SaveImage(this._outputImageSize.Width, this._outputImageSize.Height, ImageFormat.Png, string.Concat(new string[]
				{
					Main.SavePath,
					Path.DirectorySeparatorChar.ToString(),
					"Captures",
					Path.DirectorySeparatorChar.ToString(),
					this._activeSettings.OutputName,
					".png"
				})))
				{
					GC.Collect();
					Thread.Sleep(5);
					num++;
					Console.WriteLine(Language.GetTextValue("Error.CaptureError"));
					if (num > 5)
					{
						Console.WriteLine(Language.GetTextValue("Error.UnableToCapture"));
						break;
					}
				}
			}
			this._outputData = null;
			this._scaledFrameData = null;
			Main.GlobalTimerPaused = false;
			CaptureInterface.EndCamera();
			if (this._scaledFrameBuffer != null)
			{
				this._scaledFrameBuffer.Dispose();
				this._scaledFrameBuffer = null;
			}
			this._activeSettings = null;
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x00520038 File Offset: 0x0051E238
		public void Dispose()
		{
			if (!Main.dedServ)
			{
				Monitor.Enter(this._captureLock);
				if (this._isDisposed)
				{
					Monitor.Exit(this._captureLock);
					return;
				}
				this._frameBuffer.Dispose();
				this._filterFrameBuffer1.Dispose();
				this._filterFrameBuffer2.Dispose();
				this._waterTarget.Dispose();
				if (this._scaledFrameBuffer != null)
				{
					this._scaledFrameBuffer.Dispose();
					this._scaledFrameBuffer = null;
				}
				CaptureCamera.CameraExists = false;
				this._isDisposed = true;
				Monitor.Exit(this._captureLock);
			}
		}

		// Token: 0x04004A52 RID: 19026
		private static bool CameraExists;

		// Token: 0x04004A53 RID: 19027
		public const int CHUNK_SIZE = 128;

		// Token: 0x04004A54 RID: 19028
		public const int FRAMEBUFFER_PIXEL_SIZE = 2048;

		// Token: 0x04004A55 RID: 19029
		public const int INNER_CHUNK_SIZE = 126;

		// Token: 0x04004A56 RID: 19030
		public const int MAX_IMAGE_SIZE = 4096;

		// Token: 0x04004A57 RID: 19031
		public const string CAPTURE_DIRECTORY = "Captures";

		// Token: 0x04004A58 RID: 19032
		private RenderTarget2D _frameBuffer;

		// Token: 0x04004A59 RID: 19033
		private RenderTarget2D _scaledFrameBuffer;

		// Token: 0x04004A5A RID: 19034
		private RenderTarget2D _filterFrameBuffer1;

		// Token: 0x04004A5B RID: 19035
		private RenderTarget2D _filterFrameBuffer2;

		// Token: 0x04004A5C RID: 19036
		private WorldSceneLayerTarget _waterTarget;

		// Token: 0x04004A5D RID: 19037
		private GraphicsDevice _graphics;

		// Token: 0x04004A5E RID: 19038
		private readonly object _captureLock = new object();

		// Token: 0x04004A5F RID: 19039
		private bool _isDisposed;

		// Token: 0x04004A60 RID: 19040
		private CaptureSettings _activeSettings;

		// Token: 0x04004A61 RID: 19041
		private Queue<CaptureCamera.CaptureChunk> _renderQueue = new Queue<CaptureCamera.CaptureChunk>();

		// Token: 0x04004A62 RID: 19042
		private SpriteBatch _spriteBatch;

		// Token: 0x04004A63 RID: 19043
		private byte[] _scaledFrameData;

		// Token: 0x04004A64 RID: 19044
		private byte[] _outputData;

		// Token: 0x04004A65 RID: 19045
		private Size _outputImageSize;

		// Token: 0x04004A66 RID: 19046
		private SamplerState _downscaleSampleState;

		// Token: 0x04004A67 RID: 19047
		private float _tilesProcessed;

		// Token: 0x04004A68 RID: 19048
		private float _totalTiles;

		// Token: 0x0200079E RID: 1950
		private class CaptureChunk
		{
			// Token: 0x06004198 RID: 16792 RVA: 0x006BADAC File Offset: 0x006B8FAC
			public CaptureChunk(Microsoft.Xna.Framework.Rectangle area, Microsoft.Xna.Framework.Rectangle scaledArea)
			{
				this.Area = area;
				this.ScaledArea = scaledArea;
			}

			// Token: 0x04007040 RID: 28736
			public readonly Microsoft.Xna.Framework.Rectangle Area;

			// Token: 0x04007041 RID: 28737
			public readonly Microsoft.Xna.Framework.Rectangle ScaledArea;
		}
	}
}
