/*
 *      Player Jailing System
 *  -------------------------------------------------------
 *  Written by:     Kitchen
 *  
 *  File:           JailCommand.cs
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
    public class JailCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register( "jail", Settings.AccessJailCommand, new CommandEventHandler( Jail_OnCommand ) );
        }

        [Usage( "Jail <cell> <days> <hours> <minutes> <reason>" )]
        [Description( "Sends a targeted player to jail." )]
        public static void Jail_OnCommand( CommandEventArgs args )
        {
            Mobile from = args.Mobile;

            if ( args.Arguments.Length < 5 )
            {
                from.SendMessage( "The [jail command takes at least 5 parameters" );
                from.SendMessage( "<cell> <days> <hours> <minutes> <reason>" );
                return;
            }

            int index = args.Arguments[0].Length + args.Arguments[1].Length + args.Arguments[2].Length + args.Arguments[3].Length + 4;
            int cell = Convert.ToInt32( args.Arguments[0] );
            int days = Convert.ToInt32( args.Arguments[1] );
            int hours = Convert.ToInt32( args.Arguments[2] );
            int minutes = Convert.ToInt32( args.Arguments[3] );
            string reason = args.ArgString.Substring( index );

            if ( cell == 0 )
            {
                cell = Utility.RandomMinMax( 1, 8 );
            }

            if ( cell > 10 || cell < 1 )
            {
                from.SendMessage( "Invalid jail cell specified. Use a number between 1 and 10, or 0 for a random small cell." );
                return;
            }

            if ( days + hours + minutes <= 0 )
            {
                from.SendMessage( "Invalid time specified. Please jail someone for at least 1 minute." );
                return;
            }

            from.Target = new JailTarget( (JailCell)cell, days, hours, minutes, reason );
        }

        private class JailTarget : Target
        {
            private JailCell m_Cell;
            private TimeSpan m_Length;
            private string m_Reason;

            public JailTarget( JailCell cell, int days, int hours, int minutes, string reason )
                : base( 12, false, TargetFlags.None )
            {
                CheckLOS = false;
                m_Cell = (JailCell)cell;
                m_Length = new TimeSpan( days, hours, minutes, 0, 0 );
                m_Reason = reason;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
                if ( targeted is Mobile )
                {
                    if ( targeted is PlayerMobile )
                    {
                        PlayerMobile criminal = (PlayerMobile)targeted;

                        if ( Core.IsPlayerJailed( criminal ) )
                        {
                            from.SendMessage( String.Format( "{0} has already been jailed.", criminal.Name ) );
                            return;
                        }

                        if ( criminal.AccessLevel == AccessLevel.Player )
                        {
                            Core.JailPlayer( criminal, m_Cell, m_Length, m_Reason, (PlayerMobile)from );

                            from.SendMessage( String.Format( "{0} has been sent to jail cell number {1} for {2} day{3}, {4} hour{5}, and {6} minute{7} for {8}.",
                                criminal.Name,
                                (int)m_Cell,
                                m_Length.Days, m_Length.Days == 1 ? "" : "s",
                                m_Length.Hours, m_Length.Hours == 1 ? "" : "s",
                                m_Length.Minutes, m_Length.Minutes == 1 ? "" : "s",
                                m_Reason ) );
                        }
                        else
                        {
                            from.SendMessage( "You cannot jail staff members." );
                        }
                    }
                    else
                    {
                        from.SendMessage( "You can only jail players." );
                    }
                }
                else
                {
                    from.SendMessage( "You can only jail players." );
                }
            }
        }
    }
}
