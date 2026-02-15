using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A8 RID: 168
	public class GenerationProgress
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x004DD2C8 File Offset: 0x004DB4C8
		// (set) Token: 0x06001738 RID: 5944 RVA: 0x004DD2E0 File Offset: 0x004DB4E0
		public string Message
		{
			get
			{
				return string.Format(this._message, this.Value);
			}
			set
			{
				this._message = value.Replace("%", "{0:0.0%}");
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x004DD2F8 File Offset: 0x004DB4F8
		// (set) Token: 0x0600173A RID: 5946 RVA: 0x004DD300 File Offset: 0x004DB500
		public string MessageNoFormatting
		{
			get
			{
				return this._message;
			}
			set
			{
				this._message = value;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x004DD329 File Offset: 0x004DB529
		// (set) Token: 0x0600173B RID: 5947 RVA: 0x004DD309 File Offset: 0x004DB509
		public double Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = Utils.Clamp<double>(value, 0.0, 1.0);
			}
		}

		// Token: 0x17000284 RID: 644
		// (set) Token: 0x0600173D RID: 5949 RVA: 0x004DD331 File Offset: 0x004DB531
		public double TotalWeightedProgress
		{
			set
			{
				this._totalWeightedProgress = value;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x004DD33A File Offset: 0x004DB53A
		public double TotalProgress
		{
			get
			{
				if (this.TotalWeight == 0.0)
				{
					return 0.0;
				}
				return (this.Value * this.CurrentPassWeight + this._totalWeightedProgress) / this.TotalWeight;
			}
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x004DD372 File Offset: 0x004DB572
		public void Set(double value)
		{
			this.Value = value;
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x004DD37B File Offset: 0x004DB57B
		public void Set(double value, double min, double max)
		{
			this.Value = min + value * (max - min);
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x004DD38A File Offset: 0x004DB58A
		public void Start(double weight)
		{
			this.CurrentPassWeight = weight;
			this._value = 0.0;
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x004DD3A2 File Offset: 0x004DB5A2
		public void End()
		{
			this._totalWeightedProgress += this.CurrentPassWeight;
			this._value = 0.0;
		}

		// Token: 0x040011C1 RID: 4545
		private string _message = "";

		// Token: 0x040011C2 RID: 4546
		private double _value;

		// Token: 0x040011C3 RID: 4547
		private double _totalWeightedProgress;

		// Token: 0x040011C4 RID: 4548
		public double TotalWeight;

		// Token: 0x040011C5 RID: 4549
		public double CurrentPassWeight = 1.0;
	}
}
