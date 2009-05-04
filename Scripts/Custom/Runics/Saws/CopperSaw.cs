using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class CopperSaw : RunicSaw
	{

		[Constructable]
		public CopperSaw() : this( 50 )
		{
		}		

		[Constructable]
		public CopperSaw( int uses ) : base( CraftResource.Copper )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "A runic saw";
		}
		public CopperSaw( Serial serial ) : base( serial )
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