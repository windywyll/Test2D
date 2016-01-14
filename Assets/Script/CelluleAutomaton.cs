using UnityEngine;
using System.Collections;

public class CelluleAutomaton : MonoBehaviour {

    public GameObject prefab;
    Texture2D texture;
    Color[] colorList;
    Color[] colorListBuffer;
    int width = 50;
    int height = 256;
    int iterationCount = 0;
    float lastIterationTime = 0;
    float iterationDelay = 0.1f;

	// Use this for initialization
	void Start () {
	    texture = new Texture2D(width, height);
        colorList = new Color[width * height];
        colorListBuffer = new Color[width * height];
        for(int i = 0; i<width*height; i++)
        {
            colorList[i] = Random.Range(0f, 1f) > 0.4f ? Color.white : Color.black;
        }

        texture.SetPixels(colorList);
        texture.Apply(false);
        texture.filterMode = FilterMode.Point;

        this.GetComponent<Renderer>().material.mainTexture = texture;
	}
	
	// Update is called once per frame
	void Update () {
	    if(lastIterationTime + iterationDelay < Time.time)
        {
            //UpdateCubes();
            GameOfLife();
            lastIterationTime = Time.time;
            iterationCount++;
        }

        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                int x = (int)(hit.textureCoord.x * width);
                int y = (int)(hit.textureCoord.y * height);
                int index = GetIndexOfPosition(x, y);
                if(x>=0 && x < width && y>=0 && y<height)
                {
                    colorList[index] = Color.white;
                    colorListBuffer[index] = Color.white;
                    texture.SetPixel(x, y, Color.white);
                    texture.Apply();
                }
            }
        }
	}

    bool isAlive(Color color)
    {
        return color == Color.white;
    }

    bool isCellALive(int index)
    {
        if(index >= 0 && index < width*height)
        {
            return isAlive(colorList[index]);
        }
        return false;
    }

    Vector2 GetPositionOfIndex(int index)
    {
        return new Vector2(index % width, index / width);
    }

    int GetIndexOfPosition(int x, int y)
    {
        return x + y * width;
    }

    int HowManyNeighbor(int index)
    {
        int neighborCount = 0;
        Vector2 position = GetPositionOfIndex(index);
        for (int x=-1; x<=1 ; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x==0 && y==0)
                {
                }
                else
                {
                    int indexNeighbor = GetIndexOfPosition( (int)position.x + x, (int)position.y + y );
                    if (isCellALive(indexNeighbor))
                    {
                        neighborCount++;
                    }
                }
            }
        }

        return neighborCount;
    }

    void GameOfLife()
    {
        int index = 0;
        foreach (Color col in colorList)
        {
            int neighborCount = HowManyNeighbor(index);
            if(isCellALive(index))
            {
                if(neighborCount != 2 && neighborCount != 3)
                {
                    colorListBuffer[index] = Color.black;
                }
                else
                {
                    colorListBuffer[index] = Color.white;
                }
            }
            else
            {
                if(neighborCount == 3)
                {
                    colorListBuffer[index] = Color.white;
                }
                else
                {
                    colorListBuffer[index] = Color.black;
                }
            }

            index ++;
        }

        for (int i = 0; i < colorList.Length; i++)
        {
            colorList[i] = colorListBuffer[i];
        }
            
        texture.SetPixels(colorList);
        texture.Apply(false);
    }

    void UpdateCubes()
    {
        for(int i = 0; i <colorList.Length; i++){
            if(isCellALive(i))
            {
                Vector2 position = GetPositionOfIndex(i);
                GameObject.Instantiate(prefab, new Vector3(position.x, iterationCount, position.y), Quaternion.identity);
            }
        }
    }

}
