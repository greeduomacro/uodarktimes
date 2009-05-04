using System;
using Server;
using Server.Gumps;

namespace Knives.Utils
{
	public class OverrideGump : GumpPlus
	{
		public static void SendTo( Mobile m, Type type, TimerCallback callback )
		{
			new OverrideGump( m, type, callback );
		}

		private const int Width = 300;
		private const int Height = 220;

		private TimerCallback c_Callback;
		private GumpInfo c_Info;

		public OverrideGump( Mobile m, Type type, TimerCallback callback ) : base( m, 100, 100 )
		{
			c_Callback = callback;
			c_Info = GumpInfo.GetInfo( m, type );

			Override = false;

			NewGump();
		}

		protected override void BuildGump()
		{
			string textcolor = c_Info.TextColor;

			if ( c_Info.TextColorRGB == "" )
				textcolor = HTML.White;

			AddBackground( 0, 0, Width, Height, c_Info.Background == -1 ? 0x13BE : c_Info.Background );
			if ( c_Info.Transparent && !c_Info.DefaultTrans ) AddAlphaRegion( 0, 0, Width, Height );

			AddHtml( 0, 10, Width, 25, textcolor + "<CENTER>" + c_Info.Type.ToString(), false, false );

			AddButton( 0, Height/2-10, 0x15E3, 0x15E7, "Previous Template", new TimerCallback( PreviousTemplate ) );
			AddButton( Width-20, Height/2-10, 0x15E1, 0x15E5, "Next Template", new TimerCallback( NextTemplate ) );

			AddHtml( 0, 30, Width/2, 25, textcolor + "<DIV ALIGN=RIGHT>Background", false, false );
			AddButton( Width/2+10, 30, 0x983, 0x983, "Background Up", new TimerCallback( BackgroundUp ) );
			AddButton( Width/2+10, 40, 0x985, 0x985, "Background Down", new TimerCallback( BackgroundDown ) );
			if ( c_Info.Background == -1 )
				AddHtml( Width/2+30, 30, Width/2-30, 25, textcolor + "(Default)", false, false );

			AddHtml( 0, 60, Width/2, 25, textcolor + "<DIV ALIGN=RIGHT>Text Color (RGB)", false, false );
			AddImageTiled( Width/2+20, 60, 100, 21, 0xBBA );
			AddTextField( Width/2+20, 60, 100, 21, 0x480, 0, c_Info.TextColorRGB );
			AddButton( Width/2+2, 62, 0x93A, 0x93A, "Text Color", new TimerCallback( TextColor ) );

			AddHtml( 0, 90, Width, 25, textcolor + "<CENTER>Transparency", false, false );
			AddHtml( 80, 110, 50, 25, textcolor + "On", false, false );
			AddButton( 50, 110, !c_Info.DefaultTrans && c_Info.Transparent ? 0xD3 : 0xD2, !c_Info.DefaultTrans && c_Info.Transparent ? 0xD3 : 0xD2, "Transparent On", new TimerCallback( TransparentOn ) );
			AddHtml( 140, 110, 50, 25, textcolor + "Off", false, false );
			AddButton( 110, 110, !c_Info.DefaultTrans && !c_Info.Transparent ? 0xD3 : 0xD2, !c_Info.DefaultTrans && !c_Info.Transparent ? 0xD3 : 0xD2, "Transparent Off", new TimerCallback( TransparentOff ) );
			AddHtml( 200, 110, 50, 25, textcolor + "Default", false, false );
			AddButton( 170, 110, c_Info.DefaultTrans ? 0xD3 : 0xD2, c_Info.DefaultTrans ? 0xD3 : 0xD2, "Default Trans", new TimerCallback( DefaultTrans ) );

			AddHtml( 0, 140, Width/2, 25, textcolor + "<DIV ALIGN=RIGHT>Button Template", false, false );
			AddButton( Width/2+10, 140, 0x983, 0x983, "Template Up", new TimerCallback( ButtonUp ) );
			AddButton( Width/2+10, 150, 0x985, 0x985, "Template Down", new TimerCallback( ButtonDown ) );
			if ( (int)c_Info.Template == -1 )
				AddHtml( Width/2+30, 140, Width/2+30, 25, textcolor + "Default", false, false );
			else
			{
				if ( ButtonTemplate.Templates[c_Info.Template] == null )
					return;

				ButtonTemplate temp = (ButtonTemplate)ButtonTemplate.Templates[c_Info.Template];
				AddHtml( Width/2+30+temp.TextXOffset, 145+temp.TextYOffset, 70, 25, textcolor + "<CENTER>Button", false, false );
				AddImage( Width/2+30+temp.BGXOffset, 145+temp.BGYOffset, temp.Background );
			}

			AddHtml( 90, 170, 150, 40, textcolor + "Remove this Template<br>(Use Defaults)", false, false );
			AddButton( 60, 180, 0x93A, 0x93A, "Default", new TimerCallback( Default ) );
		}

		private void PreviousTemplate()
		{
			c_Info = GumpInfo.GetPreviousInfo( Owner, c_Info );

			NewGump();
		}

		private void NextTemplate()
		{
			c_Info = GumpInfo.GetNextInfo( Owner, c_Info );

			NewGump();
		}

		private void BackgroundUp()
		{
			c_Info.BackgroundUp();

			NewGump();
		}

		private void BackgroundDown()
		{
			c_Info.BackgroundDown();

			NewGump();
		}

		private void ButtonUp()
		{
			c_Info.TemplateUp();

			NewGump();
		}

		private void ButtonDown()
		{
			c_Info.TemplateDown();

			NewGump();
		}

		private void TextColor()
		{
			c_Info.TextColorRGB = GetTextField( 0 );

			NewGump();
		}

		private void TransparentOn()
		{
			c_Info.Transparent = true;
			c_Info.DefaultTrans = false;

			NewGump();
		}

		private void TransparentOff()
		{
			c_Info.Transparent = false;
			c_Info.DefaultTrans = false;

			NewGump();
		}

		private void DefaultTrans()
		{
			c_Info.DefaultTrans = true;

			NewGump();
		}

		private void Default()
		{
			GumpInfo.RemoveInfo( Owner, c_Info );

			c_Callback.Invoke();
		}

		protected override void OnClose()
		{
			c_Callback.Invoke();
		}
	}
}