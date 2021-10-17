using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SelectableHero : MonoBehaviour
    {

        public delegate void SelectableEvent(GameObject selected);

        public event SelectableEvent OnSelect;
        public event SelectableEvent OnSelectMore;


        private bool _selectedMore = false;
        private Coroutine _holdCoroutine = null;
    
        public void OnPointerDown() {
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
            Debug.Log("on hold");
            
            yield return null;
        }

        public void OnClick() {
            if (_holdCoroutine != null) {
                StopCoroutine(_holdCoroutine);
                _holdCoroutine = null;
            }

            if (!_selectedMore) {
                Debug.Log("on click");
                OnSelect?.Invoke(gameObject);
            }
            _selectedMore = false;
        }

    
    }
}
