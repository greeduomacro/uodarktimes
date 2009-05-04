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
	public class TManageGump : TBaseGump
	{
		public TManageGump(TSystemStone stone)
			: base( stone, 204, "System Management" )
		{
			m_Stone = stone;

			this.AddButton(124, 190, btnNormID, btnPressID, (int)Buttons.btnSpells, GumpButtonType.Reply, 0);
			this.AddLabel(154, 190, 100, @"Edit Restricted Spells");
			this.AddButton(124, 220, btnNormID, btnPressID, (int)Buttons.btnSkills, GumpButtonType.Reply, 0);
			this.AddLabel(154, 220, 100, @"Edit Restricted Skills");

		}
		
		private enum Buttons
		{
			btnSpells = 1,
			btnSkills
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
				case 1: //Spells
				{
					Close(from, this);

					from.SendGump( new TSpellRestrictGump( m_Stone.RestrictedSpells ) );

					break;
				}
				case 2: //Skills
				{
					Close(from, this);

					from.SendGump( new TSkillRestrictGump( m_Stone.RestrictedSkills ) );

					break;
				}
			}

			base.OnResponse(state, info);
		}
	}
}