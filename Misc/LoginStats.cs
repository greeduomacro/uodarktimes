using System;
using Server.Network;

namespace Server.Misc
{
	public class LoginStats
	{
		public static void Initialize()
		{
			// Register our event handler
			EventSink.Login += new LoginEventHandler( EventSink_Login );
		}

		private static void EventSink_Login( LoginEventArgs args )
		{
			int userCount = NetState.Instances.Count;
			int itemCount = World.Items.Count;
			int mobileCount = World.Mobiles.Count;

			Mobile m = args.Mobile;

			m.SendMessage( "Vítej, {0}! Zde {1} soucasne {2} uzivatel{3} online, s {4} predmety{5} a {6} pohyblivimy predmety{7} ve svete.",
				args.Mobile.Name,
				userCount == 1 ? "je" : "jsou",
				userCount, userCount == 1 ? "" : "s",
				itemCount, itemCount == 1 ? "" : "s",
				mobileCount, mobileCount == 1 ? "" : "s" );
		}
	}
}