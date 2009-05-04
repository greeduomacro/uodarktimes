using System;
using System.Collections;
using System.IO;
using Server;
using Server.Accounting;

namespace Knives.Utils
{
	public class GumpInfo
	{
		private static Hashtable s_Infos = new Hashtable();
		private static ArrayList s_Backgrounds = new ArrayList();

		public static void Configure()
		{
			EventSink.WorldLoad += new WorldLoadEventHandler( OnLoad );
			EventSink.WorldSave += new WorldSaveEventHandler( OnSave );
		}

		public static void Initialize()
		{
			s_Backgrounds.Add( 0xA3C );
			s_Backgrounds.Add( 0x53 );
			s_Backgrounds.Add( 0x2486 );
			s_Backgrounds.Add( 0xDAC );
			s_Backgrounds.Add( 0xE10 );
			s_Backgrounds.Add( 0x13EC );
			s_Backgrounds.Add( 0x1400 );
			s_Backgrounds.Add( 0x2422 );
			s_Backgrounds.Add( 0x242C );
			s_Backgrounds.Add( 0x13BE );
			s_Backgrounds.Add( 0x2436 );
			s_Backgrounds.Add( 0x2454 );
			s_Backgrounds.Add( 0x251C );
			s_Backgrounds.Add( 0x254E );
			s_Backgrounds.Add( 0x24A4 );
			s_Backgrounds.Add( 0x24AE );
		}

		private static void OnSave( WorldSaveEventArgs e )
		{try{

			if ( !Directory.Exists( "Saves/Gumps/" ) )
				Directory.CreateDirectory( "Saves/Gumps/" );

			GenericWriter writer = new BinaryFileWriter( Path.Combine( "Saves/Gumps/", "Gumps.bin" ), true );

			writer.Write( 0 ); // version

			ArrayList list = new ArrayList();

			GumpInfo gumpi;
			foreach( object obj in new ArrayList( s_Infos.Values ) )
			{
				if ( !(obj is Hashtable) )
					continue;

				foreach( object obje in new ArrayList( ((Hashtable)obj).Values ) )
				{
					if ( !(obje is GumpInfo ) )
						continue;

					gumpi = (GumpInfo)obje;

					if ( gumpi.Mobile != null
					&& gumpi.Mobile.Player
					&& !gumpi.Mobile.Deleted
					&& gumpi.Mobile.Account != null
					&& ((Account)gumpi.Mobile.Account).LastLogin > DateTime.Now - TimeSpan.FromDays( 30 ) )
						list.Add( obje );
				}
			}

			writer.Write( list.Count );

			foreach( GumpInfo info in list )
				info.Save( writer );

			writer.Close();

		}catch{ Errors.Report( "GumpInfo-> OnSave" ); } }

		private static void OnLoad()
		{try{

			if ( !File.Exists( Path.Combine( "Saves/Gumps/", "Gumps.bin" ) ) )
				return;

			using ( FileStream bin = new FileStream( Path.Combine( "Saves/Gumps/", "Gumps.bin" ), FileMode.Open, FileAccess.Read, FileShare.Read ) )
			{
				GenericReader reader = new BinaryFileReader( new BinaryReader( bin ) );

				int version = reader.ReadInt();

				if ( version >= 0 )
				{
					int count = reader.ReadInt();
					GumpInfo info;

					for( int i = 0; i < count; ++i )
					{
						info = new GumpInfo();
						info.Load( reader );

						if ( info.Mobile == null || info.Type == null )
							continue;

						if ( s_Infos[info.Mobile] == null )
							s_Infos[info.Mobile] = new Hashtable();

						((Hashtable)s_Infos[info.Mobile])[info.Type] = info;
					}
				}

				reader.End();
			}

		}catch{ Errors.Report( "GumpInfo-> OnLoad" ); } }

		public static GumpInfo GetInfo( Mobile m, Type type )
		{
			if ( s_Infos[m] == null )
				s_Infos[m] = new Hashtable();

			Hashtable table = (Hashtable)s_Infos[m];

			if ( table[type] == null )
				table[type] = new GumpInfo( m, type );

			return (GumpInfo)table[type];
		}

		public static GumpInfo GetPreviousInfo( Mobile m, GumpInfo info )
		{
			if ( s_Infos[m] == null )
				s_Infos[m] = new Hashtable();

			ArrayList list = new ArrayList( ((Hashtable)s_Infos[m]).Values );

			for( int i = 0; i < list.Count; ++i )
			{
				if ( !(list[i] is GumpInfo) )
					continue;

				if ( list[i] == info )
				{
					if ( i == 0 )
						return (GumpInfo)list[list.Count-1];

					return (GumpInfo)list[i-1];
				}
			}

			return info;
		}

