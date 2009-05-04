using System; 
using Server; 

namespace Server.Items
{ 
	public class VoteHalf : HalfApron
	{
		[Constructable]
		public VoteHalf()
		{
			Name = "Dark Times Supporter Apron";
			Hue = 0xB;

			Attributes.BonusDex = 1;
			Attributes.BonusStr = 1;
			Attributes.BonusInt = 1;
		}

		public VoteHalf( Serial serial ) : base( serial )
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