using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace PropHunt.Gameplay
{
    public class PlayerPlacer : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            StartCoroutine(PlacePlayerCoroutine());
        }


        //Place the player on the position 0,0. Only works because the position is client authoritative
        /// <summary>
        /// Place the player at the origin point of the scene. Only works because the position is client authoritarive.
        /// Otherwise, it would need more network shenaningans.
        /// Wait for end of frame to ensure the player was instantiated.
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlacePlayerCoroutine()
        {
            yield return new WaitForEndOfFrame();
            //NetworkManager.Singleton.LocalClient.PlayerObject.transform.position = new Vector3(0, 1.5f, 0);

        }
    }
}