using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarWithDifferential : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    public float maxSteerAngle = 30f;  // มุมการเลี้ยวสูงสุด
    public float maxMotorTorque = 150f;  // แรงบิดสูงสุด
    public float minMotorTorque = 50f;   // แรงบิดต่ำสุดเมื่อเข้าโค้ง
    public float brakeForce = 3000f;     // แรงเบรก

    public WheelCollider frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider;
    public Transform frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform;

    void Start()
    {
        // ตั้งค่าเริ่มต้นสำหรับ WheelCollider
        JointSpring suspensionSpring = new JointSpring();
        suspensionSpring.spring = 3500f;  // ความแข็งของสปริง
        suspensionSpring.damper = 200f;  // การดูดซับการสั่นสะเทือน
        suspensionSpring.targetPosition = 0.5f;  // จุดที่ระบบกันสะเทือนควรอยู่

        frontLeftWheelCollider.suspensionSpring = suspensionSpring;
        frontRightWheelCollider.suspensionSpring = suspensionSpring;

        // ปรับการเสียดทาน
        WheelFrictionCurve forwardFriction = frontLeftWheelCollider.forwardFriction;
        forwardFriction.stiffness = 1.0f;
        frontLeftWheelCollider.forwardFriction = forwardFriction;

        WheelFrictionCurve sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;
        sidewaysFriction.stiffness = 1.0f;
        frontLeftWheelCollider.sidewaysFriction = sidewaysFriction;
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 relativeVector = transform.InverseTransformPoint(target.position);

        // คำนวณมุมการเลี้ยวของล้อหน้า
        float steer = Mathf.Clamp((relativeVector.x / relativeVector.magnitude) * maxSteerAngle, -maxSteerAngle, maxSteerAngle);
        frontLeftWheelCollider.steerAngle = steer;
        frontRightWheelCollider.steerAngle = steer;

        // คำนวณระยะห่างจาก waypoint
        float distanceToWaypoint = Vector3.Distance(transform.position, target.position);

        // ลดแรงบิดเมื่อเข้าใกล้ waypoint
        float currentMotorTorque = distanceToWaypoint < 10f ? minMotorTorque : maxMotorTorque;

        // คำนวณการหมุนของล้อหลังแบบ differential
        float leftRearWheelTorque = currentMotorTorque;
        float rightRearWheelTorque = currentMotorTorque;

        // หากรถกำลังเลี้ยว, ลดแรงบิดของล้อที่อยู่ด้านในของโค้ง
        if (Mathf.Abs(frontLeftWheelCollider.steerAngle) > 0.1f)
        {
            if (frontLeftWheelCollider.steerAngle > 0)
            {
                leftRearWheelTorque *= 0.9f;  // ลดแรงบิดของล้อซ้าย
                rightRearWheelTorque *= 1.1f; // เพิ่มแรงบิดของล้อขวา
            }
            else if (frontLeftWheelCollider.steerAngle < 0)
            {
                leftRearWheelTorque *= 1.1f;  // เพิ่มแรงบิดของล้อซ้าย
                rightRearWheelTorque *= 0.9f; // ลดแรงบิดของล้อขวา
            }
        }

        // ใช้แรงบิดที่คำนวณไว้กับล้อหลัง
        rearLeftWheelCollider.motorTorque = leftRearWheelTorque;
        rearRightWheelCollider.motorTorque = rightRearWheelTorque;

        // เปลี่ยน waypoint เมื่อใกล้ถึง
        if (distanceToWaypoint < 5f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0;
        }

        // อัปเดตตำแหน่งล้อให้แสดงผลบนหน้าจอ
        UpdateWheelPosition(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPosition(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPosition(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPosition(rearRightWheelCollider, rearRightWheelTransform);

        // เบรกเมื่อรถช้าลง
        if (distanceToWaypoint < 5f)
        {
            frontLeftWheelCollider.brakeTorque = brakeForce;
            frontRightWheelCollider.brakeTorque = brakeForce;
        }
        else
        {
            frontLeftWheelCollider.brakeTorque = 0;
            frontRightWheelCollider.brakeTorque = 0;
        }
    }

    // ฟังก์ชันอัปเดตตำแหน่งของล้อ
    void UpdateWheelPosition(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
