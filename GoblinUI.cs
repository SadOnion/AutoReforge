

using Microsoft.Xna.Framework;
using System;
using System.Timers;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace AutoReroll
{
    public class GoblinUI : UIState{
		Timer timer;
		Timer stopTime;
		double counter;
		bool reforging;
		public UIImage but;
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
			 but = new UIImage(ModContent.GetTexture(AutoReroll.modName+"/Images/sprite_0"));
			
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
			}
			if(!but.IsMouseHovering && imageHover==true)
			{
				but.SetImage(ModContent.GetTexture(AutoReroll.modName+"/Images/sprite_0"));
				imageHover=false;
			}
		}
		private void Check(GameTime time)
		{
			
				counter+=time.ElapsedGameTime.TotalMilliseconds;
				if (counter > 150)
				{
					Reforge();
					counter=0;
					CheckForDesirePrefix(Main.reforgeItem);
				}
			
		}

		private void CheckForDesirePrefix(Item reforgeItem)
		{
			if (reforgeItem.melee)
			{
				if(reforgeItem.pick > 0 || reforgeItem.axe > 0 || reforgeItem.hammer > 0)
				{
					//Light
					if(reforgeItem.prefix == 15)reforging=false;
				}
				else
				{
					//Godly Legendary
					if(reforgeItem.prefix == 59 || reforgeItem.prefix == 81)reforging=false;
				}
			}else if (reforgeItem.ranged)
			{
				if(reforgeItem.knockBack > 0)
				{
					if(reforgeItem.prefix == 82)reforging=false;
				}
				else
				{
					//Demonic 
					if(reforgeItem.prefix == 60)reforging=false;
				}
			}else if(reforgeItem.magic)
			{
				if(reforgeItem.knockBack > 0)
				{
					if(reforgeItem.prefix == 83)reforging=false;
				}
				else
				{
					//Demonic 
					if(reforgeItem.prefix == 60)reforging=false;
				}
			}else if (reforgeItem.summon)
			{
				//Ruthless
				if(reforgeItem.prefix == 57)reforging=false;
			}else{
				//Wardign
				if(reforgeItem.prefix == 65)reforging=false;
				//Menacing
				if(reforgeItem.prefix == 72)reforging=false;
				//Lucky
				if(reforgeItem.prefix == 68)reforging=false;
				//Godly
				if(reforgeItem.prefix == 59)reforging=false;
			}
			
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
	}
}
