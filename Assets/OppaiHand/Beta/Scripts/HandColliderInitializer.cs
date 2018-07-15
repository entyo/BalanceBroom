﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mebiustos.OppaiHand.Beta {
    public class HandColliderInitializer : MonoBehaviour {
        public bool IsInisializedDestroy = false;

        public bool colliderInitEnabled = false;

        public bool fingerTriggerAndKinematic = true;
        public bool palmTriggerAndKinematic = false;
        public bool forearmTriggerAndKinematic = true;
        public bool wristjointTriggerAndKinematic = true;
        public bool elbowjointTriggerAndKinematic = true;

        [System.NonSerialized]
        public Collider[] _colliders;
        [System.NonSerialized]
        public Collider[] _collidersWithoutPalm;

        void Awake() {
            SetCollider();
            if (this.IsInisializedDestroy)
                Destroy(this);
        }

        void SetCollider() {
            var rigidHand = this.GetComponent<RigidHand>();

            List<Collider> colliderList = new List<Collider>();
            List<Collider> colliderListWithoutPalm = new List<Collider>();

            // finger
            foreach (var finger in rigidHand.fingers) {
                if (finger) {
                    var cols = finger.GetComponentsInChildren<Collider>();
                    foreach (var fingercol in cols) {
                        fingercol.enabled = this.colliderInitEnabled;
                        fingercol.isTrigger = this.fingerTriggerAndKinematic;

                        var fingerRigid = fingercol.GetComponent<Rigidbody>();
                        if (fingerRigid != null)
                            fingerRigid.isKinematic = this.fingerTriggerAndKinematic;
                    }
                    colliderList.AddRange(cols);
                    colliderListWithoutPalm.AddRange(cols);
                }
            }

            Collider col;
            Rigidbody rig;

            // palm
            if (rigidHand.palm) {
                col = rigidHand.palm.GetComponent<Collider>();
                col.enabled = this.colliderInitEnabled;
                col.isTrigger = this.palmTriggerAndKinematic;
                colliderList.Add(col);
                rig = col.GetComponent<Rigidbody>();
                if (rig != null)
                    rig.isKinematic = this.palmTriggerAndKinematic;
            }

            // forearm
            if (rigidHand.forearm) {
                col = rigidHand.forearm.GetComponent<Collider>();
                col.enabled = this.colliderInitEnabled;
                col.isTrigger = this.forearmTriggerAndKinematic;
                colliderList.Add(col);
                colliderListWithoutPalm.Add(col);
                rig = col.GetComponent<Rigidbody>();
                if (rig != null)
                    rig.isKinematic = this.forearmTriggerAndKinematic;
            }

            // wristjoint
            if (rigidHand.wristJoint) {
                col = rigidHand.wristJoint.GetComponent<Collider>();
                col.enabled = this.colliderInitEnabled;
                col.isTrigger = this.wristjointTriggerAndKinematic;
                colliderList.Add(col);
                colliderListWithoutPalm.Add(col);
                rig = col.GetComponent<Rigidbody>();
                if (rig != null)
                    rig.isKinematic = this.wristjointTriggerAndKinematic;
            }

            // elbowJoint
            if (rigidHand.elbowJoint) {
                col = rigidHand.elbowJoint.GetComponent<Collider>();
                col.enabled = this.colliderInitEnabled;
                col.isTrigger = this.elbowjointTriggerAndKinematic;
                colliderList.Add(col);
                colliderListWithoutPalm.Add(col);
                rig = col.GetComponent<Rigidbody>();
                if (rig != null)
                    rig.isKinematic = this.elbowjointTriggerAndKinematic;
            }

            this._colliders = colliderList.ToArray();
            this._collidersWithoutPalm = colliderListWithoutPalm.ToArray();
        }

        public void SetAllEnabled(bool enabled) {
            for (int i = 0; i < this._colliders.Length; i++) {
                this._colliders[i].enabled = enabled;
            }
        }

        public void SetAllEnabledWithoutPalm(bool enabled) {
            for (int i = 0; i < this._collidersWithoutPalm.Length; i++) {
                this._collidersWithoutPalm[i].enabled = enabled;
            }
        }

        public void SetAllTrigger(bool isTrigger) {
            for (int i = 0; i < this._colliders.Length; i++) {
                this._colliders[i].isTrigger = isTrigger;
            }
        }
    }
}