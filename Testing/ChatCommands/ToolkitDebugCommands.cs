using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.Net.Sockets;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.Testing.ChatCommands
{
	// Token: 0x02000119 RID: 281
	public static class ToolkitDebugCommands
	{
		// Token: 0x06001B09 RID: 6921 RVA: 0x004F8DB1 File Offset: 0x004F6FB1
		[DebugCommand("hh", "Opens a list of all the debug commands", CommandRequirement.Client)]
		public static bool HelpCommand(DebugMessage message)
		{
			IngameFancyUI.OpenUIState(new UIDebugCommandsList());
			return true;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x004F8DBE File Offset: 0x004F6FBE
		[DebugCommand("memo", "Creates a shortcut command with a given name. Opens the file to write in. One command per line. Accepts arg substitions ({0}, {1}, etc)", CommandRequirement.Client, HelpText = "Usage: /memo <custom-command-name>")]
		public static bool MemoCommand(DebugMessage message)
		{
			if (string.IsNullOrWhiteSpace(message.Arguments) || message.Arguments.Contains(" "))
			{
				return false;
			}
			DebugCommandProcessor.OpenMemo(message.Arguments);
			return true;
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x004F8DF0 File Offset: 0x004F6FF0
		[DebugCommand("memonum", "Creates a memo for a numpad key (0-9). Shorthand for /memo numpad{i}", CommandRequirement.Client, HelpText = "Usage: /memonum <0-9>")]
		public static bool MemoNumCommand(DebugMessage message)
		{
			int num;
			if (!int.TryParse(message.Arguments, out num) || num < 0 || num > 9)
			{
				message.ReplyError("Invalid numpad key number");
				return false;
			}
			DebugCommandProcessor.OpenMemo("numpad" + num);
			return true;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x004F8E38 File Offset: 0x004F7038
		[DebugCommand("setserverping", "Sets a target ping for all players on the server. Clients will automatically adjust /latency to achieve it.", CommandRequirement.MultiplayerRPC, HelpText = "Usage: /setserverping <ms>")]
		public static bool SetServerPingCommand(DebugMessage message)
		{
			int num;
			if (!int.TryParse(message.Arguments, out num) || num < 0 || num > 10000)
			{
				return false;
			}
			DebugOptions.Shared_ServerPing = num;
			ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(string.Format("Target ping set {0}ms", num)), new Color(250, 250, 0), -1);
			NetMessage.SendData(94, -1, -1, NetworkText.FromLiteral("/setserverping"), 0, (float)DebugOptions.Shared_ServerPing, 0f, 0f, 0, 0, 0);
			return true;
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x004F8EBC File Offset: 0x004F70BC
		[DebugCommand("latency", "Adds latency to incoming and outgoing packets sent by this client.", CommandRequirement.MultiplayerClient, HelpText = "Usage: /latency <ms>")]
		public static bool LatencyCommand(DebugMessage message)
		{
			uint num = 0U;
			if (!uint.TryParse(message.Arguments, out num))
			{
				return false;
			}
			DebugNetworkStream.Latency = num;
			message.Reply(string.Format("Latency set to {0}ms", num));
			return true;
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x004F8EFC File Offset: 0x004F70FC
		[DebugCommand("setdrawwait", "Sets a fixed waiting period to occur during each engine draw call.", CommandRequirement.Client, HelpText = "Usage: /setdrawwait <delay in ms>")]
		public static bool SetDrawWaitCommand(DebugMessage message)
		{
			double num;
			if (!double.TryParse(message.Arguments, out num) || num < 0.0 || num > 100.0)
			{
				return false;
			}
			DebugOptions.DrawWaitInMs = num;
			message.Reply(string.Format("Draw wait time set to {0}ms", num));
			return true;
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x004F8F50 File Offset: 0x004F7150
		[DebugCommand("setupdatewait", "Sets a fixed waiting period to occur during each engine update call.", CommandRequirement.Client, HelpText = "Usage: /setupdatewait <delay in ms>")]
		public static bool SetUpdateWaitCommand(DebugMessage message)
		{
			double num;
			if (!double.TryParse(message.Arguments, out num) || num < 0.0 || num > 100.0)
			{
				return false;
			}
			DebugOptions.UpdateWaitInMs = num;
			message.Reply(string.Format("Update wait time set to {0}ms", num));
			return true;
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x004F8FA3 File Offset: 0x004F71A3
		[DebugCommand("toggleinactivewait", "Toggles main thread sleeping when window is unfocused (this setting is saved).", CommandRequirement.Client)]
		public static bool ToggleInactiveWait(DebugMessage message)
		{
			Main.ThrottleWhenInactive = !Main.ThrottleWhenInactive;
			Main.SaveSettings();
			message.Reply("Inactive CPU throttling " + (Main.ThrottleWhenInactive ? "enabled" : "disabled"));
			return true;
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x004F8FDC File Offset: 0x004F71DC
		[DebugCommand("quickload", "Automatically rejoin this world/server with this player whenever the game is launched. Executes /onquickload memo when joining the world. Will relaunch all local clients when used with  Host & Play", CommandRequirement.Client, HelpText = "/quickload  [stop]")]
		public static bool QuickLoadRejoinCommand(DebugMessage message)
		{
			string a = message.Arguments.ToLowerInvariant().Trim();
			if (a == "stop" || a == "disable" || a == "clear" || a == "cancel" || a == "exit")
			{
				QuickLoad.Clear();
				message.Reply("Quick Load configuration cleared.");
				return true;
			}
			QuickLoad.Set(new QuickLoad.JoinWorld().WithCurrentState());
			message.Reply("Quick Load configuration set. Hold shift while loading to clear it.");
			return true;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x004F9068 File Offset: 0x004F7268
		[DebugCommand("quickload-regen", "Automatically regenerate this world whenever the game is launched. Executes /onquickload memo when joining the world", CommandRequirement.SinglePlayer)]
		public static bool QuickLoadRegenCommand(DebugMessage message)
		{
			QuickLoad.Set(new QuickLoad.RegenWorld().WithCurrentState());
			message.Reply("Quick Load configuration set. Hold shift while loading to clear it.");
			return true;
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x004F9085 File Offset: 0x004F7285
		[DebugCommand("light", "Toggles the lighting system between active and fullbright.", CommandRequirement.Client)]
		public static bool LightCommand(DebugMessage message)
		{
			if (DebugOptions.devLightTilesCheat)
			{
				DebugOptions.devLightTilesCheat = false;
				message.Reply("Lighting enabled");
			}
			else
			{
				DebugOptions.devLightTilesCheat = true;
				message.Reply("Lighting disabled");
			}
			return true;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x004F90B3 File Offset: 0x004F72B3
		[DebugCommand("nolimits", "No border restrictions", CommandRequirement.Client)]
		public static bool NoLimitsCommand(DebugMessage message)
		{
			if (DebugOptions.noLimits)
			{
				DebugOptions.noLimits = false;
				message.Reply("No limits disabled");
			}
			else
			{
				DebugOptions.noLimits = true;
				message.Reply("No limits enabled");
			}
			return true;
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x004F90E1 File Offset: 0x004F72E1
		[DebugCommand("save", "Save the player (and the world if single player)", CommandRequirement.Client)]
		public static bool SaveCommand(DebugMessage message)
		{
			Player.SavePlayer(Main.ActivePlayerFileData, false);
			if (Main.netMode == 0)
			{
				WorldFile.SaveWorld();
				message.Reply("Player and world saved!");
			}
			else
			{
				message.Reply("Player saved!");
			}
			return true;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x004F9114 File Offset: 0x004F7314
		[DebugCommand("reload", "Reloads the last save", CommandRequirement.SinglePlayer)]
		public static bool ReloadCommand(DebugMessage message)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			WorldFile.LoadWorld();
			Main.sectionManager.SetAllSectionsLoaded();
			message.Reply(string.Format("Reloaded in {0}ms", stopwatch.ElapsedMilliseconds));
			return true;
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x004F9152 File Offset: 0x004F7352
		[DebugCommand("quit", "Exit world without saving.", CommandRequirement.Client)]
		public static bool QuitCommand(DebugMessage message)
		{
			WorldGen.JustQuit();
			return true;
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x004F915A File Offset: 0x004F735A
		[DebugCommand("reloadpacks", "Reloads resource packs.", CommandRequirement.Client)]
		public static bool ReloadPacksCommand(DebugMessage message)
		{
			Main.instance.ResetAllContentBasedRenderTargets();
			Main.AssetSourceController.Refresh();
			message.Reply("Resource Packs Reloaded.");
			return true;
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x004F917C File Offset: 0x004F737C
		[DebugCommand("frame", "Resets all frame data", CommandRequirement.Client)]
		public static bool FrameCommand(DebugMessage message)
		{
			Main.sectionManager.SetAllSectionsLoaded();
			message.Reply("World frame data cleared");
			return true;
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x004F9194 File Offset: 0x004F7394
		[DebugCommand("hash", "Prints out the hash of all saved (non-volatile) tile data", CommandRequirement.AnyAuthority)]
		public static bool HashCommand(DebugMessage message)
		{
			message.Reply(string.Format("Tile data hash: {0:X8}", WorldGenerator.HashWorld()));
			return true;
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x004F91B1 File Offset: 0x004F73B1
		[DebugCommand("snapshot", "Creates a snapshot of the current tile state for the world.", CommandRequirement.AnyAuthority)]
		public static bool SnapshotCommand(DebugMessage message)
		{
			TileSnapshot.Create(null);
			message.Reply("Tile Snapshot Created.");
			return true;
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x004F91C5 File Offset: 0x004F73C5
		[DebugCommand("snapclear", "Clears previously created snapshot.", CommandRequirement.AnyAuthority)]
		public static bool SnapshotClearCommand(DebugMessage message)
		{
			TileSnapshot.Clear();
			message.Reply("Tile Snapshot Cleared.");
			return true;
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x004F91D8 File Offset: 0x004F73D8
		[DebugCommand("snapsave", "Saves a snapshot in dev-snapshots.", CommandRequirement.AnyAuthority, HelpText = "Usage: /snapsave <name>")]
		public static bool SnapshotSaveCommand(DebugMessage message)
		{
			if (string.IsNullOrWhiteSpace(message.Arguments))
			{
				message.ReplyError("Snapshot name required");
				return true;
			}
			if (!TileSnapshot.IsCreated)
			{
				TileSnapshot.Create(null);
			}
			Directory.CreateDirectory(Path.Combine(Main.SavePath, "dev-snapshots"));
			TileSnapshot.Save(Path.Combine(Main.SavePath, "dev-snapshots", message.Arguments + ".gensnapshot"));
			message.Reply("Tile Snapshot Saved to dev-snapshots/" + message.Arguments + ".gensnapshot");
			return true;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x004F9264 File Offset: 0x004F7464
		[DebugCommand("snapload", "Loads a snapshot in dev-snapshots.", CommandRequirement.AnyAuthority, HelpText = "Usage: /snapsave <name>")]
		public static bool SnapshotLoadCommand(DebugMessage message)
		{
			if (string.IsNullOrWhiteSpace(message.Arguments))
			{
				message.ReplyError("Snapshot name required");
				return true;
			}
			if (!TileSnapshot.IsCreated)
			{
				TileSnapshot.Create(null);
			}
			string path = Path.Combine(Main.SavePath, "dev-snapshots", message.Arguments + ".gensnapshot");
			if (!File.Exists(path))
			{
				message.ReplyError("File not found: dev-snapshots/" + message.Arguments + ".gensnapshot");
				return true;
			}
			TileSnapshot.Load(path, null);
			message.Reply("Tile Snapshot Loaded. Use /swap or /restore to apply.");
			return true;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x004F92F0 File Offset: 0x004F74F0
		[DebugCommand("restore", "Restores the world's tiles to the previously created snapshot.", CommandRequirement.AnyAuthority)]
		public static bool RestoreCommand(DebugMessage message)
		{
			if (!TileSnapshot.IsCreated)
			{
				message.ReplyError("No snapshot to restore");
			}
			else if (!TileSnapshot.SizeMatches)
			{
				message.ReplyError("Tile snapshot does not match current world size");
			}
			else
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				TileSnapshot.Restore();
				message.Reply(string.Format("Tile snapshot restored in {0}ms", stopwatch.ElapsedMilliseconds));
			}
			return true;
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x004F934C File Offset: 0x004F754C
		[DebugCommand("swap", "Swaps the world's tiles with the previously created snapshot.", CommandRequirement.AnyAuthority)]
		public static bool SwapCommand(DebugMessage message)
		{
			if (!TileSnapshot.IsCreated)
			{
				message.ReplyError("No snapshot to restore");
			}
			else if (!TileSnapshot.SizeMatches)
			{
				message.ReplyError("Tile snapshot does not match current world size");
			}
			else
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				TileSnapshot.Swap();
				message.Reply(string.Format("Tile snapshot swapped in {0}ms", stopwatch.ElapsedMilliseconds));
			}
			return true;
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x004F93A8 File Offset: 0x004F75A8
		[DebugCommand("snapshotdiff", "Finds differences between the current map and saved snapshot. Use /next to skip through them.", CommandRequirement.SinglePlayer)]
		public static bool SnapshotDiffCommand(DebugMessage message)
		{
			if (!TileSnapshot.IsCreated)
			{
				message.ReplyError("No snapshot to compare");
			}
			else if (!TileSnapshot.SizeMatches)
			{
				message.ReplyError("Tile snapshot does not match current world size");
			}
			else
			{
				ToolkitDebugCommands.FindNextEnumerable = TileSnapshot.Compare();
				ToolkitDebugCommands.FindNext();
			}
			return true;
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001B22 RID: 6946 RVA: 0x004F93E2 File Offset: 0x004F75E2
		// (set) Token: 0x06001B23 RID: 6947 RVA: 0x004F93E9 File Offset: 0x004F75E9
		public static IEnumerable<Point> FindNextEnumerable
		{
			get
			{
				return ToolkitDebugCommands._findNextEnumerable;
			}
			set
			{
				ToolkitDebugCommands._findNextEnumerable = value;
				ToolkitDebugCommands.FindNextEnumerator = null;
			}
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x004F93F8 File Offset: 0x004F75F8
		[DebugCommand("find", "Iterates through all instances of a tile in the world. Use /next to skip through them", CommandRequirement.Client, HelpText = "Usage: /find <id>")]
		public static bool FindCommand(DebugMessage message)
		{
			int tileType;
			if (!int.TryParse(message.Arguments, out tileType))
			{
				return false;
			}
			if (tileType < 0 || tileType >= (int)TileID.Count)
			{
				return false;
			}
			string text = string.Empty;
			if (MapHelper.HasOption(tileType, 0))
			{
				text = Lang.GetMapObjectName(MapHelper.TileToLookup(tileType, 0));
			}
			if (text == string.Empty)
			{
				text = "#" + tileType;
			}
			ToolkitDebugCommands.FindNextEnumerable = ToolkitDebugCommands.FindTiles(message, (Tile t) => (int)t.type == tileType, "Tile " + text, 3);
			ToolkitDebugCommands.FindNext();
			return true;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x004F94AC File Offset: 0x004F76AC
		[DebugCommand("findwall", "Iterates through all instances of a wall in the world. Use /next to skip through them", CommandRequirement.Client, HelpText = "Usage: /findwall <id>")]
		public static bool FindWallCommand(DebugMessage message)
		{
			int wallType;
			if (!int.TryParse(message.Arguments, out wallType))
			{
				return false;
			}
			if (wallType <= 0 || wallType >= (int)WallID.Count)
			{
				return false;
			}
			ToolkitDebugCommands.FindNextEnumerable = ToolkitDebugCommands.FindTiles(message, (Tile t) => (int)t.wall == wallType, "Wall #" + wallType, 10);
			ToolkitDebugCommands.FindNext();
			return true;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x004F9520 File Offset: 0x004F7720
		[DebugCommand("next", "Finds the next instance of a tile/wall/object. Requires calling /find, /findwall or /snapshotdiff first", CommandRequirement.Client)]
		public static bool NextCommand(DebugMessage message)
		{
			if (ToolkitDebugCommands.FindNextEnumerable == null)
			{
				message.ReplyError("Scan not started. Nothing to find.");
				return true;
			}
			ToolkitDebugCommands.FindNext();
			return true;
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x004F953C File Offset: 0x004F773C
		public static void FindNext()
		{
			if (ToolkitDebugCommands.FindNextEnumerator == null)
			{
				ToolkitDebugCommands.FindNextEnumerator = ToolkitDebugCommands.FindNextEnumerable.GetEnumerator();
			}
			if (ToolkitDebugCommands.FindNextEnumerator.MoveNext())
			{
				ToolkitDebugCommands.GoToTile(ToolkitDebugCommands.FindNextEnumerator.Current);
				return;
			}
			ToolkitDebugCommands.FindNextEnumerator = null;
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x004F9576 File Offset: 0x004F7776
		private static IEnumerable<Point> FindTiles(DebugMessage message, Func<Tile, bool> predicate, string descriptor, int skipDistance)
		{
			Point lastPoint = Point.Zero;
			int num;
			for (int x = 0; x < Main.maxTilesX; x = num + 1)
			{
				for (int y = 0; y < Main.maxTilesY; y = num + 1)
				{
					Tile tile = Main.tile[x, y];
					if (tile != null && predicate(tile) && (x - lastPoint.X >= skipDistance || Math.Abs(y - lastPoint.Y) >= skipDistance))
					{
						lastPoint = new Point(x, y);
						message.Reply(descriptor + " found at " + lastPoint);
						yield return lastPoint;
					}
					num = y;
				}
				num = x;
			}
			message.Reply(descriptor + " scan complete.");
			yield break;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x004F959B File Offset: 0x004F779B
		private static void GoToTile(Point coordinates)
		{
			Main.mapFullscreenPos = coordinates.ToVector2() + new Vector2(0.5f, 0.5f);
			Main.Pings.Add(Main.mapFullscreenPos);
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x004F95CB File Offset: 0x004F77CB
		[DebugCommand("showsections", "Toggles net section overlay.", CommandRequirement.Client)]
		public static bool ShowNetSectionsCommand(DebugMessage message)
		{
			DebugOptions.ShowSections = !DebugOptions.ShowSections;
			return true;
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x004F95DB File Offset: 0x004F77DB
		[DebugCommand("nopause", "Makes the game not pause when focus is lost", CommandRequirement.SinglePlayer)]
		public static bool NoPause(DebugMessage message)
		{
			DebugOptions.noPause = !DebugOptions.noPause;
			if (DebugOptions.noPause)
			{
				message.Reply("Pause on focus loss disabled");
			}
			else
			{
				message.Reply("Pause on focus loss enabled");
			}
			return true;
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x004F960C File Offset: 0x004F780C
		[DebugCommand("map", "Reveals the full map for the world.", CommandRequirement.Client, HelpText = "Usage: /map [pretty]")]
		public static bool MapCommand(DebugMessage message)
		{
			Main.clearMap = true;
			if (DebugOptions.unlockMap == 0)
			{
				DebugOptions.unlockMap = ((message.Arguments.ToLower().Trim() == "pretty") ? 2 : 1);
				Main.refreshMap = true;
			}
			else
			{
				DebugOptions.unlockMap = 0;
			}
			return true;
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x004F965A File Offset: 0x004F785A
		[DebugCommand("clearmap", "Deletes the full map for the world.", CommandRequirement.Client)]
		public static bool ClearMapCommand(DebugMessage message)
		{
			Main.clearMap = true;
			Main.Map.Clear();
			Main.refreshMap = true;
			return true;
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x004F9674 File Offset: 0x004F7874
		[DebugCommand("hideall", "Stops tiles, walls, and water from drawing", CommandRequirement.Client)]
		public static bool HideAll(DebugMessage message)
		{
			int num = 0;
			bool flag = false;
			if (!DebugOptions.hideTiles)
			{
				num++;
			}
			if (!DebugOptions.hideTiles2)
			{
				num++;
			}
			if (!DebugOptions.hideWalls)
			{
				num++;
			}
			if (!DebugOptions.hideWater)
			{
				num++;
			}
			if (num >= 2)
			{
				flag = true;
			}
			DebugOptions.hideTiles = flag;
			DebugOptions.hideTiles2 = flag;
			DebugOptions.hideWalls = flag;
			DebugOptions.hideWater = flag;
			if (flag)
			{
				message.Reply("Everything is hidden");
			}
			else
			{
				message.Reply("Everything is shown");
			}
			return true;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x004F96EB File Offset: 0x004F78EB
		[DebugCommand("hidetiles", "Stops tiles from drawing on the screen", CommandRequirement.Client)]
		public static bool HideTiles(DebugMessage message)
		{
			DebugOptions.hideTiles = !DebugOptions.hideTiles;
			if (DebugOptions.hideTiles)
			{
				message.Reply("Tiles are hidden");
			}
			else
			{
				message.Reply("Tiles are shown");
			}
			return true;
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x004F971A File Offset: 0x004F791A
		[DebugCommand("hidetiles2", "Stops non-solid tiles from drawing on the screen", CommandRequirement.Client)]
		public static bool HideTiles2(DebugMessage message)
		{
			DebugOptions.hideTiles2 = !DebugOptions.hideTiles2;
			if (DebugOptions.hideTiles2)
			{
				message.Reply("Secondary tiles are hidden");
			}
			else
			{
				message.Reply("Secondary tiles are shown");
			}
			return true;
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x004F9749 File Offset: 0x004F7949
		[DebugCommand("hidewalls", "Stops walls from drawing on the screen", CommandRequirement.Client)]
		public static bool HideWalls(DebugMessage message)
		{
			DebugOptions.hideWalls = !DebugOptions.hideWalls;
			if (DebugOptions.hideWalls)
			{
				message.Reply("Walls are hidden");
			}
			else
			{
				message.Reply("Walls are shown");
			}
			return true;
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x004F9778 File Offset: 0x004F7978
		[DebugCommand("hidewater", "Stops water from drawing on the screen", CommandRequirement.Client)]
		public static bool HideWater(DebugMessage message)
		{
			DebugOptions.hideWater = !DebugOptions.hideWater;
			if (DebugOptions.hideWater)
			{
				message.Reply("Water is hidden");
			}
			else
			{
				message.Reply("Water is shown");
			}
			return true;
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x004F97A7 File Offset: 0x004F79A7
		[DebugCommand("showunbreakablewalls", "Forces unbreakable walls to be visible even when covered by tiles", CommandRequirement.Client)]
		public static bool ShowUnbreakableWall(DebugMessage message)
		{
			DebugOptions.ShowUnbreakableWall = !DebugOptions.ShowUnbreakableWall;
			if (DebugOptions.ShowUnbreakableWall)
			{
				message.Reply("Unbreakable walls are shown");
			}
			else
			{
				message.Reply("Unbreakable walls are hidden");
			}
			return true;
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x004F97D6 File Offset: 0x004F79D6
		[DebugCommand("showlinks", "Draws gamepad link points as an interface overlay", CommandRequirement.Client)]
		public static bool DrawLinkPoints(DebugMessage message)
		{
			DebugOptions.DrawLinkPoints = !DebugOptions.DrawLinkPoints;
			message.Reply("Gamepad link points are " + (DebugOptions.DrawLinkPoints ? "shown" : "hidden"));
			return true;
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x004F9809 File Offset: 0x004F7A09
		[DebugCommand("shownetoffset", "Draws dust for debugging netOffset", CommandRequirement.Client)]
		public static bool ShowNetOffset(DebugMessage message)
		{
			DebugOptions.ShowNetOffsetDust = !DebugOptions.ShowNetOffsetDust;
			message.Reply("netOffset dust " + (DebugOptions.ShowNetOffsetDust ? "shown" : "hidden"));
			return true;
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x004F983C File Offset: 0x004F7A3C
		[DebugCommand("fakenetoffset", "Sets the netOffset for all entities to a given value (in pixels).", CommandRequirement.Client, HelpText = "Usage: /fakenetoffset <dx> <dy>")]
		public static bool FakeNetOffset(DebugMessage message)
		{
			string[] array = message.Arguments.Split(new char[]
			{
				' ',
				','
			}, StringSplitOptions.RemoveEmptyEntries);
			float num;
			float num2;
			if (array.Length < 2 || !float.TryParse(array[0], out num) || !float.TryParse(array[1], out num2))
			{
				return false;
			}
			DebugOptions.FakeNetOffset = new Vector2(num, num2);
			message.Reply(string.Concat(new object[]
			{
				"netOffset set to (",
				num,
				", ",
				num2,
				")"
			}));
			return true;
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x004F98CE File Offset: 0x004F7ACE
		[DebugCommand("nodamagevar", "Removes damage variation (the inherent +/- 15% from damage). Useful for gathering specific data on true damage.", CommandRequirement.Client)]
		public static bool NoDamageVarCommand(DebugMessage message)
		{
			DebugOptions.NoDamageVar = !DebugOptions.NoDamageVar;
			message.Reply("No Damage Vars: " + (DebugOptions.NoDamageVar ? "On" : "Off"));
			return true;
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x004F9901 File Offset: 0x004F7B01
		[DebugCommand("hurtdummies", "Allows projectiles to aim at target dummies.", CommandRequirement.Client)]
		public static bool HurtDummiesCommand(DebugMessage message)
		{
			DebugOptions.LetProjectilesAimAtTargetDummies = !DebugOptions.LetProjectilesAimAtTargetDummies;
			message.Reply("Aim At Dummies: " + (DebugOptions.LetProjectilesAimAtTargetDummies ? "Enabled" : "Disabled"));
			return true;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x004F9934 File Offset: 0x004F7B34
		[DebugCommand("practice", "Toggles practice mode, which resets boss fights when you would take lethal damage.", CommandRequirement.SinglePlayer)]
		public static bool PracticeCommand(DebugMessage message)
		{
			DebugOptions.PracticeMode = !DebugOptions.PracticeMode;
			message.Reply("Practice Mode " + (DebugOptions.PracticeMode ? "enabled" : "disabled"));
			return true;
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x004F9968 File Offset: 0x004F7B68
		[DebugCommand("showdebug", "Toggles command reporting.", CommandRequirement.MultiplayerRPC | CommandRequirement.LocalServer)]
		public static bool ShowDebugCommand(DebugMessage message)
		{
			if (message.Author != 255 && !Main.player[(int)message.Author].host)
			{
				message.ReplyError("/showdebug can only be toggled by the host or server console.");
				return true;
			}
			if (DebugOptions.Shared_ReportCommandUsage)
			{
				DebugOptions.Shared_ReportCommandUsage = false;
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Command reporting disabled"), new Color(250, 250, 0), -1);
			}
			else
			{
				DebugOptions.Shared_ReportCommandUsage = true;
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Command reporting enabled"), new Color(250, 250, 0), -1);
			}
			NetMessage.SendData(94, -1, -1, NetworkText.FromLiteral("/showdebug"), 0, (float)(DebugOptions.Shared_ReportCommandUsage ? 1 : 0), 0f, 0f, 0, 0, 0);
			return true;
		}

		// Token: 0x04001541 RID: 5441
		private static IEnumerable<Point> _findNextEnumerable;

		// Token: 0x04001542 RID: 5442
		private static IEnumerator<Point> FindNextEnumerator;
	}
}
