using System; 
using Server; 

namespace Server.Items
{ 
	public class VoteSandals : Sandals
	{
		[Constructable]
		public VoteSandals()
		{
			Name = "Dark Times Supporters Sandals";
			Hue = 0xB;
		}

		public VoteSandals( Serial serial ) : base( serial )
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