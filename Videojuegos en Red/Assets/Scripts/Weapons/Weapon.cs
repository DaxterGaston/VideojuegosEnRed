using Photon.Pun;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponData _data;
    [SerializeField] private Transform _point;
    

    private float _shootTimer;

    public void Shoot()
    {
        if (_shootTimer <= 0f)
        {
            // Dispara
            PhotonNetwork.Instantiate("Proyectile", transform.position, transform.rotation);
            _shootTimer = _data.RateOfFire;
        }
        else
        {
            _shootTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Obtengo la posicion del mouse para apuntar en esa direccion el arma y el punto de disparo.
    /// </summary>
    private void Update()
    {
        transform.LookAt(Input.mousePosition.normalized);
    }

    /// <summary>
    /// En caso de colisionar con una pared, destruyo el proyectil.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //TODO: Definir si al impactar con otro jugador el proyectil pasaria a destruirse o podria atravesar jugadores.
        //Lo mismo para las paredes.
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Player"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
