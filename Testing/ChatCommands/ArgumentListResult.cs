using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.Testing.ChatCommands
{
	// Token: 0x0200011B RID: 283
	public class ArgumentListResult : IEnumerable<string>, IEnumerable
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001B3D RID: 6973 RVA: 0x004F9A93 File Offset: 0x004F7C93
		public int Count
		{
			get
			{
				return this._results.Count;
			}
		}

		// Token: 0x170002DB RID: 731
		public string this[int index]
		{
			get
			{
				return this._results[index];
			}
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x004F9AAE File Offset: 0x004F7CAE
		public ArgumentListResult(IEnumerable<string> results)
		{
			this._results = results.ToList<string>();
			this.IsValid = true;
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x004F9AC9 File Offset: 0x004F7CC9
		private ArgumentListResult(bool isValid)
		{
			this._results = new List<string>();
			this.IsValid = isValid;
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x004F9AE3 File Offset: 0x004F7CE3
		public IEnumerator<string> GetEnumerator()
		{
			return this._results.GetEnumerator();
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x004F9AF5 File Offset: 0x004F7CF5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04001543 RID: 5443
		public static readonly ArgumentListResult Empty = new ArgumentListResult(true);

		// Token: 0x04001544 RID: 5444
		public static readonly ArgumentListResult Invalid = new ArgumentListResult(false);

		// Token: 0x04001545 RID: 5445
		public readonly bool IsValid;

		// Token: 0x04001546 RID: 5446
		private readonly List<string> _results;
	}
}
