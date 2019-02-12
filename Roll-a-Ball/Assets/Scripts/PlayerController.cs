using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

// reminder: the in class example was borrowed from and altered a bit for this
public class PlayerController : MonoBehaviour
{

    public float speed;
    public Text countText;
    public Text winText;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private Rigidbody rb;
    private int count;

    //public float jumpHeight;



    private Vector3 direction;

    private float rotationSpeed = 1f;
    private float minY = -60f;
    private float maxY = 60f;
    private float rotationY = 0f;
    private float rotationX = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        setCountText();
        winText.text = "";
    }
    // need one version of update to check every frame for input and apply that input also
    // Fixed update chosen b/c called before physics calcs
    //void FixedUpdate()
    void Update()
    {
        direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        direction = direction.normalized;
        if (direction.x != 0)
            rb.MovePosition(rb.position + transform.right * direction.x * speed * Time.deltaTime);
        if (direction.z != 0)
            rb.MovePosition(rb.position + transform.forward * direction.z * speed * Time.deltaTime);

        rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * rotationSpeed;
        rotationY += Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);
        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        // basic fire setup from the in class exercise
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    //
    void Fire()
    {
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * -30;
        Destroy(bullet, 2.0f);
    }
    // Destroy everything that enters the trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            setCountText();
        }
    }

    void setCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            winText.text = "You Win!";
        }
    }
}
