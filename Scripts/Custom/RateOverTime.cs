using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Server.Engines
{
	public class RateOverTime : Timer
	{
		private static DateTime m_LastResetTime = DateTime.Now;

		public static bool Enabled = true; //Enable the system. Disabled by default.

        public static TimeSpan ResetTime = TimeSpan.FromHours( 6.0 ); // Time of the day
		public static string SavePath = "Saves/RateInfo";
		public static string SaveFile = "RoT.bin";

		public static bool StatGainAllowed( Mobile from )
		{
			if ( from.Player )
			{
				MobileRateInfo info = MobileRateInfo.GetMobileInfo( from );

                // STAT GAIN RESTRICTIONS
                // Here you can edit restrictions suitable for your needs 
				if ( info.StatGainsCount < 8 )
                // END!
				{
					info.StatGainsCount++;
				}
				else
				{
					return false;
				}
			}

			return true;
		}

		public static bool SkillGainAllowed( Mobile from, Skill skill )
		{
			if ( from.Player )
			{
				MobileRateInfo mobileInfo = MobileRateInfo.GetMobileInfo( from );
				SkillRateInfo skillInfo = mobileInfo.GetSkillInfo( skill );

                // SKILL GAIN RESTRICTIONS
                // Here you can edit restrictions suitable for your needs 
				if ( skill.Base >= 100.0 && ( mobileInfo.SkillGainsCount >= 72 || DateTime.Now - skillInfo.LastGainTime < TimeSpan.FromMinutes( 5.0 ) ) )
					return false;
				if ( skill.Base >= 90.0 && ( mobileInfo.SkillGainsCount >= 30 || DateTime.Now - skillInfo.LastGainTime < TimeSpan.FromMinutes( 10.0 ) ) )
					return false;
				else if ( skill.Base >= 80.0 && ( mobileInfo.SkillGainsCount >= 60 || DateTime.Now - skillInfo.LastGainTime < TimeSpan.FromMinutes( 5.0 ) ) )
					return false;
				else if ( skill.Base >= 70.0 && ( mobileInfo.SkillGainsCount >= 100 || DateTime.Now - skillInfo.LastGainTime < TimeSpan.FromMinutes( 3.0 ) ) )
					return false;
				else if ( skill.Base < 70.0 )
					return true;
                // End!

				mobileInfo.SkillGainsCount++;

				skillInfo.LastGainTime = DateTime.Now;
				skillInfo.GainsCount++;
			}

			return true;
		}

		public static void Reset()
		{
			m_LastResetTime = DateTime.Now;

			MobileRateInfo.Entries.Clear();

			/*foreach ( MobileRateInfo mobileInfo in MobileRateInfo.Entries.Values )
			{
				mobileInfo.SkillGainsCount = 0;
				mobileInfo.StatGainsCount = 0;

				foreach ( SkillRateInfo skillInfo in mobileInfo.SkillRates.Values )
				{
					skillInfo.GainsCount = 0;
					skillInfo.LastGainTime = DateTime.MinValue;
				}
			}*/
		}

		public static void Initialize()
		{
			if ( Enabled )
			{
				CommandSystem.Register( "RoTReset", AccessLevel.Administrator, new CommandEventHandler( Reset_OnCommand ) );

				new RateOverTime().Start();
			}
		}

		[Usage( "RoTReset" )]
		[Description( "Reset all information stored by Rate over Time system." )]
		private static void Reset_OnCommand( CommandEventArgs e )
		{
			Reset();

			e.Mobile.SendMessage( "Rate over Time system has being reseted." );
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

		public RateOverTime()
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
				Reset();
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

			private int m_SkillGainsCount = 0;
			private int m_StatGainsCount = 0;

			public Dictionary<int, SkillRateInfo> SkillRates
			{
				get { return m_SkillRates; }
				set { m_SkillRates = value; }
			}

			public int SkillGainsCount
			{
				get { return m_SkillGainsCount; }
				set { m_SkillGainsCount = value; }
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

				writer.Write( m_SkillGainsCount );
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

						m_SkillGainsCount = reader.ReadInt();
						m_StatGainsCount = reader.ReadInt();

						break;
					}
				}
			}
		}

		private class SkillRateInfo
		{
			private DateTime m_LastGainTime;
			private int m_GainsCount;

			public DateTime LastGainTime
			{
				get { return m_LastGainTime; }
				set { m_LastGainTime = value; }
			}

			public int GainsCount
			{
				get { return m_GainsCount; }
				set { m_GainsCount = value; }
			}

			public SkillRateInfo()
			{
				m_LastGainTime = DateTime.MinValue;
				m_GainsCount = 0;
			}

			public void Serialize( GenericWriter writer )
			{
				writer.Write( (int)1 ); // version

				writer.Write( m_LastGainTime );
				writer.Write( m_GainsCount );
			}

			public void Deserialize( GenericReader reader )
			{
				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
					{
						m_LastGainTime = reader.ReadDateTime();
						m_GainsCount = reader.ReadInt();

						break;
					}
				}
			}
		}
	}
}