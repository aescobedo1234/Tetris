using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public interface ITetrimino
{
    TetriminoType GetType();
    //push tetrimino down
    void PushDown();

    bool CheckBoard();
    //sets tetrimino type active
    void SetActive(bool value);
    bool IsActive();
    
    float GetRotation();

    Vector3 GetPivot();
    Vector3 GetRoot();
}