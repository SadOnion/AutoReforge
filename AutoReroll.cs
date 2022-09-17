
using GadgetBox;
using GadgetBox.GadgetUI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Timers;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Utilities;

namespace AutoReroll
{
	public class AutoReroll : Mod
	{
		public const string modName = "Auto Reforge";
		public static AutoReroll Instance;
		public static UnifiedRandom Rng = new UnifiedRandom(Environment.TickCount);
		public static int ForgePerSec = 10;
		public static bool UseDefaultReforgeMenu = false;
		public bool isInReforgeMenu;
		public bool ReforgeMenu;
		public UserInterface userInterface;


		public override void Load()
		{
			Instance = this;
			if (!Main.dedServ)
			{
				userInterface = new UserInterface();
			}
		}
		public override void Unload()
		{
			Instance = null;
		}


	}

}