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
using System.Collections.Generic;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.TSystem
{
	public class TRegionGump : TBaseGump
	{
		private List<int> m_ButtonIDs;

		private readonly int Start = 184; //214;

		public TRegionGump(TSystemStone stone)
			: base( stone, CalcSize(stone), "Region Management & Editing" )
		{
			m_Stone = stone;

			this.AddButton(120, 184, 1209, 1210, (int)Buttons.btnAdd, GumpButtonType.Reply, 0);
			this.AddLabel(148, 182, 36, @"Add Region");
			this.AddButton(233, 184, 1209, 1210, (int)Buttons.btnShow, GumpButtonType.Reply, 0);
			this.AddLabel(261, 182, 36, @"Show All Regions");
			this.AddButton(375, 184, 1209, 1210, (int)Buttons.btnDelete, GumpButtonType.Reply, 0);
			this.AddLabel(403, 182, 36, @"Delete All");

			m_ButtonIDs = new List<int>();
			ListRegions();
		}
		
		private enum Buttons
		{
			btnAdd = 1,
			btnShow,
			btnDelete
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{ 		
			Mobile from = state.Mobile;

			if(m_ButtonIDs.Contains( info.ButtonID ))
			{
				Close(from, this);
				from.SendGump( new TRegionEditGump( m_Stone, m_Stone.Coords[GetIndex(info.ButtonID)] ) );

				base.OnResponse(state, info);
				return;
			}

			switch ( info.ButtonID ) 
			{
				case 0: //Right click to close
				{
					Close(from, this);
					from.SendGump( new TMainGump(m_Stone) );

					break;
				}
				case 1: //Add
				{
					Close(from, this);
					m_Stone.ChooseArea(from);

					break;
				}
				case 2: //Show
				{
					Close(from, this);

					if(m_Stone.Coords != null)
						for(int i = 0; i < m_Stone.Coords.Count; ++i)
							m_Stone.ShowBounds(m_Stone.Coords[i], m_Stone.Map);

					from.SendGump( new TRegionGump(m_Stone) );

					break;
				}
				case 3: //Delete
				{
					Close(from, this);

					if(m_Stone.Coords != null)
					{
						m_Stone.Coords.Clear();
						m_Stone.UpdateRegion();
					}

					from.SendGump( new TRegionGump(m_Stone) );

					break;
				}
			}

			base.OnResponse(state, info);
		}

		private void ListRegions()
		{
			if(m_Stone != null && m_Stone.Coords != null)
			{
				for(int i = 0; i < m_Stone.Coords.Count; ++i)
				{
					int location = Start + ((i + 1) * 30);
					int id = (i + 5) * 2;

					this.AddButton(120, location, btnNormID, btnPressID, id, GumpButtonType.Reply, 0);
					this.AddLabel(148, location, 100, String.Format("Start: {0} | End: {1}", m_Stone.Coords[i].Start, m_Stone.Coords[i].End));

					m_ButtonIDs.Add(id);
				}
			}
		}

		private int GetIndex(int id)
		{
			return (id / 2) - 5;
		}

		private static int CalcSize(TSystemStone from)
		{
			if(from.Coords == null)
				return 159;

			return 159 + (from.Coords.Count * 33);
		}
	}
}