﻿using System;
using UnityEngine;
using ProtoBufNet;
using System.Collections.Generic;

namespace NitroxModel.DataStructures.GameLogic
{
    [ProtoContract]
    [Serializable]
    public class NitroxTransform
    {
        [ProtoMember(1)]
        public NitroxVector3 LocalPosition;

        [ProtoMember(2)]
        public NitroxQuaternion LocalRotation;
        
        [ProtoMember(3)]
        public NitroxVector3 LocalScale;

        public NitroxMatrix4x4 localToWorldMatrix 
        {
            get
            {
                NitroxMatrix4x4 localMatrix = NitroxMatrix4x4.TRS(LocalPosition, LocalRotation, LocalScale);
                return Parent != null ? localMatrix * Parent.localToWorldMatrix : localMatrix;
            }
        }

        public NitroxTransform Parent;
        public Entity Entity;
        public NitroxVector3 Position
        {
            get 
            {
                NitroxMatrix4x4 matrix = Parent != null ? Parent.localToWorldMatrix : NitroxMatrix4x4.Identity;
                return matrix.MultiplyPoint(LocalPosition);
            }
            set
            {
                NitroxVector3 _ = value;
            }
        }
        public NitroxQuaternion Rotation
        {
            get
            {
                NitroxMatrix4x4 matrix = localToWorldMatrix;
                NitroxMatrix4x4.ExtractScale(ref matrix); // This is to just get the scale out of the matrix so the rotation is accurate
                return NitroxMatrix4x4.ExtractRotation(ref matrix);
            }
            set
            {
                NitroxQuaternion _ = value;
            }
        }

        public void SetParent(NitroxTransform parent)
        {
            Parent = parent;
            
            
        }

        public void SetParent(NitroxTransform parent, bool worldPositionStays)
        {
            throw new NotImplementedException("This is not Implementwaed yet. Added by killzoms");
        }

        private NitroxTransform()
        {}

        /// <summary>
        /// NitroxTransform is always attached to an Entity
        /// </summary>
        public NitroxTransform(NitroxVector3 localPosition, NitroxQuaternion localRotation, NitroxVector3 scale, Entity entity)
        {
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            LocalScale = scale;
            Entity = entity;
        }

        public override string ToString()
        {
            return string.Format("(Position: {0}, LocalPosition: {1}, Rotation: {2}, LocalRotation: {3}, LocalScale: {4})", Position, LocalPosition, Rotation, LocalRotation, LocalScale);
        }
    }
}
