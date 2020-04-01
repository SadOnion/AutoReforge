using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AutoReroll
{
    class PrefixUtils
    {
        public static bool GeneralPrefix(Item item)
		{
			if (item.maxStack == 1 && item.damage > 0 && item.ammo == 0)
			{
				return !item.accessory;
			}
			return false;
		}
		public static bool MeleePrefix(Item item)
		{
			if (GeneralPrefix(item) && item.melee)
			{
				return !item.noUseGraphic;
			}
			return false;
		}
		public static bool ModMeleePrefix(Item item)
		{
			if (item.modItem != null && GeneralPrefix(item) && item.melee)
			{
				return !item.noUseGraphic;
			}
			return false;
		}
		public static bool WeaponPrefix(Item item)
		{
			if (GeneralPrefix(item) && item.melee)
			{
				return item.noUseGraphic;
			}
			return false;
		}
		public static bool ModWeaponPrefix(Item item)
		{
			if (GeneralPrefix(item) && item.melee)
			{
				return item.noUseGraphic;
			}
			return false;
		}
		public static bool RangedPrefix(Item item)
		{
			if (GeneralPrefix(item))
			{
				if (!item.ranged)
				{
					return item.thrown;
				}
				return true;
			}
			return false;
		}
		public static bool ModRangedPrefix(Item item)
		{
			if (item.modItem != null && GeneralPrefix(item))
			{
				if (!item.ranged)
				{
					return item.thrown;
				}
				return true;
			}
			return false;
		}
		public static bool MagicPrefix(Item item)
		{
			if (GeneralPrefix(item))
			{
				return item.magic;
			}
			return false;
		}
		public static bool ModMagicPrefix(Item item)
		{
			if (item.modItem != null && GeneralPrefix(item))
			{
				return item.magic;
			}
			return false;
		}
		public static bool ModSummonPrefix(Item item)
		{
			if (item.modItem != null && GeneralPrefix(item))
			{
				return item.summon;
			}
			return false;
		}
		public static bool SummonPrefix(Item item)
		{
			if (GeneralPrefix(item))
			{
				return item.summon;
			}
			return false;
		}
		public enum Prefix
		{
			None,
			Legendary = 81,
			Godly = 59,
			Mythical = 83,
			Unreal = 82,
			Light=15,
			Demonic=60,
			Ruthless=57,
			Accessory,
			Fabled = 95,
			Masterful = 28
		}
		public static Prefix BestPrefix(Item item)
        {
			if(AutoReroll.AlchemistNPC != null || AutoReroll.AlchemistNPC_Lite != null)
			{
				return Prefix.None;
			}
            UnifiedRandom unifiedRandom = WorldGen.gen ? WorldGen.genRand : Main.rand;
            int num = 0;
            int modPrefix = ItemLoader.ChoosePrefix(item, unifiedRandom);
			Main.NewText(modPrefix);
            if (modPrefix >= 0)
            {
                num = modPrefix;//-1?
            }
            else if (MeleePrefix(item))
            {
				if(item.axe > 0 || item.pick>0 || item.hammer > 0)
				{
					if (item.knockBack > 0)
					{

						return Prefix.Light;
					}
					else
					{
						return Prefix.None;
					}
				}
				if (item.useStyle == 1 || item.useStyle == 3)
				{
					return Prefix.Legendary;
				}
				else
				{
					return Prefix.Godly;
				}
            }
            else if (PrefixUtils.WeaponPrefix(item))
            {
				if (item.knockBack > 0)
				{
					return Prefix.Godly;

				}
				else
				{
					return Prefix.None;
				}
            }
            else if (PrefixUtils.RangedPrefix(item))
            {
				Item defItem = item.Clone();
				defItem.SetDefaults(item.type,false);
				if(item.knockBack > 0)
				{
					if (defItem.useAnimation > 2)
					{

						return Prefix.Unreal;
					}
					else
					{
						return Prefix.Godly;
					}
				}
				else
				{
					return Prefix.Demonic;
				}
            }
            else if (PrefixUtils.MagicPrefix(item))
            {
				Item defItem = item.Clone();
				defItem.SetDefaults(item.type,false);
				if (item.mana > 0)
				{
					if(defItem.damage<=5)return Prefix.None;
					if(item.knockBack > 0)
					{
						if (defItem.rare>1)
						{
							if (defItem.useAnimation > 7 && defItem.useTime>2)
							{

							 return Prefix.Mythical;
							}
							else return Prefix.Masterful;
						}
						else
						{
							return Prefix.Godly;
						}
					
					}
					else
					{
						return Prefix.Demonic;
					}
				}
				else
				{
					return Prefix.Godly;
				}
            }else if (PrefixUtils.SummonPrefix(item))
			{
				if (item.knockBack > 0)
				{

				return Prefix.Ruthless;
				}
				else
				{
					return Prefix.Demonic;
				}
			}
            else
            {
                if (!PrefixUtils.WeaponPrefix(item))
                {
                    if (!item.vanity)
                    {
                        return Prefix.Accessory;
                    }
                    return Prefix.None;
                }
                return Prefix.Godly;
            }

			if(item.modItem.mod != null)
			{
				Mod thisItemMod = item.modItem.mod;
				if(thisItemMod == AutoReroll.Thorium)
				{
					if (item.thrown)
					{
						if (item.knockBack > 0)
						{

						return Prefix.Unreal;
						}
						else
						{
							return Prefix.Demonic;
						}
					}
					else
					{
						return Prefix.Fabled;
					}
				}
				if(thisItemMod == AutoReroll.Calamity)
				{
					if(item.knockBack > 0) return Prefix.Unreal;
					else return Prefix.Demonic;
				}
			}
			else
			{
				return Prefix.None;
			}
			return Prefix.None;
        }

    }
}
