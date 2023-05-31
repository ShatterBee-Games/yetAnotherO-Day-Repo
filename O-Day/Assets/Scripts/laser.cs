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
        if (Physics2D.Raycast(m_Transform.position, transform.right))
        {
            RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, transform.right);
            Draw2dRay(laserFirePoint.position, _hit.point);
        }
        else
        {
            Draw2dRay(laserFirePoint.position, laserFirePoint.transform.right * Distance);
        }
    }


    void Draw2dRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }
}
