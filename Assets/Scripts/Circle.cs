using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle : MonoBehaviour
{
    private Airport airport;
    public float size;

    SpriteRenderer sprite;
	
	void Start ()
    {
        airport = GetComponentInParent<Airport>();
        size = Constants.instance.circleMinSize;

        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = Constants.instance.circleInitialColor;
	}
	
	void Update ()
    {
        if (airport.capacity != 0)
        {
            var ratio = (float)airport.passengers / airport.capacity;
            var min = Constants.instance.circleMinSize;
            size = min + (1 - min) * ratio;
            transform.localScale = new Vector3(size, size, 1.0f);

            //var grad = Constants.instance.circleInitialColor - Constants.instance.circleFinalColor;
            var grad = Constants.instance.circleFinalColor - Constants.instance.circleInitialColor;
            sprite.color = Constants.instance.circleInitialColor + ratio * grad;
        }
        else
        {
            Debug.Log("Capacidade zero!!");
        }
	}
}
