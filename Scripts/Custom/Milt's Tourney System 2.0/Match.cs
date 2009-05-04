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
using Server.Mobiles;

namespace Server.TSystem
{
	public enum MatchType
	{
		Single,
		Multi
	}

	public enum MatchStatus
	{
		Waiting,
		Fighting,
		Over
	}

	public class Match
	{
		private MatchType m_MatchType;
		private MatchStatus m_MatchStatus;

		private List<Mobile> m_Attackers;
		private List<Mobile> m_Defenders;
		private List<Mobile> m_Winners;

		public MatchType MatchType{ get{ return m_MatchType; } set{ m_MatchType = value; } }
		public MatchStatus MatchStatus{ get { return m_MatchStatus; } set{ m_MatchStatus = value; } }

		public List<Mobile> Attackers{ get{ return m_Attackers; } set{ m_Attackers = value; } }
		public List<Mobile> Defenders{ get{ return m_Defenders; } set{ m_Defenders = value; } }
		public List<Mobile> Winners{ get{ return m_Winners; } set{ m_Winners = value; } }

		public bool IsMulti{ get{ return m_MatchType == MatchType.Multi; } }

		public Match(List<Mobile> attackers, List<Mobile> defenders, MatchType match)
		{
			m_Attackers = attackers;
			m_Defenders = defenders;
			m_MatchType = match;
		}
	}
}