using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;

namespace Knives.Utils
{
	public delegate void GumpCallback();
	public delegate void GumpStateCallback( object obj );

	public class GumpPlus : Gump
	{
		private Mobile c_Owner;
		private Hashtable c_Buttons, c_Fields;
		private bool c_Override;

		public Mobile Owner{ get{ return c_Owner; } }
		public bool Override{ get{ return c_Override; } set{ c_Override = value; } }

		public GumpPlus( Mobile m, int x, int y ) : base( x, y )
		{
			c_Owner = m;

			c_Buttons = new Hashtable();
			c_Fields = new Hashtable();
			c_Override = true;
		}

		public void Clear()
		{
			Entries.Clear();
			c_Buttons.Clear();
			c_Fields.Clear();
		}

		public void NewGump()
		{
			NewGump( true );
		}

		public void NewGump( bool clear )
		{
			if ( clear )
				Clear();

			BuildGump();

			if ( c_Override )
				ModifyGump();

			c_Owner.SendGump( this );
		}

		public void SameGump()
		{
			c_Owner.SendGump( this );
		}

		protected virtual void BuildGump()
		{
		}

		private void ModifyGump()
		{try{

			AddPage( 0 );

			int maxWidth = 0;
			int maxHeight = 0;
			GumpBackground bg;

			foreach( GumpEntry entry in Entries )
				if ( entry is GumpBackground )
				{
					bg = (GumpBackground)entry;

					if ( bg.X + bg.Width > maxWidth )
						maxWidth = bg.X + bg.Width;
					if ( bg.Y + bg.Height > maxHeight )
						maxHeight = bg.Y + bg.Height;
				}

			AddImage( maxWidth, maxHeight, 0x28DC, 0x387 );
			AddItem( maxWidth, maxHeight, 0xFC1 );
			AddButton( maxWidth+2, maxHeight+16, 0x93A, 0x93A, "Gump Art", new TimerCallback( GumpArt ) );

			if ( !GumpInfo.HasMods( c_Owner, GetType() ) )
				return;

			GumpInfo info = GumpInfo.GetInfo( c_Owner, GetType() );
			ArrayList backs = new ArrayList();

			foreach( GumpEntry entry in new ArrayList( Entries ) )
			{
				if ( entry is GumpBackground )
				{
					if ( info.Background != -1 )
						((GumpBackground)entry).GumpID = info.Background;

					backs.Add( entry );
				}
				else if ( entry is GumpAlphaRegion && !info.DefaultTrans && !info.Transparent )
				{
					((GumpAlphaRegion)entry).Width = 0;
					((GumpAlphaRegion)entry).Height = 0;
				}
				else if ( entry is HtmlPlus )
				{
					if ( (int)((HtmlPlus)entry).Template != -1 )
					{
						ButtonTemplate.RemoveTemplate( (HtmlPlus)entry, ((HtmlPlus)entry).Template );
						ButtonTemplate.ApplyTemplate( (HtmlPlus)entry, info.Template );
					}

					if ( !((HtmlPlus)entry).Override || info.TextColorRGB == "" )
						continue;

					string text = ((HtmlPlus)entry).Text;
					int num = 0;
					int length = 0;
					char[] chars;

					if ( text == null )
						continue;

					while( (num = text.ToLower().IndexOf( "<basefont" )) != -1 || (num = text.ToLower().IndexOf( "</font" )) != -1 )
					{
						length = 0;
						chars = text.ToCharArray();

						for( int i = num; i < chars.Length; ++i )
							if ( chars[i] == '>' )
							{
								length = i-num+1;
								break;
							}

						if ( length == 0 )
							break;

						text = text.Substring( 0, num ) + text.Substring( num+length, text.Length-num-length );
					}

					((HtmlPlus)entry).Text = info.TextColor + text;
				}
				else if ( entry is ButtonPlus && (int)((ButtonPlus)entry).Template != -1 )
				{
					ButtonTemplate.RemoveTemplate( (ButtonPlus)entry, ((ButtonPlus)entry).Template );
					ButtonTemplate.ApplyTemplate( (ButtonPlus)entry, info.Template );
				}
			}

			if ( !info.DefaultTrans && info.Transparent )
				foreach( GumpBackground back in backs )
					AddAlphaRegion( back.X, back.Y, back.Width, back.Height );

			SortEntries();

		}catch{ Errors.Report( "GumpPlus-> ModifyGump-> " + GetType() ); } }

