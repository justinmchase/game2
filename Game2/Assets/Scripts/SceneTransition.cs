using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
  public class SceneTransition : MonoBehaviour
  {
    public GameObject InteractionPopup;
    public string Destination;

    private bool _entered;
    private GameObject _player;

    void Awake ()
    {
      _player = GameObject.Find("game").GetComponent<GameManager>().player;
    }

    public void Interact(bool engaged)
    {
      this.GoToScene();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (col.gameObject == _player)
      {
        _entered = true;
        _player.GetComponent<CreatureBehavior>().interactiveObject = this.gameObject;
        InteractionPopup.SetActive(true);
      }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
      if (col.gameObject == _player)
      {
        _entered = false;
        _player.GetComponent<CreatureBehavior>().interactiveObject = null;
        InteractionPopup.SetActive(false);
      }
    }

    public void GoToScene(){
      SceneManager.LoadScene(this.Destination);
    }
  }
}