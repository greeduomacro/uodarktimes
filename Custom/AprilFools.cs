using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;

namespace Server.Items
{
	public class AprilFools : Item
	{
		[Constructable]
		public AprilFools() : base( 0x14F0 )
		{
			base.Weight = 1.0;
			base.Name = "a bank check";
			Hue = 52;

		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties( list );

			list.Add( 1060738, "100000000" ); // value: ~1_val~
		}

		public AprilFools( Serial serial ) : base( serial )
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
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{		
		from.SendMessage("If this were a real bank check you'd be rich! BUT ITS NOT! APRIL FOOLS!"  );
		}
	}
}


