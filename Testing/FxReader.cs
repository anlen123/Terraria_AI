using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Content.Readers;

namespace Terraria.Testing
{
	// Token: 0x02000110 RID: 272
	public class FxReader : IAssetReader
	{
		// Token: 0x06001AB2 RID: 6834 RVA: 0x004F7404 File Offset: 0x004F5604
		public FxReader(GraphicsDevice graphicsDevice)
		{
			this._graphicsDevice = graphicsDevice;
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x004F7414 File Offset: 0x004F5614
		public T FromStream<T>(Stream stream) where T : class
		{
			if (typeof(T) != typeof(Effect))
			{
				throw AssetLoadException.FromInvalidReader<FxReader, T>();
			}
			string effectCode;
			using (StreamReader streamReader = new StreamReader(stream))
			{
				effectCode = streamReader.ReadToEnd();
			}
			CompiledEffectContent compiledEffectContent = new EffectProcessor().Process(new EffectContent
			{
				EffectCode = effectCode
			}, new FxReader.DummyPipelineContext());
			return new Effect(this._graphicsDevice, compiledEffectContent.GetEffectCode()) as T;
		}

		// Token: 0x040014FF RID: 5375
		private readonly GraphicsDevice _graphicsDevice;

		// Token: 0x0200071F RID: 1823
		private class DummyPipelineContext : ContentProcessorContext
		{
			// Token: 0x17000510 RID: 1296
			// (get) Token: 0x06004049 RID: 16457 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public override TargetPlatform TargetPlatform
			{
				get
				{
					return TargetPlatform.Windows;
				}
			}

			// Token: 0x17000511 RID: 1297
			// (get) Token: 0x0600404A RID: 16458 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public override GraphicsProfile TargetProfile
			{
				get
				{
					return GraphicsProfile.Reach;
				}
			}

			// Token: 0x17000512 RID: 1298
			// (get) Token: 0x0600404B RID: 16459 RVA: 0x0069C968 File Offset: 0x0069AB68
			public override ContentBuildLogger Logger
			{
				get
				{
					return this._logger;
				}
			}

			// Token: 0x17000513 RID: 1299
			// (get) Token: 0x0600404C RID: 16460 RVA: 0x0069C970 File Offset: 0x0069AB70
			public override OpaqueDataDictionary Parameters
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000514 RID: 1300
			// (get) Token: 0x0600404D RID: 16461 RVA: 0x0069C977 File Offset: 0x0069AB77
			public override string BuildConfiguration
			{
				get
				{
					return "Release";
				}
			}

			// Token: 0x17000515 RID: 1301
			// (get) Token: 0x0600404E RID: 16462 RVA: 0x0069C970 File Offset: 0x0069AB70
			public override string OutputFilename
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000516 RID: 1302
			// (get) Token: 0x0600404F RID: 16463 RVA: 0x0069C970 File Offset: 0x0069AB70
			public override string OutputDirectory
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000517 RID: 1303
			// (get) Token: 0x06004050 RID: 16464 RVA: 0x0069C970 File Offset: 0x0069AB70
			public override string IntermediateDirectory
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x06004051 RID: 16465 RVA: 0x0069C970 File Offset: 0x0069AB70
			public override void AddDependency(string filename)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06004052 RID: 16466 RVA: 0x0069C970 File Offset: 0x0069AB70
			public override void AddOutputFile(string filename)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06004053 RID: 16467 RVA: 0x0069C970 File Offset: 0x0069AB70
			public override TOutput BuildAndLoadAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06004054 RID: 16468 RVA: 0x0069C970 File Offset: 0x0069AB70
			public override ExternalReference<TOutput> BuildAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName, string assetName)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06004055 RID: 16469 RVA: 0x0069C970 File Offset: 0x0069AB70
			public override TOutput Convert<TInput, TOutput>(TInput input, string processorName, OpaqueDataDictionary processorParameters)
			{
				throw new NotImplementedException();
			}

			// Token: 0x04006920 RID: 26912
			private readonly ContentBuildLogger _logger = new FxReader.PipelineLogger();
		}

		// Token: 0x02000720 RID: 1824
		private class PipelineLogger : ContentBuildLogger
		{
			// Token: 0x06004057 RID: 16471 RVA: 0x00009E06 File Offset: 0x00008006
			public override void LogImportantMessage(string message, params object[] messageArgs)
			{
			}

			// Token: 0x06004058 RID: 16472 RVA: 0x00009E06 File Offset: 0x00008006
			public override void LogMessage(string message, params object[] messageArgs)
			{
			}

			// Token: 0x06004059 RID: 16473 RVA: 0x00009E06 File Offset: 0x00008006
			public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs)
			{
			}
		}
	}
}
