using System;

namespace Terraria.Map
{
	// Token: 0x02000182 RID: 386
	public struct MapTile
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x0050EB5F File Offset: 0x0050CD5F
		// (set) Token: 0x06001E4E RID: 7758 RVA: 0x0050EB70 File Offset: 0x0050CD70
		public bool IsChanged
		{
			get
			{
				return (this._extraData & 128) > 0;
			}
			set
			{
				if (value)
				{
					this._extraData |= 128;
					return;
				}
				this._extraData &= 127;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001E4F RID: 7759 RVA: 0x0050EB99 File Offset: 0x0050CD99
		// (set) Token: 0x06001E50 RID: 7760 RVA: 0x0050EBA7 File Offset: 0x0050CDA7
		public bool UpdateQueued
		{
			get
			{
				return (this._extraData & 64) > 0;
			}
			set
			{
				if (value)
				{
					this._extraData |= 64;
					return;
				}
				this._extraData &= 191;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x0050EBD0 File Offset: 0x0050CDD0
		// (set) Token: 0x06001E52 RID: 7762 RVA: 0x0050EBDC File Offset: 0x0050CDDC
		public byte Color
		{
			get
			{
				return this._extraData & 31;
			}
			set
			{
				this._extraData = (byte)(((int)this._extraData & -32) | (int)(value & 31));
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x0050EBF3 File Offset: 0x0050CDF3
		private MapTile(ushort type, byte light, byte extraData)
		{
			this.Type = type;
			this.Light = light;
			this._extraData = extraData;
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x0050EC0A File Offset: 0x0050CE0A
		public bool Equals(MapTile other)
		{
			return this.Light == other.Light && this.Type == other.Type && this.Color == other.Color;
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x0050EC39 File Offset: 0x0050CE39
		public bool EqualsWithoutLight(MapTile other)
		{
			return this.Type == other.Type && this.Color == other.Color;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x0050EC5A File Offset: 0x0050CE5A
		public void Clear()
		{
			this.Type = 0;
			this.Light = 0;
			this._extraData = 0;
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x0050EC71 File Offset: 0x0050CE71
		public MapTile WithLight(byte light)
		{
			return new MapTile(this.Type, light, this._extraData | 128);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x0050EC8C File Offset: 0x0050CE8C
		public static MapTile Create(ushort type, byte light, byte color)
		{
			return new MapTile(type, light, color | 128);
		}

		// Token: 0x040016BB RID: 5819
		public ushort Type;

		// Token: 0x040016BC RID: 5820
		public byte Light;

		// Token: 0x040016BD RID: 5821
		private byte _extraData;
	}
}
