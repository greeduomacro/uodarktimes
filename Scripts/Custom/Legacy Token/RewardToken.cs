using System; 
using Server; 
using Server.Gumps; 
using Server.Network; 

    namespace Server.Items 
    { 
    	public class SpecialRewardToken : Item 
    	{
    		[Constructable] 
    		public SpecialRewardToken() : this( null ) 
   			{ 
    		} 

    		[Constructable]
        	public SpecialRewardToken(String name): base(13945)
    		{
        		Name = "Legacy Token";
        		Stackable = false;
        		Weight = 1.0;
        		LootType = LootType.Blessed;
        
    		}

        	public SpecialRewardToken(Serial serial)
            	: base(serial) 
    			{ 
    			} 

    		public override void OnDoubleClick( Mobile from ) 
    		{ 
    			if ( !IsChildOf( from.Backpack ) ) 
    			{ 
    				from.SendLocalizedMessage( 1042001 ); 
    			} 
    			else 
    			{
            		from.SendGump( new RewardGump( from, this ) ); 
    			} 
    		} 

    		public override void Serialize ( GenericWriter writer) 
    		{ 
    			base.Serialize ( writer ); 
   				writer.Write ( (int) 0); 
    		} 

    		public override void Deserialize( GenericReader reader ) 
    		{ 
    			base.Deserialize ( reader ); 
    			int version = reader.ReadInt(); 
    		} 
    	} 
    }
