using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static bool gameOver = false; //主角死亡标志
    public static void setGameOver(bool flag)
    {
        gameOver = flag;
    }

    public static bool getGameOver()
    {
        return gameOver;
    }
    // 在出生点重生
    public static void Respawn(GameObject respawner, Vector3 position)
    {
        Destroy(respawner);
        Instantiate(respawner,position,respawner.transform.rotation);
        gameOver = false;  //标志归否
    }

}
