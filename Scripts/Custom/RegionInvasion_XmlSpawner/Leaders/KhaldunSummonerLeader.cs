using System;
using Server.Misc;
using Server.Network;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class KhaldunSummonerLeader : RegionInvasionLeader
	{
		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public KhaldunSummonerLeader():base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 0x190;
			Name = "Zealot of Khaldun";
			Title = "the Summoner";

			SetStr( 351, 400 );
			SetDex( 101, 150 );
			SetInt( 502, 700 );

			SetHits( 421, 480 );

			SetDamage( 5, 15 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Cold, 25 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.Wrestling, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Meditation, 120.1, 130.0 );

			VirtualArmor = 36;
			Fame = 10000;
			Karma = -10000;

			LeatherGloves gloves = new LeatherGloves();
			gloves.Hue = 0x66D;
			AddItem( gloves );

			BoneHelm helm = new BoneHelm();
			helm.Hue = 0x835;
			AddItem( helm );

			Necklace necklace = new Necklace();
			necklace.Hue = 0x66D;
			AddItem( necklace );

			Cloak cloak = new Cloak();
			cloak.Hue = 0x66D;
			AddItem( cloak );

			Kilt kilt = new Kilt();
			kilt.Hue = 0x66D;
			AddItem( kilt );

			Sandals sandals = new Sandals();
			sandals.Hue = 0x66D;
			AddItem( sandals );
		}

		public override int GetIdleSound()
		{
			return 0x184;
		}

		public override int GetAngerSound()
		{
			return 0x286;
		}

		public override int GetDeathSound()
		{
			return 0x288;
		}

		public override int GetHurtSound()
		{
			return 0x19F;
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }

		public KhaldunSummonerLeader( Serial serial ) : base( serial )
		{
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
			AddLoot( LootPack.FilthyRich );
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