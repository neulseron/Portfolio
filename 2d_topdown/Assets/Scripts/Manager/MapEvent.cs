using UnityEngine;

public class MapEvent : MonoBehaviour
{
#region Variables
    protected GameObject player;
    protected PlayerAction playerLogic;
#endregion Variables


#region Unity Methods
    protected virtual void Start()
    {
        player = GameManager.Instance.player;
        playerLogic = player.GetComponent<PlayerAction>();
    }
#endregion Unity Methods


#region Event
#endregion Event
}
