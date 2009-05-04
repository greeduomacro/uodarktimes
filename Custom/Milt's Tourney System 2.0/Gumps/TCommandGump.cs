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
	public class TCommandGump : TBaseGump
	{
		public TCommandGump(TSystemStone stone)
			: base( stone, 204, "System Commands" )
		{
			m_Stone = stone;

			this.AddButton(124, 190, btnNormID, btnPressID, (int)Buttons.btnStart, GumpButtonType.Reply, 0);
			this.AddLabel(154, 190, 100, @"Start the tournament");
			this.AddButton(124, 220, btnNormID, btnPressID, (int)Buttons.btnStop, GumpButtonType.Reply, 0);
			this.AddLabel(154, 220, 100, @"Stop the tournament");

		}
		
		private enum Buttons
		{
			btnStart = 1,
			btnStop,
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{ 		
			Mobile from = state.Mobile;

			switch ( info.ButtonID ) 
			{
				case 0: //Right click to close
				{
					Close(from, this);
					from.SendGump( new TMainGump( m_Stone ) );

					break;
				}
				case 1: //Start
				{
					Close(from, this);

					m_Stone.StartTournament(from);

					from.SendGump( new TCommandGump( m_Stone ) );

					break;
				}
				case 2: //Stop
				{
					Close(from, this);

					m_Stone.Stop(from);

					from.SendGump( new TCommandGump( m_Stone ) );

					break;
				}
			}

			base.OnResponse(state, info);
		}
	}
}