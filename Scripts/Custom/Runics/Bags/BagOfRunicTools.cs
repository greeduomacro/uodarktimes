using System; 
using Server; 
using Server.Items;

namespace Server.Items 
{ 
	public class BagOfRunicTools : Bag 
	{ 
		[Constructable] 
		public BagOfRunicTools() : this( 50 ) 
		{ 
		} 

		[Constructable] 
		public BagOfRunicTools( int amount ) 
		{ 


			Name = "Bag of Runic Tools";	

			DropItem( new BagOfHammers() );
			DropItem( new BagOfSaws() );
			DropItem( new BagOfTinkerTools() );
			DropItem( new BagOfSewingKits() );
			DropItem( new BagOfFletcherTools() );

		} 

		public BagOfRunicTools( Serial serial ) : base( serial ) 
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
