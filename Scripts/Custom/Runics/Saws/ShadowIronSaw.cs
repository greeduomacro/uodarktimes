using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class ShadowIronSaw : RunicSaw
	{

		[Constructable]
		public ShadowIronSaw() : this( 50 )
		{
		}		

		[Constructable]
		public ShadowIronSaw( int uses ) : base( CraftResource.ShadowIron )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "A runic saw";
		}
		public ShadowIronSaw( Serial serial ) : base( serial )
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