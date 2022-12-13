namespace Player.Info
{
    public class PlayerInfo
    {
        public string name { get; private set; }
        public Team team { get; private set; }

        public PlayerInfo(string infoPath, Team team)
        {
            ReadInfo();
            this.team = team;

            //throw new System.NotImplementedException();
        }

        public void ReadInfo()
        {
            //reading a json file with this info at the start of a game 
            //tbd
            //throw new System.NotImplementedException();
        }
    }
}
