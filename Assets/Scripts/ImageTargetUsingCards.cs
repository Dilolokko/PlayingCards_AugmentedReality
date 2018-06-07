using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTargetUsingCards : MonoBehaviour  {

    public Transform[] players, playersGains;
    public Transform usingCards;
    public static int lastPlayedPlayer;
    public GameObject messageText;
    public Text messageTextUI;
    public GameObject finalTable;
    public Text[] userFinalPointsUI;
    public Text gainPlayerUI;
    private Transform biggestCard = null, tempCard, lastCard, firstCard ;
    private int centerBiggestCardPlayer, firstPlayedPlayer, playedPlayerCount = 0;
    private IEnumerator coroutine;
    


    // Use this for initialization
    void Start () {
        lastPlayedPlayer = 0;
        coroutine = waitForPlayer(1.0f);
        StartCoroutine(coroutine);
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("update calısıyor" + usingCards.childCount);

	}

    IEnumerator waitForPlayer(float waitTime)
    {
        while (true) {
            playEvent();

            print(Time.time);
            yield return new WaitForSeconds(waitTime);
            
        }
    }

    void playEvent()
    {
        int x = 0;
        for (int i = 0; i < usingCards.childCount; i++)
        {
            Vector3 parentPosition = usingCards.position;
            usingCards.GetChild(i).transform.position = new Vector3(parentPosition.x + x, parentPosition.y, parentPosition.z);

            x = x - 65;


            //Debug.Log("Ortadaki kart " + i + " ismi" + usingCards.GetChild(i).name + " pozisyonu " + usingCards.GetChild(i).position + " parent oriantation " + usingCards.rotation + "  ilk attığımız kartın oriant " + usingCards.GetChild(i).rotation);
        }

        if(usingCards.childCount == 1)
        {
            setFirstPlayedPlayer();
        }
        //Bir el bittikten sonra kartları kazanana aktarıldığı kısım
        findCenterBiggestCard();
        if (usingCards.childCount == 4)
        {
            //Buraya kartların parent eşleşmesi yaptırılacak;
            findCenterBiggestCard();

        }
        //Yapay zekanın oynaması tetikleyen kısım
        else if (usingCards.childCount > 0 )
        {
            messageTextSetStateFalse();
            virtualPlayer();
        }

        if(players[0].childCount == 0 && players[1].childCount == 0 && players[2].childCount == 0 && players[3].childCount == 0)
        {
            int gainPlayer = 0, tempGain = 0;
            userFinalPointsUI[0].text = (playersGains[0].childCount / 4).ToString();
            userFinalPointsUI[1].text = (playersGains[1].childCount / 4).ToString();
            userFinalPointsUI[2].text = (playersGains[2].childCount / 4).ToString();
            userFinalPointsUI[3].text = (playersGains[3].childCount / 4).ToString();
            for(int i = 0; i<4; i++)
            {
                if (tempGain < playersGains[i].childCount)
                {
                    gainPlayer = i;
                    tempGain = playersGains[i].childCount;
                }
                gainPlayerUI.text = getUserName(gainPlayer);
            }
            finalTable.SetActive(true);

        }
        
    }
    //------ SANAL OYUNCU ------

    //Yapay zekanın main metodu, birsonraki kullanıcının hangi kartı seçeceğini tetikleyen metod
    void virtualPlayer()
    {
        setFirstCard();
        setLastCard();
        //for(int i = 0; i< players.Length; i++)
        //{
            if(players[getLastPlayer()] != players[3] && canPlay(getNextPlayer(getLastPlayer()) , firstPlayedPlayer))
            {
                messageTextSetStateFalse();
                setCurrentUserPlayCard(players[getNextPlayer(getLastPlayer())]);
            Debug.Log("1. nokta"+ players[getNextPlayer(getLastPlayer())].name+ " player sırası" + getNextPlayer(getLastPlayer()));
            
            }
            else if(players[getLastPlayer()] == players[3])
            {
                messageTextChangeText("Oynama sırası sizde ");
                messageTextSetStateTrue();
            }
        //}

    }

    int getLastPlayer()
    {
        int currentUserID = 0, tempChildCount = 20, tempUserID = firstPlayedPlayer;
        for(int i = 0; i<players.Length; i++)
        {
            if(players[tempUserID].childCount <= tempChildCount)
            {
                currentUserID = tempUserID;
                tempChildCount = players[tempUserID].childCount;
            }
            tempUserID = getNextPlayer(tempUserID);
        }
       /* if(firstPlayedPlayer == 0) { 
            if(currentUserID == 3)
            {
                if(players[0].childCount == players[3].childCount)
                {
                    currentUserID = 0;
                }
            }
        }*/
        Debug.Log("son kart atan kullanici " + currentUserID);
        return currentUserID;
    }
   
    //Yapay zekanın bir sonraki kullanıcıyı tespit ettiği metod
    int getNextPlayer(int order)
    {
        switch (order)
        {
            case 0: return 1;
            case 1: return 2;
            case 2: return 3;
            case 3: return 0;
        }
        return 1;
    }

    //Yapay zekanın merkezdeki en büyük kartı bulup bu karttan göre uygun kartı seçmesini tetikleyen metod
    void setCurrentUserPlayCard(Transform tempPlayer)
    {
        Transform tempCard = findPlayerBiggestCard(tempPlayer);
        Debug.Log("2. nokta " + tempPlayer.name+" kartın ismi " + tempCard.name);
        currentUserPlayCard(tempCard);
        Debug.Log("5. nokta " + tempPlayer.name);
    }

    //Yapay zekanın en uygun kartı bulduğu metod
    Transform findPlayerBiggestCard(Transform tempPlayer)
    {
        tempCard = biggestCard;
        bool isCorrect = false;
        for (int i = 0; i < tempPlayer.childCount; i++)
        {
            Debug.Log("if 1-1  " + parseCardType(getCardName(tempPlayer.GetChild(i))) + " esit mi " + parseCardType(getCardName(tempCard)) + " kart numaraları " + parseCardNum(getCardName(tempPlayer.GetChild(i))) + ">" + parseCardNum(getCardName(tempCard)));
            if (parseCardType(getCardName(tempPlayer.GetChild(i))).Equals(parseCardType(getCardName(tempCard))))
            {
                if (parseCardNum(getCardName(tempPlayer.GetChild(i))) > parseCardNum(getCardName(tempCard)))
                {
                    tempCard = tempPlayer.GetChild(i).transform;
                    biggestCard = tempCard;
                    Debug.Log("bizim buldugumuz en büyük kart " + biggestCard.name);
                    isCorrect = true;
                }
            }

        }
        Debug.Log("3. nokta " + tempCard.name+" iscorrect " + isCorrect);
        if (!isCorrect)
        {
            for (int i = 0; i < tempPlayer.childCount; i++)
            {
                Debug.Log("if 2-1  " + parseCardType(getCardName(tempPlayer.GetChild(i))) + " esit mi " + parseCardType(getCardName(tempCard)) + " kart numaraları " + parseCardNum(getCardName(tempPlayer.GetChild(i))) + "<" + parseCardNum(getCardName(tempCard)));
                if (parseCardType(getCardName(tempPlayer.GetChild(i))).Equals(parseCardType(getCardName(tempCard))))
                {
                    if (parseCardNum(getCardName(tempPlayer.GetChild(i))) < parseCardNum(getCardName(tempCard)))
                    {
                        tempCard = tempPlayer.GetChild(i).transform;
                        Debug.Log("bizim buldugumuz en küçük kart " + tempCard.name);
                        isCorrect = true;
                    }
                }

            }
        }
        Debug.Log("4. nokta " + tempCard.name + " iscorrect " + isCorrect);
        if (!isCorrect)
        {
            for (int i = 0; i < tempPlayer.childCount; i++)
            {
                Debug.Log("if 3-1   kart numaraları " + parseCardNum(getCardName(tempPlayer.GetChild(i))) + "<" + parseCardNum(getCardName(tempCard)));
                if (parseCardNum(getCardName(tempPlayer.GetChild(i))) < parseCardNum(getCardName(tempCard)))
                {
                    tempCard = tempPlayer.GetChild(i).transform;
                    Debug.Log("bizim buldugumuz en küçük farketmeyen kart  " + tempCard.name);
                    isCorrect = true;
                }

            }
        }

        if (!isCorrect)
        {
            for (int i = 0; i < tempPlayer.childCount; i++)
            {             
                if (parseCardNum(getCardName(tempPlayer.GetChild(i))) <= parseCardNum(getCardName(tempCard)))
                {
                    tempCard = tempPlayer.GetChild(i).transform;
                    Debug.Log("bizim buldugumuz eşit farketmeyen kart  " + tempCard.name);
                    isCorrect = true;
                }

            }
        }

        if (!isCorrect)
        {
            for (int i = 0; i < tempPlayer.childCount; i++)
            {             
                if (parseCardNum(getCardName(tempPlayer.GetChild(i))) > parseCardNum(getCardName(tempCard)))
                {
                    tempCard = tempPlayer.GetChild(i).transform;
                    Debug.Log("bizim buldugumuz en büyük farketmeyen kart  " + tempCard.name);
                    isCorrect = true;
                }

            }
        }

        return tempCard;
    }

    //Yapay zekanın en uygun kartı oynadığı metod
    void currentUserPlayCard(Transform selectedCard)
    {
        Debug.Log("Atılan kart "+selectedCard.name);
        selectedCard.rotation = Quaternion.Euler(new Vector3(250, 0, 0)); 
        selectedCard.parent = usingCards;
        //selectedCard.transform.position = usingCards.transform.position;
        tempCard = null;
    }

    
    //------ MERKEZ OYUN YÖNETİMİ ------

    //Merkezdeki en büyük kartı bulduğumuz metod
    void findCenterBiggestCard()
    {
        biggestCard = null;
        Transform tempCardList = usingCards;
        int tempCount = 0;
        for (int i = 0; i < tempCardList.childCount; i++)
        {
            if (biggestCard == null)
            {
                biggestCard = tempCardList.GetChild(i).transform;
            }
            else if (parseCardType(getCardName(tempCardList.GetChild(i))).Equals(parseCardType(getCardName(biggestCard))))
            {

                if (parseCardNum(getCardName(tempCardList.GetChild(i))) > parseCardNum(getCardName(biggestCard)))
                {
                    Debug.Log("Ortadaki kartlar " + tempCardList.GetChild(i).name + " > " + biggestCard.name);
                    biggestCard = tempCardList.GetChild(i).transform;
                    centerBiggestCardPlayer = i;
                    Debug.Log("Ortadaki en büyük kart" + biggestCard.name +  " sırası "  +centerBiggestCardPlayer);
                }
            }

        }
        /*for(int i = 0; i <= centerBiggestCardPlayer; i++)
        {
            tempCount = getNextPlayer(centerBiggestCardPlayer);
        }
        centerBiggestCardPlayer = tempCount;
        */

        //Kazanan oyuncuya kartların verilmesi için metot çağrılır
        if(usingCards.childCount == 4) { 
            findGainPlayerAndSendCards();
        }
    }

    //Kazanan oyuncuya kazandığı kartları verdiğimiz metod
    void findGainPlayerAndSendCards()
    {
        int tempPlayerID = firstPlayedPlayer;
        for (int i = 0; i < centerBiggestCardPlayer; i++)
        {
            tempPlayerID = getNextPlayer(tempPlayerID);
        }

        
        Debug.Log("Ortadaki en büyük kart sahibi sırası" + centerBiggestCardPlayer + " ve kazanan oyuncunun sırası " + tempPlayerID + " kazandığı kart " + biggestCard.name);
        //kazanana iki tane kartı aktarabiliyoruz 2 kart burda kalıyor
        for (int j = 0; j < 4; j++)
        {
            usingCards.GetChild(0).transform.position = playersGains[tempPlayerID].position;
            usingCards.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            usingCards.GetChild(0).transform.parent = playersGains[tempPlayerID];
        }
        
        messageTextChangeText("Kazanan oyuncu " + getUserName(tempPlayerID));
        messageTextSetStateTrue();
            /*usingCards.GetChild(0).transform.parent = playersGains[i];
             usingCards.GetChild(1).transform.parent = playersGains[i];
             usingCards.GetChild(2).transform.parent = playersGains[i];
             usingCards.GetChild(3).transform.parent = playersGains[i];
             for(int j = 1; j<usingCards.childCount; j++)
             {
                 usingCards.GetChild(j).transform.parent = playersGains[i];
             }*/
        
    }


    //------ GETİR GÖTÜR İŞLERİ ------

    //Gönderilen kartın numarasını bulan metod 
    int parseCardNum(string name)
    {
        int x = 0;
        string[] splitName = name.Split('_');
        try
        {
            if(int.Parse(splitName[1].Substring(0, 1)) == 1)
            {
                return 10;
            }
            return int.Parse(splitName[1].Substring(0, 1));
        }
        catch
        {
            return convertBiggerNum(splitName[1].Substring(0, 1));
        }

    }

    //Gönderilen kartın türünü bulan metod
    string parseCardType(string name)
    {
        string[] splitName = name.Split('_');
        if(parseCardNum(name) == 10)
        {
            return splitName[1].Substring(2);
        }
        return splitName[1].Substring(1);
    }

    //Büyük kartların sayıya çevrildiği metod
    int convertBiggerNum(string cardNum)
    {
        switch (cardNum)
        {
            case "J": return 11;
            case "Q": return 12;
            case "K": return 13;
            case "A": return 14;
        }
        return 0;
    }

    //Kartın ismini bulan metod
    string getCardName(Transform t)
    {
        return t.name;
    }

    bool canPlay(int nextPlayer , int firstPlayer)
    {
        if(nextPlayer == firstPlayer)
        {
            return false;
        }
        return true;
    }

    void setFirstPlayedPlayer()
    {
        int currentUserID = 0, tempChildCount = 20;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].childCount <= tempChildCount)
            {
                currentUserID = i;
                tempChildCount = players[i].childCount;
            }
        }
        firstPlayedPlayer = currentUserID;
        Debug.Log("ilk kartı atan kullanıcının sayısı " + firstPlayedPlayer);
    }

    void setFirstCard()
    {
        firstCard = usingCards.GetChild(0).transform;
    }
    void setLastCard()
    {
        lastCard = usingCards.GetChild(usingCards.childCount - 1).transform;
    }
    

    void messageTextSetStateFalse()
    {
        messageText.SetActive(false);
        PauseMenu.messageTextState = false;
    }

    void messageTextSetStateTrue()
    {
        messageText.SetActive(true);
        PauseMenu.messageTextState = true;
    }

    void messageTextChangeText(string message)
    {
        messageTextUI.text = message;
    }

    string getUserName(int playerNum)
    {
        switch (playerNum)
        {
            case 0: return "Siz";
            case 1: return "Sağ";
            case 2: return "Karşı";
            case 3: return "Sol";
        }
        return "Siz";
    }
}
