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
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.TSystem
{
	public class TJoinGump : Gump
	{
		private string Information = "Welcome! There are a few rules that have been placed that will be enforced throughout this tournament.";
		//Edit information at the top of the gump here ^^

		private TSystemStone m_Stone;

		public TJoinGump(TSystemStone stone, Mobile from)
			: base( 0, 0 )
		{
			m_Stone = stone;

			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(90, 61, 494, 448, 9270);
			this.AddAlphaRegion(101, 71, 471, 427);
			this.AddLabel(111, 81, 1166, @"Milt's Automated Tourney System - v" + TSystemStone.Version);
			this.AddImage(505, 78, 5581);
			this.AddImage(115, 106, 3001);
			this.AddHtml( 111, 166, 457, 201, Rules(), (bool)false, (bool)true);
			this.AddHtml( 105, 141, 447, 21, "<baseFONT COLOR='lime'><CENTER>Tournament Signups", (bool)false, (bool)false);
			this.AddImage(407, 266, 990);
			this.AddImage(406, 266, 653);

			bool signedup = SignedUp(from);

			this.AddLabel(108, 377, 36, signedup ? (m_Stone.AllowTeams && IsLeader(from) ? "Press 'Add' to add people to your team." : "You are already signed up for this tournament.") : "What would you like to do?");

			if(!signedup)
			{
				this.AddButton(128, 432, 9903, 9904, 1, GumpButtonType.Reply, 0);
				this.AddLabel(158, 432, 100, @"Join");
			}
			else
			{
				this.AddButton(128, 432, 9903, 9904, 3, GumpButtonType.Reply, 0);
				this.AddLabel(158, 432, 100, @"Quit");

				if(IsLeader(from))
				{
					this.AddButton(298, 432, 9903, 9904, 4, GumpButtonType.Reply, 0);
 					this.AddLabel(328, 432, 100, @"Add");
				}
			}

			this.AddButton(208, 432, 9903, 9904, 2, GumpButtonType.Reply, 0);
			this.AddLabel(238, 432, 100, @"Cancel");
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{ 		
			Mobile from = state.Mobile;

			switch ( info.ButtonID ) 
			{
				case 1: //Join
				{
					from.CloseGump( typeof(TJoinGump) );

					if(SignedUp(from))
					{
						from.SendMessage("You are already signed up for this tournament.");
						break;
					}

					m_Stone.VerifyJoin(from);

					break;
				}
				case 2: //Cancel
				{
					from.CloseGump( typeof(TJoinGump) );

					break;
				}
				case 3: //Quit
				{
					from.CloseGump( typeof(TJoinGump) );

					if(!SignedUp(from))
					{
						from.SendMessage("You are not signed up for this tournament.");
						break;
					}

					m_Stone.VerifyQuit(from);

					break;
				}
				case 4: //Add
				{
					from.CloseGump( typeof(TJoinGump) );

					from.Target = new TInviteTarget( m_Stone, this );
					from.SendMessage("Target the person that you wish to invite to the team.");

					break;
				}
			}
		}

		public bool SignedUp(Mobile from)
		{
			return FindTeam(from) != null;
		}

		private List<Mobile> FindTeam(Mobile from)
		{
			for(int i = 0; i < m_Stone.SignedUp.Count; ++i)
			{
				List<Mobile> team = m_Stone.SignedUp[i];

				if(team.Contains(from))
					return team;
			}

			return null;
		}

		public bool IsLeader(Mobile from)
		{
			if(m_Stone != null && m_Stone.SignedUp != null)
			{
				if(!SignedUp(from))
					return false;

				List<Mobile> team = FindTeam(from);

				if(team.Count >= 1)
					return (team[0] == from);
			}

			return false;
		}

		private string Rules()
		{
			string final = "<baseFONT COLOR='white'><CENTER>" + Information + "</CENTER><p>" + "<i>Rules:</i><p><p>";

			if(m_Stone == null)
				return final;

			string[] rules = m_Stone.GetRules();

			for(int i = 0; i < rules.Length; ++i)
				final += rules[i];

			if(m_Stone.AllowTeams)
			{
				string teams = String.Format("<p>This tournament allows teams to be created. The maximum size a team can be is {0}. To create a team, sign up for the tournament and then come back to this menu. A button called 'Add' will appear. From there, you can send join requests to people. Please note that once someone is on the team, you can not kick them off. If you leave the team, the first person that you invited will become the new team leader. If you want to join someone's team, do not sign up here, ask the team leader to invite you.", m_Stone.TeamMaxPlayers);

				final += teams;
			}

			return final;
		}

		private class TInviteTarget : Target
		{
			private static TSystemStone m_Stone;
			private static TJoinGump m_Gump;

			public TInviteTarget(TSystemStone stone, TJoinGump g) : base(12, false, TargetFlags.None)
			{
				m_Stone = stone;
				m_Gump = g;
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if(o is PlayerMobile)
				{
					PlayerMobile target = (PlayerMobile)o;

					if (target == from)
						from.SendMessage("You can't add yourself to your team.");

					else if( m_Gump.SignedUp( target ) )
						from.SendMessage( "They are already signed up for this tournament." );

					else
					{
						from.SendMessage("Sending confirmation gump...");
						target.SendMessage("You have 20 seconds to accept or decline this offer.");

						target.SendGump( new TAcceptGump(m_Stone, target, from, m_Gump) );

						Timer.DelayCall( TimeSpan.FromSeconds(20.0), new TimerStateCallback( Timesup ), new object[] { target } );
					}					
				}
				else
					from.SendMessage("You can only add players to your team.");
			}

			public static void Timesup(object state)
			{
				object[] stuff = (object[])state;

				Mobile targ = (Mobile)stuff[0];

				if(!m_Gump.SignedUp(targ))
					targ.CloseGump( typeof(TAcceptGump) );
			}
		}
	}
}