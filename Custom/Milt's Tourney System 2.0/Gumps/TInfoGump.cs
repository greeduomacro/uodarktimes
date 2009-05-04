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
	public class TInfoGump : TBaseGump
	{
		private List<int> m_ButtonIDs;
		private List<Mobile> m_People;

		private readonly double entriesPerPage = 9;
		private int m_Page;

		public TInfoGump(TSystemStone stone, int page)
			: base( stone, 448, "Information and Statistics")
		{
			m_Stone = stone;
			m_Page = page;

			this.AddLabel(109, 203, 36, @"Kick");
			this.AddLabel(156, 203, 36, @"Player Name");
			this.AddLabel(203, 176, 36, String.Format("There are {0} players signed up. [{1} teams]", GetMasterList().Count, m_Stone.SignedUp.Count));
			this.AddLabel(282, 203, 36, @"Team ID");
			this.AddButton(490, 225, 2437, 2438, (int)Buttons.btnPageDown, GumpButtonType.Reply, 0);
			this.AddButton(476, 225, 2435, 2436, (int)Buttons.btnPageUp, GumpButtonType.Reply, 0);
			this.AddLabel(472, 204, 36, @"Page");
			this.AddImage(445, 294, 5536);

			m_ButtonIDs = new List<int>();

			if(!m_Stone.Started)
				InitializeComponent();
		}
		
		public enum Buttons
		{
			btnPageDown = 1,
			btnPageUp
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{ 		
			Mobile from = state.Mobile;

			if(m_ButtonIDs.Contains( info.ButtonID ))
			{
				Close(from, this);

				m_Stone.VerifyQuit(m_People[GetIndex(info.ButtonID)]);
				m_People[GetIndex(info.ButtonID)].SendMessage("A game master has kicked you out of the tournament.");

				from.SendGump( new TInfoGump( m_Stone, m_Page ) );

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
				case 1: //PageDown
				{
					Close(from, this);
					from.SendGump( new TInfoGump( m_Stone, (m_Page + 1) ) );
					break;
				}
				case 2: //PageUp
				{
					Close(from, this);
					from.SendGump( new TInfoGump( m_Stone, (m_Page - 1) ) );
					break;
				}
			}

			base.OnResponse(state, info);
		}

		private void InitializeComponent()
		{
			List<Mobile> master = GetMasterList();

			int TotalEntries = master.Count;

			if(TotalEntries == 0)
				return;

			int pagesTotal = (int)Math.Ceiling(((double)TotalEntries / entriesPerPage));

			if(m_Page > pagesTotal || m_Page < 1)
				m_Page = 1;

			int start = (m_Page - 1) * 9;

			m_People = new List<Mobile>();

			for(int i = start; i < master.Count; ++i)
			{
				if(m_People.Count < 9)
					m_People.Add(master[i]);
				else
					break;
			}

			for(int i = 0; i < m_People.Count; ++i)
			{
				int location = 199 + ((i + 1) * 27);
				int id = (i + 5) * 2;

				this.AddButton(108, location, 4017, 4018, id, GumpButtonType.Reply, 0);
				this.AddLabel(156, location, 100, m_People[i].Name);
				this.AddLabel(282, location, 100, GetIndexFor(m_People[i]));

				m_ButtonIDs.Add(id);
			}
		}

		private string GetIndexFor(Mobile from)
		{
			int index = 0;

			for(int i = 0; i < m_Stone.SignedUp.Count; ++i)
			{
				List<Mobile> team = m_Stone.SignedUp[i];

				if(team.Contains(from))
				{
					index = i;
					break;
				}
			}

			return index.ToString();
		}

		private List<Mobile> GetMasterList()
		{
			List<Mobile> master = new List<Mobile>();

			for(int i = 0; i < m_Stone.SignedUp.Count; ++i)
			{
				List<Mobile> team = m_Stone.SignedUp[i];

				for(int x = 0; x < team.Count; ++x)
					master.Add(team[x]);
			}

			return master;
		}

		private int GetIndex(int id){ return (id / 2) - 5; }
	}
}