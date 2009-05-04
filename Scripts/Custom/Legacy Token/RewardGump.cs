using System; 
using System.Net; 
using Server; 
using Server.Accounting; 
using Server.Gumps; 
using Server.Items; 
using Server.Mobiles; 
using Server.Network;

namespace Server.Gumps
{
    public class RewardGump : Gump
    {
        private Mobile m_Mobile;
        private Item m_Deed;

        public RewardGump(Mobile from, Item deed)
            : base(30, 20)
        {
            m_Mobile = from;
            m_Deed = deed;

            Closable = true;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(1);

            AddBackground(0, 0, 140, 400, 3000);
            AddBackground(8, 8, 134, 384, 5054);

            AddLabel(40, 12, 37, "Reward Gump");

            Account a = from.Account as Account;


            AddLabel(52, 40, 0, "AosRobe");
            AddButton(12, 40, 4005, 4007, 1, GumpButtonType.Reply, 1);
            AddLabel(52, 60, 0, "Umbra Robe");
            AddButton(12, 60, 4005, 4007, 2, GumpButtonType.Reply, 2);
            AddLabel(52, 80, 0, "PyroStaff");
            AddButton(12, 80, 4005, 4007, 3, GumpButtonType.Reply, 3);
            AddLabel(52, 100, 0, "Crimson Cincture");
            AddButton(12, 100, 4005, 4007, 4, GumpButtonType.Reply, 4);
            AddLabel(52, 120, 0, "RBG sash");
            AddButton(12, 120, 4005, 4007, 5, GumpButtonType.Reply, 5);          
}


        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 0:  
                    {
                        from.CloseGump(typeof(RewardGump));
                        break;
                    }
                case 1: 
                    {
                        Item item = new HoodedShroudOfShadows();
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(RewardGump));
                        m_Deed.Delete();
                        break;
                    }
                case 2: 
                    {
                        Item item = new HoodedRobeOfUmbra();
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(RewardGump));
                        m_Deed.Delete();
                        break;
                    }
                case 3: 
                    {
                        Item item = new StaffOfPyros();
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(RewardGump));
                        m_Deed.Delete();
                        break;
                    }
                case 4: 
                    {
                        Item item = new CrimsonCincture();
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(RewardGump));
                        m_Deed.Delete();
                        break;
                    }
                case 5: 
                    {
                        Item item = new RoyalBritanniaGuard();
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(RewardGump));
                        m_Deed.Delete();
                        break;
                    }
              }
        }
    }
}
