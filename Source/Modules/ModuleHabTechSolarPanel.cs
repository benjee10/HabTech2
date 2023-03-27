using HabTech2.LoadedNodes;
using HabTech2.Modules;
using HabTech2.Utilities;
using KSP.UI;
using KSP.UI.Screens.DebugToolbar.Screens.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static ModuleDeployablePart;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;

namespace HabTech2
{
    public class ModuleHabTechSolarPanel : ModuleInventoryPart
    {
        [KSPField]
        public string inventoryName;

        [KSPField]
        public int inventorySize = 1000;

        [KSPField]
        public int upgradeSlots = 1;

        [KSPField(isPersistant = true)]
        public bool isDeployed = false;

        [KSPField]
        public string description = "This is an Inventory, which can also be used to upgrade the part";

        [KSPField]
        public List<UpgradeInfo> upgradeInfos = new List<UpgradeInfo>();

        public Dictionary<int, GameObject> slotIcons = new Dictionary<int, GameObject>();
        public List<UpgradeInfo> loadingUpgradeInfos = new List<UpgradeInfo>();

        private bool modulesInit = false;

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);

            ConfigNode[] upgradeNodes = node.GetNodes("UPGRADES");
            for (int i = 0; i < upgradeNodes.Length; i++)
            {
                ConfigNode upgradeNode = upgradeNodes[i];
                UpgradeInfo upgradeInfo = ScriptableObject.CreateInstance<UpgradeInfo>();
                upgradeInfo.Load(upgradeNode);
                upgradeInfo.index = i;

                upgradeInfos.Add(upgradeInfo);
            }

