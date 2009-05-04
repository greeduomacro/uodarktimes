using System; 
using Server; 
using Server.Items;

namespace Server.Items 
{ 
	public class BagOfSaws : Bag 
	{ 
		[Constructable] 
		public BagOfSaws() : this( 50 ) 
		{ 
		} 

		[Constructable] 
		public BagOfSaws( int amount ) 
		{ 

			Name = "Bag of Runic Saws";

			DropItem( new ValoriteSaw() );
			DropItem( new VeriteSaw() );
			DropItem( new AgapiteSaw() );
			DropItem( new GoldSaw() );
			DropItem( new BronzeSaw() );
			DropItem( new CopperSaw() );
			DropItem( new ShadowIronSaw() );
			DropItem( new DullCopperSaw() );

		} 

		public BagOfSaws( Serial serial ) : base( serial ) 
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
