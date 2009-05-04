/***********************************************
*
* This script was made by milt, AKA Pokey.
*
* Email: pylon2007@gmail.com
*
* AIM: TrueBornStunna
*
* Website: www.pokey.f13nd.net
*
* Version: 2.0.0
*
* Release Date: June 29, 2006
*
************************************************/
using System;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.TSystem
{
	public class TBaseGump : Gump
	{
		protected readonly int btnNormID  = 9903;
		protected readonly int btnPressID = 9904;

		protected TSystemStone m_Stone;

		public TBaseGump(TSystemStone stone, int size, string header) : base(0,0)
		{
			m_Stone = stone;

			this.AddHtml(105, 141, 447, 21, "<CENTER><baseFONT color='lime'>" + header, (bool)false, (bool)false);
			//this.AddHtml( 105, 166, 463, 327, info, (bool)false, (bool)true);
			InitializeComponent(size);
		}

		private void InitializeComponent(int size)
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(90, 61, 494, size, 9270); //448 default size.
			this.AddAlphaRegion(101, 71, 471, size - 21);
			this.AddLabel(111, 81, 1166, @"Milt's Automated Tourney System - v" + TSystemStone.Version);
			this.AddButton(505, 78, 5581, 5581, (int)Buttons.btnHome, GumpButtonType.Reply, 0);
			this.AddImage(115, 106, 3001);
		}

		private enum Buttons
		{
			btnHome = 1337
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{ 		
			Mobile from = state.Mobile;

			switch ( info.ButtonID ) 
			{
				case 0: //Right click to close
				{
					break;
				}

				case 1337: //Home
				{
					Close(from, this);
					from.SendGump( new TMainGump( m_Stone ) );

					break;
				}
			}
		}

		public void CloseAll(Mobile from)
		{
			from.CloseGump( typeof(TBaseGump) );
		}

		public void Close(Mobile from, object obj)
		{
			Close(from, obj.GetType());
		}

		public void Close(Mobile from, Type type)
		{
			from.CloseGump(type);
		}
	}
}