using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Terraria.GameInput
{
	// Token: 0x02000090 RID: 144
	public class TriggersSet
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x004D4E64 File Offset: 0x004D3064
		// (set) Token: 0x060015F7 RID: 5623 RVA: 0x004D4E76 File Offset: 0x004D3076
		public bool MouseLeft
		{
			get
			{
				return this.KeyStatus["MouseLeft"];
			}
			set
			{
				this.KeyStatus["MouseLeft"] = value;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060015F8 RID: 5624 RVA: 0x004D4E89 File Offset: 0x004D3089
		// (set) Token: 0x060015F9 RID: 5625 RVA: 0x004D4E9B File Offset: 0x004D309B
		public bool MouseRight
		{
			get
			{
				return this.KeyStatus["MouseRight"];
			}
			set
			{
				this.KeyStatus["MouseRight"] = value;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060015FA RID: 5626 RVA: 0x004D4EAE File Offset: 0x004D30AE
		// (set) Token: 0x060015FB RID: 5627 RVA: 0x004D4EC0 File Offset: 0x004D30C0
		public bool Up
		{
			get
			{
				return this.KeyStatus["Up"];
			}
			set
			{
				this.KeyStatus["Up"] = value;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060015FC RID: 5628 RVA: 0x004D4ED3 File Offset: 0x004D30D3
		// (set) Token: 0x060015FD RID: 5629 RVA: 0x004D4EE5 File Offset: 0x004D30E5
		public bool Down
		{
			get
			{
				return this.KeyStatus["Down"];
			}
			set
			{
				this.KeyStatus["Down"] = value;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x004D4EF8 File Offset: 0x004D30F8
		// (set) Token: 0x060015FF RID: 5631 RVA: 0x004D4F0A File Offset: 0x004D310A
		public bool Left
		{
			get
			{
				return this.KeyStatus["Left"];
			}
			set
			{
				this.KeyStatus["Left"] = value;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x004D4F1D File Offset: 0x004D311D
		// (set) Token: 0x06001601 RID: 5633 RVA: 0x004D4F2F File Offset: 0x004D312F
		public bool Right
		{
			get
			{
				return this.KeyStatus["Right"];
			}
			set
			{
				this.KeyStatus["Right"] = value;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x004D4F42 File Offset: 0x004D3142
		// (set) Token: 0x06001603 RID: 5635 RVA: 0x004D4F54 File Offset: 0x004D3154
		public bool Jump
		{
			get
			{
				return this.KeyStatus["Jump"];
			}
			set
			{
				this.KeyStatus["Jump"] = value;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x004D4F67 File Offset: 0x004D3167
		// (set) Token: 0x06001605 RID: 5637 RVA: 0x004D4F79 File Offset: 0x004D3179
		public bool Throw
		{
			get
			{
				return this.KeyStatus["Throw"];
			}
			set
			{
				this.KeyStatus["Throw"] = value;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x004D4F8C File Offset: 0x004D318C
		// (set) Token: 0x06001607 RID: 5639 RVA: 0x004D4F9E File Offset: 0x004D319E
		public bool Inventory
		{
			get
			{
				return this.KeyStatus["Inventory"];
			}
			set
			{
				this.KeyStatus["Inventory"] = value;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x004D4FB1 File Offset: 0x004D31B1
		// (set) Token: 0x06001609 RID: 5641 RVA: 0x004D4FC3 File Offset: 0x004D31C3
		public bool Grapple
		{
			get
			{
				return this.KeyStatus["Grapple"];
			}
			set
			{
				this.KeyStatus["Grapple"] = value;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600160A RID: 5642 RVA: 0x004D4FD6 File Offset: 0x004D31D6
		// (set) Token: 0x0600160B RID: 5643 RVA: 0x004D4FE8 File Offset: 0x004D31E8
		public bool SmartSelect
		{
			get
			{
				return this.KeyStatus["SmartSelect"];
			}
			set
			{
				this.KeyStatus["SmartSelect"] = value;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x004D4FFB File Offset: 0x004D31FB
		// (set) Token: 0x0600160D RID: 5645 RVA: 0x004D500D File Offset: 0x004D320D
		public bool SmartCursor
		{
			get
			{
				return this.KeyStatus["SmartCursor"];
			}
			set
			{
				this.KeyStatus["SmartCursor"] = value;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x004D5020 File Offset: 0x004D3220
		// (set) Token: 0x0600160F RID: 5647 RVA: 0x004D5032 File Offset: 0x004D3232
		public bool QuickMount
		{
			get
			{
				return this.KeyStatus["QuickMount"];
			}
			set
			{
				this.KeyStatus["QuickMount"] = value;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001610 RID: 5648 RVA: 0x004D5045 File Offset: 0x004D3245
		// (set) Token: 0x06001611 RID: 5649 RVA: 0x004D5057 File Offset: 0x004D3257
		public bool QuickHeal
		{
			get
			{
				return this.KeyStatus["QuickHeal"];
			}
			set
			{
				this.KeyStatus["QuickHeal"] = value;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x004D506A File Offset: 0x004D326A
		// (set) Token: 0x06001613 RID: 5651 RVA: 0x004D507C File Offset: 0x004D327C
		public bool QuickMana
		{
			get
			{
				return this.KeyStatus["QuickMana"];
			}
			set
			{
				this.KeyStatus["QuickMana"] = value;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06001614 RID: 5652 RVA: 0x004D508F File Offset: 0x004D328F
		// (set) Token: 0x06001615 RID: 5653 RVA: 0x004D50A1 File Offset: 0x004D32A1
		public bool QuickBuff
		{
			get
			{
				return this.KeyStatus["QuickBuff"];
			}
			set
			{
				this.KeyStatus["QuickBuff"] = value;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06001616 RID: 5654 RVA: 0x004D50B4 File Offset: 0x004D32B4
		// (set) Token: 0x06001617 RID: 5655 RVA: 0x004D50C6 File Offset: 0x004D32C6
		public bool Loadout1
		{
			get
			{
				return this.KeyStatus["Loadout1"];
			}
			set
			{
				this.KeyStatus["Loadout1"] = value;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06001618 RID: 5656 RVA: 0x004D50D9 File Offset: 0x004D32D9
		// (set) Token: 0x06001619 RID: 5657 RVA: 0x004D50EB File Offset: 0x004D32EB
		public bool Loadout2
		{
			get
			{
				return this.KeyStatus["Loadout2"];
			}
			set
			{
				this.KeyStatus["Loadout2"] = value;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600161A RID: 5658 RVA: 0x004D50FE File Offset: 0x004D32FE
		// (set) Token: 0x0600161B RID: 5659 RVA: 0x004D5110 File Offset: 0x004D3310
		public bool Loadout3
		{
			get
			{
				return this.KeyStatus["Loadout3"];
			}
			set
			{
				this.KeyStatus["Loadout3"] = value;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x004D5123 File Offset: 0x004D3323
		// (set) Token: 0x0600161D RID: 5661 RVA: 0x004D5135 File Offset: 0x004D3335
		public bool Dash
		{
			get
			{
				return this.KeyStatus["Dash"];
			}
			set
			{
				this.KeyStatus["Dash"] = value;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600161E RID: 5662 RVA: 0x004D5148 File Offset: 0x004D3348
		// (set) Token: 0x0600161F RID: 5663 RVA: 0x004D515A File Offset: 0x004D335A
		public bool ArmorSetAbility
		{
			get
			{
				return this.KeyStatus["ArmorSetAbility"];
			}
			set
			{
				this.KeyStatus["ArmorSetAbility"] = value;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06001620 RID: 5664 RVA: 0x004D516D File Offset: 0x004D336D
		// (set) Token: 0x06001621 RID: 5665 RVA: 0x004D517F File Offset: 0x004D337F
		public bool NextLoadout
		{
			get
			{
				return this.KeyStatus["NextLoadout"];
			}
			set
			{
				this.KeyStatus["NextLoadout"] = value;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06001622 RID: 5666 RVA: 0x004D5192 File Offset: 0x004D3392
		// (set) Token: 0x06001623 RID: 5667 RVA: 0x004D51A4 File Offset: 0x004D33A4
		public bool PreviousLoadout
		{
			get
			{
				return this.KeyStatus["PreviousLoadout"];
			}
			set
			{
				this.KeyStatus["PreviousLoadout"] = value;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06001624 RID: 5668 RVA: 0x004D51B7 File Offset: 0x004D33B7
		// (set) Token: 0x06001625 RID: 5669 RVA: 0x004D51C9 File Offset: 0x004D33C9
		public bool MapZoomIn
		{
			get
			{
				return this.KeyStatus["MapZoomIn"];
			}
			set
			{
				this.KeyStatus["MapZoomIn"] = value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06001626 RID: 5670 RVA: 0x004D51DC File Offset: 0x004D33DC
		// (set) Token: 0x06001627 RID: 5671 RVA: 0x004D51EE File Offset: 0x004D33EE
		public bool MapZoomOut
		{
			get
			{
				return this.KeyStatus["MapZoomOut"];
			}
			set
			{
				this.KeyStatus["MapZoomOut"] = value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06001628 RID: 5672 RVA: 0x004D5201 File Offset: 0x004D3401
		// (set) Token: 0x06001629 RID: 5673 RVA: 0x004D5213 File Offset: 0x004D3413
		public bool MapAlphaUp
		{
			get
			{
				return this.KeyStatus["MapAlphaUp"];
			}
			set
			{
				this.KeyStatus["MapAlphaUp"] = value;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600162A RID: 5674 RVA: 0x004D5226 File Offset: 0x004D3426
		// (set) Token: 0x0600162B RID: 5675 RVA: 0x004D5238 File Offset: 0x004D3438
		public bool MapAlphaDown
		{
			get
			{
				return this.KeyStatus["MapAlphaDown"];
			}
			set
			{
				this.KeyStatus["MapAlphaDown"] = value;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600162C RID: 5676 RVA: 0x004D524B File Offset: 0x004D344B
		// (set) Token: 0x0600162D RID: 5677 RVA: 0x004D525D File Offset: 0x004D345D
		public bool MapFull
		{
			get
			{
				return this.KeyStatus["MapFull"];
			}
			set
			{
				this.KeyStatus["MapFull"] = value;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600162E RID: 5678 RVA: 0x004D5270 File Offset: 0x004D3470
		// (set) Token: 0x0600162F RID: 5679 RVA: 0x004D5282 File Offset: 0x004D3482
		public bool MapStyle
		{
			get
			{
				return this.KeyStatus["MapStyle"];
			}
			set
			{
				this.KeyStatus["MapStyle"] = value;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06001630 RID: 5680 RVA: 0x004D5295 File Offset: 0x004D3495
		// (set) Token: 0x06001631 RID: 5681 RVA: 0x004D52A7 File Offset: 0x004D34A7
		public bool Hotbar1
		{
			get
			{
				return this.KeyStatus["Hotbar1"];
			}
			set
			{
				this.KeyStatus["Hotbar1"] = value;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06001632 RID: 5682 RVA: 0x004D52BA File Offset: 0x004D34BA
		// (set) Token: 0x06001633 RID: 5683 RVA: 0x004D52CC File Offset: 0x004D34CC
		public bool Hotbar2
		{
			get
			{
				return this.KeyStatus["Hotbar2"];
			}
			set
			{
				this.KeyStatus["Hotbar2"] = value;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06001634 RID: 5684 RVA: 0x004D52DF File Offset: 0x004D34DF
		// (set) Token: 0x06001635 RID: 5685 RVA: 0x004D52F1 File Offset: 0x004D34F1
		public bool Hotbar3
		{
			get
			{
				return this.KeyStatus["Hotbar3"];
			}
			set
			{
				this.KeyStatus["Hotbar3"] = value;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06001636 RID: 5686 RVA: 0x004D5304 File Offset: 0x004D3504
		// (set) Token: 0x06001637 RID: 5687 RVA: 0x004D5316 File Offset: 0x004D3516
		public bool Hotbar4
		{
			get
			{
				return this.KeyStatus["Hotbar4"];
			}
			set
			{
				this.KeyStatus["Hotbar4"] = value;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06001638 RID: 5688 RVA: 0x004D5329 File Offset: 0x004D3529
		// (set) Token: 0x06001639 RID: 5689 RVA: 0x004D533B File Offset: 0x004D353B
		public bool Hotbar5
		{
			get
			{
				return this.KeyStatus["Hotbar5"];
			}
			set
			{
				this.KeyStatus["Hotbar5"] = value;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600163A RID: 5690 RVA: 0x004D534E File Offset: 0x004D354E
		// (set) Token: 0x0600163B RID: 5691 RVA: 0x004D5360 File Offset: 0x004D3560
		public bool Hotbar6
		{
			get
			{
				return this.KeyStatus["Hotbar6"];
			}
			set
			{
				this.KeyStatus["Hotbar6"] = value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x004D5373 File Offset: 0x004D3573
		// (set) Token: 0x0600163D RID: 5693 RVA: 0x004D5385 File Offset: 0x004D3585
		public bool Hotbar7
		{
			get
			{
				return this.KeyStatus["Hotbar7"];
			}
			set
			{
				this.KeyStatus["Hotbar7"] = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600163E RID: 5694 RVA: 0x004D5398 File Offset: 0x004D3598
		// (set) Token: 0x0600163F RID: 5695 RVA: 0x004D53AA File Offset: 0x004D35AA
		public bool Hotbar8
		{
			get
			{
				return this.KeyStatus["Hotbar8"];
			}
			set
			{
				this.KeyStatus["Hotbar8"] = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x004D53BD File Offset: 0x004D35BD
		// (set) Token: 0x06001641 RID: 5697 RVA: 0x004D53CF File Offset: 0x004D35CF
		public bool Hotbar9
		{
			get
			{
				return this.KeyStatus["Hotbar9"];
			}
			set
			{
				this.KeyStatus["Hotbar9"] = value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x004D53E2 File Offset: 0x004D35E2
		// (set) Token: 0x06001643 RID: 5699 RVA: 0x004D53F4 File Offset: 0x004D35F4
		public bool Hotbar10
		{
			get
			{
				return this.KeyStatus["Hotbar10"];
			}
			set
			{
				this.KeyStatus["Hotbar10"] = value;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06001644 RID: 5700 RVA: 0x004D5407 File Offset: 0x004D3607
		// (set) Token: 0x06001645 RID: 5701 RVA: 0x004D5419 File Offset: 0x004D3619
		public bool HotbarMinus
		{
			get
			{
				return this.KeyStatus["HotbarMinus"];
			}
			set
			{
				this.KeyStatus["HotbarMinus"] = value;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06001646 RID: 5702 RVA: 0x004D542C File Offset: 0x004D362C
		// (set) Token: 0x06001647 RID: 5703 RVA: 0x004D543E File Offset: 0x004D363E
		public bool HotbarPlus
		{
			get
			{
				return this.KeyStatus["HotbarPlus"];
			}
			set
			{
				this.KeyStatus["HotbarPlus"] = value;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001648 RID: 5704 RVA: 0x004D5451 File Offset: 0x004D3651
		// (set) Token: 0x06001649 RID: 5705 RVA: 0x004D5463 File Offset: 0x004D3663
		public bool DpadRadial1
		{
			get
			{
				return this.KeyStatus["DpadRadial1"];
			}
			set
			{
				this.KeyStatus["DpadRadial1"] = value;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x0600164A RID: 5706 RVA: 0x004D5476 File Offset: 0x004D3676
		// (set) Token: 0x0600164B RID: 5707 RVA: 0x004D5488 File Offset: 0x004D3688
		public bool DpadRadial2
		{
			get
			{
				return this.KeyStatus["DpadRadial2"];
			}
			set
			{
				this.KeyStatus["DpadRadial2"] = value;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600164C RID: 5708 RVA: 0x004D549B File Offset: 0x004D369B
		// (set) Token: 0x0600164D RID: 5709 RVA: 0x004D54AD File Offset: 0x004D36AD
		public bool DpadRadial3
		{
			get
			{
				return this.KeyStatus["DpadRadial3"];
			}
			set
			{
				this.KeyStatus["DpadRadial3"] = value;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600164E RID: 5710 RVA: 0x004D54C0 File Offset: 0x004D36C0
		// (set) Token: 0x0600164F RID: 5711 RVA: 0x004D54D2 File Offset: 0x004D36D2
		public bool DpadRadial4
		{
			get
			{
				return this.KeyStatus["DpadRadial4"];
			}
			set
			{
				this.KeyStatus["DpadRadial4"] = value;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06001650 RID: 5712 RVA: 0x004D54E5 File Offset: 0x004D36E5
		// (set) Token: 0x06001651 RID: 5713 RVA: 0x004D54F7 File Offset: 0x004D36F7
		public bool RadialHotbar
		{
			get
			{
				return this.KeyStatus["RadialHotbar"];
			}
			set
			{
				this.KeyStatus["RadialHotbar"] = value;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x004D550A File Offset: 0x004D370A
		// (set) Token: 0x06001653 RID: 5715 RVA: 0x004D551C File Offset: 0x004D371C
		public bool RadialQuickbar
		{
			get
			{
				return this.KeyStatus["RadialQuickbar"];
			}
			set
			{
				this.KeyStatus["RadialQuickbar"] = value;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06001654 RID: 5716 RVA: 0x004D552F File Offset: 0x004D372F
		// (set) Token: 0x06001655 RID: 5717 RVA: 0x004D5541 File Offset: 0x004D3741
		public bool DpadMouseSnap1
		{
			get
			{
				return this.KeyStatus["DpadSnap1"];
			}
			set
			{
				this.KeyStatus["DpadSnap1"] = value;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06001656 RID: 5718 RVA: 0x004D5554 File Offset: 0x004D3754
		// (set) Token: 0x06001657 RID: 5719 RVA: 0x004D5566 File Offset: 0x004D3766
		public bool DpadMouseSnap2
		{
			get
			{
				return this.KeyStatus["DpadSnap2"];
			}
			set
			{
				this.KeyStatus["DpadSnap2"] = value;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x004D5579 File Offset: 0x004D3779
		// (set) Token: 0x06001659 RID: 5721 RVA: 0x004D558B File Offset: 0x004D378B
		public bool DpadMouseSnap3
		{
			get
			{
				return this.KeyStatus["DpadSnap3"];
			}
			set
			{
				this.KeyStatus["DpadSnap3"] = value;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600165A RID: 5722 RVA: 0x004D559E File Offset: 0x004D379E
		// (set) Token: 0x0600165B RID: 5723 RVA: 0x004D55B0 File Offset: 0x004D37B0
		public bool DpadMouseSnap4
		{
			get
			{
				return this.KeyStatus["DpadSnap4"];
			}
			set
			{
				this.KeyStatus["DpadSnap4"] = value;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600165C RID: 5724 RVA: 0x004D55C3 File Offset: 0x004D37C3
		// (set) Token: 0x0600165D RID: 5725 RVA: 0x004D55D5 File Offset: 0x004D37D5
		public bool MenuUp
		{
			get
			{
				return this.KeyStatus["MenuUp"];
			}
			set
			{
				this.KeyStatus["MenuUp"] = value;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600165E RID: 5726 RVA: 0x004D55E8 File Offset: 0x004D37E8
		// (set) Token: 0x0600165F RID: 5727 RVA: 0x004D55FA File Offset: 0x004D37FA
		public bool MenuDown
		{
			get
			{
				return this.KeyStatus["MenuDown"];
			}
			set
			{
				this.KeyStatus["MenuDown"] = value;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06001660 RID: 5728 RVA: 0x004D560D File Offset: 0x004D380D
		// (set) Token: 0x06001661 RID: 5729 RVA: 0x004D561F File Offset: 0x004D381F
		public bool MenuLeft
		{
			get
			{
				return this.KeyStatus["MenuLeft"];
			}
			set
			{
				this.KeyStatus["MenuLeft"] = value;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x004D5632 File Offset: 0x004D3832
		// (set) Token: 0x06001663 RID: 5731 RVA: 0x004D5644 File Offset: 0x004D3844
		public bool MenuRight
		{
			get
			{
				return this.KeyStatus["MenuRight"];
			}
			set
			{
				this.KeyStatus["MenuRight"] = value;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06001664 RID: 5732 RVA: 0x004D5657 File Offset: 0x004D3857
		// (set) Token: 0x06001665 RID: 5733 RVA: 0x004D5669 File Offset: 0x004D3869
		public bool LockOn
		{
			get
			{
				return this.KeyStatus["LockOn"];
			}
			set
			{
				this.KeyStatus["LockOn"] = value;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06001666 RID: 5734 RVA: 0x004D567C File Offset: 0x004D387C
		// (set) Token: 0x06001667 RID: 5735 RVA: 0x004D568E File Offset: 0x004D388E
		public bool ViewZoomIn
		{
			get
			{
				return this.KeyStatus["ViewZoomIn"];
			}
			set
			{
				this.KeyStatus["ViewZoomIn"] = value;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06001668 RID: 5736 RVA: 0x004D56A1 File Offset: 0x004D38A1
		// (set) Token: 0x06001669 RID: 5737 RVA: 0x004D56B3 File Offset: 0x004D38B3
		public bool ViewZoomOut
		{
			get
			{
				return this.KeyStatus["ViewZoomOut"];
			}
			set
			{
				this.KeyStatus["ViewZoomOut"] = value;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x004D56C6 File Offset: 0x004D38C6
		// (set) Token: 0x0600166B RID: 5739 RVA: 0x004D56D8 File Offset: 0x004D38D8
		public bool OpenCreativePowersMenu
		{
			get
			{
				return this.KeyStatus["ToggleCreativeMenu"];
			}
			set
			{
				this.KeyStatus["ToggleCreativeMenu"] = value;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x004D56EB File Offset: 0x004D38EB
		// (set) Token: 0x0600166D RID: 5741 RVA: 0x004D56FD File Offset: 0x004D38FD
		public bool ToggleCameraMode
		{
			get
			{
				return this.KeyStatus["ToggleCameraMode"];
			}
			set
			{
				this.KeyStatus["ToggleCameraMode"] = value;
			}
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x004D5710 File Offset: 0x004D3910
		public void Reset()
		{
			string[] array = this.KeyStatus.Keys.ToArray<string>();
			for (int i = 0; i < array.Length; i++)
			{
				this.KeyStatus[array[i]] = false;
			}
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x004D574C File Offset: 0x004D394C
		public void CloneFrom(TriggersSet other)
		{
			this.KeyStatus.Clear();
			this.LatestInputMode.Clear();
			foreach (KeyValuePair<string, bool> keyValuePair in other.KeyStatus)
			{
				this.KeyStatus.Add(keyValuePair.Key, keyValuePair.Value);
			}
			this.UsedMovementKey = other.UsedMovementKey;
			this.HotbarScrollCD = other.HotbarScrollCD;
			this.HotbarHoldTime = other.HotbarHoldTime;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x004D57EC File Offset: 0x004D39EC
		public void SetupKeys()
		{
			this.KeyStatus.Clear();
			foreach (string key in PlayerInput.KnownTriggers)
			{
				this.KeyStatus.Add(key, false);
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001671 RID: 5745 RVA: 0x004D5850 File Offset: 0x004D3A50
		public Vector2 DirectionsRaw
		{
			get
			{
				return new Vector2((float)(this.Right.ToInt() - this.Left.ToInt()), (float)(this.Down.ToInt() - this.Up.ToInt()));
			}
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x004D5888 File Offset: 0x004D3A88
		public Vector2 GetNavigatorDirections()
		{
			bool flag = Main.gameMenu || Main.ingameOptionsWindow || Main.editChest || Main.editSign || ((Main.playerInventory || Main.LocalPlayer.talkNPC != -1) && PlayerInput.CurrentProfile.UsingDpadMovekeys());
			bool value = this.Up || (flag && this.MenuUp);
			bool value2 = this.Right || (flag && this.MenuRight);
			bool value3 = this.Down || (flag && this.MenuDown);
			bool value4 = this.Left || (flag && this.MenuLeft);
			return new Vector2((float)(value2.ToInt() - value4.ToInt()), (float)(value3.ToInt() - value.ToInt()));
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x004D5958 File Offset: 0x004D3B58
		public void CopyInto(Player p)
		{
			if (PlayerInput.CurrentInputMode != InputMode.XBoxGamepadUI && !PlayerInput.CursorIsBusy)
			{
				p.controlUp = this.Up;
				p.controlDown = this.Down;
				p.controlLeft = this.Left;
				p.controlRight = this.Right;
				p.controlJump = this.Jump;
				p.controlHook = this.Grapple;
				if (!p.mouseInterface)
				{
					p.controlTorch = this.SmartSelect;
				}
				p.controlSmart = this.SmartCursor;
				p.controlMount = this.QuickMount;
				p.controlQuickHeal = this.QuickHeal;
				p.controlQuickMana = this.QuickMana;
				p.controlCreativeMenu = this.OpenCreativePowersMenu;
				if (this.QuickBuff)
				{
					p.QuickBuff();
				}
				if (Utils.JustBecameTrue(this.Loadout1, ref p.releaseLoadout1))
				{
					p.TrySwitchingLoadout(0);
				}
				if (Utils.JustBecameTrue(this.Loadout2, ref p.releaseLoadout2))
				{
					p.TrySwitchingLoadout(1);
				}
				if (Utils.JustBecameTrue(this.Loadout3, ref p.releaseLoadout3))
				{
					p.TrySwitchingLoadout(2);
				}
				if (Utils.JustBecameTrue(this.NextLoadout, ref p.releaseNextLoadout))
				{
					p.TrySwitchingToNextLoadout();
				}
				if (Utils.JustBecameTrue(this.PreviousLoadout, ref p.releasePreviousLoadout))
				{
					p.TrySwitchingToPreviousLoadout();
				}
				p.controlDash = this.Dash;
				if (!p.controlDash)
				{
					p.releaseDash = true;
				}
				p.controlArmorSetAbility = this.ArmorSetAbility;
			}
			p.controlInv = this.Inventory;
			p.controlThrow = this.Throw;
			p.mapZoomIn = this.MapZoomIn;
			p.mapZoomOut = this.MapZoomOut;
			p.mapAlphaUp = this.MapAlphaUp;
			p.mapAlphaDown = this.MapAlphaDown;
			p.mapFullScreen = this.MapFull;
			p.mapStyle = this.MapStyle;
			if (this.MouseLeft)
			{
				if (!Main.blockMouse && !p.mouseInterface)
				{
					p.controlUseItem = true;
				}
			}
			else
			{
				Main.blockMouse = false;
			}
			if (!Main.playerInventory && Main.player[Main.myPlayer].stressBall && Main.player[Main.myPlayer].CanUseStressBall() && !this.MouseLeft && !Main.blockMouse && !p.mouseInterface)
			{
				p.controlUseItem = true;
			}
			if (!this.MouseRight && !Main.playerInventory)
			{
				PlayerInput.LockGamepadTileUseButton = false;
			}
			if (this.MouseRight && !p.mouseInterface && !Main.blockMouse && !this.ShouldLockTileUsage() && !PlayerInput.InBuildingMode)
			{
				p.controlUseTile = true;
			}
			if (PlayerInput.InBuildingMode && this.MouseRight)
			{
				p.controlInv = true;
			}
			InputMode mode;
			if (this.SmartSelect && this.LatestInputMode.TryGetValue("SmartSelect", out mode) && this.IsInputFromGamepad(mode))
			{
				PlayerInput.SettingsForUI.SetCursorMode(CursorMode.Gamepad);
			}
			bool flag = PlayerInput.Triggers.Current.HotbarPlus || PlayerInput.Triggers.Current.HotbarMinus;
			if (flag)
			{
				this.HotbarHoldTime++;
			}
			else
			{
				this.HotbarHoldTime = 0;
			}
			if (this.HotbarScrollCD > 0 && (this.HotbarScrollCD != 1 || !flag || PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired <= 0))
			{
				this.HotbarScrollCD--;
			}
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x004D5C88 File Offset: 0x004D3E88
		public void CopyIntoDuringChat(Player p)
		{
			if (this.MouseLeft)
			{
				if (!Main.blockMouse && !p.mouseInterface)
				{
					p.controlUseItem = true;
				}
			}
			else
			{
				Main.blockMouse = false;
			}
			if (!this.MouseRight && !Main.playerInventory)
			{
				PlayerInput.LockGamepadTileUseButton = false;
			}
			if (this.MouseRight && !p.mouseInterface && !Main.blockMouse && !this.ShouldLockTileUsage() && !PlayerInput.InBuildingMode)
			{
				p.controlUseTile = true;
			}
			bool flag = PlayerInput.Triggers.Current.HotbarPlus || PlayerInput.Triggers.Current.HotbarMinus;
			if (flag)
			{
				this.HotbarHoldTime++;
			}
			else
			{
				this.HotbarHoldTime = 0;
			}
			if (this.HotbarScrollCD > 0 && (this.HotbarScrollCD != 1 || !flag || PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired <= 0))
			{
				this.HotbarScrollCD--;
			}
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x004D5D6C File Offset: 0x004D3F6C
		private bool ShouldLockTileUsage()
		{
			return PlayerInput.LockGamepadTileUseButton && PlayerInput.UsingGamepad;
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x004D5D7C File Offset: 0x004D3F7C
		private bool IsInputFromGamepad(InputMode mode)
		{
			return mode > InputMode.Mouse;
		}

		// Token: 0x04001159 RID: 4441
		public Dictionary<string, bool> KeyStatus = new Dictionary<string, bool>();

		// Token: 0x0400115A RID: 4442
		public Dictionary<string, InputMode> LatestInputMode = new Dictionary<string, InputMode>();

		// Token: 0x0400115B RID: 4443
		public bool UsedMovementKey = true;

		// Token: 0x0400115C RID: 4444
		public int HotbarScrollCD;

		// Token: 0x0400115D RID: 4445
		public int HotbarHoldTime;
	}
}
