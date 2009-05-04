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
using Server.Spells;
using Server.Network;
using System.Collections;

public enum TRestrictType
{
	Spells,
	Skills
}

namespace Server.Gumps
{
	public abstract class TRestrictGump : Gump
	{
		private BitArray m_Restricted;

		private TRestrictType m_type;

		public TRestrictGump( BitArray ba, TRestrictType t ) : base( 50, 50 )
		{
			m_Restricted = ba;
			m_type = t;

			Closable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);

			AddBackground(10, 10, 225, 425, 9390);
			AddLabel(73, 15, 1152, (t == TRestrictType.Spells) ? @"Restrict Spells" : @"Restrict Skills" );
			AddButton(91, 411, 2122, 2123, 1, GumpButtonType.Reply, 0);

			int itemsThisPage = 0;
			int nextPageNumber = 1;
		    
			Object[] ary;// = (t == TRestrictType.Skills) ? SkillInfo.Table : SpellRegistry.Types;

			if( t == TRestrictType.Skills )
				ary = SkillInfo.Table;
			else
				ary = SpellRegistry.Types;


			for( int i = 0; i < ary.Length; i++ )
			{
				if( ary[i] != null )
				{
					if( itemsThisPage >= 8 || itemsThisPage == 0)
					{
						itemsThisPage = 0;

						if( nextPageNumber != 1)
						{
							AddButton(190, 412, 9903, 9904, 2, GumpButtonType.Page, nextPageNumber);
						}

						AddPage( nextPageNumber++ );

						if( nextPageNumber != 2)
						{
							AddButton(29, 412, 9909, 9910, 3, GumpButtonType.Page, nextPageNumber-2);
						}
					}

					AddCheck(40, 55 + ( 45 * itemsThisPage ), 210, 211, ba[i], i + ((t == TRestrictType.Spells) ? 100 : 500) );
					AddLabel(70, 55 + ( 45 * itemsThisPage ) , 0, ((t == TRestrictType.Spells) ? ((Type)(ary[i])).Name : ((SkillInfo)(ary[i])).Name ));
	
					itemsThisPage++;                    
				}
			}	
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if( info.ButtonID == 1 )
			{
				for( int i = 0; i < m_Restricted.Length; i++ )
				{
					m_Restricted[ i ] = info.IsSwitched( i + ((m_type == TRestrictType.Spells) ? 100 : 500 ));
				}
			}
		}
	}

	public class TSpellRestrictGump : TRestrictGump
	{
		public TSpellRestrictGump( BitArray ba ) : base( ba, TRestrictType.Spells )
		{

		}
	}

	public class TSkillRestrictGump : TRestrictGump
	{
		public TSkillRestrictGump( BitArray ba ) : base( ba, TRestrictType.Skills )
		{

		}
	}
}