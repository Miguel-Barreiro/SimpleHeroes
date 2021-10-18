using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gram.Utils.UI
{
    public class Selectable : EventTrigger
    {

        public delegate void SelectableEvent(PointerEventData data);

        public event SelectableEvent OnSelected;
        public event SelectableEvent OnSelectMore;
        public event SelectableEvent OnSelectMoreEnd;
        

        private bool _selectedMore = false;
        private Coroutine _holdCoroutine = null;
    
        public override void OnPointerDown(PointerEventData data) {
            if(_holdCoroutine == null && !_selectedMore)
            {
                _holdCoroutine = StartCoroutine(HoldCoroutineUtility(data));
            }
        }

        private IEnumerator HoldCoroutineUtility(PointerEventData pointerEventData) {
            _selectedMore = false;

            float deltaTime = 0;
            while (deltaTime < 3) {
                deltaTime += Time.deltaTime;
                yield return null;
            }
            _selectedMore = true;
            _holdCoroutine = null;
            OnSelectMore?.Invoke(pointerEventData);
            
            yield return null;
        }

        public override void OnPointerUp(PointerEventData data) {
            if (_holdCoroutine != null) {
                StopCoroutine(_holdCoroutine);
                _holdCoroutine = null;
            }

            if (_selectedMore) {
                OnSelectMoreEnd?.Invoke(data);
            } else {
                OnSelected?.Invoke(data);
            }
            
            _selectedMore = false;
        }

    
    }
}
