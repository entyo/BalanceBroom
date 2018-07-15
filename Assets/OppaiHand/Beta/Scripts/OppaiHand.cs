﻿using UnityEngine;
using System.Collections;
using Leap;

namespace Mebiustos.OppaiHand.Beta {
    /// <summary>
    /// 接触時のハンド制御
    /// </summary>
    public class OppaiHand : MonoBehaviour {
        public RigidHand rigidHand;
        public Transform PalmTransform;
        public Transform SlerpPalmTransform;

        public GhostRigidHandSynchronizer GhostRigidHandSync;

        //public Transform Hand1;
        //public Transform Hand2;

        [Tooltip("連続何フレームで接触解除とみなすか")]
        public int TouchReleaseCount = 2;
        [Tooltip("接触時のSlerpスピード値")]
        public float SlerpBetweenTouched = 10f;
        [Tooltip("非接触時のSlerpスピード値")]
        public float SlerpBetweenReleased = 20f;

        [Tooltip("接触時の手のひら方向への調整量。マイナス値で沈む。プラス値で浮く。")]
        public float DepthGraphicsHand = -0.01f;
        [Tooltip("離れすぎ強制同期させる距離")]
        public float ForceSyncPositonDistance = 0.20f;

        HandController handCtrler;
        [System.NonSerialized]
        public Transform graphicsHandTransform;
        Hand myHand;

        float slerpBetween;

        bool isTouching;
        bool isOnTouched;
        bool isOnReleased;
        int touchReleaseCnt;

        [System.NonSerialized]
        public bool isLeft;

        HandColliderInitializer handColliderInitializer;
        HandMMD4MRigidBodyInitializer handMMD4MRigidBodyInitializer;

        void Awake() {
            var syncTransformSource = this.PalmTransform.gameObject.AddComponent<OppaiHandSyncTransformSource>();
            syncTransformSource.syncDestination = this;

        }

        void Start() {
            this.handCtrler = GameObject.FindObjectOfType<HandController>();
            this.slerpBetween = SlerpBetweenReleased;

            this.handColliderInitializer = GetComponent<HandColliderInitializer>();
            this.handMMD4MRigidBodyInitializer = GetComponent<HandMMD4MRigidBodyInitializer>();
        }

        void Update() {
            UpdateTouchState();

            this.myHand = GetMyHand();
            if (this.myHand != null) {
                if (this.graphicsHandTransform == null) {
                    var find = FindGraphicsHand();
                    if (find) {
                        this.isLeft = this.myHand.IsLeft;
                        this.GhostRigidHandSync.ResyncPosition(this.isLeft);
                    }
                }

                MoveSlerpPalm();

                if (this.isOnTouched) {
                    //Debug.Log("isOnTouched");

                    this.handColliderInitializer.SetAllEnabledWithoutPalm(false);
                    if (this.handMMD4MRigidBodyInitializer != null) {
                        this.handMMD4MRigidBodyInitializer.SetAllKinematicWithoutPalm(false);
                        //this.handMMD4MRigidBodyInitializer.SetAllFreezedWithoutPalm(true);
                    }

                    this.GhostRigidHandSync.OnTouched();
                }
                if (this.isOnReleased) {
                    //Debug.Log("isOnReleased");

                    this.handColliderInitializer.SetAllEnabledWithoutPalm(true);
                    if (this.handMMD4MRigidBodyInitializer != null) {
                        this.handMMD4MRigidBodyInitializer.SetAllKinematicWithoutPalm(true);
                        //this.handMMD4MRigidBodyInitializer.SetAllFreezedWithoutPalm(false);
                    }

                    this.GhostRigidHandSync.OnReleased();
                }
            }
        }

        void LateUpdate() {
            if (this.graphicsHandTransform != null && this.isTouching) {
                if (this.myHand != null) {
                    ForceMoveGraphicsHand();
                }
            }
        }

        void UpdateTouchState() {
            bool before = this.isTouching;
            if (this.touchReleaseCnt > 0) {
                this.touchReleaseCnt--;
                this.isTouching = true;
            } else {
                this.isTouching = false;
            }

            this.isOnTouched = false;
            this.isOnReleased = false;
            if (before != this.isTouching) {
                if (this.isTouching) {
                    this.isOnTouched = true;
                } else {
                    this.isOnReleased = true;
                }
            }
        }

        void MoveSlerpPalm() {
            if (isTouching) {
                this.slerpBetween = SlerpBetweenTouched;
            } else {
                this.slerpBetween = SlerpBetweenReleased;
            }

            if (Vector3.Distance(this.rigidHand.palm.position, this.rigidHand.GetPalmPosition()) > this.ForceSyncPositonDistance) {
                // 離れすぎの為、強制同期
                this.PalmTransform.position = this.rigidHand.GetPalmPosition();
                this.SlerpPalmTransform.position = this.PalmTransform.position;

                this.PalmTransform.rotation = this.rigidHand.GetPalmRotation();
                this.SlerpPalmTransform.rotation = this.PalmTransform.rotation;
            } else {
                // 位置をスムース同期
                this.SlerpPalmTransform.position =
                    Vector3.Slerp(
                        this.SlerpPalmTransform.position,
                        this.PalmTransform.position,
                        Time.deltaTime * this.slerpBetween);

                // 回転をスムース同期
                this.SlerpPalmTransform.rotation =
                    Quaternion.Slerp(
                        this.SlerpPalmTransform.rotation,
                        this.PalmTransform.rotation,
                        Time.deltaTime * this.slerpBetween);
            }
        }

        void ForceMoveGraphicsHand() {
            var handContainer = this.graphicsHandTransform.GetChild(1);

            // グラフィクスハンド回転
            handContainer.rotation = this.SlerpPalmTransform.rotation;
            if (this.myHand.IsLeft) {
                handContainer.Rotate(new Vector3(0f, 270f, 0f));
            } else {
                handContainer.Rotate(new Vector3(180f, 90f, 0f));
            }

            // グラフィクスハンド位置
            var palmDirection = this.myHand.PalmNormal.ToUnity();
            palmDirection = new Vector3(palmDirection.x * -1, palmDirection.y, palmDirection.z * -1);
            handContainer.position = this.SlerpPalmTransform.position - palmDirection * DepthGraphicsHand;
        }

        Hand GetMyHand() {
            Frame frame = this.handCtrler.GetFrame();
            Hand myHand = null;
            if (frame != null) {
                var leapHand = this.rigidHand.GetLeapHand();
                foreach (var hand in frame.Hands) {
                    if (leapHand != null && leapHand.Id == hand.Id) {
                        myHand = hand;
                        break;
                    }
                }
            }
            return myHand;
        }

        bool FindGraphicsHand() {
            var hands = GameObject.FindObjectsOfType<RiggedHand>();
            foreach (var hand in hands) {
                var leapHand = hand.GetLeapHand();
                if (leapHand != null && leapHand.Id == this.rigidHand.GetLeapHand().Id) {
                    this.graphicsHandTransform = hand.transform;
                    return true;
                }
            }
            return false;
        }

        public void OnGhostSourceTouchStay() {
            //Debug.Log("OnTouchStay");
            this.touchReleaseCnt = TouchReleaseCount;
        }
    }
}