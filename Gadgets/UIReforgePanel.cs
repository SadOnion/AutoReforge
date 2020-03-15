using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace GadgetBox.GadgetUI
{
	internal class UIReforgePanel : UIPanel
	{
		private readonly Func<Item> _reforgeItem;
		private readonly Func<int> _reforgePrice;

		public UIReforgePanel(Func<Item> reforgeItem, Func<int> reforgePrice)
		{
			_reforgeItem = reforgeItem;
			_reforgePrice = reforgePrice;
			Recalculate();
		}

		public override void Recalculate()
		{
			Width.Set(Math.Max(Main.fontMouseText.MeasureString(Language.GetTextValue("LegacyInterface.20")).X + 90, 320), 0);
			base.Recalculate();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle style = GetDimensions();
			string priceText;
			Vector2 priceOffset = new Vector2(style.X + 68, style.Y + 28);
			if (!_reforgeItem().IsAir)
			{
				priceOffset += new Vector2(36, -14);
				priceText = Language.GetTextValue("LegacyInterface.46");
				float xOffset = Main.fontMouseText.MeasureString(priceText).X - 20;
				ItemSlot.DrawMoney(spriteBatch, "", priceOffset.X + xOffset, priceOffset.Y - 42, Utils.CoinsSplit(Math.Max(_reforgePrice(), 1)), true);
				ItemSlot.DrawSavings(spriteBatch, priceOffset.X, priceOffset.Y - 14, true);
			}
			else
			{
				priceText = Language.GetTextValue("LegacyInterface.20");
			}

			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, priceText, priceOffset, new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
		}
	}
}