using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	[FlipableAttribute( 0x2683, 0x2684 )]
	public class ArcheryPvPRobe : BaseArmor
	{
		private SkillMod m_SkillMod0;
		private SkillMod m_SkillMod1;
		private SkillMod m_SkillMod2;
		private StatMod m_StatMod0;
		private StatMod m_StatMod1;

		public override int BasePhysicalResistance{ get{ return (Utility.RandomMinMax(8,12)); } }
		public override int BaseFireResistance{ get{ return (Utility.RandomMinMax(1,3)); } }
		public override int BaseColdResistance{ get{ return (Utility.RandomMinMax(1,3)); } }
		public override int BasePoisonResistance{ get{ return (Utility.RandomMinMax(1,3)); } }
		public override int BaseEnergyResistance{ get{ return (Utility.RandomMinMax(1,3)); } }

		public override int AosStrReq{ get{ return 25; } }
		public override int AosIntReq{ get{ return 75; } }
		public override int AosDexReq{ get{ return 75; } }

		public override int ArmorBase{ get{ return 40; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Leather; } }

		public override int ArtifactRarity{ get{ return 15; } }

		private string m_PlayerTitle;

		[CommandProperty( AccessLevel.GameMaster )]
		public string PlayerTitle
		{
			get { return m_PlayerTitle; }
			set { m_PlayerTitle = value; }
		}

		[Constructable]
		public ArcheryPvPRobe() : base( 0x2683 )
		{
			Weight = 5.0;
			Name = NameList.RandomName( "Robe Names" );
			Layer = Layer.OuterTorso;
			DefineMods();
		}

		private void DefineMods()
		{
			m_SkillMod0 = new DefaultSkillMod( SkillName.Archery, true, 20 );
			m_SkillMod1 = new DefaultSkillMod( SkillName.Tactics, true, 20 );
			m_SkillMod2 = new DefaultSkillMod( SkillName.Anatomy, true, 20 );
			m_StatMod0 = new StatMod( StatType.Dex, "ArcheryPvPDex", 20, TimeSpan.Zero );
			m_StatMod1 = new StatMod( StatType.Str, "ArcheryPvPStr", 20, TimeSpan.Zero );
		}

		private void SetMods( Mobile wearer )
		{
			wearer.AddSkillMod( m_SkillMod0 );
			wearer.AddSkillMod( m_SkillMod1 );
			wearer.AddSkillMod( m_SkillMod2 );
			wearer.AddStatMod( m_StatMod0 );
			wearer.AddStatMod( m_StatMod1 );
		}

		public override void OnDoubleClick( Mobile m )
		{
			if( Parent != m ) { m.SendMessage( "You must be wearing the robe to use it!" ); }
			else
			{
				PlayerMobile pm = m as PlayerMobile;
				if ( ItemID == 0x2683 || ItemID == 0x2684 )
				{
					m.SendMessage( "You lower the hood." );
					m.PlaySound( 0x57 );
					ItemID = 0x1F03;
					m.BodyMod = 0;
					m.HueMod = -1;
					m.NameMod = null;
					if ( pm != null )
					{
						pm.Title = m_PlayerTitle;
						m_PlayerTitle = "";
					}
					if( m is Mobile && m.Kills >= 5) { ((Mobile)m).Criminal = true; }
					if( m is Mobile && m.GuildTitle != null) { ((Mobile)m).DisplayGuildTitle = true; }
				}
				else if ( ItemID == 0x1F03 || ItemID == 0x1F04 )
				{
					m.SendMessage( "You pull the hood over your head." );
					m.PlaySound( 0x57 );
					ItemID = 0x2683;
					m.BodyMod = Utility.RandomList( 400, 401 );
					m.HueMod = Utility.RandomSkinHue();
					m.NameMod = m.Body.IsFemale ? NameList.RandomName( "female" ) : NameList.RandomName( "male" );
					m.DisplayGuildTitle = false;
					if ( pm != null )
					{
						m_PlayerTitle = pm.Title;
						pm.Title = NameList.RandomName( "Race Name" ) + " " + NameList.RandomName( "Class Name" );
					}
				}
			}
		}

		public override bool OnEquip( Mobile from )
		{
			SetMods( from );
			if ( ItemID == 0x2683 || ItemID == 0x2684 ) { ItemID = 0x1F03; }
			return base.OnEquip(from);
		}

		public override void OnRemoved( Object o )
		{
			if( o is Mobile )
			{
				Mobile m = o as Mobile;
				if ( ItemID == 0x2683 || ItemID == 0x2684 )
				{
					PlayerMobile pm = m as PlayerMobile;
					m.SendMessage( "You lower the hood and remove the robe" );
					m.PlaySound( 0x57 );
					ItemID = 0x1F03;
					m.BodyMod = 0;
					m.HueMod = -1;
					m.NameMod = null;
					if ( pm != null )
					{
						pm.Title = m_PlayerTitle;
						m_PlayerTitle = "";
					}
					if( m is Mobile && m.Kills >= 5) { ((Mobile)m).Criminal = true; }
					if( m is Mobile && m.GuildTitle != null) { ((Mobile)m).DisplayGuildTitle = true; }
				}
				m.RemoveStatMod( "ArcheryPvPDex" );
				m.RemoveStatMod( "ArcheryPvPStr" );
				if ( m.Hits > m.HitsMax ) m.Hits = m.HitsMax;
				if ( m_SkillMod0 != null ) m_SkillMod0.Remove();
				if ( m_SkillMod1 != null ) m_SkillMod1.Remove();
				if ( m_SkillMod2 != null ) m_SkillMod2.Remove();
			}
			base.OnRemoved( o );
		}

		public ArcheryPvPRobe( Serial serial ) : base( serial )
		{
			DefineMods();
			if ( Parent != null && this.Parent is Mobile ) SetMods( (Mobile)Parent );
		}

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); writer.Write( (string) m_PlayerTitle ); }

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); m_PlayerTitle = reader.ReadString(); }
	}
}