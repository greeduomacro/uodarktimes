using System;
using Server;
using Server.Items;

namespace Server.Multis
{
	public class PirateShip : BaseBoat
	{
		public override int NorthID{ get{ return 0x4000; } }
		public override int  EastID{ get{ return 0x4001; } }
		public override int SouthID{ get{ return 0x4002; } }
		public override int  WestID{ get{ return 0x4003; } }

		public override int HoldDistance{ get{ return 4; } }
		public override int TillerManDistance{ get{ return -4; } }

		public override Point2D StarboardOffset{ get{ return new Point2D(  2, 0 ); } }
		public override Point2D      PortOffset{ get{ return new Point2D( -2, 0 ); } }

		public override Point3D MarkOffset{ get{ return new Point3D( 0, 1, 3 ); } }

		public override BaseDockedBoat DockedBoat{ get{ return new PirateShipDocked( this ); } }

		[Constructable]
		public PirateShip()
		{
		}

		public PirateShip( Serial serial ) : base( serial )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}
	}

	public class PirateShipDeed : BaseBoatDeed
	{
		public override int LabelNumber{ get{ return 1041205; } } // small ship deed
		public override BaseBoat Boat{ get{ return new PirateShip(); } }

		[Constructable]
		public PirateShipDeed() : base( 0x4000, Point3D.Zero )
		{
		Name = "Small Ship deed";
		}

		public PirateShipDeed( Serial serial ) : base( serial )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}
	}

	public class PirateShipDocked : BaseDockedBoat
	{
		public override BaseBoat Boat{ get{ return new PirateShip(); } }

		public PirateShipDocked( BaseBoat boat ) : base( 0x4000, Point3D.Zero, boat )
		{
		}

		public PirateShipDocked( Serial serial ) : base( serial )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}
	}
}
