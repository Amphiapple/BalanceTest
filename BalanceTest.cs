using MelonLoader;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using BalanceTest;
using Harmony;
using System;
using System.Text.RegularExpressions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Scenes;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

[assembly: MelonInfo(typeof(BalanceTest.BalanceTest), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BalanceTest;

public class BalanceTest : BloonsTD6Mod
{

    public override void OnApplicationStart()
    {
        base.OnApplicationStart();
        Console.WriteLine("BalanceTest loaded!");
    }

    [HarmonyPatch(typeof(TitleScreen), "Start")]
    public class Awake_Patch
    {

        [HarmonyPostfix]
        public static void Postfix()
        {
            SuperMonkey();
        }
    }

    private static void SuperMonkey()
    {
        var models = Game.instance.model;

        models.GetUpgrade("Knockback").cost = 2000;
        models.GetUpgrade("Dark Knight").cost = 6500;

        foreach (TowerModel tower in Game.instance.model.towers)
        {
            if (Regex.IsMatch(tower.name, "SuperMonkey-..[1-2]"))
            {
                foreach (var w in tower.GetWeapons())
                {
                    foreach (var b in w.projectile.behaviors)
                    {
                        try
                        {
                            b.Cast<KnockbackModel>().lightMultiplier = 1.2f;
                            b.Cast<KnockbackModel>().moabMultiplier = 0.2f;
                        }
                        catch
                        {

                        }
                    }
                }
            }

            if (Regex.IsMatch(tower.name, "SuperMonkey-3.."))
            {
                foreach (var w in tower.GetWeapons())
                {
                    try
                    {
                        w.emission.Cast<RandomArcEmissionModel>().count = 4;
                    }
                    catch
                    {

                    }
                }
            }

            if (Regex.IsMatch(tower.name, "SuperMonkey-.[3-5]."))
            {
                foreach (var w in tower.GetWeapons())
                {
                    w.projectile.pierce += 1;
                }
            }

            if (Regex.IsMatch(tower.name, "SuperMonkey-..[3-5]"))
            {
                foreach (var w in tower.GetWeapons())
                {
                    foreach (var b in w.projectile.behaviors)
                    {
                        try
                        {
                            b.Cast<KnockbackModel>().lightMultiplier = 1.3f;
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }
}