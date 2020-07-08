

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RandomRZ = UnityEngine.Random;
using TMPro.EditorUtilities;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameManager 
{
    public Text scoreText;
    public int score;

    public GameManager() { }
    public GameManager(Text text)
    {
        scoreText = text;
    }
}
[Serializable]
public class Area /*область*/
{
    public float x, y, mX, mY, mZ;
    public Area(float x, float y, float mX, float mY)
    {
        this.x = x;
        this.y = y;
        this.mX = mX;
        this.mY = mY;
    }
}
public class Player : MonoBehaviour
{
    public float speed;
    public float speedJump;
    public float _NotInTheGame;
    public bool _ground = false;

    public List<GameObject> _coin;
    public Transform[] _transforms;
    public Camera _camera;
    // serialize class members
    [SerializeField] private Area area = new Area(0,0,0,0);
    [SerializeField] private GameManager gameManager;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        gameManager = new GameManager(gameManager.scoreText);
       // _coin = new List<GameObject>(GameObject.FindGameObjectsWithTag("GameController"));

        InvokeRepeating("SpawnCoin", area.mY, area.mY);
    }
    void SpawnCoin()
    {
       /* _coin[0].transform.position = _transforms[0].position;
        Instantiate(_coin[0], _transforms[0].position, Quaternion.identity);
       

        _coin[1].transform.position = _transforms[1].position;
        Instantiate(_coin[1], _transforms[1].position, Quaternion.identity);
       */

        for (int i = 0; i < _transforms.Length; i++)
        {
            foreach (var item in _coin)
            {
                Instantiate(item, _transforms[i].position, Quaternion.identity);
            }
        }
        StartCoroutine("DeleteCoin");
        if (_coin.Count == 0)
        {
            Debug.Log("Coin " + _coin.Count);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        // камера багыты осы жука дене багытынын жана веткторы x0 y0 z-10f 
        _camera.transform.position = rb.transform.position + new Vector3(area.x,area.y,area.mZ);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(Vector2.up, speedJump);
        }
        else
        {
            GameBeginning(N: _NotInTheGame);
        }
    }
    private void Move()
    {
        float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        rb.AddForce(new Vector2(x, 0f), ForceMode2D.Impulse);
    }
    private void GameBeginning(float N)
    {
        if (transform.position.y <= -N)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
           // Debug.Log("Destroy");
        }
    }

    void Jump(Vector2 vector, float y)
    {

        if (!_ground)
        {
            return;
        }
        rb.AddForce(new Vector2(0, vector.y * y), ForceMode2D.Force);   
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _ground = true;

        if (collision.gameObject.CompareTag("GameController"))
        {
            gameManager.scoreText.text = "Score: " + gameManager.score++.ToString();
            Destroy(collision.gameObject);
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _ground = false;
    }
    /// <summary>
    ///Removed coin
    /// </summary>
    /// <returns> coin removed </returns>
    IEnumerator DeleteCoin()
    {
        for (int i = 0; i < _coin.Count; i++)
        {
            Debug.Log("coin removed");
            _coin.RemoveAt(i);
            yield return new WaitForSeconds(5f);
        }

    }
}
