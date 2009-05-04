/*
 *      Player Jailing System
 *  -------------------------------------------------------
 *  Written by:     Kitchen
 *  
 *  File:           JailedPlayer.cs
 * 
 *  Begin:          June 12, 2008
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Custom.Jailing;
using Server.Custom.Jailing.Gumps;
using Server.Mobiles;

namespace Server.Custom.Jailing
{
    public class JailedPlayer
    {
        public PlayerMobile Player;
        public Point3D JailedFromPoint;
        public Map JailedFromMap;
        public TimeSpan JailLength;
        public string JailedBy;
        public string JailedFor;
        public bool CanBeRelease
        {
            get { return JailLength <= TimeSpan.Zero; }
        }

        public JailedPlayer( PlayerMobile player, Point3D jailedFromPoint, Map jailedFromMap, TimeSpan length, string jailer, string reason )
        {
            Player = player;
            JailedFromPoint = jailedFromPoint;
            JailedFromMap = jailedFromMap;
            JailLength = length;
            JailedBy = jailer;
            JailedFor = reason;
        }

        public void Release()
        {
            if ( Settings.AutoSquelch && Player.Squelched )
            {
                Player.Squelched = false;
                Player.SendMessage( "You are no longer squelched." );
            }

            Player.SendMessage( "You have served your jail sentence, you are free to go." );

            if ( Settings.GiveRecall )
                Player.AddToBackpack( new Items.RecallScroll() );

            Player.Hidden = true;
            Player.MoveToWorld( JailedFromPoint, JailedFromMap );

            if ( Player.HasGump( typeof( JailInfoGump ) ) )
                Player.CloseGump( typeof( JailInfoGump ) );

            Core.WriteLine( String.Format( "{0} has been automatically released from jail.", Player.Name ) );
            Core.JailedPlayers.Remove( this );
        }

        public void Release( Mobile from )
        {
            if ( Settings.AutoSquelch && Player.Squelched )
            {
                Player.Squelched = false;
                Player.SendMessage( "You are no longer squelched." );
            }

            Player.SendMessage( "You have served your jail sentence, you are free to go." );

            if ( Settings.GiveRecall )
                Player.AddToBackpack( new Items.RecallScroll() );

            Player.Hidden = true;
            Player.MoveToWorld( JailedFromPoint, JailedFromMap );

            Core.WriteLine( String.Format( "{0} has been released from jail by {1}.", Player.Name, from.Name ) );
            Core.JailedPlayers.Remove( this );
        }
    }
}
