using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Reflection;
using Terraria.DataStructures;
using Terraria.GameContent.Items;

namespace Terraria.ID
{
	// Token: 0x020001BE RID: 446
	public class ItemID
	{
		// Token: 0x06001F3E RID: 7998 RVA: 0x00515FDC File Offset: 0x005141DC
		private static Dictionary<string, short> GenerateLegacyItemDictionary()
		{
			return new Dictionary<string, short>
			{
				{
					"Iron Pickaxe",
					1
				},
				{
					"Dirt Block",
					2
				},
				{
					"Stone Block",
					3
				},
				{
					"Iron Broadsword",
					4
				},
				{
					"Mushroom",
					5
				},
				{
					"Iron Shortsword",
					6
				},
				{
					"Iron Hammer",
					7
				},
				{
					"Torch",
					8
				},
				{
					"Wood",
					9
				},
				{
					"Iron Axe",
					10
				},
				{
					"Iron Ore",
					11
				},
				{
					"Copper Ore",
					12
				},
				{
					"Gold Ore",
					13
				},
				{
					"Silver Ore",
					14
				},
				{
					"Copper Watch",
					15
				},
				{
					"Silver Watch",
					16
				},
				{
					"Gold Watch",
					17
				},
				{
					"Depth Meter",
					18
				},
				{
					"Gold Bar",
					19
				},
				{
					"Copper Bar",
					20
				},
				{
					"Silver Bar",
					21
				},
				{
					"Iron Bar",
					22
				},
				{
					"Gel",
					23
				},
				{
					"Wooden Sword",
					24
				},
				{
					"Wooden Door",
					25
				},
				{
					"Stone Wall",
					26
				},
				{
					"Acorn",
					27
				},
				{
					"Lesser Healing Potion",
					28
				},
				{
					"Life Crystal",
					29
				},
				{
					"Dirt Wall",
					30
				},
				{
					"Bottle",
					31
				},
				{
					"Wooden Table",
					32
				},
				{
					"Furnace",
					33
				},
				{
					"Wooden Chair",
					34
				},
				{
					"Iron Anvil",
					35
				},
				{
					"Work Bench",
					36
				},
				{
					"Goggles",
					37
				},
				{
					"Lens",
					38
				},
				{
					"Wooden Bow",
					39
				},
				{
					"Wooden Arrow",
					40
				},
				{
					"Flaming Arrow",
					41
				},
				{
					"Shuriken",
					42
				},
				{
					"Suspicious Looking Eye",
					43
				},
				{
					"Demon Bow",
					44
				},
				{
					"War Axe of the Night",
					45
				},
				{
					"Light's Bane",
					46
				},
				{
					"Unholy Arrow",
					47
				},
				{
					"Chest",
					48
				},
				{
					"Band of Regeneration",
					49
				},
				{
					"Magic Mirror",
					50
				},
				{
					"Jester's Arrow",
					51
				},
				{
					"Angel Statue",
					52
				},
				{
					"Cloud in a Bottle",
					53
				},
				{
					"Hermes Boots",
					54
				},
				{
					"Enchanted Boomerang",
					55
				},
				{
					"Demonite Ore",
					56
				},
				{
					"Demonite Bar",
					57
				},
				{
					"Heart",
					58
				},
				{
					"Corrupt Seeds",
					59
				},
				{
					"Vile Mushroom",
					60
				},
				{
					"Ebonstone Block",
					61
				},
				{
					"Grass Seeds",
					62
				},
				{
					"Sunflower",
					63
				},
				{
					"Vilethorn",
					64
				},
				{
					"Starfury",
					65
				},
				{
					"Purification Powder",
					66
				},
				{
					"Vile Powder",
					67
				},
				{
					"Rotten Chunk",
					68
				},
				{
					"Worm Tooth",
					69
				},
				{
					"Worm Food",
					70
				},
				{
					"Copper Coin",
					71
				},
				{
					"Silver Coin",
					72
				},
				{
					"Gold Coin",
					73
				},
				{
					"Platinum Coin",
					74
				},
				{
					"Fallen Star",
					75
				},
				{
					"Copper Greaves",
					76
				},
				{
					"Iron Greaves",
					77
				},
				{
					"Silver Greaves",
					78
				},
				{
					"Gold Greaves",
					79
				},
				{
					"Copper Chainmail",
					80
				},
				{
					"Iron Chainmail",
					81
				},
				{
					"Silver Chainmail",
					82
				},
				{
					"Gold Chainmail",
					83
				},
				{
					"Grappling Hook",
					84
				},
				{
					"Chain",
					85
				},
				{
					"Shadow Scale",
					86
				},
				{
					"Piggy Bank",
					87
				},
				{
					"Mining Helmet",
					88
				},
				{
					"Copper Helmet",
					89
				},
				{
					"Iron Helmet",
					90
				},
				{
					"Silver Helmet",
					91
				},
				{
					"Gold Helmet",
					92
				},
				{
					"Wood Wall",
					93
				},
				{
					"Wood Platform",
					94
				},
				{
					"Flintlock Pistol",
					95
				},
				{
					"Musket",
					96
				},
				{
					"Musket Ball",
					97
				},
				{
					"Minishark",
					98
				},
				{
					"Iron Bow",
					99
				},
				{
					"Shadow Greaves",
					100
				},
				{
					"Shadow Scalemail",
					101
				},
				{
					"Shadow Helmet",
					102
				},
				{
					"Nightmare Pickaxe",
					103
				},
				{
					"The Breaker",
					104
				},
				{
					"Candle",
					105
				},
				{
					"Copper Chandelier",
					106
				},
				{
					"Silver Chandelier",
					107
				},
				{
					"Gold Chandelier",
					108
				},
				{
					"Mana Crystal",
					109
				},
				{
					"Lesser Mana Potion",
					110
				},
				{
					"Band of Starpower",
					111
				},
				{
					"Flower of Fire",
					112
				},
				{
					"Magic Missile",
					113
				},
				{
					"Dirt Rod",
					114
				},
				{
					"Shadow Orb",
					115
				},
				{
					"Meteorite",
					116
				},
				{
					"Meteorite Bar",
					117
				},
				{
					"Hook",
					118
				},
				{
					"Flamarang",
					119
				},
				{
					"Molten Fury",
					120
				},
				{
					"Fiery Greatsword",
					121
				},
				{
					"Molten Pickaxe",
					122
				},
				{
					"Meteor Helmet",
					123
				},
				{
					"Meteor Suit",
					124
				},
				{
					"Meteor Leggings",
					125
				},
				{
					"Bottled Water",
					126
				},
				{
					"Space Gun",
					127
				},
				{
					"Rocket Boots",
					128
				},
				{
					"Gray Brick",
					129
				},
				{
					"Gray Brick Wall",
					130
				},
				{
					"Red Brick",
					131
				},
				{
					"Red Brick Wall",
					132
				},
				{
					"Clay Block",
					133
				},
				{
					"Blue Brick",
					134
				},
				{
					"Blue Brick Wall",
					135
				},
				{
					"Chain Lantern",
					136
				},
				{
					"Green Brick",
					137
				},
				{
					"Green Brick Wall",
					138
				},
				{
					"Pink Brick",
					139
				},
				{
					"Pink Brick Wall",
					140
				},
				{
					"Gold Brick",
					141
				},
				{
					"Gold Brick Wall",
					142
				},
				{
					"Silver Brick",
					143
				},
				{
					"Silver Brick Wall",
					144
				},
				{
					"Copper Brick",
					145
				},
				{
					"Copper Brick Wall",
					146
				},
				{
					"Spike",
					147
				},
				{
					"Water Candle",
					148
				},
				{
					"Book",
					149
				},
				{
					"Cobweb",
					150
				},
				{
					"Necro Helmet",
					151
				},
				{
					"Necro Breastplate",
					152
				},
				{
					"Necro Greaves",
					153
				},
				{
					"Bone",
					154
				},
				{
					"Muramasa",
					155
				},
				{
					"Cobalt Shield",
					156
				},
				{
					"Aqua Scepter",
					157
				},
				{
					"Lucky Horseshoe",
					158
				},
				{
					"Shiny Red Balloon",
					159
				},
				{
					"Harpoon",
					160
				},
				{
					"Spiky Ball",
					161
				},
				{
					"Ball O' Hurt",
					162
				},
				{
					"Blue Moon",
					163
				},
				{
					"Handgun",
					164
				},
				{
					"Water Bolt",
					165
				},
				{
					"Bomb",
					166
				},
				{
					"Dynamite",
					167
				},
				{
					"Grenade",
					168
				},
				{
					"Sand Block",
					169
				},
				{
					"Glass",
					170
				},
				{
					"Sign",
					171
				},
				{
					"Ash Block",
					172
				},
				{
					"Obsidian",
					173
				},
				{
					"Hellstone",
					174
				},
				{
					"Hellstone Bar",
					175
				},
				{
					"Mud Block",
					176
				},
				{
					"Sapphire",
					177
				},
				{
					"Ruby",
					178
				},
				{
					"Emerald",
					179
				},
				{
					"Topaz",
					180
				},
				{
					"Amethyst",
					181
				},
				{
					"Diamond",
					182
				},
				{
					"Glowing Mushroom",
					183
				},
				{
					"Star",
					184
				},
				{
					"Ivy Whip",
					185
				},
				{
					"Breathing Reed",
					186
				},
				{
					"Flipper",
					187
				},
				{
					"Healing Potion",
					188
				},
				{
					"Mana Potion",
					189
				},
				{
					"Blade of Grass",
					190
				},
				{
					"Thorn Chakram",
					191
				},
				{
					"Obsidian Brick",
					192
				},
				{
					"Obsidian Skull",
					193
				},
				{
					"Mushroom Grass Seeds",
					194
				},
				{
					"Jungle Grass Seeds",
					195
				},
				{
					"Wooden Hammer",
					196
				},
				{
					"Star Cannon",
					197
				},
				{
					"Blue Phaseblade",
					198
				},
				{
					"Red Phaseblade",
					199
				},
				{
					"Green Phaseblade",
					200
				},
				{
					"Purple Phaseblade",
					201
				},
				{
					"White Phaseblade",
					202
				},
				{
					"Yellow Phaseblade",
					203
				},
				{
					"Meteor Hamaxe",
					204
				},
				{
					"Empty Bucket",
					205
				},
				{
					"Water Bucket",
					206
				},
				{
					"Lava Bucket",
					207
				},
				{
					"Jungle Rose",
					208
				},
				{
					"Stinger",
					209
				},
				{
					"Vine",
					210
				},
				{
					"Feral Claws",
					211
				},
				{
					"Anklet of the Wind",
					212
				},
				{
					"Staff of Regrowth",
					213
				},
				{
					"Hellstone Brick",
					214
				},
				{
					"Whoopie Cushion",
					215
				},
				{
					"Shackle",
					216
				},
				{
					"Molten Hamaxe",
					217
				},
				{
					"Flamelash",
					218
				},
				{
					"Phoenix Blaster",
					219
				},
				{
					"Sunfury",
					220
				},
				{
					"Hellforge",
					221
				},
				{
					"Clay Pot",
					222
				},
				{
					"Nature's Gift",
					223
				},
				{
					"Bed",
					224
				},
				{
					"Silk",
					225
				},
				{
					"Lesser Restoration Potion",
					226
				},
				{
					"Restoration Potion",
					227
				},
				{
					"Jungle Hat",
					228
				},
				{
					"Jungle Shirt",
					229
				},
				{
					"Jungle Pants",
					230
				},
				{
					"Molten Helmet",
					231
				},
				{
					"Molten Breastplate",
					232
				},
				{
					"Molten Greaves",
					233
				},
				{
					"Meteor Shot",
					234
				},
				{
					"Sticky Bomb",
					235
				},
				{
					"Black Lens",
					236
				},
				{
					"Sunglasses",
					237
				},
				{
					"Wizard Hat",
					238
				},
				{
					"Top Hat",
					239
				},
				{
					"Tuxedo Shirt",
					240
				},
				{
					"Tuxedo Pants",
					241
				},
				{
					"Summer Hat",
					242
				},
				{
					"Bunny Hood",
					243
				},
				{
					"Plumber's Hat",
					244
				},
				{
					"Plumber's Shirt",
					245
				},
				{
					"Plumber's Pants",
					246
				},
				{
					"Hero's Hat",
					247
				},
				{
					"Hero's Shirt",
					248
				},
				{
					"Hero's Pants",
					249
				},
				{
					"Fish Bowl",
					250
				},
				{
					"Archaeologist's Hat",
					251
				},
				{
					"Archaeologist's Jacket",
					252
				},
				{
					"Archaeologist's Pants",
					253
				},
				{
					"Black Thread",
					254
				},
				{
					"Green Thread",
					255
				},
				{
					"Ninja Hood",
					256
				},
				{
					"Ninja Shirt",
					257
				},
				{
					"Ninja Pants",
					258
				},
				{
					"Leather",
					259
				},
				{
					"Red Hat",
					260
				},
				{
					"Goldfish",
					261
				},
				{
					"Robe",
					262
				},
				{
					"Robot Hat",
					263
				},
				{
					"Gold Crown",
					264
				},
				{
					"Hellfire Arrow",
					265
				},
				{
					"Sandgun",
					266
				},
				{
					"Guide Voodoo Doll",
					267
				},
				{
					"Diving Helmet",
					268
				},
				{
					"Familiar Shirt",
					269
				},
				{
					"Familiar Pants",
					270
				},
				{
					"Familiar Wig",
					271
				},
				{
					"Demon Scythe",
					272
				},
				{
					"Night's Edge",
					273
				},
				{
					"Dark Lance",
					274
				},
				{
					"Coral",
					275
				},
				{
					"Cactus",
					276
				},
				{
					"Trident",
					277
				},
				{
					"Silver Bullet",
					278
				},
				{
					"Throwing Knife",
					279
				},
				{
					"Spear",
					280
				},
				{
					"Blowpipe",
					281
				},
				{
					"Glowstick",
					282
				},
				{
					"Seed",
					283
				},
				{
					"Wooden Boomerang",
					284
				},
				{
					"Aglet",
					285
				},
				{
					"Sticky Glowstick",
					286
				},
				{
					"Poisoned Knife",
					287
				},
				{
					"Obsidian Skin Potion",
					288
				},
				{
					"Regeneration Potion",
					289
				},
				{
					"Swiftness Potion",
					290
				},
				{
					"Gills Potion",
					291
				},
				{
					"Ironskin Potion",
					292
				},
				{
					"Mana Regeneration Potion",
					293
				},
				{
					"Magic Power Potion",
					294
				},
				{
					"Featherfall Potion",
					295
				},
				{
					"Spelunker Potion",
					296
				},
				{
					"Invisibility Potion",
					297
				},
				{
					"Shine Potion",
					298
				},
				{
					"Night Owl Potion",
					299
				},
				{
					"Battle Potion",
					300
				},
				{
					"Thorns Potion",
					301
				},
				{
					"Water Walking Potion",
					302
				},
				{
					"Archery Potion",
					303
				},
				{
					"Hunter Potion",
					304
				},
				{
					"Gravitation Potion",
					305
				},
				{
					"Gold Chest",
					306
				},
				{
					"Daybloom Seeds",
					307
				},
				{
					"Moonglow Seeds",
					308
				},
				{
					"Blinkroot Seeds",
					309
				},
				{
					"Deathweed Seeds",
					310
				},
				{
					"Waterleaf Seeds",
					311
				},
				{
					"Fireblossom Seeds",
					312
				},
				{
					"Daybloom",
					313
				},
				{
					"Moonglow",
					314
				},
				{
					"Blinkroot",
					315
				},
				{
					"Deathweed",
					316
				},
				{
					"Waterleaf",
					317
				},
				{
					"Fireblossom",
					318
				},
				{
					"Shark Fin",
					319
				},
				{
					"Feather",
					320
				},
				{
					"Tombstone",
					321
				},
				{
					"Mime Mask",
					322
				},
				{
					"Antlion Mandible",
					323
				},
				{
					"Illegal Gun Parts",
					324
				},
				{
					"The Doctor's Shirt",
					325
				},
				{
					"The Doctor's Pants",
					326
				},
				{
					"Golden Key",
					327
				},
				{
					"Shadow Chest",
					328
				},
				{
					"Shadow Key",
					329
				},
				{
					"Obsidian Brick Wall",
					330
				},
				{
					"Jungle Spores",
					331
				},
				{
					"Loom",
					332
				},
				{
					"Piano",
					333
				},
				{
					"Dresser",
					334
				},
				{
					"Bench",
					335
				},
				{
					"Bathtub",
					336
				},
				{
					"Red Banner",
					337
				},
				{
					"Green Banner",
					338
				},
				{
					"Blue Banner",
					339
				},
				{
					"Yellow Banner",
					340
				},
				{
					"Lamp Post",
					341
				},
				{
					"Tiki Torch",
					342
				},
				{
					"Barrel",
					343
				},
				{
					"Chinese Lantern",
					344
				},
				{
					"Cooking Pot",
					345
				},
				{
					"Safe",
					346
				},
				{
					"Skull Lantern",
					347
				},
				{
					"Trash Can",
					348
				},
				{
					"Candelabra",
					349
				},
				{
					"Pink Vase",
					350
				},
				{
					"Mug",
					351
				},
				{
					"Keg",
					352
				},
				{
					"Ale",
					353
				},
				{
					"Bookcase",
					354
				},
				{
					"Throne",
					355
				},
				{
					"Bowl",
					356
				},
				{
					"Bowl of Soup",
					357
				},
				{
					"Toilet",
					358
				},
				{
					"Grandfather Clock",
					359
				},
				{
					"Armor Statue",
					360
				},
				{
					"Goblin Battle Standard",
					361
				},
				{
					"Tattered Cloth",
					362
				},
				{
					"Sawmill",
					363
				},
				{
					"Cobalt Ore",
					364
				},
				{
					"Mythril Ore",
					365
				},
				{
					"Adamantite Ore",
					366
				},
				{
					"Pwnhammer",
					367
				},
				{
					"Excalibur",
					368
				},
				{
					"Hallowed Seeds",
					369
				},
				{
					"Ebonsand Block",
					370
				},
				{
					"Cobalt Hat",
					371
				},
				{
					"Cobalt Helmet",
					372
				},
				{
					"Cobalt Mask",
					373
				},
				{
					"Cobalt Breastplate",
					374
				},
				{
					"Cobalt Leggings",
					375
				},
				{
					"Mythril Hood",
					376
				},
				{
					"Mythril Helmet",
					377
				},
				{
					"Mythril Hat",
					378
				},
				{
					"Mythril Chainmail",
					379
				},
				{
					"Mythril Greaves",
					380
				},
				{
					"Cobalt Bar",
					381
				},
				{
					"Mythril Bar",
					382
				},
				{
					"Cobalt Chainsaw",
					383
				},
				{
					"Mythril Chainsaw",
					384
				},
				{
					"Cobalt Drill",
					385
				},
				{
					"Mythril Drill",
					386
				},
				{
					"Adamantite Chainsaw",
					387
				},
				{
					"Adamantite Drill",
					388
				},
				{
					"Dao of Pow",
					389
				},
				{
					"Mythril Halberd",
					390
				},
				{
					"Adamantite Bar",
					391
				},
				{
					"Glass Wall",
					392
				},
				{
					"Compass",
					393
				},
				{
					"Diving Gear",
					394
				},
				{
					"GPS",
					395
				},
				{
					"Obsidian Horseshoe",
					396
				},
				{
					"Obsidian Shield",
					397
				},
				{
					"Tinkerer's Workshop",
					398
				},
				{
					"Cloud in a Balloon",
					399
				},
				{
					"Adamantite Headgear",
					400
				},
				{
					"Adamantite Helmet",
					401
				},
				{
					"Adamantite Mask",
					402
				},
				{
					"Adamantite Breastplate",
					403
				},
				{
					"Adamantite Leggings",
					404
				},
				{
					"Spectre Boots",
					405
				},
				{
					"Adamantite Glaive",
					406
				},
				{
					"Toolbelt",
					407
				},
				{
					"Pearlsand Block",
					408
				},
				{
					"Pearlstone Block",
					409
				},
				{
					"Mining Shirt",
					410
				},
				{
					"Mining Pants",
					411
				},
				{
					"Pearlstone Brick",
					412
				},
				{
					"Iridescent Brick",
					413
				},
				{
					"Mudstone Brick",
					414
				},
				{
					"Cobalt Brick",
					415
				},
				{
					"Mythril Brick",
					416
				},
				{
					"Pearlstone Brick Wall",
					417
				},
				{
					"Iridescent Brick Wall",
					418
				},
				{
					"Mudstone Brick Wall",
					419
				},
				{
					"Cobalt Brick Wall",
					420
				},
				{
					"Mythril Brick Wall",
					421
				},
				{
					"Holy Water",
					422
				},
				{
					"Unholy Water",
					423
				},
				{
					"Silt Block",
					424
				},
				{
					"Fairy Bell",
					425
				},
				{
					"Breaker Blade",
					426
				},
				{
					"Blue Torch",
					427
				},
				{
					"Red Torch",
					428
				},
				{
					"Green Torch",
					429
				},
				{
					"Purple Torch",
					430
				},
				{
					"White Torch",
					431
				},
				{
					"Yellow Torch",
					432
				},
				{
					"Demon Torch",
					433
				},
				{
					"Clockwork Assault Rifle",
					434
				},
				{
					"Cobalt Repeater",
					435
				},
				{
					"Mythril Repeater",
					436
				},
				{
					"Dual Hook",
					437
				},
				{
					"Star Statue",
					438
				},
				{
					"Sword Statue",
					439
				},
				{
					"Slime Statue",
					440
				},
				{
					"Goblin Statue",
					441
				},
				{
					"Shield Statue",
					442
				},
				{
					"Bat Statue",
					443
				},
				{
					"Fish Statue",
					444
				},
				{
					"Bunny Statue",
					445
				},
				{
					"Skeleton Statue",
					446
				},
				{
					"Reaper Statue",
					447
				},
				{
					"Woman Statue",
					448
				},
				{
					"Imp Statue",
					449
				},
				{
					"Gargoyle Statue",
					450
				},
				{
					"Gloom Statue",
					451
				},
				{
					"Hornet Statue",
					452
				},
				{
					"Bomb Statue",
					453
				},
				{
					"Crab Statue",
					454
				},
				{
					"Hammer Statue",
					455
				},
				{
					"Potion Statue",
					456
				},
				{
					"Spear Statue",
					457
				},
				{
					"Cross Statue",
					458
				},
				{
					"Jellyfish Statue",
					459
				},
				{
					"Bow Statue",
					460
				},
				{
					"Boomerang Statue",
					461
				},
				{
					"Boot Statue",
					462
				},
				{
					"Chest Statue",
					463
				},
				{
					"Bird Statue",
					464
				},
				{
					"Axe Statue",
					465
				},
				{
					"Corrupt Statue",
					466
				},
				{
					"Tree Statue",
					467
				},
				{
					"Anvil Statue",
					468
				},
				{
					"Pickaxe Statue",
					469
				},
				{
					"Mushroom Statue",
					470
				},
				{
					"Eyeball Statue",
					471
				},
				{
					"Pillar Statue",
					472
				},
				{
					"Heart Statue",
					473
				},
				{
					"Pot Statue",
					474
				},
				{
					"Sunflower Statue",
					475
				},
				{
					"King Statue",
					476
				},
				{
					"Queen Statue",
					477
				},
				{
					"Piranha Statue",
					478
				},
				{
					"Planked Wall",
					479
				},
				{
					"Wooden Beam",
					480
				},
				{
					"Adamantite Repeater",
					481
				},
				{
					"Adamantite Sword",
					482
				},
				{
					"Cobalt Sword",
					483
				},
				{
					"Mythril Sword",
					484
				},
				{
					"Moon Charm",
					485
				},
				{
					"Ruler",
					486
				},
				{
					"Crystal Ball",
					487
				},
				{
					"Disco Ball",
					488
				},
				{
					"Sorcerer Emblem",
					489
				},
				{
					"Warrior Emblem",
					490
				},
				{
					"Ranger Emblem",
					491
				},
				{
					"Demon Wings",
					492
				},
				{
					"Angel Wings",
					493
				},
				{
					"Magical Harp",
					494
				},
				{
					"Rainbow Rod",
					495
				},
				{
					"Ice Rod",
					496
				},
				{
					"Neptune's Shell",
					497
				},
				{
					"Mannequin",
					498
				},
				{
					"Greater Healing Potion",
					499
				},
				{
					"Greater Mana Potion",
					500
				},
				{
					"Pixie Dust",
					501
				},
				{
					"Crystal Shard",
					502
				},
				{
					"Clown Hat",
					503
				},
				{
					"Clown Shirt",
					504
				},
				{
					"Clown Pants",
					505
				},
				{
					"Flamethrower",
					506
				},
				{
					"Bell",
					507
				},
				{
					"Harp",
					508
				},
				{
					"Red Wrench",
					509
				},
				{
					"Wire Cutter",
					510
				},
				{
					"Active Stone Block",
					511
				},
				{
					"Inactive Stone Block",
					512
				},
				{
					"Lever",
					513
				},
				{
					"Laser Rifle",
					514
				},
				{
					"Crystal Bullet",
					515
				},
				{
					"Holy Arrow",
					516
				},
				{
					"Magic Dagger",
					517
				},
				{
					"Crystal Storm",
					518
				},
				{
					"Cursed Flames",
					519
				},
				{
					"Soul of Light",
					520
				},
				{
					"Soul of Night",
					521
				},
				{
					"Cursed Flame",
					522
				},
				{
					"Cursed Torch",
					523
				},
				{
					"Adamantite Forge",
					524
				},
				{
					"Mythril Anvil",
					525
				},
				{
					"Unicorn Horn",
					526
				},
				{
					"Dark Shard",
					527
				},
				{
					"Light Shard",
					528
				},
				{
					"Red Pressure Plate",
					529
				},
				{
					"Wire",
					530
				},
				{
					"Spell Tome",
					531
				},
				{
					"Star Cloak",
					532
				},
				{
					"Megashark",
					533
				},
				{
					"Shotgun",
					534
				},
				{
					"Philosopher's Stone",
					535
				},
				{
					"Titan Glove",
					536
				},
				{
					"Cobalt Naginata",
					537
				},
				{
					"Switch",
					538
				},
				{
					"Dart Trap",
					539
				},
				{
					"Boulder",
					540
				},
				{
					"Green Pressure Plate",
					541
				},
				{
					"Gray Pressure Plate",
					542
				},
				{
					"Brown Pressure Plate",
					543
				},
				{
					"Mechanical Eye",
					544
				},
				{
					"Cursed Arrow",
					545
				},
				{
					"Cursed Bullet",
					546
				},
				{
					"Soul of Fright",
					547
				},
				{
					"Soul of Might",
					548
				},
				{
					"Soul of Sight",
					549
				},
				{
					"Gungnir",
					550
				},
				{
					"Hallowed Plate Mail",
					551
				},
				{
					"Hallowed Greaves",
					552
				},
				{
					"Hallowed Helmet",
					553
				},
				{
					"Cross Necklace",
					554
				},
				{
					"Mana Flower",
					555
				},
				{
					"Mechanical Worm",
					556
				},
				{
					"Mechanical Skull",
					557
				},
				{
					"Hallowed Headgear",
					558
				},
				{
					"Hallowed Mask",
					559
				},
				{
					"Slime Crown",
					560
				},
				{
					"Light Disc",
					561
				},
				{
					"Music Box (Overworld Day)",
					562
				},
				{
					"Music Box (Eerie)",
					563
				},
				{
					"Music Box (Night)",
					564
				},
				{
					"Music Box (Title)",
					565
				},
				{
					"Music Box (Underground)",
					566
				},
				{
					"Music Box (Boss 1)",
					567
				},
				{
					"Music Box (Jungle)",
					568
				},
				{
					"Music Box (Corruption)",
					569
				},
				{
					"Music Box (Underground Corruption)",
					570
				},
				{
					"Music Box (The Hallow)",
					571
				},
				{
					"Music Box (Boss 2)",
					572
				},
				{
					"Music Box (Underground Hallow)",
					573
				},
				{
					"Music Box (Boss 3)",
					574
				},
				{
					"Soul of Flight",
					575
				},
				{
					"Music Box",
					576
				},
				{
					"Demonite Brick",
					577
				},
				{
					"Hallowed Repeater",
					578
				},
				{
					"Drax",
					579
				},
				{
					"Explosives",
					580
				},
				{
					"Inlet Pump",
					581
				},
				{
					"Outlet Pump",
					582
				},
				{
					"1 Second Timer",
					583
				},
				{
					"3 Second Timer",
					584
				},
				{
					"5 Second Timer",
					585
				},
				{
					"Candy Cane Block",
					586
				},
				{
					"Candy Cane Wall",
					587
				},
				{
					"Santa Hat",
					588
				},
				{
					"Santa Shirt",
					589
				},
				{
					"Santa Pants",
					590
				},
				{
					"Green Candy Cane Block",
					591
				},
				{
					"Green Candy Cane Wall",
					592
				},
				{
					"Snow Block",
					593
				},
				{
					"Snow Brick",
					594
				},
				{
					"Snow Brick Wall",
					595
				},
				{
					"Blue Light",
					596
				},
				{
					"Red Light",
					597
				},
				{
					"Green Light",
					598
				},
				{
					"Blue Present",
					599
				},
				{
					"Green Present",
					600
				},
				{
					"Yellow Present",
					601
				},
				{
					"Snow Globe",
					602
				},
				{
					"Carrot",
					603
				},
				{
					"Yellow Phasesaber",
					3769
				},
				{
					"White Phasesaber",
					3768
				},
				{
					"Purple Phasesaber",
					3767
				},
				{
					"Green Phasesaber",
					3766
				},
				{
					"Red Phasesaber",
					3765
				},
				{
					"Blue Phasesaber",
					3764
				},
				{
					"Platinum Bow",
					3480
				},
				{
					"Platinum Hammer",
					3481
				},
				{
					"Platinum Axe",
					3482
				},
				{
					"Platinum Shortsword",
					3483
				},
				{
					"Platinum Broadsword",
					3484
				},
				{
					"Platinum Pickaxe",
					3485
				},
				{
					"Tungsten Bow",
					3486
				},
				{
					"Tungsten Hammer",
					3487
				},
				{
					"Tungsten Axe",
					3488
				},
				{
					"Tungsten Shortsword",
					3489
				},
				{
					"Tungsten Broadsword",
					3490
				},
				{
					"Tungsten Pickaxe",
					3491
				},
				{
					"Lead Bow",
					3492
				},
				{
					"Lead Hammer",
					3493
				},
				{
					"Lead Axe",
					3494
				},
				{
					"Lead Shortsword",
					3495
				},
				{
					"Lead Broadsword",
					3496
				},
				{
					"Lead Pickaxe",
					3497
				},
				{
					"Tin Bow",
					3498
				},
				{
					"Tin Hammer",
					3499
				},
				{
					"Tin Axe",
					3500
				},
				{
					"Tin Shortsword",
					3501
				},
				{
					"Tin Broadsword",
					3502
				},
				{
					"Tin Pickaxe",
					3503
				},
				{
					"Copper Bow",
					3504
				},
				{
					"Copper Hammer",
					3505
				},
				{
					"Copper Axe",
					3506
				},
				{
					"Copper Shortsword",
					3507
				},
				{
					"Copper Broadsword",
					3508
				},
				{
					"Copper Pickaxe",
					3509
				},
				{
					"Silver Bow",
					3510
				},
				{
					"Silver Hammer",
					3511
				},
				{
					"Silver Axe",
					3512
				},
				{
					"Silver Shortsword",
					3513
				},
				{
					"Silver Broadsword",
					3514
				},
				{
					"Silver Pickaxe",
					3515
				},
				{
					"Gold Bow",
					3516
				},
				{
					"Gold Hammer",
					3517
				},
				{
					"Gold Axe",
					3518
				},
				{
					"Gold Shortsword",
					3519
				},
				{
					"Gold Broadsword",
					3520
				},
				{
					"Gold Pickaxe",
					3521
				}
			};
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x0051871C File Offset: 0x0051691C
		public static short FromNetId(short id)
		{
			switch (id)
			{
			case -48:
				return 3480;
			case -47:
				return 3481;
			case -46:
				return 3482;
			case -45:
				return 3483;
			case -44:
				return 3484;
			case -43:
				return 3485;
			case -42:
				return 3486;
			case -41:
				return 3487;
			case -40:
				return 3488;
			case -39:
				return 3489;
			case -38:
				return 3490;
			case -37:
				return 3491;
			case -36:
				return 3492;
			case -35:
				return 3493;
			case -34:
				return 3494;
			case -33:
				return 3495;
			case -32:
				return 3496;
			case -31:
				return 3497;
			case -30:
				return 3498;
			case -29:
				return 3499;
			case -28:
				return 3500;
			case -27:
				return 3501;
			case -26:
				return 3502;
			case -25:
				return 3503;
			case -24:
				return 3769;
			case -23:
				return 3768;
			case -22:
				return 3767;
			case -21:
				return 3766;
			case -20:
				return 3765;
			case -19:
				return 3764;
			case -18:
				return 3504;
			case -17:
				return 3505;
			case -16:
				return 3506;
			case -15:
				return 3507;
			case -14:
				return 3508;
			case -13:
				return 3509;
			case -12:
				return 3510;
			case -11:
				return 3511;
			case -10:
				return 3512;
			case -9:
				return 3513;
			case -8:
				return 3514;
			case -7:
				return 3515;
			case -6:
				return 3516;
			case -5:
				return 3517;
			case -4:
				return 3518;
			case -3:
				return 3519;
			case -2:
				return 3520;
			case -1:
				return 3521;
			default:
				return id;
			}
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x00518918 File Offset: 0x00516B18
		public static short FromLegacyName(string name, int release)
		{
			if (ItemID._legacyItemLookup == null)
			{
				ItemID._legacyItemLookup = ItemID.GenerateLegacyItemDictionary();
			}
			if (release <= 4)
			{
				if (name == "Cobalt Helmet")
				{
					name = "Jungle Hat";
				}
				else if (name == "Cobalt Breastplate")
				{
					name = "Jungle Shirt";
				}
				else if (name == "Cobalt Greaves")
				{
					name = "Jungle Pants";
				}
			}
			if (release <= 13 && name == "Jungle Rose")
			{
				name = "Jungle Spores";
			}
			if (release <= 20)
			{
				if (name == "Gills potion")
				{
					name = "Gills Potion";
				}
				else if (name == "Thorn Chakrum")
				{
					name = "Thorn Chakram";
				}
				else if (name == "Ball 'O Hurt")
				{
					name = "Ball O' Hurt";
				}
			}
			if (release <= 41 && name == "Iron Chain")
			{
				name = "Chain";
			}
			if (release <= 44 && name == "Orb of Light")
			{
				name = "Shadow Orb";
			}
			if (release <= 46)
			{
				if (name == "Black Dye")
				{
					name = "Black Thread";
				}
				if (name == "Green Dye")
				{
					name = "Green Thread";
				}
			}
			short result;
			if (ItemID._legacyItemLookup.TryGetValue(name, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x0400251C RID: 9500
		private static Dictionary<string, short> _legacyItemLookup;

		// Token: 0x0400251D RID: 9501
		public const short YellowPhasesaberOld = -24;

		// Token: 0x0400251E RID: 9502
		public const short WhitePhasesaberOld = -23;

		// Token: 0x0400251F RID: 9503
		public const short PurplePhasesaberOld = -22;

		// Token: 0x04002520 RID: 9504
		public const short GreenPhasesaberOld = -21;

		// Token: 0x04002521 RID: 9505
		public const short RedPhasesaberOld = -20;

		// Token: 0x04002522 RID: 9506
		public const short BluePhasesaberOld = -19;

		// Token: 0x04002523 RID: 9507
		public const short PlatinumBowOld = -48;

		// Token: 0x04002524 RID: 9508
		public const short PlatinumHammerOld = -47;

		// Token: 0x04002525 RID: 9509
		public const short PlatinumAxeOld = -46;

		// Token: 0x04002526 RID: 9510
		public const short PlatinumShortswordOld = -45;

		// Token: 0x04002527 RID: 9511
		public const short PlatinumBroadswordOld = -44;

		// Token: 0x04002528 RID: 9512
		public const short PlatinumPickaxeOld = -43;

		// Token: 0x04002529 RID: 9513
		public const short TungstenBowOld = -42;

		// Token: 0x0400252A RID: 9514
		public const short TungstenHammerOld = -41;

		// Token: 0x0400252B RID: 9515
		public const short TungstenAxeOld = -40;

		// Token: 0x0400252C RID: 9516
		public const short TungstenShortswordOld = -39;

		// Token: 0x0400252D RID: 9517
		public const short TungstenBroadswordOld = -38;

		// Token: 0x0400252E RID: 9518
		public const short TungstenPickaxeOld = -37;

		// Token: 0x0400252F RID: 9519
		public const short LeadBowOld = -36;

		// Token: 0x04002530 RID: 9520
		public const short LeadHammerOld = -35;

		// Token: 0x04002531 RID: 9521
		public const short LeadAxeOld = -34;

		// Token: 0x04002532 RID: 9522
		public const short LeadShortswordOld = -33;

		// Token: 0x04002533 RID: 9523
		public const short LeadBroadswordOld = -32;

		// Token: 0x04002534 RID: 9524
		public const short LeadPickaxeOld = -31;

		// Token: 0x04002535 RID: 9525
		public const short TinBowOld = -30;

		// Token: 0x04002536 RID: 9526
		public const short TinHammerOld = -29;

		// Token: 0x04002537 RID: 9527
		public const short TinAxeOld = -28;

		// Token: 0x04002538 RID: 9528
		public const short TinShortswordOld = -27;

		// Token: 0x04002539 RID: 9529
		public const short TinBroadswordOld = -26;

		// Token: 0x0400253A RID: 9530
		public const short TinPickaxeOld = -25;

		// Token: 0x0400253B RID: 9531
		public const short CopperBowOld = -18;

		// Token: 0x0400253C RID: 9532
		public const short CopperHammerOld = -17;

		// Token: 0x0400253D RID: 9533
		public const short CopperAxeOld = -16;

		// Token: 0x0400253E RID: 9534
		public const short CopperShortswordOld = -15;

		// Token: 0x0400253F RID: 9535
		public const short CopperBroadswordOld = -14;

		// Token: 0x04002540 RID: 9536
		public const short CopperPickaxeOld = -13;

		// Token: 0x04002541 RID: 9537
		public const short SilverBowOld = -12;

		// Token: 0x04002542 RID: 9538
		public const short SilverHammerOld = -11;

		// Token: 0x04002543 RID: 9539
		public const short SilverAxeOld = -10;

		// Token: 0x04002544 RID: 9540
		public const short SilverShortswordOld = -9;

		// Token: 0x04002545 RID: 9541
		public const short SilverBroadswordOld = -8;

		// Token: 0x04002546 RID: 9542
		public const short SilverPickaxeOld = -7;

		// Token: 0x04002547 RID: 9543
		public const short GoldBowOld = -6;

		// Token: 0x04002548 RID: 9544
		public const short GoldHammerOld = -5;

		// Token: 0x04002549 RID: 9545
		public const short GoldAxeOld = -4;

		// Token: 0x0400254A RID: 9546
		public const short GoldShortswordOld = -3;

		// Token: 0x0400254B RID: 9547
		public const short GoldBroadswordOld = -2;

		// Token: 0x0400254C RID: 9548
		public const short GoldPickaxeOld = -1;

		// Token: 0x0400254D RID: 9549
		public const short None = 0;

		// Token: 0x0400254E RID: 9550
		public const short IronPickaxe = 1;

		// Token: 0x0400254F RID: 9551
		public const short DirtBlock = 2;

		// Token: 0x04002550 RID: 9552
		public const short StoneBlock = 3;

		// Token: 0x04002551 RID: 9553
		public const short IronBroadsword = 4;

		// Token: 0x04002552 RID: 9554
		public const short Mushroom = 5;

		// Token: 0x04002553 RID: 9555
		public const short IronShortsword = 6;

		// Token: 0x04002554 RID: 9556
		public const short IronHammer = 7;

		// Token: 0x04002555 RID: 9557
		public const short Torch = 8;

		// Token: 0x04002556 RID: 9558
		public const short Wood = 9;

		// Token: 0x04002557 RID: 9559
		public const short IronAxe = 10;

		// Token: 0x04002558 RID: 9560
		public const short IronOre = 11;

		// Token: 0x04002559 RID: 9561
		public const short CopperOre = 12;

		// Token: 0x0400255A RID: 9562
		public const short GoldOre = 13;

		// Token: 0x0400255B RID: 9563
		public const short SilverOre = 14;

		// Token: 0x0400255C RID: 9564
		public const short CopperWatch = 15;

		// Token: 0x0400255D RID: 9565
		public const short SilverWatch = 16;

		// Token: 0x0400255E RID: 9566
		public const short GoldWatch = 17;

		// Token: 0x0400255F RID: 9567
		public const short DepthMeter = 18;

		// Token: 0x04002560 RID: 9568
		public const short GoldBar = 19;

		// Token: 0x04002561 RID: 9569
		public const short CopperBar = 20;

		// Token: 0x04002562 RID: 9570
		public const short SilverBar = 21;

		// Token: 0x04002563 RID: 9571
		public const short IronBar = 22;

		// Token: 0x04002564 RID: 9572
		public const short Gel = 23;

		// Token: 0x04002565 RID: 9573
		public const short WoodenSword = 24;

		// Token: 0x04002566 RID: 9574
		public const short WoodenDoor = 25;

		// Token: 0x04002567 RID: 9575
		public const short StoneWall = 26;

		// Token: 0x04002568 RID: 9576
		public const short Acorn = 27;

		// Token: 0x04002569 RID: 9577
		public const short LesserHealingPotion = 28;

		// Token: 0x0400256A RID: 9578
		public const short LifeCrystal = 29;

		// Token: 0x0400256B RID: 9579
		public const short DirtWall = 30;

		// Token: 0x0400256C RID: 9580
		public const short Bottle = 31;

		// Token: 0x0400256D RID: 9581
		public const short WoodenTable = 32;

		// Token: 0x0400256E RID: 9582
		public const short Furnace = 33;

		// Token: 0x0400256F RID: 9583
		public const short WoodenChair = 34;

		// Token: 0x04002570 RID: 9584
		public const short IronAnvil = 35;

		// Token: 0x04002571 RID: 9585
		public const short WorkBench = 36;

		// Token: 0x04002572 RID: 9586
		public const short Goggles = 37;

		// Token: 0x04002573 RID: 9587
		public const short Lens = 38;

		// Token: 0x04002574 RID: 9588
		public const short WoodenBow = 39;

		// Token: 0x04002575 RID: 9589
		public const short WoodenArrow = 40;

		// Token: 0x04002576 RID: 9590
		public const short FlamingArrow = 41;

		// Token: 0x04002577 RID: 9591
		public const short Shuriken = 42;

		// Token: 0x04002578 RID: 9592
		public const short SuspiciousLookingEye = 43;

		// Token: 0x04002579 RID: 9593
		public const short DemonBow = 44;

		// Token: 0x0400257A RID: 9594
		public const short WarAxeoftheNight = 45;

		// Token: 0x0400257B RID: 9595
		public const short LightsBane = 46;

		// Token: 0x0400257C RID: 9596
		public const short UnholyArrow = 47;

		// Token: 0x0400257D RID: 9597
		public const short Chest = 48;

		// Token: 0x0400257E RID: 9598
		public const short BandofRegeneration = 49;

		// Token: 0x0400257F RID: 9599
		public const short MagicMirror = 50;

		// Token: 0x04002580 RID: 9600
		public const short JestersArrow = 51;

		// Token: 0x04002581 RID: 9601
		public const short AngelStatue = 52;

		// Token: 0x04002582 RID: 9602
		public const short CloudinaBottle = 53;

		// Token: 0x04002583 RID: 9603
		public const short HermesBoots = 54;

		// Token: 0x04002584 RID: 9604
		public const short EnchantedBoomerang = 55;

		// Token: 0x04002585 RID: 9605
		public const short DemoniteOre = 56;

		// Token: 0x04002586 RID: 9606
		public const short DemoniteBar = 57;

		// Token: 0x04002587 RID: 9607
		public const short Heart = 58;

		// Token: 0x04002588 RID: 9608
		public const short CorruptSeeds = 59;

		// Token: 0x04002589 RID: 9609
		public const short VileMushroom = 60;

		// Token: 0x0400258A RID: 9610
		public const short EbonstoneBlock = 61;

		// Token: 0x0400258B RID: 9611
		public const short GrassSeeds = 62;

		// Token: 0x0400258C RID: 9612
		public const short Sunflower = 63;

		// Token: 0x0400258D RID: 9613
		public const short Vilethorn = 64;

		// Token: 0x0400258E RID: 9614
		public const short Starfury = 65;

		// Token: 0x0400258F RID: 9615
		public const short PurificationPowder = 66;

		// Token: 0x04002590 RID: 9616
		public const short VilePowder = 67;

		// Token: 0x04002591 RID: 9617
		public const short RottenChunk = 68;

		// Token: 0x04002592 RID: 9618
		public const short WormTooth = 69;

		// Token: 0x04002593 RID: 9619
		public const short WormFood = 70;

		// Token: 0x04002594 RID: 9620
		public const short CopperCoin = 71;

		// Token: 0x04002595 RID: 9621
		public const short SilverCoin = 72;

		// Token: 0x04002596 RID: 9622
		public const short GoldCoin = 73;

		// Token: 0x04002597 RID: 9623
		public const short PlatinumCoin = 74;

		// Token: 0x04002598 RID: 9624
		public const short FallenStar = 75;

		// Token: 0x04002599 RID: 9625
		public const short CopperGreaves = 76;

		// Token: 0x0400259A RID: 9626
		public const short IronGreaves = 77;

		// Token: 0x0400259B RID: 9627
		public const short SilverGreaves = 78;

		// Token: 0x0400259C RID: 9628
		public const short GoldGreaves = 79;

		// Token: 0x0400259D RID: 9629
		public const short CopperChainmail = 80;

		// Token: 0x0400259E RID: 9630
		public const short IronChainmail = 81;

		// Token: 0x0400259F RID: 9631
		public const short SilverChainmail = 82;

		// Token: 0x040025A0 RID: 9632
		public const short GoldChainmail = 83;

		// Token: 0x040025A1 RID: 9633
		public const short GrapplingHook = 84;

		// Token: 0x040025A2 RID: 9634
		public const short Chain = 85;

		// Token: 0x040025A3 RID: 9635
		public const short ShadowScale = 86;

		// Token: 0x040025A4 RID: 9636
		public const short PiggyBank = 87;

		// Token: 0x040025A5 RID: 9637
		public const short MiningHelmet = 88;

		// Token: 0x040025A6 RID: 9638
		public const short CopperHelmet = 89;

		// Token: 0x040025A7 RID: 9639
		public const short IronHelmet = 90;

		// Token: 0x040025A8 RID: 9640
		public const short SilverHelmet = 91;

		// Token: 0x040025A9 RID: 9641
		public const short GoldHelmet = 92;

		// Token: 0x040025AA RID: 9642
		public const short WoodWall = 93;

		// Token: 0x040025AB RID: 9643
		public const short WoodPlatform = 94;

		// Token: 0x040025AC RID: 9644
		public const short FlintlockPistol = 95;

		// Token: 0x040025AD RID: 9645
		public const short Musket = 96;

		// Token: 0x040025AE RID: 9646
		public const short MusketBall = 97;

		// Token: 0x040025AF RID: 9647
		public const short Minishark = 98;

		// Token: 0x040025B0 RID: 9648
		public const short IronBow = 99;

		// Token: 0x040025B1 RID: 9649
		public const short ShadowGreaves = 100;

		// Token: 0x040025B2 RID: 9650
		public const short ShadowScalemail = 101;

		// Token: 0x040025B3 RID: 9651
		public const short ShadowHelmet = 102;

		// Token: 0x040025B4 RID: 9652
		public const short NightmarePickaxe = 103;

		// Token: 0x040025B5 RID: 9653
		public const short TheBreaker = 104;

		// Token: 0x040025B6 RID: 9654
		public const short Candle = 105;

		// Token: 0x040025B7 RID: 9655
		public const short CopperChandelier = 106;

		// Token: 0x040025B8 RID: 9656
		public const short SilverChandelier = 107;

		// Token: 0x040025B9 RID: 9657
		public const short GoldChandelier = 108;

		// Token: 0x040025BA RID: 9658
		public const short ManaCrystal = 109;

		// Token: 0x040025BB RID: 9659
		public const short LesserManaPotion = 110;

		// Token: 0x040025BC RID: 9660
		public const short BandofStarpower = 111;

		// Token: 0x040025BD RID: 9661
		public const short FlowerofFire = 112;

		// Token: 0x040025BE RID: 9662
		public const short MagicMissile = 113;

		// Token: 0x040025BF RID: 9663
		public const short DirtRod = 114;

		// Token: 0x040025C0 RID: 9664
		public const short ShadowOrb = 115;

		// Token: 0x040025C1 RID: 9665
		public const short Meteorite = 116;

		// Token: 0x040025C2 RID: 9666
		public const short MeteoriteBar = 117;

		// Token: 0x040025C3 RID: 9667
		public const short Hook = 118;

		// Token: 0x040025C4 RID: 9668
		public const short Flamarang = 119;

		// Token: 0x040025C5 RID: 9669
		public const short MoltenFury = 120;

		// Token: 0x040025C6 RID: 9670
		public const short FieryGreatsword = 121;

		// Token: 0x040025C7 RID: 9671
		public const short MoltenPickaxe = 122;

		// Token: 0x040025C8 RID: 9672
		public const short MeteorHelmet = 123;

		// Token: 0x040025C9 RID: 9673
		public const short MeteorSuit = 124;

		// Token: 0x040025CA RID: 9674
		public const short MeteorLeggings = 125;

		// Token: 0x040025CB RID: 9675
		public const short BottledWater = 126;

		// Token: 0x040025CC RID: 9676
		public const short SpaceGun = 127;

		// Token: 0x040025CD RID: 9677
		public const short RocketBoots = 128;

		// Token: 0x040025CE RID: 9678
		public const short GrayBrick = 129;

		// Token: 0x040025CF RID: 9679
		public const short GrayBrickWall = 130;

		// Token: 0x040025D0 RID: 9680
		public const short RedBrick = 131;

		// Token: 0x040025D1 RID: 9681
		public const short RedBrickWall = 132;

		// Token: 0x040025D2 RID: 9682
		public const short ClayBlock = 133;

		// Token: 0x040025D3 RID: 9683
		public const short BlueBrick = 134;

		// Token: 0x040025D4 RID: 9684
		public const short BlueBrickWall = 135;

		// Token: 0x040025D5 RID: 9685
		public const short ChainLantern = 136;

		// Token: 0x040025D6 RID: 9686
		public const short GreenBrick = 137;

		// Token: 0x040025D7 RID: 9687
		public const short GreenBrickWall = 138;

		// Token: 0x040025D8 RID: 9688
		public const short PinkBrick = 139;

		// Token: 0x040025D9 RID: 9689
		public const short PinkBrickWall = 140;

		// Token: 0x040025DA RID: 9690
		public const short GoldBrick = 141;

		// Token: 0x040025DB RID: 9691
		public const short GoldBrickWall = 142;

		// Token: 0x040025DC RID: 9692
		public const short SilverBrick = 143;

		// Token: 0x040025DD RID: 9693
		public const short SilverBrickWall = 144;

		// Token: 0x040025DE RID: 9694
		public const short CopperBrick = 145;

		// Token: 0x040025DF RID: 9695
		public const short CopperBrickWall = 146;

		// Token: 0x040025E0 RID: 9696
		public const short Spike = 147;

		// Token: 0x040025E1 RID: 9697
		public const short WaterCandle = 148;

		// Token: 0x040025E2 RID: 9698
		public const short Book = 149;

		// Token: 0x040025E3 RID: 9699
		public const short Cobweb = 150;

		// Token: 0x040025E4 RID: 9700
		public const short NecroHelmet = 151;

		// Token: 0x040025E5 RID: 9701
		public const short NecroBreastplate = 152;

		// Token: 0x040025E6 RID: 9702
		public const short NecroGreaves = 153;

		// Token: 0x040025E7 RID: 9703
		public const short Bone = 154;

		// Token: 0x040025E8 RID: 9704
		public const short Muramasa = 155;

		// Token: 0x040025E9 RID: 9705
		public const short CobaltShield = 156;

		// Token: 0x040025EA RID: 9706
		public const short AquaScepter = 157;

		// Token: 0x040025EB RID: 9707
		public const short LuckyHorseshoe = 158;

		// Token: 0x040025EC RID: 9708
		public const short ShinyRedBalloon = 159;

		// Token: 0x040025ED RID: 9709
		public const short Harpoon = 160;

		// Token: 0x040025EE RID: 9710
		public const short SpikyBall = 161;

		// Token: 0x040025EF RID: 9711
		public const short BallOHurt = 162;

		// Token: 0x040025F0 RID: 9712
		public const short BlueMoon = 163;

		// Token: 0x040025F1 RID: 9713
		public const short Handgun = 164;

		// Token: 0x040025F2 RID: 9714
		public const short WaterBolt = 165;

		// Token: 0x040025F3 RID: 9715
		public const short Bomb = 166;

		// Token: 0x040025F4 RID: 9716
		public const short Dynamite = 167;

		// Token: 0x040025F5 RID: 9717
		public const short Grenade = 168;

		// Token: 0x040025F6 RID: 9718
		public const short SandBlock = 169;

		// Token: 0x040025F7 RID: 9719
		public const short Glass = 170;

		// Token: 0x040025F8 RID: 9720
		public const short Sign = 171;

		// Token: 0x040025F9 RID: 9721
		public const short AshBlock = 172;

		// Token: 0x040025FA RID: 9722
		public const short Obsidian = 173;

		// Token: 0x040025FB RID: 9723
		public const short Hellstone = 174;

		// Token: 0x040025FC RID: 9724
		public const short HellstoneBar = 175;

		// Token: 0x040025FD RID: 9725
		public const short MudBlock = 176;

		// Token: 0x040025FE RID: 9726
		public const short Sapphire = 177;

		// Token: 0x040025FF RID: 9727
		public const short Ruby = 178;

		// Token: 0x04002600 RID: 9728
		public const short Emerald = 179;

		// Token: 0x04002601 RID: 9729
		public const short Topaz = 180;

		// Token: 0x04002602 RID: 9730
		public const short Amethyst = 181;

		// Token: 0x04002603 RID: 9731
		public const short Diamond = 182;

		// Token: 0x04002604 RID: 9732
		public const short GlowingMushroom = 183;

		// Token: 0x04002605 RID: 9733
		public const short Star = 184;

		// Token: 0x04002606 RID: 9734
		public const short IvyWhip = 185;

		// Token: 0x04002607 RID: 9735
		public const short BreathingReed = 186;

		// Token: 0x04002608 RID: 9736
		public const short Flipper = 187;

		// Token: 0x04002609 RID: 9737
		public const short HealingPotion = 188;

		// Token: 0x0400260A RID: 9738
		public const short ManaPotion = 189;

		// Token: 0x0400260B RID: 9739
		public const short BladeofGrass = 190;

		// Token: 0x0400260C RID: 9740
		public const short ThornChakram = 191;

		// Token: 0x0400260D RID: 9741
		public const short ObsidianBrick = 192;

		// Token: 0x0400260E RID: 9742
		public const short ObsidianSkull = 193;

		// Token: 0x0400260F RID: 9743
		public const short MushroomGrassSeeds = 194;

		// Token: 0x04002610 RID: 9744
		public const short JungleGrassSeeds = 195;

		// Token: 0x04002611 RID: 9745
		public const short WoodenHammer = 196;

		// Token: 0x04002612 RID: 9746
		public const short StarCannon = 197;

		// Token: 0x04002613 RID: 9747
		public const short BluePhaseblade = 198;

		// Token: 0x04002614 RID: 9748
		public const short RedPhaseblade = 199;

		// Token: 0x04002615 RID: 9749
		public const short GreenPhaseblade = 200;

		// Token: 0x04002616 RID: 9750
		public const short PurplePhaseblade = 201;

		// Token: 0x04002617 RID: 9751
		public const short WhitePhaseblade = 202;

		// Token: 0x04002618 RID: 9752
		public const short YellowPhaseblade = 203;

		// Token: 0x04002619 RID: 9753
		public const short MeteorHamaxe = 204;

		// Token: 0x0400261A RID: 9754
		public const short EmptyBucket = 205;

		// Token: 0x0400261B RID: 9755
		public const short WaterBucket = 206;

		// Token: 0x0400261C RID: 9756
		public const short LavaBucket = 207;

		// Token: 0x0400261D RID: 9757
		public const short JungleRose = 208;

		// Token: 0x0400261E RID: 9758
		public const short Stinger = 209;

		// Token: 0x0400261F RID: 9759
		public const short Vine = 210;

		// Token: 0x04002620 RID: 9760
		public const short FeralClaws = 211;

		// Token: 0x04002621 RID: 9761
		public const short AnkletoftheWind = 212;

		// Token: 0x04002622 RID: 9762
		public const short StaffofRegrowth = 213;

		// Token: 0x04002623 RID: 9763
		public const short HellstoneBrick = 214;

		// Token: 0x04002624 RID: 9764
		public const short WhoopieCushion = 215;

		// Token: 0x04002625 RID: 9765
		public const short Shackle = 216;

		// Token: 0x04002626 RID: 9766
		public const short MoltenHamaxe = 217;

		// Token: 0x04002627 RID: 9767
		public const short Flamelash = 218;

		// Token: 0x04002628 RID: 9768
		public const short PhoenixBlaster = 219;

		// Token: 0x04002629 RID: 9769
		public const short Sunfury = 220;

		// Token: 0x0400262A RID: 9770
		public const short Hellforge = 221;

		// Token: 0x0400262B RID: 9771
		public const short ClayPot = 222;

		// Token: 0x0400262C RID: 9772
		public const short NaturesGift = 223;

		// Token: 0x0400262D RID: 9773
		public const short Bed = 224;

		// Token: 0x0400262E RID: 9774
		public const short Silk = 225;

		// Token: 0x0400262F RID: 9775
		public const short LesserRestorationPotion = 226;

		// Token: 0x04002630 RID: 9776
		public const short RestorationPotion = 227;

		// Token: 0x04002631 RID: 9777
		public const short JungleHat = 228;

		// Token: 0x04002632 RID: 9778
		public const short JungleShirt = 229;

		// Token: 0x04002633 RID: 9779
		public const short JunglePants = 230;

		// Token: 0x04002634 RID: 9780
		public const short MoltenHelmet = 231;

		// Token: 0x04002635 RID: 9781
		public const short MoltenBreastplate = 232;

		// Token: 0x04002636 RID: 9782
		public const short MoltenGreaves = 233;

		// Token: 0x04002637 RID: 9783
		public const short MeteorShot = 234;

		// Token: 0x04002638 RID: 9784
		public const short StickyBomb = 235;

		// Token: 0x04002639 RID: 9785
		public const short BlackLens = 236;

		// Token: 0x0400263A RID: 9786
		public const short Sunglasses = 237;

		// Token: 0x0400263B RID: 9787
		public const short WizardHat = 238;

		// Token: 0x0400263C RID: 9788
		public const short TopHat = 239;

		// Token: 0x0400263D RID: 9789
		public const short TuxedoShirt = 240;

		// Token: 0x0400263E RID: 9790
		public const short TuxedoPants = 241;

		// Token: 0x0400263F RID: 9791
		public const short SummerHat = 242;

		// Token: 0x04002640 RID: 9792
		public const short BunnyHood = 243;

		// Token: 0x04002641 RID: 9793
		public const short PlumbersHat = 244;

		// Token: 0x04002642 RID: 9794
		public const short PlumbersShirt = 245;

		// Token: 0x04002643 RID: 9795
		public const short PlumbersPants = 246;

		// Token: 0x04002644 RID: 9796
		public const short HerosHat = 247;

		// Token: 0x04002645 RID: 9797
		public const short HerosShirt = 248;

		// Token: 0x04002646 RID: 9798
		public const short HerosPants = 249;

		// Token: 0x04002647 RID: 9799
		public const short FishBowl = 250;

		// Token: 0x04002648 RID: 9800
		public const short ArchaeologistsHat = 251;

		// Token: 0x04002649 RID: 9801
		public const short ArchaeologistsJacket = 252;

		// Token: 0x0400264A RID: 9802
		public const short ArchaeologistsPants = 253;

		// Token: 0x0400264B RID: 9803
		public const short BlackThread = 254;

		// Token: 0x0400264C RID: 9804
		public const short GreenThread = 255;

		// Token: 0x0400264D RID: 9805
		public const short NinjaHood = 256;

		// Token: 0x0400264E RID: 9806
		public const short NinjaShirt = 257;

		// Token: 0x0400264F RID: 9807
		public const short NinjaPants = 258;

		// Token: 0x04002650 RID: 9808
		public const short Leather = 259;

		// Token: 0x04002651 RID: 9809
		public const short RedHat = 260;

		// Token: 0x04002652 RID: 9810
		public const short Goldfish = 261;

		// Token: 0x04002653 RID: 9811
		public const short Robe = 262;

		// Token: 0x04002654 RID: 9812
		public const short RobotHat = 263;

		// Token: 0x04002655 RID: 9813
		public const short GoldCrown = 264;

		// Token: 0x04002656 RID: 9814
		public const short HellfireArrow = 265;

		// Token: 0x04002657 RID: 9815
		public const short Sandgun = 266;

		// Token: 0x04002658 RID: 9816
		public const short GuideVoodooDoll = 267;

		// Token: 0x04002659 RID: 9817
		public const short DivingHelmet = 268;

		// Token: 0x0400265A RID: 9818
		public const short FamiliarShirt = 269;

		// Token: 0x0400265B RID: 9819
		public const short FamiliarPants = 270;

		// Token: 0x0400265C RID: 9820
		public const short FamiliarWig = 271;

		// Token: 0x0400265D RID: 9821
		public const short DemonScythe = 272;

		// Token: 0x0400265E RID: 9822
		public const short NightsEdge = 273;

		// Token: 0x0400265F RID: 9823
		public const short DarkLance = 274;

		// Token: 0x04002660 RID: 9824
		public const short Coral = 275;

		// Token: 0x04002661 RID: 9825
		public const short Cactus = 276;

		// Token: 0x04002662 RID: 9826
		public const short Trident = 277;

		// Token: 0x04002663 RID: 9827
		public const short SilverBullet = 278;

		// Token: 0x04002664 RID: 9828
		public const short ThrowingKnife = 279;

		// Token: 0x04002665 RID: 9829
		public const short Spear = 280;

		// Token: 0x04002666 RID: 9830
		public const short Blowpipe = 281;

		// Token: 0x04002667 RID: 9831
		public const short Glowstick = 282;

		// Token: 0x04002668 RID: 9832
		public const short Seed = 283;

		// Token: 0x04002669 RID: 9833
		public const short WoodenBoomerang = 284;

		// Token: 0x0400266A RID: 9834
		public const short Aglet = 285;

		// Token: 0x0400266B RID: 9835
		public const short StickyGlowstick = 286;

		// Token: 0x0400266C RID: 9836
		public const short PoisonedKnife = 287;

		// Token: 0x0400266D RID: 9837
		public const short ObsidianSkinPotion = 288;

		// Token: 0x0400266E RID: 9838
		public const short RegenerationPotion = 289;

		// Token: 0x0400266F RID: 9839
		public const short SwiftnessPotion = 290;

		// Token: 0x04002670 RID: 9840
		public const short GillsPotion = 291;

		// Token: 0x04002671 RID: 9841
		public const short IronskinPotion = 292;

		// Token: 0x04002672 RID: 9842
		public const short ManaRegenerationPotion = 293;

		// Token: 0x04002673 RID: 9843
		public const short MagicPowerPotion = 294;

		// Token: 0x04002674 RID: 9844
		public const short FeatherfallPotion = 295;

		// Token: 0x04002675 RID: 9845
		public const short SpelunkerPotion = 296;

		// Token: 0x04002676 RID: 9846
		public const short InvisibilityPotion = 297;

		// Token: 0x04002677 RID: 9847
		public const short ShinePotion = 298;

		// Token: 0x04002678 RID: 9848
		public const short NightOwlPotion = 299;

		// Token: 0x04002679 RID: 9849
		public const short BattlePotion = 300;

		// Token: 0x0400267A RID: 9850
		public const short ThornsPotion = 301;

		// Token: 0x0400267B RID: 9851
		public const short WaterWalkingPotion = 302;

		// Token: 0x0400267C RID: 9852
		public const short ArcheryPotion = 303;

		// Token: 0x0400267D RID: 9853
		public const short HunterPotion = 304;

		// Token: 0x0400267E RID: 9854
		public const short GravitationPotion = 305;

		// Token: 0x0400267F RID: 9855
		public const short GoldChest = 306;

		// Token: 0x04002680 RID: 9856
		public const short DaybloomSeeds = 307;

		// Token: 0x04002681 RID: 9857
		public const short MoonglowSeeds = 308;

		// Token: 0x04002682 RID: 9858
		public const short BlinkrootSeeds = 309;

		// Token: 0x04002683 RID: 9859
		public const short DeathweedSeeds = 310;

		// Token: 0x04002684 RID: 9860
		public const short WaterleafSeeds = 311;

		// Token: 0x04002685 RID: 9861
		public const short FireblossomSeeds = 312;

		// Token: 0x04002686 RID: 9862
		public const short Daybloom = 313;

		// Token: 0x04002687 RID: 9863
		public const short Moonglow = 314;

		// Token: 0x04002688 RID: 9864
		public const short Blinkroot = 315;

		// Token: 0x04002689 RID: 9865
		public const short Deathweed = 316;

		// Token: 0x0400268A RID: 9866
		public const short Waterleaf = 317;

		// Token: 0x0400268B RID: 9867
		public const short Fireblossom = 318;

		// Token: 0x0400268C RID: 9868
		public const short SharkFin = 319;

		// Token: 0x0400268D RID: 9869
		public const short Feather = 320;

		// Token: 0x0400268E RID: 9870
		public const short Tombstone = 321;

		// Token: 0x0400268F RID: 9871
		public const short MimeMask = 322;

		// Token: 0x04002690 RID: 9872
		public const short AntlionMandible = 323;

		// Token: 0x04002691 RID: 9873
		public const short IllegalGunParts = 324;

		// Token: 0x04002692 RID: 9874
		public const short TheDoctorsShirt = 325;

		// Token: 0x04002693 RID: 9875
		public const short TheDoctorsPants = 326;

		// Token: 0x04002694 RID: 9876
		public const short GoldenKey = 327;

		// Token: 0x04002695 RID: 9877
		public const short ShadowChest = 328;

		// Token: 0x04002696 RID: 9878
		public const short ShadowKey = 329;

		// Token: 0x04002697 RID: 9879
		public const short ObsidianBrickWall = 330;

		// Token: 0x04002698 RID: 9880
		public const short JungleSpores = 331;

		// Token: 0x04002699 RID: 9881
		public const short Loom = 332;

		// Token: 0x0400269A RID: 9882
		public const short Piano = 333;

		// Token: 0x0400269B RID: 9883
		public const short Dresser = 334;

		// Token: 0x0400269C RID: 9884
		public const short Bench = 335;

		// Token: 0x0400269D RID: 9885
		public const short Bathtub = 336;

		// Token: 0x0400269E RID: 9886
		public const short RedBanner = 337;

		// Token: 0x0400269F RID: 9887
		public const short GreenBanner = 338;

		// Token: 0x040026A0 RID: 9888
		public const short BlueBanner = 339;

		// Token: 0x040026A1 RID: 9889
		public const short YellowBanner = 340;

		// Token: 0x040026A2 RID: 9890
		public const short LampPost = 341;

		// Token: 0x040026A3 RID: 9891
		public const short TikiTorch = 342;

		// Token: 0x040026A4 RID: 9892
		public const short Barrel = 343;

		// Token: 0x040026A5 RID: 9893
		public const short ChineseLantern = 344;

		// Token: 0x040026A6 RID: 9894
		public const short CookingPot = 345;

		// Token: 0x040026A7 RID: 9895
		public const short Safe = 346;

		// Token: 0x040026A8 RID: 9896
		public const short SkullLantern = 347;

		// Token: 0x040026A9 RID: 9897
		public const short TrashCan = 348;

		// Token: 0x040026AA RID: 9898
		public const short Candelabra = 349;

		// Token: 0x040026AB RID: 9899
		public const short PinkVase = 350;

		// Token: 0x040026AC RID: 9900
		public const short Mug = 351;

		// Token: 0x040026AD RID: 9901
		public const short Keg = 352;

		// Token: 0x040026AE RID: 9902
		public const short Ale = 353;

		// Token: 0x040026AF RID: 9903
		public const short Bookcase = 354;

		// Token: 0x040026B0 RID: 9904
		public const short Throne = 355;

		// Token: 0x040026B1 RID: 9905
		public const short Bowl = 356;

		// Token: 0x040026B2 RID: 9906
		public const short BowlofSoup = 357;

		// Token: 0x040026B3 RID: 9907
		public const short Toilet = 358;

		// Token: 0x040026B4 RID: 9908
		public const short GrandfatherClock = 359;

		// Token: 0x040026B5 RID: 9909
		public const short ArmorStatue = 360;

		// Token: 0x040026B6 RID: 9910
		public const short GoblinBattleStandard = 361;

		// Token: 0x040026B7 RID: 9911
		public const short TatteredCloth = 362;

		// Token: 0x040026B8 RID: 9912
		public const short Sawmill = 363;

		// Token: 0x040026B9 RID: 9913
		public const short CobaltOre = 364;

		// Token: 0x040026BA RID: 9914
		public const short MythrilOre = 365;

		// Token: 0x040026BB RID: 9915
		public const short AdamantiteOre = 366;

		// Token: 0x040026BC RID: 9916
		public const short Pwnhammer = 367;

		// Token: 0x040026BD RID: 9917
		public const short Excalibur = 368;

		// Token: 0x040026BE RID: 9918
		public const short HallowedSeeds = 369;

		// Token: 0x040026BF RID: 9919
		public const short EbonsandBlock = 370;

		// Token: 0x040026C0 RID: 9920
		public const short CobaltHat = 371;

		// Token: 0x040026C1 RID: 9921
		public const short CobaltHelmet = 372;

		// Token: 0x040026C2 RID: 9922
		public const short CobaltMask = 373;

		// Token: 0x040026C3 RID: 9923
		public const short CobaltBreastplate = 374;

		// Token: 0x040026C4 RID: 9924
		public const short CobaltLeggings = 375;

		// Token: 0x040026C5 RID: 9925
		public const short MythrilHood = 376;

		// Token: 0x040026C6 RID: 9926
		public const short MythrilHelmet = 377;

		// Token: 0x040026C7 RID: 9927
		public const short MythrilHat = 378;

		// Token: 0x040026C8 RID: 9928
		public const short MythrilChainmail = 379;

		// Token: 0x040026C9 RID: 9929
		public const short MythrilGreaves = 380;

		// Token: 0x040026CA RID: 9930
		public const short CobaltBar = 381;

		// Token: 0x040026CB RID: 9931
		public const short MythrilBar = 382;

		// Token: 0x040026CC RID: 9932
		public const short CobaltChainsaw = 383;

		// Token: 0x040026CD RID: 9933
		public const short MythrilChainsaw = 384;

		// Token: 0x040026CE RID: 9934
		public const short CobaltDrill = 385;

		// Token: 0x040026CF RID: 9935
		public const short MythrilDrill = 386;

		// Token: 0x040026D0 RID: 9936
		public const short AdamantiteChainsaw = 387;

		// Token: 0x040026D1 RID: 9937
		public const short AdamantiteDrill = 388;

		// Token: 0x040026D2 RID: 9938
		public const short DaoofPow = 389;

		// Token: 0x040026D3 RID: 9939
		public const short MythrilHalberd = 390;

		// Token: 0x040026D4 RID: 9940
		public const short AdamantiteBar = 391;

		// Token: 0x040026D5 RID: 9941
		public const short GlassWall = 392;

		// Token: 0x040026D6 RID: 9942
		public const short Compass = 393;

		// Token: 0x040026D7 RID: 9943
		public const short DivingGear = 394;

		// Token: 0x040026D8 RID: 9944
		public const short GPS = 395;

		// Token: 0x040026D9 RID: 9945
		public const short ObsidianHorseshoe = 396;

		// Token: 0x040026DA RID: 9946
		public const short ObsidianShield = 397;

		// Token: 0x040026DB RID: 9947
		public const short TinkerersWorkshop = 398;

		// Token: 0x040026DC RID: 9948
		public const short CloudinaBalloon = 399;

		// Token: 0x040026DD RID: 9949
		public const short AdamantiteHeadgear = 400;

		// Token: 0x040026DE RID: 9950
		public const short AdamantiteHelmet = 401;

		// Token: 0x040026DF RID: 9951
		public const short AdamantiteMask = 402;

		// Token: 0x040026E0 RID: 9952
		public const short AdamantiteBreastplate = 403;

		// Token: 0x040026E1 RID: 9953
		public const short AdamantiteLeggings = 404;

		// Token: 0x040026E2 RID: 9954
		public const short SpectreBoots = 405;

		// Token: 0x040026E3 RID: 9955
		public const short AdamantiteGlaive = 406;

		// Token: 0x040026E4 RID: 9956
		public const short Toolbelt = 407;

		// Token: 0x040026E5 RID: 9957
		public const short PearlsandBlock = 408;

		// Token: 0x040026E6 RID: 9958
		public const short PearlstoneBlock = 409;

		// Token: 0x040026E7 RID: 9959
		public const short MiningShirt = 410;

		// Token: 0x040026E8 RID: 9960
		public const short MiningPants = 411;

		// Token: 0x040026E9 RID: 9961
		public const short PearlstoneBrick = 412;

		// Token: 0x040026EA RID: 9962
		public const short IridescentBrick = 413;

		// Token: 0x040026EB RID: 9963
		public const short MudstoneBlock = 414;

		// Token: 0x040026EC RID: 9964
		public const short CobaltBrick = 415;

		// Token: 0x040026ED RID: 9965
		public const short MythrilBrick = 416;

		// Token: 0x040026EE RID: 9966
		public const short PearlstoneBrickWall = 417;

		// Token: 0x040026EF RID: 9967
		public const short IridescentBrickWall = 418;

		// Token: 0x040026F0 RID: 9968
		public const short MudstoneBrickWall = 419;

		// Token: 0x040026F1 RID: 9969
		public const short CobaltBrickWall = 420;

		// Token: 0x040026F2 RID: 9970
		public const short MythrilBrickWall = 421;

		// Token: 0x040026F3 RID: 9971
		public const short HolyWater = 422;

		// Token: 0x040026F4 RID: 9972
		public const short UnholyWater = 423;

		// Token: 0x040026F5 RID: 9973
		public const short SiltBlock = 424;

		// Token: 0x040026F6 RID: 9974
		public const short FairyBell = 425;

		// Token: 0x040026F7 RID: 9975
		public const short BreakerBlade = 426;

		// Token: 0x040026F8 RID: 9976
		public const short BlueTorch = 427;

		// Token: 0x040026F9 RID: 9977
		public const short RedTorch = 428;

		// Token: 0x040026FA RID: 9978
		public const short GreenTorch = 429;

		// Token: 0x040026FB RID: 9979
		public const short PurpleTorch = 430;

		// Token: 0x040026FC RID: 9980
		public const short WhiteTorch = 431;

		// Token: 0x040026FD RID: 9981
		public const short YellowTorch = 432;

		// Token: 0x040026FE RID: 9982
		public const short DemonTorch = 433;

		// Token: 0x040026FF RID: 9983
		public const short ClockworkAssaultRifle = 434;

		// Token: 0x04002700 RID: 9984
		public const short CobaltRepeater = 435;

		// Token: 0x04002701 RID: 9985
		public const short MythrilRepeater = 436;

		// Token: 0x04002702 RID: 9986
		public const short DualHook = 437;

		// Token: 0x04002703 RID: 9987
		public const short StarStatue = 438;

		// Token: 0x04002704 RID: 9988
		public const short SwordStatue = 439;

		// Token: 0x04002705 RID: 9989
		public const short SlimeStatue = 440;

		// Token: 0x04002706 RID: 9990
		public const short GoblinStatue = 441;

		// Token: 0x04002707 RID: 9991
		public const short ShieldStatue = 442;

		// Token: 0x04002708 RID: 9992
		public const short BatStatue = 443;

		// Token: 0x04002709 RID: 9993
		public const short FishStatue = 444;

		// Token: 0x0400270A RID: 9994
		public const short BunnyStatue = 445;

		// Token: 0x0400270B RID: 9995
		public const short SkeletonStatue = 446;

		// Token: 0x0400270C RID: 9996
		public const short ReaperStatue = 447;

		// Token: 0x0400270D RID: 9997
		public const short WomanStatue = 448;

		// Token: 0x0400270E RID: 9998
		public const short ImpStatue = 449;

		// Token: 0x0400270F RID: 9999
		public const short GargoyleStatue = 450;

		// Token: 0x04002710 RID: 10000
		public const short GloomStatue = 451;

		// Token: 0x04002711 RID: 10001
		public const short HornetStatue = 452;

		// Token: 0x04002712 RID: 10002
		public const short BombStatue = 453;

		// Token: 0x04002713 RID: 10003
		public const short CrabStatue = 454;

		// Token: 0x04002714 RID: 10004
		public const short HammerStatue = 455;

		// Token: 0x04002715 RID: 10005
		public const short PotionStatue = 456;

		// Token: 0x04002716 RID: 10006
		public const short SpearStatue = 457;

		// Token: 0x04002717 RID: 10007
		public const short CrossStatue = 458;

		// Token: 0x04002718 RID: 10008
		public const short JellyfishStatue = 459;

		// Token: 0x04002719 RID: 10009
		public const short BowStatue = 460;

		// Token: 0x0400271A RID: 10010
		public const short BoomerangStatue = 461;

		// Token: 0x0400271B RID: 10011
		public const short BootStatue = 462;

		// Token: 0x0400271C RID: 10012
		public const short ChestStatue = 463;

		// Token: 0x0400271D RID: 10013
		public const short BirdStatue = 464;

		// Token: 0x0400271E RID: 10014
		public const short AxeStatue = 465;

		// Token: 0x0400271F RID: 10015
		public const short CorruptStatue = 466;

		// Token: 0x04002720 RID: 10016
		public const short TreeStatue = 467;

		// Token: 0x04002721 RID: 10017
		public const short AnvilStatue = 468;

		// Token: 0x04002722 RID: 10018
		public const short PickaxeStatue = 469;

		// Token: 0x04002723 RID: 10019
		public const short MushroomStatue = 470;

		// Token: 0x04002724 RID: 10020
		public const short EyeballStatue = 471;

		// Token: 0x04002725 RID: 10021
		public const short PillarStatue = 472;

		// Token: 0x04002726 RID: 10022
		public const short HeartStatue = 473;

		// Token: 0x04002727 RID: 10023
		public const short PotStatue = 474;

		// Token: 0x04002728 RID: 10024
		public const short SunflowerStatue = 475;

		// Token: 0x04002729 RID: 10025
		public const short KingStatue = 476;

		// Token: 0x0400272A RID: 10026
		public const short QueenStatue = 477;

		// Token: 0x0400272B RID: 10027
		public const short PiranhaStatue = 478;

		// Token: 0x0400272C RID: 10028
		public const short PlankedWall = 479;

		// Token: 0x0400272D RID: 10029
		public const short WoodenBeam = 480;

		// Token: 0x0400272E RID: 10030
		public const short AdamantiteRepeater = 481;

		// Token: 0x0400272F RID: 10031
		public const short AdamantiteSword = 482;

		// Token: 0x04002730 RID: 10032
		public const short CobaltSword = 483;

		// Token: 0x04002731 RID: 10033
		public const short MythrilSword = 484;

		// Token: 0x04002732 RID: 10034
		public const short MoonCharm = 485;

		// Token: 0x04002733 RID: 10035
		public const short Ruler = 486;

		// Token: 0x04002734 RID: 10036
		public const short CrystalBall = 487;

		// Token: 0x04002735 RID: 10037
		public const short DiscoBall = 488;

		// Token: 0x04002736 RID: 10038
		public const short SorcererEmblem = 489;

		// Token: 0x04002737 RID: 10039
		public const short WarriorEmblem = 490;

		// Token: 0x04002738 RID: 10040
		public const short RangerEmblem = 491;

		// Token: 0x04002739 RID: 10041
		public const short DemonWings = 492;

		// Token: 0x0400273A RID: 10042
		public const short AngelWings = 493;

		// Token: 0x0400273B RID: 10043
		public const short MagicalHarp = 494;

		// Token: 0x0400273C RID: 10044
		public const short RainbowRod = 495;

		// Token: 0x0400273D RID: 10045
		public const short IceRod = 496;

		// Token: 0x0400273E RID: 10046
		public const short NeptunesShell = 497;

		// Token: 0x0400273F RID: 10047
		public const short Mannequin = 498;

		// Token: 0x04002740 RID: 10048
		public const short GreaterHealingPotion = 499;

		// Token: 0x04002741 RID: 10049
		public const short GreaterManaPotion = 500;

		// Token: 0x04002742 RID: 10050
		public const short PixieDust = 501;

		// Token: 0x04002743 RID: 10051
		public const short CrystalShard = 502;

		// Token: 0x04002744 RID: 10052
		public const short ClownHat = 503;

		// Token: 0x04002745 RID: 10053
		public const short ClownShirt = 504;

		// Token: 0x04002746 RID: 10054
		public const short ClownPants = 505;

		// Token: 0x04002747 RID: 10055
		public const short Flamethrower = 506;

		// Token: 0x04002748 RID: 10056
		public const short Bell = 507;

		// Token: 0x04002749 RID: 10057
		public const short Harp = 508;

		// Token: 0x0400274A RID: 10058
		public const short Wrench = 509;

		// Token: 0x0400274B RID: 10059
		public const short WireCutter = 510;

		// Token: 0x0400274C RID: 10060
		public const short ActiveStoneBlock = 511;

		// Token: 0x0400274D RID: 10061
		public const short InactiveStoneBlock = 512;

		// Token: 0x0400274E RID: 10062
		public const short Lever = 513;

		// Token: 0x0400274F RID: 10063
		public const short LaserRifle = 514;

		// Token: 0x04002750 RID: 10064
		public const short CrystalBullet = 515;

		// Token: 0x04002751 RID: 10065
		public const short HolyArrow = 516;

		// Token: 0x04002752 RID: 10066
		public const short MagicDagger = 517;

		// Token: 0x04002753 RID: 10067
		public const short CrystalStorm = 518;

		// Token: 0x04002754 RID: 10068
		public const short CursedFlames = 519;

		// Token: 0x04002755 RID: 10069
		public const short SoulofLight = 520;

		// Token: 0x04002756 RID: 10070
		public const short SoulofNight = 521;

		// Token: 0x04002757 RID: 10071
		public const short CursedFlame = 522;

		// Token: 0x04002758 RID: 10072
		public const short CursedTorch = 523;

		// Token: 0x04002759 RID: 10073
		public const short AdamantiteForge = 524;

		// Token: 0x0400275A RID: 10074
		public const short MythrilAnvil = 525;

		// Token: 0x0400275B RID: 10075
		public const short UnicornHorn = 526;

		// Token: 0x0400275C RID: 10076
		public const short DarkShard = 527;

		// Token: 0x0400275D RID: 10077
		public const short LightShard = 528;

		// Token: 0x0400275E RID: 10078
		public const short RedPressurePlate = 529;

		// Token: 0x0400275F RID: 10079
		public const short Wire = 530;

		// Token: 0x04002760 RID: 10080
		public const short SpellTome = 531;

		// Token: 0x04002761 RID: 10081
		public const short StarCloak = 532;

		// Token: 0x04002762 RID: 10082
		public const short Megashark = 533;

		// Token: 0x04002763 RID: 10083
		public const short Shotgun = 534;

		// Token: 0x04002764 RID: 10084
		public const short PhilosophersStone = 535;

		// Token: 0x04002765 RID: 10085
		public const short TitanGlove = 536;

		// Token: 0x04002766 RID: 10086
		public const short CobaltNaginata = 537;

		// Token: 0x04002767 RID: 10087
		public const short Switch = 538;

		// Token: 0x04002768 RID: 10088
		public const short DartTrap = 539;

		// Token: 0x04002769 RID: 10089
		public const short Boulder = 540;

		// Token: 0x0400276A RID: 10090
		public const short GreenPressurePlate = 541;

		// Token: 0x0400276B RID: 10091
		public const short GrayPressurePlate = 542;

		// Token: 0x0400276C RID: 10092
		public const short BrownPressurePlate = 543;

		// Token: 0x0400276D RID: 10093
		public const short MechanicalEye = 544;

		// Token: 0x0400276E RID: 10094
		public const short CursedArrow = 545;

		// Token: 0x0400276F RID: 10095
		public const short CursedBullet = 546;

		// Token: 0x04002770 RID: 10096
		public const short SoulofFright = 547;

		// Token: 0x04002771 RID: 10097
		public const short SoulofMight = 548;

		// Token: 0x04002772 RID: 10098
		public const short SoulofSight = 549;

		// Token: 0x04002773 RID: 10099
		public const short Gungnir = 550;

		// Token: 0x04002774 RID: 10100
		public const short HallowedPlateMail = 551;

		// Token: 0x04002775 RID: 10101
		public const short HallowedGreaves = 552;

		// Token: 0x04002776 RID: 10102
		public const short HallowedHelmet = 553;

		// Token: 0x04002777 RID: 10103
		public const short CrossNecklace = 554;

		// Token: 0x04002778 RID: 10104
		public const short ManaFlower = 555;

		// Token: 0x04002779 RID: 10105
		public const short MechanicalWorm = 556;

		// Token: 0x0400277A RID: 10106
		public const short MechanicalSkull = 557;

		// Token: 0x0400277B RID: 10107
		public const short HallowedHeadgear = 558;

		// Token: 0x0400277C RID: 10108
		public const short HallowedMask = 559;

		// Token: 0x0400277D RID: 10109
		public const short SlimeCrown = 560;

		// Token: 0x0400277E RID: 10110
		public const short LightDisc = 561;

		// Token: 0x0400277F RID: 10111
		public const short MusicBoxOverworldDay = 562;

		// Token: 0x04002780 RID: 10112
		public const short MusicBoxEerie = 563;

		// Token: 0x04002781 RID: 10113
		public const short MusicBoxNight = 564;

		// Token: 0x04002782 RID: 10114
		public const short MusicBoxTitle = 565;

		// Token: 0x04002783 RID: 10115
		public const short MusicBoxUnderground = 566;

		// Token: 0x04002784 RID: 10116
		public const short MusicBoxBoss1 = 567;

		// Token: 0x04002785 RID: 10117
		public const short MusicBoxJungle = 568;

		// Token: 0x04002786 RID: 10118
		public const short MusicBoxCorruption = 569;

		// Token: 0x04002787 RID: 10119
		public const short MusicBoxUndergroundCorruption = 570;

		// Token: 0x04002788 RID: 10120
		public const short MusicBoxTheHallow = 571;

		// Token: 0x04002789 RID: 10121
		public const short MusicBoxBoss2 = 572;

		// Token: 0x0400278A RID: 10122
		public const short MusicBoxUndergroundHallow = 573;

		// Token: 0x0400278B RID: 10123
		public const short MusicBoxBoss3 = 574;

		// Token: 0x0400278C RID: 10124
		public const short SoulofFlight = 575;

		// Token: 0x0400278D RID: 10125
		public const short MusicBox = 576;

		// Token: 0x0400278E RID: 10126
		public const short DemoniteBrick = 577;

		// Token: 0x0400278F RID: 10127
		public const short HallowedRepeater = 578;

		// Token: 0x04002790 RID: 10128
		public const short Drax = 579;

		// Token: 0x04002791 RID: 10129
		public const short Explosives = 580;

		// Token: 0x04002792 RID: 10130
		public const short InletPump = 581;

		// Token: 0x04002793 RID: 10131
		public const short OutletPump = 582;

		// Token: 0x04002794 RID: 10132
		public const short Timer1Second = 583;

		// Token: 0x04002795 RID: 10133
		public const short Timer3Second = 584;

		// Token: 0x04002796 RID: 10134
		public const short Timer5Second = 585;

		// Token: 0x04002797 RID: 10135
		public const short CandyCaneBlock = 586;

		// Token: 0x04002798 RID: 10136
		public const short CandyCaneWall = 587;

		// Token: 0x04002799 RID: 10137
		public const short SantaHat = 588;

		// Token: 0x0400279A RID: 10138
		public const short SantaShirt = 589;

		// Token: 0x0400279B RID: 10139
		public const short SantaPants = 590;

		// Token: 0x0400279C RID: 10140
		public const short GreenCandyCaneBlock = 591;

		// Token: 0x0400279D RID: 10141
		public const short GreenCandyCaneWall = 592;

		// Token: 0x0400279E RID: 10142
		public const short SnowBlock = 593;

		// Token: 0x0400279F RID: 10143
		public const short SnowBrick = 594;

		// Token: 0x040027A0 RID: 10144
		public const short SnowBrickWall = 595;

		// Token: 0x040027A1 RID: 10145
		public const short BlueLight = 596;

		// Token: 0x040027A2 RID: 10146
		public const short RedLight = 597;

		// Token: 0x040027A3 RID: 10147
		public const short GreenLight = 598;

		// Token: 0x040027A4 RID: 10148
		public const short BluePresent = 599;

		// Token: 0x040027A5 RID: 10149
		public const short GreenPresent = 600;

		// Token: 0x040027A6 RID: 10150
		public const short YellowPresent = 601;

		// Token: 0x040027A7 RID: 10151
		public const short SnowGlobe = 602;

		// Token: 0x040027A8 RID: 10152
		public const short Carrot = 603;

		// Token: 0x040027A9 RID: 10153
		public const short AdamantiteBeam = 604;

		// Token: 0x040027AA RID: 10154
		public const short AdamantiteBeamWall = 605;

		// Token: 0x040027AB RID: 10155
		public const short DemoniteBrickWall = 606;

		// Token: 0x040027AC RID: 10156
		public const short SandstoneBrick = 607;

		// Token: 0x040027AD RID: 10157
		public const short SandstoneBrickWall = 608;

		// Token: 0x040027AE RID: 10158
		public const short EbonstoneBrick = 609;

		// Token: 0x040027AF RID: 10159
		public const short EbonstoneBrickWall = 610;

		// Token: 0x040027B0 RID: 10160
		public const short RedStucco = 611;

		// Token: 0x040027B1 RID: 10161
		public const short YellowStucco = 612;

		// Token: 0x040027B2 RID: 10162
		public const short GreenStucco = 613;

		// Token: 0x040027B3 RID: 10163
		public const short GrayStucco = 614;

		// Token: 0x040027B4 RID: 10164
		public const short RedStuccoWall = 615;

		// Token: 0x040027B5 RID: 10165
		public const short YellowStuccoWall = 616;

		// Token: 0x040027B6 RID: 10166
		public const short GreenStuccoWall = 617;

		// Token: 0x040027B7 RID: 10167
		public const short GrayStuccoWall = 618;

		// Token: 0x040027B8 RID: 10168
		public const short Ebonwood = 619;

		// Token: 0x040027B9 RID: 10169
		public const short RichMahogany = 620;

		// Token: 0x040027BA RID: 10170
		public const short Pearlwood = 621;

		// Token: 0x040027BB RID: 10171
		public const short EbonwoodWall = 622;

		// Token: 0x040027BC RID: 10172
		public const short RichMahoganyWall = 623;

		// Token: 0x040027BD RID: 10173
		public const short PearlwoodWall = 624;

		// Token: 0x040027BE RID: 10174
		public const short EbonwoodChest = 625;

		// Token: 0x040027BF RID: 10175
		public const short RichMahoganyChest = 626;

		// Token: 0x040027C0 RID: 10176
		public const short PearlwoodChest = 627;

		// Token: 0x040027C1 RID: 10177
		public const short EbonwoodChair = 628;

		// Token: 0x040027C2 RID: 10178
		public const short RichMahoganyChair = 629;

		// Token: 0x040027C3 RID: 10179
		public const short PearlwoodChair = 630;

		// Token: 0x040027C4 RID: 10180
		public const short EbonwoodPlatform = 631;

		// Token: 0x040027C5 RID: 10181
		public const short RichMahoganyPlatform = 632;

		// Token: 0x040027C6 RID: 10182
		public const short PearlwoodPlatform = 633;

		// Token: 0x040027C7 RID: 10183
		public const short BonePlatform = 634;

		// Token: 0x040027C8 RID: 10184
		public const short EbonwoodWorkBench = 635;

		// Token: 0x040027C9 RID: 10185
		public const short RichMahoganyWorkBench = 636;

		// Token: 0x040027CA RID: 10186
		public const short PearlwoodWorkBench = 637;

		// Token: 0x040027CB RID: 10187
		public const short EbonwoodTable = 638;

		// Token: 0x040027CC RID: 10188
		public const short RichMahoganyTable = 639;

		// Token: 0x040027CD RID: 10189
		public const short PearlwoodTable = 640;

		// Token: 0x040027CE RID: 10190
		public const short EbonwoodPiano = 641;

		// Token: 0x040027CF RID: 10191
		public const short RichMahoganyPiano = 642;

		// Token: 0x040027D0 RID: 10192
		public const short PearlwoodPiano = 643;

		// Token: 0x040027D1 RID: 10193
		public const short EbonwoodBed = 644;

		// Token: 0x040027D2 RID: 10194
		public const short RichMahoganyBed = 645;

		// Token: 0x040027D3 RID: 10195
		public const short PearlwoodBed = 646;

		// Token: 0x040027D4 RID: 10196
		public const short EbonwoodDresser = 647;

		// Token: 0x040027D5 RID: 10197
		public const short RichMahoganyDresser = 648;

		// Token: 0x040027D6 RID: 10198
		public const short PearlwoodDresser = 649;

		// Token: 0x040027D7 RID: 10199
		public const short EbonwoodDoor = 650;

		// Token: 0x040027D8 RID: 10200
		public const short RichMahoganyDoor = 651;

		// Token: 0x040027D9 RID: 10201
		public const short PearlwoodDoor = 652;

		// Token: 0x040027DA RID: 10202
		public const short EbonwoodSword = 653;

		// Token: 0x040027DB RID: 10203
		public const short EbonwoodHammer = 654;

		// Token: 0x040027DC RID: 10204
		public const short EbonwoodBow = 655;

		// Token: 0x040027DD RID: 10205
		public const short RichMahoganySword = 656;

		// Token: 0x040027DE RID: 10206
		public const short RichMahoganyHammer = 657;

		// Token: 0x040027DF RID: 10207
		public const short RichMahoganyBow = 658;

		// Token: 0x040027E0 RID: 10208
		public const short PearlwoodSword = 659;

		// Token: 0x040027E1 RID: 10209
		public const short PearlwoodHammer = 660;

		// Token: 0x040027E2 RID: 10210
		public const short PearlwoodBow = 661;

		// Token: 0x040027E3 RID: 10211
		public const short RainbowBrick = 662;

		// Token: 0x040027E4 RID: 10212
		public const short RainbowBrickWall = 663;

		// Token: 0x040027E5 RID: 10213
		public const short IceBlock = 664;

		// Token: 0x040027E6 RID: 10214
		public const short RedsWings = 665;

		// Token: 0x040027E7 RID: 10215
		public const short RedsHelmet = 666;

		// Token: 0x040027E8 RID: 10216
		public const short RedsBreastplate = 667;

		// Token: 0x040027E9 RID: 10217
		public const short RedsLeggings = 668;

		// Token: 0x040027EA RID: 10218
		public const short Fish = 669;

		// Token: 0x040027EB RID: 10219
		public const short IceBoomerang = 670;

		// Token: 0x040027EC RID: 10220
		public const short Keybrand = 671;

		// Token: 0x040027ED RID: 10221
		public const short Cutlass = 672;

		// Token: 0x040027EE RID: 10222
		public const short BorealWoodWorkBench = 673;

		// Token: 0x040027EF RID: 10223
		public const short TrueExcalibur = 674;

		// Token: 0x040027F0 RID: 10224
		public const short TrueNightsEdge = 675;

		// Token: 0x040027F1 RID: 10225
		public const short Frostbrand = 676;

		// Token: 0x040027F2 RID: 10226
		public const short BorealWoodTable = 677;

		// Token: 0x040027F3 RID: 10227
		public const short RedPotion = 678;

		// Token: 0x040027F4 RID: 10228
		public const short TacticalShotgun = 679;

		// Token: 0x040027F5 RID: 10229
		public const short IvyChest = 680;

		// Token: 0x040027F6 RID: 10230
		public const short IceChest = 681;

		// Token: 0x040027F7 RID: 10231
		public const short Marrow = 682;

		// Token: 0x040027F8 RID: 10232
		public const short UnholyTrident = 683;

		// Token: 0x040027F9 RID: 10233
		public const short FrostHelmet = 684;

		// Token: 0x040027FA RID: 10234
		public const short FrostBreastplate = 685;

		// Token: 0x040027FB RID: 10235
		public const short FrostLeggings = 686;

		// Token: 0x040027FC RID: 10236
		public const short TinHelmet = 687;

		// Token: 0x040027FD RID: 10237
		public const short TinChainmail = 688;

		// Token: 0x040027FE RID: 10238
		public const short TinGreaves = 689;

		// Token: 0x040027FF RID: 10239
		public const short LeadHelmet = 690;

		// Token: 0x04002800 RID: 10240
		public const short LeadChainmail = 691;

		// Token: 0x04002801 RID: 10241
		public const short LeadGreaves = 692;

		// Token: 0x04002802 RID: 10242
		public const short TungstenHelmet = 693;

		// Token: 0x04002803 RID: 10243
		public const short TungstenChainmail = 694;

		// Token: 0x04002804 RID: 10244
		public const short TungstenGreaves = 695;

		// Token: 0x04002805 RID: 10245
		public const short PlatinumHelmet = 696;

		// Token: 0x04002806 RID: 10246
		public const short PlatinumChainmail = 697;

		// Token: 0x04002807 RID: 10247
		public const short PlatinumGreaves = 698;

		// Token: 0x04002808 RID: 10248
		public const short TinOre = 699;

		// Token: 0x04002809 RID: 10249
		public const short LeadOre = 700;

		// Token: 0x0400280A RID: 10250
		public const short TungstenOre = 701;

		// Token: 0x0400280B RID: 10251
		public const short PlatinumOre = 702;

		// Token: 0x0400280C RID: 10252
		public const short TinBar = 703;

		// Token: 0x0400280D RID: 10253
		public const short LeadBar = 704;

		// Token: 0x0400280E RID: 10254
		public const short TungstenBar = 705;

		// Token: 0x0400280F RID: 10255
		public const short PlatinumBar = 706;

		// Token: 0x04002810 RID: 10256
		public const short TinWatch = 707;

		// Token: 0x04002811 RID: 10257
		public const short TungstenWatch = 708;

		// Token: 0x04002812 RID: 10258
		public const short PlatinumWatch = 709;

		// Token: 0x04002813 RID: 10259
		public const short TinChandelier = 710;

		// Token: 0x04002814 RID: 10260
		public const short TungstenChandelier = 711;

		// Token: 0x04002815 RID: 10261
		public const short PlatinumChandelier = 712;

		// Token: 0x04002816 RID: 10262
		public const short PlatinumCandle = 713;

		// Token: 0x04002817 RID: 10263
		public const short PlatinumCandelabra = 714;

		// Token: 0x04002818 RID: 10264
		public const short PlatinumCrown = 715;

		// Token: 0x04002819 RID: 10265
		public const short LeadAnvil = 716;

		// Token: 0x0400281A RID: 10266
		public const short TinBrick = 717;

		// Token: 0x0400281B RID: 10267
		public const short TungstenBrick = 718;

		// Token: 0x0400281C RID: 10268
		public const short PlatinumBrick = 719;

		// Token: 0x0400281D RID: 10269
		public const short TinBrickWall = 720;

		// Token: 0x0400281E RID: 10270
		public const short TungstenBrickWall = 721;

		// Token: 0x0400281F RID: 10271
		public const short PlatinumBrickWall = 722;

		// Token: 0x04002820 RID: 10272
		public const short BeamSword = 723;

		// Token: 0x04002821 RID: 10273
		public const short IceBlade = 724;

		// Token: 0x04002822 RID: 10274
		public const short IceBow = 725;

		// Token: 0x04002823 RID: 10275
		public const short FrostStaff = 726;

		// Token: 0x04002824 RID: 10276
		public const short WoodHelmet = 727;

		// Token: 0x04002825 RID: 10277
		public const short WoodBreastplate = 728;

		// Token: 0x04002826 RID: 10278
		public const short WoodGreaves = 729;

		// Token: 0x04002827 RID: 10279
		public const short EbonwoodHelmet = 730;

		// Token: 0x04002828 RID: 10280
		public const short EbonwoodBreastplate = 731;

		// Token: 0x04002829 RID: 10281
		public const short EbonwoodGreaves = 732;

		// Token: 0x0400282A RID: 10282
		public const short RichMahoganyHelmet = 733;

		// Token: 0x0400282B RID: 10283
		public const short RichMahoganyBreastplate = 734;

		// Token: 0x0400282C RID: 10284
		public const short RichMahoganyGreaves = 735;

		// Token: 0x0400282D RID: 10285
		public const short PearlwoodHelmet = 736;

		// Token: 0x0400282E RID: 10286
		public const short PearlwoodBreastplate = 737;

		// Token: 0x0400282F RID: 10287
		public const short PearlwoodGreaves = 738;

		// Token: 0x04002830 RID: 10288
		public const short AmethystStaff = 739;

		// Token: 0x04002831 RID: 10289
		public const short TopazStaff = 740;

		// Token: 0x04002832 RID: 10290
		public const short SapphireStaff = 741;

		// Token: 0x04002833 RID: 10291
		public const short EmeraldStaff = 742;

		// Token: 0x04002834 RID: 10292
		public const short RubyStaff = 743;

		// Token: 0x04002835 RID: 10293
		public const short DiamondStaff = 744;

		// Token: 0x04002836 RID: 10294
		public const short GrassWall = 745;

		// Token: 0x04002837 RID: 10295
		public const short JungleWall = 746;

		// Token: 0x04002838 RID: 10296
		public const short FlowerWall = 747;

		// Token: 0x04002839 RID: 10297
		public const short Jetpack = 748;

		// Token: 0x0400283A RID: 10298
		public const short ButterflyWings = 749;

		// Token: 0x0400283B RID: 10299
		public const short CactusWall = 750;

		// Token: 0x0400283C RID: 10300
		public const short Cloud = 751;

		// Token: 0x0400283D RID: 10301
		public const short CloudWall = 752;

		// Token: 0x0400283E RID: 10302
		public const short Seaweed = 753;

		// Token: 0x0400283F RID: 10303
		public const short RuneHat = 754;

		// Token: 0x04002840 RID: 10304
		public const short RuneRobe = 755;

		// Token: 0x04002841 RID: 10305
		public const short MushroomSpear = 756;

		// Token: 0x04002842 RID: 10306
		public const short TerraBlade = 757;

		// Token: 0x04002843 RID: 10307
		public const short GrenadeLauncher = 758;

		// Token: 0x04002844 RID: 10308
		public const short RocketLauncher = 759;

		// Token: 0x04002845 RID: 10309
		public const short ProximityMineLauncher = 760;

		// Token: 0x04002846 RID: 10310
		public const short FairyWings = 761;

		// Token: 0x04002847 RID: 10311
		public const short SlimeBlock = 762;

		// Token: 0x04002848 RID: 10312
		public const short FleshBlock = 763;

		// Token: 0x04002849 RID: 10313
		public const short MushroomWall = 764;

		// Token: 0x0400284A RID: 10314
		public const short RainCloud = 765;

		// Token: 0x0400284B RID: 10315
		public const short BoneBlock = 766;

		// Token: 0x0400284C RID: 10316
		public const short FrozenSlimeBlock = 767;

		// Token: 0x0400284D RID: 10317
		public const short BoneBlockWall = 768;

		// Token: 0x0400284E RID: 10318
		public const short SlimeBlockWall = 769;

		// Token: 0x0400284F RID: 10319
		public const short FleshBlockWall = 770;

		// Token: 0x04002850 RID: 10320
		public const short RocketI = 771;

		// Token: 0x04002851 RID: 10321
		public const short RocketII = 772;

		// Token: 0x04002852 RID: 10322
		public const short RocketIII = 773;

		// Token: 0x04002853 RID: 10323
		public const short RocketIV = 774;

		// Token: 0x04002854 RID: 10324
		public const short AsphaltBlock = 775;

		// Token: 0x04002855 RID: 10325
		public const short CobaltPickaxe = 776;

		// Token: 0x04002856 RID: 10326
		public const short MythrilPickaxe = 777;

		// Token: 0x04002857 RID: 10327
		public const short AdamantitePickaxe = 778;

		// Token: 0x04002858 RID: 10328
		public const short Clentaminator = 779;

		// Token: 0x04002859 RID: 10329
		public const short GreenSolution = 780;

		// Token: 0x0400285A RID: 10330
		public const short BlueSolution = 781;

		// Token: 0x0400285B RID: 10331
		public const short PurpleSolution = 782;

		// Token: 0x0400285C RID: 10332
		public const short DarkBlueSolution = 783;

		// Token: 0x0400285D RID: 10333
		public const short RedSolution = 784;

		// Token: 0x0400285E RID: 10334
		public const short HarpyWings = 785;

		// Token: 0x0400285F RID: 10335
		public const short BoneWings = 786;

		// Token: 0x04002860 RID: 10336
		public const short Hammush = 787;

		// Token: 0x04002861 RID: 10337
		public const short NettleBurst = 788;

		// Token: 0x04002862 RID: 10338
		public const short AnkhBanner = 789;

		// Token: 0x04002863 RID: 10339
		public const short SnakeBanner = 790;

		// Token: 0x04002864 RID: 10340
		public const short OmegaBanner = 791;

		// Token: 0x04002865 RID: 10341
		public const short CrimsonHelmet = 792;

		// Token: 0x04002866 RID: 10342
		public const short CrimsonScalemail = 793;

		// Token: 0x04002867 RID: 10343
		public const short CrimsonGreaves = 794;

		// Token: 0x04002868 RID: 10344
		public const short BloodButcherer = 795;

		// Token: 0x04002869 RID: 10345
		public const short TendonBow = 796;

		// Token: 0x0400286A RID: 10346
		public const short FleshGrinder = 797;

		// Token: 0x0400286B RID: 10347
		public const short DeathbringerPickaxe = 798;

		// Token: 0x0400286C RID: 10348
		public const short BloodLustCluster = 799;

		// Token: 0x0400286D RID: 10349
		public const short TheUndertaker = 800;

		// Token: 0x0400286E RID: 10350
		public const short TheMeatball = 801;

		// Token: 0x0400286F RID: 10351
		public const short TheRottedFork = 802;

		// Token: 0x04002870 RID: 10352
		public const short EskimoHood = 803;

		// Token: 0x04002871 RID: 10353
		public const short EskimoCoat = 804;

		// Token: 0x04002872 RID: 10354
		public const short EskimoPants = 805;

		// Token: 0x04002873 RID: 10355
		public const short LivingWoodChair = 806;

		// Token: 0x04002874 RID: 10356
		public const short CactusChair = 807;

		// Token: 0x04002875 RID: 10357
		public const short BoneChair = 808;

		// Token: 0x04002876 RID: 10358
		public const short FleshChair = 809;

		// Token: 0x04002877 RID: 10359
		public const short MushroomChair = 810;

		// Token: 0x04002878 RID: 10360
		public const short BoneWorkBench = 811;

		// Token: 0x04002879 RID: 10361
		public const short CactusWorkBench = 812;

		// Token: 0x0400287A RID: 10362
		public const short FleshWorkBench = 813;

		// Token: 0x0400287B RID: 10363
		public const short MushroomWorkBench = 814;

		// Token: 0x0400287C RID: 10364
		public const short SlimeWorkBench = 815;

		// Token: 0x0400287D RID: 10365
		public const short CactusDoor = 816;

		// Token: 0x0400287E RID: 10366
		public const short FleshDoor = 817;

		// Token: 0x0400287F RID: 10367
		public const short MushroomDoor = 818;

		// Token: 0x04002880 RID: 10368
		public const short LivingWoodDoor = 819;

		// Token: 0x04002881 RID: 10369
		public const short BoneDoor = 820;

		// Token: 0x04002882 RID: 10370
		public const short FlameWings = 821;

		// Token: 0x04002883 RID: 10371
		public const short FrozenWings = 822;

		// Token: 0x04002884 RID: 10372
		public const short GhostWings = 823;

		// Token: 0x04002885 RID: 10373
		public const short SunplateBlock = 824;

		// Token: 0x04002886 RID: 10374
		public const short DiscWall = 825;

		// Token: 0x04002887 RID: 10375
		public const short SkywareChair = 826;

		// Token: 0x04002888 RID: 10376
		public const short BoneTable = 827;

		// Token: 0x04002889 RID: 10377
		public const short FleshTable = 828;

		// Token: 0x0400288A RID: 10378
		public const short LivingWoodTable = 829;

		// Token: 0x0400288B RID: 10379
		public const short SkywareTable = 830;

		// Token: 0x0400288C RID: 10380
		public const short LivingWoodChest = 831;

		// Token: 0x0400288D RID: 10381
		public const short LivingWoodWand = 832;

		// Token: 0x0400288E RID: 10382
		public const short PurpleIceBlock = 833;

		// Token: 0x0400288F RID: 10383
		public const short PinkIceBlock = 834;

		// Token: 0x04002890 RID: 10384
		public const short RedIceBlock = 835;

		// Token: 0x04002891 RID: 10385
		public const short CrimstoneBlock = 836;

		// Token: 0x04002892 RID: 10386
		public const short SkywareDoor = 837;

		// Token: 0x04002893 RID: 10387
		public const short SkywareChest = 838;

		// Token: 0x04002894 RID: 10388
		public const short SteampunkHat = 839;

		// Token: 0x04002895 RID: 10389
		public const short SteampunkShirt = 840;

		// Token: 0x04002896 RID: 10390
		public const short SteampunkPants = 841;

		// Token: 0x04002897 RID: 10391
		public const short BeeHat = 842;

		// Token: 0x04002898 RID: 10392
		public const short BeeShirt = 843;

		// Token: 0x04002899 RID: 10393
		public const short BeePants = 844;

		// Token: 0x0400289A RID: 10394
		public const short WorldBanner = 845;

		// Token: 0x0400289B RID: 10395
		public const short SunBanner = 846;

		// Token: 0x0400289C RID: 10396
		public const short GravityBanner = 847;

		// Token: 0x0400289D RID: 10397
		public const short PharaohsMask = 848;

		// Token: 0x0400289E RID: 10398
		public const short Actuator = 849;

		// Token: 0x0400289F RID: 10399
		public const short BlueWrench = 850;

		// Token: 0x040028A0 RID: 10400
		public const short GreenWrench = 851;

		// Token: 0x040028A1 RID: 10401
		public const short BluePressurePlate = 852;

		// Token: 0x040028A2 RID: 10402
		public const short YellowPressurePlate = 853;

		// Token: 0x040028A3 RID: 10403
		public const short DiscountCard = 854;

		// Token: 0x040028A4 RID: 10404
		public const short LuckyCoin = 855;

		// Token: 0x040028A5 RID: 10405
		public const short UnicornonaStick = 856;

		// Token: 0x040028A6 RID: 10406
		public const short SandstorminaBottle = 857;

		// Token: 0x040028A7 RID: 10407
		public const short BorealWoodSofa = 858;

		// Token: 0x040028A8 RID: 10408
		public const short BeachBall = 859;

		// Token: 0x040028A9 RID: 10409
		public const short CharmofMyths = 860;

		// Token: 0x040028AA RID: 10410
		public const short MoonShell = 861;

		// Token: 0x040028AB RID: 10411
		public const short StarVeil = 862;

		// Token: 0x040028AC RID: 10412
		public const short WaterWalkingBoots = 863;

		// Token: 0x040028AD RID: 10413
		public const short Tiara = 864;

		// Token: 0x040028AE RID: 10414
		public const short PrincessDress = 865;

		// Token: 0x040028AF RID: 10415
		public const short PharaohsRobe = 866;

		// Token: 0x040028B0 RID: 10416
		public const short GreenCap = 867;

		// Token: 0x040028B1 RID: 10417
		public const short MushroomCap = 868;

		// Token: 0x040028B2 RID: 10418
		public const short TamOShanter = 869;

		// Token: 0x040028B3 RID: 10419
		public const short MummyMask = 870;

		// Token: 0x040028B4 RID: 10420
		public const short MummyShirt = 871;

		// Token: 0x040028B5 RID: 10421
		public const short MummyPants = 872;

		// Token: 0x040028B6 RID: 10422
		public const short CowboyHat = 873;

		// Token: 0x040028B7 RID: 10423
		public const short CowboyJacket = 874;

		// Token: 0x040028B8 RID: 10424
		public const short CowboyPants = 875;

		// Token: 0x040028B9 RID: 10425
		public const short PirateHat = 876;

		// Token: 0x040028BA RID: 10426
		public const short PirateShirt = 877;

		// Token: 0x040028BB RID: 10427
		public const short PiratePants = 878;

		// Token: 0x040028BC RID: 10428
		public const short VikingHelmet = 879;

		// Token: 0x040028BD RID: 10429
		public const short CrimtaneOre = 880;

		// Token: 0x040028BE RID: 10430
		public const short CactusSword = 881;

		// Token: 0x040028BF RID: 10431
		public const short CactusPickaxe = 882;

		// Token: 0x040028C0 RID: 10432
		public const short IceBrick = 883;

		// Token: 0x040028C1 RID: 10433
		public const short IceBrickWall = 884;

		// Token: 0x040028C2 RID: 10434
		public const short AdhesiveBandage = 885;

		// Token: 0x040028C3 RID: 10435
		public const short ArmorPolish = 886;

		// Token: 0x040028C4 RID: 10436
		public const short Bezoar = 887;

		// Token: 0x040028C5 RID: 10437
		public const short Blindfold = 888;

		// Token: 0x040028C6 RID: 10438
		public const short FastClock = 889;

		// Token: 0x040028C7 RID: 10439
		public const short Megaphone = 890;

		// Token: 0x040028C8 RID: 10440
		public const short Nazar = 891;

		// Token: 0x040028C9 RID: 10441
		public const short Vitamins = 892;

		// Token: 0x040028CA RID: 10442
		public const short TrifoldMap = 893;

		// Token: 0x040028CB RID: 10443
		public const short CactusHelmet = 894;

		// Token: 0x040028CC RID: 10444
		public const short CactusBreastplate = 895;

		// Token: 0x040028CD RID: 10445
		public const short CactusLeggings = 896;

		// Token: 0x040028CE RID: 10446
		public const short PowerGlove = 897;

		// Token: 0x040028CF RID: 10447
		public const short LightningBoots = 898;

		// Token: 0x040028D0 RID: 10448
		public const short SunStone = 899;

		// Token: 0x040028D1 RID: 10449
		public const short MoonStone = 900;

		// Token: 0x040028D2 RID: 10450
		public const short ArmorBracing = 901;

		// Token: 0x040028D3 RID: 10451
		public const short MedicatedBandage = 902;

		// Token: 0x040028D4 RID: 10452
		public const short ThePlan = 903;

		// Token: 0x040028D5 RID: 10453
		public const short CountercurseMantra = 904;

		// Token: 0x040028D6 RID: 10454
		public const short CoinGun = 905;

		// Token: 0x040028D7 RID: 10455
		public const short LavaCharm = 906;

		// Token: 0x040028D8 RID: 10456
		public const short ObsidianWaterWalkingBoots = 907;

		// Token: 0x040028D9 RID: 10457
		public const short LavaWaders = 908;

		// Token: 0x040028DA RID: 10458
		public const short PureWaterFountain = 909;

		// Token: 0x040028DB RID: 10459
		public const short DesertWaterFountain = 910;

		// Token: 0x040028DC RID: 10460
		public const short Shadewood = 911;

		// Token: 0x040028DD RID: 10461
		public const short ShadewoodDoor = 912;

		// Token: 0x040028DE RID: 10462
		public const short ShadewoodPlatform = 913;

		// Token: 0x040028DF RID: 10463
		public const short ShadewoodChest = 914;

		// Token: 0x040028E0 RID: 10464
		public const short ShadewoodChair = 915;

		// Token: 0x040028E1 RID: 10465
		public const short ShadewoodWorkBench = 916;

		// Token: 0x040028E2 RID: 10466
		public const short ShadewoodTable = 917;

		// Token: 0x040028E3 RID: 10467
		public const short ShadewoodDresser = 918;

		// Token: 0x040028E4 RID: 10468
		public const short ShadewoodPiano = 919;

		// Token: 0x040028E5 RID: 10469
		public const short ShadewoodBed = 920;

		// Token: 0x040028E6 RID: 10470
		public const short ShadewoodSword = 921;

		// Token: 0x040028E7 RID: 10471
		public const short ShadewoodHammer = 922;

		// Token: 0x040028E8 RID: 10472
		public const short ShadewoodBow = 923;

		// Token: 0x040028E9 RID: 10473
		public const short ShadewoodHelmet = 924;

		// Token: 0x040028EA RID: 10474
		public const short ShadewoodBreastplate = 925;

		// Token: 0x040028EB RID: 10475
		public const short ShadewoodGreaves = 926;

		// Token: 0x040028EC RID: 10476
		public const short ShadewoodWall = 927;

		// Token: 0x040028ED RID: 10477
		public const short Cannon = 928;

		// Token: 0x040028EE RID: 10478
		public const short Cannonball = 929;

		// Token: 0x040028EF RID: 10479
		public const short FlareGun = 930;

		// Token: 0x040028F0 RID: 10480
		public const short Flare = 931;

		// Token: 0x040028F1 RID: 10481
		public const short BoneWand = 932;

		// Token: 0x040028F2 RID: 10482
		public const short LeafWand = 933;

		// Token: 0x040028F3 RID: 10483
		public const short FlyingCarpet = 934;

		// Token: 0x040028F4 RID: 10484
		public const short AvengerEmblem = 935;

		// Token: 0x040028F5 RID: 10485
		public const short MechanicalGlove = 936;

		// Token: 0x040028F6 RID: 10486
		public const short LandMine = 937;

		// Token: 0x040028F7 RID: 10487
		public const short PaladinsShield = 938;

		// Token: 0x040028F8 RID: 10488
		public const short WebSlinger = 939;

		// Token: 0x040028F9 RID: 10489
		public const short JungleWaterFountain = 940;

		// Token: 0x040028FA RID: 10490
		public const short IcyWaterFountain = 941;

		// Token: 0x040028FB RID: 10491
		public const short CorruptWaterFountain = 942;

		// Token: 0x040028FC RID: 10492
		public const short CrimsonWaterFountain = 943;

		// Token: 0x040028FD RID: 10493
		public const short HallowedWaterFountain = 944;

		// Token: 0x040028FE RID: 10494
		public const short BloodWaterFountain = 945;

		// Token: 0x040028FF RID: 10495
		public const short Umbrella = 946;

		// Token: 0x04002900 RID: 10496
		public const short ChlorophyteOre = 947;

		// Token: 0x04002901 RID: 10497
		public const short SteampunkWings = 948;

		// Token: 0x04002902 RID: 10498
		public const short Snowball = 949;

		// Token: 0x04002903 RID: 10499
		public const short IceSkates = 950;

		// Token: 0x04002904 RID: 10500
		public const short SnowballLauncher = 951;

		// Token: 0x04002905 RID: 10501
		public const short WebCoveredChest = 952;

		// Token: 0x04002906 RID: 10502
		public const short ClimbingClaws = 953;

		// Token: 0x04002907 RID: 10503
		public const short AncientIronHelmet = 954;

		// Token: 0x04002908 RID: 10504
		public const short AncientGoldHelmet = 955;

		// Token: 0x04002909 RID: 10505
		public const short AncientShadowHelmet = 956;

		// Token: 0x0400290A RID: 10506
		public const short AncientShadowScalemail = 957;

		// Token: 0x0400290B RID: 10507
		public const short AncientShadowGreaves = 958;

		// Token: 0x0400290C RID: 10508
		public const short AncientNecroHelmet = 959;

		// Token: 0x0400290D RID: 10509
		public const short AncientCobaltHelmet = 960;

		// Token: 0x0400290E RID: 10510
		public const short AncientCobaltBreastplate = 961;

		// Token: 0x0400290F RID: 10511
		public const short AncientCobaltLeggings = 962;

		// Token: 0x04002910 RID: 10512
		public const short BlackBelt = 963;

		// Token: 0x04002911 RID: 10513
		public const short Boomstick = 964;

		// Token: 0x04002912 RID: 10514
		public const short Rope = 965;

		// Token: 0x04002913 RID: 10515
		public const short Campfire = 966;

		// Token: 0x04002914 RID: 10516
		public const short Marshmallow = 967;

		// Token: 0x04002915 RID: 10517
		public const short MarshmallowonaStick = 968;

		// Token: 0x04002916 RID: 10518
		public const short CookedMarshmallow = 969;

		// Token: 0x04002917 RID: 10519
		public const short RedRocket = 970;

		// Token: 0x04002918 RID: 10520
		public const short GreenRocket = 971;

		// Token: 0x04002919 RID: 10521
		public const short BlueRocket = 972;

		// Token: 0x0400291A RID: 10522
		public const short YellowRocket = 973;

		// Token: 0x0400291B RID: 10523
		public const short IceTorch = 974;

		// Token: 0x0400291C RID: 10524
		public const short ShoeSpikes = 975;

		// Token: 0x0400291D RID: 10525
		public const short TigerClimbingGear = 976;

		// Token: 0x0400291E RID: 10526
		public const short Tabi = 977;

		// Token: 0x0400291F RID: 10527
		public const short PinkEskimoHood = 978;

		// Token: 0x04002920 RID: 10528
		public const short PinkEskimoCoat = 979;

		// Token: 0x04002921 RID: 10529
		public const short PinkEskimoPants = 980;

		// Token: 0x04002922 RID: 10530
		public const short PinkThread = 981;

		// Token: 0x04002923 RID: 10531
		public const short ManaRegenerationBand = 982;

		// Token: 0x04002924 RID: 10532
		public const short SandstorminaBalloon = 983;

		// Token: 0x04002925 RID: 10533
		public const short MasterNinjaGear = 984;

		// Token: 0x04002926 RID: 10534
		public const short RopeCoil = 985;

		// Token: 0x04002927 RID: 10535
		public const short Blowgun = 986;

		// Token: 0x04002928 RID: 10536
		public const short BlizzardinaBottle = 987;

		// Token: 0x04002929 RID: 10537
		public const short FrostburnArrow = 988;

		// Token: 0x0400292A RID: 10538
		public const short EnchantedSword = 989;

		// Token: 0x0400292B RID: 10539
		public const short PickaxeAxe = 990;

		// Token: 0x0400292C RID: 10540
		public const short CobaltWaraxe = 991;

		// Token: 0x0400292D RID: 10541
		public const short MythrilWaraxe = 992;

		// Token: 0x0400292E RID: 10542
		public const short AdamantiteWaraxe = 993;

		// Token: 0x0400292F RID: 10543
		public const short EatersBone = 994;

		// Token: 0x04002930 RID: 10544
		public const short BlendOMatic = 995;

		// Token: 0x04002931 RID: 10545
		public const short MeatGrinder = 996;

		// Token: 0x04002932 RID: 10546
		public const short Extractinator = 997;

		// Token: 0x04002933 RID: 10547
		public const short Solidifier = 998;

		// Token: 0x04002934 RID: 10548
		public const short Amber = 999;

		// Token: 0x04002935 RID: 10549
		public const short ConfettiGun = 1000;

		// Token: 0x04002936 RID: 10550
		public const short ChlorophyteMask = 1001;

		// Token: 0x04002937 RID: 10551
		public const short ChlorophyteHelmet = 1002;

		// Token: 0x04002938 RID: 10552
		public const short ChlorophyteHeadgear = 1003;

		// Token: 0x04002939 RID: 10553
		public const short ChlorophytePlateMail = 1004;

		// Token: 0x0400293A RID: 10554
		public const short ChlorophyteGreaves = 1005;

		// Token: 0x0400293B RID: 10555
		public const short ChlorophyteBar = 1006;

		// Token: 0x0400293C RID: 10556
		public const short RedDye = 1007;

		// Token: 0x0400293D RID: 10557
		public const short OrangeDye = 1008;

		// Token: 0x0400293E RID: 10558
		public const short YellowDye = 1009;

		// Token: 0x0400293F RID: 10559
		public const short LimeDye = 1010;

		// Token: 0x04002940 RID: 10560
		public const short GreenDye = 1011;

		// Token: 0x04002941 RID: 10561
		public const short TealDye = 1012;

		// Token: 0x04002942 RID: 10562
		public const short CyanDye = 1013;

		// Token: 0x04002943 RID: 10563
		public const short SkyBlueDye = 1014;

		// Token: 0x04002944 RID: 10564
		public const short BlueDye = 1015;

		// Token: 0x04002945 RID: 10565
		public const short PurpleDye = 1016;

		// Token: 0x04002946 RID: 10566
		public const short VioletDye = 1017;

		// Token: 0x04002947 RID: 10567
		public const short PinkDye = 1018;

		// Token: 0x04002948 RID: 10568
		public const short RedandBlackDye = 1019;

		// Token: 0x04002949 RID: 10569
		public const short OrangeandBlackDye = 1020;

		// Token: 0x0400294A RID: 10570
		public const short YellowandBlackDye = 1021;

		// Token: 0x0400294B RID: 10571
		public const short LimeandBlackDye = 1022;

		// Token: 0x0400294C RID: 10572
		public const short GreenandBlackDye = 1023;

		// Token: 0x0400294D RID: 10573
		public const short TealandBlackDye = 1024;

		// Token: 0x0400294E RID: 10574
		public const short CyanandBlackDye = 1025;

		// Token: 0x0400294F RID: 10575
		public const short SkyBlueandBlackDye = 1026;

		// Token: 0x04002950 RID: 10576
		public const short BlueandBlackDye = 1027;

		// Token: 0x04002951 RID: 10577
		public const short PurpleandBlackDye = 1028;

		// Token: 0x04002952 RID: 10578
		public const short VioletandBlackDye = 1029;

		// Token: 0x04002953 RID: 10579
		public const short PinkandBlackDye = 1030;

		// Token: 0x04002954 RID: 10580
		public const short FlameDye = 1031;

		// Token: 0x04002955 RID: 10581
		public const short FlameAndBlackDye = 1032;

		// Token: 0x04002956 RID: 10582
		public const short GreenFlameDye = 1033;

		// Token: 0x04002957 RID: 10583
		public const short GreenFlameAndBlackDye = 1034;

		// Token: 0x04002958 RID: 10584
		public const short BlueFlameDye = 1035;

		// Token: 0x04002959 RID: 10585
		public const short BlueFlameAndBlackDye = 1036;

		// Token: 0x0400295A RID: 10586
		public const short SilverDye = 1037;

		// Token: 0x0400295B RID: 10587
		public const short BrightRedDye = 1038;

		// Token: 0x0400295C RID: 10588
		public const short BrightOrangeDye = 1039;

		// Token: 0x0400295D RID: 10589
		public const short BrightYellowDye = 1040;

		// Token: 0x0400295E RID: 10590
		public const short BrightLimeDye = 1041;

		// Token: 0x0400295F RID: 10591
		public const short BrightGreenDye = 1042;

		// Token: 0x04002960 RID: 10592
		public const short BrightTealDye = 1043;

		// Token: 0x04002961 RID: 10593
		public const short BrightCyanDye = 1044;

		// Token: 0x04002962 RID: 10594
		public const short BrightSkyBlueDye = 1045;

		// Token: 0x04002963 RID: 10595
		public const short BrightBlueDye = 1046;

		// Token: 0x04002964 RID: 10596
		public const short BrightPurpleDye = 1047;

		// Token: 0x04002965 RID: 10597
		public const short BrightVioletDye = 1048;

		// Token: 0x04002966 RID: 10598
		public const short BrightPinkDye = 1049;

		// Token: 0x04002967 RID: 10599
		public const short BlackDye = 1050;

		// Token: 0x04002968 RID: 10600
		public const short RedandSilverDye = 1051;

		// Token: 0x04002969 RID: 10601
		public const short OrangeandSilverDye = 1052;

		// Token: 0x0400296A RID: 10602
		public const short YellowandSilverDye = 1053;

		// Token: 0x0400296B RID: 10603
		public const short LimeandSilverDye = 1054;

		// Token: 0x0400296C RID: 10604
		public const short GreenandSilverDye = 1055;

		// Token: 0x0400296D RID: 10605
		public const short TealandSilverDye = 1056;

		// Token: 0x0400296E RID: 10606
		public const short CyanandSilverDye = 1057;

		// Token: 0x0400296F RID: 10607
		public const short SkyBlueandSilverDye = 1058;

		// Token: 0x04002970 RID: 10608
		public const short BlueandSilverDye = 1059;

		// Token: 0x04002971 RID: 10609
		public const short PurpleandSilverDye = 1060;

		// Token: 0x04002972 RID: 10610
		public const short VioletandSilverDye = 1061;

		// Token: 0x04002973 RID: 10611
		public const short PinkandSilverDye = 1062;

		// Token: 0x04002974 RID: 10612
		public const short IntenseFlameDye = 1063;

		// Token: 0x04002975 RID: 10613
		public const short IntenseGreenFlameDye = 1064;

		// Token: 0x04002976 RID: 10614
		public const short IntenseBlueFlameDye = 1065;

		// Token: 0x04002977 RID: 10615
		public const short RainbowDye = 1066;

		// Token: 0x04002978 RID: 10616
		public const short IntenseRainbowDye = 1067;

		// Token: 0x04002979 RID: 10617
		public const short YellowGradientDye = 1068;

		// Token: 0x0400297A RID: 10618
		public const short CyanGradientDye = 1069;

		// Token: 0x0400297B RID: 10619
		public const short VioletGradientDye = 1070;

		// Token: 0x0400297C RID: 10620
		public const short Paintbrush = 1071;

		// Token: 0x0400297D RID: 10621
		public const short PaintRoller = 1072;

		// Token: 0x0400297E RID: 10622
		public const short RedPaint = 1073;

		// Token: 0x0400297F RID: 10623
		public const short OrangePaint = 1074;

		// Token: 0x04002980 RID: 10624
		public const short YellowPaint = 1075;

		// Token: 0x04002981 RID: 10625
		public const short LimePaint = 1076;

		// Token: 0x04002982 RID: 10626
		public const short GreenPaint = 1077;

		// Token: 0x04002983 RID: 10627
		public const short TealPaint = 1078;

		// Token: 0x04002984 RID: 10628
		public const short CyanPaint = 1079;

		// Token: 0x04002985 RID: 10629
		public const short SkyBluePaint = 1080;

		// Token: 0x04002986 RID: 10630
		public const short BluePaint = 1081;

		// Token: 0x04002987 RID: 10631
		public const short PurplePaint = 1082;

		// Token: 0x04002988 RID: 10632
		public const short VioletPaint = 1083;

		// Token: 0x04002989 RID: 10633
		public const short PinkPaint = 1084;

		// Token: 0x0400298A RID: 10634
		public const short DeepRedPaint = 1085;

		// Token: 0x0400298B RID: 10635
		public const short DeepOrangePaint = 1086;

		// Token: 0x0400298C RID: 10636
		public const short DeepYellowPaint = 1087;

		// Token: 0x0400298D RID: 10637
		public const short DeepLimePaint = 1088;

		// Token: 0x0400298E RID: 10638
		public const short DeepGreenPaint = 1089;

		// Token: 0x0400298F RID: 10639
		public const short DeepTealPaint = 1090;

		// Token: 0x04002990 RID: 10640
		public const short DeepCyanPaint = 1091;

		// Token: 0x04002991 RID: 10641
		public const short DeepSkyBluePaint = 1092;

		// Token: 0x04002992 RID: 10642
		public const short DeepBluePaint = 1093;

		// Token: 0x04002993 RID: 10643
		public const short DeepPurplePaint = 1094;

		// Token: 0x04002994 RID: 10644
		public const short DeepVioletPaint = 1095;

		// Token: 0x04002995 RID: 10645
		public const short DeepPinkPaint = 1096;

		// Token: 0x04002996 RID: 10646
		public const short BlackPaint = 1097;

		// Token: 0x04002997 RID: 10647
		public const short WhitePaint = 1098;

		// Token: 0x04002998 RID: 10648
		public const short GrayPaint = 1099;

		// Token: 0x04002999 RID: 10649
		public const short PaintScraper = 1100;

		// Token: 0x0400299A RID: 10650
		public const short LihzahrdBrick = 1101;

		// Token: 0x0400299B RID: 10651
		public const short LihzahrdBrickWall = 1102;

		// Token: 0x0400299C RID: 10652
		public const short SlushBlock = 1103;

		// Token: 0x0400299D RID: 10653
		public const short PalladiumOre = 1104;

		// Token: 0x0400299E RID: 10654
		public const short OrichalcumOre = 1105;

		// Token: 0x0400299F RID: 10655
		public const short TitaniumOre = 1106;

		// Token: 0x040029A0 RID: 10656
		public const short TealMushroom = 1107;

		// Token: 0x040029A1 RID: 10657
		public const short GreenMushroom = 1108;

		// Token: 0x040029A2 RID: 10658
		public const short SkyBlueFlower = 1109;

		// Token: 0x040029A3 RID: 10659
		public const short YellowMarigold = 1110;

		// Token: 0x040029A4 RID: 10660
		public const short BlueBerries = 1111;

		// Token: 0x040029A5 RID: 10661
		public const short LimeKelp = 1112;

		// Token: 0x040029A6 RID: 10662
		public const short PinkPricklyPear = 1113;

		// Token: 0x040029A7 RID: 10663
		public const short OrangeBloodroot = 1114;

		// Token: 0x040029A8 RID: 10664
		public const short RedHusk = 1115;

		// Token: 0x040029A9 RID: 10665
		public const short CyanHusk = 1116;

		// Token: 0x040029AA RID: 10666
		public const short VioletHusk = 1117;

		// Token: 0x040029AB RID: 10667
		public const short PurpleMucos = 1118;

		// Token: 0x040029AC RID: 10668
		public const short BlackInk = 1119;

		// Token: 0x040029AD RID: 10669
		public const short DyeVat = 1120;

		// Token: 0x040029AE RID: 10670
		public const short BeeGun = 1121;

		// Token: 0x040029AF RID: 10671
		public const short PossessedHatchet = 1122;

		// Token: 0x040029B0 RID: 10672
		public const short BeeKeeper = 1123;

		// Token: 0x040029B1 RID: 10673
		public const short Hive = 1124;

		// Token: 0x040029B2 RID: 10674
		public const short HoneyBlock = 1125;

		// Token: 0x040029B3 RID: 10675
		public const short HiveWall = 1126;

		// Token: 0x040029B4 RID: 10676
		public const short CrispyHoneyBlock = 1127;

		// Token: 0x040029B5 RID: 10677
		public const short HoneyBucket = 1128;

		// Token: 0x040029B6 RID: 10678
		public const short HiveWand = 1129;

		// Token: 0x040029B7 RID: 10679
		public const short Beenade = 1130;

		// Token: 0x040029B8 RID: 10680
		public const short GravityGlobe = 1131;

		// Token: 0x040029B9 RID: 10681
		public const short HoneyComb = 1132;

		// Token: 0x040029BA RID: 10682
		public const short Abeemination = 1133;

		// Token: 0x040029BB RID: 10683
		public const short BottledHoney = 1134;

		// Token: 0x040029BC RID: 10684
		public const short RainHat = 1135;

		// Token: 0x040029BD RID: 10685
		public const short RainCoat = 1136;

		// Token: 0x040029BE RID: 10686
		public const short LihzahrdDoor = 1137;

		// Token: 0x040029BF RID: 10687
		public const short DungeonDoor = 1138;

		// Token: 0x040029C0 RID: 10688
		public const short LeadDoor = 1139;

		// Token: 0x040029C1 RID: 10689
		public const short IronDoor = 1140;

		// Token: 0x040029C2 RID: 10690
		public const short TempleKey = 1141;

		// Token: 0x040029C3 RID: 10691
		public const short LihzahrdChest = 1142;

		// Token: 0x040029C4 RID: 10692
		public const short LihzahrdChair = 1143;

		// Token: 0x040029C5 RID: 10693
		public const short LihzahrdTable = 1144;

		// Token: 0x040029C6 RID: 10694
		public const short LihzahrdWorkBench = 1145;

		// Token: 0x040029C7 RID: 10695
		public const short SuperDartTrap = 1146;

		// Token: 0x040029C8 RID: 10696
		public const short FlameTrap = 1147;

		// Token: 0x040029C9 RID: 10697
		public const short SpikyBallTrap = 1148;

		// Token: 0x040029CA RID: 10698
		public const short SpearTrap = 1149;

		// Token: 0x040029CB RID: 10699
		public const short WoodenSpike = 1150;

		// Token: 0x040029CC RID: 10700
		public const short LihzahrdPressurePlate = 1151;

		// Token: 0x040029CD RID: 10701
		public const short LihzahrdStatue = 1152;

		// Token: 0x040029CE RID: 10702
		public const short LihzahrdWatcherStatue = 1153;

		// Token: 0x040029CF RID: 10703
		public const short LihzahrdGuardianStatue = 1154;

		// Token: 0x040029D0 RID: 10704
		public const short WaspGun = 1155;

		// Token: 0x040029D1 RID: 10705
		public const short PiranhaGun = 1156;

		// Token: 0x040029D2 RID: 10706
		public const short PygmyStaff = 1157;

		// Token: 0x040029D3 RID: 10707
		public const short PygmyNecklace = 1158;

		// Token: 0x040029D4 RID: 10708
		public const short TikiMask = 1159;

		// Token: 0x040029D5 RID: 10709
		public const short TikiShirt = 1160;

		// Token: 0x040029D6 RID: 10710
		public const short TikiPants = 1161;

		// Token: 0x040029D7 RID: 10711
		public const short LeafWings = 1162;

		// Token: 0x040029D8 RID: 10712
		public const short BlizzardinaBalloon = 1163;

		// Token: 0x040029D9 RID: 10713
		public const short BundleofBalloons = 1164;

		// Token: 0x040029DA RID: 10714
		public const short BatWings = 1165;

		// Token: 0x040029DB RID: 10715
		public const short BoneSword = 1166;

		// Token: 0x040029DC RID: 10716
		public const short HerculesBeetle = 1167;

		// Token: 0x040029DD RID: 10717
		public const short SmokeBomb = 1168;

		// Token: 0x040029DE RID: 10718
		public const short BoneKey = 1169;

		// Token: 0x040029DF RID: 10719
		public const short Nectar = 1170;

		// Token: 0x040029E0 RID: 10720
		public const short TikiTotem = 1171;

		// Token: 0x040029E1 RID: 10721
		public const short LizardEgg = 1172;

		// Token: 0x040029E2 RID: 10722
		public const short GraveMarker = 1173;

		// Token: 0x040029E3 RID: 10723
		public const short CrossGraveMarker = 1174;

		// Token: 0x040029E4 RID: 10724
		public const short Headstone = 1175;

		// Token: 0x040029E5 RID: 10725
		public const short Gravestone = 1176;

		// Token: 0x040029E6 RID: 10726
		public const short Obelisk = 1177;

		// Token: 0x040029E7 RID: 10727
		public const short LeafBlower = 1178;

		// Token: 0x040029E8 RID: 10728
		public const short ChlorophyteBullet = 1179;

		// Token: 0x040029E9 RID: 10729
		public const short ParrotCracker = 1180;

		// Token: 0x040029EA RID: 10730
		public const short StrangeGlowingMushroom = 1181;

		// Token: 0x040029EB RID: 10731
		public const short Seedling = 1182;

		// Token: 0x040029EC RID: 10732
		public const short WispinaBottle = 1183;

		// Token: 0x040029ED RID: 10733
		public const short PalladiumBar = 1184;

		// Token: 0x040029EE RID: 10734
		public const short PalladiumSword = 1185;

		// Token: 0x040029EF RID: 10735
		public const short PalladiumPike = 1186;

		// Token: 0x040029F0 RID: 10736
		public const short PalladiumRepeater = 1187;

		// Token: 0x040029F1 RID: 10737
		public const short PalladiumPickaxe = 1188;

		// Token: 0x040029F2 RID: 10738
		public const short PalladiumDrill = 1189;

		// Token: 0x040029F3 RID: 10739
		public const short PalladiumChainsaw = 1190;

		// Token: 0x040029F4 RID: 10740
		public const short OrichalcumBar = 1191;

		// Token: 0x040029F5 RID: 10741
		public const short OrichalcumSword = 1192;

		// Token: 0x040029F6 RID: 10742
		public const short OrichalcumHalberd = 1193;

		// Token: 0x040029F7 RID: 10743
		public const short OrichalcumRepeater = 1194;

		// Token: 0x040029F8 RID: 10744
		public const short OrichalcumPickaxe = 1195;

		// Token: 0x040029F9 RID: 10745
		public const short OrichalcumDrill = 1196;

		// Token: 0x040029FA RID: 10746
		public const short OrichalcumChainsaw = 1197;

		// Token: 0x040029FB RID: 10747
		public const short TitaniumBar = 1198;

		// Token: 0x040029FC RID: 10748
		public const short TitaniumSword = 1199;

		// Token: 0x040029FD RID: 10749
		public const short TitaniumTrident = 1200;

		// Token: 0x040029FE RID: 10750
		public const short TitaniumRepeater = 1201;

		// Token: 0x040029FF RID: 10751
		public const short TitaniumPickaxe = 1202;

		// Token: 0x04002A00 RID: 10752
		public const short TitaniumDrill = 1203;

		// Token: 0x04002A01 RID: 10753
		public const short TitaniumChainsaw = 1204;

		// Token: 0x04002A02 RID: 10754
		public const short PalladiumMask = 1205;

		// Token: 0x04002A03 RID: 10755
		public const short PalladiumHelmet = 1206;

		// Token: 0x04002A04 RID: 10756
		public const short PalladiumHeadgear = 1207;

		// Token: 0x04002A05 RID: 10757
		public const short PalladiumBreastplate = 1208;

		// Token: 0x04002A06 RID: 10758
		public const short PalladiumLeggings = 1209;

		// Token: 0x04002A07 RID: 10759
		public const short OrichalcumMask = 1210;

		// Token: 0x04002A08 RID: 10760
		public const short OrichalcumHelmet = 1211;

		// Token: 0x04002A09 RID: 10761
		public const short OrichalcumHeadgear = 1212;

		// Token: 0x04002A0A RID: 10762
		public const short OrichalcumBreastplate = 1213;

		// Token: 0x04002A0B RID: 10763
		public const short OrichalcumLeggings = 1214;

		// Token: 0x04002A0C RID: 10764
		public const short TitaniumMask = 1215;

		// Token: 0x04002A0D RID: 10765
		public const short TitaniumHelmet = 1216;

		// Token: 0x04002A0E RID: 10766
		public const short TitaniumHeadgear = 1217;

		// Token: 0x04002A0F RID: 10767
		public const short TitaniumBreastplate = 1218;

		// Token: 0x04002A10 RID: 10768
		public const short TitaniumLeggings = 1219;

		// Token: 0x04002A11 RID: 10769
		public const short OrichalcumAnvil = 1220;

		// Token: 0x04002A12 RID: 10770
		public const short TitaniumForge = 1221;

		// Token: 0x04002A13 RID: 10771
		public const short PalladiumWaraxe = 1222;

		// Token: 0x04002A14 RID: 10772
		public const short OrichalcumWaraxe = 1223;

		// Token: 0x04002A15 RID: 10773
		public const short TitaniumWaraxe = 1224;

		// Token: 0x04002A16 RID: 10774
		public const short HallowedBar = 1225;

		// Token: 0x04002A17 RID: 10775
		public const short ChlorophyteClaymore = 1226;

		// Token: 0x04002A18 RID: 10776
		public const short ChlorophyteSaber = 1227;

		// Token: 0x04002A19 RID: 10777
		public const short ChlorophytePartisan = 1228;

		// Token: 0x04002A1A RID: 10778
		public const short ChlorophyteShotbow = 1229;

		// Token: 0x04002A1B RID: 10779
		public const short ChlorophytePickaxe = 1230;

		// Token: 0x04002A1C RID: 10780
		public const short ChlorophyteDrill = 1231;

		// Token: 0x04002A1D RID: 10781
		public const short ChlorophyteChainsaw = 1232;

		// Token: 0x04002A1E RID: 10782
		public const short ChlorophyteGreataxe = 1233;

		// Token: 0x04002A1F RID: 10783
		public const short ChlorophyteWarhammer = 1234;

		// Token: 0x04002A20 RID: 10784
		public const short ChlorophyteArrow = 1235;

		// Token: 0x04002A21 RID: 10785
		public const short AmethystHook = 1236;

		// Token: 0x04002A22 RID: 10786
		public const short TopazHook = 1237;

		// Token: 0x04002A23 RID: 10787
		public const short SapphireHook = 1238;

		// Token: 0x04002A24 RID: 10788
		public const short EmeraldHook = 1239;

		// Token: 0x04002A25 RID: 10789
		public const short RubyHook = 1240;

		// Token: 0x04002A26 RID: 10790
		public const short DiamondHook = 1241;

		// Token: 0x04002A27 RID: 10791
		public const short AmberMosquito = 1242;

		// Token: 0x04002A28 RID: 10792
		public const short UmbrellaHat = 1243;

		// Token: 0x04002A29 RID: 10793
		public const short NimbusRod = 1244;

		// Token: 0x04002A2A RID: 10794
		public const short OrangeTorch = 1245;

		// Token: 0x04002A2B RID: 10795
		public const short CrimsandBlock = 1246;

		// Token: 0x04002A2C RID: 10796
		public const short BeeCloak = 1247;

		// Token: 0x04002A2D RID: 10797
		public const short EyeoftheGolem = 1248;

		// Token: 0x04002A2E RID: 10798
		public const short HoneyBalloon = 1249;

		// Token: 0x04002A2F RID: 10799
		public const short BlueHorseshoeBalloon = 1250;

		// Token: 0x04002A30 RID: 10800
		public const short WhiteHorseshoeBalloon = 1251;

		// Token: 0x04002A31 RID: 10801
		public const short YellowHorseshoeBalloon = 1252;

		// Token: 0x04002A32 RID: 10802
		public const short FrozenTurtleShell = 1253;

		// Token: 0x04002A33 RID: 10803
		public const short SniperRifle = 1254;

		// Token: 0x04002A34 RID: 10804
		public const short VenusMagnum = 1255;

		// Token: 0x04002A35 RID: 10805
		public const short CrimsonRod = 1256;

		// Token: 0x04002A36 RID: 10806
		public const short CrimtaneBar = 1257;

		// Token: 0x04002A37 RID: 10807
		public const short Stynger = 1258;

		// Token: 0x04002A38 RID: 10808
		public const short FlowerPow = 1259;

		// Token: 0x04002A39 RID: 10809
		public const short RainbowGun = 1260;

		// Token: 0x04002A3A RID: 10810
		public const short StyngerBolt = 1261;

		// Token: 0x04002A3B RID: 10811
		public const short ChlorophyteJackhammer = 1262;

		// Token: 0x04002A3C RID: 10812
		public const short Teleporter = 1263;

		// Token: 0x04002A3D RID: 10813
		public const short FlowerofFrost = 1264;

		// Token: 0x04002A3E RID: 10814
		public const short Uzi = 1265;

		// Token: 0x04002A3F RID: 10815
		public const short MagnetSphere = 1266;

		// Token: 0x04002A40 RID: 10816
		public const short PurpleStainedGlass = 1267;

		// Token: 0x04002A41 RID: 10817
		public const short YellowStainedGlass = 1268;

		// Token: 0x04002A42 RID: 10818
		public const short BlueStainedGlass = 1269;

		// Token: 0x04002A43 RID: 10819
		public const short GreenStainedGlass = 1270;

		// Token: 0x04002A44 RID: 10820
		public const short RedStainedGlass = 1271;

		// Token: 0x04002A45 RID: 10821
		public const short MulticoloredStainedGlass = 1272;

		// Token: 0x04002A46 RID: 10822
		public const short SkeletronHand = 1273;

		// Token: 0x04002A47 RID: 10823
		public const short Skull = 1274;

		// Token: 0x04002A48 RID: 10824
		public const short BallaHat = 1275;

		// Token: 0x04002A49 RID: 10825
		public const short GangstaHat = 1276;

		// Token: 0x04002A4A RID: 10826
		public const short SailorHat = 1277;

		// Token: 0x04002A4B RID: 10827
		public const short EyePatch = 1278;

		// Token: 0x04002A4C RID: 10828
		public const short SailorShirt = 1279;

		// Token: 0x04002A4D RID: 10829
		public const short SailorPants = 1280;

		// Token: 0x04002A4E RID: 10830
		public const short SkeletronMask = 1281;

		// Token: 0x04002A4F RID: 10831
		public const short AmethystRobe = 1282;

		// Token: 0x04002A50 RID: 10832
		public const short TopazRobe = 1283;

		// Token: 0x04002A51 RID: 10833
		public const short SapphireRobe = 1284;

		// Token: 0x04002A52 RID: 10834
		public const short EmeraldRobe = 1285;

		// Token: 0x04002A53 RID: 10835
		public const short RubyRobe = 1286;

		// Token: 0x04002A54 RID: 10836
		public const short DiamondRobe = 1287;

		// Token: 0x04002A55 RID: 10837
		public const short WhiteTuxedoShirt = 1288;

		// Token: 0x04002A56 RID: 10838
		public const short WhiteTuxedoPants = 1289;

		// Token: 0x04002A57 RID: 10839
		public const short PanicNecklace = 1290;

		// Token: 0x04002A58 RID: 10840
		public const short LifeFruit = 1291;

		// Token: 0x04002A59 RID: 10841
		public const short LihzahrdAltar = 1292;

		// Token: 0x04002A5A RID: 10842
		public const short LihzahrdPowerCell = 1293;

		// Token: 0x04002A5B RID: 10843
		public const short Picksaw = 1294;

		// Token: 0x04002A5C RID: 10844
		public const short HeatRay = 1295;

		// Token: 0x04002A5D RID: 10845
		public const short StaffofEarth = 1296;

		// Token: 0x04002A5E RID: 10846
		public const short GolemFist = 1297;

		// Token: 0x04002A5F RID: 10847
		public const short WaterChest = 1298;

		// Token: 0x04002A60 RID: 10848
		public const short Binoculars = 1299;

		// Token: 0x04002A61 RID: 10849
		public const short RifleScope = 1300;

		// Token: 0x04002A62 RID: 10850
		public const short DestroyerEmblem = 1301;

		// Token: 0x04002A63 RID: 10851
		public const short HighVelocityBullet = 1302;

		// Token: 0x04002A64 RID: 10852
		public const short JellyfishNecklace = 1303;

		// Token: 0x04002A65 RID: 10853
		public const short ZombieArm = 1304;

		// Token: 0x04002A66 RID: 10854
		public const short TheAxe = 1305;

		// Token: 0x04002A67 RID: 10855
		public const short IceSickle = 1306;

		// Token: 0x04002A68 RID: 10856
		public const short ClothierVoodooDoll = 1307;

		// Token: 0x04002A69 RID: 10857
		public const short PoisonStaff = 1308;

		// Token: 0x04002A6A RID: 10858
		public const short SlimeStaff = 1309;

		// Token: 0x04002A6B RID: 10859
		public const short PoisonDart = 1310;

		// Token: 0x04002A6C RID: 10860
		public const short EyeSpring = 1311;

		// Token: 0x04002A6D RID: 10861
		public const short ToySled = 1312;

		// Token: 0x04002A6E RID: 10862
		public const short BookofSkulls = 1313;

		// Token: 0x04002A6F RID: 10863
		public const short KOCannon = 1314;

		// Token: 0x04002A70 RID: 10864
		public const short PirateMap = 1315;

		// Token: 0x04002A71 RID: 10865
		public const short TurtleHelmet = 1316;

		// Token: 0x04002A72 RID: 10866
		public const short TurtleScaleMail = 1317;

		// Token: 0x04002A73 RID: 10867
		public const short TurtleLeggings = 1318;

		// Token: 0x04002A74 RID: 10868
		public const short SnowballCannon = 1319;

		// Token: 0x04002A75 RID: 10869
		public const short BonePickaxe = 1320;

		// Token: 0x04002A76 RID: 10870
		public const short MagicQuiver = 1321;

		// Token: 0x04002A77 RID: 10871
		public const short MagmaStone = 1322;

		// Token: 0x04002A78 RID: 10872
		public const short ObsidianRose = 1323;

		// Token: 0x04002A79 RID: 10873
		public const short Bananarang = 1324;

		// Token: 0x04002A7A RID: 10874
		public const short ChainKnife = 1325;

		// Token: 0x04002A7B RID: 10875
		public const short RodofDiscord = 1326;

		// Token: 0x04002A7C RID: 10876
		public const short DeathSickle = 1327;

		// Token: 0x04002A7D RID: 10877
		public const short TurtleShell = 1328;

		// Token: 0x04002A7E RID: 10878
		public const short TissueSample = 1329;

		// Token: 0x04002A7F RID: 10879
		public const short Vertebrae = 1330;

		// Token: 0x04002A80 RID: 10880
		public const short BloodySpine = 1331;

		// Token: 0x04002A81 RID: 10881
		public const short Ichor = 1332;

		// Token: 0x04002A82 RID: 10882
		public const short IchorTorch = 1333;

		// Token: 0x04002A83 RID: 10883
		public const short IchorArrow = 1334;

		// Token: 0x04002A84 RID: 10884
		public const short IchorBullet = 1335;

		// Token: 0x04002A85 RID: 10885
		public const short GoldenShower = 1336;

		// Token: 0x04002A86 RID: 10886
		public const short BunnyCannon = 1337;

		// Token: 0x04002A87 RID: 10887
		public const short ExplosiveBunny = 1338;

		// Token: 0x04002A88 RID: 10888
		public const short VialofVenom = 1339;

		// Token: 0x04002A89 RID: 10889
		public const short FlaskofVenom = 1340;

		// Token: 0x04002A8A RID: 10890
		public const short VenomArrow = 1341;

		// Token: 0x04002A8B RID: 10891
		public const short VenomBullet = 1342;

		// Token: 0x04002A8C RID: 10892
		public const short FireGauntlet = 1343;

		// Token: 0x04002A8D RID: 10893
		public const short Cog = 1344;

		// Token: 0x04002A8E RID: 10894
		public const short Confetti = 1345;

		// Token: 0x04002A8F RID: 10895
		public const short Nanites = 1346;

		// Token: 0x04002A90 RID: 10896
		public const short ExplosivePowder = 1347;

		// Token: 0x04002A91 RID: 10897
		public const short GoldDust = 1348;

		// Token: 0x04002A92 RID: 10898
		public const short PartyBullet = 1349;

		// Token: 0x04002A93 RID: 10899
		public const short NanoBullet = 1350;

		// Token: 0x04002A94 RID: 10900
		public const short ExplodingBullet = 1351;

		// Token: 0x04002A95 RID: 10901
		public const short GoldenBullet = 1352;

		// Token: 0x04002A96 RID: 10902
		public const short FlaskofCursedFlames = 1353;

		// Token: 0x04002A97 RID: 10903
		public const short FlaskofFire = 1354;

		// Token: 0x04002A98 RID: 10904
		public const short FlaskofGold = 1355;

		// Token: 0x04002A99 RID: 10905
		public const short FlaskofIchor = 1356;

		// Token: 0x04002A9A RID: 10906
		public const short FlaskofNanites = 1357;

		// Token: 0x04002A9B RID: 10907
		public const short FlaskofParty = 1358;

		// Token: 0x04002A9C RID: 10908
		public const short FlaskofPoison = 1359;

		// Token: 0x04002A9D RID: 10909
		public const short EyeofCthulhuTrophy = 1360;

		// Token: 0x04002A9E RID: 10910
		public const short EaterofWorldsTrophy = 1361;

		// Token: 0x04002A9F RID: 10911
		public const short BrainofCthulhuTrophy = 1362;

		// Token: 0x04002AA0 RID: 10912
		public const short SkeletronTrophy = 1363;

		// Token: 0x04002AA1 RID: 10913
		public const short QueenBeeTrophy = 1364;

		// Token: 0x04002AA2 RID: 10914
		public const short WallofFleshTrophy = 1365;

		// Token: 0x04002AA3 RID: 10915
		public const short DestroyerTrophy = 1366;

		// Token: 0x04002AA4 RID: 10916
		public const short SkeletronPrimeTrophy = 1367;

		// Token: 0x04002AA5 RID: 10917
		public const short RetinazerTrophy = 1368;

		// Token: 0x04002AA6 RID: 10918
		public const short SpazmatismTrophy = 1369;

		// Token: 0x04002AA7 RID: 10919
		public const short PlanteraTrophy = 1370;

		// Token: 0x04002AA8 RID: 10920
		public const short GolemTrophy = 1371;

		// Token: 0x04002AA9 RID: 10921
		public const short BloodMoonRising = 1372;

		// Token: 0x04002AAA RID: 10922
		public const short TheHangedMan = 1373;

		// Token: 0x04002AAB RID: 10923
		public const short GloryoftheFire = 1374;

		// Token: 0x04002AAC RID: 10924
		public const short BoneWarp = 1375;

		// Token: 0x04002AAD RID: 10925
		public const short WallSkeleton = 1376;

		// Token: 0x04002AAE RID: 10926
		public const short HangingSkeleton = 1377;

		// Token: 0x04002AAF RID: 10927
		public const short BlueSlabWall = 1378;

		// Token: 0x04002AB0 RID: 10928
		public const short BlueTiledWall = 1379;

		// Token: 0x04002AB1 RID: 10929
		public const short PinkSlabWall = 1380;

		// Token: 0x04002AB2 RID: 10930
		public const short PinkTiledWall = 1381;

		// Token: 0x04002AB3 RID: 10931
		public const short GreenSlabWall = 1382;

		// Token: 0x04002AB4 RID: 10932
		public const short GreenTiledWall = 1383;

		// Token: 0x04002AB5 RID: 10933
		public const short BlueBrickPlatform = 1384;

		// Token: 0x04002AB6 RID: 10934
		public const short PinkBrickPlatform = 1385;

		// Token: 0x04002AB7 RID: 10935
		public const short GreenBrickPlatform = 1386;

		// Token: 0x04002AB8 RID: 10936
		public const short MetalShelf = 1387;

		// Token: 0x04002AB9 RID: 10937
		public const short BrassShelf = 1388;

		// Token: 0x04002ABA RID: 10938
		public const short WoodShelf = 1389;

		// Token: 0x04002ABB RID: 10939
		public const short BrassLantern = 1390;

		// Token: 0x04002ABC RID: 10940
		public const short CagedLantern = 1391;

		// Token: 0x04002ABD RID: 10941
		public const short CarriageLantern = 1392;

		// Token: 0x04002ABE RID: 10942
		public const short AlchemyLantern = 1393;

		// Token: 0x04002ABF RID: 10943
		public const short DiablostLamp = 1394;

		// Token: 0x04002AC0 RID: 10944
		public const short OilRagSconse = 1395;

		// Token: 0x04002AC1 RID: 10945
		public const short BlueDungeonChair = 1396;

		// Token: 0x04002AC2 RID: 10946
		public const short BlueDungeonTable = 1397;

		// Token: 0x04002AC3 RID: 10947
		public const short BlueDungeonWorkBench = 1398;

		// Token: 0x04002AC4 RID: 10948
		public const short GreenDungeonChair = 1399;

		// Token: 0x04002AC5 RID: 10949
		public const short GreenDungeonTable = 1400;

		// Token: 0x04002AC6 RID: 10950
		public const short GreenDungeonWorkBench = 1401;

		// Token: 0x04002AC7 RID: 10951
		public const short PinkDungeonChair = 1402;

		// Token: 0x04002AC8 RID: 10952
		public const short PinkDungeonTable = 1403;

		// Token: 0x04002AC9 RID: 10953
		public const short PinkDungeonWorkBench = 1404;

		// Token: 0x04002ACA RID: 10954
		public const short BlueDungeonCandle = 1405;

		// Token: 0x04002ACB RID: 10955
		public const short GreenDungeonCandle = 1406;

		// Token: 0x04002ACC RID: 10956
		public const short PinkDungeonCandle = 1407;

		// Token: 0x04002ACD RID: 10957
		public const short BlueDungeonVase = 1408;

		// Token: 0x04002ACE RID: 10958
		public const short GreenDungeonVase = 1409;

		// Token: 0x04002ACF RID: 10959
		public const short PinkDungeonVase = 1410;

		// Token: 0x04002AD0 RID: 10960
		public const short BlueDungeonDoor = 1411;

		// Token: 0x04002AD1 RID: 10961
		public const short GreenDungeonDoor = 1412;

		// Token: 0x04002AD2 RID: 10962
		public const short PinkDungeonDoor = 1413;

		// Token: 0x04002AD3 RID: 10963
		public const short BlueDungeonBookcase = 1414;

		// Token: 0x04002AD4 RID: 10964
		public const short GreenDungeonBookcase = 1415;

		// Token: 0x04002AD5 RID: 10965
		public const short PinkDungeonBookcase = 1416;

		// Token: 0x04002AD6 RID: 10966
		public const short Catacomb = 1417;

		// Token: 0x04002AD7 RID: 10967
		public const short DungeonShelf = 1418;

		// Token: 0x04002AD8 RID: 10968
		public const short SkellingtonJSkellingsworth = 1419;

		// Token: 0x04002AD9 RID: 10969
		public const short TheCursedMan = 1420;

		// Token: 0x04002ADA RID: 10970
		public const short TheEyeSeestheEnd = 1421;

		// Token: 0x04002ADB RID: 10971
		public const short SomethingEvilisWatchingYou = 1422;

		// Token: 0x04002ADC RID: 10972
		public const short TheTwinsHaveAwoken = 1423;

		// Token: 0x04002ADD RID: 10973
		public const short TheScreamer = 1424;

		// Token: 0x04002ADE RID: 10974
		public const short GoblinsPlayingPoker = 1425;

		// Token: 0x04002ADF RID: 10975
		public const short Dryadisque = 1426;

		// Token: 0x04002AE0 RID: 10976
		public const short Sunflowers = 1427;

		// Token: 0x04002AE1 RID: 10977
		public const short TerrarianGothic = 1428;

		// Token: 0x04002AE2 RID: 10978
		public const short Beanie = 1429;

		// Token: 0x04002AE3 RID: 10979
		public const short ImbuingStation = 1430;

		// Token: 0x04002AE4 RID: 10980
		public const short StarinaBottle = 1431;

		// Token: 0x04002AE5 RID: 10981
		public const short EmptyBullet = 1432;

		// Token: 0x04002AE6 RID: 10982
		public const short Impact = 1433;

		// Token: 0x04002AE7 RID: 10983
		public const short PoweredbyBirds = 1434;

		// Token: 0x04002AE8 RID: 10984
		public const short TheDestroyer = 1435;

		// Token: 0x04002AE9 RID: 10985
		public const short ThePersistencyofEyes = 1436;

		// Token: 0x04002AEA RID: 10986
		public const short UnicornCrossingtheHallows = 1437;

		// Token: 0x04002AEB RID: 10987
		public const short GreatWave = 1438;

		// Token: 0x04002AEC RID: 10988
		public const short StarryNight = 1439;

		// Token: 0x04002AED RID: 10989
		public const short GuidePicasso = 1440;

		// Token: 0x04002AEE RID: 10990
		public const short TheGuardiansGaze = 1441;

		// Token: 0x04002AEF RID: 10991
		public const short FatherofSomeone = 1442;

		// Token: 0x04002AF0 RID: 10992
		public const short NurseLisa = 1443;

		// Token: 0x04002AF1 RID: 10993
		public const short ShadowbeamStaff = 1444;

		// Token: 0x04002AF2 RID: 10994
		public const short InfernoFork = 1445;

		// Token: 0x04002AF3 RID: 10995
		public const short SpectreStaff = 1446;

		// Token: 0x04002AF4 RID: 10996
		public const short WoodenFence = 1447;

		// Token: 0x04002AF5 RID: 10997
		public const short LeadFence = 1448;

		// Token: 0x04002AF6 RID: 10998
		public const short BubbleMachine = 1449;

		// Token: 0x04002AF7 RID: 10999
		public const short BubbleWand = 1450;

		// Token: 0x04002AF8 RID: 11000
		public const short MarchingBonesBanner = 1451;

		// Token: 0x04002AF9 RID: 11001
		public const short NecromanticSign = 1452;

		// Token: 0x04002AFA RID: 11002
		public const short RustedCompanyStandard = 1453;

		// Token: 0x04002AFB RID: 11003
		public const short RaggedBrotherhoodSigil = 1454;

		// Token: 0x04002AFC RID: 11004
		public const short MoltenLegionFlag = 1455;

		// Token: 0x04002AFD RID: 11005
		public const short DiabolicSigil = 1456;

		// Token: 0x04002AFE RID: 11006
		public const short ObsidianPlatform = 1457;

		// Token: 0x04002AFF RID: 11007
		public const short ObsidianDoor = 1458;

		// Token: 0x04002B00 RID: 11008
		public const short ObsidianChair = 1459;

		// Token: 0x04002B01 RID: 11009
		public const short ObsidianTable = 1460;

		// Token: 0x04002B02 RID: 11010
		public const short ObsidianWorkBench = 1461;

		// Token: 0x04002B03 RID: 11011
		public const short ObsidianVase = 1462;

		// Token: 0x04002B04 RID: 11012
		public const short ObsidianBookcase = 1463;

		// Token: 0x04002B05 RID: 11013
		public const short HellboundBanner = 1464;

		// Token: 0x04002B06 RID: 11014
		public const short HellHammerBanner = 1465;

		// Token: 0x04002B07 RID: 11015
		public const short HelltowerBanner = 1466;

		// Token: 0x04002B08 RID: 11016
		public const short LostHopesofManBanner = 1467;

		// Token: 0x04002B09 RID: 11017
		public const short ObsidianWatcherBanner = 1468;

		// Token: 0x04002B0A RID: 11018
		public const short LavaEruptsBanner = 1469;

		// Token: 0x04002B0B RID: 11019
		public const short BlueDungeonBed = 1470;

		// Token: 0x04002B0C RID: 11020
		public const short GreenDungeonBed = 1471;

		// Token: 0x04002B0D RID: 11021
		public const short PinkDungeonBed = 1472;

		// Token: 0x04002B0E RID: 11022
		public const short ObsidianBed = 1473;

		// Token: 0x04002B0F RID: 11023
		public const short Waldo = 1474;

		// Token: 0x04002B10 RID: 11024
		public const short Darkness = 1475;

		// Token: 0x04002B11 RID: 11025
		public const short DarkSoulReaper = 1476;

		// Token: 0x04002B12 RID: 11026
		public const short Land = 1477;

		// Token: 0x04002B13 RID: 11027
		public const short TrappedGhost = 1478;

		// Token: 0x04002B14 RID: 11028
		public const short DemonsEye = 1479;

		// Token: 0x04002B15 RID: 11029
		public const short FindingGold = 1480;

		// Token: 0x04002B16 RID: 11030
		public const short FirstEncounter = 1481;

		// Token: 0x04002B17 RID: 11031
		public const short GoodMorning = 1482;

		// Token: 0x04002B18 RID: 11032
		public const short UndergroundReward = 1483;

		// Token: 0x04002B19 RID: 11033
		public const short ThroughtheWindow = 1484;

		// Token: 0x04002B1A RID: 11034
		public const short PlaceAbovetheClouds = 1485;

		// Token: 0x04002B1B RID: 11035
		public const short DoNotStepontheGrass = 1486;

		// Token: 0x04002B1C RID: 11036
		public const short ColdWatersintheWhiteLand = 1487;

		// Token: 0x04002B1D RID: 11037
		public const short LightlessChasms = 1488;

		// Token: 0x04002B1E RID: 11038
		public const short TheLandofDeceivingLooks = 1489;

		// Token: 0x04002B1F RID: 11039
		public const short Daylight = 1490;

		// Token: 0x04002B20 RID: 11040
		public const short SecretoftheSands = 1491;

		// Token: 0x04002B21 RID: 11041
		public const short DeadlandComesAlive = 1492;

		// Token: 0x04002B22 RID: 11042
		public const short EvilPresence = 1493;

		// Token: 0x04002B23 RID: 11043
		public const short SkyGuardian = 1494;

		// Token: 0x04002B24 RID: 11044
		public const short AmericanExplosive = 1495;

		// Token: 0x04002B25 RID: 11045
		public const short Discover = 1496;

		// Token: 0x04002B26 RID: 11046
		public const short HandEarth = 1497;

		// Token: 0x04002B27 RID: 11047
		public const short OldMiner = 1498;

		// Token: 0x04002B28 RID: 11048
		public const short Skelehead = 1499;

		// Token: 0x04002B29 RID: 11049
		public const short FacingtheCerebralMastermind = 1500;

		// Token: 0x04002B2A RID: 11050
		public const short LakeofFire = 1501;

		// Token: 0x04002B2B RID: 11051
		public const short TrioSuperHeroes = 1502;

		// Token: 0x04002B2C RID: 11052
		public const short SpectreHood = 1503;

		// Token: 0x04002B2D RID: 11053
		public const short SpectreRobe = 1504;

		// Token: 0x04002B2E RID: 11054
		public const short SpectrePants = 1505;

		// Token: 0x04002B2F RID: 11055
		public const short SpectrePickaxe = 1506;

		// Token: 0x04002B30 RID: 11056
		public const short SpectreHamaxe = 1507;

		// Token: 0x04002B31 RID: 11057
		public const short Ectoplasm = 1508;

		// Token: 0x04002B32 RID: 11058
		public const short GothicChair = 1509;

		// Token: 0x04002B33 RID: 11059
		public const short GothicTable = 1510;

		// Token: 0x04002B34 RID: 11060
		public const short GothicWorkBench = 1511;

		// Token: 0x04002B35 RID: 11061
		public const short GothicBookcase = 1512;

		// Token: 0x04002B36 RID: 11062
		public const short PaladinsHammer = 1513;

		// Token: 0x04002B37 RID: 11063
		public const short SWATHelmet = 1514;

		// Token: 0x04002B38 RID: 11064
		public const short BeeWings = 1515;

		// Token: 0x04002B39 RID: 11065
		public const short GiantHarpyFeather = 1516;

		// Token: 0x04002B3A RID: 11066
		public const short BoneFeather = 1517;

		// Token: 0x04002B3B RID: 11067
		public const short FireFeather = 1518;

		// Token: 0x04002B3C RID: 11068
		public const short IceFeather = 1519;

		// Token: 0x04002B3D RID: 11069
		public const short BrokenBatWing = 1520;

		// Token: 0x04002B3E RID: 11070
		public const short TatteredBeeWing = 1521;

		// Token: 0x04002B3F RID: 11071
		public const short LargeAmethyst = 1522;

		// Token: 0x04002B40 RID: 11072
		public const short LargeTopaz = 1523;

		// Token: 0x04002B41 RID: 11073
		public const short LargeSapphire = 1524;

		// Token: 0x04002B42 RID: 11074
		public const short LargeEmerald = 1525;

		// Token: 0x04002B43 RID: 11075
		public const short LargeRuby = 1526;

		// Token: 0x04002B44 RID: 11076
		public const short LargeDiamond = 1527;

		// Token: 0x04002B45 RID: 11077
		public const short JungleChest = 1528;

		// Token: 0x04002B46 RID: 11078
		public const short CorruptionChest = 1529;

		// Token: 0x04002B47 RID: 11079
		public const short CrimsonChest = 1530;

		// Token: 0x04002B48 RID: 11080
		public const short HallowedChest = 1531;

		// Token: 0x04002B49 RID: 11081
		public const short FrozenChest = 1532;

		// Token: 0x04002B4A RID: 11082
		public const short JungleKey = 1533;

		// Token: 0x04002B4B RID: 11083
		public const short CorruptionKey = 1534;

		// Token: 0x04002B4C RID: 11084
		public const short CrimsonKey = 1535;

		// Token: 0x04002B4D RID: 11085
		public const short HallowedKey = 1536;

		// Token: 0x04002B4E RID: 11086
		public const short FrozenKey = 1537;

		// Token: 0x04002B4F RID: 11087
		public const short ImpFace = 1538;

		// Token: 0x04002B50 RID: 11088
		public const short OminousPresence = 1539;

		// Token: 0x04002B51 RID: 11089
		public const short ShiningMoon = 1540;

		// Token: 0x04002B52 RID: 11090
		public const short LivingGore = 1541;

		// Token: 0x04002B53 RID: 11091
		public const short FlowingMagma = 1542;

		// Token: 0x04002B54 RID: 11092
		public const short SpectrePaintbrush = 1543;

		// Token: 0x04002B55 RID: 11093
		public const short SpectrePaintRoller = 1544;

		// Token: 0x04002B56 RID: 11094
		public const short SpectrePaintScraper = 1545;

		// Token: 0x04002B57 RID: 11095
		public const short ShroomiteHeadgear = 1546;

		// Token: 0x04002B58 RID: 11096
		public const short ShroomiteMask = 1547;

		// Token: 0x04002B59 RID: 11097
		public const short ShroomiteHelmet = 1548;

		// Token: 0x04002B5A RID: 11098
		public const short ShroomiteBreastplate = 1549;

		// Token: 0x04002B5B RID: 11099
		public const short ShroomiteLeggings = 1550;

		// Token: 0x04002B5C RID: 11100
		public const short Autohammer = 1551;

		// Token: 0x04002B5D RID: 11101
		public const short ShroomiteBar = 1552;

		// Token: 0x04002B5E RID: 11102
		public const short SDMG = 1553;

		// Token: 0x04002B5F RID: 11103
		public const short CenxsTiara = 1554;

		// Token: 0x04002B60 RID: 11104
		public const short CenxsBreastplate = 1555;

		// Token: 0x04002B61 RID: 11105
		public const short CenxsLeggings = 1556;

		// Token: 0x04002B62 RID: 11106
		public const short CrownosMask = 1557;

		// Token: 0x04002B63 RID: 11107
		public const short CrownosBreastplate = 1558;

		// Token: 0x04002B64 RID: 11108
		public const short CrownosLeggings = 1559;

		// Token: 0x04002B65 RID: 11109
		public const short WillsHelmet = 1560;

		// Token: 0x04002B66 RID: 11110
		public const short WillsBreastplate = 1561;

		// Token: 0x04002B67 RID: 11111
		public const short WillsLeggings = 1562;

		// Token: 0x04002B68 RID: 11112
		public const short JimsHelmet = 1563;

		// Token: 0x04002B69 RID: 11113
		public const short JimsBreastplate = 1564;

		// Token: 0x04002B6A RID: 11114
		public const short JimsLeggings = 1565;

		// Token: 0x04002B6B RID: 11115
		public const short AaronsHelmet = 1566;

		// Token: 0x04002B6C RID: 11116
		public const short AaronsBreastplate = 1567;

		// Token: 0x04002B6D RID: 11117
		public const short AaronsLeggings = 1568;

		// Token: 0x04002B6E RID: 11118
		public const short VampireKnives = 1569;

		// Token: 0x04002B6F RID: 11119
		public const short BrokenHeroSword = 1570;

		// Token: 0x04002B70 RID: 11120
		public const short ScourgeoftheCorruptor = 1571;

		// Token: 0x04002B71 RID: 11121
		public const short StaffoftheFrostHydra = 1572;

		// Token: 0x04002B72 RID: 11122
		public const short TheCreationoftheGuide = 1573;

		// Token: 0x04002B73 RID: 11123
		public const short TheMerchant = 1574;

		// Token: 0x04002B74 RID: 11124
		public const short CrownoDevoursHisLunch = 1575;

		// Token: 0x04002B75 RID: 11125
		public const short RareEnchantment = 1576;

		// Token: 0x04002B76 RID: 11126
		public const short GloriousNight = 1577;

		// Token: 0x04002B77 RID: 11127
		public const short SweetheartNecklace = 1578;

		// Token: 0x04002B78 RID: 11128
		public const short FlurryBoots = 1579;

		// Token: 0x04002B79 RID: 11129
		public const short DTownsHelmet = 1580;

		// Token: 0x04002B7A RID: 11130
		public const short DTownsBreastplate = 1581;

		// Token: 0x04002B7B RID: 11131
		public const short DTownsLeggings = 1582;

		// Token: 0x04002B7C RID: 11132
		public const short DTownsWings = 1583;

		// Token: 0x04002B7D RID: 11133
		public const short WillsWings = 1584;

		// Token: 0x04002B7E RID: 11134
		public const short CrownosWings = 1585;

		// Token: 0x04002B7F RID: 11135
		public const short CenxsWings = 1586;

		// Token: 0x04002B80 RID: 11136
		public const short CenxsDress = 1587;

		// Token: 0x04002B81 RID: 11137
		public const short CenxsDressPants = 1588;

		// Token: 0x04002B82 RID: 11138
		public const short PalladiumColumn = 1589;

		// Token: 0x04002B83 RID: 11139
		public const short PalladiumColumnWall = 1590;

		// Token: 0x04002B84 RID: 11140
		public const short BubblegumBlock = 1591;

		// Token: 0x04002B85 RID: 11141
		public const short BubblegumBlockWall = 1592;

		// Token: 0x04002B86 RID: 11142
		public const short TitanstoneBlock = 1593;

		// Token: 0x04002B87 RID: 11143
		public const short TitanstoneBlockWall = 1594;

		// Token: 0x04002B88 RID: 11144
		public const short MagicCuffs = 1595;

		// Token: 0x04002B89 RID: 11145
		public const short MusicBoxSnow = 1596;

		// Token: 0x04002B8A RID: 11146
		public const short MusicBoxSpace = 1597;

		// Token: 0x04002B8B RID: 11147
		public const short MusicBoxCrimson = 1598;

		// Token: 0x04002B8C RID: 11148
		public const short MusicBoxBoss4 = 1599;

		// Token: 0x04002B8D RID: 11149
		public const short MusicBoxAltOverworldDay = 1600;

		// Token: 0x04002B8E RID: 11150
		public const short MusicBoxRain = 1601;

		// Token: 0x04002B8F RID: 11151
		public const short MusicBoxIce = 1602;

		// Token: 0x04002B90 RID: 11152
		public const short MusicBoxDesert = 1603;

		// Token: 0x04002B91 RID: 11153
		public const short MusicBoxOcean = 1604;

		// Token: 0x04002B92 RID: 11154
		public const short MusicBoxDungeon = 1605;

		// Token: 0x04002B93 RID: 11155
		public const short MusicBoxPlantera = 1606;

		// Token: 0x04002B94 RID: 11156
		public const short MusicBoxBoss5 = 1607;

		// Token: 0x04002B95 RID: 11157
		public const short MusicBoxTemple = 1608;

		// Token: 0x04002B96 RID: 11158
		public const short MusicBoxEclipse = 1609;

		// Token: 0x04002B97 RID: 11159
		public const short MusicBoxMushrooms = 1610;

		// Token: 0x04002B98 RID: 11160
		public const short ButterflyDust = 1611;

		// Token: 0x04002B99 RID: 11161
		public const short AnkhCharm = 1612;

		// Token: 0x04002B9A RID: 11162
		public const short AnkhShield = 1613;

		// Token: 0x04002B9B RID: 11163
		public const short BlueFlare = 1614;

		// Token: 0x04002B9C RID: 11164
		public const short AnglerFishBanner = 1615;

		// Token: 0x04002B9D RID: 11165
		public const short AngryNimbusBanner = 1616;

		// Token: 0x04002B9E RID: 11166
		public const short AnomuraFungusBanner = 1617;

		// Token: 0x04002B9F RID: 11167
		public const short AntlionBanner = 1618;

		// Token: 0x04002BA0 RID: 11168
		public const short ArapaimaBanner = 1619;

		// Token: 0x04002BA1 RID: 11169
		public const short ArmoredSkeletonBanner = 1620;

		// Token: 0x04002BA2 RID: 11170
		public const short BatBanner = 1621;

		// Token: 0x04002BA3 RID: 11171
		public const short BirdBanner = 1622;

		// Token: 0x04002BA4 RID: 11172
		public const short BlackRecluseBanner = 1623;

		// Token: 0x04002BA5 RID: 11173
		public const short BloodFeederBanner = 1624;

		// Token: 0x04002BA6 RID: 11174
		public const short BloodJellyBanner = 1625;

		// Token: 0x04002BA7 RID: 11175
		public const short BloodCrawlerBanner = 1626;

		// Token: 0x04002BA8 RID: 11176
		public const short BoneSerpentBanner = 1627;

		// Token: 0x04002BA9 RID: 11177
		public const short BunnyBanner = 1628;

		// Token: 0x04002BAA RID: 11178
		public const short ChaosElementalBanner = 1629;

		// Token: 0x04002BAB RID: 11179
		public const short MimicBanner = 1630;

		// Token: 0x04002BAC RID: 11180
		public const short ClownBanner = 1631;

		// Token: 0x04002BAD RID: 11181
		public const short CorruptBunnyBanner = 1632;

		// Token: 0x04002BAE RID: 11182
		public const short CorruptGoldfishBanner = 1633;

		// Token: 0x04002BAF RID: 11183
		public const short CrabBanner = 1634;

		// Token: 0x04002BB0 RID: 11184
		public const short CrimeraBanner = 1635;

		// Token: 0x04002BB1 RID: 11185
		public const short CrimsonAxeBanner = 1636;

		// Token: 0x04002BB2 RID: 11186
		public const short CursedHammerBanner = 1637;

		// Token: 0x04002BB3 RID: 11187
		public const short DemonBanner = 1638;

		// Token: 0x04002BB4 RID: 11188
		public const short DemonEyeBanner = 1639;

		// Token: 0x04002BB5 RID: 11189
		public const short DerplingBanner = 1640;

		// Token: 0x04002BB6 RID: 11190
		public const short EaterofSoulsBanner = 1641;

		// Token: 0x04002BB7 RID: 11191
		public const short EnchantedSwordBanner = 1642;

		// Token: 0x04002BB8 RID: 11192
		public const short ZombieEskimoBanner = 1643;

		// Token: 0x04002BB9 RID: 11193
		public const short FaceMonsterBanner = 1644;

		// Token: 0x04002BBA RID: 11194
		public const short FloatyGrossBanner = 1645;

		// Token: 0x04002BBB RID: 11195
		public const short FlyingFishBanner = 1646;

		// Token: 0x04002BBC RID: 11196
		public const short FlyingSnakeBanner = 1647;

		// Token: 0x04002BBD RID: 11197
		public const short FrankensteinBanner = 1648;

		// Token: 0x04002BBE RID: 11198
		public const short FungiBulbBanner = 1649;

		// Token: 0x04002BBF RID: 11199
		public const short FungoFishBanner = 1650;

		// Token: 0x04002BC0 RID: 11200
		public const short GastropodBanner = 1651;

		// Token: 0x04002BC1 RID: 11201
		public const short GoblinThiefBanner = 1652;

		// Token: 0x04002BC2 RID: 11202
		public const short GoblinSorcererBanner = 1653;

		// Token: 0x04002BC3 RID: 11203
		public const short GoblinPeonBanner = 1654;

		// Token: 0x04002BC4 RID: 11204
		public const short GoblinScoutBanner = 1655;

		// Token: 0x04002BC5 RID: 11205
		public const short GoblinWarriorBanner = 1656;

		// Token: 0x04002BC6 RID: 11206
		public const short GoldfishBanner = 1657;

		// Token: 0x04002BC7 RID: 11207
		public const short HarpyBanner = 1658;

		// Token: 0x04002BC8 RID: 11208
		public const short HellbatBanner = 1659;

		// Token: 0x04002BC9 RID: 11209
		public const short HerplingBanner = 1660;

		// Token: 0x04002BCA RID: 11210
		public const short HornetBanner = 1661;

		// Token: 0x04002BCB RID: 11211
		public const short IceElementalBanner = 1662;

		// Token: 0x04002BCC RID: 11212
		public const short IcyMermanBanner = 1663;

		// Token: 0x04002BCD RID: 11213
		public const short FireImpBanner = 1664;

		// Token: 0x04002BCE RID: 11214
		public const short JellyfishBanner = 1665;

		// Token: 0x04002BCF RID: 11215
		public const short JungleCreeperBanner = 1666;

		// Token: 0x04002BD0 RID: 11216
		public const short LihzahrdBanner = 1667;

		// Token: 0x04002BD1 RID: 11217
		public const short ManEaterBanner = 1668;

		// Token: 0x04002BD2 RID: 11218
		public const short MeteorHeadBanner = 1669;

		// Token: 0x04002BD3 RID: 11219
		public const short MothBanner = 1670;

		// Token: 0x04002BD4 RID: 11220
		public const short MummyBanner = 1671;

		// Token: 0x04002BD5 RID: 11221
		public const short MushiLadybugBanner = 1672;

		// Token: 0x04002BD6 RID: 11222
		public const short ParrotBanner = 1673;

		// Token: 0x04002BD7 RID: 11223
		public const short PigronBanner = 1674;

		// Token: 0x04002BD8 RID: 11224
		public const short PiranhaBanner = 1675;

		// Token: 0x04002BD9 RID: 11225
		public const short PirateBanner = 1676;

		// Token: 0x04002BDA RID: 11226
		public const short PixieBanner = 1677;

		// Token: 0x04002BDB RID: 11227
		public const short RaincoatZombieBanner = 1678;

		// Token: 0x04002BDC RID: 11228
		public const short ReaperBanner = 1679;

		// Token: 0x04002BDD RID: 11229
		public const short SharkBanner = 1680;

		// Token: 0x04002BDE RID: 11230
		public const short SkeletonBanner = 1681;

		// Token: 0x04002BDF RID: 11231
		public const short SkeletonMageBanner = 1682;

		// Token: 0x04002BE0 RID: 11232
		public const short SlimeBanner = 1683;

		// Token: 0x04002BE1 RID: 11233
		public const short SnowFlinxBanner = 1684;

		// Token: 0x04002BE2 RID: 11234
		public const short SpiderBanner = 1685;

		// Token: 0x04002BE3 RID: 11235
		public const short SporeZombieBanner = 1686;

		// Token: 0x04002BE4 RID: 11236
		public const short SwampThingBanner = 1687;

		// Token: 0x04002BE5 RID: 11237
		public const short TortoiseBanner = 1688;

		// Token: 0x04002BE6 RID: 11238
		public const short ToxicSludgeBanner = 1689;

		// Token: 0x04002BE7 RID: 11239
		public const short UmbrellaSlimeBanner = 1690;

		// Token: 0x04002BE8 RID: 11240
		public const short UnicornBanner = 1691;

		// Token: 0x04002BE9 RID: 11241
		public const short VampireBanner = 1692;

		// Token: 0x04002BEA RID: 11242
		public const short VultureBanner = 1693;

		// Token: 0x04002BEB RID: 11243
		public const short NypmhBanner = 1694;

		// Token: 0x04002BEC RID: 11244
		public const short WerewolfBanner = 1695;

		// Token: 0x04002BED RID: 11245
		public const short WolfBanner = 1696;

		// Token: 0x04002BEE RID: 11246
		public const short WorldFeederBanner = 1697;

		// Token: 0x04002BEF RID: 11247
		public const short WormBanner = 1698;

		// Token: 0x04002BF0 RID: 11248
		public const short WraithBanner = 1699;

		// Token: 0x04002BF1 RID: 11249
		public const short WyvernBanner = 1700;

		// Token: 0x04002BF2 RID: 11250
		public const short ZombieBanner = 1701;

		// Token: 0x04002BF3 RID: 11251
		public const short GlassPlatform = 1702;

		// Token: 0x04002BF4 RID: 11252
		public const short GlassChair = 1703;

		// Token: 0x04002BF5 RID: 11253
		public const short GoldenChair = 1704;

		// Token: 0x04002BF6 RID: 11254
		public const short GoldenToilet = 1705;

		// Token: 0x04002BF7 RID: 11255
		public const short BarStool = 1706;

		// Token: 0x04002BF8 RID: 11256
		public const short HoneyChair = 1707;

		// Token: 0x04002BF9 RID: 11257
		public const short SteampunkChair = 1708;

		// Token: 0x04002BFA RID: 11258
		public const short GlassDoor = 1709;

		// Token: 0x04002BFB RID: 11259
		public const short GoldenDoor = 1710;

		// Token: 0x04002BFC RID: 11260
		public const short HoneyDoor = 1711;

		// Token: 0x04002BFD RID: 11261
		public const short SteampunkDoor = 1712;

		// Token: 0x04002BFE RID: 11262
		public const short GlassTable = 1713;

		// Token: 0x04002BFF RID: 11263
		public const short BanquetTable = 1714;

		// Token: 0x04002C00 RID: 11264
		public const short Bar = 1715;

		// Token: 0x04002C01 RID: 11265
		public const short GoldenTable = 1716;

		// Token: 0x04002C02 RID: 11266
		public const short HoneyTable = 1717;

		// Token: 0x04002C03 RID: 11267
		public const short SteampunkTable = 1718;

		// Token: 0x04002C04 RID: 11268
		public const short GlassBed = 1719;

		// Token: 0x04002C05 RID: 11269
		public const short GoldenBed = 1720;

		// Token: 0x04002C06 RID: 11270
		public const short HoneyBed = 1721;

		// Token: 0x04002C07 RID: 11271
		public const short SteampunkBed = 1722;

		// Token: 0x04002C08 RID: 11272
		public const short LivingWoodWall = 1723;

		// Token: 0x04002C09 RID: 11273
		public const short FartinaJar = 1724;

		// Token: 0x04002C0A RID: 11274
		public const short Pumpkin = 1725;

		// Token: 0x04002C0B RID: 11275
		public const short PumpkinWall = 1726;

		// Token: 0x04002C0C RID: 11276
		public const short Hay = 1727;

		// Token: 0x04002C0D RID: 11277
		public const short HayWall = 1728;

		// Token: 0x04002C0E RID: 11278
		public const short SpookyWood = 1729;

		// Token: 0x04002C0F RID: 11279
		public const short SpookyWoodWall = 1730;

		// Token: 0x04002C10 RID: 11280
		public const short PumpkinHelmet = 1731;

		// Token: 0x04002C11 RID: 11281
		public const short PumpkinBreastplate = 1732;

		// Token: 0x04002C12 RID: 11282
		public const short PumpkinLeggings = 1733;

		// Token: 0x04002C13 RID: 11283
		public const short CandyApple = 1734;

		// Token: 0x04002C14 RID: 11284
		public const short SoulCake = 1735;

		// Token: 0x04002C15 RID: 11285
		public const short NurseHat = 1736;

		// Token: 0x04002C16 RID: 11286
		public const short NurseShirt = 1737;

		// Token: 0x04002C17 RID: 11287
		public const short NursePants = 1738;

		// Token: 0x04002C18 RID: 11288
		public const short WizardsHat = 1739;

		// Token: 0x04002C19 RID: 11289
		public const short GuyFawkesMask = 1740;

		// Token: 0x04002C1A RID: 11290
		public const short DyeTraderRobe = 1741;

		// Token: 0x04002C1B RID: 11291
		public const short SteampunkGoggles = 1742;

		// Token: 0x04002C1C RID: 11292
		public const short CyborgHelmet = 1743;

		// Token: 0x04002C1D RID: 11293
		public const short CyborgShirt = 1744;

		// Token: 0x04002C1E RID: 11294
		public const short CyborgPants = 1745;

		// Token: 0x04002C1F RID: 11295
		public const short CreeperMask = 1746;

		// Token: 0x04002C20 RID: 11296
		public const short CreeperShirt = 1747;

		// Token: 0x04002C21 RID: 11297
		public const short CreeperPants = 1748;

		// Token: 0x04002C22 RID: 11298
		public const short CatMask = 1749;

		// Token: 0x04002C23 RID: 11299
		public const short CatShirt = 1750;

		// Token: 0x04002C24 RID: 11300
		public const short CatPants = 1751;

		// Token: 0x04002C25 RID: 11301
		public const short GhostMask = 1752;

		// Token: 0x04002C26 RID: 11302
		public const short GhostShirt = 1753;

		// Token: 0x04002C27 RID: 11303
		public const short PumpkinMask = 1754;

		// Token: 0x04002C28 RID: 11304
		public const short PumpkinShirt = 1755;

		// Token: 0x04002C29 RID: 11305
		public const short PumpkinPants = 1756;

		// Token: 0x04002C2A RID: 11306
		public const short RobotMask = 1757;

		// Token: 0x04002C2B RID: 11307
		public const short RobotShirt = 1758;

		// Token: 0x04002C2C RID: 11308
		public const short RobotPants = 1759;

		// Token: 0x04002C2D RID: 11309
		public const short UnicornMask = 1760;

		// Token: 0x04002C2E RID: 11310
		public const short UnicornShirt = 1761;

		// Token: 0x04002C2F RID: 11311
		public const short UnicornPants = 1762;

		// Token: 0x04002C30 RID: 11312
		public const short VampireMask = 1763;

		// Token: 0x04002C31 RID: 11313
		public const short VampireShirt = 1764;

		// Token: 0x04002C32 RID: 11314
		public const short VampirePants = 1765;

		// Token: 0x04002C33 RID: 11315
		public const short WitchHat = 1766;

		// Token: 0x04002C34 RID: 11316
		public const short LeprechaunHat = 1767;

		// Token: 0x04002C35 RID: 11317
		public const short LeprechaunShirt = 1768;

		// Token: 0x04002C36 RID: 11318
		public const short LeprechaunPants = 1769;

		// Token: 0x04002C37 RID: 11319
		public const short PixieShirt = 1770;

		// Token: 0x04002C38 RID: 11320
		public const short PixiePants = 1771;

		// Token: 0x04002C39 RID: 11321
		public const short PrincessHat = 1772;

		// Token: 0x04002C3A RID: 11322
		public const short PrincessDressNew = 1773;

		// Token: 0x04002C3B RID: 11323
		public const short GoodieBag = 1774;

		// Token: 0x04002C3C RID: 11324
		public const short WitchDress = 1775;

		// Token: 0x04002C3D RID: 11325
		public const short WitchBoots = 1776;

		// Token: 0x04002C3E RID: 11326
		public const short BrideofFrankensteinMask = 1777;

		// Token: 0x04002C3F RID: 11327
		public const short BrideofFrankensteinDress = 1778;

		// Token: 0x04002C40 RID: 11328
		public const short KarateTortoiseMask = 1779;

		// Token: 0x04002C41 RID: 11329
		public const short KarateTortoiseShirt = 1780;

		// Token: 0x04002C42 RID: 11330
		public const short KarateTortoisePants = 1781;

		// Token: 0x04002C43 RID: 11331
		public const short CandyCornRifle = 1782;

		// Token: 0x04002C44 RID: 11332
		public const short CandyCorn = 1783;

		// Token: 0x04002C45 RID: 11333
		public const short JackOLanternLauncher = 1784;

		// Token: 0x04002C46 RID: 11334
		public const short ExplosiveJackOLantern = 1785;

		// Token: 0x04002C47 RID: 11335
		public const short Sickle = 1786;

		// Token: 0x04002C48 RID: 11336
		public const short PumpkinPie = 1787;

		// Token: 0x04002C49 RID: 11337
		public const short ScarecrowHat = 1788;

		// Token: 0x04002C4A RID: 11338
		public const short ScarecrowShirt = 1789;

		// Token: 0x04002C4B RID: 11339
		public const short ScarecrowPants = 1790;

		// Token: 0x04002C4C RID: 11340
		public const short Cauldron = 1791;

		// Token: 0x04002C4D RID: 11341
		public const short PumpkinChair = 1792;

		// Token: 0x04002C4E RID: 11342
		public const short PumpkinDoor = 1793;

		// Token: 0x04002C4F RID: 11343
		public const short PumpkinTable = 1794;

		// Token: 0x04002C50 RID: 11344
		public const short PumpkinWorkBench = 1795;

		// Token: 0x04002C51 RID: 11345
		public const short PumpkinPlatform = 1796;

		// Token: 0x04002C52 RID: 11346
		public const short TatteredFairyWings = 1797;

		// Token: 0x04002C53 RID: 11347
		public const short SpiderEgg = 1798;

		// Token: 0x04002C54 RID: 11348
		public const short MagicalPumpkinSeed = 1799;

		// Token: 0x04002C55 RID: 11349
		public const short BatHook = 1800;

		// Token: 0x04002C56 RID: 11350
		public const short BatScepter = 1801;

		// Token: 0x04002C57 RID: 11351
		public const short RavenStaff = 1802;

		// Token: 0x04002C58 RID: 11352
		public const short JungleKeyMold = 1803;

		// Token: 0x04002C59 RID: 11353
		public const short CorruptionKeyMold = 1804;

		// Token: 0x04002C5A RID: 11354
		public const short CrimsonKeyMold = 1805;

		// Token: 0x04002C5B RID: 11355
		public const short HallowedKeyMold = 1806;

		// Token: 0x04002C5C RID: 11356
		public const short FrozenKeyMold = 1807;

		// Token: 0x04002C5D RID: 11357
		public const short HangingJackOLantern = 1808;

		// Token: 0x04002C5E RID: 11358
		public const short RottenEgg = 1809;

		// Token: 0x04002C5F RID: 11359
		public const short UnluckyYarn = 1810;

		// Token: 0x04002C60 RID: 11360
		public const short BlackFairyDust = 1811;

		// Token: 0x04002C61 RID: 11361
		public const short Jackelier = 1812;

		// Token: 0x04002C62 RID: 11362
		public const short JackOLantern = 1813;

		// Token: 0x04002C63 RID: 11363
		public const short SpookyChair = 1814;

		// Token: 0x04002C64 RID: 11364
		public const short SpookyDoor = 1815;

		// Token: 0x04002C65 RID: 11365
		public const short SpookyTable = 1816;

		// Token: 0x04002C66 RID: 11366
		public const short SpookyWorkBench = 1817;

		// Token: 0x04002C67 RID: 11367
		public const short SpookyPlatform = 1818;

		// Token: 0x04002C68 RID: 11368
		public const short ReaperHood = 1819;

		// Token: 0x04002C69 RID: 11369
		public const short ReaperRobe = 1820;

		// Token: 0x04002C6A RID: 11370
		public const short FoxMask = 1821;

		// Token: 0x04002C6B RID: 11371
		public const short FoxShirt = 1822;

		// Token: 0x04002C6C RID: 11372
		public const short FoxPants = 1823;

		// Token: 0x04002C6D RID: 11373
		public const short CatEars = 1824;

		// Token: 0x04002C6E RID: 11374
		public const short BloodyMachete = 1825;

		// Token: 0x04002C6F RID: 11375
		public const short TheHorsemansBlade = 1826;

		// Token: 0x04002C70 RID: 11376
		public const short BladedGlove = 1827;

		// Token: 0x04002C71 RID: 11377
		public const short PumpkinSeed = 1828;

		// Token: 0x04002C72 RID: 11378
		public const short SpookyHook = 1829;

		// Token: 0x04002C73 RID: 11379
		public const short SpookyWings = 1830;

		// Token: 0x04002C74 RID: 11380
		public const short SpookyTwig = 1831;

		// Token: 0x04002C75 RID: 11381
		public const short SpookyHelmet = 1832;

		// Token: 0x04002C76 RID: 11382
		public const short SpookyBreastplate = 1833;

		// Token: 0x04002C77 RID: 11383
		public const short SpookyLeggings = 1834;

		// Token: 0x04002C78 RID: 11384
		public const short StakeLauncher = 1835;

		// Token: 0x04002C79 RID: 11385
		public const short Stake = 1836;

		// Token: 0x04002C7A RID: 11386
		public const short CursedSapling = 1837;

		// Token: 0x04002C7B RID: 11387
		public const short SpaceCreatureMask = 1838;

		// Token: 0x04002C7C RID: 11388
		public const short SpaceCreatureShirt = 1839;

		// Token: 0x04002C7D RID: 11389
		public const short SpaceCreaturePants = 1840;

		// Token: 0x04002C7E RID: 11390
		public const short WolfMask = 1841;

		// Token: 0x04002C7F RID: 11391
		public const short WolfShirt = 1842;

		// Token: 0x04002C80 RID: 11392
		public const short WolfPants = 1843;

		// Token: 0x04002C81 RID: 11393
		public const short PumpkinMoonMedallion = 1844;

		// Token: 0x04002C82 RID: 11394
		public const short NecromanticScroll = 1845;

		// Token: 0x04002C83 RID: 11395
		public const short JackingSkeletron = 1846;

		// Token: 0x04002C84 RID: 11396
		public const short BitterHarvest = 1847;

		// Token: 0x04002C85 RID: 11397
		public const short BloodMoonCountess = 1848;

		// Token: 0x04002C86 RID: 11398
		public const short HallowsEve = 1849;

		// Token: 0x04002C87 RID: 11399
		public const short MorbidCuriosity = 1850;

		// Token: 0x04002C88 RID: 11400
		public const short TreasureHunterShirt = 1851;

		// Token: 0x04002C89 RID: 11401
		public const short TreasureHunterPants = 1852;

		// Token: 0x04002C8A RID: 11402
		public const short DryadCoverings = 1853;

		// Token: 0x04002C8B RID: 11403
		public const short DryadLoincloth = 1854;

		// Token: 0x04002C8C RID: 11404
		public const short MourningWoodTrophy = 1855;

		// Token: 0x04002C8D RID: 11405
		public const short PumpkingTrophy = 1856;

		// Token: 0x04002C8E RID: 11406
		public const short JackOLanternMask = 1857;

		// Token: 0x04002C8F RID: 11407
		public const short SniperScope = 1858;

		// Token: 0x04002C90 RID: 11408
		public const short HeartLantern = 1859;

		// Token: 0x04002C91 RID: 11409
		public const short JellyfishDivingGear = 1860;

		// Token: 0x04002C92 RID: 11410
		public const short ArcticDivingGear = 1861;

		// Token: 0x04002C93 RID: 11411
		public const short FrostsparkBoots = 1862;

		// Token: 0x04002C94 RID: 11412
		public const short FartInABalloon = 1863;

		// Token: 0x04002C95 RID: 11413
		public const short PapyrusScarab = 1864;

		// Token: 0x04002C96 RID: 11414
		public const short CelestialStone = 1865;

		// Token: 0x04002C97 RID: 11415
		public const short Hoverboard = 1866;

		// Token: 0x04002C98 RID: 11416
		public const short CandyCane = 1867;

		// Token: 0x04002C99 RID: 11417
		public const short SugarPlum = 1868;

		// Token: 0x04002C9A RID: 11418
		public const short Present = 1869;

		// Token: 0x04002C9B RID: 11419
		public const short RedRyder = 1870;

		// Token: 0x04002C9C RID: 11420
		public const short FestiveWings = 1871;

		// Token: 0x04002C9D RID: 11421
		public const short PineTreeBlock = 1872;

		// Token: 0x04002C9E RID: 11422
		public const short ChristmasTree = 1873;

		// Token: 0x04002C9F RID: 11423
		public const short StarTopper1 = 1874;

		// Token: 0x04002CA0 RID: 11424
		public const short StarTopper2 = 1875;

		// Token: 0x04002CA1 RID: 11425
		public const short StarTopper3 = 1876;

		// Token: 0x04002CA2 RID: 11426
		public const short BowTopper = 1877;

		// Token: 0x04002CA3 RID: 11427
		public const short WhiteGarland = 1878;

		// Token: 0x04002CA4 RID: 11428
		public const short WhiteAndRedGarland = 1879;

		// Token: 0x04002CA5 RID: 11429
		public const short RedGardland = 1880;

		// Token: 0x04002CA6 RID: 11430
		public const short RedAndGreenGardland = 1881;

		// Token: 0x04002CA7 RID: 11431
		public const short GreenGardland = 1882;

		// Token: 0x04002CA8 RID: 11432
		public const short GreenAndWhiteGarland = 1883;

		// Token: 0x04002CA9 RID: 11433
		public const short MulticoloredBulb = 1884;

		// Token: 0x04002CAA RID: 11434
		public const short RedBulb = 1885;

		// Token: 0x04002CAB RID: 11435
		public const short YellowBulb = 1886;

		// Token: 0x04002CAC RID: 11436
		public const short GreenBulb = 1887;

		// Token: 0x04002CAD RID: 11437
		public const short RedAndGreenBulb = 1888;

		// Token: 0x04002CAE RID: 11438
		public const short YellowAndGreenBulb = 1889;

		// Token: 0x04002CAF RID: 11439
		public const short RedAndYellowBulb = 1890;

		// Token: 0x04002CB0 RID: 11440
		public const short WhiteBulb = 1891;

		// Token: 0x04002CB1 RID: 11441
		public const short WhiteAndRedBulb = 1892;

		// Token: 0x04002CB2 RID: 11442
		public const short WhiteAndYellowBulb = 1893;

		// Token: 0x04002CB3 RID: 11443
		public const short WhiteAndGreenBulb = 1894;

		// Token: 0x04002CB4 RID: 11444
		public const short MulticoloredLights = 1895;

		// Token: 0x04002CB5 RID: 11445
		public const short RedLights = 1896;

		// Token: 0x04002CB6 RID: 11446
		public const short GreenLights = 1897;

		// Token: 0x04002CB7 RID: 11447
		public const short BlueLights = 1898;

		// Token: 0x04002CB8 RID: 11448
		public const short YellowLights = 1899;

		// Token: 0x04002CB9 RID: 11449
		public const short RedAndYellowLights = 1900;

		// Token: 0x04002CBA RID: 11450
		public const short RedAndGreenLights = 1901;

		// Token: 0x04002CBB RID: 11451
		public const short YellowAndGreenLights = 1902;

		// Token: 0x04002CBC RID: 11452
		public const short BlueAndGreenLights = 1903;

		// Token: 0x04002CBD RID: 11453
		public const short RedAndBlueLights = 1904;

		// Token: 0x04002CBE RID: 11454
		public const short BlueAndYellowLights = 1905;

		// Token: 0x04002CBF RID: 11455
		public const short GiantBow = 1906;

		// Token: 0x04002CC0 RID: 11456
		public const short ReindeerAntlers = 1907;

		// Token: 0x04002CC1 RID: 11457
		public const short Holly = 1908;

		// Token: 0x04002CC2 RID: 11458
		public const short CandyCaneSword = 1909;

		// Token: 0x04002CC3 RID: 11459
		public const short ElfMelter = 1910;

		// Token: 0x04002CC4 RID: 11460
		public const short ChristmasPudding = 1911;

		// Token: 0x04002CC5 RID: 11461
		public const short Eggnog = 1912;

		// Token: 0x04002CC6 RID: 11462
		public const short StarAnise = 1913;

		// Token: 0x04002CC7 RID: 11463
		public const short ReindeerBells = 1914;

		// Token: 0x04002CC8 RID: 11464
		public const short CandyCaneHook = 1915;

		// Token: 0x04002CC9 RID: 11465
		public const short ChristmasHook = 1916;

		// Token: 0x04002CCA RID: 11466
		public const short CnadyCanePickaxe = 1917;

		// Token: 0x04002CCB RID: 11467
		public const short FruitcakeChakram = 1918;

		// Token: 0x04002CCC RID: 11468
		public const short SugarCookie = 1919;

		// Token: 0x04002CCD RID: 11469
		public const short GingerbreadCookie = 1920;

		// Token: 0x04002CCE RID: 11470
		public const short HandWarmer = 1921;

		// Token: 0x04002CCF RID: 11471
		public const short Coal = 1922;

		// Token: 0x04002CD0 RID: 11472
		public const short Toolbox = 1923;

		// Token: 0x04002CD1 RID: 11473
		public const short PineDoor = 1924;

		// Token: 0x04002CD2 RID: 11474
		public const short PineChair = 1925;

		// Token: 0x04002CD3 RID: 11475
		public const short PineTable = 1926;

		// Token: 0x04002CD4 RID: 11476
		public const short DogWhistle = 1927;

		// Token: 0x04002CD5 RID: 11477
		public const short ChristmasTreeSword = 1928;

		// Token: 0x04002CD6 RID: 11478
		public const short ChainGun = 1929;

		// Token: 0x04002CD7 RID: 11479
		public const short Razorpine = 1930;

		// Token: 0x04002CD8 RID: 11480
		public const short BlizzardStaff = 1931;

		// Token: 0x04002CD9 RID: 11481
		public const short MrsClauseHat = 1932;

		// Token: 0x04002CDA RID: 11482
		public const short MrsClauseShirt = 1933;

		// Token: 0x04002CDB RID: 11483
		public const short MrsClauseHeels = 1934;

		// Token: 0x04002CDC RID: 11484
		public const short ParkaHood = 1935;

		// Token: 0x04002CDD RID: 11485
		public const short ParkaCoat = 1936;

		// Token: 0x04002CDE RID: 11486
		public const short ParkaPants = 1937;

		// Token: 0x04002CDF RID: 11487
		public const short SnowHat = 1938;

		// Token: 0x04002CE0 RID: 11488
		public const short UglySweater = 1939;

		// Token: 0x04002CE1 RID: 11489
		public const short TreeMask = 1940;

		// Token: 0x04002CE2 RID: 11490
		public const short TreeShirt = 1941;

		// Token: 0x04002CE3 RID: 11491
		public const short TreeTrunks = 1942;

		// Token: 0x04002CE4 RID: 11492
		public const short ElfHat = 1943;

		// Token: 0x04002CE5 RID: 11493
		public const short ElfShirt = 1944;

		// Token: 0x04002CE6 RID: 11494
		public const short ElfPants = 1945;

		// Token: 0x04002CE7 RID: 11495
		public const short SnowmanCannon = 1946;

		// Token: 0x04002CE8 RID: 11496
		public const short NorthPole = 1947;

		// Token: 0x04002CE9 RID: 11497
		public const short ChristmasTreeWallpaper = 1948;

		// Token: 0x04002CEA RID: 11498
		public const short OrnamentWallpaper = 1949;

		// Token: 0x04002CEB RID: 11499
		public const short CandyCaneWallpaper = 1950;

		// Token: 0x04002CEC RID: 11500
		public const short FestiveWallpaper = 1951;

		// Token: 0x04002CED RID: 11501
		public const short StarsWallpaper = 1952;

		// Token: 0x04002CEE RID: 11502
		public const short SquigglesWallpaper = 1953;

		// Token: 0x04002CEF RID: 11503
		public const short SnowflakeWallpaper = 1954;

		// Token: 0x04002CF0 RID: 11504
		public const short KrampusHornWallpaper = 1955;

		// Token: 0x04002CF1 RID: 11505
		public const short BluegreenWallpaper = 1956;

		// Token: 0x04002CF2 RID: 11506
		public const short GrinchFingerWallpaper = 1957;

		// Token: 0x04002CF3 RID: 11507
		public const short NaughtyPresent = 1958;

		// Token: 0x04002CF4 RID: 11508
		public const short BabyGrinchMischiefWhistle = 1959;

		// Token: 0x04002CF5 RID: 11509
		public const short IceQueenTrophy = 1960;

		// Token: 0x04002CF6 RID: 11510
		public const short SantaNK1Trophy = 1961;

		// Token: 0x04002CF7 RID: 11511
		public const short EverscreamTrophy = 1962;

		// Token: 0x04002CF8 RID: 11512
		public const short MusicBoxPumpkinMoon = 1963;

		// Token: 0x04002CF9 RID: 11513
		public const short MusicBoxAltUnderground = 1964;

		// Token: 0x04002CFA RID: 11514
		public const short MusicBoxFrostMoon = 1965;

		// Token: 0x04002CFB RID: 11515
		public const short BrownPaint = 1966;

		// Token: 0x04002CFC RID: 11516
		public const short ShadowPaint = 1967;

		// Token: 0x04002CFD RID: 11517
		public const short NegativePaint = 1968;

		// Token: 0x04002CFE RID: 11518
		public const short TeamDye = 1969;

		// Token: 0x04002CFF RID: 11519
		public const short AmethystGemsparkBlock = 1970;

		// Token: 0x04002D00 RID: 11520
		public const short TopazGemsparkBlock = 1971;

		// Token: 0x04002D01 RID: 11521
		public const short SapphireGemsparkBlock = 1972;

		// Token: 0x04002D02 RID: 11522
		public const short EmeraldGemsparkBlock = 1973;

		// Token: 0x04002D03 RID: 11523
		public const short RubyGemsparkBlock = 1974;

		// Token: 0x04002D04 RID: 11524
		public const short DiamondGemsparkBlock = 1975;

		// Token: 0x04002D05 RID: 11525
		public const short AmberGemsparkBlock = 1976;

		// Token: 0x04002D06 RID: 11526
		public const short LifeHairDye = 1977;

		// Token: 0x04002D07 RID: 11527
		public const short ManaHairDye = 1978;

		// Token: 0x04002D08 RID: 11528
		public const short DepthHairDye = 1979;

		// Token: 0x04002D09 RID: 11529
		public const short MoneyHairDye = 1980;

		// Token: 0x04002D0A RID: 11530
		public const short TimeHairDye = 1981;

		// Token: 0x04002D0B RID: 11531
		public const short TeamHairDye = 1982;

		// Token: 0x04002D0C RID: 11532
		public const short BiomeHairDye = 1983;

		// Token: 0x04002D0D RID: 11533
		public const short PartyHairDye = 1984;

		// Token: 0x04002D0E RID: 11534
		public const short RainbowHairDye = 1985;

		// Token: 0x04002D0F RID: 11535
		public const short SpeedHairDye = 1986;

		// Token: 0x04002D10 RID: 11536
		public const short AngelHalo = 1987;

		// Token: 0x04002D11 RID: 11537
		public const short Fez = 1988;

		// Token: 0x04002D12 RID: 11538
		public const short Womannquin = 1989;

		// Token: 0x04002D13 RID: 11539
		public const short HairDyeRemover = 1990;

		// Token: 0x04002D14 RID: 11540
		public const short BugNet = 1991;

		// Token: 0x04002D15 RID: 11541
		public const short Firefly = 1992;

		// Token: 0x04002D16 RID: 11542
		public const short FireflyinaBottle = 1993;

		// Token: 0x04002D17 RID: 11543
		public const short MonarchButterfly = 1994;

		// Token: 0x04002D18 RID: 11544
		public const short PurpleEmperorButterfly = 1995;

		// Token: 0x04002D19 RID: 11545
		public const short RedAdmiralButterfly = 1996;

		// Token: 0x04002D1A RID: 11546
		public const short UlyssesButterfly = 1997;

		// Token: 0x04002D1B RID: 11547
		public const short SulphurButterfly = 1998;

		// Token: 0x04002D1C RID: 11548
		public const short TreeNymphButterfly = 1999;

		// Token: 0x04002D1D RID: 11549
		public const short ZebraSwallowtailButterfly = 2000;

		// Token: 0x04002D1E RID: 11550
		public const short JuliaButterfly = 2001;

		// Token: 0x04002D1F RID: 11551
		public const short Worm = 2002;

		// Token: 0x04002D20 RID: 11552
		public const short Mouse = 2003;

		// Token: 0x04002D21 RID: 11553
		public const short LightningBug = 2004;

		// Token: 0x04002D22 RID: 11554
		public const short LightningBuginaBottle = 2005;

		// Token: 0x04002D23 RID: 11555
		public const short Snail = 2006;

		// Token: 0x04002D24 RID: 11556
		public const short GlowingSnail = 2007;

		// Token: 0x04002D25 RID: 11557
		public const short FancyGreyWallpaper = 2008;

		// Token: 0x04002D26 RID: 11558
		public const short IceFloeWallpaper = 2009;

		// Token: 0x04002D27 RID: 11559
		public const short MusicWallpaper = 2010;

		// Token: 0x04002D28 RID: 11560
		public const short PurpleRainWallpaper = 2011;

		// Token: 0x04002D29 RID: 11561
		public const short RainbowWallpaper = 2012;

		// Token: 0x04002D2A RID: 11562
		public const short SparkleStoneWallpaper = 2013;

		// Token: 0x04002D2B RID: 11563
		public const short StarlitHeavenWallpaper = 2014;

		// Token: 0x04002D2C RID: 11564
		public const short Bird = 2015;

		// Token: 0x04002D2D RID: 11565
		public const short BlueJay = 2016;

		// Token: 0x04002D2E RID: 11566
		public const short Cardinal = 2017;

		// Token: 0x04002D2F RID: 11567
		public const short Squirrel = 2018;

		// Token: 0x04002D30 RID: 11568
		public const short Bunny = 2019;

		// Token: 0x04002D31 RID: 11569
		public const short CactusBookcase = 2020;

		// Token: 0x04002D32 RID: 11570
		public const short EbonwoodBookcase = 2021;

		// Token: 0x04002D33 RID: 11571
		public const short FleshBookcase = 2022;

		// Token: 0x04002D34 RID: 11572
		public const short HoneyBookcase = 2023;

		// Token: 0x04002D35 RID: 11573
		public const short SteampunkBookcase = 2024;

		// Token: 0x04002D36 RID: 11574
		public const short GlassBookcase = 2025;

		// Token: 0x04002D37 RID: 11575
		public const short RichMahoganyBookcase = 2026;

		// Token: 0x04002D38 RID: 11576
		public const short PearlwoodBookcase = 2027;

		// Token: 0x04002D39 RID: 11577
		public const short SpookyBookcase = 2028;

		// Token: 0x04002D3A RID: 11578
		public const short SkywareBookcase = 2029;

		// Token: 0x04002D3B RID: 11579
		public const short LihzahrdBookcase = 2030;

		// Token: 0x04002D3C RID: 11580
		public const short FrozenBookcase = 2031;

		// Token: 0x04002D3D RID: 11581
		public const short CactusLantern = 2032;

		// Token: 0x04002D3E RID: 11582
		public const short EbonwoodLantern = 2033;

		// Token: 0x04002D3F RID: 11583
		public const short FleshLantern = 2034;

		// Token: 0x04002D40 RID: 11584
		public const short HoneyLantern = 2035;

		// Token: 0x04002D41 RID: 11585
		public const short SteampunkLantern = 2036;

		// Token: 0x04002D42 RID: 11586
		public const short GlassLantern = 2037;

		// Token: 0x04002D43 RID: 11587
		public const short RichMahoganyLantern = 2038;

		// Token: 0x04002D44 RID: 11588
		public const short PearlwoodLantern = 2039;

		// Token: 0x04002D45 RID: 11589
		public const short FrozenLantern = 2040;

		// Token: 0x04002D46 RID: 11590
		public const short LihzahrdLantern = 2041;

		// Token: 0x04002D47 RID: 11591
		public const short SkywareLantern = 2042;

		// Token: 0x04002D48 RID: 11592
		public const short SpookyLantern = 2043;

		// Token: 0x04002D49 RID: 11593
		public const short FrozenDoor = 2044;

		// Token: 0x04002D4A RID: 11594
		public const short CactusCandle = 2045;

		// Token: 0x04002D4B RID: 11595
		public const short EbonwoodCandle = 2046;

		// Token: 0x04002D4C RID: 11596
		public const short FleshCandle = 2047;

		// Token: 0x04002D4D RID: 11597
		public const short GlassCandle = 2048;

		// Token: 0x04002D4E RID: 11598
		public const short FrozenCandle = 2049;

		// Token: 0x04002D4F RID: 11599
		public const short RichMahoganyCandle = 2050;

		// Token: 0x04002D50 RID: 11600
		public const short PearlwoodCandle = 2051;

		// Token: 0x04002D51 RID: 11601
		public const short LihzahrdCandle = 2052;

		// Token: 0x04002D52 RID: 11602
		public const short SkywareCandle = 2053;

		// Token: 0x04002D53 RID: 11603
		public const short PumpkinCandle = 2054;

		// Token: 0x04002D54 RID: 11604
		public const short CactusChandelier = 2055;

		// Token: 0x04002D55 RID: 11605
		public const short EbonwoodChandelier = 2056;

		// Token: 0x04002D56 RID: 11606
		public const short FleshChandelier = 2057;

		// Token: 0x04002D57 RID: 11607
		public const short HoneyChandelier = 2058;

		// Token: 0x04002D58 RID: 11608
		public const short FrozenChandelier = 2059;

		// Token: 0x04002D59 RID: 11609
		public const short RichMahoganyChandelier = 2060;

		// Token: 0x04002D5A RID: 11610
		public const short PearlwoodChandelier = 2061;

		// Token: 0x04002D5B RID: 11611
		public const short LihzahrdChandelier = 2062;

		// Token: 0x04002D5C RID: 11612
		public const short SkywareChandelier = 2063;

		// Token: 0x04002D5D RID: 11613
		public const short SpookyChandelier = 2064;

		// Token: 0x04002D5E RID: 11614
		public const short GlassChandelier = 2065;

		// Token: 0x04002D5F RID: 11615
		public const short CactusBed = 2066;

		// Token: 0x04002D60 RID: 11616
		public const short FleshBed = 2067;

		// Token: 0x04002D61 RID: 11617
		public const short FrozenBed = 2068;

		// Token: 0x04002D62 RID: 11618
		public const short LihzahrdBed = 2069;

		// Token: 0x04002D63 RID: 11619
		public const short SkywareBed = 2070;

		// Token: 0x04002D64 RID: 11620
		public const short SpookyBed = 2071;

		// Token: 0x04002D65 RID: 11621
		public const short CactusBathtub = 2072;

		// Token: 0x04002D66 RID: 11622
		public const short EbonwoodBathtub = 2073;

		// Token: 0x04002D67 RID: 11623
		public const short FleshBathtub = 2074;

		// Token: 0x04002D68 RID: 11624
		public const short GlassBathtub = 2075;

		// Token: 0x04002D69 RID: 11625
		public const short FrozenBathtub = 2076;

		// Token: 0x04002D6A RID: 11626
		public const short RichMahoganyBathtub = 2077;

		// Token: 0x04002D6B RID: 11627
		public const short PearlwoodBathtub = 2078;

		// Token: 0x04002D6C RID: 11628
		public const short LihzahrdBathtub = 2079;

		// Token: 0x04002D6D RID: 11629
		public const short SkywareBathtub = 2080;

		// Token: 0x04002D6E RID: 11630
		public const short SpookyBathtub = 2081;

		// Token: 0x04002D6F RID: 11631
		public const short CactusLamp = 2082;

		// Token: 0x04002D70 RID: 11632
		public const short EbonwoodLamp = 2083;

		// Token: 0x04002D71 RID: 11633
		public const short FleshLamp = 2084;

		// Token: 0x04002D72 RID: 11634
		public const short GlassLamp = 2085;

		// Token: 0x04002D73 RID: 11635
		public const short FrozenLamp = 2086;

		// Token: 0x04002D74 RID: 11636
		public const short RichMahoganyLamp = 2087;

		// Token: 0x04002D75 RID: 11637
		public const short PearlwoodLamp = 2088;

		// Token: 0x04002D76 RID: 11638
		public const short LihzahrdLamp = 2089;

		// Token: 0x04002D77 RID: 11639
		public const short SkywareLamp = 2090;

		// Token: 0x04002D78 RID: 11640
		public const short SpookyLamp = 2091;

		// Token: 0x04002D79 RID: 11641
		public const short CactusCandelabra = 2092;

		// Token: 0x04002D7A RID: 11642
		public const short EbonwoodCandelabra = 2093;

		// Token: 0x04002D7B RID: 11643
		public const short FleshCandelabra = 2094;

		// Token: 0x04002D7C RID: 11644
		public const short HoneyCandelabra = 2095;

		// Token: 0x04002D7D RID: 11645
		public const short SteampunkCandelabra = 2096;

		// Token: 0x04002D7E RID: 11646
		public const short GlassCandelabra = 2097;

		// Token: 0x04002D7F RID: 11647
		public const short RichMahoganyCandelabra = 2098;

		// Token: 0x04002D80 RID: 11648
		public const short PearlwoodCandelabra = 2099;

		// Token: 0x04002D81 RID: 11649
		public const short FrozenCandelabra = 2100;

		// Token: 0x04002D82 RID: 11650
		public const short LihzahrdCandelabra = 2101;

		// Token: 0x04002D83 RID: 11651
		public const short SkywareCandelabra = 2102;

		// Token: 0x04002D84 RID: 11652
		public const short SpookyCandelabra = 2103;

		// Token: 0x04002D85 RID: 11653
		public const short BrainMask = 2104;

		// Token: 0x04002D86 RID: 11654
		public const short FleshMask = 2105;

		// Token: 0x04002D87 RID: 11655
		public const short TwinMask = 2106;

		// Token: 0x04002D88 RID: 11656
		public const short SkeletronPrimeMask = 2107;

		// Token: 0x04002D89 RID: 11657
		public const short BeeMask = 2108;

		// Token: 0x04002D8A RID: 11658
		public const short PlanteraMask = 2109;

		// Token: 0x04002D8B RID: 11659
		public const short GolemMask = 2110;

		// Token: 0x04002D8C RID: 11660
		public const short EaterMask = 2111;

		// Token: 0x04002D8D RID: 11661
		public const short EyeMask = 2112;

		// Token: 0x04002D8E RID: 11662
		public const short DestroyerMask = 2113;

		// Token: 0x04002D8F RID: 11663
		public const short BlacksmithRack = 2114;

		// Token: 0x04002D90 RID: 11664
		public const short CarpentryRack = 2115;

		// Token: 0x04002D91 RID: 11665
		public const short HelmetRack = 2116;

		// Token: 0x04002D92 RID: 11666
		public const short SpearRack = 2117;

		// Token: 0x04002D93 RID: 11667
		public const short SwordRack = 2118;

		// Token: 0x04002D94 RID: 11668
		public const short StoneSlab = 2119;

		// Token: 0x04002D95 RID: 11669
		public const short SandstoneSlab = 2120;

		// Token: 0x04002D96 RID: 11670
		public const short Frog = 2121;

		// Token: 0x04002D97 RID: 11671
		public const short MallardDuck = 2122;

		// Token: 0x04002D98 RID: 11672
		public const short Duck = 2123;

		// Token: 0x04002D99 RID: 11673
		public const short HoneyBathtub = 2124;

		// Token: 0x04002D9A RID: 11674
		public const short SteampunkBathtub = 2125;

		// Token: 0x04002D9B RID: 11675
		public const short LivingWoodBathtub = 2126;

		// Token: 0x04002D9C RID: 11676
		public const short ShadewoodBathtub = 2127;

		// Token: 0x04002D9D RID: 11677
		public const short BoneBathtub = 2128;

		// Token: 0x04002D9E RID: 11678
		public const short HoneyLamp = 2129;

		// Token: 0x04002D9F RID: 11679
		public const short SteampunkLamp = 2130;

		// Token: 0x04002DA0 RID: 11680
		public const short LivingWoodLamp = 2131;

		// Token: 0x04002DA1 RID: 11681
		public const short ShadewoodLamp = 2132;

		// Token: 0x04002DA2 RID: 11682
		public const short GoldenLamp = 2133;

		// Token: 0x04002DA3 RID: 11683
		public const short BoneLamp = 2134;

		// Token: 0x04002DA4 RID: 11684
		public const short LivingWoodBookcase = 2135;

		// Token: 0x04002DA5 RID: 11685
		public const short ShadewoodBookcase = 2136;

		// Token: 0x04002DA6 RID: 11686
		public const short GoldenBookcase = 2137;

		// Token: 0x04002DA7 RID: 11687
		public const short BoneBookcase = 2138;

		// Token: 0x04002DA8 RID: 11688
		public const short LivingWoodBed = 2139;

		// Token: 0x04002DA9 RID: 11689
		public const short BoneBed = 2140;

		// Token: 0x04002DAA RID: 11690
		public const short LivingWoodChandelier = 2141;

		// Token: 0x04002DAB RID: 11691
		public const short ShadewoodChandelier = 2142;

		// Token: 0x04002DAC RID: 11692
		public const short GoldenChandelier = 2143;

		// Token: 0x04002DAD RID: 11693
		public const short BoneChandelier = 2144;

		// Token: 0x04002DAE RID: 11694
		public const short LivingWoodLantern = 2145;

		// Token: 0x04002DAF RID: 11695
		public const short ShadewoodLantern = 2146;

		// Token: 0x04002DB0 RID: 11696
		public const short GoldenLantern = 2147;

		// Token: 0x04002DB1 RID: 11697
		public const short BoneLantern = 2148;

		// Token: 0x04002DB2 RID: 11698
		public const short LivingWoodCandelabra = 2149;

		// Token: 0x04002DB3 RID: 11699
		public const short ShadewoodCandelabra = 2150;

		// Token: 0x04002DB4 RID: 11700
		public const short GoldenCandelabra = 2151;

		// Token: 0x04002DB5 RID: 11701
		public const short BoneCandelabra = 2152;

		// Token: 0x04002DB6 RID: 11702
		public const short LivingWoodCandle = 2153;

		// Token: 0x04002DB7 RID: 11703
		public const short ShadewoodCandle = 2154;

		// Token: 0x04002DB8 RID: 11704
		public const short GoldenCandle = 2155;

		// Token: 0x04002DB9 RID: 11705
		public const short BlackScorpion = 2156;

		// Token: 0x04002DBA RID: 11706
		public const short Scorpion = 2157;

		// Token: 0x04002DBB RID: 11707
		public const short BubbleWallpaper = 2158;

		// Token: 0x04002DBC RID: 11708
		public const short CopperPipeWallpaper = 2159;

		// Token: 0x04002DBD RID: 11709
		public const short DuckyWallpaper = 2160;

		// Token: 0x04002DBE RID: 11710
		public const short FrostCore = 2161;

		// Token: 0x04002DBF RID: 11711
		public const short BunnyCage = 2162;

		// Token: 0x04002DC0 RID: 11712
		public const short SquirrelCage = 2163;

		// Token: 0x04002DC1 RID: 11713
		public const short MallardDuckCage = 2164;

		// Token: 0x04002DC2 RID: 11714
		public const short DuckCage = 2165;

		// Token: 0x04002DC3 RID: 11715
		public const short BirdCage = 2166;

		// Token: 0x04002DC4 RID: 11716
		public const short BlueJayCage = 2167;

		// Token: 0x04002DC5 RID: 11717
		public const short CardinalCage = 2168;

		// Token: 0x04002DC6 RID: 11718
		public const short WaterfallWall = 2169;

		// Token: 0x04002DC7 RID: 11719
		public const short LavafallWall = 2170;

		// Token: 0x04002DC8 RID: 11720
		public const short CrimsonSeeds = 2171;

		// Token: 0x04002DC9 RID: 11721
		public const short HeavyWorkBench = 2172;

		// Token: 0x04002DCA RID: 11722
		public const short CopperPlating = 2173;

		// Token: 0x04002DCB RID: 11723
		public const short SnailCage = 2174;

		// Token: 0x04002DCC RID: 11724
		public const short GlowingSnailCage = 2175;

		// Token: 0x04002DCD RID: 11725
		public const short ShroomiteDiggingClaw = 2176;

		// Token: 0x04002DCE RID: 11726
		public const short AmmoBox = 2177;

		// Token: 0x04002DCF RID: 11727
		public const short MonarchButterflyJar = 2178;

		// Token: 0x04002DD0 RID: 11728
		public const short PurpleEmperorButterflyJar = 2179;

		// Token: 0x04002DD1 RID: 11729
		public const short RedAdmiralButterflyJar = 2180;

		// Token: 0x04002DD2 RID: 11730
		public const short UlyssesButterflyJar = 2181;

		// Token: 0x04002DD3 RID: 11731
		public const short SulphurButterflyJar = 2182;

		// Token: 0x04002DD4 RID: 11732
		public const short TreeNymphButterflyJar = 2183;

		// Token: 0x04002DD5 RID: 11733
		public const short ZebraSwallowtailButterflyJar = 2184;

		// Token: 0x04002DD6 RID: 11734
		public const short JuliaButterflyJar = 2185;

		// Token: 0x04002DD7 RID: 11735
		public const short ScorpionCage = 2186;

		// Token: 0x04002DD8 RID: 11736
		public const short BlackScorpionCage = 2187;

		// Token: 0x04002DD9 RID: 11737
		public const short VenomStaff = 2188;

		// Token: 0x04002DDA RID: 11738
		public const short SpectreMask = 2189;

		// Token: 0x04002DDB RID: 11739
		public const short FrogCage = 2190;

		// Token: 0x04002DDC RID: 11740
		public const short MouseCage = 2191;

		// Token: 0x04002DDD RID: 11741
		public const short BoneWelder = 2192;

		// Token: 0x04002DDE RID: 11742
		public const short FleshCloningVaat = 2193;

		// Token: 0x04002DDF RID: 11743
		public const short GlassKiln = 2194;

		// Token: 0x04002DE0 RID: 11744
		public const short LihzahrdFurnace = 2195;

		// Token: 0x04002DE1 RID: 11745
		public const short LivingLoom = 2196;

		// Token: 0x04002DE2 RID: 11746
		public const short SkyMill = 2197;

		// Token: 0x04002DE3 RID: 11747
		public const short IceMachine = 2198;

		// Token: 0x04002DE4 RID: 11748
		public const short BeetleHelmet = 2199;

		// Token: 0x04002DE5 RID: 11749
		public const short BeetleScaleMail = 2200;

		// Token: 0x04002DE6 RID: 11750
		public const short BeetleShell = 2201;

		// Token: 0x04002DE7 RID: 11751
		public const short BeetleLeggings = 2202;

		// Token: 0x04002DE8 RID: 11752
		public const short SteampunkBoiler = 2203;

		// Token: 0x04002DE9 RID: 11753
		public const short HoneyDispenser = 2204;

		// Token: 0x04002DEA RID: 11754
		public const short Penguin = 2205;

		// Token: 0x04002DEB RID: 11755
		public const short PenguinCage = 2206;

		// Token: 0x04002DEC RID: 11756
		public const short WormCage = 2207;

		// Token: 0x04002DED RID: 11757
		public const short Terrarium = 2208;

		// Token: 0x04002DEE RID: 11758
		public const short SuperManaPotion = 2209;

		// Token: 0x04002DEF RID: 11759
		public const short EbonwoodFence = 2210;

		// Token: 0x04002DF0 RID: 11760
		public const short RichMahoganyFence = 2211;

		// Token: 0x04002DF1 RID: 11761
		public const short PearlwoodFence = 2212;

		// Token: 0x04002DF2 RID: 11762
		public const short ShadewoodFence = 2213;

		// Token: 0x04002DF3 RID: 11763
		public const short BrickLayer = 2214;

		// Token: 0x04002DF4 RID: 11764
		public const short ExtendoGrip = 2215;

		// Token: 0x04002DF5 RID: 11765
		public const short PaintSprayer = 2216;

		// Token: 0x04002DF6 RID: 11766
		public const short PortableCementMixer = 2217;

		// Token: 0x04002DF7 RID: 11767
		public const short BeetleHusk = 2218;

		// Token: 0x04002DF8 RID: 11768
		public const short CelestialMagnet = 2219;

		// Token: 0x04002DF9 RID: 11769
		public const short CelestialEmblem = 2220;

		// Token: 0x04002DFA RID: 11770
		public const short CelestialCuffs = 2221;

		// Token: 0x04002DFB RID: 11771
		public const short PeddlersHat = 2222;

		// Token: 0x04002DFC RID: 11772
		public const short PulseBow = 2223;

		// Token: 0x04002DFD RID: 11773
		public const short DynastyChandelier = 2224;

		// Token: 0x04002DFE RID: 11774
		public const short DynastyLamp = 2225;

		// Token: 0x04002DFF RID: 11775
		public const short DynastyLantern = 2226;

		// Token: 0x04002E00 RID: 11776
		public const short DynastyCandelabra = 2227;

		// Token: 0x04002E01 RID: 11777
		public const short DynastyChair = 2228;

		// Token: 0x04002E02 RID: 11778
		public const short DynastyWorkBench = 2229;

		// Token: 0x04002E03 RID: 11779
		public const short DynastyChest = 2230;

		// Token: 0x04002E04 RID: 11780
		public const short DynastyBed = 2231;

		// Token: 0x04002E05 RID: 11781
		public const short DynastyBathtub = 2232;

		// Token: 0x04002E06 RID: 11782
		public const short DynastyBookcase = 2233;

		// Token: 0x04002E07 RID: 11783
		public const short DynastyCup = 2234;

		// Token: 0x04002E08 RID: 11784
		public const short DynastyBowl = 2235;

		// Token: 0x04002E09 RID: 11785
		public const short DynastyCandle = 2236;

		// Token: 0x04002E0A RID: 11786
		public const short DynastyClock = 2237;

		// Token: 0x04002E0B RID: 11787
		public const short GoldenClock = 2238;

		// Token: 0x04002E0C RID: 11788
		public const short GlassClock = 2239;

		// Token: 0x04002E0D RID: 11789
		public const short HoneyClock = 2240;

		// Token: 0x04002E0E RID: 11790
		public const short SteampunkClock = 2241;

		// Token: 0x04002E0F RID: 11791
		public const short FancyDishes = 2242;

		// Token: 0x04002E10 RID: 11792
		public const short GlassBowl = 2243;

		// Token: 0x04002E11 RID: 11793
		public const short WineGlass = 2244;

		// Token: 0x04002E12 RID: 11794
		public const short LivingWoodPiano = 2245;

		// Token: 0x04002E13 RID: 11795
		public const short FleshPiano = 2246;

		// Token: 0x04002E14 RID: 11796
		public const short FrozenPiano = 2247;

		// Token: 0x04002E15 RID: 11797
		public const short FrozenTable = 2248;

		// Token: 0x04002E16 RID: 11798
		public const short HoneyChest = 2249;

		// Token: 0x04002E17 RID: 11799
		public const short SteampunkChest = 2250;

		// Token: 0x04002E18 RID: 11800
		public const short HoneyWorkBench = 2251;

		// Token: 0x04002E19 RID: 11801
		public const short FrozenWorkBench = 2252;

		// Token: 0x04002E1A RID: 11802
		public const short SteampunkWorkBench = 2253;

		// Token: 0x04002E1B RID: 11803
		public const short GlassPiano = 2254;

		// Token: 0x04002E1C RID: 11804
		public const short HoneyPiano = 2255;

		// Token: 0x04002E1D RID: 11805
		public const short SteampunkPiano = 2256;

		// Token: 0x04002E1E RID: 11806
		public const short HoneyCup = 2257;

		// Token: 0x04002E1F RID: 11807
		public const short SteampunkCup = 2258;

		// Token: 0x04002E20 RID: 11808
		public const short DynastyTable = 2259;

		// Token: 0x04002E21 RID: 11809
		public const short DynastyWood = 2260;

		// Token: 0x04002E22 RID: 11810
		public const short RedDynastyShingles = 2261;

		// Token: 0x04002E23 RID: 11811
		public const short BlueDynastyShingles = 2262;

		// Token: 0x04002E24 RID: 11812
		public const short WhiteDynastyWall = 2263;

		// Token: 0x04002E25 RID: 11813
		public const short BlueDynastyWall = 2264;

		// Token: 0x04002E26 RID: 11814
		public const short DynastyDoor = 2265;

		// Token: 0x04002E27 RID: 11815
		public const short Sake = 2266;

		// Token: 0x04002E28 RID: 11816
		public const short PadThai = 2267;

		// Token: 0x04002E29 RID: 11817
		public const short Pho = 2268;

		// Token: 0x04002E2A RID: 11818
		public const short Revolver = 2269;

		// Token: 0x04002E2B RID: 11819
		public const short Gatligator = 2270;

		// Token: 0x04002E2C RID: 11820
		public const short ArcaneRuneWall = 2271;

		// Token: 0x04002E2D RID: 11821
		public const short WaterGun = 2272;

		// Token: 0x04002E2E RID: 11822
		public const short Katana = 2273;

		// Token: 0x04002E2F RID: 11823
		public const short UltrabrightTorch = 2274;

		// Token: 0x04002E30 RID: 11824
		public const short MagicHat = 2275;

		// Token: 0x04002E31 RID: 11825
		public const short DiamondRing = 2276;

		// Token: 0x04002E32 RID: 11826
		public const short Gi = 2277;

		// Token: 0x04002E33 RID: 11827
		public const short Kimono = 2278;

		// Token: 0x04002E34 RID: 11828
		public const short GypsyRobe = 2279;

		// Token: 0x04002E35 RID: 11829
		public const short BeetleWings = 2280;

		// Token: 0x04002E36 RID: 11830
		public const short TigerSkin = 2281;

		// Token: 0x04002E37 RID: 11831
		public const short LeopardSkin = 2282;

		// Token: 0x04002E38 RID: 11832
		public const short ZebraSkin = 2283;

		// Token: 0x04002E39 RID: 11833
		public const short CrimsonCloak = 2284;

		// Token: 0x04002E3A RID: 11834
		public const short MysteriousCape = 2285;

		// Token: 0x04002E3B RID: 11835
		public const short RedCape = 2286;

		// Token: 0x04002E3C RID: 11836
		public const short WinterCape = 2287;

		// Token: 0x04002E3D RID: 11837
		public const short FrozenChair = 2288;

		// Token: 0x04002E3E RID: 11838
		public const short WoodFishingPole = 2289;

		// Token: 0x04002E3F RID: 11839
		public const short Bass = 2290;

		// Token: 0x04002E40 RID: 11840
		public const short ReinforcedFishingPole = 2291;

		// Token: 0x04002E41 RID: 11841
		public const short FiberglassFishingPole = 2292;

		// Token: 0x04002E42 RID: 11842
		public const short FisherofSouls = 2293;

		// Token: 0x04002E43 RID: 11843
		public const short GoldenFishingRod = 2294;

		// Token: 0x04002E44 RID: 11844
		public const short MechanicsRod = 2295;

		// Token: 0x04002E45 RID: 11845
		public const short SittingDucksFishingRod = 2296;

		// Token: 0x04002E46 RID: 11846
		public const short Trout = 2297;

		// Token: 0x04002E47 RID: 11847
		public const short Salmon = 2298;

		// Token: 0x04002E48 RID: 11848
		public const short AtlanticCod = 2299;

		// Token: 0x04002E49 RID: 11849
		public const short Tuna = 2300;

		// Token: 0x04002E4A RID: 11850
		public const short RedSnapper = 2301;

		// Token: 0x04002E4B RID: 11851
		public const short NeonTetra = 2302;

		// Token: 0x04002E4C RID: 11852
		public const short ArmoredCavefish = 2303;

		// Token: 0x04002E4D RID: 11853
		public const short Damselfish = 2304;

		// Token: 0x04002E4E RID: 11854
		public const short CrimsonTigerfish = 2305;

		// Token: 0x04002E4F RID: 11855
		public const short FrostMinnow = 2306;

		// Token: 0x04002E50 RID: 11856
		public const short PrincessFish = 2307;

		// Token: 0x04002E51 RID: 11857
		public const short GoldenCarp = 2308;

		// Token: 0x04002E52 RID: 11858
		public const short SpecularFish = 2309;

		// Token: 0x04002E53 RID: 11859
		public const short Prismite = 2310;

		// Token: 0x04002E54 RID: 11860
		public const short VariegatedLardfish = 2311;

		// Token: 0x04002E55 RID: 11861
		public const short FlarefinKoi = 2312;

		// Token: 0x04002E56 RID: 11862
		public const short DoubleCod = 2313;

		// Token: 0x04002E57 RID: 11863
		public const short Honeyfin = 2314;

		// Token: 0x04002E58 RID: 11864
		public const short Obsidifish = 2315;

		// Token: 0x04002E59 RID: 11865
		public const short Shrimp = 2316;

		// Token: 0x04002E5A RID: 11866
		public const short ChaosFish = 2317;

		// Token: 0x04002E5B RID: 11867
		public const short Ebonkoi = 2318;

		// Token: 0x04002E5C RID: 11868
		public const short Hemopiranha = 2319;

		// Token: 0x04002E5D RID: 11869
		public const short Rockfish = 2320;

		// Token: 0x04002E5E RID: 11870
		public const short Stinkfish = 2321;

		// Token: 0x04002E5F RID: 11871
		public const short MiningPotion = 2322;

		// Token: 0x04002E60 RID: 11872
		public const short HeartreachPotion = 2323;

		// Token: 0x04002E61 RID: 11873
		public const short CalmingPotion = 2324;

		// Token: 0x04002E62 RID: 11874
		public const short BuilderPotion = 2325;

		// Token: 0x04002E63 RID: 11875
		public const short TitanPotion = 2326;

		// Token: 0x04002E64 RID: 11876
		public const short FlipperPotion = 2327;

		// Token: 0x04002E65 RID: 11877
		public const short SummoningPotion = 2328;

		// Token: 0x04002E66 RID: 11878
		public const short TrapsightPotion = 2329;

		// Token: 0x04002E67 RID: 11879
		public const short PurpleClubberfish = 2330;

		// Token: 0x04002E68 RID: 11880
		public const short ObsidianSwordfish = 2331;

		// Token: 0x04002E69 RID: 11881
		public const short Swordfish = 2332;

		// Token: 0x04002E6A RID: 11882
		public const short IronFence = 2333;

		// Token: 0x04002E6B RID: 11883
		public const short WoodenCrate = 2334;

		// Token: 0x04002E6C RID: 11884
		public const short IronCrate = 2335;

		// Token: 0x04002E6D RID: 11885
		public const short GoldenCrate = 2336;

		// Token: 0x04002E6E RID: 11886
		public const short OldShoe = 2337;

		// Token: 0x04002E6F RID: 11887
		public const short FishingSeaweed = 2338;

		// Token: 0x04002E70 RID: 11888
		public const short TinCan = 2339;

		// Token: 0x04002E71 RID: 11889
		public const short MinecartTrack = 2340;

		// Token: 0x04002E72 RID: 11890
		public const short ReaverShark = 2341;

		// Token: 0x04002E73 RID: 11891
		public const short SawtoothShark = 2342;

		// Token: 0x04002E74 RID: 11892
		public const short Minecart = 2343;

		// Token: 0x04002E75 RID: 11893
		public const short AmmoReservationPotion = 2344;

		// Token: 0x04002E76 RID: 11894
		public const short LifeforcePotion = 2345;

		// Token: 0x04002E77 RID: 11895
		public const short EndurancePotion = 2346;

		// Token: 0x04002E78 RID: 11896
		public const short RagePotion = 2347;

		// Token: 0x04002E79 RID: 11897
		public const short InfernoPotion = 2348;

		// Token: 0x04002E7A RID: 11898
		public const short WrathPotion = 2349;

		// Token: 0x04002E7B RID: 11899
		public const short RecallPotion = 2350;

		// Token: 0x04002E7C RID: 11900
		public const short TeleportationPotion = 2351;

		// Token: 0x04002E7D RID: 11901
		public const short LovePotion = 2352;

		// Token: 0x04002E7E RID: 11902
		public const short StinkPotion = 2353;

		// Token: 0x04002E7F RID: 11903
		public const short FishingPotion = 2354;

		// Token: 0x04002E80 RID: 11904
		public const short SonarPotion = 2355;

		// Token: 0x04002E81 RID: 11905
		public const short CratePotion = 2356;

		// Token: 0x04002E82 RID: 11906
		public const short ShiverthornSeeds = 2357;

		// Token: 0x04002E83 RID: 11907
		public const short Shiverthorn = 2358;

		// Token: 0x04002E84 RID: 11908
		public const short WarmthPotion = 2359;

		// Token: 0x04002E85 RID: 11909
		public const short FishHook = 2360;

		// Token: 0x04002E86 RID: 11910
		public const short BeeHeadgear = 2361;

		// Token: 0x04002E87 RID: 11911
		public const short BeeBreastplate = 2362;

		// Token: 0x04002E88 RID: 11912
		public const short BeeGreaves = 2363;

		// Token: 0x04002E89 RID: 11913
		public const short HornetStaff = 2364;

		// Token: 0x04002E8A RID: 11914
		public const short ImpStaff = 2365;

		// Token: 0x04002E8B RID: 11915
		public const short QueenSpiderStaff = 2366;

		// Token: 0x04002E8C RID: 11916
		public const short AnglerHat = 2367;

		// Token: 0x04002E8D RID: 11917
		public const short AnglerVest = 2368;

		// Token: 0x04002E8E RID: 11918
		public const short AnglerPants = 2369;

		// Token: 0x04002E8F RID: 11919
		public const short SpiderMask = 2370;

		// Token: 0x04002E90 RID: 11920
		public const short SpiderBreastplate = 2371;

		// Token: 0x04002E91 RID: 11921
		public const short SpiderGreaves = 2372;

		// Token: 0x04002E92 RID: 11922
		public const short HighTestFishingLine = 2373;

		// Token: 0x04002E93 RID: 11923
		public const short AnglerEarring = 2374;

		// Token: 0x04002E94 RID: 11924
		public const short TackleBox = 2375;

		// Token: 0x04002E95 RID: 11925
		public const short BlueDungeonPiano = 2376;

		// Token: 0x04002E96 RID: 11926
		public const short GreenDungeonPiano = 2377;

		// Token: 0x04002E97 RID: 11927
		public const short PinkDungeonPiano = 2378;

		// Token: 0x04002E98 RID: 11928
		public const short GoldenPiano = 2379;

		// Token: 0x04002E99 RID: 11929
		public const short ObsidianPiano = 2380;

		// Token: 0x04002E9A RID: 11930
		public const short BonePiano = 2381;

		// Token: 0x04002E9B RID: 11931
		public const short CactusPiano = 2382;

		// Token: 0x04002E9C RID: 11932
		public const short SpookyPiano = 2383;

		// Token: 0x04002E9D RID: 11933
		public const short SkywarePiano = 2384;

		// Token: 0x04002E9E RID: 11934
		public const short LihzahrdPiano = 2385;

		// Token: 0x04002E9F RID: 11935
		public const short BlueDungeonDresser = 2386;

		// Token: 0x04002EA0 RID: 11936
		public const short GreenDungeonDresser = 2387;

		// Token: 0x04002EA1 RID: 11937
		public const short PinkDungeonDresser = 2388;

		// Token: 0x04002EA2 RID: 11938
		public const short GoldenDresser = 2389;

		// Token: 0x04002EA3 RID: 11939
		public const short ObsidianDresser = 2390;

		// Token: 0x04002EA4 RID: 11940
		public const short BoneDresser = 2391;

		// Token: 0x04002EA5 RID: 11941
		public const short CactusDresser = 2392;

		// Token: 0x04002EA6 RID: 11942
		public const short SpookyDresser = 2393;

		// Token: 0x04002EA7 RID: 11943
		public const short SkywareDresser = 2394;

		// Token: 0x04002EA8 RID: 11944
		public const short HoneyDresser = 2395;

		// Token: 0x04002EA9 RID: 11945
		public const short LihzahrdDresser = 2396;

		// Token: 0x04002EAA RID: 11946
		public const short Sofa = 2397;

		// Token: 0x04002EAB RID: 11947
		public const short EbonwoodSofa = 2398;

		// Token: 0x04002EAC RID: 11948
		public const short RichMahoganySofa = 2399;

		// Token: 0x04002EAD RID: 11949
		public const short PearlwoodSofa = 2400;

		// Token: 0x04002EAE RID: 11950
		public const short ShadewoodSofa = 2401;

		// Token: 0x04002EAF RID: 11951
		public const short BlueDungeonSofa = 2402;

		// Token: 0x04002EB0 RID: 11952
		public const short GreenDungeonSofa = 2403;

		// Token: 0x04002EB1 RID: 11953
		public const short PinkDungeonSofa = 2404;

		// Token: 0x04002EB2 RID: 11954
		public const short GoldenSofa = 2405;

		// Token: 0x04002EB3 RID: 11955
		public const short ObsidianSofa = 2406;

		// Token: 0x04002EB4 RID: 11956
		public const short BoneSofa = 2407;

		// Token: 0x04002EB5 RID: 11957
		public const short CactusSofa = 2408;

		// Token: 0x04002EB6 RID: 11958
		public const short SpookySofa = 2409;

		// Token: 0x04002EB7 RID: 11959
		public const short SkywareSofa = 2410;

		// Token: 0x04002EB8 RID: 11960
		public const short HoneySofa = 2411;

		// Token: 0x04002EB9 RID: 11961
		public const short SteampunkSofa = 2412;

		// Token: 0x04002EBA RID: 11962
		public const short MushroomSofa = 2413;

		// Token: 0x04002EBB RID: 11963
		public const short GlassSofa = 2414;

		// Token: 0x04002EBC RID: 11964
		public const short PumpkinSofa = 2415;

		// Token: 0x04002EBD RID: 11965
		public const short LihzahrdSofa = 2416;

		// Token: 0x04002EBE RID: 11966
		public const short SeashellHairpin = 2417;

		// Token: 0x04002EBF RID: 11967
		public const short MermaidAdornment = 2418;

		// Token: 0x04002EC0 RID: 11968
		public const short MermaidTail = 2419;

		// Token: 0x04002EC1 RID: 11969
		public const short ZephyrFish = 2420;

		// Token: 0x04002EC2 RID: 11970
		public const short Fleshcatcher = 2421;

		// Token: 0x04002EC3 RID: 11971
		public const short HotlineFishingHook = 2422;

		// Token: 0x04002EC4 RID: 11972
		public const short FrogLeg = 2423;

		// Token: 0x04002EC5 RID: 11973
		public const short Anchor = 2424;

		// Token: 0x04002EC6 RID: 11974
		public const short CookedFish = 2425;

		// Token: 0x04002EC7 RID: 11975
		public const short CookedShrimp = 2426;

		// Token: 0x04002EC8 RID: 11976
		public const short Sashimi = 2427;

		// Token: 0x04002EC9 RID: 11977
		public const short FuzzyCarrot = 2428;

		// Token: 0x04002ECA RID: 11978
		public const short ScalyTruffle = 2429;

		// Token: 0x04002ECB RID: 11979
		public const short SlimySaddle = 2430;

		// Token: 0x04002ECC RID: 11980
		public const short BeeWax = 2431;

		// Token: 0x04002ECD RID: 11981
		public const short CopperPlatingWall = 2432;

		// Token: 0x04002ECE RID: 11982
		public const short StoneSlabWall = 2433;

		// Token: 0x04002ECF RID: 11983
		public const short Sail = 2434;

		// Token: 0x04002ED0 RID: 11984
		public const short CoralstoneBlock = 2435;

		// Token: 0x04002ED1 RID: 11985
		public const short BlueJellyfish = 2436;

		// Token: 0x04002ED2 RID: 11986
		public const short GreenJellyfish = 2437;

		// Token: 0x04002ED3 RID: 11987
		public const short PinkJellyfish = 2438;

		// Token: 0x04002ED4 RID: 11988
		public const short BlueJellyfishJar = 2439;

		// Token: 0x04002ED5 RID: 11989
		public const short GreenJellyfishJar = 2440;

		// Token: 0x04002ED6 RID: 11990
		public const short PinkJellyfishJar = 2441;

		// Token: 0x04002ED7 RID: 11991
		public const short LifePreserver = 2442;

		// Token: 0x04002ED8 RID: 11992
		public const short ShipsWheel = 2443;

		// Token: 0x04002ED9 RID: 11993
		public const short CompassRose = 2444;

		// Token: 0x04002EDA RID: 11994
		public const short WallAnchor = 2445;

		// Token: 0x04002EDB RID: 11995
		public const short GoldfishTrophy = 2446;

		// Token: 0x04002EDC RID: 11996
		public const short BunnyfishTrophy = 2447;

		// Token: 0x04002EDD RID: 11997
		public const short SwordfishTrophy = 2448;

		// Token: 0x04002EDE RID: 11998
		public const short SharkteethTrophy = 2449;

		// Token: 0x04002EDF RID: 11999
		public const short Batfish = 2450;

		// Token: 0x04002EE0 RID: 12000
		public const short BumblebeeTuna = 2451;

		// Token: 0x04002EE1 RID: 12001
		public const short Catfish = 2452;

		// Token: 0x04002EE2 RID: 12002
		public const short Cloudfish = 2453;

		// Token: 0x04002EE3 RID: 12003
		public const short Cursedfish = 2454;

		// Token: 0x04002EE4 RID: 12004
		public const short Dirtfish = 2455;

		// Token: 0x04002EE5 RID: 12005
		public const short DynamiteFish = 2456;

		// Token: 0x04002EE6 RID: 12006
		public const short EaterofPlankton = 2457;

		// Token: 0x04002EE7 RID: 12007
		public const short FallenStarfish = 2458;

		// Token: 0x04002EE8 RID: 12008
		public const short TheFishofCthulu = 2459;

		// Token: 0x04002EE9 RID: 12009
		public const short Fishotron = 2460;

		// Token: 0x04002EEA RID: 12010
		public const short Harpyfish = 2461;

		// Token: 0x04002EEB RID: 12011
		public const short Hungerfish = 2462;

		// Token: 0x04002EEC RID: 12012
		public const short Ichorfish = 2463;

		// Token: 0x04002EED RID: 12013
		public const short Jewelfish = 2464;

		// Token: 0x04002EEE RID: 12014
		public const short MirageFish = 2465;

		// Token: 0x04002EEF RID: 12015
		public const short MutantFlinxfin = 2466;

		// Token: 0x04002EF0 RID: 12016
		public const short Pengfish = 2467;

		// Token: 0x04002EF1 RID: 12017
		public const short Pixiefish = 2468;

		// Token: 0x04002EF2 RID: 12018
		public const short Spiderfish = 2469;

		// Token: 0x04002EF3 RID: 12019
		public const short TundraTrout = 2470;

		// Token: 0x04002EF4 RID: 12020
		public const short UnicornFish = 2471;

		// Token: 0x04002EF5 RID: 12021
		public const short GuideVoodooFish = 2472;

		// Token: 0x04002EF6 RID: 12022
		public const short Wyverntail = 2473;

		// Token: 0x04002EF7 RID: 12023
		public const short ZombieFish = 2474;

		// Token: 0x04002EF8 RID: 12024
		public const short AmanitaFungifin = 2475;

		// Token: 0x04002EF9 RID: 12025
		public const short Angelfish = 2476;

		// Token: 0x04002EFA RID: 12026
		public const short BloodyManowar = 2477;

		// Token: 0x04002EFB RID: 12027
		public const short Bonefish = 2478;

		// Token: 0x04002EFC RID: 12028
		public const short Bunnyfish = 2479;

		// Token: 0x04002EFD RID: 12029
		public const short CapnTunabeard = 2480;

		// Token: 0x04002EFE RID: 12030
		public const short Clownfish = 2481;

		// Token: 0x04002EFF RID: 12031
		public const short DemonicHellfish = 2482;

		// Token: 0x04002F00 RID: 12032
		public const short Derpfish = 2483;

		// Token: 0x04002F01 RID: 12033
		public const short Fishron = 2484;

		// Token: 0x04002F02 RID: 12034
		public const short InfectedScabbardfish = 2485;

		// Token: 0x04002F03 RID: 12035
		public const short Mudfish = 2486;

		// Token: 0x04002F04 RID: 12036
		public const short Slimefish = 2487;

		// Token: 0x04002F05 RID: 12037
		public const short TropicalBarracuda = 2488;

		// Token: 0x04002F06 RID: 12038
		public const short KingSlimeTrophy = 2489;

		// Token: 0x04002F07 RID: 12039
		public const short ShipInABottle = 2490;

		// Token: 0x04002F08 RID: 12040
		public const short HardySaddle = 2491;

		// Token: 0x04002F09 RID: 12041
		public const short PressureTrack = 2492;

		// Token: 0x04002F0A RID: 12042
		public const short KingSlimeMask = 2493;

		// Token: 0x04002F0B RID: 12043
		public const short FinWings = 2494;

		// Token: 0x04002F0C RID: 12044
		public const short TreasureMap = 2495;

		// Token: 0x04002F0D RID: 12045
		public const short SeaweedPlanter = 2496;

		// Token: 0x04002F0E RID: 12046
		public const short PillaginMePixels = 2497;

		// Token: 0x04002F0F RID: 12047
		public const short FishCostumeMask = 2498;

		// Token: 0x04002F10 RID: 12048
		public const short FishCostumeShirt = 2499;

		// Token: 0x04002F11 RID: 12049
		public const short FishCostumeFinskirt = 2500;

		// Token: 0x04002F12 RID: 12050
		public const short GingerBeard = 2501;

		// Token: 0x04002F13 RID: 12051
		public const short HoneyedGoggles = 2502;

		// Token: 0x04002F14 RID: 12052
		public const short BorealWood = 2503;

		// Token: 0x04002F15 RID: 12053
		public const short PalmWood = 2504;

		// Token: 0x04002F16 RID: 12054
		public const short BorealWoodWall = 2505;

		// Token: 0x04002F17 RID: 12055
		public const short PalmWoodWall = 2506;

		// Token: 0x04002F18 RID: 12056
		public const short BorealWoodFence = 2507;

		// Token: 0x04002F19 RID: 12057
		public const short PalmWoodFence = 2508;

		// Token: 0x04002F1A RID: 12058
		public const short BorealWoodHelmet = 2509;

		// Token: 0x04002F1B RID: 12059
		public const short BorealWoodBreastplate = 2510;

		// Token: 0x04002F1C RID: 12060
		public const short BorealWoodGreaves = 2511;

		// Token: 0x04002F1D RID: 12061
		public const short PalmWoodHelmet = 2512;

		// Token: 0x04002F1E RID: 12062
		public const short PalmWoodBreastplate = 2513;

		// Token: 0x04002F1F RID: 12063
		public const short PalmWoodGreaves = 2514;

		// Token: 0x04002F20 RID: 12064
		public const short PalmWoodBow = 2515;

		// Token: 0x04002F21 RID: 12065
		public const short PalmWoodHammer = 2516;

		// Token: 0x04002F22 RID: 12066
		public const short PalmWoodSword = 2517;

		// Token: 0x04002F23 RID: 12067
		public const short PalmWoodPlatform = 2518;

		// Token: 0x04002F24 RID: 12068
		public const short PalmWoodBathtub = 2519;

		// Token: 0x04002F25 RID: 12069
		public const short PalmWoodBed = 2520;

		// Token: 0x04002F26 RID: 12070
		public const short PalmWoodBench = 2521;

		// Token: 0x04002F27 RID: 12071
		public const short PalmWoodCandelabra = 2522;

		// Token: 0x04002F28 RID: 12072
		public const short PalmWoodCandle = 2523;

		// Token: 0x04002F29 RID: 12073
		public const short PalmWoodChair = 2524;

		// Token: 0x04002F2A RID: 12074
		public const short PalmWoodChandelier = 2525;

		// Token: 0x04002F2B RID: 12075
		public const short PalmWoodChest = 2526;

		// Token: 0x04002F2C RID: 12076
		public const short PalmWoodSofa = 2527;

		// Token: 0x04002F2D RID: 12077
		public const short PalmWoodDoor = 2528;

		// Token: 0x04002F2E RID: 12078
		public const short PalmWoodDresser = 2529;

		// Token: 0x04002F2F RID: 12079
		public const short PalmWoodLantern = 2530;

		// Token: 0x04002F30 RID: 12080
		public const short PalmWoodPiano = 2531;

		// Token: 0x04002F31 RID: 12081
		public const short PalmWoodTable = 2532;

		// Token: 0x04002F32 RID: 12082
		public const short PalmWoodLamp = 2533;

		// Token: 0x04002F33 RID: 12083
		public const short PalmWoodWorkBench = 2534;

		// Token: 0x04002F34 RID: 12084
		public const short OpticStaff = 2535;

		// Token: 0x04002F35 RID: 12085
		public const short PalmWoodBookcase = 2536;

		// Token: 0x04002F36 RID: 12086
		public const short MushroomBathtub = 2537;

		// Token: 0x04002F37 RID: 12087
		public const short MushroomBed = 2538;

		// Token: 0x04002F38 RID: 12088
		public const short MushroomBench = 2539;

		// Token: 0x04002F39 RID: 12089
		public const short MushroomBookcase = 2540;

		// Token: 0x04002F3A RID: 12090
		public const short MushroomCandelabra = 2541;

		// Token: 0x04002F3B RID: 12091
		public const short MushroomCandle = 2542;

		// Token: 0x04002F3C RID: 12092
		public const short MushroomChandelier = 2543;

		// Token: 0x04002F3D RID: 12093
		public const short MushroomChest = 2544;

		// Token: 0x04002F3E RID: 12094
		public const short MushroomDresser = 2545;

		// Token: 0x04002F3F RID: 12095
		public const short MushroomLantern = 2546;

		// Token: 0x04002F40 RID: 12096
		public const short MushroomLamp = 2547;

		// Token: 0x04002F41 RID: 12097
		public const short MushroomPiano = 2548;

		// Token: 0x04002F42 RID: 12098
		public const short MushroomPlatform = 2549;

		// Token: 0x04002F43 RID: 12099
		public const short MushroomTable = 2550;

		// Token: 0x04002F44 RID: 12100
		public const short SpiderStaff = 2551;

		// Token: 0x04002F45 RID: 12101
		public const short BorealWoodBathtub = 2552;

		// Token: 0x04002F46 RID: 12102
		public const short BorealWoodBed = 2553;

		// Token: 0x04002F47 RID: 12103
		public const short BorealWoodBookcase = 2554;

		// Token: 0x04002F48 RID: 12104
		public const short BorealWoodCandelabra = 2555;

		// Token: 0x04002F49 RID: 12105
		public const short BorealWoodCandle = 2556;

		// Token: 0x04002F4A RID: 12106
		public const short BorealWoodChair = 2557;

		// Token: 0x04002F4B RID: 12107
		public const short BorealWoodChandelier = 2558;

		// Token: 0x04002F4C RID: 12108
		public const short BorealWoodChest = 2559;

		// Token: 0x04002F4D RID: 12109
		public const short BorealWoodClock = 2560;

		// Token: 0x04002F4E RID: 12110
		public const short BorealWoodDoor = 2561;

		// Token: 0x04002F4F RID: 12111
		public const short BorealWoodDresser = 2562;

		// Token: 0x04002F50 RID: 12112
		public const short BorealWoodLamp = 2563;

		// Token: 0x04002F51 RID: 12113
		public const short BorealWoodLantern = 2564;

		// Token: 0x04002F52 RID: 12114
		public const short BorealWoodPiano = 2565;

		// Token: 0x04002F53 RID: 12115
		public const short BorealWoodPlatform = 2566;

		// Token: 0x04002F54 RID: 12116
		public const short SlimeBathtub = 2567;

		// Token: 0x04002F55 RID: 12117
		public const short SlimeBed = 2568;

		// Token: 0x04002F56 RID: 12118
		public const short SlimeBookcase = 2569;

		// Token: 0x04002F57 RID: 12119
		public const short SlimeCandelabra = 2570;

		// Token: 0x04002F58 RID: 12120
		public const short SlimeCandle = 2571;

		// Token: 0x04002F59 RID: 12121
		public const short SlimeChair = 2572;

		// Token: 0x04002F5A RID: 12122
		public const short SlimeChandelier = 2573;

		// Token: 0x04002F5B RID: 12123
		public const short SlimeChest = 2574;

		// Token: 0x04002F5C RID: 12124
		public const short SlimeClock = 2575;

		// Token: 0x04002F5D RID: 12125
		public const short SlimeDoor = 2576;

		// Token: 0x04002F5E RID: 12126
		public const short SlimeDresser = 2577;

		// Token: 0x04002F5F RID: 12127
		public const short SlimeLamp = 2578;

		// Token: 0x04002F60 RID: 12128
		public const short SlimeLantern = 2579;

		// Token: 0x04002F61 RID: 12129
		public const short SlimePiano = 2580;

		// Token: 0x04002F62 RID: 12130
		public const short SlimePlatform = 2581;

		// Token: 0x04002F63 RID: 12131
		public const short SlimeSofa = 2582;

		// Token: 0x04002F64 RID: 12132
		public const short SlimeTable = 2583;

		// Token: 0x04002F65 RID: 12133
		public const short PirateStaff = 2584;

		// Token: 0x04002F66 RID: 12134
		public const short SlimeHook = 2585;

		// Token: 0x04002F67 RID: 12135
		public const short StickyGrenade = 2586;

		// Token: 0x04002F68 RID: 12136
		public const short TartarSauce = 2587;

		// Token: 0x04002F69 RID: 12137
		public const short DukeFishronMask = 2588;

		// Token: 0x04002F6A RID: 12138
		public const short DukeFishronTrophy = 2589;

		// Token: 0x04002F6B RID: 12139
		public const short MolotovCocktail = 2590;

		// Token: 0x04002F6C RID: 12140
		public const short BoneClock = 2591;

		// Token: 0x04002F6D RID: 12141
		public const short CactusClock = 2592;

		// Token: 0x04002F6E RID: 12142
		public const short EbonwoodClock = 2593;

		// Token: 0x04002F6F RID: 12143
		public const short FrozenClock = 2594;

		// Token: 0x04002F70 RID: 12144
		public const short LihzahrdClock = 2595;

		// Token: 0x04002F71 RID: 12145
		public const short LivingWoodClock = 2596;

		// Token: 0x04002F72 RID: 12146
		public const short RichMahoganyClock = 2597;

		// Token: 0x04002F73 RID: 12147
		public const short FleshClock = 2598;

		// Token: 0x04002F74 RID: 12148
		public const short MushroomClock = 2599;

		// Token: 0x04002F75 RID: 12149
		public const short ObsidianClock = 2600;

		// Token: 0x04002F76 RID: 12150
		public const short PalmWoodClock = 2601;

		// Token: 0x04002F77 RID: 12151
		public const short PearlwoodClock = 2602;

		// Token: 0x04002F78 RID: 12152
		public const short PumpkinClock = 2603;

		// Token: 0x04002F79 RID: 12153
		public const short ShadewoodClock = 2604;

		// Token: 0x04002F7A RID: 12154
		public const short SpookyClock = 2605;

		// Token: 0x04002F7B RID: 12155
		public const short SkywareClock = 2606;

		// Token: 0x04002F7C RID: 12156
		public const short SpiderFang = 2607;

		// Token: 0x04002F7D RID: 12157
		public const short FalconBlade = 2608;

		// Token: 0x04002F7E RID: 12158
		public const short FishronWings = 2609;

		// Token: 0x04002F7F RID: 12159
		public const short SlimeGun = 2610;

		// Token: 0x04002F80 RID: 12160
		public const short Flairon = 2611;

		// Token: 0x04002F81 RID: 12161
		public const short GreenDungeonChest = 2612;

		// Token: 0x04002F82 RID: 12162
		public const short PinkDungeonChest = 2613;

		// Token: 0x04002F83 RID: 12163
		public const short BlueDungeonChest = 2614;

		// Token: 0x04002F84 RID: 12164
		public const short BoneChest = 2615;

		// Token: 0x04002F85 RID: 12165
		public const short CactusChest = 2616;

		// Token: 0x04002F86 RID: 12166
		public const short FleshChest = 2617;

		// Token: 0x04002F87 RID: 12167
		public const short ObsidianChest = 2618;

		// Token: 0x04002F88 RID: 12168
		public const short PumpkinChest = 2619;

		// Token: 0x04002F89 RID: 12169
		public const short SpookyChest = 2620;

		// Token: 0x04002F8A RID: 12170
		public const short TempestStaff = 2621;

		// Token: 0x04002F8B RID: 12171
		public const short RazorbladeTyphoon = 2622;

		// Token: 0x04002F8C RID: 12172
		public const short BubbleGun = 2623;

		// Token: 0x04002F8D RID: 12173
		public const short Tsunami = 2624;

		// Token: 0x04002F8E RID: 12174
		public const short Seashell = 2625;

		// Token: 0x04002F8F RID: 12175
		public const short Starfish = 2626;

		// Token: 0x04002F90 RID: 12176
		public const short SteampunkPlatform = 2627;

		// Token: 0x04002F91 RID: 12177
		public const short SkywarePlatform = 2628;

		// Token: 0x04002F92 RID: 12178
		public const short LivingWoodPlatform = 2629;

		// Token: 0x04002F93 RID: 12179
		public const short HoneyPlatform = 2630;

		// Token: 0x04002F94 RID: 12180
		public const short SkywareWorkbench = 2631;

		// Token: 0x04002F95 RID: 12181
		public const short GlassWorkBench = 2632;

		// Token: 0x04002F96 RID: 12182
		public const short LivingWoodWorkBench = 2633;

		// Token: 0x04002F97 RID: 12183
		public const short FleshSofa = 2634;

		// Token: 0x04002F98 RID: 12184
		public const short FrozenSofa = 2635;

		// Token: 0x04002F99 RID: 12185
		public const short LivingWoodSofa = 2636;

		// Token: 0x04002F9A RID: 12186
		public const short PumpkinDresser = 2637;

		// Token: 0x04002F9B RID: 12187
		public const short SteampunkDresser = 2638;

		// Token: 0x04002F9C RID: 12188
		public const short GlassDresser = 2639;

		// Token: 0x04002F9D RID: 12189
		public const short FleshDresser = 2640;

		// Token: 0x04002F9E RID: 12190
		public const short PumpkinLantern = 2641;

		// Token: 0x04002F9F RID: 12191
		public const short ObsidianLantern = 2642;

		// Token: 0x04002FA0 RID: 12192
		public const short PumpkinLamp = 2643;

		// Token: 0x04002FA1 RID: 12193
		public const short ObsidianLamp = 2644;

		// Token: 0x04002FA2 RID: 12194
		public const short BlueDungeonLamp = 2645;

		// Token: 0x04002FA3 RID: 12195
		public const short GreenDungeonLamp = 2646;

		// Token: 0x04002FA4 RID: 12196
		public const short PinkDungeonLamp = 2647;

		// Token: 0x04002FA5 RID: 12197
		public const short HoneyCandle = 2648;

		// Token: 0x04002FA6 RID: 12198
		public const short SteampunkCandle = 2649;

		// Token: 0x04002FA7 RID: 12199
		public const short SpookyCandle = 2650;

		// Token: 0x04002FA8 RID: 12200
		public const short ObsidianCandle = 2651;

		// Token: 0x04002FA9 RID: 12201
		public const short BlueDungeonChandelier = 2652;

		// Token: 0x04002FAA RID: 12202
		public const short GreenDungeonChandelier = 2653;

		// Token: 0x04002FAB RID: 12203
		public const short PinkDungeonChandelier = 2654;

		// Token: 0x04002FAC RID: 12204
		public const short SteampunkChandelier = 2655;

		// Token: 0x04002FAD RID: 12205
		public const short PumpkinChandelier = 2656;

		// Token: 0x04002FAE RID: 12206
		public const short ObsidianChandelier = 2657;

		// Token: 0x04002FAF RID: 12207
		public const short BlueDungeonBathtub = 2658;

		// Token: 0x04002FB0 RID: 12208
		public const short GreenDungeonBathtub = 2659;

		// Token: 0x04002FB1 RID: 12209
		public const short PinkDungeonBathtub = 2660;

		// Token: 0x04002FB2 RID: 12210
		public const short PumpkinBathtub = 2661;

		// Token: 0x04002FB3 RID: 12211
		public const short ObsidianBathtub = 2662;

		// Token: 0x04002FB4 RID: 12212
		public const short GoldenBathtub = 2663;

		// Token: 0x04002FB5 RID: 12213
		public const short BlueDungeonCandelabra = 2664;

		// Token: 0x04002FB6 RID: 12214
		public const short GreenDungeonCandelabra = 2665;

		// Token: 0x04002FB7 RID: 12215
		public const short PinkDungeonCandelabra = 2666;

		// Token: 0x04002FB8 RID: 12216
		public const short ObsidianCandelabra = 2667;

		// Token: 0x04002FB9 RID: 12217
		public const short PumpkinCandelabra = 2668;

		// Token: 0x04002FBA RID: 12218
		public const short PumpkinBed = 2669;

		// Token: 0x04002FBB RID: 12219
		public const short PumpkinBookcase = 2670;

		// Token: 0x04002FBC RID: 12220
		public const short PumpkinPiano = 2671;

		// Token: 0x04002FBD RID: 12221
		public const short SharkStatue = 2672;

		// Token: 0x04002FBE RID: 12222
		public const short TruffleWorm = 2673;

		// Token: 0x04002FBF RID: 12223
		public const short ApprenticeBait = 2674;

		// Token: 0x04002FC0 RID: 12224
		public const short JourneymanBait = 2675;

		// Token: 0x04002FC1 RID: 12225
		public const short MasterBait = 2676;

		// Token: 0x04002FC2 RID: 12226
		public const short AmberGemsparkWall = 2677;

		// Token: 0x04002FC3 RID: 12227
		public const short AmberGemsparkWallOff = 2678;

		// Token: 0x04002FC4 RID: 12228
		public const short AmethystGemsparkWall = 2679;

		// Token: 0x04002FC5 RID: 12229
		public const short AmethystGemsparkWallOff = 2680;

		// Token: 0x04002FC6 RID: 12230
		public const short DiamondGemsparkWall = 2681;

		// Token: 0x04002FC7 RID: 12231
		public const short DiamondGemsparkWallOff = 2682;

		// Token: 0x04002FC8 RID: 12232
		public const short EmeraldGemsparkWall = 2683;

		// Token: 0x04002FC9 RID: 12233
		public const short EmeraldGemsparkWallOff = 2684;

		// Token: 0x04002FCA RID: 12234
		public const short RubyGemsparkWall = 2685;

		// Token: 0x04002FCB RID: 12235
		public const short RubyGemsparkWallOff = 2686;

		// Token: 0x04002FCC RID: 12236
		public const short SapphireGemsparkWall = 2687;

		// Token: 0x04002FCD RID: 12237
		public const short SapphireGemsparkWallOff = 2688;

		// Token: 0x04002FCE RID: 12238
		public const short TopazGemsparkWall = 2689;

		// Token: 0x04002FCF RID: 12239
		public const short TopazGemsparkWallOff = 2690;

		// Token: 0x04002FD0 RID: 12240
		public const short TinPlatingWall = 2691;

		// Token: 0x04002FD1 RID: 12241
		public const short TinPlating = 2692;

		// Token: 0x04002FD2 RID: 12242
		public const short WaterfallBlock = 2693;

		// Token: 0x04002FD3 RID: 12243
		public const short LavafallBlock = 2694;

		// Token: 0x04002FD4 RID: 12244
		public const short ConfettiBlock = 2695;

		// Token: 0x04002FD5 RID: 12245
		public const short ConfettiWall = 2696;

		// Token: 0x04002FD6 RID: 12246
		public const short ConfettiBlockBlack = 2697;

		// Token: 0x04002FD7 RID: 12247
		public const short ConfettiWallBlack = 2698;

		// Token: 0x04002FD8 RID: 12248
		public const short WeaponRack = 2699;

		// Token: 0x04002FD9 RID: 12249
		public const short FireworksBox = 2700;

		// Token: 0x04002FDA RID: 12250
		public const short LivingFireBlock = 2701;

		// Token: 0x04002FDB RID: 12251
		public const short AlphabetStatue0 = 2702;

		// Token: 0x04002FDC RID: 12252
		public const short AlphabetStatue1 = 2703;

		// Token: 0x04002FDD RID: 12253
		public const short AlphabetStatue2 = 2704;

		// Token: 0x04002FDE RID: 12254
		public const short AlphabetStatue3 = 2705;

		// Token: 0x04002FDF RID: 12255
		public const short AlphabetStatue4 = 2706;

		// Token: 0x04002FE0 RID: 12256
		public const short AlphabetStatue5 = 2707;

		// Token: 0x04002FE1 RID: 12257
		public const short AlphabetStatue6 = 2708;

		// Token: 0x04002FE2 RID: 12258
		public const short AlphabetStatue7 = 2709;

		// Token: 0x04002FE3 RID: 12259
		public const short AlphabetStatue8 = 2710;

		// Token: 0x04002FE4 RID: 12260
		public const short AlphabetStatue9 = 2711;

		// Token: 0x04002FE5 RID: 12261
		public const short AlphabetStatueA = 2712;

		// Token: 0x04002FE6 RID: 12262
		public const short AlphabetStatueB = 2713;

		// Token: 0x04002FE7 RID: 12263
		public const short AlphabetStatueC = 2714;

		// Token: 0x04002FE8 RID: 12264
		public const short AlphabetStatueD = 2715;

		// Token: 0x04002FE9 RID: 12265
		public const short AlphabetStatueE = 2716;

		// Token: 0x04002FEA RID: 12266
		public const short AlphabetStatueF = 2717;

		// Token: 0x04002FEB RID: 12267
		public const short AlphabetStatueG = 2718;

		// Token: 0x04002FEC RID: 12268
		public const short AlphabetStatueH = 2719;

		// Token: 0x04002FED RID: 12269
		public const short AlphabetStatueI = 2720;

		// Token: 0x04002FEE RID: 12270
		public const short AlphabetStatueJ = 2721;

		// Token: 0x04002FEF RID: 12271
		public const short AlphabetStatueK = 2722;

		// Token: 0x04002FF0 RID: 12272
		public const short AlphabetStatueL = 2723;

		// Token: 0x04002FF1 RID: 12273
		public const short AlphabetStatueM = 2724;

		// Token: 0x04002FF2 RID: 12274
		public const short AlphabetStatueN = 2725;

		// Token: 0x04002FF3 RID: 12275
		public const short AlphabetStatueO = 2726;

		// Token: 0x04002FF4 RID: 12276
		public const short AlphabetStatueP = 2727;

		// Token: 0x04002FF5 RID: 12277
		public const short AlphabetStatueQ = 2728;

		// Token: 0x04002FF6 RID: 12278
		public const short AlphabetStatueR = 2729;

		// Token: 0x04002FF7 RID: 12279
		public const short AlphabetStatueS = 2730;

		// Token: 0x04002FF8 RID: 12280
		public const short AlphabetStatueT = 2731;

		// Token: 0x04002FF9 RID: 12281
		public const short AlphabetStatueU = 2732;

		// Token: 0x04002FFA RID: 12282
		public const short AlphabetStatueV = 2733;

		// Token: 0x04002FFB RID: 12283
		public const short AlphabetStatueW = 2734;

		// Token: 0x04002FFC RID: 12284
		public const short AlphabetStatueX = 2735;

		// Token: 0x04002FFD RID: 12285
		public const short AlphabetStatueY = 2736;

		// Token: 0x04002FFE RID: 12286
		public const short AlphabetStatueZ = 2737;

		// Token: 0x04002FFF RID: 12287
		public const short FireworkFountain = 2738;

		// Token: 0x04003000 RID: 12288
		public const short BoosterTrack = 2739;

		// Token: 0x04003001 RID: 12289
		public const short Grasshopper = 2740;

		// Token: 0x04003002 RID: 12290
		public const short GrasshopperCage = 2741;

		// Token: 0x04003003 RID: 12291
		public const short MusicBoxUndergroundCrimson = 2742;

		// Token: 0x04003004 RID: 12292
		public const short CactusTable = 2743;

		// Token: 0x04003005 RID: 12293
		public const short CactusPlatform = 2744;

		// Token: 0x04003006 RID: 12294
		public const short BorealWoodSword = 2745;

		// Token: 0x04003007 RID: 12295
		public const short BorealWoodHammer = 2746;

		// Token: 0x04003008 RID: 12296
		public const short BorealWoodBow = 2747;

		// Token: 0x04003009 RID: 12297
		public const short GlassChest = 2748;

		// Token: 0x0400300A RID: 12298
		public const short XenoStaff = 2749;

		// Token: 0x0400300B RID: 12299
		public const short MeteorStaff = 2750;

		// Token: 0x0400300C RID: 12300
		public const short LivingCursedFireBlock = 2751;

		// Token: 0x0400300D RID: 12301
		public const short LivingDemonFireBlock = 2752;

		// Token: 0x0400300E RID: 12302
		public const short LivingFrostFireBlock = 2753;

		// Token: 0x0400300F RID: 12303
		public const short LivingIchorBlock = 2754;

		// Token: 0x04003010 RID: 12304
		public const short LivingUltrabrightFireBlock = 2755;

		// Token: 0x04003011 RID: 12305
		public const short GenderChangePotion = 2756;

		// Token: 0x04003012 RID: 12306
		public const short VortexHelmet = 2757;

		// Token: 0x04003013 RID: 12307
		public const short VortexBreastplate = 2758;

		// Token: 0x04003014 RID: 12308
		public const short VortexLeggings = 2759;

		// Token: 0x04003015 RID: 12309
		public const short NebulaHelmet = 2760;

		// Token: 0x04003016 RID: 12310
		public const short NebulaBreastplate = 2761;

		// Token: 0x04003017 RID: 12311
		public const short NebulaLeggings = 2762;

		// Token: 0x04003018 RID: 12312
		public const short SolarFlareHelmet = 2763;

		// Token: 0x04003019 RID: 12313
		public const short SolarFlareBreastplate = 2764;

		// Token: 0x0400301A RID: 12314
		public const short SolarFlareLeggings = 2765;

		// Token: 0x0400301B RID: 12315
		public const short LunarTabletFragment = 2766;

		// Token: 0x0400301C RID: 12316
		public const short SolarTablet = 2767;

		// Token: 0x0400301D RID: 12317
		public const short DrillContainmentUnit = 2768;

		// Token: 0x0400301E RID: 12318
		public const short CosmicCarKey = 2769;

		// Token: 0x0400301F RID: 12319
		public const short MothronWings = 2770;

		// Token: 0x04003020 RID: 12320
		public const short BrainScrambler = 2771;

		// Token: 0x04003021 RID: 12321
		public const short VortexAxe = 2772;

		// Token: 0x04003022 RID: 12322
		public const short VortexChainsaw = 2773;

		// Token: 0x04003023 RID: 12323
		public const short VortexDrill = 2774;

		// Token: 0x04003024 RID: 12324
		public const short VortexHammer = 2775;

		// Token: 0x04003025 RID: 12325
		public const short VortexPickaxe = 2776;

		// Token: 0x04003026 RID: 12326
		public const short NebulaAxe = 2777;

		// Token: 0x04003027 RID: 12327
		public const short NebulaChainsaw = 2778;

		// Token: 0x04003028 RID: 12328
		public const short NebulaDrill = 2779;

		// Token: 0x04003029 RID: 12329
		public const short NebulaHammer = 2780;

		// Token: 0x0400302A RID: 12330
		public const short NebulaPickaxe = 2781;

		// Token: 0x0400302B RID: 12331
		public const short SolarFlareAxe = 2782;

		// Token: 0x0400302C RID: 12332
		public const short SolarFlareChainsaw = 2783;

		// Token: 0x0400302D RID: 12333
		public const short SolarFlareDrill = 2784;

		// Token: 0x0400302E RID: 12334
		public const short SolarFlareHammer = 2785;

		// Token: 0x0400302F RID: 12335
		public const short SolarFlarePickaxe = 2786;

		// Token: 0x04003030 RID: 12336
		public const short HoneyfallBlock = 2787;

		// Token: 0x04003031 RID: 12337
		public const short HoneyfallWall = 2788;

		// Token: 0x04003032 RID: 12338
		public const short ChlorophyteBrickWall = 2789;

		// Token: 0x04003033 RID: 12339
		public const short CrimtaneBrickWall = 2790;

		// Token: 0x04003034 RID: 12340
		public const short ShroomitePlatingWall = 2791;

		// Token: 0x04003035 RID: 12341
		public const short ChlorophyteBrick = 2792;

		// Token: 0x04003036 RID: 12342
		public const short CrimtaneBrick = 2793;

		// Token: 0x04003037 RID: 12343
		public const short ShroomitePlating = 2794;

		// Token: 0x04003038 RID: 12344
		public const short LaserMachinegun = 2795;

		// Token: 0x04003039 RID: 12345
		public const short ElectrosphereLauncher = 2796;

		// Token: 0x0400303A RID: 12346
		public const short Xenopopper = 2797;

		// Token: 0x0400303B RID: 12347
		public const short LaserDrill = 2798;

		// Token: 0x0400303C RID: 12348
		public const short LaserRuler = 2799;

		// Token: 0x0400303D RID: 12349
		public const short AntiGravityHook = 2800;

		// Token: 0x0400303E RID: 12350
		public const short MoonMask = 2801;

		// Token: 0x0400303F RID: 12351
		public const short SunMask = 2802;

		// Token: 0x04003040 RID: 12352
		public const short MartianCostumeMask = 2803;

		// Token: 0x04003041 RID: 12353
		public const short MartianCostumeShirt = 2804;

		// Token: 0x04003042 RID: 12354
		public const short MartianCostumePants = 2805;

		// Token: 0x04003043 RID: 12355
		public const short MartianUniformHelmet = 2806;

		// Token: 0x04003044 RID: 12356
		public const short MartianUniformTorso = 2807;

		// Token: 0x04003045 RID: 12357
		public const short MartianUniformPants = 2808;

		// Token: 0x04003046 RID: 12358
		public const short MartianAstroClock = 2809;

		// Token: 0x04003047 RID: 12359
		public const short MartianBathtub = 2810;

		// Token: 0x04003048 RID: 12360
		public const short MartianBed = 2811;

		// Token: 0x04003049 RID: 12361
		public const short MartianHoverChair = 2812;

		// Token: 0x0400304A RID: 12362
		public const short MartianChandelier = 2813;

		// Token: 0x0400304B RID: 12363
		public const short MartianChest = 2814;

		// Token: 0x0400304C RID: 12364
		public const short MartianDoor = 2815;

		// Token: 0x0400304D RID: 12365
		public const short MartianDresser = 2816;

		// Token: 0x0400304E RID: 12366
		public const short MartianHolobookcase = 2817;

		// Token: 0x0400304F RID: 12367
		public const short MartianHoverCandle = 2818;

		// Token: 0x04003050 RID: 12368
		public const short MartianLamppost = 2819;

		// Token: 0x04003051 RID: 12369
		public const short MartianLantern = 2820;

		// Token: 0x04003052 RID: 12370
		public const short MartianPiano = 2821;

		// Token: 0x04003053 RID: 12371
		public const short MartianPlatform = 2822;

		// Token: 0x04003054 RID: 12372
		public const short MartianSofa = 2823;

		// Token: 0x04003055 RID: 12373
		public const short MartianTable = 2824;

		// Token: 0x04003056 RID: 12374
		public const short MartianTableLamp = 2825;

		// Token: 0x04003057 RID: 12375
		public const short MartianWorkBench = 2826;

		// Token: 0x04003058 RID: 12376
		public const short WoodenSink = 2827;

		// Token: 0x04003059 RID: 12377
		public const short EbonwoodSink = 2828;

		// Token: 0x0400305A RID: 12378
		public const short RichMahoganySink = 2829;

		// Token: 0x0400305B RID: 12379
		public const short PearlwoodSink = 2830;

		// Token: 0x0400305C RID: 12380
		public const short BoneSink = 2831;

		// Token: 0x0400305D RID: 12381
		public const short FleshSink = 2832;

		// Token: 0x0400305E RID: 12382
		public const short LivingWoodSink = 2833;

		// Token: 0x0400305F RID: 12383
		public const short SkywareSink = 2834;

		// Token: 0x04003060 RID: 12384
		public const short ShadewoodSink = 2835;

		// Token: 0x04003061 RID: 12385
		public const short LihzahrdSink = 2836;

		// Token: 0x04003062 RID: 12386
		public const short BlueDungeonSink = 2837;

		// Token: 0x04003063 RID: 12387
		public const short GreenDungeonSink = 2838;

		// Token: 0x04003064 RID: 12388
		public const short PinkDungeonSink = 2839;

		// Token: 0x04003065 RID: 12389
		public const short ObsidianSink = 2840;

		// Token: 0x04003066 RID: 12390
		public const short MetalSink = 2841;

		// Token: 0x04003067 RID: 12391
		public const short GlassSink = 2842;

		// Token: 0x04003068 RID: 12392
		public const short GoldenSink = 2843;

		// Token: 0x04003069 RID: 12393
		public const short HoneySink = 2844;

		// Token: 0x0400306A RID: 12394
		public const short SteampunkSink = 2845;

		// Token: 0x0400306B RID: 12395
		public const short PumpkinSink = 2846;

		// Token: 0x0400306C RID: 12396
		public const short SpookySink = 2847;

		// Token: 0x0400306D RID: 12397
		public const short FrozenSink = 2848;

		// Token: 0x0400306E RID: 12398
		public const short DynastySink = 2849;

		// Token: 0x0400306F RID: 12399
		public const short PalmWoodSink = 2850;

		// Token: 0x04003070 RID: 12400
		public const short MushroomSink = 2851;

		// Token: 0x04003071 RID: 12401
		public const short BorealWoodSink = 2852;

		// Token: 0x04003072 RID: 12402
		public const short SlimeSink = 2853;

		// Token: 0x04003073 RID: 12403
		public const short CactusSink = 2854;

		// Token: 0x04003074 RID: 12404
		public const short MartianSink = 2855;

		// Token: 0x04003075 RID: 12405
		public const short WhiteLunaticHood = 2856;

		// Token: 0x04003076 RID: 12406
		public const short BlueLunaticHood = 2857;

		// Token: 0x04003077 RID: 12407
		public const short WhiteLunaticRobe = 2858;

		// Token: 0x04003078 RID: 12408
		public const short BlueLunaticRobe = 2859;

		// Token: 0x04003079 RID: 12409
		public const short MartianConduitPlating = 2860;

		// Token: 0x0400307A RID: 12410
		public const short MartianConduitWall = 2861;

		// Token: 0x0400307B RID: 12411
		public const short HiTekSunglasses = 2862;

		// Token: 0x0400307C RID: 12412
		public const short MartianHairDye = 2863;

		// Token: 0x0400307D RID: 12413
		public const short MartianArmorDye = 2864;

		// Token: 0x0400307E RID: 12414
		public const short PaintingCastleMarsberg = 2865;

		// Token: 0x0400307F RID: 12415
		public const short PaintingMartiaLisa = 2866;

		// Token: 0x04003080 RID: 12416
		public const short PaintingTheTruthIsUpThere = 2867;

		// Token: 0x04003081 RID: 12417
		public const short SmokeBlock = 2868;

		// Token: 0x04003082 RID: 12418
		public const short LivingFlameDye = 2869;

		// Token: 0x04003083 RID: 12419
		public const short LivingRainbowDye = 2870;

		// Token: 0x04003084 RID: 12420
		public const short ShadowDye = 2871;

		// Token: 0x04003085 RID: 12421
		public const short NegativeDye = 2872;

		// Token: 0x04003086 RID: 12422
		public const short LivingOceanDye = 2873;

		// Token: 0x04003087 RID: 12423
		public const short BrownDye = 2874;

		// Token: 0x04003088 RID: 12424
		public const short BrownAndBlackDye = 2875;

		// Token: 0x04003089 RID: 12425
		public const short BrightBrownDye = 2876;

		// Token: 0x0400308A RID: 12426
		public const short BrownAndSilverDye = 2877;

		// Token: 0x0400308B RID: 12427
		public const short WispDye = 2878;

		// Token: 0x0400308C RID: 12428
		public const short PixieDye = 2879;

		// Token: 0x0400308D RID: 12429
		public const short InfluxWaver = 2880;

		// Token: 0x0400308E RID: 12430
		public const short PhasicWarpEjector = 2881;

		// Token: 0x0400308F RID: 12431
		public const short ChargedBlasterCannon = 2882;

		// Token: 0x04003090 RID: 12432
		public const short ChlorophyteDye = 2883;

		// Token: 0x04003091 RID: 12433
		public const short UnicornWispDye = 2884;

		// Token: 0x04003092 RID: 12434
		public const short InfernalWispDye = 2885;

		// Token: 0x04003093 RID: 12435
		public const short ViciousPowder = 2886;

		// Token: 0x04003094 RID: 12436
		public const short ViciousMushroom = 2887;

		// Token: 0x04003095 RID: 12437
		public const short BeesKnees = 2888;

		// Token: 0x04003096 RID: 12438
		public const short GoldBird = 2889;

		// Token: 0x04003097 RID: 12439
		public const short GoldBunny = 2890;

		// Token: 0x04003098 RID: 12440
		public const short GoldButterfly = 2891;

		// Token: 0x04003099 RID: 12441
		public const short GoldFrog = 2892;

		// Token: 0x0400309A RID: 12442
		public const short GoldGrasshopper = 2893;

		// Token: 0x0400309B RID: 12443
		public const short GoldMouse = 2894;

		// Token: 0x0400309C RID: 12444
		public const short GoldWorm = 2895;

		// Token: 0x0400309D RID: 12445
		public const short StickyDynamite = 2896;

		// Token: 0x0400309E RID: 12446
		public const short AngryTrapperBanner = 2897;

		// Token: 0x0400309F RID: 12447
		public const short ArmoredVikingBanner = 2898;

		// Token: 0x040030A0 RID: 12448
		public const short BlackSlimeBanner = 2899;

		// Token: 0x040030A1 RID: 12449
		public const short BlueArmoredBonesBanner = 2900;

		// Token: 0x040030A2 RID: 12450
		public const short BlueCultistArcherBanner = 2901;

		// Token: 0x040030A3 RID: 12451
		public const short BlueCultistCasterBanner = 2902;

		// Token: 0x040030A4 RID: 12452
		public const short BlueCultistFighterBanner = 2903;

		// Token: 0x040030A5 RID: 12453
		public const short BoneLeeBanner = 2904;

		// Token: 0x040030A6 RID: 12454
		public const short ClingerBanner = 2905;

		// Token: 0x040030A7 RID: 12455
		public const short CochinealBeetleBanner = 2906;

		// Token: 0x040030A8 RID: 12456
		public const short CorruptPenguinBanner = 2907;

		// Token: 0x040030A9 RID: 12457
		public const short CorruptSlimeBanner = 2908;

		// Token: 0x040030AA RID: 12458
		public const short CorruptorBanner = 2909;

		// Token: 0x040030AB RID: 12459
		public const short CrimslimeBanner = 2910;

		// Token: 0x040030AC RID: 12460
		public const short CursedSkullBanner = 2911;

		// Token: 0x040030AD RID: 12461
		public const short CyanBeetleBanner = 2912;

		// Token: 0x040030AE RID: 12462
		public const short DevourerBanner = 2913;

		// Token: 0x040030AF RID: 12463
		public const short DiablolistBanner = 2914;

		// Token: 0x040030B0 RID: 12464
		public const short DoctorBonesBanner = 2915;

		// Token: 0x040030B1 RID: 12465
		public const short DungeonSlimeBanner = 2916;

		// Token: 0x040030B2 RID: 12466
		public const short DungeonSpiritBanner = 2917;

		// Token: 0x040030B3 RID: 12467
		public const short ElfArcherBanner = 2918;

		// Token: 0x040030B4 RID: 12468
		public const short ElfCopterBanner = 2919;

		// Token: 0x040030B5 RID: 12469
		public const short EyezorBanner = 2920;

		// Token: 0x040030B6 RID: 12470
		public const short FlockoBanner = 2921;

		// Token: 0x040030B7 RID: 12471
		public const short GhostBanner = 2922;

		// Token: 0x040030B8 RID: 12472
		public const short GiantBatBanner = 2923;

		// Token: 0x040030B9 RID: 12473
		public const short GiantCursedSkullBanner = 2924;

		// Token: 0x040030BA RID: 12474
		public const short GiantFlyingFoxBanner = 2925;

		// Token: 0x040030BB RID: 12475
		public const short GingerbreadManBanner = 2926;

		// Token: 0x040030BC RID: 12476
		public const short GoblinArcherBanner = 2927;

		// Token: 0x040030BD RID: 12477
		public const short GreenSlimeBanner = 2928;

		// Token: 0x040030BE RID: 12478
		public const short HeadlessHorsemanBanner = 2929;

		// Token: 0x040030BF RID: 12479
		public const short HellArmoredBonesBanner = 2930;

		// Token: 0x040030C0 RID: 12480
		public const short HellhoundBanner = 2931;

		// Token: 0x040030C1 RID: 12481
		public const short HoppinJackBanner = 2932;

		// Token: 0x040030C2 RID: 12482
		public const short IceBatBanner = 2933;

		// Token: 0x040030C3 RID: 12483
		public const short IceGolemBanner = 2934;

		// Token: 0x040030C4 RID: 12484
		public const short IceSlimeBanner = 2935;

		// Token: 0x040030C5 RID: 12485
		public const short IchorStickerBanner = 2936;

		// Token: 0x040030C6 RID: 12486
		public const short IlluminantBatBanner = 2937;

		// Token: 0x040030C7 RID: 12487
		public const short IlluminantSlimeBanner = 2938;

		// Token: 0x040030C8 RID: 12488
		public const short JungleBatBanner = 2939;

		// Token: 0x040030C9 RID: 12489
		public const short JungleSlimeBanner = 2940;

		// Token: 0x040030CA RID: 12490
		public const short KrampusBanner = 2941;

		// Token: 0x040030CB RID: 12491
		public const short LacBeetleBanner = 2942;

		// Token: 0x040030CC RID: 12492
		public const short LavaBatBanner = 2943;

		// Token: 0x040030CD RID: 12493
		public const short LavaSlimeBanner = 2944;

		// Token: 0x040030CE RID: 12494
		public const short MartianBrainscramblerBanner = 2945;

		// Token: 0x040030CF RID: 12495
		public const short MartianDroneBanner = 2946;

		// Token: 0x040030D0 RID: 12496
		public const short MartianEngineerBanner = 2947;

		// Token: 0x040030D1 RID: 12497
		public const short MartianGigazapperBanner = 2948;

		// Token: 0x040030D2 RID: 12498
		public const short MartianGreyGruntBanner = 2949;

		// Token: 0x040030D3 RID: 12499
		public const short MartianOfficerBanner = 2950;

		// Token: 0x040030D4 RID: 12500
		public const short MartianRaygunnerBanner = 2951;

		// Token: 0x040030D5 RID: 12501
		public const short MartianScutlixGunnerBanner = 2952;

		// Token: 0x040030D6 RID: 12502
		public const short MartianTeslaTurretBanner = 2953;

		// Token: 0x040030D7 RID: 12503
		public const short MisterStabbyBanner = 2954;

		// Token: 0x040030D8 RID: 12504
		public const short MotherSlimeBanner = 2955;

		// Token: 0x040030D9 RID: 12505
		public const short NecromancerBanner = 2956;

		// Token: 0x040030DA RID: 12506
		public const short NutcrackerBanner = 2957;

		// Token: 0x040030DB RID: 12507
		public const short PaladinBanner = 2958;

		// Token: 0x040030DC RID: 12508
		public const short PenguinBanner = 2959;

		// Token: 0x040030DD RID: 12509
		public const short PinkyBanner = 2960;

		// Token: 0x040030DE RID: 12510
		public const short PoltergeistBanner = 2961;

		// Token: 0x040030DF RID: 12511
		public const short PossessedArmorBanner = 2962;

		// Token: 0x040030E0 RID: 12512
		public const short PresentMimicBanner = 2963;

		// Token: 0x040030E1 RID: 12513
		public const short PurpleSlimeBanner = 2964;

		// Token: 0x040030E2 RID: 12514
		public const short RaggedCasterBanner = 2965;

		// Token: 0x040030E3 RID: 12515
		public const short RainbowSlimeBanner = 2966;

		// Token: 0x040030E4 RID: 12516
		public const short RavenBanner = 2967;

		// Token: 0x040030E5 RID: 12517
		public const short RedSlimeBanner = 2968;

		// Token: 0x040030E6 RID: 12518
		public const short RuneWizardBanner = 2969;

		// Token: 0x040030E7 RID: 12519
		public const short RustyArmoredBonesBanner = 2970;

		// Token: 0x040030E8 RID: 12520
		public const short ScarecrowBanner = 2971;

		// Token: 0x040030E9 RID: 12521
		public const short ScutlixBanner = 2972;

		// Token: 0x040030EA RID: 12522
		public const short SkeletonArcherBanner = 2973;

		// Token: 0x040030EB RID: 12523
		public const short SkeletonCommandoBanner = 2974;

		// Token: 0x040030EC RID: 12524
		public const short SkeletonSniperBanner = 2975;

		// Token: 0x040030ED RID: 12525
		public const short SlimerBanner = 2976;

		// Token: 0x040030EE RID: 12526
		public const short SnatcherBanner = 2977;

		// Token: 0x040030EF RID: 12527
		public const short SnowBallaBanner = 2978;

		// Token: 0x040030F0 RID: 12528
		public const short SnowmanGangstaBanner = 2979;

		// Token: 0x040030F1 RID: 12529
		public const short SpikedIceSlimeBanner = 2980;

		// Token: 0x040030F2 RID: 12530
		public const short SpikedJungleSlimeBanner = 2981;

		// Token: 0x040030F3 RID: 12531
		public const short SplinterlingBanner = 2982;

		// Token: 0x040030F4 RID: 12532
		public const short SquidBanner = 2983;

		// Token: 0x040030F5 RID: 12533
		public const short TacticalSkeletonBanner = 2984;

		// Token: 0x040030F6 RID: 12534
		public const short TheGroomBanner = 2985;

		// Token: 0x040030F7 RID: 12535
		public const short TimBanner = 2986;

		// Token: 0x040030F8 RID: 12536
		public const short UndeadMinerBanner = 2987;

		// Token: 0x040030F9 RID: 12537
		public const short UndeadVikingBanner = 2988;

		// Token: 0x040030FA RID: 12538
		public const short WhiteCultistArcherBanner = 2989;

		// Token: 0x040030FB RID: 12539
		public const short WhiteCultistCasterBanner = 2990;

		// Token: 0x040030FC RID: 12540
		public const short WhiteCultistFighterBanner = 2991;

		// Token: 0x040030FD RID: 12541
		public const short YellowSlimeBanner = 2992;

		// Token: 0x040030FE RID: 12542
		public const short YetiBanner = 2993;

		// Token: 0x040030FF RID: 12543
		public const short ZombieElfBanner = 2994;

		// Token: 0x04003100 RID: 12544
		public const short SparkyPainting = 2995;

		// Token: 0x04003101 RID: 12545
		public const short VineRope = 2996;

		// Token: 0x04003102 RID: 12546
		public const short WormholePotion = 2997;

		// Token: 0x04003103 RID: 12547
		public const short SummonerEmblem = 2998;

		// Token: 0x04003104 RID: 12548
		public const short BewitchingTable = 2999;

		// Token: 0x04003105 RID: 12549
		public const short AlchemyTable = 3000;

		// Token: 0x04003106 RID: 12550
		public const short StrangeBrew = 3001;

		// Token: 0x04003107 RID: 12551
		public const short SpelunkerGlowstick = 3002;

		// Token: 0x04003108 RID: 12552
		public const short BoneArrow = 3003;

		// Token: 0x04003109 RID: 12553
		public const short BoneTorch = 3004;

		// Token: 0x0400310A RID: 12554
		public const short VineRopeCoil = 3005;

		// Token: 0x0400310B RID: 12555
		public const short SoulDrain = 3006;

		// Token: 0x0400310C RID: 12556
		public const short DartPistol = 3007;

		// Token: 0x0400310D RID: 12557
		public const short DartRifle = 3008;

		// Token: 0x0400310E RID: 12558
		public const short CrystalDart = 3009;

		// Token: 0x0400310F RID: 12559
		public const short CursedDart = 3010;

		// Token: 0x04003110 RID: 12560
		public const short IchorDart = 3011;

		// Token: 0x04003111 RID: 12561
		public const short ChainGuillotines = 3012;

		// Token: 0x04003112 RID: 12562
		public const short FetidBaghnakhs = 3013;

		// Token: 0x04003113 RID: 12563
		public const short ClingerStaff = 3014;

		// Token: 0x04003114 RID: 12564
		public const short PutridScent = 3015;

		// Token: 0x04003115 RID: 12565
		public const short FleshKnuckles = 3016;

		// Token: 0x04003116 RID: 12566
		public const short FlowerBoots = 3017;

		// Token: 0x04003117 RID: 12567
		public const short Seedler = 3018;

		// Token: 0x04003118 RID: 12568
		public const short HellwingBow = 3019;

		// Token: 0x04003119 RID: 12569
		public const short TendonHook = 3020;

		// Token: 0x0400311A RID: 12570
		public const short ThornHook = 3021;

		// Token: 0x0400311B RID: 12571
		public const short IlluminantHook = 3022;

		// Token: 0x0400311C RID: 12572
		public const short WormHook = 3023;

		// Token: 0x0400311D RID: 12573
		public const short DevDye = 3024;

		// Token: 0x0400311E RID: 12574
		public const short PurpleOozeDye = 3025;

		// Token: 0x0400311F RID: 12575
		public const short ReflectiveSilverDye = 3026;

		// Token: 0x04003120 RID: 12576
		public const short ReflectiveGoldDye = 3027;

		// Token: 0x04003121 RID: 12577
		public const short BlueAcidDye = 3028;

		// Token: 0x04003122 RID: 12578
		public const short DaedalusStormbow = 3029;

		// Token: 0x04003123 RID: 12579
		public const short FlyingKnife = 3030;

		// Token: 0x04003124 RID: 12580
		public const short BottomlessBucket = 3031;

		// Token: 0x04003125 RID: 12581
		public const short SuperAbsorbantSponge = 3032;

		// Token: 0x04003126 RID: 12582
		public const short GoldRing = 3033;

		// Token: 0x04003127 RID: 12583
		public const short CoinRing = 3034;

		// Token: 0x04003128 RID: 12584
		public const short GreedyRing = 3035;

		// Token: 0x04003129 RID: 12585
		public const short FishFinder = 3036;

		// Token: 0x0400312A RID: 12586
		public const short WeatherRadio = 3037;

		// Token: 0x0400312B RID: 12587
		public const short HadesDye = 3038;

		// Token: 0x0400312C RID: 12588
		public const short TwilightDye = 3039;

		// Token: 0x0400312D RID: 12589
		public const short AcidDye = 3040;

		// Token: 0x0400312E RID: 12590
		public const short MushroomDye = 3041;

		// Token: 0x0400312F RID: 12591
		public const short PhaseDye = 3042;

		// Token: 0x04003130 RID: 12592
		public const short MagicLantern = 3043;

		// Token: 0x04003131 RID: 12593
		public const short MusicBoxLunarBoss = 3044;

		// Token: 0x04003132 RID: 12594
		public const short RainbowTorch = 3045;

		// Token: 0x04003133 RID: 12595
		public const short CursedCampfire = 3046;

		// Token: 0x04003134 RID: 12596
		public const short DemonCampfire = 3047;

		// Token: 0x04003135 RID: 12597
		public const short FrozenCampfire = 3048;

		// Token: 0x04003136 RID: 12598
		public const short IchorCampfire = 3049;

		// Token: 0x04003137 RID: 12599
		public const short RainbowCampfire = 3050;

		// Token: 0x04003138 RID: 12600
		public const short CrystalVileShard = 3051;

		// Token: 0x04003139 RID: 12601
		public const short ShadowFlameBow = 3052;

		// Token: 0x0400313A RID: 12602
		public const short ShadowFlameHexDoll = 3053;

		// Token: 0x0400313B RID: 12603
		public const short ShadowFlameKnife = 3054;

		// Token: 0x0400313C RID: 12604
		public const short PaintingAcorns = 3055;

		// Token: 0x0400313D RID: 12605
		public const short PaintingColdSnap = 3056;

		// Token: 0x0400313E RID: 12606
		public const short PaintingCursedSaint = 3057;

		// Token: 0x0400313F RID: 12607
		public const short PaintingSnowfellas = 3058;

		// Token: 0x04003140 RID: 12608
		public const short PaintingTheSeason = 3059;

		// Token: 0x04003141 RID: 12609
		public const short BoneRattle = 3060;

		// Token: 0x04003142 RID: 12610
		public const short ArchitectGizmoPack = 3061;

		// Token: 0x04003143 RID: 12611
		public const short CrimsonHeart = 3062;

		// Token: 0x04003144 RID: 12612
		public const short Meowmere = 3063;

		// Token: 0x04003145 RID: 12613
		public const short Sundial = 3064;

		// Token: 0x04003146 RID: 12614
		public const short StarWrath = 3065;

		// Token: 0x04003147 RID: 12615
		public const short MarbleBlock = 3066;

		// Token: 0x04003148 RID: 12616
		public const short HellstoneBrickWall = 3067;

		// Token: 0x04003149 RID: 12617
		public const short CordageGuide = 3068;

		// Token: 0x0400314A RID: 12618
		public const short WandofSparking = 3069;

		// Token: 0x0400314B RID: 12619
		public const short GoldBirdCage = 3070;

		// Token: 0x0400314C RID: 12620
		public const short GoldBunnyCage = 3071;

		// Token: 0x0400314D RID: 12621
		public const short GoldButterflyCage = 3072;

		// Token: 0x0400314E RID: 12622
		public const short GoldFrogCage = 3073;

		// Token: 0x0400314F RID: 12623
		public const short GoldGrasshopperCage = 3074;

		// Token: 0x04003150 RID: 12624
		public const short GoldMouseCage = 3075;

		// Token: 0x04003151 RID: 12625
		public const short GoldWormCage = 3076;

		// Token: 0x04003152 RID: 12626
		public const short SilkRope = 3077;

		// Token: 0x04003153 RID: 12627
		public const short WebRope = 3078;

		// Token: 0x04003154 RID: 12628
		public const short SilkRopeCoil = 3079;

		// Token: 0x04003155 RID: 12629
		public const short WebRopeCoil = 3080;

		// Token: 0x04003156 RID: 12630
		public const short Marble = 3081;

		// Token: 0x04003157 RID: 12631
		public const short MarbleWall = 3082;

		// Token: 0x04003158 RID: 12632
		public const short MarbleBlockWall = 3083;

		// Token: 0x04003159 RID: 12633
		public const short Radar = 3084;

		// Token: 0x0400315A RID: 12634
		public const short LockBox = 3085;

		// Token: 0x0400315B RID: 12635
		public const short Granite = 3086;

		// Token: 0x0400315C RID: 12636
		public const short GraniteBlock = 3087;

		// Token: 0x0400315D RID: 12637
		public const short GraniteWall = 3088;

		// Token: 0x0400315E RID: 12638
		public const short GraniteBlockWall = 3089;

		// Token: 0x0400315F RID: 12639
		public const short RoyalGel = 3090;

		// Token: 0x04003160 RID: 12640
		public const short NightKey = 3091;

		// Token: 0x04003161 RID: 12641
		public const short LightKey = 3092;

		// Token: 0x04003162 RID: 12642
		public const short HerbBag = 3093;

		// Token: 0x04003163 RID: 12643
		public const short Javelin = 3094;

		// Token: 0x04003164 RID: 12644
		public const short TallyCounter = 3095;

		// Token: 0x04003165 RID: 12645
		public const short Sextant = 3096;

		// Token: 0x04003166 RID: 12646
		public const short EoCShield = 3097;

		// Token: 0x04003167 RID: 12647
		public const short ButchersChainsaw = 3098;

		// Token: 0x04003168 RID: 12648
		public const short Stopwatch = 3099;

		// Token: 0x04003169 RID: 12649
		public const short MeteoriteBrick = 3100;

		// Token: 0x0400316A RID: 12650
		public const short MeteoriteBrickWall = 3101;

		// Token: 0x0400316B RID: 12651
		public const short MetalDetector = 3102;

		// Token: 0x0400316C RID: 12652
		public const short EndlessQuiver = 3103;

		// Token: 0x0400316D RID: 12653
		public const short EndlessMusketPouch = 3104;

		// Token: 0x0400316E RID: 12654
		public const short ToxicFlask = 3105;

		// Token: 0x0400316F RID: 12655
		public const short PsychoKnife = 3106;

		// Token: 0x04003170 RID: 12656
		public const short NailGun = 3107;

		// Token: 0x04003171 RID: 12657
		public const short Nail = 3108;

		// Token: 0x04003172 RID: 12658
		public const short NightVisionHelmet = 3109;

		// Token: 0x04003173 RID: 12659
		public const short CelestialShell = 3110;

		// Token: 0x04003174 RID: 12660
		public const short PinkGel = 3111;

		// Token: 0x04003175 RID: 12661
		public const short BouncyGlowstick = 3112;

		// Token: 0x04003176 RID: 12662
		public const short PinkSlimeBlock = 3113;

		// Token: 0x04003177 RID: 12663
		public const short PinkTorch = 3114;

		// Token: 0x04003178 RID: 12664
		public const short BouncyBomb = 3115;

		// Token: 0x04003179 RID: 12665
		public const short BouncyGrenade = 3116;

		// Token: 0x0400317A RID: 12666
		public const short PeaceCandle = 3117;

		// Token: 0x0400317B RID: 12667
		public const short LifeformAnalyzer = 3118;

		// Token: 0x0400317C RID: 12668
		public const short DPSMeter = 3119;

		// Token: 0x0400317D RID: 12669
		public const short FishermansGuide = 3120;

		// Token: 0x0400317E RID: 12670
		public const short GoblinTech = 3121;

		// Token: 0x0400317F RID: 12671
		public const short REK = 3122;

		// Token: 0x04003180 RID: 12672
		public const short PDA = 3123;

		// Token: 0x04003181 RID: 12673
		public const short CellPhone = 3124;

		// Token: 0x04003182 RID: 12674
		public const short GraniteChest = 3125;

		// Token: 0x04003183 RID: 12675
		public const short MeteoriteClock = 3126;

		// Token: 0x04003184 RID: 12676
		public const short MarbleClock = 3127;

		// Token: 0x04003185 RID: 12677
		public const short GraniteClock = 3128;

		// Token: 0x04003186 RID: 12678
		public const short MeteoriteDoor = 3129;

		// Token: 0x04003187 RID: 12679
		public const short MarbleDoor = 3130;

		// Token: 0x04003188 RID: 12680
		public const short GraniteDoor = 3131;

		// Token: 0x04003189 RID: 12681
		public const short MeteoriteDresser = 3132;

		// Token: 0x0400318A RID: 12682
		public const short MarbleDresser = 3133;

		// Token: 0x0400318B RID: 12683
		public const short GraniteDresser = 3134;

		// Token: 0x0400318C RID: 12684
		public const short MeteoriteLamp = 3135;

		// Token: 0x0400318D RID: 12685
		public const short MarbleLamp = 3136;

		// Token: 0x0400318E RID: 12686
		public const short GraniteLamp = 3137;

		// Token: 0x0400318F RID: 12687
		public const short MeteoriteLantern = 3138;

		// Token: 0x04003190 RID: 12688
		public const short MarbleLantern = 3139;

		// Token: 0x04003191 RID: 12689
		public const short GraniteLantern = 3140;

		// Token: 0x04003192 RID: 12690
		public const short MeteoritePiano = 3141;

		// Token: 0x04003193 RID: 12691
		public const short MarblePiano = 3142;

		// Token: 0x04003194 RID: 12692
		public const short GranitePiano = 3143;

		// Token: 0x04003195 RID: 12693
		public const short MeteoritePlatform = 3144;

		// Token: 0x04003196 RID: 12694
		public const short MarblePlatform = 3145;

		// Token: 0x04003197 RID: 12695
		public const short GranitePlatform = 3146;

		// Token: 0x04003198 RID: 12696
		public const short MeteoriteSink = 3147;

		// Token: 0x04003199 RID: 12697
		public const short MarbleSink = 3148;

		// Token: 0x0400319A RID: 12698
		public const short GraniteSink = 3149;

		// Token: 0x0400319B RID: 12699
		public const short MeteoriteSofa = 3150;

		// Token: 0x0400319C RID: 12700
		public const short MarbleSofa = 3151;

		// Token: 0x0400319D RID: 12701
		public const short GraniteSofa = 3152;

		// Token: 0x0400319E RID: 12702
		public const short MeteoriteTable = 3153;

		// Token: 0x0400319F RID: 12703
		public const short MarbleTable = 3154;

		// Token: 0x040031A0 RID: 12704
		public const short GraniteTable = 3155;

		// Token: 0x040031A1 RID: 12705
		public const short MeteoriteWorkBench = 3156;

		// Token: 0x040031A2 RID: 12706
		public const short MarbleWorkBench = 3157;

		// Token: 0x040031A3 RID: 12707
		public const short GraniteWorkBench = 3158;

		// Token: 0x040031A4 RID: 12708
		public const short MeteoriteBathtub = 3159;

		// Token: 0x040031A5 RID: 12709
		public const short MarbleBathtub = 3160;

		// Token: 0x040031A6 RID: 12710
		public const short GraniteBathtub = 3161;

		// Token: 0x040031A7 RID: 12711
		public const short MeteoriteBed = 3162;

		// Token: 0x040031A8 RID: 12712
		public const short MarbleBed = 3163;

		// Token: 0x040031A9 RID: 12713
		public const short GraniteBed = 3164;

		// Token: 0x040031AA RID: 12714
		public const short MeteoriteBookcase = 3165;

		// Token: 0x040031AB RID: 12715
		public const short MarbleBookcase = 3166;

		// Token: 0x040031AC RID: 12716
		public const short GraniteBookcase = 3167;

		// Token: 0x040031AD RID: 12717
		public const short MeteoriteCandelabra = 3168;

		// Token: 0x040031AE RID: 12718
		public const short MarbleCandelabra = 3169;

		// Token: 0x040031AF RID: 12719
		public const short GraniteCandelabra = 3170;

		// Token: 0x040031B0 RID: 12720
		public const short MeteoriteCandle = 3171;

		// Token: 0x040031B1 RID: 12721
		public const short MarbleCandle = 3172;

		// Token: 0x040031B2 RID: 12722
		public const short GraniteCandle = 3173;

		// Token: 0x040031B3 RID: 12723
		public const short MeteoriteChair = 3174;

		// Token: 0x040031B4 RID: 12724
		public const short MarbleChair = 3175;

		// Token: 0x040031B5 RID: 12725
		public const short GraniteChair = 3176;

		// Token: 0x040031B6 RID: 12726
		public const short MeteoriteChandelier = 3177;

		// Token: 0x040031B7 RID: 12727
		public const short MarbleChandelier = 3178;

		// Token: 0x040031B8 RID: 12728
		public const short GraniteChandelier = 3179;

		// Token: 0x040031B9 RID: 12729
		public const short MeteoriteChest = 3180;

		// Token: 0x040031BA RID: 12730
		public const short MarbleChest = 3181;

		// Token: 0x040031BB RID: 12731
		public const short MagicWaterDropper = 3182;

		// Token: 0x040031BC RID: 12732
		public const short GoldenBugNet = 3183;

		// Token: 0x040031BD RID: 12733
		public const short MagicLavaDropper = 3184;

		// Token: 0x040031BE RID: 12734
		public const short MagicHoneyDropper = 3185;

		// Token: 0x040031BF RID: 12735
		public const short EmptyDropper = 3186;

		// Token: 0x040031C0 RID: 12736
		public const short GladiatorHelmet = 3187;

		// Token: 0x040031C1 RID: 12737
		public const short GladiatorBreastplate = 3188;

		// Token: 0x040031C2 RID: 12738
		public const short GladiatorLeggings = 3189;

		// Token: 0x040031C3 RID: 12739
		public const short ReflectiveDye = 3190;

		// Token: 0x040031C4 RID: 12740
		public const short EnchantedNightcrawler = 3191;

		// Token: 0x040031C5 RID: 12741
		public const short Grubby = 3192;

		// Token: 0x040031C6 RID: 12742
		public const short Sluggy = 3193;

		// Token: 0x040031C7 RID: 12743
		public const short Buggy = 3194;

		// Token: 0x040031C8 RID: 12744
		public const short GrubSoup = 3195;

		// Token: 0x040031C9 RID: 12745
		public const short BombFish = 3196;

		// Token: 0x040031CA RID: 12746
		public const short FrostDaggerfish = 3197;

		// Token: 0x040031CB RID: 12747
		public const short SharpeningStation = 3198;

		// Token: 0x040031CC RID: 12748
		public const short IceMirror = 3199;

		// Token: 0x040031CD RID: 12749
		public const short SailfishBoots = 3200;

		// Token: 0x040031CE RID: 12750
		public const short TsunamiInABottle = 3201;

		// Token: 0x040031CF RID: 12751
		public const short TargetDummy = 3202;

		// Token: 0x040031D0 RID: 12752
		public const short CorruptFishingCrate = 3203;

		// Token: 0x040031D1 RID: 12753
		public const short CrimsonFishingCrate = 3204;

		// Token: 0x040031D2 RID: 12754
		public const short DungeonFishingCrate = 3205;

		// Token: 0x040031D3 RID: 12755
		public const short FloatingIslandFishingCrate = 3206;

		// Token: 0x040031D4 RID: 12756
		public const short HallowedFishingCrate = 3207;

		// Token: 0x040031D5 RID: 12757
		public const short JungleFishingCrate = 3208;

		// Token: 0x040031D6 RID: 12758
		public const short CrystalSerpent = 3209;

		// Token: 0x040031D7 RID: 12759
		public const short Toxikarp = 3210;

		// Token: 0x040031D8 RID: 12760
		public const short Bladetongue = 3211;

		// Token: 0x040031D9 RID: 12761
		public const short SharkToothNecklace = 3212;

		// Token: 0x040031DA RID: 12762
		public const short MoneyTrough = 3213;

		// Token: 0x040031DB RID: 12763
		public const short Bubble = 3214;

		// Token: 0x040031DC RID: 12764
		public const short DayBloomPlanterBox = 3215;

		// Token: 0x040031DD RID: 12765
		public const short MoonglowPlanterBox = 3216;

		// Token: 0x040031DE RID: 12766
		public const short CorruptPlanterBox = 3217;

		// Token: 0x040031DF RID: 12767
		public const short CrimsonPlanterBox = 3218;

		// Token: 0x040031E0 RID: 12768
		public const short BlinkrootPlanterBox = 3219;

		// Token: 0x040031E1 RID: 12769
		public const short WaterleafPlanterBox = 3220;

		// Token: 0x040031E2 RID: 12770
		public const short ShiverthornPlanterBox = 3221;

		// Token: 0x040031E3 RID: 12771
		public const short FireBlossomPlanterBox = 3222;

		// Token: 0x040031E4 RID: 12772
		public const short BrainOfConfusion = 3223;

		// Token: 0x040031E5 RID: 12773
		public const short WormScarf = 3224;

		// Token: 0x040031E6 RID: 12774
		public const short BalloonPufferfish = 3225;

		// Token: 0x040031E7 RID: 12775
		public const short BejeweledValkyrieHead = 3226;

		// Token: 0x040031E8 RID: 12776
		public const short BejeweledValkyrieBody = 3227;

		// Token: 0x040031E9 RID: 12777
		public const short BejeweledValkyrieWing = 3228;

		// Token: 0x040031EA RID: 12778
		public const short RichGravestone1 = 3229;

		// Token: 0x040031EB RID: 12779
		public const short RichGravestone2 = 3230;

		// Token: 0x040031EC RID: 12780
		public const short RichGravestone3 = 3231;

		// Token: 0x040031ED RID: 12781
		public const short RichGravestone4 = 3232;

		// Token: 0x040031EE RID: 12782
		public const short RichGravestone5 = 3233;

		// Token: 0x040031EF RID: 12783
		public const short CrystalBlock = 3234;

		// Token: 0x040031F0 RID: 12784
		public const short MusicBoxMartians = 3235;

		// Token: 0x040031F1 RID: 12785
		public const short MusicBoxPirates = 3236;

		// Token: 0x040031F2 RID: 12786
		public const short MusicBoxHell = 3237;

		// Token: 0x040031F3 RID: 12787
		public const short CrystalBlockWall = 3238;

		// Token: 0x040031F4 RID: 12788
		public const short Trapdoor = 3239;

		// Token: 0x040031F5 RID: 12789
		public const short TallGate = 3240;

		// Token: 0x040031F6 RID: 12790
		public const short SharkronBalloon = 3241;

		// Token: 0x040031F7 RID: 12791
		public const short TaxCollectorHat = 3242;

		// Token: 0x040031F8 RID: 12792
		public const short TaxCollectorSuit = 3243;

		// Token: 0x040031F9 RID: 12793
		public const short TaxCollectorPants = 3244;

		// Token: 0x040031FA RID: 12794
		public const short BoneGlove = 3245;

		// Token: 0x040031FB RID: 12795
		public const short ClothierJacket = 3246;

		// Token: 0x040031FC RID: 12796
		public const short ClothierPants = 3247;

		// Token: 0x040031FD RID: 12797
		public const short DyeTraderTurban = 3248;

		// Token: 0x040031FE RID: 12798
		public const short DeadlySphereStaff = 3249;

		// Token: 0x040031FF RID: 12799
		public const short BalloonHorseshoeFart = 3250;

		// Token: 0x04003200 RID: 12800
		public const short BalloonHorseshoeHoney = 3251;

		// Token: 0x04003201 RID: 12801
		public const short BalloonHorseshoeSharkron = 3252;

		// Token: 0x04003202 RID: 12802
		public const short LavaLamp = 3253;

		// Token: 0x04003203 RID: 12803
		public const short CageEnchantedNightcrawler = 3254;

		// Token: 0x04003204 RID: 12804
		public const short CageBuggy = 3255;

		// Token: 0x04003205 RID: 12805
		public const short CageGrubby = 3256;

		// Token: 0x04003206 RID: 12806
		public const short CageSluggy = 3257;

		// Token: 0x04003207 RID: 12807
		public const short SlapHand = 3258;

		// Token: 0x04003208 RID: 12808
		public const short TwilightHairDye = 3259;

		// Token: 0x04003209 RID: 12809
		public const short BlessedApple = 3260;

		// Token: 0x0400320A RID: 12810
		public const short SpectreBar = 3261;

		// Token: 0x0400320B RID: 12811
		public const short Code1 = 3262;

		// Token: 0x0400320C RID: 12812
		public const short BuccaneerBandana = 3263;

		// Token: 0x0400320D RID: 12813
		public const short BuccaneerShirt = 3264;

		// Token: 0x0400320E RID: 12814
		public const short BuccaneerPants = 3265;

		// Token: 0x0400320F RID: 12815
		public const short ObsidianHelm = 3266;

		// Token: 0x04003210 RID: 12816
		public const short ObsidianShirt = 3267;

		// Token: 0x04003211 RID: 12817
		public const short ObsidianPants = 3268;

		// Token: 0x04003212 RID: 12818
		public const short MedusaHead = 3269;

		// Token: 0x04003213 RID: 12819
		public const short ItemFrame = 3270;

		// Token: 0x04003214 RID: 12820
		public const short Sandstone = 3271;

		// Token: 0x04003215 RID: 12821
		public const short HardenedSand = 3272;

		// Token: 0x04003216 RID: 12822
		public const short SandstoneWall = 3273;

		// Token: 0x04003217 RID: 12823
		public const short CorruptHardenedSand = 3274;

		// Token: 0x04003218 RID: 12824
		public const short CrimsonHardenedSand = 3275;

		// Token: 0x04003219 RID: 12825
		public const short CorruptSandstone = 3276;

		// Token: 0x0400321A RID: 12826
		public const short CrimsonSandstone = 3277;

		// Token: 0x0400321B RID: 12827
		public const short WoodYoyo = 3278;

		// Token: 0x0400321C RID: 12828
		public const short CorruptYoyo = 3279;

		// Token: 0x0400321D RID: 12829
		public const short CrimsonYoyo = 3280;

		// Token: 0x0400321E RID: 12830
		public const short JungleYoyo = 3281;

		// Token: 0x0400321F RID: 12831
		public const short Cascade = 3282;

		// Token: 0x04003220 RID: 12832
		public const short Chik = 3283;

		// Token: 0x04003221 RID: 12833
		public const short Code2 = 3284;

		// Token: 0x04003222 RID: 12834
		public const short Rally = 3285;

		// Token: 0x04003223 RID: 12835
		public const short Yelets = 3286;

		// Token: 0x04003224 RID: 12836
		public const short RedsYoyo = 3287;

		// Token: 0x04003225 RID: 12837
		public const short ValkyrieYoyo = 3288;

		// Token: 0x04003226 RID: 12838
		public const short Amarok = 3289;

		// Token: 0x04003227 RID: 12839
		public const short HelFire = 3290;

		// Token: 0x04003228 RID: 12840
		public const short Kraken = 3291;

		// Token: 0x04003229 RID: 12841
		public const short TheEyeOfCthulhu = 3292;

		// Token: 0x0400322A RID: 12842
		public const short RedString = 3293;

		// Token: 0x0400322B RID: 12843
		public const short OrangeString = 3294;

		// Token: 0x0400322C RID: 12844
		public const short YellowString = 3295;

		// Token: 0x0400322D RID: 12845
		public const short LimeString = 3296;

		// Token: 0x0400322E RID: 12846
		public const short GreenString = 3297;

		// Token: 0x0400322F RID: 12847
		public const short TealString = 3298;

		// Token: 0x04003230 RID: 12848
		public const short CyanString = 3299;

		// Token: 0x04003231 RID: 12849
		public const short SkyBlueString = 3300;

		// Token: 0x04003232 RID: 12850
		public const short BlueString = 3301;

		// Token: 0x04003233 RID: 12851
		public const short PurpleString = 3302;

		// Token: 0x04003234 RID: 12852
		public const short VioletString = 3303;

		// Token: 0x04003235 RID: 12853
		public const short PinkString = 3304;

		// Token: 0x04003236 RID: 12854
		public const short BrownString = 3305;

		// Token: 0x04003237 RID: 12855
		public const short WhiteString = 3306;

		// Token: 0x04003238 RID: 12856
		public const short RainbowString = 3307;

		// Token: 0x04003239 RID: 12857
		public const short BlackString = 3308;

		// Token: 0x0400323A RID: 12858
		public const short BlackCounterweight = 3309;

		// Token: 0x0400323B RID: 12859
		public const short BlueCounterweight = 3310;

		// Token: 0x0400323C RID: 12860
		public const short GreenCounterweight = 3311;

		// Token: 0x0400323D RID: 12861
		public const short PurpleCounterweight = 3312;

		// Token: 0x0400323E RID: 12862
		public const short RedCounterweight = 3313;

		// Token: 0x0400323F RID: 12863
		public const short YellowCounterweight = 3314;

		// Token: 0x04003240 RID: 12864
		public const short FormatC = 3315;

		// Token: 0x04003241 RID: 12865
		public const short Gradient = 3316;

		// Token: 0x04003242 RID: 12866
		public const short Valor = 3317;

		// Token: 0x04003243 RID: 12867
		public const short KingSlimeBossBag = 3318;

		// Token: 0x04003244 RID: 12868
		public const short EyeOfCthulhuBossBag = 3319;

		// Token: 0x04003245 RID: 12869
		public const short EaterOfWorldsBossBag = 3320;

		// Token: 0x04003246 RID: 12870
		public const short BrainOfCthulhuBossBag = 3321;

		// Token: 0x04003247 RID: 12871
		public const short QueenBeeBossBag = 3322;

		// Token: 0x04003248 RID: 12872
		public const short SkeletronBossBag = 3323;

		// Token: 0x04003249 RID: 12873
		public const short WallOfFleshBossBag = 3324;

		// Token: 0x0400324A RID: 12874
		public const short DestroyerBossBag = 3325;

		// Token: 0x0400324B RID: 12875
		public const short TwinsBossBag = 3326;

		// Token: 0x0400324C RID: 12876
		public const short SkeletronPrimeBossBag = 3327;

		// Token: 0x0400324D RID: 12877
		public const short PlanteraBossBag = 3328;

		// Token: 0x0400324E RID: 12878
		public const short GolemBossBag = 3329;

		// Token: 0x0400324F RID: 12879
		public const short FishronBossBag = 3330;

		// Token: 0x04003250 RID: 12880
		public const short CultistBossBag = 3331;

		// Token: 0x04003251 RID: 12881
		public const short MoonLordBossBag = 3332;

		// Token: 0x04003252 RID: 12882
		public const short HiveBackpack = 3333;

		// Token: 0x04003253 RID: 12883
		public const short YoYoGlove = 3334;

		// Token: 0x04003254 RID: 12884
		public const short DemonHeart = 3335;

		// Token: 0x04003255 RID: 12885
		public const short SporeSac = 3336;

		// Token: 0x04003256 RID: 12886
		public const short ShinyStone = 3337;

		// Token: 0x04003257 RID: 12887
		public const short HallowHardenedSand = 3338;

		// Token: 0x04003258 RID: 12888
		public const short HallowSandstone = 3339;

		// Token: 0x04003259 RID: 12889
		public const short HardenedSandWall = 3340;

		// Token: 0x0400325A RID: 12890
		public const short CorruptHardenedSandWall = 3341;

		// Token: 0x0400325B RID: 12891
		public const short CrimsonHardenedSandWall = 3342;

		// Token: 0x0400325C RID: 12892
		public const short HallowHardenedSandWall = 3343;

		// Token: 0x0400325D RID: 12893
		public const short CorruptSandstoneWall = 3344;

		// Token: 0x0400325E RID: 12894
		public const short CrimsonSandstoneWall = 3345;

		// Token: 0x0400325F RID: 12895
		public const short HallowSandstoneWall = 3346;

		// Token: 0x04003260 RID: 12896
		public const short DesertFossil = 3347;

		// Token: 0x04003261 RID: 12897
		public const short DesertFossilWall = 3348;

		// Token: 0x04003262 RID: 12898
		public const short DyeTradersScimitar = 3349;

		// Token: 0x04003263 RID: 12899
		public const short PainterPaintballGun = 3350;

		// Token: 0x04003264 RID: 12900
		public const short TaxCollectorsStickOfDoom = 3351;

		// Token: 0x04003265 RID: 12901
		public const short StylistKilLaKillScissorsIWish = 3352;

		// Token: 0x04003266 RID: 12902
		public const short MinecartMech = 3353;

		// Token: 0x04003267 RID: 12903
		public const short MechanicalWheelPiece = 3354;

		// Token: 0x04003268 RID: 12904
		public const short MechanicalWagonPiece = 3355;

		// Token: 0x04003269 RID: 12905
		public const short MechanicalBatteryPiece = 3356;

		// Token: 0x0400326A RID: 12906
		public const short AncientCultistTrophy = 3357;

		// Token: 0x0400326B RID: 12907
		public const short MartianSaucerTrophy = 3358;

		// Token: 0x0400326C RID: 12908
		public const short FlyingDutchmanTrophy = 3359;

		// Token: 0x0400326D RID: 12909
		public const short LivingMahoganyWand = 3360;

		// Token: 0x0400326E RID: 12910
		public const short LivingMahoganyLeafWand = 3361;

		// Token: 0x0400326F RID: 12911
		public const short FallenTuxedoShirt = 3362;

		// Token: 0x04003270 RID: 12912
		public const short FallenTuxedoPants = 3363;

		// Token: 0x04003271 RID: 12913
		public const short Fireplace = 3364;

		// Token: 0x04003272 RID: 12914
		public const short Chimney = 3365;

		// Token: 0x04003273 RID: 12915
		public const short YoyoBag = 3366;

		// Token: 0x04003274 RID: 12916
		public const short ShrimpyTruffle = 3367;

		// Token: 0x04003275 RID: 12917
		public const short Arkhalis = 3368;

		// Token: 0x04003276 RID: 12918
		public const short ConfettiCannon = 3369;

		// Token: 0x04003277 RID: 12919
		public const short MusicBoxTowers = 3370;

		// Token: 0x04003278 RID: 12920
		public const short MusicBoxGoblins = 3371;

		// Token: 0x04003279 RID: 12921
		public const short BossMaskCultist = 3372;

		// Token: 0x0400327A RID: 12922
		public const short BossMaskMoonlord = 3373;

		// Token: 0x0400327B RID: 12923
		public const short FossilHelm = 3374;

		// Token: 0x0400327C RID: 12924
		public const short FossilShirt = 3375;

		// Token: 0x0400327D RID: 12925
		public const short FossilPants = 3376;

		// Token: 0x0400327E RID: 12926
		public const short AmberStaff = 3377;

		// Token: 0x0400327F RID: 12927
		public const short BoneJavelin = 3378;

		// Token: 0x04003280 RID: 12928
		public const short BoneDagger = 3379;

		// Token: 0x04003281 RID: 12929
		public const short FossilOre = 3380;

		// Token: 0x04003282 RID: 12930
		public const short StardustHelmet = 3381;

		// Token: 0x04003283 RID: 12931
		public const short StardustBreastplate = 3382;

		// Token: 0x04003284 RID: 12932
		public const short StardustLeggings = 3383;

		// Token: 0x04003285 RID: 12933
		public const short PortalGun = 3384;

		// Token: 0x04003286 RID: 12934
		public const short StrangePlant1 = 3385;

		// Token: 0x04003287 RID: 12935
		public const short StrangePlant2 = 3386;

		// Token: 0x04003288 RID: 12936
		public const short StrangePlant3 = 3387;

		// Token: 0x04003289 RID: 12937
		public const short StrangePlant4 = 3388;

		// Token: 0x0400328A RID: 12938
		public const short Terrarian = 3389;

		// Token: 0x0400328B RID: 12939
		public const short GoblinSummonerBanner = 3390;

		// Token: 0x0400328C RID: 12940
		public const short SalamanderBanner = 3391;

		// Token: 0x0400328D RID: 12941
		public const short GiantShellyBanner = 3392;

		// Token: 0x0400328E RID: 12942
		public const short CrawdadBanner = 3393;

		// Token: 0x0400328F RID: 12943
		public const short FritzBanner = 3394;

		// Token: 0x04003290 RID: 12944
		public const short CreatureFromTheDeepBanner = 3395;

		// Token: 0x04003291 RID: 12945
		public const short DrManFlyBanner = 3396;

		// Token: 0x04003292 RID: 12946
		public const short MothronBanner = 3397;

		// Token: 0x04003293 RID: 12947
		public const short SeveredHandBanner = 3398;

		// Token: 0x04003294 RID: 12948
		public const short ThePossessedBanner = 3399;

		// Token: 0x04003295 RID: 12949
		public const short ButcherBanner = 3400;

		// Token: 0x04003296 RID: 12950
		public const short PsychoBanner = 3401;

		// Token: 0x04003297 RID: 12951
		public const short DeadlySphereBanner = 3402;

		// Token: 0x04003298 RID: 12952
		public const short NailheadBanner = 3403;

		// Token: 0x04003299 RID: 12953
		public const short PoisonousSporeBanner = 3404;

		// Token: 0x0400329A RID: 12954
		public const short MedusaBanner = 3405;

		// Token: 0x0400329B RID: 12955
		public const short GreekSkeletonBanner = 3406;

		// Token: 0x0400329C RID: 12956
		public const short GraniteFlyerBanner = 3407;

		// Token: 0x0400329D RID: 12957
		public const short GraniteGolemBanner = 3408;

		// Token: 0x0400329E RID: 12958
		public const short BloodZombieBanner = 3409;

		// Token: 0x0400329F RID: 12959
		public const short DripplerBanner = 3410;

		// Token: 0x040032A0 RID: 12960
		public const short TombCrawlerBanner = 3411;

		// Token: 0x040032A1 RID: 12961
		public const short DuneSplicerBanner = 3412;

		// Token: 0x040032A2 RID: 12962
		public const short FlyingAntlionBanner = 3413;

		// Token: 0x040032A3 RID: 12963
		public const short WalkingAntlionBanner = 3414;

		// Token: 0x040032A4 RID: 12964
		public const short DesertGhoulBanner = 3415;

		// Token: 0x040032A5 RID: 12965
		public const short DesertLamiaBanner = 3416;

		// Token: 0x040032A6 RID: 12966
		public const short DesertDjinnBanner = 3417;

		// Token: 0x040032A7 RID: 12967
		public const short DesertBasiliskBanner = 3418;

		// Token: 0x040032A8 RID: 12968
		public const short RavagerScorpionBanner = 3419;

		// Token: 0x040032A9 RID: 12969
		public const short StardustSoldierBanner = 3420;

		// Token: 0x040032AA RID: 12970
		public const short StardustWormBanner = 3421;

		// Token: 0x040032AB RID: 12971
		public const short StardustJellyfishBanner = 3422;

		// Token: 0x040032AC RID: 12972
		public const short StardustSpiderBanner = 3423;

		// Token: 0x040032AD RID: 12973
		public const short StardustSmallCellBanner = 3424;

		// Token: 0x040032AE RID: 12974
		public const short StardustLargeCellBanner = 3425;

		// Token: 0x040032AF RID: 12975
		public const short SolarCoriteBanner = 3426;

		// Token: 0x040032B0 RID: 12976
		public const short SolarSrollerBanner = 3427;

		// Token: 0x040032B1 RID: 12977
		public const short SolarCrawltipedeBanner = 3428;

		// Token: 0x040032B2 RID: 12978
		public const short SolarDrakomireRiderBanner = 3429;

		// Token: 0x040032B3 RID: 12979
		public const short SolarDrakomireBanner = 3430;

		// Token: 0x040032B4 RID: 12980
		public const short SolarSolenianBanner = 3431;

		// Token: 0x040032B5 RID: 12981
		public const short NebulaSoldierBanner = 3432;

		// Token: 0x040032B6 RID: 12982
		public const short NebulaHeadcrabBanner = 3433;

		// Token: 0x040032B7 RID: 12983
		public const short NebulaBrainBanner = 3434;

		// Token: 0x040032B8 RID: 12984
		public const short NebulaBeastBanner = 3435;

		// Token: 0x040032B9 RID: 12985
		public const short VortexLarvaBanner = 3436;

		// Token: 0x040032BA RID: 12986
		public const short VortexHornetQueenBanner = 3437;

		// Token: 0x040032BB RID: 12987
		public const short VortexHornetBanner = 3438;

		// Token: 0x040032BC RID: 12988
		public const short VortexSoldierBanner = 3439;

		// Token: 0x040032BD RID: 12989
		public const short VortexRiflemanBanner = 3440;

		// Token: 0x040032BE RID: 12990
		public const short PirateCaptainBanner = 3441;

		// Token: 0x040032BF RID: 12991
		public const short PirateDeadeyeBanner = 3442;

		// Token: 0x040032C0 RID: 12992
		public const short PirateCorsairBanner = 3443;

		// Token: 0x040032C1 RID: 12993
		public const short PirateCrossbowerBanner = 3444;

		// Token: 0x040032C2 RID: 12994
		public const short MartianWalkerBanner = 3445;

		// Token: 0x040032C3 RID: 12995
		public const short RedDevilBanner = 3446;

		// Token: 0x040032C4 RID: 12996
		public const short PinkJellyfishBanner = 3447;

		// Token: 0x040032C5 RID: 12997
		public const short GreenJellyfishBanner = 3448;

		// Token: 0x040032C6 RID: 12998
		public const short DarkMummyBanner = 3449;

		// Token: 0x040032C7 RID: 12999
		public const short LightMummyBanner = 3450;

		// Token: 0x040032C8 RID: 13000
		public const short AngryBonesBanner = 3451;

		// Token: 0x040032C9 RID: 13001
		public const short IceTortoiseBanner = 3452;

		// Token: 0x040032CA RID: 13002
		public const short NebulaPickup1 = 3453;

		// Token: 0x040032CB RID: 13003
		public const short NebulaPickup2 = 3454;

		// Token: 0x040032CC RID: 13004
		public const short NebulaPickup3 = 3455;

		// Token: 0x040032CD RID: 13005
		public const short FragmentVortex = 3456;

		// Token: 0x040032CE RID: 13006
		public const short FragmentNebula = 3457;

		// Token: 0x040032CF RID: 13007
		public const short FragmentSolar = 3458;

		// Token: 0x040032D0 RID: 13008
		public const short FragmentStardust = 3459;

		// Token: 0x040032D1 RID: 13009
		public const short LunarOre = 3460;

		// Token: 0x040032D2 RID: 13010
		public const short LunarBrick = 3461;

		// Token: 0x040032D3 RID: 13011
		public const short StardustAxe = 3462;

		// Token: 0x040032D4 RID: 13012
		public const short StardustChainsaw = 3463;

		// Token: 0x040032D5 RID: 13013
		public const short StardustDrill = 3464;

		// Token: 0x040032D6 RID: 13014
		public const short StardustHammer = 3465;

		// Token: 0x040032D7 RID: 13015
		public const short StardustPickaxe = 3466;

		// Token: 0x040032D8 RID: 13016
		public const short LunarBar = 3467;

		// Token: 0x040032D9 RID: 13017
		public const short WingsSolar = 3468;

		// Token: 0x040032DA RID: 13018
		public const short WingsVortex = 3469;

		// Token: 0x040032DB RID: 13019
		public const short WingsNebula = 3470;

		// Token: 0x040032DC RID: 13020
		public const short WingsStardust = 3471;

		// Token: 0x040032DD RID: 13021
		public const short LunarBrickWall = 3472;

		// Token: 0x040032DE RID: 13022
		public const short SolarEruption = 3473;

		// Token: 0x040032DF RID: 13023
		public const short StardustCellStaff = 3474;

		// Token: 0x040032E0 RID: 13024
		public const short VortexBeater = 3475;

		// Token: 0x040032E1 RID: 13025
		public const short NebulaArcanum = 3476;

		// Token: 0x040032E2 RID: 13026
		public const short BloodWater = 3477;

		// Token: 0x040032E3 RID: 13027
		public const short TheBrideHat = 3478;

		// Token: 0x040032E4 RID: 13028
		public const short TheBrideDress = 3479;

		// Token: 0x040032E5 RID: 13029
		public const short PlatinumBow = 3480;

		// Token: 0x040032E6 RID: 13030
		public const short PlatinumHammer = 3481;

		// Token: 0x040032E7 RID: 13031
		public const short PlatinumAxe = 3482;

		// Token: 0x040032E8 RID: 13032
		public const short PlatinumShortsword = 3483;

		// Token: 0x040032E9 RID: 13033
		public const short PlatinumBroadsword = 3484;

		// Token: 0x040032EA RID: 13034
		public const short PlatinumPickaxe = 3485;

		// Token: 0x040032EB RID: 13035
		public const short TungstenBow = 3486;

		// Token: 0x040032EC RID: 13036
		public const short TungstenHammer = 3487;

		// Token: 0x040032ED RID: 13037
		public const short TungstenAxe = 3488;

		// Token: 0x040032EE RID: 13038
		public const short TungstenShortsword = 3489;

		// Token: 0x040032EF RID: 13039
		public const short TungstenBroadsword = 3490;

		// Token: 0x040032F0 RID: 13040
		public const short TungstenPickaxe = 3491;

		// Token: 0x040032F1 RID: 13041
		public const short LeadBow = 3492;

		// Token: 0x040032F2 RID: 13042
		public const short LeadHammer = 3493;

		// Token: 0x040032F3 RID: 13043
		public const short LeadAxe = 3494;

		// Token: 0x040032F4 RID: 13044
		public const short LeadShortsword = 3495;

		// Token: 0x040032F5 RID: 13045
		public const short LeadBroadsword = 3496;

		// Token: 0x040032F6 RID: 13046
		public const short LeadPickaxe = 3497;

		// Token: 0x040032F7 RID: 13047
		public const short TinBow = 3498;

		// Token: 0x040032F8 RID: 13048
		public const short TinHammer = 3499;

		// Token: 0x040032F9 RID: 13049
		public const short TinAxe = 3500;

		// Token: 0x040032FA RID: 13050
		public const short TinShortsword = 3501;

		// Token: 0x040032FB RID: 13051
		public const short TinBroadsword = 3502;

		// Token: 0x040032FC RID: 13052
		public const short TinPickaxe = 3503;

		// Token: 0x040032FD RID: 13053
		public const short CopperBow = 3504;

		// Token: 0x040032FE RID: 13054
		public const short CopperHammer = 3505;

		// Token: 0x040032FF RID: 13055
		public const short CopperAxe = 3506;

		// Token: 0x04003300 RID: 13056
		public const short CopperShortsword = 3507;

		// Token: 0x04003301 RID: 13057
		public const short CopperBroadsword = 3508;

		// Token: 0x04003302 RID: 13058
		public const short CopperPickaxe = 3509;

		// Token: 0x04003303 RID: 13059
		public const short SilverBow = 3510;

		// Token: 0x04003304 RID: 13060
		public const short SilverHammer = 3511;

		// Token: 0x04003305 RID: 13061
		public const short SilverAxe = 3512;

		// Token: 0x04003306 RID: 13062
		public const short SilverShortsword = 3513;

		// Token: 0x04003307 RID: 13063
		public const short SilverBroadsword = 3514;

		// Token: 0x04003308 RID: 13064
		public const short SilverPickaxe = 3515;

		// Token: 0x04003309 RID: 13065
		public const short GoldBow = 3516;

		// Token: 0x0400330A RID: 13066
		public const short GoldHammer = 3517;

		// Token: 0x0400330B RID: 13067
		public const short GoldAxe = 3518;

		// Token: 0x0400330C RID: 13068
		public const short GoldShortsword = 3519;

		// Token: 0x0400330D RID: 13069
		public const short GoldBroadsword = 3520;

		// Token: 0x0400330E RID: 13070
		public const short GoldPickaxe = 3521;

		// Token: 0x0400330F RID: 13071
		public const short LunarHamaxeSolar = 3522;

		// Token: 0x04003310 RID: 13072
		public const short LunarHamaxeVortex = 3523;

		// Token: 0x04003311 RID: 13073
		public const short LunarHamaxeNebula = 3524;

		// Token: 0x04003312 RID: 13074
		public const short LunarHamaxeStardust = 3525;

		// Token: 0x04003313 RID: 13075
		public const short SolarDye = 3526;

		// Token: 0x04003314 RID: 13076
		public const short NebulaDye = 3527;

		// Token: 0x04003315 RID: 13077
		public const short VortexDye = 3528;

		// Token: 0x04003316 RID: 13078
		public const short StardustDye = 3529;

		// Token: 0x04003317 RID: 13079
		public const short VoidDye = 3530;

		// Token: 0x04003318 RID: 13080
		public const short StardustDragonStaff = 3531;

		// Token: 0x04003319 RID: 13081
		public const short Bacon = 3532;

		// Token: 0x0400331A RID: 13082
		public const short ShiftingSandsDye = 3533;

		// Token: 0x0400331B RID: 13083
		public const short MirageDye = 3534;

		// Token: 0x0400331C RID: 13084
		public const short ShiftingPearlSandsDye = 3535;

		// Token: 0x0400331D RID: 13085
		public const short VortexMonolith = 3536;

		// Token: 0x0400331E RID: 13086
		public const short NebulaMonolith = 3537;

		// Token: 0x0400331F RID: 13087
		public const short StardustMonolith = 3538;

		// Token: 0x04003320 RID: 13088
		public const short SolarMonolith = 3539;

		// Token: 0x04003321 RID: 13089
		public const short Phantasm = 3540;

		// Token: 0x04003322 RID: 13090
		public const short LastPrism = 3541;

		// Token: 0x04003323 RID: 13091
		public const short NebulaBlaze = 3542;

		// Token: 0x04003324 RID: 13092
		public const short DayBreak = 3543;

		// Token: 0x04003325 RID: 13093
		public const short SuperHealingPotion = 3544;

		// Token: 0x04003326 RID: 13094
		public const short Detonator = 3545;

		// Token: 0x04003327 RID: 13095
		public const short FireworksLauncher = 3546;

		// Token: 0x04003328 RID: 13096
		public const short BouncyDynamite = 3547;

		// Token: 0x04003329 RID: 13097
		public const short PartyGirlGrenade = 3548;

		// Token: 0x0400332A RID: 13098
		public const short LunarCraftingStation = 3549;

		// Token: 0x0400332B RID: 13099
		public const short FlameAndSilverDye = 3550;

		// Token: 0x0400332C RID: 13100
		public const short GreenFlameAndSilverDye = 3551;

		// Token: 0x0400332D RID: 13101
		public const short BlueFlameAndSilverDye = 3552;

		// Token: 0x0400332E RID: 13102
		public const short ReflectiveCopperDye = 3553;

		// Token: 0x0400332F RID: 13103
		public const short ReflectiveObsidianDye = 3554;

		// Token: 0x04003330 RID: 13104
		public const short ReflectiveMetalDye = 3555;

		// Token: 0x04003331 RID: 13105
		public const short MidnightRainbowDye = 3556;

		// Token: 0x04003332 RID: 13106
		public const short BlackAndWhiteDye = 3557;

		// Token: 0x04003333 RID: 13107
		public const short BrightSilverDye = 3558;

		// Token: 0x04003334 RID: 13108
		public const short SilverAndBlackDye = 3559;

		// Token: 0x04003335 RID: 13109
		public const short RedAcidDye = 3560;

		// Token: 0x04003336 RID: 13110
		public const short GelDye = 3561;

		// Token: 0x04003337 RID: 13111
		public const short PinkGelDye = 3562;

		// Token: 0x04003338 RID: 13112
		public const short SquirrelRed = 3563;

		// Token: 0x04003339 RID: 13113
		public const short SquirrelGold = 3564;

		// Token: 0x0400333A RID: 13114
		public const short SquirrelOrangeCage = 3565;

		// Token: 0x0400333B RID: 13115
		public const short SquirrelGoldCage = 3566;

		// Token: 0x0400333C RID: 13116
		public const short MoonlordBullet = 3567;

		// Token: 0x0400333D RID: 13117
		public const short MoonlordArrow = 3568;

		// Token: 0x0400333E RID: 13118
		public const short MoonlordTurretStaff = 3569;

		// Token: 0x0400333F RID: 13119
		public const short LunarFlareBook = 3570;

		// Token: 0x04003340 RID: 13120
		public const short RainbowCrystalStaff = 3571;

		// Token: 0x04003341 RID: 13121
		public const short LunarHook = 3572;

		// Token: 0x04003342 RID: 13122
		public const short LunarBlockSolar = 3573;

		// Token: 0x04003343 RID: 13123
		public const short LunarBlockVortex = 3574;

		// Token: 0x04003344 RID: 13124
		public const short LunarBlockNebula = 3575;

		// Token: 0x04003345 RID: 13125
		public const short LunarBlockStardust = 3576;

		// Token: 0x04003346 RID: 13126
		public const short SuspiciousLookingTentacle = 3577;

		// Token: 0x04003347 RID: 13127
		public const short Yoraiz0rShirt = 3578;

		// Token: 0x04003348 RID: 13128
		public const short Yoraiz0rPants = 3579;

		// Token: 0x04003349 RID: 13129
		public const short Yoraiz0rWings = 3580;

		// Token: 0x0400334A RID: 13130
		public const short Yoraiz0rDarkness = 3581;

		// Token: 0x0400334B RID: 13131
		public const short JimsWings = 3582;

		// Token: 0x0400334C RID: 13132
		public const short Yoraiz0rHead = 3583;

		// Token: 0x0400334D RID: 13133
		public const short LivingLeafWall = 3584;

		// Token: 0x0400334E RID: 13134
		public const short SkiphsHelm = 3585;

		// Token: 0x0400334F RID: 13135
		public const short SkiphsShirt = 3586;

		// Token: 0x04003350 RID: 13136
		public const short SkiphsPants = 3587;

		// Token: 0x04003351 RID: 13137
		public const short SkiphsWings = 3588;

		// Token: 0x04003352 RID: 13138
		public const short LokisHelm = 3589;

		// Token: 0x04003353 RID: 13139
		public const short LokisShirt = 3590;

		// Token: 0x04003354 RID: 13140
		public const short LokisPants = 3591;

		// Token: 0x04003355 RID: 13141
		public const short LokisWings = 3592;

		// Token: 0x04003356 RID: 13142
		public const short SandSlimeBanner = 3593;

		// Token: 0x04003357 RID: 13143
		public const short SeaSnailBanner = 3594;

		// Token: 0x04003358 RID: 13144
		public const short MoonLordTrophy = 3595;

		// Token: 0x04003359 RID: 13145
		public const short MoonLordPainting = 3596;

		// Token: 0x0400335A RID: 13146
		public const short BurningHadesDye = 3597;

		// Token: 0x0400335B RID: 13147
		public const short GrimDye = 3598;

		// Token: 0x0400335C RID: 13148
		public const short LokisDye = 3599;

		// Token: 0x0400335D RID: 13149
		public const short ShadowflameHadesDye = 3600;

		// Token: 0x0400335E RID: 13150
		public const short CelestialSigil = 3601;

		// Token: 0x0400335F RID: 13151
		public const short LogicGateLamp_Off = 3602;

		// Token: 0x04003360 RID: 13152
		public const short LogicGate_AND = 3603;

		// Token: 0x04003361 RID: 13153
		public const short LogicGate_OR = 3604;

		// Token: 0x04003362 RID: 13154
		public const short LogicGate_NAND = 3605;

		// Token: 0x04003363 RID: 13155
		public const short LogicGate_NOR = 3606;

		// Token: 0x04003364 RID: 13156
		public const short LogicGate_XOR = 3607;

		// Token: 0x04003365 RID: 13157
		public const short LogicGate_NXOR = 3608;

		// Token: 0x04003366 RID: 13158
		public const short ConveyorBeltLeft = 3609;

		// Token: 0x04003367 RID: 13159
		public const short ConveyorBeltRight = 3610;

		// Token: 0x04003368 RID: 13160
		public const short WireKite = 3611;

		// Token: 0x04003369 RID: 13161
		public const short YellowWrench = 3612;

		// Token: 0x0400336A RID: 13162
		public const short LogicSensor_Sun = 3613;

		// Token: 0x0400336B RID: 13163
		public const short LogicSensor_Moon = 3614;

		// Token: 0x0400336C RID: 13164
		public const short LogicSensor_Above = 3615;

		// Token: 0x0400336D RID: 13165
		public const short WirePipe = 3616;

		// Token: 0x0400336E RID: 13166
		public const short AnnouncementBox = 3617;

		// Token: 0x0400336F RID: 13167
		public const short LogicGateLamp_On = 3618;

		// Token: 0x04003370 RID: 13168
		public const short MechanicalLens = 3619;

		// Token: 0x04003371 RID: 13169
		public const short ActuationRod = 3620;

		// Token: 0x04003372 RID: 13170
		public const short TeamBlockRed = 3621;

		// Token: 0x04003373 RID: 13171
		public const short TeamBlockRedPlatform = 3622;

		// Token: 0x04003374 RID: 13172
		public const short StaticHook = 3623;

		// Token: 0x04003375 RID: 13173
		public const short ActuationAccessory = 3624;

		// Token: 0x04003376 RID: 13174
		public const short MulticolorWrench = 3625;

		// Token: 0x04003377 RID: 13175
		public const short WeightedPressurePlatePink = 3626;

		// Token: 0x04003378 RID: 13176
		public const short EngineeringHelmet = 3627;

		// Token: 0x04003379 RID: 13177
		public const short CompanionCube = 3628;

		// Token: 0x0400337A RID: 13178
		public const short WireBulb = 3629;

		// Token: 0x0400337B RID: 13179
		public const short WeightedPressurePlateOrange = 3630;

		// Token: 0x0400337C RID: 13180
		public const short WeightedPressurePlatePurple = 3631;

		// Token: 0x0400337D RID: 13181
		public const short WeightedPressurePlateCyan = 3632;

		// Token: 0x0400337E RID: 13182
		public const short TeamBlockGreen = 3633;

		// Token: 0x0400337F RID: 13183
		public const short TeamBlockBlue = 3634;

		// Token: 0x04003380 RID: 13184
		public const short TeamBlockYellow = 3635;

		// Token: 0x04003381 RID: 13185
		public const short TeamBlockPink = 3636;

		// Token: 0x04003382 RID: 13186
		public const short TeamBlockWhite = 3637;

		// Token: 0x04003383 RID: 13187
		public const short TeamBlockGreenPlatform = 3638;

		// Token: 0x04003384 RID: 13188
		public const short TeamBlockBluePlatform = 3639;

		// Token: 0x04003385 RID: 13189
		public const short TeamBlockYellowPlatform = 3640;

		// Token: 0x04003386 RID: 13190
		public const short TeamBlockPinkPlatform = 3641;

		// Token: 0x04003387 RID: 13191
		public const short TeamBlockWhitePlatform = 3642;

		// Token: 0x04003388 RID: 13192
		public const short LargeAmber = 3643;

		// Token: 0x04003389 RID: 13193
		public const short GemLockRuby = 3644;

		// Token: 0x0400338A RID: 13194
		public const short GemLockSapphire = 3645;

		// Token: 0x0400338B RID: 13195
		public const short GemLockEmerald = 3646;

		// Token: 0x0400338C RID: 13196
		public const short GemLockTopaz = 3647;

		// Token: 0x0400338D RID: 13197
		public const short GemLockAmethyst = 3648;

		// Token: 0x0400338E RID: 13198
		public const short GemLockDiamond = 3649;

		// Token: 0x0400338F RID: 13199
		public const short GemLockAmber = 3650;

		// Token: 0x04003390 RID: 13200
		public const short SquirrelStatue = 3651;

		// Token: 0x04003391 RID: 13201
		public const short ButterflyStatue = 3652;

		// Token: 0x04003392 RID: 13202
		public const short WormStatue = 3653;

		// Token: 0x04003393 RID: 13203
		public const short FireflyStatue = 3654;

		// Token: 0x04003394 RID: 13204
		public const short ScorpionStatue = 3655;

		// Token: 0x04003395 RID: 13205
		public const short SnailStatue = 3656;

		// Token: 0x04003396 RID: 13206
		public const short GrasshopperStatue = 3657;

		// Token: 0x04003397 RID: 13207
		public const short MouseStatue = 3658;

		// Token: 0x04003398 RID: 13208
		public const short DuckStatue = 3659;

		// Token: 0x04003399 RID: 13209
		public const short PenguinStatue = 3660;

		// Token: 0x0400339A RID: 13210
		public const short FrogStatue = 3661;

		// Token: 0x0400339B RID: 13211
		public const short BuggyStatue = 3662;

		// Token: 0x0400339C RID: 13212
		public const short LogicGateLamp_Faulty = 3663;

		// Token: 0x0400339D RID: 13213
		public const short PortalGunStation = 3664;

		// Token: 0x0400339E RID: 13214
		public const short Fake_Chest = 3665;

		// Token: 0x0400339F RID: 13215
		public const short Fake_GoldChest = 3666;

		// Token: 0x040033A0 RID: 13216
		public const short Fake_ShadowChest = 3667;

		// Token: 0x040033A1 RID: 13217
		public const short Fake_EbonwoodChest = 3668;

		// Token: 0x040033A2 RID: 13218
		public const short Fake_RichMahoganyChest = 3669;

		// Token: 0x040033A3 RID: 13219
		public const short Fake_PearlwoodChest = 3670;

		// Token: 0x040033A4 RID: 13220
		public const short Fake_IvyChest = 3671;

		// Token: 0x040033A5 RID: 13221
		public const short Fake_IceChest = 3672;

		// Token: 0x040033A6 RID: 13222
		public const short Fake_LivingWoodChest = 3673;

		// Token: 0x040033A7 RID: 13223
		public const short Fake_SkywareChest = 3674;

		// Token: 0x040033A8 RID: 13224
		public const short Fake_ShadewoodChest = 3675;

		// Token: 0x040033A9 RID: 13225
		public const short Fake_WebCoveredChest = 3676;

		// Token: 0x040033AA RID: 13226
		public const short Fake_LihzahrdChest = 3677;

		// Token: 0x040033AB RID: 13227
		public const short Fake_WaterChest = 3678;

		// Token: 0x040033AC RID: 13228
		public const short Fake_JungleChest = 3679;

		// Token: 0x040033AD RID: 13229
		public const short Fake_CorruptionChest = 3680;

		// Token: 0x040033AE RID: 13230
		public const short Fake_CrimsonChest = 3681;

		// Token: 0x040033AF RID: 13231
		public const short Fake_HallowedChest = 3682;

		// Token: 0x040033B0 RID: 13232
		public const short Fake_FrozenChest = 3683;

		// Token: 0x040033B1 RID: 13233
		public const short Fake_DynastyChest = 3684;

		// Token: 0x040033B2 RID: 13234
		public const short Fake_HoneyChest = 3685;

		// Token: 0x040033B3 RID: 13235
		public const short Fake_SteampunkChest = 3686;

		// Token: 0x040033B4 RID: 13236
		public const short Fake_PalmWoodChest = 3687;

		// Token: 0x040033B5 RID: 13237
		public const short Fake_MushroomChest = 3688;

		// Token: 0x040033B6 RID: 13238
		public const short Fake_BorealWoodChest = 3689;

		// Token: 0x040033B7 RID: 13239
		public const short Fake_SlimeChest = 3690;

		// Token: 0x040033B8 RID: 13240
		public const short Fake_GreenDungeonChest = 3691;

		// Token: 0x040033B9 RID: 13241
		public const short Fake_PinkDungeonChest = 3692;

		// Token: 0x040033BA RID: 13242
		public const short Fake_BlueDungeonChest = 3693;

		// Token: 0x040033BB RID: 13243
		public const short Fake_BoneChest = 3694;

		// Token: 0x040033BC RID: 13244
		public const short Fake_CactusChest = 3695;

		// Token: 0x040033BD RID: 13245
		public const short Fake_FleshChest = 3696;

		// Token: 0x040033BE RID: 13246
		public const short Fake_ObsidianChest = 3697;

		// Token: 0x040033BF RID: 13247
		public const short Fake_PumpkinChest = 3698;

		// Token: 0x040033C0 RID: 13248
		public const short Fake_SpookyChest = 3699;

		// Token: 0x040033C1 RID: 13249
		public const short Fake_GlassChest = 3700;

		// Token: 0x040033C2 RID: 13250
		public const short Fake_MartianChest = 3701;

		// Token: 0x040033C3 RID: 13251
		public const short Fake_MeteoriteChest = 3702;

		// Token: 0x040033C4 RID: 13252
		public const short Fake_GraniteChest = 3703;

		// Token: 0x040033C5 RID: 13253
		public const short Fake_MarbleChest = 3704;

		// Token: 0x040033C6 RID: 13254
		public const short Fake_newchest1 = 3705;

		// Token: 0x040033C7 RID: 13255
		public const short Fake_newchest2 = 3706;

		// Token: 0x040033C8 RID: 13256
		public const short ProjectilePressurePad = 3707;

		// Token: 0x040033C9 RID: 13257
		public const short WallCreeperStatue = 3708;

		// Token: 0x040033CA RID: 13258
		public const short UnicornStatue = 3709;

		// Token: 0x040033CB RID: 13259
		public const short DripplerStatue = 3710;

		// Token: 0x040033CC RID: 13260
		public const short WraithStatue = 3711;

		// Token: 0x040033CD RID: 13261
		public const short BoneSkeletonStatue = 3712;

		// Token: 0x040033CE RID: 13262
		public const short UndeadVikingStatue = 3713;

		// Token: 0x040033CF RID: 13263
		public const short MedusaStatue = 3714;

		// Token: 0x040033D0 RID: 13264
		public const short HarpyStatue = 3715;

		// Token: 0x040033D1 RID: 13265
		public const short PigronStatue = 3716;

		// Token: 0x040033D2 RID: 13266
		public const short HopliteStatue = 3717;

		// Token: 0x040033D3 RID: 13267
		public const short GraniteGolemStatue = 3718;

		// Token: 0x040033D4 RID: 13268
		public const short ZombieArmStatue = 3719;

		// Token: 0x040033D5 RID: 13269
		public const short BloodZombieStatue = 3720;

		// Token: 0x040033D6 RID: 13270
		public const short AnglerTackleBag = 3721;

		// Token: 0x040033D7 RID: 13271
		public const short GeyserTrap = 3722;

		// Token: 0x040033D8 RID: 13272
		public const short UltraBrightCampfire = 3723;

		// Token: 0x040033D9 RID: 13273
		public const short BoneCampfire = 3724;

		// Token: 0x040033DA RID: 13274
		public const short PixelBox = 3725;

		// Token: 0x040033DB RID: 13275
		public const short LogicSensor_Water = 3726;

		// Token: 0x040033DC RID: 13276
		public const short LogicSensor_Lava = 3727;

		// Token: 0x040033DD RID: 13277
		public const short LogicSensor_Honey = 3728;

		// Token: 0x040033DE RID: 13278
		public const short LogicSensor_Liquid = 3729;

		// Token: 0x040033DF RID: 13279
		public const short PartyBundleOfBalloonsAccessory = 3730;

		// Token: 0x040033E0 RID: 13280
		public const short PartyBalloonAnimal = 3731;

		// Token: 0x040033E1 RID: 13281
		public const short PartyHat = 3732;

		// Token: 0x040033E2 RID: 13282
		public const short FlowerBoyHat = 3733;

		// Token: 0x040033E3 RID: 13283
		public const short FlowerBoyShirt = 3734;

		// Token: 0x040033E4 RID: 13284
		public const short FlowerBoyPants = 3735;

		// Token: 0x040033E5 RID: 13285
		public const short SillyBalloonPink = 3736;

		// Token: 0x040033E6 RID: 13286
		public const short SillyBalloonPurple = 3737;

		// Token: 0x040033E7 RID: 13287
		public const short SillyBalloonGreen = 3738;

		// Token: 0x040033E8 RID: 13288
		public const short SillyStreamerBlue = 3739;

		// Token: 0x040033E9 RID: 13289
		public const short SillyStreamerGreen = 3740;

		// Token: 0x040033EA RID: 13290
		public const short SillyStreamerPink = 3741;

		// Token: 0x040033EB RID: 13291
		public const short SillyBalloonMachine = 3742;

		// Token: 0x040033EC RID: 13292
		public const short SillyBalloonTiedPink = 3743;

		// Token: 0x040033ED RID: 13293
		public const short SillyBalloonTiedPurple = 3744;

		// Token: 0x040033EE RID: 13294
		public const short SillyBalloonTiedGreen = 3745;

		// Token: 0x040033EF RID: 13295
		public const short Pigronata = 3746;

		// Token: 0x040033F0 RID: 13296
		public const short PartyMonolith = 3747;

		// Token: 0x040033F1 RID: 13297
		public const short PartyBundleOfBalloonTile = 3748;

		// Token: 0x040033F2 RID: 13298
		public const short PartyPresent = 3749;

		// Token: 0x040033F3 RID: 13299
		public const short SliceOfCake = 3750;

		// Token: 0x040033F4 RID: 13300
		public const short CogWall = 3751;

		// Token: 0x040033F5 RID: 13301
		public const short SandFallWall = 3752;

		// Token: 0x040033F6 RID: 13302
		public const short SnowFallWall = 3753;

		// Token: 0x040033F7 RID: 13303
		public const short SandFallBlock = 3754;

		// Token: 0x040033F8 RID: 13304
		public const short SnowFallBlock = 3755;

		// Token: 0x040033F9 RID: 13305
		public const short SnowCloudBlock = 3756;

		// Token: 0x040033FA RID: 13306
		public const short PedguinHat = 3757;

		// Token: 0x040033FB RID: 13307
		public const short PedguinShirt = 3758;

		// Token: 0x040033FC RID: 13308
		public const short PedguinPants = 3759;

		// Token: 0x040033FD RID: 13309
		public const short SillyBalloonPinkWall = 3760;

		// Token: 0x040033FE RID: 13310
		public const short SillyBalloonPurpleWall = 3761;

		// Token: 0x040033FF RID: 13311
		public const short SillyBalloonGreenWall = 3762;

		// Token: 0x04003400 RID: 13312
		public const short AviatorSunglasses = 3763;

		// Token: 0x04003401 RID: 13313
		public const short BluePhasesaber = 3764;

		// Token: 0x04003402 RID: 13314
		public const short RedPhasesaber = 3765;

		// Token: 0x04003403 RID: 13315
		public const short GreenPhasesaber = 3766;

		// Token: 0x04003404 RID: 13316
		public const short PurplePhasesaber = 3767;

		// Token: 0x04003405 RID: 13317
		public const short WhitePhasesaber = 3768;

		// Token: 0x04003406 RID: 13318
		public const short YellowPhasesaber = 3769;

		// Token: 0x04003407 RID: 13319
		public const short DjinnsCurse = 3770;

		// Token: 0x04003408 RID: 13320
		public const short AncientHorn = 3771;

		// Token: 0x04003409 RID: 13321
		public const short AntlionClaw = 3772;

		// Token: 0x0400340A RID: 13322
		public const short AncientArmorHat = 3773;

		// Token: 0x0400340B RID: 13323
		public const short AncientArmorShirt = 3774;

		// Token: 0x0400340C RID: 13324
		public const short AncientArmorPants = 3775;

		// Token: 0x0400340D RID: 13325
		public const short AncientBattleArmorHat = 3776;

		// Token: 0x0400340E RID: 13326
		public const short AncientBattleArmorShirt = 3777;

		// Token: 0x0400340F RID: 13327
		public const short AncientBattleArmorPants = 3778;

		// Token: 0x04003410 RID: 13328
		public const short SpiritFlame = 3779;

		// Token: 0x04003411 RID: 13329
		public const short SandElementalBanner = 3780;

		// Token: 0x04003412 RID: 13330
		public const short PocketMirror = 3781;

		// Token: 0x04003413 RID: 13331
		public const short MagicSandDropper = 3782;

		// Token: 0x04003414 RID: 13332
		public const short AncientBattleArmorMaterial = 3783;

		// Token: 0x04003415 RID: 13333
		public const short LamiaPants = 3784;

		// Token: 0x04003416 RID: 13334
		public const short LamiaShirt = 3785;

		// Token: 0x04003417 RID: 13335
		public const short LamiaHat = 3786;

		// Token: 0x04003418 RID: 13336
		public const short SkyFracture = 3787;

		// Token: 0x04003419 RID: 13337
		public const short OnyxBlaster = 3788;

		// Token: 0x0400341A RID: 13338
		public const short SandsharkBanner = 3789;

		// Token: 0x0400341B RID: 13339
		public const short SandsharkCorruptBanner = 3790;

		// Token: 0x0400341C RID: 13340
		public const short SandsharkCrimsonBanner = 3791;

		// Token: 0x0400341D RID: 13341
		public const short SandsharkHallowedBanner = 3792;

		// Token: 0x0400341E RID: 13342
		public const short TumbleweedBanner = 3793;

		// Token: 0x0400341F RID: 13343
		public const short AncientCloth = 3794;

		// Token: 0x04003420 RID: 13344
		public const short DjinnLamp = 3795;

		// Token: 0x04003421 RID: 13345
		public const short MusicBoxSandstorm = 3796;

		// Token: 0x04003422 RID: 13346
		public const short ApprenticeHat = 3797;

		// Token: 0x04003423 RID: 13347
		public const short ApprenticeRobe = 3798;

		// Token: 0x04003424 RID: 13348
		public const short ApprenticeTrousers = 3799;

		// Token: 0x04003425 RID: 13349
		public const short SquireGreatHelm = 3800;

		// Token: 0x04003426 RID: 13350
		public const short SquirePlating = 3801;

		// Token: 0x04003427 RID: 13351
		public const short SquireGreaves = 3802;

		// Token: 0x04003428 RID: 13352
		public const short HuntressWig = 3803;

		// Token: 0x04003429 RID: 13353
		public const short HuntressJerkin = 3804;

		// Token: 0x0400342A RID: 13354
		public const short HuntressPants = 3805;

		// Token: 0x0400342B RID: 13355
		public const short MonkBrows = 3806;

		// Token: 0x0400342C RID: 13356
		public const short MonkShirt = 3807;

		// Token: 0x0400342D RID: 13357
		public const short MonkPants = 3808;

		// Token: 0x0400342E RID: 13358
		public const short ApprenticeScarf = 3809;

		// Token: 0x0400342F RID: 13359
		public const short SquireShield = 3810;

		// Token: 0x04003430 RID: 13360
		public const short HuntressBuckler = 3811;

		// Token: 0x04003431 RID: 13361
		public const short MonkBelt = 3812;

		// Token: 0x04003432 RID: 13362
		public const short DefendersForge = 3813;

		// Token: 0x04003433 RID: 13363
		public const short WarTable = 3814;

		// Token: 0x04003434 RID: 13364
		public const short WarTableBanner = 3815;

		// Token: 0x04003435 RID: 13365
		public const short DD2ElderCrystalStand = 3816;

		// Token: 0x04003436 RID: 13366
		public const short DefenderMedal = 3817;

		// Token: 0x04003437 RID: 13367
		public const short DD2FlameburstTowerT1Popper = 3818;

		// Token: 0x04003438 RID: 13368
		public const short DD2FlameburstTowerT2Popper = 3819;

		// Token: 0x04003439 RID: 13369
		public const short DD2FlameburstTowerT3Popper = 3820;

		// Token: 0x0400343A RID: 13370
		public const short AleThrowingGlove = 3821;

		// Token: 0x0400343B RID: 13371
		public const short DD2EnergyCrystal = 3822;

		// Token: 0x0400343C RID: 13372
		public const short DD2SquireDemonSword = 3823;

		// Token: 0x0400343D RID: 13373
		public const short DD2BallistraTowerT1Popper = 3824;

		// Token: 0x0400343E RID: 13374
		public const short DD2BallistraTowerT2Popper = 3825;

		// Token: 0x0400343F RID: 13375
		public const short DD2BallistraTowerT3Popper = 3826;

		// Token: 0x04003440 RID: 13376
		public const short DD2SquireBetsySword = 3827;

		// Token: 0x04003441 RID: 13377
		public const short DD2ElderCrystal = 3828;

		// Token: 0x04003442 RID: 13378
		public const short DD2LightningAuraT1Popper = 3829;

		// Token: 0x04003443 RID: 13379
		public const short DD2LightningAuraT2Popper = 3830;

		// Token: 0x04003444 RID: 13380
		public const short DD2LightningAuraT3Popper = 3831;

		// Token: 0x04003445 RID: 13381
		public const short DD2ExplosiveTrapT1Popper = 3832;

		// Token: 0x04003446 RID: 13382
		public const short DD2ExplosiveTrapT2Popper = 3833;

		// Token: 0x04003447 RID: 13383
		public const short DD2ExplosiveTrapT3Popper = 3834;

		// Token: 0x04003448 RID: 13384
		public const short MonkStaffT1 = 3835;

		// Token: 0x04003449 RID: 13385
		public const short MonkStaffT2 = 3836;

		// Token: 0x0400344A RID: 13386
		public const short DD2GoblinBomberBanner = 3837;

		// Token: 0x0400344B RID: 13387
		public const short DD2GoblinBanner = 3838;

		// Token: 0x0400344C RID: 13388
		public const short DD2SkeletonBanner = 3839;

		// Token: 0x0400344D RID: 13389
		public const short DD2DrakinBanner = 3840;

		// Token: 0x0400344E RID: 13390
		public const short DD2KoboldFlyerBanner = 3841;

		// Token: 0x0400344F RID: 13391
		public const short DD2KoboldBanner = 3842;

		// Token: 0x04003450 RID: 13392
		public const short DD2WitherBeastBanner = 3843;

		// Token: 0x04003451 RID: 13393
		public const short DD2WyvernBanner = 3844;

		// Token: 0x04003452 RID: 13394
		public const short DD2JavelinThrowerBanner = 3845;

		// Token: 0x04003453 RID: 13395
		public const short DD2LightningBugBanner = 3846;

		// Token: 0x04003454 RID: 13396
		public const short OgreMask = 3847;

		// Token: 0x04003455 RID: 13397
		public const short GoblinMask = 3848;

		// Token: 0x04003456 RID: 13398
		public const short GoblinBomberCap = 3849;

		// Token: 0x04003457 RID: 13399
		public const short EtherianJavelin = 3850;

		// Token: 0x04003458 RID: 13400
		public const short KoboldDynamiteBackpack = 3851;

		// Token: 0x04003459 RID: 13401
		public const short BookStaff = 3852;

		// Token: 0x0400345A RID: 13402
		public const short BoringBow = 3853;

		// Token: 0x0400345B RID: 13403
		public const short DD2PhoenixBow = 3854;

		// Token: 0x0400345C RID: 13404
		public const short DD2PetGato = 3855;

		// Token: 0x0400345D RID: 13405
		public const short DD2PetGhost = 3856;

		// Token: 0x0400345E RID: 13406
		public const short DD2PetDragon = 3857;

		// Token: 0x0400345F RID: 13407
		public const short MonkStaffT3 = 3858;

		// Token: 0x04003460 RID: 13408
		public const short DD2BetsyBow = 3859;

		// Token: 0x04003461 RID: 13409
		public const short BossBagBetsy = 3860;

		// Token: 0x04003462 RID: 13410
		public const short BossBagOgre = 3861;

		// Token: 0x04003463 RID: 13411
		public const short BossBagDarkMage = 3862;

		// Token: 0x04003464 RID: 13412
		public const short BossMaskBetsy = 3863;

		// Token: 0x04003465 RID: 13413
		public const short BossMaskDarkMage = 3864;

		// Token: 0x04003466 RID: 13414
		public const short BossMaskOgre = 3865;

		// Token: 0x04003467 RID: 13415
		public const short BossTrophyBetsy = 3866;

		// Token: 0x04003468 RID: 13416
		public const short BossTrophyDarkmage = 3867;

		// Token: 0x04003469 RID: 13417
		public const short BossTrophyOgre = 3868;

		// Token: 0x0400346A RID: 13418
		public const short MusicBoxDD2 = 3869;

		// Token: 0x0400346B RID: 13419
		public const short ApprenticeStaffT3 = 3870;

		// Token: 0x0400346C RID: 13420
		public const short SquireAltHead = 3871;

		// Token: 0x0400346D RID: 13421
		public const short SquireAltShirt = 3872;

		// Token: 0x0400346E RID: 13422
		public const short SquireAltPants = 3873;

		// Token: 0x0400346F RID: 13423
		public const short ApprenticeAltHead = 3874;

		// Token: 0x04003470 RID: 13424
		public const short ApprenticeAltShirt = 3875;

		// Token: 0x04003471 RID: 13425
		public const short ApprenticeAltPants = 3876;

		// Token: 0x04003472 RID: 13426
		public const short HuntressAltHead = 3877;

		// Token: 0x04003473 RID: 13427
		public const short HuntressAltShirt = 3878;

		// Token: 0x04003474 RID: 13428
		public const short HuntressAltPants = 3879;

		// Token: 0x04003475 RID: 13429
		public const short MonkAltHead = 3880;

		// Token: 0x04003476 RID: 13430
		public const short MonkAltShirt = 3881;

		// Token: 0x04003477 RID: 13431
		public const short MonkAltPants = 3882;

		// Token: 0x04003478 RID: 13432
		public const short BetsyWings = 3883;

		// Token: 0x04003479 RID: 13433
		public const short CrystalChest = 3884;

		// Token: 0x0400347A RID: 13434
		public const short GoldenChest = 3885;

		// Token: 0x0400347B RID: 13435
		public const short Fake_CrystalChest = 3886;

		// Token: 0x0400347C RID: 13436
		public const short Fake_GoldenChest = 3887;

		// Token: 0x0400347D RID: 13437
		public const short CrystalDoor = 3888;

		// Token: 0x0400347E RID: 13438
		public const short CrystalChair = 3889;

		// Token: 0x0400347F RID: 13439
		public const short CrystalCandle = 3890;

		// Token: 0x04003480 RID: 13440
		public const short CrystalLantern = 3891;

		// Token: 0x04003481 RID: 13441
		public const short CrystalLamp = 3892;

		// Token: 0x04003482 RID: 13442
		public const short CrystalCandelabra = 3893;

		// Token: 0x04003483 RID: 13443
		public const short CrystalChandelier = 3894;

		// Token: 0x04003484 RID: 13444
		public const short CrystalBathtub = 3895;

		// Token: 0x04003485 RID: 13445
		public const short CrystalSink = 3896;

		// Token: 0x04003486 RID: 13446
		public const short CrystalBed = 3897;

		// Token: 0x04003487 RID: 13447
		public const short CrystalClock = 3898;

		// Token: 0x04003488 RID: 13448
		public const short SkywareClock2 = 3899;

		// Token: 0x04003489 RID: 13449
		public const short DungeonClockBlue = 3900;

		// Token: 0x0400348A RID: 13450
		public const short DungeonClockGreen = 3901;

		// Token: 0x0400348B RID: 13451
		public const short DungeonClockPink = 3902;

		// Token: 0x0400348C RID: 13452
		public const short CrystalPlatform = 3903;

		// Token: 0x0400348D RID: 13453
		public const short GoldenPlatform = 3904;

		// Token: 0x0400348E RID: 13454
		public const short DynastyPlatform = 3905;

		// Token: 0x0400348F RID: 13455
		public const short LihzahrdPlatform = 3906;

		// Token: 0x04003490 RID: 13456
		public const short FleshPlatform = 3907;

		// Token: 0x04003491 RID: 13457
		public const short FrozenPlatform = 3908;

		// Token: 0x04003492 RID: 13458
		public const short CrystalWorkbench = 3909;

		// Token: 0x04003493 RID: 13459
		public const short GoldenWorkbench = 3910;

		// Token: 0x04003494 RID: 13460
		public const short CrystalDresser = 3911;

		// Token: 0x04003495 RID: 13461
		public const short DynastyDresser = 3912;

		// Token: 0x04003496 RID: 13462
		public const short FrozenDresser = 3913;

		// Token: 0x04003497 RID: 13463
		public const short LivingWoodDresser = 3914;

		// Token: 0x04003498 RID: 13464
		public const short CrystalPiano = 3915;

		// Token: 0x04003499 RID: 13465
		public const short DynastyPiano = 3916;

		// Token: 0x0400349A RID: 13466
		public const short CrystalBookCase = 3917;

		// Token: 0x0400349B RID: 13467
		public const short CrystalSofaHowDoesThatEvenWork = 3918;

		// Token: 0x0400349C RID: 13468
		public const short DynastySofa = 3919;

		// Token: 0x0400349D RID: 13469
		public const short CrystalTable = 3920;

		// Token: 0x0400349E RID: 13470
		public const short ArkhalisHat = 3921;

		// Token: 0x0400349F RID: 13471
		public const short ArkhalisShirt = 3922;

		// Token: 0x040034A0 RID: 13472
		public const short ArkhalisPants = 3923;

		// Token: 0x040034A1 RID: 13473
		public const short ArkhalisWings = 3924;

		// Token: 0x040034A2 RID: 13474
		public const short LeinforsHat = 3925;

		// Token: 0x040034A3 RID: 13475
		public const short LeinforsShirt = 3926;

		// Token: 0x040034A4 RID: 13476
		public const short LeinforsPants = 3927;

		// Token: 0x040034A5 RID: 13477
		public const short LeinforsWings = 3928;

		// Token: 0x040034A6 RID: 13478
		public const short LeinforsAccessory = 3929;

		// Token: 0x040034A7 RID: 13479
		public const short Celeb2 = 3930;

		// Token: 0x040034A8 RID: 13480
		public const short SpiderBathtub = 3931;

		// Token: 0x040034A9 RID: 13481
		public const short SpiderBed = 3932;

		// Token: 0x040034AA RID: 13482
		public const short SpiderBookcase = 3933;

		// Token: 0x040034AB RID: 13483
		public const short SpiderDresser = 3934;

		// Token: 0x040034AC RID: 13484
		public const short SpiderCandelabra = 3935;

		// Token: 0x040034AD RID: 13485
		public const short SpiderCandle = 3936;

		// Token: 0x040034AE RID: 13486
		public const short SpiderChair = 3937;

		// Token: 0x040034AF RID: 13487
		public const short SpiderChandelier = 3938;

		// Token: 0x040034B0 RID: 13488
		public const short SpiderChest = 3939;

		// Token: 0x040034B1 RID: 13489
		public const short SpiderClock = 3940;

		// Token: 0x040034B2 RID: 13490
		public const short SpiderDoor = 3941;

		// Token: 0x040034B3 RID: 13491
		public const short SpiderLamp = 3942;

		// Token: 0x040034B4 RID: 13492
		public const short SpiderLantern = 3943;

		// Token: 0x040034B5 RID: 13493
		public const short SpiderPiano = 3944;

		// Token: 0x040034B6 RID: 13494
		public const short SpiderPlatform = 3945;

		// Token: 0x040034B7 RID: 13495
		public const short SpiderSinkSpiderSinkDoesWhateverASpiderSinkDoes = 3946;

		// Token: 0x040034B8 RID: 13496
		public const short SpiderSofa = 3947;

		// Token: 0x040034B9 RID: 13497
		public const short SpiderTable = 3948;

		// Token: 0x040034BA RID: 13498
		public const short SpiderWorkbench = 3949;

		// Token: 0x040034BB RID: 13499
		public const short Fake_SpiderChest = 3950;

		// Token: 0x040034BC RID: 13500
		public const short IronBrick = 3951;

		// Token: 0x040034BD RID: 13501
		public const short IronBrickWall = 3952;

		// Token: 0x040034BE RID: 13502
		public const short LeadBrick = 3953;

		// Token: 0x040034BF RID: 13503
		public const short LeadBrickWall = 3954;

		// Token: 0x040034C0 RID: 13504
		public const short LesionBlock = 3955;

		// Token: 0x040034C1 RID: 13505
		public const short LesionBlockWall = 3956;

		// Token: 0x040034C2 RID: 13506
		public const short LesionPlatform = 3957;

		// Token: 0x040034C3 RID: 13507
		public const short LesionBathtub = 3958;

		// Token: 0x040034C4 RID: 13508
		public const short LesionBed = 3959;

		// Token: 0x040034C5 RID: 13509
		public const short LesionBookcase = 3960;

		// Token: 0x040034C6 RID: 13510
		public const short LesionCandelabra = 3961;

		// Token: 0x040034C7 RID: 13511
		public const short LesionCandle = 3962;

		// Token: 0x040034C8 RID: 13512
		public const short LesionChair = 3963;

		// Token: 0x040034C9 RID: 13513
		public const short LesionChandelier = 3964;

		// Token: 0x040034CA RID: 13514
		public const short LesionChest = 3965;

		// Token: 0x040034CB RID: 13515
		public const short LesionClock = 3966;

		// Token: 0x040034CC RID: 13516
		public const short LesionDoor = 3967;

		// Token: 0x040034CD RID: 13517
		public const short LesionDresser = 3968;

		// Token: 0x040034CE RID: 13518
		public const short LesionLamp = 3969;

		// Token: 0x040034CF RID: 13519
		public const short LesionLantern = 3970;

		// Token: 0x040034D0 RID: 13520
		public const short LesionPiano = 3971;

		// Token: 0x040034D1 RID: 13521
		public const short LesionSink = 3972;

		// Token: 0x040034D2 RID: 13522
		public const short LesionSofa = 3973;

		// Token: 0x040034D3 RID: 13523
		public const short LesionTable = 3974;

		// Token: 0x040034D4 RID: 13524
		public const short LesionWorkbench = 3975;

		// Token: 0x040034D5 RID: 13525
		public const short Fake_LesionChest = 3976;

		// Token: 0x040034D6 RID: 13526
		public const short HatRack = 3977;

		// Token: 0x040034D7 RID: 13527
		public const short ColorOnlyDye = 3978;

		// Token: 0x040034D8 RID: 13528
		public const short WoodenCrateHard = 3979;

		// Token: 0x040034D9 RID: 13529
		public const short IronCrateHard = 3980;

		// Token: 0x040034DA RID: 13530
		public const short GoldenCrateHard = 3981;

		// Token: 0x040034DB RID: 13531
		public const short CorruptFishingCrateHard = 3982;

		// Token: 0x040034DC RID: 13532
		public const short CrimsonFishingCrateHard = 3983;

		// Token: 0x040034DD RID: 13533
		public const short DungeonFishingCrateHard = 3984;

		// Token: 0x040034DE RID: 13534
		public const short FloatingIslandFishingCrateHard = 3985;

		// Token: 0x040034DF RID: 13535
		public const short HallowedFishingCrateHard = 3986;

		// Token: 0x040034E0 RID: 13536
		public const short JungleFishingCrateHard = 3987;

		// Token: 0x040034E1 RID: 13537
		public const short DeadMansChest = 3988;

		// Token: 0x040034E2 RID: 13538
		public const short GolfBall = 3989;

		// Token: 0x040034E3 RID: 13539
		public const short AmphibianBoots = 3990;

		// Token: 0x040034E4 RID: 13540
		public const short ArcaneFlower = 3991;

		// Token: 0x040034E5 RID: 13541
		public const short BerserkerGlove = 3992;

		// Token: 0x040034E6 RID: 13542
		public const short FairyBoots = 3993;

		// Token: 0x040034E7 RID: 13543
		public const short FrogFlipper = 3994;

		// Token: 0x040034E8 RID: 13544
		public const short FrogGear = 3995;

		// Token: 0x040034E9 RID: 13545
		public const short FrogWebbing = 3996;

		// Token: 0x040034EA RID: 13546
		public const short FrozenShield = 3997;

		// Token: 0x040034EB RID: 13547
		public const short HeroShield = 3998;

		// Token: 0x040034EC RID: 13548
		public const short LavaSkull = 3999;

		// Token: 0x040034ED RID: 13549
		public const short MagnetFlower = 4000;

		// Token: 0x040034EE RID: 13550
		public const short ManaCloak = 4001;

		// Token: 0x040034EF RID: 13551
		public const short MoltenQuiver = 4002;

		// Token: 0x040034F0 RID: 13552
		public const short MoltenSkullRose = 4003;

		// Token: 0x040034F1 RID: 13553
		public const short ObsidianSkullRose = 4004;

		// Token: 0x040034F2 RID: 13554
		public const short ReconScope = 4005;

		// Token: 0x040034F3 RID: 13555
		public const short StalkersQuiver = 4006;

		// Token: 0x040034F4 RID: 13556
		public const short StingerNecklace = 4007;

		// Token: 0x040034F5 RID: 13557
		public const short UltrabrightHelmet = 4008;

		// Token: 0x040034F6 RID: 13558
		public const short Apple = 4009;

		// Token: 0x040034F7 RID: 13559
		public const short ApplePieSlice = 4010;

		// Token: 0x040034F8 RID: 13560
		public const short ApplePie = 4011;

		// Token: 0x040034F9 RID: 13561
		public const short BananaSplit = 4012;

		// Token: 0x040034FA RID: 13562
		public const short BBQRibs = 4013;

		// Token: 0x040034FB RID: 13563
		public const short BunnyStew = 4014;

		// Token: 0x040034FC RID: 13564
		public const short Burger = 4015;

		// Token: 0x040034FD RID: 13565
		public const short ChickenNugget = 4016;

		// Token: 0x040034FE RID: 13566
		public const short ChocolateChipCookie = 4017;

		// Token: 0x040034FF RID: 13567
		public const short CreamSoda = 4018;

		// Token: 0x04003500 RID: 13568
		public const short Escargot = 4019;

		// Token: 0x04003501 RID: 13569
		public const short FriedEgg = 4020;

		// Token: 0x04003502 RID: 13570
		public const short Fries = 4021;

		// Token: 0x04003503 RID: 13571
		public const short GoldenDelight = 4022;

		// Token: 0x04003504 RID: 13572
		public const short Grapes = 4023;

		// Token: 0x04003505 RID: 13573
		public const short GrilledSquirrel = 4024;

		// Token: 0x04003506 RID: 13574
		public const short Hotdog = 4025;

		// Token: 0x04003507 RID: 13575
		public const short IceCream = 4026;

		// Token: 0x04003508 RID: 13576
		public const short Milkshake = 4027;

		// Token: 0x04003509 RID: 13577
		public const short Nachos = 4028;

		// Token: 0x0400350A RID: 13578
		public const short Pizza = 4029;

		// Token: 0x0400350B RID: 13579
		public const short PotatoChips = 4030;

		// Token: 0x0400350C RID: 13580
		public const short RoastedBird = 4031;

		// Token: 0x0400350D RID: 13581
		public const short RoastedDuck = 4032;

		// Token: 0x0400350E RID: 13582
		public const short SauteedFrogLegs = 4033;

		// Token: 0x0400350F RID: 13583
		public const short SeafoodDinner = 4034;

		// Token: 0x04003510 RID: 13584
		public const short ShrimpPoBoy = 4035;

		// Token: 0x04003511 RID: 13585
		public const short Spaghetti = 4036;

		// Token: 0x04003512 RID: 13586
		public const short Steak = 4037;

		// Token: 0x04003513 RID: 13587
		public const short MoltenCharm = 4038;

		// Token: 0x04003514 RID: 13588
		public const short GolfClubIron = 4039;

		// Token: 0x04003515 RID: 13589
		public const short GolfCup = 4040;

		// Token: 0x04003516 RID: 13590
		public const short FlowerPacketBlue = 4041;

		// Token: 0x04003517 RID: 13591
		public const short FlowerPacketMagenta = 4042;

		// Token: 0x04003518 RID: 13592
		public const short FlowerPacketPink = 4043;

		// Token: 0x04003519 RID: 13593
		public const short FlowerPacketRed = 4044;

		// Token: 0x0400351A RID: 13594
		public const short FlowerPacketYellow = 4045;

		// Token: 0x0400351B RID: 13595
		public const short FlowerPacketViolet = 4046;

		// Token: 0x0400351C RID: 13596
		public const short FlowerPacketWhite = 4047;

		// Token: 0x0400351D RID: 13597
		public const short FlowerPacketTallGrass = 4048;

		// Token: 0x0400351E RID: 13598
		public const short LawnMower = 4049;

		// Token: 0x0400351F RID: 13599
		public const short CrimstoneBrick = 4050;

		// Token: 0x04003520 RID: 13600
		public const short SmoothSandstone = 4051;

		// Token: 0x04003521 RID: 13601
		public const short CrimstoneBrickWall = 4052;

		// Token: 0x04003522 RID: 13602
		public const short SmoothSandstoneWall = 4053;

		// Token: 0x04003523 RID: 13603
		public const short BloodMoonMonolith = 4054;

		// Token: 0x04003524 RID: 13604
		public const short SandBoots = 4055;

		// Token: 0x04003525 RID: 13605
		public const short AncientChisel = 4056;

		// Token: 0x04003526 RID: 13606
		public const short CarbonGuitar = 4057;

		// Token: 0x04003527 RID: 13607
		public const short SkeletonBow = 4058;

		// Token: 0x04003528 RID: 13608
		public const short FossilPickaxe = 4059;

		// Token: 0x04003529 RID: 13609
		public const short SuperStarCannon = 4060;

		// Token: 0x0400352A RID: 13610
		public const short ThunderSpear = 4061;

		// Token: 0x0400352B RID: 13611
		public const short ThunderStaff = 4062;

		// Token: 0x0400352C RID: 13612
		public const short DrumSet = 4063;

		// Token: 0x0400352D RID: 13613
		public const short PicnicTable = 4064;

		// Token: 0x0400352E RID: 13614
		public const short PicnicTableWithCloth = 4065;

		// Token: 0x0400352F RID: 13615
		public const short DesertMinecart = 4066;

		// Token: 0x04003530 RID: 13616
		public const short FishMinecart = 4067;

		// Token: 0x04003531 RID: 13617
		public const short FairyCritterPink = 4068;

		// Token: 0x04003532 RID: 13618
		public const short FairyCritterGreen = 4069;

		// Token: 0x04003533 RID: 13619
		public const short FairyCritterBlue = 4070;

		// Token: 0x04003534 RID: 13620
		public const short JunoniaShell = 4071;

		// Token: 0x04003535 RID: 13621
		public const short LightningWhelkShell = 4072;

		// Token: 0x04003536 RID: 13622
		public const short TulipShell = 4073;

		// Token: 0x04003537 RID: 13623
		public const short PinWheel = 4074;

		// Token: 0x04003538 RID: 13624
		public const short WeatherVane = 4075;

		// Token: 0x04003539 RID: 13625
		public const short VoidVault = 4076;

		// Token: 0x0400353A RID: 13626
		public const short MusicBoxOceanAlt = 4077;

		// Token: 0x0400353B RID: 13627
		public const short MusicBoxSlimeRain = 4078;

		// Token: 0x0400353C RID: 13628
		public const short MusicBoxSpaceAlt = 4079;

		// Token: 0x0400353D RID: 13629
		public const short MusicBoxTownDay = 4080;

		// Token: 0x0400353E RID: 13630
		public const short MusicBoxTownNight = 4081;

		// Token: 0x0400353F RID: 13631
		public const short MusicBoxWindyDay = 4082;

		// Token: 0x04003540 RID: 13632
		public const short GolfCupFlagWhite = 4083;

		// Token: 0x04003541 RID: 13633
		public const short GolfCupFlagRed = 4084;

		// Token: 0x04003542 RID: 13634
		public const short GolfCupFlagGreen = 4085;

		// Token: 0x04003543 RID: 13635
		public const short GolfCupFlagBlue = 4086;

		// Token: 0x04003544 RID: 13636
		public const short GolfCupFlagYellow = 4087;

		// Token: 0x04003545 RID: 13637
		public const short GolfCupFlagPurple = 4088;

		// Token: 0x04003546 RID: 13638
		public const short GolfTee = 4089;

		// Token: 0x04003547 RID: 13639
		public const short ShellPileBlock = 4090;

		// Token: 0x04003548 RID: 13640
		public const short AntiPortalBlock = 4091;

		// Token: 0x04003549 RID: 13641
		public const short GolfClubPutter = 4092;

		// Token: 0x0400354A RID: 13642
		public const short GolfClubWedge = 4093;

		// Token: 0x0400354B RID: 13643
		public const short GolfClubDriver = 4094;

		// Token: 0x0400354C RID: 13644
		public const short GolfWhistle = 4095;

		// Token: 0x0400354D RID: 13645
		public const short ToiletEbonyWood = 4096;

		// Token: 0x0400354E RID: 13646
		public const short ToiletRichMahogany = 4097;

		// Token: 0x0400354F RID: 13647
		public const short ToiletPearlwood = 4098;

		// Token: 0x04003550 RID: 13648
		public const short ToiletLivingWood = 4099;

		// Token: 0x04003551 RID: 13649
		public const short ToiletCactus = 4100;

		// Token: 0x04003552 RID: 13650
		public const short ToiletBone = 4101;

		// Token: 0x04003553 RID: 13651
		public const short ToiletFlesh = 4102;

		// Token: 0x04003554 RID: 13652
		public const short ToiletMushroom = 4103;

		// Token: 0x04003555 RID: 13653
		public const short ToiletSunplate = 4104;

		// Token: 0x04003556 RID: 13654
		public const short ToiletShadewood = 4105;

		// Token: 0x04003557 RID: 13655
		public const short ToiletLihzhard = 4106;

		// Token: 0x04003558 RID: 13656
		public const short ToiletDungeonBlue = 4107;

		// Token: 0x04003559 RID: 13657
		public const short ToiletDungeonGreen = 4108;

		// Token: 0x0400355A RID: 13658
		public const short ToiletDungeonPink = 4109;

		// Token: 0x0400355B RID: 13659
		public const short ToiletObsidian = 4110;

		// Token: 0x0400355C RID: 13660
		public const short ToiletFrozen = 4111;

		// Token: 0x0400355D RID: 13661
		public const short ToiletGlass = 4112;

		// Token: 0x0400355E RID: 13662
		public const short ToiletHoney = 4113;

		// Token: 0x0400355F RID: 13663
		public const short ToiletSteampunk = 4114;

		// Token: 0x04003560 RID: 13664
		public const short ToiletPumpkin = 4115;

		// Token: 0x04003561 RID: 13665
		public const short ToiletSpooky = 4116;

		// Token: 0x04003562 RID: 13666
		public const short ToiletDynasty = 4117;

		// Token: 0x04003563 RID: 13667
		public const short ToiletPalm = 4118;

		// Token: 0x04003564 RID: 13668
		public const short ToiletBoreal = 4119;

		// Token: 0x04003565 RID: 13669
		public const short ToiletSlime = 4120;

		// Token: 0x04003566 RID: 13670
		public const short ToiletMartian = 4121;

		// Token: 0x04003567 RID: 13671
		public const short ToiletGranite = 4122;

		// Token: 0x04003568 RID: 13672
		public const short ToiletMarble = 4123;

		// Token: 0x04003569 RID: 13673
		public const short ToiletCrystal = 4124;

		// Token: 0x0400356A RID: 13674
		public const short ToiletSpider = 4125;

		// Token: 0x0400356B RID: 13675
		public const short ToiletLesion = 4126;

		// Token: 0x0400356C RID: 13676
		public const short ToiletDiamond = 4127;

		// Token: 0x0400356D RID: 13677
		public const short MaidHead = 4128;

		// Token: 0x0400356E RID: 13678
		public const short MaidShirt = 4129;

		// Token: 0x0400356F RID: 13679
		public const short MaidPants = 4130;

		// Token: 0x04003570 RID: 13680
		public const short VoidLens = 4131;

		// Token: 0x04003571 RID: 13681
		public const short MaidHead2 = 4132;

		// Token: 0x04003572 RID: 13682
		public const short MaidShirt2 = 4133;

		// Token: 0x04003573 RID: 13683
		public const short MaidPants2 = 4134;

		// Token: 0x04003574 RID: 13684
		public const short GolfHat = 4135;

		// Token: 0x04003575 RID: 13685
		public const short GolfShirt = 4136;

		// Token: 0x04003576 RID: 13686
		public const short GolfPants = 4137;

		// Token: 0x04003577 RID: 13687
		public const short GolfVisor = 4138;

		// Token: 0x04003578 RID: 13688
		public const short SpiderBlock = 4139;

		// Token: 0x04003579 RID: 13689
		public const short SpiderWall = 4140;

		// Token: 0x0400357A RID: 13690
		public const short ToiletMeteor = 4141;

		// Token: 0x0400357B RID: 13691
		public const short LesionStation = 4142;

		// Token: 0x0400357C RID: 13692
		public const short ManaCloakStar = 4143;

		// Token: 0x0400357D RID: 13693
		public const short Terragrim = 4144;

		// Token: 0x0400357E RID: 13694
		public const short SolarBathtub = 4145;

		// Token: 0x0400357F RID: 13695
		public const short SolarBed = 4146;

		// Token: 0x04003580 RID: 13696
		public const short SolarBookcase = 4147;

		// Token: 0x04003581 RID: 13697
		public const short SolarDresser = 4148;

		// Token: 0x04003582 RID: 13698
		public const short SolarCandelabra = 4149;

		// Token: 0x04003583 RID: 13699
		public const short SolarCandle = 4150;

		// Token: 0x04003584 RID: 13700
		public const short SolarChair = 4151;

		// Token: 0x04003585 RID: 13701
		public const short SolarChandelier = 4152;

		// Token: 0x04003586 RID: 13702
		public const short SolarChest = 4153;

		// Token: 0x04003587 RID: 13703
		public const short SolarClock = 4154;

		// Token: 0x04003588 RID: 13704
		public const short SolarDoor = 4155;

		// Token: 0x04003589 RID: 13705
		public const short SolarLamp = 4156;

		// Token: 0x0400358A RID: 13706
		public const short SolarLantern = 4157;

		// Token: 0x0400358B RID: 13707
		public const short SolarPiano = 4158;

		// Token: 0x0400358C RID: 13708
		public const short SolarPlatform = 4159;

		// Token: 0x0400358D RID: 13709
		public const short SolarSink = 4160;

		// Token: 0x0400358E RID: 13710
		public const short SolarSofa = 4161;

		// Token: 0x0400358F RID: 13711
		public const short SolarTable = 4162;

		// Token: 0x04003590 RID: 13712
		public const short SolarWorkbench = 4163;

		// Token: 0x04003591 RID: 13713
		public const short Fake_SolarChest = 4164;

		// Token: 0x04003592 RID: 13714
		public const short SolarToilet = 4165;

		// Token: 0x04003593 RID: 13715
		public const short VortexBathtub = 4166;

		// Token: 0x04003594 RID: 13716
		public const short VortexBed = 4167;

		// Token: 0x04003595 RID: 13717
		public const short VortexBookcase = 4168;

		// Token: 0x04003596 RID: 13718
		public const short VortexDresser = 4169;

		// Token: 0x04003597 RID: 13719
		public const short VortexCandelabra = 4170;

		// Token: 0x04003598 RID: 13720
		public const short VortexCandle = 4171;

		// Token: 0x04003599 RID: 13721
		public const short VortexChair = 4172;

		// Token: 0x0400359A RID: 13722
		public const short VortexChandelier = 4173;

		// Token: 0x0400359B RID: 13723
		public const short VortexChest = 4174;

		// Token: 0x0400359C RID: 13724
		public const short VortexClock = 4175;

		// Token: 0x0400359D RID: 13725
		public const short VortexDoor = 4176;

		// Token: 0x0400359E RID: 13726
		public const short VortexLamp = 4177;

		// Token: 0x0400359F RID: 13727
		public const short VortexLantern = 4178;

		// Token: 0x040035A0 RID: 13728
		public const short VortexPiano = 4179;

		// Token: 0x040035A1 RID: 13729
		public const short VortexPlatform = 4180;

		// Token: 0x040035A2 RID: 13730
		public const short VortexSink = 4181;

		// Token: 0x040035A3 RID: 13731
		public const short VortexSofa = 4182;

		// Token: 0x040035A4 RID: 13732
		public const short VortexTable = 4183;

		// Token: 0x040035A5 RID: 13733
		public const short VortexWorkbench = 4184;

		// Token: 0x040035A6 RID: 13734
		public const short Fake_VortexChest = 4185;

		// Token: 0x040035A7 RID: 13735
		public const short VortexToilet = 4186;

		// Token: 0x040035A8 RID: 13736
		public const short NebulaBathtub = 4187;

		// Token: 0x040035A9 RID: 13737
		public const short NebulaBed = 4188;

		// Token: 0x040035AA RID: 13738
		public const short NebulaBookcase = 4189;

		// Token: 0x040035AB RID: 13739
		public const short NebulaDresser = 4190;

		// Token: 0x040035AC RID: 13740
		public const short NebulaCandelabra = 4191;

		// Token: 0x040035AD RID: 13741
		public const short NebulaCandle = 4192;

		// Token: 0x040035AE RID: 13742
		public const short NebulaChair = 4193;

		// Token: 0x040035AF RID: 13743
		public const short NebulaChandelier = 4194;

		// Token: 0x040035B0 RID: 13744
		public const short NebulaChest = 4195;

		// Token: 0x040035B1 RID: 13745
		public const short NebulaClock = 4196;

		// Token: 0x040035B2 RID: 13746
		public const short NebulaDoor = 4197;

		// Token: 0x040035B3 RID: 13747
		public const short NebulaLamp = 4198;

		// Token: 0x040035B4 RID: 13748
		public const short NebulaLantern = 4199;

		// Token: 0x040035B5 RID: 13749
		public const short NebulaPiano = 4200;

		// Token: 0x040035B6 RID: 13750
		public const short NebulaPlatform = 4201;

		// Token: 0x040035B7 RID: 13751
		public const short NebulaSink = 4202;

		// Token: 0x040035B8 RID: 13752
		public const short NebulaSofa = 4203;

		// Token: 0x040035B9 RID: 13753
		public const short NebulaTable = 4204;

		// Token: 0x040035BA RID: 13754
		public const short NebulaWorkbench = 4205;

		// Token: 0x040035BB RID: 13755
		public const short Fake_NebulaChest = 4206;

		// Token: 0x040035BC RID: 13756
		public const short NebulaToilet = 4207;

		// Token: 0x040035BD RID: 13757
		public const short StardustBathtub = 4208;

		// Token: 0x040035BE RID: 13758
		public const short StardustBed = 4209;

		// Token: 0x040035BF RID: 13759
		public const short StardustBookcase = 4210;

		// Token: 0x040035C0 RID: 13760
		public const short StardustDresser = 4211;

		// Token: 0x040035C1 RID: 13761
		public const short StardustCandelabra = 4212;

		// Token: 0x040035C2 RID: 13762
		public const short StardustCandle = 4213;

		// Token: 0x040035C3 RID: 13763
		public const short StardustChair = 4214;

		// Token: 0x040035C4 RID: 13764
		public const short StardustChandelier = 4215;

		// Token: 0x040035C5 RID: 13765
		public const short StardustChest = 4216;

		// Token: 0x040035C6 RID: 13766
		public const short StardustClock = 4217;

		// Token: 0x040035C7 RID: 13767
		public const short StardustDoor = 4218;

		// Token: 0x040035C8 RID: 13768
		public const short StardustLamp = 4219;

		// Token: 0x040035C9 RID: 13769
		public const short StardustLantern = 4220;

		// Token: 0x040035CA RID: 13770
		public const short StardustPiano = 4221;

		// Token: 0x040035CB RID: 13771
		public const short StardustPlatform = 4222;

		// Token: 0x040035CC RID: 13772
		public const short StardustSink = 4223;

		// Token: 0x040035CD RID: 13773
		public const short StardustSofa = 4224;

		// Token: 0x040035CE RID: 13774
		public const short StardustTable = 4225;

		// Token: 0x040035CF RID: 13775
		public const short StardustWorkbench = 4226;

		// Token: 0x040035D0 RID: 13776
		public const short Fake_StardustChest = 4227;

		// Token: 0x040035D1 RID: 13777
		public const short StardustToilet = 4228;

		// Token: 0x040035D2 RID: 13778
		public const short SolarBrick = 4229;

		// Token: 0x040035D3 RID: 13779
		public const short VortexBrick = 4230;

		// Token: 0x040035D4 RID: 13780
		public const short NebulaBrick = 4231;

		// Token: 0x040035D5 RID: 13781
		public const short StardustBrick = 4232;

		// Token: 0x040035D6 RID: 13782
		public const short SolarBrickWall = 4233;

		// Token: 0x040035D7 RID: 13783
		public const short VortexBrickWall = 4234;

		// Token: 0x040035D8 RID: 13784
		public const short NebulaBrickWall = 4235;

		// Token: 0x040035D9 RID: 13785
		public const short StardustBrickWall = 4236;

		// Token: 0x040035DA RID: 13786
		public const short MusicBoxDayRemix = 4237;

		// Token: 0x040035DB RID: 13787
		public const short CrackedBlueBrick = 4238;

		// Token: 0x040035DC RID: 13788
		public const short CrackedGreenBrick = 4239;

		// Token: 0x040035DD RID: 13789
		public const short CrackedPinkBrick = 4240;

		// Token: 0x040035DE RID: 13790
		public const short FlowerPacketWild = 4241;

		// Token: 0x040035DF RID: 13791
		public const short GolfBallDyedBlack = 4242;

		// Token: 0x040035E0 RID: 13792
		public const short GolfBallDyedBlue = 4243;

		// Token: 0x040035E1 RID: 13793
		public const short GolfBallDyedBrown = 4244;

		// Token: 0x040035E2 RID: 13794
		public const short GolfBallDyedCyan = 4245;

		// Token: 0x040035E3 RID: 13795
		public const short GolfBallDyedGreen = 4246;

		// Token: 0x040035E4 RID: 13796
		public const short GolfBallDyedLimeGreen = 4247;

		// Token: 0x040035E5 RID: 13797
		public const short GolfBallDyedOrange = 4248;

		// Token: 0x040035E6 RID: 13798
		public const short GolfBallDyedPink = 4249;

		// Token: 0x040035E7 RID: 13799
		public const short GolfBallDyedPurple = 4250;

		// Token: 0x040035E8 RID: 13800
		public const short GolfBallDyedRed = 4251;

		// Token: 0x040035E9 RID: 13801
		public const short GolfBallDyedSkyBlue = 4252;

		// Token: 0x040035EA RID: 13802
		public const short GolfBallDyedTeal = 4253;

		// Token: 0x040035EB RID: 13803
		public const short GolfBallDyedViolet = 4254;

		// Token: 0x040035EC RID: 13804
		public const short GolfBallDyedYellow = 4255;

		// Token: 0x040035ED RID: 13805
		public const short AmberRobe = 4256;

		// Token: 0x040035EE RID: 13806
		public const short AmberHook = 4257;

		// Token: 0x040035EF RID: 13807
		public const short OrangePhaseblade = 4258;

		// Token: 0x040035F0 RID: 13808
		public const short OrangePhasesaber = 4259;

		// Token: 0x040035F1 RID: 13809
		public const short OrangeStainedGlass = 4260;

		// Token: 0x040035F2 RID: 13810
		public const short OrangePressurePlate = 4261;

		// Token: 0x040035F3 RID: 13811
		public const short MysticCoilSnake = 4262;

		// Token: 0x040035F4 RID: 13812
		public const short MagicConch = 4263;

		// Token: 0x040035F5 RID: 13813
		public const short GolfCart = 4264;

		// Token: 0x040035F6 RID: 13814
		public const short GolfChest = 4265;

		// Token: 0x040035F7 RID: 13815
		public const short Fake_GolfChest = 4266;

		// Token: 0x040035F8 RID: 13816
		public const short DesertChest = 4267;

		// Token: 0x040035F9 RID: 13817
		public const short Fake_DesertChest = 4268;

		// Token: 0x040035FA RID: 13818
		public const short SanguineStaff = 4269;

		// Token: 0x040035FB RID: 13819
		public const short SharpTears = 4270;

		// Token: 0x040035FC RID: 13820
		public const short BloodMoonStarter = 4271;

		// Token: 0x040035FD RID: 13821
		public const short DripplerFlail = 4272;

		// Token: 0x040035FE RID: 13822
		public const short VampireFrogStaff = 4273;

		// Token: 0x040035FF RID: 13823
		public const short GoldGoldfish = 4274;

		// Token: 0x04003600 RID: 13824
		public const short GoldGoldfishBowl = 4275;

		// Token: 0x04003601 RID: 13825
		public const short CatBast = 4276;

		// Token: 0x04003602 RID: 13826
		public const short GoldStarryGlassBlock = 4277;

		// Token: 0x04003603 RID: 13827
		public const short BlueStarryGlassBlock = 4278;

		// Token: 0x04003604 RID: 13828
		public const short GoldStarryGlassWall = 4279;

		// Token: 0x04003605 RID: 13829
		public const short BlueStarryGlassWall = 4280;

		// Token: 0x04003606 RID: 13830
		public const short BabyBirdStaff = 4281;

		// Token: 0x04003607 RID: 13831
		public const short Apricot = 4282;

		// Token: 0x04003608 RID: 13832
		public const short Banana = 4283;

		// Token: 0x04003609 RID: 13833
		public const short BlackCurrant = 4284;

		// Token: 0x0400360A RID: 13834
		public const short BloodOrange = 4285;

		// Token: 0x0400360B RID: 13835
		public const short Cherry = 4286;

		// Token: 0x0400360C RID: 13836
		public const short Coconut = 4287;

		// Token: 0x0400360D RID: 13837
		public const short Dragonfruit = 4288;

		// Token: 0x0400360E RID: 13838
		public const short Elderberry = 4289;

		// Token: 0x0400360F RID: 13839
		public const short Grapefruit = 4290;

		// Token: 0x04003610 RID: 13840
		public const short Lemon = 4291;

		// Token: 0x04003611 RID: 13841
		public const short Mango = 4292;

		// Token: 0x04003612 RID: 13842
		public const short Peach = 4293;

		// Token: 0x04003613 RID: 13843
		public const short Pineapple = 4294;

		// Token: 0x04003614 RID: 13844
		public const short Plum = 4295;

		// Token: 0x04003615 RID: 13845
		public const short Rambutan = 4296;

		// Token: 0x04003616 RID: 13846
		public const short Starfruit = 4297;

		// Token: 0x04003617 RID: 13847
		public const short SandstoneBathtub = 4298;

		// Token: 0x04003618 RID: 13848
		public const short SandstoneBed = 4299;

		// Token: 0x04003619 RID: 13849
		public const short SandstoneBookcase = 4300;

		// Token: 0x0400361A RID: 13850
		public const short SandstoneDresser = 4301;

		// Token: 0x0400361B RID: 13851
		public const short SandstoneCandelabra = 4302;

		// Token: 0x0400361C RID: 13852
		public const short SandstoneCandle = 4303;

		// Token: 0x0400361D RID: 13853
		public const short SandstoneChair = 4304;

		// Token: 0x0400361E RID: 13854
		public const short SandstoneChandelier = 4305;

		// Token: 0x0400361F RID: 13855
		public const short SandstoneClock = 4306;

		// Token: 0x04003620 RID: 13856
		public const short SandstoneDoor = 4307;

		// Token: 0x04003621 RID: 13857
		public const short SandstoneLamp = 4308;

		// Token: 0x04003622 RID: 13858
		public const short SandstoneLantern = 4309;

		// Token: 0x04003623 RID: 13859
		public const short SandstonePiano = 4310;

		// Token: 0x04003624 RID: 13860
		public const short SandstonePlatform = 4311;

		// Token: 0x04003625 RID: 13861
		public const short SandstoneSink = 4312;

		// Token: 0x04003626 RID: 13862
		public const short SandstoneSofa = 4313;

		// Token: 0x04003627 RID: 13863
		public const short SandstoneTable = 4314;

		// Token: 0x04003628 RID: 13864
		public const short SandstoneWorkbench = 4315;

		// Token: 0x04003629 RID: 13865
		public const short SandstoneToilet = 4316;

		// Token: 0x0400362A RID: 13866
		public const short BloodHamaxe = 4317;

		// Token: 0x0400362B RID: 13867
		public const short VoidMonolith = 4318;

		// Token: 0x0400362C RID: 13868
		public const short ArrowSign = 4319;

		// Token: 0x0400362D RID: 13869
		public const short PaintedArrowSign = 4320;

		// Token: 0x0400362E RID: 13870
		public const short GameMasterShirt = 4321;

		// Token: 0x0400362F RID: 13871
		public const short GameMasterPants = 4322;

		// Token: 0x04003630 RID: 13872
		public const short StarPrincessCrown = 4323;

		// Token: 0x04003631 RID: 13873
		public const short StarPrincessDress = 4324;

		// Token: 0x04003632 RID: 13874
		public const short BloodFishingRod = 4325;

		// Token: 0x04003633 RID: 13875
		public const short FoodPlatter = 4326;

		// Token: 0x04003634 RID: 13876
		public const short BlackDragonflyJar = 4327;

		// Token: 0x04003635 RID: 13877
		public const short BlueDragonflyJar = 4328;

		// Token: 0x04003636 RID: 13878
		public const short GreenDragonflyJar = 4329;

		// Token: 0x04003637 RID: 13879
		public const short OrangeDragonflyJar = 4330;

		// Token: 0x04003638 RID: 13880
		public const short RedDragonflyJar = 4331;

		// Token: 0x04003639 RID: 13881
		public const short YellowDragonflyJar = 4332;

		// Token: 0x0400363A RID: 13882
		public const short GoldDragonflyJar = 4333;

		// Token: 0x0400363B RID: 13883
		public const short BlackDragonfly = 4334;

		// Token: 0x0400363C RID: 13884
		public const short BlueDragonfly = 4335;

		// Token: 0x0400363D RID: 13885
		public const short GreenDragonfly = 4336;

		// Token: 0x0400363E RID: 13886
		public const short OrangeDragonfly = 4337;

		// Token: 0x0400363F RID: 13887
		public const short RedDragonfly = 4338;

		// Token: 0x04003640 RID: 13888
		public const short YellowDragonfly = 4339;

		// Token: 0x04003641 RID: 13889
		public const short GoldDragonfly = 4340;

		// Token: 0x04003642 RID: 13890
		public const short PortableStool = 4341;

		// Token: 0x04003643 RID: 13891
		public const short DragonflyStatue = 4342;

		// Token: 0x04003644 RID: 13892
		public const short PaperAirplaneA = 4343;

		// Token: 0x04003645 RID: 13893
		public const short PaperAirplaneB = 4344;

		// Token: 0x04003646 RID: 13894
		public const short CanOfWorms = 4345;

		// Token: 0x04003647 RID: 13895
		public const short EncumberingStone = 4346;

		// Token: 0x04003648 RID: 13896
		public const short ZapinatorGray = 4347;

		// Token: 0x04003649 RID: 13897
		public const short ZapinatorOrange = 4348;

		// Token: 0x0400364A RID: 13898
		public const short GreenMoss = 4349;

		// Token: 0x0400364B RID: 13899
		public const short BrownMoss = 4350;

		// Token: 0x0400364C RID: 13900
		public const short RedMoss = 4351;

		// Token: 0x0400364D RID: 13901
		public const short BlueMoss = 4352;

		// Token: 0x0400364E RID: 13902
		public const short PurpleMoss = 4353;

		// Token: 0x0400364F RID: 13903
		public const short LavaMoss = 4354;

		// Token: 0x04003650 RID: 13904
		public const short BoulderStatue = 4355;

		// Token: 0x04003651 RID: 13905
		public const short MusicBoxTitleAlt = 4356;

		// Token: 0x04003652 RID: 13906
		public const short MusicBoxStorm = 4357;

		// Token: 0x04003653 RID: 13907
		public const short MusicBoxGraveyard = 4358;

		// Token: 0x04003654 RID: 13908
		public const short Seagull = 4359;

		// Token: 0x04003655 RID: 13909
		public const short SeagullStatue = 4360;

		// Token: 0x04003656 RID: 13910
		public const short LadyBug = 4361;

		// Token: 0x04003657 RID: 13911
		public const short GoldLadyBug = 4362;

		// Token: 0x04003658 RID: 13912
		public const short Maggot = 4363;

		// Token: 0x04003659 RID: 13913
		public const short MaggotCage = 4364;

		// Token: 0x0400365A RID: 13914
		public const short CelestialWand = 4365;

		// Token: 0x0400365B RID: 13915
		public const short EucaluptusSap = 4366;

		// Token: 0x0400365C RID: 13916
		public const short KiteBlue = 4367;

		// Token: 0x0400365D RID: 13917
		public const short KiteBlueAndYellow = 4368;

		// Token: 0x0400365E RID: 13918
		public const short KiteRed = 4369;

		// Token: 0x0400365F RID: 13919
		public const short KiteRedAndYellow = 4370;

		// Token: 0x04003660 RID: 13920
		public const short KiteYellow = 4371;

		// Token: 0x04003661 RID: 13921
		public const short IvyGuitar = 4372;

		// Token: 0x04003662 RID: 13922
		public const short Pupfish = 4373;

		// Token: 0x04003663 RID: 13923
		public const short Grebe = 4374;

		// Token: 0x04003664 RID: 13924
		public const short Rat = 4375;

		// Token: 0x04003665 RID: 13925
		public const short RatCage = 4376;

		// Token: 0x04003666 RID: 13926
		public const short KryptonMoss = 4377;

		// Token: 0x04003667 RID: 13927
		public const short XenonMoss = 4378;

		// Token: 0x04003668 RID: 13928
		public const short KiteWyvern = 4379;

		// Token: 0x04003669 RID: 13929
		public const short LadybugCage = 4380;

		// Token: 0x0400366A RID: 13930
		public const short BloodRainBow = 4381;

		// Token: 0x0400366B RID: 13931
		public const short CombatBook = 4382;

		// Token: 0x0400366C RID: 13932
		public const short DesertTorch = 4383;

		// Token: 0x0400366D RID: 13933
		public const short CoralTorch = 4384;

		// Token: 0x0400366E RID: 13934
		public const short CorruptTorch = 4385;

		// Token: 0x0400366F RID: 13935
		public const short CrimsonTorch = 4386;

		// Token: 0x04003670 RID: 13936
		public const short HallowedTorch = 4387;

		// Token: 0x04003671 RID: 13937
		public const short JungleTorch = 4388;

		// Token: 0x04003672 RID: 13938
		public const short ArgonMoss = 4389;

		// Token: 0x04003673 RID: 13939
		public const short RollingCactus = 4390;

		// Token: 0x04003674 RID: 13940
		public const short ThinIce = 4391;

		// Token: 0x04003675 RID: 13941
		public const short EchoBlock = 4392;

		// Token: 0x04003676 RID: 13942
		public const short ScarabFish = 4393;

		// Token: 0x04003677 RID: 13943
		public const short ScorpioFish = 4394;

		// Token: 0x04003678 RID: 13944
		public const short Owl = 4395;

		// Token: 0x04003679 RID: 13945
		public const short OwlCage = 4396;

		// Token: 0x0400367A RID: 13946
		public const short OwlStatue = 4397;

		// Token: 0x0400367B RID: 13947
		public const short PupfishBowl = 4398;

		// Token: 0x0400367C RID: 13948
		public const short GoldLadybugCage = 4399;

		// Token: 0x0400367D RID: 13949
		public const short Geode = 4400;

		// Token: 0x0400367E RID: 13950
		public const short Flounder = 4401;

		// Token: 0x0400367F RID: 13951
		public const short RockLobster = 4402;

		// Token: 0x04003680 RID: 13952
		public const short LobsterTail = 4403;

		// Token: 0x04003681 RID: 13953
		public const short FloatingTube = 4404;

		// Token: 0x04003682 RID: 13954
		public const short FrozenCrate = 4405;

		// Token: 0x04003683 RID: 13955
		public const short FrozenCrateHard = 4406;

		// Token: 0x04003684 RID: 13956
		public const short OasisCrate = 4407;

		// Token: 0x04003685 RID: 13957
		public const short OasisCrateHard = 4408;

		// Token: 0x04003686 RID: 13958
		public const short SpectreGoggles = 4409;

		// Token: 0x04003687 RID: 13959
		public const short Oyster = 4410;

		// Token: 0x04003688 RID: 13960
		public const short ShuckedOyster = 4411;

		// Token: 0x04003689 RID: 13961
		public const short WhitePearl = 4412;

		// Token: 0x0400368A RID: 13962
		public const short BlackPearl = 4413;

		// Token: 0x0400368B RID: 13963
		public const short PinkPearl = 4414;

		// Token: 0x0400368C RID: 13964
		public const short StoneDoor = 4415;

		// Token: 0x0400368D RID: 13965
		public const short StonePlatform = 4416;

		// Token: 0x0400368E RID: 13966
		public const short OasisFountain = 4417;

		// Token: 0x0400368F RID: 13967
		public const short WaterStrider = 4418;

		// Token: 0x04003690 RID: 13968
		public const short GoldWaterStrider = 4419;

		// Token: 0x04003691 RID: 13969
		public const short LawnFlamingo = 4420;

		// Token: 0x04003692 RID: 13970
		public const short MusicBoxUndergroundJungle = 4421;

		// Token: 0x04003693 RID: 13971
		public const short Grate = 4422;

		// Token: 0x04003694 RID: 13972
		public const short ScarabBomb = 4423;

		// Token: 0x04003695 RID: 13973
		public const short WroughtIronFence = 4424;

		// Token: 0x04003696 RID: 13974
		public const short SharkBait = 4425;

		// Token: 0x04003697 RID: 13975
		public const short BeeMinecart = 4426;

		// Token: 0x04003698 RID: 13976
		public const short LadybugMinecart = 4427;

		// Token: 0x04003699 RID: 13977
		public const short PigronMinecart = 4428;

		// Token: 0x0400369A RID: 13978
		public const short SunflowerMinecart = 4429;

		// Token: 0x0400369B RID: 13979
		public const short PottedForestCedar = 4430;

		// Token: 0x0400369C RID: 13980
		public const short PottedJungleCedar = 4431;

		// Token: 0x0400369D RID: 13981
		public const short PottedHallowCedar = 4432;

		// Token: 0x0400369E RID: 13982
		public const short PottedForestTree = 4433;

		// Token: 0x0400369F RID: 13983
		public const short PottedJungleTree = 4434;

		// Token: 0x040036A0 RID: 13984
		public const short PottedHallowTree = 4435;

		// Token: 0x040036A1 RID: 13985
		public const short PottedForestPalm = 4436;

		// Token: 0x040036A2 RID: 13986
		public const short PottedJunglePalm = 4437;

		// Token: 0x040036A3 RID: 13987
		public const short PottedHallowPalm = 4438;

		// Token: 0x040036A4 RID: 13988
		public const short PottedForestBamboo = 4439;

		// Token: 0x040036A5 RID: 13989
		public const short PottedJungleBamboo = 4440;

		// Token: 0x040036A6 RID: 13990
		public const short PottedHallowBamboo = 4441;

		// Token: 0x040036A7 RID: 13991
		public const short ScarabFishingRod = 4442;

		// Token: 0x040036A8 RID: 13992
		public const short HellMinecart = 4443;

		// Token: 0x040036A9 RID: 13993
		public const short WitchBroom = 4444;

		// Token: 0x040036AA RID: 13994
		public const short ClusterRocketI = 4445;

		// Token: 0x040036AB RID: 13995
		public const short ClusterRocketII = 4446;

		// Token: 0x040036AC RID: 13996
		public const short WetRocket = 4447;

		// Token: 0x040036AD RID: 13997
		public const short LavaRocket = 4448;

		// Token: 0x040036AE RID: 13998
		public const short HoneyRocket = 4449;

		// Token: 0x040036AF RID: 13999
		public const short ShroomMinecart = 4450;

		// Token: 0x040036B0 RID: 14000
		public const short AmethystMinecart = 4451;

		// Token: 0x040036B1 RID: 14001
		public const short TopazMinecart = 4452;

		// Token: 0x040036B2 RID: 14002
		public const short SapphireMinecart = 4453;

		// Token: 0x040036B3 RID: 14003
		public const short EmeraldMinecart = 4454;

		// Token: 0x040036B4 RID: 14004
		public const short RubyMinecart = 4455;

		// Token: 0x040036B5 RID: 14005
		public const short DiamondMinecart = 4456;

		// Token: 0x040036B6 RID: 14006
		public const short MiniNukeI = 4457;

		// Token: 0x040036B7 RID: 14007
		public const short MiniNukeII = 4458;

		// Token: 0x040036B8 RID: 14008
		public const short DryRocket = 4459;

		// Token: 0x040036B9 RID: 14009
		public const short SandcastleBucket = 4460;

		// Token: 0x040036BA RID: 14010
		public const short TurtleCage = 4461;

		// Token: 0x040036BB RID: 14011
		public const short TurtleJungleCage = 4462;

		// Token: 0x040036BC RID: 14012
		public const short Gladius = 4463;

		// Token: 0x040036BD RID: 14013
		public const short Turtle = 4464;

		// Token: 0x040036BE RID: 14014
		public const short TurtleJungle = 4465;

		// Token: 0x040036BF RID: 14015
		public const short TurtleStatue = 4466;

		// Token: 0x040036C0 RID: 14016
		public const short AmberMinecart = 4467;

		// Token: 0x040036C1 RID: 14017
		public const short BeetleMinecart = 4468;

		// Token: 0x040036C2 RID: 14018
		public const short MeowmereMinecart = 4469;

		// Token: 0x040036C3 RID: 14019
		public const short PartyMinecart = 4470;

		// Token: 0x040036C4 RID: 14020
		public const short PirateMinecart = 4471;

		// Token: 0x040036C5 RID: 14021
		public const short SteampunkMinecart = 4472;

		// Token: 0x040036C6 RID: 14022
		public const short GrebeCage = 4473;

		// Token: 0x040036C7 RID: 14023
		public const short SeagullCage = 4474;

		// Token: 0x040036C8 RID: 14024
		public const short WaterStriderCage = 4475;

		// Token: 0x040036C9 RID: 14025
		public const short GoldWaterStriderCage = 4476;

		// Token: 0x040036CA RID: 14026
		public const short LuckPotionLesser = 4477;

		// Token: 0x040036CB RID: 14027
		public const short LuckPotion = 4478;

		// Token: 0x040036CC RID: 14028
		public const short LuckPotionGreater = 4479;

		// Token: 0x040036CD RID: 14029
		public const short Seahorse = 4480;

		// Token: 0x040036CE RID: 14030
		public const short SeahorseCage = 4481;

		// Token: 0x040036CF RID: 14031
		public const short GoldSeahorse = 4482;

		// Token: 0x040036D0 RID: 14032
		public const short GoldSeahorseCage = 4483;

		// Token: 0x040036D1 RID: 14033
		public const short TimerOneHalfSecond = 4484;

		// Token: 0x040036D2 RID: 14034
		public const short TimerOneFourthSecond = 4485;

		// Token: 0x040036D3 RID: 14035
		public const short EbonstoneEcho = 4486;

		// Token: 0x040036D4 RID: 14036
		public const short MudWallEcho = 4487;

		// Token: 0x040036D5 RID: 14037
		public const short PearlstoneEcho = 4488;

		// Token: 0x040036D6 RID: 14038
		public const short SnowWallEcho = 4489;

		// Token: 0x040036D7 RID: 14039
		public const short AmethystEcho = 4490;

		// Token: 0x040036D8 RID: 14040
		public const short TopazEcho = 4491;

		// Token: 0x040036D9 RID: 14041
		public const short SapphireEcho = 4492;

		// Token: 0x040036DA RID: 14042
		public const short EmeraldEcho = 4493;

		// Token: 0x040036DB RID: 14043
		public const short RubyEcho = 4494;

		// Token: 0x040036DC RID: 14044
		public const short DiamondEcho = 4495;

		// Token: 0x040036DD RID: 14045
		public const short Cave1Echo = 4496;

		// Token: 0x040036DE RID: 14046
		public const short Cave2Echo = 4497;

		// Token: 0x040036DF RID: 14047
		public const short Cave3Echo = 4498;

		// Token: 0x040036E0 RID: 14048
		public const short Cave4Echo = 4499;

		// Token: 0x040036E1 RID: 14049
		public const short Cave5Echo = 4500;

		// Token: 0x040036E2 RID: 14050
		public const short Cave6Echo = 4501;

		// Token: 0x040036E3 RID: 14051
		public const short Cave7Echo = 4502;

		// Token: 0x040036E4 RID: 14052
		public const short SpiderEcho = 4503;

		// Token: 0x040036E5 RID: 14053
		public const short CorruptGrassEcho = 4504;

		// Token: 0x040036E6 RID: 14054
		public const short HallowedGrassEcho = 4505;

		// Token: 0x040036E7 RID: 14055
		public const short IceEcho = 4506;

		// Token: 0x040036E8 RID: 14056
		public const short ObsidianBackEcho = 4507;

		// Token: 0x040036E9 RID: 14057
		public const short CrimsonGrassEcho = 4508;

		// Token: 0x040036EA RID: 14058
		public const short CrimstoneEcho = 4509;

		// Token: 0x040036EB RID: 14059
		public const short CaveWall1Echo = 4510;

		// Token: 0x040036EC RID: 14060
		public const short CaveWall2Echo = 4511;

		// Token: 0x040036ED RID: 14061
		public const short Cave8Echo = 4512;

		// Token: 0x040036EE RID: 14062
		public const short Corruption1Echo = 4513;

		// Token: 0x040036EF RID: 14063
		public const short Corruption2Echo = 4514;

		// Token: 0x040036F0 RID: 14064
		public const short Corruption3Echo = 4515;

		// Token: 0x040036F1 RID: 14065
		public const short Corruption4Echo = 4516;

		// Token: 0x040036F2 RID: 14066
		public const short Crimson1Echo = 4517;

		// Token: 0x040036F3 RID: 14067
		public const short Crimson2Echo = 4518;

		// Token: 0x040036F4 RID: 14068
		public const short Crimson3Echo = 4519;

		// Token: 0x040036F5 RID: 14069
		public const short Crimson4Echo = 4520;

		// Token: 0x040036F6 RID: 14070
		public const short Dirt1Echo = 4521;

		// Token: 0x040036F7 RID: 14071
		public const short Dirt2Echo = 4522;

		// Token: 0x040036F8 RID: 14072
		public const short Dirt3Echo = 4523;

		// Token: 0x040036F9 RID: 14073
		public const short Dirt4Echo = 4524;

		// Token: 0x040036FA RID: 14074
		public const short Hallow1Echo = 4525;

		// Token: 0x040036FB RID: 14075
		public const short Hallow2Echo = 4526;

		// Token: 0x040036FC RID: 14076
		public const short Hallow3Echo = 4527;

		// Token: 0x040036FD RID: 14077
		public const short Hallow4Echo = 4528;

		// Token: 0x040036FE RID: 14078
		public const short Jungle1Echo = 4529;

		// Token: 0x040036FF RID: 14079
		public const short Jungle2Echo = 4530;

		// Token: 0x04003700 RID: 14080
		public const short Jungle3Echo = 4531;

		// Token: 0x04003701 RID: 14081
		public const short Jungle4Echo = 4532;

		// Token: 0x04003702 RID: 14082
		public const short Lava1Echo = 4533;

		// Token: 0x04003703 RID: 14083
		public const short Lava2Echo = 4534;

		// Token: 0x04003704 RID: 14084
		public const short Lava3Echo = 4535;

		// Token: 0x04003705 RID: 14085
		public const short Lava4Echo = 4536;

		// Token: 0x04003706 RID: 14086
		public const short Rocks1Echo = 4537;

		// Token: 0x04003707 RID: 14087
		public const short Rocks2Echo = 4538;

		// Token: 0x04003708 RID: 14088
		public const short Rocks3Echo = 4539;

		// Token: 0x04003709 RID: 14089
		public const short Rocks4Echo = 4540;

		// Token: 0x0400370A RID: 14090
		public const short TheBrideBanner = 4541;

		// Token: 0x0400370B RID: 14091
		public const short ZombieMermanBanner = 4542;

		// Token: 0x0400370C RID: 14092
		public const short EyeballFlyingFishBanner = 4543;

		// Token: 0x0400370D RID: 14093
		public const short BloodSquidBanner = 4544;

		// Token: 0x0400370E RID: 14094
		public const short BloodEelBanner = 4545;

		// Token: 0x0400370F RID: 14095
		public const short GoblinSharkBanner = 4546;

		// Token: 0x04003710 RID: 14096
		public const short LargeBambooBlock = 4547;

		// Token: 0x04003711 RID: 14097
		public const short LargeBambooBlockWall = 4548;

		// Token: 0x04003712 RID: 14098
		public const short DemonHorns = 4549;

		// Token: 0x04003713 RID: 14099
		public const short BambooLeaf = 4550;

		// Token: 0x04003714 RID: 14100
		public const short HellCake = 4551;

		// Token: 0x04003715 RID: 14101
		public const short FogMachine = 4552;

		// Token: 0x04003716 RID: 14102
		public const short PlasmaLamp = 4553;

		// Token: 0x04003717 RID: 14103
		public const short MarbleColumn = 4554;

		// Token: 0x04003718 RID: 14104
		public const short ChefHat = 4555;

		// Token: 0x04003719 RID: 14105
		public const short ChefShirt = 4556;

		// Token: 0x0400371A RID: 14106
		public const short ChefPants = 4557;

		// Token: 0x0400371B RID: 14107
		public const short StarHairpin = 4558;

		// Token: 0x0400371C RID: 14108
		public const short HeartHairpin = 4559;

		// Token: 0x0400371D RID: 14109
		public const short BunnyEars = 4560;

		// Token: 0x0400371E RID: 14110
		public const short DevilHorns = 4561;

		// Token: 0x0400371F RID: 14111
		public const short Fedora = 4562;

		// Token: 0x04003720 RID: 14112
		public const short UnicornHornHat = 4563;

		// Token: 0x04003721 RID: 14113
		public const short BambooBlock = 4564;

		// Token: 0x04003722 RID: 14114
		public const short BambooBlockWall = 4565;

		// Token: 0x04003723 RID: 14115
		public const short BambooBathtub = 4566;

		// Token: 0x04003724 RID: 14116
		public const short BambooBed = 4567;

		// Token: 0x04003725 RID: 14117
		public const short BambooBookcase = 4568;

		// Token: 0x04003726 RID: 14118
		public const short BambooDresser = 4569;

		// Token: 0x04003727 RID: 14119
		public const short BambooCandelabra = 4570;

		// Token: 0x04003728 RID: 14120
		public const short BambooCandle = 4571;

		// Token: 0x04003729 RID: 14121
		public const short BambooChair = 4572;

		// Token: 0x0400372A RID: 14122
		public const short BambooChandelier = 4573;

		// Token: 0x0400372B RID: 14123
		public const short BambooChest = 4574;

		// Token: 0x0400372C RID: 14124
		public const short BambooClock = 4575;

		// Token: 0x0400372D RID: 14125
		public const short BambooDoor = 4576;

		// Token: 0x0400372E RID: 14126
		public const short BambooLamp = 4577;

		// Token: 0x0400372F RID: 14127
		public const short BambooLantern = 4578;

		// Token: 0x04003730 RID: 14128
		public const short BambooPiano = 4579;

		// Token: 0x04003731 RID: 14129
		public const short BambooPlatform = 4580;

		// Token: 0x04003732 RID: 14130
		public const short BambooSink = 4581;

		// Token: 0x04003733 RID: 14131
		public const short BambooSofa = 4582;

		// Token: 0x04003734 RID: 14132
		public const short BambooTable = 4583;

		// Token: 0x04003735 RID: 14133
		public const short BambooWorkbench = 4584;

		// Token: 0x04003736 RID: 14134
		public const short Fake_BambooChest = 4585;

		// Token: 0x04003737 RID: 14135
		public const short BambooToilet = 4586;

		// Token: 0x04003738 RID: 14136
		public const short GolfClubStoneIron = 4587;

		// Token: 0x04003739 RID: 14137
		public const short GolfClubRustyPutter = 4588;

		// Token: 0x0400373A RID: 14138
		public const short GolfClubBronzeWedge = 4589;

		// Token: 0x0400373B RID: 14139
		public const short GolfClubWoodDriver = 4590;

		// Token: 0x0400373C RID: 14140
		public const short GolfClubMythrilIron = 4591;

		// Token: 0x0400373D RID: 14141
		public const short GolfClubLeadPutter = 4592;

		// Token: 0x0400373E RID: 14142
		public const short GolfClubGoldWedge = 4593;

		// Token: 0x0400373F RID: 14143
		public const short GolfClubPearlwoodDriver = 4594;

		// Token: 0x04003740 RID: 14144
		public const short GolfClubTitaniumIron = 4595;

		// Token: 0x04003741 RID: 14145
		public const short GolfClubShroomitePutter = 4596;

		// Token: 0x04003742 RID: 14146
		public const short GolfClubDiamondWedge = 4597;

		// Token: 0x04003743 RID: 14147
		public const short GolfClubChlorophyteDriver = 4598;

		// Token: 0x04003744 RID: 14148
		public const short GolfTrophyBronze = 4599;

		// Token: 0x04003745 RID: 14149
		public const short GolfTrophySilver = 4600;

		// Token: 0x04003746 RID: 14150
		public const short GolfTrophyGold = 4601;

		// Token: 0x04003747 RID: 14151
		public const short BloodNautilusBanner = 4602;

		// Token: 0x04003748 RID: 14152
		public const short BirdieRattle = 4603;

		// Token: 0x04003749 RID: 14153
		public const short ExoticEasternChewToy = 4604;

		// Token: 0x0400374A RID: 14154
		public const short BedazzledNectar = 4605;

		// Token: 0x0400374B RID: 14155
		public const short MusicBoxJungleNight = 4606;

		// Token: 0x0400374C RID: 14156
		public const short StormTigerStaff = 4607;

		// Token: 0x0400374D RID: 14157
		public const short ChumBucket = 4608;

		// Token: 0x0400374E RID: 14158
		public const short GardenGnome = 4609;

		// Token: 0x0400374F RID: 14159
		public const short KiteBoneSerpent = 4610;

		// Token: 0x04003750 RID: 14160
		public const short KiteWorldFeeder = 4611;

		// Token: 0x04003751 RID: 14161
		public const short KiteBunny = 4612;

		// Token: 0x04003752 RID: 14162
		public const short KitePigron = 4613;

		// Token: 0x04003753 RID: 14163
		public const short AppleJuice = 4614;

		// Token: 0x04003754 RID: 14164
		public const short GrapeJuice = 4615;

		// Token: 0x04003755 RID: 14165
		public const short Lemonade = 4616;

		// Token: 0x04003756 RID: 14166
		public const short BananaDaiquiri = 4617;

		// Token: 0x04003757 RID: 14167
		public const short PeachSangria = 4618;

		// Token: 0x04003758 RID: 14168
		public const short PinaColada = 4619;

		// Token: 0x04003759 RID: 14169
		public const short TropicalSmoothie = 4620;

		// Token: 0x0400375A RID: 14170
		public const short BloodyMoscato = 4621;

		// Token: 0x0400375B RID: 14171
		public const short SmoothieofDarkness = 4622;

		// Token: 0x0400375C RID: 14172
		public const short PrismaticPunch = 4623;

		// Token: 0x0400375D RID: 14173
		public const short FruitJuice = 4624;

		// Token: 0x0400375E RID: 14174
		public const short FruitSalad = 4625;

		// Token: 0x0400375F RID: 14175
		public const short AndrewSphinx = 4626;

		// Token: 0x04003760 RID: 14176
		public const short WatchfulAntlion = 4627;

		// Token: 0x04003761 RID: 14177
		public const short BurningSpirit = 4628;

		// Token: 0x04003762 RID: 14178
		public const short JawsOfDeath = 4629;

		// Token: 0x04003763 RID: 14179
		public const short TheSandsOfSlime = 4630;

		// Token: 0x04003764 RID: 14180
		public const short SnakesIHateSnakes = 4631;

		// Token: 0x04003765 RID: 14181
		public const short LifeAboveTheSand = 4632;

		// Token: 0x04003766 RID: 14182
		public const short Oasis = 4633;

		// Token: 0x04003767 RID: 14183
		public const short PrehistoryPreserved = 4634;

		// Token: 0x04003768 RID: 14184
		public const short AncientTablet = 4635;

		// Token: 0x04003769 RID: 14185
		public const short Uluru = 4636;

		// Token: 0x0400376A RID: 14186
		public const short VisitingThePyramids = 4637;

		// Token: 0x0400376B RID: 14187
		public const short BandageBoy = 4638;

		// Token: 0x0400376C RID: 14188
		public const short DivineEye = 4639;

		// Token: 0x0400376D RID: 14189
		public const short AmethystStoneBlock = 4640;

		// Token: 0x0400376E RID: 14190
		public const short TopazStoneBlock = 4641;

		// Token: 0x0400376F RID: 14191
		public const short SapphireStoneBlock = 4642;

		// Token: 0x04003770 RID: 14192
		public const short EmeraldStoneBlock = 4643;

		// Token: 0x04003771 RID: 14193
		public const short RubyStoneBlock = 4644;

		// Token: 0x04003772 RID: 14194
		public const short DiamondStoneBlock = 4645;

		// Token: 0x04003773 RID: 14195
		public const short AmberStoneBlock = 4646;

		// Token: 0x04003774 RID: 14196
		public const short AmberStoneWallEcho = 4647;

		// Token: 0x04003775 RID: 14197
		public const short KiteManEater = 4648;

		// Token: 0x04003776 RID: 14198
		public const short KiteJellyfishBlue = 4649;

		// Token: 0x04003777 RID: 14199
		public const short KiteJellyfishPink = 4650;

		// Token: 0x04003778 RID: 14200
		public const short KiteShark = 4651;

		// Token: 0x04003779 RID: 14201
		public const short SuperHeroMask = 4652;

		// Token: 0x0400377A RID: 14202
		public const short SuperHeroCostume = 4653;

		// Token: 0x0400377B RID: 14203
		public const short SuperHeroTights = 4654;

		// Token: 0x0400377C RID: 14204
		public const short PinkFairyJar = 4655;

		// Token: 0x0400377D RID: 14205
		public const short GreenFairyJar = 4656;

		// Token: 0x0400377E RID: 14206
		public const short BlueFairyJar = 4657;

		// Token: 0x0400377F RID: 14207
		public const short GolfPainting1 = 4658;

		// Token: 0x04003780 RID: 14208
		public const short GolfPainting2 = 4659;

		// Token: 0x04003781 RID: 14209
		public const short GolfPainting3 = 4660;

		// Token: 0x04003782 RID: 14210
		public const short GolfPainting4 = 4661;

		// Token: 0x04003783 RID: 14211
		public const short FogboundDye = 4662;

		// Token: 0x04003784 RID: 14212
		public const short BloodbathDye = 4663;

		// Token: 0x04003785 RID: 14213
		public const short PrettyPinkDressSkirt = 4664;

		// Token: 0x04003786 RID: 14214
		public const short PrettyPinkDressPants = 4665;

		// Token: 0x04003787 RID: 14215
		public const short PrettyPinkRibbon = 4666;

		// Token: 0x04003788 RID: 14216
		public const short BambooFence = 4667;

		// Token: 0x04003789 RID: 14217
		public const short GlowPaint = 4668;

		// Token: 0x0400378A RID: 14218
		public const short KiteSandShark = 4669;

		// Token: 0x0400378B RID: 14219
		public const short KiteBunnyCorrupt = 4670;

		// Token: 0x0400378C RID: 14220
		public const short KiteBunnyCrimson = 4671;

		// Token: 0x0400378D RID: 14221
		public const short BlandWhip = 4672;

		// Token: 0x0400378E RID: 14222
		public const short DrumStick = 4673;

		// Token: 0x0400378F RID: 14223
		public const short KiteGoldfish = 4674;

		// Token: 0x04003790 RID: 14224
		public const short KiteAngryTrapper = 4675;

		// Token: 0x04003791 RID: 14225
		public const short KiteKoi = 4676;

		// Token: 0x04003792 RID: 14226
		public const short KiteCrawltipede = 4677;

		// Token: 0x04003793 RID: 14227
		public const short SwordWhip = 4678;

		// Token: 0x04003794 RID: 14228
		public const short MaceWhip = 4679;

		// Token: 0x04003795 RID: 14229
		public const short ScytheWhip = 4680;

		// Token: 0x04003796 RID: 14230
		public const short KiteSpectrum = 4681;

		// Token: 0x04003797 RID: 14231
		public const short ReleaseDoves = 4682;

		// Token: 0x04003798 RID: 14232
		public const short KiteWanderingEye = 4683;

		// Token: 0x04003799 RID: 14233
		public const short KiteUnicorn = 4684;

		// Token: 0x0400379A RID: 14234
		public const short UndertakerHat = 4685;

		// Token: 0x0400379B RID: 14235
		public const short UndertakerCoat = 4686;

		// Token: 0x0400379C RID: 14236
		public const short DandelionBanner = 4687;

		// Token: 0x0400379D RID: 14237
		public const short GnomeBanner = 4688;

		// Token: 0x0400379E RID: 14238
		public const short DesertCampfire = 4689;

		// Token: 0x0400379F RID: 14239
		public const short CoralCampfire = 4690;

		// Token: 0x040037A0 RID: 14240
		public const short CorruptCampfire = 4691;

		// Token: 0x040037A1 RID: 14241
		public const short CrimsonCampfire = 4692;

		// Token: 0x040037A2 RID: 14242
		public const short HallowedCampfire = 4693;

		// Token: 0x040037A3 RID: 14243
		public const short JungleCampfire = 4694;

		// Token: 0x040037A4 RID: 14244
		public const short SoulBottleLight = 4695;

		// Token: 0x040037A5 RID: 14245
		public const short SoulBottleNight = 4696;

		// Token: 0x040037A6 RID: 14246
		public const short SoulBottleFlight = 4697;

		// Token: 0x040037A7 RID: 14247
		public const short SoulBottleSight = 4698;

		// Token: 0x040037A8 RID: 14248
		public const short SoulBottleMight = 4699;

		// Token: 0x040037A9 RID: 14249
		public const short SoulBottleFright = 4700;

		// Token: 0x040037AA RID: 14250
		public const short MudBud = 4701;

		// Token: 0x040037AB RID: 14251
		public const short ReleaseLantern = 4702;

		// Token: 0x040037AC RID: 14252
		public const short QuadBarrelShotgun = 4703;

		// Token: 0x040037AD RID: 14253
		public const short FuneralHat = 4704;

		// Token: 0x040037AE RID: 14254
		public const short FuneralCoat = 4705;

		// Token: 0x040037AF RID: 14255
		public const short FuneralPants = 4706;

		// Token: 0x040037B0 RID: 14256
		public const short TragicUmbrella = 4707;

		// Token: 0x040037B1 RID: 14257
		public const short VictorianGothHat = 4708;

		// Token: 0x040037B2 RID: 14258
		public const short VictorianGothDress = 4709;

		// Token: 0x040037B3 RID: 14259
		public const short TatteredWoodSign = 4710;

		// Token: 0x040037B4 RID: 14260
		public const short GravediggerShovel = 4711;

		// Token: 0x040037B5 RID: 14261
		public const short DungeonDesertChest = 4712;

		// Token: 0x040037B6 RID: 14262
		public const short Fake_DungeonDesertChest = 4713;

		// Token: 0x040037B7 RID: 14263
		public const short DungeonDesertKey = 4714;

		// Token: 0x040037B8 RID: 14264
		public const short SparkleGuitar = 4715;

		// Token: 0x040037B9 RID: 14265
		public const short MolluskWhistle = 4716;

		// Token: 0x040037BA RID: 14266
		public const short BorealBeam = 4717;

		// Token: 0x040037BB RID: 14267
		public const short RichMahoganyBeam = 4718;

		// Token: 0x040037BC RID: 14268
		public const short GraniteColumn = 4719;

		// Token: 0x040037BD RID: 14269
		public const short SandstoneColumn = 4720;

		// Token: 0x040037BE RID: 14270
		public const short MushroomBeam = 4721;

		// Token: 0x040037BF RID: 14271
		public const short FirstFractal = 4722;

		// Token: 0x040037C0 RID: 14272
		public const short Nevermore = 4723;

		// Token: 0x040037C1 RID: 14273
		public const short Reborn = 4724;

		// Token: 0x040037C2 RID: 14274
		public const short Graveyard = 4725;

		// Token: 0x040037C3 RID: 14275
		public const short GhostManifestation = 4726;

		// Token: 0x040037C4 RID: 14276
		public const short WickedUndead = 4727;

		// Token: 0x040037C5 RID: 14277
		public const short BloodyGoblet = 4728;

		// Token: 0x040037C6 RID: 14278
		public const short StillLife = 4729;

		// Token: 0x040037C7 RID: 14279
		public const short GhostarsWings = 4730;

		// Token: 0x040037C8 RID: 14280
		public const short TerraToilet = 4731;

		// Token: 0x040037C9 RID: 14281
		public const short GhostarSkullPin = 4732;

		// Token: 0x040037CA RID: 14282
		public const short GhostarShirt = 4733;

		// Token: 0x040037CB RID: 14283
		public const short GhostarPants = 4734;

		// Token: 0x040037CC RID: 14284
		public const short BallOfFuseWire = 4735;

		// Token: 0x040037CD RID: 14285
		public const short FullMoonSqueakyToy = 4736;

		// Token: 0x040037CE RID: 14286
		public const short OrnateShadowKey = 4737;

		// Token: 0x040037CF RID: 14287
		public const short DrManFlyMask = 4738;

		// Token: 0x040037D0 RID: 14288
		public const short DrManFlyLabCoat = 4739;

		// Token: 0x040037D1 RID: 14289
		public const short ButcherMask = 4740;

		// Token: 0x040037D2 RID: 14290
		public const short ButcherApron = 4741;

		// Token: 0x040037D3 RID: 14291
		public const short ButcherPants = 4742;

		// Token: 0x040037D4 RID: 14292
		public const short Football = 4743;

		// Token: 0x040037D5 RID: 14293
		public const short HunterCloak = 4744;

		// Token: 0x040037D6 RID: 14294
		public const short CoffinMinecart = 4745;

		// Token: 0x040037D7 RID: 14295
		public const short SafemanWings = 4746;

		// Token: 0x040037D8 RID: 14296
		public const short SafemanSunHair = 4747;

		// Token: 0x040037D9 RID: 14297
		public const short SafemanSunDress = 4748;

		// Token: 0x040037DA RID: 14298
		public const short SafemanDressLeggings = 4749;

		// Token: 0x040037DB RID: 14299
		public const short FoodBarbarianWings = 4750;

		// Token: 0x040037DC RID: 14300
		public const short FoodBarbarianHelm = 4751;

		// Token: 0x040037DD RID: 14301
		public const short FoodBarbarianArmor = 4752;

		// Token: 0x040037DE RID: 14302
		public const short FoodBarbarianGreaves = 4753;

		// Token: 0x040037DF RID: 14303
		public const short GroxTheGreatWings = 4754;

		// Token: 0x040037E0 RID: 14304
		public const short GroxTheGreatHelm = 4755;

		// Token: 0x040037E1 RID: 14305
		public const short GroxTheGreatArmor = 4756;

		// Token: 0x040037E2 RID: 14306
		public const short GroxTheGreatGreaves = 4757;

		// Token: 0x040037E3 RID: 14307
		public const short Smolstar = 4758;

		// Token: 0x040037E4 RID: 14308
		public const short SquirrelHook = 4759;

		// Token: 0x040037E5 RID: 14309
		public const short BouncingShield = 4760;

		// Token: 0x040037E6 RID: 14310
		public const short RockGolemHead = 4761;

		// Token: 0x040037E7 RID: 14311
		public const short CritterShampoo = 4762;

		// Token: 0x040037E8 RID: 14312
		public const short DiggingMoleMinecart = 4763;

		// Token: 0x040037E9 RID: 14313
		public const short Shroomerang = 4764;

		// Token: 0x040037EA RID: 14314
		public const short TreeGlobe = 4765;

		// Token: 0x040037EB RID: 14315
		public const short WorldGlobe = 4766;

		// Token: 0x040037EC RID: 14316
		public const short DontHurtCrittersBook = 4767;

		// Token: 0x040037ED RID: 14317
		public const short DogEars = 4768;

		// Token: 0x040037EE RID: 14318
		public const short DogTail = 4769;

		// Token: 0x040037EF RID: 14319
		public const short FoxEars = 4770;

		// Token: 0x040037F0 RID: 14320
		public const short FoxTail = 4771;

		// Token: 0x040037F1 RID: 14321
		public const short LizardEars = 4772;

		// Token: 0x040037F2 RID: 14322
		public const short LizardTail = 4773;

		// Token: 0x040037F3 RID: 14323
		public const short PandaEars = 4774;

		// Token: 0x040037F4 RID: 14324
		public const short BunnyTail = 4775;

		// Token: 0x040037F5 RID: 14325
		public const short FairyGlowstick = 4776;

		// Token: 0x040037F6 RID: 14326
		public const short LightningCarrot = 4777;

		// Token: 0x040037F7 RID: 14327
		public const short HallowBossDye = 4778;

		// Token: 0x040037F8 RID: 14328
		public const short MushroomHat = 4779;

		// Token: 0x040037F9 RID: 14329
		public const short MushroomVest = 4780;

		// Token: 0x040037FA RID: 14330
		public const short MushroomPants = 4781;

		// Token: 0x040037FB RID: 14331
		public const short FairyQueenBossBag = 4782;

		// Token: 0x040037FC RID: 14332
		public const short FairyQueenTrophy = 4783;

		// Token: 0x040037FD RID: 14333
		public const short FairyQueenMask = 4784;

		// Token: 0x040037FE RID: 14334
		public const short PaintedHorseSaddle = 4785;

		// Token: 0x040037FF RID: 14335
		public const short MajesticHorseSaddle = 4786;

		// Token: 0x04003800 RID: 14336
		public const short DarkHorseSaddle = 4787;

		// Token: 0x04003801 RID: 14337
		public const short JoustingLance = 4788;

		// Token: 0x04003802 RID: 14338
		public const short ShadowJoustingLance = 4789;

		// Token: 0x04003803 RID: 14339
		public const short HallowJoustingLance = 4790;

		// Token: 0x04003804 RID: 14340
		public const short PogoStick = 4791;

		// Token: 0x04003805 RID: 14341
		public const short PirateShipMountItem = 4792;

		// Token: 0x04003806 RID: 14342
		public const short SpookyWoodMountItem = 4793;

		// Token: 0x04003807 RID: 14343
		public const short SantankMountItem = 4794;

		// Token: 0x04003808 RID: 14344
		public const short WallOfFleshGoatMountItem = 4795;

		// Token: 0x04003809 RID: 14345
		public const short DarkMageBookMountItem = 4796;

		// Token: 0x0400380A RID: 14346
		public const short KingSlimePetItem = 4797;

		// Token: 0x0400380B RID: 14347
		public const short EyeOfCthulhuPetItem = 4798;

		// Token: 0x0400380C RID: 14348
		public const short EaterOfWorldsPetItem = 4799;

		// Token: 0x0400380D RID: 14349
		public const short BrainOfCthulhuPetItem = 4800;

		// Token: 0x0400380E RID: 14350
		public const short SkeletronPetItem = 4801;

		// Token: 0x0400380F RID: 14351
		public const short QueenBeePetItem = 4802;

		// Token: 0x04003810 RID: 14352
		public const short DestroyerPetItem = 4803;

		// Token: 0x04003811 RID: 14353
		public const short TwinsPetItem = 4804;

		// Token: 0x04003812 RID: 14354
		public const short SkeletronPrimePetItem = 4805;

		// Token: 0x04003813 RID: 14355
		public const short PlanteraPetItem = 4806;

		// Token: 0x04003814 RID: 14356
		public const short GolemPetItem = 4807;

		// Token: 0x04003815 RID: 14357
		public const short DukeFishronPetItem = 4808;

		// Token: 0x04003816 RID: 14358
		public const short LunaticCultistPetItem = 4809;

		// Token: 0x04003817 RID: 14359
		public const short MoonLordPetItem = 4810;

		// Token: 0x04003818 RID: 14360
		public const short FairyQueenPetItem = 4811;

		// Token: 0x04003819 RID: 14361
		public const short PumpkingPetItem = 4812;

		// Token: 0x0400381A RID: 14362
		public const short EverscreamPetItem = 4813;

		// Token: 0x0400381B RID: 14363
		public const short IceQueenPetItem = 4814;

		// Token: 0x0400381C RID: 14364
		public const short MartianPetItem = 4815;

		// Token: 0x0400381D RID: 14365
		public const short DD2OgrePetItem = 4816;

		// Token: 0x0400381E RID: 14366
		public const short DD2BetsyPetItem = 4817;

		// Token: 0x0400381F RID: 14367
		public const short CombatWrench = 4818;

		// Token: 0x04003820 RID: 14368
		public const short DemonConch = 4819;

		// Token: 0x04003821 RID: 14369
		public const short BottomlessLavaBucket = 4820;

		// Token: 0x04003822 RID: 14370
		public const short FireproofBugNet = 4821;

		// Token: 0x04003823 RID: 14371
		public const short FlameWakerBoots = 4822;

		// Token: 0x04003824 RID: 14372
		public const short RainbowWings = 4823;

		// Token: 0x04003825 RID: 14373
		public const short WetBomb = 4824;

		// Token: 0x04003826 RID: 14374
		public const short LavaBomb = 4825;

		// Token: 0x04003827 RID: 14375
		public const short HoneyBomb = 4826;

		// Token: 0x04003828 RID: 14376
		public const short DryBomb = 4827;

		// Token: 0x04003829 RID: 14377
		public const short SuperheatedBlood = 4828;

		// Token: 0x0400382A RID: 14378
		public const short LicenseCat = 4829;

		// Token: 0x0400382B RID: 14379
		public const short LicenseDog = 4830;

		// Token: 0x0400382C RID: 14380
		public const short GemSquirrelAmethyst = 4831;

		// Token: 0x0400382D RID: 14381
		public const short GemSquirrelTopaz = 4832;

		// Token: 0x0400382E RID: 14382
		public const short GemSquirrelSapphire = 4833;

		// Token: 0x0400382F RID: 14383
		public const short GemSquirrelEmerald = 4834;

		// Token: 0x04003830 RID: 14384
		public const short GemSquirrelRuby = 4835;

		// Token: 0x04003831 RID: 14385
		public const short GemSquirrelDiamond = 4836;

		// Token: 0x04003832 RID: 14386
		public const short GemSquirrelAmber = 4837;

		// Token: 0x04003833 RID: 14387
		public const short GemBunnyAmethyst = 4838;

		// Token: 0x04003834 RID: 14388
		public const short GemBunnyTopaz = 4839;

		// Token: 0x04003835 RID: 14389
		public const short GemBunnySapphire = 4840;

		// Token: 0x04003836 RID: 14390
		public const short GemBunnyEmerald = 4841;

		// Token: 0x04003837 RID: 14391
		public const short GemBunnyRuby = 4842;

		// Token: 0x04003838 RID: 14392
		public const short GemBunnyDiamond = 4843;

		// Token: 0x04003839 RID: 14393
		public const short GemBunnyAmber = 4844;

		// Token: 0x0400383A RID: 14394
		public const short HellButterfly = 4845;

		// Token: 0x0400383B RID: 14395
		public const short HellButterflyJar = 4846;

		// Token: 0x0400383C RID: 14396
		public const short Lavafly = 4847;

		// Token: 0x0400383D RID: 14397
		public const short LavaflyinaBottle = 4848;

		// Token: 0x0400383E RID: 14398
		public const short MagmaSnail = 4849;

		// Token: 0x0400383F RID: 14399
		public const short MagmaSnailCage = 4850;

		// Token: 0x04003840 RID: 14400
		public const short GemTreeTopazSeed = 4851;

		// Token: 0x04003841 RID: 14401
		public const short GemTreeAmethystSeed = 4852;

		// Token: 0x04003842 RID: 14402
		public const short GemTreeSapphireSeed = 4853;

		// Token: 0x04003843 RID: 14403
		public const short GemTreeEmeraldSeed = 4854;

		// Token: 0x04003844 RID: 14404
		public const short GemTreeRubySeed = 4855;

		// Token: 0x04003845 RID: 14405
		public const short GemTreeDiamondSeed = 4856;

		// Token: 0x04003846 RID: 14406
		public const short GemTreeAmberSeed = 4857;

		// Token: 0x04003847 RID: 14407
		public const short PotSuspended = 4858;

		// Token: 0x04003848 RID: 14408
		public const short PotSuspendedDaybloom = 4859;

		// Token: 0x04003849 RID: 14409
		public const short PotSuspendedMoonglow = 4860;

		// Token: 0x0400384A RID: 14410
		public const short PotSuspendedWaterleaf = 4861;

		// Token: 0x0400384B RID: 14411
		public const short PotSuspendedShiverthorn = 4862;

		// Token: 0x0400384C RID: 14412
		public const short PotSuspendedBlinkroot = 4863;

		// Token: 0x0400384D RID: 14413
		public const short PotSuspendedDeathweedCorrupt = 4864;

		// Token: 0x0400384E RID: 14414
		public const short PotSuspendedDeathweedCrimson = 4865;

		// Token: 0x0400384F RID: 14415
		public const short PotSuspendedFireblossom = 4866;

		// Token: 0x04003850 RID: 14416
		public const short BrazierSuspended = 4867;

		// Token: 0x04003851 RID: 14417
		public const short VolcanoSmall = 4868;

		// Token: 0x04003852 RID: 14418
		public const short VolcanoLarge = 4869;

		// Token: 0x04003853 RID: 14419
		public const short PotionOfReturn = 4870;

		// Token: 0x04003854 RID: 14420
		public const short VanityTreeSakuraSeed = 4871;

		// Token: 0x04003855 RID: 14421
		public const short LavaAbsorbantSponge = 4872;

		// Token: 0x04003856 RID: 14422
		public const short HallowedHood = 4873;

		// Token: 0x04003857 RID: 14423
		public const short HellfireTreads = 4874;

		// Token: 0x04003858 RID: 14424
		public const short TeleportationPylonJungle = 4875;

		// Token: 0x04003859 RID: 14425
		public const short TeleportationPylonPurity = 4876;

		// Token: 0x0400385A RID: 14426
		public const short LavaCrate = 4877;

		// Token: 0x0400385B RID: 14427
		public const short LavaCrateHard = 4878;

		// Token: 0x0400385C RID: 14428
		public const short ObsidianLockbox = 4879;

		// Token: 0x0400385D RID: 14429
		public const short LavaFishbowl = 4880;

		// Token: 0x0400385E RID: 14430
		public const short LavaFishingHook = 4881;

		// Token: 0x0400385F RID: 14431
		public const short AmethystBunnyCage = 4882;

		// Token: 0x04003860 RID: 14432
		public const short TopazBunnyCage = 4883;

		// Token: 0x04003861 RID: 14433
		public const short SapphireBunnyCage = 4884;

		// Token: 0x04003862 RID: 14434
		public const short EmeraldBunnyCage = 4885;

		// Token: 0x04003863 RID: 14435
		public const short RubyBunnyCage = 4886;

		// Token: 0x04003864 RID: 14436
		public const short DiamondBunnyCage = 4887;

		// Token: 0x04003865 RID: 14437
		public const short AmberBunnyCage = 4888;

		// Token: 0x04003866 RID: 14438
		public const short AmethystSquirrelCage = 4889;

		// Token: 0x04003867 RID: 14439
		public const short TopazSquirrelCage = 4890;

		// Token: 0x04003868 RID: 14440
		public const short SapphireSquirrelCage = 4891;

		// Token: 0x04003869 RID: 14441
		public const short EmeraldSquirrelCage = 4892;

		// Token: 0x0400386A RID: 14442
		public const short RubySquirrelCage = 4893;

		// Token: 0x0400386B RID: 14443
		public const short DiamondSquirrelCage = 4894;

		// Token: 0x0400386C RID: 14444
		public const short AmberSquirrelCage = 4895;

		// Token: 0x0400386D RID: 14445
		public const short AncientHallowedMask = 4896;

		// Token: 0x0400386E RID: 14446
		public const short AncientHallowedHelmet = 4897;

		// Token: 0x0400386F RID: 14447
		public const short AncientHallowedHeadgear = 4898;

		// Token: 0x04003870 RID: 14448
		public const short AncientHallowedHood = 4899;

		// Token: 0x04003871 RID: 14449
		public const short AncientHallowedPlateMail = 4900;

		// Token: 0x04003872 RID: 14450
		public const short AncientHallowedGreaves = 4901;

		// Token: 0x04003873 RID: 14451
		public const short PottedLavaPlantPalm = 4902;

		// Token: 0x04003874 RID: 14452
		public const short PottedLavaPlantBush = 4903;

		// Token: 0x04003875 RID: 14453
		public const short PottedLavaPlantBramble = 4904;

		// Token: 0x04003876 RID: 14454
		public const short PottedLavaPlantBulb = 4905;

		// Token: 0x04003877 RID: 14455
		public const short PottedLavaPlantTendrils = 4906;

		// Token: 0x04003878 RID: 14456
		public const short VanityTreeYellowWillowSeed = 4907;

		// Token: 0x04003879 RID: 14457
		public const short DirtBomb = 4908;

		// Token: 0x0400387A RID: 14458
		public const short DirtStickyBomb = 4909;

		// Token: 0x0400387B RID: 14459
		public const short LicenseBunny = 4910;

		// Token: 0x0400387C RID: 14460
		public const short CoolWhip = 4911;

		// Token: 0x0400387D RID: 14461
		public const short FireWhip = 4912;

		// Token: 0x0400387E RID: 14462
		public const short ThornWhip = 4913;

		// Token: 0x0400387F RID: 14463
		public const short RainbowWhip = 4914;

		// Token: 0x04003880 RID: 14464
		public const short TungstenBullet = 4915;

		// Token: 0x04003881 RID: 14465
		public const short TeleportationPylonHallow = 4916;

		// Token: 0x04003882 RID: 14466
		public const short TeleportationPylonUnderground = 4917;

		// Token: 0x04003883 RID: 14467
		public const short TeleportationPylonOcean = 4918;

		// Token: 0x04003884 RID: 14468
		public const short TeleportationPylonDesert = 4919;

		// Token: 0x04003885 RID: 14469
		public const short TeleportationPylonSnow = 4920;

		// Token: 0x04003886 RID: 14470
		public const short TeleportationPylonMushroom = 4921;

		// Token: 0x04003887 RID: 14471
		public const short CavernFountain = 4922;

		// Token: 0x04003888 RID: 14472
		public const short PiercingStarlight = 4923;

		// Token: 0x04003889 RID: 14473
		public const short EyeofCthulhuMasterTrophy = 4924;

		// Token: 0x0400388A RID: 14474
		public const short EaterofWorldsMasterTrophy = 4925;

		// Token: 0x0400388B RID: 14475
		public const short BrainofCthulhuMasterTrophy = 4926;

		// Token: 0x0400388C RID: 14476
		public const short SkeletronMasterTrophy = 4927;

		// Token: 0x0400388D RID: 14477
		public const short QueenBeeMasterTrophy = 4928;

		// Token: 0x0400388E RID: 14478
		public const short KingSlimeMasterTrophy = 4929;

		// Token: 0x0400388F RID: 14479
		public const short WallofFleshMasterTrophy = 4930;

		// Token: 0x04003890 RID: 14480
		public const short TwinsMasterTrophy = 4931;

		// Token: 0x04003891 RID: 14481
		public const short DestroyerMasterTrophy = 4932;

		// Token: 0x04003892 RID: 14482
		public const short SkeletronPrimeMasterTrophy = 4933;

		// Token: 0x04003893 RID: 14483
		public const short PlanteraMasterTrophy = 4934;

		// Token: 0x04003894 RID: 14484
		public const short GolemMasterTrophy = 4935;

		// Token: 0x04003895 RID: 14485
		public const short DukeFishronMasterTrophy = 4936;

		// Token: 0x04003896 RID: 14486
		public const short LunaticCultistMasterTrophy = 4937;

		// Token: 0x04003897 RID: 14487
		public const short MoonLordMasterTrophy = 4938;

		// Token: 0x04003898 RID: 14488
		public const short UFOMasterTrophy = 4939;

		// Token: 0x04003899 RID: 14489
		public const short FlyingDutchmanMasterTrophy = 4940;

		// Token: 0x0400389A RID: 14490
		public const short MourningWoodMasterTrophy = 4941;

		// Token: 0x0400389B RID: 14491
		public const short PumpkingMasterTrophy = 4942;

		// Token: 0x0400389C RID: 14492
		public const short IceQueenMasterTrophy = 4943;

		// Token: 0x0400389D RID: 14493
		public const short EverscreamMasterTrophy = 4944;

		// Token: 0x0400389E RID: 14494
		public const short SantankMasterTrophy = 4945;

		// Token: 0x0400389F RID: 14495
		public const short DarkMageMasterTrophy = 4946;

		// Token: 0x040038A0 RID: 14496
		public const short OgreMasterTrophy = 4947;

		// Token: 0x040038A1 RID: 14497
		public const short BetsyMasterTrophy = 4948;

		// Token: 0x040038A2 RID: 14498
		public const short FairyQueenMasterTrophy = 4949;

		// Token: 0x040038A3 RID: 14499
		public const short QueenSlimeMasterTrophy = 4950;

		// Token: 0x040038A4 RID: 14500
		public const short TeleportationPylonVictory = 4951;

		// Token: 0x040038A5 RID: 14501
		public const short FairyQueenMagicItem = 4952;

		// Token: 0x040038A6 RID: 14502
		public const short FairyQueenRangedItem = 4953;

		// Token: 0x040038A7 RID: 14503
		public const short LongRainbowTrailWings = 4954;

		// Token: 0x040038A8 RID: 14504
		public const short RabbitOrder = 4955;

		// Token: 0x040038A9 RID: 14505
		public const short Zenith = 4956;

		// Token: 0x040038AA RID: 14506
		public const short QueenSlimeBossBag = 4957;

		// Token: 0x040038AB RID: 14507
		public const short QueenSlimeTrophy = 4958;

		// Token: 0x040038AC RID: 14508
		public const short QueenSlimeMask = 4959;

		// Token: 0x040038AD RID: 14509
		public const short QueenSlimePetItem = 4960;

		// Token: 0x040038AE RID: 14510
		public const short EmpressButterfly = 4961;

		// Token: 0x040038AF RID: 14511
		public const short AccentSlab = 4962;

		// Token: 0x040038B0 RID: 14512
		public const short TruffleWormCage = 4963;

		// Token: 0x040038B1 RID: 14513
		public const short EmpressButterflyJar = 4964;

		// Token: 0x040038B2 RID: 14514
		public const short RockGolemBanner = 4965;

		// Token: 0x040038B3 RID: 14515
		public const short BloodMummyBanner = 4966;

		// Token: 0x040038B4 RID: 14516
		public const short SporeSkeletonBanner = 4967;

		// Token: 0x040038B5 RID: 14517
		public const short SporeBatBanner = 4968;

		// Token: 0x040038B6 RID: 14518
		public const short LarvaeAntlionBanner = 4969;

		// Token: 0x040038B7 RID: 14519
		public const short CrimsonBunnyBanner = 4970;

		// Token: 0x040038B8 RID: 14520
		public const short CrimsonGoldfishBanner = 4971;

		// Token: 0x040038B9 RID: 14521
		public const short CrimsonPenguinBanner = 4972;

		// Token: 0x040038BA RID: 14522
		public const short BigMimicCorruptionBanner = 4973;

		// Token: 0x040038BB RID: 14523
		public const short BigMimicCrimsonBanner = 4974;

		// Token: 0x040038BC RID: 14524
		public const short BigMimicHallowBanner = 4975;

		// Token: 0x040038BD RID: 14525
		public const short MossHornetBanner = 4976;

		// Token: 0x040038BE RID: 14526
		public const short WanderingEyeBanner = 4977;

		// Token: 0x040038BF RID: 14527
		public const short CreativeWings = 4978;

		// Token: 0x040038C0 RID: 14528
		public const short MusicBoxQueenSlime = 4979;

		// Token: 0x040038C1 RID: 14529
		public const short QueenSlimeHook = 4980;

		// Token: 0x040038C2 RID: 14530
		public const short QueenSlimeMountSaddle = 4981;

		// Token: 0x040038C3 RID: 14531
		public const short CrystalNinjaHelmet = 4982;

		// Token: 0x040038C4 RID: 14532
		public const short CrystalNinjaChestplate = 4983;

		// Token: 0x040038C5 RID: 14533
		public const short CrystalNinjaLeggings = 4984;

		// Token: 0x040038C6 RID: 14534
		public const short MusicBoxEmpressOfLight = 4985;

		// Token: 0x040038C7 RID: 14535
		public const short GelBalloon = 4986;

		// Token: 0x040038C8 RID: 14536
		public const short VolatileGelatin = 4987;

		// Token: 0x040038C9 RID: 14537
		public const short QueenSlimeCrystal = 4988;

		// Token: 0x040038CA RID: 14538
		public const short EmpressFlightBooster = 4989;

		// Token: 0x040038CB RID: 14539
		public const short MusicBoxDukeFishron = 4990;

		// Token: 0x040038CC RID: 14540
		public const short MusicBoxMorningRain = 4991;

		// Token: 0x040038CD RID: 14541
		public const short MusicBoxConsoleTitle = 4992;

		// Token: 0x040038CE RID: 14542
		public const short ChippysCouch = 4993;

		// Token: 0x040038CF RID: 14543
		public const short GraduationCapBlue = 4994;

		// Token: 0x040038D0 RID: 14544
		public const short GraduationCapMaroon = 4995;

		// Token: 0x040038D1 RID: 14545
		public const short GraduationCapBlack = 4996;

		// Token: 0x040038D2 RID: 14546
		public const short GraduationGownBlue = 4997;

		// Token: 0x040038D3 RID: 14547
		public const short GraduationGownMaroon = 4998;

		// Token: 0x040038D4 RID: 14548
		public const short GraduationGownBlack = 4999;

		// Token: 0x040038D5 RID: 14549
		public const short TerrasparkBoots = 5000;

		// Token: 0x040038D6 RID: 14550
		public const short MoonLordLegs = 5001;

		// Token: 0x040038D7 RID: 14551
		public const short OceanCrate = 5002;

		// Token: 0x040038D8 RID: 14552
		public const short OceanCrateHard = 5003;

		// Token: 0x040038D9 RID: 14553
		public const short BadgersHat = 5004;

		// Token: 0x040038DA RID: 14554
		public const short EmpressBlade = 5005;

		// Token: 0x040038DB RID: 14555
		public const short MusicBoxUndergroundDesert = 5006;

		// Token: 0x040038DC RID: 14556
		public const short DeadMansSweater = 5007;

		// Token: 0x040038DD RID: 14557
		public const short TeaKettle = 5008;

		// Token: 0x040038DE RID: 14558
		public const short Teacup = 5009;

		// Token: 0x040038DF RID: 14559
		public const short TreasureMagnet = 5010;

		// Token: 0x040038E0 RID: 14560
		public const short Mace = 5011;

		// Token: 0x040038E1 RID: 14561
		public const short FlamingMace = 5012;

		// Token: 0x040038E2 RID: 14562
		public const short SleepingIcon = 5013;

		// Token: 0x040038E3 RID: 14563
		public const short MusicBoxOWRain = 5014;

		// Token: 0x040038E4 RID: 14564
		public const short MusicBoxOWDay = 5015;

		// Token: 0x040038E5 RID: 14565
		public const short MusicBoxOWNight = 5016;

		// Token: 0x040038E6 RID: 14566
		public const short MusicBoxOWUnderground = 5017;

		// Token: 0x040038E7 RID: 14567
		public const short MusicBoxOWDesert = 5018;

		// Token: 0x040038E8 RID: 14568
		public const short MusicBoxOWOcean = 5019;

		// Token: 0x040038E9 RID: 14569
		public const short MusicBoxOWMushroom = 5020;

		// Token: 0x040038EA RID: 14570
		public const short MusicBoxOWDungeon = 5021;

		// Token: 0x040038EB RID: 14571
		public const short MusicBoxOWSpace = 5022;

		// Token: 0x040038EC RID: 14572
		public const short MusicBoxOWUnderworld = 5023;

		// Token: 0x040038ED RID: 14573
		public const short MusicBoxOWSnow = 5024;

		// Token: 0x040038EE RID: 14574
		public const short MusicBoxOWCorruption = 5025;

		// Token: 0x040038EF RID: 14575
		public const short MusicBoxOWUndergroundCorruption = 5026;

		// Token: 0x040038F0 RID: 14576
		public const short MusicBoxOWCrimson = 5027;

		// Token: 0x040038F1 RID: 14577
		public const short MusicBoxOWUndergroundCrimson = 5028;

		// Token: 0x040038F2 RID: 14578
		public const short MusicBoxOWUndergroundSnow = 5029;

		// Token: 0x040038F3 RID: 14579
		public const short MusicBoxOWUndergroundHallow = 5030;

		// Token: 0x040038F4 RID: 14580
		public const short MusicBoxOWBloodMoon = 5031;

		// Token: 0x040038F5 RID: 14581
		public const short MusicBoxOWBoss2 = 5032;

		// Token: 0x040038F6 RID: 14582
		public const short MusicBoxOWBoss1 = 5033;

		// Token: 0x040038F7 RID: 14583
		public const short MusicBoxOWInvasion = 5034;

		// Token: 0x040038F8 RID: 14584
		public const short MusicBoxOWTowers = 5035;

		// Token: 0x040038F9 RID: 14585
		public const short MusicBoxOWMoonLord = 5036;

		// Token: 0x040038FA RID: 14586
		public const short MusicBoxOWPlantera = 5037;

		// Token: 0x040038FB RID: 14587
		public const short MusicBoxOWJungle = 5038;

		// Token: 0x040038FC RID: 14588
		public const short MusicBoxOWWallOfFlesh = 5039;

		// Token: 0x040038FD RID: 14589
		public const short MusicBoxOWHallow = 5040;

		// Token: 0x040038FE RID: 14590
		public const short MilkCarton = 5041;

		// Token: 0x040038FF RID: 14591
		public const short CoffeeCup = 5042;

		// Token: 0x04003900 RID: 14592
		public const short TorchGodsFavor = 5043;

		// Token: 0x04003901 RID: 14593
		public const short MusicBoxCredits = 5044;

		// Token: 0x04003902 RID: 14594
		public const short PlaguebringerHelmet = 5045;

		// Token: 0x04003903 RID: 14595
		public const short PlaguebringerChestplate = 5046;

		// Token: 0x04003904 RID: 14596
		public const short PlaguebringerGreaves = 5047;

		// Token: 0x04003905 RID: 14597
		public const short RoninHat = 5048;

		// Token: 0x04003906 RID: 14598
		public const short RoninShirt = 5049;

		// Token: 0x04003907 RID: 14599
		public const short RoninPants = 5050;

		// Token: 0x04003908 RID: 14600
		public const short TimelessTravelerHood = 5051;

		// Token: 0x04003909 RID: 14601
		public const short TimelessTravelerRobe = 5052;

		// Token: 0x0400390A RID: 14602
		public const short TimelessTravelerBottom = 5053;

		// Token: 0x0400390B RID: 14603
		public const short FloretProtectorHelmet = 5054;

		// Token: 0x0400390C RID: 14604
		public const short FloretProtectorChestplate = 5055;

		// Token: 0x0400390D RID: 14605
		public const short FloretProtectorLegs = 5056;

		// Token: 0x0400390E RID: 14606
		public const short CapricornMask = 5057;

		// Token: 0x0400390F RID: 14607
		public const short CapricornChestplate = 5058;

		// Token: 0x04003910 RID: 14608
		public const short CapricornLegs = 5059;

		// Token: 0x04003911 RID: 14609
		public const short CapricornTail = 5060;

		// Token: 0x04003912 RID: 14610
		public const short TVHeadMask = 5061;

		// Token: 0x04003913 RID: 14611
		public const short TVHeadSuit = 5062;

		// Token: 0x04003914 RID: 14612
		public const short TVHeadPants = 5063;

		// Token: 0x04003915 RID: 14613
		public const short LavaproofTackleBag = 5064;

		// Token: 0x04003916 RID: 14614
		public const short PrincessWeapon = 5065;

		// Token: 0x04003917 RID: 14615
		public const short BeeHive = 5066;

		// Token: 0x04003918 RID: 14616
		public const short AntlionEggs = 5067;

		// Token: 0x04003919 RID: 14617
		public const short FlinxFurCoat = 5068;

		// Token: 0x0400391A RID: 14618
		public const short FlinxStaff = 5069;

		// Token: 0x0400391B RID: 14619
		public const short FlinxFur = 5070;

		// Token: 0x0400391C RID: 14620
		public const short RoyalTiara = 5071;

		// Token: 0x0400391D RID: 14621
		public const short RoyalDressTop = 5072;

		// Token: 0x0400391E RID: 14622
		public const short RoyalDressBottom = 5073;

		// Token: 0x0400391F RID: 14623
		public const short BoneWhip = 5074;

		// Token: 0x04003920 RID: 14624
		public const short RainbowCursor = 5075;

		// Token: 0x04003921 RID: 14625
		public const short RoyalScepter = 5076;

		// Token: 0x04003922 RID: 14626
		public const short GlassSlipper = 5077;

		// Token: 0x04003923 RID: 14627
		public const short PrinceUniform = 5078;

		// Token: 0x04003924 RID: 14628
		public const short PrincePants = 5079;

		// Token: 0x04003925 RID: 14629
		public const short PrinceCape = 5080;

		// Token: 0x04003926 RID: 14630
		public const short PottedCrystalPlantFern = 5081;

		// Token: 0x04003927 RID: 14631
		public const short PottedCrystalPlantSpiral = 5082;

		// Token: 0x04003928 RID: 14632
		public const short PottedCrystalPlantTeardrop = 5083;

		// Token: 0x04003929 RID: 14633
		public const short PottedCrystalPlantTree = 5084;

		// Token: 0x0400392A RID: 14634
		public const short Princess64 = 5085;

		// Token: 0x0400392B RID: 14635
		public const short PaintingOfALass = 5086;

		// Token: 0x0400392C RID: 14636
		public const short DarkSideHallow = 5087;

		// Token: 0x0400392D RID: 14637
		public const short BerniePetItem = 5088;

		// Token: 0x0400392E RID: 14638
		public const short GlommerPetItem = 5089;

		// Token: 0x0400392F RID: 14639
		public const short DeerclopsPetItem = 5090;

		// Token: 0x04003930 RID: 14640
		public const short PigPetItem = 5091;

		// Token: 0x04003931 RID: 14641
		public const short MonsterLasagna = 5092;

		// Token: 0x04003932 RID: 14642
		public const short FroggleBunwich = 5093;

		// Token: 0x04003933 RID: 14643
		public const short TentacleSpike = 5094;

		// Token: 0x04003934 RID: 14644
		public const short LucyTheAxe = 5095;

		// Token: 0x04003935 RID: 14645
		public const short HamBat = 5096;

		// Token: 0x04003936 RID: 14646
		public const short BatBat = 5097;

		// Token: 0x04003937 RID: 14647
		public const short ChesterPetItem = 5098;

		// Token: 0x04003938 RID: 14648
		public const short GarlandHat = 5099;

		// Token: 0x04003939 RID: 14649
		public const short BoneHelm = 5100;

		// Token: 0x0400393A RID: 14650
		public const short Eyebrella = 5101;

		// Token: 0x0400393B RID: 14651
		public const short WilsonShirt = 5102;

		// Token: 0x0400393C RID: 14652
		public const short WilsonPants = 5103;

		// Token: 0x0400393D RID: 14653
		public const short WilsonBeardShort = 5104;

		// Token: 0x0400393E RID: 14654
		public const short WilsonBeardLong = 5105;

		// Token: 0x0400393F RID: 14655
		public const short WilsonBeardMagnificent = 5106;

		// Token: 0x04003940 RID: 14656
		public const short Magiluminescence = 5107;

		// Token: 0x04003941 RID: 14657
		public const short DeerclopsTrophy = 5108;

		// Token: 0x04003942 RID: 14658
		public const short DeerclopsMask = 5109;

		// Token: 0x04003943 RID: 14659
		public const short DeerclopsMasterTrophy = 5110;

		// Token: 0x04003944 RID: 14660
		public const short DeerclopsBossBag = 5111;

		// Token: 0x04003945 RID: 14661
		public const short MusicBoxDeerclops = 5112;

		// Token: 0x04003946 RID: 14662
		public const short DontStarveShaderItem = 5113;

		// Token: 0x04003947 RID: 14663
		public const short AbigailsFlower = 5114;

		// Token: 0x04003948 RID: 14664
		public const short WillowShirt = 5115;

		// Token: 0x04003949 RID: 14665
		public const short WillowSkirt = 5116;

		// Token: 0x0400394A RID: 14666
		public const short PewMaticHorn = 5117;

		// Token: 0x0400394B RID: 14667
		public const short WeatherPain = 5118;

		// Token: 0x0400394C RID: 14668
		public const short HoundiusShootius = 5119;

		// Token: 0x0400394D RID: 14669
		public const short DeerThing = 5120;

		// Token: 0x0400394E RID: 14670
		public const short PaintingWilson = 5121;

		// Token: 0x0400394F RID: 14671
		public const short PaintingWillow = 5122;

		// Token: 0x04003950 RID: 14672
		public const short PaintingWendy = 5123;

		// Token: 0x04003951 RID: 14673
		public const short PaintingWolfgang = 5124;

		// Token: 0x04003952 RID: 14674
		public const short FartMinecart = 5125;

		// Token: 0x04003953 RID: 14675
		public const short HandOfCreation = 5126;

		// Token: 0x04003954 RID: 14676
		public const short VioletMoss = 5127;

		// Token: 0x04003955 RID: 14677
		public const short RainbowMoss = 5128;

		// Token: 0x04003956 RID: 14678
		public const short Flymeal = 5129;

		// Token: 0x04003957 RID: 14679
		public const short WolfMountItem = 5130;

		// Token: 0x04003958 RID: 14680
		public const short ResplendentDessert = 5131;

		// Token: 0x04003959 RID: 14681
		public const short Stinkbug = 5132;

		// Token: 0x0400395A RID: 14682
		public const short StinkbugCage = 5133;

		// Token: 0x0400395B RID: 14683
		public const short Clentaminator2 = 5134;

		// Token: 0x0400395C RID: 14684
		public const short VenomDartTrap = 5135;

		// Token: 0x0400395D RID: 14685
		public const short VulkelfEar = 5136;

		// Token: 0x0400395E RID: 14686
		public const short StinkbugHousingBlocker = 5137;

		// Token: 0x0400395F RID: 14687
		public const short StinkbugHousingBlockerEcho = 5138;

		// Token: 0x04003960 RID: 14688
		public const short FishingBobber = 5139;

		// Token: 0x04003961 RID: 14689
		public const short FishingBobberGlowingStar = 5140;

		// Token: 0x04003962 RID: 14690
		public const short FishingBobberGlowingLava = 5141;

		// Token: 0x04003963 RID: 14691
		public const short FishingBobberGlowingKrypton = 5142;

		// Token: 0x04003964 RID: 14692
		public const short FishingBobberGlowingXenon = 5143;

		// Token: 0x04003965 RID: 14693
		public const short FishingBobberGlowingArgon = 5144;

		// Token: 0x04003966 RID: 14694
		public const short FishingBobberGlowingViolet = 5145;

		// Token: 0x04003967 RID: 14695
		public const short FishingBobberGlowingRainbow = 5146;

		// Token: 0x04003968 RID: 14696
		public const short WandofFrosting = 5147;

		// Token: 0x04003969 RID: 14697
		public const short CoralBathtub = 5148;

		// Token: 0x0400396A RID: 14698
		public const short CoralBed = 5149;

		// Token: 0x0400396B RID: 14699
		public const short CoralBookcase = 5150;

		// Token: 0x0400396C RID: 14700
		public const short CoralDresser = 5151;

		// Token: 0x0400396D RID: 14701
		public const short CoralCandelabra = 5152;

		// Token: 0x0400396E RID: 14702
		public const short CoralCandle = 5153;

		// Token: 0x0400396F RID: 14703
		public const short CoralChair = 5154;

		// Token: 0x04003970 RID: 14704
		public const short CoralChandelier = 5155;

		// Token: 0x04003971 RID: 14705
		public const short CoralChest = 5156;

		// Token: 0x04003972 RID: 14706
		public const short CoralClock = 5157;

		// Token: 0x04003973 RID: 14707
		public const short CoralDoor = 5158;

		// Token: 0x04003974 RID: 14708
		public const short CoralLamp = 5159;

		// Token: 0x04003975 RID: 14709
		public const short CoralLantern = 5160;

		// Token: 0x04003976 RID: 14710
		public const short CoralPiano = 5161;

		// Token: 0x04003977 RID: 14711
		public const short CoralPlatform = 5162;

		// Token: 0x04003978 RID: 14712
		public const short CoralSink = 5163;

		// Token: 0x04003979 RID: 14713
		public const short CoralSofa = 5164;

		// Token: 0x0400397A RID: 14714
		public const short CoralTable = 5165;

		// Token: 0x0400397B RID: 14715
		public const short CoralWorkbench = 5166;

		// Token: 0x0400397C RID: 14716
		public const short Fake_CoralChest = 5167;

		// Token: 0x0400397D RID: 14717
		public const short CoralToilet = 5168;

		// Token: 0x0400397E RID: 14718
		public const short BalloonBathtub = 5169;

		// Token: 0x0400397F RID: 14719
		public const short BalloonBed = 5170;

		// Token: 0x04003980 RID: 14720
		public const short BalloonBookcase = 5171;

		// Token: 0x04003981 RID: 14721
		public const short BalloonDresser = 5172;

		// Token: 0x04003982 RID: 14722
		public const short BalloonCandelabra = 5173;

		// Token: 0x04003983 RID: 14723
		public const short BalloonCandle = 5174;

		// Token: 0x04003984 RID: 14724
		public const short BalloonChair = 5175;

		// Token: 0x04003985 RID: 14725
		public const short BalloonChandelier = 5176;

		// Token: 0x04003986 RID: 14726
		public const short BalloonChest = 5177;

		// Token: 0x04003987 RID: 14727
		public const short BalloonClock = 5178;

		// Token: 0x04003988 RID: 14728
		public const short BalloonDoor = 5179;

		// Token: 0x04003989 RID: 14729
		public const short BalloonLamp = 5180;

		// Token: 0x0400398A RID: 14730
		public const short BalloonLantern = 5181;

		// Token: 0x0400398B RID: 14731
		public const short BalloonPiano = 5182;

		// Token: 0x0400398C RID: 14732
		public const short BalloonPlatform = 5183;

		// Token: 0x0400398D RID: 14733
		public const short BalloonSink = 5184;

		// Token: 0x0400398E RID: 14734
		public const short BalloonSofa = 5185;

		// Token: 0x0400398F RID: 14735
		public const short BalloonTable = 5186;

		// Token: 0x04003990 RID: 14736
		public const short BalloonWorkbench = 5187;

		// Token: 0x04003991 RID: 14737
		public const short Fake_BalloonChest = 5188;

		// Token: 0x04003992 RID: 14738
		public const short BalloonToilet = 5189;

		// Token: 0x04003993 RID: 14739
		public const short AshWoodBathtub = 5190;

		// Token: 0x04003994 RID: 14740
		public const short AshWoodBed = 5191;

		// Token: 0x04003995 RID: 14741
		public const short AshWoodBookcase = 5192;

		// Token: 0x04003996 RID: 14742
		public const short AshWoodDresser = 5193;

		// Token: 0x04003997 RID: 14743
		public const short AshWoodCandelabra = 5194;

		// Token: 0x04003998 RID: 14744
		public const short AshWoodCandle = 5195;

		// Token: 0x04003999 RID: 14745
		public const short AshWoodChair = 5196;

		// Token: 0x0400399A RID: 14746
		public const short AshWoodChandelier = 5197;

		// Token: 0x0400399B RID: 14747
		public const short AshWoodChest = 5198;

		// Token: 0x0400399C RID: 14748
		public const short AshWoodClock = 5199;

		// Token: 0x0400399D RID: 14749
		public const short AshWoodDoor = 5200;

		// Token: 0x0400399E RID: 14750
		public const short AshWoodLamp = 5201;

		// Token: 0x0400399F RID: 14751
		public const short AshWoodLantern = 5202;

		// Token: 0x040039A0 RID: 14752
		public const short AshWoodPiano = 5203;

		// Token: 0x040039A1 RID: 14753
		public const short AshWoodPlatform = 5204;

		// Token: 0x040039A2 RID: 14754
		public const short AshWoodSink = 5205;

		// Token: 0x040039A3 RID: 14755
		public const short AshWoodSofa = 5206;

		// Token: 0x040039A4 RID: 14756
		public const short AshWoodTable = 5207;

		// Token: 0x040039A5 RID: 14757
		public const short AshWoodWorkbench = 5208;

		// Token: 0x040039A6 RID: 14758
		public const short Fake_AshWoodChest = 5209;

		// Token: 0x040039A7 RID: 14759
		public const short AshWoodToilet = 5210;

		// Token: 0x040039A8 RID: 14760
		public const short BiomeSightPotion = 5211;

		// Token: 0x040039A9 RID: 14761
		public const short ScarletMacaw = 5212;

		// Token: 0x040039AA RID: 14762
		public const short ScarletMacawCage = 5213;

		// Token: 0x040039AB RID: 14763
		public const short AshGrassSeeds = 5214;

		// Token: 0x040039AC RID: 14764
		public const short AshWood = 5215;

		// Token: 0x040039AD RID: 14765
		public const short AshWoodWall = 5216;

		// Token: 0x040039AE RID: 14766
		public const short AshWoodFence = 5217;

		// Token: 0x040039AF RID: 14767
		public const short Outcast = 5218;

		// Token: 0x040039B0 RID: 14768
		public const short FairyGuides = 5219;

		// Token: 0x040039B1 RID: 14769
		public const short AHorribleNightforAlchemy = 5220;

		// Token: 0x040039B2 RID: 14770
		public const short MorningHunt = 5221;

		// Token: 0x040039B3 RID: 14771
		public const short SuspiciouslySparkly = 5222;

		// Token: 0x040039B4 RID: 14772
		public const short Requiem = 5223;

		// Token: 0x040039B5 RID: 14773
		public const short CatSword = 5224;

		// Token: 0x040039B6 RID: 14774
		public const short KargohsSummon = 5225;

		// Token: 0x040039B7 RID: 14775
		public const short HighPitch = 5226;

		// Token: 0x040039B8 RID: 14776
		public const short AMachineforTerrarians = 5227;

		// Token: 0x040039B9 RID: 14777
		public const short TerraBladeChronicles = 5228;

		// Token: 0x040039BA RID: 14778
		public const short BennyWarhol = 5229;

		// Token: 0x040039BB RID: 14779
		public const short LizardKing = 5230;

		// Token: 0x040039BC RID: 14780
		public const short MySon = 5231;

		// Token: 0x040039BD RID: 14781
		public const short Duality = 5232;

		// Token: 0x040039BE RID: 14782
		public const short ParsecPals = 5233;

		// Token: 0x040039BF RID: 14783
		public const short RemnantsofDevotion = 5234;

		// Token: 0x040039C0 RID: 14784
		public const short NotSoLostInParadise = 5235;

		// Token: 0x040039C1 RID: 14785
		public const short OcularResonance = 5236;

		// Token: 0x040039C2 RID: 14786
		public const short WingsofEvil = 5237;

		// Token: 0x040039C3 RID: 14787
		public const short Constellation = 5238;

		// Token: 0x040039C4 RID: 14788
		public const short Eyezorhead = 5239;

		// Token: 0x040039C5 RID: 14789
		public const short DreadoftheRedSea = 5240;

		// Token: 0x040039C6 RID: 14790
		public const short DoNotEattheVileMushroom = 5241;

		// Token: 0x040039C7 RID: 14791
		public const short YuumaTheBlueTiger = 5242;

		// Token: 0x040039C8 RID: 14792
		public const short MoonmanandCompany = 5243;

		// Token: 0x040039C9 RID: 14793
		public const short SunshineofIsrapony = 5244;

		// Token: 0x040039CA RID: 14794
		public const short Purity = 5245;

		// Token: 0x040039CB RID: 14795
		public const short SufficientlyAdvanced = 5246;

		// Token: 0x040039CC RID: 14796
		public const short StrangeGrowth = 5247;

		// Token: 0x040039CD RID: 14797
		public const short HappyLittleTree = 5248;

		// Token: 0x040039CE RID: 14798
		public const short StrangeDeadFellows = 5249;

		// Token: 0x040039CF RID: 14799
		public const short Secrets = 5250;

		// Token: 0x040039D0 RID: 14800
		public const short Thunderbolt = 5251;

		// Token: 0x040039D1 RID: 14801
		public const short Crustography = 5252;

		// Token: 0x040039D2 RID: 14802
		public const short TheWerewolf = 5253;

		// Token: 0x040039D3 RID: 14803
		public const short BlessingfromTheHeavens = 5254;

		// Token: 0x040039D4 RID: 14804
		public const short LoveisintheTrashSlot = 5255;

		// Token: 0x040039D5 RID: 14805
		public const short Fangs = 5256;

		// Token: 0x040039D6 RID: 14806
		public const short HailtotheKing = 5257;

		// Token: 0x040039D7 RID: 14807
		public const short SeeTheWorldForWhatItIs = 5258;

		// Token: 0x040039D8 RID: 14808
		public const short WhatLurksBelow = 5259;

		// Token: 0x040039D9 RID: 14809
		public const short ThisIsGettingOutOfHand = 5260;

		// Token: 0x040039DA RID: 14810
		public const short Buddies = 5261;

		// Token: 0x040039DB RID: 14811
		public const short MidnightSun = 5262;

		// Token: 0x040039DC RID: 14812
		public const short CouchGag = 5263;

		// Token: 0x040039DD RID: 14813
		public const short SilentFish = 5264;

		// Token: 0x040039DE RID: 14814
		public const short TheDuke = 5265;

		// Token: 0x040039DF RID: 14815
		public const short RoyalRomance = 5266;

		// Token: 0x040039E0 RID: 14816
		public const short Bioluminescence = 5267;

		// Token: 0x040039E1 RID: 14817
		public const short Wildflowers = 5268;

		// Token: 0x040039E2 RID: 14818
		public const short VikingVoyage = 5269;

		// Token: 0x040039E3 RID: 14819
		public const short Bifrost = 5270;

		// Token: 0x040039E4 RID: 14820
		public const short Heartlands = 5271;

		// Token: 0x040039E5 RID: 14821
		public const short ForestTroll = 5272;

		// Token: 0x040039E6 RID: 14822
		public const short AuroraBorealis = 5273;

		// Token: 0x040039E7 RID: 14823
		public const short LadyOfTheLake = 5274;

		// Token: 0x040039E8 RID: 14824
		public const short JojaCola = 5275;

		// Token: 0x040039E9 RID: 14825
		public const short JunimoPetItem = 5276;

		// Token: 0x040039EA RID: 14826
		public const short SpicyPepper = 5277;

		// Token: 0x040039EB RID: 14827
		public const short Pomegranate = 5278;

		// Token: 0x040039EC RID: 14828
		public const short AshWoodHelmet = 5279;

		// Token: 0x040039ED RID: 14829
		public const short AshWoodBreastplate = 5280;

		// Token: 0x040039EE RID: 14830
		public const short AshWoodGreaves = 5281;

		// Token: 0x040039EF RID: 14831
		public const short AshWoodBow = 5282;

		// Token: 0x040039F0 RID: 14832
		public const short AshWoodHammer = 5283;

		// Token: 0x040039F1 RID: 14833
		public const short AshWoodSword = 5284;

		// Token: 0x040039F2 RID: 14834
		public const short MoonGlobe = 5285;

		// Token: 0x040039F3 RID: 14835
		public const short RepairedLifeCrystal = 5286;

		// Token: 0x040039F4 RID: 14836
		public const short RepairedManaCrystal = 5287;

		// Token: 0x040039F5 RID: 14837
		public const short TerraFartMinecart = 5288;

		// Token: 0x040039F6 RID: 14838
		public const short MinecartPowerup = 5289;

		// Token: 0x040039F7 RID: 14839
		public const short JimsCap = 5290;

		// Token: 0x040039F8 RID: 14840
		public const short EchoWall = 5291;

		// Token: 0x040039F9 RID: 14841
		public const short EchoPlatform = 5292;

		// Token: 0x040039FA RID: 14842
		public const short MushroomTorch = 5293;

		// Token: 0x040039FB RID: 14843
		public const short HiveFive = 5294;

		// Token: 0x040039FC RID: 14844
		public const short AcornAxe = 5295;

		// Token: 0x040039FD RID: 14845
		public const short ChlorophyteExtractinator = 5296;

		// Token: 0x040039FE RID: 14846
		public const short BlueEgg = 5297;

		// Token: 0x040039FF RID: 14847
		public const short Trimarang = 5298;

		// Token: 0x04003A00 RID: 14848
		public const short MushroomCampfire = 5299;

		// Token: 0x04003A01 RID: 14849
		public const short BlueMacaw = 5300;

		// Token: 0x04003A02 RID: 14850
		public const short BlueMacawCage = 5301;

		// Token: 0x04003A03 RID: 14851
		public const short BottomlessHoneyBucket = 5302;

		// Token: 0x04003A04 RID: 14852
		public const short HoneyAbsorbantSponge = 5303;

		// Token: 0x04003A05 RID: 14853
		public const short UltraAbsorbantSponge = 5304;

		// Token: 0x04003A06 RID: 14854
		public const short GoblorcEar = 5305;

		// Token: 0x04003A07 RID: 14855
		public const short ReefBlock = 5306;

		// Token: 0x04003A08 RID: 14856
		public const short ReefWall = 5307;

		// Token: 0x04003A09 RID: 14857
		public const short PlacePainting = 5308;

		// Token: 0x04003A0A RID: 14858
		public const short DontHurtNatureBook = 5309;

		// Token: 0x04003A0B RID: 14859
		public const short PrincessStyle = 5310;

		// Token: 0x04003A0C RID: 14860
		public const short Toucan = 5311;

		// Token: 0x04003A0D RID: 14861
		public const short YellowCockatiel = 5312;

		// Token: 0x04003A0E RID: 14862
		public const short GrayCockatiel = 5313;

		// Token: 0x04003A0F RID: 14863
		public const short ToucanCage = 5314;

		// Token: 0x04003A10 RID: 14864
		public const short YellowCockatielCage = 5315;

		// Token: 0x04003A11 RID: 14865
		public const short GrayCockatielCage = 5316;

		// Token: 0x04003A12 RID: 14866
		public const short MacawStatue = 5317;

		// Token: 0x04003A13 RID: 14867
		public const short ToucanStatue = 5318;

		// Token: 0x04003A14 RID: 14868
		public const short CockatielStatue = 5319;

		// Token: 0x04003A15 RID: 14869
		public const short PlaceableHealingPotion = 5320;

		// Token: 0x04003A16 RID: 14870
		public const short PlaceableManaPotion = 5321;

		// Token: 0x04003A17 RID: 14871
		public const short ShadowCandle = 5322;

		// Token: 0x04003A18 RID: 14872
		public const short DontHurtComboBook = 5323;

		// Token: 0x04003A19 RID: 14873
		public const short RubblemakerSmall = 5324;

		// Token: 0x04003A1A RID: 14874
		public const short ClosedVoidBag = 5325;

		// Token: 0x04003A1B RID: 14875
		public const short ArtisanLoaf = 5326;

		// Token: 0x04003A1C RID: 14876
		public const short TNTBarrel = 5327;

		// Token: 0x04003A1D RID: 14877
		public const short ChestLock = 5328;

		// Token: 0x04003A1E RID: 14878
		public const short RubblemakerMedium = 5329;

		// Token: 0x04003A1F RID: 14879
		public const short RubblemakerLarge = 5330;

		// Token: 0x04003A20 RID: 14880
		public const short HorseshoeBundle = 5331;

		// Token: 0x04003A21 RID: 14881
		public const short SpiffoPlush = 5332;

		// Token: 0x04003A22 RID: 14882
		public const short GlowTulip = 5333;

		// Token: 0x04003A23 RID: 14883
		public const short MechdusaSummon = 5334;

		// Token: 0x04003A24 RID: 14884
		public const short RodOfHarmony = 5335;

		// Token: 0x04003A25 RID: 14885
		public const short CombatBookVolumeTwo = 5336;

		// Token: 0x04003A26 RID: 14886
		public const short AegisCrystal = 5337;

		// Token: 0x04003A27 RID: 14887
		public const short AegisFruit = 5338;

		// Token: 0x04003A28 RID: 14888
		public const short ArcaneCrystal = 5339;

		// Token: 0x04003A29 RID: 14889
		public const short GalaxyPearl = 5340;

		// Token: 0x04003A2A RID: 14890
		public const short GummyWorm = 5341;

		// Token: 0x04003A2B RID: 14891
		public const short Ambrosia = 5342;

		// Token: 0x04003A2C RID: 14892
		public const short PeddlersSatchel = 5343;

		// Token: 0x04003A2D RID: 14893
		public const short EchoCoating = 5344;

		// Token: 0x04003A2E RID: 14894
		public const short EchoMonolith = 5345;

		// Token: 0x04003A2F RID: 14895
		public const short GasTrap = 5346;

		// Token: 0x04003A30 RID: 14896
		public const short ShimmerMonolith = 5347;

		// Token: 0x04003A31 RID: 14897
		public const short ShimmerArrow = 5348;

		// Token: 0x04003A32 RID: 14898
		public const short ShimmerBlock = 5349;

		// Token: 0x04003A33 RID: 14899
		public const short Shimmerfly = 5350;

		// Token: 0x04003A34 RID: 14900
		public const short ShimmerflyinaBottle = 5351;

		// Token: 0x04003A35 RID: 14901
		public const short ShimmerSlimeBanner = 5352;

		// Token: 0x04003A36 RID: 14902
		public const short ShimmerTorch = 5353;

		// Token: 0x04003A37 RID: 14903
		public const short ReflectiveShades = 5354;

		// Token: 0x04003A38 RID: 14904
		public const short ShimmerCloak = 5355;

		// Token: 0x04003A39 RID: 14905
		public const short UsedGasTrap = 5356;

		// Token: 0x04003A3A RID: 14906
		public const short ShimmerCampfire = 5357;

		// Token: 0x04003A3B RID: 14907
		public const short Shellphone = 5358;

		// Token: 0x04003A3C RID: 14908
		public const short ShellphoneSpawn = 5359;

		// Token: 0x04003A3D RID: 14909
		public const short ShellphoneOcean = 5360;

		// Token: 0x04003A3E RID: 14910
		public const short ShellphoneHell = 5361;

		// Token: 0x04003A3F RID: 14911
		public const short MusicBoxShimmer = 5362;

		// Token: 0x04003A40 RID: 14912
		public const short SpiderWallUnsafe = 5363;

		// Token: 0x04003A41 RID: 14913
		public const short BottomlessShimmerBucket = 5364;

		// Token: 0x04003A42 RID: 14914
		public const short BlueBrickWallUnsafe = 5365;

		// Token: 0x04003A43 RID: 14915
		public const short BlueSlabWallUnsafe = 5366;

		// Token: 0x04003A44 RID: 14916
		public const short BlueTiledWallUnsafe = 5367;

		// Token: 0x04003A45 RID: 14917
		public const short PinkBrickWallUnsafe = 5368;

		// Token: 0x04003A46 RID: 14918
		public const short PinkSlabWallUnsafe = 5369;

		// Token: 0x04003A47 RID: 14919
		public const short PinkTiledWallUnsafe = 5370;

		// Token: 0x04003A48 RID: 14920
		public const short GreenBrickWallUnsafe = 5371;

		// Token: 0x04003A49 RID: 14921
		public const short GreenSlabWallUnsafe = 5372;

		// Token: 0x04003A4A RID: 14922
		public const short GreenTiledWallUnsafe = 5373;

		// Token: 0x04003A4B RID: 14923
		public const short SandstoneWallUnsafe = 5374;

		// Token: 0x04003A4C RID: 14924
		public const short HardenedSandWallUnsafe = 5375;

		// Token: 0x04003A4D RID: 14925
		public const short LihzahrdWallUnsafe = 5376;

		// Token: 0x04003A4E RID: 14926
		public const short SpelunkerFlare = 5377;

		// Token: 0x04003A4F RID: 14927
		public const short CursedFlare = 5378;

		// Token: 0x04003A50 RID: 14928
		public const short RainbowFlare = 5379;

		// Token: 0x04003A51 RID: 14929
		public const short ShimmerFlare = 5380;

		// Token: 0x04003A52 RID: 14930
		public const short Moondial = 5381;

		// Token: 0x04003A53 RID: 14931
		public const short WaffleIron = 5382;

		// Token: 0x04003A54 RID: 14932
		public const short BouncyBoulder = 5383;

		// Token: 0x04003A55 RID: 14933
		public const short LifeCrystalBoulder = 5384;

		// Token: 0x04003A56 RID: 14934
		public const short DizzyHat = 5385;

		// Token: 0x04003A57 RID: 14935
		public const short LincolnsHoodie = 5386;

		// Token: 0x04003A58 RID: 14936
		public const short LincolnsPants = 5387;

		// Token: 0x04003A59 RID: 14937
		public const short SunOrnament = 5388;

		// Token: 0x04003A5A RID: 14938
		public const short HoplitePizza = 5389;

		// Token: 0x04003A5B RID: 14939
		public const short LincolnsHood = 5390;

		// Token: 0x04003A5C RID: 14940
		public const short UncumberingStone = 5391;

		// Token: 0x04003A5D RID: 14941
		public const short SandSolution = 5392;

		// Token: 0x04003A5E RID: 14942
		public const short SnowSolution = 5393;

		// Token: 0x04003A5F RID: 14943
		public const short DirtSolution = 5394;

		// Token: 0x04003A60 RID: 14944
		public const short PoopBlock = 5395;

		// Token: 0x04003A61 RID: 14945
		public const short PoopWall = 5396;

		// Token: 0x04003A62 RID: 14946
		public const short ShimmerWall = 5397;

		// Token: 0x04003A63 RID: 14947
		public const short ShimmerBrick = 5398;

		// Token: 0x04003A64 RID: 14948
		public const short ShimmerBrickWall = 5399;

		// Token: 0x04003A65 RID: 14949
		public const short DirtiestBlock = 5400;

		// Token: 0x04003A66 RID: 14950
		public const short LunarRustBrick = 5401;

		// Token: 0x04003A67 RID: 14951
		public const short DarkCelestialBrick = 5402;

		// Token: 0x04003A68 RID: 14952
		public const short AstraBrick = 5403;

		// Token: 0x04003A69 RID: 14953
		public const short CosmicEmberBrick = 5404;

		// Token: 0x04003A6A RID: 14954
		public const short CryocoreBrick = 5405;

		// Token: 0x04003A6B RID: 14955
		public const short MercuryBrick = 5406;

		// Token: 0x04003A6C RID: 14956
		public const short StarRoyaleBrick = 5407;

		// Token: 0x04003A6D RID: 14957
		public const short HeavenforgeBrick = 5408;

		// Token: 0x04003A6E RID: 14958
		public const short LunarRustBrickWall = 5409;

		// Token: 0x04003A6F RID: 14959
		public const short DarkCelestialBrickWall = 5410;

		// Token: 0x04003A70 RID: 14960
		public const short AstraBrickWall = 5411;

		// Token: 0x04003A71 RID: 14961
		public const short CosmicEmberBrickWall = 5412;

		// Token: 0x04003A72 RID: 14962
		public const short CryocoreBrickWall = 5413;

		// Token: 0x04003A73 RID: 14963
		public const short MercuryBrickWall = 5414;

		// Token: 0x04003A74 RID: 14964
		public const short StarRoyaleBrickWall = 5415;

		// Token: 0x04003A75 RID: 14965
		public const short HeavenforgeBrickWall = 5416;

		// Token: 0x04003A76 RID: 14966
		public const short AncientBlueDungeonBrick = 5417;

		// Token: 0x04003A77 RID: 14967
		public const short AncientBlueDungeonBrickWall = 5418;

		// Token: 0x04003A78 RID: 14968
		public const short AncientGreenDungeonBrick = 5419;

		// Token: 0x04003A79 RID: 14969
		public const short AncientGreenDungeonBrickWall = 5420;

		// Token: 0x04003A7A RID: 14970
		public const short AncientPinkDungeonBrick = 5421;

		// Token: 0x04003A7B RID: 14971
		public const short AncientPinkDungeonBrickWall = 5422;

		// Token: 0x04003A7C RID: 14972
		public const short AncientGoldBrick = 5423;

		// Token: 0x04003A7D RID: 14973
		public const short AncientGoldBrickWall = 5424;

		// Token: 0x04003A7E RID: 14974
		public const short AncientSilverBrick = 5425;

		// Token: 0x04003A7F RID: 14975
		public const short AncientSilverBrickWall = 5426;

		// Token: 0x04003A80 RID: 14976
		public const short AncientCopperBrick = 5427;

		// Token: 0x04003A81 RID: 14977
		public const short AncientCopperBrickWall = 5428;

		// Token: 0x04003A82 RID: 14978
		public const short AncientCobaltBrick = 5429;

		// Token: 0x04003A83 RID: 14979
		public const short AncientCobaltBrickWall = 5430;

		// Token: 0x04003A84 RID: 14980
		public const short AncientMythrilBrick = 5431;

		// Token: 0x04003A85 RID: 14981
		public const short AncientMythrilBrickWall = 5432;

		// Token: 0x04003A86 RID: 14982
		public const short AncientObsidianBrick = 5433;

		// Token: 0x04003A87 RID: 14983
		public const short AncientObsidianBrickWall = 5434;

		// Token: 0x04003A88 RID: 14984
		public const short AncientHellstoneBrick = 5435;

		// Token: 0x04003A89 RID: 14985
		public const short AncientHellstoneBrickWall = 5436;

		// Token: 0x04003A8A RID: 14986
		public const short ShellphoneDummy = 5437;

		// Token: 0x04003A8B RID: 14987
		public const short Fertilizer = 5438;

		// Token: 0x04003A8C RID: 14988
		public const short LavaMossBlock = 5439;

		// Token: 0x04003A8D RID: 14989
		public const short ArgonMossBlock = 5440;

		// Token: 0x04003A8E RID: 14990
		public const short KryptonMossBlock = 5441;

		// Token: 0x04003A8F RID: 14991
		public const short XenonMossBlock = 5442;

		// Token: 0x04003A90 RID: 14992
		public const short VioletMossBlock = 5443;

		// Token: 0x04003A91 RID: 14993
		public const short RainbowMossBlock = 5444;

		// Token: 0x04003A92 RID: 14994
		public const short LavaMossBlockWall = 5445;

		// Token: 0x04003A93 RID: 14995
		public const short ArgonMossBlockWall = 5446;

		// Token: 0x04003A94 RID: 14996
		public const short KryptonMossBlockWall = 5447;

		// Token: 0x04003A95 RID: 14997
		public const short XenonMossBlockWall = 5448;

		// Token: 0x04003A96 RID: 14998
		public const short VioletMossBlockWall = 5449;

		// Token: 0x04003A97 RID: 14999
		public const short RainbowMossBlockWall = 5450;

		// Token: 0x04003A98 RID: 15000
		public const short JimsDrone = 5451;

		// Token: 0x04003A99 RID: 15001
		public const short JimsDroneVisor = 5452;

		// Token: 0x04003A9A RID: 15002
		public const short DontHurtCrittersBookInactive = 5453;

		// Token: 0x04003A9B RID: 15003
		public const short DontHurtNatureBookInactive = 5454;

		// Token: 0x04003A9C RID: 15004
		public const short DontHurtComboBookInactive = 5455;

		// Token: 0x04003A9D RID: 15005
		public const short DeadCellsMushroomBoiSummonItem = 5456;

		// Token: 0x04003A9E RID: 15006
		public const short DeadCellsBeheadedHead = 5457;

		// Token: 0x04003A9F RID: 15007
		public const short DeadCellsBeheadedBody = 5458;

		// Token: 0x04003AA0 RID: 15008
		public const short DeadCellsBeheadedLegs = 5459;

		// Token: 0x04003AA1 RID: 15009
		public const short DeadCellsBarrelLauncher = 5460;

		// Token: 0x04003AA2 RID: 15010
		public const short DeadCellsKillingDeck = 5461;

		// Token: 0x04003AA3 RID: 15011
		public const short DeadCellsFlint = 5462;

		// Token: 0x04003AA4 RID: 15012
		public const short DeadCellsBarnacleSummonItem = 5463;

		// Token: 0x04003AA5 RID: 15013
		public const short MiteyTitey = 5464;

		// Token: 0x04003AA6 RID: 15014
		public const short DeadCellsRamRune = 5465;

		// Token: 0x04003AA7 RID: 15015
		public const short DeadCellsSwarmGrenade = 5466;

		// Token: 0x04003AA8 RID: 15016
		public const short DemonAltarReplica = 5467;

		// Token: 0x04003AA9 RID: 15017
		public const short CrimsonAltarReplica = 5468;

		// Token: 0x04003AAA RID: 15018
		public const short ShadowOrbReplica = 5469;

		// Token: 0x04003AAB RID: 15019
		public const short CrimsonHeartReplica = 5470;

		// Token: 0x04003AAC RID: 15020
		public const short CobwebReplica = 5471;

		// Token: 0x04003AAD RID: 15021
		public const short DeadCellsDisplayJar = 5472;

		// Token: 0x04003AAE RID: 15022
		public const short CobWhip = 5473;

		// Token: 0x04003AAF RID: 15023
		public const short CorruptWhip = 5474;

		// Token: 0x04003AB0 RID: 15024
		public const short CrimsonWhip = 5475;

		// Token: 0x04003AB1 RID: 15025
		public const short MeteorWhip = 5476;

		// Token: 0x04003AB2 RID: 15026
		public const short FlowerWhip = 5477;

		// Token: 0x04003AB3 RID: 15027
		public const short EelWhip = 5478;

		// Token: 0x04003AB4 RID: 15028
		public const short ConstellationWhip = 5479;

		// Token: 0x04003AB5 RID: 15029
		public const short MoonLordWhip = 5480;

		// Token: 0x04003AB6 RID: 15030
		public const short PortableKiln = 5481;

		// Token: 0x04003AB7 RID: 15031
		public const short DeadCellsPotionStation = 5482;

		// Token: 0x04003AB8 RID: 15032
		public const short QueenOfBees = 5483;

		// Token: 0x04003AB9 RID: 15033
		public const short PlayerVoiceCowbellItem = 5484;

		// Token: 0x04003ABA RID: 15034
		public const short PlayerVoiceChickenFeetItem = 5485;

		// Token: 0x04003ABB RID: 15035
		public const short TheSeaOfSilence = 5486;

		// Token: 0x04003ABC RID: 15036
		public const short HeroesFromAnotherWorld = 5487;

		// Token: 0x04003ABD RID: 15037
		public const short Crystallize = 5488;

		// Token: 0x04003ABE RID: 15038
		public const short EaterOfLife = 5489;

		// Token: 0x04003ABF RID: 15039
		public const short ThisIsCanonNow = 5490;

		// Token: 0x04003AC0 RID: 15040
		public const short WinterAtVaringskollen = 5491;

		// Token: 0x04003AC1 RID: 15041
		public const short MagicShimmerDropper = 5492;

		// Token: 0x04003AC2 RID: 15042
		public const short ShimmerFallBlock = 5493;

		// Token: 0x04003AC3 RID: 15043
		public const short ShimmerFallWall = 5494;

		// Token: 0x04003AC4 RID: 15044
		public const short ShimmerGun = 5495;

		// Token: 0x04003AC5 RID: 15045
		public const short LifeFruitHealingPotion = 5496;

		// Token: 0x04003AC6 RID: 15046
		public const short PinkBanner = 5497;

		// Token: 0x04003AC7 RID: 15047
		public const short WhiteBanner = 5498;

		// Token: 0x04003AC8 RID: 15048
		public const short PlayerVoiceFrogItem = 5499;

		// Token: 0x04003AC9 RID: 15049
		public const short PlayerVoiceGoatItem = 5500;

		// Token: 0x04003ACA RID: 15050
		public const short PlayerVoiceRetroItem = 5501;

		// Token: 0x04003ACB RID: 15051
		public const short PlayerVoiceCatItem = 5502;

		// Token: 0x04003ACC RID: 15052
		public const short PlayerVoiceDogItem = 5503;

		// Token: 0x04003ACD RID: 15053
		public const short PlayerVoiceTurkeyItem = 5504;

		// Token: 0x04003ACE RID: 15054
		public const short PlayerVoiceGoblinItem = 5505;

		// Token: 0x04003ACF RID: 15055
		public const short PlayerVoiceCrowItem = 5506;

		// Token: 0x04003AD0 RID: 15056
		public const short PlayerVoiceBalloonItem = 5507;

		// Token: 0x04003AD1 RID: 15057
		public const short PlayerVoiceUndeadItem = 5508;

		// Token: 0x04003AD2 RID: 15058
		public const short PlayerVoiceVampireItem = 5509;

		// Token: 0x04003AD3 RID: 15059
		public const short VelociraptorMountItem = 5510;

		// Token: 0x04003AD4 RID: 15060
		public const short Pufferfish = 5511;

		// Token: 0x04003AD5 RID: 15061
		public const short PufferfishCage = 5512;

		// Token: 0x04003AD6 RID: 15062
		public const short PufferfishPet = 5513;

		// Token: 0x04003AD7 RID: 15063
		public const short RainbowBoulder = 5514;

		// Token: 0x04003AD8 RID: 15064
		public const short MoonLordBody = 5515;

		// Token: 0x04003AD9 RID: 15065
		public const short Poulder = 5516;

		// Token: 0x04003ADA RID: 15066
		public const short AxeFairyPetItem = 5517;

		// Token: 0x04003ADB RID: 15067
		public const short SuperFertilizer = 5518;

		// Token: 0x04003ADC RID: 15068
		public const short Axearang = 5519;

		// Token: 0x04003ADD RID: 15069
		public const short LavaBoulder = 5520;

		// Token: 0x04003ADE RID: 15070
		public const short SpiderBoulder = 5521;

		// Token: 0x04003ADF RID: 15071
		public const short Ghoulder = 5522;

		// Token: 0x04003AE0 RID: 15072
		public const short BoulderPet = 5523;

		// Token: 0x04003AE1 RID: 15073
		public const short ChlorophyteVisor = 5524;

		// Token: 0x04003AE2 RID: 15074
		public const short RatMountItem = 5525;

		// Token: 0x04003AE3 RID: 15075
		public const short FlaironFlail = 5526;

		// Token: 0x04003AE4 RID: 15076
		public const short OfSeaAndDreams = 5527;

		// Token: 0x04003AE5 RID: 15077
		public const short TheRunicPixie = 5528;

		// Token: 0x04003AE6 RID: 15078
		public const short BannerOfTheBeast = 5529;

		// Token: 0x04003AE7 RID: 15079
		public const short StickmanVsTerrTerr = 5530;

		// Token: 0x04003AE8 RID: 15080
		public const short CozyWindow = 5531;

		// Token: 0x04003AE9 RID: 15081
		public const short DemonAltar = 5532;

		// Token: 0x04003AEA RID: 15082
		public const short CrimsonAltar = 5533;

		// Token: 0x04003AEB RID: 15083
		public const short PlayerVoiceFairyItem = 5534;

		// Token: 0x04003AEC RID: 15084
		public const short PinkPhaseblade = 5535;

		// Token: 0x04003AED RID: 15085
		public const short PinkPhasesaber = 5536;

		// Token: 0x04003AEE RID: 15086
		public const short BlackenedFish = 5537;

		// Token: 0x04003AEF RID: 15087
		public const short MusicBoxQueenBee = 5538;

		// Token: 0x04003AF0 RID: 15088
		public const short MusicBoxTwins = 5539;

		// Token: 0x04003AF1 RID: 15089
		public const short MagicString = 5540;

		// Token: 0x04003AF2 RID: 15090
		public const short MagicYoyoBag = 5541;

		// Token: 0x04003AF3 RID: 15091
		public const short FreezeBomb = 5542;

		// Token: 0x04003AF4 RID: 15092
		public const short StressBall = 5543;

		// Token: 0x04003AF5 RID: 15093
		public const short CloudPlatform = 5544;

		// Token: 0x04003AF6 RID: 15094
		public const short LivingWoodWallUnsafe = 5545;

		// Token: 0x04003AF7 RID: 15095
		public const short DirtWallUnsafe = 5546;

		// Token: 0x04003AF8 RID: 15096
		public const short StrungCounterweight = 5547;

		// Token: 0x04003AF9 RID: 15097
		public const short AetheriumBathtub = 5548;

		// Token: 0x04003AFA RID: 15098
		public const short AetheriumBed = 5549;

		// Token: 0x04003AFB RID: 15099
		public const short AetheriumBookcase = 5550;

		// Token: 0x04003AFC RID: 15100
		public const short AetheriumDresser = 5551;

		// Token: 0x04003AFD RID: 15101
		public const short AetheriumCandelabra = 5552;

		// Token: 0x04003AFE RID: 15102
		public const short AetheriumCandle = 5553;

		// Token: 0x04003AFF RID: 15103
		public const short AetheriumChair = 5554;

		// Token: 0x04003B00 RID: 15104
		public const short AetheriumChandelier = 5555;

		// Token: 0x04003B01 RID: 15105
		public const short AetheriumChest = 5556;

		// Token: 0x04003B02 RID: 15106
		public const short AetheriumClock = 5557;

		// Token: 0x04003B03 RID: 15107
		public const short AetheriumDoor = 5558;

		// Token: 0x04003B04 RID: 15108
		public const short AetheriumLamp = 5559;

		// Token: 0x04003B05 RID: 15109
		public const short AetheriumLantern = 5560;

		// Token: 0x04003B06 RID: 15110
		public const short AetheriumPiano = 5561;

		// Token: 0x04003B07 RID: 15111
		public const short AetheriumPlatform = 5562;

		// Token: 0x04003B08 RID: 15112
		public const short AetheriumSink = 5563;

		// Token: 0x04003B09 RID: 15113
		public const short AetheriumSofa = 5564;

		// Token: 0x04003B0A RID: 15114
		public const short AetheriumTable = 5565;

		// Token: 0x04003B0B RID: 15115
		public const short AetheriumWorkbench = 5566;

		// Token: 0x04003B0C RID: 15116
		public const short Fake_AetheriumChest = 5567;

		// Token: 0x04003B0D RID: 15117
		public const short AetheriumToilet = 5568;

		// Token: 0x04003B0E RID: 15118
		public const short LavaCloud = 5569;

		// Token: 0x04003B0F RID: 15119
		public const short StarCloud = 5570;

		// Token: 0x04003B10 RID: 15120
		public const short RainbowCloud = 5571;

		// Token: 0x04003B11 RID: 15121
		public const short MudBallPlayer = 5572;

		// Token: 0x04003B12 RID: 15122
		public const short TorchGodPotion = 5573;

		// Token: 0x04003B13 RID: 15123
		public const short LuckyClover = 5574;

		// Token: 0x04003B14 RID: 15124
		public const short WiltedClover = 5575;

		// Token: 0x04003B15 RID: 15125
		public const short RavenFeather = 5576;

		// Token: 0x04003B16 RID: 15126
		public const short PrettyMirror = 5577;

		// Token: 0x04003B17 RID: 15127
		public const short MusicBoxKingSlime = 5578;

		// Token: 0x04003B18 RID: 15128
		public const short MusicBoxQueenBeeAlt = 5579;

		// Token: 0x04003B19 RID: 15129
		public const short MusicBoxLunaticCultist = 5580;

		// Token: 0x04003B1A RID: 15130
		public const short MusicBoxSkeletronPrime = 5581;

		// Token: 0x04003B1B RID: 15131
		public const short MusicBoxDestroyer = 5582;

		// Token: 0x04003B1C RID: 15132
		public const short ChickenBonesHead = 5583;

		// Token: 0x04003B1D RID: 15133
		public const short ChickenBonesBody = 5584;

		// Token: 0x04003B1E RID: 15134
		public const short ChickenBonesLegs = 5585;

		// Token: 0x04003B1F RID: 15135
		public const short ChickenBonesWings = 5586;

		// Token: 0x04003B20 RID: 15136
		public const short ChickenBonesRobe = 5587;

		// Token: 0x04003B21 RID: 15137
		public const short UpgradedMiningHead = 5588;

		// Token: 0x04003B22 RID: 15138
		public const short UpgradedMiningBody = 5589;

		// Token: 0x04003B23 RID: 15139
		public const short UpgradedMiningLegs = 5590;

		// Token: 0x04003B24 RID: 15140
		public const short UpgradedFishingHead = 5591;

		// Token: 0x04003B25 RID: 15141
		public const short UpgradedFishingBody = 5592;

		// Token: 0x04003B26 RID: 15142
		public const short UpgradedFishingLegs = 5593;

		// Token: 0x04003B27 RID: 15143
		public const short SuperBomb = 5594;

		// Token: 0x04003B28 RID: 15144
		public const short SuperStickyBomb = 5595;

		// Token: 0x04003B29 RID: 15145
		public const short WeldingMask = 5596;

		// Token: 0x04003B2A RID: 15146
		public const short BatMountItem = 5597;

		// Token: 0x04003B2B RID: 15147
		public const short CRTMonolith = 5598;

		// Token: 0x04003B2C RID: 15148
		public const short RetroMonolith = 5599;

		// Token: 0x04003B2D RID: 15149
		public const short RollerSkatesBlueMountItem = 5600;

		// Token: 0x04003B2E RID: 15150
		public const short FallenStarBathtub = 5601;

		// Token: 0x04003B2F RID: 15151
		public const short FallenStarBed = 5602;

		// Token: 0x04003B30 RID: 15152
		public const short FallenStarBookcase = 5603;

		// Token: 0x04003B31 RID: 15153
		public const short FallenStarDresser = 5604;

		// Token: 0x04003B32 RID: 15154
		public const short FallenStarCandelabra = 5605;

		// Token: 0x04003B33 RID: 15155
		public const short FallenStarCandle = 5606;

		// Token: 0x04003B34 RID: 15156
		public const short FallenStarChair = 5607;

		// Token: 0x04003B35 RID: 15157
		public const short FallenStarChandelier = 5608;

		// Token: 0x04003B36 RID: 15158
		public const short FallenStarChest = 5609;

		// Token: 0x04003B37 RID: 15159
		public const short FallenStarClock = 5610;

		// Token: 0x04003B38 RID: 15160
		public const short FallenStarDoor = 5611;

		// Token: 0x04003B39 RID: 15161
		public const short FallenStarLamp = 5612;

		// Token: 0x04003B3A RID: 15162
		public const short FallenStarLantern = 5613;

		// Token: 0x04003B3B RID: 15163
		public const short FallenStarPiano = 5614;

		// Token: 0x04003B3C RID: 15164
		public const short FallenStarPlatform = 5615;

		// Token: 0x04003B3D RID: 15165
		public const short FallenStarSink = 5616;

		// Token: 0x04003B3E RID: 15166
		public const short FallenStarSofa = 5617;

		// Token: 0x04003B3F RID: 15167
		public const short FallenStarTable = 5618;

		// Token: 0x04003B40 RID: 15168
		public const short FallenStarWorkbench = 5619;

		// Token: 0x04003B41 RID: 15169
		public const short Fake_FallenStarChest = 5620;

		// Token: 0x04003B42 RID: 15170
		public const short FallenStarToilet = 5621;

		// Token: 0x04003B43 RID: 15171
		public const short FallenStarBlock = 5622;

		// Token: 0x04003B44 RID: 15172
		public const short FallenStarWall = 5623;

		// Token: 0x04003B45 RID: 15173
		public const short ChippysHead = 5624;

		// Token: 0x04003B46 RID: 15174
		public const short ChippysBody = 5625;

		// Token: 0x04003B47 RID: 15175
		public const short ChippysLegs = 5626;

		// Token: 0x04003B48 RID: 15176
		public const short ChippysWings = 5627;

		// Token: 0x04003B49 RID: 15177
		public const short ChippysHeadband = 5628;

		// Token: 0x04003B4A RID: 15178
		public const short AcornSlingshot = 5629;

		// Token: 0x04003B4B RID: 15179
		public const short PaintingRPlace2023 = 5630;

		// Token: 0x04003B4C RID: 15180
		public const short PaintingBouldChoices = 5631;

		// Token: 0x04003B4D RID: 15181
		public const short PaintingDarkForebodings = 5632;

		// Token: 0x04003B4E RID: 15182
		public const short PaintingGermanZenith = 5633;

		// Token: 0x04003B4F RID: 15183
		public const short PaintingItsScragglinTime = 5634;

		// Token: 0x04003B50 RID: 15184
		public const short PaintingKaguya = 5635;

		// Token: 0x04003B51 RID: 15185
		public const short PaintingGermanBeer = 5636;

		// Token: 0x04003B52 RID: 15186
		public const short MusicBoxEaterOfWorlds = 5637;

		// Token: 0x04003B53 RID: 15187
		public const short MusicBoxTorchGod = 5638;

		// Token: 0x04003B54 RID: 15188
		public const short MusicBoxTorchGodAlt = 5639;

		// Token: 0x04003B55 RID: 15189
		public const short RollerSkatesGreenMountItem = 5640;

		// Token: 0x04003B56 RID: 15190
		public const short RollerSkatesClassicMountItem = 5641;

		// Token: 0x04003B57 RID: 15191
		public const short RollerSkatesPartyMountItem = 5642;

		// Token: 0x04003B58 RID: 15192
		public const short RainbowGlowstick = 5643;

		// Token: 0x04003B59 RID: 15193
		public const short ScryingOrb = 5644;

		// Token: 0x04003B5A RID: 15194
		public const short RockCandy = 5645;

		// Token: 0x04003B5B RID: 15195
		public const short BlueBikiniBody = 5646;

		// Token: 0x04003B5C RID: 15196
		public const short BlueBikiniLegs = 5647;

		// Token: 0x04003B5D RID: 15197
		public const short RedSwimsuit = 5648;

		// Token: 0x04003B5E RID: 15198
		public const short GreenSwimshorts = 5649;

		// Token: 0x04003B5F RID: 15199
		public const short GraySwimshorts = 5650;

		// Token: 0x04003B60 RID: 15200
		public const short OrcaBanner = 5651;

		// Token: 0x04003B61 RID: 15201
		public const short TeleportationPylonUnderworld = 5652;

		// Token: 0x04003B62 RID: 15202
		public const short TeleportationPylonShimmer = 5653;

		// Token: 0x04003B63 RID: 15203
		public const short RainbowBoulderPet = 5654;

		// Token: 0x04003B64 RID: 15204
		public const short NoirMonolith = 5655;

		// Token: 0x04003B65 RID: 15205
		public const short HeroicisHead = 5656;

		// Token: 0x04003B66 RID: 15206
		public const short HeroicisBody = 5657;

		// Token: 0x04003B67 RID: 15207
		public const short HeroicisLegs = 5658;

		// Token: 0x04003B68 RID: 15208
		public const short HeroicisWings = 5659;

		// Token: 0x04003B69 RID: 15209
		public const short HallowedCrown = 5660;

		// Token: 0x04003B6A RID: 15210
		public const short HeroicisWingsInactive = 5661;

		// Token: 0x04003B6B RID: 15211
		public const short EnchantedPixieDust = 5662;

		// Token: 0x04003B6C RID: 15212
		public const short PalworldMinionCattiva = 5663;

		// Token: 0x04003B6D RID: 15213
		public const short PalworldMinionFoxsparks = 5664;

		// Token: 0x04003B6E RID: 15214
		public const short PalworldPetChillet = 5665;

		// Token: 0x04003B6F RID: 15215
		public const short PalworldPetChilletIgnis = 5666;

		// Token: 0x04003B70 RID: 15216
		public const short PalworldDigtoise = 5667;

		// Token: 0x04003B71 RID: 15217
		public const short SoundGun = 5668;

		// Token: 0x04003B72 RID: 15218
		public const short TrueCopperShortsword = 5669;

		// Token: 0x04003B73 RID: 15219
		public const short RainbowPhaseblade = 5670;

		// Token: 0x04003B74 RID: 15220
		public const short RainbowPhasesaber = 5671;

		// Token: 0x04003B75 RID: 15221
		public const short LibrarianSkeletonBanner = 5672;

		// Token: 0x04003B76 RID: 15222
		public const short WaterBoltMimicBanner = 5673;

		// Token: 0x04003B77 RID: 15223
		public const short TeamBlockRedVariant = 5674;

		// Token: 0x04003B78 RID: 15224
		public const short TeamBlockGreenVariant = 5675;

		// Token: 0x04003B79 RID: 15225
		public const short TeamBlockBlueVariant = 5676;

		// Token: 0x04003B7A RID: 15226
		public const short TeamBlockYellowVariant = 5677;

		// Token: 0x04003B7B RID: 15227
		public const short TeamBlockPinkVariant = 5678;

		// Token: 0x04003B7C RID: 15228
		public const short TeamBlockWhiteVariant = 5679;

		// Token: 0x04003B7D RID: 15229
		public const short LilacDuskHead = 5680;

		// Token: 0x04003B7E RID: 15230
		public const short LilacDuskBody = 5681;

		// Token: 0x04003B7F RID: 15231
		public const short LilacDuskLegs = 5682;

		// Token: 0x04003B80 RID: 15232
		public const short KazzymodusHood = 5683;

		// Token: 0x04003B81 RID: 15233
		public const short KazzymodusChestpiece = 5684;

		// Token: 0x04003B82 RID: 15234
		public const short KazzymodusLeggings = 5685;

		// Token: 0x04003B83 RID: 15235
		public const short KazzymodusWings = 5686;

		// Token: 0x04003B84 RID: 15236
		public const short SlimeSpear = 5687;

		// Token: 0x04003B85 RID: 15237
		public const short SlimeWhip = 5688;

		// Token: 0x04003B86 RID: 15238
		public const short FeywoodBathtub = 5689;

		// Token: 0x04003B87 RID: 15239
		public const short FeywoodBed = 5690;

		// Token: 0x04003B88 RID: 15240
		public const short FeywoodBookcase = 5691;

		// Token: 0x04003B89 RID: 15241
		public const short FeywoodDresser = 5692;

		// Token: 0x04003B8A RID: 15242
		public const short FeywoodCandelabra = 5693;

		// Token: 0x04003B8B RID: 15243
		public const short FeywoodCandle = 5694;

		// Token: 0x04003B8C RID: 15244
		public const short FeywoodChair = 5695;

		// Token: 0x04003B8D RID: 15245
		public const short FeywoodChandelier = 5696;

		// Token: 0x04003B8E RID: 15246
		public const short FeywoodChest = 5697;

		// Token: 0x04003B8F RID: 15247
		public const short FeywoodClock = 5698;

		// Token: 0x04003B90 RID: 15248
		public const short FeywoodDoor = 5699;

		// Token: 0x04003B91 RID: 15249
		public const short FeywoodLamp = 5700;

		// Token: 0x04003B92 RID: 15250
		public const short FeywoodLantern = 5701;

		// Token: 0x04003B93 RID: 15251
		public const short FeywoodPiano = 5702;

		// Token: 0x04003B94 RID: 15252
		public const short FeywoodPlatform = 5703;

		// Token: 0x04003B95 RID: 15253
		public const short FeywoodSink = 5704;

		// Token: 0x04003B96 RID: 15254
		public const short FeywoodSofa = 5705;

		// Token: 0x04003B97 RID: 15255
		public const short FeywoodTable = 5706;

		// Token: 0x04003B98 RID: 15256
		public const short FeywoodWorkbench = 5707;

		// Token: 0x04003B99 RID: 15257
		public const short Fake_FeywoodChest = 5708;

		// Token: 0x04003B9A RID: 15258
		public const short FeywoodToilet = 5709;

		// Token: 0x04003B9B RID: 15259
		public const short Feywood = 5710;

		// Token: 0x04003B9C RID: 15260
		public const short FeywoodWall = 5711;

		// Token: 0x04003B9D RID: 15261
		public const short HallowedBathtub = 5712;

		// Token: 0x04003B9E RID: 15262
		public const short HallowedBed = 5713;

		// Token: 0x04003B9F RID: 15263
		public const short HallowedBookcase = 5714;

		// Token: 0x04003BA0 RID: 15264
		public const short HallowedDresser = 5715;

		// Token: 0x04003BA1 RID: 15265
		public const short HallowedCandelabra = 5716;

		// Token: 0x04003BA2 RID: 15266
		public const short HallowedCandle = 5717;

		// Token: 0x04003BA3 RID: 15267
		public const short HallowedChair = 5718;

		// Token: 0x04003BA4 RID: 15268
		public const short HallowedChandelier = 5719;

		// Token: 0x04003BA5 RID: 15269
		public const short HallowedFurnitureChest = 5720;

		// Token: 0x04003BA6 RID: 15270
		public const short HallowedClock = 5721;

		// Token: 0x04003BA7 RID: 15271
		public const short HallowedDoor = 5722;

		// Token: 0x04003BA8 RID: 15272
		public const short HallowedLamp = 5723;

		// Token: 0x04003BA9 RID: 15273
		public const short HallowedLantern = 5724;

		// Token: 0x04003BAA RID: 15274
		public const short HallowedPiano = 5725;

		// Token: 0x04003BAB RID: 15275
		public const short HallowedPlatform = 5726;

		// Token: 0x04003BAC RID: 15276
		public const short HallowedSink = 5727;

		// Token: 0x04003BAD RID: 15277
		public const short HallowedSofa = 5728;

		// Token: 0x04003BAE RID: 15278
		public const short HallowedTable = 5729;

		// Token: 0x04003BAF RID: 15279
		public const short HallowedWorkbench = 5730;

		// Token: 0x04003BB0 RID: 15280
		public const short Fake_HallowedFurnitureChest = 5731;

		// Token: 0x04003BB1 RID: 15281
		public const short HallowedToilet = 5732;

		// Token: 0x04003BB2 RID: 15282
		public const short HallowedBrick = 5733;

		// Token: 0x04003BB3 RID: 15283
		public const short HallowedBrickWall = 5734;

		// Token: 0x04003BB4 RID: 15284
		public const short PalworldPalMetalArmorBody = 5735;

		// Token: 0x04003BB5 RID: 15285
		public const short PalworldPalMetalArmorLegs = 5736;

		// Token: 0x04003BB6 RID: 15286
		public const short ChippysWingsInactive = 5737;

		// Token: 0x04003BB7 RID: 15287
		public const short RemoteControlCar = 5738;

		// Token: 0x04003BB8 RID: 15288
		public const short GothicBathtub = 5739;

		// Token: 0x04003BB9 RID: 15289
		public const short GothicBed = 5740;

		// Token: 0x04003BBA RID: 15290
		public const short GothicDresser = 5741;

		// Token: 0x04003BBB RID: 15291
		public const short GothicCandelabra = 5742;

		// Token: 0x04003BBC RID: 15292
		public const short GothicCandle = 5743;

		// Token: 0x04003BBD RID: 15293
		public const short GothicChandelier = 5744;

		// Token: 0x04003BBE RID: 15294
		public const short GothicChest = 5745;

		// Token: 0x04003BBF RID: 15295
		public const short GothicClock = 5746;

		// Token: 0x04003BC0 RID: 15296
		public const short GothicDoor = 5747;

		// Token: 0x04003BC1 RID: 15297
		public const short GothicLamp = 5748;

		// Token: 0x04003BC2 RID: 15298
		public const short GothicLantern = 5749;

		// Token: 0x04003BC3 RID: 15299
		public const short GothicPiano = 5750;

		// Token: 0x04003BC4 RID: 15300
		public const short GothicPlatform = 5751;

		// Token: 0x04003BC5 RID: 15301
		public const short GothicSink = 5752;

		// Token: 0x04003BC6 RID: 15302
		public const short GothicSofa = 5753;

		// Token: 0x04003BC7 RID: 15303
		public const short Fake_GothicChest = 5754;

		// Token: 0x04003BC8 RID: 15304
		public const short GothicToilet = 5755;

		// Token: 0x04003BC9 RID: 15305
		public const short DemoniteBathtub = 5756;

		// Token: 0x04003BCA RID: 15306
		public const short DemoniteBed = 5757;

		// Token: 0x04003BCB RID: 15307
		public const short DemoniteBookcase = 5758;

		// Token: 0x04003BCC RID: 15308
		public const short DemoniteCandelabra = 5759;

		// Token: 0x04003BCD RID: 15309
		public const short DemoniteCandle = 5760;

		// Token: 0x04003BCE RID: 15310
		public const short DemoniteChair = 5761;

		// Token: 0x04003BCF RID: 15311
		public const short DemoniteChandelier = 5762;

		// Token: 0x04003BD0 RID: 15312
		public const short DemoniteChest = 5763;

		// Token: 0x04003BD1 RID: 15313
		public const short DemoniteClock = 5764;

		// Token: 0x04003BD2 RID: 15314
		public const short DemoniteDoor = 5765;

		// Token: 0x04003BD3 RID: 15315
		public const short DemoniteDresser = 5766;

		// Token: 0x04003BD4 RID: 15316
		public const short DemoniteLamp = 5767;

		// Token: 0x04003BD5 RID: 15317
		public const short DemoniteLantern = 5768;

		// Token: 0x04003BD6 RID: 15318
		public const short DemonitePiano = 5769;

		// Token: 0x04003BD7 RID: 15319
		public const short DemonitePlatform = 5770;

		// Token: 0x04003BD8 RID: 15320
		public const short DemoniteSink = 5771;

		// Token: 0x04003BD9 RID: 15321
		public const short DemoniteSofa = 5772;

		// Token: 0x04003BDA RID: 15322
		public const short DemoniteTable = 5773;

		// Token: 0x04003BDB RID: 15323
		public const short DemoniteToilet = 5774;

		// Token: 0x04003BDC RID: 15324
		public const short DemoniteWorkbench = 5775;

		// Token: 0x04003BDD RID: 15325
		public const short Fake_DemoniteChest = 5776;

		// Token: 0x04003BDE RID: 15326
		public const short CrimtaneBathtub = 5777;

		// Token: 0x04003BDF RID: 15327
		public const short CrimtaneBed = 5778;

		// Token: 0x04003BE0 RID: 15328
		public const short CrimtaneBookcase = 5779;

		// Token: 0x04003BE1 RID: 15329
		public const short CrimtaneCandelabra = 5780;

		// Token: 0x04003BE2 RID: 15330
		public const short CrimtaneCandle = 5781;

		// Token: 0x04003BE3 RID: 15331
		public const short CrimtaneChair = 5782;

		// Token: 0x04003BE4 RID: 15332
		public const short CrimtaneChandelier = 5783;

		// Token: 0x04003BE5 RID: 15333
		public const short CrimtaneChest = 5784;

		// Token: 0x04003BE6 RID: 15334
		public const short CrimtaneClock = 5785;

		// Token: 0x04003BE7 RID: 15335
		public const short CrimtaneDoor = 5786;

		// Token: 0x04003BE8 RID: 15336
		public const short CrimtaneDresser = 5787;

		// Token: 0x04003BE9 RID: 15337
		public const short CrimtaneLamp = 5788;

		// Token: 0x04003BEA RID: 15338
		public const short CrimtaneLantern = 5789;

		// Token: 0x04003BEB RID: 15339
		public const short CrimtanePiano = 5790;

		// Token: 0x04003BEC RID: 15340
		public const short CrimtanePlatform = 5791;

		// Token: 0x04003BED RID: 15341
		public const short CrimtaneSink = 5792;

		// Token: 0x04003BEE RID: 15342
		public const short CrimtaneSofa = 5793;

		// Token: 0x04003BEF RID: 15343
		public const short CrimtaneTable = 5794;

		// Token: 0x04003BF0 RID: 15344
		public const short CrimtaneToilet = 5795;

		// Token: 0x04003BF1 RID: 15345
		public const short CrimtaneWorkbench = 5796;

		// Token: 0x04003BF2 RID: 15346
		public const short Fake_CrimtaneChest = 5797;

		// Token: 0x04003BF3 RID: 15347
		public const short SnowBathtub = 5798;

		// Token: 0x04003BF4 RID: 15348
		public const short SnowBed = 5799;

		// Token: 0x04003BF5 RID: 15349
		public const short SnowBookcase = 5800;

		// Token: 0x04003BF6 RID: 15350
		public const short SnowCandelabra = 5801;

		// Token: 0x04003BF7 RID: 15351
		public const short SnowCandle = 5802;

		// Token: 0x04003BF8 RID: 15352
		public const short SnowChair = 5803;

		// Token: 0x04003BF9 RID: 15353
		public const short SnowChandelier = 5804;

		// Token: 0x04003BFA RID: 15354
		public const short SnowChest = 5805;

		// Token: 0x04003BFB RID: 15355
		public const short SnowClock = 5806;

		// Token: 0x04003BFC RID: 15356
		public const short SnowDoor = 5807;

		// Token: 0x04003BFD RID: 15357
		public const short SnowDresser = 5808;

		// Token: 0x04003BFE RID: 15358
		public const short SnowLamp = 5809;

		// Token: 0x04003BFF RID: 15359
		public const short SnowLantern = 5810;

		// Token: 0x04003C00 RID: 15360
		public const short SnowPiano = 5811;

		// Token: 0x04003C01 RID: 15361
		public const short SnowPlatform = 5812;

		// Token: 0x04003C02 RID: 15362
		public const short SnowSink = 5813;

		// Token: 0x04003C03 RID: 15363
		public const short SnowSofa = 5814;

		// Token: 0x04003C04 RID: 15364
		public const short SnowTable = 5815;

		// Token: 0x04003C05 RID: 15365
		public const short SnowToilet = 5816;

		// Token: 0x04003C06 RID: 15366
		public const short SnowWorkbench = 5817;

		// Token: 0x04003C07 RID: 15367
		public const short Fake_SnowChest = 5818;

		// Token: 0x04003C08 RID: 15368
		public const short FlinxFurBathtub = 5819;

		// Token: 0x04003C09 RID: 15369
		public const short FlinxFurBed = 5820;

		// Token: 0x04003C0A RID: 15370
		public const short FlinxFurBookcase = 5821;

		// Token: 0x04003C0B RID: 15371
		public const short FlinxFurCandelabra = 5822;

		// Token: 0x04003C0C RID: 15372
		public const short FlinxFurCandle = 5823;

		// Token: 0x04003C0D RID: 15373
		public const short FlinxFurChair = 5824;

		// Token: 0x04003C0E RID: 15374
		public const short FlinxFurChandelier = 5825;

		// Token: 0x04003C0F RID: 15375
		public const short FlinxFurChest = 5826;

		// Token: 0x04003C10 RID: 15376
		public const short FlinxFurClock = 5827;

		// Token: 0x04003C11 RID: 15377
		public const short FlinxFurDoor = 5828;

		// Token: 0x04003C12 RID: 15378
		public const short FlinxFurDresser = 5829;

		// Token: 0x04003C13 RID: 15379
		public const short FlinxFurLamp = 5830;

		// Token: 0x04003C14 RID: 15380
		public const short FlinxFurLantern = 5831;

		// Token: 0x04003C15 RID: 15381
		public const short FlinxFurPiano = 5832;

		// Token: 0x04003C16 RID: 15382
		public const short FlinxFurPlatform = 5833;

		// Token: 0x04003C17 RID: 15383
		public const short FlinxFurSink = 5834;

		// Token: 0x04003C18 RID: 15384
		public const short FlinxFurSofa = 5835;

		// Token: 0x04003C19 RID: 15385
		public const short FlinxFurTable = 5836;

		// Token: 0x04003C1A RID: 15386
		public const short FlinxFurToilet = 5837;

		// Token: 0x04003C1B RID: 15387
		public const short FlinxFurWorkbench = 5838;

		// Token: 0x04003C1C RID: 15388
		public const short Fake_FlinxFurChest = 5839;

		// Token: 0x04003C1D RID: 15389
		public const short PineBathtub = 5840;

		// Token: 0x04003C1E RID: 15390
		public const short PineBed = 5841;

		// Token: 0x04003C1F RID: 15391
		public const short PineBookcase = 5842;

		// Token: 0x04003C20 RID: 15392
		public const short PineCandelabra = 5843;

		// Token: 0x04003C21 RID: 15393
		public const short PineCandle = 5844;

		// Token: 0x04003C22 RID: 15394
		public const short PineChandelier = 5845;

		// Token: 0x04003C23 RID: 15395
		public const short PineChest = 5846;

		// Token: 0x04003C24 RID: 15396
		public const short PineClock = 5847;

		// Token: 0x04003C25 RID: 15397
		public const short PineDresser = 5848;

		// Token: 0x04003C26 RID: 15398
		public const short PineLamp = 5849;

		// Token: 0x04003C27 RID: 15399
		public const short PineLantern = 5850;

		// Token: 0x04003C28 RID: 15400
		public const short PinePiano = 5851;

		// Token: 0x04003C29 RID: 15401
		public const short PinePlatform = 5852;

		// Token: 0x04003C2A RID: 15402
		public const short PineSink = 5853;

		// Token: 0x04003C2B RID: 15403
		public const short PineSofa = 5854;

		// Token: 0x04003C2C RID: 15404
		public const short PineToilet = 5855;

		// Token: 0x04003C2D RID: 15405
		public const short PineWorkbench = 5856;

		// Token: 0x04003C2E RID: 15406
		public const short Fake_PineChest = 5857;

		// Token: 0x04003C2F RID: 15407
		public const short EasterBathtub = 5858;

		// Token: 0x04003C30 RID: 15408
		public const short EasterBed = 5859;

		// Token: 0x04003C31 RID: 15409
		public const short EasterBookcase = 5860;

		// Token: 0x04003C32 RID: 15410
		public const short EasterCandelabra = 5861;

		// Token: 0x04003C33 RID: 15411
		public const short EasterCandle = 5862;

		// Token: 0x04003C34 RID: 15412
		public const short EasterChair = 5863;

		// Token: 0x04003C35 RID: 15413
		public const short EasterChandelier = 5864;

		// Token: 0x04003C36 RID: 15414
		public const short EasterChest = 5865;

		// Token: 0x04003C37 RID: 15415
		public const short EasterClock = 5866;

		// Token: 0x04003C38 RID: 15416
		public const short EasterDoor = 5867;

		// Token: 0x04003C39 RID: 15417
		public const short EasterDresser = 5868;

		// Token: 0x04003C3A RID: 15418
		public const short EasterLamp = 5869;

		// Token: 0x04003C3B RID: 15419
		public const short EasterLantern = 5870;

		// Token: 0x04003C3C RID: 15420
		public const short EasterPiano = 5871;

		// Token: 0x04003C3D RID: 15421
		public const short EasterPlatform = 5872;

		// Token: 0x04003C3E RID: 15422
		public const short EasterSink = 5873;

		// Token: 0x04003C3F RID: 15423
		public const short EasterSofa = 5874;

		// Token: 0x04003C40 RID: 15424
		public const short EasterTable = 5875;

		// Token: 0x04003C41 RID: 15425
		public const short EasterToilet = 5876;

		// Token: 0x04003C42 RID: 15426
		public const short EasterWorkbench = 5877;

		// Token: 0x04003C43 RID: 15427
		public const short Fake_EasterChest = 5878;

		// Token: 0x04003C44 RID: 15428
		public const short StoneBathtub = 5879;

		// Token: 0x04003C45 RID: 15429
		public const short StoneBed = 5880;

		// Token: 0x04003C46 RID: 15430
		public const short StoneBookcase = 5881;

		// Token: 0x04003C47 RID: 15431
		public const short StoneCandelabra = 5882;

		// Token: 0x04003C48 RID: 15432
		public const short StoneCandle = 5883;

		// Token: 0x04003C49 RID: 15433
		public const short StoneChair = 5884;

		// Token: 0x04003C4A RID: 15434
		public const short StoneChandelier = 5885;

		// Token: 0x04003C4B RID: 15435
		public const short StoneChest = 5886;

		// Token: 0x04003C4C RID: 15436
		public const short StoneClock = 5887;

		// Token: 0x04003C4D RID: 15437
		public const short StoneDresser = 5888;

		// Token: 0x04003C4E RID: 15438
		public const short StoneLamp = 5889;

		// Token: 0x04003C4F RID: 15439
		public const short StoneLantern = 5890;

		// Token: 0x04003C50 RID: 15440
		public const short StonePiano = 5891;

		// Token: 0x04003C51 RID: 15441
		public const short StoneSink = 5892;

		// Token: 0x04003C52 RID: 15442
		public const short StoneSofa = 5893;

		// Token: 0x04003C53 RID: 15443
		public const short StoneTable = 5894;

		// Token: 0x04003C54 RID: 15444
		public const short StoneToilet = 5895;

		// Token: 0x04003C55 RID: 15445
		public const short StoneWorkbench = 5896;

		// Token: 0x04003C56 RID: 15446
		public const short Fake_StoneChest = 5897;

		// Token: 0x04003C57 RID: 15447
		public const short JellyfishBathtub = 5898;

		// Token: 0x04003C58 RID: 15448
		public const short JellyfishBed = 5899;

		// Token: 0x04003C59 RID: 15449
		public const short JellyfishBookcase = 5900;

		// Token: 0x04003C5A RID: 15450
		public const short JellyfishCandelabra = 5901;

		// Token: 0x04003C5B RID: 15451
		public const short JellyfishCandle = 5902;

		// Token: 0x04003C5C RID: 15452
		public const short JellyfishChair = 5903;

		// Token: 0x04003C5D RID: 15453
		public const short JellyfishChandelier = 5904;

		// Token: 0x04003C5E RID: 15454
		public const short JellyfishChest = 5905;

		// Token: 0x04003C5F RID: 15455
		public const short JellyfishClock = 5906;

		// Token: 0x04003C60 RID: 15456
		public const short JellyfishDoor = 5907;

		// Token: 0x04003C61 RID: 15457
		public const short JellyfishDresser = 5908;

		// Token: 0x04003C62 RID: 15458
		public const short JellyfishLamp = 5909;

		// Token: 0x04003C63 RID: 15459
		public const short JellyfishLantern = 5910;

		// Token: 0x04003C64 RID: 15460
		public const short JellyfishPiano = 5911;

		// Token: 0x04003C65 RID: 15461
		public const short JellyfishPlatform = 5912;

		// Token: 0x04003C66 RID: 15462
		public const short JellyfishSink = 5913;

		// Token: 0x04003C67 RID: 15463
		public const short JellyfishSofa = 5914;

		// Token: 0x04003C68 RID: 15464
		public const short JellyfishTable = 5915;

		// Token: 0x04003C69 RID: 15465
		public const short JellyfishToilet = 5916;

		// Token: 0x04003C6A RID: 15466
		public const short JellyfishWorkbench = 5917;

		// Token: 0x04003C6B RID: 15467
		public const short Fake_JellyfishChest = 5918;

		// Token: 0x04003C6C RID: 15468
		public const short PineTreeBlockWall = 5919;

		// Token: 0x04003C6D RID: 15469
		public const short EasterBlock = 5920;

		// Token: 0x04003C6E RID: 15470
		public const short EasterBlockWall = 5921;

		// Token: 0x04003C6F RID: 15471
		public const short GothicBrick = 5922;

		// Token: 0x04003C70 RID: 15472
		public const short GothicBrickWall = 5923;

		// Token: 0x04003C71 RID: 15473
		public const short FlinxFurBlock = 5924;

		// Token: 0x04003C72 RID: 15474
		public const short FlinxFurBlockWall = 5925;

		// Token: 0x04003C73 RID: 15475
		public const short JellyfishBlock = 5926;

		// Token: 0x04003C74 RID: 15476
		public const short JellyfishBlockWall = 5927;

		// Token: 0x04003C75 RID: 15477
		public const short ToyBreakerBlock = 5928;

		// Token: 0x04003C76 RID: 15478
		public const short PaintingRemix = 5929;

		// Token: 0x04003C77 RID: 15479
		public const short PineWoodBlock = 5930;

		// Token: 0x04003C78 RID: 15480
		public const short PineWoodBlockWall = 5931;

		// Token: 0x04003C79 RID: 15481
		public const short HarpyBathtub = 5932;

		// Token: 0x04003C7A RID: 15482
		public const short HarpyBed = 5933;

		// Token: 0x04003C7B RID: 15483
		public const short HarpyBookcase = 5934;

		// Token: 0x04003C7C RID: 15484
		public const short HarpyCandelabra = 5935;

		// Token: 0x04003C7D RID: 15485
		public const short HarpyCandle = 5936;

		// Token: 0x04003C7E RID: 15486
		public const short HarpyChair = 5937;

		// Token: 0x04003C7F RID: 15487
		public const short HarpyChandelier = 5938;

		// Token: 0x04003C80 RID: 15488
		public const short HarpyChest = 5939;

		// Token: 0x04003C81 RID: 15489
		public const short HarpyClock = 5940;

		// Token: 0x04003C82 RID: 15490
		public const short HarpyDoor = 5941;

		// Token: 0x04003C83 RID: 15491
		public const short HarpyDresser = 5942;

		// Token: 0x04003C84 RID: 15492
		public const short HarpyLamp = 5943;

		// Token: 0x04003C85 RID: 15493
		public const short HarpyLantern = 5944;

		// Token: 0x04003C86 RID: 15494
		public const short HarpyPiano = 5945;

		// Token: 0x04003C87 RID: 15495
		public const short HarpyPlatform = 5946;

		// Token: 0x04003C88 RID: 15496
		public const short HarpySink = 5947;

		// Token: 0x04003C89 RID: 15497
		public const short HarpySofa = 5948;

		// Token: 0x04003C8A RID: 15498
		public const short HarpyTable = 5949;

		// Token: 0x04003C8B RID: 15499
		public const short HarpyToilet = 5950;

		// Token: 0x04003C8C RID: 15500
		public const short HarpyWorkbench = 5951;

		// Token: 0x04003C8D RID: 15501
		public const short Fake_HarpyChest = 5952;

		// Token: 0x04003C8E RID: 15502
		public const short HarpyBlock = 5953;

		// Token: 0x04003C8F RID: 15503
		public const short HarpyBlockWall = 5954;

		// Token: 0x04003C90 RID: 15504
		public const short CloudBathtub = 5955;

		// Token: 0x04003C91 RID: 15505
		public const short CloudBed = 5956;

		// Token: 0x04003C92 RID: 15506
		public const short CloudBookcase = 5957;

		// Token: 0x04003C93 RID: 15507
		public const short CloudCandelabra = 5958;

		// Token: 0x04003C94 RID: 15508
		public const short CloudCandle = 5959;

		// Token: 0x04003C95 RID: 15509
		public const short CloudChair = 5960;

		// Token: 0x04003C96 RID: 15510
		public const short CloudChandelier = 5961;

		// Token: 0x04003C97 RID: 15511
		public const short CloudChest = 5962;

		// Token: 0x04003C98 RID: 15512
		public const short CloudClock = 5963;

		// Token: 0x04003C99 RID: 15513
		public const short CloudDoor = 5964;

		// Token: 0x04003C9A RID: 15514
		public const short CloudDresser = 5965;

		// Token: 0x04003C9B RID: 15515
		public const short CloudLamp = 5966;

		// Token: 0x04003C9C RID: 15516
		public const short CloudLantern = 5967;

		// Token: 0x04003C9D RID: 15517
		public const short CloudPiano = 5968;

		// Token: 0x04003C9E RID: 15518
		public const short CloudSink = 5969;

		// Token: 0x04003C9F RID: 15519
		public const short CloudSofa = 5970;

		// Token: 0x04003CA0 RID: 15520
		public const short CloudTable = 5971;

		// Token: 0x04003CA1 RID: 15521
		public const short CloudToilet = 5972;

		// Token: 0x04003CA2 RID: 15522
		public const short CloudWorkbench = 5973;

		// Token: 0x04003CA3 RID: 15523
		public const short Fake_CloudChest = 5974;

		// Token: 0x04003CA4 RID: 15524
		public const short MoonplateBathtub = 5975;

		// Token: 0x04003CA5 RID: 15525
		public const short MoonplateBed = 5976;

		// Token: 0x04003CA6 RID: 15526
		public const short MoonplateBookcase = 5977;

		// Token: 0x04003CA7 RID: 15527
		public const short MoonplateCandelabra = 5978;

		// Token: 0x04003CA8 RID: 15528
		public const short MoonplateCandle = 5979;

		// Token: 0x04003CA9 RID: 15529
		public const short MoonplateChair = 5980;

		// Token: 0x04003CAA RID: 15530
		public const short MoonplateChandelier = 5981;

		// Token: 0x04003CAB RID: 15531
		public const short MoonplateChest = 5982;

		// Token: 0x04003CAC RID: 15532
		public const short MoonplateClock = 5983;

		// Token: 0x04003CAD RID: 15533
		public const short MoonplateDoor = 5984;

		// Token: 0x04003CAE RID: 15534
		public const short MoonplateDresser = 5985;

		// Token: 0x04003CAF RID: 15535
		public const short MoonplateLamp = 5986;

		// Token: 0x04003CB0 RID: 15536
		public const short MoonplateLantern = 5987;

		// Token: 0x04003CB1 RID: 15537
		public const short MoonplatePiano = 5988;

		// Token: 0x04003CB2 RID: 15538
		public const short MoonplatePlatform = 5989;

		// Token: 0x04003CB3 RID: 15539
		public const short MoonplateSink = 5990;

		// Token: 0x04003CB4 RID: 15540
		public const short MoonplateSofa = 5991;

		// Token: 0x04003CB5 RID: 15541
		public const short MoonplateTable = 5992;

		// Token: 0x04003CB6 RID: 15542
		public const short MoonplateToilet = 5993;

		// Token: 0x04003CB7 RID: 15543
		public const short MoonplateWorkbench = 5994;

		// Token: 0x04003CB8 RID: 15544
		public const short Fake_MoonplateChest = 5995;

		// Token: 0x04003CB9 RID: 15545
		public const short MoonplateBlock = 5996;

		// Token: 0x04003CBA RID: 15546
		public const short MoonplateBlockWall = 5997;

		// Token: 0x04003CBB RID: 15547
		public const short LibrarianBathtub = 5998;

		// Token: 0x04003CBC RID: 15548
		public const short LibrarianBed = 5999;

		// Token: 0x04003CBD RID: 15549
		public const short LibrarianBookcase = 6000;

		// Token: 0x04003CBE RID: 15550
		public const short LibrarianCandelabra = 6001;

		// Token: 0x04003CBF RID: 15551
		public const short LibrarianCandle = 6002;

		// Token: 0x04003CC0 RID: 15552
		public const short LibrarianChair = 6003;

		// Token: 0x04003CC1 RID: 15553
		public const short LibrarianChandelier = 6004;

		// Token: 0x04003CC2 RID: 15554
		public const short LibrarianChest = 6005;

		// Token: 0x04003CC3 RID: 15555
		public const short LibrarianClock = 6006;

		// Token: 0x04003CC4 RID: 15556
		public const short LibrarianDoor = 6007;

		// Token: 0x04003CC5 RID: 15557
		public const short LibrarianDresser = 6008;

		// Token: 0x04003CC6 RID: 15558
		public const short LibrarianLamp = 6009;

		// Token: 0x04003CC7 RID: 15559
		public const short LibrarianLantern = 6010;

		// Token: 0x04003CC8 RID: 15560
		public const short LibrarianPiano = 6011;

		// Token: 0x04003CC9 RID: 15561
		public const short LibrarianPlatform = 6012;

		// Token: 0x04003CCA RID: 15562
		public const short LibrarianSink = 6013;

		// Token: 0x04003CCB RID: 15563
		public const short LibrarianSofa = 6014;

		// Token: 0x04003CCC RID: 15564
		public const short LibrarianTable = 6015;

		// Token: 0x04003CCD RID: 15565
		public const short LibrarianToilet = 6016;

		// Token: 0x04003CCE RID: 15566
		public const short LibrarianWorkbench = 6017;

		// Token: 0x04003CCF RID: 15567
		public const short Fake_LibrarianChest = 6018;

		// Token: 0x04003CD0 RID: 15568
		public const short LibrarianBlock = 6019;

		// Token: 0x04003CD1 RID: 15569
		public const short LibrarianBlockWall = 6020;

		// Token: 0x04003CD2 RID: 15570
		public const short SpikeBathtub = 6021;

		// Token: 0x04003CD3 RID: 15571
		public const short SpikeBed = 6022;

		// Token: 0x04003CD4 RID: 15572
		public const short SpikeBookcase = 6023;

		// Token: 0x04003CD5 RID: 15573
		public const short SpikeCandelabra = 6024;

		// Token: 0x04003CD6 RID: 15574
		public const short SpikeCandle = 6025;

		// Token: 0x04003CD7 RID: 15575
		public const short SpikeChair = 6026;

		// Token: 0x04003CD8 RID: 15576
		public const short SpikeChandelier = 6027;

		// Token: 0x04003CD9 RID: 15577
		public const short SpikeChest = 6028;

		// Token: 0x04003CDA RID: 15578
		public const short SpikeClock = 6029;

		// Token: 0x04003CDB RID: 15579
		public const short SpikeDoor = 6030;

		// Token: 0x04003CDC RID: 15580
		public const short SpikeDresser = 6031;

		// Token: 0x04003CDD RID: 15581
		public const short SpikeLamp = 6032;

		// Token: 0x04003CDE RID: 15582
		public const short SpikeLantern = 6033;

		// Token: 0x04003CDF RID: 15583
		public const short SpikePiano = 6034;

		// Token: 0x04003CE0 RID: 15584
		public const short SpikePlatform = 6035;

		// Token: 0x04003CE1 RID: 15585
		public const short SpikeSink = 6036;

		// Token: 0x04003CE2 RID: 15586
		public const short SpikeSofa = 6037;

		// Token: 0x04003CE3 RID: 15587
		public const short SpikeTable = 6038;

		// Token: 0x04003CE4 RID: 15588
		public const short SpikeToilet = 6039;

		// Token: 0x04003CE5 RID: 15589
		public const short SpikeWorkbench = 6040;

		// Token: 0x04003CE6 RID: 15590
		public const short Fake_SpikeChest = 6041;

		// Token: 0x04003CE7 RID: 15591
		public const short SpikeBlock = 6042;

		// Token: 0x04003CE8 RID: 15592
		public const short SpikeBlockWall = 6043;

		// Token: 0x04003CE9 RID: 15593
		public const short OfficeBathtub = 6044;

		// Token: 0x04003CEA RID: 15594
		public const short OfficeBed = 6045;

		// Token: 0x04003CEB RID: 15595
		public const short OfficeBookcase = 6046;

		// Token: 0x04003CEC RID: 15596
		public const short OfficeCandelabra = 6047;

		// Token: 0x04003CED RID: 15597
		public const short OfficeCandle = 6048;

		// Token: 0x04003CEE RID: 15598
		public const short OfficeChair = 6049;

		// Token: 0x04003CEF RID: 15599
		public const short OfficeChandelier = 6050;

		// Token: 0x04003CF0 RID: 15600
		public const short OfficeChest = 6051;

		// Token: 0x04003CF1 RID: 15601
		public const short OfficeClock = 6052;

		// Token: 0x04003CF2 RID: 15602
		public const short OfficeDoor = 6053;

		// Token: 0x04003CF3 RID: 15603
		public const short OfficeDresser = 6054;

		// Token: 0x04003CF4 RID: 15604
		public const short OfficeLamp = 6055;

		// Token: 0x04003CF5 RID: 15605
		public const short OfficeLantern = 6056;

		// Token: 0x04003CF6 RID: 15606
		public const short OfficePiano = 6057;

		// Token: 0x04003CF7 RID: 15607
		public const short OfficePlatform = 6058;

		// Token: 0x04003CF8 RID: 15608
		public const short OfficeSink = 6059;

		// Token: 0x04003CF9 RID: 15609
		public const short OfficeSofa = 6060;

		// Token: 0x04003CFA RID: 15610
		public const short OfficeTable = 6061;

		// Token: 0x04003CFB RID: 15611
		public const short OfficeToilet = 6062;

		// Token: 0x04003CFC RID: 15612
		public const short OfficeWorkbench = 6063;

		// Token: 0x04003CFD RID: 15613
		public const short Fake_OfficeChest = 6064;

		// Token: 0x04003CFE RID: 15614
		public const short OfficeBlock = 6065;

		// Token: 0x04003CFF RID: 15615
		public const short OfficeBlockWall = 6066;

		// Token: 0x04003D00 RID: 15616
		public const short ForbiddenBathtub = 6067;

		// Token: 0x04003D01 RID: 15617
		public const short ForbiddenBed = 6068;

		// Token: 0x04003D02 RID: 15618
		public const short ForbiddenBookcase = 6069;

		// Token: 0x04003D03 RID: 15619
		public const short ForbiddenCandelabra = 6070;

		// Token: 0x04003D04 RID: 15620
		public const short ForbiddenCandle = 6071;

		// Token: 0x04003D05 RID: 15621
		public const short ForbiddenChair = 6072;

		// Token: 0x04003D06 RID: 15622
		public const short ForbiddenChandelier = 6073;

		// Token: 0x04003D07 RID: 15623
		public const short ForbiddenChest = 6074;

		// Token: 0x04003D08 RID: 15624
		public const short ForbiddenClock = 6075;

		// Token: 0x04003D09 RID: 15625
		public const short ForbiddenDoor = 6076;

		// Token: 0x04003D0A RID: 15626
		public const short ForbiddenDresser = 6077;

		// Token: 0x04003D0B RID: 15627
		public const short ForbiddenLamp = 6078;

		// Token: 0x04003D0C RID: 15628
		public const short ForbiddenLantern = 6079;

		// Token: 0x04003D0D RID: 15629
		public const short ForbiddenPiano = 6080;

		// Token: 0x04003D0E RID: 15630
		public const short ForbiddenPlatform = 6081;

		// Token: 0x04003D0F RID: 15631
		public const short ForbiddenSink = 6082;

		// Token: 0x04003D10 RID: 15632
		public const short ForbiddenSofa = 6083;

		// Token: 0x04003D11 RID: 15633
		public const short ForbiddenTable = 6084;

		// Token: 0x04003D12 RID: 15634
		public const short ForbiddenToilet = 6085;

		// Token: 0x04003D13 RID: 15635
		public const short ForbiddenWorkbench = 6086;

		// Token: 0x04003D14 RID: 15636
		public const short Fake_ForbiddenChest = 6087;

		// Token: 0x04003D15 RID: 15637
		public const short ForbiddenBlock = 6088;

		// Token: 0x04003D16 RID: 15638
		public const short ForbiddenBlockWall = 6089;

		// Token: 0x04003D17 RID: 15639
		public const short WaterBathtub = 6090;

		// Token: 0x04003D18 RID: 15640
		public const short WaterBed = 6091;

		// Token: 0x04003D19 RID: 15641
		public const short WaterBookcase = 6092;

		// Token: 0x04003D1A RID: 15642
		public const short WaterCandelabra = 6093;

		// Token: 0x04003D1B RID: 15643
		public const short WaterFurnitureCandle = 6094;

		// Token: 0x04003D1C RID: 15644
		public const short WaterChair = 6095;

		// Token: 0x04003D1D RID: 15645
		public const short WaterChandelier = 6096;

		// Token: 0x04003D1E RID: 15646
		public const short WaterClock = 6097;

		// Token: 0x04003D1F RID: 15647
		public const short WaterDoor = 6098;

		// Token: 0x04003D20 RID: 15648
		public const short WaterDresser = 6099;

		// Token: 0x04003D21 RID: 15649
		public const short WaterLamp = 6100;

		// Token: 0x04003D22 RID: 15650
		public const short WaterLantern = 6101;

		// Token: 0x04003D23 RID: 15651
		public const short WaterPiano = 6102;

		// Token: 0x04003D24 RID: 15652
		public const short WaterPlatform = 6103;

		// Token: 0x04003D25 RID: 15653
		public const short WaterSink = 6104;

		// Token: 0x04003D26 RID: 15654
		public const short WaterSofa = 6105;

		// Token: 0x04003D27 RID: 15655
		public const short WaterTable = 6106;

		// Token: 0x04003D28 RID: 15656
		public const short WaterToilet = 6107;

		// Token: 0x04003D29 RID: 15657
		public const short WaterWorkbench = 6108;

		// Token: 0x04003D2A RID: 15658
		public const short WaterBlock = 6109;

		// Token: 0x04003D2B RID: 15659
		public const short WaterBlockWall = 6110;

		// Token: 0x04003D2C RID: 15660
		public const short BoulderBathtub = 6111;

		// Token: 0x04003D2D RID: 15661
		public const short BoulderBed = 6112;

		// Token: 0x04003D2E RID: 15662
		public const short BoulderBookcase = 6113;

		// Token: 0x04003D2F RID: 15663
		public const short BoulderCandelabra = 6114;

		// Token: 0x04003D30 RID: 15664
		public const short BoulderCandle = 6115;

		// Token: 0x04003D31 RID: 15665
		public const short BoulderChair = 6116;

		// Token: 0x04003D32 RID: 15666
		public const short BoulderChandelier = 6117;

		// Token: 0x04003D33 RID: 15667
		public const short BoulderChest = 6118;

		// Token: 0x04003D34 RID: 15668
		public const short BoulderClock = 6119;

		// Token: 0x04003D35 RID: 15669
		public const short BoulderDoor = 6120;

		// Token: 0x04003D36 RID: 15670
		public const short BoulderDresser = 6121;

		// Token: 0x04003D37 RID: 15671
		public const short BoulderLamp = 6122;

		// Token: 0x04003D38 RID: 15672
		public const short BoulderLantern = 6123;

		// Token: 0x04003D39 RID: 15673
		public const short BoulderPiano = 6124;

		// Token: 0x04003D3A RID: 15674
		public const short BoulderPlatform = 6125;

		// Token: 0x04003D3B RID: 15675
		public const short BoulderSink = 6126;

		// Token: 0x04003D3C RID: 15676
		public const short BoulderSofa = 6127;

		// Token: 0x04003D3D RID: 15677
		public const short BoulderTable = 6128;

		// Token: 0x04003D3E RID: 15678
		public const short BoulderToilet = 6129;

		// Token: 0x04003D3F RID: 15679
		public const short BoulderWorkbench = 6130;

		// Token: 0x04003D40 RID: 15680
		public const short Fake_BoulderChest = 6131;

		// Token: 0x04003D41 RID: 15681
		public const short BoulderBlock = 6132;

		// Token: 0x04003D42 RID: 15682
		public const short BoulderBlockWall = 6133;

		// Token: 0x04003D43 RID: 15683
		public const short DamagingSpikeBlock = 6134;

		// Token: 0x04003D44 RID: 15684
		public const short DemonAltarIcon = 6135;

		// Token: 0x04003D45 RID: 15685
		public const short CrimsonAltarIcon = 6136;

		// Token: 0x04003D46 RID: 15686
		public const short LunasHead = 6137;

		// Token: 0x04003D47 RID: 15687
		public const short LunasBody = 6138;

		// Token: 0x04003D48 RID: 15688
		public const short LunasLegs = 6139;

		// Token: 0x04003D49 RID: 15689
		public const short LunasWings = 6140;

		// Token: 0x04003D4A RID: 15690
		public const short LunasCloak = 6141;

		// Token: 0x04003D4B RID: 15691
		public const short PalworldChilletEgg = 6142;

		// Token: 0x04003D4C RID: 15692
		public const short FoxparksTagEffect = 6143;

		// Token: 0x04003D4D RID: 15693
		public const short MusicBoxSkeletron = 6144;

		// Token: 0x04003D4E RID: 15694
		public static readonly short Count = 6145;

		// Token: 0x04003D4F RID: 15695
		public static readonly IdDictionary Search = IdDictionary.Create<ItemID, short>();

		// Token: 0x02000784 RID: 1924
		public struct BannerEffect
		{
			// Token: 0x06004145 RID: 16709 RVA: 0x006A062C File Offset: 0x0069E82C
			public BannerEffect(float strength = 1f)
			{
				float output = 1f + strength * 0.5f;
				float output2 = 1f + strength;
				this.DamageDealt = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
				{
					new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Classic, output),
					new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Expert, output2)
				});
				float num = 1f / (strength + 1f);
				float output3 = (1f + num) / 2f;
				this.DamageReceived = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
				{
					new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Classic, output3),
					new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Expert, num)
				});
				this.Enabled = (strength != 0f);
			}

			// Token: 0x06004146 RID: 16710 RVA: 0x006A06E6 File Offset: 0x0069E8E6
			public BannerEffect(GameDifficultyData.LinearCurve damageDealt, GameDifficultyData.LinearCurve damageReceived)
			{
				this.DamageDealt = damageDealt;
				this.DamageReceived = damageReceived;
				this.Enabled = true;
			}

			// Token: 0x04006E59 RID: 28249
			public static readonly ItemID.BannerEffect None = new ItemID.BannerEffect(0f);

			// Token: 0x04006E5A RID: 28250
			public static readonly ItemID.BannerEffect Reduced = new ItemID.BannerEffect(0.2f);

			// Token: 0x04006E5B RID: 28251
			public readonly GameDifficultyData.LinearCurve DamageDealt;

			// Token: 0x04006E5C RID: 28252
			public readonly GameDifficultyData.LinearCurve DamageReceived;

			// Token: 0x04006E5D RID: 28253
			public readonly bool Enabled;
		}

		// Token: 0x02000785 RID: 1925
		public class Sets
		{
			// Token: 0x06004148 RID: 16712 RVA: 0x006A0720 File Offset: 0x0069E920
			public static void PostSetupContent()
			{
				for (int i = 0; i < (int)ItemID.Count; i++)
				{
					Item item = ContentSamples.ItemsByType[i];
					int tileType = -1;
					short tileStyle = 0;
					if (item.createTile >= 0)
					{
						tileType = item.createTile;
					}
					if (item.placeStyle >= 0)
					{
						tileStyle = (short)item.placeStyle;
					}
					ItemID.Sets.DerivedPlacementDetails[i] = new PlacementDetails
					{
						tileType = tileType,
						tileStyle = tileStyle
					};
				}
			}

			// Token: 0x04006E5E RID: 28254
			public static SetFactory Factory = new SetFactory((int)ItemID.Count);

			// Token: 0x04006E5F RID: 28255
			public static PlacementDetails[] DerivedPlacementDetails = ItemID.Sets.Factory.CreateCustomSet<PlacementDetails>(new PlacementDetails
			{
				tileType = -1,
				tileStyle = 0
			}, new object[0]);

			// Token: 0x04006E60 RID: 28256
			public static List<int> ItemsThatAreProcessedAfterNormalContentSample = new List<int>
			{
				1533,
				1534,
				1535,
				1536,
				1537
			};

			// Token: 0x04006E61 RID: 28257
			public static bool[] DualEquipArmor = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				205,
				5004,
				4955
			});

			// Token: 0x04006E62 RID: 28258
			public static bool[] ReceivesDiagonalCorrectionAsFakeFish = ItemID.Sets.Factory.CreateBoolSet(true, new int[]
			{
				2342
			});

			// Token: 0x04006E63 RID: 28259
			public static bool[] ShootsOnUseRelease = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				198,
				3764,
				200,
				3766,
				4258,
				4259,
				201,
				3767,
				199,
				3765,
				203,
				3769,
				202,
				3768,
				5535,
				5536,
				5670,
				5671,
				671,
				3772,
				3352
			});

			// Token: 0x04006E64 RID: 28260
			public static bool[] NoMeleeSpeedBonus = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				3821
			});

			// Token: 0x04006E65 RID: 28261
			public static bool[] IsLavaImmuneRegardlessOfRarity = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				318,
				312,
				173,
				174,
				175,
				4422,
				2701,
				205,
				206,
				207,
				1128,
				2340,
				2739,
				2492,
				1127,
				85,
				581,
				582
			});

			// Token: 0x04006E66 RID: 28262
			public static bool[] ItemsThatAllowRepeatedRightClick = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				3384,
				3858,
				3852
			});

			// Token: 0x04006E67 RID: 28263
			public static bool[] ItemsThatCountAsBombsForDemolitionistToSpawn = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				168,
				2586,
				3116,
				166,
				235,
				3115,
				167,
				2896,
				3547,
				3196,
				4423,
				1130,
				1168,
				4824,
				4825,
				4826,
				4827,
				4908,
				4909,
				5594,
				5595
			});

			// Token: 0x04006E68 RID: 28264
			public static bool[] RapidAttackBonusDamage = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				2608,
				3349,
				672
			});

			// Token: 0x04006E69 RID: 28265
			public static bool[] ItemsThatShouldNotBeInInventory = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				58,
				184,
				1734,
				1735,
				1867,
				1868,
				3453,
				3454,
				3455,
				4143,
				5013,
				6135,
				6136,
				6143
			});

			// Token: 0x04006E6A RID: 28266
			public static bool[] DrawUnsafeIndicator = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				5363,
				5365,
				5366,
				5367,
				5368,
				5369,
				5370,
				5371,
				5372,
				5373,
				5376,
				5375,
				5374,
				3988,
				5384,
				5532,
				5533,
				5545,
				5546
			});

			// Token: 0x04006E6B RID: 28267
			public static bool[] CanBeExtractinated = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				424,
				1103,
				2339,
				2338,
				2337,
				1124,
				5395,
				4090,
				173
			});

			// Token: 0x04006E6C RID: 28268
			public static bool[] UsesBetterMeleeItemLocation = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				426
			});

			// Token: 0x04006E6D RID: 28269
			public static bool[] WorksInVoidBag = ItemID.Sets.Factory.CreateBoolSet(true, new int[]
			{
				4346,
				5095
			});

			// Token: 0x04006E6E RID: 28270
			public static bool[] GrassSeeds = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				62,
				59,
				2171,
				369,
				195,
				194,
				5214
			});

			// Token: 0x04006E6F RID: 28271
			public static bool[] Moss = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				4349,
				4350,
				4351,
				4352,
				4353,
				4354,
				4377,
				4378,
				4389,
				5127,
				5128
			});

			// Token: 0x04006E70 RID: 28272
			public static int[] ShimmerTransformToItem = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				3460,
				947,
				947,
				1106,
				1106,
				366,
				366,
				1105,
				1105,
				365,
				365,
				1104,
				1104,
				364,
				364,
				702,
				702,
				13,
				13,
				701,
				701,
				14,
				14,
				700,
				700,
				11,
				11,
				699,
				699,
				12,
				12,
				133,
				133,
				3,
				3,
				2,
				5574,
				5575,
				182,
				178,
				178,
				179,
				179,
				177,
				177,
				180,
				180,
				181,
				181,
				133,
				4843,
				182,
				4836,
				182,
				4842,
				178,
				4835,
				178,
				4841,
				179,
				4834,
				179,
				4840,
				177,
				4833,
				177,
				4832,
				180,
				4839,
				180,
				4838,
				181,
				4831,
				181,
				4844,
				999,
				4837,
				999,
				620,
				9,
				619,
				9,
				911,
				9,
				621,
				9,
				2503,
				9,
				2504,
				9,
				2260,
				9,
				1729,
				9,
				5215,
				9,
				9,
				2,
				3271,
				169,
				3272,
				169,
				3276,
				370,
				3274,
				370,
				3339,
				408,
				3338,
				408,
				3277,
				1246,
				3275,
				1246,
				1127,
				1124,
				1125,
				1124,
				4503,
				5363,
				30,
				5546,
				1723,
				5545,
				134,
				5417,
				137,
				5419,
				139,
				5421,
				141,
				5423,
				143,
				5425,
				145,
				5427,
				415,
				5429,
				416,
				5431,
				192,
				5433,
				214,
				5435,
				135,
				5365,
				1379,
				5367,
				1378,
				5366,
				140,
				5368,
				1381,
				5370,
				1380,
				5369,
				138,
				5371,
				1383,
				5373,
				1382,
				5372,
				1102,
				5376,
				3340,
				5375,
				3273,
				5374,
				664,
				593,
				593,
				5349,
				3982,
				3203,
				3983,
				3204,
				3984,
				3205,
				3985,
				3206,
				4406,
				4405,
				3981,
				2336,
				3986,
				3207,
				3980,
				2335,
				3987,
				3208,
				4878,
				4877,
				4408,
				4407,
				5003,
				5002,
				3979,
				2334,
				3064,
				5381,
				3086,
				3081,
				3081,
				3086,
				1534,
				1529,
				1535,
				1530,
				1536,
				1531,
				1537,
				1532,
				4714,
				4712,
				1533,
				1528,
				206,
				207,
				207,
				1128,
				1128,
				206,
				832,
				4281,
				5295,
				5519,
				5519,
				5295,
				5438,
				5518,
				3818,
				3824,
				3824,
				3832,
				3832,
				3829,
				3829,
				3818,
				3819,
				3825,
				3825,
				3833,
				3833,
				3830,
				3830,
				3819,
				3820,
				3826,
				3826,
				3834,
				3834,
				3831,
				3831,
				3820,
				960,
				228,
				961,
				229,
				962,
				230,
				228,
				960,
				229,
				961,
				230,
				962,
				956,
				102,
				957,
				101,
				958,
				100,
				102,
				956,
				101,
				957,
				100,
				958,
				959,
				151,
				151,
				959,
				955,
				92,
				92,
				955,
				954,
				90,
				90,
				954,
				3093,
				4345,
				4345,
				3093,
				215,
				5346,
				5356,
				5346,
				3000,
				5482,
				5482,
				2999,
				2999,
				3000,
				332,
				2196,
				411,
				410,
				410,
				411,
				1725,
				276,
				276,
				1725,
				2886,
				66,
				67,
				66,
				195,
				194,
				194,
				195,
				4389,
				5128,
				4377,
				5128,
				4378,
				5128,
				4354,
				5128,
				5127,
				5128,
				8,
				5353,
				427,
				5353,
				3004,
				5353,
				523,
				5353,
				433,
				5353,
				429,
				5353,
				974,
				5353,
				1333,
				5353,
				1245,
				5353,
				3114,
				5353,
				430,
				5353,
				3045,
				5353,
				428,
				5353,
				2274,
				5353,
				431,
				5353,
				432,
				5353,
				4383,
				5353,
				4384,
				5353,
				4385,
				5353,
				4386,
				5353,
				4387,
				5353,
				4388,
				5353,
				5293,
				5353,
				966,
				5357,
				52,
				5347,
				280,
				277,
				1304,
				215,
				40,
				5348,
				265,
				5348,
				931,
				5380,
				1614,
				5380,
				848,
				857,
				857,
				848,
				866,
				934,
				934,
				866,
				532,
				5355,
				3225,
				159,
				3120,
				3096,
				3096,
				3037,
				3037,
				3120,
				2373,
				2374,
				2374,
				2375,
				2375,
				2373,
				855,
				3033,
				854,
				855,
				3033,
				854,
				490,
				491,
				491,
				489,
				489,
				2998,
				2998,
				490,
				892,
				886,
				886,
				892,
				885,
				887,
				887,
				885,
				891,
				890,
				890,
				891,
				893,
				889,
				889,
				893,
				888,
				3781,
				3781,
				888,
				1322,
				906,
				906,
				1322,
				351,
				353,
				3187,
				3188,
				3188,
				3189,
				3189,
				3187,
				531,
				5336,
				29,
				5337,
				1291,
				5338,
				109,
				5339,
				4414,
				5340,
				2895,
				5341,
				2222,
				5343,
				4009,
				5342,
				4282,
				5342,
				4290,
				5342,
				4291,
				5342,
				4293,
				5342,
				4286,
				5342,
				4295,
				5342,
				4284,
				5342,
				4289,
				5342,
				4285,
				5342,
				4296,
				5342,
				4292,
				5342,
				4294,
				5342,
				4283,
				5342,
				4287,
				5342,
				4288,
				5342,
				4297,
				5342,
				5278,
				5342,
				5277,
				5342,
				3186,
				5492,
				2693,
				5493,
				60,
				5,
				2887,
				5,
				1326,
				5335,
				779,
				5134,
				3031,
				5364,
				5364,
				3031,
				2610,
				5495,
				782,
				784,
				784,
				782,
				1146,
				1147,
				1147,
				1148,
				1148,
				1149,
				1149,
				1146,
				2261,
				2262,
				2262,
				2261,
				4907,
				4871,
				4871,
				4907,
				3015,
				3016,
				3016,
				3015,
				953,
				975,
				975,
				953,
				4919,
				5653,
				4916,
				5653,
				4875,
				5653,
				4921,
				5653,
				4918,
				5653,
				4876,
				5653,
				4920,
				5653,
				4917,
				5653,
				4951,
				5653,
				5652,
				5653,
				5523,
				5654,
				5654,
				5523,
				4956,
				5669,
				136,
				1390,
				1390,
				1391,
				1391,
				1392,
				1392,
				1393,
				1393,
				1394,
				1394,
				1395,
				1395,
				136,
				3621,
				5674,
				3633,
				5675,
				3634,
				5676,
				3635,
				5677,
				3636,
				5678,
				3637,
				5679,
				824,
				5996,
				5996,
				824,
				825,
				5997,
				5997,
				825
			});

			// Token: 0x04006E71 RID: 28273
			public static int[] ShimmerCountsAsItem = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				5358,
				5437,
				5361,
				5437,
				5360,
				5437,
				5359,
				5437,
				5455,
				5323
			});

			// Token: 0x04006E72 RID: 28274
			public static int[] ShimmerCountsAsItemForDecraft = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				5669,
				4956
			});

			// Token: 0x04006E73 RID: 28275
			public static bool[] ShimmerPostMoonlord = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				1326,
				779,
				3031,
				5364,
				784,
				782
			});

			// Token: 0x04006E74 RID: 28276
			public static int[] OverflowProtectionTimeOffset = ItemID.Sets.Factory.CreateIntSet(0, new int[]
			{
				2,
				200,
				3,
				150,
				61,
				150,
				836,
				150,
				409,
				150,
				593,
				200,
				664,
				100,
				834,
				100,
				833,
				100,
				835,
				100,
				169,
				100,
				370,
				100,
				1246,
				100,
				408,
				100,
				3271,
				150,
				3277,
				150,
				3339,
				150,
				3276,
				150,
				3272,
				150,
				3274,
				150,
				3275,
				150,
				3338,
				150,
				176,
				100,
				172,
				200,
				424,
				50,
				1103,
				50,
				3087,
				100,
				3066,
				100
			});

			// Token: 0x04006E75 RID: 28277
			public static int[] ItemsForStuffCannon = new int[]
			{
				2,
				3,
				61,
				836,
				409,
				593,
				664,
				169,
				370,
				1246,
				408,
				3271,
				3277,
				3339,
				3276,
				3272,
				3274,
				3275,
				3338,
				176,
				172,
				424,
				1103,
				3087,
				3066,
				9,
				2503,
				2504,
				619,
				911,
				621,
				620,
				1727,
				276,
				4564,
				751,
				1124,
				1125,
				824,
				129,
				131,
				607,
				594,
				883,
				414,
				413,
				609,
				4050,
				192,
				412
			};

			// Token: 0x04006E76 RID: 28278
			public static bool?[] CanBeQuickusedOnGamepad = ItemID.Sets.Factory.CreateCustomSet<bool?>(null, new object[]
			{
				50,
				true,
				3199,
				true,
				3124,
				true,
				2350,
				true,
				2351,
				true,
				29,
				true,
				109,
				true,
				1291,
				true,
				4870,
				true,
				5358,
				true,
				5359,
				true,
				5360,
				true,
				5361,
				true,
				4263,
				true,
				4819,
				true
			});

			// Token: 0x04006E77 RID: 28279
			public static bool?[] ForcesBreaksSleeping = ItemID.Sets.Factory.CreateCustomSet<bool?>(null, new object[]
			{
				1991,
				true,
				4821,
				true,
				3183,
				true
			});

			// Token: 0x04006E78 RID: 28280
			public static bool[] SkipsInitialUseSound = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				2350,
				4870
			});

			// Token: 0x04006E79 RID: 28281
			public static bool?[] NetUseSoundSync = ItemID.Sets.Factory.CreateCustomSet<bool?>(null, new object[0]);

			// Token: 0x04006E7A RID: 28282
			public static bool[] UsesCursedByPlanteraTooltip = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				1533,
				1534,
				1535,
				1536,
				1537,
				4714
			});

			// Token: 0x04006E7B RID: 28283
			public static bool[] IsAKite = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				4367,
				4368,
				4369,
				4370,
				4371,
				4379,
				4610,
				4611,
				4612,
				4613,
				4648,
				4649,
				4650,
				4651,
				4669,
				4670,
				4671,
				4674,
				4675,
				4676,
				4677,
				4681,
				4683,
				4684
			});

			// Token: 0x04006E7C RID: 28284
			public static bool[] PlaceTileOnAltUse = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				4367,
				4368,
				4369,
				4370,
				4371,
				4379,
				4610,
				4611,
				4612,
				4613,
				4648,
				4649,
				4650,
				4651,
				4669,
				4670,
				4671,
				4674,
				4675,
				4676,
				4677,
				4681,
				4683,
				4684,
				261,
				1338,
				1992,
				1994,
				1995,
				1996,
				1997,
				1998,
				1999,
				2000,
				2001,
				2002,
				2003,
				2004,
				2006,
				2007,
				2015,
				2016,
				2017,
				2018,
				2019,
				2121,
				2122,
				2123,
				2156,
				2157,
				2205,
				2673,
				2740,
				2889,
				2890,
				2891,
				2892,
				2893,
				2894,
				2895,
				3191,
				3192,
				3193,
				3194,
				3563,
				3564,
				4068,
				4069,
				4070,
				4274,
				4334,
				4335,
				4336,
				4337,
				4338,
				4339,
				4340,
				4359,
				4361,
				4362,
				4363,
				4373,
				4374,
				4375,
				4395,
				4418,
				4419,
				4464,
				4465,
				4480,
				4482,
				4831,
				4832,
				4833,
				4834,
				4835,
				4836,
				4837,
				4838,
				4839,
				4840,
				4841,
				4842,
				4843,
				4844,
				4845,
				4847,
				4849,
				4961,
				5132,
				5212,
				5300,
				5311,
				5312,
				5313,
				5350,
				5511
			});

			// Token: 0x04006E7D RID: 28285
			public static bool?[] ForceConsumption = ItemID.Sets.Factory.CreateCustomSet<bool?>(null, new object[]
			{
				2350,
				false,
				4870,
				false,
				2351,
				false,
				2756,
				false,
				4343,
				true,
				4344,
				true
			});

			// Token: 0x04006E7E RID: 28286
			public static bool[] HasAProjectileThatHasAUsabilityCheck = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				4367,
				4368,
				4369,
				4370,
				4371,
				4379,
				4610,
				4611,
				4612,
				4613,
				4648,
				4649,
				4650,
				4651,
				4669,
				4670,
				4671,
				4674,
				4675,
				4676,
				4677,
				4681,
				4683,
				4684
			});

			// Token: 0x04006E7F RID: 28287
			public static bool[] CanGetPrefixes = ItemID.Sets.Factory.CreateBoolSet(true, new int[]
			{
				267,
				1307,
				562,
				563,
				564,
				565,
				566,
				567,
				568,
				569,
				570,
				571,
				572,
				573,
				574,
				576,
				1596,
				1597,
				1598,
				1599,
				1600,
				1601,
				1602,
				1603,
				1604,
				1605,
				1606,
				1607,
				1608,
				1609,
				1610,
				1963,
				1964,
				1965,
				2742,
				3044,
				3235,
				3236,
				3237,
				3370,
				3371,
				3796,
				3869,
				4077,
				4078,
				4079,
				4080,
				4081,
				4082,
				4237,
				4356,
				4357,
				4358,
				4421,
				4606,
				4979,
				4985,
				4990,
				4991,
				4992,
				5006,
				5014,
				5015,
				5016,
				5017,
				5018,
				5019,
				5020,
				5021,
				5022,
				5023,
				5024,
				5025,
				5026,
				5027,
				5028,
				5029,
				5030,
				5031,
				5032,
				5033,
				5034,
				5035,
				5036,
				5037,
				5038,
				5039,
				5040,
				5044,
				5112,
				5362,
				5538,
				5578,
				5579,
				5580,
				5581,
				5582,
				5637,
				5638,
				5639,
				6144
			});

			// Token: 0x04006E80 RID: 28288
			public static List<int> NonColorfulDyeItems = new List<int>
			{
				3599,
				3530,
				3534
			};

			// Token: 0x04006E81 RID: 28289
			public static bool[] ColorfulDyeValues = new bool[0];

			// Token: 0x04006E82 RID: 28290
			public static FlowerPacketInfo[] flowerPacketInfo = ItemID.Sets.Factory.CreateCustomSet<FlowerPacketInfo>(null, new object[]
			{
				4041,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						9,
						16,
						20
					}
				},
				4042,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						6,
						30,
						31,
						32
					}
				},
				4043,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						7,
						17,
						33,
						34,
						35
					}
				},
				4044,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						19,
						21,
						22,
						23,
						39,
						40,
						41
					}
				},
				4045,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						10,
						11,
						13,
						18,
						24,
						25,
						26
					}
				},
				4046,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						12,
						42,
						43,
						44
					}
				},
				4047,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						14,
						15,
						27,
						28,
						29,
						36,
						37,
						38
					}
				},
				4241,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						6,
						7,
						9,
						10,
						11,
						12,
						13,
						14,
						15,
						16,
						17,
						18,
						19,
						20,
						21,
						22,
						23,
						24,
						25,
						26,
						27,
						28,
						29,
						30,
						31,
						32,
						33,
						34,
						35,
						36,
						37,
						38,
						39,
						40,
						41,
						42,
						43,
						44
					}
				},
				4048,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						0,
						1,
						2,
						3,
						4,
						5
					}
				}
			});

			// Token: 0x04006E83 RID: 28291
			public static bool[] IsAMaterial = ItemID.Sets.Factory.CreateBoolSet(new int[0]);

			// Token: 0x04006E84 RID: 28292
			public static int[] IsCrafted = ItemID.Sets.Factory.CreateIntSet(new int[0]);

			// Token: 0x04006E85 RID: 28293
			public static int[] IsCraftedCrimson = ItemID.Sets.Factory.CreateIntSet(new int[0]);

			// Token: 0x04006E86 RID: 28294
			public static int[] IsCraftedCorruption = ItemID.Sets.Factory.CreateIntSet(new int[0]);

			// Token: 0x04006E87 RID: 28295
			public static bool[] IgnoresEncumberingStone = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				58,
				184,
				1734,
				1735,
				1867,
				1868,
				3453,
				3454,
				3455,
				4143
			});

			// Token: 0x04006E88 RID: 28296
			public static float[] ToolTipDamageMultiplier = ItemID.Sets.Factory.CreateFloatSet(1f, new float[]
			{
				162f,
				2f,
				801f,
				2f,
				163f,
				2f,
				220f,
				2f,
				389f,
				2f,
				1259f,
				2f,
				4272f,
				2f,
				5011f,
				2f,
				5012f,
				2f,
				5526f,
				2f
			});

			// Token: 0x04006E89 RID: 28297
			public static bool[] IsAPickup = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				58,
				184,
				1734,
				1735,
				1867,
				1868,
				3453,
				3454,
				3455,
				4143
			});

			// Token: 0x04006E8A RID: 28298
			public static bool[] IsDrill = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				388,
				1231,
				385,
				386,
				2779,
				1196,
				1189,
				2784,
				3464,
				1203,
				2774,
				579
			});

			// Token: 0x04006E8B RID: 28299
			public static bool[] IsChainsaw = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				387,
				3098,
				1232,
				383,
				384,
				2778,
				1197,
				1190,
				2783,
				3463,
				1204,
				2773,
				2342,
				579
			});

			// Token: 0x04006E8C RID: 28300
			public static bool[] IsPaintScraper = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				1100,
				1545
			});

			// Token: 0x04006E8D RID: 28301
			public static bool[] SummonerWeaponThatScalesWithAttackSpeed = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				4672,
				4679,
				4680,
				4678,
				4913,
				4912,
				4911,
				4914,
				5074,
				5473,
				5474,
				5475,
				5476,
				5477,
				5478,
				5479,
				5480,
				5688
			});

			// Token: 0x04006E8E RID: 28302
			public static bool[] IsFood = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				353,
				357,
				1787,
				1911,
				1912,
				1919,
				1920,
				2266,
				2267,
				2268,
				2425,
				2426,
				2427,
				3195,
				3532,
				4009,
				4010,
				4011,
				4012,
				4013,
				4014,
				4015,
				4016,
				4017,
				4018,
				4019,
				4020,
				4021,
				4022,
				4023,
				4024,
				4025,
				4026,
				4027,
				4028,
				4029,
				4030,
				4031,
				4032,
				4033,
				4034,
				4035,
				4036,
				4037,
				967,
				969,
				4282,
				4283,
				4284,
				4285,
				4286,
				4287,
				4288,
				4289,
				4290,
				4291,
				4292,
				4293,
				4294,
				4295,
				4296,
				4297,
				4403,
				4411,
				4614,
				4615,
				4616,
				4617,
				4618,
				4619,
				4620,
				4621,
				4622,
				4623,
				4624,
				4625,
				5009,
				5042,
				5041,
				5092,
				5093,
				5275,
				5277,
				5278,
				5537,
				5645
			});

			// Token: 0x04006E8F RID: 28303
			public static Color[][] FoodParticleColors = ItemID.Sets.Factory.CreateCustomSet<Color[]>(new Color[0], new object[]
			{
				357,
				new Color[]
				{
					new Color(253, 209, 77),
					new Color(253, 178, 78)
				},
				1787,
				new Color[]
				{
					new Color(215, 146, 96),
					new Color(250, 160, 15),
					new Color(226, 130, 33)
				},
				1911,
				new Color[]
				{
					new Color(219, 219, 213),
					new Color(255, 228, 133),
					new Color(237, 159, 85),
					new Color(207, 32, 51)
				},
				1919,
				new Color[]
				{
					new Color(206, 168, 119),
					new Color(73, 182, 126),
					new Color(230, 89, 92),
					new Color(228, 238, 241)
				},
				1920,
				new Color[]
				{
					new Color(218, 167, 69),
					new Color(204, 209, 219),
					new Color(204, 22, 40),
					new Color(0, 212, 47)
				},
				2267,
				new Color[]
				{
					new Color(229, 129, 82),
					new Color(255, 223, 126),
					new Color(190, 226, 65)
				},
				2268,
				new Color[]
				{
					new Color(250, 232, 220),
					new Color(216, 189, 157),
					new Color(190, 226, 65)
				},
				2425,
				new Color[]
				{
					new Color(199, 166, 129),
					new Color(127, 105, 81),
					new Color(128, 151, 43),
					new Color(193, 14, 7)
				},
				2426,
				new Color[]
				{
					new Color(246, 187, 165),
					new Color(255, 134, 86)
				},
				2427,
				new Color[]
				{
					new Color(235, 122, 128),
					new Color(216, 193, 186),
					new Color(252, 108, 40)
				},
				3195,
				new Color[]
				{
					new Color(139, 86, 218),
					new Color(218, 86, 104),
					new Color(218, 182, 86),
					new Color(36, 203, 185)
				},
				3532,
				new Color[]
				{
					new Color(218, 113, 90),
					new Color(183, 65, 68)
				},
				4009,
				new Color[]
				{
					new Color(221, 67, 87),
					new Color(255, 252, 217)
				},
				4011,
				new Color[]
				{
					new Color(224, 143, 91),
					new Color(214, 170, 105)
				},
				4012,
				new Color[]
				{
					new Color(255, 236, 184),
					new Color(242, 183, 236),
					new Color(215, 137, 122),
					new Color(242, 70, 88)
				},
				4013,
				new Color[]
				{
					new Color(216, 93, 61),
					new Color(159, 48, 28)
				},
				4014,
				new Color[]
				{
					new Color(216, 93, 61),
					new Color(205, 150, 71),
					new Color(123, 72, 27)
				},
				4015,
				new Color[]
				{
					new Color(197, 136, 85),
					new Color(143, 86, 59),
					new Color(100, 156, 58),
					new Color(216, 93, 61)
				},
				4016,
				new Color[]
				{
					new Color(241, 167, 70),
					new Color(215, 121, 64)
				},
				4017,
				new Color[]
				{
					new Color(200, 133, 84),
					new Color(141, 71, 19),
					new Color(103, 54, 18)
				},
				4019,
				new Color[]
				{
					new Color(248, 234, 196),
					new Color(121, 92, 18),
					new Color(128, 151, 43)
				},
				4020,
				new Color[]
				{
					new Color(237, 243, 248),
					new Color(255, 200, 82)
				},
				4021,
				new Color[]
				{
					new Color(255, 221, 119),
					new Color(241, 167, 70),
					new Color(215, 121, 64)
				},
				4022,
				new Color[]
				{
					new Color(255, 249, 181),
					new Color(203, 179, 73),
					new Color(216, 93, 61)
				},
				4023,
				new Color[]
				{
					new Color(189, 0, 107) * 0.5f,
					new Color(123, 0, 57) * 0.5f
				},
				4024,
				new Color[]
				{
					new Color(217, 134, 83),
					new Color(179, 80, 54)
				},
				4025,
				new Color[]
				{
					new Color(229, 114, 63),
					new Color(255, 184, 51),
					new Color(197, 136, 85)
				},
				4026,
				new Color[]
				{
					new Color(245, 247, 250),
					new Color(142, 96, 60),
					new Color(204, 209, 219),
					new Color(234, 85, 79)
				},
				4028,
				new Color[]
				{
					new Color(255, 250, 184),
					new Color(217, 123, 0),
					new Color(209, 146, 33)
				},
				4029,
				new Color[]
				{
					new Color(255, 250, 184),
					new Color(167, 57, 68),
					new Color(209, 146, 33),
					new Color(220, 185, 152)
				},
				4030,
				new Color[]
				{
					new Color(247, 237, 127),
					new Color(215, 187, 59),
					new Color(174, 139, 43)
				},
				4031,
				new Color[]
				{
					new Color(255, 198, 134),
					new Color(219, 109, 68),
					new Color(160, 83, 63)
				},
				4032,
				new Color[]
				{
					new Color(228, 152, 107),
					new Color(170, 81, 57),
					new Color(128, 49, 49)
				},
				4033,
				new Color[]
				{
					new Color(190, 226, 65),
					new Color(63, 69, 15),
					new Color(173, 50, 37)
				},
				4034,
				new Color[]
				{
					new Color(255, 134, 86),
					new Color(193, 57, 37),
					new Color(186, 155, 130),
					new Color(178, 206, 46)
				},
				4035,
				new Color[]
				{
					new Color(234, 244, 82),
					new Color(255, 182, 121),
					new Color(205, 89, 0),
					new Color(240, 157, 81)
				},
				4036,
				new Color[]
				{
					new Color(223, 207, 74),
					new Color(189, 158, 36),
					new Color(226, 45, 38),
					new Color(131, 9, 0)
				},
				4037,
				new Color[]
				{
					new Color(195, 109, 68),
					new Color(162, 69, 59),
					new Color(209, 194, 189)
				},
				4282,
				new Color[]
				{
					new Color(237, 169, 78),
					new Color(211, 106, 62)
				},
				4283,
				new Color[]
				{
					new Color(242, 235, 172),
					new Color(254, 247, 177),
					new Color(255, 230, 122)
				},
				4284,
				new Color[]
				{
					new Color(59, 81, 114),
					new Color(105, 62, 118),
					new Color(35, 22, 57)
				},
				4285,
				new Color[]
				{
					new Color(231, 115, 68),
					new Color(212, 42, 55),
					new Color(168, 16, 37)
				},
				4286,
				new Color[]
				{
					new Color(185, 27, 68),
					new Color(124, 17, 53)
				},
				4287,
				new Color[]
				{
					new Color(199, 163, 121),
					new Color(250, 245, 218),
					new Color(252, 250, 235)
				},
				4288,
				new Color[]
				{
					new Color(209, 44, 90),
					new Color(83, 83, 83),
					new Color(245, 245, 245),
					new Color(250, 250, 250)
				},
				4289,
				new Color[]
				{
					new Color(59, 81, 114),
					new Color(105, 62, 118),
					new Color(35, 22, 57)
				},
				4290,
				new Color[]
				{
					new Color(247, 178, 52),
					new Color(221, 60, 96),
					new Color(225, 83, 115)
				},
				4291,
				new Color[]
				{
					new Color(254, 231, 67),
					new Color(253, 239, 117)
				},
				4292,
				new Color[]
				{
					new Color(231, 121, 68),
					new Color(216, 139, 33),
					new Color(251, 220, 77)
				},
				4293,
				new Color[]
				{
					new Color(242, 153, 80),
					new Color(248, 208, 52)
				},
				4294,
				new Color[]
				{
					new Color(253, 208, 17),
					new Color(253, 239, 117)
				},
				4295,
				new Color[]
				{
					new Color(192, 47, 129),
					new Color(247, 178, 52),
					new Color(251, 220, 77)
				},
				4296,
				new Color[]
				{
					new Color(212, 42, 55),
					new Color(250, 245, 218),
					new Color(252, 250, 235)
				},
				4297,
				new Color[]
				{
					new Color(253, 239, 117),
					new Color(254, 247, 177)
				},
				4403,
				new Color[]
				{
					new Color(255, 134, 86),
					new Color(193, 57, 37),
					new Color(242, 202, 174),
					new Color(128, 151, 43)
				},
				4411,
				new Color[]
				{
					new Color(191, 157, 174),
					new Color(222, 196, 197)
				},
				4625,
				new Color[]
				{
					new Color(255, 230, 122),
					new Color(216, 69, 33),
					new Color(128, 151, 43)
				},
				5092,
				new Color[]
				{
					new Color(247, 178, 52),
					new Color(221, 60, 96),
					new Color(225, 83, 115)
				},
				5093,
				new Color[]
				{
					new Color(197, 136, 85),
					new Color(143, 86, 59),
					new Color(100, 156, 58),
					new Color(216, 93, 61),
					new Color(255, 200, 40),
					new Color(50, 160, 30)
				},
				5277,
				new Color[]
				{
					new Color(255, 204, 130),
					new Color(238, 102, 70),
					new Color(206, 47, 47)
				},
				5278,
				new Color[]
				{
					new Color(255, 204, 130),
					new Color(215, 67, 51),
					new Color(150, 30, 84)
				},
				5537,
				new Color[]
				{
					new Color(241, 163, 120),
					new Color(166, 87, 48),
					new Color(72, 28, 23)
				},
				5645,
				new Color[]
				{
					new Color(255, 255, 255),
					new Color(120, 207, 238),
					new Color(33, 184, 115),
					new Color(238, 200, 75),
					new Color(233, 115, 255),
					new Color(245, 46, 48)
				}
			});

			// Token: 0x04006E90 RID: 28304
			public static Color[][] DrinkParticleColors = ItemID.Sets.Factory.CreateCustomSet<Color[]>(new Color[0], new object[]
			{
				28,
				new Color[]
				{
					new Color(164, 16, 47),
					new Color(246, 34, 79),
					new Color(255, 95, 129)
				},
				110,
				new Color[]
				{
					new Color(16, 45, 152),
					new Color(11, 61, 245),
					new Color(93, 127, 255)
				},
				126,
				new Color[]
				{
					new Color(9, 61, 191),
					new Color(30, 84, 220),
					new Color(51, 107, 249)
				},
				188,
				new Color[]
				{
					new Color(164, 16, 47),
					new Color(246, 34, 79),
					new Color(255, 95, 129)
				},
				189,
				new Color[]
				{
					new Color(16, 45, 152),
					new Color(11, 61, 245),
					new Color(93, 127, 255)
				},
				226,
				new Color[]
				{
					new Color(200, 25, 116),
					new Color(229, 30, 202),
					new Color(254, 149, 210)
				},
				227,
				new Color[]
				{
					new Color(200, 25, 116),
					new Color(229, 30, 202),
					new Color(254, 149, 210)
				},
				288,
				new Color[]
				{
					new Color(58, 48, 102),
					new Color(90, 72, 168),
					new Color(132, 116, 199)
				},
				289,
				new Color[]
				{
					new Color(174, 13, 97),
					new Color(255, 156, 209),
					new Color(255, 56, 162)
				},
				290,
				new Color[]
				{
					new Color(83, 137, 13),
					new Color(100, 164, 16),
					new Color(134, 230, 10)
				},
				291,
				new Color[]
				{
					new Color(13, 74, 137),
					new Color(16, 89, 164),
					new Color(10, 119, 230)
				},
				292,
				new Color[]
				{
					new Color(164, 159, 16),
					new Color(230, 222, 10),
					new Color(255, 252, 159)
				},
				293,
				new Color[]
				{
					new Color(137, 13, 86),
					new Color(230, 10, 139),
					new Color(255, 144, 210)
				},
				294,
				new Color[]
				{
					new Color(66, 13, 137),
					new Color(103, 10, 230),
					new Color(163, 95, 255)
				},
				295,
				new Color[]
				{
					new Color(13, 106, 137),
					new Color(10, 176, 230),
					new Color(146, 229, 255)
				},
				296,
				new Color[]
				{
					new Color(146, 102, 14),
					new Color(225, 185, 22),
					new Color(250, 213, 64)
				},
				297,
				new Color[]
				{
					new Color(9, 101, 110),
					new Color(15, 164, 177),
					new Color(34, 229, 246)
				},
				298,
				new Color[]
				{
					new Color(133, 137, 13),
					new Color(222, 230, 10),
					new Color(252, 254, 161)
				},
				299,
				new Color[]
				{
					new Color(92, 137, 13),
					new Color(121, 184, 11),
					new Color(189, 255, 73)
				},
				300,
				new Color[]
				{
					new Color(81, 60, 120),
					new Color(127, 96, 184),
					new Color(165, 142, 208)
				},
				301,
				new Color[]
				{
					new Color(112, 137, 13),
					new Color(163, 202, 7),
					new Color(204, 246, 34)
				},
				302,
				new Color[]
				{
					new Color(12, 63, 131),
					new Color(16, 79, 164),
					new Color(34, 124, 246)
				},
				303,
				new Color[]
				{
					new Color(164, 96, 16),
					new Color(230, 129, 10),
					new Color(255, 200, 134)
				},
				304,
				new Color[]
				{
					new Color(137, 63, 13),
					new Color(197, 87, 13),
					new Color(230, 98, 10)
				},
				305,
				new Color[]
				{
					new Color(69, 13, 131),
					new Color(134, 34, 246),
					new Color(170, 95, 255)
				},
				499,
				new Color[]
				{
					new Color(164, 16, 47),
					new Color(246, 34, 79),
					new Color(255, 95, 129)
				},
				500,
				new Color[]
				{
					new Color(16, 45, 152),
					new Color(11, 61, 245),
					new Color(93, 127, 255)
				},
				678,
				new Color[]
				{
					new Color(254, 0, 38),
					new Color(199, 29, 15)
				},
				967,
				new Color[]
				{
					new Color(221, 226, 229),
					new Color(180, 189, 194)
				},
				969,
				new Color[]
				{
					new Color(150, 99, 69),
					new Color(219, 170, 132),
					new Color(251, 244, 240)
				},
				1134,
				new Color[]
				{
					new Color(235, 144, 10),
					new Color(254, 194, 20),
					new Color(254, 246, 37)
				},
				1340,
				new Color[]
				{
					new Color(151, 79, 162),
					new Color(185, 128, 193),
					new Color(240, 185, 217)
				},
				1353,
				new Color[]
				{
					new Color(77, 227, 45),
					new Color(218, 253, 9),
					new Color(96, 248, 2)
				},
				1354,
				new Color[]
				{
					new Color(235, 36, 1),
					new Color(255, 127, 39),
					new Color(255, 203, 83)
				},
				1355,
				new Color[]
				{
					new Color(148, 126, 24),
					new Color(233, 207, 137),
					new Color(255, 249, 183)
				},
				1356,
				new Color[]
				{
					new Color(253, 152, 0),
					new Color(254, 202, 80),
					new Color(255, 251, 166)
				},
				1357,
				new Color[]
				{
					new Color(106, 107, 134),
					new Color(118, 134, 207),
					new Color(120, 200, 226)
				},
				1358,
				new Color[]
				{
					new Color(202, 0, 147),
					new Color(255, 66, 222),
					new Color(255, 170, 253)
				},
				1359,
				new Color[]
				{
					new Color(45, 174, 76),
					new Color(112, 218, 138),
					new Color(182, 236, 195)
				},
				1977,
				new Color[]
				{
					new Color(221, 0, 0),
					new Color(146, 17, 17),
					new Color(51, 21, 21)
				},
				1978,
				new Color[]
				{
					new Color(24, 92, 248),
					new Color(97, 112, 169),
					new Color(228, 228, 228)
				},
				1979,
				new Color[]
				{
					new Color(128, 128, 128),
					new Color(151, 107, 75),
					new Color(13, 101, 36),
					new Color(28, 216, 94)
				},
				1980,
				new Color[]
				{
					new Color(122, 92, 10),
					new Color(185, 164, 23),
					new Color(241, 227, 75)
				},
				1981,
				new Color[]
				{
					new Color(255, 186, 0),
					new Color(87, 20, 170)
				},
				1982,
				new Color[]
				{
					new Color(218, 183, 59),
					new Color(59, 218, 85),
					new Color(59, 149, 218),
					new Color(218, 59, 59)
				},
				1983,
				new Color[]
				{
					new Color(208, 80, 80),
					new Color(109, 106, 174),
					new Color(143, 215, 29),
					new Color(30, 150, 72)
				},
				1984,
				new Color[]
				{
					new Color(255, 9, 172),
					new Color(219, 4, 121),
					new Color(111, 218, 171),
					new Color(72, 175, 130)
				},
				1985,
				new Color[]
				{
					new Color(176, 101, 239),
					new Color(101, 238, 239),
					new Color(221, 0, 0),
					new Color(62, 235, 137)
				},
				1986,
				new Color[]
				{
					new Color(55, 246, 211),
					new Color(20, 223, 168),
					new Color(0, 181, 128)
				},
				1990,
				new Color[]
				{
					new Color(254, 254, 254),
					new Color(214, 232, 240),
					new Color(234, 242, 246)
				},
				2209,
				new Color[]
				{
					new Color(16, 45, 152),
					new Color(11, 61, 245),
					new Color(93, 127, 255)
				},
				2322,
				new Color[]
				{
					new Color(55, 92, 95),
					new Color(84, 149, 154),
					new Color(149, 196, 200)
				},
				2323,
				new Color[]
				{
					new Color(91, 8, 106),
					new Color(184, 9, 131),
					new Color(250, 64, 188)
				},
				2324,
				new Color[]
				{
					new Color(21, 40, 138),
					new Color(102, 101, 201),
					new Color(122, 147, 196)
				},
				2325,
				new Color[]
				{
					new Color(100, 67, 50),
					new Color(141, 93, 68),
					new Color(182, 126, 97)
				},
				2326,
				new Color[]
				{
					new Color(159, 224, 124),
					new Color(92, 175, 46),
					new Color(51, 95, 27)
				},
				2327,
				new Color[]
				{
					new Color(95, 194, 255),
					new Color(12, 109, 167),
					new Color(13, 76, 115)
				},
				2328,
				new Color[]
				{
					new Color(215, 241, 109),
					new Color(150, 178, 31),
					new Color(105, 124, 25)
				},
				2329,
				new Color[]
				{
					new Color(251, 105, 29),
					new Color(220, 73, 4),
					new Color(140, 33, 10)
				},
				2344,
				new Color[]
				{
					new Color(166, 166, 166),
					new Color(255, 186, 0),
					new Color(165, 58, 0)
				},
				2345,
				new Color[]
				{
					new Color(239, 17, 0),
					new Color(209, 15, 0),
					new Color(136, 9, 0)
				},
				2346,
				new Color[]
				{
					new Color(156, 157, 153),
					new Color(99, 99, 99),
					new Color(63, 62, 58)
				},
				2347,
				new Color[]
				{
					new Color(243, 11, 11),
					new Color(255, 188, 55),
					new Color(252, 136, 58)
				},
				2348,
				new Color[]
				{
					new Color(255, 227, 0),
					new Color(255, 135, 0),
					new Color(226, 56, 0)
				},
				2349,
				new Color[]
				{
					new Color(120, 36, 30),
					new Color(216, 73, 63),
					new Color(233, 125, 117)
				},
				2351,
				new Color[]
				{
					new Color(255, 95, 252),
					new Color(147, 0, 240),
					new Color(67, 0, 150)
				},
				2354,
				new Color[]
				{
					new Color(117, 233, 164),
					new Color(40, 199, 103),
					new Color(30, 120, 66)
				},
				2355,
				new Color[]
				{
					new Color(217, 254, 161),
					new Color(69, 110, 9),
					new Color(135, 219, 11)
				},
				2356,
				new Color[]
				{
					new Color(233, 175, 117),
					new Color(199, 120, 40),
					new Color(143, 89, 36)
				},
				2359,
				new Color[]
				{
					new Color(255, 51, 0),
					new Color(248, 184, 0),
					new Color(255, 215, 0)
				},
				2756,
				new Color[]
				{
					new Color(178, 236, 255),
					new Color(92, 214, 255),
					new Color(184, 96, 163),
					new Color(255, 78, 178)
				},
				2863,
				new Color[]
				{
					new Color(97, 199, 224),
					new Color(98, 152, 177),
					new Color(26, 232, 249)
				},
				3001,
				new Color[]
				{
					new Color(104, 25, 103),
					new Color(155, 32, 154),
					new Color(190, 138, 223)
				},
				3259,
				new Color[]
				{
					new Color(40, 20, 66),
					new Color(186, 68, 255),
					new Color(26, 8, 49),
					new Color(255, 122, 255)
				},
				3544,
				new Color[]
				{
					new Color(164, 16, 47),
					new Color(246, 34, 79),
					new Color(255, 95, 129)
				},
				353,
				new Color[]
				{
					new Color(205, 152, 2) * 0.5f,
					new Color(240, 208, 88) * 0.5f,
					new Color(251, 243, 215) * 0.5f
				},
				1912,
				new Color[]
				{
					new Color(237, 159, 85),
					new Color(255, 228, 133),
					new Color(149, 97, 45)
				},
				2266,
				new Color[]
				{
					new Color(233, 233, 218) * 0.3f
				},
				4018,
				new Color[]
				{
					new Color(214, 170, 105) * 0.5f,
					new Color(180, 132, 73) * 0.5f
				},
				4027,
				new Color[]
				{
					new Color(242, 183, 236),
					new Color(245, 242, 193),
					new Color(226, 133, 217),
					new Color(242, 70, 88)
				},
				4477,
				new Color[]
				{
					new Color(161, 192, 220),
					new Color(143, 154, 201)
				},
				4478,
				new Color[]
				{
					new Color(40, 60, 70),
					new Color(26, 27, 36)
				},
				4479,
				new Color[]
				{
					new Color(224, 0, 152),
					new Color(137, 13, 126)
				},
				4614,
				new Color[]
				{
					new Color(153, 62, 2),
					new Color(208, 166, 59)
				},
				4615,
				new Color[]
				{
					new Color(164, 88, 178),
					new Color(124, 64, 152)
				},
				4616,
				new Color[]
				{
					new Color(255, 245, 109),
					new Color(235, 210, 89)
				},
				4617,
				new Color[]
				{
					new Color(245, 247, 250),
					new Color(255, 250, 133)
				},
				4618,
				new Color[]
				{
					new Color(255, 175, 133),
					new Color(237, 93, 85)
				},
				4619,
				new Color[]
				{
					new Color(247, 245, 224),
					new Color(232, 214, 179)
				},
				4620,
				new Color[]
				{
					new Color(181, 215, 0),
					new Color(255, 112, 4)
				},
				4621,
				new Color[]
				{
					new Color(242, 134, 81),
					new Color(153, 2, 42)
				},
				4622,
				new Color[]
				{
					new Color(90, 62, 123),
					new Color(59, 49, 104)
				},
				4623,
				new Color[]
				{
					new Color(255, 175, 152),
					new Color(147, 255, 228),
					new Color(231, 247, 150)
				},
				4624,
				new Color[]
				{
					new Color(155, 0, 67),
					new Color(208, 124, 59)
				},
				5009,
				new Color[]
				{
					new Color(210, 130, 10),
					new Color(255, 195, 20)
				},
				5041,
				new Color[]
				{
					new Color(221, 226, 229),
					new Color(180, 189, 194)
				},
				5042,
				new Color[]
				{
					new Color(70, 43, 21),
					new Color(142, 96, 60)
				},
				5275,
				new Color[]
				{
					new Color(70, 43, 21),
					new Color(142, 96, 60)
				},
				5496,
				new Color[]
				{
					new Color(164, 16, 47),
					new Color(246, 34, 79),
					new Color(255, 95, 129)
				},
				5211,
				new Color[]
				{
					new Color(255, 206, 237),
					new Color(253, 160, 197),
					new Color(251, 126, 73)
				}
			});

			// Token: 0x04006E91 RID: 28305
			private static ItemID.BannerEffect DD2BannerEffect = ItemID.BannerEffect.Reduced;

			// Token: 0x04006E92 RID: 28306
			public static ItemID.BannerEffect[] BannerStrength = ItemID.Sets.Factory.CreateCustomSet<ItemID.BannerEffect>(new ItemID.BannerEffect(1f), new object[]
			{
				3838,
				ItemID.Sets.DD2BannerEffect,
				3845,
				ItemID.Sets.DD2BannerEffect,
				3837,
				ItemID.Sets.DD2BannerEffect,
				3844,
				ItemID.Sets.DD2BannerEffect,
				3843,
				ItemID.Sets.DD2BannerEffect,
				3839,
				ItemID.Sets.DD2BannerEffect,
				3840,
				ItemID.Sets.DD2BannerEffect,
				3842,
				ItemID.Sets.DD2BannerEffect,
				3841,
				ItemID.Sets.DD2BannerEffect,
				3846,
				ItemID.Sets.DD2BannerEffect
			});

			// Token: 0x04006E93 RID: 28307
			public static int DefaultKillsForBannerNeeded = 50;

			// Token: 0x04006E94 RID: 28308
			public static int[] KillsToBanner = ItemID.Sets.Factory.CreateIntSet(ItemID.Sets.DefaultKillsForBannerNeeded, new int[]
			{
				3838,
				1000,
				3845,
				200,
				3837,
				500,
				3844,
				200,
				3843,
				50,
				3839,
				150,
				3840,
				100,
				3842,
				200,
				3841,
				100,
				3846,
				50,
				2971,
				150,
				2982,
				150,
				2931,
				100,
				2961,
				100,
				2994,
				100,
				2985,
				10,
				4541,
				10,
				2969,
				10,
				2986,
				10,
				2915,
				10,
				4602,
				10,
				4542,
				25,
				4543,
				25,
				4546,
				25,
				4545,
				25,
				2901,
				25,
				2902,
				25,
				1631,
				25,
				2913,
				25,
				4688,
				25,
				3390,
				25,
				4973,
				25,
				4974,
				25,
				4975,
				25,
				2934,
				25,
				1670,
				25,
				1694,
				25,
				2958,
				25,
				2960,
				25,
				3441,
				25,
				3780,
				25,
				3397,
				25,
				3403,
				25
			});

			// Token: 0x04006E95 RID: 28309
			public static bool[] CanFishInLava = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				2422
			});

			// Token: 0x04006E96 RID: 28310
			public static bool[] IsLavaBait = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				4849,
				4845,
				4847
			});

			// Token: 0x04006E97 RID: 28311
			public static bool[] FakeFishInanimate = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				2423
			});

			// Token: 0x04006E98 RID: 28312
			public static bool[] IsBasicFish = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				2303,
				2299,
				2290,
				2436,
				2317,
				2305,
				2304,
				2313,
				2318,
				2312,
				4401,
				2306,
				2308,
				2437,
				2319,
				2314,
				2302,
				2315,
				2438,
				2307,
				2310,
				2301,
				4402,
				2298,
				2316,
				2309,
				2321,
				2297,
				2300,
				2311
			});

			// Token: 0x04006E99 RID: 28313
			public static bool[] IsFishingCrate = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				2334,
				2335,
				2336,
				3203,
				3204,
				3205,
				3206,
				3207,
				3208,
				4405,
				4407,
				4877,
				5002,
				3979,
				3980,
				3981,
				3982,
				3983,
				3984,
				3985,
				3986,
				3987,
				4406,
				4408,
				4878,
				5003
			});

			// Token: 0x04006E9A RID: 28314
			public static bool[] IsFishingCrateHardmode = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3979,
				3980,
				3981,
				3982,
				3983,
				3984,
				3985,
				3986,
				3987,
				4406,
				4408,
				4878,
				5003
			});

			// Token: 0x04006E9B RID: 28315
			public static bool[] IsAVoiceChangeItem = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				5507,
				5502,
				5485,
				5484,
				5506,
				5503,
				5534,
				5499,
				5500,
				5505,
				5501,
				5504,
				5508,
				5509,
				215
			});

			// Token: 0x04006E9C RID: 28316
			public static bool[] CanBePlacedOnWeaponRacks = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3196,
				166,
				235,
				3115,
				167,
				2896,
				3547,
				580,
				937,
				4423,
				4824,
				4825,
				4826,
				4827,
				4908,
				4909,
				5594,
				5595,
				4094,
				4039,
				4092,
				4093,
				4587,
				4588,
				4589,
				4590,
				4591,
				4592,
				4593,
				4594,
				4595,
				4596,
				4597,
				4598,
				905,
				1326,
				5335,
				2272,
				2610,
				5495,
				3384,
				3225,
				2303,
				2299,
				2290,
				2317,
				2305,
				2304,
				2313,
				2318,
				2312,
				2306,
				2308,
				2319,
				2314,
				2302,
				2315,
				2307,
				2310,
				2301,
				2298,
				2316,
				2309,
				2321,
				2297,
				2300,
				2311,
				2420,
				2438,
				2437,
				2436,
				4401,
				4402,
				2475,
				2476,
				2450,
				2477,
				2478,
				2451,
				2479,
				2480,
				2452,
				2453,
				2481,
				2454,
				2482,
				2483,
				2455,
				2456,
				2457,
				2458,
				2459,
				2460,
				2484,
				2472,
				2461,
				2462,
				2463,
				2485,
				2464,
				2465,
				2486,
				2466,
				2467,
				2468,
				2487,
				2469,
				2488,
				2470,
				2471,
				2473,
				2474,
				4393,
				4394
			});

			// Token: 0x04006E9D RID: 28317
			public static bool[] IsQuestFish = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				2475,
				2476,
				2450,
				2477,
				2478,
				2451,
				2479,
				2480,
				2452,
				2453,
				2481,
				2454,
				2482,
				2483,
				2455,
				2456,
				2457,
				2458,
				2459,
				2460,
				2484,
				2472,
				2461,
				2462,
				2463,
				2485,
				2464,
				2465,
				2486,
				2466,
				2467,
				2468,
				2487,
				2469,
				2488,
				2470,
				2471,
				2473,
				2474,
				4393,
				4394
			});

			// Token: 0x04006E9E RID: 28318
			public static int[] TextureCopyLoad = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				3665,
				48,
				3666,
				306,
				3667,
				328,
				3668,
				625,
				3669,
				626,
				3670,
				627,
				3671,
				680,
				3672,
				681,
				3673,
				831,
				3674,
				838,
				3675,
				914,
				3676,
				952,
				3677,
				1142,
				3678,
				1298,
				3679,
				1528,
				3680,
				1529,
				3681,
				1530,
				3682,
				1531,
				3683,
				1532,
				3684,
				2230,
				3685,
				2249,
				3686,
				2250,
				3687,
				2526,
				3688,
				2544,
				3689,
				2559,
				3690,
				2574,
				3691,
				2612,
				3692,
				2613,
				3693,
				2614,
				3694,
				2615,
				3695,
				2616,
				3696,
				2617,
				3697,
				2618,
				3698,
				2619,
				3699,
				2620,
				3700,
				2748,
				3701,
				2814,
				3703,
				3125,
				3702,
				3180,
				3704,
				3181,
				3705,
				3665,
				3706,
				3665,
				4713,
				4712,
				5167,
				5156,
				5188,
				5177,
				5209,
				5198,
				5567,
				5556,
				5620,
				5609,
				5708,
				5697,
				5731,
				5720,
				5754,
				5745,
				5776,
				5763,
				5797,
				5784,
				5818,
				5805,
				5839,
				5826,
				5857,
				5846,
				5878,
				5865,
				5897,
				5886,
				5918,
				5905,
				5952,
				5939,
				5974,
				5962,
				5995,
				5982,
				6018,
				6005,
				6041,
				6028,
				6064,
				6051,
				6087,
				6074,
				6131,
				6118
			});

			// Token: 0x04006E9F RID: 28319
			public static bool[] TrapSigned = ItemID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				3665,
				3666,
				3667,
				3668,
				3669,
				3670,
				3671,
				3672,
				3673,
				3674,
				3675,
				3676,
				3677,
				3678,
				3679,
				3680,
				3681,
				3682,
				3683,
				3684,
				3685,
				3686,
				3687,
				3688,
				3689,
				3690,
				3691,
				3692,
				3693,
				3694,
				3695,
				3696,
				3697,
				3698,
				3699,
				3700,
				3701,
				3703,
				3702,
				3704,
				3705,
				3706,
				3886,
				3887,
				3950,
				3976,
				4164,
				4185,
				4206,
				4227,
				4266,
				4268,
				4585,
				4713,
				5167,
				5188,
				5209,
				5567,
				5620,
				5708,
				5731,
				5754,
				5776,
				5797,
				5818,
				5839,
				5857,
				5878,
				5897,
				5918,
				5952,
				5974,
				5995,
				6018,
				6041,
				6064,
				6087,
				6131
			});

			// Token: 0x04006EA0 RID: 28320
			public static bool[] Deprecated = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				2783,
				2785,
				2782,
				2773,
				2775,
				2772,
				2778,
				2780,
				2777,
				3463,
				3465,
				3462,
				2881,
				3847,
				3848,
				3849,
				3850,
				3851,
				3850,
				3861,
				3862,
				4010,
				4058,
				4722,
				3978
			});

			// Token: 0x04006EA1 RID: 28321
			public static bool[] NeverAppearsAsNewInInventory = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				71,
				72,
				73,
				74
			});

			// Token: 0x04006EA2 RID: 28322
			public static bool[] CommonCoin = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				71,
				72,
				73,
				74
			});

			// Token: 0x04006EA3 RID: 28323
			public static bool[] ItemIconPulse = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				520,
				521,
				575,
				549,
				548,
				547,
				3456,
				3457,
				3458,
				3459,
				3580,
				3581
			});

			// Token: 0x04006EA4 RID: 28324
			public static bool[] ItemNoGravity = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				520,
				521,
				575,
				549,
				548,
				547,
				3453,
				3454,
				3455,
				3456,
				3457,
				3458,
				3459,
				3580,
				3581,
				4143
			});

			// Token: 0x04006EA5 RID: 28325
			public static int[] ExtractinatorMode = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				424,
				0,
				1103,
				0,
				3347,
				1,
				2339,
				2,
				2338,
				2,
				2337,
				2,
				4354,
				3,
				4389,
				3,
				4377,
				3,
				4378,
				3,
				5127,
				3,
				5128,
				3,
				5395,
				4,
				1124,
				5,
				4090,
				6,
				173,
				6
			});

			// Token: 0x04006EA6 RID: 28326
			public static int[] StaffMinionSlotsRequired = ItemID.Sets.Factory.CreateIntSet(1, new int[0]);

			// Token: 0x04006EA7 RID: 28327
			public static bool[] ExoticPlantsForDyeTrade = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3385,
				3386,
				3387,
				3388
			});

			// Token: 0x04006EA8 RID: 28328
			public static bool[] NebulaPickup = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3453,
				3454,
				3455
			});

			// Token: 0x04006EA9 RID: 28329
			public static bool[] AnimatesAsSoul = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				575,
				547,
				520,
				548,
				521,
				549,
				3580,
				3581
			});

			// Token: 0x04006EAA RID: 28330
			public static bool[] ErrorWorldChestSwapImmunity = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				0,
				1156,
				1571,
				1569,
				1260,
				1572,
				4607
			});

			// Token: 0x04006EAB RID: 28331
			public static bool[] gunProj = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3475,
				3540,
				3854,
				3930
			});

			// Token: 0x04006EAC RID: 28332
			public static int[] SortingPriorityMiscAcorns = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				5518,
				1,
				5438,
				2,
				4857,
				3,
				4856,
				4,
				4855,
				5,
				4854,
				6,
				4853,
				7,
				4851,
				8,
				4852,
				9,
				4907,
				10,
				4871,
				11,
				27,
				12
			});

			// Token: 0x04006EAD RID: 28333
			public static int[] SortingPriorityMiscGems = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				999,
				1,
				182,
				2,
				178,
				3,
				179,
				4,
				177,
				5,
				180,
				6,
				181,
				7
			});

			// Token: 0x04006EAE RID: 28334
			public static int[] SortingPriorityMiscHerbsAndSeeds = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				276,
				1,
				318,
				2,
				312,
				3,
				316,
				4,
				310,
				5,
				2358,
				6,
				2357,
				7,
				314,
				8,
				308,
				9,
				313,
				10,
				307,
				11,
				317,
				12,
				311,
				13,
				315,
				14,
				309,
				15
			});

			// Token: 0x04006EAF RID: 28335
			public static int[] SortingPriorityMiscBossBags = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				3318,
				1,
				3319,
				2,
				3320,
				3,
				3321,
				4,
				3323,
				5,
				3322,
				6,
				5111,
				7,
				3324,
				8,
				4957,
				9,
				3325,
				10,
				3326,
				11,
				3327,
				12,
				3328,
				13,
				3329,
				14,
				3330,
				15,
				4782,
				16,
				3331,
				17,
				3332,
				18,
				3861,
				19,
				3862,
				20,
				3860,
				21
			});

			// Token: 0x04006EB0 RID: 28336
			public static int[] SortingPriorityToolsInstruments = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				508,
				1,
				494,
				2,
				507,
				3,
				1305,
				4,
				4057,
				5,
				4372,
				6,
				4715,
				7,
				4673,
				8,
				4063,
				9
			});

			// Token: 0x04006EB1 RID: 28337
			public static int[] SortingPriorityToolsFishing = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				4608,
				1
			});

			// Token: 0x04006EB2 RID: 28338
			public static int[] SortingPriorityToolsGolf = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				4595,
				1,
				4598,
				2,
				4597,
				3,
				4596,
				4,
				4591,
				5,
				4594,
				6,
				4593,
				7,
				4592,
				8,
				4039,
				9,
				4094,
				10,
				4093,
				11,
				4092,
				12,
				4587,
				13,
				4590,
				14,
				4589,
				15,
				4588,
				16,
				4095,
				17
			});

			// Token: 0x04006EB3 RID: 28339
			public static int[] SortingPriorityToolsKites = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				4367,
				1,
				4368,
				2,
				4369,
				3,
				4370,
				4,
				4371,
				5,
				4379,
				6,
				4610,
				7,
				4611,
				8,
				4612,
				9,
				4613,
				10,
				4648,
				11,
				4649,
				12,
				4650,
				13,
				4651,
				14,
				4669,
				15,
				4670,
				16,
				4671,
				17,
				4674,
				18,
				4675,
				19,
				4676,
				20,
				4677,
				21,
				4681,
				22,
				4683,
				23,
				4684,
				24
			});

			// Token: 0x04006EB4 RID: 28340
			public static int[] SortingPriorityToolsKeys = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				327,
				1,
				329,
				2,
				3092,
				3,
				3091,
				4,
				1141,
				5,
				1533,
				6,
				4714,
				7,
				1537,
				8,
				1534,
				9,
				1535,
				10,
				1536,
				11
			});

			// Token: 0x04006EB5 RID: 28341
			public static int[] SortingPriorityToolsMisc = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				1326,
				1,
				5335,
				2,
				3384,
				3,
				1299,
				4,
				2272,
				5,
				2610,
				6,
				5495,
				7,
				1000,
				8,
				1450,
				9,
				114,
				10,
				5451,
				11,
				5738,
				12,
				5644,
				13,
				3213,
				20,
				4131,
				21,
				5325,
				22,
				4767,
				23,
				5453,
				24,
				5309,
				25,
				5454,
				26,
				5323,
				27,
				5455,
				28,
				4346,
				29,
				5391,
				30,
				282,
				40,
				286,
				41,
				3112,
				42,
				3002,
				43,
				4776,
				44,
				5643,
				45,
				5574,
				51,
				5575,
				52,
				5576,
				53
			});

			// Token: 0x04006EB6 RID: 28342
			public static int[] SortingPriorityWeaponsRanged = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				905,
				1
			});

			// Token: 0x04006EB7 RID: 28343
			public static int[] SortingPriorityPotionsBuffs = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				678,
				1,
				2350,
				2,
				2997,
				3,
				4870,
				4,
				2351,
				5,
				2756,
				6,
				2352,
				7,
				2353,
				8
			});

			// Token: 0x04006EB8 RID: 28344
			public static int[] SortingPriorityPotionsDyeMaterial = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				1115,
				1,
				1114,
				2,
				1110,
				3,
				1112,
				4,
				1108,
				5,
				1107,
				6,
				1116,
				7,
				1109,
				8,
				1111,
				9,
				1118,
				10,
				1117,
				11,
				1113,
				12,
				1119,
				13
			});

			// Token: 0x04006EB9 RID: 28345
			public static int[] SortingPriorityMiscImportants = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				43,
				1,
				560,
				2,
				70,
				3,
				1331,
				3,
				361,
				4,
				5120,
				5,
				1133,
				5,
				4988,
				6,
				5334,
				7,
				544,
				8,
				556,
				9,
				557,
				10,
				1315,
				11,
				2673,
				12,
				602,
				13,
				1844,
				14,
				1958,
				15,
				1293,
				16,
				2767,
				17,
				4271,
				18,
				3601,
				19,
				1291,
				20,
				109,
				21,
				29,
				22,
				50,
				23,
				3199,
				24,
				3124,
				25,
				5437,
				26,
				5358,
				27,
				5359,
				28,
				5360,
				29,
				5361,
				30,
				4263,
				31,
				4819,
				32,
				5342,
				50,
				5341,
				51,
				5340,
				52,
				5339,
				53,
				5338,
				54,
				5337,
				55,
				4910,
				56,
				4830,
				57,
				4829,
				58,
				5343,
				59,
				5336,
				60,
				4382,
				61,
				5326,
				62,
				5043,
				63,
				5289,
				64,
				3335,
				65
			});

			// Token: 0x04006EBA RID: 28346
			public static int[] SortingPriorityWiring = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				510,
				103,
				3625,
				102,
				509,
				101,
				851,
				100,
				850,
				99,
				3612,
				98,
				849,
				97,
				4485,
				96,
				4484,
				95,
				583,
				94,
				584,
				93,
				585,
				92,
				538,
				91,
				513,
				90,
				3545,
				90,
				853,
				89,
				541,
				88,
				529,
				88,
				1151,
				87,
				852,
				87,
				543,
				87,
				542,
				87,
				3707,
				87,
				2492,
				86,
				530,
				85,
				581,
				84,
				582,
				84,
				1263,
				83
			});

			// Token: 0x04006EBB RID: 28347
			public static int[] SortingPriorityMaterials = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				3467,
				101,
				3460,
				100,
				3458,
				99,
				3456,
				98,
				3457,
				97,
				3459,
				96,
				3261,
				95,
				1508,
				94,
				1552,
				93,
				1006,
				92,
				947,
				91,
				1225,
				90,
				1198,
				89,
				1106,
				88,
				391,
				87,
				366,
				86,
				1191,
				85,
				1105,
				84,
				382,
				83,
				365,
				82,
				1184,
				81,
				1104,
				80,
				381,
				79,
				364,
				78,
				548,
				77,
				547,
				76,
				549,
				75,
				575,
				74,
				521,
				73,
				520,
				72,
				175,
				71,
				173,
				70,
				174,
				69,
				3380,
				68,
				1329,
				67,
				1257,
				66,
				880,
				65,
				86,
				64,
				57,
				63,
				56,
				62,
				117,
				61,
				116,
				60,
				706,
				59,
				702,
				58,
				19,
				57,
				13,
				56,
				705,
				55,
				701,
				54,
				21,
				53,
				14,
				52,
				704,
				51,
				700,
				50,
				22,
				49,
				11,
				48,
				703,
				47,
				699,
				46,
				20,
				45,
				12,
				44
			});

			// Token: 0x04006EBC RID: 28348
			public static int[] SortingPriorityExtractibles = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				997,
				4,
				3347,
				3,
				1103,
				2,
				424,
				1
			});

			// Token: 0x04006EBD RID: 28349
			public static int[] SortingPriorityRopes = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				965,
				1,
				85,
				1,
				2996,
				1,
				3077,
				1,
				3078,
				1
			});

			// Token: 0x04006EBE RID: 28350
			public static int[] SortingPriorityPainting = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				1543,
				100,
				1544,
				99,
				1545,
				98,
				1071,
				97,
				1072,
				96,
				1100,
				95
			});

			// Token: 0x04006EBF RID: 28351
			public static int[] SortingPriorityTerraforming = ItemID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				5134,
				100,
				779,
				99,
				780,
				98,
				783,
				97,
				781,
				96,
				782,
				95,
				784,
				94,
				5392,
				93,
				5393,
				92,
				5394,
				91,
				422,
				90,
				423,
				89,
				3477,
				88,
				66,
				67,
				67,
				86,
				2886,
				85,
				5304,
				84,
				5303,
				83,
				4872,
				82,
				3032,
				81,
				5364,
				80,
				5302,
				79,
				1128,
				78,
				4820,
				77,
				207,
				76,
				3031,
				75,
				206,
				74,
				205,
				73,
				5542,
				72,
				4909,
				71,
				4908,
				70,
				4827,
				69,
				4826,
				68,
				4825,
				67,
				4824,
				66,
				580,
				64,
				5595,
				63,
				5594,
				62,
				3547,
				61,
				2896,
				60,
				167,
				59,
				4423,
				58,
				3196,
				57,
				3115,
				56,
				235,
				55,
				166,
				54
			});

			// Token: 0x04006EC0 RID: 28352
			public static int[] GamepadExtraRange = ItemID.Sets.Factory.CreateIntSet(0, new int[]
			{
				2797,
				20,
				3278,
				4,
				3285,
				6,
				3279,
				8,
				3280,
				8,
				3281,
				9,
				3262,
				10,
				3317,
				10,
				5294,
				10,
				3282,
				10,
				3315,
				10,
				3316,
				11,
				3283,
				12,
				3290,
				13,
				3289,
				11,
				3284,
				13,
				3286,
				13,
				3287,
				18,
				3288,
				18,
				3291,
				17,
				3292,
				18,
				3389,
				21
			});

			// Token: 0x04006EC1 RID: 28353
			public static bool[] GamepadWholeScreenUseRange = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				1326,
				5335,
				1256,
				1244,
				3014,
				113,
				218,
				495,
				114,
				496,
				65,
				1931,
				3570,
				2750,
				3065,
				3029,
				3030,
				4381,
				4956,
				5669,
				5065,
				2796,
				494,
				3006,
				4715,
				1309,
				2364,
				2365,
				2551,
				2535,
				2584,
				1157,
				2749,
				1802,
				2621,
				3249,
				3531,
				3474,
				2366,
				1572,
				3569,
				3571,
				4269,
				4273,
				4281,
				5119,
				5456,
				5463,
				5663,
				5664,
				3611,
				1299,
				1254
			});

			// Token: 0x04006EC2 RID: 28354
			public static float[] BonusMeleeSpeedMultiplier = ItemID.Sets.Factory.CreateFloatSet(1f, new float[]
			{
				1827f,
				0.5f,
				3013f,
				0.25f,
				3106f,
				0.33f,
				757f,
				0.75f
			});

			// Token: 0x04006EC3 RID: 28355
			public static bool[] GamepadSmartQuickReach = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				2798,
				2797,
				3030,
				3262,
				3278,
				3279,
				3280,
				3281,
				3282,
				3283,
				3284,
				3285,
				3286,
				3287,
				3288,
				3289,
				3290,
				3291,
				3292,
				3315,
				3316,
				3317,
				5294,
				3389,
				2798,
				65,
				1931,
				3570,
				2750,
				3065,
				3029,
				4956,
				5669,
				5065,
				1256,
				1244,
				3014,
				113,
				218,
				495
			});

			// Token: 0x04006EC4 RID: 28356
			public static bool[] Yoyo = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3262,
				3278,
				3279,
				3280,
				3281,
				3282,
				3283,
				3284,
				3285,
				3286,
				3287,
				3288,
				3289,
				3290,
				3291,
				3292,
				3315,
				3316,
				3317,
				3389,
				5294
			});

			// Token: 0x04006EC5 RID: 28357
			public static bool[] AlsoABuildingItem = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3031,
				205,
				1128,
				207,
				206,
				3032,
				849,
				3620,
				509,
				851,
				850,
				3625,
				510,
				1071,
				1543,
				1072,
				1544,
				1100,
				1545,
				4820,
				4872,
				5303,
				5304,
				5302,
				5364
			});

			// Token: 0x04006EC6 RID: 28358
			public static bool[] NeverLocksOn = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				496,
				1326,
				5335
			});

			// Token: 0x04006EC7 RID: 28359
			public static bool[] LockOnIgnoresCollision = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				64,
				3570,
				1327,
				3006,
				1227,
				788,
				756,
				1228,
				65,
				3065,
				3473,
				3051,
				5065,
				1309,
				2364,
				2365,
				2551,
				2535,
				2584,
				1157,
				2749,
				1802,
				2621,
				3249,
				3531,
				3474,
				2366,
				1572,
				4269,
				4273,
				4281,
				4607,
				5069,
				5114,
				5119,
				5456,
				5463,
				5663,
				5664,
				3014,
				3569,
				3571
			});

			// Token: 0x04006EC8 RID: 28360
			public static int[] LockOnAimAbove = ItemID.Sets.Factory.CreateIntSet(0, new int[]
			{
				1256,
				15,
				1244,
				15,
				3014,
				15,
				3569,
				15,
				3571,
				15
			});

			// Token: 0x04006EC9 RID: 28361
			public static float?[] LockOnAimCompensation = ItemID.Sets.Factory.CreateCustomSet<float?>(null, new object[]
			{
				1336,
				0.2f,
				157,
				0.29f,
				2590,
				0.4f,
				3821,
				0.4f,
				160,
				0.4f
			});

			// Token: 0x04006ECA RID: 28362
			public static bool[] SingleUseInGamepad = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				8,
				427,
				3004,
				523,
				433,
				429,
				974,
				1333,
				1245,
				3114,
				430,
				3045,
				428,
				2274,
				431,
				432,
				4383,
				4384,
				4385,
				4386,
				4387,
				4388,
				5293,
				5353
			});

			// Token: 0x04006ECB RID: 28363
			public static bool[] Torches = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				8,
				427,
				3004,
				523,
				433,
				429,
				974,
				1333,
				1245,
				3114,
				430,
				3045,
				428,
				2274,
				431,
				432,
				4383,
				4384,
				4385,
				4386,
				4387,
				4388,
				5293,
				5353
			});

			// Token: 0x04006ECC RID: 28364
			public static bool[] WaterTorches = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				523,
				1333,
				4384
			});

			// Token: 0x04006ECD RID: 28365
			public static bool[] Campfires = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				966,
				3724,
				4690,
				4691,
				4692,
				3046,
				3047,
				4689,
				3048,
				4693,
				3049,
				4694,
				5299,
				3050,
				5357,
				3723
			});

			// Token: 0x04006ECE RID: 28366
			public static bool[] Glowsticks = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				282,
				286,
				3112,
				3002,
				4776,
				5643
			});

			// Token: 0x04006ECF RID: 28367
			public static short[] Workbenches = new short[]
			{
				36,
				635,
				636,
				637,
				673,
				811,
				812,
				813,
				814,
				815,
				916,
				1145,
				1398,
				1401,
				1404,
				1461,
				1511,
				1795,
				1817,
				2229,
				2251,
				2252,
				2253,
				2534,
				2631,
				2632,
				2633,
				2826,
				3156,
				3157,
				3158,
				3909,
				3910,
				3949,
				3975,
				4163,
				4184,
				4205,
				4226,
				4315,
				4584,
				5166,
				5187,
				5208
			};

			// Token: 0x04006ED0 RID: 28368
			public static bool[] BossBag = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3318,
				3319,
				3320,
				3321,
				3322,
				3323,
				3324,
				3325,
				3326,
				3327,
				3328,
				3329,
				3330,
				3331,
				3332,
				3860,
				3861,
				3862,
				4782,
				4957,
				5111
			});

			// Token: 0x04006ED1 RID: 28369
			public static bool[] OpenableBag = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3318,
				3319,
				3320,
				3321,
				3322,
				3323,
				3324,
				3325,
				3326,
				3327,
				3328,
				3329,
				3330,
				3331,
				3332,
				3860,
				3861,
				3862,
				4782,
				4957,
				5111,
				2334,
				2335,
				2336,
				3203,
				3204,
				3205,
				3206,
				3207,
				3208,
				4405,
				4407,
				4877,
				5002,
				3979,
				3980,
				3981,
				3982,
				3983,
				3984,
				3985,
				3986,
				3987,
				4406,
				4408,
				4878,
				5003,
				3093,
				4345,
				4410,
				1774,
				6142,
				3085,
				4879,
				1869,
				599,
				600,
				601
			});

			// Token: 0x04006ED2 RID: 28370
			public static bool[] HasItemSwap = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				5325,
				4131,
				5060,
				5059,
				5324,
				5329,
				5330,
				4346,
				5391,
				5323,
				5455,
				4767,
				5453,
				5309,
				5454,
				5358,
				5360,
				5361,
				5359,
				5437,
				2611,
				5526
			});

			// Token: 0x04006ED3 RID: 28371
			public static bool[] RecoverableImportantItem = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				3318,
				3319,
				3320,
				3321,
				3322,
				3323,
				3324,
				3325,
				3326,
				3327,
				3328,
				3329,
				3330,
				3331,
				3332,
				3860,
				3861,
				3862,
				4782,
				4957,
				5111
			});

			// Token: 0x04006ED4 RID: 28372
			public static bool?[] OnlyNeedOneInInventoryOverride = ItemID.Sets.Factory.CreateCustomSet<bool?>(null, new object[]
			{
				1522,
				true,
				1523,
				true,
				1524,
				true,
				1525,
				true,
				1526,
				true,
				1527,
				true,
				3643,
				true,
				3103,
				true,
				3104,
				true,
				856,
				true,
				1299,
				true,
				329,
				true,
				4346,
				true,
				5391,
				true,
				4767,
				true,
				5453,
				true,
				5309,
				true,
				5454,
				true,
				5323,
				true,
				5455,
				true,
				205,
				false,
				206,
				false,
				207,
				false,
				1128,
				false,
				267,
				false,
				849,
				false
			});

			// Token: 0x04006ED5 RID: 28373
			public static bool[] HasRightFire = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				4346,
				5391,
				5323,
				5455,
				4767,
				5453,
				5309,
				5454,
				4131,
				5325,
				5358,
				5359,
				5360,
				5361,
				3384,
				3823,
				3858,
				4760,
				3852,
				4367,
				4368,
				4369,
				4370,
				4371,
				4379,
				4610,
				4611,
				4612,
				4613,
				4648,
				4649,
				4650,
				4651,
				4669,
				4670,
				4671,
				4673,
				4674,
				4675,
				4676,
				4677,
				4681,
				4683,
				4684,
				2611,
				5526
			});

			// Token: 0x04006ED6 RID: 28374
			public static bool?[] CanPassivelyStackInWorldOverride = ItemID.Sets.Factory.CreateCustomSet<bool?>(null, new object[]
			{
				71,
				false,
				72,
				false,
				73,
				false,
				74,
				false,
				75,
				false,
				205,
				true,
				206,
				true,
				207,
				true,
				1128,
				true,
				530,
				true
			});

			// Token: 0x04006ED7 RID: 28375
			public static bool[] IsRangedSpecialistWeapon = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				1156,
				3350,
				3210,
				160,
				266,
				5629,
				3821,
				5460
			});

			// Token: 0x04006ED8 RID: 28376
			public static UniqueTagEffect[] UniqueTagEffects = ItemID.Sets.Factory.CreateCustomSet<UniqueTagEffect>(null, new object[]
			{
				5688,
				new WhipTagEffect
				{
					TagDamage = 3
				},
				5473,
				new WhipTagEffect
				{
					TagDamage = 3,
					PlayerBuffId = 365,
					PlayerBuffTime = 180,
					PlayerBuffAppliedManually = true
				},
				4672,
				new WhipTagEffect
				{
					TagDamage = 4
				},
				5474,
				new WhipTagEffect
				{
					TagDamage = 5
				},
				5475,
				new WhipTagEffect
				{
					TagDamage = 5
				},
				5476,
				new WhipTagEffect_Starcrash
				{
					TagDamage = 2
				},
				4913,
				new WhipTagEffect
				{
					TagDamage = 6,
					PlayerBuffId = 314,
					PlayerBuffTime = 180
				},
				5074,
				new WhipTagEffect
				{
					TagDamage = 7
				},
				4911,
				new WhipTagEffect
				{
					TagDamage = 6,
					PlayerBuffId = 312,
					PlayerBuffTime = 180,
					PlayerBuffAppliedManually = true
				},
				4678,
				new WhipTagEffect
				{
					TagDamage = 9,
					PlayerBuffId = 308,
					PlayerBuffTime = 180
				},
				5477,
				new WhipTagEffect_ViolentDisplayOfFlower
				{
					TagDamage = 9
				},
				4680,
				new WhipTagEffect_DarkHarvest
				{
					TagDamage = 15,
					PlayerBuffId = 311,
					PlayerBuffTime = 180
				},
				4679,
				new WhipTagEffect
				{
					TagDamage = 8,
					CritChance = 10
				},
				5478,
				new WhipTagEffect
				{
					TagDamage = 12,
					CritChance = 5,
					NetSync = true
				},
				5479,
				new WhipTagEffect
				{
					TagDamage = 15,
					CritChance = 15
				},
				4914,
				new WhipTagEffect_Kaleidoscope
				{
					TagDamage = 20,
					CritChance = 10
				},
				5480,
				new WhipTagEffect_Possession
				{
					TagDamage = 25,
					CritChance = 10
				},
				4912,
				new WhipTagEffect_Firecracker(),
				6143,
				new WhipTagEffect
				{
					TagDamage = 4
				}
			});

			// Token: 0x04006ED9 RID: 28377
			public static bool[] IsWireableStatue = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				441,
				449,
				466,
				471,
				3651,
				3652,
				3653,
				3654,
				3655,
				3656,
				3657,
				3658,
				3659,
				3660,
				3661,
				3662,
				3708,
				3709,
				3710,
				3711,
				3712,
				3713,
				3714,
				3715,
				3716,
				3717,
				3718,
				3719,
				3720,
				4397,
				4360,
				4342,
				4466,
				5317,
				5318,
				5319,
				440,
				443,
				444,
				445,
				446,
				452,
				454,
				459,
				463,
				464,
				478,
				473,
				2672,
				438,
				453,
				476,
				477,
				470,
				4355,
				4276
			});
		}
	}
}
