/*
name: Hollow Soul
description: Farms "Hollow Soul"
tags: hollow soul, shadowrealm, hollowborn
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreDailies.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/Hollowborn/CoreHollowborn.cs
//cs_include Scripts/Nation/CoreNation.cs

using Skua.Core.Interfaces;

public class HollowSoul
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new();
    public CoreAdvanced Adv = new();
    public CoreHollowborn HB = new();
    public CoreNation Nation = new();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        GetYaSoulsHeeeere();

        Core.SetOptions(false);
    }

    public void GetYaSoulsHeeeere(int HSQuant = 2500)
    {
        if (Core.CheckInventory("Hollow Soul", HSQuant))
            return;

        Core.FarmingLogger("Hollow Soul", HSQuant);
        Core.EquipClass(ClassType.Farm);
        Core.RegisterQuests(7553, 7555);
        Core.Join("Shadowrealm", "r2", "Left");
        Bot.Player.SetSpawnPoint();
        while (!Bot.ShouldExit && !Core.CheckInventory("Hollow Soul", HSQuant))
        {
            foreach (int MapID in Bot.Monsters.CurrentAvailableMonsters
                    .Where(x => Core.IsMonsterAlive(x.MapID, useMapID: true))
                    .Select(m => m.MapID))

            {
                Bot.Kill.Monster(MapID);
                if (Core.CheckInventory("Hollow Soul", HSQuant))
                    break;
            }
        }
    }
}
