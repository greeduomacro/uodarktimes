using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("evil guard corpse")]
    public class NewArcherEvilGuard : BaseEvilGuard
    {
    private int currentweapon;

        [Constructable]
        public NewArcherEvilGuard()
            : base(AIType.AI_Archer)
        {
            SetStr(60, 120);
            SetDex(100, 150);
            SetInt(26, 50);

            SetHits(80, 110);

            SetMana(40);

            if (Utility.RandomBool())
            {
                new Horse().Rider = this;
            }

            SetResistance(ResistanceType.Physical, 20, 20);
            SetResistance(ResistanceType.Fire, 20, 25);
            SetResistance(ResistanceType.Cold, 20, 20);
            SetResistance(ResistanceType.Poison, 20, 30);

            SetSkill(SkillName.MagicResist, 50.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 120.0);
            SetSkill(SkillName.Anatomy, 80.0, 120.0);
            SetSkill(SkillName.Healing, 50.0, 65.0);
            SetSkill(SkillName.Archery, 80.0, 120.0);
            SetSkill(SkillName.Fencing, 80.0, 120.0);

            Fame = 1000;
            Karma = -10000;

            VirtualArmor = 16;

            AddItem(new Boots(0x484));
            AddItem(new Cloak(0x484));
            PackItem(new Dagger());
            StuddedChest chest = new StuddedChest();
            chest.Hue = 0x484;
            AddItem(chest);
            StuddedLegs legs = new StuddedLegs();
            legs.Hue = 0x484;
            AddItem(legs);
            LeatherCap head = new LeatherCap();
            head.Hue = 0x484;
            AddItem(head);
            StuddedGloves gloves = new StuddedGloves();
            gloves.Hue = 0x484;
            AddItem(gloves);
            StuddedGorget gorget = new StuddedGorget();
            gorget.Hue = 0x484;
            AddItem(gorget);
            StuddedArms arms = new StuddedArms();
            arms.Hue = 0x484;
            AddItem(arms);
            AddItem(new Bandage(30));

            switch (Utility.Random(4))
            {  
                case 0: AddItem(new Bow());
                        AddItem(new Arrow(50));
                        SetDamage(5, 15);
                        currentweapon=1;
                        break;
                case 1: AddItem(new CompositeBow());
                        AddItem(new Arrow(50));
                        SetDamage(5, 20);
                        currentweapon=2;
                        break;
                case 2: AddItem(new Crossbow());
                        AddItem(new Bolt(50));
                        SetDamage(15, 25);
                        currentweapon=3;
                        break;
                case 3: AddItem(new HeavyCrossbow());
                        AddItem(new Bolt(50));
                        SetDamage(18, 30);
                        currentweapon=4;
                        break;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

	public override bool OnBeforeDeath()
	{
	    IMount mount = this.Mount;

            if ( mount != null )
	    mount.Rider = null;

	    if ( mount is Mobile )
	    ((Mobile)mount).Kill();

	    return base.OnBeforeDeath();
	}

        public override Poison PoisonImmune { get { return Poison.Lesser; } }
        public override bool AlwaysMurderer { get { return true; } }

        public NewArcherEvilGuard(Serial serial)
            : base(serial)
        {
        }

        public override void OnGaveMeleeAttack(Mobile d)
        {
            if (0.2 >= Utility.RandomDouble())
            {
                this.Say("M� neute�e�!");
            }

            if (currentweapon == 4)
            {
                if (this.FindItemOnLayer(Layer.TwoHanded) != null)
                {
                    if (0.2 >= Utility.RandomDouble())
                    {
                        IMount mount = d.Mount;

                        if (mount != null)
                        {
                            mount.Rider = null;

                            if (d is PlayerMobile)
                            {
                                d.SendLocalizedMessage(1040023); // You have been knocked off of your mount!
                            }
                            else
                            {
                                ((Mobile)mount).Kill();
                            }
                        }
                    }
                }
            }

            if (d is BaseCreature)
            {
                if ((d as BaseCreature).IsDispellable)
                {
                    d.Hits -= Utility.RandomMinMax(20, 50);
                }
            }

        }

        public override void OnGotMeleeAttack(Mobile d)
        {
                Item item = this.FindItemOnLayer( Layer.TwoHanded );
                if (item != null)
                {
                    Item dagger = this.Backpack.FindItemByType(typeof(Dagger));
                    this.Backpack.DropItem(item);
                    EquipItem(dagger);
                    SetDamage(5, 12);
                    InvalidateProperties();
                }

                if (0.2 >= Utility.RandomDouble())
                {
                    this.Say("You will have to do something better than that.");
                }
                if (this.Hits < this.HitsMax / 2)
                {
                    if (BandageContext.GetContext(this) == null)
                    {
                        BandageContext.BeginHeal(this, this);
                        this.Say("*Pou��v� band�*");
                    }
                }

                if (d is BaseCreature)
                {
                    BaseCreature t = (BaseCreature)d;
                    if (t.Controlled && !(t as BaseCreature).IsDispellable)
                    {
                        if (0.5 >= Utility.RandomDouble())
                        {
                            t.Combatant = null;
                            t.Warmode = false;
                            t.Pacify(this, DateTime.Now + TimeSpan.FromSeconds(20.0));
                        }
                        if (this.Combatant == t)
                        {
                            this.Provoke(this, t.ControlMaster, true);
                        }
                    }
                }
        }

        public override void OnActionCombat()
        {
            Item item = this.FindItemOnLayer(Layer.OneHanded);
            if (item != null)
            {
                if (!InRange(this.Combatant, 1))
                {
                    this.Backpack.DropItem(item);

                    switch(currentweapon)
                    {
                    case 1: Item rangedbow = this.Backpack.FindItemByType(typeof(Bow));
                            EquipItem(rangedbow);
                            SetDamage(5, 15);
                            break;

                    case 2: Item rangedcbow = this.Backpack.FindItemByType(typeof(CompositeBow));
                            EquipItem(rangedcbow);
                            SetDamage(5, 20);
                            break;
                    
                    case 3: Item rangedcross = this.Backpack.FindItemByType(typeof(Crossbow));
                            EquipItem(rangedcross);
                            SetDamage(15, 25);
                            break;

                    case 4: Item rangedhcross = this.Backpack.FindItemByType(typeof(HeavyCrossbow));
                            EquipItem(rangedhcross);
                            SetDamage(18, 30);
                            break;
                    }
                    InvalidateProperties();
                    this.Warmode = true;
                }
            }
        }
        public override void OnDamagedBySpell(Mobile d)
        {
            if (0.3 >= Utility.RandomDouble())
            {
                this.Say("Your weak spells will not help you.");
            }

            if (this.Hits < this.HitsMax / 2)
            {
                if (BandageContext.GetContext(this) == null)
                {
                    BandageContext.BeginHeal(this, this);
                    this.Say("*Pou��v� band�*");
                }
            }

            if (d is BaseCreature)
            {
                BaseCreature t = (BaseCreature)d;
                if (t.Controlled && !(t as BaseCreature).IsDispellable)
                {
                    if (0.5 >= Utility.RandomDouble())
                    {
                        t.Combatant = null;
                        t.Warmode = false;
                        t.Pacify(this, DateTime.Now + TimeSpan.FromSeconds(20.0));
                    }
                    if (this.Combatant == t)
                    {
                        this.Provoke(this, t.ControlMaster, true);
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write( currentweapon );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            currentweapon = reader.ReadInt();
        }
    }
}