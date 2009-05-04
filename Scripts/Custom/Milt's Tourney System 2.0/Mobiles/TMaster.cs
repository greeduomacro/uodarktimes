/***********************************************
*
* This script was made by milt, AKA Pokey.
*
* Email: pylon2007@gmail.com
*
* AIM: TrueBornStunna
*
* Website: www.pokey.f13nd.net
*
* Version: 2.0.0
*
* Release Date: June 29, 2006
*
************************************************/
using System;

using Server;
using Server.Items;

namespace Server.TSystem
{
	public class TMaster : Mobile
	{
		private TSystemStone m_Link;

		[CommandProperty( AccessLevel.GameMaster )]
		public TSystemStone Link{ get{ return m_Link; } set{ m_Link = value; } }

		public override void OnDoubleClick( Mobile from )
		{
			if(from.InRange( this.Location, 3 ))
			{
				if(m_Link == null)
				{
					this.Say("I am not linked.");
					return;
				}

				if(m_Link.Enabled && !m_Link.Started)
				{
					from.CloseGump( typeof(TJoinGump) );
					from.SendGump( new TJoinGump(m_Link, from) );
					return;
				}

				this.Say("Signups are closed.");

				return;
			}

			from.SendMessage("You are too far away to do that.");
		}

		[Constructable]
		public TMaster()
		{
			InitStats( 100, 100, 25 );

			Title = "the tournament master";
			Hue = Utility.RandomSkinHue();
			Direction = Direction.East;

			if ( this.Female = Utility.RandomBool() )
			{
				this.Body = 0x191;
				this.Name = NameList.RandomName( "female" );
			}
			else
			{
				this.Body = 0x190;
				this.Name = NameList.RandomName( "male" );
			}

			AddItem( new FancyShirt( Utility.RandomBlueHue() ) );

			AddItem( new ShortPants( Utility.RandomRedHue() ) );

			AddItem( new HalfApron( Utility.RandomBlueHue() ) );

			AddItem( new Cap( Utility.RandomRedHue() ) );

			AddItem( new Sandals( Utility.RandomBlueHue() ) );

			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );

			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;

			AddItem( hair );
		}

		public override bool CanBeDamaged()
		{
			return false;
		}

		public TMaster( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Link );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Core.AOS && NameHue == 0x35 )
				NameHue = -1;

			m_Link = (TSystemStone)reader.ReadItem();
		}
	}
}