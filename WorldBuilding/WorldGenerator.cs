using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ReLogic.Threading;
using Terraria.GameContent.UI.States;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000BD RID: 189
	public class WorldGenerator
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x004DEA54 File Offset: 0x004DCC54
		public static List<GenPassResult> PassResults
		{
			get
			{
				return WorldGen.Manifest.GenPassResults;
			}
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x004DEA60 File Offset: 0x004DCC60
		public WorldGenerator(int seed, WorldGenConfiguration configuration, GenerationProgress progress = null, WorldGenerator.Controller controller = null)
		{
			this._seed = seed;
			this._configuration = configuration;
			this._progress = ((progress == null) ? new GenerationProgress() : progress);
			this._controller = ((controller == null) ? new WorldGenerator.Controller(null) : controller);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x004DEABC File Offset: 0x004DCCBC
		public void Append(GenPass pass)
		{
			this._passes.Add(pass);
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x004DEACC File Offset: 0x004DCCCC
		public bool GenerateWorld()
		{
			WorldGenerator._hashTime.Reset();
			this._controller.SetGenerator(this);
			WorldGenerator.CurrentController = this._controller;
			this._progress.TotalWeight = (from p in this._passes
			where p.Enabled
			select p).Sum((GenPass p) => p.Weight);
			WorldGenerator.CurrentGenerationProgress = this._progress;
			if (this._controller.PauseAfterPass != null)
			{
				WorldGenerator.SetDebugWorldGenUIVisibility(true);
			}
			bool flag = false;
			while (!this._controller.QueuedAbort)
			{
				if (!this._controller.Paused)
				{
					object controlLock = this._controlLock;
					lock (controlLock)
					{
						if (WorldGenerator.PassResults.Count != this._passes.Count)
						{
							this._currentPass = this._passes[WorldGenerator.PassResults.Count];
							GenPass currentPass = this._currentPass;
							lock (currentPass)
							{
								WorldGenerator.PassResults.Add(this.RunPass(this._currentPass));
								this._controller.OnPassCompleted();
							}
							this._currentPass = null;
							continue;
						}
					}
					IL_163:
					string str = string.Join<GenPassResult>("\n", WorldGenerator.PassResults);
					string format = "\nFinished world - Seed: {0} Width: {1}, Height: {2}, Evil: {3}, Difficulty: {4}\nTotal Generation Time: {5}\n";
					object[] array = new object[6];
					array[0] = Main.ActiveWorldFileData.SeedText;
					array[1] = Main.maxTilesX;
					array[2] = Main.maxTilesY;
					array[3] = WorldGen.WorldGenParam_Evil;
					array[4] = Main.GameMode;
					array[5] = WorldGenerator.PassResults.Sum((GenPassResult r) => r.DurationMs);
					Trace.WriteLine(str + string.Format(format, array));
					WorldGenerator.SetDebugWorldGenUIVisibility(false);
					WorldGenerator.CurrentGenerationProgress = null;
					WorldGenerator.CurrentController = null;
					return !flag;
				}
				this._controller.OnPaused();
			}
			flag = true;
			goto IL_163;
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x004DED0C File Offset: 0x004DCF0C
		private static void SetDebugWorldGenUIVisibility(bool visible)
		{
			bool flag = UIWorldGenDebug.ActiveInstance != null;
			if (visible == flag)
			{
				return;
			}
			Main.RunOnMainThread(delegate()
			{
				if (visible)
				{
					UIWorldGenDebug.Open();
					return;
				}
				UIWorldGenDebug.Close();
			}).Wait();
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x004DED50 File Offset: 0x004DCF50
		private GenPassResult RunPass(GenPass pass)
		{
			if (!pass.Enabled)
			{
				return new GenPassResult
				{
					Name = pass.Name,
					Skipped = true
				};
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			Main.rand = new UnifiedRandom(this._seed);
			this._progress.Start(pass.Weight);
			try
			{
				pass.Apply(this._progress, this._configuration.GetPassConfiguration(pass.Name));
			}
			catch (Exception ex)
			{
				this._controller.ReportException("Exception in Pass: " + pass.Name, ex);
			}
			this._progress.End();
			return new GenPassResult
			{
				Name = pass.Name,
				DurationMs = (int)stopwatch.ElapsedMilliseconds,
				RandNext = WorldGen.genRand.Next()
			};
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x004DEE2C File Offset: 0x004DD02C
		public static uint HashWorld()
		{
			WorldGenerator._hashTime.Start();
			uint[] line_hashes = new uint[Main.maxTilesX];
			FastParallel.For(0, Main.maxTilesX, delegate(int x0, int x1, object _)
			{
				Tile[,] tile = Main.tile;
				int maxTilesY = Main.maxTilesY;
				for (int j = x0; j < x1; j++)
				{
					uint num3 = 0U;
					for (int k = 0; k < maxTilesY; k++)
					{
						num3 ^= (uint)TileSnapshot.TileStruct.From(tile[j, k]).GetHashCode();
						num3 = (num3 << 13 | num3 >> 19);
						num3 = num3 * 5U + 3864292196U;
					}
					line_hashes[j] = num3;
				}
			}, null);
			uint num = 0U;
			foreach (uint num2 in line_hashes)
			{
				num ^= num2;
				num = (num << 13 | num >> 19);
				num = num * 5U + 3864292196U;
			}
			WorldGenerator._hashTime.Stop();
			return num;
		}

		// Token: 0x04001270 RID: 4720
		internal readonly List<GenPass> _passes = new List<GenPass>();

		// Token: 0x04001271 RID: 4721
		private readonly int _seed;

		// Token: 0x04001272 RID: 4722
		private readonly WorldGenConfiguration _configuration;

		// Token: 0x04001273 RID: 4723
		private readonly GenerationProgress _progress;

		// Token: 0x04001274 RID: 4724
		private readonly WorldGenerator.Controller _controller;

		// Token: 0x04001275 RID: 4725
		private readonly object _controlLock = new object();

		// Token: 0x04001276 RID: 4726
		private GenPass _currentPass;

		// Token: 0x04001277 RID: 4727
		public static GenerationProgress CurrentGenerationProgress;

		// Token: 0x04001278 RID: 4728
		public static WorldGenerator.Controller CurrentController;

		// Token: 0x04001279 RID: 4729
		private static Stopwatch _hashTime = new Stopwatch();

		// Token: 0x020006E6 RID: 1766
		public enum SnapshotFrequency
		{
			// Token: 0x040067AE RID: 26542
			None = -1,
			// Token: 0x040067AF RID: 26543
			Manual,
			// Token: 0x040067B0 RID: 26544
			Automatic,
			// Token: 0x040067B1 RID: 26545
			Always
		}

		// Token: 0x020006E7 RID: 1767
		public class Controller
		{
			// Token: 0x170004EA RID: 1258
			// (get) Token: 0x06003F46 RID: 16198 RVA: 0x00698D25 File Offset: 0x00696F25
			public List<GenPass> Passes
			{
				get
				{
					return this._generator._passes;
				}
			}

			// Token: 0x170004EB RID: 1259
			// (get) Token: 0x06003F47 RID: 16199 RVA: 0x00698D32 File Offset: 0x00696F32
			public GenPass CurrentPass
			{
				get
				{
					return this._generator._currentPass;
				}
			}

			// Token: 0x170004EC RID: 1260
			// (get) Token: 0x06003F48 RID: 16200 RVA: 0x00698D3F File Offset: 0x00696F3F
			public GenPass LastCompletedPass
			{
				get
				{
					if (WorldGenerator.PassResults.Count != 0)
					{
						return this.Passes[WorldGenerator.PassResults.Count - 1];
					}
					return null;
				}
			}

			// Token: 0x170004ED RID: 1261
			// (get) Token: 0x06003F49 RID: 16201 RVA: 0x00698D66 File Offset: 0x00696F66
			// (set) Token: 0x06003F4A RID: 16202 RVA: 0x00698D6E File Offset: 0x00696F6E
			public GenPass PauseAfterPass { get; set; }

			// Token: 0x170004EE RID: 1262
			// (get) Token: 0x06003F4B RID: 16203 RVA: 0x00698D77 File Offset: 0x00696F77
			// (set) Token: 0x06003F4C RID: 16204 RVA: 0x00698D7F File Offset: 0x00696F7F
			public bool PauseOnHashMismatch { get; set; }

			// Token: 0x170004EF RID: 1263
			// (get) Token: 0x06003F4D RID: 16205 RVA: 0x00698D88 File Offset: 0x00696F88
			// (set) Token: 0x06003F4E RID: 16206 RVA: 0x00698D90 File Offset: 0x00696F90
			public bool PausedDueToHashMismatch { get; set; }

			// Token: 0x170004F0 RID: 1264
			// (get) Token: 0x06003F4F RID: 16207 RVA: 0x00698D99 File Offset: 0x00696F99
			// (set) Token: 0x06003F50 RID: 16208 RVA: 0x00698DA1 File Offset: 0x00696FA1
			public WorldGenerator.SnapshotFrequency SnapshotFrequency { get; set; }

			// Token: 0x170004F1 RID: 1265
			// (get) Token: 0x06003F51 RID: 16209 RVA: 0x00698DAA File Offset: 0x00696FAA
			// (set) Token: 0x06003F52 RID: 16210 RVA: 0x00698DB2 File Offset: 0x00696FB2
			public bool Paused
			{
				get
				{
					return this._paused;
				}
				set
				{
					this._paused = value;
					if (value)
					{
						this.PauseAfterPass = null;
						return;
					}
					this.PausedDueToHashMismatch = false;
				}
			}

			// Token: 0x170004F2 RID: 1266
			// (get) Token: 0x06003F53 RID: 16211 RVA: 0x00698DCD File Offset: 0x00696FCD
			// (set) Token: 0x06003F54 RID: 16212 RVA: 0x00698DD5 File Offset: 0x00696FD5
			public bool QueuedAbort { get; set; }

			// Token: 0x06003F55 RID: 16213 RVA: 0x00698DE0 File Offset: 0x00696FE0
			public WorldGenSnapshot GetSnapshot(GenPass pass)
			{
				WorldGenSnapshot result;
				if (!this._snapshots.TryGetValue(pass, out result))
				{
					return null;
				}
				return result;
			}

			// Token: 0x06003F56 RID: 16214 RVA: 0x00698E00 File Offset: 0x00697000
			public Controller(WorldManifest prevManifest = null)
			{
				this._previousManifest = prevManifest;
				this.PauseOnHashMismatch = true;
				this.SnapshotFrequency = WorldGenerator.SnapshotFrequency.None;
			}

			// Token: 0x06003F57 RID: 16215 RVA: 0x00698E20 File Offset: 0x00697020
			internal void SetGenerator(WorldGenerator generator)
			{
				this._generator = generator;
				this._snapshots = WorldGenSnapshot.LoadSnapshots(this._previousManifest, this.Passes);
				if (this._previousManifest != null)
				{
					using (IEnumerator<GenPassResult> enumerator = (from r in this._previousManifest.GenPassResults
					where r.Skipped
					select r).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GenPassResult r = enumerator.Current;
							GenPass genPass = this.Passes.SingleOrDefault((GenPass p) => p.Name == r.Name);
							if (genPass != null)
							{
								genPass.Disable();
							}
						}
					}
				}
				if (this.OnPassesLoaded != null)
				{
					this.OnPassesLoaded(this);
				}
			}

			// Token: 0x06003F58 RID: 16216 RVA: 0x00698EF8 File Offset: 0x006970F8
			internal void OnPaused()
			{
				WorldGenerator.SetDebugWorldGenUIVisibility(true);
				this.ForceUpdateProgress();
				Thread.Sleep(10);
			}

			// Token: 0x06003F59 RID: 16217 RVA: 0x00698F10 File Offset: 0x00697110
			internal void OnPassCompleted()
			{
				int num = WorldGenerator.PassResults.Count - 1;
				GenPassResult genPassResult = WorldGenerator.PassResults[num];
				WorldGenSnapshot snapshot = this.GetSnapshot(this.CurrentPass);
				GenPass genPass = this.Passes.Skip(WorldGenerator.PassResults.Count).FirstOrDefault<GenPass>();
				if (UIWorldGenDebug.ActiveInstance != null || genPass == null)
				{
					genPassResult.Hash = new uint?(WorldGenerator.HashWorld());
				}
				Trace.WriteLine(genPassResult);
				foreach (GenPass genPass2 in this.Passes.Skip(num))
				{
					WorldGenSnapshot snapshot2 = this.GetSnapshot(genPass2);
					if (snapshot2 != null && !snapshot2.GenPassResults[num].Matches(genPassResult))
					{
						this._snapshots.Remove(genPass2);
					}
				}
				bool flag = this.SnapshotFrequency == WorldGenerator.SnapshotFrequency.Always || (this.SnapshotFrequency == WorldGenerator.SnapshotFrequency.Automatic && (this.MsSinceLastSnapshot() > 500 || (genPass != null && genPass == this.PauseAfterPass)));
				if (genPassResult.Skipped)
				{
					flag = false;
				}
				if (this.QueuedAbort)
				{
					flag = false;
				}
				if (snapshot != null && snapshot.IsValidHistoryOf(WorldGen.Manifest))
				{
					flag = false;
					if (snapshot.Outdated)
					{
						snapshot.ResaveForCurrentVersion();
					}
				}
				if (flag)
				{
					this.TryCreateSnapshot();
				}
				this.CheckLatestPassResultAgainstManifest(num, genPassResult, snapshot);
				if (this.PauseAfterPass == this.CurrentPass)
				{
					this.Paused = true;
				}
				if (!Main.gameMenu)
				{
					Main.QueueMainThreadAction(new Action(Main.sectionManager.SetAllFramedSectionsAsNeedingRefresh));
				}
			}

			// Token: 0x06003F5A RID: 16218 RVA: 0x006990AC File Offset: 0x006972AC
			private void CheckLatestPassResultAgainstManifest(int currentPassIndex, GenPassResult result, WorldGenSnapshot prevSnapshot)
			{
				if (this._previousManifest == null)
				{
					return;
				}
				if (currentPassIndex >= this._previousManifest.GenPassResults.Count)
				{
					return;
				}
				if (this._previousManifest.GenPassResults[currentPassIndex].Matches(result))
				{
					return;
				}
				this._previousManifest = null;
				string text = string.Format("{0} output changed since last gen.", this.CurrentPass.Name);
				if (this.PauseOnHashMismatch && prevSnapshot != null)
				{
					try
					{
						prevSnapshot.Load();
						this.ReportException(text + " The previous output has been loaded as a snapshot (use /swap and /snapshotdiff to compare)", null);
						goto IL_96;
					}
					catch (Exception ex)
					{
						this.ReportException(text + "An attempt was made to load a snapshot of the previous output, but an exception occurred", ex);
						goto IL_96;
					}
				}
				this.ReportException(text, null);
				IL_96:
				if (this.PauseOnHashMismatch)
				{
					this.Paused = true;
					this.PausedDueToHashMismatch = true;
				}
			}

			// Token: 0x06003F5B RID: 16219 RVA: 0x00699178 File Offset: 0x00697378
			public void DeleteSnapshot(GenPass pass)
			{
				Utils.TryOperateInLock(pass, delegate
				{
					WorldGenSnapshot snap;
					if (this._snapshots.TryGetValue(pass, out snap))
					{
						this._snapshots.Remove(pass);
						WorldGenSnapshot.Delete(snap);
					}
				});
			}

			// Token: 0x06003F5C RID: 16220 RVA: 0x006991B1 File Offset: 0x006973B1
			public void DeleteAllSnapshots()
			{
				this.TryOperateInControlLock(delegate
				{
					this._snapshots.Clear();
					WorldGenSnapshot.DeleteAllForCurrentWorld();
				});
			}

			// Token: 0x06003F5D RID: 16221 RVA: 0x006991C8 File Offset: 0x006973C8
			private int MsSinceLastSnapshot()
			{
				int num = this.Passes.GetRange(0, WorldGenerator.PassResults.Count).FindLastIndex(new Predicate<GenPass>(this._snapshots.ContainsKey));
				return WorldGenerator.PassResults.Skip(num + 1).Sum((GenPassResult r) => r.DurationMs);
			}

			// Token: 0x06003F5E RID: 16222 RVA: 0x00699234 File Offset: 0x00697434
			public void ForceUpdateProgress()
			{
				GenerationProgress progress = this._generator._progress;
				progress.Message = ((WorldGenerator.PassResults.Count == 0) ? "World Cleared" : ("Paused after " + this.Passes[WorldGenerator.PassResults.Count - 1].Name));
				progress.TotalWeight = (from p in this.Passes
				where p.Enabled
				select p).Sum((GenPass p) => p.Weight);
				progress.TotalWeightedProgress = (from p in this.Passes.Take(WorldGenerator.PassResults.Count)
				where p.Enabled
				select p).Sum((GenPass p) => p.Weight);
			}

			// Token: 0x06003F5F RID: 16223 RVA: 0x00699341 File Offset: 0x00697541
			public bool TryOperateInControlLock(Action action)
			{
				return Utils.TryOperateInLock(this._generator._controlLock, action);
			}

			// Token: 0x06003F60 RID: 16224 RVA: 0x00699354 File Offset: 0x00697554
			public bool TryCreateSnapshot()
			{
				return this.TryOperateInControlLock(delegate
				{
					if (WorldGen.Manifest.FinalHash == null)
					{
						Main.NewText("Pass was not run with worldgen debugging enabled, please re-run", 240, 30, 30);
						return;
					}
					uint? finalHash = WorldGen.Manifest.FinalHash;
					uint num = WorldGenerator.HashWorld();
					if (!(finalHash.GetValueOrDefault() == num & finalHash != null))
					{
						Main.NewText("World has been modified since last gen pass completed. Please rerun or use /snapshot instead", 240, 30, 30);
						return;
					}
					try
					{
						this._snapshots[this.LastCompletedPass] = WorldGenSnapshot.Create();
					}
					catch (Exception ex)
					{
						this.ReportException("Exception occured while creating snapshot", ex);
					}
				});
			}

			// Token: 0x06003F61 RID: 16225 RVA: 0x00699368 File Offset: 0x00697568
			public bool TryReset()
			{
				return this.TryOperateInControlLock(delegate
				{
					this.UpdatePreviousManifest();
					WorldGen.RestoreTemporaryStateChanges();
					WorldGen.clearWorld();
					WorldGen.Reset();
					this.ForceUpdateProgress();
					this.Paused = true;
					Main.NewText("World Reset", byte.MaxValue, byte.MaxValue, 0);
				});
			}

			// Token: 0x06003F62 RID: 16226 RVA: 0x0069937C File Offset: 0x0069757C
			private void UpdatePreviousManifest()
			{
				if (this._previousManifest == null || WorldGenerator.PassResults.Count > this._previousManifest.GenPassResults.Count)
				{
					this._previousManifest = WorldGen.Manifest;
				}
			}

			// Token: 0x06003F63 RID: 16227 RVA: 0x006993B0 File Offset: 0x006975B0
			public bool TryResetToSnapshot(GenPass pass)
			{
				WorldGenSnapshot snap = this.GetSnapshot(pass);
				return snap != null && !snap.Outdated && this.TryOperateInControlLock(delegate
				{
					try
					{
						this.UpdatePreviousManifest();
						snap.Restore();
						this.ForceUpdateProgress();
					}
					catch (Exception ex)
					{
						this.ReportException("Exception occured while restoring snapshot", ex);
					}
				});
			}

			// Token: 0x06003F64 RID: 16228 RVA: 0x00699400 File Offset: 0x00697600
			public bool TryRunToEndOfPass(GenPass pass, bool useSnapshots = true, bool mustRunPass = true)
			{
				if (!pass.Enabled)
				{
					return false;
				}
				int passIndex = this.Passes.IndexOf(pass);
				Func<GenPass, bool> <>9__1;
				if (this.TryOperateInControlLock(delegate
				{
					IEnumerable<GenPass> source = this.Passes.Take(passIndex + (mustRunPass ? 0 : 1)).Reverse<GenPass>();
					Func<GenPass, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((GenPass p) => this.GetSnapshot(p) != null && !this.GetSnapshot(p).Outdated));
					}
					GenPass genPass = source.FirstOrDefault(predicate);
					bool flag = passIndex < WorldGenerator.PassResults.Count;
					if (useSnapshots && genPass != null && (flag || this.Passes.IndexOf(genPass) >= WorldGenerator.PassResults.Count))
					{
						this.TryResetToSnapshot(genPass);
					}
					else if (flag)
					{
						this.TryReset();
					}
					if (WorldGenerator.PassResults.Count == passIndex + 1)
					{
						this.Paused = true;
						return;
					}
					this.PauseAfterPass = pass;
					this.Paused = false;
				}))
				{
					return true;
				}
				if (pass == this.CurrentPass || passIndex > WorldGenerator.PassResults.Count)
				{
					this.PauseAfterPass = pass;
					return true;
				}
				return false;
			}

			// Token: 0x06003F65 RID: 16229 RVA: 0x0069949C File Offset: 0x0069769C
			public bool TryResetToPreviousPass(GenPass pass)
			{
				int count = this.Passes.IndexOf(pass);
				GenPass genPass = this.Passes.Take(count).Reverse<GenPass>().FirstOrDefault((GenPass p) => p.Enabled);
				if (genPass == null)
				{
					return this.TryReset();
				}
				return this.TryRunToEndOfPass(genPass, true, false);
			}

			// Token: 0x06003F66 RID: 16230 RVA: 0x006994FF File Offset: 0x006976FF
			internal void ReportException(string message, Exception ex = null)
			{
				Trace.WriteLine((ex != null) ? ex.ToString() : message);
				if (!DebugOptions.enableDebugCommands)
				{
					return;
				}
				this.Paused = true;
				WorldGenerator.SetDebugWorldGenUIVisibility(true);
				UIWorldGenDebug.ActiveInstance.UnhideChat();
				Main.NewText(message, byte.MaxValue, 0, 0);
			}

			// Token: 0x040067B2 RID: 26546
			private WorldManifest _previousManifest;

			// Token: 0x040067B3 RID: 26547
			private Dictionary<GenPass, WorldGenSnapshot> _snapshots;

			// Token: 0x040067B4 RID: 26548
			public Action<WorldGenerator.Controller> OnPassesLoaded;

			// Token: 0x040067B5 RID: 26549
			private WorldGenerator _generator;

			// Token: 0x040067BA RID: 26554
			private bool _paused;
		}
	}
}
