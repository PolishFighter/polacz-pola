using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquareColor
{
    red, yellow, blue, green
}

[System.Serializable]
public class ArrayLayout
{
    [System.Serializable]
    public struct rowData
    {
        public SquareColor[] row;
        public Color GetColor(int i)
        {
            switch(row[i])
            {
                case SquareColor.red:
                    return Color.red;
                case SquareColor.yellow:
                    return Color.yellow;
                case SquareColor.blue:
                    return Color.blue;
                case SquareColor.green:
                    return Color.green;
                default:
                    return Color.black;
            }
        }
    }

    public rowData[] rows = new rowData[8];
}
