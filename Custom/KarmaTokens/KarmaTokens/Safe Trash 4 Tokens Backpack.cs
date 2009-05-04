/// Trash 4 Tokens Backpack v0.1
///created by Daat99 26/03/2005
///Modified by Karmageddon to work with my Token system 02/12/06
using System;
using System.Collections;
using System.Collections.Generic;
using Server.Multis;
using Server.ContextMenus;

namespace Server.Items
{
	public class TrashBackpack : Container
	{
		public override int MaxWeight{ get{ return 0; } } // A value of 0 signals unlimited weight
		public override int DefaultGumpID{ get{ return 0x3C; } }
		public override int DefaultDropSound{ get{ return 0x50; } }

		private DateTime m_LastTrash;
		public DateTime LastTrash{ get{ return m_LastTrash; } set{ m_LastTrash = value; } }


		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 18, 105, 144, 73 ); }
		}

		/*public override bool CanStore( Mobile m )
		{
			return true; 
		}*/

		[Constructable]
		public TrashBackpack() : base( 0x9b2 )
		{
			Name = "A Safe Trash 4 Tokens Backpack"; 
			Movable = true;
			Hue = 1173;
			LootType = LootType.Blessed;
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			List<Item> items = this.Items;
			if ( items.Count > 0 && m_LastTrash <= DateTime.Now)
			{
				Empty(from);
				from.SendMessage("3 minutes safety was over clearing trash before adding more");
			}
			//TotalWeight = 0;
			if ( !base.OnDragDrop( from, dropped ) )
				return false;
			m_LastTrash = (DateTime.Now + TimeSpan.FromMinutes( 3 ));
			return true;
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			List<Item> items = this.Items;
			if ( items.Count > 0 && m_LastTrash <= DateTime.Now)
			{
				Empty(from);
				from.SendMessage("3 minutes safety was over clearing trash before adding more");
			}
			//TotalWeight = 0;
			if ( !base.OnDragDropInto( from, item, p ) )
				return false;
			m_LastTrash = (DateTime.Now + TimeSpan.FromMinutes( 3 ));
			return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			List<Item> items = this.Items;
			if ( items.Count > 0 && m_LastTrash <= DateTime.Now)
			{
				Empty(from);
				from.SendMessage("The 3 minutes safety was over, you can not recover the items.");
			}
			base.OnDoubleClick(from);
		}

		public override void OnItemRemoved( Item item ) 
		{ 
			if (m_LastTrash <= DateTime.Now)
			{
				item.Delete();
				Empty();
			}
			else 
				base.OnItemRemoved( item );
			//TotalWeight = 0;
		} 

		public override void UpdateTotals()
		{
			base.UpdateTotals();
			//SetTotalWeight( 0 );
		}

		public override void OnItemAdded( Item item )
		{
			base.OnItemAdded( item );
			//TotalWeight = 0;
		}

		public void Empty()
		{
			List<Item> items = this.Items;
			if ( items.Count > 0 )
			{
				Mobile from = RootParent as Mobile;
				if (from != null)
				{
					from.SendMessage( "You passed the 3 minutes safety, you can't recover the items." );
					Empty(from);
				}
				else
				{
					for ( int i = items.Count - 1; i >= 0; --i )
					{
						if ( i >= items.Count )
							continue;
						((Item)items[i]).Delete();
					}
				}
			}
		}

		public void Empty(Mobile from)
		{
			EmptyTrash(from, this);
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
			List<Item> items = this.Items;
			if ( items.Count > 0 )
				list.Add( new EmptyTrash4TokensBackpack( from, this ) );
		}

		public TrashBackpack( Serial serial ) : base( serial )
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
			m_LastTrash = DateTime.Now;
		}

		public static void EmptyTrash(Mobile from, Item item)
		{
			List<Item> items = item.Items;
			if ( items.Count > 0 )
			{
				int i_Reward = 0;
				from.PlaySound(0x76);
				for ( int i = items.Count - 1; i >= 0; --i )
				{
					if ( i >= items.Count )
						continue;
					Item it = (Item)items[i] as Item;
					if ( it.Stackable == false && !(item is BaseBook) )
						i_Reward += Utility.RandomMinMax(5,10);
					((Item)items[i]).Delete();
				}
				if (i_Reward > 0)
				{
					Item[] box = from.Backpack.FindItemsByType( typeof( TokenBox ) );

					foreach( TokenBox tb in box )
					{
						if ( from == tb.Owner )
						{
							if ((tb.Token + i_Reward) <= 2000000000 )
							{
								tb.Token = (tb.Token + i_Reward);
								from.SendMessage(1173, "You were rewarded {0} Tokens to your box for cleaning the shard.", i_Reward);
								break;
							}
							else 
								from.SendMessage(1173, "You have a full token box, please make a check and store it in your bank.");
						}
					}
				}
			}
		}

		public class EmptyTrash4TokensBackpack : ContextMenuEntry
		{
			private Mobile m_From;
			private Item m_Item;

			public EmptyTrash4TokensBackpack( Mobile from, Item item ) : base( 0154, 5 )
			{
				m_From = from;
				m_Item = item;
			}

			public override void OnClick()
			{
				EmptyTrash(m_From, m_Item);
			}
		}
	}
}