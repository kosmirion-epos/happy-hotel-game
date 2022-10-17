using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine.Serialization;

namespace Autohand {
    [DefaultExecutionOrder(-2)]
    public class GrabbableBase : MonoBehaviour{

        [AutoHeader("Grabbable")]
        public bool ignoreMe;

        [Tooltip("The physics body to connect this colliders grab to - if left empty will default to local body")]
        public Rigidbody body;

        [Tooltip("A copy of the mesh will be created and slighly scaled and this material will be applied to create a highlight effect with options")]
        public Material hightlightMaterial;

        [HideInInspector]
        public bool isGrabbable = true;

        private PlacePoint _placePoint = null;
        public PlacePoint placePoint { get { return _placePoint; } protected set { _placePoint = value; } }

        internal bool ignoreParent = false;

        protected List<Hand> heldBy = new List<Hand>();
        protected bool hightlighting;
        protected GameObject highlightObj;
        protected PlacePoint lastPlacePoint = null;

        protected Transform originalParent;
        protected Vector3 lastCenterOfMassPos;
        protected Quaternion lastCenterOfMassRot;
        protected CollisionDetectionMode detectionMode;
        protected RigidbodyInterpolation startInterpolation;

        protected internal bool beingGrabbed = false;
        protected bool heldBodyJointed = false;
        protected bool wasIsGrabbable = false;
        protected bool beingDestroyed = false;
        protected int originalLayer;
        protected Coroutine resetLayerRoutine = null;
        protected List<GrabbableChild> grabChildren = new List<GrabbableChild>();
        public IReadOnlyList<GrabbableChild> GrabChildren => grabChildren;
        protected List<Transform> jointedParents = new List<Transform>();
        protected GrabbablePoseCombiner poseCombiner;


        private CollisionTracker _collisionTracker;
        public CollisionTracker collisionTracker {
            get {
                if(_collisionTracker == null) {
                    _collisionTracker = gameObject.AddComponent<CollisionTracker>();
                    _collisionTracker.disableTriggersTracking = true;
                }
                return _collisionTracker;
            }
            protected set {
                if(_collisionTracker != null)
                    Destroy(_collisionTracker);

                _collisionTracker = value;
            }
        }

#if UNITY_EDITOR
        bool editorSelected = false;
    #endif

        protected virtual void Awake() {
            //Delete these layer setters if you want to use your own custom layer set
            if(gameObject.layer == LayerMask.NameToLayer("Default") || LayerMask.LayerToName(gameObject.layer) == "")
                gameObject.layer = LayerMask.NameToLayer(Hand.grabbableLayerNameDefault);
            
            if(heldBy == null)
                heldBy = new List<Hand>();

            if(body == null){
                if(GetComponent<Rigidbody>())
                    body = GetComponent<Rigidbody>();
                else
                    Debug.LogError("RIGIDBODY MISSING FROM GRABBABLE: " + transform.name + " \nPlease add/attach a rigidbody", this);
            }


    #if UNITY_EDITOR
            if (Selection.activeGameObject == gameObject){
                Selection.activeGameObject = null;
                Debug.Log("Auto Hand: Selecting the grabbable can cause lag and quality reduction at runtime. (Automatically deselecting at runtime) Remove this code at any time.", this);
                editorSelected = true;
            }

            Application.quitting += () => { if (editorSelected) Selection.activeGameObject = gameObject; };
    #endif

            originalLayer = gameObject.layer;
            originalParent = body.transform.parent;
            detectionMode = body.collisionDetectionMode;
            startInterpolation = body.interpolation;
            SetCollidersRecursive(body.transform);
        }

        protected virtual void Start() {
            if(!gameObject.CanGetComponent(out poseCombiner))
                poseCombiner = gameObject.AddComponent<GrabbablePoseCombiner>();

            GetPoseSaves(transform);

            void GetPoseSaves(Transform obj) {
                //Stop if you get to another grabbable
                if(obj.CanGetComponent(out Grabbable grab) && grab != this)
                    return;

                var poses = obj.GetComponents<GrabbablePose>();
                for(int i = 0; i < poses.Length; i++)
                    poseCombiner.AddPose(poses[i]);

                for(int i = 0; i < obj.childCount; i++)
                    GetPoseSaves(obj.GetChild(i));
            }
        }

        protected virtual void FixedUpdate() {
            if(heldBy.Count > 0) {
                lastCenterOfMassRot = body.transform.rotation;
                lastCenterOfMassPos = body.transform.position;
            }
        }


        protected virtual void OnDisable(){
            if (resetLayerRoutine != null){
                StopCoroutine(resetLayerRoutine);
                resetLayerRoutine = null;
            }
        }
        

        
        internal void SetPlacePoint(PlacePoint point) {
            this.placePoint = point;
        }

        internal void SetGrabbableChild(GrabbableChild child) {
            if(!grabChildren.Contains(child))
                grabChildren.Add(child);
        }
        




        protected int GetOriginalLayer(){
            return originalLayer;
        }

        
        internal void SetLayerRecursive(Transform obj, int oldLayer, int newLayer) {
            for(int i = 0; i < grabChildren.Count; i++) {
                if(grabChildren[i].gameObject.layer == oldLayer)
                    grabChildren[i].gameObject.layer = newLayer;
            }
            SetChildrenLayers(obj);

            void SetChildrenLayers(Transform obj1){
                if(obj1.gameObject.layer == oldLayer)
                    obj1.gameObject.layer = newLayer;
                for(int i = 0; i < obj1.childCount; i++) {
                    SetChildrenLayers(obj1.GetChild(i));
                }
            }
        }

        internal void SetLayerRecursive(Transform obj, int newLayer) {
            SetLayerRecursive(obj, obj.gameObject.layer, newLayer);
        }


        //Invoked a quatersecond after releasing
        protected IEnumerator IgnoreHandCollision(float time, Hand hand) {
            IgnoreHand(hand, true);

            yield return new WaitForSeconds(time);

            IgnoreHand(hand, false);

            resetLayerRoutine = null;

        }

        public bool GetSavedPose(out GrabbablePoseCombiner pose) {
            if(poseCombiner.PoseCount() > 0) {
                pose = poseCombiner;
                return true;
            }
            else {
                pose = null;
                return false;
            }
        }

        public bool HasCustomPose() {
            return poseCombiner.PoseCount() > 0;
        }


        public void IgnoreHand(Hand hand, bool ignore)
        {
            foreach (var col in grabColliders)
                hand.HandIgnoreCollider(col, ignore);
        }

        protected List<Collider> grabColliders = new List<Collider>();
        void SetCollidersRecursive(Transform obj){
            foreach (var col in obj.GetComponents<Collider>())
                grabColliders.Add(col);

            for (int i = 0; i < obj.childCount; i++)
                SetCollidersRecursive(obj.GetChild(i));
        }
        
        //Resets to original collision dection
        protected void ResetRigidbody() {
            body.collisionDetectionMode = detectionMode;
            body.interpolation = startInterpolation;
        }

        public bool BeingDestroyed() {
            return beingDestroyed;
        }

        public void DebugBreak() {
#if UNITY_EDITOR
            Debug.Break();
#endif
        }


    }
}