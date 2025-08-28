using System.Collections;
using TarodevController;
using UnityEngine;
public class LevelController : MonoBehaviour
{
    public static void LoadLevel(GameObject fromRoom, GameObject room, GameObject player)
    {
        GameObject level = GameObject.FindWithTag("Level");
        Destroy(level);
        GameObject newRoom = Instantiate(room);
        newRoom.name = room.name;
        Debug.Log(fromRoom.name);
        foreach (var transition in newRoom.GetComponentsInChildren<LevelTransition>())
        {
            Debug.Log(transition.room.name);
            if (fromRoom.name == transition.room.name)
            {
                var playerPosition = transition.transform.position;
                if (playerPosition.x > 7) playerPosition += new Vector3(-1, -0.5f, 0);
                if (playerPosition.x < -7) playerPosition += new Vector3(1, -0.5f, 0);
                if (playerPosition.y > 5) playerPosition.y -= 1;
                if (playerPosition.y < -5) playerPosition.y += 1;
                Debug.Log(playerPosition);
                player.GetComponent<PlayerController>().WarpTo(playerPosition);
                break;
            }
        }
    }
    public static void LoadLevel(GameObject room, Vector3 playerPosition, GameObject player)
    {
        GameObject level = GameObject.FindWithTag("Level");
        Destroy(level);
        GameObject newRoom = Instantiate(room);
        newRoom.name = room.name;
        player.GetComponent<PlayerController>().WarpTo(playerPosition);
    }
}
