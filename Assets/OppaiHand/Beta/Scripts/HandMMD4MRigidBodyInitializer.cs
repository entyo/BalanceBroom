﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mebiustos.OppaiHand.Beta {
    public class HandMMD4MRigidBodyInitializer : MonoBehaviour {
        public bool IsInisializedDestroy = false;

        public bool MMD4MecanimRigidBodyInitEnabled = true;

        public bool fingerIsKinematic = true;
        public bool palmIsKinematic = true;
        public bool forearmIsKinematic = true;
        public bool wristjointIsKinematic = true;
        public bool elbowjointIsKinematic = true;

        [System.NonSerialized]
        public MMD4MecanimRigidBody[] _mmd4mecanimRigidBodys;
        [System.NonSerialized]
        public MMD4MecanimRigidBody[] _mmd4mecanimRigidBodysWithoutPalm;

        void Awake() {
            CreateAll();
            if (this.IsInisializedDestroy)
                Destroy(this);
        }

        void CreateAll() {
            var rigidHand = this.GetComponent<RigidHand>();

            List<MMD4MecanimRigidBody> rbodyList = new List<MMD4MecanimRigidBody>();
            List<MMD4MecanimRigidBody> rbodyListWithoutPalm = new List<MMD4MecanimRigidBody>();

            // finger
            MMD4MecanimRigidBody rigid;
            foreach (var finger in rigidHand.fingers) {
                if (finger) {
                    var cols = finger.GetComponentsInChildren<Collider>();
                    foreach (var fingercol in cols) {
                        rigid = fingercol.gameObject.AddComponent<MMD4MecanimRigidBody>();
                        rigid.enabled = this.MMD4MecanimRigidBodyInitEnabled;
                        rigid.bulletPhysicsRigidBodyProperty = new MMD4MecanimInternal.Bullet.RigidBodyProperty();
                        rigid.bulletPhysicsRigidBodyProperty.isKinematic = this.fingerIsKinematic;
                        rbodyList.Add(rigid);
                        rbodyListWithoutPalm.Add(rigid);
                    }
                }
            }

            //Collider col;

            // palm
            if (rigidHand.palm) {
                //col = rigidHand.palm.GetComponent<Collider>();
                rigid = rigidHand.palm.gameObject.AddComponent<MMD4MecanimRigidBody>();
                rigid.enabled = this.MMD4MecanimRigidBodyInitEnabled;
                rigid.bulletPhysicsRigidBodyProperty = new MMD4MecanimInternal.Bullet.RigidBodyProperty();
                rigid.bulletPhysicsRigidBodyProperty.isKinematic = this.palmIsKinematic;
                rbodyList.Add(rigid);
            }

            // forearm
            if (rigidHand.forearm) {
                //col = rigidHand.forearm.GetComponent<Collider>();
                rigid = rigidHand.forearm.gameObject.AddComponent<MMD4MecanimRigidBody>();
                rigid.enabled = this.MMD4MecanimRigidBodyInitEnabled;
                rigid.bulletPhysicsRigidBodyProperty = new MMD4MecanimInternal.Bullet.RigidBodyProperty();
                rigid.bulletPhysicsRigidBodyProperty.isKinematic = this.forearmIsKinematic;
                rbodyList.Add(rigid);
                rbodyListWithoutPalm.Add(rigid);
            }

            // wristjoint
            if (rigidHand.wristJoint) {
                //col = rigidHand.wristJoint.GetComponent<Collider>();
                rigid = rigidHand.wristJoint.gameObject.AddComponent<MMD4MecanimRigidBody>();
                rigid.enabled = this.MMD4MecanimRigidBodyInitEnabled;
                rigid.bulletPhysicsRigidBodyProperty = new MMD4MecanimInternal.Bullet.RigidBodyProperty();
                rigid.bulletPhysicsRigidBodyProperty.isKinematic = this.wristjointIsKinematic;
                rbodyList.Add(rigid);
                rbodyListWithoutPalm.Add(rigid);
            }

            // elbowJoint
            if (rigidHand.elbowJoint) {
                //col = rigidHand.elbowJoint.GetComponent<Collider>();
                rigid = rigidHand.elbowJoint.gameObject.AddComponent<MMD4MecanimRigidBody>();
                rigid.enabled = this.MMD4MecanimRigidBodyInitEnabled;
                rigid.bulletPhysicsRigidBodyProperty = new MMD4MecanimInternal.Bullet.RigidBodyProperty();
                rigid.bulletPhysicsRigidBodyProperty.isKinematic = this.elbowjointIsKinematic;
                rbodyList.Add(rigid);
                rbodyListWithoutPalm.Add(rigid);
            }

            this._mmd4mecanimRigidBodys = rbodyList.ToArray();
            this._mmd4mecanimRigidBodysWithoutPalm = rbodyListWithoutPalm.ToArray();
        }

        public void SetAllEnabled(bool enabled) {
            for (int i = 0; i < this._mmd4mecanimRigidBodys.Length; i++) {
                this._mmd4mecanimRigidBodys[i].enabled = enabled;
            }
        }

        public void SetAllKinematic(bool isKinematic) {
            for (int i = 0; i < this._mmd4mecanimRigidBodys.Length; i++) {
                this._mmd4mecanimRigidBodys[i].bulletPhysicsRigidBodyProperty.isKinematic = isKinematic;
            }
        }

        public void SetAllKinematicWithoutPalm(bool isKinematic) {
            for (int i = 0; i < this._mmd4mecanimRigidBodysWithoutPalm.Length; i++) {
                this._mmd4mecanimRigidBodysWithoutPalm[i].bulletPhysicsRigidBodyProperty.isKinematic = isKinematic;
            }
        }

        public void SetAllFreezedWithoutPalm(bool isFreezed) {
            for (int i = 0; i < this._mmd4mecanimRigidBodysWithoutPalm.Length; i++) {
                this._mmd4mecanimRigidBodysWithoutPalm[i].bulletPhysicsRigidBodyProperty.isFreezed = isFreezed;
            }
        }
    }
}