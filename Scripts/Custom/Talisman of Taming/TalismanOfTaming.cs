using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    

    public class TalismanOfTaming : BaseTalisman
	{


		//public override int LabelNumber{ get{ return 1061102; } } // Ring of the Vile
		//public override int ArtifactRarity{ get{ return 859; } }

        private Mobile m_Owner;
		private bool m_Blessed;

        public override bool DisplayLootType
        {
            get { return true; }
        }
	
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; InvalidateProperties(); }
		}	
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Blessed
		{
			get{ return m_Blessed; }
			set{ m_Blessed = value; InvalidateProperties(); }
		}
        
        [Constructable]
        public TalismanOfTaming(): base(0x2f5b)
		{

            Name = "Talisman Of Taming";
			Weight = 0.1;
			Hue = 1152;



			SkillBonuses.SetValues( 0, SkillName.AnimalTaming, 20.0 );
			SkillBonuses.SetValues( 1, SkillName.AnimalLore, 20.0 );
			
			Attributes.BonusDex = 30;
		 	Attributes.BonusStr = 40;
		 	Attributes.BonusInt = 50;
		 	
		 	Attributes.BonusHits = 30;
			Attributes.BonusMana = 50;
			Attributes.NightSight = 1;		 	

		 	Attributes.AttackChance = 30;
		 	Attributes.ReflectPhysical = 30;
			Attributes.DefendChance = 30;

			Attributes. CastSpeed = 5;
			Attributes. CastRecovery = 5;
			Attributes.WeaponDamage = 45;	
			
			Attributes.LowerManaCost = 20;
			Attributes.RegenMana = 10;
		 	Attributes.RegenHits = 15;	
			
			
            //LootType = LootType.Blessed;
		}
        [Constructable]
        public TalismanOfTaming(Serial serial)
            : base(serial)
		{
		}
        public override bool OnEquip(Mobile from)
        {

            if (m_Owner == null)
            {
                m_Owner = from;
                base.OnEquip(from);
                from.FollowersMax = 8;
                this.Name = m_Owner.Name.ToString() + "'s Talisman Of Taming";
                this.LootType = LootType.Blessed;
                from.SendMessage("These now belongs to you, and only you!");
            }
            else if (from == m_Owner || from.AccessLevel >= AccessLevel.GameMaster)
            {
                base.OnEquip(from);
                from.FollowersMax = 8;
                from.SendMessage("You now have 8 max followers");
                
                }
                else
                {
                    from.SendMessage("This does not belong to you.");
                    return false;
                }
                return true;
            }
        
		public override void OnRemoved( object parent )
		{ 

      if ( parent is Mobile ) 		                      
            {       
			((Mobile)parent).FollowersMax = 5;
            ((Mobile)parent).SendMessage("Your max followers has just been reduced to 5");	
			}
            

        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

            SaveFlag flags = SaveFlag.None;

            SetSaveFlag(ref flags, SaveFlag.Owner, m_Owner != null);
            SetSaveFlag(ref flags, SaveFlag.Blessed, m_Blessed);

            writer.WriteEncodedInt((int)flags);

            if (GetSaveFlag(flags, SaveFlag.Owner))
                writer.Write((Mobile)m_Owner);
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Owner))
                            m_Owner = reader.ReadMobile();
                        m_Blessed = GetSaveFlag(flags, SaveFlag.Blessed);

                        break;
                }
            }
        }
	}
}
