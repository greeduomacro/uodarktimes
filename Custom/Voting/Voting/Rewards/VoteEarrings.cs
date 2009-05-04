using System; 
using Server; 

namespace Server.Items
{ 
	public class VoteEarrings : SilverEarrings
	{
		[Constructable]
		public VoteEarrings()
		{
			Name = "Dark Times Supporter Earrings";
			Hue = 0xB;

		}

		public VoteEarrings( Serial serial ) : base( serial )
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