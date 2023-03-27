using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HabTech2.Utilities
{
    public class HabTechLogger
    {
        private const string MODNAME = "HabTech2";

        public static void Log(string message)
        {
            Debug.Log($"[{MODNAME}] {message}");
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning($"[{MODNAME}] {message}");
        }

        public static void LogError(string message)
        {
            Debug.LogError($"[{MODNAME}] {message}");
        }
    }
}
