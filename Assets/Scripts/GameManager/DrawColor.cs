using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawColor : MonoBehaviour
{
    public Shader drawShader;
    public Material drawMaterial;
    public Material myMaterial;
    [SerializeField]
    private List<RenderTexture> _splatMap = new List<RenderTexture>();
    [SerializeField]
    private List<GameObject> _terrain = new List<GameObject>();

    [Range(0f, 500)]
    public float _brushSize;
    [Range(0.01f, 1)]
    public float _texSize;
    [Range(0, 1)]
    public float _brushStrength;
    [SerializeField]
    Texture2D[] splatTexture = new Texture2D[12];

    public List<GameObject> _Terrain { get => _terrain; set => _terrain = value; }

    private void Start()
    {
        //_terrain = GameObject.Find("Ground");
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector("_Color", Color.red);
        drawMaterial.SetTexture("_SplatTex", splatTexture[0]);
        for (int i = 0; i < _terrain.Count; i++)
        {
            myMaterial = _terrain[i].GetComponent<MeshRenderer>().material;
            _splatMap.Add(new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));

          //  Debug.Log(myMaterial);
            myMaterial.SetTexture("_SplatMap", _splatMap[i]);
        }
        
        
    }
    private void Update()
    {
        drawMaterial.SetFloat("_Strength", _brushStrength);
        //drawMaterial.SetFloat("_Size", _brushSize);
        //drawMaterial.SetFloat("_TexSize", _texSize);
    }

    public void DrawOnSplatmap(RaycastHit hit, Color splatMapColor, float size = 400, float _texMultiplier = 1f, float _sizeMultiplier = 1f)
    {

        drawMaterial.SetFloat("_TexSize", _texSize * _texMultiplier);
        drawMaterial.SetFloat("_Size", size * _sizeMultiplier);
        drawMaterial.SetTexture("_SplatTex", splatTexture[Random.Range(0, 11)]);

        int terrainNum = _terrain.IndexOf(hit.collider.gameObject);
        //Debug.Log(terrainNum);

        drawMaterial.SetColor("_Color", splatMapColor);
        drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
        
        RenderTexture temp = RenderTexture.GetTemporary(_splatMap[terrainNum].width, _splatMap[terrainNum].height, 0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(_splatMap[terrainNum], temp);
        Graphics.Blit(temp, _splatMap[terrainNum], drawMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }

    private void OnGUI()
    {
        ////USE TO VIEW A SPLATMAP
        //GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatMap[0], ScaleMode.ScaleToFit, false, 1);
    }
}
