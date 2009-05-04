using System;

namespace Server.Items
{
	public class CrimsonCinicture : HalfApron
	{
		public override int LabelNumber{ get{ return 1075043; } } // Crimson Cincture
	
		[Constructable]
		public CrimsonCinicture() : base()
		{
			Hue = 0x485;
			
			Attributes.BonusStr = 5;
			Attributes.BonusHits = 10;
			Attributes.RegenHits = 2;
		}

		public CrimsonCinicture( Serial serial ) : base( serial )
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

