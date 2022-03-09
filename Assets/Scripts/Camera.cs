using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        //y•ûŒü‚È‚µ
        transform.position = new Vector3(player.transform.position.x + offset.x, 0, player.transform.position.z + offset.z);
    }
}
