using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute( 0x2B06, 0x2B07 )]
	public class HonorLegs : BaseArmor
	{
		public override int LabelNumber{ get{ return 1075193; } } // Legs of Honor (Virtue Armor Set)
		
		public override SetItem SetID{ get{ return SetItem.Virtue; } }
		public override int Pieces{ get{ return 8; } }
	
		public override int BasePhysicalResistance{ get{ return 8; } }
		public override int BaseFireResistance{ get{ return 7; } }
		public override int BaseColdResistance{ get{ return 10; } }
		public override int BasePoisonResistance{ get{ return 7; } }
		public override int BaseEnergyResistance{ get{ return 8; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		
		public override int AosStrReq{ get{ return 70; } }
		
		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		[Constructable]
		public HonorLegs() : base( 0x2B06 )
		{
			LootType = LootType.Blessed;
			Weight = 9.0;
			SetHue = 0;
			Hue = 0x226;
			
			SetArmorAttributes.SelfRepair = 5;
			
			SetPhysicalBonus = 5;
			SetFireBonus = 5;
			SetColdBonus = 5;
			SetPoisonBonus = 5;
			SetEnergyBonus = 5;
		}

		public HonorLegs( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
	}
}