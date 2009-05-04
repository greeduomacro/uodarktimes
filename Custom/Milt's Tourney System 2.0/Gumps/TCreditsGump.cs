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

namespace Server.TSystem
{
	public class TAboutGump : TBaseGump
	{
		public TAboutGump(TSystemStone stone)
			: base( stone, 448, "About" )
		{
			m_Stone = stone;

			this.AddHtml( 116, 166, 452, 327, About, (bool)false, (bool)true);
		}

		string About = "<baseFONT color='white'><center>Milt's Automated Tournament System - v2.0</center><p>After the first release of this system back at Christmas 2005, I thought I had done a pretty good job. Now, I go through that code and I see how much room for improvement there was. I have learned a lot since then, and so I put that knowledge to use to design version 2. While in the process of coding this version, I gained a lot more knowledge. Right now I am looking back on some of the code and I can see stuff that needs improved. This will be the start of version 3. I realize that I didn't implement some of the features that I did in the last version, but some of them were buggy anyway. Version 3 will probably be my final and most stable release, with tons of features. I just want to send a special thank you to everyone who has helped me with this project, and anyone who has helped beta test or give ideas.";
	}
}