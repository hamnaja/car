using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public TimeAttackTimer timerController; // กำหนดให้สามารถลาก object ของ TimerController ได้จาก Inspector
    private bool isFinishLineCrossed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFinishLineCrossed)
        {
            isFinishLineCrossed = true;
            Debug.Log("Player crossed the finish line!");
            if (timerController != null)
            {
                timerController.OnCrossFinishLine(); // เรียกฟังก์ชันจาก TimerController
            }
            else
            {
                Debug.LogError("TimerController is not assigned!");
            }
        }
    }
}