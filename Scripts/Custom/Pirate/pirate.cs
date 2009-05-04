using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class Pirate : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public Pirate() : base( AIType.AI_Archer, FightMode.Closest, 15, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
			   	Title = "Pirate";
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new ThighBoots());
			}
			else
			{
				Title = "Pirate";		
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ThighBoots());
			}

			SetStr( 195, 200 );
			SetDex( 181, 195 );
			SetInt( 61, 75 );
			SetHits( 288, 308 );

			SetDamage( 20, 40 );

			SetSkill( SkillName.Fencing, 86.0, 97.5 );
			SetSkill( SkillName.Macing, 85.0, 87.5 );
			SetSkill( SkillName.MagicResist, 55.0, 67.5 );
			SetSkill( SkillName.Swords, 85.0, 87.5 );
			SetSkill( SkillName.Tactics, 85.0, 87.5 );
			SetSkill( SkillName.Wrestling, 35.0, 37.5 );
			SetSkill( SkillName.Archery, 85.0, 87.5 );

			Fame = 2000;
			Karma = -2000;
			VirtualArmor = 66;
			
			switch ( Utility.Random( 1 ))
			{
				case 0: AddItem( new LongPants ( Utility.RandomRedHue() ) ); break;
				case 1: AddItem( new ShortPants( Utility.RandomRedHue() ) ); break;
			}				
			
			switch ( Utility.Random( 3 ))
			{
				case 0: AddItem( new FancyShirt( Utility.RandomRedHue() ) ); break;
				case 1: AddItem( new Shirt( Utility.RandomRedHue() ) ); break;
				case 2: AddItem( new Doublet( Utility.RandomRedHue() ) ); break;
			}					
			

			switch ( Utility.Random( 3 ))
			{
				case 0: AddItem( new Bandana( Utility.RandomRedHue() ) ); break;
				case 1: AddItem( new SkullCap( Utility.RandomRedHue() ) ); break;
			}			

			switch ( Utility.Random( 5 ))
			{
				case 0: AddItem( new Bow() ); break;
				case 1: AddItem( new CompositeBow() ); break;
				case 2: AddItem( new Crossbow() ); break;
				case 3: AddItem( new RepeatingCrossbow() ); break;
				case 4: AddItem( new HeavyCrossbow() ); break;
			}		

			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );
		}


		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }


		public Pirate( Serial serial ) : base( serial )
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
