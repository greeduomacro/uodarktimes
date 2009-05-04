//Created By Animal Crackers
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class StaffOfPyros : BlackStaff
  {
public override int ArtifactRarity{ get{ return 11; } }


      [Constructable]
		public StaffOfPyros()
		{
          Name = "StaffOfPyros";
          Hue = 1259;
      WeaponAttributes.MageWeapon = 1;
      Attributes.CastSpeed = 1;
      Attributes.SpellChanneling = 1;
      Attributes.WeaponDamage = 30;
		}

		public StaffOfPyros( Serial serial ) : base( serial )
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
