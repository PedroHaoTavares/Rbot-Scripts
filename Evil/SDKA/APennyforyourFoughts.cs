//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreDailies.cs
//cs_include Scripts/Evil/SDKA/CoreSDKA.cs
using Skua.Core.Interfaces;

public class DSOPenny
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreSDKA SDKA = new CoreSDKA();
    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        SDKA.Penny();

        Core.SetOptions(false);
    }
}
