using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Server.Engines
{
	public class GuaranteedGainSystem : Timer
	{
        // From OSI - http://guide.uo.com/skill_1001.html
		private static int[] m_Terms350 = new int[] { 1, 4, 7, 9, 12, 14, 17, 20, 23, 25, 27, 33, 55, 78, 114, 144, 180, 228, 276, 336, 396, 468, 540, 618 };
		private static int[] m_Terms500 = new int[] { 3, 10, 17, 24, 31, 38, 45, 52, 60, 66, 72, 90, 150, 216, 294, 384, 492, 606, 744, 894, 1056, 1242, 1440, 1662 };
		private static int[] m_Terms700 = new int[] { 5, 18, 30, 44, 57, 72, 84, 96, 108, 120, 138, 162, 264, 390, 540, 708, 900, 1116, 1356, 1620, 1920, 2280, 2580, 3060 };

		private static DateTime m_LastResetTime = DateTime.Now;

		public static bool Enabled = true; //Enable the system

        public static TimeSpan ResetTime = TimeSpan.FromHours( 6.0 ); // Time of the day
		public static string SavePath = "Saves/RateInfo";
		public static string SaveFile = "GGS.bin";

		public static bool ForceSkillGain( Mobile from, Skill skill )
		{
			if ( from.Player )
			{
				MobileRateInfo mobileInfo = MobileRateInfo.GetMobileInfo( from );
				SkillRateInfo skillInfo = mobileInfo.GetSkillInfo( skill );

				int[] table = null;

				if ( from.Skills.Total <= 350 )
					table = m_Terms350;
				else if ( from.Skills.Total <= 500 )
					table = m_Terms500;
				else
					table = m_Terms700;

                int index = skill.BaseFixedPoint / 50;

				if ( DateTime.Now - skillInfo.LastGainTime < TimeSpan.FromMinutes( table[ index > 23 ? 23 : index ] ) )
					return false;

				return true;
			}

			return false;
		}

		public static bool ForceStatGain( Mobile from )
		{
			if ( from.Player )
			{
				MobileRateInfo mobileInfo = MobileRateInfo.GetMobileInfo( from );

				if ( mobileInfo.StatGainsCount < 10 && DateTime.Now - mobileInfo.LastStatGainTime > TimeSpan.FromMinutes( 15 ) )
					return true;
			}

			return false;
		}

		public static void RegisterSkillGain( Mobile from, Skill skill )
		{
			if ( from.Player )
			{
				MobileRateInfo mobileInfo = MobileRateInfo.GetMobileInfo( from );
				SkillRateInfo skillInfo = mobileInfo.GetSkillInfo( skill );

				skillInfo.LastGainTime = DateTime.Now;
			}
		}

		public static void RegisterStatGain( Mobile from )
		{
			if ( from.Player )
			{
				MobileRateInfo mobileInfo = MobileRateInfo.GetMobileInfo( from );

				mobileInfo.LastStatGainTime = DateTime.Now;
				mobileInfo.StatGainsCount++;
			}
		}

		public static void Reset( bool full )
		{
			m_LastResetTime = DateTime.Now;

			foreach ( MobileRateInfo mobileInfo in MobileRateInfo.Entries.Values )
			{
				mobileInfo.LastStatGainTime = DateTime.MinValue;
				mobileInfo.StatGainsCount = 0;

				if ( full )
				{
					foreach ( SkillRateInfo skillInfo in mobileInfo.SkillRates.Values )
						skillInfo.LastGainTime = DateTime.MinValue;
				}
			}
		}

		public static void Initialize()
		{
			if ( Enabled )
			{
				CommandSystem.Register( "GGSReset", AccessLevel.Administrator, new CommandEventHandler( Reset_OnCommand ) );

				new GuaranteedGainSystem().Start();
			}
		}

		[Usage( "GGSReset <full>" )]
		[Description( "Reset all information stored by Guaranteed Gain System system." )]
		private static void Reset_OnCommand( CommandEventArgs e )
		{
			if ( e.Length == 1 )
			{
				bool full = e.GetString( 0 ).Trim().ToLower() == "true";

				Reset( full );

				if ( full )
					e.Mobile.SendMessage( "Guaranteed Gain System has being fully reseted." );
				else
					e.Mobile.SendMessage( "Guaranteed Gain System stat info has being reseted." );
			}
			else
			{
				e.Mobile.SendMessage( "Usage: GGSReset <full>" );
			}
		}

		public static void Configure()
		{
			if ( Enabled )
			{
				EventSink.WorldLoad += new WorldLoadEventHandler( OnLoad );
				EventSink.WorldSave += new WorldSaveEventHandler( OnSave );
			}
		}

		private static void OnSave( WorldSaveEventArgs args )
		{
			if ( !Directory.Exists( SavePath ) )
			{
				Directory.CreateDirectory( SavePath );
			}

			GenericWriter writer = new BinaryFileWriter( Path.Combine( SavePath, SaveFile ), true );

			writer.Write( m_LastResetTime );

			writer.Write( MobileRateInfo.Entries.Count );

			foreach ( KeyValuePair<Mobile, MobileRateInfo> kvp in MobileRateInfo.Entries )
			{
				writer.Write( (Mobile)kvp.Key );

				MobileRateInfo info = (MobileRateInfo)kvp.Value;

				info.Serialize( writer );
			}

			writer.Close();
		}

		private static void OnLoad()
		{
			if ( !File.Exists( Path.Combine( SavePath, SaveFile ) ) )
			{
				return;
			}

			using ( FileStream bin = new FileStream( Path.Combine( SavePath, SaveFile ), FileMode.Open, FileAccess.Read, FileShare.Read ) )
			{
				GenericReader reader = new BinaryFileReader( new BinaryReader( bin ) );

				m_LastResetTime = reader.ReadDateTime();

				int count = reader.ReadInt();

				for ( int i = 0; i < count; ++i )
				{
					Mobile mobile = reader.ReadMobile();

					MobileRateInfo info = new MobileRateInfo();

					info.Deserialize( reader );

					if ( mobile != null )
					{
						MobileRateInfo.Entries.Add( mobile, info );
					}
				}
			}
		}

		public GuaranteedGainSystem()
			: base( TimeSpan.FromSeconds( 30.0 ), TimeSpan.FromSeconds( 30.0 ) )
		{
			Priority = TimerPriority.FiveSeconds;
		}

		protected override void OnTick()
		{
			if ( !Enabled )
				return;

			if ( DateTime.Now >= DateTime.Now.Date + ResetTime && DateTime.Now.Date != m_LastResetTime.Date )
			{
				Reset( false );
			}
		}

		private class MobileRateInfo
		{
			private static Dictionary<Mobile, MobileRateInfo> m_Entries = new Dictionary<Mobile, MobileRateInfo>();

			public static Dictionary<Mobile, MobileRateInfo> Entries
			{
				get { return m_Entries; }
				set { m_Entries = value; }
			}

			public static MobileRateInfo GetMobileInfo( Mobile from )
			{
				MobileRateInfo info = null;

				if ( !Entries.TryGetValue( from, out info ) )
				{
					info = new MobileRateInfo();

					Entries.Add( from, info );
				}

				return info;
			}

			private Dictionary<int, SkillRateInfo> m_SkillRates;

			private DateTime m_LastStatGainTime = DateTime.MinValue;
			private int m_StatGainsCount = 0;

			public Dictionary<int, SkillRateInfo> SkillRates
			{
				get { return m_SkillRates; }
				set { m_SkillRates = value; }
			}

			public DateTime LastStatGainTime
			{
				get { return m_LastStatGainTime; }
				set { m_LastStatGainTime = value; }
			}

			public int StatGainsCount
			{
				get { return m_StatGainsCount; }
				set { m_StatGainsCount = value; }
			}

			public SkillRateInfo GetSkillInfo( Skill skill )
			{
				SkillRateInfo info = null;

				if ( !m_SkillRates.TryGetValue( skill.SkillID, out info ) )
				{
					info = new SkillRateInfo();

					SkillRates.Add( skill.SkillID, info );
				}

				return info;
			}

			public MobileRateInfo()
			{
				m_SkillRates = new Dictionary<int, SkillRateInfo>();
			}

			public void Serialize( GenericWriter writer )
			{
				writer.Write( (int)1 ); // version

				writer.Write( m_SkillRates.Count );

				foreach ( KeyValuePair<int, SkillRateInfo> kvp in m_SkillRates )
				{
					writer.Write( (int)kvp.Key );

					SkillRateInfo info = (SkillRateInfo)kvp.Value;

					info.Serialize( writer );
				}

				writer.Write( m_LastStatGainTime );
				writer.Write( m_StatGainsCount );
			}

			public void Deserialize( GenericReader reader )
			{
				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
					{
						int count = reader.ReadInt();

						for ( int i = 0; i < count; i++ )
						{
							int id = reader.ReadInt();

							SkillRateInfo info = new SkillRateInfo();

							info.Deserialize( reader );

							m_SkillRates.Add( id, info );
						}

						m_LastStatGainTime = reader.ReadDateTime();
						m_StatGainsCount = reader.ReadInt();

						break;
					}
				}
			}
		}

		private class SkillRateInfo
		{
			private DateTime m_LastGainTime;

			public DateTime LastGainTime
			{
				get { return m_LastGainTime; }
				set { m_LastGainTime = value; }
			}

			public SkillRateInfo()
			{
				m_LastGainTime = DateTime.MinValue;
			}

			public void Serialize( GenericWriter writer )
			{
				writer.Write( (int)1 ); // version

				writer.Write( m_LastGainTime );
			}

			public void Deserialize( GenericReader reader )
			{
				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
					{
						m_LastGainTime = reader.ReadDateTime();

						break;
					}
				}
			}
		}
	}
}