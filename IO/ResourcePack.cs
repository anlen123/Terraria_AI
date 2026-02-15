using System;
using System.IO;
using Ionic.Zip;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using ReLogic.Content.Sources;
using ReLogic.Utilities;
using Terraria.GameContent;

namespace Terraria.IO
{
	// Token: 0x0200006A RID: 106
	public class ResourcePack
	{
		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x004BAD05 File Offset: 0x004B8F05
		public Texture2D Icon
		{
			get
			{
				if (this._icon == null)
				{
					this._icon = this.CreateIcon();
				}
				return this._icon;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x004BAD21 File Offset: 0x004B8F21
		// (set) Token: 0x06001485 RID: 5253 RVA: 0x004BAD29 File Offset: 0x004B8F29
		public string Name { get; private set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x004BAD32 File Offset: 0x004B8F32
		// (set) Token: 0x06001487 RID: 5255 RVA: 0x004BAD3A File Offset: 0x004B8F3A
		public string Description { get; private set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x004BAD43 File Offset: 0x004B8F43
		// (set) Token: 0x06001489 RID: 5257 RVA: 0x004BAD4B File Offset: 0x004B8F4B
		public string Author { get; private set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x004BAD54 File Offset: 0x004B8F54
		// (set) Token: 0x0600148B RID: 5259 RVA: 0x004BAD5C File Offset: 0x004B8F5C
		public ResourcePackVersion Version { get; private set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x004BAD65 File Offset: 0x004B8F65
		// (set) Token: 0x0600148D RID: 5261 RVA: 0x004BAD6D File Offset: 0x004B8F6D
		public bool IsEnabled { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x004BAD76 File Offset: 0x004B8F76
		// (set) Token: 0x0600148F RID: 5263 RVA: 0x004BAD7E File Offset: 0x004B8F7E
		public int SortingOrder { get; set; }

		// Token: 0x06001490 RID: 5264 RVA: 0x004BAD88 File Offset: 0x004B8F88
		public ResourcePack(IServiceProvider services, string path, ResourcePack.BrandingType branding = ResourcePack.BrandingType.None)
		{
			if (File.Exists(path))
			{
				this.IsCompressed = true;
			}
			else if (!Directory.Exists(path))
			{
				throw new FileNotFoundException("Unable to find file or folder for resource pack at: " + path);
			}
			this.FileName = Path.GetFileName(path);
			this._services = services;
			this.FullPath = path;
			this.Branding = branding;
			if (this.IsCompressed)
			{
				this._zipFile = ZipFile.Read(path);
			}
			this.LoadManifest();
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x004BAE00 File Offset: 0x004B9000
		public IContentSource GetContentSource()
		{
			if (this._contentSource == null)
			{
				if (this.IsCompressed)
				{
					this._contentSource = new ZipContentSource(this.FullPath, "Content");
				}
				else
				{
					this._contentSource = new FileSystemContentSource(Path.Combine(this.FullPath, "Content"));
				}
				this._contentSource.ContentValidator = VanillaContentValidator.Instance;
			}
			return this._contentSource;
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x004BAE68 File Offset: 0x004B9068
		private Texture2D CreateIcon()
		{
			if (!this.HasFile("icon.png"))
			{
				return XnaExtensions.Get<IAssetRepository>(this._services).Request<Texture2D>("Images/UI/DefaultResourcePackIcon", 1).Value;
			}
			Texture2D result;
			using (Stream stream = this.OpenStream("icon.png"))
			{
				result = XnaExtensions.Get<AssetReaderCollection>(this._services).Read<Texture2D>(stream, ".png");
			}
			return result;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x004BAEE0 File Offset: 0x004B90E0
		private void LoadManifest()
		{
			if (!this.HasFile("pack.json"))
			{
				throw new FileNotFoundException(string.Format("Resource Pack at \"{0}\" must contain a {1} file.", this.FullPath, "pack.json"));
			}
			JObject jobject;
			using (Stream stream = this.OpenStream("pack.json"))
			{
				using (StreamReader streamReader = new StreamReader(stream))
				{
					jobject = JObject.Parse(streamReader.ReadToEnd());
				}
			}
			this.Name = Extensions.Value<string>(jobject["Name"]);
			this.Description = Extensions.Value<string>(jobject["Description"]);
			this.Author = Extensions.Value<string>(jobject["Author"]);
			this.Version = jobject["Version"].ToObject<ResourcePackVersion>();
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x004BAFC0 File Offset: 0x004B91C0
		private Stream OpenStream(string fileName)
		{
			if (!this.IsCompressed)
			{
				return File.OpenRead(Path.Combine(this.FullPath, fileName));
			}
			ZipEntry zipEntry = this._zipFile[fileName];
			MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize);
			zipEntry.Extract(memoryStream);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x004BB00F File Offset: 0x004B920F
		private bool HasFile(string fileName)
		{
			if (!this.IsCompressed)
			{
				return File.Exists(Path.Combine(this.FullPath, fileName));
			}
			return this._zipFile.ContainsEntry(fileName);
		}

		// Token: 0x04001064 RID: 4196
		public readonly string FullPath;

		// Token: 0x04001065 RID: 4197
		public readonly string FileName;

		// Token: 0x0400106C RID: 4204
		private readonly IServiceProvider _services;

		// Token: 0x0400106D RID: 4205
		public readonly bool IsCompressed;

		// Token: 0x0400106E RID: 4206
		public readonly ResourcePack.BrandingType Branding;

		// Token: 0x0400106F RID: 4207
		private readonly ZipFile _zipFile;

		// Token: 0x04001070 RID: 4208
		private Texture2D _icon;

		// Token: 0x04001071 RID: 4209
		private IContentSource _contentSource;

		// Token: 0x04001072 RID: 4210
		private const string ICON_FILE_NAME = "icon.png";

		// Token: 0x04001073 RID: 4211
		private const string PACK_FILE_NAME = "pack.json";

		// Token: 0x0200065E RID: 1630
		public enum BrandingType
		{
			// Token: 0x04006629 RID: 26153
			None,
			// Token: 0x0400662A RID: 26154
			SteamWorkshop
		}
	}
}
