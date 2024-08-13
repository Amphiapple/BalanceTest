using MelonLoader;
using BTD_Mod_Helper;
using BalanceTest;

[assembly: MelonInfo(typeof(BalanceTest.BalanceTest), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BalanceTest;

public class BalanceTest : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<BalanceTest>("BalanceTest loaded!");
    }
}