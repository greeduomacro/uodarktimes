/*
 *      Player Jailing System
 *  -------------------------------------------------------
 *  Written by:     Kitchen
 *  
 *  File:           JailInfoCommand.cs
 * 
 *  Begin:          June 13, 2008
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Commands;
using Server.Custom.Jailing;
using Server.Custom.Jailing.Gumps;
using Server.Mobiles;

namespace Server.Custom.Jailing.Commands
{
    public class JailInfoCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register( "jailinfo", AccessLevel.Player, new CommandEventHandler( JailInfo_OnCommand ) );
        }

        [Usage( "JailInfo" )]
        [Description( "Brings up a gump that shows the jailed player information reguarding their jail sentence." )]
        public static void JailInfo_OnCommand( CommandEventArgs args )
        {
            Mobile from = args.Mobile;

            if ( Core.IsPlayerJailed( (PlayerMobile)from ) )
            {
                if ( !from.HasGump( typeof( JailInfoGump ) ) )
                {
                    JailedPlayer player = Core.GetJailedPlayer( (PlayerMobile)from );
                    from.SendGump( new JailInfoGump( player.JailedBy, player.JailLength, player.JailedFor ) );
                }
            }
            else
            {
                from.SendMessage( "Only jailed players can access this command." );
            }
        }
    }
}
