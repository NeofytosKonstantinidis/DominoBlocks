using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Domino : MonoBehaviour
{
    public Camera camera;
    public Sprite[] dominonumbers;
    public GameObject dominoPrefab;

    public Transform player1;
    public Transform player2;

    public Text gamestatus;
    public Text roundstatus;

    Sprite dominotopnum;
    Sprite dominobottomnum;
    SpriteRenderer spriteRenderer;
    int roundstarter;
    int roundcounter = 0;
    bool chkd = false;

    //Creating the deck of dominos for the 2 players
    List<DominoPawn> player1dominos = new List<DominoPawn>();
    List<DominoPawn> player2dominos = new List<DominoPawn>();
    List<DominoPawn> dominosPlayed = new List<DominoPawn>();
    List<DominoPawn> leftdominos = new List<DominoPawn>();
    List<DominoPawn> rightdominos = new List<DominoPawn>();
    DominoPawn centerdomino;

    //Creating class DominoPawn to use in Lists of Domino's
    public class DominoPawn
        {
        int topnum;
        int bottomnum;
        float xpos;
        int angle;
        bool active;

        public DominoPawn(int topnum,int bottomnum)
        {
            this.topnum = topnum;
            this.bottomnum = bottomnum;
            this.xpos = 0;
            this.angle = 0;
            this.active = true;
        }
        public DominoPawn(int topnum, int bottomnum,float xpos,int angle)
        {
            this.topnum = topnum;
            this.bottomnum = bottomnum;
            this.xpos = xpos;
            this.angle = angle;
            this.active = true;
        }
        public int getTopnum()
        {
            return topnum;
        }
        public int getBottomnum()
        {
            return bottomnum;
        }
        public float getPos()
        {
            return xpos;
        }
        public int getAngle()
        {
            return angle;
        }
        public bool getActive()
        {
            return active;
        }
        public void setActive(bool active)
        {
            this.active = active;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        setdominos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setdominos()
    {
        // Creating the list that checks if a domino numbers combination is already Created
        
        int tempnumber1;
        int tempnumber2;
        System.Random rand = new System.Random();
        for (int pl = 0; pl < 2; pl++)
        {
            int i = 0;
            while (i < 7)
            {
                tempnumber1 = rand.Next(0, 7);
                tempnumber2 = rand.Next(0, 7);
                if (checkcards(tempnumber1, tempnumber2))
                {
                    if (pl == 0)
                    {
                        player1dominos.Add(new DominoPawn(tempnumber1, tempnumber2));
                    }
                    else if (pl == 1)
                    {
                        player2dominos.Add(new DominoPawn(tempnumber1, tempnumber2));
                    }
                    dominosPlayed.Add(new DominoPawn(tempnumber1, tempnumber2));
                    i++;
                }
            }
        }
        while (true)
        {
            tempnumber1 = rand.Next(0, 7);
            tempnumber2 = rand.Next(0, 7);
            if (checkcards(tempnumber1, tempnumber2))
            {
                dominosPlayed.Add(new DominoPawn(tempnumber1, tempnumber2));
                centerdomino = new DominoPawn(tempnumber1, tempnumber2);
                leftdominos.Add(centerdomino);
                rightdominos.Add(centerdomino);
                break;
            }
        }
        float xoffset = 1;
        foreach (DominoPawn domino in player1dominos)
        {
            GameObject Newdomino = Instantiate(dominoPrefab, new Vector3(-5+xoffset,-3.59f,0), Quaternion.identity);
            xoffset++;
            Newdomino.name = "" + (xoffset-2);
            Newdomino.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = dominonumbers[domino.getTopnum()];
            Newdomino.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = dominonumbers[domino.getBottomnum()];
            Newdomino.tag = "Player1Dominos";
            Newdomino.transform.parent = player1;
        }
        xoffset = 1;
        foreach (DominoPawn domino in player2dominos)
        {
            GameObject Newdomino = Instantiate(dominoPrefab, new Vector3(-6 + xoffset, 3.59f, 0), Quaternion.identity);
            Newdomino.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = dominonumbers[domino.getTopnum()];
            Newdomino.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = dominonumbers[domino.getBottomnum()];
            xoffset++;
            Newdomino.name = "" + (xoffset - 2);
            Newdomino.tag = "Player2Dominos";
            Newdomino.transform.parent = player2;
        }
        GameObject middomino = Instantiate(dominoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        middomino.name = "Center Domino";
        middomino.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = dominonumbers[centerdomino.getTopnum()];
        middomino.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = dominonumbers[centerdomino.getBottomnum()];
        beginrounds();


    }

    public void beginrounds()
    {
        System.Random rand = new System.Random();
        roundstarter = rand.Next(0, 2);
        changeround();
    }

    public void changeround()
    {
        int temproundstarter = roundstarter;
        if (!chkd)
        {
            playerscont();
        }
        else
        {
            chkd = false;
        }
        roundcounter++;
        if (roundcounter == 4)
        {
            camera.orthographicSize = 6.0f;
        }
        else if (roundcounter==6)
        {
            camera.orthographicSize = 7.0f;
        }
        else if (roundcounter == 8)
        {
            camera.orthographicSize = 8.0f;
        }
        else if (roundcounter == 6)
        {
            camera.orthographicSize = 9.0f;
        }
        if (roundstarter == 0)
        {
            Debug.Log("Your Turn");
            roundstatus.text = "Your Turn";
            roundstatus.color = Color.white;
        }
        else if (roundstarter == 1)
        {
            roundstatus.text = "Enemy's turn";
            roundstatus.color = Color.red;
            StartCoroutine(Waiting());
        }
        else if (roundstarter == 4)
        {
            roundstatus.text = "";
        }
        else if (roundstarter==5)
        {
            if (temproundstarter == 1)
            {
                roundstatus.text = "Enemy's Turn";
                roundstatus.color = Color.red;
                chkd = true;
                StartCoroutine(Pass(temproundstarter));

            }
            else if (temproundstarter == 0)
            {
                roundstarter = 0;
                chkd = true;
                changeround();
            }
        }
        else if (roundstarter == 6)
        {
            if (temproundstarter == 1)
            {
                roundstarter = 1;
                chkd = true;
                changeround();
            }
            else if (temproundstarter == 0)
            {
                roundstatus.text = "Your Turn";
                roundstatus.color = Color.white;
                chkd = true;
                StartCoroutine(Pass(temproundstarter));
            }
        }
    }

    public void playerscont()
    {
        bool player1chck = playercont(player1dominos);
        bool player2chk = playercont(player2dominos);
        Debug.Log("P1 " + player1chck + " P2 " + player2chk);
        if (!player1chck && !player2chk)
        {
            int pawncounter = 0;
            foreach (DominoPawn dominop in player2dominos)
            {
                if (dominop.getActive())
                {
                    pawncounter++;
                }
            }
            Debug.Log("" + pawncounter);
            if (pawncounter == 0)
            {
                roundstarter = 4;
                matchend(3);
            }
            pawncounter = 0;
            foreach (DominoPawn dominop in player1dominos)
            {
                if (dominop.getActive())
                {
                    pawncounter++;
                }
            }
            if (pawncounter == 0)
            {
                roundstarter = 4;
                matchend(2);
            }
            else
            {
                roundstarter = 4;
                matchend(4);
            }
        }
        else if (player1chck && !player2chk)
        {
            int pawncounter = 0;
            foreach (DominoPawn dominop in player2dominos)
            {
                if (dominop.getActive())
                {
                    pawncounter++;
                }
            }
            if (pawncounter==0)
            {
                roundstarter = 4;
                matchend(3);
            }
            else
            {
                roundstarter = 5;
            }

        }
        else if (!player1chck && player2chk)
        {
            int pawncounter = 0;
            foreach (DominoPawn dominop in player1dominos)
            {
                if (dominop.getActive())
                {
                    pawncounter++;
                }
            }
            if (pawncounter == 0)
            {
                roundstarter = 4;
                matchend(2);
            }
            else
            {
                roundstarter = 6;
            }
        }
    }

    public bool playercont(List<DominoPawn> playerdominos)
    {
        int lefttopnum = leftdominos[leftdominos.Count - 1].getTopnum();
        int leftbotnum = leftdominos[leftdominos.Count - 1].getBottomnum();
        int leftangle = leftdominos[leftdominos.Count - 1].getAngle();

        int righttopnum = rightdominos[rightdominos.Count - 1].getTopnum();
        int rightbotnum = rightdominos[rightdominos.Count - 1].getBottomnum();
        int rightangle = rightdominos[rightdominos.Count - 1].getAngle();

        bool checker = false;

        foreach (DominoPawn dominop in playerdominos)
        {
            if (dominop.getActive())
            {
                int topnum = dominop.getTopnum();
                int botnum = dominop.getBottomnum();

                checker = checkempty(topnum, botnum, lefttopnum, leftbotnum, leftangle);
                if (checker)
                {
                    return true;
                }

                checker = checkempty(topnum, botnum, righttopnum, rightbotnum, rightangle);
                if (checker)
                {
                    return true;
                }
            }

        }
        return false;     
    }
    public void matchend(int num)
    {
        if (num==2)
        {
            gamestatus.text = "Victory";
            roundstatus.color = Color.green;
        }
        if (num == 3)
        {
            gamestatus.text = "Defeat";
            roundstatus.color = Color.red;
        }
        if (num == 4)
        {
            gamestatus.text = "Tie";
            roundstatus.color = Color.black;
        }

    }
    public bool checkcards(int tempnumber1, int tempnumber2)
    {
        if (dominosPlayed.Count > 0)
        {
            bool checker = true;
            foreach (DominoPawn domino in dominosPlayed)
            {
                if ((tempnumber1 == domino.getTopnum() && tempnumber2 == domino.getBottomnum()) || (tempnumber2 == domino.getTopnum() && tempnumber1 == domino.getBottomnum()))
                {
                    checker = false;
                }
            }
            return checker;
        }
        else
        {
            return true;
        }
    }

    public void runbotround()
    {
        bool check = botround();
        Debug.Log("Bot Round Begun");
        if (check)
        {
            roundstarter = 0;
        }
        else if (!check)
        {
            roundstarter = 2;
        }
        
        changeround();
    }
    public bool botround()
    {
        int lefttopnum = leftdominos[leftdominos.Count - 1].getTopnum();
        int leftbotnum = leftdominos[leftdominos.Count - 1].getBottomnum();
        float leftpos = leftdominos[leftdominos.Count - 1].getPos();
        int leftangle = leftdominos[leftdominos.Count - 1].getAngle();

        int righttopnum = rightdominos[rightdominos.Count - 1].getTopnum();
        int rightbotnum = rightdominos[rightdominos.Count - 1].getBottomnum();
        float rightpos = rightdominos[rightdominos.Count - 1].getPos();
        int rightangle = rightdominos[rightdominos.Count - 1].getAngle();

        int i = 0;
        System.Random rand = new System.Random();
        int checkside = rand.Next(0, 2);
        bool checker = false;
        foreach (DominoPawn dominop in player2dominos)
        {
            if (dominop.getActive() && roundstarter==1)
            {
                int topnum = dominop.getTopnum();
                int botnum = dominop.getBottomnum();
                GameObject domino = player2.GetChild(i).gameObject;
                if (checkside == 0)
                {
                    checker = checksides(domino, topnum, botnum, lefttopnum, leftbotnum, 1, leftpos, leftangle);
                    checkside = 1;
                }
                else if (checkside == 1)
                {
                    checker = checksides(domino, topnum, botnum, righttopnum, rightbotnum, -1, rightpos, rightangle);
                    checkside = 0;
                }
                if (!checker)
                {
                    if (checkside == 0)
                    {
                        checker = checksides(domino, topnum, botnum, lefttopnum, leftbotnum, 1, leftpos, leftangle);
                    }
                    else if (checkside == 1)
                    {
                        checker = checksides(domino, topnum, botnum, righttopnum, rightbotnum, -1, rightpos, rightangle);
                    }
                    if (checker)
                    {
                        dominop.setActive(false);
                        roundstarter = 0;
                        return true;
                    }
                }
                else if (checker)
                {
                    dominop.setActive(false);
                    roundstarter = 0;
                    return true;
                }
            }
            
            i++;

        }
        return false;
        
    }

    public void dominoclicked(GameObject domino,int dominonum,int player)
    {
        if (player == 0 && roundstarter==0)
        {
            System.Random rand = new System.Random();
            int checkside = rand.Next(0, 2);
            int topnum = player1dominos[dominonum].getTopnum();
            int botnum = player1dominos[dominonum].getBottomnum();

            int lefttopnum = leftdominos[leftdominos.Count-1].getTopnum() ;
            int leftbotnum = leftdominos[leftdominos.Count-1].getBottomnum();
            float leftpos = leftdominos[leftdominos.Count-1].getPos();
            int leftangle = leftdominos[leftdominos.Count - 1].getAngle();

            int righttopnum = rightdominos[rightdominos.Count - 1].getTopnum(); 
            int rightbotnum = rightdominos[rightdominos.Count - 1].getBottomnum(); 
            float rightpos = rightdominos[rightdominos.Count - 1].getPos();
            int rightangle = rightdominos[rightdominos.Count - 1].getAngle();
            bool checker = false;
            if (checkside==0)
            {
                checker=checksides(domino, topnum, botnum, lefttopnum, leftbotnum, 1,leftpos,leftangle);
                checkside = 1;
            }
            else if (checkside==1)
            {
                checker=checksides(domino, topnum, botnum, righttopnum, rightbotnum, -1,rightpos,rightangle);
                checkside = 0;
            }
            if(!checker)
            {
                if (checkside == 0)
                {
                    checker=checksides(domino, topnum, botnum, lefttopnum, leftbotnum, 1, leftpos, leftangle);
                }
                else if (checkside == 1)
                {
                    checker=checksides(domino, topnum, botnum, righttopnum, rightbotnum, -1, rightpos, rightangle);
                }
                if (checker)
                {
                    player1dominos[dominonum].setActive(false);
                    roundstarter = 1;
                    changeround();
                }
            }
            else if (checker)
            {
                player1dominos[dominonum].setActive(false);
                roundstarter = 1;
                changeround();
            }
            
            
            
        }
        
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(2);
        runbotround();

    }
    IEnumerator Pass(int roundst)
    {
        yield return new WaitForSeconds(1);

        gamestatus.text = "Pass";
        gamestatus.color = Color.black;

        yield return new WaitForSeconds(1);

        if (roundst == 0)
        {
            roundstarter = 1;
        }
        else
        {
            roundstarter = 0;
        }
        gamestatus.text = "";
        changeround();


    }
    public bool checkempty(int topnum,int botnum, int centtopnum, int centbotnum, int angle)
    {
        if (angle == 0)
        {
            if (topnum == centtopnum || botnum == centtopnum || botnum == centbotnum || topnum == centbotnum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (angle == 1)
        {
            if (botnum == centbotnum || topnum == centbotnum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (angle == -1)
        {
            if (topnum == centtopnum || botnum == centtopnum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else return false;
    }

    public bool checksides(GameObject domino, int topnum, int botnum, int centtopnum, int centbotnum, int side, float xpos, int angle)
    {
        if (angle == 0)
        {
            if ((topnum == centtopnum && botnum == centtopnum) || (botnum == centbotnum && topnum == centbotnum))
            {
                domino.transform.position = new Vector3((-0.94f * side) + xpos, 0.0f, 0.0f);
                if (side == 1)
                {
                    leftdominos.Add(new DominoPawn(topnum, botnum, (-0.94f * side) + xpos, 0));
                }
                else if (side == -1)
                {
                    rightdominos.Add(new DominoPawn(topnum, botnum, (-0.94f * side) + xpos, 0));
                }
                return true;
            }
            else if (topnum == centtopnum || topnum == centbotnum)
            {
                domino.transform.Rotate(0, 0, (-90f * side));
                domino.transform.position = new Vector3((-1.4f * side) + xpos, 0.0f, 0.0f);
                if (side == 1)
                {
                    leftdominos.Add(new DominoPawn(topnum, botnum, (-1.4f * side) + xpos, 1));
                }
                else if (side == -1)
                {
                    rightdominos.Add(new DominoPawn(topnum, botnum, (-1.4f * side) + xpos, 1));
                }
                return true;
            }
            else if (botnum == centtopnum || botnum == centbotnum)
            {
                domino.transform.Rotate(0, 0, (90f * side));
                domino.transform.position = new Vector3((-1.4f * side) + xpos, 0.0f, 0.0f);
                if (side == 1)
                {
                    leftdominos.Add(new DominoPawn(topnum, botnum, (-1.4f * side) + xpos, -1));
                }
                else if (side == -1)
                {
                    rightdominos.Add(new DominoPawn(topnum, botnum, (-1.4f * side) + xpos, -1));
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (angle == 1)
        {
            if (botnum == centbotnum && topnum == centbotnum)
            {
                domino.transform.position = new Vector3((-1.41f * side) + xpos, 0.0f, 0.0f);
                if (side == 1)
                {
                    leftdominos.Add(new DominoPawn(topnum, botnum, (-1.41f * side) + xpos, 0));
                }
                else if (side == -1)
                {
                    rightdominos.Add(new DominoPawn(topnum, botnum, (-1.41f * side) + xpos, 0));
                }
                return true;
            }
            else if (topnum == centbotnum)
            {
                domino.transform.Rotate(0, 0, (-90f * side));
                domino.transform.position = new Vector3((-1.88f * side) + xpos, 0.0f, 0.0f);
                if (side == 1)
                {
                    leftdominos.Add(new DominoPawn(topnum, botnum, (-1.88f * side) + xpos, 1));
                }
                else if (side == -1)
                {
                    rightdominos.Add(new DominoPawn(topnum, botnum, (-1.88f * side) + xpos, 1));
                }
                return true;
            }
            else if (botnum == centbotnum)
            {
                domino.transform.Rotate(0, 0, (90f * side));
                domino.transform.position = new Vector3((-1.88f * side) + xpos, 0.0f, 0.0f);
                if (side == 1)
                {
                    leftdominos.Add(new DominoPawn(topnum, botnum, (-1.88f * side) + xpos, -1));
                }
                else if (side == -1)
                {
                    rightdominos.Add(new DominoPawn(topnum, botnum, (-1.88f * side) + xpos, -1));
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (angle == -1)
        {
            if (topnum == centtopnum && botnum == centtopnum)
            {
                domino.transform.position = new Vector3((-1.41f * side) + xpos, 0.0f, 0.0f);
                if (side == 1)
                {
                    leftdominos.Add(new DominoPawn(topnum, botnum, (-1.41f * side) + xpos, 0));
                }
                else if (side == -1)
                {
                    rightdominos.Add(new DominoPawn(topnum, botnum, (-1.41f * side) + xpos, 0));
                }
                return true;
            }
            else if (topnum == centtopnum)
            {
                domino.transform.Rotate(0, 0, (-90f * side));
                domino.transform.position = new Vector3((-1.88f * side) + xpos, 0.0f, 0.0f);
                if (side == 1)
                {
                    leftdominos.Add(new DominoPawn(topnum, botnum, (-1.88f * side) + xpos, 1));
                }
                else if (side == -1)
                {
                    rightdominos.Add(new DominoPawn(topnum, botnum, (-1.88f * side) + xpos, 1));
                }
                return true;
            }
            else if (botnum == centtopnum)
            {
                domino.transform.Rotate(0, 0, (90f * side));
                domino.transform.position = new Vector3((-1.88f * side) + xpos, 0.0f, 0.0f);
                if (side == 1)
                {
                    leftdominos.Add(new DominoPawn(topnum, botnum, (-1.88f * side) + xpos, -1));
                }
                else if (side == -1)
                {
                    rightdominos.Add(new DominoPawn(topnum, botnum, (-1.88f * side) + xpos, -1));
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else return false;
    }
}
