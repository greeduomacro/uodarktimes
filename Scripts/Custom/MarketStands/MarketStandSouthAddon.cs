/////////////////////////////////////////////////
//                                             //
// Automatically generated by the              //
// AddonGenerator script by Arya               //
//                                             //
/////////////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MarketStandSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new MarketStandSouthAddonDeed();
			}
		}

		[ Constructable ]
		public MarketStandSouthAddon()
		{
            AddonComponent ac;
            ac = new AddonComponent(6787);
            AddComponent(ac, 0, 0, 0);
            ac.Name = "market stand";
            ac = new AddonComponent(2938);
            AddComponent(ac, 0, 0, 1);
            ac.Name = "market stand";
            ac = new AddonComponent(2938);
            AddComponent(ac, 0, 1, 1);
            ac.Name = "market stand";
            ac = new AddonComponent(6787);
            AddComponent(ac, 0, 2, 0);
            ac.Name = "market stand";
            ac = new AddonComponent(2938);
            AddComponent(ac, 0, 2, 1);
            ac.Name = "market stand";
		}

        public MarketStandSouthAddon(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class MarketStandSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new MarketStandSouthAddon();
			}
		}

		[Constructable]
		public MarketStandSouthAddonDeed()
		{
			Name = "market stand facing south deed";
		}

        public MarketStandSouthAddonDeed(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}