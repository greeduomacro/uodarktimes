using System; 
using Server; 

namespace Server.Items
{ 
	public class VoteShroud : Robe
	{
		[Constructable]
		public VoteShroud()
		{
			Name = "Dark Times Supporter Robe";
			Hue = 0xB;

		}

		public VoteShroud( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
} 