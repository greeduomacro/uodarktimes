using System;
using Server.Items;

namespace Server.Items
{
	public class RunicStone : Item
	{
		public override string DefaultName
		{
			get { return "A Runic Stone"; }
		}

		[Constructable]
		public RunicStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 1209;
		}

		public override void OnDoubleClick( Mobile from )
		{
			BagOfRunicTools runicBag = new BagOfRunicTools( 50 );

			if ( !from.AddToBackpack( runicBag ) )
				runicBag.Delete();
		}

		public RunicStone( Serial serial ) : base( serial )
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