using System; 
using Server; 
using Server.Items;

namespace Server.Items 
{ 
	public class BagOfFletcherTools : Bag 
	{ 
		[Constructable] 
		public BagOfFletcherTools() : this( 50 ) 
		{ 
		} 

		[Constructable] 
		public BagOfFletcherTools( int amount ) 
		{ 


			Name = "Bag of Runic Fletcher Tools";	

			DropItem( new ValoriteFletcherTools() );
			DropItem( new VeriteFletcherTools() );
			DropItem( new AgapiteFletcherTools() );
			DropItem( new GoldFletcherTools() );
			DropItem( new BronzeFletcherTools() );
			DropItem( new CopperFletcherTools() );
			DropItem( new ShadowIronFletcherTools() );
			DropItem( new DullCopperFletcherTools() );

		} 

		public BagOfFletcherTools( Serial serial ) : base( serial ) 
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
