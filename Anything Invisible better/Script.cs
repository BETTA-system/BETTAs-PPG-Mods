using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mod
{
    public class Mod
    {
        public static void Main()
        {
            ModAPI.OnItemSpawned += (sender, args) =>
            {
                // Exclude the lightbulb
                if (!args.Instance.TryGetComponent<BulbBehaviour>(out _))
                {
                    // Check if the object is a Person thing whatever 
                    if (args.Instance.TryGetComponent<PersonBehaviour>(out PersonBehaviour personBehaviour))
                    {
                        // Add buttons to each limb
                        foreach (var limb in personBehaviour.Limbs)
                        {
                            AddButtonsToPhysicalBehaviour(limb.GetComponent<PhysicalBehaviour>());
                        }
                    }
                    else
                    {
                        // Add buttons to the main object and each physical child object
                        AddButtonsToPhysicalBehaviour(args.Instance.GetComponent<PhysicalBehaviour>());
                        foreach (Transform child in args.Instance.transform)
                        {
                            AddButtonsToPhysicalBehaviour(child.GetComponent<PhysicalBehaviour>());
                        }
                        
                        CheckChildrenForInvisibleOption(args.Instance.transform);
                    }
                }
            };
        }

        // Check children for improperly added components and remove them
        private static void CheckChildrenForInvisibleOption(Transform parent)
        {
            foreach (Transform child in parent)
            {
                PhysicalBehaviour childPhysicalBehaviour = child.GetComponent<PhysicalBehaviour>();
                if (childPhysicalBehaviour == null)
                {
                    // Check for component in the current child
                    if (child.TryGetComponent<InvisibleOptionInContextMenu>(out InvisibleOptionInContextMenu invisOption))
                    {
                        ModAPI.Notify($"<color=green>{parent.name} had improperly added components and was automatically fixed");
                        RemoveInvisibleOption(child.gameObject);
                        break;
                    }

                    // Check grandchildren and great-grandchildren
                    CheckChildrenForInvisibleOption(child);
                }
            }
        }

        // Add context menu buttons to physical objects
        private static void AddButtonsToPhysicalBehaviour(PhysicalBehaviour phys)
        {
            if (phys != null)
            {
                // Button to add invisible option
                phys.ContextMenuOptions.Buttons.Add
                (
                    new ContextMenuButton("AddInvisOption", "Add Invisible Option", "Adds the Invisible Option (ToggleinvisibilityinDetailview component) to the object", () =>
                    {
                        AddInvisibleOption(phys.gameObject);
                        ModAPI.Notify($"Invisible Option added to {phys.gameObject.name}");
                    })
                    {
                        LabelWhenMultipleAreSelected = "Add Invisible Option"
                    }
                );

                // Button to remove invisible option
                phys.ContextMenuOptions.Buttons.Add
                (
                    new ContextMenuButton("RemoveInvisOption", "Remove Invisible Option", "Removes the Invisible Option (ToggleinvisibilityinDetailview component) from the object and its children", () =>
                    {
                        RemoveInvisibleOption(phys.gameObject);

                        foreach (Transform child in phys.gameObject.transform)
                        {
                            RemoveInvisibleOption(child.gameObject);
                        }

                        ModAPI.Notify($"Invisible Option removed from {phys.gameObject.name}. <color=orange>Copy-paste this object to update it");
                    })
                    {
                        LabelWhenMultipleAreSelected = "Remove Invisible Option"
                    }
                );

                // Button to toggle visibility in detail view
                phys.ContextMenuOptions.Buttons.Add
                (
                    new ContextMenuButton("ToggleInvisibilityInDetailView", "Toggle Invisibility in Detail View", "Makes the object visible or invisible in detail view", () =>
                    {
                        InvisibleOptionInContextMenu invisOptionComponent = phys.gameObject.GetComponent<InvisibleOptionInContextMenu>();
                        if (invisOptionComponent != null)
                        {
                            bool newVisibilityState = !invisOptionComponent.VisibleInDetailView;
                            invisOptionComponent.VisibleInDetailView = newVisibilityState;
                            ModAPI.Notify($"{phys.gameObject.name} is now {(newVisibilityState ? "visible" : "invisible")} in detail view");
                        }
                    })
                    {
                        LabelWhenMultipleAreSelected = "Toggle Invisibility in Detail View"
                    }
                );
            }
        }

        // Logic for removing
        private static void RemoveInvisibleOption(GameObject gameObject)
        {
            InvisibleOptionInContextMenu invisOption = gameObject.GetComponent<InvisibleOptionInContextMenu>();
            if (invisOption != null)
            {
                GameObject.Destroy(invisOption);
            }
        }

        // Logic for adding
        private static void AddInvisibleOption(GameObject gameObject)
        {
            InvisibleOptionInContextMenu newInvisOption = gameObject.GetOrAddComponent<InvisibleOptionInContextMenu>();
            newInvisOption.Invisible = true;
        }
    }
}
