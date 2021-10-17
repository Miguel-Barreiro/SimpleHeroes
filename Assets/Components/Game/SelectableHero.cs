using System.Collections;
using UnityEngine;

namespace Gram.Game
{
    public class SelectableHero : MonoBehaviour
    {

        public delegate void SelectableEvent(GameObject selected);

        public event SelectableEvent OnSelect;
        public event SelectableEvent OnSelectMore;


        private bool _selectedMore = false;
        private Coroutine _holdCoroutine = null;
    
        public void OnPointerDown() {
            
            Debug.Log("OnPointerDown");
            
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

        public void OnClick() {

            Debug.Log("onclick");
            
            if (_holdCoroutine != null) {
                StopCoroutine(_holdCoroutine);
                _holdCoroutine = null;
            }

            if (!_selectedMore) {
                OnSelect?.Invoke(gameObject);
            }
            _selectedMore = false;
        }

    
    }
}
