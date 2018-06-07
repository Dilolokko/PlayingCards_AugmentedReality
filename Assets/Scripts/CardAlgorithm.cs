
using UnityEngine;
using Vuforia;


public class CardAlgorithm : MonoBehaviour{
    public Camera kullanilanCamera;
    public Transform[] playersGainsCardsList;
    public Transform rightSelectedCardsList, leftSelectedCardsList, acrossSelectedCardsList, mySelectedCardsList;
    public Transform playCardList , selectedCard;
    public Transform usingCards;
    private int x = -420, y = -580, z = -800, xmy = 0, xacross = 0;
    private int xright = 0, yright = 0, zright = 0;
    private int xleft = 0, yleft = 0, zleft = 0;
    public static bool playState = false;
  
    TouchPhase touchPhase = TouchPhase.Ended;

    int sayac = 0;
    // Use this for initialization
    void Start () {
        getCards();
        setPlayerGainsPosition();
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }
	
	// Update is called once per frame
	void Update () {
       
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            sayac++;
            Debug.Log("sayacımız: "+sayac);

            Ray ray = kullanilanCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("denmemeeee" + hit.transform.name);          
                /*if (hit.transform.parent == mySelectedCardsList) hit.transform.parent = acrossSelectedCardsList;
                else hit.transform.parent = mySelectedCardsList;
                */
                if (hit.transform.parent == mySelectedCardsList)
                {
                    if((usingCards.childCount) == 0 || playState)
                    {
                        Debug.Log("kartın parenti" + hit.transform.parent.name + "  " + mySelectedCardsList.name);
                        //Şimdilik ilk kartı sadece biz atıyoruz
                        hit.transform.parent = usingCards;

                        //hit.transform.Translate(new Vector3(0, 0, 0)*Time.deltaTime);
                        //hit.transform.localPosition = new Vector3(0, 0, 0);
                        playState = false;
                        ImageTargetUsingCards.lastPlayedPlayer = 0;
                    }

                }
                hit.transform.tag = hit.transform.parent.name;
                Debug.Log("objenin tag i" + hit.transform.tag);
            }
            
        }
        
        
        
            Ray ray2 = kullanilanCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit2;
        if (Physics.Raycast(ray2, out hit2))
        {
            Debug.Log("denmemeeee" + hit2.transform.name);
            if (hit2.transform.parent == mySelectedCardsList)
            {
                if ((usingCards.childCount) == 0 || playState)
                {
                    Debug.Log("kartın parenti" + hit2.transform.parent.name + "  " + mySelectedCardsList.name);
                    //Şimdilik ilk kartı sadece biz atıyoruz
                    hit2.transform.parent = usingCards;

                    //hit.transform.Translate(new Vector3(0, 0, 0)*Time.deltaTime);
                    //hit.transform.localPosition = new Vector3(0, 0, 0);
                    playState = false;
                    ImageTargetUsingCards.lastPlayedPlayer = 0;
                }
            }
        }


        //Geri butonu ile uygulamayı kapatma
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
            

    }

    void getCards()
    {
        /*int x = -420, y = -580, z = -800;
        for (int i = 0; i < 13; i++)
        {
            selectedCard = playCardList.GetChild(Random.Range(0, playCardList.childCount));
            selectedCardsList.position = kullanilanCamera.transform.position;
            selectedCard.parent = selectedCardsList;
            selectedCard.localPosition = new Vector3(x, y, z);
            selectedCard.localScale = new Vector3(1000, 1000, 1);
            selectedCard.eulerAngles = new Vector4(0, 0, 0);
            x = x + 65;
        }*/

        //mySelectedCardsList.position = kullanilanCamera.transform.position;

        mySelectedCardsList.localPosition = new Vector3(-450, -250, 1200);
        rightSelectedCardsList.localPosition = new Vector3(800, 0, 1500);
        leftSelectedCardsList.localPosition = new Vector3(-800, 0, 1500);
        acrossSelectedCardsList.localPosition = new Vector3(-450, 450, 1900);

        for (int i = 0; i < 52; i++)
        {
            selectedCard = playCardList.GetChild(Random.Range(0, playCardList.childCount));
            selectedCard.localScale = new Vector3(1000, 1000, 1);
            if (mySelectedCardsList.childCount < 13) myCards(selectedCard);
            else if (rightSelectedCardsList.childCount < 13) rightCards(selectedCard);
            else if (leftSelectedCardsList.childCount < 13) leftCards(selectedCard);
            else acrossCards(selectedCard);
        }
    }

    void setPlayerGainsPosition()
    {
        playersGainsCardsList[0].localPosition = new Vector3(-550, -250, 1200);
        playersGainsCardsList[1].localPosition = new Vector3(850, 0, 1500);
        playersGainsCardsList[2].localPosition = new Vector3(-850, 0, 1500);
        playersGainsCardsList[3].localPosition = new Vector3(-550, 450, 1900);
    }
    

    void myCards(Transform card)
    {
        card.parent = mySelectedCardsList;
        card.localPosition = new Vector3(xmy, 0, 0);
        card.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        xmy = xmy + 65;
    }
    void rightCards(Transform card)
    {
        card.parent = rightSelectedCardsList;
        card.localPosition = new Vector3(xright, yright, zright);
        card.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
        yright += 30;
        zright += 20;
        xright -= 5;
    }
    void leftCards(Transform card)
    {
        card.parent = leftSelectedCardsList;
        card.localPosition = new Vector3(xleft, yleft, zleft);
        card.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
        yleft += 30;
        zleft += 20;
        xleft += 5;
    }
    void acrossCards(Transform card)
    {
        card.parent = acrossSelectedCardsList;
        card.localPosition = new Vector3(xacross, 0, 0);
        card.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
        xacross += 65;
    }

}
