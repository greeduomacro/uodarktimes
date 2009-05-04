using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class BronzeFletcherTools : RunicFletcherTools
	{

		[Constructable]
		public BronzeFletcherTools() : this( 50 )
		{
		}		

		[Constructable]
		public BronzeFletcherTools( int uses ) : base( CraftResource.Bronze )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "Runic Fletcher Tools";
		}
		public BronzeFletcherTools( Serial serial ) : base( serial )
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