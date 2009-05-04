using System;
using Server;
using Server.Gumps;

namespace Knives.Utils
{
	public class HtmlPlus : GumpHtml
	{
		private bool c_Override;
		private Template c_Template = (Template)(-1);

		public bool Override{ get{ return c_Override; } set{ c_Override = value; } }
		public Template Template{ get{ return c_Template; } set{ c_Template = value; } }

		public HtmlPlus( int x, int y, int width, int height, string text, bool back, bool scroll ) : base( x, y, width, height, text, back, scroll )
		{
			c_Override = true;
		}

		public HtmlPlus( int x, int y, int width, int height, string text, bool back, bool scroll, bool over ) : base( x, y, width, height, text, back, scroll )
		{
			c_Override = over;
		}
	}
}