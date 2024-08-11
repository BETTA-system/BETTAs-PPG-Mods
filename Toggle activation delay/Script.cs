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
                PhysicalBehaviour[] pbs = args.Instance.GetComponentsInChildren<PhysicalBehaviour>();

                foreach (PhysicalBehaviour pb in pbs)
                {
                    ContextMenuButton[] Buttons = {
                        new ContextMenuButton("setActivationDelayToZero", "Set Activation Delay to 0", "Set the activation delay of the object to 0", new UnityAction[]
                        {
                            delegate()
                            {
                                pb.ActivationPropagationDelay = 0f;

                                string objectName = pb.gameObject.name;

                                ModAPI.Notify($"Activation delay set to 0 for {objectName}");
                            }
                        })
                        {
                            LabelWhenMultipleAreSelected = "Set Activation Delay to 0"
                        },
                        new ContextMenuButton("setActivationDelayToDefault", "Set Activation Delay to Default", "Set the activation delay of the object to default", new UnityAction[]
                        {
                            delegate()
                            {
                                pb.ActivationPropagationDelay = 0.1f;

                                string objectName = pb.gameObject.name;

                                ModAPI.Notify($"Activation delay set to default for {objectName}");
                            }
                        })
                        {
                            LabelWhenMultipleAreSelected = "Set Activation Delay to Default"
                        }
                    };

                    pb.ContextMenuOptions.Buttons.AddRange(Buttons);
                }
            };
        }
    }
}

//If you want just 1 button for toggling between 0 and default you can use this code instead (just remove /* and */ at start and end, also delete the code above):

/*using System;
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
                PhysicalBehaviour[] pbs = args.Instance.GetComponentsInChildren<PhysicalBehaviour>();

                foreach (PhysicalBehaviour pb in pbs)
                {
                    ContextMenuButton[] Buttons = {
                        new ContextMenuButton("toggleActivationDelay", "Toggle Activation Delay", "Toggle the activation delay of the object", new UnityAction[]
                        {
                            delegate()
                            {
                                float newDelay = Mathf.Approximately(pb.ActivationPropagationDelay, 0.1f) ? 0f : 0.1f;
                                pb.ActivationPropagationDelay = newDelay;

                                ModAPI.Notify("Toggled activation delay");
                            }
                        })
                        {
                            LabelWhenMultipleAreSelected = "Toggle Activation Delay"
                        }
                    };

                    pb.ContextMenuOptions.Buttons.AddRange(Buttons);
                }
            };
        }
    }
}
*/
