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
using System.Collections.Generic;

using Server;
using Server.Spells;
using Server.Mobiles;
using Server.Regions;
using Server.Items;

namespace Server.TSystem
{
	public class TArenaRegion : GuardedRegion
	{
		private TSystemStone m_Stone;

		public TArenaRegion( TSystemStone m, Map map, params Rectangle3D[] area ) : base( "Tournament Arena", map, DefaultPriority, area )
		{
            		m_Stone = m;
		}

		/*public override bool AllowBeneficial( Mobile from, Mobile target )
		{
			Match fromMatch = m_Stone.FindMatch(from);
			Match targetMatch = m_Stone.FindMatch(target);

			if(fromMatch != null && targetMatch != null)
			{
				if(fromMatch == targetMatch)
				{
					List<Mobile> fromTeam = m_Stone.FindTeam(from);
					List<Mobile> targetTeam = m_Stone.FindTeam(target);

					if(fromTeam != null && targetTeam != null)
					{
						if(fromTeam != targetTeam)
						{
							from.SendMessage("You may not heal the enemy.");
							return false;
						}
					}
				}
			}

			return true; //Returns true so factions won't interfere
		}

		public override bool AllowHarmful( Mobile from, Mobile target )
		{
			Match fromMatch = m_Stone.FindMatch(from);
			Match targetMatch = m_Stone.FindMatch(target);

			if(fromMatch != null && targetMatch != null)
			{
				if(fromMatch == targetMatch)
				{
					List<Mobile> fromTeam = m_Stone.FindTeam(from);
					List<Mobile> targetTeam = m_Stone.FindTeam(target);

					if(fromTeam != null && targetTeam != null)
					{
						if(fromTeam == targetTeam)
						{
							from.SendMessage("You may not harm your team mate.");
							return false;
						}
					}
				}
			}

			return true; //Returns true so there is no interference
		}*/

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override bool CanUseStuckMenu( Mobile from )
		{
			from.SendMessage( "You may not use the stuck menu here." );

			return false;
		}

		public override bool OnBeginSpellCast( Mobile from, ISpell s )
		{
			if( from.AccessLevel == AccessLevel.Player )
			{
				bool restricted = m_Stone.IsRestrictedSpell( s );

				if(restricted)
				{
					from.SendMessage( "You may not cast that spell here." ); 
					return false;
				}

				if( ((Spell)s).Info.Name == "Ethereal Mount" )
				{
					from.SendMessage( "You may not mount your ethereal here." );
					return false; 
				}
			}

			return true;
		}

		public override bool OnHeal( Mobile from, ref int Heal )
		{
			if( !m_Stone.CanHeal )
				from.SendMessage("You cannot be healed here.");

			return m_Stone.CanHeal;
		}

		public override bool OnSkillUse( Mobile from, int skill )
		{
			bool restricted = m_Stone.IsRestrictedSkill( skill );

			if( restricted && from.AccessLevel == AccessLevel.Player )
			{
				from.SendMessage( "You may not use that skill here." ); 
				return false;
			}

			return base.OnSkillUse( from, skill );
		}
		
		public override bool OnMoveInto( Mobile from, Direction d, Point3D newLocation, Point3D oldLocation )
		{
			if( !this.Contains( oldLocation ) )
			{
				from.SendMessage( "You may not enter this area." );
				return false; 
			}

			return true;
		}

		public override bool OnDoubleClick(Mobile from, object o)
		{
			if(!m_Stone.Potions && o is BasePotion)
			{
				from.SendMessage("Potions have been restricted for this tournament.");
				return false;
			}

			return base.OnDoubleClick(from, o);
		}
	}
}
