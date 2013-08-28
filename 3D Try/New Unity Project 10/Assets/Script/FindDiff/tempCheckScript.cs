/*using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
   
    public Texture checkMark;
    bool isChecked = false;
    bool isPrinting = false;
    bool isPrinting1 = false;
    GUIStyle NoStyle = new GUIStyle();
       
    void OnGUI () {
           
   
            // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            if(GUI.Button(new Rect(20,100,80,20),"")) {
               
           
            isPrinting = true;
           
            /*
            if (isPrinting)
                isPrinting = false;
                else
                isPrinting = true;
                */
           
   /*            
            }
        if(GUI.Button(new Rect(100,100,80,20),"")) {
               
           
            isPrinting1 = true;
        }
           
            if (isPrinting)
                printTexture(20, 70);
             if (isPrinting1)
                printTexture(100, 70);
        }
   
     void printTexture(int x, int y){
 
        GUI.DrawTexture(new Rect(x, y, 20, 20), checkMark);
    }
   
}
*/