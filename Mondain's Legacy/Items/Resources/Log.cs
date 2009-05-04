using System;
using Server;

namespace Server.Items
{
	public class BaseLog : Item, ICommodity
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; InvalidateProperties(); }
		}

		public override double DefaultWeight
		{
			get { return 2.0; }
		}
		
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} {1} log" : "{0} {1} logs", Amount, CraftResources.GetName( m_Resource ).ToLower() );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

			writer.Write( (int) m_Resource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
				{
					m_Resource = (CraftResource) reader.ReadInt();
					break;
				}
			}
		}

		public BaseLog( CraftResource resource ) : this( resource, 1 )
		{
		}

		public BaseLog( CraftResource resource, int amount ) : base( 0x1BDD )
		{
			Stackable = true;
			Amount = amount;
			Hue = CraftResources.GetHue( resource );

			m_Resource = resource;
		}

		public BaseLog( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( Amount > 1 )
				list.Add( 1050039, "{0}\t#{1}", Amount, 1027134 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add( 1027133 ); // log
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( !CraftResources.IsStandard( m_Resource ) )
			{
				int num = CraftResources.GetLocalizationNumber( m_Resource );

				if ( num > 0 )
					list.Add( num );
				else
					list.Add( CraftResources.GetName( m_Resource ) );
			}
		}

		public override int LabelNumber
		{
			get
			{
				if ( m_Resource >= CraftResource.OakWood && m_Resource <= CraftResource.Frostwood )
					return 1075052 + (int)(m_Resource - CraftResource.OakWood);

				return 1042692;
			}
		}

        #region ICommodity Members

        public int DescriptionNumber
        {
            get { return 0; }
        }

        #endregion
    }
	
	[FlipableAttribute( 0x1BDD, 0x1BE0 )]
	public class Log : BaseLog
	{
		[Constructable]
		public Log() : this( 1 )
		{
		}

		[Constructable]
		public Log( int amount ) : base( CraftResource.RegularWood, amount )
		{
		}

		public Log( Serial serial ) : base( serial )
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
	
	[FlipableAttribute( 0x1BDD, 0x1BE0 )]
	public class OakLog : BaseLog
	{
		[Constructable]
		public OakLog() : this( 1 )
		{
		}

		[Constructable]
        public OakLog(int amount)
            : base(CraftResource.OakWood, amount)
		{
		}

		public OakLog( Serial serial ) : base( serial )
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
	
	[FlipableAttribute( 0x1BDD, 0x1BE0 )]
	public class AshLog : BaseLog
	{
		[Constructable]
		public AshLog() : this( 1 )
		{
		}

		[Constructable]
		public AshLog( int amount ) : base( CraftResource.AshWood, amount )
		{
		}

		public AshLog( Serial serial ) : base( serial )
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
	
	[FlipableAttribute( 0x1BDD, 0x1BE0 )]
	public class YewLog : BaseLog
	{
		[Constructable]
		public YewLog() : this( 1 )
		{
		}

		[Constructable]
		public YewLog( int amount ) : base( CraftResource.YewWood, amount )
		{
		}

		public YewLog( Serial serial ) : base( serial )
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
	
	[FlipableAttribute( 0x1BDD, 0x1BE0 )]
	public class HeartwoodLog : BaseLog
	{
		[Constructable]
		public HeartwoodLog() : this( 1 )
		{
		}

		[Constructable]
		public HeartwoodLog( int amount ) : base( CraftResource.Heartwood, amount )
		{
		}

		public HeartwoodLog( Serial serial ) : base( serial )
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
	
	[FlipableAttribute( 0x1BDD, 0x1BE0 )]
	public class BloodwoodLog : BaseLog
	{
		[Constructable]
		public BloodwoodLog() : this( 1 )
		{
		}

		[Constructable]
		public BloodwoodLog( int amount ) : base( CraftResource.Bloodwood, amount )
		{
		}

		public BloodwoodLog( Serial serial ) : base( serial )
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
	
	[FlipableAttribute( 0x1BDD, 0x1BE0 )]
	public class FrostwoodLog : BaseLog
	{
		[Constructable]
		public FrostwoodLog() : this( 1 )
		{
		}

		[Constructable]
		public FrostwoodLog( int amount ) : base( CraftResource.Frostwood, amount )
		{
		}

		public FrostwoodLog( Serial serial ) : base( serial )
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