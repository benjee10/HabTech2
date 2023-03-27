using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HabTech2.LoadedNodes
{
    public class UpgradeInfo : ScriptableObject
    {
        public new string name;
        public string upgradeName;
        public string imageUrl;
        public int index;
        public int partModuleIndex = -1;

        public List<string> transformsString;
        public List<Transform> transforms;

        public ConfigNode moduleNode;
        public PartModule partModule;

        public Sprite sprite;

        public bool upgradeApplied = false;

        public void Load(ConfigNode configNode)
        {
            configNode.TryGetValue("name", ref name);
            configNode.TryGetValue("upgradeName", ref upgradeName);
            configNode.TryGetValue("imageUrl", ref imageUrl);

            if (configNode.HasValue("transform"))
            {
                transformsString = new List<string>();
                transformsString = configNode.GetValues("transform").ToList();
            }

            if (configNode.HasNode("MODULE"))
            {
                moduleNode = configNode.GetNode("MODULE");
            }
        }

    }
}

