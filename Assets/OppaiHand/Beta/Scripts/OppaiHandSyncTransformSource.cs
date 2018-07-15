﻿using UnityEngine;
using System.Collections;

namespace Mebiustos.OppaiHand.Beta {
    public class OppaiHandSyncTransformSource : MonoBehaviour {

        public OppaiHand syncDestination;

        void OnCollisionStay(Collision collisionInfo) {
            //Debug.Log("OnCollisionStay");
            syncDestination.OnGhostSourceTouchStay();
        }

        void OnTriggerStay(Collider collider) {
            //Debug.Log("OnTriggerStay : " + collider.gameObject.name);
            syncDestination.OnGhostSourceTouchStay();
        }
    }
}
