using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class RunicSewingKit1 : BaseRunicTool
	{
		public override CraftSystem CraftSystem{ get{ return DefTailoring.CraftSystem; } }

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			int index = CraftResources.GetIndex( Resource );

			if ( index >= 1 && index <= 8 )
				return;

			if ( !CraftResources.IsStandard( Resource ) )
			{
				int num = CraftResources.GetLocalizationNumber( Resource );

				if ( num > 0 )
					list.Add( num );
				else
					list.Add( CraftResources.GetName( Resource ) );
			}
		}


		[Constructable]
		public RunicSewingKit1( CraftResource resource ) : base( resource, 0xF9D )
		{
			Weight = 2.0;
			Hue = CraftResources.GetHue( resource );
		}

		[Constructable]
		public RunicSewingKit1( CraftResource resource, int uses ) : base( resource, uses, 0xF9D )
		{
			Weight = 2.0;
			Hue = CraftResources.GetHue( resource );
		}

		public RunicSewingKit1( Serial serial ) : base( serial )
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

			if ( ItemID == 0x13E4 || ItemID == 0x13E3 )
				ItemID = 0xF9D;
		}
	}
}