using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace GadgetBox.GadgetUI
{
	internal class UIFancyButton : UIHoverText
	{
		private Texture2D _texture;
		private Texture2D _hoverTexture;
		private bool _isClicking;
		private float _clickScale;

		public bool Visible { get; internal set; }
		public float Rotation { get; internal set; }
		internal event Func<bool> CanClick;

		public UIFancyButton(Texture2D texture, Texture2D hoverTexture, float clickScale = 0.85f) : base()
		{
			_texture = texture;
			_hoverTexture = hoverTexture;
			_clickScale = clickScale;
			Width.Set(_texture.Width, 0f);
			Height.Set(_texture.Height, 0f);
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			if (!Visible)
			{
				return;
			}

			_isClicking = true;
			if (CanClick?.Invoke() ?? true)
			{
				base.MouseDown(evt);
			}
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			if (!Visible)
			{
				return;
			}

			if (_isClicking)
			{
				_isClicking = false;
			}

			base.MouseUp(evt);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			if (!Visible)
			{
				return;
			}

			if (_isClicking)
			{
				_isClicking = false;
			}

			base.MouseOut(evt);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			if (!Visible)
			{
				return;
			}

			base.MouseOver(evt);
			Main.PlaySound(SoundID.MenuTick);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (!Visible)
			{
				return;
			}

			Texture2D texture = IsMouseHovering ? _hoverTexture : _texture;
			float scale = _isClicking ? _clickScale : 1f;
			Vector2 origin = texture.Size() * 0.5f * scale;
			spriteBatch.Draw(texture, GetDimensions().Position() + origin, null, Color.White, Rotation, origin, scale, SpriteEffects.None, 0);
			base.DrawSelf(spriteBatch);
		}
	}
}