/*
 *      Player Jailing System
 *  -------------------------------------------------------
 *  Written by:     Kitchen
 *  
 *  File:           SpeechHandler.cs
 * 
 *  Begin:          June 12, 2008
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Mobiles;

namespace Server.Custom.Jailing
{
    public static class SpeechHandler
    {
        private static List<char> m_CharsToRemove;

        public static void Initialize()
        {
            if ( Settings.SwearEnabled )
            {
                EventSink.Speech += new SpeechEventHandler( EventSink_Speech );

                m_CharsToRemove = new List<char>();
                m_CharsToRemove.Add( '`' );
                m_CharsToRemove.Add( '~' );
                m_CharsToRemove.Add( '!' );
                m_CharsToRemove.Add( '@' );
                m_CharsToRemove.Add( '#' );
                m_CharsToRemove.Add( '$' );
                m_CharsToRemove.Add( '%' );
                m_CharsToRemove.Add( '^' );
                m_CharsToRemove.Add( '&' );
                m_CharsToRemove.Add( '*' );
                m_CharsToRemove.Add( '(' );
                m_CharsToRemove.Add( ')' );
                m_CharsToRemove.Add( '-' );
                m_CharsToRemove.Add( '_' );
                m_CharsToRemove.Add( '=' );
                m_CharsToRemove.Add( '+' );
                m_CharsToRemove.Add( '\\' );
                m_CharsToRemove.Add( '|' );
                m_CharsToRemove.Add( '[' );
                m_CharsToRemove.Add( '{' );
                m_CharsToRemove.Add( ']' );
                m_CharsToRemove.Add( '}' );
                m_CharsToRemove.Add( ';' );
                m_CharsToRemove.Add( ':' );
                m_CharsToRemove.Add( '\'' );
                m_CharsToRemove.Add( '"' );
                m_CharsToRemove.Add( ',' );
                m_CharsToRemove.Add( '<' );
                m_CharsToRemove.Add( '.' );
                m_CharsToRemove.Add( '>' );
                m_CharsToRemove.Add( '/' );
                m_CharsToRemove.Add( '?' );
            }
        }

        private static void EventSink_Speech( SpeechEventArgs e )
        {
            Mobile from = e.Mobile;

            if ( from.Squelched )
                return;

            if ( Core.IsPlayerJailed( (PlayerMobile)from ) )
                return;

            if ( from.AccessLevel > AccessLevel.Player )
                return;

            string speech = e.Speech;

            // Strip useless characters out
            foreach ( char c in m_CharsToRemove )
            {
                speech = speech.Replace( c.ToString(), "" );
            }

            // Replace numbers with letters
            speech = speech.Replace( '0', 'o' );
            speech = speech.Replace( '1', 'i' );
            speech = speech.Replace( '3', 'e' );
            speech = speech.Replace( '4', 'a' );
            speech = speech.Replace( '5', 's' );
            speech = speech.Replace( '6', 'g' );
            speech = speech.Replace( '7', 't' );
            speech = speech.Replace( '9', 'g' );

            // Replace consecutive whitespaces
            while ( speech.IndexOf( "  " ) > -1 )
            {
                speech = speech.Replace( "  ", " " );
            }

            if ( Core.BadWords.Count > 0 )
            {
                string[] splitWords = speech.Split( ' ' );

                foreach ( string word in splitWords )
                {
                    if ( Core.BadWords.Contains( word ) )
                    {
                        // Double check for special exceptions here
                        if ( word == "ass" && !e.Speech.Contains( "ass" ) && e.Speech.Contains( "455" ) )
                            continue;   // Player was just typing the number 455!

                        Core.JailPlayer( (PlayerMobile)from, (JailCell)Utility.RandomMinMax( 1, 8 ), new TimeSpan( Settings.SwearDays, Settings.SwearHours, Settings.SwearMinutes, 0, 0 ), String.Format( "Automatically jailed for using swear word: {0}", word ), (PlayerMobile)from );
                    }
                }
            }
        }
    }
}