		public static GumpInfo GetNextInfo( Mobile m, GumpInfo info )
		{
			if ( s_Infos[m] == null )
				s_Infos[m] = new Hashtable();

			ArrayList list = new ArrayList( ((Hashtable)s_Infos[m]).Values );

			for( int i = 0; i < list.Count; ++i )
			{
				if ( !(list[i] is GumpInfo) )
					continue;

				if ( list[i] == info )
				{
					if ( i == list.Count-1 )
						return (GumpInfo)list[0];

					return (GumpInfo)list[i+1];
				}
			}

			return info;
		}

		public static void RemoveInfo( Mobile m, GumpInfo info )
		{
			if ( s_Infos[m] == null )
				s_Infos[m] = new Hashtable();

			if ( ((Hashtable)s_Infos[m])[info.Type] == null )
				return;

			((Hashtable)s_Infos[m])[info.Type] = null;
		}

		public static bool HasMods( Mobile m, Type type )
		{
			if ( s_Infos[m] == null )
				s_Infos[m] = new Hashtable();

			if ( ((Hashtable)s_Infos[m])[type] == null )
				return false;

			return true;
		}

		private Mobile c_Mobile;
		private Type c_Type;
		private bool c_Transparent, c_DefaultTrans;
		private string c_TextColorRGB;
		private int c_Background;
		private Template c_Template;

		public Mobile Mobile{ get{ return c_Mobile; } }
		public Type Type{ get{ return c_Type; } }
		public bool Transparent{ get{ return c_Transparent; } set{ c_Transparent = value; } }
		public bool DefaultTrans{ get{ return c_DefaultTrans; } set{ c_DefaultTrans = value; } }
		public string TextColorRGB{ get{ return c_TextColorRGB; } set{ c_TextColorRGB = value; } }
		public string TextColor{ get{ return String.Format( "<BASEFONT COLOR=#{0}>", c_TextColorRGB ); } }
		public int Background{ get{ return c_Background; } }
		public Template Template{ get{ return c_Template; } }

		public GumpInfo()
		{
		}

		public GumpInfo( Mobile m, Type type )
		{
			c_Mobile = m;
			c_Type = type;
			c_TextColorRGB = "";
			c_Background = -1;
			c_Template = (Template)(-1);
			c_DefaultTrans = true;
		}

		public void BackgroundUp()
		{
			if ( c_Background == -1 )
			{
				c_Background = (int)s_Backgrounds[0];
				return;
			}

			for( int i = 0; i < s_Backgrounds.Count; ++ i )
				if ( c_Background == (int)s_Backgrounds[i] )
				{
					if ( i == s_Backgrounds.Count-1 )
					{
						c_Background = (int)s_Backgrounds[0];
						return;
					}

					c_Background = (int)s_Backgrounds[i+1];
					return;
				}
		}

		public void BackgroundDown()
		{
			if ( c_Background == -1 )
			{
				c_Background = (int)s_Backgrounds[s_Backgrounds.Count-1];
				return;
			}

			for( int i = 0; i < s_Backgrounds.Count; ++ i )
				if ( c_Background == (int)s_Backgrounds[i] )
				{
					if ( i == 0 )
					{
						c_Background = (int)s_Backgrounds[s_Backgrounds.Count-1];
						return;
					}

					c_Background = (int)s_Backgrounds[i-1];
					return;
				}
		}

		public void TemplateUp()
		{
			c_Template++;

			if ( c_Template.ToString().Length < 3 )
				c_Template = (Template)0;
		}

		public void TemplateDown()
		{
			c_Template--;

			if ( c_Template < 0 )
				for( c_Template = 0; ; ++c_Template )
					if ( c_Template.ToString().Length < 3 )
					{
						c_Template--;
						return;
					}
					
		}

		private void Save( GenericWriter writer )
		{try{

			writer.Write( 1 ); // version

			// Version 1
			writer.Write( (int)c_Template );

			// Version 0
			writer.Write( c_Mobile );
			writer.Write( c_Type.ToString() );
			writer.Write( c_Transparent );
			writer.Write( c_DefaultTrans );
			writer.Write( c_TextColorRGB );
			writer.Write( c_Background );

		}catch{ Errors.Report( "GumpInfo -> Save" ); } }

		private void Load( GenericReader reader )
		{try{
			int version = reader.ReadInt();

			if ( version >= 1 )
				c_Template = (Template)reader.ReadInt();

			if ( version >= 0 )
			{
				c_Mobile = reader.ReadMobile();
				c_Type = ScriptCompiler.FindTypeByFullName( reader.ReadString() );
				c_Transparent = reader.ReadBool();
				c_DefaultTrans = reader.ReadBool();
				c_TextColorRGB = reader.ReadString();
				c_Background = reader.ReadInt();
			}

		}catch{ Errors.Report( "GumpInfo -> Load" ); } }
	}
}