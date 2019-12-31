using System.ComponentModel;
using Terraria.ModLoader.Config;
namespace AutoReroll
{
    class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Range(1,20)]
        [Label("AutoReforge Speed")]
        [DefaultValue(10)]
        [Increment(1)]
        [Tooltip("Reforge Per Sec")]
        public int ReforgePerSec;

        public override void OnChanged()
        {
            AutoReroll.ForgePerSec=ReforgePerSec;
        }
    }
}
