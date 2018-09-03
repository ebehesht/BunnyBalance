using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TanvasTouch.Model;

//[System.Serializable]
public class HapticSetting : MonoBehaviour
{

    [SerializeField]
    private Camera _camera;
    private HapticServiceAdapter mHapticServiceAdapter;
    private HapticView mHapticView;
    private HapticTexture mHapticTexture;
    private HapticMaterial mHapticMaterial;
    private HapticSprite mHapticSprite;
    //private ScreenOrientation _previousOrientation = ScreenOrientation.Unknown;
    private int _previousWidth = 0;
    private int _previousHeight = 0;

    //public GameObject hapticOnOffButton;
    //public Sprite hapticsOn;
    //public Sprite hapticsOff;

    //Called at start of application.
    void Start()
    {
        //Connect to the service and begin intializing the haptic resources.
        InitHaptics();
    }
    // Update is called once per frame
    void Update()
    {
        if (mHapticView != null)
        {
            //Ensure haptic view orientation matches current screen orientation.
            mHapticView.SetOrientation(Screen.orientation);

            //Retrieve x and y position of square.
            Mesh _mesh = gameObject.GetComponent<MeshFilter>().mesh;
            Vector3[] _meshVerts = _mesh.vertices;
            for (var i = 0; i < _mesh.vertexCount; ++i)
            {
                _meshVerts[i] = _camera.WorldToScreenPoint(gameObject.transform.TransformPoint(_meshVerts[i]));
            }

            //Set the size and position of the haptic sprite to correspond to square.
            mHapticSprite.SetPosition((int)(_meshVerts[0].x), (int)(_meshVerts[0].y));
            mHapticSprite.SetSize((double)_meshVerts[1].x - _meshVerts[0].x, (double)_meshVerts[1].y - _meshVerts[0].y);
        }

    }

    public sealed class HapticType
    {
        public static readonly string RIBBED = "ribbed";
        public static readonly string CLICK = "click";
        public static readonly string BUMPY = "bumpy";
    }

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
        Texture2D _texture = Resources.Load("noise_texture") as Texture2D;
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

    public void SetHapticViewState()
    {

        Debug.Log("button is clicked");
        if (mHapticView != null)
        {
            if (mHapticView.IsActive())
            {
                mHapticView.Deactivate();
                Debug.Log("Haptic is deactivated");
                //hapticOnOffButton.GetComponent<Image>().sprite = hapticsOff;
                //hapticViewStateText.text = "Activate Haptics";
            }
            else
            {
                mHapticView.Activate();
                Debug.Log("Haptic is activated");
                //hapticOnOffButton.GetComponent<Image>().sprite = hapticsOn;
                //hapticViewStateText.text = "Deactivate Haptics";
            }
        }
    }



    void OnDestroy()
    {
        if (mHapticView != null) mHapticView.Deactivate();
    }
}
