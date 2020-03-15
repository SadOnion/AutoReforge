using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace GadgetBox.GadgetUI
{
	internal class UILeftAlignedLabel : UIPanel
	{
		private float _textScale = 1f;
		private Vector2 _textSize = Vector2.Zero;
		private bool _isLarge;

		internal string Text { get; set; } = "";
		internal Color TextColor { get; set; } = Color.White;

		public UILeftAlignedLabel(string text, Color color, float textScale = 1f, bool large = false)
		{
			SetText(text, textScale, large);
			TextColor = color;
		}

		public override void Recalculate()
		{
			SetText(Text, _textScale, _isLarge);
			base.Recalculate();
		}

		public void SetText(string text, float textScale, bool large)
		{
			DynamicSpriteFont spriteFont = large ? Main.fontDeathText : Main.fontMouseText;
			Vector2 textSize = new Vector2(spriteFont.MeasureString(text).X, large ? 32f : 16f) * textScale;
			Text = text;
			_textScale = textScale;
			_isLarge = large;
			Width.Precent = 1f;
			MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
			MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			Vector2 pos = GetInnerDimensions().Position();
			pos.Y -= _textScale * (_isLarge ? 10 : 2);
			if (_isLarge)
			{
				Utils.DrawBorderStringBig(spriteBatch, Text, pos, Colors.AlphaDarken(TextColor), _textScale, 0f, 0f, -1);
				return;
			}
			Utils.DrawBorderString(spriteBatch, Text, pos, Colors.AlphaDarken(TextColor), _textScale, 0f, 0f, -1);
		}
	}
}