using System;
using System.Collections;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using System.Reflection;
using PolyTechFramework;
namespace NoJointLimit {
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInDependency(PolyTechMain.PluginGuid, BepInDependency.DependencyFlags.HardDependency)]
    
    
    public class NoJointLimit : PolyTechMod {
        
        
        public const string pluginGuid = "polytech.NoJointLimit";
        public const string pluginName = "No Joint Limit";
        public const string pluginVersion = "1.0.0";
        
        public static ConfigEntry<bool> mEnabled;
        public static ConfigEntry<bool> infiniteAmount;
        public static ConfigEntry<int> jointLimit;
        
        public ConfigDefinition mEnabledDef = new ConfigDefinition(pluginVersion, "Enable/Disable Mod");
        public ConfigDefinition infiniteAmountDef = new ConfigDefinition(pluginVersion, "Infinite Edges On Node");
        public ConfigDefinition jointLimitDef = new ConfigDefinition(pluginVersion, "Joint Limit (When infinite not enabled)");
        
        
        
        
        public override void enableMod(){
            mEnabled.Value = true;
            this.isEnabled = true;
        }
        public override void disableMod(){
            mEnabled.Value = false;
            this.isEnabled = false;
        }
        public override string getSettings(){
            return "";
        }
        public override void setSettings(string settings){
            
        }
        public NoJointLimit(){
            
            mEnabled = Config.Bind(mEnabledDef, false, new ConfigDescription("Controls if the mod should be enabled or disabled", null, new ConfigurationManagerAttributes {Order = 0}));
            infiniteAmount = Config.Bind(infiniteAmountDef, true, new ConfigDescription("Allows Infinite Edges On Each Node", null, new ConfigurationManagerAttributes {Order = -1}));
            jointLimit = Config.Bind(jointLimitDef, 32, new ConfigDescription("The limit number of joints when infinite joints is disabled", null, new ConfigurationManagerAttributes {Order = -2}));
        }
        void Awake(){
            this.repositoryUrl = null;
            this.isCheat = true;
            PolyTechMain.registerMod(this);
            Logger.LogInfo("No Joint Limit Registered");
            Harmony.CreateAndPatchAll(typeof(NoJointLimit));
            Logger.LogInfo("No Joint Limit Methods Patched");
        }
        void Update(){
            
        }
        
        [HarmonyPatch(typeof(BridgeJoint), "HasMaxEdges")]
        [HarmonyPrefix]
        private static bool BridgeJointHasMaxEdgesPrefixPatch(ref BridgeJoint __instance, ref bool __result){
          
            if(mEnabled.Value){
                if(infiniteAmount.Value){
                    __result = false;
                }else{
                    __result = (float)BridgeEdges.GetNumEdgesConnectedToJoint(__instance) >= jointLimit.Value;
                }
                return false; // Cancels Original Function
            }
            return true;
        }
    }
}