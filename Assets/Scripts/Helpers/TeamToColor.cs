using Unity.VisualScripting;
using UnityEngine;

public static class TeamToColor
{
    public static void GetTeamColor(this ref Color col, Team team)
    {
        if (team is Team.Blue)
            col = new Color(0f, 0.4f, 1f);
        else if (team is Team.Red)
            col = Color.red;

        col = Color.white;
    }
    public static Color GetTeamColor(Team team)
    {
        if (team is Team.Blue)
            return new Color(0f, 0.4f, 1f);
        else if (team is Team.Red)
            return Color.red;

        return Color.white;
    }
}
