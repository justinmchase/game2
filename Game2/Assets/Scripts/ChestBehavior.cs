using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
  public GameObject InteractionPopup;

  private bool _entered;
  private GameObject _popup;
  private GameObject _player;

  private bool _isOpen;

  void Awake ()
  {
    _player = GameObject.Find("game").GetComponent<GameManager>().player;
  }

  public void Interact(bool engaged)
  {
    if (engaged)
    {
      _isOpen = !_isOpen;
      this.GetComponent<Animator>().SetBool("IsOpen", _isOpen);
    }
  }

  public void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject == _player)
    {
      _entered = true;
      _popup = GameObject.Instantiate(this.InteractionPopup);
      _popup.name = "interaction popup";
      _popup.transform.parent = this.transform;
      _popup.transform.position = this.transform.position;

      _player.GetComponent<CreatureBehavior>().interactiveObject = this.gameObject;
    }
  }

  public void OnTriggerExit2D(Collider2D col)
  {
    if (col.gameObject == _player && _popup != null)
    {
      _entered = false;
      GameObject.Destroy(_popup);
      _popup = null;
      _player.GetComponent<CreatureBehavior>().interactiveObject = null;
      
      _isOpen = false;
      this.GetComponent<Animator>().SetBool("IsOpen", _isOpen);
    }
  }
}
