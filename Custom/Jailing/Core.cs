/*
 *      Player Jailing System
 *  -------------------------------------------------------
 *  Written by:     Kitchen
 *  
 *  File:           Core.cs
 * 
 *  Begin:          June 12, 2008
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Server;
using Server.Custom.Jailing;
using Server.Custom.Jailing.Gumps;
using Server.Mobiles;

namespace Server.Custom.Jailing
{
    public class Core
    {
        public static List<JailedPlayer> JailedPlayers;
        public static List<string> BadWords;
        public static JailTimer ReleaseTimer;
        public static Thread SaveThread;

        #region Logging

        public static void WriteLine( string value )
        {
            if ( Settings.ConsoleOutput )
            {
                Console.WriteLine( String.Format( "Jail System: {0}", value ) );
            }
        }

        public static void WriteLine( Exception e, string message )
        {
            if ( Settings.ConsoleOutput )
            {
                WriteLine( message );

                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.Write( String.Format( "\nError:\n\tMessage: {0}\n\tException: {1}\n\tSource: {2}\n\tSite: {3}",
                    e.Message, e.InnerException, e.Source, e.TargetSite ) );

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        #endregion

        #region Initialization, Loading, and Saving

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler( EventSink_WorldSave );

            WriteLine( "Initializing..." );

            // Timer Initialization
            ReleaseTimer = new JailTimer();
            ReleaseTimer.Start();
            // Load list of previously jailed players
            JailedPlayers = new Core().Load();
            // Bad Word List Initialization
            BadWords = new List<string>();

            try
            {
                using ( StreamReader reader = new StreamReader( Settings.BadWordsPath ) )
                {
                    while ( !reader.EndOfStream )
                    {
                        BadWords.Add( reader.ReadLine() );
                    }
                }
            }
            catch ( Exception e )
            {
                WriteLine( e, "Error loading bad words from text file." );
            }

            WriteLine( String.Format( "{0} bad words loaded successfully.", BadWords.Count ) );

            WriteLine( "Initializing finished" );
        }

        private static void EventSink_WorldSave( WorldSaveEventArgs e )
        {
            Core.Save();
        }

        public List<JailedPlayer> Load()
        {
            try
            {
                List<JailedPlayer> temp = new List<JailedPlayer>();

                using ( StreamReader reader = new StreamReader( Settings.JailedPlayersPath ) )
                {
                    while ( !reader.EndOfStream )
                    {
                        string readLine = reader.ReadLine();

                        if ( readLine.StartsWith( "#" ) || readLine.Length <= 0 ) // It's a comment/empty line, skip it!
                            continue;

                        string[] line = readLine.Split( '\t' );

                        if ( line.Length != 6 ) // Corrupt line, skip it!
                            continue;

                        int serial = Convert.ToInt32( line[0] );

                        string[] xyz = line[1].Split( ',' );

                        if ( xyz.Length != 3 ) // Corrupt line, skip it!
                            continue;

                        int x = Convert.ToInt32( xyz[0] );
                        int y = Convert.ToInt32( xyz[1] );
                        int z = Convert.ToInt32( xyz[2] );

                        string map = line[2];

                        string[] ddhhmm = line[3].Split( ',' );

                        if ( ddhhmm.Length != 4 ) // Corrupt line, skip it!
                            continue;

                        int dd = Convert.ToInt32( ddhhmm[0] );
                        int hh = Convert.ToInt32( ddhhmm[1] );
                        int mm = Convert.ToInt32( ddhhmm[2] );
                        int ss = Convert.ToInt32( ddhhmm[3] );

                        string jailer = line[4];
                        string reason = line[5];

                        PlayerMobile tempPlayer = (PlayerMobile)World.FindEntity( serial );
                        Point3D tempPoint = new Point3D( x, y, z );
                        Map tempMap;

                        switch ( map.ToLower() )
                        {
                            #region Cases
                            case "trammel":
                            {
                                tempMap = Map.Trammel;
                                break;
                            }
                            case "felucca":
                            {
                                tempMap = Map.Felucca;
                                break;
                            }
                            case "ilshenar":
                            {
                                tempMap = Map.Ilshenar;
                                break;
                            }
                            case "malas":
                            {
                                tempMap = Map.Malas;
                                break;
                            }
                            case "tokuno":
                            {
                                tempMap = Map.Tokuno;
                                break;
                            }
                            default:
                            {
                                tempMap = Map.Felucca;
                                break;
                            }
                            #endregion
                        }

                        TimeSpan tempTime = new TimeSpan( dd, hh, mm, 0, 0 );

                        temp.Add( new JailedPlayer( tempPlayer, tempPoint, tempMap, tempTime, jailer, reason ) );
                    }

                    WriteLine( String.Format( "{0} jailed player{1} loaded.", temp.Count, temp.Count == 1 ? "" : "s" ) );

                    return temp;
                }
            }
            catch ( Exception e )
            {
                WriteLine( e, "Error while loading the list of jailed players." );

                return new List<JailedPlayer>();
            }
        }

        public static void Save()
        {
            SaveThread = new Thread( new ThreadStart( DoSave ) );
            SaveThread.IsBackground = true;
            SaveThread.Priority = ThreadPriority.BelowNormal;
            SaveThread.Start();
        }

        public static void DoSave()
        {
            try
            {
                using ( StreamWriter writer = new StreamWriter( Settings.JailedPlayersPath ) )
                {
                    writer.WriteLine( "# FORMAT:" );
                    writer.WriteLine( "#" );
                    writer.WriteLine( "# [Serial] [X,Y,Z] [Map] [DD,HH,MM,SS] [Jailer] [Reason]" );
                    writer.WriteLine( "#" );

                    foreach ( JailedPlayer player in JailedPlayers )
                    {
                        string saveString = "";

                        saveString += player.Player.Serial.ToString().Replace( "0x", "" );
                        saveString += "\t";
                        saveString += player.JailedFromPoint.X + "," + player.JailedFromPoint.Y + "," + player.JailedFromPoint.Z;
                        saveString += "\t";
                        saveString += player.JailedFromMap.Name;
                        saveString += "\t";
                        saveString += player.JailLength.Days + "," + player.JailLength.Hours + "," + player.JailLength.Minutes + "," + player.JailLength.Seconds;
                        saveString += "\t";
                        saveString += player.JailedBy;
                        saveString += "\t";
                        saveString += player.JailedFor;
                        
                        writer.WriteLine( saveString );
                    }

                    WriteLine( "Saved the list of jailed players." );
                }
            }
            catch ( Exception e )
            {
                WriteLine( e, "Error saving the list of jailed players." );
            }
            finally
            {
                SaveThread.Abort();
            }
        }

        #endregion

        #region Jailing

        public static void JailPlayer( PlayerMobile criminal, JailCell cell, TimeSpan length, string reason, PlayerMobile jailer )
        {
            Point3D jailedFromPoint = criminal.Location;
            Map jailedFromMap = criminal.Map;

            if ( Settings.AutoSquelch && !criminal.Squelched )
            {
                criminal.Squelched = true;
                criminal.SendMessage( "You have been squelched." );
            }

            if ( criminal.Mounted )
            {
                if ( criminal.Mount is BaseMount )
                {
                    BaseMount pet = (BaseMount)criminal.Mount;
                    pet.Rider = null;
                    StablePet( criminal, pet );
                }
            }

            foreach ( Mobile mobile in World.Mobiles.Values )
            {
                if ( mobile is BaseCreature )
                {
                    BaseCreature creature = (BaseCreature)mobile;

                    if ( creature.Controlled && creature.ControlMaster == criminal )
                    {
                        StablePet( criminal, creature );
                    }
                }
            }

            criminal.MoveToWorld( GetCellLocation( cell ), Settings.SendToMap );

            JailedPlayers.Add( new JailedPlayer( criminal, jailedFromPoint, jailedFromMap, length, jailer.Name, reason ) );

            criminal.SendGump( new JailInfoGump( jailer.Name, length, reason ) );

            criminal.SendMessage( String.Format( "You have been jailed by {0} for {1} day{2}, {3} hour{4}, and {5} minute{6}.",
                criminal == jailer ? "yourself" : jailer.Name,
                length.Days, length.Days == 1 ? "" : "s",
                length.Hours, length.Hours == 1 ? "" : "s",
                length.Minutes, length.Minutes == 1 ? "" : "s" ) );
            criminal.SendMessage( String.Format( "Reason: {0}", reason ) );

            WriteLine( String.Format( "{0} has been jailed for {1} day{2}, {3} hour{4}, and {5} minute{6} for {7} by {8}.",
                criminal.Name,
                length.Days, length.Days == 1 ? "" : "s",
                length.Hours, length.Hours == 1 ? "" : "s",
                length.Minutes, length.Minutes == 1 ? "" : "s",
                reason,
                jailer.Name ) );
        }

        public static void StablePet( PlayerMobile owner, BaseCreature pet )
        {
            pet.Internalize();
            pet.IsStabled = true;
            owner.Stabled.Add( pet );
            owner.SendMessage( String.Format( "{0} has been stabled.", pet.Name ) );
        }

        #endregion

        #region Utilities

        public static Point3D GetCellLocation( JailCell cell )
        {
            switch ( cell )
            {
                case JailCell.SmallCellOne:
                {
                    return new Point3D( 5276, 1164, 0 );
                }
                case JailCell.SmallCellTwo:
                {
                    return new Point3D( 5286, 1164, 0 );
                }
                case JailCell.SmallCellThree:
                {
                    return new Point3D( 5296, 1164, 0 );
                }
                case JailCell.SmallCellFour:
                {
                    return new Point3D( 5306, 1164, 0 );
                }
                case JailCell.SmallCellFive:
                {
                    return new Point3D( 5276, 1174, 0 );
                }
                case JailCell.SmallCellSix:
                {
                    return new Point3D( 5286, 1174, 0 );
                }
                case JailCell.SmallCellSeven:
                {
                    return new Point3D( 5296, 1174, 0 );
                }
                case JailCell.SmallCellEight:
                {
                    return new Point3D( 5306, 1174, 0 );
                }
                case JailCell.BigCellOne:
                {
                    return new Point3D( 5283, 1184, 0 );
                }
                case JailCell.BigCellTwo:
                {
                    return new Point3D( 5304, 1184, 0 );
                }
                default:
                {
                    return new Point3D( 5283, 1184, 0 );
                }
            }
        }

        public static JailedPlayer GetJailedPlayer( PlayerMobile player )
        {
            foreach ( JailedPlayer jailedPlayer in JailedPlayers )
            {
                if ( jailedPlayer.Player == player )
                    return jailedPlayer;
            }

            return null;
        }

        public static bool IsPlayerJailed( PlayerMobile player )
        {
            foreach ( JailedPlayer jailedPlayer in JailedPlayers )
            {
                if ( jailedPlayer.Player == player )
                    return true;
            }

            return false;
        }

        #endregion
    }
}