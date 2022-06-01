using System.ComponentModel;
using Terraria.ModLoader.Config;
namespace AutoReroll
{
	class Config : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		[Range(1, 100)]
		[Label("AutoReforge Speed")]
		[DefaultValue(10)]
		[Increment(1)]
		[Tooltip("Reforge Per Sec")]
		public int ReforgePerSec;
		[DefaultValue(false)]
		public bool UseDefaultReforgeMenu;
		public override void OnChanged()
		{
			AutoReroll.ForgePerSec = ReforgePerSec;
			AutoReroll.UseDefaultReforgeMenu = UseDefaultReforgeMenu;
		}
	}
}
