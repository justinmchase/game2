using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBehavior : MonoBehaviour {

    private bool _entered;
    private GameObject _popup;

    public GameObject Popup;
    public GameObject Target;

    private void Update()
    {
        if (_entered && Input.GetKeyDown(KeyCode.Return))
        {
            Target.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == GameManager.current.player)
        {
            _entered = true;

            if (this.Popup != null)
            {
                _popup = GameObject.Instantiate(this.Popup);
                _popup.name = "interaction popup";
                _popup.transform.parent = this.transform;
                _popup.transform.position = this.transform.position;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == GameManager.current.player)
        {
            _entered = false;
            if (_popup != null)
            {
                GameObject.Destroy(_popup);
                _popup = null;
            }
        }
    }
}
