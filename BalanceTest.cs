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
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

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
            foreach (TowerModel tower in Game.instance.model.towers)
            {
                if (Regex.IsMatch(tower.name, "CaptainChurchill"))
                {
                    foreach (var w in tower.GetWeapons())
                    {
                        if (w.projectile.id == "Projectile")
                        {
                            w.rate /= 1.5f;
                            var b = w.projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>();
                            b.minimumTimeDifferenceInFrames = 2;
                            b.projectile.GetDamageModel().damage /= 1.5f;

                            try
                            {
                                var c = b.projectile.GetBehavior<DamageModifierForTagModel>();
                                if (c.tag == "Fortified")
                                {
                                    c.damageAddative /= 1.5f;
                                }
                            }
                            catch
                            {

                            }
                            
                        }
                        if (w.projectile.id == "MachineGunProjectile")
                        {
                            w.rate = 999f;

                        }
                    }

                    foreach (var a in tower.GetAbilities())
                    {
                        foreach (var b in a.behaviors)
                        {
                            try
                            {
                                var d = b.Cast<MutateProjectileOnAbilityModel>();
                                //d.damageIncrease *= 2;
                                d.projectileBehaviorModel.Cast<DamageModifierForTagModel>().damageAddative /= 1.5f;
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
}