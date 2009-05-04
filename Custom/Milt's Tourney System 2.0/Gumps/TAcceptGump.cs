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
	public class TAcceptGump : Gump
	{
		private TSystemStone m_Stone;
		private TJoinGump m_Gump;
		private Mobile m_Sender;

		public TAcceptGump(TSystemStone stone, Mobile target, Mobile from, TJoinGump g)
			: base( 0, 0 )
		{
			m_Stone = stone;
			m_Sender = from;
			m_Gump = g;

			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(152, 167, 295, 134, 9270);
			this.AddAlphaRegion(163, 177, 273, 113);
			this.AddHtml( 162, 194, 274, 20, "<baseFONT color='lime'><CENTER>" + from.Name, (bool)false, (bool)false);
			this.AddLabel(168, 211, 100, @"has requested that you join their team for");
			this.AddLabel(193, 232, 100, @"the tournament. Will you accept?");
			this.AddButton(227, 259, 247, 248, (int)Buttons.btnOK, GumpButtonType.Reply, 0);
			this.AddButton(305, 259, 242, 241, (int)Buttons.btnCancel, GumpButtonType.Reply, 0);
		}
		
		public enum Buttons
		{
			btnOK = 1,
			btnCancel
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{ 		
			Mobile from = state.Mobile;

			switch ( info.ButtonID ) 
			{
				case 1: //OK
				{
					from.CloseGump( typeof(TAcceptGump) );

					if(m_Gump.SignedUp(from))
					{
						from.SendMessage("Sorry, you are already signed up for the tournament.");
						break;
					}

					m_Stone.VerifyTeamJoin(from, m_Sender);

					break;
				}
				case 2: //Cancel
				{
					from.CloseGump( typeof(TAcceptGump) );
	
					m_Sender.SendMessage(String.Format("{0} does not wish to join the team.", from.Name));
					from.SendMessage("You decide not to join the team.");

					break;
				}
			}
		}
	}
}