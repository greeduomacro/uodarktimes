using System;
using Server;

namespace Server.Items
{
public class AdminWatch : BaseBracelet
	{
		[Constructable]
		public AdminWatch() : base( 0x1086 )
		{
			Weight = 0.0;
			Name = "Admin Watch";
          		Hue = 0x35; 
			LootType = LootType.Blessed;
			
			Attributes.Luck = 9999999;
			Attributes.LowerManaCost = 9999999;
			Attributes.LowerRegCost = 9999999;

			Attributes.BonusStr = 9999999;
			Attributes.BonusHits = 9999999;
			Attributes.BonusDex = 9999999;
			Attributes.BonusMana = 9999999;
			Attributes.BonusInt = 9999999;
			Attributes.BonusStam = 9999999;
			Attributes.CastSpeed = 9999999;
			Attributes.EnhancePotions = 9999999;

			Attributes.WeaponDamage = 9999999;
			Attributes.AttackChance = 9999999;
			Attributes.DefendChance = 9999999;

			Attributes.RegenStam = 9999999;
			Attributes.RegenHits = 9999999;
			Attributes.RegenMana = 9999999;
			Attributes.NightSight = 9999999;

			Attributes.WeaponSpeed = 9999999;
			Attributes.SpellDamage = 9999999;
			Attributes.SpellChanneling = 1;
			Attributes.CastRecovery = 9999999;
			Attributes.WeaponSpeed = 9999999;
			Attributes.ReflectPhysical = 9999999;
		
			Resistances.Energy = 9999999;
			Resistances.Cold = 9999999;
			Resistances.Fire = 9999999;
			Resistances.Physical = 9999999;
			Resistances.Poison = 9999999;
			
			SkillBonuses.SetValues( 0, SkillName.Parry, 65535.5 );
			SkillBonuses.SetValues( 1, SkillName.Begging, 65535.5 );
			SkillBonuses.SetValues( 2, SkillName.Blacksmith, 65535.5 );
			SkillBonuses.SetValues( 3, SkillName.Fletching, 65535.5 );
			SkillBonuses.SetValues( 4, SkillName.Peacemaking, 65535.5 );

		}

		public AdminWatch( Serial serial ) : base( serial )
		{
		}

		/// <summary>
	 	/// i with to make a gm clothing packet for my gm's ingame
	 	/// </summary>
	 	
	 	public override bool OnEquip( Mobile from )
	 	{     
	 		if ( from.AccessLevel <= AccessLevel.GameMaster )
		    {
	 			from.SendMessage( "You are not granted to wear this item" );
		    	this.Delete();
		    	return false;
	 		}
	 		else
	 		{
	 			from.SendMessage( "Access Granted mighty one" );
	 			return true;
	 		}
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
