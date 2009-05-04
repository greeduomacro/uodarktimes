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
	public class TMainGump : TBaseGump
	{
		public TMainGump(TSystemStone stone)
			: base( stone, 312, "Navigation" )
		{
			m_Stone = stone;

			this.AddButton(120, 189, 9903, 9904, (int)Buttons.btnInfo, GumpButtonType.Reply, 0);
			this.AddLabel(148, 189, 100, @"Information & Statistics");
			this.AddButton(120, 249, 9903, 9904, (int)Buttons.btnCommands, GumpButtonType.Reply, 0);
			this.AddButton(120, 220, 9903, 9904, (int)Buttons.btnManagement, GumpButtonType.Reply, 0);
			this.AddLabel(148, 249, 100, @"System Commands");
			this.AddLabel(148, 220, 100, @"System Management");
			this.AddButton(120, 279, 9903, 9904, (int)Buttons.btnRegions, GumpButtonType.Reply, 0);
			this.AddLabel(148, 279, 100, @"Region Management & Editing");
			this.AddButton(120, 309, 9903, 9904, (int)Buttons.btnAbout, GumpButtonType.Reply, 0);
			this.AddLabel(148, 309, 100, @"About");
			this.AddImage(353, 133, 990);
		}
		
		private enum Buttons
		{
			btnInfo = 1,
			btnCommands,
			btnManagement,
			btnRegions,
			btnAbout
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
				case 1: //Info
				{
					Close(from, this);
					from.SendGump( new TInfoGump( m_Stone, 1 ) );
					break;
				}
				case 2: //Commands
				{
					Close(from, this);
					from.SendGump( new TCommandGump( m_Stone ) );
					break;
				}
				case 3: //Management
				{
					Close(from, this);
					from.SendGump( new TManageGump( m_Stone ) );
					break;
				}
				case 4: //Regions
				{
					Close(from, this);
					from.SendGump( new TRegionGump( m_Stone ) );
					break;
				}
				case 5: //About
				{
					Close(from, this);
					from.SendGump( new TAboutGump( m_Stone ) );
					break;
				}
			}

			base.OnResponse(state, info);
		}
	}
}