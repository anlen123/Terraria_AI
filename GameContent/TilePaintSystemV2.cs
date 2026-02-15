using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent
{
	// Token: 0x0200025A RID: 602
	public class TilePaintSystemV2
	{
		// Token: 0x0600233F RID: 9023 RVA: 0x0053D118 File Offset: 0x0053B318
		public void Reset()
		{
			foreach (TilePaintSystemV2.TileRenderTargetHolder tileRenderTargetHolder in this._tilesRenders.Values)
			{
				tileRenderTargetHolder.Clear();
			}
			this._tilesRenders.Clear();
			foreach (TilePaintSystemV2.CageTopRenderTargetHolder cageTopRenderTargetHolder in this._cageTopRenders.Values)
			{
				cageTopRenderTargetHolder.Clear();
			}
			this._cageTopRenders.Clear();
			foreach (TilePaintSystemV2.WallRenderTargetHolder wallRenderTargetHolder in this._wallsRenders.Values)
			{
				wallRenderTargetHolder.Clear();
			}
			this._wallsRenders.Clear();
			foreach (TilePaintSystemV2.TreeTopRenderTargetHolder treeTopRenderTargetHolder in this._treeTopRenders.Values)
			{
				treeTopRenderTargetHolder.Clear();
			}
			this._treeTopRenders.Clear();
			foreach (TilePaintSystemV2.TreeBranchTargetHolder treeBranchTargetHolder in this._treeBranchRenders.Values)
			{
				treeBranchTargetHolder.Clear();
			}
			this._treeBranchRenders.Clear();
			foreach (TilePaintSystemV2.ARenderTargetHolder arenderTargetHolder in this._requests)
			{
				arenderTargetHolder.Clear();
			}
			this._requests.Clear();
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0053D300 File Offset: 0x0053B500
		public void RequestTile(ref TilePaintSystemV2.TileVariationkey lookupKey)
		{
			TilePaintSystemV2.TileRenderTargetHolder tileRenderTargetHolder;
			if (!this._tilesRenders.TryGetValue(lookupKey, out tileRenderTargetHolder))
			{
				tileRenderTargetHolder = new TilePaintSystemV2.TileRenderTargetHolder
				{
					Key = lookupKey
				};
				this._tilesRenders.Add(lookupKey, tileRenderTargetHolder);
			}
			if (tileRenderTargetHolder.IsReady)
			{
				return;
			}
			this._requests.Add(tileRenderTargetHolder);
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0053D35C File Offset: 0x0053B55C
		public void RequestCageTop(ref TilePaintSystemV2.CageTopVariationkey lookupKey)
		{
			TilePaintSystemV2.CageTopRenderTargetHolder cageTopRenderTargetHolder;
			if (!this._cageTopRenders.TryGetValue(lookupKey, out cageTopRenderTargetHolder))
			{
				cageTopRenderTargetHolder = new TilePaintSystemV2.CageTopRenderTargetHolder
				{
					Key = lookupKey
				};
				this._cageTopRenders.Add(lookupKey, cageTopRenderTargetHolder);
			}
			if (cageTopRenderTargetHolder.IsReady)
			{
				return;
			}
			this._requests.Add(cageTopRenderTargetHolder);
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0053D3B8 File Offset: 0x0053B5B8
		private void RequestTile_CheckForRelatedTileRequests(ref TilePaintSystemV2.TileVariationkey lookupKey)
		{
			if (lookupKey.TileType == 83)
			{
				TilePaintSystemV2.TileVariationkey tileVariationkey = new TilePaintSystemV2.TileVariationkey
				{
					TileType = 84,
					TileStyle = lookupKey.TileStyle,
					PaintColor = lookupKey.PaintColor
				};
				this.RequestTile(ref tileVariationkey);
			}
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x0053D404 File Offset: 0x0053B604
		public void RequestWall(ref TilePaintSystemV2.WallVariationKey lookupKey)
		{
			TilePaintSystemV2.WallRenderTargetHolder wallRenderTargetHolder;
			if (!this._wallsRenders.TryGetValue(lookupKey, out wallRenderTargetHolder))
			{
				wallRenderTargetHolder = new TilePaintSystemV2.WallRenderTargetHolder
				{
					Key = lookupKey
				};
				this._wallsRenders.Add(lookupKey, wallRenderTargetHolder);
			}
			if (wallRenderTargetHolder.IsReady)
			{
				return;
			}
			this._requests.Add(wallRenderTargetHolder);
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x0053D460 File Offset: 0x0053B660
		public void RequestTreeTop(ref TilePaintSystemV2.TreeFoliageVariantKey lookupKey)
		{
			TilePaintSystemV2.TreeTopRenderTargetHolder treeTopRenderTargetHolder;
			if (!this._treeTopRenders.TryGetValue(lookupKey, out treeTopRenderTargetHolder))
			{
				treeTopRenderTargetHolder = new TilePaintSystemV2.TreeTopRenderTargetHolder
				{
					Key = lookupKey
				};
				this._treeTopRenders.Add(lookupKey, treeTopRenderTargetHolder);
			}
			if (treeTopRenderTargetHolder.IsReady)
			{
				return;
			}
			this._requests.Add(treeTopRenderTargetHolder);
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0053D4BC File Offset: 0x0053B6BC
		public void RequestTreeBranch(ref TilePaintSystemV2.TreeFoliageVariantKey lookupKey)
		{
			TilePaintSystemV2.TreeBranchTargetHolder treeBranchTargetHolder;
			if (!this._treeBranchRenders.TryGetValue(lookupKey, out treeBranchTargetHolder))
			{
				treeBranchTargetHolder = new TilePaintSystemV2.TreeBranchTargetHolder
				{
					Key = lookupKey
				};
				this._treeBranchRenders.Add(lookupKey, treeBranchTargetHolder);
			}
			if (treeBranchTargetHolder.IsReady)
			{
				return;
			}
			this._requests.Add(treeBranchTargetHolder);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x0053D518 File Offset: 0x0053B718
		public Texture2D TryGetTileAndRequestIfNotReady(int tileType, int tileStyle, int paintColor)
		{
			TilePaintSystemV2.TileVariationkey key = new TilePaintSystemV2.TileVariationkey
			{
				TileType = tileType,
				TileStyle = tileStyle,
				PaintColor = paintColor
			};
			TilePaintSystemV2.TileRenderTargetHolder tileRenderTargetHolder;
			if (this._tilesRenders.TryGetValue(key, out tileRenderTargetHolder) && tileRenderTargetHolder.IsReady)
			{
				return tileRenderTargetHolder.Target;
			}
			this.RequestTile(ref key);
			return null;
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x0053D570 File Offset: 0x0053B770
		public Texture2D TryGetCageTopAndRequestIfNotReady(int cageStyle, int paintColor)
		{
			TilePaintSystemV2.CageTopVariationkey key = new TilePaintSystemV2.CageTopVariationkey
			{
				CageStyle = cageStyle,
				PaintColor = paintColor
			};
			TilePaintSystemV2.CageTopRenderTargetHolder cageTopRenderTargetHolder;
			if (this._cageTopRenders.TryGetValue(key, out cageTopRenderTargetHolder) && cageTopRenderTargetHolder.IsReady)
			{
				return cageTopRenderTargetHolder.Target;
			}
			this.RequestCageTop(ref key);
			return null;
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x0053D5C0 File Offset: 0x0053B7C0
		public Texture2D TryGetWallAndRequestIfNotReady(int wallType, int paintColor)
		{
			TilePaintSystemV2.WallVariationKey key = new TilePaintSystemV2.WallVariationKey
			{
				WallType = wallType,
				PaintColor = paintColor
			};
			TilePaintSystemV2.WallRenderTargetHolder wallRenderTargetHolder;
			if (this._wallsRenders.TryGetValue(key, out wallRenderTargetHolder) && wallRenderTargetHolder.IsReady)
			{
				return wallRenderTargetHolder.Target;
			}
			this.RequestWall(ref key);
			return null;
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0053D610 File Offset: 0x0053B810
		public Texture2D TryGetTreeTopAndRequestIfNotReady(int treeTopIndex, int treeTopStyle, int paintColor)
		{
			TilePaintSystemV2.TreeFoliageVariantKey key = new TilePaintSystemV2.TreeFoliageVariantKey
			{
				TextureIndex = treeTopIndex,
				TextureStyle = treeTopStyle,
				PaintColor = paintColor
			};
			TilePaintSystemV2.TreeTopRenderTargetHolder treeTopRenderTargetHolder;
			if (this._treeTopRenders.TryGetValue(key, out treeTopRenderTargetHolder) && treeTopRenderTargetHolder.IsReady)
			{
				return treeTopRenderTargetHolder.Target;
			}
			this.RequestTreeTop(ref key);
			return null;
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x0053D668 File Offset: 0x0053B868
		public Texture2D TryGetTreeBranchAndRequestIfNotReady(int treeTopIndex, int treeTopStyle, int paintColor)
		{
			TilePaintSystemV2.TreeFoliageVariantKey key = new TilePaintSystemV2.TreeFoliageVariantKey
			{
				TextureIndex = treeTopIndex,
				TextureStyle = treeTopStyle,
				PaintColor = paintColor
			};
			TilePaintSystemV2.TreeBranchTargetHolder treeBranchTargetHolder;
			if (this._treeBranchRenders.TryGetValue(key, out treeBranchTargetHolder) && treeBranchTargetHolder.IsReady)
			{
				return treeBranchTargetHolder.Target;
			}
			this.RequestTreeBranch(ref key);
			return null;
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0053D6C0 File Offset: 0x0053B8C0
		public void PrepareAllRequests()
		{
			if (this._requests.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._requests.Count; i++)
			{
				this._requests[i].Prepare();
			}
			this._requests.Clear();
		}

		// Token: 0x04004D5C RID: 19804
		private Dictionary<TilePaintSystemV2.CageTopVariationkey, TilePaintSystemV2.CageTopRenderTargetHolder> _cageTopRenders = new Dictionary<TilePaintSystemV2.CageTopVariationkey, TilePaintSystemV2.CageTopRenderTargetHolder>();

		// Token: 0x04004D5D RID: 19805
		private Dictionary<TilePaintSystemV2.TileVariationkey, TilePaintSystemV2.TileRenderTargetHolder> _tilesRenders = new Dictionary<TilePaintSystemV2.TileVariationkey, TilePaintSystemV2.TileRenderTargetHolder>();

		// Token: 0x04004D5E RID: 19806
		private Dictionary<TilePaintSystemV2.WallVariationKey, TilePaintSystemV2.WallRenderTargetHolder> _wallsRenders = new Dictionary<TilePaintSystemV2.WallVariationKey, TilePaintSystemV2.WallRenderTargetHolder>();

		// Token: 0x04004D5F RID: 19807
		private Dictionary<TilePaintSystemV2.TreeFoliageVariantKey, TilePaintSystemV2.TreeTopRenderTargetHolder> _treeTopRenders = new Dictionary<TilePaintSystemV2.TreeFoliageVariantKey, TilePaintSystemV2.TreeTopRenderTargetHolder>();

		// Token: 0x04004D60 RID: 19808
		private Dictionary<TilePaintSystemV2.TreeFoliageVariantKey, TilePaintSystemV2.TreeBranchTargetHolder> _treeBranchRenders = new Dictionary<TilePaintSystemV2.TreeFoliageVariantKey, TilePaintSystemV2.TreeBranchTargetHolder>();

		// Token: 0x04004D61 RID: 19809
		private List<TilePaintSystemV2.ARenderTargetHolder> _requests = new List<TilePaintSystemV2.ARenderTargetHolder>();

		// Token: 0x020007DC RID: 2012
		public abstract class ARenderTargetHolder
		{
			// Token: 0x17000538 RID: 1336
			// (get) Token: 0x0600423E RID: 16958 RVA: 0x006BCFCC File Offset: 0x006BB1CC
			public bool IsReady
			{
				get
				{
					return this._wasPrepared;
				}
			}

			// Token: 0x0600423F RID: 16959
			public abstract void Prepare();

			// Token: 0x06004240 RID: 16960
			public abstract void PrepareShader();

			// Token: 0x06004241 RID: 16961 RVA: 0x006BCFD4 File Offset: 0x006BB1D4
			public void Clear()
			{
				if (this.Target != null && !this.Target.IsDisposed)
				{
					this.Target.Dispose();
				}
			}

			// Token: 0x06004242 RID: 16962 RVA: 0x006BCFF8 File Offset: 0x006BB1F8
			protected void PrepareTextureIfNecessary(Texture2D originalTexture, Rectangle? sourceRect = null)
			{
				if (this.Target != null && !this.Target.IsContentLost)
				{
					return;
				}
				Main instance = Main.instance;
				if (sourceRect == null)
				{
					sourceRect = new Rectangle?(originalTexture.Frame(1, 1, 0, 0, 0, 0));
				}
				this.Target = new RenderTarget2D(instance.GraphicsDevice, sourceRect.Value.Width, sourceRect.Value.Height, false, instance.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				this.Target.ContentLost += this.Target_ContentLost;
				this.Target.Disposing += this.Target_Disposing;
				this.Target.Name = originalTexture.Name;
				instance.GraphicsDevice.SetRenderTarget(this.Target);
				instance.GraphicsDevice.Clear(Color.Transparent);
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
				this.PrepareShader();
				Rectangle value = sourceRect.Value;
				value.X = 0;
				value.Y = 0;
				Main.spriteBatch.Draw(originalTexture, value, Color.White);
				Main.spriteBatch.End();
				instance.GraphicsDevice.SetRenderTarget(null);
				this._wasPrepared = true;
			}

			// Token: 0x06004243 RID: 16963 RVA: 0x006BD136 File Offset: 0x006BB336
			private void Target_Disposing(object sender, EventArgs e)
			{
				this._wasPrepared = false;
				this.Target = null;
			}

			// Token: 0x06004244 RID: 16964 RVA: 0x006BD146 File Offset: 0x006BB346
			private void Target_ContentLost(object sender, EventArgs e)
			{
				this._wasPrepared = false;
			}

			// Token: 0x06004245 RID: 16965 RVA: 0x006BD150 File Offset: 0x006BB350
			protected void PrepareShader(int paintColor, TreePaintingSettings settings)
			{
				Effect tileShader = Main.tileShader;
				tileShader.Parameters["leafHueTestOffset"].SetValue(settings.HueTestOffset);
				tileShader.Parameters["leafMinHue"].SetValue(settings.SpecialGroupMinimalHueValue);
				tileShader.Parameters["leafMaxHue"].SetValue(settings.SpecialGroupMaximumHueValue);
				tileShader.Parameters["leafMinSat"].SetValue(settings.SpecialGroupMinimumSaturationValue);
				tileShader.Parameters["leafMaxSat"].SetValue(settings.SpecialGroupMaximumSaturationValue);
				tileShader.Parameters["invertSpecialGroupResult"].SetValue(settings.InvertSpecialGroupResult);
				int index = Main.ConvertPaintIdToTileShaderIndex(paintColor, settings.UseSpecialGroups, settings.UseWallShaderHacks);
				tileShader.CurrentTechnique.Passes[index].Apply();
				RenderTarget2D target = this.Target;
				target.Name = target.Name + " paint: " + paintColor;
			}

			// Token: 0x040070E7 RID: 28903
			public RenderTarget2D Target;

			// Token: 0x040070E8 RID: 28904
			protected bool _wasPrepared;
		}

		// Token: 0x020007DD RID: 2013
		public class TreeTopRenderTargetHolder : TilePaintSystemV2.ARenderTargetHolder
		{
			// Token: 0x06004247 RID: 16967 RVA: 0x006BD250 File Offset: 0x006BB450
			public override void Prepare()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>(TextureAssets.TreeTop[this.Key.TextureIndex].Name, 1);
				base.PrepareTextureIfNecessary(asset.Value, null);
			}

			// Token: 0x06004248 RID: 16968 RVA: 0x006BD294 File Offset: 0x006BB494
			public override void PrepareShader()
			{
				base.PrepareShader(this.Key.PaintColor, TreePaintSystemData.GetTreeFoliageSettings(this.Key.TextureIndex, this.Key.TextureStyle));
			}

			// Token: 0x040070E9 RID: 28905
			public TilePaintSystemV2.TreeFoliageVariantKey Key;
		}

		// Token: 0x020007DE RID: 2014
		public class TreeBranchTargetHolder : TilePaintSystemV2.ARenderTargetHolder
		{
			// Token: 0x0600424A RID: 16970 RVA: 0x006BD2CC File Offset: 0x006BB4CC
			public override void Prepare()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>(TextureAssets.TreeBranch[this.Key.TextureIndex].Name, 1);
				base.PrepareTextureIfNecessary(asset.Value, null);
			}

			// Token: 0x0600424B RID: 16971 RVA: 0x006BD310 File Offset: 0x006BB510
			public override void PrepareShader()
			{
				base.PrepareShader(this.Key.PaintColor, TreePaintSystemData.GetTreeFoliageSettings(this.Key.TextureIndex, this.Key.TextureStyle));
			}

			// Token: 0x040070EA RID: 28906
			public TilePaintSystemV2.TreeFoliageVariantKey Key;
		}

		// Token: 0x020007DF RID: 2015
		public class TileRenderTargetHolder : TilePaintSystemV2.ARenderTargetHolder
		{
			// Token: 0x0600424D RID: 16973 RVA: 0x006BD340 File Offset: 0x006BB540
			public override void Prepare()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>(TextureAssets.Tile[this.Key.TileType].Name, 1);
				base.PrepareTextureIfNecessary(asset.Value, null);
			}

			// Token: 0x0600424E RID: 16974 RVA: 0x006BD384 File Offset: 0x006BB584
			public override void PrepareShader()
			{
				base.PrepareShader(this.Key.PaintColor, TreePaintSystemData.GetTileSettings(this.Key.TileType, this.Key.TileStyle));
			}

			// Token: 0x040070EB RID: 28907
			public TilePaintSystemV2.TileVariationkey Key;
		}

		// Token: 0x020007E0 RID: 2016
		public class CageTopRenderTargetHolder : TilePaintSystemV2.ARenderTargetHolder
		{
			// Token: 0x06004250 RID: 16976 RVA: 0x006BD3B4 File Offset: 0x006BB5B4
			public override void Prepare()
			{
				base.PrepareTextureIfNecessary(TextureAssets.CageTop[this.Key.CageStyle].Value, null);
			}

			// Token: 0x06004251 RID: 16977 RVA: 0x006BD3E6 File Offset: 0x006BB5E6
			public override void PrepareShader()
			{
				base.PrepareShader(this.Key.PaintColor, TreePaintSystemData.GetCageTopSettings());
			}

			// Token: 0x040070EC RID: 28908
			public TilePaintSystemV2.CageTopVariationkey Key;
		}

		// Token: 0x020007E1 RID: 2017
		public class WallRenderTargetHolder : TilePaintSystemV2.ARenderTargetHolder
		{
			// Token: 0x06004253 RID: 16979 RVA: 0x006BD400 File Offset: 0x006BB600
			public override void Prepare()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>(TextureAssets.Wall[this.Key.WallType].Name, 1);
				base.PrepareTextureIfNecessary(asset.Value, null);
			}

			// Token: 0x06004254 RID: 16980 RVA: 0x006BD444 File Offset: 0x006BB644
			public override void PrepareShader()
			{
				base.PrepareShader(this.Key.PaintColor, TreePaintSystemData.GetWallSettings(this.Key.WallType));
			}

			// Token: 0x040070ED RID: 28909
			public TilePaintSystemV2.WallVariationKey Key;
		}

		// Token: 0x020007E2 RID: 2018
		public struct TileVariationkey : IEquatable<TilePaintSystemV2.TileVariationkey>
		{
			// Token: 0x06004256 RID: 16982 RVA: 0x006BD467 File Offset: 0x006BB667
			public bool Equals(TilePaintSystemV2.TileVariationkey other)
			{
				return this.TileType == other.TileType && this.TileStyle == other.TileStyle && this.PaintColor == other.PaintColor;
			}

			// Token: 0x06004257 RID: 16983 RVA: 0x006BD495 File Offset: 0x006BB695
			public override bool Equals(object obj)
			{
				return obj is TilePaintSystemV2.TileVariationkey && this.Equals((TilePaintSystemV2.TileVariationkey)obj);
			}

			// Token: 0x06004258 RID: 16984 RVA: 0x006BD4AD File Offset: 0x006BB6AD
			public override int GetHashCode()
			{
				return (this.TileType * 397 ^ this.TileStyle) * 397 ^ this.PaintColor;
			}

			// Token: 0x06004259 RID: 16985 RVA: 0x006BD4CF File Offset: 0x006BB6CF
			public static bool operator ==(TilePaintSystemV2.TileVariationkey left, TilePaintSystemV2.TileVariationkey right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600425A RID: 16986 RVA: 0x006BD4D9 File Offset: 0x006BB6D9
			public static bool operator !=(TilePaintSystemV2.TileVariationkey left, TilePaintSystemV2.TileVariationkey right)
			{
				return !left.Equals(right);
			}

			// Token: 0x040070EE RID: 28910
			public int TileType;

			// Token: 0x040070EF RID: 28911
			public int TileStyle;

			// Token: 0x040070F0 RID: 28912
			public int PaintColor;
		}

		// Token: 0x020007E3 RID: 2019
		public struct WallVariationKey : IEquatable<TilePaintSystemV2.WallVariationKey>
		{
			// Token: 0x0600425B RID: 16987 RVA: 0x006BD4E6 File Offset: 0x006BB6E6
			public bool Equals(TilePaintSystemV2.WallVariationKey other)
			{
				return this.WallType == other.WallType && this.PaintColor == other.PaintColor;
			}

			// Token: 0x0600425C RID: 16988 RVA: 0x006BD506 File Offset: 0x006BB706
			public override bool Equals(object obj)
			{
				return obj is TilePaintSystemV2.WallVariationKey && this.Equals((TilePaintSystemV2.WallVariationKey)obj);
			}

			// Token: 0x0600425D RID: 16989 RVA: 0x006BD51E File Offset: 0x006BB71E
			public override int GetHashCode()
			{
				return this.WallType * 397 ^ this.PaintColor;
			}

			// Token: 0x0600425E RID: 16990 RVA: 0x006BD533 File Offset: 0x006BB733
			public static bool operator ==(TilePaintSystemV2.WallVariationKey left, TilePaintSystemV2.WallVariationKey right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600425F RID: 16991 RVA: 0x006BD53D File Offset: 0x006BB73D
			public static bool operator !=(TilePaintSystemV2.WallVariationKey left, TilePaintSystemV2.WallVariationKey right)
			{
				return !left.Equals(right);
			}

			// Token: 0x040070F1 RID: 28913
			public int WallType;

			// Token: 0x040070F2 RID: 28914
			public int PaintColor;
		}

		// Token: 0x020007E4 RID: 2020
		public struct TreeFoliageVariantKey : IEquatable<TilePaintSystemV2.TreeFoliageVariantKey>
		{
			// Token: 0x06004260 RID: 16992 RVA: 0x006BD54A File Offset: 0x006BB74A
			public bool Equals(TilePaintSystemV2.TreeFoliageVariantKey other)
			{
				return this.TextureIndex == other.TextureIndex && this.TextureStyle == other.TextureStyle && this.PaintColor == other.PaintColor;
			}

			// Token: 0x06004261 RID: 16993 RVA: 0x006BD578 File Offset: 0x006BB778
			public override bool Equals(object obj)
			{
				return obj is TilePaintSystemV2.TreeFoliageVariantKey && this.Equals((TilePaintSystemV2.TreeFoliageVariantKey)obj);
			}

			// Token: 0x06004262 RID: 16994 RVA: 0x006BD590 File Offset: 0x006BB790
			public override int GetHashCode()
			{
				return (this.TextureIndex * 397 ^ this.TextureStyle) * 397 ^ this.PaintColor;
			}

			// Token: 0x06004263 RID: 16995 RVA: 0x006BD5B2 File Offset: 0x006BB7B2
			public static bool operator ==(TilePaintSystemV2.TreeFoliageVariantKey left, TilePaintSystemV2.TreeFoliageVariantKey right)
			{
				return left.Equals(right);
			}

			// Token: 0x06004264 RID: 16996 RVA: 0x006BD5BC File Offset: 0x006BB7BC
			public static bool operator !=(TilePaintSystemV2.TreeFoliageVariantKey left, TilePaintSystemV2.TreeFoliageVariantKey right)
			{
				return !left.Equals(right);
			}

			// Token: 0x040070F3 RID: 28915
			public int TextureIndex;

			// Token: 0x040070F4 RID: 28916
			public int TextureStyle;

			// Token: 0x040070F5 RID: 28917
			public int PaintColor;
		}

		// Token: 0x020007E5 RID: 2021
		public struct CageTopVariationkey
		{
			// Token: 0x06004265 RID: 16997 RVA: 0x006BD5C9 File Offset: 0x006BB7C9
			public bool Equals(TilePaintSystemV2.CageTopVariationkey other)
			{
				return this.CageStyle == other.CageStyle && this.PaintColor == other.PaintColor;
			}

			// Token: 0x06004266 RID: 16998 RVA: 0x006BD5E9 File Offset: 0x006BB7E9
			public override bool Equals(object obj)
			{
				return obj is TilePaintSystemV2.CageTopVariationkey && this.Equals((TilePaintSystemV2.CageTopVariationkey)obj);
			}

			// Token: 0x06004267 RID: 16999 RVA: 0x006BD601 File Offset: 0x006BB801
			public override int GetHashCode()
			{
				return this.CageStyle * 397 ^ this.PaintColor;
			}

			// Token: 0x06004268 RID: 17000 RVA: 0x006BD616 File Offset: 0x006BB816
			public static bool operator ==(TilePaintSystemV2.CageTopVariationkey left, TilePaintSystemV2.CageTopVariationkey right)
			{
				return left.Equals(right);
			}

			// Token: 0x06004269 RID: 17001 RVA: 0x006BD620 File Offset: 0x006BB820
			public static bool operator !=(TilePaintSystemV2.CageTopVariationkey left, TilePaintSystemV2.CageTopVariationkey right)
			{
				return !left.Equals(right);
			}

			// Token: 0x040070F6 RID: 28918
			public int CageStyle;

			// Token: 0x040070F7 RID: 28919
			public int PaintColor;
		}
	}
}
