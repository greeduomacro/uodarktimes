/***********************************************
*
* This script was made by milt, AKA Pokey.
*
* Email: pylon2007@gmail.com
*
* AIM: TrueBornStunna
*
* Website: www.pokey.f13nd.net
*
* Version: 2.0.0
*
* Release Date: June 29, 2006
*
************************************************/
using System;
using Server;

namespace Server.Items
{
	public class WallOfStoneEast : BaseAddon
	{
		[Constructable]
		public WallOfStoneEast()
		{
			AddComponent( new AddonComponent( 0x80 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0x80 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x80 ), 1, 0, 0 );
		}

		public WallOfStoneEast( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}