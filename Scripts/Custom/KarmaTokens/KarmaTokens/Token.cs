using System;
using Server;

namespace Server.Items
{
	public class Tokens : Item
	{
		[Constructable]
		public Tokens() : this( 1 )
		{
		}

		[Constructable]
		public Tokens( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public Tokens( int amount ) : base( 0xEF0 )
		{
			Stackable = true;
			Name = "Tokens";
			Hue = 1364;
			Weight = 0;
			Amount = amount;
		}

		public Tokens( Serial serial ) : base( serial )
		{
		}

		public override int GetDropSound()
		{
			if ( Amount <= 1 )
				return 0x2E4;
			else if ( Amount <= 5 )
				return 0x2E5;
			else
				return 0x2E6;
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