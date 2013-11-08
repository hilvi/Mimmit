using UnityEngine;

public class GameSelectionButton
{
	#region MEMBERS
	public Texture texture;
	public float horizontalOffset;
	public string startSceneName;
	
	private Rect movieRect; // Used for video clip
	private Rect backgroundRect; // Used for borders
	private float floatingHeight;
	#endregion
	
	#region METHODS
	public GameSelectionButton (float x, float y, float width, float height, 
		Texture texture, string startSceneName, float borderWidth)
	{
		this.movieRect = new Rect (x + width / 2f, y - height / 2f, width, height);
		this.texture = texture;
		this.startSceneName = startSceneName;
		
		// Create crude black border
		backgroundRect = new Rect(movieRect);
		backgroundRect.x -= borderWidth;
		backgroundRect.y -= borderWidth;
		backgroundRect.width += borderWidth * 2f;
		backgroundRect.height += borderWidth * 2f;
	}
	
	public Rect CalcRect ()
	{
		Rect __r = new Rect (movieRect);
		__r.x += horizontalOffset;
		__r.y -= floatingHeight;
		return __r;
	}
	
	public Rect CalcBGRect () {
		Rect __r = new Rect (backgroundRect);
		__r.x += horizontalOffset;
		__r.y -= floatingHeight;
		return __r;
	}
	
	public Rect CalcStaticRect() {
		Rect __r = new Rect (backgroundRect);
		__r.x += horizontalOffset;
		return __r;
	}
	
	public void FloatUp() {
		floatingHeight = Mathf.Lerp(floatingHeight, 20f, Time.deltaTime * 10f);
	}
	
	public void FloatBack() {
		floatingHeight = Mathf.Lerp(floatingHeight, 0f, Time.deltaTime * 5f);
	}
	#endregion
}
	
