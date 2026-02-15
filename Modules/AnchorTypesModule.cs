using System;

namespace Terraria.Modules
{
	// Token: 0x0200005F RID: 95
	public class AnchorTypesModule
	{
		// Token: 0x0600144B RID: 5195 RVA: 0x004BA514 File Offset: 0x004B8714
		public AnchorTypesModule(AnchorTypesModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				this.tileValid = null;
				this.tileInvalid = null;
				this.tileAlternates = null;
				this.wallValid = null;
				return;
			}
			if (copyFrom.tileValid == null)
			{
				this.tileValid = null;
			}
			else
			{
				this.tileValid = new int[copyFrom.tileValid.Length];
				Array.Copy(copyFrom.tileValid, this.tileValid, this.tileValid.Length);
			}
			if (copyFrom.tileInvalid == null)
			{
				this.tileInvalid = null;
			}
			else
			{
				this.tileInvalid = new int[copyFrom.tileInvalid.Length];
				Array.Copy(copyFrom.tileInvalid, this.tileInvalid, this.tileInvalid.Length);
			}
			if (copyFrom.tileAlternates == null)
			{
				this.tileAlternates = null;
			}
			else
			{
				this.tileAlternates = new int[copyFrom.tileAlternates.Length];
				Array.Copy(copyFrom.tileAlternates, this.tileAlternates, this.tileAlternates.Length);
			}
			if (copyFrom.wallValid == null)
			{
				this.wallValid = null;
				return;
			}
			this.wallValid = new int[copyFrom.wallValid.Length];
			Array.Copy(copyFrom.wallValid, this.wallValid, this.wallValid.Length);
		}

		// Token: 0x04001036 RID: 4150
		public int[] tileValid;

		// Token: 0x04001037 RID: 4151
		public int[] tileInvalid;

		// Token: 0x04001038 RID: 4152
		public int[] tileAlternates;

		// Token: 0x04001039 RID: 4153
		public int[] wallValid;
	}
}
