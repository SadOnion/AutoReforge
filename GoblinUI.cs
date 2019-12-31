

using BestModifierRoll;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.Utilities;

namespace AutoReroll
{
    public class GoblinUI : UIState{
		double counter;
		bool reforging;
		public UIHoverImage but;
		UIPanel panel;
		bool imageHover;
		public override void OnInitialize()
		{
			counter=0;
			  panel = new UIPanel(); // 2
			panel.MarginLeft = 90;
			panel.MarginTop = 325;
			 panel.Width.Set(50, 0); // 3
			 panel.Height.Set(50, 0); // 3
			panel.BackgroundColor = Color.Transparent;
			panel.BorderColor = Color.Transparent;
			 Append(panel); // 4
			 but = new UIHoverImage(ModContent.GetTexture(AutoReroll.modName+"/Images/sprite_0"),"Reforge");
			
			but.Width.Set(50,0);
			
			but.Height.Set(50,0);
			
			but.OnClick += But_OnClick;
			panel.Append(but);

		}
		
		private void But_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			reforging=true;
			counter=0;
		}
		
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (!Main.InReforgeMenu || Main.reforgeItem.type == 0)
			{
				reforging=false;
			}
			if(Main.reforgeItem!=null && reforging)Check(gameTime);
			if (but.IsMouseHovering && imageHover==false)
			{
				but.SetImage(ModContent.GetTexture(AutoReroll.modName+"/Images/sprite_1"));
				imageHover=true;
				Main.PlaySound(SoundID.MenuTick);
				but.SetHoverText(MakeTextHover(Main.reforgeItem));
			
			}
			if(!but.IsMouseHovering && imageHover==true)
			{
				but.SetImage(ModContent.GetTexture(AutoReroll.modName+"/Images/sprite_0"));
				imageHover=false;
			}
		}

		private string MakeTextHover(Item reforgeItem){
			
			Prefix pref = BestPrefix(reforgeItem);
			if(pref == Prefix.None) 
			{
				return "Can't identify best modifier sorry ;(";
			}
			if (pref == Prefix.Accessory)
			{
				 return "Reforge until Menacing,Warding or Lucky";
			}

			return "Reforge until "+pref.ToString();

		}
		

		private void Check(GameTime time)
		{
			
				counter+=time.ElapsedGameTime.TotalMilliseconds;
				if (counter > 1000/AutoReroll.ForgePerSec)
				{
					Reforge();
					counter=0;
					CheckForDesirePrefix(Main.reforgeItem);
				}
			
		}

		private void CheckForDesirePrefix(Item reforgeItem)
		{
			Prefix pref = BestPrefix(reforgeItem);
			if(pref == Prefix.None) reforging=false;
			else if (pref == Prefix.Accessory)
			{
				if(reforgeItem.prefix == 65||reforgeItem.prefix == 68||reforgeItem.prefix == 72) reforging=false;
			}
			else if(reforgeItem.prefix == (int)pref) reforging=false;
		}

		private void Reforge()
		{
			if (Main.reforgeItem.type > 0)
			{
				bool canApplyDiscount = true;
				int num72 = Main.reforgeItem.value;
				if (ItemLoader.ReforgePrice(Main.reforgeItem, ref num72, ref canApplyDiscount))
				{
					if (canApplyDiscount && Main.player[Main.myPlayer].discount)
					{
						num72 = (int)((double)num72 * 0.8);
					}
					num72 /= 3;
				}

				int num103 = num72;
				if (num103 < 1)
				{
					num103 = 1;
				}
			

				if(Main.player[Main.myPlayer].BuyItem(num72, -1) == false)
				{
					reforging=false;
					return;
				}
				bool favorited = Main.reforgeItem.favorited;
				int stack = Main.reforgeItem.stack;
				Item obj8 = new Item();
				obj8.netDefaults(Main.reforgeItem.netID);
				Item obj9 = obj8.CloneWithModdedDataFrom(Main.reforgeItem);
				obj9.Prefix(-2);
				Main.reforgeItem = obj9.Clone();
				Main.reforgeItem.position.X = Main.player[Main.myPlayer].position.X + (float)(Main.player[Main.myPlayer].width / 2) - (float)(Main.reforgeItem.width / 2);
				Main.reforgeItem.position.Y = Main.player[Main.myPlayer].position.Y + (float)(Main.player[Main.myPlayer].height / 2) - (float)(Main.reforgeItem.height / 2);
				Main.reforgeItem.favorited = favorited;
				Main.reforgeItem.stack = stack;
				ItemLoader.PostReforge(Main.reforgeItem);
				ItemText.NewText(Main.reforgeItem, Main.reforgeItem.stack, true, false);
				Main.PlaySound(SoundID.Item37, -1, -1);
			}
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
			Fabled = 95
		}
		
        private Prefix BestPrefix(Item item)
        {
            UnifiedRandom unifiedRandom = WorldGen.gen ? WorldGen.genRand : Main.rand;
            int num = 0;
            int modPrefix = ItemLoader.ChoosePrefix(item, unifiedRandom);
            if (modPrefix >= 0)
            {
                num = modPrefix;//-1?
            }
            else if (PrefixUtils.MeleePrefix(item))
            {
				if(item.axe > 0 || item.pick>0 || item.hammer > 0)
				{
					
						return Prefix.Light;
				}
				
                return Prefix.Legendary; 
            }
            else if (PrefixUtils.WeaponPrefix(item))
            {
                return Prefix.Godly;
            }
            else if (PrefixUtils.RangedPrefix(item))
            {
				if(item.knockBack > 0)
				{

				 return Prefix.Unreal;
				}
				else
				{
					return Prefix.Demonic;
				}
            }
            else if (PrefixUtils.MagicPrefix(item))
            {
				if (item.mana > 0)
				{
					if(item.damage<5)return Prefix.None;
					if(item.knockBack > 0)
					{
						if (item.rare > 2)
						{
							 return Prefix.Mythical;
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
				return Prefix.Ruthless;
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