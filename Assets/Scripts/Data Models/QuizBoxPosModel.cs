using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuizBoxPosModel
{
    public Vector3 Position;
    public Vector3 Rotation;

    public QuizBoxPosModel(Vector3 position, Vector3 rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}

