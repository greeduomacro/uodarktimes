/*
 *      Player Jailing System
 *  -------------------------------------------------------
 *  Written by:     Kitchen
 *  
 *  File:           JailTimer.cs
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
using Server.Network;

namespace Server.Custom.Jailing
{
    public class JailTimer : Timer
    {
        public JailTimer()
            : base( Settings.TimerTick, Settings.TimerTick )
        {
            Priority = TimerPriority.FiftyMS;
        }

        protected override void OnTick()
        {
            for ( int i = 0; i < Core.JailedPlayers.Count; i++ )
            {
                if ( Core.JailedPlayers[i].Player == null )
                {
                    Core.JailedPlayers.Remove( Core.JailedPlayers[i] );
                    continue;
                }

                if ( Core.JailedPlayers[i].Player.Region.Name != "Jail" )
                {
                    Core.WriteLine( String.Format( "{0} tried to escape from jail using the stuck menu.", Core.JailedPlayers[i].Player.Name ) );
                    Core.JailedPlayers[i].Player.MoveToWorld( Core.GetCellLocation( (JailCell)Utility.RandomMinMax( 1, 10 ) ), Settings.SendToMap );
                }

                if ( Settings.MustBeOnline )
                {
                    if ( NetState.Instances.Contains( Core.JailedPlayers[i].Player.NetState ) )
                    {
                        Core.JailedPlayers[i].JailLength -= Settings.TimerTick;
                    }
                }
                else
                {
                    Core.JailedPlayers[i].JailLength -= Settings.TimerTick;
                }

                if ( Core.JailedPlayers[i].CanBeRelease )
                {
                    Core.JailedPlayers[i].Release();
                }
            }
        }
    }
}
