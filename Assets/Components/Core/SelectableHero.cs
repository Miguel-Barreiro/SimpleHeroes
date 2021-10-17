using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gram.Core
{
    public class SelectableHero : EventTrigger
    {

        public delegate void SelectableEvent(GameObject selected);

        public event SelectableEvent OnSelected;
        public event SelectableEvent OnSelectMore;


        private bool _selectedMore = false;
        private Coroutine _holdCoroutine = null;
    
        public override void OnPointerDown(PointerEventData data) {
            if(_holdCoroutine == null && !_selectedMore)
            {
                _holdCoroutine = StartCoroutine(HoldCoroutineUtility());
            }
        }

        private IEnumerator HoldCoroutineUtility() {
            _selectedMore = false;

            float deltaTime = 0;
            while (deltaTime < 3) {
                deltaTime += Time.deltaTime;
                yield return null;
            }
            _selectedMore = true;
            _holdCoroutine = null;
            OnSelectMore?.Invoke(gameObject);
            
            yield return null;
        }

        public override void OnPointerUp(PointerEventData data) {
            if (_holdCoroutine != null) {
                StopCoroutine(_holdCoroutine);
                _holdCoroutine = null;
            }

            if (!_selectedMore) {
                Debug.Log("OnSelect");
                OnSelected?.Invoke(gameObject);
            }
            _selectedMore = false;
        }

    
    }
}
