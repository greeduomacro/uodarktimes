/*
 *      Player Jailing System
 *  -------------------------------------------------------
 *  Written by:     Kitchen
 *  
 *  File:           ReleaseCommand.cs
 * 
 *  Begin:          June 12, 2008
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Commands;
using Server.Custom.Jailing;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.Jailing.Commands
{
    public class ReleaseCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register( "release", Settings.AccessReleaseCommand, new CommandEventHandler( Release_OnCommand ) );
        }

        [Usage( "Release" )]
        [Description( "Releases a player from jail." )]
        public static void Release_OnCommand( CommandEventArgs args )
        {
            Mobile from = args.Mobile;

            from.Target = new ReleaseTarget();
        }

        private class ReleaseTarget : Target
        {
            public ReleaseTarget()
                : base( 12, false, TargetFlags.None )
            {
                CheckLOS = false;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
                if ( targeted is Mobile )
                {
                    if ( targeted is PlayerMobile )
                    {
                        PlayerMobile target = (PlayerMobile)targeted;

                        if ( Core.IsPlayerJailed( target ) )
                        {
                            JailedPlayer toRelease = Core.GetJailedPlayer( target );

                            if ( toRelease == null )
                            {
                                from.SendMessage( "You can only release players who are in jail." );
                            }
                            else
                            {
                                toRelease.Release( from );
                                from.SendMessage( String.Format( "{0} has been release from jail.", toRelease.Player.Name ) );
                            }
                            return;
                        }
                        else
                        {
                            from.SendMessage( "You can only release players who are in jail." );
                        }
                    }
                    else
                    {
                        from.SendMessage( "You can only release players who are in jail." );
                    }
                }
                else
                {
                    from.SendMessage( "You can only release players who are in jail." );
                }
            }
        }
    }
}
