//Created by AnimalCrackers
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class RoyalBritanniaGuard : BodySash
  {


      
      [Constructable]
		public RoyalBritanniaGuard()
		{
          Name = "Royal Britannia Guard";
          Hue = 1175;
      LootType = LootType.Blessed;
		}

		public RoyalBritanniaGuard( Serial serial ) : base( serial )
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
