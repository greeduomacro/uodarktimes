using System; 
using Server; 
using Server.Items;
using Server.Misc;
using Server.Targeting;
using Server.Mobiles; 
using Server.Network;
using System.Collections.Generic;
using System.Collections;
using Server.Regions;

namespace Server.Mobiles 
{ 
    public class BaseGoodGuard : BaseCreature 
    {
        public override bool BardImmune{ get{ return true; } } 
        public virtual bool AdvancedGuardsCommand{ get{ return false; } }

        public BaseGoodGuard(AIType aiType)
            : base(aiType,FightMode.Evil, 10, 1, 0.175, 0.350)
        {
            Title = "[�estn� Str�]";

            if (Female = Utility.RandomBool())
            {
                Name = NameList.RandomName("female");
                Body = 0x191;
            }
            else
            {
                Name = NameList.RandomName("male");
                Body = 0x190;
            }
            int hairHue = Utility.RandomHairHue();

            Utility.AssignRandomHair(this, hairHue);
            Utility.AssignRandomFacialHair(this, hairHue);

            Hue = Utility.RandomSkinHue();
            SpeechHue = Utility.RandomDyedHue();
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (base.Combatant == null)
            {
                base.Warmode = false;
                if (InRange(m, base.RangePerception) && InRange(oldLocation, base.RangePerception) && InLOS(m))
                {
                    if (base.CanSee(m))
                    {
                        if (m is PlayerMobile && m.AccessLevel == AccessLevel.Player)
                        {
                            if ( (m.Kills >= 5) )
                            {
                                base.Combatant = m;
                                base.Warmode = true;
                                if (0.2 >= Utility.RandomDouble())
                                base.Say("P�iprav se na smrt, vrahu!");
                            }
                        }
 			else
                        {
                            if (m is BaseEvilGuard)
                            {
                                base.Combatant = m;
                                base.Warmode = true;
				base.Say("Jsem p�ipraven st�t proti temnot�!");
                            }
                        }
                    }
                }
            }
        }

        public BaseGoodGuard(Serial serial)
            : base(serial) 
        { 
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (e.Mobile.InRange(this, 6))
            {
                if (Insensitive.Contains(e.Speech, "guards"))
                {
                    if (base.Combatant != null)
                    {
                        if (!(base.InLOS(base.Combatant)))
                        {
                            base.Say("Je m�j!");
                            Point3D from = (Point3D)this.Location;
                            Point3D to = new Point3D((Point3D)base.Combatant.Location);
                            base.Location = to;
                            Effects.SendLocationParticles(EffectItem.Create(from, base.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                            Effects.SendLocationParticles(EffectItem.Create(to, base.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
                            base.PlaySound(0x1FE);
                        }
                    }
                    else if (AdvancedGuardsCommand)
                    {
                        foreach (Mobile m in base.GetMobilesInRange(base.RangePerception))
                        {
                            if (m is PlayerMobile && !(m.Hidden) && m.Alive && m.AccessLevel == AccessLevel.Player)
                            {
                                 if ((m.Kills > 5) || (m.Criminal))

                                {
                                    base.Combatant = m;
                                    base.Warmode = true;
				  
                                    if (!(base.InLOS(m)))
                                    {
                                        base.Say("Je m�j!");
                                        Point3D fromt = (Point3D)this.Location;
                                        Point3D tot = new Point3D((Point3D)base.Combatant.Location);
                                        base.Location = tot;
                                        Effects.SendLocationParticles(EffectItem.Create(fromt, base.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                                        Effects.SendLocationParticles(EffectItem.Create(tot, base.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
                                        base.PlaySound(0x1FE);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                   
                }
                else
                {
                    base.OnSpeech(e);
                }
            }

        } 

        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.newguards; }
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
