using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    void Start()
    {
        Instance = this;

        Warrior.Instance.SetInputEnabled(true);
    }
    
    public void WarriorCollision(Collider2D collider)
    {
        Warrior.Instance.SetInputEnabled(false);
        Black.Instance.FadeToBlack(1);

        StartCoroutine(delayedAction(() => {
            Grid.Instance.TeleportChangeMap(collider.gameObject.GetComponent<ITeleporter>().GetTileMap());
            Warrior.Instance.TeleportToLocationAndFaceDirection(collider.gameObject.GetComponent<ITeleporter>().GetLocation(),
                                                                collider.gameObject.GetComponent<ITeleporter>().GetFacingDirection());
        }, 1));

        StartCoroutine(delayedAction(() => {
            Black.Instance.FadeToClear(1);
        }, 1));

        StartCoroutine(delayedAction(() => {
            Warrior.Instance.SetInputEnabled(true);
        }, 2));
    }

    private IEnumerator delayedAction(UnityAction action, int delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
