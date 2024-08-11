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
                        new ContextMenuButton("setActivationDelay", "Set Activation Delay", "Set activation delay in seconds", new UnityAction[]
                        {
                            delegate()
                            {
                                ShowDelayInputDialog(pb);
                            }
                        })
                        {
                            LabelWhenMultipleAreSelected = "Set Activation Delay"
                        }
                    };

                    pb.ContextMenuOptions.Buttons.AddRange(Buttons);
                }
            };
        }

        private static void ShowDelayInputDialog(PhysicalBehaviour pb)
        {
            float currentDelay = pb.ActivationPropagationDelay;

            Utils.OpenFloatInputDialog<PhysicalBehaviour>(currentDelay, pb, delegate(PhysicalBehaviour phys, float delay)
            {
                phys.ActivationPropagationDelay = delay;
                string objectName = phys.gameObject.name;
                ModAPI.Notify($"Activation delay set to {delay} for {objectName}");
            }, "Set Activation Delay", "delay in seconds");
        }
    }
}
