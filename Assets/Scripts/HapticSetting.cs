using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TanvasTouch.Model;
using System.IO;

public class HapticSetting : MonoBehaviour {

    [SerializeField]
    private Camera _camera;
    private HapticServiceAdapter mHapticServiceAdapter;
    private HapticView mHapticView;
    private HapticTexture mHapticTexture;
    private HapticMaterial mHapticMaterial;
    private HapticSprite mHapticSprite;
    //private ScreenOrientation _previousOrientation = ScreenOrientation.Unknown;
    //private int _previousWidth = 0;
    //private int _previousHeight = 0;

    // Called at start of application.
    void Start () {

        // create a texture2d
        // white.png is a 150 by 150 pixel image
        Texture2D newTexture = Resources.Load("Textures/customized/white400") as Texture2D;
        int imageHeight = newTexture.height;
        int imageWidth = newTexture.width;

        for (int i = 0; i < imageWidth; i++)
        {
            for (int j = 0; j < imageHeight; j++)
            {
                if ((i + j) % 7 == 0)
                {
                    newTexture.SetPixel(i, j, Color.black);
                }
                if ((i + j) % 7 == 1)
                {
                    newTexture.SetPixel(i, j, new Vector4(0.6F, 0.6F, 0.6F, 1));
                }
                if ((i + j) % 7 == 2)
                {
                    newTexture.SetPixel(i, j, new Vector4(0.3F, 0.3F, 0.3F, 1));
                }
                //float value = Random.value;
                //newTexture.SetPixel(i, j, new Color(value, value, value));
            }

        }

        //Debug.Log("this is the texture: " + newTexture.GetPixel(148, 149));
        byte[] bytes = newTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Textures/customized/createdTexture2" + ".png", bytes);

        //Connect to the service and begin intializing the haptic resources.
        InitHaptics();
        

    }

    // Following Start() this is called in a loop.
    void Update () {
        if (mHapticView != null)
        {
            //Ensure haptic view orientation matches current screen orientation.
            mHapticView.SetOrientation(Screen.orientation);

            //Retrieve x and y position of bunny.
            Mesh _mesh = gameObject.GetComponent<MeshFilter>().mesh;
            Vector3[] _meshVerts = _mesh.vertices;
            for (var i = 0; i < _mesh.vertexCount; ++i)
            {
                _meshVerts[i] = _camera.WorldToScreenPoint(gameObject.transform.TransformPoint(_meshVerts[i]));
            }

            //Set the size and position of the haptic sprite to correspond to bunny's boundry.
            mHapticSprite.SetPosition((int)(_meshVerts[0].x), (int)(_meshVerts[0].y));
            mHapticSprite.SetSize((double)_meshVerts[1].x - _meshVerts[0].x, (double)_meshVerts[1].y - _meshVerts[0].y);
        }

    }

    //initializing the application's haptic resources
    void InitHaptics()
    {
        //Get the service adapter
        mHapticServiceAdapter = HapticServiceAdapter.GetInstance();

        //Create the haptic view with the service adapter instance and then activate it.
        mHapticView = HapticView.Create(mHapticServiceAdapter);
        mHapticView.Activate();

        //Set orientation of haptic view based on screen orientation.
        mHapticView.SetOrientation(Screen.orientation);

        //Retrieve texture data from bitmap.
        string imagePath = "";
        switch (this.gameObject.name)

        {
            case "HapticMesh1":
                imagePath = "Textures/noise/noise_texture4";
                break;

            case "HapticMesh2":
                imagePath = "Textures/noise/noise_texture3";
                break;

            case "HapticMesh3":
                imagePath = "Textures/noise/noise_texture2";
                break;

            case "HapticMesh4":
                imagePath = "Textures/noise/noise_texture1";
                break;

            case "HapticMesh5":
                imagePath = "Textures/noise/noise_texture0";
                break;

        }

        /*
        {
            case "HapticMesh1":
                imagePath = "Textures/checker_highres/checker_hr0";
            break;

            case "HapticMesh2":
                imagePath = "Textures/checker_highres/checker_hr1";
            break;

            case "HapticMesh3":
                imagePath = "Textures/checker_highres/checker_hr2";
            break;

            case "HapticMesh4":
                imagePath = "Textures/checker_highres/checker_hr3";
            break;

            case "HapticMesh5":
                imagePath = "Textures/checker_highres/checker_hr4";
            break;

        }
        */
        /*
        {
            case "HapticMesh1":
                imagePath = "Textures/sandpaper/sandpaper0";
            break;

            case "HapticMesh2":
                imagePath = "Textures/sandpaper/sandpaper1";
            break;

            case "HapticMesh3":
                imagePath = "Textures/sandpaper/sandpaper2";
            break;

            case "HapticMesh4":
                imagePath = "Textures/sandpaper/sandpaper3";
            break;

            case "HapticMesh5":
                imagePath = "Textures/sandpaper/sandpaper4";
            break;

        }
        */

        Texture2D _texture = Resources.Load(imagePath) as Texture2D;
        byte[] textureData = TanvasTouch.HapticUtil.CreateHapticDataFromTexture(_texture, TanvasTouch.HapticUtil.Mode.Brightness);

        //Create a haptic texture with the retrieved texture data.
        mHapticTexture = HapticTexture.Create(mHapticServiceAdapter);
        mHapticTexture.SetSize(_texture.width, _texture.height);
        mHapticTexture.SetData(textureData);

        //Create a haptic material with the created haptic texture.
        mHapticMaterial = HapticMaterial.Create(mHapticServiceAdapter);
        mHapticMaterial.SetTexture(0, mHapticTexture);

        //Create a haptic sprite with the haptic material.
        mHapticSprite = HapticSprite.Create(mHapticServiceAdapter);
        mHapticSprite.SetMaterial(mHapticMaterial);

        //Add the haptic sprite to the haptic view.
        mHapticView.AddSprite(mHapticSprite);
    }

    void OnDestroy()
    {
        mHapticView.Deactivate();
    }
}
