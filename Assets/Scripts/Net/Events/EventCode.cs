using UnityEngine;
using System.Collections;

public static class EventCode
{
    public static byte STATS_STRAT = 1;
    public static byte STATS_HERO = 2;
    public static byte HERO_SPELL_LAUNCHED = 3;
    public static byte HERO_DEATH = 4;

    public static byte STRAT_LAUNCH_INSTANT = 50;
    public static byte STRAT_LAUNCH_TO_TARGET = 51;
    public static byte STRAT_LAUNCH_TO_POSITION = 52;

    public static byte STRAT_MODIFY_NODE = 60;
    public static byte STRAT_CREATE_TREE = 61;

    public static byte STRAT_SPAWN_MINION = 70;
    public static byte STRAT_MODIFY_LANE_COMPOSITION = 71;

    public static byte POWER_UP = 10;
    public static byte Door = 11;

    public static byte SIGNAL_PING_TEAM_1 = 91;
    public static byte SIGNAL_PING_TEAM_2 = 92;
}
