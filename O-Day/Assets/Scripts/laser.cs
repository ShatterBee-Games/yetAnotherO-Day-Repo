using UnityEngine;

public class laser : MonoBehaviour
{
    [SerializeField] private float Distance = 100;
    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;
    Transform m_Transform;


    private void Update()
    {
        ShootLaser();
    }
    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
    }

    void ShootLaser()
    {
        //optimized zoe
        RaycastHit2D hit;
        Vector3 firePointPosition = laserFirePoint.position;
        Vector3 rightDirection = transform.right;
        
        if (Physics2D.RaycastNonAlloc(m_Transform.position, rightDirection, new RaycastHit2D[1], 1) > 0)
        {
            hit = Physics2D.Raycast(firePointPosition, rightDirection);
            Draw2dRay(firePointPosition, hit.point);
        }
        else
        {
            Draw2dRay(firePointPosition, rightDirection * Distance);
        }
    }


    void Draw2dRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }
}
