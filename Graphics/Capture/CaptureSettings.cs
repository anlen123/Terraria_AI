using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.Capture
{
	// Token: 0x020001DE RID: 478
	public class CaptureSettings
	{
		// Token: 0x0600200A RID: 8202 RVA: 0x00520208 File Offset: 0x0051E408
		public CaptureSettings()
		{
			DateTime dateTime = DateTime.Now.ToLocalTime();
			this.OutputName = string.Concat(new string[]
			{
				"Capture ",
				dateTime.Year.ToString("D4"),
				"-",
				dateTime.Month.ToString("D2"),
				"-",
				dateTime.Day.ToString("D2"),
				" ",
				dateTime.Hour.ToString("D2"),
				"_",
				dateTime.Minute.ToString("D2"),
				"_",
				dateTime.Second.ToString("D2")
			});
		}

		// Token: 0x04004A6C RID: 19052
		public Rectangle Area;

		// Token: 0x04004A6D RID: 19053
		public bool UseScaling = true;

		// Token: 0x04004A6E RID: 19054
		public string OutputName;

		// Token: 0x04004A6F RID: 19055
		public bool CaptureEntities = true;

		// Token: 0x04004A70 RID: 19056
		public CaptureBiome Biome = CaptureBiome.DefaultPurity;

		// Token: 0x04004A71 RID: 19057
		public bool CaptureMech;

		// Token: 0x04004A72 RID: 19058
		public bool CaptureBackground;

		// Token: 0x04004A73 RID: 19059
		public bool CameraSpaceEffects;
	}
}
