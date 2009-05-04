using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{		
	public class PointyEarsQuest : BaseQuest
	{				
		/* Pointy Ears */
		public override object Title{ get{ return 1074640; } }
		
		/* I've heard ... there's some that will pay a good bounty for pointed ears, much like we used to pay for each 
		wolf skin.  I've got nothing personal against these elves.  It's just business.  You want in on this?  I'm not 
		fussy who I work with. */	
		public override object Description{ get{ return 1074641; } }
		
		/* Suit yourself. */
		public override object Refuse{ get{ return 1074642; } }
		
		/* I can't pay a bounty if you don't bring bag the ears. */
		public override object Uncomplete{ get{ return 1074643; } }
		
		/* Here to collect on a bounty? */
		public override object Complete{ get{ return 1074644; } }
		
		public PointyEarsQuest() : base()
		{							
			AddObjective( new ObtainObjective( typeof( SeveredElfEars ), "severed elf ears", 20, 0x312D ) );
							
			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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
	
	public class Gorrow : MondainQuester
	{
		public override Type[] Quests{ get{ return new Type[] 
		{ 
			typeof( CommonBrigandsQuest ), 
			typeof( ForkedTongueQuest ),
			typeof( PointyEarsQuest ),
		}; } }
		
		[Constructable]
		public Gorrow() : base( "Gorrow", "the mayor" )
		{			
			SetSkill( SkillName.Focus, 60.0, 83.0 );
		}
		
		public Gorrow( Serial serial ) : base( serial )
		{
		}
		
		public override void InitBody()
		{
			InitStats( 100, 100, 25 );
			
			Female = false;
			Race = Race.Human;
			
			Hue = 0x8412;
			HairItemID = 0x2047;
			HairHue = 0x465;
		}
		
		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Shoes( 0x1BB ) );
			AddItem( new LongPants( 0x901 ) );
			AddItem( new Tunic( 0x70A ) );
			AddItem( new Cloak( 0x675 ) );
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