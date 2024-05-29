using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    private BaseCreature _creature;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private TMP_Text _text;
    private bool info_opened = false;
    // Start is called before the first frame update
    //Hello Testing Testing
    void Start()
    {
        _canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //int id = CreatureManager.instance.GetCreatureAt();
        //_creatu
        //re = CreatureManager.instance.GetCreature(id);
    }

    private void FixedUpdate()
    {
        if(_creature == null)
        {
            _canvas.enabled = false;
            return;
        }
        if(info_opened) {
            SetTextInfo();
        }
    }

    private void SetTextInfo()
    {
        //energy / max energy
        //age
        //sight
        //speed

        string stats = "";
        stats += "ID:\t" +_creature.data.ID + "\n";
        stats += "Energy:" +_creature.data.Current_energy + " / " + _creature.data.Energy + "\n";
        stats += "Age:\t" +_creature.GetAge() + "\n";
        stats += "Sight Range:" +_creature.data.Sight_range + "\n";
        stats += "Speed:"+_creature.data.Speed + "\n";
        stats += "Action:" + _creature.GetCurrentAction();

        _text.text = stats;
    }
}
