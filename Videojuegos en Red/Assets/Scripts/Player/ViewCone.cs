using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ViewCone : MonoBehaviour
{
    public float _radius;
    [Range(0f, 360f)]
    public float _angle;
    [SerializeField]
    private LayerMask _objectives;
    [SerializeField]
    private LayerMask _obstacles;
    private Vector2 _viewDirection;
    [SerializeField] private Camera _viewCamera;
    private Character _player;
    public Vector2 DirectionalVector { get { return _viewDirection; } private set { _viewDirection = value; } }

    #region Mesh
    private Mesh mesh;
    [SerializeField]
    private float _meshResolution;
    [SerializeField]
    private MeshFilter viewMeshFilter;

    /// <summary>
    /// Dibujo el campo de vision del personaje.
    /// </summary>
    private void DrawFieldOfView()
    {
        //Defino la cantidad de rayos que se van a dibujar para mostrar el campo de vision.
        int stepCount = Mathf.RoundToInt(_angle * _meshResolution);
        float stepAngleSize = _angle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - _angle / 2 + stepAngleSize * i;
            //Debug.DrawLine(transform.position, transform.position + DireccionDesdeAngulo(angle) * _radius, Color.red);
            ViewCastInfo viewCast = ViewCast(angle);
            viewPoints.Add(viewCast._point);
        }

        int verticesCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[verticesCount];
        int[] triangles = new int[(verticesCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < verticesCount -1 ; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < verticesCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private ViewCastInfo ViewCast(float angle)
    {
        Vector3 direction = DirectionFromAngle(angle);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit,_radius, _obstacles))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        }
        return new ViewCastInfo(false, transform.position + direction * _radius, _radius, angle);
    }

    #endregion

    private void Start()
    {
        mesh = new Mesh();
        mesh.name = "ViewCone Mesh";
        viewMeshFilter.mesh = mesh;
    }

    private void Awake()
    {
        
        _player = GetComponentInParent<Character>();
    }

    private void Update()
    {
        //Roto el objeto hacia el punto en el que se encuentra actualmente el mouse.
        _viewDirection = Input.mousePosition - _viewCamera.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(_viewDirection.x, _viewDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        DrawFieldOfView();
    }
    
    /// <summary>
    /// Obtengo el vector director de la direccion en la que estoy apuntando rotado en la cantidad de grados del campo de vision.
    /// El limite se define con este vector y su equivalente negativo.
    /// </summary>
    /// <param name="angleDegrees"></param>
    /// <returns></returns>
    public Vector3 DirectionFromAngle(float angleDegrees)
    {
        //Tomo en cuenta mi propia rotacion y posicion para el angulo del cono de vision.
        angleDegrees += transform.eulerAngles.z;
        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
    
}
/// <summary>
/// Estructura utilizada para delimitar el campo de vision por obstaculos y/o objetivos.
/// </summary>
struct ViewCastInfo
{
    public bool _hit;
    public Vector3 _point;
    public float _distance;
    public float _angle;

    public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
    {
        _hit = hit;
        _point = point;
        _distance = distance;
        _angle = angle;
    }
}

/// <summary>
/// Estructura utilizada para optimizar la delimitacion del campo de vision.
/// </summary>
struct Edge
{
    public Vector3 _min;
    public Vector3 _max;
    public Edge(Vector3 min, Vector3 max)
    {
        _min = min;
        _max = max;
    }
}
