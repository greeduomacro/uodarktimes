using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class SpinedSewingKit : RunicSewingKit1
	{

		[Constructable]
		public SpinedSewingKit() : this( 50 )
		{
		}		

		[Constructable]
		public SpinedSewingKit( int uses ) : base( CraftResource.SpinedLeather )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "A runic sewing kit";

		}
		public SpinedSewingKit( Serial serial ) : base( serial )
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