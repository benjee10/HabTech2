using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabTech2.Modules
{
    public class ModuleHabTechItem : ModuleCargoPart
    {
        [KSPField]
        public string upgradeName;

        [KSPField]
        public string upgradeNameDisplay;

        [KSPField]
        public string description = "This is a Upgradable Cargo Part that can be placed in Inventories to upgrade it.";

        public override string GetInfo()
        {
            string txt = $"{description} \n\nPacked Volume: {this.packedVolume} \n";
            
            if (!string.IsNullOrEmpty(upgradeNameDisplay))
            {
                txt += $"Upgrade: {upgradeNameDisplay}";
            }

            return txt;
        }

        public override string GetModuleDisplayName()
        {
            return "Upgrade Part";
        }
    }
}
