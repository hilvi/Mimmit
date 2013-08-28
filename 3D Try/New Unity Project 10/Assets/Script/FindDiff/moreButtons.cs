using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
   
    public Texture checkMark;
    bool[] manyButtons = new bool[5];
    Vector2[] manyVect = new Vector2[5];
   
    void Start(){
       
    }
       
    void OnGUI () {
           
   
            // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            if(GUI.Button(new Rect(20,100,80,20),"")) {
               
                manyVect[1].x = 50;
                manyVect[1].y = 70;
           
                //manyButtons[1] = true;
           
            if(manyButtons[1])
                manyButtons[1] = false;
            else
                manyButtons[1] = true;
           
           
            }
        if(GUI.Button(new Rect(100,100,80,20),"")) {
               
                if(manyButtons[2])
                manyButtons[2] = false;
            else
                manyButtons[2] = true;
               
                manyVect[2].x = 130;
                manyVect[2].y = 70;
        }
        if(GUI.Button(new Rect(180,100,80,20),"")) {
               
                if(manyButtons[3])
                manyButtons[3] = false;
            else
                manyButtons[3] = true;
               
                manyVect[3].x = 210;
                manyVect[3].y = 70;
        }
        if(GUI.Button(new Rect(260,100,80,20),"")) {
               
                if(manyButtons[4])
                manyButtons[4] = false;
            else
                manyButtons[4] = true;
               
                manyVect[4].x = 290;
                manyVect[4].y = 70;
        }
        if(GUI.Button(new Rect(340,100,80,20),"")) {
               
                if(manyButtons[0])
                manyButtons[0] = false;
            else
                manyButtons[0] = true;
               
                manyVect[0].x = 370;
                manyVect[0].y = 70;
        }
           
            for(int i = 0; i<5;i++)
            {
                if(manyButtons[i]){
                   
                    int x, y;
                    x = (int)manyVect[i].x;
                    y = (int)manyVect[i].y;
                    printTexture(x, y);
                }
            }
           
    }
   
     void printTexture(int x, int y){
 
        GUI.DrawTexture(new Rect(x, y, 20, 20), checkMark);
    }
   
}