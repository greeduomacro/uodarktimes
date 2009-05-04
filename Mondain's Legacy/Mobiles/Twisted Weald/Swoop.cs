using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a swoop corpse" )]
	public class Swoop : BaseCreature
	{		
		[Constructable]
		public Swoop() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.05, 0.2 )
		{
			Name = "a Swoop";
			Body = 0x5;
			Hue = 0xE0;

			SetStr( 239, 245 );
			SetDex( 528, 577 );
			SetInt( 184, 195 );

			SetHits( 686, 752 );
			SetStam( 478, 490 );
			SetMana( 14, 14 );

			SetDamage( 17, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 70, 72 );
			SetResistance( ResistanceType.Fire, 35, 36 );
			SetResistance( ResistanceType.Cold, 40, 44 );
			SetResistance( ResistanceType.Poison, 25, 29 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.Wrestling, 94.4, 104.3 );
			SetSkill( SkillName.Tactics, 98.0, 113.7 );
			SetSkill( SkillName.MagicResist, 95.4, 102.9 );
			SetSkill( SkillName.Focus, 20.0, 120.0 );

			Fame = 12000;
			Karma = -500;
			
			PackReg( 4 );
		}
		
		public Swoop( Serial serial ) : base( serial )
		{
		}
		
		public override int GetIdleSound() { return 0x2EF; }
		public override int GetAttackSound() { return 0x2EE; }
		public override int GetAngerSound() { return 0x2EF; }
		public override int GetHurtSound() { return 0x2F1; }
		public override int GetDeathSound()	{ return 0x2F2; }
		
		public override bool GivesMinorArtifact{ get{ return true; } }
		public override int Feathers{ get{ return 72; } }
		public override int Meat{ get{ return 1; } }
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.AosUltraRich, 3 );
		}		
		
		public override void OnDeath( Container c )
		{
			base.OnDeath( c );		
			
			if ( Utility.RandomDouble() < 0.025 )
			{
				switch ( Utility.Random( 19 ) )
				{
					case 0: c.DropItem( new AssassinChest() ); break;
					case 1: c.DropItem( new AssassinArms() ); break;
					case 2: c.DropItem( new DeathChest() );	break;		
					case 4: c.DropItem( new MyrmidonArms() ); break;
					case 5: c.DropItem( new MyrmidonLegs() ); break;
					case 6: c.DropItem( new MyrmidonGorget() ); break;
					case 7: c.DropItem( new LeafweaveGloves() ); break;
					case 8: c.DropItem( new LeafweaveLegs() ); break;
					case 9: c.DropItem( new LeafweavePauldrons() ); break;
					case 10: c.DropItem( new PaladinGloves() ); break;
					case 11: c.DropItem( new PaladinGorget() ); break;
					case 12: c.DropItem( new PaladinArms() ); break;
					case 13: c.DropItem( new HunterArms() ); break;
					case 14: c.DropItem( new HunterGloves() ); break;
					case 15: c.DropItem( new HunterLegs() ); break;
					case 16: c.DropItem( new HunterChest() ); break;
					case 17: c.DropItem( new GreymistArms() ); break;
					case 18: c.DropItem( new GreymistGloves() ); break;
				}
			}
						
			if ( Utility.RandomDouble() < 0.1 )
				c.DropItem( new ParrotItem() );
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
