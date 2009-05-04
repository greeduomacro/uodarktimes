/*
 *      Player Jailing System
 *  -------------------------------------------------------
 *  Written by:     Kitchen
 *  
 *  File:           Settings.cs
 * 
 *  Begin:          June 12, 2008
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Custom.Jailing
{
    public struct Settings
    {
        // The path to the file that stores the bad words
        public static string BadWordsPath = @"Data\Custom\Jailing\BadWords.txt";

        // The path to the file that stores the list of jailed players
        public static string JailedPlayersPath = @"Data\Custom\Jailing\JailedPlayers.txt";

        // Display events that occur in the console
        public static bool ConsoleOutput = true;

        // AccessLevels for certain commands
        public static AccessLevel AccessJailCommand = AccessLevel.Counselor;
        public static AccessLevel AccessReleaseCommand = AccessLevel.GameMaster;

        // How often to check if a player can be released from jail
        public static TimeSpan TimerTick = TimeSpan.FromSeconds( 5 );

        // Does the player have to be online to get out of jail
        public static bool MustBeOnline = true;

        // Automatically squelch players when they're jailed
        public static bool AutoSquelch = true;

        // Give the player a recall scroll after he's been released
        public static bool GiveRecall = true;

        // Hide the player when they are released
        public static bool HidePlayer = true;

        // Map to send jailed players to
        public static Map SendToMap = Map.Felucca;

        // Enable automatic jailing for players who swear
        public static bool SwearEnabled = true;

        // How long a player gets jailed for when a bad word is detected
        public static int SwearDays = 0;
        public static int SwearHours = 0;
        public static int SwearMinutes = 10;
    }
}