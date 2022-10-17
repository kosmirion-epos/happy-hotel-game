using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autohand.Demo {
    public class ToggleHandProjection : MonoBehaviour {
        public void DisableGripProjection() {
            var projections = FindObjectsOfType<HandProjector>(true);
            foreach(var projection in projections) {
                if(projection.useGrabTransition)
                    projection.enabled = false;
            }
        }

        public void EnableGripProjection() {
            var projections = FindObjectsOfType<HandProjector>(true);
            foreach(var projection in projections) {
                if(projection.useGrabTransition)
                    projection.enabled = true;
            }
        }

        public void DisableHighlightProjection() {
            var projections = FindObjectsOfType<HandProjector>(true);
            foreach(var projection in projections) {
                if(!projection.useGrabTransition)
                    projection.enabled = false;
            }
        }

        public void EnableHighlightProjection() {
            var projections = FindObjectsOfType<HandProjector>(true);
            foreach(var projection in projections) {
                if(!projection.useGrabTransition)
                    projection.enabled = true;
            }
        }
    }
}