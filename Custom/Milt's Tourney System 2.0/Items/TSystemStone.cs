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
using System.Net;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Second;
using Server.Spells.Third;

namespace Server.TSystem
{
	public class TSystemStone : Item
	{
		public static readonly string Version = "2.0";

		private Match[] GameTable;
		private List<List<Mobile>> m_SignedUp;

		private TArenaRegion m_Region;
		private List<Rectangle3D> m_Coords;

		private bool m_Enabled;
		private bool m_Started;

		private BitArray m_RestrictedSpells;
		private BitArray m_RestrictedSkills;

		public BitArray RestrictedSpells{ get{ return m_RestrictedSpells; } set{ m_RestrictedSpells = value; } }
		public BitArray RestrictedSkills{ get{ return m_RestrictedSkills; } set{ m_RestrictedSkills = value; } }

		#region Tournament Options

		private bool m_CanHeal;
		private bool m_Potions;
		private bool m_MagicWeapons;
		private bool m_AllowSameIP;
		private bool m_AllowTeams;

		private int m_TeamMaxPlayers;

		private Point3D m_SpectatorDest;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Enabled{ get{ return m_Enabled; } set{ m_Enabled = value; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanHeal{ get{ return m_CanHeal; } set{ m_CanHeal = value; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Potions{ get{ return m_Potions; } set{ m_Potions = value; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public bool MagicWeapons{ get{ return m_MagicWeapons; } set{ m_MagicWeapons = value; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowSameIP{ get{ return m_AllowSameIP; } set{ m_AllowSameIP = value; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowTeams{ get{ return m_AllowTeams; } set{ m_AllowTeams = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int TeamMaxPlayers{ get{ return m_TeamMaxPlayers; } set{ m_TeamMaxPlayers = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D SpectatorDest{ get{ return m_SpectatorDest; } set{ m_SpectatorDest = value; } }

		#endregion

		private static TSystemStone StaticRef;

		public List<Rectangle3D> Coords{ get{ return m_Coords; } set{ m_Coords = value; } }
		public List<List<Mobile>> SignedUp{ get{ return m_SignedUp; } set{ m_SignedUp = value; } }

		public bool Started{ get{ return m_Started; } }

		public static void Initialize()
		{
			Console.Write("Milt's Fully Automated Tournament System...");

			EventSink.Disconnected += new DisconnectedEventHandler( TSystem_OnDisconnect );
			EventSink.PlayerDeath += new PlayerDeathEventHandler( TSystem_OnDeath );

			Notoriety.Handler = new NotorietyHandler( TSystem_Notoriety );
			Mobile.AllowBeneficialHandler = new AllowBeneficialHandler( TSystem_AllowBeneficial );
			Mobile.AllowHarmfulHandler = new AllowHarmfulHandler( TSystem_AllowHarmful );

			if(StaticRef != null)
			{
				StaticRef.m_SignedUp = new List<List<Mobile>>();
				StaticRef.UpdateRegion();
			}

			Console.WriteLine("Initialized");
		}

		[Constructable]
		public TSystemStone() : base(0xEDD)
		{
			m_SignedUp = new List<List<Mobile>>();
			m_Coords = new List<Rectangle3D>();

			StaticRef = this;

			Movable = false;
			Visible = false;
			Name = "Tournament System Control Stone";

			UpdateRegion();

			m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );
			m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );

			m_CanHeal = true;
			m_Potions = true;
			m_MagicWeapons = true;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if(from.AccessLevel > AccessLevel.Counselor)
			{
				from.CloseGump( typeof( TMainGump ) );
				from.SendGump( new TMainGump(this) );
			}

			base.OnDoubleClick(from);
		}
		
		private int CalculateMatches(int playercount)
		{
			return (playercount / 16 + 1) * 8;
		}

		private bool CanStart()
		{
			if(m_Coords.Count == 0)			//Must have at least one arena
				return false;
			if(m_SignedUp.Count == 0)		//Must have someone signed up
				return false;
			if(!m_Enabled)				//System must be enabled
				return false;
			if(m_Started)				//May not be already running
				return false;
			if(m_SpectatorDest == Point3D.Zero) 	//Spectator dest must be set
				return false;

			return true;
		}

		public void StartTournament(Mobile starter)
		{
			if(!CanStart())
			{
				starter.SendMessage("The tournament can not be started. Please check all settings, and make sure that there is at least one person signed up.");
				return;
			}

			m_Started = true;

			int NumberOfMatches = CalculateMatches(m_SignedUp.Count);
			GameTable = new Match[NumberOfMatches];

			ShuffleList(m_SignedUp);
			FillTable();

			Round_NewMatch();
		}

		private void Tournament_OnEnd(Match match)
		{
			string names = "";
			bool multi = match.Winners.Count > 1;

			for(int i = 0; i < match.Winners.Count; ++i)
				names += match.Winners[i].Name + (i == match.Winners.Count - 1 ? "" : ", ");

			World.Broadcast( 1154, true, String.Format("The tournament is now over. The winner{0} {1}: {2}", multi ? "s" : "", multi ? "were" : "was", names) );

			GameTable = null;
			m_SignedUp = new List<List<Mobile>>();
			m_Started = false;
		}

		private void Round_NewMatch()
		{
			if( AvailableArena() )
			{
				Match match = FindWaitingMatch();

				if(match != null)
				{
					Rectangle3D open = FindOpenArena();

					if(!RectangleNull(open))
						PrepMatch(match, open);
				}
			}
		}

		private void Match_OnEnd(Match match, List<Mobile> losers)
		{
			if(losers == match.Attackers)
				match.Winners = match.Defenders;
			else
				match.Winners = match.Attackers;

			match.MatchStatus = MatchStatus.Over;

			if(match.Attackers != null && match.Defenders != null)
			{
				//Prepare attackers
				for(int i = 0; i < match.Attackers.Count; ++i)
					HandleMatchEnd(match.Attackers[i], true);

				//Prepare defenders
				for(int i = 0; i < match.Defenders.Count; ++i)
					HandleMatchEnd(match.Defenders[i], true);
			}

			NullifyEmptyTeams();

			if(Round_IsOver())
			{
				if(GameTable.Length > 1)
					RefactorTable();
				else //FINAL ROUND
				{
					Tournament_OnEnd(match);
					return;
				}
			}

			Round_NewMatch();
		}

		private void Match_OnEnd(Match match) //Called by "PrepMatch()" when there is a null fight or a "bye"
		{
			List<Mobile> losers;

			//Find the losers
			if(match.Winners == match.Attackers)
				losers = match.Defenders;
			else
				losers = match.Attackers;

			Match_OnEnd(match, losers);
		}

		public static void TSystem_OnDisconnect( DisconnectedEventArgs e )
		{
			if(StaticRef != null && StaticRef.Started)
			{
				if(StaticRef.IsParticipant(e.Mobile))
				{
					if(StaticRef.FindMatch(e.Mobile).MatchStatus == MatchStatus.Fighting)
						e.Mobile.Kill(); //Mobile will later be removed from tournament because netstate is null
				}
			}
		}

		public static void TSystem_OnDeath(PlayerDeathEventArgs e)
		{
			if(StaticRef != null)
			{
				if(StaticRef.Started) //Adding this also made a huge memory save.
				{
					Match current = StaticRef.FindMatch(e.Mobile);

					if(current != null) //Player is currently involved in tournament
					{
						if(current.MatchStatus == MatchStatus.Fighting) //Make sure they are fighting
						{
							if(StaticRef.Mob_TeamDefeated(e.Mobile))
								StaticRef.Match_OnEnd(current, StaticRef.FindTeam(e.Mobile));
						}
					}
				}
			}
		}

		public static bool TSystem_AllowBeneficial(Mobile from, Mobile target)
		{
			if(StaticRef != null)
			{
				if(StaticRef.Started)
				{
					if(StaticRef.IsParticipant(from) || StaticRef.IsParticipant(target))
					{
						if(StaticRef.AreAllies(from, target))
							return true;
						else
							return false;
					}
				}
			}

			return NotorietyHandlers.Mobile_AllowBeneficial(from, target);
		}
		
		public static bool TSystem_AllowHarmful( Mobile from, Mobile target )
		{
			if(StaticRef != null)
			{
				if(StaticRef.Started)
				{
					if(StaticRef.IsParticipant(from) || StaticRef.IsParticipant(target))
					{
						if(StaticRef.AreOpponents(from, target))
							return true;
						else
							return false;
					}
				}
			}

			return NotorietyHandlers.Mobile_AllowHarmful(from, target);
		}

		public static int TSystem_Notoriety( Mobile source, Mobile target )
		{
			if(StaticRef != null)
			{
				if(StaticRef.Started)
				{
					if(StaticRef.IsParticipant(source) || StaticRef.IsParticipant(target))
					{
						if(StaticRef.AreOpponents(source, target))
							return Notoriety.Enemy;
						else if(StaticRef.AreAllies(source, target))
							return Notoriety.Ally;
						else
							return Notoriety.Invulnerable;
					}
				}
			}

			return NotorietyHandlers.MobileNotoriety(source, target);
		}

		private bool AreOpponents(Mobile from, Mobile target)
		{
			Match fromMatch = FindMatch(from);
			Match targMatch = FindMatch(target);

			if(fromMatch != null && targMatch != null && fromMatch == targMatch && fromMatch.MatchStatus == MatchStatus.Fighting)
			{
				List<Mobile> fromTeam = FindTeam(from);
				List<Mobile> targTeam = FindTeam(target);

				if(fromTeam == null || targTeam == null)
					return false;

				return !(fromTeam == targTeam);				
			}

			return false;
		}

		private bool AreAllies(Mobile from, Mobile target)
		{
			Match fromMatch = FindMatch(from);
			Match targMatch = FindMatch(target);

			if(fromMatch != null && targMatch != null && fromMatch == targMatch && fromMatch.MatchStatus == MatchStatus.Fighting)
			{
				List<Mobile> fromTeam = FindTeam(from);
				List<Mobile> targTeam = FindTeam(target);

				if(fromTeam == null || targTeam == null)
					return false;

				return (fromTeam == targTeam);				
			}

			return false;
		}

		public bool IsParticipant(Mobile m)
		{
			if(GameTable == null)
				return false;

			for(int i = 0; i < GameTable.Length; ++i)
			{
				Match current = GameTable[i];

				if(current == null)
				{
					Console.WriteLine("Error: IsParticipant() found null match. Index: {0}", i); //Debugging Purposes
					continue;
				}

				if( (current.Attackers != null && current.Attackers.Contains(m)) || (current.Defenders != null && current.Defenders.Contains(m)) )
					return true;
			}

			return false;
		}

		public Match FindMatch(Mobile m)
		{
			if(GameTable == null)
				return null;

			for(int i = 0; i < GameTable.Length; ++i)
			{
				Match current = GameTable[i];

				if(current == null)
				{
					Console.WriteLine("Error: FindMatch() found null match. Index: {0}", i); //Debugging Purposes
					continue;
				}

				if( (current.Attackers != null && current.Attackers.Contains(m)) || (current.Defenders != null && current.Defenders.Contains(m)) )
					return current;
			}

			return null;
		}

		public List<Mobile> FindTeam(Mobile m)
		{
			Match current = FindMatch(m);

			if(current != null)
			{
				if(current.Attackers.Contains(m))
					return current.Attackers;
				if(current.Defenders.Contains(m))
					return current.Defenders;
			}

			return null;
		}

		private Match FindWaitingMatch()
		{
			for(int i = 0; i < GameTable.Length; ++i)
			{
				Match current = GameTable[i];

				if(current == null)
				{
					Console.WriteLine("Error: Match was null. Index: {0}", i); //Debugging Purposes
					continue;
				}

				if(current.MatchStatus == MatchStatus.Waiting) //Would be the First match that is waiting to be fought
				{
					return current;
				}
			}

			return null;
		}

		private bool Mob_TeamDefeated(Mobile from)
		{
			Match current = FindMatch(from);

			if(current != null)
			{
				List<Mobile> team = FindTeam(from);

				if(team != null)
				{
					int DeadCount = 0;

					for(int i = 0; i < team.Count; ++i) //Loop through team and see if everyone is dead
					{
						if(!team[i].Alive)
							++DeadCount;
					}

					if(DeadCount == team.Count) //Everyone is dead
						return true;
				}
			}

			return false;
		}

		private bool RectangleEqual(Rectangle3D one, Rectangle3D two)
		{
			if(one.Start == two.Start && one.End == two.End)
				return true;

			return false;
		}

		private bool RectangleNull(Rectangle3D from)
		{
			if(from.Width == 0 && from.Height == 0 && from.Depth == 0)
				return true;

			return false;
		}

		private void PrepMatch(Match m, Rectangle3D arena)
		{
			if(!CheckFight(m)) //Checks for nulls in Matches
			{
				Match_OnEnd(m);
				return;
			}

			m.MatchStatus = MatchStatus.Fighting;

			//Prepare attackers
			for(int i = 0; i < m.Attackers.Count; ++i)
			{
				PrepFighter(m.Attackers[i]); //Res -> Unmount -> AntiCheat
				m.Attackers[i].Location = GetLocation(arena, true);
			}
			//Prepare defenders
			for(int i = 0; i < m.Defenders.Count; ++i)
			{
				PrepFighter(m.Defenders[i]); //Res -> Unmount -> AntiCheat
				m.Defenders[i].Location = GetLocation(arena, false);
			}

			Item wall = new WallOfStoneEast();
			wall.MoveToWorld(Middle(arena), this.Map);

			Timer.DelayCall(TimeSpan.FromSeconds(10.0),new TimerStateCallback( Start_CallBack ), new object[]{ m, wall });

			Round_NewMatch(); //Check to see if any more matches can take place
		}

		public static void Start_CallBack(object state)
		{
			object[] states = (object[])state;

			Match m = (Match)states[0];
			Item wall = (Item)states[1];

			wall.Delete();

			if(StaticRef.m_Started)
			{
				//Prepare attackers
				for(int i = 0; i < m.Attackers.Count; ++i)
					StaticRef.FinalPrep(m.Attackers[i]);
				//Prepare defenders
				for(int i = 0; i < m.Defenders.Count; ++i)
					StaticRef.FinalPrep(m.Defenders[i]);
			}
		}

		public void FinalPrep(Mobile from)
		{
			if(!from.Alive)
				from.Resurrect();

			from.CurePoison(from);
			from.Blessed = false;
			from.Frozen = false;

			from.StatMods.Clear();
			from.Hits = from.HitsMax;
			from.Mana = from.ManaMax;
			from.Stam = from.StamMax;

			Targeting.Target.Cancel( from );
			from.MagicDamageAbsorb = 0;
			from.MeleeDamageAbsorb = 0;
			ProtectionSpell.Registry.Remove( from );
			DefensiveSpell.Nullify( from );
			from.Combatant = null;

			from.Delta( MobileDelta.Noto ); //Update notoriety
		}

		private Point3D GetLocation(Rectangle3D arena, bool attackers)
		{
			Point3D middle = Middle(arena);

			return new Point3D( middle.X, (attackers ? middle.Y + 2 : middle.Y - 2), middle.Z );
		}

		public Point3D Middle(Rectangle3D from)
		{
			int wMid = from.Start.X + (from.Width / 2);
			int hMid = from.Start.Y + (from.Height / 2);

			return new Point3D( wMid, hMid, from.Start.Z );
		}

		private void NullifyEmptyTeams()
		{
			for(int i = 0; i < GameTable.Length; ++i)
			{
				Match current = GameTable[i];

				if(current.Attackers != null && current.Attackers.Count == 0)
					current.Attackers = null;
				if(current.Defenders != null && current.Defenders.Count == 0)
					current.Defenders = null;
			}
		}

		private void PrepFighter(Mobile from)
		{
			if(!from.Alive)
				from.Resurrect();

			if(from.Mount != null)
				from.Mount.Rider = null;

			from.CurePoison(from);
			from.Blessed = true;
			from.Frozen = true;

			from.StatMods.Clear();
			from.Hits = from.HitsMax;
			from.Mana = from.ManaMax;
			from.Stam = from.StamMax;

			Targeting.Target.Cancel( from );
			from.MagicDamageAbsorb = 0;
			from.MeleeDamageAbsorb = 0;
			ProtectionSpell.Registry.Remove( from );
			DefensiveSpell.Nullify( from );
			from.Combatant = null;

			from.Delta( MobileDelta.Noto ); //Update notoriety

			from.SendMessage("The fight will begin in ten seconds.");
		}

		private void HandleMatchEnd(Mobile from, bool kick)
		{
			if(!from.Alive)
			{
				from.Resurrect();

				if(from.Corpse != null && from.Corpse is Corpse)
				{
					Corpse c = (Corpse)from.Corpse;

					for ( int i = 0; i < c.Items.Count; ++i )
					{
						Item item = c.Items[i];

						c.SetRestoreInfo( item, item.Location );
					}

					c.Open(from, true);
					c.Delete();
				}
			}

			from.Criminal = false;
			from.Blessed = false;
			from.Frozen = false;

			from.CurePoison(from);
			from.StatMods.Clear();
			from.Hits = from.HitsMax;
			from.Mana = from.ManaMax;
			from.Stam = from.StamMax;

			Targeting.Target.Cancel( from );
			from.MagicDamageAbsorb = 0;
			from.MeleeDamageAbsorb = 0;
			ProtectionSpell.Registry.Remove( from );
			DefensiveSpell.Nullify( from );
			from.Combatant = null;

			from.Delta( MobileDelta.Noto ); //Update notoriety

			from.Location = m_SpectatorDest;

			if(kick && from.NetState == null)
			{
				Match m = FindMatch(from);

				if(m.Attackers.Contains(from))
					m.Attackers.Remove(from);
				else if(m.Defenders.Contains(from))
					m.Defenders.Remove(from);
			}
		}

		private bool CheckFight(Match m)
		{
			if(m.Attackers == null && m.Defenders != null)
			{
				m.Winners = m.Defenders;
				return false;
			}
			if(m.Attackers != null && m.Defenders == null)
			{
				m.Winners = m.Attackers;
				return false;
			}
			if(m.Attackers == null && m.Defenders == null)
			{
				m.Winners = null;
				return false;
			}

			return true;
		}

		private Match[] GetCurrentMatches()
		{
			List<Match> temp = new List<Match>();

			for(int i = 0; i < GameTable.Length; ++i)
			{
				Match current = GameTable[i];

				if(current != null && current.MatchStatus == MatchStatus.Fighting)
					temp.Add(current);
			}

			if(temp.Count != 0)
			{
				Match[] fighting = new Match[temp.Count];

				for(int i = 0; i < fighting.Length; ++i)
					fighting[i] = temp[i];

				return fighting;
			}

			return null;
		}

		private List<Rectangle3D> GetOpenArenas()
		{
			List<Rectangle3D> open = new List<Rectangle3D>();
			List<Rectangle3D> closed = new List<Rectangle3D>();
			Match[] fighting = GetCurrentMatches();

			if(fighting == null) //Means that all arenas are open
				return m_Coords;

			for(int i = 0; i < m_Coords.Count; ++i) //Loop through all arenas, find closed arenas
			{
				for(int x = 0; x < fighting.Length; ++x) //Loop through all current Matches
				{
					Match current = fighting[x];

					if(ArenaContainsMatch(m_Coords[i], current)) //Match is occupying arena
						if(!closed.Contains(m_Coords[i]))
							closed.Add(m_Coords[i]);
				}

				if(!closed.Contains(m_Coords[i])) //Current arena is unoccupied
					if(!open.Contains(m_Coords[i]))
						open.Add(m_Coords[i]);
			}

			if(open.Count > 0)
				return open;

			return null;
		}

		private Rectangle3D FindOpenArena()
		{
			List<Rectangle3D> open = GetOpenArenas();

			if(open == null)
				return new Rectangle3D(0,0,0,0,0,0);
				//return null; Rectangle3D is a struct...

			return open[Utility.Random(open.Count)]; //Returns a random open arena
		}

		private bool AvailableArena()
		{
			if(RectangleNull(FindOpenArena()))
				return false;

			return true;
		}

		private bool ArenaContainsMatch(Rectangle3D arena, Match match)
		{
			for(int i = 0; i < match.Attackers.Count; ++i) //Loop through attackers
			{
				if(arena.Contains(match.Attackers[i].Location))
					return true;
			}

			for(int i = 0; i < match.Defenders.Count; ++i) //Loop through defenders
			{
				if(arena.Contains(match.Defenders[i].Location))
					return true;
			}

			return false;
		}

		private bool Round_IsOver()
		{
			int over = 0;

			for(int i = 0; i < GameTable.Length; ++i)
			{
				Match current = GameTable[i];

				if(current != null && current.MatchStatus == MatchStatus.Over)
					++over;
			}

			if(over == GameTable.Length)
				return true;

			return false;
		}

		private void FillTable() //Uses SignedUp to fill the GameTable with Matches -> NOTE: Shuffle before calling
		{
			for(int i = 0; i < GameTable.Length; ++i)
			{
				int defender = GameTable.Length + i;

				GameTable[i] = new Match((List<Mobile>)(i > m_SignedUp.Count - 1 ? null : m_SignedUp[i]), (List<Mobile>)(defender > m_SignedUp.Count - 1 ? null : m_SignedUp[defender]), (m_AllowTeams ? MatchType.Multi : MatchType.Single));
			}
		}

		private void RefactorTable()
		{
			if(GameTable.Length % 2 != 0)
				Console.WriteLine("GameTable has uneven amount of matches. Matches: {0}", GameTable.Length); //Debugging purposes

			//Take the first half of the Game Table, and load the winners into Attacker slots
			for(int i = 0; i < (GameTable.Length / 2); i++)
			{
				Match current = GameTable[i];

				current.Attackers = current.Winners;

				current.Defenders = null;
				current.Winners = null;
				current.MatchStatus = MatchStatus.Waiting;
			}

			//Take second half of the Game Table, and load all winners into Defender slots
			for(int i = (GameTable.Length / 2); i < GameTable.Length; i++)
			{
				Match current = GameTable[i];
				Match toSwitch = GameTable[i - (GameTable.Length / 2)];

				toSwitch.Defenders = current.Winners;
			}

			//Make a temp array half sized
			Match[] temp = new Match[GameTable.Length / 2];

			//Move first half of GameTable to temp
			for(int i = 0; i < temp.Length; ++i)
			{
				temp[i] = GameTable[i];
			}

			//Reassign GameTable
			GameTable = temp;
		}

		private void ShuffleList(List<List<Mobile>> from)
		{
			for (int i = from.Count - 1; i > 0; --i)
			{
				int position = Utility.Random(i + 1);
				List<Mobile> temp = from[i];

				from[i] = from[position];
				from[position] = temp;
			}
		}

		private List<Mobile> SignedUp_FindTeam(Mobile m)
		{
			if(m_SignedUp != null)
			{
				for(int i = 0; i < m_SignedUp.Count; ++i)
				{
					List<Mobile> team = m_SignedUp[i];

					if(team == null)
						return null;

					if(team.Contains(m))
						return team;
				}
			}

			return null;
		}

		public void VerifyTeamJoin(Mobile from, Mobile sender)
		{
			if(!m_AllowSameIP)
			{
				List<IPAddress> adr = GetAddresses();

				for(int i = 0; i < adr.Count; ++i)
				{
					if(adr[i].ToString() == from.NetState.Address.ToString())
					{
						from.SendMessage(String.Format("There is already someone signed up with your IP address, {0}.", from.NetState.Address.ToString()));
						sender.SendMessage("Someone with their IP address is already signed up for this tournament.");
						return;
					}
				}
			}

			List<Mobile> team = SignedUp_FindTeam(sender);

			if(team != null && team.Count < m_TeamMaxPlayers)
			{
				team.Add(from);

				from.SendMessage("You have successfully joined the team.");
				sender.SendMessage(String.Format("{0} has joined the team.", from.Name));
				return;
			}

			from.SendMessage("Either the team you are trying to join is full, or has been abandoned.");
		}

		public void VerifyJoin(Mobile from)
		{
			if(!m_AllowSameIP)
			{
				List<IPAddress> adr = GetAddresses();

				for(int i = 0; i < adr.Count; ++i)
				{
					if(adr[i].ToString() == from.NetState.Address.ToString())
					{
						from.SendMessage(String.Format("There is already someone signed up with your IP address, {0}.", from.NetState.Address.ToString()));
						return;
					}
				}
			}

			List<Mobile> team = new List<Mobile>();
			team.Add(from);

			m_SignedUp.Add(team);

			from.SendMessage("You have been signed up for this tournament.");
		}

		public void VerifyQuit(Mobile from)
		{
			bool leader = false;

			for(int i = 0; i < m_SignedUp.Count; ++i)
			{
				List<Mobile> team = m_SignedUp[i];

				if(team.Count >= 1)
					if(team[0] == from)
						leader = true;
			}

			for(int i = 0; i < m_SignedUp.Count; ++i)
			{
				if(m_SignedUp[i].Contains(from))
				{
					m_SignedUp[i].Remove(from);
					from.SendMessage("You have been removed from this tournament.");
				}

				if(m_SignedUp[i].Count == 0)
					m_SignedUp.Remove(m_SignedUp[i]);

				else if(leader)
				{
					List<Mobile> team = m_SignedUp[i];

					team[0].SendMessage("The leader of the team has left. You are now the leader.");
				}
			}
		}

		private List<IPAddress> GetAddresses()
		{
			List<IPAddress> list = new List<IPAddress>();

			for(int i = 0; i < m_SignedUp.Count; ++i)
			{
				List<Mobile> team = m_SignedUp[i];

				for(int x = 0; x < team.Count; ++x)
					if(team[x].NetState != null)
						list.Add(team[x].NetState.Address);
			}

			return list;
		}

		public string[] GetRules()
		{
			return new string[] { CreateRule("Healing", m_CanHeal), CreateRule("Potions", m_Potions), CreateRule("Magic Weapons", m_MagicWeapons) };
		}

		private string FormatString(bool allow)
		{
			return ("<baseFONT COLOR=" + (allow ? "'lime'" : "'red'") + ">" + (allow ? "Allowed" : "Disallowed") + "</baseFONT>");
		}

		private string CreateRule(string name, bool check)
		{
			return (name + ": " + FormatString(check) + "<p>");
		}

		public void Stop(Mobile from)
		{
			if(!m_Started)
			{
				from.SendMessage("The tournament is not running!");
				return;
			}

			if(GameTable != null)
			{
				Match[] toStop = GetCurrentMatches();

				for(int i = 0; i < toStop.Length; ++i)
				{
					Match current = toStop[i];

					for(int x = 0; x < current.Attackers.Count; ++x)
					{
						HandleMatchEnd(current.Attackers[x], false);
						current.Attackers[x].SendMessage("The tournament has been stopped by a game master.");
					}
					for(int x = 0; x < current.Defenders.Count; ++x)
					{
						HandleMatchEnd(current.Defenders[x], false);
						current.Defenders[x].SendMessage("The tournament has been stopped by a game master.");
					}
				}

				m_Started = false;
				GameTable = null;
				m_SignedUp = new List<List<Mobile>>();
				from.SendMessage("The tournament has been stopped.");
				return;
			}

			from.SendMessage("Error stopping the tournament.");
		}

		public TSystemStone(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int) 5 ); //version

			writer.Write( (int) m_TeamMaxPlayers );
			writer.Write( m_AllowTeams );

			writer.Write( m_AllowSameIP );
			writer.Write( m_SpectatorDest );

			writer.Write( m_Enabled );
			writer.Write( m_CanHeal );
			writer.Write( m_Potions );
			writer.Write( m_MagicWeapons );

			writer.Write( (int) m_Coords.Count );

			for(int i = 0; i < m_Coords.Count; ++i)
			{
				writer.Write( (Point3D) m_Coords[i].Start );
				writer.Write( (Point3D) m_Coords[i].End );
			}

			WriteBitArray( writer, m_RestrictedSpells );
			WriteBitArray( writer, m_RestrictedSkills );
		}

		#region Serialization Helpers

		public static void WriteBitArray( GenericWriter writer, BitArray ba )
		{
			writer.Write( ba.Length );

				for( int i = 0; i < ba.Length; i++ )
				{
					writer.Write( ba[i] );
				}
			return;
		}

		public static BitArray ReadBitArray( GenericReader reader )
		{
			int size = reader.ReadInt();
			BitArray newBA = new BitArray( size );

			for( int i = 0; i < size; i++ )
			{
				newBA[i] = reader.ReadBool();
			}

			return newBA;
		}

		#endregion

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			StaticRef = this;

			int version = reader.ReadInt();

			switch( version )
			{
				case 5:
				{
					m_TeamMaxPlayers = reader.ReadInt();
					m_AllowTeams = reader.ReadBool();

					goto case 4;
				}
				case 4:
				{
					m_AllowSameIP = reader.ReadBool();
					m_SpectatorDest = reader.ReadPoint3D();

					goto case 3;
				}
				case 3:
				{
					m_Enabled = reader.ReadBool();
					m_CanHeal = reader.ReadBool();
					m_Potions = reader.ReadBool();
					m_MagicWeapons = reader.ReadBool();

					goto case 2;
				}
				case 2:
				{
					int size = reader.ReadInt();

					m_Coords = new List<Rectangle3D>();

					for( int i = 0; i < size; ++i )
					{
						m_Coords.Add( new Rectangle3D( reader.ReadPoint3D(), reader.ReadPoint3D() ) );
					}

					goto case 1;
				}
				case 1:
				{
					m_RestrictedSpells = ReadBitArray( reader );
					m_RestrictedSkills = ReadBitArray( reader );

					goto case 0;
				}
				case 0:
				{
					break;
				}
			}
		}

		public void UpdateRegion()
		{
			if( m_Coords != null )
			{
				if( m_Region != null )
					m_Region.Unregister();

				CheckRectFix();

				m_Region = new TArenaRegion( this, this.Map, this.m_Coords.ToArray() );

				m_Region.Disabled = true;

				m_Region.Register();
			}

			return;
		}

		private void CheckRectFix() //Makes sure rectangles have a depth of at least 1
		{
			for(int i = 0; i < m_Coords.Count; ++i)
			{
				Rectangle3D cur = m_Coords[i];

				if(cur.Depth == 0)
					m_Coords[i] = new Rectangle3D( cur.Start, new Point3D(cur.End.X, cur.End.Y, cur.Start.Z + 1) );
			}
		}

		public static int GetRegistryNumber( ISpell s )
		{
			Type[] t = SpellRegistry.Types;

			for( int i = 0; i < t.Length; i++ )
			{
				if( s.GetType() == t[i] )
					return i;
			}

			return -1;
		}

		public bool IsRestrictedSpell( ISpell s )
		{
			int regNum = GetRegistryNumber( s );
			
			if( regNum < 0 )	//Happens with unregistered Spells
				return false;

			return m_RestrictedSpells[regNum];
		}

		public bool IsRestrictedSkill( int skill )
		{
			if( skill < 0 )
				return false;

			return m_RestrictedSkills[skill];
		}

		public void ChooseArea( Mobile m )
		{
			BoundingBoxPicker.Begin( m, new BoundingBoxCallback( TArenaRegion_Callback ), this );
		}

		private static void TArenaRegion_Callback( Mobile from, Map map, Point3D start, Point3D end, object state )
		{
			DoChooseArea( from, map, start, end, state );
		}

		private static void DoChooseArea( Mobile from, Map map, Point3D start, Point3D end, object control )
		{
			TSystemStone master = (TSystemStone)control;
			Rectangle3D rect = new Rectangle3D( start, end );

			if(master.m_Coords == null)
				master.m_Coords = new List<Rectangle3D>();

			master.m_Coords.Add( rect );

			master.UpdateRegion();

			from.SendGump( new TRegionGump(master) );
		}

		public override void OnDelete()
		{
			if( m_Region != null )
				m_Region.Unregister();

			foreach(Mobile m in World.Mobiles.Values)
			{
				if(m is TMaster)
				{
					TMaster master = (TMaster)m;

					if(master.Link == this)
					{
						master.Link = null;
						master.Say("My link is now null.");
					}
				}
			}

			base.OnDelete();
		}

		public override void OnMapChange()
		{
			UpdateRegion();
			base.OnMapChange();
		}

		public void ShowBounds( Rectangle3D r, Map m ) //Jeff is the man :-)
		{
			int depth = r.Depth;
			int width = r.Start.X + r.Width + 1;
			int height = r.Start.Y + r.Height + 1;
			//int mod = depth % 5;
 
			do
			{
				for ( int x = r.Start.X; x < width; ++x )
				{
					for ( int y = r.Start.Y; y < height; ++y )
					{
						if ( ( ( x == r.Start.X || x == r.End.X ) || ( y == r.Start.Y || y == r.End.Y ) ) ) //&& depth % 5 == mod )
							Effects.SendLocationEffect( new Point3D( x, y, r.Start.Z + depth ), m, 437, 75, 1, 1156, 3 );
					}
				}
 
				if(depth != 0)
					--depth;
			}
			while ( depth != 0 );
		}
	}
}