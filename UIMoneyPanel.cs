using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Localization;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using GadgetBox;
using System;

namespace AutoReroll
{
	class UIMoneyPanel : UIPanel
	{
		public bool Visible { get; internal set; }
		Color textColor = Color.White;
		string text = Language.GetTextValue("Mods.AutoReroll.UI.LeaveMe");
		public UICounterButton[] CounterButtons { get; private set; } = new UICounterButton[4];
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (!Visible)
			{

				return;
			}
			base.DrawSelf(spriteBatch);

		}
		protected override void DrawChildren(SpriteBatch spriteBatch)
		{
			if (!Visible)
			{

				return;
			}

			Utils.DrawBorderString(spriteBatch, text, GetInnerDimensions().Position() - FontAssets.MouseText.Value.MeasureString(text).X * Vector2.UnitX, Colors.AlphaDarken(textColor), .85f);
			base.DrawChildren(spriteBatch);
		}
		public int GetMoneyValue()
		{
			return CounterButtons[0].Counter * Item.copper + CounterButtons[1].Counter * Item.silver
				+ CounterButtons[2].Counter * Item.gold + CounterButtons[3].Counter * Item.platinum;
		}
		public override void OnInitialize()
		{

			MinWidth.Pixels = 150;
			MinHeight.Pixels = 40;
			Width.Pixels = 150;
			Height.Pixels = 40;
			int padding = 25;
			Player player = Main.CurrentPlayer;
			UICounterButton platinumButton = new UICounterButton(TextureAssets.Coin[3]);
			platinumButton.Left.Pixels = 25;
			UICounterButton goldButton = new UICounterButton(TextureAssets.Coin[2]);
			goldButton.Left.Pixels = platinumButton.Left.Pixels + padding;
			UICounterButton silverButton = new UICounterButton(TextureAssets.Coin[1]);
			silverButton.Left.Pixels = goldButton.Left.Pixels + padding;
			//xd
			UICounterButton copperButton = new UICounterButton(TextureAssets.Coin[0]);
			copperButton.Left.Pixels = silverButton.Left.Pixels + padding;

			CounterButtons[3] = platinumButton;
			CounterButtons[2] = goldButton;
			CounterButtons[1] = silverButton;
			CounterButtons[0] = copperButton;

			Append(platinumButton);
			Append(goldButton);
			Append(silverButton);
			Append(copperButton);
		}
	}
	class UICounterButton : UIImageButton
	{
		public int Counter { get; private set; }
		private UIText counterText;
		public UICounterButton(Asset<Texture2D> texture, int defaultValue = 0) : base(texture)
		{
			OnScrollWheel += (a, b) => ChangeCounterOnScrool(a.ScrollWheelValue);
			Counter = defaultValue;
			counterText = new UIText(Counter.ToString(), .75f);
			counterText.Top.Pixels = 5;
			Append(counterText);
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (IsMouseHovering)
			{
				Main.hoverItemName = Language.GetTextValue("Mods.AutoReroll.UI.UseMouse");
			}
			base.DrawSelf(spriteBatch);
		}
		private void ChangeCounterOnScrool(int scroll)
		{
			int value = Counter + (scroll > 0 ? 1 : -1);
			value = value < 0 ? 99 : value > 99 ? 0 : value;
			SetCounter(value);
		}
		public void SetCounter(int value)
		{
			Counter = value;
			counterText.SetText(Counter.ToString());
		}

	}
}
