using System;
using Server;

namespace Server.Items
{
    [FlipableAttribute(0x153b, 0x153c)]
    public class CrimsonCincture : BaseWaist
    {
        [Constructable]
        public CrimsonCincture(): base (0x153b)
        {
            Name = "Crimson Cincture";
            Hue = 37;
            Weight = 2.0;
            Attributes.BonusDex = 5;
            Attributes.RegenHits = 2;
            Attributes.BonusHits = 10;

        }



        public CrimsonCincture(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}