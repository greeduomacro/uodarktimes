using System;
using System.Collections;
using Server;
using Server.Gumps;

namespace Knives.Utils
{
	public enum Template{ RedSquare, BlueCircle, WhiteChecker, BrownChecker, GoldBorder, StoneCross, GreenCircle, RedCircle }

	public class ButtonTemplate
	{
		private static Hashtable s_Templates = new Hashtable();

		public static Hashtable Templates{ get{ return s_Templates; } }

		public static void Initialize()
		{
			s_Templates[Template.RedSquare] = new ButtonTemplate( 0x29F6, 0, -3, 27, 0 );
			s_Templates[Template.BlueCircle] = new ButtonTemplate( 0x846, 0, -3, 26, 0 );
			s_Templates[Template.WhiteChecker] = new ButtonTemplate( 0x91B, 0, -3, 25, -2 );
			s_Templates[Template.BrownChecker] = new ButtonTemplate( 0x922, 0, -3, 25, -2 );
			s_Templates[Template.GoldBorder] = new ButtonTemplate( 0x98B, 0, -3, 2, -5 );
			s_Templates[Template.StoneCross] = new ButtonTemplate( 0x2774, 0, -3, 26, -3 );
			s_Templates[Template.GreenCircle] = new ButtonTemplate( 0x2C89, 0, -3, 26, -1 );
			s_Templates[Template.RedCircle] = new ButtonTemplate( 0x2C94, 0, -3, 26, -1 );
		}

		public static void RemoveTemplate( ButtonPlus button, Template t )
		{
			if ( (int)t == -1 )
				return;

			ButtonTemplate template = (ButtonTemplate)s_Templates[t];

			button.X = button.X - template.BGXOffset;
			button.Y = button.Y - template.BGYOffset;
		}

		public static void RemoveTemplate( HtmlPlus html, Template t )
		{
			if ( (int)t == -1 )
				return;

			ButtonTemplate template = (ButtonTemplate)s_Templates[t];

			html.X = html.X - template.TextXOffset;
			html.Y = html.Y - template.TextYOffset;
		}

		public static void ApplyTemplate( ButtonPlus button, HtmlPlus html, Template t )
		{
			ApplyTemplate( button, t );
			ApplyTemplate( html, t );
		}

		public static void ApplyTemplate( ButtonPlus button, Template t )
		{
			if ( (int)t == -1 )
				return;

			ButtonTemplate template = (ButtonTemplate)s_Templates[t];

			button.NormalID = template.Background;
			button.PressedID = template.Background;
			button.X = button.X + template.BGXOffset;
			button.Y = button.Y + template.BGYOffset;
		}

		public static void ApplyTemplate( HtmlPlus html, Template t )
		{
			if ( (int)t == -1 )
				return;

			ButtonTemplate template = (ButtonTemplate)s_Templates[t];

			html.X = html.X + template.TextXOffset;
			html.Y = html.Y + template.TextYOffset;
		}

		private int c_Background;
		private int c_TextXOffset;
		private int c_TextYOffset;
		private int c_BGXOffset;
		private int c_BGYOffset;

		public int Background{ get{ return c_Background; } }
		public int TextXOffset{ get{ return c_TextXOffset; } }
		public int TextYOffset{ get{ return c_TextYOffset; } }
		public int BGXOffset{ get{ return c_BGXOffset; } }
		public int BGYOffset{ get{ return c_BGYOffset; } }

		public ButtonTemplate( int bg, int textx, int texty, int bgx, int bgy )
		{
			c_Background = bg;
			c_TextXOffset = textx;
			c_TextYOffset = texty;
			c_BGXOffset = bgx;
			c_BGYOffset = bgy;
		}
	}
}