using UnityEngine;
using System.Collections;

public class FDManager : MonoBehaviour {
	
	SetStage num;
	public Texture checkMark;
	GUIStyle noStyle = new GUIStyle();
    bool[] manyButtons = new bool[5];
    Vector2[] manyVect = new Vector2[5];
	
	
	// Use this for initialization
	void Start () {
		num = GameObject.Find("Main Camera").GetComponent<SetStage>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		if(num.num==1){
			if(GUI.Button(new Rect(Screen.width/2+(Screen.width/5),Screen.height/4-Screen.height/40,15,15),"",noStyle)){	
				checkCheck(0,Screen.width/2+(Screen.width/5),Screen.height/4-Screen.height/40);
			}
			if(GUI.Button(new Rect(Screen.width/2-Screen.width/7-Screen.width/120,Screen.height/2+Screen.height/10,35,30),"",noStyle)){
				checkCheck(1,Screen.width/2-Screen.width/7-Screen.width/120,Screen.height/2+Screen.height/10);
			}
			if(GUI.Button(new Rect(Screen.width/2-Screen.width/18,Screen.height/2+Screen.height/5-Screen.height/50,13,18),"",noStyle)){
				checkCheck(2,Screen.width/2-Screen.width/18,Screen.height/2+Screen.height/5-Screen.height/50);
			}
			if(GUI.Button(new Rect(Screen.width-Screen.width/5+Screen.width/40,Screen.height/2+Screen.height/7+Screen.height/50,15,10),"",noStyle)){
				checkCheck(3,Screen.width-Screen.width/5+Screen.width/40,Screen.height/2+Screen.height/7+Screen.height/50);
			}
			if(GUI.Button(new Rect(Screen.width-Screen.width/8-Screen.width/60,Screen.height-Screen.height/9,33,30),"",noStyle)){
				checkCheck(4,Screen.width-Screen.width/8-Screen.width/60,Screen.height-Screen.height/9);
			}
			
		}
		
		else if(num.num==2){
			if(GUI.Button(new Rect(Screen.width-Screen.width/8,Screen.height/2-Screen.height/20,45,45),"",noStyle)){
				checkCheck(0,Screen.width-Screen.width/8,Screen.height/2-Screen.height/20);
			}
			if(GUI.Button(new Rect(Screen.width/15,Screen.height/2+Screen.height/10,200,200),"",noStyle)){
				checkCheck(1,Screen.width/15,Screen.height/2+Screen.height/10);
			}
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/40,0+Screen.height/8,50,50),"",noStyle)){
				checkCheck(2,Screen.width/2-Screen.width/40,0+Screen.height/8);
			}
			if(GUI.Button (new Rect(Screen.width/4+Screen.width/17,Screen.height/2-Screen.height/15,15,20),"",noStyle)){
				checkCheck(3,Screen.width/4+Screen.width/17,Screen.height/2-Screen.height/15);
			}
		}
		else if (num.num==3){
			if(GUI.Button(new Rect(Screen.width-Screen.width/9,Screen.height-Screen.height/8,40,35),"",noStyle)){
				checkCheck(0,Screen.width-Screen.width/9,Screen.height-Screen.height/8);
			}
			if(GUI.Button(new Rect(Screen.width/2+Screen.width/16,Screen.height/3-Screen.height/5,200,70),"",noStyle)){
				checkCheck(1,Screen.width/2+Screen.width/16,Screen.height/3-Screen.height/5);
			}
			if(GUI.Button (new Rect(0,Screen.height/8,135,80),"",noStyle)){
				checkCheck(2,0,Screen.height/8);
			}
			if(GUI.Button (new Rect(Screen.width/2,Screen.height/2-Screen.height/10,80,80),"",noStyle)){
				checkCheck(3,Screen.width/2,Screen.height/2-Screen.height/10);
			}
			if(GUI.Button(new Rect(Screen.width-Screen.width/5,Screen.height/2+Screen.height/11,70,70),"",noStyle)){
				checkCheck(4,Screen.width-Screen.width/5,Screen.height/2+Screen.height/11);
			}
		}
		else if (num.num==4){
			if(GUI.Button(new Rect(Screen.width/2-Screen.width/8,0+Screen.height/2-Screen.height/7,20,20),"",noStyle)){
				checkCheck(0,Screen.width/2-Screen.width/8,0+Screen.height/2-Screen.height/7);
			}
			if(GUI.Button(new Rect(Screen.width/2+Screen.width/16,Screen.height/3-Screen.height/5,110,80),"",noStyle)){
				checkCheck(1,Screen.width/2+Screen.width/16,Screen.height/3-Screen.height/5);
			}
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/5-Screen.width/80,Screen.height/2-Screen.height/60,40,10),"",noStyle)){
				checkCheck(2,Screen.width/2-Screen.width/5-Screen.width/80,Screen.height/2-Screen.height/60);
			}
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/20,2*Screen.height/3,300,120),"",noStyle)){
				checkCheck(3,Screen.width/2-Screen.width/20,2*Screen.height/3);
			}
			if(GUI.Button(new Rect(Screen.width-Screen.width/7,Screen.height/2-Screen.height/5,40,30),"",noStyle)){
				checkCheck(4,Screen.width-Screen.width/7,Screen.height/2-Screen.height/5);
			}
		}
		else if (num.num==5){
			if(GUI.Button(new Rect(Screen.width/2-Screen.width/5-Screen.width/40,Screen.height/2-Screen.height/7,20,20),"",noStyle)){
				checkCheck(0,Screen.width/2-Screen.width/5-Screen.width/40,Screen.height/2-Screen.height/7);
			}
			if(GUI.Button(new Rect(Screen.width/2-Screen.width/8,Screen.height/3+Screen.height/18,145,30),"",noStyle)){
				checkCheck(1,Screen.width/2-Screen.width/8,Screen.height/3+Screen.height/18);
			}
			if(GUI.Button (new Rect(Screen.width -2*Screen.width/3,Screen.height/2-Screen.height/60,90,80),"",noStyle)){
				checkCheck(2,Screen.width -2*Screen.width/3,Screen.height/2-Screen.height/60);
			}
			if(GUI.Button(new Rect(Screen.width/2+Screen.width/8+Screen.width/80,0,110,150),"",noStyle)){
				checkCheck(3,Screen.width/2+Screen.width/8+Screen.width/80,0);
			}
		}
		else if(num.num==6){
			if(GUI.Button(new Rect(Screen.width/4-Screen.width/18,Screen.height/2-Screen.height/5,47,30),"",noStyle)){
				checkCheck(0,Screen.width/4-Screen.width/18,Screen.height/2-Screen.height/5);
			}
			if(GUI.Button(new Rect(0,8*Screen.height/9-Screen.height/20,450,50),"",noStyle)){
				checkCheck(1,0,8*Screen.height/9-Screen.height/20);
			}
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/30,2*Screen.height/3,90,50),"",noStyle)){
				checkCheck(2,Screen.width/2-Screen.width/30,2*Screen.height/3);
			}
			if(GUI.Button(new Rect(Screen.width/15,0,70,250),"",noStyle)){
				checkCheck(3,Screen.width/15,0);
			}
			if(GUI.Button(new Rect(7*Screen.width/12-Screen.width/40,3*Screen.height/13+Screen.height/25,40,20),"",noStyle)){
				checkCheck(4,7*Screen.width/12-Screen.width/40,3*Screen.height/13+Screen.height/25);
			}
		}
			
		else if (num.num==7){
			if(GUI.Button(new Rect(Screen.width/4,Screen.height/4+Screen.height/40,20,20),"",noStyle)){
				checkCheck(0,Screen.width/4,Screen.height/4+Screen.height/40);
			}
			if(GUI.Button(new Rect(Screen.width-Screen.width/4,Screen.height/2-Screen.height/6,30,30),"",noStyle)){
				checkCheck(1,Screen.width-Screen.width/4,Screen.height/2-Screen.height/6);
			}
			if(GUI.Button (new Rect(2*Screen.width/5-Screen.width/30,Screen.height/2-Screen.height/50,70,40),"",noStyle)){
				checkCheck(2,2*Screen.width/5-Screen.width/30,Screen.height/2-Screen.height/50);
			}
			if(GUI.Button(new Rect(Screen.width/2+Screen.width/3,0+Screen.height/30,50,50),"",noStyle)){
				checkCheck(3,Screen.width/2+Screen.width/3,0+Screen.height/30);
			}
			if(GUI.Button(new Rect(Screen.width/2+Screen.width/3,3*Screen.height/4,40,40),"",noStyle)){
				checkCheck(4,Screen.width/2+Screen.width/3,3*Screen.height/4);
			}
		}
		else if (num.num ==8){
			if(GUI.Button(new Rect(Screen.width/4+Screen.width/100,Screen.height/5-Screen.height/100,15,15),"",noStyle)){
				checkCheck(0,Screen.width/4+Screen.width/100,Screen.height/5-Screen.height/100);
			}
			if(GUI.Button(new Rect(4*Screen.width/5+Screen.width/20,Screen.height/6,55,50),"",noStyle)){
				checkCheck(1,4*Screen.width/5+Screen.width/20,Screen.height/6);
			}
			if(GUI.Button (new Rect(Screen.width/6,Screen.height/2+Screen.height/28,60,30),"",noStyle)){
				checkCheck(2,Screen.width/6,Screen.height/2+Screen.height/28);
			}
			if(GUI.Button(new Rect(0,4*Screen.height/5,70,50),"")){
				checkCheck(3,0,4*Screen.height/5);
			}
			if(GUI.Button(new Rect(2*Screen.width/5-Screen.width/10,4*Screen.height/5,220,60),"",noStyle)){
				checkCheck(4,2*Screen.width/5-Screen.width/10,4*Screen.height/5);
			}
		}
		else if (num.num==9){
			if(GUI.Button(new Rect(Screen.width/4-Screen.width/45,Screen.height/5+Screen.height/30,20,20),"",noStyle)){
				checkCheck(0,Screen.width/4-Screen.width/45,Screen.height/5+Screen.height/30);
			}
			if(GUI.Button(new Rect(3*Screen.width/7-Screen.width/25,3*Screen.height/8-Screen.height/15,65,50),"",noStyle)){
				checkCheck(1,3*Screen.width/7-Screen.width/25,3*Screen.height/8-Screen.height/15);
			}
			if(GUI.Button (new Rect(4*Screen.width/7+Screen.width/12,Screen.height/2+Screen.height/18,10,10),"",noStyle)){
				checkCheck(2,4*Screen.width/7+Screen.width/12,Screen.height/2+Screen.height/18);
			}
			if(GUI.Button(new Rect(0,4*Screen.height/5,70,50),"",noStyle)){
				checkCheck(3,0,4*Screen.height/5);
			}
			if(GUI.Button(new Rect(2*Screen.width/5-Screen.width/10,3*Screen.height/5,80,60),"",noStyle)){
				checkCheck(4,2*Screen.width/5-Screen.width/10,3*Screen.height/5);
			}
		}
		else if (num.num==10){
			if(GUI.Button(new Rect(3*Screen.width/4-Screen.width/30,3*Screen.height/4-Screen.height/30,20,20),"",noStyle)){
				checkCheck(0,3*Screen.width/4-Screen.width/30,3*Screen.height/4-Screen.height/30);
			}
			if(GUI.Button(new Rect(3*Screen.width/7-Screen.width/25,Screen.height/8-Screen.height/15,275,70),"",noStyle)){
				checkCheck(1,3*Screen.width/7-Screen.width/25,Screen.height/8-Screen.height/15);
			}
			if(GUI.Button (new Rect(4*Screen.width/7+Screen.width/15,3*Screen.height/8-Screen.height/20,20,120),"",noStyle)){
				checkCheck(2,4*Screen.width/7+Screen.width/15,3*Screen.height/8-Screen.height/20);
			}
			if(GUI.Button(new Rect(3*Screen.width/8+Screen.width/40,4*Screen.height/5+Screen.height/30,220,72),"",noStyle)){
				checkCheck(3,3*Screen.width/8+Screen.width/40,4*Screen.height/5+Screen.height/30);
			}
			if(GUI.Button(new Rect(Screen.width/5-Screen.width/20,3*Screen.height/5,160,50),"",noStyle)){
				checkCheck(4,Screen.width/5-Screen.width/20,3*Screen.height/5);
			}
		}
		else if (num.num==11){
			if(GUI.Button(new Rect(3*Screen.width/5-Screen.width/30,2*Screen.height/3,20,20),"",noStyle)){
				checkCheck(0,3*Screen.width/5-Screen.width/30,2*Screen.height/3);
			}
			if(GUI.Button(new Rect(Screen.width/7-Screen.width/30,Screen.height/8-Screen.height/20,50,70),"",noStyle)){
				checkCheck(1,Screen.width/7-Screen.width/30,Screen.height/8-Screen.height/20);
			}
			if(GUI.Button (new Rect(3*Screen.width/10+Screen.width/40,Screen.height/5,50,45),"",noStyle)){
				checkCheck(2,3*Screen.width/10+Screen.width/40,Screen.height/5);
			}
			if(GUI.Button(new Rect(3*Screen.width/8+Screen.width/50,3*Screen.height/8+Screen.height/50,50,30),"",noStyle)){
				checkCheck(3,3*Screen.width/8+Screen.width/50,3*Screen.height/8+Screen.height/50);
			}
			if(GUI.Button(new Rect(7*Screen.width/8,Screen.height/10,20,20),"",noStyle)){
				checkCheck(4,7*Screen.width/8,Screen.height/10);
			}
		}
		else if (num.num==12) {
			if(GUI.Button(new Rect(5*Screen.width/7-Screen.width/30,Screen.height/9,140,110),"",noStyle)){
				checkCheck(0,5*Screen.width/7-Screen.width/30,Screen.height/9);
			}
			if(GUI.Button(new Rect(2*Screen.width/5-Screen.width/15,9*Screen.height/10-Screen.height/15,40,40),"",noStyle)){
				checkCheck(1,2*Screen.width/5-Screen.width/15,9*Screen.height/10-Screen.height/15);
			}
			if(GUI.Button (new Rect(3*Screen.width/10+Screen.width/40,Screen.height/12,200,50),"",noStyle)){
				checkCheck(2,3*Screen.width/10+Screen.width/40,Screen.height/12);
			}
			if(GUI.Button(new Rect(Screen.width/7+Screen.width/20,Screen.height/8+Screen.height/50,50,30),"",noStyle)){
				checkCheck(3,Screen.width/7+Screen.width/20,Screen.height/8+Screen.height/50);
			}
			if(GUI.Button(new Rect(7*Screen.width/8-Screen.width/70,6*Screen.height/7-Screen.height/30,30,30),"",noStyle)){
				checkCheck(4,7*Screen.width/8-Screen.width/70,6*Screen.height/7-Screen.height/30);
			}
		}
		else{
			Debug.Log ("number "+num+" is not allowed");
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
	void checkCheck(int n,int x,int y){
		manyVect[n].x = x;
		manyVect[n].y = y;
		manyButtons[n] = true;     
		
	}
}