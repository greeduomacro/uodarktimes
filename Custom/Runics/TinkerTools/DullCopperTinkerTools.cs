using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class DullCopperTinkerTools : RunicTinkerTools
	{

		[Constructable]
		public DullCopperTinkerTools() : this( 50 )
		{
		}		

		[Constructable]
		public DullCopperTinkerTools( int uses ) : base( CraftResource.DullCopper )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "Runic Tinker Tools";
		}
		public DullCopperTinkerTools( Serial serial ) : base( serial )
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