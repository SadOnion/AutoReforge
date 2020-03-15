using Terraria;
using Terraria.ModLoader;

namespace GadgetBox
{
	internal static class ModCompat
	{
		internal static Mod RAmod;
		internal static Mod EMMMod;

		internal static void Load()
		{
			RAmod = ModLoader.GetMod("ReforgeArmor");
			EMMMod = ModLoader.GetMod("Loot");
		}

		internal static void Unload()
		{
			RAmod = null;
			EMMMod = null;
		}

		internal static bool ArmorPrefix(Item item) => (RAmod != null || EMMMod != null) && !item.vanity && (item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1);

		internal static void ApplyArmorPrefix(Item item, byte prefix)
		{
			if (RAmod != null && item.value <= 1 && item.rare > 0)
			{
				item.value = (item.rare * item.defense * 2500);
				item.Prefix(prefix);
			}
		}
	}
}