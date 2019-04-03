using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBehavior : MonoBehaviour {

    private bool _entered;
    private bool _activated;
    private GameObject _popup;

    public GameObject Popup;
    public GameObject Target;

    public string OnActivateMessage = "Activate";
    public string OnSelectMessage = "Select";
    public string OnDeselectMessage = "Unselect";
    public string OnDeactivateMessage = "Deactivate";

    private void Update()
    {
        if (_entered && Input.GetKeyDown(KeyCode.Return))
        {
            this._activated = true;
            Target.SendMessage(this.OnActivateMessage, SendMessageOptions.DontRequireReceiver);
        }

        if (this._activated && this._entered && Input.GetKeyUp(KeyCode.Return))
        {
            this._activated = false;
            Target.SendMessage(this.OnDeactivateMessage, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == GameManager.current.player)
        {
            _entered = true;

            Target.SendMessage(this.OnSelectMessage, SendMessageOptions.DontRequireReceiver);

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
            if (this._activated)
            {
                this._activated = false;
                Target.SendMessage(this.OnDeactivateMessage, SendMessageOptions.DontRequireReceiver);
            }

            Target.SendMessage(this.OnDeselectMessage, SendMessageOptions.DontRequireReceiver);

            _entered = false;
            if (_popup != null)
            {
                GameObject.Destroy(_popup);
                _popup = null;
            }
        }
    }
}