            if (!modulesInit)
            {
                foreach (StoredPart storedPart in storedParts.Values)
                {
                    CheckAndApplyUpgrades(storedPart);
                }

                modulesInit = true;
            }
        }

        public override void OnStart(StartState state)
        {

            base.InventorySlots = upgradeSlots;
            base.packedVolumeLimit = inventorySize;

            if(!string.IsNullOrEmpty(inventoryName))
                base.Fields["InventorySlots"].guiName = inventoryName;

            CacheSprites();


            base.OnStart(state);
        }

        public void Start()
        {
            FinishUpgradeApplying();

            GameEvents.onModuleInventoryChanged.Add(OnModuleInventoryChanged);
            GameEvents.onPartActionUIShown.Add(OnPawUIShown);
            GameEvents.onFlightReady.Add(OnFlightReady);
            GameEvents.onAboutToSaveShip.Add(BeforeSave);
            GameEvents.onEditorPartPlaced.Add(OnEditorPartPlaced);
            GameEvents.onEditorShipModified.Add(OnEditorShipModified);
            GameEvents.onEditorPartDeleted.Add(OnEditorPartDeleted);
            //GameEvents.onEditorPartEvent.Add(OnEditorPartEvent);
           
        }

        private void OnEditorPartDeleted(Part data) => StartCoroutine(WaitFramePAW());

        //  private void OnEditorPartEvent(ConstructionEventType eventType, Part part) => OnPawUIShown(part.PartActionWindow, part);
        private void OnEditorShipModified(ShipConstruct data) => StartCoroutine(WaitFramePAW());

        private System.Collections.IEnumerator WaitFramePAW()
        {
            // kinda hacky i guess? B9PartSwitch fires onEditorShipModified before B9PS updates the PAW.
            // so we need to make sure that we wait until B9PS is finished to update the PAW, otherwise the icon 
            // would disapear

            // waits one frame
            yield return 0;
            OnPawUIShown(part.PartActionWindow, part);
        }

        public void UpdateReferences()
        {
            // Deletes reference to old part if part is copied in the Editor
            List<UpgradeInfo> upgradeInfoCopy = new List<UpgradeInfo>();
                
            foreach (UpgradeInfo upgradeInfo in upgradeInfos)
            {
                UpgradeInfo copy = new UpgradeInfo();

                copy.imageUrl = upgradeInfo.imageUrl;
                copy.sprite = upgradeInfo.sprite;
                copy.transformsString = upgradeInfo.transformsString;
                copy.partModule = upgradeInfo.partModule;
                copy.index = upgradeInfo.index;
                copy.moduleNode = upgradeInfo.moduleNode;
                copy.partModuleIndex = upgradeInfo.partModuleIndex;
                copy.upgradeApplied = upgradeInfo.upgradeApplied;
                copy.upgradeName = upgradeInfo.upgradeName;

                if (copy.partModuleIndex != -1)
                {
                    copy.partModule = this.part.Modules.GetModule(copy.partModuleIndex);
                }

                upgradeInfoCopy.Add(copy);
            }

            upgradeInfos = upgradeInfoCopy;
        }

        private void OnEditorPartPlaced(Part part)
        {
            if (part != this.part)
                return;

            UpdateReferences();
            CheckUpgrades();

            //CheckUpgrades();
            //OnPawUIShown(this.part.PartActionWindow, this.part);
        }

        private void BeforeSave(ShipConstruct data)
        {
            if (!HighLogic.LoadedSceneIsFlight)
                return;

           foreach(UpgradeInfo upgradeInfo in upgradeInfos)
            {
                if (upgradeInfo.partModuleIndex != -1)
                {
                  RemovePartModule(upgradeInfo);
                }
            }
        }

        public void OnFlightReady()
        {          
            foreach (UpgradeInfo upgradeInfo in upgradeInfos)
            {
                if (upgradeInfo.partModule != null)
                {
                    if (upgradeInfo.partModule is ModuleDeployableSolarPanel)
                    {
                        ModuleDeployableSolarPanel moduleDeployableSolarPanel = upgradeInfo.partModule as ModuleDeployableSolarPanel;

                        isDeployed = moduleDeployableSolarPanel.deployState == DeployState.EXTENDED ? true : false;
                        string animationName = moduleDeployableSolarPanel.animationName;
                        Animation anim = part.FindModelAnimator(animationName);

                       if (isDeployed)
                       {
                            anim[animationName].wrapMode = WrapMode.ClampForever;
                            anim[animationName].normalizedTime = 1.0f;
                            anim[animationName].enabled = true;
                            anim[animationName].weight = 1.0f;
                       }
                       else
                       {
                            anim[animationName].wrapMode = WrapMode.ClampForever;
                            anim[animationName].normalizedTime = 0.0f;
                            anim[animationName].enabled = true;
                            anim[animationName].weight = 1.0f;
                            anim.Stop(animationName);
                       }
                    }
                }
            }
        }

        /// <returns>Returns a Dictionary that are in the Inventory and have ModuleHabTechItem</returns>
        private Dictionary<string, StoredPart> GetUpgradeItems()
        {
            Dictionary<string, StoredPart> upgradeItems = new Dictionary<string, StoredPart>();
            foreach (StoredPart storedPart in storedParts.Values)
            {
                ConfigNode habTechItemNode = storedPart.FindModule(nameof(ModuleHabTechItem));
                if (habTechItemNode == null) // Stored Part is not a Upgrade
                    continue;


                if (!habTechItemNode.HasValue("upgradeName"))
                    continue;

                string upgradeName = habTechItemNode.GetValue("upgradeName");
                upgradeItems.Add(upgradeName, storedPart);
            }

            return upgradeItems;
        }

        private void OnModuleInventoryChanged(ModuleInventoryPart inventoryModule)
        {
           // if (!HighLogic.LoadedSceneIsFlight)
           //     return;

            if (inventoryModule != this)
                return;

            CheckUpgrades();
            OnPawUIShown(part.PartActionWindow, this.part);
        }

        public void CheckUpgrades()
        {
            Dictionary<string, StoredPart> upgradeItems = new Dictionary<string, StoredPart>();
            upgradeItems = GetUpgradeItems();

            var appliedUpgrades = upgradeInfos.Where(u => u.upgradeApplied == true).ToList();

            if (appliedUpgrades == null)
                return;

            foreach (UpgradeInfo upgradeInfo in upgradeInfos)
            {
                // Upgrade Applied, should not be allthough
                if (upgradeInfo.upgradeApplied && !upgradeItems.Keys.Contains(upgradeInfo.upgradeName))
                {
                    if (upgradeInfo.transformsString != null)
                    {
                        foreach (string transformString in upgradeInfo.transformsString)
                        {
                            Transform foundTransform = part.FindModelTransform(transformString);
                            foundTransform?.gameObject.SetActive(false);
                        }
                    }

                    if (upgradeInfo.partModule != null)
                    {
                        RemovePartModule(upgradeInfo);
                    }
                    
                    if (slotIcons.TryGetValue(upgradeInfo.index, out GameObject icon))
                    {
                        icon?.SetActive(true);
                    }

                    upgradeInfo.upgradeApplied = false;
                }
                else if (!upgradeInfo.upgradeApplied && upgradeItems.Keys.Contains(upgradeInfo.upgradeName))
                {
                    if (slotIcons.TryGetValue(upgradeInfo.index, out GameObject icon))
                    {
                        icon?.SetActive(false);
                    }

                    if (upgradeInfo.transformsString != null)
                    {
                        foreach (string transformString in upgradeInfo.transformsString)
                        {
                            Transform foundTransform = part.FindModelTransform(transformString);
                            foundTransform?.gameObject.SetActive(true);
                        }
                    }
                    if (upgradeInfo.moduleNode != null)
                    {
                        upgradeItems.TryGetValue(upgradeInfo.upgradeName, out StoredPart storedPart);
                        GeneratePartModule(upgradeInfo, storedPart);
                    }

                    /*
                    if (resetAnimation)
                    {
                        Animation anim = part.FindModelAnimator(animationName);
                        anim[animationName].normalizedTime = 0.0f;
                        anim.Stop(animationName);
                    }*/

                    upgradeInfo.upgradeApplied = true;
                }
            }
        }

        public void CheckAndApplyUpgrades(StoredPart storedInvPart)
        {
            ConfigNode habTechItemNode = storedInvPart.FindModule(nameof(ModuleHabTechItem));
            if (habTechItemNode == null) // Stored Part is not a Upgrade
                return;

            if (!habTechItemNode.HasValue("upgradeName"))
                return;

            string itemUpgraded = habTechItemNode.GetValue("upgradeName");

            UpgradeInfo upgradeInfo = upgradeInfos.Where(u => u.upgradeName == itemUpgraded).FirstOrDefault();
            if (upgradeInfo == null) // Stored Part doesn't have any upgrades on this module
                return;

            if (upgradeInfo.moduleNode != null)
            {
                 GeneratePartModule(upgradeInfo, storedInvPart, false);
                //PartModule partModule = part.AddModule(upgradeInfo.moduleNode, false);
            }

            loadingUpgradeInfos.Add(upgradeInfo);
            upgradeInfo.upgradeApplied = true;
        }

        public void FinishUpgradeApplying()
        {
            foreach (UpgradeInfo upgradeInfo in upgradeInfos)
            {
                if (upgradeInfo.transformsString != null)
                {
                    foreach (string transformString in upgradeInfo.transformsString)
                    {
                        Transform foundTransform = part.FindModelTransform(transformString);
                        foundTransform?.gameObject.SetActive(false);
                    }
                }

                if (upgradeInfo.partModule != null && HighLogic.LoadedSceneIsFlight)
                {
                    if (upgradeInfo.partModule is ModuleDeployableSolarPanel)
                    {
                        ModuleDeployableSolarPanel moduleDeployableSolarPanel = upgradeInfo.partModule as ModuleDeployableSolarPanel;
                        moduleDeployableSolarPanel.startFSM();
                        
                    }
                }
            }

            foreach (UpgradeInfo upgradeInfo in loadingUpgradeInfos)
            {
                if (upgradeInfo.transformsString != null)
                {
                    foreach (string transformString in upgradeInfo.transformsString)
                    {
                        Transform foundTransform = part.FindModelTransform(transformString);
                        foundTransform?.gameObject.SetActive(true);
                    }
                }
            }
            loadingUpgradeInfos = new List<UpgradeInfo>();
        }

        public void GeneratePartModule(UpgradeInfo upgradeInfo, StoredPart storedPart, bool moduleNeedsInit = true)
        {
            PartModule partModule = part.AddModule(upgradeInfo.moduleNode, true);
            upgradeInfo.partModuleIndex = this.part.Modules.IndexOf(partModule);


            if (partModule == null)
            {
                HabTechLogger.LogWarning($"Could not find PartModule with name '{upgradeInfo.moduleNode.GetValue("name")}'");
                return;
            }

            int moduleIndex = part.Modules.IndexOf(partModule);
            ProtoPartModuleSnapshot moduleSnapshot = storedPart.snapshot.FindModule(partModule, moduleIndex);
            if (moduleSnapshot != null)
            {
                moduleSnapshot.moduleValues.AddValue("modulePersistentId", partModule.GetPersistentId());
            }

            if (partModule is ModuleDeployableSolarPanel)
            {
                ModuleDeployableSolarPanel moduleDeployableSolarPanel = partModule as ModuleDeployableSolarPanel;
                moduleDeployableSolarPanel.deployState = isDeployed ? DeployState.EXTENDED : DeployState.RETRACTED;
            }

            if (moduleNeedsInit)
            {
                
                StartState startState = part.GetModuleStartState();

                partModule.OnAwake();
                partModule.resHandler.SetPartModule(partModule);

                partModule.OnStart(startState);
                partModule.OnStartFinished(startState);

                partModule.ApplyAdjustersOnStart();
                partModule.OnInitialize();
                partModule.UpdateStagingToggle();
                partModule.OnActive();


                UpdatePAW();

            }

            upgradeInfo.partModule = partModule;
        }

        public void RemovePartModule(UpgradeInfo upgradeInfo)
        {
            if (upgradeInfo.partModule == null)
            {
                HabTechLogger.LogWarning("Something went wrong removing the Part Module");
                return;
            }

            if (upgradeInfo.partModule is ModuleDeployableSolarPanel)
            {
                ModuleDeployableSolarPanel moduleDeployableSolarPanel = upgradeInfo.partModule as ModuleDeployableSolarPanel;
                isDeployed = moduleDeployableSolarPanel.deployState == DeployState.EXTENDED ? true : false;
            }

            part.RemoveModule(upgradeInfo.partModule);
            upgradeInfo.partModule = null;
            upgradeInfo.partModuleIndex = -1;
        }


        private void CacheSprites()
        {
            foreach(UpgradeInfo upgradeInfo in upgradeInfos)
            {
                if (!string.IsNullOrEmpty(upgradeInfo.imageUrl))
                {
                    upgradeInfo.sprite = GameDatabase.Instance.GetTexture(upgradeInfo.imageUrl, false)?.ToSprite();
                }
            }
        }

        /// <summary>
        /// Loads Image into empty inventory slots
        /// </summary>
        private void OnPawUIShown(UIPartActionWindow pawWindow, Part part)
        {
            // Was triggered for other part
            if (this.part != part)
                return;

            foreach(GameObject gameObject in slotIcons.Values)
            {
                gameObject.DestroyGameObject();
            }

            if (Fields.TryGetFieldUIControl("InventorySlots", out UI_Grid inventorySlotsField))
            {
                UIPartActionInventory inventory = inventorySlotsField.pawInventory;
                slotIcons = new Dictionary<int, GameObject>();

                if (inventory == null)
                    return;

                for (int i = 0; i < inventory.slotButton.Count; i++)
                {
                    UIPartActionInventorySlot inventorySlot = inventory.slotButton[i];
                    UpgradeInfo upgradeInfo = upgradeInfos.Where(u => u.index.Equals(inventorySlot.slotIndex)).FirstOrDefault();

                    if (!this.IsSlotEmpty(i))
                        continue;

                    if (upgradeInfo.sprite != null)
                    {
                        Transform imageTransform = inventorySlot.transform.gameObject.GetComponentsInChildren<Transform>().Where(t => t.name.Equals("ButtonImage")).FirstOrDefault();

                        GameObject imageObject = Instantiate(imageTransform.gameObject, imageTransform.parent);
                        Image image = imageObject.GetComponent<Image>();
                        image.sprite = upgradeInfo.sprite;

                        slotIcons.Add(upgradeInfo.index, imageObject);
                    }
                }
            }
        }

        public void UpdatePAW()
        {
            // Don't reload the PAW if we picked up a Cargo Part. Otherwise the part will vanish
            if (UIPartActionControllerInventory.Instance.CurrentModuleCargoPart != null)
                return;

            UIPartActionWindow window = UIPartActionController.Instance?.GetItem(part, false);

            if (window == null)
                return;

            window.ClearList();
            window.RecyclePartList();
        }

        public override string GetInfo()
        {
            string txt = $"{description} \n\nUpgrade Slots: {this.upgradeSlots} \nVolume Limit: {this.inventorySize}";
            return txt;
        }

        public override string GetModuleDisplayName()
        {
            string txt = "Inventory Upgrade Part";
            return txt;
        }

        public new void OnDestroy()
        {
            GameEvents.onModuleInventoryChanged.Remove(OnModuleInventoryChanged);
            GameEvents.onPartActionUIShown.Remove(OnPawUIShown);
            GameEvents.onFlightReady.Remove(OnFlightReady);
            GameEvents.onAboutToSaveShip.Remove(BeforeSave);
            GameEvents.onEditorPartPlaced.Remove(OnEditorPartPlaced);
            GameEvents.onEditorShipModified.Remove(OnEditorShipModified);
            GameEvents.onEditorPartDeleted.Remove(OnEditorPartDeleted);
            // GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
        }
    }
}
