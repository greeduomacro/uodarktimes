using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class VeriteSaw : RunicSaw
	{

		[Constructable]
		public VeriteSaw() : this( 50 )
		{
		}		

		[Constructable]
		public VeriteSaw( int uses ) : base( CraftResource.Verite )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "A runic saw";
		}
		public VeriteSaw( Serial serial ) : base( serial )
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