using System;
using System.Linq;
using NitroxClient.Communication.Abstract;
using NitroxClient.MonoBehaviours;
using NitroxModel.DataStructures;
using NitroxModel.Helper;
using NitroxModel.Logger;
using NitroxModel.Subnautica.DataStructures.GameLogic;
using NitroxModel.Subnautica.Packets;
using UnityEngine;

namespace NitroxClient.GameLogic
{
    public class ExosuitModuleEvent
    {
        private static readonly int animatorUseTool = Animator.StringToHash("use_tool");
        private static readonly int animatorBash = Animator.StringToHash("bash");

        private readonly IPacketSender packetSender;
        private readonly Vehicles vehicles;

        public ExosuitModuleEvent(IPacketSender packetSender, Vehicles vehicles)
        {
            this.packetSender = packetSender;
            this.vehicles = vehicles;
        }

        public void SpawnedArm(Exosuit exosuit)
        {
            NitroxId id = NitroxEntity.GetId(exosuit.gameObject);
            ExosuitModel exosuitModel = vehicles.GetVehicles<ExosuitModel>(id);

            IExosuitArm rightArm = (IExosuitArm)exosuit.ReflectionGet("rightArm");
            IExosuitArm leftArm = (IExosuitArm)exosuit.ReflectionGet("leftArm");

            try
            {
                GameObject rightArmGameObject = rightArm.GetGameObject();
                NitroxEntity.SetNewId(rightArmGameObject, exosuitModel.RightArmId);

                GameObject leftArmGameObject = leftArm.GetGameObject();
                NitroxEntity.SetNewId(leftArmGameObject, exosuitModel.LeftArmId);
            }
            catch (Exception ex)
            {
                Log.Warn($"Got error setting arm GameObjects. This is probably due to docking sync and can be ignored. {ex.Message}");
            }

            Log.Debug($"Spawn exosuit arms for: {id}");
        }

        public void BroadcastArmAction(TechType techType, IExosuitArm exosuitArm, ExosuitArmAction armAction, Vector3? opVector = null, Quaternion? opRotation = null)
        {
            NitroxId id = NitroxEntity.GetId(exosuitArm.GetGameObject());
            ExosuitArmActionPacket packet = new ExosuitArmActionPacket(techType, id, armAction, opVector, opRotation);
            packetSender.Send(packet);
        }

        public void BroadcastClawUse(ExosuitClawArm clawArm, float cooldown)
        {
            ExosuitArmAction action;

            // If cooldown of claw arm matches pickup cooldown, the exosuit arm performed a pickup action
            if (Math.Abs(cooldown - clawArm.cooldownPickup) < 0.05f)
            {
                action = ExosuitArmAction.START_USE_TOOL;
            } // Else if it matches the punch cooldown, it has punched something (or nothing but water, who knows)
            else if (Math.Abs(cooldown - clawArm.cooldownPunch) < 0.05f)
            {
                action = ExosuitArmAction.ALT_HIT;
            }
            else
            {
                Log.Error("Cooldown time does not match pickup or punch time");
                return;
            }
            BroadcastArmAction(TechType.ExosuitClawArmModule, clawArm, action, null, null);
        }

        public static void UseClaw(ExosuitClawArm clawArm, ExosuitArmAction armAction)
        {
            switch (armAction)
            {
                case ExosuitArmAction.START_USE_TOOL:
                    clawArm.animator.SetTrigger(animatorUseTool);
                    break;
                case ExosuitArmAction.ALT_HIT:
                    clawArm.animator.SetTrigger(animatorBash);
                    clawArm.fxControl.Play(0);
                    break;
            }
        }

        public static void UseDrill(ExosuitDrillArm drillArm, ExosuitArmAction armAction)
        {
            switch (armAction)
            {
                case ExosuitArmAction.START_USE_TOOL:
                    drillArm.animator.SetBool(animatorUseTool, true);
                    drillArm.loop.Play();
                    break;
                case ExosuitArmAction.END_USE_TOOL:
                    drillArm.animator.SetBool(animatorUseTool, false);
                    drillArm.ReflectionCall("StopEffects");
                    break;
                default:
                    Log.Error($"Drill arm got an arm action he should not get: {armAction}");
                    break;
            }
        }

