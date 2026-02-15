using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent
{
	// Token: 0x02000270 RID: 624
	public class VanillaContentValidator : IContentValidator
	{
		// Token: 0x0600240A RID: 9226 RVA: 0x005499FC File Offset: 0x00547BFC
		public VanillaContentValidator(string infoFilePath)
		{
			foreach (string text in Regex.Split(Utils.ReadEmbeddedResource(infoFilePath), "\r\n|\r|\n"))
			{
				if (!text.StartsWith("//"))
				{
					string[] array2 = text.Split(new char[]
					{
						'\t'
					});
					int width;
					int height;
					if (array2.Length >= 3 && int.TryParse(array2[1], out width) && int.TryParse(array2[2], out height))
					{
						string key = array2[0].Replace('/', '\\');
						this._info[key] = new VanillaContentValidator.TextureMetaData
						{
							Width = width,
							Height = height
						};
					}
				}
			}
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x00549AB4 File Offset: 0x00547CB4
		public bool AssetIsValid<T>(T content, string contentPath, out IRejectionReason rejectReason) where T : class
		{
			Texture2D texture2D = content as Texture2D;
			rejectReason = null;
			VanillaContentValidator.TextureMetaData textureMetaData;
			return texture2D == null || !this._info.TryGetValue(contentPath, out textureMetaData) || textureMetaData.Matches(texture2D, out rejectReason);
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x00549AF0 File Offset: 0x00547CF0
		public HashSet<string> GetValidImageFilePaths()
		{
			return new HashSet<string>(from x in this._info
			select x.Key);
		}

		// Token: 0x04004DAD RID: 19885
		public static VanillaContentValidator Instance;

		// Token: 0x04004DAE RID: 19886
		private Dictionary<string, VanillaContentValidator.TextureMetaData> _info = new Dictionary<string, VanillaContentValidator.TextureMetaData>();

		// Token: 0x020007F1 RID: 2033
		private struct TextureMetaData
		{
			// Token: 0x06004286 RID: 17030 RVA: 0x006BDA58 File Offset: 0x006BBC58
			public bool Matches(Texture2D texture, out IRejectionReason rejectReason)
			{
				if (texture.Width != this.Width || texture.Height != this.Height)
				{
					rejectReason = new ContentRejectionFromSize(this.Width, this.Height, texture.Width, texture.Height);
					return false;
				}
				rejectReason = null;
				return true;
			}

			// Token: 0x0400713A RID: 28986
			public int Width;

			// Token: 0x0400713B RID: 28987
			public int Height;
		}
	}
}
