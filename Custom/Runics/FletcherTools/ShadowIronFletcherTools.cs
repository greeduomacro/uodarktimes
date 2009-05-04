using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class ShadowIronFletcherTools : RunicFletcherTools
	{

		[Constructable]
		public ShadowIronFletcherTools() : this( 50 )
		{
		}		

		[Constructable]
		public ShadowIronFletcherTools( int uses ) : base( CraftResource.ShadowIron )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "Runic Fletcher Tools";
		}
		public ShadowIronFletcherTools( Serial serial ) : base( serial )
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