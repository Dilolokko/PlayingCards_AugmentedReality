using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightGainsAndPlay : MonoBehaviour {
    public Transform player, gain, center;
    public int gain_card_count, random_number;
	// Use this for initialization
	void Start () {
        gain_card_count = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(gain.GetChildCount() != gain_card_count)
        {
            if ((gain.GetChildCount() % 4) == 0 && player.childCount != 0)
            {

                random_number = Random.Range(0, (player.GetChildCount()-1));
                Transform selectedCard = player.GetChild(random_number);
                selectedCard.rotation = Quaternion.Euler(new Vector3(250, 0, 0));
                selectedCard.position = center.position;
                selectedCard.parent = center;
                gain_card_count += 4;
                CardAlgorithm.playState = true;
                ImageTargetUsingCards.lastPlayedPlayer = 0;
            }
        }
	}
}
