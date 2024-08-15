using MelonLoader;

using BTD_Mod_Helper;

using BTD_Mod_Helper.Extensions;

using BalanceTest;

using HarmonyLib;

using System;

using System.Text.RegularExpressions;

using Il2Cpp;

using Il2CppAssets.Scripts.Models.Towers;

using Il2CppAssets.Scripts.Models.Towers.Behaviors;

using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;

using Il2CppAssets.Scripts.Models.Towers.Projectiles;

using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

using Il2CppAssets.Scripts.Unity;

using Il2CppAssets.Scripts.Unity.Scenes;

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
            foreach (TowerModel t in Game.instance.model.towers)
            {
                if (Regex.IsMatch(t.name, "Adora"))
                {
                    foreach (var w in t.GetWeapons())
                    {
                        if (w.projectile.id == "BaseProjectile")
                        {
                            //Remove main attack fortified damage
                            try
                            {
                                var b = w.projectile.GetBehavior<DamageModifierForTagModel>();
                                if (b.tag == "Fortified")
                                {
                                    b.damageAddative = 0;
                                }
                            }
                            catch
                            {

                            }

                            //Increase main attack pierce
                            if (Regex.IsMatch(t.name, "Adora 1[3-9]") || Regex.IsMatch(t.name, "Adora 20"))
                            {
                                w.projectile.pierce += 2;
                            }

                            //Increase main attack rate
                            if (Regex.IsMatch(t.name, "Adora 1[1-6]"))
                            {
                                w.rate = 0.8f;
                            }
                            else if (Regex.IsMatch(t.name, "Adora 1[7-9]") || Regex.IsMatch(t.name, "Adora 20"))
                            {
                                w.rate = 0.6f;
                            }
                        }
                    }

                    foreach (var a in t.GetAbilities())
                    {
                        if (a.displayName == "Blood Sacrifice")
                        {
                            //Decrease blood sacrifice cooldown
                            a.cooldown = 10;

                            //Decrease sacrifice amount for buff
                            if (Regex.IsMatch(t.name, "Adora 20"))
                            {
                               a.GetBehavior<BloodSacrificeModel>().bonusSacrificeAmount = 50;
                            }
                        }

                        if (a.displayName == "Ball of Light")
                        {
                            //Remove ball of light fortified damage
                            var b = a.GetBehavior<AbilityCreateTowerModel>().towerModel.GetWeapon().projectile.GetBehavior<DamageModifierForTagModel>();
                            if (b.tag == "Fortified")
                            {
                                b.damageAddative = 0;
                            }

                            //Increase lv20 ball of light lifespan
                            if (Regex.IsMatch(t.name, "Adora 20"))
                            {
                                a.GetBehavior<AbilityCreateTowerModel>().towerModel.GetBehavior<TowerExpireModel>().lifespan = 20;
                            }
                        }
                    }
                }
            }
        }
    }
}