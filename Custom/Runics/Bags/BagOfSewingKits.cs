using System; 
using Server; 
using Server.Items;

namespace Server.Items 
{ 
	public class BagOfSewingKits : Bag 
	{ 
		[Constructable] 
		public BagOfSewingKits() : this( 50 ) 
		{ 
		} 

		[Constructable] 
		public BagOfSewingKits( int amount ) 
		{ 


			Name = "Bag of Runic Sewing Kits";	

			DropItem( new BarbedSewingKit() );
			DropItem( new SpinedSewingKit() );
			DropItem( new HornedSewingKit() );
	
		} 

		public BagOfSewingKits( Serial serial ) : base( serial ) 
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
