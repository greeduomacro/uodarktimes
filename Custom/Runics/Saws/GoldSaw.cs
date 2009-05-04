using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class GoldSaw : RunicSaw
	{

		[Constructable]
		public GoldSaw() : this( 50 )
		{
		}		

		[Constructable]
		public GoldSaw( int uses ) : base( CraftResource.Gold )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "A runic saw";
		}
		public GoldSaw( Serial serial ) : base( serial )
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