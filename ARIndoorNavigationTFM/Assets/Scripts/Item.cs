using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string ItemName;
    public Sprite ItemImage;
    public List<string> descripciones;
    public List<Vector3> positions;
}
