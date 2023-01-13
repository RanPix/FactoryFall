
namespace Player.Info
{
    public class PlayerInfo
    {
        public string netID { get; private set; }
        public string name { get; private set; }
        public Team team { get; private set; }

        public int kills;
        public int deaths;
        public int ping;

        public PlayerInfo(string infoPath, Team team, string netID, string name /*for testing*/)
        {
            ReadInfo();
            this.team = team;
            this.netID = netID;
            this.name = name + netID; // for testing

            //throw new System.NotImplementedException();
        }

        public void ReadInfo()
        {
            //reading a json file with this info at the start of a game 
            //players name and etc
            //tbd
            //throw new System.NotImplementedException();
        }
    }
}
