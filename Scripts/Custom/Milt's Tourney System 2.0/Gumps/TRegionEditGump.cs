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

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.TSystem
{
	public class TRegionEditGump : TBaseGump
	{
		private Rectangle3D m_Rect;

		public TRegionEditGump(TSystemStone stone, Rectangle3D rect)
			: base( stone, 195, "Region Properties" )
		{
			m_Stone = stone;
			m_Rect = rect;

			this.AddButton(120, 184, 1209, 9904, (int)Buttons.btnShow, GumpButtonType.Reply, 0);
			this.AddButton(196, 184, 1209, 9904, (int)Buttons.btnDelete, GumpButtonType.Reply, 0);
			this.AddButton(279, 184, 1209, 9904, (int)Buttons.btnBack, GumpButtonType.Reply, 0);
			this.AddLabel(144, 182, 36, @"Show");
			this.AddLabel(220, 182, 36, @"Delete");
			this.AddLabel(303, 182, 36, @"Back to Management");
			this.AddHtml( 105, 215, 447, 21, "<baseFONT color='white'><Center>" + String.Format("Start: {0} | End: {1}", rect.Start, rect.End), (bool)false, (bool)false);
		}
		
		private enum Buttons
		{
			btnShow = 1,
			btnDelete,
			btnBack
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{ 		
			Mobile from = state.Mobile;

			switch ( info.ButtonID ) 
			{
				case 0: //Right click to close
				{
					Close(from, this);
					from.SendGump( new TRegionGump(m_Stone) );

					break;
				}

				case 1: //Show
				{
					Close(from, this);

					if(m_Stone.Coords != null)
							m_Stone.ShowBounds(m_Rect, m_Stone.Map);

					from.SendGump( new TRegionEditGump(m_Stone, m_Rect) );

					break;
				}

				case 2: //Delete
				{
					Close(from, this);

					if(m_Stone.Coords != null)
					{
						if(m_Stone.Coords.Contains(m_Rect))
							m_Stone.Coords.Remove(m_Rect);

						m_Stone.UpdateRegion();
					}

					from.SendGump( new TRegionGump(m_Stone) );

					break;
				}

				case 3: //Back
				{
					Close(from, this);
					from.SendGump( new TRegionGump(m_Stone) );

					break;
				}
			}

			base.OnResponse(state, info);
		}
	}
}