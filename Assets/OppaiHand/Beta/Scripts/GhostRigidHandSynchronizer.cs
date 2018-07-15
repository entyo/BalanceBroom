﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mebiustos.OppaiHand.Beta {
    public class GhostRigidHandSynchronizer : MonoBehaviour {
        public RigidHand parentRigidHand;
        public RigidHand childRigidHand;
        //public OppaiHand oppaiHand;
        public Transform baseTransform;
        public Transform myPalm;
       //public GameObject hand1;
       public GameObject hand2;

        Transform myTransform;

        Transform[] hand1ColliderTransforms;
        Transform[] hand2ColliderTransforms;

        HandColliderInitializer handColliderInitializer;
        HandMMD4MRigidBodyInitializer handMMD4MRigidBodyInitializer;

        void Start() {
            this.myTransform = this.transform;
            this.hand1ColliderTransforms = GetColliders(this.parentRigidHand);
            this.hand2ColliderTransforms = GetColliders(this.childRigidHand);

            this.handColliderInitializer = GetComponent<HandColliderInitializer>();
            this.handMMD4MRigidBodyInitializer = GetComponent<HandMMD4MRigidBodyInitializer>();

        }

        void Update() {
            hand2.transform.position = this.parentRigidHand.GetPalmPosition();
            hand2.transform.rotation = this.parentRigidHand.GetPalmRotation();

            //Debug.Log(this.hand1ColliderTransforms.Length + "/" + this.hand2ColliderTransforms.Length);

            for (int i = 0; i < this.hand1ColliderTransforms.Length; i++) {
                var tra1 = this.hand1ColliderTransforms[i];
                var tra2 = this.hand2ColliderTransforms[i];
                tra2.position = tra1.position;
                tra2.rotation = tra1.rotation;
                //if (hand1ColliderTransforms[i].parent.gameObject.name == "index" && hand1ColliderTransforms[i].gameObject.name == "bone1") {
                //    Debug.Log("instantid: " + hand1ColliderTransforms[i].GetInstanceID() + "    Update[" + i + "]" + tra1.position);
                //}
            }

            hand2.transform.position = this.myPalm.position;
            hand2.transform.rotation = this.myPalm.rotation;
        }

        void OnEnable() {
            this.hand2.SetActive(true);
        }

        void OnDisable() {
            this.hand2.SetActive(false);
        }

        public void OnTouched() {
            this.handColliderInitializer.SetAllEnabledWithoutPalm(true);
            if (this.handMMD4MRigidBodyInitializer != null) {
                GetComponent<HandMMD4MRigidBodyInitializer>().SetAllKinematicWithoutPalm(true);
                GetComponent<HandMMD4MRigidBodyInitializer>().SetAllFreezedWithoutPalm(false);
            }

            gameObject.SetActive(true);
        }

        public void OnReleased() {
            this.handColliderInitializer.SetAllEnabledWithoutPalm(false);
            if (this.handMMD4MRigidBodyInitializer != null) {
                GetComponent<HandMMD4MRigidBodyInitializer>().SetAllKinematicWithoutPalm(false);
                GetComponent<HandMMD4MRigidBodyInitializer>().SetAllFreezedWithoutPalm(true);
            }

            gameObject.SetActive(false);
        }

        public void ResyncPosition(bool isLeft) {
            var dis = baseTransform.position - myPalm.position;
            if (!isLeft)
                dis *= -1;
            this.myTransform.position += dis;
        }

        Transform[] GetColliders(RigidHand regidHand) {
            List<Transform> colliderList = new List<Transform>();

            // finger
            foreach (var finger in regidHand.fingers) {
                if (finger) {
                    var cols = finger.GetComponentsInChildren<Collider>();
                    foreach (var collider in cols)
                        colliderList.Add(collider.transform);
                }
            }
            return colliderList.ToArray();
        }
    }
}