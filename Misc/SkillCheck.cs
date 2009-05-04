using System;
using Server;
using Server.Mobiles;

namespace Server.Misc
{
	public class SkillCheck
	{
		private static readonly bool AntiMacroCode = false;		//Change this to false to disable anti-macro code (original !Core.ML)

		public static TimeSpan AntiMacroExpire = TimeSpan.FromMinutes( 60.0 ); //How long do we remember targets/locations?
		public const int Allowance = 3;	//How many times may we use the same location/target for gain
		private const int LocationSize = 8; //The size of each location, make this smaller so players dont have to move as far
		private static bool[] UseAntiMacro = new bool[]
		{
			// true if this skill uses the anti-macro code, false if it does not
			false,// Alchemy = 0,
			true,// Anatomy = 1,
			true,// AnimalLore = 2,
			true,// ItemID = 3,
			true,// ArmsLore = 4,
			false,// Parry = 5,
			true,// Begging = 6,
			false,// Blacksmith = 7,
			false,// Fletching = 8,
			true,// Peacemaking = 9,
			true,// Camping = 10,
			false,// Carpentry = 11,
			false,// Cartography = 12,
			false,// Cooking = 13,
			true,// DetectHidden = 14,
			true,// Discordance = 15,
			true,// EvalInt = 16,
			true,// Healing = 17,
			true,// Fishing = 18,
			true,// Forensics = 19,
			true,// Herding = 20,
			true,// Hiding = 21,
			true,// Provocation = 22,
			false,// Inscribe = 23,
			true,// Lockpicking = 24,
			true,// Magery = 25,
			true,// MagicResist = 26,
			false,// Tactics = 27,
			true,// Snooping = 28,
			true,// Musicianship = 29,
			false,// Poisoning = 30,
			false,// Archery = 31,
			true,// SpiritSpeak = 32,
			true,// Stealing = 33,
			false,// Tailoring = 34,
			true,// AnimalTaming = 35,
			true,// TasteID = 36,
			false,// Tinkering = 37,
			true,// Tracking = 38,
			true,// Veterinary = 39,
			false,// Swords = 40,
			false,// Macing = 41,
			false,// Fencing = 42,
			false,// Wrestling = 43,
			true,// Lumberjacking = 44,
			true,// Mining = 45,
			true,// Meditation = 46,
			true,// Stealth = 47,
			true,// RemoveTrap = 48,
			true,// Necromancy = 49,
			false,// Focus = 50,
			true,// Chivalry = 51
			true,// Bushido = 52
			true,//Ninjitsu = 53
			true // Spellweaving
		};

