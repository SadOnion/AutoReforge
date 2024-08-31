using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GadgetBox
{
    public static class GadgetMethods
    {
        public static int ReforgePrice(this Item item, Player player = null)
        {
            if (item == null || item.IsAir)
            {
                return 0;
            }
            if (player == null)
            {
                player = Main.LocalPlayer;
            }
            int reforgePrice = item.value;
            bool canApplyDiscount = true;
            if (ItemLoader.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount))
            {
                if (canApplyDiscount && player.discountAvailable)
                {
                    reforgePrice = (int)(reforgePrice * 0.8f);
                }
                reforgePrice /= 3;
            }
            return reforgePrice;
        }

        public static void PrefixItem(ref Item item, bool silent = false, bool reset = false)
        {
            ItemLoader.PreReforge(item);
            item.ResetPrefix();
            bool favorited = item.favorited;
            int stack = item.stack;
            Item tempItem = new Item();
            tempItem.netDefaults(item.netID);
            tempItem = item.Clone()/* tModPorter Note: Removed. Use Clone, ResetPrefix or Refresh */;
            if (!reset)
            {
                tempItem.Prefix(-2);
            }
            item = tempItem.Clone();
            item.Center = Main.LocalPlayer.Center;
            item.favorited = favorited;
            item.stack = stack;
            if (!reset)
            {
                ItemLoader.PostReforge(item);
            }
            PopupText.NewText(PopupTextContext.ItemReforge, item, item.stack, true, false);
            if (silent)
            {
                return;
            }
            SoundEngine.PlaySound(reset ? SoundID.Grab : SoundID.Item37);
        }
        public static bool GeneralPrefix(Item item)
        {
            if (item.maxStack == 1 && item.damage > 0 && item.ammo == 0)
            {
                return !item.accessory;
            }

            return false;
        }

        internal static bool MeleePrefix(Item item)
        {
            if (GeneralPrefix(item) && item.DamageType == DamageClass.Melee)
            {
                return !item.noUseGraphic;
            }

            return false;
        }

        internal static bool WeaponPrefix(Item item)
        {
            if (GeneralPrefix(item) && item.DamageType == DamageClass.Melee)
            {
                return item.noUseGraphic;
            }

            return false;
        }

        internal static bool RangedPrefix(Item item)
        {
            if (GeneralPrefix(item))
            {
                return item.DamageType == DamageClass.Ranged;
            }

            return false;
        }

        internal static bool MagicPrefix(Item item)
        {
            if (GeneralPrefix(item))
            {
                if (!(item.DamageType == DamageClass.Magic))
                {
                    return item.DamageType == DamageClass.Summon;
                }

                return true;
            }

            return false;
        }
        public static bool IsAPrefixableAccessory(Item item)
        {
            if (item.accessory && !item.vanity)
            {
                return ItemID.Sets.CanGetPrefixes[item.type];
            }

            return false;
        }
        public static HashSet<int> GetValidModedPrefixes(Item item, params PrefixCategory[] categories)
        {
            HashSet<int> prefixes = new HashSet<int>();
            foreach (PrefixCategory key in categories)
            {
                foreach (ModPrefix prefix in PrefixLoader.GetPrefixesInCategory(key).Where((ModPrefix x) => x.CanRoll(item)))
                {
                    if (ValidatePrefix(prefix, item))
                    {
                        prefixes.Add(prefix.Type);
                    }
                }
            }

            return prefixes;
        }
        public static bool ValidatePrefix(ModPrefix prefix, Item originalItem)
        {
            bool invalid = false;
            float damageMult = 1f;
            float knockbackMult = 1f;
            float useTimeMult = 1f;
            float scaleMult = 1f;
            float shootSpeedMult = 1f;
            float manaMult = 1f;
            int critBonus = 0;
            Item item = originalItem.Clone();

            int num = prefix.Type;

            if (num >= 85)
            {
                prefix.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
            }

            if (damageMult != 1f && Math.Round(item.damage * damageMult) == item.damage)
            {
                invalid = true;
                num = -1;
            }

            if (useTimeMult != 1f && Math.Round(item.useAnimation * useTimeMult) == item.useAnimation)
            {
                invalid = true;
                num = -1;
            }

            if (manaMult != 1f && Math.Round(item.mana * manaMult) == item.mana)
            {
                invalid = true;
                num = -1;
            }

            if (knockbackMult != 1f && item.knockBack == 0f)
            {
                invalid = true;
                num = -1;
            }

            if (num >= 85)
            {
                invalid &= prefix.AllStatChangesHaveEffectOn(item);
            }

            if (num == 0)
            {
                num = -1;
                invalid = true;
            }

            if (!invalid && !ItemLoader.AllowPrefix(item, num))
            {
                invalid = true;
            }
            return !invalid;
        }

        public static long GetTotalMoney(this Player player)
        {
            long num = Utils.CoinsCount(out bool overflowing, player.inventory, new int[5]
            {
                58,
                57,
                56,
                55,
                54
            });
            long num2 = Utils.CoinsCount(out overflowing, player.bank.item, new int[0]);
            long num3 = Utils.CoinsCount(out overflowing, player.bank2.item, new int[0]);
            long num4 = Utils.CoinsCount(out overflowing, player.bank3.item, new int[0]);
            long num5 = Utils.CoinsCount(out overflowing, player.bank4.item, new int[0]);
            long totalMoney = (Utils.CoinsCombineStacks(out overflowing, new long[5]
            {
                num,
                num2,
                num3,
                num4,
                num5
            }));
            return totalMoney;
        }
    }
}