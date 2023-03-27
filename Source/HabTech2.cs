using HabTech2.Utilities;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HabTech2
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class HabTech2 : MonoBehaviour
    {
        public void Awake()
        {
            if (Versioning.version_major == 1 && Versioning.version_minor < 11)
            {
                // This is just a simple log message, the disabling part works with MM.
                // The MM patch adding the modules adds a :NEEDS part which only triggers
                // in 1.11 or above.
                HabTechLogger.Log("Disabling HabTech2 Part Upgrade System. Requires 1.11 or above.");
            }


            DestroyImmediate(this);
        }
    }
}
