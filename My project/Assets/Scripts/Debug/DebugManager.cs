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
    // Start is called before the first frame update
    //Hello Testing Testing
    void Start()
    {
        _canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if on click
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            int id = CreatureManager.instance.GetCreatureAt(GameManager.Instance.getGrid().WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            if (id != -1)
            {
                _creature = CreatureManager.instance.GetCreature(id);
                _canvas.enabled = true;
                SetTextInfo();
                CameraControl.Instance.SetFollow(_creature.transform);
            }
        }
        //int id = CreatureManager.instance.GetCreatureAt();
        //_creature = CreatureManager.instance.GetCreature(id);
    }

    private void SetTextInfo()
    {
        //energy / max energy
        //age
        //sight
        //speed

        string stats = "";
        stats += "ID:\t" +_creature.GetID() + "\n";
        stats += "Energy:" +_creature.data.Current_energy + " / " + _creature.data.Energy + "\n";
        stats += "Age:\t" +_creature.GetAge() + "\n";
        stats += "Sight Range:" +_creature.data.Sight_range + "\n";
        stats += "Speed:"+_creature.data.Speed;

        _text.text = stats;
    }
}
