using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AlteredBlockInfo
{
    private List<GameObject> newBlock { get; set; }
    public int MaxDistance { get; set; }

    /// Returns distinct list of altered Blocks
    public IEnumerable<GameObject> AlteredBlock
    {
        get	{	return newBlock.Distinct();	}
    }

    public void AddBlock(GameObject go){
        if (!newBlock.Contains(go))
            newBlock.Add(go);
    }

    public AlteredBlockInfo(){
        newBlock = new List<GameObject>();
    }
}
