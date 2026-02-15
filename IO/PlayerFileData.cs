using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.IO
{
	// Token: 0x0200006F RID: 111
	public class PlayerFileData : FileData
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x004BBA5D File Offset: 0x004B9C5D
		// (set) Token: 0x060014C3 RID: 5315 RVA: 0x004BBA65 File Offset: 0x004B9C65
		public Player Player
		{
			get
			{
				return this._player;
			}
			set
			{
				this._player = value;
				if (value != null)
				{
					this.Name = this._player.name;
				}
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x004BBA82 File Offset: 0x004B9C82
		// (set) Token: 0x060014C5 RID: 5317 RVA: 0x004BBA8A File Offset: 0x004B9C8A
		public bool ServerSideCharacter { get; private set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060014C6 RID: 5318 RVA: 0x004BBA93 File Offset: 0x004B9C93
		public DateTime LastPlayed
		{
			get
			{
				return DateTime.FromBinary(this.Player.lastTimePlayerWasSaved);
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x004BBAA5 File Offset: 0x004B9CA5
		public PlayerFileData() : base("Player")
		{
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x004BBAC8 File Offset: 0x004B9CC8
		public PlayerFileData(string path, bool cloudSave) : base("Player", path, cloudSave)
		{
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x004BBAF0 File Offset: 0x004B9CF0
		public static PlayerFileData CreateAndSave(Player player)
		{
			PlayerFileData playerFileData = new PlayerFileData();
			playerFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.Player);
			playerFileData.Player = player;
			playerFileData._isCloudSave = (SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault);
			playerFileData._path = Main.GetPlayerPathFromName(player.name, playerFileData.IsCloudSave);
			(playerFileData.IsCloudSave ? Main.CloudFavoritesData : Main.LocalFavoriteData).ClearEntry(playerFileData);
			Player.SavePlayer(playerFileData, true);
			return playerFileData;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x004BBB69 File Offset: 0x004B9D69
		public override void SetAsActive()
		{
			Main.ActivePlayerFileData = this;
			Main.player[Main.myPlayer] = this.Player;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x004BBB82 File Offset: 0x004B9D82
		public void MarkAsServerSide()
		{
			this.ServerSideCharacter = true;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x004BBB8C File Offset: 0x004B9D8C
		public override void MoveToCloud()
		{
			if (base.IsCloudSave || SocialAPI.Cloud == null)
			{
				return;
			}
			string playerPathFromName = Main.GetPlayerPathFromName(this.Name, true);
			if (FileUtilities.MoveToCloud(base.Path, playerPathFromName))
			{
				string fileName = base.GetFileName(false);
				string path = Main.PlayerPath + System.IO.Path.DirectorySeparatorChar.ToString() + fileName + System.IO.Path.DirectorySeparatorChar.ToString();
				string str = playerPathFromName.Substring(0, playerPathFromName.Length - 4);
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path);
					for (int i = 0; i < files.Length; i++)
					{
						string cloudPath = str + "/" + FileUtilities.GetFileName(files[i], true);
						FileUtilities.MoveToCloud(files[i], cloudPath);
					}
				}
				Main.LocalFavoriteData.ClearEntry(this);
				this._isCloudSave = true;
				this._path = playerPathFromName;
				Main.CloudFavoritesData.SaveFavorite(this);
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x004BBC74 File Offset: 0x004B9E74
		public override void MoveToLocal()
		{
			if (!base.IsCloudSave || SocialAPI.Cloud == null)
			{
				return;
			}
			string playerPathFromName = Main.GetPlayerPathFromName(this.Name, false);
			if (FileUtilities.MoveToLocal(base.Path, playerPathFromName))
			{
				string fileName = base.GetFileName(false);
				string mapPath = System.IO.Path.Combine(Main.CloudPlayerPath, fileName);
				foreach (string text in (from path in SocialAPI.Cloud.GetFiles().ToList<string>()
				where this.MapBelongsToPath(mapPath, path)
				select path).ToList<string>())
				{
					string localPath = System.IO.Path.Combine(Main.PlayerPath, fileName, FileUtilities.GetFileName(text, true));
					FileUtilities.MoveToLocal(text, localPath);
				}
				Main.CloudFavoritesData.ClearEntry(this);
				this._isCloudSave = false;
				this._path = playerPathFromName;
				Main.LocalFavoriteData.SaveFavorite(this);
			}
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x004BBD78 File Offset: 0x004B9F78
		private bool MapBelongsToPath(string mapPath, string filePath)
		{
			if (!filePath.EndsWith(".map", StringComparison.CurrentCultureIgnoreCase))
			{
				return false;
			}
			string value = mapPath.Replace('\\', '/');
			return filePath.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x004BBDA8 File Offset: 0x004B9FA8
		public void UpdatePlayTimer()
		{
			if (FocusHelper.AllowCountingPlayerTime)
			{
				this.StartPlayTimer();
				return;
			}
			this.PausePlayTimer();
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x004BBDBE File Offset: 0x004B9FBE
		public void StartPlayTimer()
		{
			if (this._isTimerActive)
			{
				return;
			}
			this._isTimerActive = true;
			if (!this._timer.IsRunning)
			{
				this._timer.Start();
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x004BBDE8 File Offset: 0x004B9FE8
		public void PausePlayTimer()
		{
			this.StopPlayTimer();
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x004BBDF0 File Offset: 0x004B9FF0
		public TimeSpan GetPlayTime()
		{
			if (this._timer.IsRunning)
			{
				return this._playTime + this._timer.Elapsed;
			}
			return this._playTime;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x004BBE1C File Offset: 0x004BA01C
		public void UpdatePlayTimerAndKeepState()
		{
			bool isRunning = this._timer.IsRunning;
			this._playTime += this._timer.Elapsed;
			this._timer.Reset();
			if (isRunning)
			{
				this._timer.Start();
			}
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x004BBE68 File Offset: 0x004BA068
		public void StopPlayTimer()
		{
			if (!this._isTimerActive)
			{
				return;
			}
			this._isTimerActive = false;
			if (this._timer.IsRunning)
			{
				this._playTime += this._timer.Elapsed;
				this._timer.Reset();
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x004BBEB9 File Offset: 0x004BA0B9
		public void SetPlayTime(TimeSpan time)
		{
			this._playTime = time;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x004BBEC2 File Offset: 0x004BA0C2
		public void Rename(string newName)
		{
			if (this.Player != null)
			{
				this.Player.name = newName.Trim();
			}
			Player.SavePlayer(this, false);
		}

		// Token: 0x04001081 RID: 4225
		private Player _player;

		// Token: 0x04001082 RID: 4226
		private TimeSpan _playTime = TimeSpan.Zero;

		// Token: 0x04001083 RID: 4227
		private readonly Stopwatch _timer = new Stopwatch();

		// Token: 0x04001084 RID: 4228
		private bool _isTimerActive;
	}
}