		public static void Initialize()
		{
			Mobile.SkillCheckLocationHandler = new SkillCheckLocationHandler( Mobile_SkillCheckLocation );
			Mobile.SkillCheckDirectLocationHandler = new SkillCheckDirectLocationHandler( Mobile_SkillCheckDirectLocation );

			Mobile.SkillCheckTargetHandler = new SkillCheckTargetHandler( Mobile_SkillCheckTarget );
			Mobile.SkillCheckDirectTargetHandler = new SkillCheckDirectTargetHandler( Mobile_SkillCheckDirectTarget );

			SkillInfo.Table[0].GainFactor = 1.60;// Alchemy = 0, 
			SkillInfo.Table[1].GainFactor = 1.30;// Anatomy = 1, 
			SkillInfo.Table[2].GainFactor = 1.40;// AnimalLore = 2, 
			SkillInfo.Table[3].GainFactor = 1.90;// ItemID = 3, 
			SkillInfo.Table[4].GainFactor = 1.90;// ArmsLore = 4, 
			SkillInfo.Table[5].GainFactor = 1.30;// Parry = 5, 
			SkillInfo.Table[6].GainFactor = 1.90;// Begging = 6, 
			SkillInfo.Table[7].GainFactor = 1.60;// Blacksmith = 7, 
			SkillInfo.Table[8].GainFactor = 1.60;// Fletching = 8, 
			SkillInfo.Table[9].GainFactor = 1.50;// Peacemaking = 9, 
			SkillInfo.Table[10].GainFactor = 1.90;// Camping = 10, 
			SkillInfo.Table[11].GainFactor = 1.70;// Carpentry = 11, 
			SkillInfo.Table[12].GainFactor = 1.70;// Cartography = 12, 
			SkillInfo.Table[13].GainFactor = 1.70;// Cooking = 13, 
			SkillInfo.Table[14].GainFactor = 1.90;// DetectHidden = 14, 
			SkillInfo.Table[15].GainFactor = 1.50;// Discordance = 15, 
			SkillInfo.Table[16].GainFactor = 1.25;// EvalInt = 16, 
			SkillInfo.Table[17].GainFactor = 1.30;// Healing = 17, 
			SkillInfo.Table[18].GainFactor = 1.70;// Fishing = 18, 
			SkillInfo.Table[19].GainFactor = 1.90;// Forensics = 19, 
			SkillInfo.Table[20].GainFactor = 1.90;// Herding = 20, 
			SkillInfo.Table[21].GainFactor = 1.00;// Hiding = 21, 
			SkillInfo.Table[22].GainFactor = 1.90;// Provocation = 22, 
			SkillInfo.Table[23].GainFactor = 1.50;// Inscribe = 23, 
			SkillInfo.Table[24].GainFactor = 1.70;// Lockpicking = 24, 
			SkillInfo.Table[25].GainFactor = 1.20;// Magery = 25, 
			SkillInfo.Table[26].GainFactor = 1.30;// MagicResist = 26, 
			SkillInfo.Table[27].GainFactor = 1.20;// Tactics = 27, 
			SkillInfo.Table[28].GainFactor = 1.30;// Snooping = 28, 
			SkillInfo.Table[29].GainFactor = 1.30;// Musicianship = 29, 
			SkillInfo.Table[30].GainFactor = 1.80;// Poisoning = 30 
			SkillInfo.Table[31].GainFactor = 1.30;// Archery = 31 
			SkillInfo.Table[32].GainFactor = 1.00;// SpiritSpeak = 32 
			SkillInfo.Table[33].GainFactor = 1.50;// Stealing = 33 
			SkillInfo.Table[34].GainFactor = 1.60;// Tailoring = 34 
			SkillInfo.Table[35].GainFactor = 1.90;// AnimalTaming = 35 
			SkillInfo.Table[36].GainFactor = 1.90;// TasteID = 36 
			SkillInfo.Table[37].GainFactor = 1.60;// Tinkering = 37 
			SkillInfo.Table[38].GainFactor = 1.80;// Tracking = 38 
			SkillInfo.Table[39].GainFactor = 1.40;// Veterinary = 39 
			SkillInfo.Table[40].GainFactor = 1.60;// Swords = 40 
			SkillInfo.Table[41].GainFactor = 1.50;// Macing = 41 
			SkillInfo.Table[42].GainFactor = 1.55;// Fencing = 42 
			SkillInfo.Table[43].GainFactor = 1.50;// Wrestling = 43 
			SkillInfo.Table[44].GainFactor = 1.80;// Lumberjacking = 44 
			SkillInfo.Table[45].GainFactor = 2.00;// Mining = 45 
			SkillInfo.Table[46].GainFactor = 1.10;// Meditation = 46 
			SkillInfo.Table[47].GainFactor = 1.10;// Stealth = 47 
			SkillInfo.Table[48].GainFactor = 1.70;// RemoveTrap = 48 
			SkillInfo.Table[49].GainFactor = 1.30;// Necromancy = 49 
			SkillInfo.Table[50].GainFactor = 1.20;// Focus = 50 
			SkillInfo.Table[51].GainFactor = 1.40;// Chivalry = 51 
			SkillInfo.Table[52].GainFactor = 1.30;// Bushido = 50 
			SkillInfo.Table[53].GainFactor = 1.30;// Ninjitsu = 51 
			SkillInfo.Table[54].GainFactor = 2.00;// SpellWeaving = 51

		}

		public static bool Mobile_SkillCheckLocation( Mobile from, SkillName skillName, double minSkill, double maxSkill )
		{
			Skill skill = from.Skills[skillName];

			if ( skill == null )
				return false;

			double value = skill.Value;

			if ( value < minSkill )
				return false; // Too difficult
			else if ( value >= maxSkill )
				return true; // No challenge

			double chance = (value - minSkill) / (maxSkill - minSkill);

			Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize );
			return CheckSkill( from, skill, loc, chance );
		}

