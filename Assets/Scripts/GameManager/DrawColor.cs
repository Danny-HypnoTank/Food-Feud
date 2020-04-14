using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawColor : MonoBehaviour
{

    [SerializeField]
    private bool useSplatMap;

    public Shader drawShader;
    public Shader clearShader;
    public Shader transparentShader;
    public Material drawMaterial;
    public Material clearMaterial;
    public Material transparentMaterial;
    public Material myMaterial;
    [SerializeField]
    private List<RenderTexture> _splatMap = new List<RenderTexture>();
    [SerializeField]
    private List<RenderTexture> _transparentMap = new List<RenderTexture>();
    [SerializeField]
    private List<GameObject> _terrain;
    [Range(0f, 500)]
    public float _brushSize;
    [Range(0.01f, 1)]
    public float _texSize;
    [Range(0, 1)]
    public float _brushStrength;
    [SerializeField]
    Texture2D[] splatTexture = new Texture2D[12];

    [SerializeField]
    ManageGame manageGame;
    

    public List<GameObject> _Terrain { get => _terrain; set => _terrain = value; }

    private void Start()
    {
        //Turn sprites into Texture2D
        for(int i = 0; i < splatSprite.Length; i++)
        {
            splatTexture[i] = new Texture2D((int)splatSprite[i].rect.width, (int)splatSprite[i].rect.height);

            

            var pixels = splatSprite[i].texture.GetPixels((int)splatSprite[i].textureRect.x, (int)splatSprite[i].textureRect.y, (int)splatSprite[i].textureRect.width, (int)splatSprite[i].textureRect.height);
            splatTexture[i].SetPixels(pixels);
            splatTexture[i].wrapMode = TextureWrapMode.Clamp;
            splatTexture[i].Apply();
            
        }

        ///To go back to the unoptimized painting, comment the next line out, go to "manage game" and uncomment the line below the "terrain" line
        _terrain.Add(GameObject.Find("Floor"));
        drawMaterial = new Material(drawShader);
        clearMaterial = new Material(clearShader);

        transparentMaterial = new Material(transparentShader);

        drawMaterial.SetVector("_Color", Color.red);
        drawMaterial.SetTexture("_SplatTex", splatTexture[0]);
        clearMaterial.SetTexture("_SplatTex", splatTexture[0]);
        transparentMaterial.SetTexture("_SplatTex", splatTexture[0]);

        for (int i = 0; i < _terrain.Count; i++)
        {
            if(_terrain[i].GetComponent<Renderer>().materials.Length > 1)
            {
                Material[] _tempMaterial = _terrain[i].GetComponent<MeshRenderer>().materials;
                myMaterial = _tempMaterial[1];
               
            }
            else
            {
                myMaterial = _terrain[i].GetComponent<MeshRenderer>().material;
            }

            RenderTexture rend = new RenderTexture(1024 * 2, 1024 * 2, 0, RenderTextureFormat.ARGBFloat);
            rend.name = "RenderTex " + i;
            _splatMap.Add(rend);
            RenderTexture rend2 = new RenderTexture(1024 * 2, 1024 * 2, 0, RenderTextureFormat.ARGBFloat);
            rend2.name = "RenderTex2 " + i;
            _transparentMap.Add(rend2);

          //  Debug.Log(myMaterial);
            myMaterial.SetTexture("_SplatMap", _splatMap[i]);
            myMaterial.SetTexture("_TransparentMap", _transparentMap[i]);
            MatchPaintToSkin(myMaterial);
        }
        
        
    }

    public void DrawOnSplatmap(RaycastHit hit, int id, Player player, float _sizeMultiplier = 1f, float _rotation = 0)
    {

        int terrainNum = _terrain.IndexOf(hit.collider.gameObject);

        int _currentSplat = UnityEngine.Random.Range(0, splatTexture.Length);
        //int _currentSplat = 3;

        //coreCalc.Instance.CircleLogic(hit, (UInt16)id, terrainNum);

        drawMaterial.SetFloat("_Size", 0.1f * _sizeMultiplier);
        drawMaterial.SetFloat("_Rotation", _rotation);
        drawMaterial.SetTexture("_SplatTex", splatTexture[_currentSplat]);

        transparentMaterial.SetFloat("_Size", 0.1f * _sizeMultiplier);
        transparentMaterial.SetFloat("_Rotation", _rotation);
        transparentMaterial.SetTexture("_SplatTex", splatTexture[_currentSplat]);

        clearMaterial.SetFloat("_Size", 0.1f * _sizeMultiplier);
        clearMaterial.SetFloat("_Rotation", _rotation);
        clearMaterial.SetTexture("_SplatTex", splatTexture[_currentSplat]);


        //Debug.Log(terrainNum);

        drawMaterial.SetColor("_Color", SetColour(id));
        drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
        clearMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
        transparentMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));

        RenderTexture temp = RenderTexture.GetTemporary(_splatMap[terrainNum].width, _splatMap[terrainNum].height, 0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(_splatMap[terrainNum], temp);
        Graphics.Blit(temp, _splatMap[terrainNum], clearMaterial);
        Graphics.Blit(temp, _splatMap[terrainNum], drawMaterial);
        RenderTexture.ReleaseTemporary(temp);

        RenderTexture temp2 = RenderTexture.GetTemporary(_splatMap[terrainNum].width, _splatMap[terrainNum].height, 0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(_transparentMap[terrainNum], temp);
        Graphics.Blit(temp, _transparentMap[terrainNum], transparentMaterial);
        RenderTexture.ReleaseTemporary(temp2);
        // Debug.Log("SplatMapHit:" + _splatMap[terrainNum].name);

        player.playerScore += 1;
    }

    private Color SetColour(int id)
    {

        Color color = new Color(0,0,0,0);

        switch (id)
        {
            case (1):
                {
                    color = new Color(1, 0, 0, 0);
                    break;
                }
            case (2):
                {
                    color = new Color(0, 1, 0, 0);
                    break;
                }
            case (3):
                {
                    color = new Color(0, 0, 1, 0);
                    break;
                }
            case (4):
                {
                    color = new Color(0, 0, 0, 1);
                    break;
                }
        }

        return color;

    }

    private void OnGUI()
    {
        if (useSplatMap)
        {
            ////USE TO VIEW A SPLATMAP
            //GUI.DrawTexture(new Rect(0, 0, 256, 128), _splatMap[0], ScaleMode.ScaleToFit, false, 1);
        }
    }

    void MatchPaintToSkin(Material m)
    {
        for(int i = 1; i <= 4; i++)
        {
            
            if (manageGame.Players.Length < i)
            {
                break;
            }
            else
            {
                
                m.SetColor("_Color_" + i, LookUpSkinColour(manageGame, i));
            }
        }
    }

    private Color LookUpSkinColour(ManageGame mg, int _iteration)
    {
        Color32 color = new Color(0, 0, 0, 0);

        
        

        color = mg.Players[_iteration - 1].SkinColours[mg.Players[_iteration - 1].skinId];

       
        return color;
    }

    
}
