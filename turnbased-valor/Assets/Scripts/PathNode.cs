using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private PathNode cameFromPathNode;
    private int gCost;
    private int hCost;
    private int fCost;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost()
    { return gCost; }

    public int GetHCost()
    { return hCost; }
    
    public int GetFCost()
    { return fCost; }

}
