using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveBackground : MonoBehaviour
{

    private GameRaceController _GameRaceController;

    private bool    _hasInstantiate = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _GameRaceController = FindFirstObjectByType(typeof(GameRaceController)) as GameRaceController;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, _GameRaceController._backgroundSpeed, 0);

        if (transform.position.y <= -13.13f)
        {
            Destroy(this.gameObject);
        }
        else if (transform.position.y <= -0.3f)
        {
            if (_hasInstantiate == false)
            {
                _hasInstantiate = true;
                GameObject obj = Instantiate(this.gameObject, new Vector3(transform.position.x, _GameRaceController._instantiateInterval, transform.position.z), Quaternion.Euler(0, 0, 0));
            }
        }
    }
}
