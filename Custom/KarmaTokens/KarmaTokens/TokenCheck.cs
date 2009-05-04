using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class TokenCheck : Item
	{
		private int m_Worth;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Worth
		{
			get{ return m_Worth; }
			set{ m_Worth = value; InvalidateProperties(); }
		}

		public TokenCheck( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Worth );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			LootType = LootType.Blessed;

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Worth = reader.ReadInt();
					break;
				}
			}
		}

		[Constructable]
		public TokenCheck( int worth ) : base( 0x14F0 )
		{
			Weight = 1.0;
			Hue = 1266;
			Name = "Token Check";
			LootType = LootType.Blessed;

			m_Worth = worth;
		}

		public override bool DisplayLootType{ get{ return false; } }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties( list );

			list.Add( 1060738, m_Worth.ToString() ); // value: ~1_val~
		}

		public override void OnSingleClick( Mobile from )
		{
			from.Send( new MessageLocalizedAffix( Serial, ItemID, MessageType.Label, 0x3B2, 3, 1041361 , "", AffixType.Append, String.Concat( " ", m_Worth.ToString() ), "" ) ); // A bank check:
		}
	}
}