using UnityEngine;

public class _2DC_ArrowSelection : MonoBehaviour
{
	public Camera cam;

	private Vector3 pos;

	private float x;

	public float posx;

	private float timeskip;

	private float timemult;

	private void Update()
	{
		if (timeskip < 1f)
		{
			timemult = 1f;
		}
		if (timeskip > 1f)
		{
			timemult = 1.2f;
		}
		if (timeskip > 1.1f)
		{
			timemult = 1.4f;
		}
		if (timeskip > 1.2f)
		{
			timemult = 1.6f;
		}
		if (timeskip > 1.3f)
		{
			timemult = 1.8f;
		}
		if (timeskip > 1.4f)
		{
			timemult = 2f;
		}
		if (timeskip > 1.5f)
		{
			timemult = 2.2f;
		}
		if (timeskip > 1.6f)
		{
			timemult = 2.5f;
		}
		if (timeskip > 1.7f)
		{
			timemult = 2.8f;
		}
		if (timeskip > 1.8f)
		{
			timemult = 3f;
		}
		if (timeskip > 1.9f)
		{
			timemult = 4f;
		}
		if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
		{
			pos = cam.transform.position;
			pos.x += -0.2f;
			cam.transform.position = pos;
		}
		if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
		{
			pos = cam.transform.position;
			pos.x += 0.2f;
			cam.transform.position = pos;
		}
	}

	private void OnMouseDrag()
	{
		pos = cam.transform.position;
		pos.x -= posx * timemult;
		cam.transform.position = pos;
		timeskip += Time.deltaTime;
	}

	private void OnMouseExit()
	{
		timeskip = 0f;
	}

	private void OnMouseUp()
	{
		timeskip = 0f;
	}
}