		private void SortEntries()
		{
			ArrayList list = new ArrayList();

			foreach( GumpEntry entry in new ArrayList( Entries ) )
				if ( entry is GumpBackground )
				{
					list.Add( entry );
					Entries.Remove( entry );
				}

			foreach( GumpEntry entry in new ArrayList( Entries ) )
				if ( entry is GumpAlphaRegion )
				{
					list.Add( entry );
					Entries.Remove( entry );
				}

			list.AddRange( Entries );

			Entries.Clear();
            foreach (GumpEntry entry in list)
                Entries.Add(entry);
        }

		private int UniqueButton()
		{
			int random = 0;

			do
			{
				random = Utility.Random( 20000 );

			}while( c_Buttons[random] != null );

			return random;
		}

		public void AddButton( int x, int y, int up, int down, TimerCallback callback )
		{
			AddButton( x, y, up, down, "None", callback );
		}

		public void AddButton( int x, int y, int up, int down, string name, TimerCallback callback )
		{
			int id = UniqueButton();

			ButtonPlus button = new ButtonPlus( x, y, up, down, id, name, callback );

			Add( button );

			c_Buttons[id] = button;
		}

		public void AddButton( int x, int y, int up, int down, TimerStateCallback callback, object arg )
		{
			AddButton( x, y, up, down, "None", callback, arg );
		}

		public void AddButton( int x, int y, int up, int down, string name, TimerStateCallback callback, object arg )
		{
			int id = UniqueButton();

			ButtonPlus button = new ButtonPlus( x, y, up, down, id, name, callback, arg );

			Add( button );

			c_Buttons[id] = button;
		}

		public void AddTemplateButton( int x, int y, int w, Template t, string name, string text, TimerCallback callback )
		{
			AddTemplateButton( x, y, w, t, name, text, callback, true );
		}

		public void AddTemplateButton( int x, int y, int w, Template t, string name, string text, TimerCallback callback, bool over )
		{
			int id = UniqueButton();

			ButtonPlus button = new ButtonPlus( x, y, 0x0, 0x0, id, name, callback );
			button.Template = t;
			Add( button );
			c_Buttons[id] = button;

			HtmlPlus html = new HtmlPlus( x, y, w, 21, text, false, false, over );
			html.Template = t;
			Add( html );

			ButtonTemplate.ApplyTemplate( button, html, t );
		}

		public void AddTemplateButton( int x, int y, int w, Template t, string name, string text, TimerStateCallback callback, object arg )
		{
			AddTemplateButton( x, y, w, t, name, text, callback, arg, true );
		}

		public void AddTemplateButton( int x, int y, int w, Template t, string name, string text, TimerStateCallback callback, object arg, bool over )
		{
			int id = UniqueButton();

			ButtonPlus button = new ButtonPlus( x, y, 0x0, 0x0, id, name, callback, arg );
			button.Template = t;
			Add( button );
			c_Buttons[id] = button;

			HtmlPlus html = new HtmlPlus( x, y, w, 21, text, false, false, over );
			html.Template = t;
			Add( html );

			ButtonTemplate.ApplyTemplate( button, html, t );
		}

		public new void AddHtml( int x, int y, int width, int height, string text, bool back, bool scroll )
		{
			AddHtml( x, y, width, height, text, back, scroll, true );
		}

		public void AddHtml( int x, int y, int width, int height, string text, bool back, bool scroll, bool over )
		{
			HtmlPlus html = new HtmlPlus( x, y, width, height, text, back, scroll, over );

			Add( html );
		}

		public void AddTextField( int x, int y, int width, int height, int color, int id, string text )
		{
			base.AddTextEntry( x, y, width, height, color, id, text );

			c_Fields[id] = text;
		}

		public string GetTextField( int id )
		{
			if ( c_Fields[id] == null )
				return "";

			return c_Fields[id].ToString();
		}

		protected virtual void OnClose()
		{
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			string name = "";

			try{

			if ( info.ButtonID == -5 )
			{
				NewGump();
				return;
			}

			foreach( TextRelay t in info.TextEntries )
				c_Fields[t.EntryID] = t.Text;

			if ( info.ButtonID == 0 )
				OnClose();

			if ( c_Buttons[info.ButtonID] == null || !(c_Buttons[info.ButtonID] is ButtonPlus) )
				return;

			name = ((ButtonPlus)c_Buttons[info.ButtonID]).Name;

			((ButtonPlus)c_Buttons[info.ButtonID]).Invoke();

		}catch{ Errors.Report( String.Format( "GumpPlus-> OnResponse-> |{0}|-> {1}-> {2}", c_Owner, GetType(), name ) ); } }

		private void GumpArt()
		{
			OverrideGump.SendTo( Owner, GetType(), new TimerCallback( NewGump ) );
		}
	}
}