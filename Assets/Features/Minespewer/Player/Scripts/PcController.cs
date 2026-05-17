using UnityEngine;

public class PcController : MonoBehaviour
{
    private Player player;
    private Movement playerMove;
    private Mortar mortar;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        playerMove = player.GetComponentInChildren<Movement>();
        mortar = player.GetComponentInChildren<Mortar>();
    }

    void Update()
    {
        if (mortar == null)
            return;

        var axisX = Input.GetAxis("Vertical");
        var axisZ = Input.GetAxis("Horizontal");

        playerMove.Move(new Vector3(axisX / 2 + axisZ / 2, 0, axisX / 2 - axisZ / 2));

        if (Input.GetMouseButtonDown(0))
            mortar.Shoot();
    }
}
