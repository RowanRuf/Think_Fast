using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public int enemiesLeft = 1;
    private bool canEnd = false;
    private bool clenchedFist = false;
    public GameObject fistTutorialCanvas;
    public MainMenu menu;
    public Slowmotion[] slowmoHands;

    [SerializeField]
    private XRNode xrNodeL = XRNode.LeftHand;
    [SerializeField]
    private XRNode xrNodeR = XRNode.RightHand;

    private List<InputDevice> devices = new List<InputDevice>();

    private InputDevice deviceL;
    private InputDevice deviceR;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    void GetDevices()
    {
        InputDevices.GetDevicesAtXRNode(xrNodeL, devices);
        InputDevices.GetDevicesAtXRNode(xrNodeR, devices);
        deviceL = devices.FirstOrDefault();
        deviceR = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!deviceL.isValid || !deviceR.isValid)
        {
            GetDevices();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!deviceL.isValid || !deviceR.isValid)
        {
            GetDevices();
        }
        //List<InputFeatureUsage> features = new List<InputFeatureUsage>();
        //deviceL.TryGetFeatureUsages(features);
        //deviceR.TryGetFeatureUsages(features);
        //foreach (var feature in features)
        //{
        //    Debug.Log(feature.name);
        //}
        if (noEnemiesLeft())
        {
            canEnd = true;
        }

        bool gripButtonAction = false;
    
        if (canEnd)
        {
            fistTutorialCanvas.SetActive(true);
            if (deviceL.TryGetFeatureValue(CommonUsages.gripButton, out gripButtonAction) && deviceR.TryGetFeatureValue(CommonUsages.gripButton, out gripButtonAction) && gripButtonAction && !clenchedFist)
            {
                clenchedFist = true;
                menu.StartGame();
                foreach(Slowmotion slowmo in slowmoHands)
                {
                    if(slowmo != null)
                    {
                        slowmo.enabled = false;
                    }
                }
                Time.timeScale = 1.0f;
            }
        }
    }

    public bool noEnemiesLeft()
    {
        return enemiesLeft <= 0;
    }
}
