using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000C0 RID: 192
	public class WorldGenSnapshot
	{
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x004DF0B3 File Offset: 0x004DD2B3
		// (set) Token: 0x060017B0 RID: 6064 RVA: 0x004DF0BB File Offset: 0x004DD2BB
		public WorldManifest Manifest { get; private set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x004DF0C4 File Offset: 0x004DD2C4
		// (set) Token: 0x060017B2 RID: 6066 RVA: 0x004DF0CC File Offset: 0x004DD2CC
		private string Path { get; set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060017B3 RID: 6067 RVA: 0x004DF0D5 File Offset: 0x004DD2D5
		// (set) Token: 0x060017B4 RID: 6068 RVA: 0x004DF0DD File Offset: 0x004DD2DD
		private string GenVarsJson { get; set; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060017B5 RID: 6069 RVA: 0x004DF0E6 File Offset: 0x004DD2E6
		public List<GenPassResult> GenPassResults
		{
			get
			{
				return this.Manifest.GenPassResults;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060017B6 RID: 6070 RVA: 0x004DF0F4 File Offset: 0x004DD2F4
		public bool Outdated
		{
			get
			{
				if (!(this.Manifest.GitSHA != GitStatus.GitSHA) && !(this.Manifest.Version != Main.versionNumber))
				{
					return !this._matchingPasses.Zip(this.GenPassResults, (GenPass p, GenPassResult r) => p.Enabled == !r.Skipped).All((bool x) => x);
				}
				return true;
			}
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x004DF188 File Offset: 0x004DD388
		public override string ToString()
		{
			GenPassResult genPassResult = this.GenPassResults.Last<GenPassResult>();
			return string.Format("Pass - {0}, rand - {1:X8}, hash - {2:X8}", genPassResult.Name, genPassResult.RandNext, genPassResult.Hash);
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060017B8 RID: 6072 RVA: 0x004DF1C7 File Offset: 0x004DD3C7
		private static string PathForActiveWorld
		{
			get
			{
				return System.IO.Path.ChangeExtension(Main.ActiveWorldFileData.Path, null) + WorldGenSnapshot.SnapshotFolderSuffix;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060017B9 RID: 6073 RVA: 0x004DF1E3 File Offset: 0x004DD3E3
		public static long EstimatedDiskUsage
		{
			get
			{
				return WorldGenSnapshot._snapshotSizeCache.Values.Sum();
			}
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x004DF1F4 File Offset: 0x004DD3F4
		public static void DeleteAllForCurrentWorld()
		{
			if (Directory.Exists(WorldGenSnapshot.PathForActiveWorld))
			{
				try
				{
					Directory.Delete(WorldGenSnapshot.PathForActiveWorld, true);
				}
				catch (Exception)
				{
				}
			}
			WorldGenSnapshot._snapshotSizeCache.Clear();
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x004DF238 File Offset: 0x004DD438
		public static WorldGenSnapshot Create()
		{
			WorldGenSnapshot worldGenSnapshot = new WorldGenSnapshot
			{
				Manifest = WorldGen.Manifest.Clone(),
				GenVarsJson = WorldGenSnapshot.SnapshotGenVars.Serialize()
			};
			worldGenSnapshot._matchingPasses = WorldGenerator.CurrentController.Passes.GetRange(0, worldGenSnapshot.GenPassResults.Count);
			worldGenSnapshot.Path = System.IO.Path.Combine(WorldGenSnapshot.PathForActiveWorld, worldGenSnapshot + WorldGenSnapshot.Extension);
			if (!Directory.Exists(WorldGenSnapshot.PathForActiveWorld))
			{
				Directory.CreateDirectory(WorldGenSnapshot.PathForActiveWorld);
			}
			TileSnapshot.Create(worldGenSnapshot);
			using (BinaryWriter binaryWriter = new BinaryWriter(File.Create(worldGenSnapshot.Path)))
			{
				binaryWriter.Write(worldGenSnapshot.Manifest.Serialize());
				binaryWriter.Write(worldGenSnapshot.GenVarsJson);
				worldGenSnapshot._dataOffset = (int)binaryWriter.BaseStream.Position;
				TileSnapshot.Save(binaryWriter);
				WorldGenSnapshot._snapshotSizeCache[worldGenSnapshot.Path] = binaryWriter.BaseStream.Length;
			}
			return worldGenSnapshot;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x004DF340 File Offset: 0x004DD540
		public static void Delete(WorldGenSnapshot snap)
		{
			try
			{
				File.Delete(snap.Path);
			}
			catch (Exception)
			{
			}
			WorldGenSnapshot._snapshotSizeCache.Remove(snap.Path);
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x004DF380 File Offset: 0x004DD580
		public void ResaveForCurrentVersion()
		{
			this.Manifest = WorldGen.Manifest.Clone();
			this.GenVarsJson = WorldGenSnapshot.SnapshotGenVars.Serialize();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (FileStream fileStream = File.OpenRead(this.Path))
				{
					fileStream.Seek((long)this._dataOffset, SeekOrigin.Current);
					fileStream.CopyTo(memoryStream);
				}
				memoryStream.Position = 0L;
				using (BinaryWriter binaryWriter = new BinaryWriter(File.Create(this.Path)))
				{
					binaryWriter.Write(this.Manifest.Serialize());
					binaryWriter.Write(this.GenVarsJson);
					this._dataOffset = (int)binaryWriter.BaseStream.Position;
					memoryStream.CopyTo(binaryWriter.BaseStream);
					WorldGenSnapshot._snapshotSizeCache[this.Path] = binaryWriter.BaseStream.Length;
				}
			}
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x004DF48C File Offset: 0x004DD68C
		public static Dictionary<GenPass, WorldGenSnapshot> LoadSnapshots(WorldManifest worldManifest, List<GenPass> passes)
		{
			WorldGenSnapshot._snapshotSizeCache.Clear();
			Dictionary<GenPass, WorldGenSnapshot> dictionary = new Dictionary<GenPass, WorldGenSnapshot>();
			Task.Factory.StartNew(delegate()
			{
				WorldGenSnapshot.DeleteSnapshotsForOtherWorlds(WorldGenSnapshot.PathForActiveWorld);
			});
			if (!Directory.Exists(WorldGenSnapshot.PathForActiveWorld))
			{
				return dictionary;
			}
			if (worldManifest == null)
			{
				Trace.WriteLine("Deleting old snapshots because a new world is being created (/regen was not used)");
				WorldGenSnapshot.DeleteAllForCurrentWorld();
				return dictionary;
			}
			using (IEnumerator<string> enumerator = Directory.EnumerateFiles(WorldGenSnapshot.PathForActiveWorld, "*" + WorldGenSnapshot.Extension).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WorldGenSnapshot worldGenSnapshot;
					GenPass key;
					if (WorldGenSnapshot.ReadSnapshot(enumerator.Current, out worldGenSnapshot) && worldGenSnapshot.IsValidHistoryOf(worldManifest) && WorldGenSnapshot.FindMatchingGenPass(worldGenSnapshot.Manifest, passes, out key))
					{
						worldGenSnapshot._matchingPasses = passes.GetRange(0, worldGenSnapshot.GenPassResults.Count);
						dictionary[key] = worldGenSnapshot;
					}
					else
					{
						Trace.WriteLine(string.Format("Deleting snapshot ({0}) due to manifest mismatch. A change to the codebase has probably invalidated it.", worldGenSnapshot));
						WorldGenSnapshot.Delete(worldGenSnapshot);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x004DF59C File Offset: 0x004DD79C
		private static void DeleteSnapshotsForOtherWorlds(string snapshotPathForActiveWorld)
		{
			string directoryName = System.IO.Path.GetDirectoryName(snapshotPathForActiveWorld);
			string fileName = System.IO.Path.GetFileName(snapshotPathForActiveWorld);
			foreach (string text in Directory.EnumerateDirectories(directoryName))
			{
				if (text.EndsWith(WorldGenSnapshot.SnapshotFolderSuffix) && !(System.IO.Path.GetFileName(text) == fileName))
				{
					Trace.WriteLine("Deleting snapshot directory: " + text);
					try
					{
						Directory.Delete(text, true);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x004DF634 File Offset: 0x004DD834
		private static bool FindMatchingGenPass(WorldManifest manifest, List<GenPass> passes, out GenPass pass)
		{
			pass = null;
			List<GenPassResult> genPassResults = manifest.GenPassResults;
			if (genPassResults.Count <= passes.Count)
			{
				if (genPassResults.Zip(passes, (GenPassResult r, GenPass p) => r.Name == p.Name).All((bool x) => x))
				{
					pass = passes[genPassResults.Count - 1];
					return true;
				}
			}
			return false;
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x004DF6B8 File Offset: 0x004DD8B8
		public bool IsValidHistoryOf(WorldManifest target)
		{
			return WorldGenSnapshot.StartsWith(target.GenPassResults, this.Manifest.GenPassResults);
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x004DF6D0 File Offset: 0x004DD8D0
		private static bool StartsWith(List<GenPassResult> list, List<GenPassResult> prefix)
		{
			if (prefix.Count <= list.Count)
			{
				return list.Zip(prefix, (GenPassResult a, GenPassResult b) => a.Matches(b)).All((bool x) => x);
			}
			return false;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x004DF738 File Offset: 0x004DD938
		private static bool ReadSnapshot(string path, out WorldGenSnapshot snap)
		{
			bool result;
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(path)))
				{
					snap = new WorldGenSnapshot
					{
						Path = path,
						Manifest = WorldManifest.Deserialize(binaryReader.ReadString()),
						GenVarsJson = binaryReader.ReadString(),
						_dataOffset = (int)binaryReader.BaseStream.Position
					};
					WorldGenSnapshot._snapshotSizeCache[path] = binaryReader.BaseStream.Length;
					result = true;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Concat(new object[]
				{
					"Failed to read snapshot: ",
					path,
					", ",
					ex
				}));
				snap = null;
				result = false;
			}
			return result;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x004DF800 File Offset: 0x004DDA00
		public void Load()
		{
			if (TileSnapshot.Context == this)
			{
				return;
			}
			using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(this.Path)))
			{
				binaryReader.BaseStream.Seek((long)this._dataOffset, SeekOrigin.Current);
				TileSnapshot.Load(binaryReader, this);
			}
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x004DF860 File Offset: 0x004DDA60
		public void Restore()
		{
			this.Load();
			WorldGen.RestoreTemporaryStateChanges();
			WorldGen.Reset();
			WorldGen.Manifest = this.Manifest.Clone();
			WorldGenSnapshot.SnapshotGenVars.DeserializeAndApply(this.GenVarsJson);
			TileSnapshot.Restore();
			NPC[] npc = Main.npc;
			for (int i = 0; i < npc.Length; i++)
			{
				npc[i].active = false;
			}
			Main.NewText("Restored " + this, byte.MaxValue, byte.MaxValue, 0);
		}

		// Token: 0x04001283 RID: 4739
		private int _dataOffset;

		// Token: 0x04001284 RID: 4740
		private List<GenPass> _matchingPasses;

		// Token: 0x04001285 RID: 4741
		private static string SnapshotFolderSuffix = "_gensnapshots";

		// Token: 0x04001286 RID: 4742
		private static string Extension = ".gensnapshot";

		// Token: 0x04001287 RID: 4743
		private static IDictionary<string, long> _snapshotSizeCache = new Dictionary<string, long>();

		// Token: 0x020006EC RID: 1772
		[JsonConverter(typeof(WorldGenSnapshot.SnapshotGenVars))]
		private class SnapshotGenVars : JsonConverter
		{
			// Token: 0x06003F73 RID: 16243 RVA: 0x006996E0 File Offset: 0x006978E0
			public static string Serialize()
			{
				return JsonConvert.SerializeObject(new WorldGenSnapshot.SnapshotGenVars(), WorldGenSnapshot.SnapshotGenVars.SerializerSettings);
			}

			// Token: 0x06003F74 RID: 16244 RVA: 0x006996F1 File Offset: 0x006978F1
			public static void DeserializeAndApply(string json)
			{
				JsonConvert.DeserializeObject<WorldGenSnapshot.SnapshotGenVars>(json, WorldGenSnapshot.SnapshotGenVars.SerializerSettings);
			}

			// Token: 0x06003F75 RID: 16245 RVA: 0x006996FF File Offset: 0x006978FF
			public override bool CanConvert(Type objectType)
			{
				return objectType == typeof(WorldGenSnapshot.SnapshotGenVars);
			}

			// Token: 0x06003F76 RID: 16246 RVA: 0x00699714 File Offset: 0x00697914
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				if (reader.TokenType != 1)
				{
					throw new JsonReaderException();
				}
				while (reader.Read() && reader.TokenType != 13)
				{
					if (reader.TokenType != 4)
					{
						throw new JsonReaderException("Expected PropertyName");
					}
					string key = (string)reader.Value;
					if (!reader.Read())
					{
						throw new JsonReaderException();
					}
					MemberInfo member;
					if (WorldGenSnapshot.SnapshotGenVars.fieldsAndProperties.TryGetValue(key, out member))
					{
						this.SetValue(member, serializer.Deserialize(reader, this.GetType(member)));
					}
					else
					{
						reader.Skip();
					}
				}
				return null;
			}

			// Token: 0x06003F77 RID: 16247 RVA: 0x006997A0 File Offset: 0x006979A0
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				writer.WriteStartObject();
				foreach (MemberInfo memberInfo in WorldGenSnapshot.SnapshotGenVars.fieldsAndProperties.Values)
				{
					writer.WritePropertyName(memberInfo.Name);
					serializer.Serialize(writer, this.GetValue(memberInfo), this.GetType(memberInfo));
				}
				writer.WriteEndObject();
			}

			// Token: 0x06003F78 RID: 16248 RVA: 0x00699820 File Offset: 0x00697A20
			private Type GetType(MemberInfo member)
			{
				if (member is PropertyInfo)
				{
					return ((PropertyInfo)member).PropertyType;
				}
				if (member is FieldInfo)
				{
					return ((FieldInfo)member).FieldType;
				}
				throw new ArgumentException(member.GetType().ToString());
			}

			// Token: 0x06003F79 RID: 16249 RVA: 0x0069985C File Offset: 0x00697A5C
			private object GetValue(MemberInfo member)
			{
				if (member is PropertyInfo)
				{
					return ((PropertyInfo)member).GetGetMethod().Invoke(null, null);
				}
				if (member is FieldInfo)
				{
					return ((FieldInfo)member).GetValue(null);
				}
				throw new ArgumentException(member.GetType().ToString());
			}

			// Token: 0x06003F7A RID: 16250 RVA: 0x006998AC File Offset: 0x00697AAC
			private void SetValue(MemberInfo member, object v)
			{
				if (member is PropertyInfo)
				{
					((PropertyInfo)member).GetSetMethod().Invoke(null, new object[]
					{
						v
					});
					return;
				}
				if (member is FieldInfo)
				{
					((FieldInfo)member).SetValue(null, v);
					return;
				}
				throw new ArgumentException(member.GetType().ToString());
			}

			// Token: 0x040067C6 RID: 26566
			public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
			{
				ContractResolver = new EasyDeserializationJsonContractResolver(),
				PreserveReferencesHandling = 1,
				ReferenceLoopHandling = 2,
				TypeNameHandling = 4
			};

			// Token: 0x040067C7 RID: 26567
			private static Dictionary<string, MemberInfo> fieldsAndProperties = (from m in typeof(GenVars).GetFields(BindingFlags.Static | BindingFlags.Public).Concat(typeof(GenVars).GetProperties(BindingFlags.Static | BindingFlags.Public))
			where !(m is PropertyInfo) || ((PropertyInfo)m).CanWrite
			where !(m is FieldInfo) || !((FieldInfo)m).IsInitOnly
			where !m.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any<object>()
			select m).ToDictionary((MemberInfo m) => m.Name);
		}
	}
}
