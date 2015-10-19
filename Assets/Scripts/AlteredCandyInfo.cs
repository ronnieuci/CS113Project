using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AlteredCandyInfo
{
    private List<GameObject> newBlock { get; set; }
    public int MaxDistance { get; set; }

    /// <summary>
    /// Returns distinct list of altered candy
    /// </summary>
    public IEnumerable<GameObject> AlteredBlock
    {
        get
        {
            return newBlock.Distinct();
        }
    }

    public void AddBlock(GameObject go)
    {
        if (!newBlock.Contains(go))
            newBlock.Add(go);
    }

    public AlteredCandyInfo()
    {
        newBlock = new List<GameObject>();
    }
}
