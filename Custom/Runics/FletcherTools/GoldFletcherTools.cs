using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class GoldFletcherTools : RunicFletcherTools
	{

		[Constructable]
		public GoldFletcherTools() : this( 50 )
		{
		}		

		[Constructable]
		public GoldFletcherTools( int uses ) : base( CraftResource.Gold )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "Runic Fletcher Tools";
		}
		public GoldFletcherTools( Serial serial ) : base( serial )
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