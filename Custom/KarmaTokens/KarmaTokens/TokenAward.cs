//Original by Dupre
//Reworked by Karmageddon
//For new token system 
//

using System; 
using System.Collections; 
using Server.Items;
using Server.Misc;

namespace Server.Mobiles 
{ 
	public class TokenValidate
	{
		private Mobile m_Owner;		
		 
		public static void TokenAward(Mobile from, BaseCreature bc)
		{		
			if ( from.Backpack == null )
				return;

			int karma = Math.Abs( bc.Karma );
			int tokenbase = ( bc.TotalGold + karma + bc.Fame + ((bc.Hits+bc.Stam+bc.Mana)/3)) / 4500;
			int maxtokens = 6 + ( 50 * tokenbase );
			int mintokens = maxtokens/100;

			int amount = Utility.Random( mintokens, maxtokens );			
			
			TokensGiven(from, amount);
		}
			
		public static void TokensGiven(Mobile from, int amount)
		{
			
			if (amount < 1)
				return;
			
			Item[] items = from.Backpack.FindItemsByType( typeof( TokenBox ) );

				foreach( TokenBox box in items )
				{					
					if ( from == box.Owner )
					{
						if (( box.Token  + amount ) <= 200000000 )
						{
							box.Token = (box.Token + amount);
							from.SendMessage( "You have received {0} tokens", amount );
							break;
					}
					else
						from.SendMessage(1173, "You have a full token box, please make a check and store it in your bank.");
					}
				}
			}
		}
	} 

