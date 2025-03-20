using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarVectorSteering : MonoBehaviour
{
    public Transform playerCar;  // รถของผู้เล่น (เป้าหมายของ AI)
    public float maxSteerAngle = 30f;  // มุมเลี้ยวสูงสุด
    public float maxMotorTorque = 150f;  // แรงบิดสูงสุด
    public float brakeForce = 3000f;  // แรงเบรก
    public float followDistance = 10f;  // ระยะห่างที่ AI ต้องรักษา

    public WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;

    void FixedUpdate()
    {
        if (playerCar == null) return;

        // คำนวณเวกเตอร์ไปหาผู้เล่น
        Vector3 targetPosition = playerCar.position - playerCar.forward * followDistance;
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // คำนวณมุมที่ต้องเลี้ยว
        float angleToTarget = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);
        float steer = Mathf.Clamp(angleToTarget / 45f * maxSteerAngle, -maxSteerAngle, maxSteerAngle);

        // ตั้งค่ามุมเลี้ยวของล้อหน้า
        frontLeftWheel.steerAngle = steer;
        frontRightWheel.steerAngle = steer;

        // คำนวณระยะห่างจากเป้าหมาย
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // ควบคุมการเร่งและเบรก
        if (distanceToTarget < followDistance * 0.7f)  // ถ้าใกล้เกินไป → เบรก
        {
            rearLeftWheel.motorTorque = 0;
            rearRightWheel.motorTorque = 0;
            rearLeftWheel.brakeTorque = brakeForce;
            rearRightWheel.brakeTorque = brakeForce;
        }
        else  // ถ้าไกลเกินไป → เร่งเครื่อง
        {
            rearLeftWheel.brakeTorque = 0;
            rearRightWheel.brakeTorque = 0;
            float motorTorque = Mathf.Lerp(50f, maxMotorTorque, distanceToTarget / followDistance);
            rearLeftWheel.motorTorque = motorTorque;
            rearRightWheel.motorTorque = motorTorque;
        }
    }
}
