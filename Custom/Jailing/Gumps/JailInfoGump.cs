/*
 *      Player Jailing System
 *  -------------------------------------------------------
 *  Written by:     Kitchen
 *  
 *  File:           JailInfoGump.cs
 * 
 *  Begin:          June 13, 2008
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.Jailing.Gumps
{
    public class JailInfoGump : Gump
    {
        public JailInfoGump( string jailer, TimeSpan timeLeft, string reason )
            : base( 50, 50 )
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = false;
            this.Resizable = false;

            AddPage( 0 );
            AddBackground( 1, 1, 405, 280, 3600 );
            AddAlphaRegion( 16, 15, 375, 250 );
            AddHtml( 17, 30, 375, 25, @"<BASEFONT COLOR#000008><CENTER>You have been jailed!</CENTER></BASEFONT>", false, false );
            AddHtml( 15, 30, 375, 25, @"<BASEFONT COLOR#000008><CENTER>You have been jailed!</CENTER></BASEFONT>", false, false );
            AddHtml( 16, 29, 375, 25, @"<BASEFONT COLOR#000008><CENTER>You have been jailed!</CENTER></BASEFONT>", false, false );
            AddHtml( 16, 31, 375, 25, @"<BASEFONT COLOR#000008><CENTER>You have been jailed!</CENTER></BASEFONT>", false, false );
            AddHtml( 16, 30, 375, 25, @"<BASEFONT COLOR#FFFFFF><CENTER>You have been jailed!</CENTER></BASEFONT>", false, false );
            AddImageTiled( 23, 58, 350, 1, 9107 );
            AddImageTiled( 28, 60, 350, 1, 9157 );
            AddLabel( 20, 70, 1152, @"Jailed By:" );
            AddLabel( 20, 100, 1152, @"Time Left:" );
            AddLabel( 20, 130, 1152, @"Notes: " );
            AddLabel( 20, 160, 1152, @"Reason:" );
            AddLabel( 100, 70, 1152, String.Format( "{0}", jailer ) );

            if ( timeLeft.Minutes <= 0 )
            {
                AddLabel( 100, 100, 1152, @"You will be released soon" );
            }
            else
            {
                AddLabel( 100, 100, 1152, String.Format( "{0} day{1}, {2} hour{3}, and {4} minute{5}",
                    timeLeft.Days, timeLeft.Days == 1 ? "" : "s",
                    timeLeft.Hours, timeLeft.Hours == 1 ? "" : "s",
                    timeLeft.Minutes, timeLeft.Minutes == 1 ? "" : "s" ) );
            }

            AddLabel( 100, 130, 1152, String.Format( "{0}", Settings.MustBeOnline ? "You must remain online to serve your sentence" : "None" ) );
            AddHtml( 100, 160, 286, 100, String.Format( "{0}", reason ), true, true );
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            Mobile from = sender.Mobile;

            switch ( info.ButtonID )
            {
                case 0:
                {
                    if ( Core.IsPlayerJailed( (PlayerMobile)from ) )
                        from.SendMessage( "To view this page again, type [jailinfo." );

                    break;
                }
            }
        }
    }
}