using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.Modules;

namespace Terraria.ObjectData
{
	// Token: 0x0200005D RID: 93
	public class TileObjectData
	{
		// Token: 0x060013C9 RID: 5065 RVA: 0x004AE980 File Offset: 0x004ACB80
		public TileObjectData(TileObjectData copyFrom = null)
		{
			this._parent = null;
			this._linkedAlternates = false;
			if (copyFrom == null)
			{
				this._usesCustomCanPlace = false;
				this._alternates = null;
				this._anchor = null;
				this._anchorTiles = null;
				this._tileObjectBase = null;
				this._liquidDeath = null;
				this._liquidPlacement = null;
				this._placementHooks = null;
				this._tileObjectDraw = null;
				this._tileObjectStyle = null;
				this._tileObjectCoords = null;
				return;
			}
			this.CopyFrom(copyFrom);
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x004AE9FC File Offset: 0x004ACBFC
		public void CopyFrom(TileObjectData copy)
		{
			if (copy == null)
			{
				return;
			}
			this._usesCustomCanPlace = copy._usesCustomCanPlace;
			this._alternates = copy._alternates;
			this._anchor = copy._anchor;
			this._anchorTiles = copy._anchorTiles;
			this._tileObjectBase = copy._tileObjectBase;
			this._liquidDeath = copy._liquidDeath;
			this._liquidPlacement = copy._liquidPlacement;
			this._placementHooks = copy._placementHooks;
			this._tileObjectDraw = copy._tileObjectDraw;
			this._tileObjectStyle = copy._tileObjectStyle;
			this._tileObjectCoords = copy._tileObjectCoords;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x004AEA91 File Offset: 0x004ACC91
		public void FullCopyFrom(ushort tileType)
		{
			this.FullCopyFrom(TileObjectData.GetTileData((int)tileType, 0, 0));
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x004AEAA4 File Offset: 0x004ACCA4
		public void FullCopyFrom(TileObjectData copy)
		{
			if (copy == null)
			{
				return;
			}
			this._usesCustomCanPlace = copy._usesCustomCanPlace;
			this._alternates = copy._alternates;
			this._anchor = copy._anchor;
			this._anchorTiles = copy._anchorTiles;
			this._tileObjectBase = copy._tileObjectBase;
			this._liquidDeath = copy._liquidDeath;
			this._liquidPlacement = copy._liquidPlacement;
			this._placementHooks = copy._placementHooks;
			this._tileObjectDraw = copy._tileObjectDraw;
			this._tileObjectStyle = copy._tileObjectStyle;
			this._tileObjectCoords = copy._tileObjectCoords;
			this._subTiles = new TileObjectSubTilesModule(copy._subTiles, null);
			this._hasOwnSubTiles = true;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x004AEB54 File Offset: 0x004ACD54
		private void SetupBaseObject()
		{
			this._alternates = new TileObjectAlternatesModule(null);
			this._hasOwnAlternates = true;
			this.Alternates = new List<TileObjectData>();
			this._anchor = new AnchorDataModule(null);
			this._hasOwnAnchor = true;
			this.AnchorTop = default(AnchorData);
			this.AnchorBottom = default(AnchorData);
			this.AnchorLeft = default(AnchorData);
			this.AnchorRight = default(AnchorData);
			this.AnchorWall = false;
			this._anchorTiles = new AnchorTypesModule(null);
			this._hasOwnAnchorTiles = true;
			this.AnchorValidTiles = null;
			this.AnchorInvalidTiles = null;
			this.AnchorAlternateTiles = null;
			this.AnchorValidWalls = null;
			this._liquidDeath = new LiquidDeathModule(null);
			this._hasOwnLiquidDeath = true;
			this.WaterDeath = false;
			this.LavaDeath = false;
			this._liquidPlacement = new LiquidPlacementModule(null);
			this._hasOwnLiquidPlacement = true;
			this.WaterPlacement = LiquidPlacement.Allowed;
			this.LavaPlacement = LiquidPlacement.NotAllowed;
			this._placementHooks = new TilePlacementHooksModule(null);
			this._hasOwnPlacementHooks = true;
			this.HookCheckIfCanPlace = default(PlacementHook);
			this.HookPostPlaceEveryone = default(PlacementHook);
			this.HookPostPlaceMyPlayer = default(PlacementHook);
			this.HookPlaceOverride = default(PlacementHook);
			this.SubTiles = new List<TileObjectData>((int)TileID.Count);
			this._tileObjectBase = new TileObjectBaseModule(null);
			this._hasOwnTileObjectBase = true;
			this.Width = 1;
			this.Height = 1;
			this.Origin = Point16.Zero;
			this.Direction = TileObjectDirection.None;
			this.RandomStyleRange = 0;
			this.FlattenAnchors = false;
			this._tileObjectCoords = new TileObjectCoordinatesModule(null, null, null);
			this._hasOwnTileObjectCoords = true;
			this.CoordinateHeights = new int[]
			{
				16
			};
			this.CoordinateWidth = 0;
			this.CoordinatePadding = 0;
			this.CoordinatePaddingFix = Point16.Zero;
			this._tileObjectDraw = new TileObjectDrawModule(null);
			this._hasOwnTileObjectDraw = true;
			this.DrawYOffset = 0;
			this.DrawFlipHorizontal = false;
			this.DrawFlipVertical = false;
			this.DrawStepDown = 0;
			this._tileObjectStyle = new TileObjectStyleModule(null);
			this._hasOwnTileObjectStyle = true;
			this.Style = 0;
			this.StyleHorizontal = false;
			this.StyleWrapLimit = 0;
			this.StyleMultiplier = 1;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x004AED84 File Offset: 0x004ACF84
		private void Calculate()
		{
			if (this._tileObjectCoords.calculated)
			{
				return;
			}
			this._tileObjectCoords.calculated = true;
			this._tileObjectCoords.styleWidth = (this._tileObjectCoords.width + this._tileObjectCoords.padding) * this.Width + (int)this._tileObjectCoords.paddingFix.X;
			int num = 0;
			this._tileObjectCoords.styleHeight = 0;
			for (int i = 0; i < this._tileObjectCoords.heights.Length; i++)
			{
				num += this._tileObjectCoords.heights[i] + this._tileObjectCoords.padding;
			}
			num += (int)this._tileObjectCoords.paddingFix.Y;
			this._tileObjectCoords.styleHeight = num;
			if (this._hasOwnLiquidDeath)
			{
				if (this._liquidDeath.lava)
				{
					this.LavaPlacement = LiquidPlacement.NotAllowed;
				}
				if (this._liquidDeath.water)
				{
					this.WaterPlacement = LiquidPlacement.NotAllowed;
				}
			}
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x004AEE76 File Offset: 0x004AD076
		private void WriteCheck()
		{
			if (TileObjectData.readOnlyData)
			{
				throw new FieldAccessException("Tile data is locked and only accessible during startup.");
			}
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x004AEE8A File Offset: 0x004AD08A
		private void LockWrites()
		{
			TileObjectData.readOnlyData = true;
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x004AEE92 File Offset: 0x004AD092
		// (set) Token: 0x060013D2 RID: 5074 RVA: 0x004AEE9A File Offset: 0x004AD09A
		private bool LinkedAlternates
		{
			get
			{
				return this._linkedAlternates;
			}
			set
			{
				this.WriteCheck();
				if (value && !this._hasOwnAlternates)
				{
					this._hasOwnAlternates = true;
					this._alternates = new TileObjectAlternatesModule(this._alternates);
				}
				this._linkedAlternates = value;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060013D3 RID: 5075 RVA: 0x004AEECC File Offset: 0x004AD0CC
		// (set) Token: 0x060013D4 RID: 5076 RVA: 0x004AEED4 File Offset: 0x004AD0D4
		public bool UsesCustomCanPlace
		{
			get
			{
				return this._usesCustomCanPlace;
			}
			set
			{
				this.WriteCheck();
				this._usesCustomCanPlace = value;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x004AEEE3 File Offset: 0x004AD0E3
		// (set) Token: 0x060013D6 RID: 5078 RVA: 0x004AEF03 File Offset: 0x004AD103
		private List<TileObjectData> Alternates
		{
			get
			{
				if (this._alternates == null)
				{
					return TileObjectData._baseObject.Alternates;
				}
				return this._alternates.data;
			}
			set
			{
				if (!this._hasOwnAlternates)
				{
					this._hasOwnAlternates = true;
					this._alternates = new TileObjectAlternatesModule(this._alternates);
				}
				this._alternates.data = value;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060013D7 RID: 5079 RVA: 0x004AEF31 File Offset: 0x004AD131
		// (set) Token: 0x060013D8 RID: 5080 RVA: 0x004AEF54 File Offset: 0x004AD154
		public AnchorData AnchorTop
		{
			get
			{
				if (this._anchor == null)
				{
					return TileObjectData._baseObject.AnchorTop;
				}
				return this._anchor.top;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnAnchor)
				{
					if (this._anchor.top == value)
					{
						return;
					}
					this._hasOwnAnchor = true;
					this._anchor = new AnchorDataModule(this._anchor);
				}
				this._anchor.top = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].AnchorTop = value;
					}
				}
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x004AEFE1 File Offset: 0x004AD1E1
		// (set) Token: 0x060013DA RID: 5082 RVA: 0x004AF004 File Offset: 0x004AD204
		public AnchorData AnchorBottom
		{
			get
			{
				if (this._anchor == null)
				{
					return TileObjectData._baseObject.AnchorBottom;
				}
				return this._anchor.bottom;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnAnchor)
				{
					if (this._anchor.bottom == value)
					{
						return;
					}
					this._hasOwnAnchor = true;
					this._anchor = new AnchorDataModule(this._anchor);
				}
				this._anchor.bottom = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].AnchorBottom = value;
					}
				}
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x004AF091 File Offset: 0x004AD291
		// (set) Token: 0x060013DC RID: 5084 RVA: 0x004AF0B4 File Offset: 0x004AD2B4
		public AnchorData AnchorLeft
		{
			get
			{
				if (this._anchor == null)
				{
					return TileObjectData._baseObject.AnchorLeft;
				}
				return this._anchor.left;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnAnchor)
				{
					if (this._anchor.left == value)
					{
						return;
					}
					this._hasOwnAnchor = true;
					this._anchor = new AnchorDataModule(this._anchor);
				}
				this._anchor.left = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].AnchorLeft = value;
					}
				}
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x004AF141 File Offset: 0x004AD341
		// (set) Token: 0x060013DE RID: 5086 RVA: 0x004AF164 File Offset: 0x004AD364
		public AnchorData AnchorRight
		{
			get
			{
				if (this._anchor == null)
				{
					return TileObjectData._baseObject.AnchorRight;
				}
				return this._anchor.right;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnAnchor)
				{
					if (this._anchor.right == value)
					{
						return;
					}
					this._hasOwnAnchor = true;
					this._anchor = new AnchorDataModule(this._anchor);
				}
				this._anchor.right = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].AnchorRight = value;
					}
				}
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x004AF1F1 File Offset: 0x004AD3F1
		// (set) Token: 0x060013E0 RID: 5088 RVA: 0x004AF214 File Offset: 0x004AD414
		public bool AnchorWall
		{
			get
			{
				if (this._anchor == null)
				{
					return TileObjectData._baseObject.AnchorWall;
				}
				return this._anchor.wall;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnAnchor)
				{
					if (this._anchor.wall == value)
					{
						return;
					}
					this._hasOwnAnchor = true;
					this._anchor = new AnchorDataModule(this._anchor);
				}
				this._anchor.wall = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].AnchorWall = value;
					}
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x004AF29C File Offset: 0x004AD49C
		// (set) Token: 0x060013E2 RID: 5090 RVA: 0x004AF2BC File Offset: 0x004AD4BC
		public int[] AnchorValidTiles
		{
			get
			{
				if (this._anchorTiles == null)
				{
					return TileObjectData._baseObject.AnchorValidTiles;
				}
				return this._anchorTiles.tileValid;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnAnchorTiles)
				{
					if (value.deepCompare(this._anchorTiles.tileValid))
					{
						return;
					}
					this._hasOwnAnchorTiles = true;
					this._anchorTiles = new AnchorTypesModule(this._anchorTiles);
				}
				this._anchorTiles.tileValid = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						int[] anchorValidTiles = value;
						if (value != null)
						{
							anchorValidTiles = (int[])value.Clone();
						}
						this._alternates.data[i].AnchorValidTiles = anchorValidTiles;
					}
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x004AF35A File Offset: 0x004AD55A
		// (set) Token: 0x060013E4 RID: 5092 RVA: 0x004AF37C File Offset: 0x004AD57C
		public int[] AnchorInvalidTiles
		{
			get
			{
				if (this._anchorTiles == null)
				{
					return TileObjectData._baseObject.AnchorInvalidTiles;
				}
				return this._anchorTiles.tileInvalid;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnAnchorTiles)
				{
					if (value.deepCompare(this._anchorTiles.tileInvalid))
					{
						return;
					}
					this._hasOwnAnchorTiles = true;
					this._anchorTiles = new AnchorTypesModule(this._anchorTiles);
				}
				this._anchorTiles.tileInvalid = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						int[] anchorInvalidTiles = value;
						if (value != null)
						{
							anchorInvalidTiles = (int[])value.Clone();
						}
						this._alternates.data[i].AnchorInvalidTiles = anchorInvalidTiles;
					}
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x004AF41A File Offset: 0x004AD61A
		// (set) Token: 0x060013E6 RID: 5094 RVA: 0x004AF43C File Offset: 0x004AD63C
		public int[] AnchorAlternateTiles
		{
			get
			{
				if (this._anchorTiles == null)
				{
					return TileObjectData._baseObject.AnchorAlternateTiles;
				}
				return this._anchorTiles.tileAlternates;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnAnchorTiles)
				{
					if (value.deepCompare(this._anchorTiles.tileInvalid))
					{
						return;
					}
					this._hasOwnAnchorTiles = true;
					this._anchorTiles = new AnchorTypesModule(this._anchorTiles);
				}
				this._anchorTiles.tileAlternates = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						int[] anchorAlternateTiles = value;
						if (value != null)
						{
							anchorAlternateTiles = (int[])value.Clone();
						}
						this._alternates.data[i].AnchorAlternateTiles = anchorAlternateTiles;
					}
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x004AF4DA File Offset: 0x004AD6DA
		// (set) Token: 0x060013E8 RID: 5096 RVA: 0x004AF4FC File Offset: 0x004AD6FC
		public int[] AnchorValidWalls
		{
			get
			{
				if (this._anchorTiles == null)
				{
					return TileObjectData._baseObject.AnchorValidWalls;
				}
				return this._anchorTiles.wallValid;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnAnchorTiles)
				{
					this._hasOwnAnchorTiles = true;
					this._anchorTiles = new AnchorTypesModule(this._anchorTiles);
				}
				this._anchorTiles.wallValid = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						int[] anchorValidWalls = value;
						if (value != null)
						{
							anchorValidWalls = (int[])value.Clone();
						}
						this._alternates.data[i].AnchorValidWalls = anchorValidWalls;
					}
				}
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x004AF586 File Offset: 0x004AD786
		// (set) Token: 0x060013EA RID: 5098 RVA: 0x004AF5A8 File Offset: 0x004AD7A8
		public bool WaterDeath
		{
			get
			{
				if (this._liquidDeath == null)
				{
					return TileObjectData._baseObject.WaterDeath;
				}
				return this._liquidDeath.water;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnLiquidDeath)
				{
					if (this._liquidDeath.water == value)
					{
						return;
					}
					this._hasOwnLiquidDeath = true;
					this._liquidDeath = new LiquidDeathModule(this._liquidDeath);
				}
				this._liquidDeath.water = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].WaterDeath = value;
					}
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x004AF630 File Offset: 0x004AD830
		// (set) Token: 0x060013EC RID: 5100 RVA: 0x004AF650 File Offset: 0x004AD850
		public bool LavaDeath
		{
			get
			{
				if (this._liquidDeath == null)
				{
					return TileObjectData._baseObject.LavaDeath;
				}
				return this._liquidDeath.lava;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnLiquidDeath)
				{
					if (this._liquidDeath.lava == value)
					{
						return;
					}
					this._hasOwnLiquidDeath = true;
					this._liquidDeath = new LiquidDeathModule(this._liquidDeath);
				}
				this._liquidDeath.lava = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].LavaDeath = value;
					}
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x004AF6D8 File Offset: 0x004AD8D8
		// (set) Token: 0x060013EE RID: 5102 RVA: 0x004AF6F8 File Offset: 0x004AD8F8
		public LiquidPlacement WaterPlacement
		{
			get
			{
				if (this._liquidPlacement == null)
				{
					return TileObjectData._baseObject.WaterPlacement;
				}
				return this._liquidPlacement.water;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnLiquidPlacement)
				{
					if (this._liquidPlacement.water == value)
					{
						return;
					}
					this._hasOwnLiquidPlacement = true;
					this._liquidPlacement = new LiquidPlacementModule(this._liquidPlacement);
				}
				this._liquidPlacement.water = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].WaterPlacement = value;
					}
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x004AF780 File Offset: 0x004AD980
		// (set) Token: 0x060013F0 RID: 5104 RVA: 0x004AF7A0 File Offset: 0x004AD9A0
		public LiquidPlacement LavaPlacement
		{
			get
			{
				if (this._liquidPlacement == null)
				{
					return TileObjectData._baseObject.LavaPlacement;
				}
				return this._liquidPlacement.lava;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnLiquidPlacement)
				{
					if (this._liquidPlacement.lava == value)
					{
						return;
					}
					this._hasOwnLiquidPlacement = true;
					this._liquidPlacement = new LiquidPlacementModule(this._liquidPlacement);
				}
				this._liquidPlacement.lava = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].LavaPlacement = value;
					}
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x004AF828 File Offset: 0x004ADA28
		// (set) Token: 0x060013F2 RID: 5106 RVA: 0x004AF848 File Offset: 0x004ADA48
		public PlacementHook HookCheckIfCanPlace
		{
			get
			{
				if (this._placementHooks == null)
				{
					return TileObjectData._baseObject.HookCheckIfCanPlace;
				}
				return this._placementHooks.check;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnPlacementHooks)
				{
					this._hasOwnPlacementHooks = true;
					this._placementHooks = new TilePlacementHooksModule(this._placementHooks);
				}
				this._placementHooks.check = value;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x004AF87C File Offset: 0x004ADA7C
		// (set) Token: 0x060013F4 RID: 5108 RVA: 0x004AF89C File Offset: 0x004ADA9C
		public PlacementHook HookPostPlaceEveryone
		{
			get
			{
				if (this._placementHooks == null)
				{
					return TileObjectData._baseObject.HookPostPlaceEveryone;
				}
				return this._placementHooks.postPlaceEveryone;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnPlacementHooks)
				{
					this._hasOwnPlacementHooks = true;
					this._placementHooks = new TilePlacementHooksModule(this._placementHooks);
				}
				this._placementHooks.postPlaceEveryone = value;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x004AF8D0 File Offset: 0x004ADAD0
		// (set) Token: 0x060013F6 RID: 5110 RVA: 0x004AF8F0 File Offset: 0x004ADAF0
		public PlacementHook HookPostPlaceMyPlayer
		{
			get
			{
				if (this._placementHooks == null)
				{
					return TileObjectData._baseObject.HookPostPlaceMyPlayer;
				}
				return this._placementHooks.postPlaceMyPlayer;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnPlacementHooks)
				{
					this._hasOwnPlacementHooks = true;
					this._placementHooks = new TilePlacementHooksModule(this._placementHooks);
				}
				this._placementHooks.postPlaceMyPlayer = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x004AF924 File Offset: 0x004ADB24
		// (set) Token: 0x060013F8 RID: 5112 RVA: 0x004AF944 File Offset: 0x004ADB44
		public PlacementHook HookPlaceOverride
		{
			get
			{
				if (this._placementHooks == null)
				{
					return TileObjectData._baseObject.HookPlaceOverride;
				}
				return this._placementHooks.placeOverride;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnPlacementHooks)
				{
					this._hasOwnPlacementHooks = true;
					this._placementHooks = new TilePlacementHooksModule(this._placementHooks);
				}
				this._placementHooks.placeOverride = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x004AF978 File Offset: 0x004ADB78
		// (set) Token: 0x060013FA RID: 5114 RVA: 0x004AF998 File Offset: 0x004ADB98
		private List<TileObjectData> SubTiles
		{
			get
			{
				if (this._subTiles == null)
				{
					return TileObjectData._baseObject.SubTiles;
				}
				return this._subTiles.data;
			}
			set
			{
				if (!this._hasOwnSubTiles)
				{
					this._hasOwnSubTiles = true;
					this._subTiles = new TileObjectSubTilesModule(null, null);
				}
				if (value == null)
				{
					this._subTiles.data = null;
					return;
				}
				this._subTiles.data = value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x004AF9D2 File Offset: 0x004ADBD2
		// (set) Token: 0x060013FC RID: 5116 RVA: 0x004AF9F0 File Offset: 0x004ADBF0
		public int DrawYOffset
		{
			get
			{
				if (this._tileObjectDraw == null)
				{
					return this.DrawYOffset;
				}
				return this._tileObjectDraw.yOffset;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectDraw)
				{
					if (this._tileObjectDraw.yOffset == value)
					{
						return;
					}
					this._hasOwnTileObjectDraw = true;
					this._tileObjectDraw = new TileObjectDrawModule(this._tileObjectDraw);
				}
				this._tileObjectDraw.yOffset = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].DrawYOffset = value;
					}
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x004AFA78 File Offset: 0x004ADC78
		// (set) Token: 0x060013FE RID: 5118 RVA: 0x004AFA94 File Offset: 0x004ADC94
		public int DrawXOffset
		{
			get
			{
				if (this._tileObjectDraw == null)
				{
					return this.DrawXOffset;
				}
				return this._tileObjectDraw.xOffset;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectDraw)
				{
					if (this._tileObjectDraw.xOffset == value)
					{
						return;
					}
					this._hasOwnTileObjectDraw = true;
					this._tileObjectDraw = new TileObjectDrawModule(this._tileObjectDraw);
				}
				this._tileObjectDraw.xOffset = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].DrawXOffset = value;
					}
				}
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x004AFB1C File Offset: 0x004ADD1C
		// (set) Token: 0x06001400 RID: 5120 RVA: 0x004AFB38 File Offset: 0x004ADD38
		public bool DrawFlipHorizontal
		{
			get
			{
				if (this._tileObjectDraw == null)
				{
					return this.DrawFlipHorizontal;
				}
				return this._tileObjectDraw.flipHorizontal;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectDraw)
				{
					if (this._tileObjectDraw.flipHorizontal == value)
					{
						return;
					}
					this._hasOwnTileObjectDraw = true;
					this._tileObjectDraw = new TileObjectDrawModule(this._tileObjectDraw);
				}
				this._tileObjectDraw.flipHorizontal = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].DrawFlipHorizontal = value;
					}
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x004AFBC0 File Offset: 0x004ADDC0
		// (set) Token: 0x06001402 RID: 5122 RVA: 0x004AFBDC File Offset: 0x004ADDDC
		public bool DrawFlipVertical
		{
			get
			{
				if (this._tileObjectDraw == null)
				{
					return this.DrawFlipVertical;
				}
				return this._tileObjectDraw.flipVertical;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectDraw)
				{
					if (this._tileObjectDraw.flipVertical == value)
					{
						return;
					}
					this._hasOwnTileObjectDraw = true;
					this._tileObjectDraw = new TileObjectDrawModule(this._tileObjectDraw);
				}
				this._tileObjectDraw.flipVertical = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].DrawFlipVertical = value;
					}
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x004AFC64 File Offset: 0x004ADE64
		// (set) Token: 0x06001404 RID: 5124 RVA: 0x004AFC80 File Offset: 0x004ADE80
		public int DrawStepDown
		{
			get
			{
				if (this._tileObjectDraw == null)
				{
					return this.DrawStepDown;
				}
				return this._tileObjectDraw.stepDown;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectDraw)
				{
					if (this._tileObjectDraw.stepDown == value)
					{
						return;
					}
					this._hasOwnTileObjectDraw = true;
					this._tileObjectDraw = new TileObjectDrawModule(this._tileObjectDraw);
				}
				this._tileObjectDraw.stepDown = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].DrawStepDown = value;
					}
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x004AFD08 File Offset: 0x004ADF08
		// (set) Token: 0x06001406 RID: 5126 RVA: 0x004AFD24 File Offset: 0x004ADF24
		public bool StyleHorizontal
		{
			get
			{
				if (this._tileObjectStyle == null)
				{
					return this.StyleHorizontal;
				}
				return this._tileObjectStyle.horizontal;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectStyle)
				{
					if (this._tileObjectStyle.horizontal == value)
					{
						return;
					}
					this._hasOwnTileObjectStyle = true;
					this._tileObjectStyle = new TileObjectStyleModule(this._tileObjectStyle);
				}
				this._tileObjectStyle.horizontal = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].StyleHorizontal = value;
					}
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x004AFDAC File Offset: 0x004ADFAC
		// (set) Token: 0x06001408 RID: 5128 RVA: 0x004AFDCC File Offset: 0x004ADFCC
		public int Style
		{
			get
			{
				if (this._tileObjectStyle == null)
				{
					return TileObjectData._baseObject.Style;
				}
				return this._tileObjectStyle.style;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectStyle)
				{
					if (this._tileObjectStyle.style == value)
					{
						return;
					}
					this._hasOwnTileObjectStyle = true;
					this._tileObjectStyle = new TileObjectStyleModule(this._tileObjectStyle);
				}
				this._tileObjectStyle.style = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].Style = value;
					}
				}
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x004AFE54 File Offset: 0x004AE054
		// (set) Token: 0x0600140A RID: 5130 RVA: 0x004AFE74 File Offset: 0x004AE074
		public int StyleWrapLimit
		{
			get
			{
				if (this._tileObjectStyle == null)
				{
					return TileObjectData._baseObject.StyleWrapLimit;
				}
				return this._tileObjectStyle.styleWrapLimit;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectStyle)
				{
					if (this._tileObjectStyle.styleWrapLimit == value)
					{
						return;
					}
					this._hasOwnTileObjectStyle = true;
					this._tileObjectStyle = new TileObjectStyleModule(this._tileObjectStyle);
				}
				this._tileObjectStyle.styleWrapLimit = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].StyleWrapLimit = value;
					}
				}
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x004AFEFC File Offset: 0x004AE0FC
		// (set) Token: 0x0600140C RID: 5132 RVA: 0x004AFF1C File Offset: 0x004AE11C
		public int? StyleWrapLimitVisualOverride
		{
			get
			{
				if (this._tileObjectStyle == null)
				{
					return TileObjectData._baseObject.StyleWrapLimitVisualOverride;
				}
				return this._tileObjectStyle.styleWrapLimitVisualOverride;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectStyle)
				{
					int? styleWrapLimitVisualOverride = this._tileObjectStyle.styleWrapLimitVisualOverride;
					int? num = value;
					if (styleWrapLimitVisualOverride.GetValueOrDefault() == num.GetValueOrDefault() & styleWrapLimitVisualOverride != null == (num != null))
					{
						return;
					}
					this._hasOwnTileObjectStyle = true;
					this._tileObjectStyle = new TileObjectStyleModule(this._tileObjectStyle);
				}
				this._tileObjectStyle.styleWrapLimitVisualOverride = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].StyleWrapLimitVisualOverride = value;
					}
				}
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x004AFFC7 File Offset: 0x004AE1C7
		// (set) Token: 0x0600140E RID: 5134 RVA: 0x004AFFE8 File Offset: 0x004AE1E8
		public int? styleLineSkipVisualOverride
		{
			get
			{
				if (this._tileObjectStyle == null)
				{
					return TileObjectData._baseObject.styleLineSkipVisualOverride;
				}
				return this._tileObjectStyle.styleLineSkipVisualoverride;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectStyle)
				{
					int? styleLineSkipVisualoverride = this._tileObjectStyle.styleLineSkipVisualoverride;
					int? num = value;
					if (styleLineSkipVisualoverride.GetValueOrDefault() == num.GetValueOrDefault() & styleLineSkipVisualoverride != null == (num != null))
					{
						return;
					}
					this._hasOwnTileObjectStyle = true;
					this._tileObjectStyle = new TileObjectStyleModule(this._tileObjectStyle);
				}
				this._tileObjectStyle.styleLineSkipVisualoverride = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].styleLineSkipVisualOverride = value;
					}
				}
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x004B0093 File Offset: 0x004AE293
		// (set) Token: 0x06001410 RID: 5136 RVA: 0x004B00B4 File Offset: 0x004AE2B4
		public int StyleLineSkip
		{
			get
			{
				if (this._tileObjectStyle == null)
				{
					return TileObjectData._baseObject.StyleLineSkip;
				}
				return this._tileObjectStyle.styleLineSkip;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectStyle)
				{
					if (this._tileObjectStyle.styleLineSkip == value)
					{
						return;
					}
					this._hasOwnTileObjectStyle = true;
					this._tileObjectStyle = new TileObjectStyleModule(this._tileObjectStyle);
				}
				this._tileObjectStyle.styleLineSkip = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].StyleLineSkip = value;
					}
				}
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x004B013C File Offset: 0x004AE33C
		// (set) Token: 0x06001412 RID: 5138 RVA: 0x004B015C File Offset: 0x004AE35C
		public int StyleMultiplier
		{
			get
			{
				if (this._tileObjectStyle == null)
				{
					return TileObjectData._baseObject.StyleMultiplier;
				}
				return this._tileObjectStyle.styleMultiplier;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectStyle)
				{
					if (this._tileObjectStyle.styleMultiplier == value)
					{
						return;
					}
					this._hasOwnTileObjectStyle = true;
					this._tileObjectStyle = new TileObjectStyleModule(this._tileObjectStyle);
				}
				this._tileObjectStyle.styleMultiplier = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].StyleMultiplier = value;
					}
				}
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x004B01E4 File Offset: 0x004AE3E4
		// (set) Token: 0x06001414 RID: 5140 RVA: 0x004B0204 File Offset: 0x004AE404
		public int Width
		{
			get
			{
				if (this._tileObjectBase == null)
				{
					return TileObjectData._baseObject.Width;
				}
				return this._tileObjectBase.width;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectBase)
				{
					if (this._tileObjectBase.width == value)
					{
						return;
					}
					this._hasOwnTileObjectBase = true;
					this._tileObjectBase = new TileObjectBaseModule(this._tileObjectBase);
					if (!this._hasOwnTileObjectCoords)
					{
						this._hasOwnTileObjectCoords = true;
						this._tileObjectCoords = new TileObjectCoordinatesModule(this._tileObjectCoords, null, null);
						this._tileObjectCoords.calculated = false;
					}
				}
				this._tileObjectBase.width = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].Width = value;
					}
				}
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x004B02BA File Offset: 0x004AE4BA
		// (set) Token: 0x06001416 RID: 5142 RVA: 0x004B02DC File Offset: 0x004AE4DC
		public int Height
		{
			get
			{
				if (this._tileObjectBase == null)
				{
					return TileObjectData._baseObject.Height;
				}
				return this._tileObjectBase.height;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectBase)
				{
					if (this._tileObjectBase.height == value)
					{
						return;
					}
					this._hasOwnTileObjectBase = true;
					this._tileObjectBase = new TileObjectBaseModule(this._tileObjectBase);
					if (!this._hasOwnTileObjectCoords)
					{
						this._hasOwnTileObjectCoords = true;
						this._tileObjectCoords = new TileObjectCoordinatesModule(this._tileObjectCoords, null, null);
						this._tileObjectCoords.calculated = false;
					}
				}
				this._tileObjectBase.height = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].Height = value;
					}
				}
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x004B0392 File Offset: 0x004AE592
		// (set) Token: 0x06001418 RID: 5144 RVA: 0x004B03B4 File Offset: 0x004AE5B4
		public Point16 Origin
		{
			get
			{
				if (this._tileObjectBase == null)
				{
					return TileObjectData._baseObject.Origin;
				}
				return this._tileObjectBase.origin;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectBase)
				{
					if (this._tileObjectBase.origin == value)
					{
						return;
					}
					this._hasOwnTileObjectBase = true;
					this._tileObjectBase = new TileObjectBaseModule(this._tileObjectBase);
				}
				this._tileObjectBase.origin = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].Origin = value;
					}
				}
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x004B0441 File Offset: 0x004AE641
		// (set) Token: 0x0600141A RID: 5146 RVA: 0x004B0464 File Offset: 0x004AE664
		public TileObjectDirection Direction
		{
			get
			{
				if (this._tileObjectBase == null)
				{
					return TileObjectData._baseObject.Direction;
				}
				return this._tileObjectBase.direction;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectBase)
				{
					if (this._tileObjectBase.direction == value)
					{
						return;
					}
					this._hasOwnTileObjectBase = true;
					this._tileObjectBase = new TileObjectBaseModule(this._tileObjectBase);
				}
				this._tileObjectBase.direction = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].Direction = value;
					}
				}
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x004B04EC File Offset: 0x004AE6EC
		// (set) Token: 0x0600141C RID: 5148 RVA: 0x004B050C File Offset: 0x004AE70C
		public int RandomStyleRange
		{
			get
			{
				if (this._tileObjectBase == null)
				{
					return TileObjectData._baseObject.RandomStyleRange;
				}
				return this._tileObjectBase.randomRange;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectBase)
				{
					if (this._tileObjectBase.randomRange == value)
					{
						return;
					}
					this._hasOwnTileObjectBase = true;
					this._tileObjectBase = new TileObjectBaseModule(this._tileObjectBase);
				}
				this._tileObjectBase.randomRange = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].RandomStyleRange = value;
					}
				}
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x004B0594 File Offset: 0x004AE794
		// (set) Token: 0x0600141E RID: 5150 RVA: 0x004B05B4 File Offset: 0x004AE7B4
		public int[] SpecificRandomStyles
		{
			get
			{
				if (this._tileObjectBase == null)
				{
					return TileObjectData._baseObject.SpecificRandomStyles;
				}
				return this._tileObjectBase.specificRandomStyles;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectBase)
				{
					if (this._tileObjectBase.specificRandomStyles == value)
					{
						return;
					}
					this._hasOwnTileObjectBase = true;
					this._tileObjectBase = new TileObjectBaseModule(this._tileObjectBase);
				}
				this._tileObjectBase.specificRandomStyles = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].SpecificRandomStyles = value;
					}
				}
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x004B063C File Offset: 0x004AE83C
		// (set) Token: 0x06001420 RID: 5152 RVA: 0x004B065C File Offset: 0x004AE85C
		public bool FlattenAnchors
		{
			get
			{
				if (this._tileObjectBase == null)
				{
					return TileObjectData._baseObject.FlattenAnchors;
				}
				return this._tileObjectBase.flattenAnchors;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectBase)
				{
					if (this._tileObjectBase.flattenAnchors == value)
					{
						return;
					}
					this._hasOwnTileObjectBase = true;
					this._tileObjectBase = new TileObjectBaseModule(this._tileObjectBase);
				}
				this._tileObjectBase.flattenAnchors = value;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].FlattenAnchors = value;
					}
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x004B06E4 File Offset: 0x004AE8E4
		// (set) Token: 0x06001422 RID: 5154 RVA: 0x004B0704 File Offset: 0x004AE904
		public int[] CoordinateHeights
		{
			get
			{
				if (this._tileObjectCoords == null)
				{
					return TileObjectData._baseObject.CoordinateHeights;
				}
				return this._tileObjectCoords.heights;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectCoords)
				{
					if (value.deepCompare(this._tileObjectCoords.heights))
					{
						return;
					}
					this._hasOwnTileObjectCoords = true;
					this._tileObjectCoords = new TileObjectCoordinatesModule(this._tileObjectCoords, value, this._tileObjectCoords.drawFrameOffsets);
				}
				else
				{
					this._tileObjectCoords.heights = value;
				}
				this._tileObjectCoords.calculated = false;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						int[] coordinateHeights = value;
						if (value != null)
						{
							coordinateHeights = (int[])value.Clone();
						}
						this._alternates.data[i].CoordinateHeights = coordinateHeights;
					}
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x004B07BC File Offset: 0x004AE9BC
		// (set) Token: 0x06001424 RID: 5156 RVA: 0x004B07DC File Offset: 0x004AE9DC
		public Rectangle[,] DrawFrameOffsets
		{
			get
			{
				if (this._tileObjectCoords == null)
				{
					return TileObjectData._baseObject.DrawFrameOffsets;
				}
				return this._tileObjectCoords.drawFrameOffsets;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectCoords)
				{
					if (value.deepCompare(this._tileObjectCoords.drawFrameOffsets))
					{
						return;
					}
					this._hasOwnTileObjectCoords = true;
					this._tileObjectCoords = new TileObjectCoordinatesModule(this._tileObjectCoords, this._tileObjectCoords.heights, value);
				}
				else
				{
					this._tileObjectCoords.drawFrameOffsets = value;
				}
				this._tileObjectCoords.calculated = false;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						Rectangle[,] drawFrameOffsets = value;
						if (value != null)
						{
							drawFrameOffsets = (Rectangle[,])value.Clone();
						}
						this._alternates.data[i].DrawFrameOffsets = drawFrameOffsets;
					}
				}
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x004B0894 File Offset: 0x004AEA94
		// (set) Token: 0x06001426 RID: 5158 RVA: 0x004B08B4 File Offset: 0x004AEAB4
		public int CoordinateWidth
		{
			get
			{
				if (this._tileObjectCoords == null)
				{
					return TileObjectData._baseObject.CoordinateWidth;
				}
				return this._tileObjectCoords.width;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectCoords)
				{
					if (this._tileObjectCoords.width == value)
					{
						return;
					}
					this._hasOwnTileObjectCoords = true;
					this._tileObjectCoords = new TileObjectCoordinatesModule(this._tileObjectCoords, null, null);
				}
				this._tileObjectCoords.width = value;
				this._tileObjectCoords.calculated = false;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].CoordinateWidth = value;
					}
				}
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x004B094A File Offset: 0x004AEB4A
		// (set) Token: 0x06001428 RID: 5160 RVA: 0x004B096C File Offset: 0x004AEB6C
		public int CoordinatePadding
		{
			get
			{
				if (this._tileObjectCoords == null)
				{
					return TileObjectData._baseObject.CoordinatePadding;
				}
				return this._tileObjectCoords.padding;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectCoords)
				{
					if (this._tileObjectCoords.padding == value)
					{
						return;
					}
					this._hasOwnTileObjectCoords = true;
					this._tileObjectCoords = new TileObjectCoordinatesModule(this._tileObjectCoords, null, null);
				}
				this._tileObjectCoords.padding = value;
				this._tileObjectCoords.calculated = false;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].CoordinatePadding = value;
					}
				}
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x004B0A02 File Offset: 0x004AEC02
		// (set) Token: 0x0600142A RID: 5162 RVA: 0x004B0A24 File Offset: 0x004AEC24
		public Point16 CoordinatePaddingFix
		{
			get
			{
				if (this._tileObjectCoords == null)
				{
					return TileObjectData._baseObject.CoordinatePaddingFix;
				}
				return this._tileObjectCoords.paddingFix;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectCoords)
				{
					if (this._tileObjectCoords.paddingFix == value)
					{
						return;
					}
					this._hasOwnTileObjectCoords = true;
					this._tileObjectCoords = new TileObjectCoordinatesModule(this._tileObjectCoords, null, null);
				}
				this._tileObjectCoords.paddingFix = value;
				this._tileObjectCoords.calculated = false;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].CoordinatePaddingFix = value;
					}
				}
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x004B0ABF File Offset: 0x004AECBF
		public int CoordinateFullWidth
		{
			get
			{
				if (this._tileObjectCoords == null)
				{
					return TileObjectData._baseObject.CoordinateFullWidth;
				}
				if (!this._tileObjectCoords.calculated)
				{
					this.Calculate();
				}
				return this._tileObjectCoords.styleWidth;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x004B0AF2 File Offset: 0x004AECF2
		public int CoordinateFullHeight
		{
			get
			{
				if (this._tileObjectCoords == null)
				{
					return TileObjectData._baseObject.CoordinateFullHeight;
				}
				if (!this._tileObjectCoords.calculated)
				{
					this.Calculate();
				}
				return this._tileObjectCoords.styleHeight;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x004B0B25 File Offset: 0x004AED25
		// (set) Token: 0x0600142E RID: 5166 RVA: 0x004B0B48 File Offset: 0x004AED48
		public int DrawStyleOffset
		{
			get
			{
				if (this._tileObjectCoords == null)
				{
					return TileObjectData._baseObject.DrawStyleOffset;
				}
				return this._tileObjectCoords.drawStyleOffset;
			}
			set
			{
				this.WriteCheck();
				if (!this._hasOwnTileObjectCoords)
				{
					if (this._tileObjectCoords.drawStyleOffset == value)
					{
						return;
					}
					this._hasOwnTileObjectCoords = true;
					this._tileObjectCoords = new TileObjectCoordinatesModule(this._tileObjectCoords, null, null);
				}
				this._tileObjectCoords.drawStyleOffset = value;
				this._tileObjectCoords.calculated = false;
				if (this._linkedAlternates)
				{
					for (int i = 0; i < this._alternates.data.Count; i++)
					{
						this._alternates.data[i].DrawStyleOffset = value;
					}
				}
			}
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x004B0BE0 File Offset: 0x004AEDE0
		public bool LiquidPlace(Tile checkTile)
		{
			if (checkTile == null)
			{
				return false;
			}
			if (checkTile.liquid > 0)
			{
				switch (checkTile.liquidType())
				{
				case 0:
				case 2:
				case 3:
					if (this.WaterPlacement == LiquidPlacement.NotAllowed)
					{
						return false;
					}
					if (this.WaterPlacement == LiquidPlacement.OnlyInFullLiquid && checkTile.liquid != 255)
					{
						return false;
					}
					break;
				case 1:
					if (this.LavaPlacement == LiquidPlacement.NotAllowed)
					{
						return false;
					}
					if (this.LavaPlacement == LiquidPlacement.OnlyInFullLiquid && checkTile.liquid != 255)
					{
						return false;
					}
					break;
				}
			}
			else
			{
				switch (checkTile.liquidType())
				{
				case 0:
				case 2:
				case 3:
					if (this.WaterPlacement == LiquidPlacement.OnlyInFullLiquid || this.WaterPlacement == LiquidPlacement.OnlyInLiquid)
					{
						return false;
					}
					break;
				case 1:
					if (this.LavaPlacement == LiquidPlacement.OnlyInFullLiquid || this.LavaPlacement == LiquidPlacement.OnlyInLiquid)
					{
						return false;
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x004B0CAB File Offset: 0x004AEEAB
		public int AlternatesCount
		{
			get
			{
				return this.Alternates.Count;
			}
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x004B0CB8 File Offset: 0x004AEEB8
		public bool isValidTileAnchor(int type)
		{
			int[] array;
			int[] array2;
			if (this._anchorTiles == null)
			{
				array = null;
				array2 = null;
			}
			else
			{
				array = this._anchorTiles.tileValid;
				array2 = this._anchorTiles.tileInvalid;
			}
			if (array2 != null)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					if (type == array2[i])
					{
						return false;
					}
				}
			}
			if (array == null)
			{
				return true;
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (type == array[j])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x004B0D20 File Offset: 0x004AEF20
		public bool isValidWallAnchor(int type)
		{
			int[] array;
			if (this._anchorTiles == null)
			{
				array = null;
			}
			else
			{
				array = this._anchorTiles.wallValid;
			}
			if (array == null)
			{
				return type != 0;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (type == array[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x004B0D68 File Offset: 0x004AEF68
		public bool isValidAlternateAnchor(int type)
		{
			if (this._anchorTiles == null)
			{
				return false;
			}
			int[] tileAlternates = this._anchorTiles.tileAlternates;
			if (tileAlternates == null)
			{
				return false;
			}
			for (int i = 0; i < tileAlternates.Length; i++)
			{
				if (type == tileAlternates[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x004B0DA8 File Offset: 0x004AEFA8
		public int CalculatePlacementStyle(int style, int alternate, int random)
		{
			int num = style * this.StyleMultiplier;
			num += this.Style;
			if (random >= 0)
			{
				num += random;
			}
			return num;
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x004B0DD0 File Offset: 0x004AEFD0
		private static void addBaseTile(out TileObjectData baseTile)
		{
			TileObjectData.newTile.Calculate();
			baseTile = TileObjectData.newTile;
			baseTile._parent = TileObjectData._baseObject;
			TileObjectData.newTile = new TileObjectData(TileObjectData._baseObject);
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x004B0DFE File Offset: 0x004AEFFE
		private static void addTile(int tileType)
		{
			TileObjectData.newTile.Calculate();
			TileObjectData._data[tileType] = TileObjectData.newTile;
			TileObjectData.newTile = new TileObjectData(TileObjectData._baseObject);
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x004B0E2C File Offset: 0x004AF02C
		private static void addSubTile(params int[] styles)
		{
			TileObjectData.newSubTile.Calculate();
			foreach (int num in styles)
			{
				List<TileObjectData> list;
				if (!TileObjectData.newTile._hasOwnSubTiles)
				{
					list = new List<TileObjectData>(num + 1);
					TileObjectData.newTile.SubTiles = list;
				}
				else
				{
					list = TileObjectData.newTile.SubTiles;
				}
				if (list.Count <= num)
				{
					for (int j = list.Count; j <= num; j++)
					{
						list.Add(null);
					}
				}
				TileObjectData.newSubTile._parent = TileObjectData.newTile;
				list[num] = TileObjectData.newSubTile;
			}
			TileObjectData.newSubTile = new TileObjectData(TileObjectData._baseObject);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x004B0ED0 File Offset: 0x004AF0D0
		private static void addSubTileRange(int startStyle, int styleCount)
		{
			TileObjectData.newSubTile.Calculate();
			for (int i = 0; i < styleCount; i++)
			{
				int num = startStyle + i;
				List<TileObjectData> list;
				if (!TileObjectData.newTile._hasOwnSubTiles)
				{
					list = new List<TileObjectData>(num + 1);
					TileObjectData.newTile.SubTiles = list;
				}
				else
				{
					list = TileObjectData.newTile.SubTiles;
				}
				if (list.Count <= num)
				{
					for (int j = list.Count; j <= num; j++)
					{
						list.Add(null);
					}
				}
				TileObjectData.newSubTile._parent = TileObjectData.newTile;
				list[num] = TileObjectData.newSubTile;
			}
			TileObjectData.newSubTile = new TileObjectData(TileObjectData._baseObject);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x004B0F70 File Offset: 0x004AF170
		private static void addSubTile(int style)
		{
			TileObjectData.newSubTile.Calculate();
			List<TileObjectData> list;
			if (!TileObjectData.newTile._hasOwnSubTiles)
			{
				list = new List<TileObjectData>(style + 1);
				TileObjectData.newTile.SubTiles = list;
			}
			else
			{
				list = TileObjectData.newTile.SubTiles;
			}
			if (list.Count <= style)
			{
				for (int i = list.Count; i <= style; i++)
				{
					list.Add(null);
				}
			}
			TileObjectData.newSubTile._parent = TileObjectData.newTile;
			list[style] = TileObjectData.newSubTile;
			TileObjectData.newSubTile = new TileObjectData(TileObjectData._baseObject);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x004B1000 File Offset: 0x004AF200
		private static void addAlternate(int baseStyle)
		{
			TileObjectData.newAlternate.Calculate();
			if (!TileObjectData.newTile._hasOwnAlternates)
			{
				TileObjectData.newTile.Alternates = new List<TileObjectData>();
			}
			TileObjectData.newAlternate.Style = baseStyle;
			TileObjectData.newAlternate._parent = TileObjectData.newTile;
			TileObjectData.newTile.Alternates.Add(TileObjectData.newAlternate);
			TileObjectData.newAlternate = new TileObjectData(TileObjectData._baseObject);
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x004B1070 File Offset: 0x004AF270
		public static void Initialize()
		{
			int[] anchorInvalidTiles = new int[]
			{
				127,
				138,
				664,
				665,
				484,
				711,
				712,
				713,
				714,
				715,
				716
			};
			TileObjectData._baseObject = new TileObjectData(null);
			TileObjectData._baseObject.SetupBaseObject();
			TileObjectData._data = new List<TileObjectData>((int)TileID.Count);
			for (int i = 0; i < (int)TileID.Count; i++)
			{
				TileObjectData._data.Add(null);
			}
			TileObjectData.newTile = new TileObjectData(TileObjectData._baseObject);
			TileObjectData.newSubTile = new TileObjectData(TileObjectData._baseObject);
			TileObjectData.newAlternate = new TileObjectData(TileObjectData._baseObject);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleMultiplier = 27;
			TileObjectData.newTile.StyleWrapLimit = 27;
			TileObjectData.newTile.UsesCustomCanPlace = false;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				13,
				43,
				47
			});
			TileObjectData.addTile(19);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleMultiplier = 27;
			TileObjectData.newTile.StyleWrapLimit = 27;
			TileObjectData.newTile.UsesCustomCanPlace = false;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(427);
			for (int j = 435; j <= 439; j++)
			{
				TileObjectData.newTile.CoordinateHeights = new int[]
				{
					16
				};
				TileObjectData.newTile.CoordinateWidth = 16;
				TileObjectData.newTile.CoordinatePadding = 2;
				TileObjectData.newTile.StyleHorizontal = true;
				TileObjectData.newTile.StyleMultiplier = 27;
				TileObjectData.newTile.StyleWrapLimit = 27;
				TileObjectData.newTile.UsesCustomCanPlace = false;
				TileObjectData.newTile.LavaDeath = true;
				TileObjectData.addTile(j);
			}
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.Origin = new Point16(1, 7);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.HookPlaceOverride = new PlacementHook(new Func<int, int, int, int, int, int, int>(WorldGen.PlaceXmasTree_Direct), -1, 0, true);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(171);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				38
			};
			TileObjectData.newTile.CoordinateWidth = 32;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.DrawYOffset = -20;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.addBaseTile(out TileObjectData.StyleDye);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleDye);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleDye);
			TileObjectData.newSubTile.AnchorValidWalls = new int[1];
			TileObjectData.addSubTile(3);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleDye);
			TileObjectData.newSubTile.AnchorValidWalls = new int[1];
			TileObjectData.addSubTile(4);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleDye);
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.OnlyInFullLiquid;
			TileObjectData.addSubTile(5);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleDye);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				80
			};
			TileObjectData.newSubTile.AnchorLeft = new AnchorData(AnchorType.EmptyTile, 1, 1);
			TileObjectData.newSubTile.AnchorRight = new AnchorData(AnchorType.EmptyTile, 1, 1);
			TileObjectData.addSubTile(6);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleDye);
			TileObjectData.newSubTile.DrawYOffset = -6;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newSubTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newSubTile.Width, 0);
			TileObjectData.newSubTile.AnchorBottom = new AnchorData(AnchorType.EmptyTile, TileObjectData.newSubTile.Width, 0);
			TileObjectData.addSubTile(7);
			TileObjectData.addTile(227);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleDye);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.CoordinateWidth = 20;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = false;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(579);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.newTile.StyleLineSkip = 3;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 1);
			TileObjectData.addAlternate(0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 2);
			TileObjectData.addAlternate(0);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				19,
				48
			});
			TileObjectData.addTile(10);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.newTile.StyleLineSkip = 2;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 1);
			TileObjectData.addAlternate(0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 2);
			TileObjectData.addAlternate(0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(1, 0);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
			TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(1, 1);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
			TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(1, 2);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
			TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.addAlternate(1);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				19,
				48
			});
			TileObjectData.addTile(11);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				18,
				16,
				16,
				16,
				18
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleWrapLimit = 2;
			for (int k = 1; k < 5; k++)
			{
				TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
				TileObjectData.newAlternate.Origin = new Point16(0, k);
				TileObjectData.addAlternate(0);
			}
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			for (int l = 1; l < 5; l++)
			{
				TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
				TileObjectData.newAlternate.Origin = new Point16(0, l);
				TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
				TileObjectData.addAlternate(1);
			}
			TileObjectData.addTile(388);
			TileObjectData.newTile.FullCopyFrom(388);
			TileObjectData.addTile(389);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addBaseTile(out TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(13);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.DrawYOffset = -4;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				25,
				41
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(39);
			TileObjectData.addTile(33);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.DrawYOffset = -4;
			TileObjectData.addTile(49);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TEFoodPlatter.Hook_AfterPlacement), -1, 0, true);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(520);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.DrawYOffset = -4;
			TileObjectData.addTile(372);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.DrawYOffset = -4;
			TileObjectData.addTile(646);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(50);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(707);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(494);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(78);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.DrawYOffset = -4;
			TileObjectData.addTile(174);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addBaseTile(out TileObjectData.Style1xX);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(37);
			TileObjectData.newTile.StyleLineSkip = 2;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				23,
				42
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(40);
			TileObjectData.addTile(93);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
			TileObjectData.newTile.Height = 6;
			TileObjectData.newTile.Origin = new Point16(0, 5);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.addTile(92);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(453);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style1x2Top);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(270);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(271);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(581);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(660);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawXOffset = 10;
			TileObjectData tileObjectData = TileObjectData.newTile;
			Rectangle[,] array = new Rectangle[1, 2];
			array[0, 0] = new Rectangle(0, 0, 20, 0);
			array[0, 1] = new Rectangle(0, -2, 20, 28);
			tileObjectData.DrawFrameOffsets = array;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				213,
				366,
				365,
				353,
				214,
				449,
				450,
				451
			};
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TEDeadCellsDisplayJar.Hook_AfterPlacement), -1, 0, true);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData tileObjectData2 = TileObjectData.newSubTile;
			Rectangle[,] array2 = new Rectangle[1, 2];
			array2[0, 0] = new Rectangle(20, 0, 20, 0);
			array2[0, 1] = new Rectangle(20, -2, 20, 28);
			tileObjectData2.DrawFrameOffsets = array2;
			TileObjectData.addSubTile(1);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData tileObjectData3 = TileObjectData.newSubTile;
			Rectangle[,] array3 = new Rectangle[1, 2];
			array3[0, 0] = new Rectangle(40, 0, 20, 0);
			array3[0, 1] = new Rectangle(40, -2, 20, 28);
			tileObjectData3.DrawFrameOffsets = array3;
			TileObjectData.addSubTile(2);
			TileObjectData.addTile(698);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.StyleWrapLimit = 6;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(572);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(56);
			TileObjectData.newTile.styleLineSkipVisualOverride = new int?(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				32,
				48
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(46);
			TileObjectData.addTile(42);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(91);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(487);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addBaseTile(out TileObjectData.Style4x2);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				25,
				42
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.Alternates = new List<TileObjectData>();
			TileObjectData.newSubTile.DrawStyleOffset = -112;
			TileObjectData tileObjectData4 = TileObjectData.newSubTile;
			Rectangle[,] array4 = new Rectangle[4, 2];
			array4[0, 0] = new Rectangle(144, 0, 0, 0);
			array4[0, 1] = new Rectangle(144, 0, 0, 0);
			array4[1, 0] = new Rectangle(144, 0, 0, 0);
			array4[1, 1] = new Rectangle(144, 0, 0, 0);
			array4[2, 0] = new Rectangle(144, 0, 0, 0);
			array4[2, 1] = new Rectangle(144, 0, 0, 0);
			array4[3, 0] = new Rectangle(144, 0, 0, 0);
			array4[3, 1] = new Rectangle(144, 0, 0, 0);
			tileObjectData4.DrawFrameOffsets = array4;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newSubTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.newAlternate.Calculate();
			TileObjectData.newAlternate.Style = 1;
			TileObjectData.newAlternate._parent = TileObjectData.newSubTile;
			TileObjectData.newSubTile.Alternates.Add(TileObjectData.newAlternate);
			TileObjectData.newAlternate = new TileObjectData(TileObjectData._baseObject);
			TileObjectData.addSubTile(new int[]
			{
				56,
				57,
				58,
				59,
				60,
				61,
				62,
				63,
				64
			});
			TileObjectData.addTile(90);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				8,
				42
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.Alternates = new List<TileObjectData>();
			TileObjectData.newSubTile.DrawStyleOffset = -112;
			TileObjectData tileObjectData5 = TileObjectData.newSubTile;
			Rectangle[,] array5 = new Rectangle[4, 2];
			array5[0, 0] = new Rectangle(144, 0, 0, 0);
			array5[0, 1] = new Rectangle(144, 0, 0, 0);
			array5[1, 0] = new Rectangle(144, 0, 0, 0);
			array5[1, 1] = new Rectangle(144, 0, 0, 0);
			array5[2, 0] = new Rectangle(144, 0, 0, 0);
			array5[2, 1] = new Rectangle(144, 0, 0, 0);
			array5[3, 0] = new Rectangle(144, 0, 0, 0);
			array5[3, 1] = new Rectangle(144, 0, 0, 0);
			tileObjectData5.DrawFrameOffsets = array5;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newSubTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.newAlternate.Calculate();
			TileObjectData.newAlternate.Style = 1;
			TileObjectData.newAlternate._parent = TileObjectData.newSubTile;
			TileObjectData.newSubTile.Alternates.Add(TileObjectData.newAlternate);
			TileObjectData.newAlternate = new TileObjectData(TileObjectData._baseObject);
			TileObjectData.addSubTile(new int[]
			{
				56,
				57,
				58,
				59,
				60,
				61,
				62,
				63,
				64
			});
			TileObjectData.addTile(79);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 1);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(209);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addBaseTile(out TileObjectData.StyleSmallCage);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(285);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(286);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(582);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(619);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(298);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(299);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(310);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(532);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(533);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(339);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(538);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(555);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(556);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(629);
			TileObjectData.newTile.Width = 6;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(3, 2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style6x3);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(275);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(276);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(413);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(414);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(277);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(278);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(279);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(280);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(281);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(632);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(640);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(643);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(644);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(645);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(710);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(296);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(297);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(309);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(550);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(551);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(553);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(554);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(558);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(559);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(599);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(600);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(601);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(602);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(603);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(604);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(605);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(606);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(607);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(608);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(609);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(610);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(611);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(612);
			TileObjectData.newTile.Width = 5;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Origin = new Point16(2, 3);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style5x4);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.addTile(464);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.addTile(466);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style2x1);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(29);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(103);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(462);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(56);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				18
			};
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				14,
				43
			});
			TileObjectData.addTile(18);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				18
			};
			TileObjectData.addTile(16);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(134);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Height, 0);
			TileObjectData.addTile(387);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleWrapLimit = 53;
			TileObjectData.addTile(649);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -2;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -2;
			TileObjectData.addAlternate(3);
			TileObjectData.addTile(443);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addBaseTile(out TileObjectData.Style2xX);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.Origin = new Point16(1, 4);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(547);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.Origin = new Point16(1, 4);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(623);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Origin = new Point16(1, 3);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(207);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				18
			};
			TileObjectData.addTile(410);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(480);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(509);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(657);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(658);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(720);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(721);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(725);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.addTile(489);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleWrapLimit = 7;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(7);
			TileObjectData.addTile(349);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.addTile(337);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(560);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.PlatformNonHammered | AnchorType.AllFlatHeight, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(465);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(531);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.addTile(320);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.addTile(456);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TETrainingDummy.Hook_AfterPlacement), -1, 0, false);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(378);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleWrapLimit = 55;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(165);
			TileObjectData.addTile(105);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(2);
			TileObjectData.addTile(545);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(56);
			TileObjectData.newTile.Origin = new Point16(0, 4);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				17,
				43
			});
			TileObjectData.addTile(104);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(128);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(506);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(269);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.DrawStyleOffset = 4;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TEDisplayDoll.Hook_AfterPlacement), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = anchorInvalidTiles;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(470);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.PlatformNonHammered | AnchorType.AllFlatHeight, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(591);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.PlatformNonHammered | AnchorType.AllFlatHeight, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(592);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addBaseTile(out TileObjectData.Style3x3);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Height = 6;
			TileObjectData.newTile.Origin = new Point16(1, 5);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(7);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(8);
			TileObjectData.addTile(548);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.Origin = new Point16(1, 4);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(613);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Height = 6;
			TileObjectData.newTile.Origin = new Point16(1, 5);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(614);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.StyleWrapLimit = 37;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.StyleLineSkip = 2;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				32,
				48
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(46);
			TileObjectData.addTile(34);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Origin = new Point16(2, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.addTile(454);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style3x2);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newSubTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(13);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newSubTile.Height = 1;
			TileObjectData.newSubTile.Origin = new Point16(1, 0);
			TileObjectData.newSubTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.addSubTile(25);
			TileObjectData.addTile(14);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newSubTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(11);
			TileObjectData.addTile(469);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(37);
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(new Func<int, int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = anchorInvalidTiles;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				9,
				42
			});
			TileObjectData.addTile(88);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addTile(237);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(244);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(647);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleWrapLimit = 35;
			TileObjectData.addTile(648);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(706);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(651);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.addTile(26);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.addTile(695);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.addTile(86);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(377);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(37);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				15,
				42
			});
			TileObjectData.addTile(87);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.addTile(486);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(488);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(704);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 9;
			TileObjectData.addTile(705);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(37);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				10,
				46
			});
			TileObjectData.addTile(89);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.addTile(114);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				59,
				70
			};
			TileObjectData.addSubTile(new int[]
			{
				32,
				33,
				34
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				147,
				161,
				163,
				200,
				164,
				162,
				224
			};
			TileObjectData.addSubTile(new int[]
			{
				26,
				27,
				28,
				29,
				30,
				31
			});
			TileObjectData.addTile(186);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleWrapLimit = 35;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				59,
				60,
				226
			};
			TileObjectData.addSubTile(new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				57,
				58,
				75,
				76
			};
			TileObjectData.addSubTile(new int[]
			{
				6,
				7,
				8
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				53,
				397,
				396,
				112,
				398,
				400,
				234,
				399,
				401,
				116,
				402,
				403
			};
			TileObjectData.addSubTile(new int[]
			{
				29,
				30,
				31,
				32,
				33,
				34
			});
			TileObjectData.addTile(187);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				53,
				112,
				234,
				116
			};
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addTile(552);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleWrapLimit = 16;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(1);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(4);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(9);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(1 + TileObjectData.newTile.StyleWrapLimit);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(4 + TileObjectData.newTile.StyleWrapLimit);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(9 + TileObjectData.newTile.StyleWrapLimit);
			TileObjectData.addTile(215);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(217);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(218);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.addTile(17);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(77);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(133);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.addTile(405);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(235);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Origin = new Point16(1, 3);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style3x4);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(37);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				4,
				43
			});
			TileObjectData.addTile(101);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(102);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(463);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TEHatRack.Hook_AfterPlacement), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = anchorInvalidTiles;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(475);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(new Func<int, int, int, int, int, int, int>(TETeleportationPylon.PlacementPreviewHook_CheckIfCanPlace), 1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TETeleportationPylon.PlacementPreviewHook_AfterPlacement), -1, 0, false);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(597);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(2);
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.styleLineSkipVisualOverride = new int?(0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(617);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Origin = new Point16(1, 3);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style4x4);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x4);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(699);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style2x2);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(new Func<int, int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = anchorInvalidTiles;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(21);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(new Func<int, int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = anchorInvalidTiles;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(467);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.AnchorInvalidTiles = anchorInvalidTiles;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(441);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.AnchorInvalidTiles = anchorInvalidTiles;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(468);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 6;
			TileObjectData.newTile.StyleMultiplier = 6;
			TileObjectData.newTile.RandomStyleRange = 6;
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				2,
				477,
				109,
				492
			};
			TileObjectData.addTile(254);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(96);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 4;
			TileObjectData.newTile.StyleMultiplier = 1;
			TileObjectData.newTile.RandomStyleRange = 4;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(485);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(457);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(490);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(56);
			TileObjectData.newTile.styleLineSkipVisualOverride = new int?(2);
			TileObjectData.addTile(139);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.addTile(35);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(652);
			int styleWrapLimit = 3;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = styleWrapLimit;
			TileObjectData.addTile(653);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.addTile(95);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.addTile(126);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.addTile(444);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.addTile(98);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(53);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				13,
				43
			});
			TileObjectData.addTile(172);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(94);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(411);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(97);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(99);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleWrapLimitVisualOverride = new int?(56);
			TileObjectData.newTile.StyleLineSkip = 2;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				25,
				42
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(40);
			TileObjectData.addTile(100);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(125);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(621);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(622);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(173);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(287);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(319);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(287);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(376);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(138);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(664);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData tileObjectData6 = TileObjectData.newTile;
			Rectangle[,] array6 = new Rectangle[2, 2];
			array6[0, 0] = new Rectangle(0, 0, 0, 0);
			array6[0, 1] = new Rectangle(0, 0, 2, 0);
			array6[1, 0] = new Rectangle(0, 0, 0, 2);
			array6[1, 1] = new Rectangle(0, 0, 2, 2);
			tileObjectData6.DrawFrameOffsets = array6;
			TileObjectData.addTile(711);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(712);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(654);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(484);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(142);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(143);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(282);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(543);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(598);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(568);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(569);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(570);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(288);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(289);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(290);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(291);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(292);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(293);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(294);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(295);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(316);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(317);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(318);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(360);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(580);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(620);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(565);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(521);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(522);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(523);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(524);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(525);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(526);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(527);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(505);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(358);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(359);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.addTile(542);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(361);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(362);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(363);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(364);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(544);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(391);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(392);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(393);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(394);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(287);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(335);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(564);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(594);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(354);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(355);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(491);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.addTile(356);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.addTile(663);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile, 1, 1);
			TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile, 1, 1);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.addTile(386);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(2);
			TileObjectData.addTile(132);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(1, 0);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(3);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(55);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(1, 0);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(3);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(573);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(1, 0);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(3);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(425);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(1, 0);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(3);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(510);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(1, 0);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(3);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(511);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(85);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TEItemFrame.Hook_AfterPlacement), -1, 0, true);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(1, 0);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 2, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(3);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = Point16.Zero;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(395);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(12);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(665);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addTile(713);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(714);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(715);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(639);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.addTile(696);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(702);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(751);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(752);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style3x3);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.addTile(106);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(212);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(219);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(642);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(220);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(228);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(231);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(243);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(247);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(283);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(300);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(301);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(302);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(303);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(304);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(305);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(306);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(307);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(308);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.addTile(406);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.addTile(452);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(412);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(455);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(499);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData tileObjectData7 = TileObjectData.newTile;
			Rectangle[,] array7 = new Rectangle[3, 3];
			array7[0, 0] = new Rectangle(54, 0, 0, 0);
			array7[0, 1] = new Rectangle(54, 0, 0, 0);
			array7[0, 2] = new Rectangle(54, 0, 0, 0);
			array7[1, 0] = new Rectangle(54, 0, 0, 0);
			array7[1, 1] = new Rectangle(54, 0, 0, 0);
			array7[1, 2] = new Rectangle(54, 0, 0, 0);
			array7[2, 0] = new Rectangle(54, 0, 0, 0);
			array7[2, 1] = new Rectangle(54, 0, 0, 0);
			array7[2, 2] = new Rectangle(54, 0, 0, 0);
			tileObjectData7.DrawFrameOffsets = array7;
			TileObjectData.addTile(733);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style1x2);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				16,
				47
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.Alternates = new List<TileObjectData>();
			TileObjectData.newSubTile.DrawStyleOffset = -102;
			TileObjectData tileObjectData8 = TileObjectData.newSubTile;
			Rectangle[,] array8 = new Rectangle[1, 2];
			array8[0, 0] = new Rectangle(36, 0, 0, 0);
			array8[0, 1] = new Rectangle(36, 0, 0, 0);
			tileObjectData8.DrawFrameOffsets = array8;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newSubTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.newAlternate.Calculate();
			TileObjectData.newAlternate.Style = 1;
			TileObjectData.newAlternate._parent = TileObjectData.newSubTile;
			TileObjectData.newSubTile.Alternates.Add(TileObjectData.newAlternate);
			TileObjectData.newAlternate = new TileObjectData(TileObjectData._baseObject);
			TileObjectData.addSubTile(new int[]
			{
				51,
				52,
				53,
				54,
				55,
				56,
				57,
				58,
				59,
				60,
				61,
				62,
				63,
				64,
				65,
				66,
				67
			});
			TileObjectData.addTile(15);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(new int[]
			{
				14,
				42
			});
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.Alternates = new List<TileObjectData>();
			TileObjectData.newSubTile.DrawStyleOffset = -102;
			TileObjectData tileObjectData9 = TileObjectData.newSubTile;
			Rectangle[,] array9 = new Rectangle[1, 2];
			array9[0, 0] = new Rectangle(36, 0, 0, 0);
			array9[0, 1] = new Rectangle(36, 0, 0, 0);
			tileObjectData9.DrawFrameOffsets = array9;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newSubTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.newAlternate.Calculate();
			TileObjectData.newAlternate.Style = 1;
			TileObjectData.newAlternate._parent = TileObjectData.newSubTile;
			TileObjectData.newSubTile.Alternates.Add(TileObjectData.newAlternate);
			TileObjectData.newAlternate = new TileObjectData(TileObjectData._baseObject);
			TileObjectData.addSubTile(new int[]
			{
				51,
				52,
				53,
				54,
				55,
				56,
				57,
				58,
				59,
				60,
				61,
				62,
				63,
				64
			});
			TileObjectData.addTile(497);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				20
			};
			TileObjectData.addTile(216);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(390);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.addTile(338);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 6;
			TileObjectData.newTile.DrawStyleOffset = 13 * TileObjectData.newTile.StyleWrapLimit;
			TileObjectData.addTile(493);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				18,
				18
			};
			TileObjectData.newTile.CoordinateWidth = 26;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.addTile(567);
			int num = 39;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = num;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.Origin = new Point16(0, 1);
			TileObjectData.newSubTile.AnchorTop = AnchorData.Empty;
			TileObjectData.newSubTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addSubTileRange(num, num);
			TileObjectData.addTile(694);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorInvalidTiles = anchorInvalidTiles;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style1x1);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
			TileObjectData.newTile.AnchorInvalidTiles = null;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(420);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.addTile(624);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 20;
			TileObjectData.newTile.DrawXOffset = 0;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.addTile(700);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.addTile(656);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				34
			};
			TileObjectData.newTile.CoordinateWidth = 24;
			TileObjectData.newTile.DrawXOffset = 0;
			TileObjectData.newTile.DrawYOffset = -16;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.addTile(701);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.addTile(703);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				18
			};
			TileObjectData.newTile.CoordinateWidth = 20;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(476);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				420,
				419
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 1);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[]
			{
				419
			};
			TileObjectData.addTile(419);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
			TileObjectData.newTile.AnchorInvalidTiles = null;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TELogicSensor.Hook_AfterPlacement), -1, 0, true);
			TileObjectData.addTile(423);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
			TileObjectData.newTile.AnchorInvalidTiles = null;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(424);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
			TileObjectData.newTile.AnchorInvalidTiles = null;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(445);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
			TileObjectData.newTile.AnchorInvalidTiles = null;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(429);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				26
			};
			TileObjectData.newTile.CoordinateWidth = 24;
			TileObjectData.newTile.DrawYOffset = -8;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(81);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				18
			};
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(135);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				18
			};
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(428);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.RandomStyleRange = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(141);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
			TileObjectData.newTile.AnchorInvalidTiles = null;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(144);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(210);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(239);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(650);
			int num2 = 39;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = num2;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.AnchorTop = AnchorData.Empty;
			TileObjectData.newSubTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addSubTileRange(num2, num2);
			TileObjectData.addTile(693);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(36);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 3;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.CoordinateWidth = 20;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(324);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(593);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				124,
				561,
				574,
				575,
				576,
				577,
				578
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.addAlternate(3);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(630);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				124,
				561,
				574,
				575,
				576,
				577,
				578
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.addAlternate(3);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(631);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorInvalidTiles = null;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TEKiteAnchor.Hook_AfterPlacement), -1, 0, true);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -2;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.DrawYOffset = 0;
			TileObjectData.newAlternate.DrawXOffset = 2;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.DrawYOffset = 0;
			TileObjectData.newAlternate.DrawXOffset = -2;
			TileObjectData.addAlternate(3);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.newAlternate.DrawYOffset = 0;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(723);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorInvalidTiles = null;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleMultiplier = 5;
			TileObjectData.newTile.StyleWrapLimit = 5;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TECritterAnchor.Hook_AfterPlacement), -1, 0, true);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -2;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.DrawYOffset = 0;
			TileObjectData.newAlternate.DrawXOffset = 2;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.DrawYOffset = 0;
			TileObjectData.newAlternate.DrawXOffset = -2;
			TileObjectData.addAlternate(3);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.newAlternate.DrawYOffset = 0;
			TileObjectData.addAlternate(4);
			TileObjectData.addTile(724);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.FlattenAnchors = true;
			TileObjectData.addBaseTile(out TileObjectData.StyleSwitch);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleSwitch);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[]
			{
				124,
				561,
				574,
				575,
				576,
				577,
				578
			};
			TileObjectData.newAlternate.DrawXOffset = 2;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleSwitch);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[]
			{
				124,
				561,
				574,
				575,
				576,
				577,
				578
			};
			TileObjectData.newAlternate.DrawXOffset = -2;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleSwitch);
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(3);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(136);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.FlattenAnchors = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.DrawStepDown = 2;
			TileObjectData.newTile.CoordinateWidth = 20;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleMultiplier = 6;
			TileObjectData.newTile.StyleWrapLimit = 6;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.addBaseTile(out TileObjectData.StyleTorch);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[]
			{
				124,
				561,
				574,
				575,
				576,
				577,
				578
			};
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[]
			{
				124,
				561,
				574,
				575,
				576,
				577,
				578
			};
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(0);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(8);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(11);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.LinkedAlternates = true;
			TileObjectData.newSubTile.WaterDeath = false;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(17);
			TileObjectData.addTile(4);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.FlattenAnchors = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.DrawStepDown = 2;
			TileObjectData.newTile.CoordinateWidth = 20;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleWrapLimit = 4;
			TileObjectData.newTile.StyleMultiplier = 4;
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(new Func<int, int, int, int, int, int, int>(WorldGen.CanPlaceProjectilePressurePad), -1, 0, true);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile | AnchorType.EmptyTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawStepDown = 0;
			TileObjectData.newAlternate.DrawYOffset = -4;
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile | AnchorType.EmptyTile | AnchorType.SolidBottom, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[]
			{
				124,
				561,
				574,
				575,
				576,
				577,
				578
			};
			TileObjectData.newAlternate.DrawXOffset = -2;
			TileObjectData.newAlternate.DrawYOffset = -2;
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile | AnchorType.EmptyTile | AnchorType.SolidBottom, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new int[]
			{
				124,
				561,
				574,
				575,
				576,
				577,
				578
			};
			TileObjectData.newAlternate.DrawXOffset = 2;
			TileObjectData.newAlternate.DrawYOffset = -2;
			TileObjectData.addAlternate(3);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile | AnchorType.EmptyTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(442);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = Point16.Zero;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addBaseTile(out TileObjectData.StyleAlch);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				2,
				477,
				109,
				492
			};
			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				78
			};
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				60
			};
			TileObjectData.newSubTile.AnchorAlternateTiles = new int[]
			{
				78
			};
			TileObjectData.addSubTile(1);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				0,
				59
			};
			TileObjectData.newSubTile.AnchorAlternateTiles = new int[]
			{
				78
			};
			TileObjectData.addSubTile(2);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				199,
				203,
				25,
				23
			};
			TileObjectData.newSubTile.AnchorAlternateTiles = new int[]
			{
				78
			};
			TileObjectData.addSubTile(3);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				53,
				116
			};
			TileObjectData.newSubTile.AnchorAlternateTiles = new int[]
			{
				78
			};
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(4);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				57,
				633
			};
			TileObjectData.newSubTile.AnchorAlternateTiles = new int[]
			{
				78
			};
			TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newSubTile.LavaDeath = false;
			TileObjectData.addSubTile(5);
			TileObjectData.newSubTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newSubTile.AnchorValidTiles = new int[]
			{
				147,
				161,
				163,
				164,
				200
			};
			TileObjectData.newSubTile.AnchorAlternateTiles = new int[]
			{
				78
			};
			TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.addSubTile(6);
			TileObjectData.addTile(82);
			TileObjectData.newTile.FullCopyFrom(82);
			TileObjectData.addTile(83);
			TileObjectData.newTile.FullCopyFrom(83);
			TileObjectData.addTile(84);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addBaseTile(out TileObjectData.Style3x3Wall);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(240);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(440);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(334);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(TEWeaponsRack.Hook_AfterPlacement), -1, 0, true);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(471);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
			TileObjectData.newSubTile.RandomStyleRange = 4;
			TileObjectData.addSubTile(15);
			TileObjectData.addTile(245);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.addTile(246);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.addTile(241);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Width = 6;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Origin = new Point16(2, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.StyleWrapLimit = 27;
			TileObjectData.addTile(242);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Origin = new Point16(0, 3);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				18
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				2,
				477,
				109,
				60,
				492,
				633
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addTile(27);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				2,
				477
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				147
			};
			TileObjectData.addAlternate(3);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				60
			};
			TileObjectData.addAlternate(6);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				23,
				661
			};
			TileObjectData.addAlternate(9);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				199,
				662
			};
			TileObjectData.addAlternate(12);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				109,
				492
			};
			TileObjectData.addAlternate(15);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				53
			};
			TileObjectData.addAlternate(18);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				116
			};
			TileObjectData.addAlternate(21);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				234
			};
			TileObjectData.addAlternate(24);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				112
			};
			TileObjectData.addAlternate(27);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorValidTiles = new int[]
			{
				633
			};
			TileObjectData.addAlternate(30);
			TileObjectData.addTile(20);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				1,
				25,
				117,
				203,
				182,
				180,
				179,
				381,
				183,
				181,
				534,
				536,
				539,
				625,
				627
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleMultiplier = 3;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(590);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				2,
				477,
				492,
				60,
				109,
				199,
				23
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleMultiplier = 3;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(595);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				2,
				477,
				492,
				60,
				109,
				199,
				23
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleMultiplier = 3;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(615);
			TileObjectData.readOnlyData = true;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x004B9EE8 File Offset: 0x004B80E8
		public static bool CustomPlace(int type, int style)
		{
			if (type < 0 || type >= TileObjectData._data.Count || style < 0)
			{
				return false;
			}
			TileObjectData tileObjectData = TileObjectData._data[type];
			if (tileObjectData == null)
			{
				return false;
			}
			List<TileObjectData> subTiles = tileObjectData.SubTiles;
			if (subTiles != null && style < subTiles.Count)
			{
				TileObjectData tileObjectData2 = subTiles[style];
				if (tileObjectData2 != null)
				{
					return tileObjectData2._usesCustomCanPlace;
				}
			}
			return tileObjectData._usesCustomCanPlace;
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x004B9F48 File Offset: 0x004B8148
		public static bool CheckLiquidPlacement(int type, int style, Tile checkTile)
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, style, 0);
			if (tileData != null)
			{
				return tileData.LiquidPlace(checkTile);
			}
			return TileObjectData.LiquidPlace(type, checkTile);
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x004B9F70 File Offset: 0x004B8170
		public static bool LiquidPlace(int type, Tile checkTile)
		{
			if (checkTile == null)
			{
				return false;
			}
			if (checkTile.liquid > 0)
			{
				switch (checkTile.liquidType())
				{
				case 0:
				case 2:
				case 3:
					if (Main.tileWaterDeath[type])
					{
						return false;
					}
					break;
				case 1:
					if (Main.tileLavaDeath[type])
					{
						return false;
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x004B9FC4 File Offset: 0x004B81C4
		public static bool CheckWaterDeath(int type, int style)
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, style, 0);
			if (tileData == null)
			{
				return Main.tileWaterDeath[type];
			}
			return tileData.WaterDeath;
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x004B9FEC File Offset: 0x004B81EC
		public static bool CheckWaterDeath(Tile checkTile)
		{
			if (!checkTile.active())
			{
				return false;
			}
			TileObjectData tileData = TileObjectData.GetTileData(checkTile);
			if (tileData == null)
			{
				return Main.tileWaterDeath[(int)checkTile.type];
			}
			return tileData.WaterDeath;
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x004BA020 File Offset: 0x004B8220
		public static bool CheckLavaDeath(int type, int style)
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, style, 0);
			if (tileData == null)
			{
				return Main.tileLavaDeath[type];
			}
			return tileData.LavaDeath;
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x004BA048 File Offset: 0x004B8248
		public static bool CheckLavaDeath(Tile checkTile)
		{
			if (!checkTile.active())
			{
				return false;
			}
			TileObjectData tileData = TileObjectData.GetTileData(checkTile);
			if (tileData == null)
			{
				return Main.tileLavaDeath[(int)checkTile.type];
			}
			return tileData.LavaDeath;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x004BA07C File Offset: 0x004B827C
		public static int PlatformFrameWidth()
		{
			return TileObjectData._data[19].CoordinateFullWidth;
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x004BA090 File Offset: 0x004B8290
		public static TileObjectData GetTileData(int type, int style, int alternate = 0)
		{
			if (type < 0 || type >= TileObjectData._data.Count)
			{
				throw new ArgumentOutOfRangeException("Function called with a bad type argument");
			}
			if (style < 0)
			{
				throw new ArgumentOutOfRangeException("Function called with a bad style argument");
			}
			TileObjectData tileObjectData = TileObjectData._data[type];
			if (tileObjectData == null)
			{
				return null;
			}
			List<TileObjectData> subTiles = tileObjectData.SubTiles;
			if (subTiles != null && style < subTiles.Count)
			{
				TileObjectData tileObjectData2 = subTiles[style];
				if (tileObjectData2 != null)
				{
					tileObjectData = tileObjectData2;
				}
			}
			alternate--;
			List<TileObjectData> alternates = tileObjectData.Alternates;
			if (alternates != null && alternate >= 0 && alternate < alternates.Count)
			{
				TileObjectData tileObjectData3 = alternates[alternate];
				if (tileObjectData3 != null)
				{
					tileObjectData = tileObjectData3;
				}
			}
			return tileObjectData;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x004BA128 File Offset: 0x004B8328
		public static bool TryGetTileBounds(int x, int y, out Rectangle bounds)
		{
			bounds = new Rectangle(x, y, 1, 1);
			Tile tile = Main.tile[x, y];
			TileObjectData tileData = TileObjectData.GetTileData(tile);
			if (tileData == null)
			{
				return false;
			}
			int num = (int)tile.frameX / tileData.CoordinateFullWidth;
			int num2 = (int)tile.frameY / tileData.CoordinateFullHeight;
			int i = (int)tile.frameX - num * tileData.CoordinateFullWidth;
			int num3 = (int)tile.frameY - num2 * tileData.CoordinateFullHeight;
			int coordinateWidth = tileData.CoordinateWidth;
			while (i >= coordinateWidth)
			{
				i -= coordinateWidth;
				bounds.X--;
			}
			int[] coordinateHeights = tileData.CoordinateHeights;
			for (int j = 0; j < coordinateHeights.Length; j++)
			{
				if (num3 >= coordinateHeights[j])
				{
					num3 -= coordinateHeights[j];
					bounds.Y--;
				}
			}
			bounds.Width = tileData.Width;
			bounds.Height = tileData.Height;
			return true;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x004BA210 File Offset: 0x004B8410
		public static TileObjectData GetTileData(Tile getTile)
		{
			if (getTile == null || !getTile.active())
			{
				return null;
			}
			int type = (int)getTile.type;
			if (type < 0 || type >= TileObjectData._data.Count)
			{
				throw new ArgumentOutOfRangeException("Function called with a bad tile type");
			}
			TileObjectData tileObjectData = TileObjectData._data[type];
			if (tileObjectData == null)
			{
				return null;
			}
			int num = (int)getTile.frameX / tileObjectData.CoordinateFullWidth;
			int num2 = (int)getTile.frameY / tileObjectData.CoordinateFullHeight;
			int num3 = tileObjectData.StyleWrapLimit;
			if (num3 == 0)
			{
				num3 = 1;
			}
			int num4;
			if (tileObjectData.StyleHorizontal)
			{
				num4 = num2 * num3 + num;
			}
			else
			{
				num4 = num * num3 + num2;
			}
			int num5 = num4 / tileObjectData.StyleMultiplier;
			int num6 = num4 % tileObjectData.StyleMultiplier;
			int styleLineSkip = tileObjectData.StyleLineSkip;
			if (styleLineSkip > 1)
			{
				if (tileObjectData.StyleHorizontal)
				{
					num5 = num2 / styleLineSkip * num3 + num;
					num6 = num2 % styleLineSkip;
				}
				else
				{
					num5 = num / styleLineSkip * num3 + num2;
					num6 = num % styleLineSkip;
				}
			}
			if (tileObjectData.SubTiles != null && num5 >= 0 && num5 < tileObjectData.SubTiles.Count)
			{
				TileObjectData tileObjectData2 = tileObjectData.SubTiles[num5];
				if (tileObjectData2 != null)
				{
					tileObjectData = tileObjectData2;
				}
			}
			if (tileObjectData._alternates != null)
			{
				for (int i = 0; i < tileObjectData.Alternates.Count; i++)
				{
					TileObjectData tileObjectData3 = tileObjectData.Alternates[i];
					if (tileObjectData3 != null && num6 >= tileObjectData3.Style && num6 <= tileObjectData3.Style + tileObjectData3.RandomStyleRange)
					{
						return tileObjectData3;
					}
				}
			}
			return tileObjectData;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x004BA380 File Offset: 0x004B8580
		public static void SyncObjectPlacement(int tileX, int tileY, int type, int style, int dir)
		{
			NetMessage.SendData(17, -1, -1, null, 1, (float)tileX, (float)tileY, (float)type, style, 0, 0);
			TileObjectData.GetTileData(type, style, 0);
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x004BA3AC File Offset: 0x004B85AC
		public static bool CallPostPlacementPlayerHook(int tileX, int tileY, int type, int style, int dir, int alternate, TileObject data)
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, style, data.alternate);
			if (tileData == null || tileData._placementHooks == null || tileData._placementHooks.postPlaceMyPlayer.hook == null)
			{
				return false;
			}
			PlacementHook postPlaceMyPlayer = tileData._placementHooks.postPlaceMyPlayer;
			if (postPlaceMyPlayer.processedCoordinates)
			{
				tileX -= (int)tileData.Origin.X;
				tileY -= (int)tileData.Origin.Y;
			}
			return postPlaceMyPlayer.hook(tileX, tileY, type, style, dir, data.alternate) == postPlaceMyPlayer.badReturn;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x004BA43C File Offset: 0x004B863C
		public static void OriginToTopLeft(int type, int style, ref Point16 baseCoords)
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, style, 0);
			if (tileData == null)
			{
				return;
			}
			baseCoords = new Point16((int)(baseCoords.X - tileData.Origin.X), (int)(baseCoords.Y - tileData.Origin.Y));
		}

		// Token: 0x04000FFD RID: 4093
		private TileObjectData _parent;

		// Token: 0x04000FFE RID: 4094
		private bool _linkedAlternates;

		// Token: 0x04000FFF RID: 4095
		private bool _usesCustomCanPlace;

		// Token: 0x04001000 RID: 4096
		private TileObjectAlternatesModule _alternates;

		// Token: 0x04001001 RID: 4097
		private AnchorDataModule _anchor;

		// Token: 0x04001002 RID: 4098
		private AnchorTypesModule _anchorTiles;

		// Token: 0x04001003 RID: 4099
		private LiquidDeathModule _liquidDeath;

		// Token: 0x04001004 RID: 4100
		private LiquidPlacementModule _liquidPlacement;

		// Token: 0x04001005 RID: 4101
		private TilePlacementHooksModule _placementHooks;

		// Token: 0x04001006 RID: 4102
		private TileObjectSubTilesModule _subTiles;

		// Token: 0x04001007 RID: 4103
		private TileObjectDrawModule _tileObjectDraw;

		// Token: 0x04001008 RID: 4104
		private TileObjectStyleModule _tileObjectStyle;

		// Token: 0x04001009 RID: 4105
		private TileObjectBaseModule _tileObjectBase;

		// Token: 0x0400100A RID: 4106
		private TileObjectCoordinatesModule _tileObjectCoords;

		// Token: 0x0400100B RID: 4107
		private bool _hasOwnAlternates;

		// Token: 0x0400100C RID: 4108
		private bool _hasOwnAnchor;

		// Token: 0x0400100D RID: 4109
		private bool _hasOwnAnchorTiles;

		// Token: 0x0400100E RID: 4110
		private bool _hasOwnLiquidDeath;

		// Token: 0x0400100F RID: 4111
		private bool _hasOwnLiquidPlacement;

		// Token: 0x04001010 RID: 4112
		private bool _hasOwnPlacementHooks;

		// Token: 0x04001011 RID: 4113
		private bool _hasOwnSubTiles;

		// Token: 0x04001012 RID: 4114
		private bool _hasOwnTileObjectBase;

		// Token: 0x04001013 RID: 4115
		private bool _hasOwnTileObjectDraw;

		// Token: 0x04001014 RID: 4116
		private bool _hasOwnTileObjectStyle;

		// Token: 0x04001015 RID: 4117
		private bool _hasOwnTileObjectCoords;

		// Token: 0x04001016 RID: 4118
		private static List<TileObjectData> _data;

		// Token: 0x04001017 RID: 4119
		private static TileObjectData _baseObject;

		// Token: 0x04001018 RID: 4120
		private static bool readOnlyData;

		// Token: 0x04001019 RID: 4121
		private static TileObjectData newTile;

		// Token: 0x0400101A RID: 4122
		private static TileObjectData newSubTile;

		// Token: 0x0400101B RID: 4123
		private static TileObjectData newAlternate;

		// Token: 0x0400101C RID: 4124
		private static TileObjectData StyleSwitch;

		// Token: 0x0400101D RID: 4125
		private static TileObjectData StyleTorch;

		// Token: 0x0400101E RID: 4126
		private static TileObjectData Style4x2;

		// Token: 0x0400101F RID: 4127
		private static TileObjectData Style2x2;

		// Token: 0x04001020 RID: 4128
		private static TileObjectData Style1x2;

		// Token: 0x04001021 RID: 4129
		private static TileObjectData Style1x1;

		// Token: 0x04001022 RID: 4130
		private static TileObjectData StyleAlch;

		// Token: 0x04001023 RID: 4131
		private static TileObjectData StyleDye;

		// Token: 0x04001024 RID: 4132
		private static TileObjectData Style2x1;

		// Token: 0x04001025 RID: 4133
		private static TileObjectData Style6x3;

		// Token: 0x04001026 RID: 4134
		private static TileObjectData StyleSmallCage;

		// Token: 0x04001027 RID: 4135
		private static TileObjectData StyleOnTable1x1;

		// Token: 0x04001028 RID: 4136
		private static TileObjectData Style1x2Top;

		// Token: 0x04001029 RID: 4137
		private static TileObjectData Style1xX;

		// Token: 0x0400102A RID: 4138
		private static TileObjectData Style2xX;

		// Token: 0x0400102B RID: 4139
		private static TileObjectData Style3x2;

		// Token: 0x0400102C RID: 4140
		private static TileObjectData Style3x3;

		// Token: 0x0400102D RID: 4141
		private static TileObjectData Style3x4;

		// Token: 0x0400102E RID: 4142
		private static TileObjectData Style4x4;

		// Token: 0x0400102F RID: 4143
		private static TileObjectData Style5x4;

		// Token: 0x04001030 RID: 4144
		private static TileObjectData Style3x3Wall;
	}
}
