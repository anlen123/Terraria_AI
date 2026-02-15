using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.WorldBuilding
{
	// Token: 0x02000098 RID: 152
	public abstract class AWorldGenerationOption
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060016EB RID: 5867 RVA: 0x004DCAB8 File Offset: 0x004DACB8
		// (remove) Token: 0x060016EC RID: 5868 RVA: 0x004DCAEC File Offset: 0x004DACEC
		protected static event Action<AWorldGenerationOption> OnOptionStateChanged;

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x004DCB1F File Offset: 0x004DAD1F
		// (set) Token: 0x060016EE RID: 5870 RVA: 0x004DCB27 File Offset: 0x004DAD27
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				if (this._enabled == value)
				{
					return;
				}
				this._enabled = value;
				this.OnEnabledStateChanged();
				AWorldGenerationOption.OnOptionStateChanged(this);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060016EF RID: 5871
		protected abstract string KeyName { get; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060016F0 RID: 5872
		public abstract string ServerConfigName { get; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x004DCB4B File Offset: 0x004DAD4B
		// (set) Token: 0x060016F2 RID: 5874 RVA: 0x004DCB53 File Offset: 0x004DAD53
		public string[] SpecialSeedNames { get; protected set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x004DCB5C File Offset: 0x004DAD5C
		// (set) Token: 0x060016F4 RID: 5876 RVA: 0x004DCB64 File Offset: 0x004DAD64
		public int[] SpecialSeedValues { get; protected set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060016F5 RID: 5877 RVA: 0x004DCB6D File Offset: 0x004DAD6D
		// (set) Token: 0x060016F6 RID: 5878 RVA: 0x004DCB75 File Offset: 0x004DAD75
		public LocalizedText Description { get; private set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060016F7 RID: 5879 RVA: 0x004DCB7E File Offset: 0x004DAD7E
		// (set) Token: 0x060016F8 RID: 5880 RVA: 0x004DCB86 File Offset: 0x004DAD86
		public LocalizedText Title { get; private set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x004DCB8F File Offset: 0x004DAD8F
		// (set) Token: 0x060016FA RID: 5882 RVA: 0x004DCB97 File Offset: 0x004DAD97
		private protected Asset<Texture2D> Texture { protected get; private set; }

		// Token: 0x060016FB RID: 5883 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void OnEnabledStateChanged()
		{
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x004DCBA0 File Offset: 0x004DADA0
		public void Load()
		{
			if (this.Texture != null)
			{
				return;
			}
			this.Description = Language.GetText("UI." + this.KeyName);
			this.Title = Language.GetText("UI." + this.KeyName + "_Title");
			if (Main.dedServ)
			{
				return;
			}
			this.Texture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/" + this.KeyName, 1);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x004DCC1A File Offset: 0x004DAE1A
		public virtual UIElement ProvideUIElement()
		{
			return new UIImage(this.Texture)
			{
				Left = StyleDimension.FromPixels(-1f)
			};
		}

		// Token: 0x040011B2 RID: 4530
		private bool _enabled;

		// Token: 0x040011B3 RID: 4531
		public bool AutoGenEnabled;
	}
}
