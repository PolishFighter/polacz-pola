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
                    return new Color(233f/255f, 79f/255f, 55f/255f);
                case SquareColor.yellow:
                    return new Color(251f/255f, 175f/255f, 0/255f);
                case SquareColor.blue:
                    return new Color(18f/255f, 130f/255f, 162f/255f);
                case SquareColor.green:
                    return new Color(80f/255f, 210f/255f, 90f/255f);
                default:
                    return Color.black;
            }
        }
    }

    public rowData[] rows = new rowData[8];
}
