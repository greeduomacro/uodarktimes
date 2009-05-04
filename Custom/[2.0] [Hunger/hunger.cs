using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Commands;

namespace Server.Commands
{
	public class HungerCheck
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Hlad", AccessLevel.Player, new CommandEventHandler( GetHunger_OnCommand ) );
		}
		
	public static void GetHunger_OnCommand( CommandEventArgs arg )
	{
		PlayerMobile m = arg.Mobile as PlayerMobile;
				if ( m.Hunger < 5 )
					m.SendMessage( "Umiras hlady." );
				else if ( m.Hunger < 10 )
					m.SendMessage( "Mas velky hlad." );
				else if ( m.Hunger < 15 )
					m.SendMessage( "Citis se celkem najedeny." );
				else
					m.SendMessage( "Tvuj zaludek je naplnen k prasknuti." );
	}
	}
}