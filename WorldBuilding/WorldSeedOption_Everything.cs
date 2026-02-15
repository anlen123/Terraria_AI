using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A1 RID: 161
	public class WorldSeedOption_Everything : AWorldGenerationOption
	{
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600171A RID: 5914 RVA: 0x004DCE9F File Offset: 0x004DB09F
		protected override string KeyName
		{
			get
			{
				return "Seed_Everything";
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x004DCEA6 File Offset: 0x004DB0A6
		public override string ServerConfigName
		{
			get
			{
				return "zenith";
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x004DCEB0 File Offset: 0x004DB0B0
		public List<AWorldGenerationOption> Dependencies
		{
			get
			{
				if (this._dependencies == null)
				{
					this._dependencies = new List<AWorldGenerationOption>
					{
						WorldGenerationOptions.Get<WorldSeedOption_Remix>(),
						WorldGenerationOptions.Get<WorldSeedOption_Drunk>(),
						WorldGenerationOptions.Get<WorldSeedOption_NotTheBees>(),
						WorldGenerationOptions.Get<WorldSeedOption_NoTraps>(),
						WorldGenerationOptions.Get<WorldSeedOption_DontStarve>(),
						WorldGenerationOptions.Get<WorldSeedOption_Anniversary>(),
						WorldGenerationOptions.Get<WorldSeedOption_ForTheWorthy>()
					};
				}
				return this._dependencies;
			}
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x004DCF23 File Offset: 0x004DB123
		public WorldSeedOption_Everything()
		{
			base.SpecialSeedNames = new string[]
			{
				"getfixedboi"
			};
			base.SpecialSeedValues = new int[0];
			AWorldGenerationOption.OnOptionStateChanged += this.UpdateDependentState;
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x004DCF5C File Offset: 0x004DB15C
		private void UpdateDependentState(AWorldGenerationOption changed)
		{
			if (this.Dependencies.Contains(changed) && changed.Enabled != base.Enabled)
			{
				base.Enabled = this.Dependencies.All((AWorldGenerationOption d) => d.Enabled);
			}
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x004DCFB8 File Offset: 0x004DB1B8
		protected override void OnEnabledStateChanged()
		{
			if (!base.Enabled)
			{
				if (this.Dependencies.Any((AWorldGenerationOption d) => !d.Enabled))
				{
					return;
				}
			}
			foreach (AWorldGenerationOption aworldGenerationOption in this.Dependencies)
			{
				aworldGenerationOption.Enabled = base.Enabled;
			}
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x004DD044 File Offset: 0x004DB244
		public override UIElement ProvideUIElement()
		{
			UIImageFramed image = new UIImageFramed(base.Texture, base.Texture.Frame(7, 16, 0, 0, 0, 0))
			{
				Left = StyleDimension.FromPixels(-1f)
			};
			int glitchFrameCounter = 0;
			int glitchFrame = 0;
			int glitchVariation = 0;
			image.OnUpdate += delegate(UIElement _)
			{
				int minValue = 3;
				int num = 3;
				if (glitchFrame == 0)
				{
					minValue = 15;
					num = 120;
				}
				int num2 = glitchFrameCounter + 1;
				glitchFrameCounter = num2;
				if (num2 >= Main.rand.Next(minValue, num + 1))
				{
					glitchFrameCounter = 0;
					glitchFrame = (glitchFrame + 1) % 16;
					if ((glitchFrame == 4 || glitchFrame == 8 || glitchFrame == 12) && Main.rand.Next(3) == 0)
					{
						glitchVariation = Main.rand.Next(7);
					}
				}
				image.SetFrame(7, 16, glitchVariation, glitchFrame, 0, 0);
			};
			return image;
		}

		// Token: 0x040011B9 RID: 4537
		protected List<AWorldGenerationOption> _dependencies;
	}
}
