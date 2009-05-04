using System; 
using Server; 
using Server.Items;

namespace Server.Items 
{ 
	public class BagOfTinkerTools : Bag 
	{ 
		[Constructable] 
		public BagOfTinkerTools() : this( 50 ) 
		{ 
		} 

		[Constructable] 
		public BagOfTinkerTools( int amount ) 
		{ 


			Name = "Bag of Runic Tinker Tools";	

			DropItem( new ValoriteTinkerTools() );
			DropItem( new VeriteTinkerTools() );
			DropItem( new AgapiteTinkerTools() );
			DropItem( new GoldTinkerTools() );
			DropItem( new BronzeTinkerTools() );
			DropItem( new CopperTinkerTools() );
			DropItem( new ShadowIronTinkerTools() );
			DropItem( new DullCopperTinkerTools() );

		} 

		public BagOfTinkerTools( Serial serial ) : base( serial ) 
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
