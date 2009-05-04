using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    [FlipableAttribute(0x2D25, 0x2D31)]
    public class Boomstick : BaseBashing
    {
        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Block; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ForceOfNature; } }

        public override int AosStrengthReq { get { return 15; } }
        public override int AosMinDamage { get { return 10; } }
        public override int AosMaxDamage { get { return 12; } }
        public override int AosSpeed { get { return 48; } }

        public override int OldStrengthReq { get { return 15; } }
        public override int OldMinDamage { get { return 10; } }
        public override int OldMaxDamage { get { return 12; } }
        public override int OldSpeed { get { return 48; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public Boomstick(): base(0x2D25)
        {
            Weight = 8.0;
            Attributes.RegenMana = 3;
	    Attributes.SpellChanneling = 1;
            Attributes.CastSpeed = 1;
            Attributes.LowerRegCost = 20;
            Hue = 37;
            Name = "Boomstick";
            Cursed = true;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Chaos Damage 100%");
        }

        private void OnTick()
        {
            if (Cursed == false)
            Cursed = true;
        }

        public Boomstick(Serial serial): base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}