        public static void UseGrappling(ExosuitGrapplingArm grapplingArm, ExosuitArmAction armAction, Vector3? opHitVector)
        {
            switch (armAction)
            {
                case ExosuitArmAction.END_USE_TOOL:
                    grapplingArm.animator.SetBool(animatorUseTool, false);
                    grapplingArm.ReflectionCall("ResetHook");
                    break;
                case ExosuitArmAction.START_USE_TOOL:
                    {
                        grapplingArm.animator.SetBool(animatorUseTool, true);
                        if (!grapplingArm.rope.isLaunching)
                        {
                            grapplingArm.rope.LaunchHook(35f);
                        }

                        GrapplingHook hook = (GrapplingHook)grapplingArm.ReflectionGet("hook");

                        hook.transform.parent = null;
                        hook.transform.position = grapplingArm.front.transform.position;
                        hook.SetFlying(true);
                        Exosuit componentInParent = grapplingArm.GetComponentInParent<Exosuit>();

                        if (!opHitVector.HasValue)
                        {
                            Log.Error("No vector given that contains the hook direction");
                            return;
                        }

                        hook.rb.velocity = opHitVector.Value;
                        Utils.PlayFMODAsset(grapplingArm.shootSound, grapplingArm.front, 15f);
                        grapplingArm.ReflectionSet("grapplingStartPos", componentInParent.transform.position);
                        break;
                    }
                default:
                    Log.Error($"Grappling arm got an arm action he should not get: {armAction}");
                    break;
            }
        }

        public static void UseTorpedo(ExosuitTorpedoArm torpedoArm, ExosuitArmAction armAction, Vector3? opVector, Quaternion? opRotation)
        {
            switch (armAction)
            {
                case ExosuitArmAction.START_USE_TOOL:
                case ExosuitArmAction.ALT_HIT:
                    {
                        if (!opVector.HasValue || !opRotation.HasValue)
                        {
                            Log.Error("Torpedo arm action shoot: no vector or rotation present");
                            return;
                        }
                        Vector3 forward = opVector.Value;
                        Quaternion rotation = opRotation.Value;
                        Transform silo = armAction == ExosuitArmAction.START_USE_TOOL ? torpedoArm.siloFirst : torpedoArm.siloSecond;
                        ItemsContainer container = (ItemsContainer)torpedoArm.ReflectionGet("container");
                        Exosuit exosuit = torpedoArm.GetComponentInParent<Exosuit>();
                        TorpedoType[] torpedoTypes = exosuit.torpedoTypes;

                        TorpedoType torpedoType = torpedoTypes.FirstOrDefault(type => container.Contains(type.techType));

                        // Copied from SeamothModuleActionProcessor. We need to synchronize both methods
                        GameObject gameObject = UnityEngine.Object.Instantiate(torpedoType.prefab);
                        SeamothTorpedo component2 = gameObject.GetComponent<SeamothTorpedo>();
                        Rigidbody componentInParent = silo.GetComponentInParent<Rigidbody>();
                        Vector3 rhs = !componentInParent ? Vector3.zero : componentInParent.velocity;
                        float speed = Vector3.Dot(forward, rhs);
                        component2.Shoot(silo.position, rotation, speed, -1f);

                        torpedoArm.animator.SetBool(animatorUseTool, true);
                        if (container.count == 0)
                        {
                            Utils.PlayFMODAsset(torpedoArm.torpedoDisarmed, torpedoArm.transform, 1f);
                        }

                        break;
                    }
                case ExosuitArmAction.END_USE_TOOL:
                    torpedoArm.animator.SetBool(animatorUseTool, false);
                    break;
                default:
                    Log.Error($"Torpedo arm got an arm action he should not get: {armAction}");
                    break;
            }
        }

    }
}