		public static bool Mobile_SkillCheckDirectLocation( Mobile from, SkillName skillName, double chance )
		{
			Skill skill = from.Skills[skillName];

			if ( skill == null )
				return false;

			if ( chance < 0.0 )
				return false; // Too difficult
			else if ( chance >= 1.0 )
				return true; // No challenge

			Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize );
			return CheckSkill( from, skill, loc, chance );
		}

		public static bool CheckSkill( Mobile from, Skill skill, object amObj, double chance )
		{
			if ( from.Skills.Cap == 0 )
				return false;

			bool success = ( chance >= Utility.RandomDouble() );
			double gc = (double)(from.Skills.Cap - from.Skills.Total) / from.Skills.Cap;
			gc += ( skill.Cap - skill.Base ) / skill.Cap;
			gc /= 5;

			gc += ( 1.0 - chance ) * ( success ? 0.5 : (Core.AOS ? 0.0 : 0.2) );
			gc /= 5;

			gc *= skill.Info.GainFactor;

			if ( gc < 0.001 )
				gc = 0.001;

			if ( from is BaseCreature && ((BaseCreature)from).Controlled )
				gc *= 2;

			if ( from.Alive && ( ( gc >= Utility.RandomDouble() && AllowGain( from, skill, amObj ) ) || skill.Base < 10.0 ) )
				Gain( from, skill );

// GGS start
					else if ( chance <= 0.99 && chance >= 0.1 && skill.Base < skill.Cap && skill.Lock == SkillLock.Up ) 
					{
						if ( from.SkillsTotal >= from.SkillsCap )
						{
							for ( int i = 0; i < from.Skills.Length; ++i )
							{
								Skill sk = from.Skills[i];
	
								if ( sk.Base == 0 || sk.Lock != SkillLock.Down )
									continue;

								Gain_GGS( from, skill );
							}
						}
						else
							Gain_GGS( from, skill );
					}
// GGS end

			return success;
		}

		private static TimeSpan[,] GGSTimes = new TimeSpan[,]
                {
			{TimeSpan.FromMinutes(1),	TimeSpan.FromMinutes(3),	TimeSpan.FromMinutes(5)},	// 0
			{TimeSpan.FromMinutes(4),	TimeSpan.FromMinutes(10),	TimeSpan.FromMinutes(18)},	// 5
			{TimeSpan.FromMinutes(7),	TimeSpan.FromMinutes(17),	TimeSpan.FromMinutes(30)},	// 10
			{TimeSpan.FromMinutes(9),	TimeSpan.FromMinutes(24),	TimeSpan.FromMinutes(44)},	// 15
			{TimeSpan.FromMinutes(12),	TimeSpan.FromMinutes(31),	TimeSpan.FromMinutes(57)},	// 20
			{TimeSpan.FromMinutes(14),	TimeSpan.FromMinutes(38),	TimeSpan.FromHours(1.2)},	// 25
			{TimeSpan.FromMinutes(17),	TimeSpan.FromMinutes(45),	TimeSpan.FromHours(1.4)},	// 30
			{TimeSpan.FromMinutes(20),	TimeSpan.FromMinutes(52),	TimeSpan.FromHours(1.6)},	// 35
			{TimeSpan.FromMinutes(23),	TimeSpan.FromMinutes(60),	TimeSpan.FromHours(1.8)},	// 40
			{TimeSpan.FromMinutes(25),	TimeSpan.FromHours(1.1),	TimeSpan.FromHours(2.0)},	// 45
			{TimeSpan.FromMinutes(27),	TimeSpan.FromHours(1.2),	TimeSpan.FromHours(2.3)},	// 50
			{TimeSpan.FromMinutes(33),	TimeSpan.FromHours(1.5),	TimeSpan.FromHours(2.7)},	// 55
			{TimeSpan.FromMinutes(55),	TimeSpan.FromHours(2.5),	TimeSpan.FromHours(4.4)},	// 60
			{TimeSpan.FromHours(1.3),	TimeSpan.FromHours(3.6),	TimeSpan.FromHours(6.5)},	// 65
			{TimeSpan.FromHours(1.9),	TimeSpan.FromHours(4.9),	TimeSpan.FromHours(9.0)},	// 70
			{TimeSpan.FromHours(2.4),	TimeSpan.FromHours(6.4),	TimeSpan.FromHours(11.8)},	// 75
			{TimeSpan.FromHours(3.0),	TimeSpan.FromHours(8.2),	TimeSpan.FromHours(15.0)},	// 80
			{TimeSpan.FromHours(3.8),	TimeSpan.FromHours(10.1),	TimeSpan.FromHours(18.6)},	// 85
			{TimeSpan.FromHours(4.6),	TimeSpan.FromHours(12.4),	TimeSpan.FromHours(22.6)},	// 90
			{TimeSpan.FromHours(5.6),	TimeSpan.FromHours(14.9),	TimeSpan.FromHours(27.0)},	// 95
			{TimeSpan.FromHours(6.6),	TimeSpan.FromHours(17.6),	TimeSpan.FromHours(32.0)},	// 100
			{TimeSpan.FromHours(7.8),	TimeSpan.FromHours(20.7),	TimeSpan.FromHours(38.0)},	// 105
			{TimeSpan.FromHours(9.0),	TimeSpan.FromHours(24.0),	TimeSpan.FromHours(43.0)},	// 110
			{TimeSpan.FromHours(10.3),	TimeSpan.FromHours(27.7),	TimeSpan.FromHours(51.0)}	// 115
		};

		public static void Gain_GGS( Mobile from, Skill skill )
		{
			if ( !(from is PlayerMobile) ) 
				return;

			PlayerMobile pmob = (PlayerMobile)from;

			int ggCol;
			if(from.Skills.Total <= 3500)
				ggCol = 0;
			else if(from.Skills.Total <= 5000)
				ggCol = 1;
			else
				ggCol = 2;

			int ggRow = (int)(skill.Base / 5);
   
			if(ggRow < 0)
				ggRow = 0;
			else if(ggRow > 115/5)
				ggRow = 115/5;

			TimeSpan ggDelay = GGSTimes[ggRow,ggCol];

			Skills skills = from.Skills;

			object lgt = pmob.SkillGainTimes[skill.SkillID];

			if ( lgt == null || (DateTime)lgt + ggDelay < DateTime.Now ) 
			{
				Gain(from, skill);

				int hours = (int)ggDelay.TotalHours;

				if ( hours < 1 )
				{
					int minutes = (int)ggDelay.TotalMinutes;

					pmob.SendMessage("Narust skillu diky GGS!");
					pmob.SendMessage("Tva GGS pauza pro {0} je {1} Minutes", skill.SkillName.ToString(), minutes.ToString());	


				}
				else
				{
					pmob.SendMessage("Narust skillu diky GGS!");
					pmob.SendMessage("Tva GGS pauza pro {0} je {1} Hours", skill.SkillName.ToString(), hours.ToString());	
				}
			}
		}

		public static bool Mobile_SkillCheckTarget( Mobile from, SkillName skillName, object target, double minSkill, double maxSkill )
		{
			Skill skill = from.Skills[skillName];

			if ( skill == null )
				return false;

			double value = skill.Value;

			if ( value < minSkill )
				return false; // Too difficult
			else if ( value >= maxSkill )
				return true; // No challenge

			double chance = (value - minSkill) / (maxSkill - minSkill);

			return CheckSkill( from, skill, target, chance );
		}

		public static bool Mobile_SkillCheckDirectTarget( Mobile from, SkillName skillName, object target, double chance )
		{
			Skill skill = from.Skills[skillName];

			if ( skill == null )
				return false;

			if ( chance < 0.0 )
				return false; // Too difficult
			else if ( chance >= 1.0 )
				return true; // No challenge

			return CheckSkill( from, skill, target, chance );
		}

		private static bool AllowGain( Mobile from, Skill skill, object obj )
		{
			if ( AntiMacroCode && from is PlayerMobile && UseAntiMacro[skill.Info.SkillID] )
				return ((PlayerMobile)from).AntiMacroCheck( skill, obj );
			else
				return true;
		}

		public enum Stat { Str, Dex, Int }

		public static void Gain( Mobile from, Skill skill )
		{
			if ( from.Region.IsPartOf( typeof( Regions.Jail ) ) )
				return;

			if ( from is BaseCreature && ((BaseCreature)from).IsDeadPet )
				return;

			if ( skill.SkillName == SkillName.Focus && from is BaseCreature )
				return;

			if ( skill.Base < skill.Cap && skill.Lock == SkillLock.Up )
			{
				int toGain = 1;

				if ( skill.Base <= 10.0 )
					toGain = Utility.Random( 4 ) + 1;

				Skills skills = from.Skills;

				if ( ( skills.Total / skills.Cap ) >= Utility.RandomDouble() )//( skills.Total >= skills.Cap )
				{


					for ( int i = 0; i < skills.Length; ++i )
					{
						Skill toLower = skills[i];

						if ( toLower != skill && toLower.Lock == SkillLock.Down && toLower.BaseFixedPoint >= toGain )
						{
							toLower.BaseFixedPoint -= toGain;
							break;
						}
					}
				}			
				
				#region Mondain's Legacy
				if ( from is PlayerMobile )
					if ( Server.Engines.Quests.QuestHelper.EnhancedSkill( (PlayerMobile) from, skill ) )
						toGain *= Utility.RandomMinMax( 2, 4 );
				#endregion

				if ( (skills.Total + toGain) <= skills.Cap )
				{
					
				skill.BaseFixedPoint += toGain;
				}
				
				if(from is PlayerMobile)

				{
				PlayerMobile pm = (PlayerMobile)from;
				pm.SkillGainTimes[skill.SkillID] = DateTime.Now;
				} 
			}
				
			#region Mondain's Legacy
			if ( from is PlayerMobile )
				Server.Engines.Quests.QuestHelper.CheckSkill( (PlayerMobile) from, skill );
			#endregion

			if ( skill.Lock == SkillLock.Up )
			{
				SkillInfo info = skill.Info;

				if ( from.StrLock == StatLockType.Up && (info.StrGain / 33.3) > Utility.RandomDouble() )
					GainStat( from, Stat.Str );
				else if ( from.DexLock == StatLockType.Up && (info.DexGain / 33.3) > Utility.RandomDouble() )
					GainStat( from, Stat.Dex );
				else if ( from.IntLock == StatLockType.Up && (info.IntGain / 33.3) > Utility.RandomDouble() )
					GainStat( from, Stat.Int );

				else if ( from.StrLock == StatLockType.Up  )
					GainStat_GGS( from, Stat.Str );
				else if ( from.DexLock == StatLockType.Up )
					GainStat_GGS( from, Stat.Dex );
				else if ( from.IntLock == StatLockType.Up )
					GainStat_GGS( from, Stat.Int );
			}
			
		}

		public static bool CanLower( Mobile from, Stat stat )
		{
			switch ( stat )
			{
				case Stat.Str: return ( from.StrLock == StatLockType.Down && from.RawStr > 10 );
				case Stat.Dex: return ( from.DexLock == StatLockType.Down && from.RawDex > 10 );
				case Stat.Int: return ( from.IntLock == StatLockType.Down && from.RawInt > 10 );
			}

			return false;
		}

		public static bool CanRaise( Mobile from, Stat stat )
		{
			if ( !(from is BaseCreature && ((BaseCreature)from).Controlled) )
			{
				if ( from.RawStatTotal >= from.StatCap )
					return false;
			}

			switch ( stat )
			{
				case Stat.Str: return ( from.StrLock == StatLockType.Up && from.RawStr < 125 );
				case Stat.Dex: return ( from.DexLock == StatLockType.Up && from.RawDex < 125 );
				case Stat.Int: return ( from.IntLock == StatLockType.Up && from.RawInt < 125 );
			}

			return false;
		}

		public static bool CanRaiseGGS( Mobile from, Stat stat )
		{
			if ( from.RawStatTotal >= from.StatCap && from.StrLock != StatLockType.Down &&  from.DexLock != StatLockType.Down && from.IntLock != StatLockType.Down )
				return false;

			switch ( stat )
			{
				case Stat.Str: return ( from.RawStr < 125 );
				case Stat.Dex: return ( from.RawDex < 125 );
				case Stat.Int: return ( from.RawInt < 125 );
			}

			return false;
		}

		public static void IncreaseStat( Mobile from, Stat stat, bool atrophy )
		{
			atrophy = atrophy || (from.RawStatTotal >= from.StatCap);

			switch ( stat )
			{
				case Stat.Str:
				{
					if ( atrophy )
					{
						if ( CanLower( from, Stat.Dex ) && (from.RawDex < from.RawInt || !CanLower( from, Stat.Int )) )
							--from.RawDex;
						else if ( CanLower( from, Stat.Int ) )
							--from.RawInt;
					}

					if ( CanRaise( from, Stat.Str ) )
						++from.RawStr;

					break;
				}
				case Stat.Dex:
				{
					if ( atrophy )
					{
						if ( CanLower( from, Stat.Str ) && (from.RawStr < from.RawInt || !CanLower( from, Stat.Int )) )
							--from.RawStr;
						else if ( CanLower( from, Stat.Int ) )
							--from.RawInt;
					}

					if ( CanRaise( from, Stat.Dex ) )
						++from.RawDex;

					break;
				}
				case Stat.Int:
				{
					if ( atrophy )
					{
						if ( CanLower( from, Stat.Str ) && (from.RawStr < from.RawDex || !CanLower( from, Stat.Dex )) )
							--from.RawStr;
						else if ( CanLower( from, Stat.Dex ) )
							--from.RawDex;
					}

					if ( CanRaise( from, Stat.Int ) )
						++from.RawInt;

					break;
				}
			}
		}

		private static TimeSpan m_StatGainDelay = TimeSpan.FromMinutes( 15.0 );
		
		private static TimeSpan m_GGSStatGainDelay = TimeSpan.FromMinutes( 30.0 );

		public static void GainStat_GGS( Mobile from, Stat stat )
		{
			if ( from is PlayerMobile && CanRaiseGGS( from, stat ) )
			{
				PlayerMobile pmob = (PlayerMobile)from;

				if( ( pmob.LastStatGain + TimeSpan.FromHours( 24 ) ) <= DateTime.Now )
					pmob.GGS_Stattime = DateTime.Now - TimeSpan.FromHours( 24 );

				if ( pmob.GGS_Stattime + TimeSpan.FromHours( 24 ) <= DateTime.Now )
					pmob.GGS_StatCount = 0;

				if ( pmob == null || (pmob.LastStatGain + m_GGSStatGainDelay) >= DateTime.Now || pmob.GGS_StatCount >= 6 )
					return;

				bool atrophy = ( (pmob.RawStatTotal / (double)pmob.StatCap) >= Utility.RandomDouble() );

				IncreaseStat( pmob, stat, atrophy );

				pmob.GGSActive = true;
			
				pmob.GGS_StatCount += 1;

				int info = ( 6 - pmob.GGS_StatCount );

				pmob.SendMessage("Prave jsi ziskal jeden stat diky GGS!");
				pmob.SendMessage("{0} dalsich statu muzes pro dnesek ziskat diky GGS!", info.ToString() );

				if( pmob.GGS_StatCount >= 6 )
				{
					pmob.GGS_Stattime = DateTime.Now;
				}

				switch( stat )
				{
					case Stat.Str:
					{
						if( (pmob.LastStrGain + m_StatGainDelay) >= DateTime.Now )
							return;

						pmob.LastStrGain = DateTime.Now;
						break;
					}
					case Stat.Dex:
					{
						if( (pmob.LastDexGain + m_StatGainDelay) >= DateTime.Now )
							return;

						pmob.LastDexGain = DateTime.Now;
						break;
					}
					case Stat.Int:
					{
						if( (pmob.LastIntGain + m_StatGainDelay) >= DateTime.Now )
							return;

						pmob.LastIntGain = DateTime.Now;
						break;
					}
				}
			}
		}

		public static void GainStat( Mobile from, Stat stat )
		{
			switch( stat )
			{
				case Stat.Str:
				{
					if( (from.LastStrGain + m_StatGainDelay) >= DateTime.Now )
						return;

					from.LastStrGain = DateTime.Now;
					break;
				}
				case Stat.Dex:
				{
					if( (from.LastDexGain + m_StatGainDelay) >= DateTime.Now )
						return;

					from.LastDexGain = DateTime.Now;
					break;
				}
				case Stat.Int:
				{
					if( (from.LastIntGain + m_StatGainDelay) >= DateTime.Now )
						return;

					from.LastIntGain = DateTime.Now;
					break;
				}
			}

			bool atrophy = ( (from.RawStatTotal / (double)from.StatCap) >= Utility.RandomDouble() );

			IncreaseStat( from, stat, atrophy );
		}
	}
}