using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OPS.AntiCheat.Field;
public class WallMoveScript : MonoBehaviour
{
    public ProtectedFloat moveSpeed;
    void Update()
    {
        transform.position += Vector3.right * (Time.deltaTime * moveSpeed);
		//moveSpeed += Time.deltaTime / 5;
	}
}
