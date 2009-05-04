using System;
using Server;

namespace Server.Items
{
	public class ApprenticeEarrings : SilverEarrings
	{

		[Constructable]
		public ApprenticeEarrings()
		{
			Name = "Apprentice Earrings";
						
			Attributes.BonusStr = 1;
			Attributes.BonusDex = 1;
			Attributes.BonusInt = 1;
			
			
		}

		public ApprenticeEarrings( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}