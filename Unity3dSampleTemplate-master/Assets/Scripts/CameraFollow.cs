/*
Copyright (c) Sam Hastings
*/

using UnityEngine;
using UnityEngine.U2D;
using Photon.Pun;
using OPS.AntiCheat;
using OPS.AntiCheat.Field;
public class CameraFollow : MonoBehaviourPunCallbacks
{
    //#region Variables
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
	//#endregion

	private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.GetChild(0).position + offset;
           Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
       
            //transform.position = new Vector3(target.position.x, target.position.y, -4f);
        }
        
        
        
    }
}
