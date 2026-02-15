using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Generation.Dungeon;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A4 RID: 164
	public static class Actions
	{
		// Token: 0x0600172A RID: 5930 RVA: 0x004DD160 File Offset: 0x004DB360
		public static GenAction Chain(params GenAction[] actions)
		{
			for (int i = 0; i < actions.Length - 1; i++)
			{
				actions[i].NextAction = actions[i + 1];
			}
			return actions[0];
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x004DD18D File Offset: 0x004DB38D
		public static GenAction Continue(GenAction action)
		{
			return new Actions.ContinueWrapper(action);
		}

		// Token: 0x0200068F RID: 1679
		public class ContinueWrapper : GenAction
		{
			// Token: 0x06003E83 RID: 16003 RVA: 0x00696B73 File Offset: 0x00694D73
			public ContinueWrapper(GenAction action)
			{
				this._action = action;
			}

			// Token: 0x06003E84 RID: 16004 RVA: 0x00696B82 File Offset: 0x00694D82
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				this._action.Apply(origin, x, y, args);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006736 RID: 26422
			private GenAction _action;
		}

		// Token: 0x02000690 RID: 1680
		public class Count : GenAction
		{
			// Token: 0x06003E85 RID: 16005 RVA: 0x00696BA0 File Offset: 0x00694DA0
			public Count(Ref<int> count)
			{
				this._count = count;
			}

			// Token: 0x06003E86 RID: 16006 RVA: 0x00696BAF File Offset: 0x00694DAF
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				this._count.Value++;
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006737 RID: 26423
			private Ref<int> _count;
		}

		// Token: 0x02000691 RID: 1681
		public class Scanner : GenAction
		{
			// Token: 0x06003E87 RID: 16007 RVA: 0x00696BCF File Offset: 0x00694DCF
			public Scanner(Ref<int> count)
			{
				this._count = count;
			}

			// Token: 0x06003E88 RID: 16008 RVA: 0x00696BDE File Offset: 0x00694DDE
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				this._count.Value++;
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006738 RID: 26424
			private Ref<int> _count;
		}

		// Token: 0x02000692 RID: 1682
		public class TileScanner : GenAction
		{
			// Token: 0x06003E89 RID: 16009 RVA: 0x00696C00 File Offset: 0x00694E00
			public TileScanner(params ushort[] tiles)
			{
				this._tileIds = tiles;
				this._tileCounts = new Dictionary<ushort, int>();
				for (int i = 0; i < tiles.Length; i++)
				{
					this._tileCounts[this._tileIds[i]] = 0;
				}
			}

			// Token: 0x06003E8A RID: 16010 RVA: 0x00696C48 File Offset: 0x00694E48
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile tile = GenBase._tiles[x, y];
				if (tile.active() && this._tileCounts.ContainsKey(tile.type))
				{
					Dictionary<ushort, int> tileCounts = this._tileCounts;
					ushort type = tile.type;
					int num = tileCounts[type];
					tileCounts[type] = num + 1;
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x06003E8B RID: 16011 RVA: 0x00696CA8 File Offset: 0x00694EA8
			public Actions.TileScanner Output(Dictionary<ushort, int> resultsOutput)
			{
				this._tileCounts = resultsOutput;
				for (int i = 0; i < this._tileIds.Length; i++)
				{
					if (!this._tileCounts.ContainsKey(this._tileIds[i]))
					{
						this._tileCounts[this._tileIds[i]] = 0;
					}
				}
				return this;
			}

			// Token: 0x06003E8C RID: 16012 RVA: 0x00696CF9 File Offset: 0x00694EF9
			public Dictionary<ushort, int> GetResults()
			{
				return this._tileCounts;
			}

			// Token: 0x06003E8D RID: 16013 RVA: 0x00696D01 File Offset: 0x00694F01
			public int GetCount(ushort tileId)
			{
				if (!this._tileCounts.ContainsKey(tileId))
				{
					return -1;
				}
				return this._tileCounts[tileId];
			}

			// Token: 0x04006739 RID: 26425
			private ushort[] _tileIds;

			// Token: 0x0400673A RID: 26426
			private Dictionary<ushort, int> _tileCounts;
		}

		// Token: 0x02000693 RID: 1683
		public class Blank : GenAction
		{
			// Token: 0x06003E8E RID: 16014 RVA: 0x00696D1F File Offset: 0x00694F1F
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				return base.UnitApply(origin, x, y, args);
			}
		}

		// Token: 0x02000694 RID: 1684
		public class Custom : GenAction
		{
			// Token: 0x06003E90 RID: 16016 RVA: 0x00696D2C File Offset: 0x00694F2C
			public Custom(GenBase.CustomPerUnitAction perUnit)
			{
				this._perUnit = perUnit;
			}

			// Token: 0x06003E91 RID: 16017 RVA: 0x00696D3B File Offset: 0x00694F3B
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				return this._perUnit(x, y, args) | base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400673B RID: 26427
			private GenBase.CustomPerUnitAction _perUnit;
		}

		// Token: 0x02000695 RID: 1685
		public class ClearMetadata : GenAction
		{
			// Token: 0x06003E92 RID: 16018 RVA: 0x00696D58 File Offset: 0x00694F58
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].ClearMetadata();
				return base.UnitApply(origin, x, y, args);
			}
		}

		// Token: 0x02000696 RID: 1686
		public class Clear : GenAction
		{
			// Token: 0x06003E94 RID: 16020 RVA: 0x00696D76 File Offset: 0x00694F76
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].ClearEverything();
				return base.UnitApply(origin, x, y, args);
			}
		}

		// Token: 0x02000697 RID: 1687
		public class ClearTile : GenAction
		{
			// Token: 0x06003E96 RID: 16022 RVA: 0x00696D94 File Offset: 0x00694F94
			public ClearTile(bool frameNeighbors = false)
			{
				this._frameNeighbors = frameNeighbors;
			}

			// Token: 0x06003E97 RID: 16023 RVA: 0x00696DA3 File Offset: 0x00694FA3
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldUtils.ClearTile(x, y, this._frameNeighbors);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400673C RID: 26428
			private bool _frameNeighbors;
		}

		// Token: 0x02000698 RID: 1688
		public class ClearWall : GenAction
		{
			// Token: 0x06003E98 RID: 16024 RVA: 0x00696DBD File Offset: 0x00694FBD
			public ClearWall(bool frameNeighbors = false)
			{
				this._frameNeighbors = frameNeighbors;
			}

			// Token: 0x06003E99 RID: 16025 RVA: 0x00696DCC File Offset: 0x00694FCC
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldUtils.ClearWall(x, y, this._frameNeighbors);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400673D RID: 26429
			private bool _frameNeighbors;
		}

		// Token: 0x02000699 RID: 1689
		public class HalfBlock : GenAction
		{
			// Token: 0x06003E9A RID: 16026 RVA: 0x00696DE6 File Offset: 0x00694FE6
			public HalfBlock(bool value = true)
			{
				this._value = value;
			}

			// Token: 0x06003E9B RID: 16027 RVA: 0x00696DF5 File Offset: 0x00694FF5
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].halfBrick(this._value);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400673E RID: 26430
			private bool _value;
		}

		// Token: 0x0200069A RID: 1690
		public class SetTile : GenAction
		{
			// Token: 0x06003E9C RID: 16028 RVA: 0x00696E19 File Offset: 0x00695019
			public SetTile(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true, bool clearTile = true)
			{
				this._type = type;
				this._doFraming = setSelfFrames;
				this._doNeighborFraming = setNeighborFrames;
				this._clearTile = clearTile;
			}

			// Token: 0x06003E9D RID: 16029 RVA: 0x00696E40 File Offset: 0x00695040
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile tile = GenBase._tiles[x, y];
				if (this._clearTile)
				{
					tile.Clear(~(TileDataType.Wiring | TileDataType.Actuator));
				}
				tile.type = this._type;
				tile.active(true);
				if (this._doFraming)
				{
					WorldUtils.TileFrame(x, y, this._doNeighborFraming);
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400673F RID: 26431
			private ushort _type;

			// Token: 0x04006740 RID: 26432
			private bool _doFraming;

			// Token: 0x04006741 RID: 26433
			private bool _doNeighborFraming;

			// Token: 0x04006742 RID: 26434
			private bool _clearTile;
		}

		// Token: 0x0200069B RID: 1691
		public class SetWall : GenAction
		{
			// Token: 0x06003E9E RID: 16030 RVA: 0x00696E9D File Offset: 0x0069509D
			public SetWall(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true, bool clearTile = true)
			{
				this._type = type;
				this._doFraming = setSelfFrames;
				this._doNeighborFraming = setNeighborFrames;
				this._clearTile = clearTile;
			}

			// Token: 0x06003E9F RID: 16031 RVA: 0x00696EC4 File Offset: 0x006950C4
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile tile = GenBase._tiles[x, y];
				if (this._clearTile)
				{
					tile.Clear(~(TileDataType.Wiring | TileDataType.Actuator));
				}
				tile.wall = this._type;
				if (this._doFraming)
				{
					WorldUtils.WallFrame(x, y, this._doNeighborFraming);
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006743 RID: 26435
			private ushort _type;

			// Token: 0x04006744 RID: 26436
			private bool _doFraming;

			// Token: 0x04006745 RID: 26437
			private bool _doNeighborFraming;

			// Token: 0x04006746 RID: 26438
			private bool _clearTile;
		}

		// Token: 0x0200069C RID: 1692
		public class SetTileKeepWall : GenAction
		{
			// Token: 0x06003EA0 RID: 16032 RVA: 0x00696F1A File Offset: 0x0069511A
			public SetTileKeepWall(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true)
			{
				this._type = type;
				this._doFraming = setSelfFrames;
				this._doNeighborFraming = setNeighborFrames;
			}

			// Token: 0x06003EA1 RID: 16033 RVA: 0x00696F38 File Offset: 0x00695138
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				ushort wall = GenBase._tiles[x, y].wall;
				int wallFrameX = GenBase._tiles[x, y].wallFrameX();
				int wallFrameY = GenBase._tiles[x, y].wallFrameY();
				GenBase._tiles[x, y].Clear(~(TileDataType.Wiring | TileDataType.Actuator));
				GenBase._tiles[x, y].type = this._type;
				GenBase._tiles[x, y].active(true);
				if (wall > 0)
				{
					GenBase._tiles[x, y].wall = wall;
					GenBase._tiles[x, y].wallFrameX(wallFrameX);
					GenBase._tiles[x, y].wallFrameY(wallFrameY);
				}
				if (this._doFraming)
				{
					WorldUtils.TileFrame(x, y, this._doNeighborFraming);
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006747 RID: 26439
			private ushort _type;

			// Token: 0x04006748 RID: 26440
			private bool _doFraming;

			// Token: 0x04006749 RID: 26441
			private bool _doNeighborFraming;
		}

		// Token: 0x0200069D RID: 1693
		public class UpdateBounds : GenAction
		{
			// Token: 0x06003EA2 RID: 16034 RVA: 0x00697011 File Offset: 0x00695211
			public UpdateBounds(DungeonBounds bounds)
			{
				this._bounds = bounds;
			}

			// Token: 0x06003EA3 RID: 16035 RVA: 0x00697020 File Offset: 0x00695220
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				this._bounds.UpdateBounds(x, y);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400674A RID: 26442
			private DungeonBounds _bounds;
		}

		// Token: 0x0200069E RID: 1694
		public class DebugDraw : GenAction
		{
			// Token: 0x06003EA4 RID: 16036 RVA: 0x0069703A File Offset: 0x0069523A
			public DebugDraw(SpriteBatch spriteBatch, Color color = default(Color))
			{
				this._spriteBatch = spriteBatch;
				this._color = color;
			}

			// Token: 0x06003EA5 RID: 16037 RVA: 0x00697050 File Offset: 0x00695250
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				this._spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((x << 4) - (int)Main.screenPosition.X, (y << 4) - (int)Main.screenPosition.Y, 16, 16), this._color);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400674B RID: 26443
			private Color _color;

			// Token: 0x0400674C RID: 26444
			private SpriteBatch _spriteBatch;
		}

		// Token: 0x0200069F RID: 1695
		public class SetSlope : GenAction
		{
			// Token: 0x06003EA6 RID: 16038 RVA: 0x006970AA File Offset: 0x006952AA
			public SetSlope(int slope)
			{
				this._slope = slope;
			}

			// Token: 0x06003EA7 RID: 16039 RVA: 0x006970B9 File Offset: 0x006952B9
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldGen.SlopeTile(x, y, this._slope, false, true);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400674D RID: 26445
			private int _slope;
		}

		// Token: 0x020006A0 RID: 1696
		public class SetHalfTile : GenAction
		{
			// Token: 0x06003EA8 RID: 16040 RVA: 0x006970D6 File Offset: 0x006952D6
			public SetHalfTile(bool halfTile)
			{
				this._halfTile = halfTile;
			}

			// Token: 0x06003EA9 RID: 16041 RVA: 0x006970E5 File Offset: 0x006952E5
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].halfBrick(this._halfTile);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400674E RID: 26446
			private bool _halfTile;
		}

		// Token: 0x020006A1 RID: 1697
		public class SetTilePaint : GenAction
		{
			// Token: 0x06003EAA RID: 16042 RVA: 0x00697109 File Offset: 0x00695309
			public SetTilePaint(byte paintID)
			{
				this.paintID = paintID;
			}

			// Token: 0x06003EAB RID: 16043 RVA: 0x00697118 File Offset: 0x00695318
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (this.paintID == 0)
				{
					return base.Fail();
				}
				GenBase._tiles[x, y].color(this.paintID);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400674F RID: 26447
			private byte paintID;
		}

		// Token: 0x020006A2 RID: 1698
		public class ClearTilePaint : GenAction
		{
			// Token: 0x06003EAD RID: 16045 RVA: 0x0069714B File Offset: 0x0069534B
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].color(0);
				return base.UnitApply(origin, x, y, args);
			}
		}

		// Token: 0x020006A3 RID: 1699
		public class SetWallPaint : GenAction
		{
			// Token: 0x06003EAE RID: 16046 RVA: 0x0069716A File Offset: 0x0069536A
			public SetWallPaint(byte paintID)
			{
				this.paintID = paintID;
			}

			// Token: 0x06003EAF RID: 16047 RVA: 0x00697179 File Offset: 0x00695379
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (this.paintID == 0)
				{
					return base.Fail();
				}
				GenBase._tiles[x, y].wallColor(this.paintID);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006750 RID: 26448
			private byte paintID;
		}

		// Token: 0x020006A4 RID: 1700
		public class ClearWallPaint : GenAction
		{
			// Token: 0x06003EB1 RID: 16049 RVA: 0x006971AC File Offset: 0x006953AC
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].wallColor(0);
				return base.UnitApply(origin, x, y, args);
			}
		}

		// Token: 0x020006A5 RID: 1701
		public class SetTileAndWallPaint : GenAction
		{
			// Token: 0x06003EB2 RID: 16050 RVA: 0x006971CB File Offset: 0x006953CB
			public SetTileAndWallPaint(byte paintID)
			{
				this.paintID = paintID;
			}

			// Token: 0x06003EB3 RID: 16051 RVA: 0x006971DC File Offset: 0x006953DC
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (this.paintID == 0)
				{
					return base.Fail();
				}
				if (GenBase._tiles[x, y].active())
				{
					GenBase._tiles[x, y].color(this.paintID);
				}
				if (GenBase._tiles[x, y].wall != 0)
				{
					GenBase._tiles[x, y].wallColor(this.paintID);
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006751 RID: 26449
			private byte paintID;
		}

		// Token: 0x020006A6 RID: 1702
		public class ClearTileAndWallPaint : GenAction
		{
			// Token: 0x06003EB5 RID: 16053 RVA: 0x00697257 File Offset: 0x00695457
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].color(0);
				GenBase._tiles[x, y].wallColor(0);
				return base.UnitApply(origin, x, y, args);
			}
		}

		// Token: 0x020006A7 RID: 1703
		public class SetTileAndWallRainbowPaint : GenAction
		{
			// Token: 0x06003EB7 RID: 16055 RVA: 0x00697288 File Offset: 0x00695488
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				byte rainbowPaintIDForPosition = WorldGen.GetRainbowPaintIDForPosition(x, y, false);
				if (GenBase._tiles[x, y].active())
				{
					GenBase._tiles[x, y].color(rainbowPaintIDForPosition);
				}
				if (GenBase._tiles[x, y].wall != 0)
				{
					GenBase._tiles[x, y].wallColor(rainbowPaintIDForPosition);
				}
				return base.UnitApply(origin, x, y, args);
			}
		}

		// Token: 0x020006A8 RID: 1704
		public class PlaceTile : GenAction
		{
			// Token: 0x06003EB8 RID: 16056 RVA: 0x006972F3 File Offset: 0x006954F3
			public PlaceTile(ushort type, int style = 0)
			{
				this._type = type;
				this._style = style;
			}

			// Token: 0x06003EB9 RID: 16057 RVA: 0x00697309 File Offset: 0x00695509
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldGen.PlaceTile(x, y, (int)this._type, true, false, -1, this._style);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006752 RID: 26450
			private ushort _type;

			// Token: 0x04006753 RID: 26451
			private int _style;
		}

		// Token: 0x020006A9 RID: 1705
		public class RemoveWall : GenAction
		{
			// Token: 0x06003EBA RID: 16058 RVA: 0x0069732D File Offset: 0x0069552D
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].wall = 0;
				return base.UnitApply(origin, x, y, args);
			}
		}

		// Token: 0x020006AA RID: 1706
		public class PlaceWall : GenAction
		{
			// Token: 0x06003EBC RID: 16060 RVA: 0x0069734C File Offset: 0x0069554C
			public PlaceWall(ushort type, bool neighbors = true)
			{
				this._type = type;
				this._neighbors = neighbors;
			}

			// Token: 0x06003EBD RID: 16061 RVA: 0x00697364 File Offset: 0x00695564
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].wall = this._type;
				WorldGen.SquareWallFrame(x, y, true);
				if (this._neighbors)
				{
					WorldGen.SquareWallFrame(x + 1, y, true);
					WorldGen.SquareWallFrame(x - 1, y, true);
					WorldGen.SquareWallFrame(x, y - 1, true);
					WorldGen.SquareWallFrame(x, y + 1, true);
				}
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006754 RID: 26452
			private ushort _type;

			// Token: 0x04006755 RID: 26453
			private bool _neighbors;
		}

		// Token: 0x020006AB RID: 1707
		public class SetLiquid : GenAction
		{
			// Token: 0x06003EBE RID: 16062 RVA: 0x006973CB File Offset: 0x006955CB
			public SetLiquid(int type = 0, byte value = 255)
			{
				this._value = value;
				this._type = type;
			}

			// Token: 0x06003EBF RID: 16063 RVA: 0x006973E1 File Offset: 0x006955E1
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].liquidType(this._type);
				GenBase._tiles[x, y].liquid = this._value;
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006756 RID: 26454
			private int _type;

			// Token: 0x04006757 RID: 26455
			private byte _value;
		}

		// Token: 0x020006AC RID: 1708
		public class SwapSolidTile : GenAction
		{
			// Token: 0x06003EC0 RID: 16064 RVA: 0x0069741C File Offset: 0x0069561C
			public SwapSolidTile(ushort type)
			{
				this._type = type;
			}

			// Token: 0x06003EC1 RID: 16065 RVA: 0x0069742C File Offset: 0x0069562C
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile tile = GenBase._tiles[x, y];
				if (WorldGen.SolidTile(tile))
				{
					tile.ResetToType(this._type);
					return base.UnitApply(origin, x, y, args);
				}
				return base.Fail();
			}

			// Token: 0x04006758 RID: 26456
			private ushort _type;
		}

		// Token: 0x020006AD RID: 1709
		public class SetFrames : GenAction
		{
			// Token: 0x06003EC2 RID: 16066 RVA: 0x0069746C File Offset: 0x0069566C
			public SetFrames(bool frameNeighbors = false)
			{
				this._frameNeighbors = frameNeighbors;
			}

			// Token: 0x06003EC3 RID: 16067 RVA: 0x0069747B File Offset: 0x0069567B
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldUtils.TileFrame(x, y, this._frameNeighbors);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x04006759 RID: 26457
			private bool _frameNeighbors;
		}

		// Token: 0x020006AE RID: 1710
		public class Smooth : GenAction
		{
			// Token: 0x06003EC4 RID: 16068 RVA: 0x00697495 File Offset: 0x00695695
			public Smooth(bool applyToNeighbors = false)
			{
				this._applyToNeighbors = applyToNeighbors;
			}

			// Token: 0x06003EC5 RID: 16069 RVA: 0x006974A4 File Offset: 0x006956A4
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile.SmoothSlope(x, y, this._applyToNeighbors, false);
				return base.UnitApply(origin, x, y, args);
			}

			// Token: 0x0400675A RID: 26458
			private bool _applyToNeighbors;
		}
	}
}
