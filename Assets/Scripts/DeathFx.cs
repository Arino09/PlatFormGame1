using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFx : MonoBehaviour

{
    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
