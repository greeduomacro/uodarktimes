//Created By Animal Crackers
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class HoodedRobeOfUmbra : HoodedShroudOfShadows
  {


      
      [Constructable]
		public HoodedRobeOfUmbra()
		{
          Name = "HoodedRobeOfUmbra";
          Hue = 1155;
		}

		public HoodedRobeOfUmbra( Serial serial ) : base( serial )
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
