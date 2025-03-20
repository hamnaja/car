using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    public Transform[] waypoints; // จุด Waypoints ที่ AI ต้องขับผ่าน
    private int currentWaypointIndex = 0;
    public float speed = 50f;
    public float turnSpeed = 5f;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // หาเป้าหมาย Waypoint ถัดไป
        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // หมุนรถให้หันไปทาง Waypoint
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // เคลื่อนที่ไปข้างหน้า
        transform.position += transform.forward * speed * Time.deltaTime;

        // เช็กว่าถึง Waypoint แล้วหรือยัง
        if (Vector3.Distance(transform.position, target.position) < 5f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0; // วนกลับไปที่จุดแรก
        }
    }
}
