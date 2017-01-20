using UnityEngine;
using System.Collections;

namespace StrategistUI
{
    public class MarkActivator : MonoBehaviour
    {
        public Camera cam;

        public int layermaskToTrack;// = LayerMask.NameToLayer("MinionEntity"); // for now, because i can't track anything else

        void Update()
        {
            _HightlightMarks();
        }

        private Collider _highlightedMark = null;
        private void _HightlightMarks()
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << layermaskToTrack);
            if (hit.collider != _highlightedMark)
            {
                if (_highlightedMark != null)
                    _highlightedMark.transform.root.BroadcastMessage("OnMouseExitCustom");

                _highlightedMark = hit.collider;
                if (_highlightedMark != null)
                    _highlightedMark.transform.root.BroadcastMessage("OnMouseEnterCustom");
            }
        }
    }
}