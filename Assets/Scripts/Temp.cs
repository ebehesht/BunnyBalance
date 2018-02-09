using UnityEngine;
using TanvasTouch.Model;

public class Temp : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    private HapticServiceAdapter _hapticServiceAdapter;
    private HapticView _hapticView;
    private HapticTexture _hapticTexture;
    private HapticMaterial _hapticMaterial;
    private HapticSprite _hapticSprite;

    //Called at start of application.
    void Start()
    {
        //Connect to the service and begin intializing the haptic resources.
        InitHaptics();
    }

    //Following Start() this is called in a loop.
    void Update()
    {
        if (_hapticView != null)
        {
            //Ensure haptic view orientation matches current screen orientation.
            _hapticView.SetOrientation(Screen.orientation);

            //Retrieve x and y position of the bunny square
            Mesh _mesh = gameObject.GetComponent<MeshFilter>().mesh;

            Vector3[] _meshVerts = _mesh.vertices;

            for (var i = 0; i < _mesh.vertexCount; ++i)
            {
                _meshVerts[i] = _camera.WorldToScreenPoint(gameObject.transform.TransformPoint(_meshVerts[i]));
            }

            //Set the size and position of the haptic sprite to correspond to the bunny square.
            _hapticSprite.SetPosition((int)(_meshVerts[0].x), (int)(_meshVerts[0].y));
            _hapticSprite.SetSize((double)_meshVerts[1].x - _meshVerts[0].x, (double)_meshVerts[1].y - _meshVerts[0].y);
        }
    }

    private void InitHaptics()
    {
        //Get the service adapter
        _hapticServiceAdapter = HapticServiceAdapter.GetInstance();

        //Create the haptic view with the service adapter instance and then activate it.
        _hapticView = HapticView.Create(_hapticServiceAdapter);
        _hapticView.Activate();

        //Set orientation of haptic view based on screen orientation.
        _hapticView.SetOrientation(Screen.orientation);

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
        _hapticTexture = HapticTexture.Create(_hapticServiceAdapter);
        _hapticTexture.SetSize(_texture.width, _texture.height);
        _hapticTexture.SetData(textureData);

        //Create a haptic material with the created haptic texture.
        _hapticMaterial = HapticMaterial.Create(_hapticServiceAdapter);
        _hapticMaterial.SetTexture(0, _hapticTexture);

        //Create a haptic sprite with the haptic material.
        _hapticSprite = HapticSprite.Create(_hapticServiceAdapter);
        _hapticSprite.SetMaterial(_hapticMaterial);

        //Add the haptic sprite to the haptic view.
        _hapticView.AddSprite(_hapticSprite);

    }

    void OnDestroy()
    {
        _hapticView.Deactivate();
    }
}
