using System;
using Terraria.Utilities;

namespace Terraria.IO
{
	// Token: 0x0200006E RID: 110
	public abstract class FileData
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x004BB9B8 File Offset: 0x004B9BB8
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060014B8 RID: 5304 RVA: 0x004BB9C0 File Offset: 0x004B9BC0
		public bool IsCloudSave
		{
			get
			{
				return this._isCloudSave;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x004BB9C8 File Offset: 0x004B9BC8
		public bool IsFavorite
		{
			get
			{
				return this._isFavorite;
			}
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x004BB9D0 File Offset: 0x004B9BD0
		protected FileData(string type)
		{
			this.Type = type;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x004BB9DF File Offset: 0x004B9BDF
		protected FileData(string type, string path, bool isCloud)
		{
			this.Type = type;
			this._path = path;
			this._isCloudSave = isCloud;
			this._isFavorite = (isCloud ? Main.CloudFavoritesData : Main.LocalFavoriteData).IsFavorite(this);
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x004BBA17 File Offset: 0x004B9C17
		public void ToggleFavorite()
		{
			this.SetFavorite(!this.IsFavorite, true);
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x004BBA29 File Offset: 0x004B9C29
		public string GetFileName(bool includeExtension = true)
		{
			return FileUtilities.GetFileName(this.Path, includeExtension);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x004BBA37 File Offset: 0x004B9C37
		public void SetFavorite(bool favorite, bool saveChanges = true)
		{
			this._isFavorite = favorite;
			if (saveChanges)
			{
				(this.IsCloudSave ? Main.CloudFavoritesData : Main.LocalFavoriteData).SaveFavorite(this);
			}
		}

		// Token: 0x060014BF RID: 5311
		public abstract void SetAsActive();

		// Token: 0x060014C0 RID: 5312
		public abstract void MoveToCloud();

		// Token: 0x060014C1 RID: 5313
		public abstract void MoveToLocal();

		// Token: 0x0400107B RID: 4219
		protected string _path;

		// Token: 0x0400107C RID: 4220
		protected bool _isCloudSave;

		// Token: 0x0400107D RID: 4221
		public FileMetadata Metadata;

		// Token: 0x0400107E RID: 4222
		public string Name;

		// Token: 0x0400107F RID: 4223
		public readonly string Type;

		// Token: 0x04001080 RID: 4224
		protected bool _isFavorite;
	}
}
