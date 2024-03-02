/*
name: script name here
description: Farms [InsertItem] using your army.
tags: army, [item]
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/Army/CoreArmyLite.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using Skua.Core.Models.Monsters;
using Skua.Core.Models.Quests;
using Skua.Core.Options;

public class ArmyTemplatev5 //Rename This
{
    private static IScriptInterface Bot => IScriptInterface.Instance;
    private static CoreBots Core => CoreBots.Instance;
    private readonly CoreArmyLite Army = new();
    private static readonly CoreArmyLite sArmy = new();

    public string OptionsStorage = "CustomAggroMon";
    public bool DontPreconfigure = true;
    public List<IOption> Options = new()
    {
        //scroll to after the `WTFisGoingOn()` void, and edit teh enum for the quest rewards vv
        new Option<Rewards>("QuestRewards", "Pick your reward", "Pick your reward", Rewards.Off),
        sArmy.player1,
        sArmy.player2,
        sArmy.player3,
        sArmy.player4,
        sArmy.player5,
        sArmy.player6,
        sArmy.player7,
        sArmy.packetDelay,
        CoreBots.Instance.SkipOptions
    };


    // Comment out one of these depending:
    readonly int[] QuestIDs = { 0000 };
    readonly int PickRewardQuest = 0000;

    public void ScriptMain(IScriptInterface bot)
    {

        //automaticly add the quest rewards to the banking blacklist (it wotn bank then even if bankmisc in corebots is on)

        //Non-Pick Reward Quest:
        Core.BankingBlackList.AddRange(Core.QuestRewards(QuestIDs));
        //Pick Reward Quest:
        // Core.BankingBlackList.AddRange(Core.QuestRewards(PickRewardQuest));

        Core.SetOptions();

        WTFisGoingOn();

        Core.SetOptions(false);
    }

    //move everything outside the scriptmain (exe will yell at you otherwise)
    //take the void name vvv and put it after the set options above.
    public void WTFisGoingOn()
    {

        // Instructions for using the ArmyBits method 
        // 1. Fill in the map name.
        // 2. Fill in the cell(s) you want to jump to (can be multiple cells).
        // 3. Fill in the MonsterMapID(s) you want to target (can be multiple IDs for multi-targeting).
        // 4. Fill in the item(s) you want to farm (can be multiple items).
        // 5. Fill in the desired quantity of the item(s). --can only use 1 quant atm unless you wanna start getting into ditionary stuff.. and i cba >:()
        // 6. Leave the `QuestIDs` param alone as it takes the ints from what you edited above.
        // 7. Uncomment the appropriate method  based on single/multi-targeting.
        // 8. Repeat the method for each item you want to farm.

        #region Edit This vvv                                           vvv Edit this
        //~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~
        Core.EnsureLoad(QuestIDs);

        // vv more for single item farming like DreadRock LTs
        if (Bot.Config!.Get<Rewards>("QuestRewards") == Rewards.Off)
        {
            // if u want to do a continous farm just do maxstack+1 in the quant
            // vv        Select one of these [single/multi target] to use [leave the `QuestIDs` as it goes off of what is at the top there]    vvvv

            // Single-target example (target MID is the first 1):
            // ArmyBits("map", new[] { "cell" }, 1, new[] { "item" }, 1, ClassType.Solo, QuestIDs);

            // Multi-target example (target MIDs is the first { 1, 2}):
            // ArmyBits("map", new[] { "cell" }, new[] { 1, 2 }, new[] { "item" }, 1, ClassType.Solo, QuestIDs);
        }

        //this is for the `Pick Reward`vv
        else
        {
            // Edit these here replacing item# & "item" with the apropriate item name        
            // -- the [1][2] corresponds to  the Questid posion in the  "readonly int[] QuestIDs = new[] { 0000 };" above {1,2,3}  <- [2] is "2".
            // you can add more Questitem# item bases for each item you want
            // add more Quest#Item#

            ItemBase? Quest1Item1 = Core.EnsureLoad(QuestIDs[1])?.Rewards.FirstOrDefault(x => x.Name == "Item");
            ItemBase? Quest2Item1 = Core.EnsureLoad(QuestIDs[2])?.Rewards.FirstOrDefault(x => x.Name == "Item");
            //add more itembases if needed, and if the quest is different change the QuetID in teh `EnsureLoad()`.

            while (!Bot.ShouldExit
         //Change the quant if u dont want max stack (replace the Item!.MaxStack with your desired quant)
         && !Core.CheckInventory(Quest1Item1!.Name, Quest1Item1!.MaxStack)
         && !Core.CheckInventory(Quest2Item1!.Name, Quest2Item1!.MaxStack))
            //add `&& !Core.CheckInventory(item!.Name, item!.MaxStack)` if more items are in the rewards.
            // 'QuestIDs' can be edited above and dont need to be inserted here vv
            {

                // if u want to do a continous farm just do maxstack+1 in the quant
                // vv        Select one of these [single/multi target] to use    vvvv

                // Single-target example:
                // ArmyBits("map", new[] { "cell" }, 1, new[] { "item" }, 1, ClassType.Solo, QuestIDs);

                // Multi-target example:
                // ArmyBits("map", new[] { "cell" }, new[] { 1, 2 }, new[] { "item" }, 1, ClassType.Solo, QuestIDs);

                // --Max stack all--
                if (Bot.Config!.Get<Rewards>("QuestRewards") == Rewards.All)
                    foreach (var rewardValue in Enum.GetValues(typeof(Rewards)).Cast<int>().Where(value => value != (int)Rewards.All))
                    {
                        var rewardItem = Bot.Inventory.Items.FirstOrDefault(x => x.ID == rewardValue);

                        //!!!dont change the quant here ----------------------------------vvvvv!!!
                        if (rewardItem != null && !Core.CheckInventory(rewardItem.Name, rewardItem.MaxStack))
                        {
                            Core.EnsureComplete(PickRewardQuest, rewardValue);
                            continue;
                        }
                    }

                // --Max stack specific--
                else if (Bot.Config!.Get<Rewards>("QuestRewards") != Rewards.All && Bot.Config!.Get<Rewards>("QuestRewards") != Rewards.Off)
                {
                    foreach (var rewardValue in Enum.GetValues(typeof(Rewards)).Cast<int>().Where(value => value != (int)Rewards.All))
                    {
                        ItemBase? RewardID = Core.EnsureLoad(PickRewardQuest)?.Rewards.FirstOrDefault(x => x.ID == rewardValue);
                        switch (rewardValue)
                        {
                            // add more cases if the quest has more rewards, and add them and their itemids to the enum below
                            // you may edit here vvv
                            case (int)Rewards.item_one:
                            case (int)Rewards.item_two:
                                {
                                    //leave this quant be its just to set an int
                                    int Quant = 0;

                                    // you may change the quant here --------vvv
                                    //these 2 quants will set the Checkinvs quants below
                                    if (rewardValue == (int)Rewards.item_one)
                                        Quant = 0;
                                    else if (rewardValue == (int)Rewards.item_two)
                                        Quant = 0;
                                    // you may change the quant here --------^^

                                    //leave this vv as it goes of he quants above here
                                    if (!Core.CheckInventory(rewardValue, Quant > 0 ? Quant : RewardID!.ID))
                                        Core.EnsureComplete(PickRewardQuest, rewardValue);
                                    continue;
                                }
                        }

                    }

                    //~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~~-~-~
                }
                #endregion Edit This ^^^                                          ^^^ Edit this
            }
        }
    }

    private enum Rewards
    {
        //you may edit here vvv
        //[item_name = itemID,] - _'s are required
        All = 0,
        item_one = 1,
        item_two = 2,
        Off = 3

        //you may edit here ^^^
    };

    //ignore everything below here vvvvvv
    #region IgnoreME

    // Use this one for when its a single map, and your just aggroing around (Example: Dreadrock LTs)
    public void ArmyBits(string map, string[] cell, int MonsterMapID, string[] items, int quant, ClassType classToUse, int[] QuestIDs)
    {
        // Setting up private rooms and class
        Core.PrivateRooms = true;
        Core.PrivateRoomNumber = Army.getRoomNr();
        Core.EquipClass(classToUse);

        Core.AddDrop(items);
        Core.AddDrop(Core.QuestRewards(QuestIDs));

        Core.EnsureAcceptmultiple(true, QuestIDs);

        // Single-target scenario
        // Explanation: For each item you specified, target the specified MonsterMapID
        foreach (string item in items)
        {
            // Aggro and divide on cells
            Army.AggroMonMIDs(MonsterMapID);
            Army.AggroMonStart(map);
            Army.DivideOnCells(cell);

            // Farm the specified item
            while (!Bot.ShouldExit && !Core.CheckInventory(item, quant))
            {
                foreach (Monster Mob in Bot.Monsters.CurrentAvailableMonsters.Where(m => m.MapID == MonsterMapID))
                {
                    while (!Bot.ShouldExit && Core.IsMonsterAlive(Mob.MapID, true))
                    {
                        Bot.Combat.Attack(Mob.MapID);
                        if (Bot.Config!.Get<Rewards>("QuestRewards") == Rewards.Off)
                        {
                            foreach (int Q in QuestIDs)
                                if (Bot.Quests.CanComplete(Q))
                                    Bot.Quests.Complete(Q);
                        }
                        if (Core.CheckInventory(item, quant))
                            return;
                    }
                }
            }

            // Clean up
            Army.AggroMonStop(true);
            Core.JumpWait();
            Core.CancelRegisteredQuests();
        }
    }

    // Use this one for multi-mob farms (like VA's where there are multiple bosses)
    public void ArmyBits(string map, string[] cell, int[] MonsterMapIDs, string[] items, int quant, ClassType classToUse, int[] QuestIDs)
    {
        // Setting up private rooms and class
        Core.PrivateRooms = true;
        Core.PrivateRoomNumber = Army.getRoomNr();
        Core.EquipClass(classToUse);

        Core.AddDrop(items);
        Core.AddDrop(Core.QuestRewards(QuestIDs));

        Core.EnsureAcceptmultiple(true, QuestIDs);

        foreach (string item in items)
        {
            bool inventoryConditionMet = Core.CheckInventory(item, quant);

            if (inventoryConditionMet)
                return;

            Army.AggroMonMIDs(MonsterMapIDs);
            Army.AggroMonStart(map);
            Army.DivideOnCells(cell);
            Core.FarmingLogger(item, quant);
            Bot.Player.SetSpawnPoint();
            string dividedCell = Bot.Player.Cell;

            while (!Bot.ShouldExit && !inventoryConditionMet)
            {
                foreach (int monsterMapID in MonsterMapIDs)
                {
                    // Making sure you're on divided cell
                    while (!Bot.ShouldExit && Bot.Player.Cell != dividedCell)
                    {
                        Core.Jump(dividedCell);
                        Core.Sleep();
                        if (Bot.Player.Cell == dividedCell)
                            break;
                    }

                    // Attacking MID
                    while (!Bot.ShouldExit && Core.IsMonsterAlive(monsterMapID, useMapID: true))
                        Bot.Combat.Attack(monsterMapID);

                    // Completing Quest
                    if (Bot.Config!.Get<Rewards>("QuestRewards") == Rewards.Off)
                    {
                        foreach (int questID in QuestIDs)
                            if (Bot.Quests.CanComplete(questID))
                                Bot.Quests.Complete(questID);
                    }

                    inventoryConditionMet = Core.CheckInventory(item, quant);

                    // Break out of the foreach loop
                    if (inventoryConditionMet)
                    {
                        Army.AggroMonStop(true);
                        Core.JumpWait();
                        return;
                    }
                }
            }
        }
    }

    #endregion IgnoreME

}