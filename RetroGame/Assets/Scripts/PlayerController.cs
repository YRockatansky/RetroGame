using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Usually, I wouldn't use any Menu handler in player controller. But we needed to handle this quickly. And I didn't want to make things complicated for you
/// So, this is for nearly whole player controller.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Menu handlers
    public GameObject GameOverMenu;
    public GameObject StartGameMenu;
    public GameObject MusicPlayer;

    // Private 
    private int Lane;
    private Vector3 BaselineStartPosition;
    private Vector3 targetVector;

    // Required to avoid key bouncing caused by your keyboard.
    private bool triggerLock = false;
    
    // Set predefined values
    void Start()
    {
        Time.timeScale = 0;  // Stop time..
        Lane = 1;                // Start location is 1.  Means middle lane.
        BaselineStartPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        targetVector = BaselineStartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Always try to change the lane. If target is same, nothing will happen anyway.
        this.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetVector, 20f * Time.deltaTime);
        // Cheesy drift animation..
        this.transform.LookAt(new Vector3(targetVector.x, targetVector.y, targetVector.z + 35f) );

        // If position is same, remove the lock.
        if (this.transform.position == targetVector)
        {
            triggerLock = false;
        }

        // If locked, don't do anything. It can create some unwanted problems.
        if (!triggerLock)
        {
            // If left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (Lane > 0)
                {
                    // Decrease lane and move player. Also, lock the input recieving stuff
                    Lane--;
                    movePlayer();
                    triggerLock = true;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (Lane < 2)
                {
                    Lane++;
                    movePlayer();
                    triggerLock = true;
                }
            }
        }
    }

    private void movePlayer()
    {
        // Change lane. It basically means changin the target location. Moving is handled from the update field anyway
        switch (Lane)
        {
            case 0:
                targetVector = new Vector3(BaselineStartPosition.x - 10, BaselineStartPosition.y, BaselineStartPosition.z);
                break;
            case 1:
                targetVector = new Vector3(BaselineStartPosition.x, BaselineStartPosition.y, BaselineStartPosition.z);
                break;
            case 2:
                targetVector = new Vector3(BaselineStartPosition.x + 10, BaselineStartPosition.y, BaselineStartPosition.z);
                break;
        }
       // Debug.Log("Lane changed " + Lane);
    }

    // Go with game. Set menus inactive and make the time flow..
    public void StartGame()
    {
        StartGameMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        MusicPlayer.SetActive(true);
        Time.timeScale = 1;
    }

    // Well thats a bit tricky.
    public void RestartGame()
    {
        // Reload the scene. Thats not what I do usually. But cheapeast way to achieve this. If you have some performance issues, go with commented section.
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        /*
        StartGameMenu.SetActive(false);
        GameOverMenu.SetActive(false);

        Lane = 1;
        this.transform.position = BaselineStartPosition;
        targetVector = BaselineStartPosition;
        Time.timeScale = 1;
        */
    }

    // If enter, game over. No need to check. We know who entered..
    public void OnTriggerEnter(Collider other)
    {
        GameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
