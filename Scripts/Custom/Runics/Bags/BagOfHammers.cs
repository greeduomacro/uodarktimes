using System; 
using Server; 
using Server.Items;

namespace Server.Items 
{ 
	public class BagOfHammers : Bag 
	{ 
		[Constructable] 
		public BagOfHammers() : this( 50 ) 
		{ 
		} 

		[Constructable] 
		public BagOfHammers( int amount ) 
		{ 


			Name = "Bag of Runic Hammers";	

			DropItem( new ValoriteHammer() );
			DropItem( new VeriteHammer() );
			DropItem( new AgapiteHammer() );
			DropItem( new GoldHammer() );
			DropItem( new BronzeHammer() );
			DropItem( new CopperHammer() );
			DropItem( new ShadowIronHammer() );
			DropItem( new DullCopperHammer() );

		} 

		public BagOfHammers( Serial serial ) : base( serial ) 
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
