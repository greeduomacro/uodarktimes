using System;
using System.Collections;
using Server;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{	
	public class TokenBox : Item 
	{
		private int m_Token;
		private Mobile m_Owner;
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int Token { get { return m_Token; } set { m_Token = value; InvalidateProperties(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; }
		}
		
		[Constructable]
		public TokenBox() : base( 0xE80 )
		{
			Movable = true;
			Weight = 0;
			Name = "Token Box";
			LootType = LootType.Blessed;			
		}
		
		[Constructable]
		public TokenBox( Mobile m ) : base( 0xE80 )
		{
			Movable = true;
			Weight = 0;
			Name = "Token Box";
			LootType = LootType.Blessed;
			m_Owner = m;			
		}		

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_Owner == null )
			{
				Mobile mobile = (Mobile)from;
				PlayerMobile pm = (PlayerMobile)from;

				Owner = pm;
			}
			if ( from != m_Owner )
			{
				from.SendMessage( "This is not your box, return it to it's owner." ); 
			}
			else if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( from is PlayerMobile )
			{
				from.SendGump( new TokenBoxGump( (PlayerMobile)from, this ) );
			}
		}

		public void BeginCombine( Mobile from )
		{
			from.Target = new TokenBoxTarget( this );
		}

		public void EndCombine( Mobile from, object o )
		{
			if ( o is Item && ((Item)o).IsChildOf( from.Backpack ) )
			{
				if (!( o is Tokens || o is TokenCheck ))
				{
					from.SendMessage( "That is not an item you can put in here." );
				}
				if ( o is Tokens  )
				{

					if ( Token >= 200000000 )
					from.SendMessage( "This box is too full to add more." );
					else
					{
						Item curItem = o as Item;
						Token += curItem.Amount;
						curItem.Delete();
						from.SendGump( new TokenBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
				
				if ( o is TokenCheck )
				{
					if ( Token >= (200000000 - ((TokenCheck)o).Worth) )
					from.SendMessage( "The box is too full to add more." );
					else
					{
						Token = ( Token + ((TokenCheck)o).Worth );
						((Item)o).Delete();
						from.SendGump( new TokenBoxGump( (PlayerMobile)from, this ) );
						BeginCombine( from );
					}
				}
			}
			else
			{
				from.SendLocalizedMessage( 1045158 ); // You must have the item in your backpack to target it.
			}
		}

		public TokenBox( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) m_Token);
			writer.Write( ( Mobile ) m_Owner);
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			m_Token = reader.ReadInt();
			m_Owner = reader.ReadMobile();
			
		}
	}
}

namespace Server.Gumps
{
	
	public class TokenBoxGump : Gump
	{
	
		private PlayerMobile m_From;
		private TokenBox m_Box;	
      	   
		public TokenBoxGump( PlayerMobile from, Item item ): base( 0, 0 )
		{
		
			m_From = from;			
			if (!(item is TokenBox))
				return;
			TokenBox box = item as TokenBox;
			m_Box = box;

			m_From.CloseGump( typeof( TokenBoxGump ) );
			
			Closable=true;
			Disposable=false;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddImage(112, 135, 75);
			AddLabel(143, 280, 1160, @"Karmageddon's Tokens");
			AddButton(125, 265, 1209, 1210, 1, GumpButtonType.Reply, 0);
			AddButton(125, 245, 1209, 1210, 2, GumpButtonType.Reply, 0);
			AddButton(125, 225, 1209, 1210, 3, GumpButtonType.Reply, 0);
			AddLabel(145, 260, 37, @"Add Tokens");
			AddLabel(145, 240, 67, @"Extract Tokens");
			AddLabel(145, 220, 67, @"Write Check");
			AddBackground(132, 200, 165, 20, 9300);
			AddTextEntry(135, 200, 155, 25, 1265, 1, "0");
			AddLabel(125, 160, 1160, @"Current Tokens:");
			AddLabel(240, 160, 1160, box.Token.ToString());
			AddLabel(180, 140, 187, @"Token Box");
			AddButton(285, 145, 2708, 2709, 4, GumpButtonType.Reply, 0);
			AddLabel(140, 180, 67, @"Tokens to be Extracted");

		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Box.Deleted )
			return;
			
			if ( info.ButtonID == 1)
			{
				m_From.SendGump( new TokenBoxGump( m_From, m_Box ) );
				m_Box.BeginCombine( m_From );
			}
			
			if ( info.ButtonID == 2 )
			{
				TextRelay tr_TokenAmount = info.GetTextEntry( 1 );
				if(tr_TokenAmount != null)
				{
					int i_MaxAmount = 0;
					try
					{
						i_MaxAmount = Convert.ToInt32(tr_TokenAmount.Text,10);
					} 
					catch
					{
						m_From.SendMessage(1161, "Please make sure you write only numbers.");
					}
					if(i_MaxAmount > 0) 
					{
						if (i_MaxAmount <= ((TokenBox)m_Box ).Token)
						{				
							if (i_MaxAmount <= 60000)
							{
								m_From.AddToBackpack(new Tokens(i_MaxAmount));
								m_From.SendMessage(1161, "You extracted {0} tokens from your box.", i_MaxAmount);
								((TokenBox)m_Box ).Token = (((TokenBox)m_Box ).Token - i_MaxAmount);
							}
							else
								m_From.SendMessage(1161, "You can't extract more then 60,000 tokens at one time.");
						}
						else
							m_From.SendMessage(1173, "You don't have that many tokens in your box.");
					}
					m_From.SendGump( new TokenBoxGump( m_From, m_Box ) );
				}
			}
			
			if ( info.ButtonID == 3 )
			{
				TextRelay tr_TokenAmount = info.GetTextEntry( 1 );
				if(tr_TokenAmount != null)
				{
					int i_MaxAmount = 0;
					try
					{
						i_MaxAmount = Convert.ToInt32(tr_TokenAmount.Text,10);
					} 
					catch
					{
						m_From.SendMessage(1161, "Please make sure you write only numbers.");
					}
					if(i_MaxAmount > 0) 
					{
						if (i_MaxAmount <= ((TokenBox)m_Box ).Token)
						{				
							if (i_MaxAmount <= 1000000)
							{
								m_From.AddToBackpack(new TokenCheck(i_MaxAmount));
								m_From.SendMessage(1161, "A check for {0} tokens has been placed in your pack.", i_MaxAmount);
								((TokenBox)m_Box ).Token = (((TokenBox)m_Box ).Token - i_MaxAmount);
							}
							else
								m_From.SendMessage(1161, "You can't write a check for more then 1,000,000 tokens at one time.");
						}
						else
							m_From.SendMessage(1173, "You don't have that many tokens in your box.");
					}
					m_From.SendGump( new TokenBoxGump( m_From, m_Box ) );
				}
			}
		}
	}
}

namespace Server.Items
{
	public class TokenBoxTarget : Target
	{
		private TokenBox m_Box;

		public TokenBoxTarget( TokenBox box ) : base( 18, false, TargetFlags.None )
		{
			m_Box = box;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( m_Box.Deleted )
			return;

			m_Box.EndCombine( from, targeted );
		}
	}
}