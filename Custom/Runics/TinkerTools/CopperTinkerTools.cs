using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class CopperTinkerTools : RunicTinkerTools
	{

		[Constructable]
		public CopperTinkerTools() : this( 50 )
		{
		}		

		[Constructable]
		public CopperTinkerTools( int uses ) : base( CraftResource.Copper )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "Runic Tinker Tools";
		}
		public CopperTinkerTools( Serial serial ) : base( serial )
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