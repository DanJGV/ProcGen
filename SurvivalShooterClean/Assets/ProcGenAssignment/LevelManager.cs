using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    // can't see 2D array in inspector
    public Transform[] roomSpawnersRow0;
    public Transform[] roomSpawnersRow1;
    public Transform[] roomSpawnersRow2;
    public Transform[] roomSpawnersRow3;

    


    public GameObject[] rooms; // 0 -> No exit, 1 -> LR, 2-> LRB, 3 -> LRT, 4 -> LRTB

    public int testRow = 0;
    public int testColumn = 0;
    public int testType = 0; 

    int direction;
    public float moveAmount;
    float spawnTime;
    public float startSpawnTime = 0.25f;

    public float minX;
    public float maxX;
    public float minZ;
    bool stopSpawn;
    public int currentIndex;
    public LayerMask room;
  
    // Use this for initialization
    void Start () {

        CreateBoundaries();
        int randomStartPos = Random.Range(0, roomSpawnersRow0.Length);
        
        transform.position = roomSpawnersRow0[randomStartPos].position;
        testColumn = randomStartPos;
        Instantiate(rooms[2], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
	}
	
	// Update is called once per frame
	void Update () {
		
        if(spawnTime <= 0 && stopSpawn == false)
        {
            Move();
            spawnTime = startSpawnTime;
        }
        else
        {
            spawnTime -= Time.deltaTime;
        }

        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            AddRoom(testRow, testColumn, testType);
        }*/
	}

   public void Move()
    {
       
        if (direction == 1 || direction == 2) //Spawns to the right
        {
            if (transform.position.x < maxX)
            {
                
                Vector3 newPos = new Vector3(transform.position.x + moveAmount, 0, transform.position.z);
                transform.position = newPos;

                int randomRoomIndex = Random.Range(1, 3);

                currentIndex = randomRoomIndex;
                Instantiate(rooms[randomRoomIndex], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6); 
                if (direction == 3)
                {
                    
                    direction = 2;
                    
                }
                else if(direction == 4 /*&& (rooms[currentIndex].GetComponent<RoomType>().type == 2 || rooms[currentIndex].GetComponent<RoomType>().type == 4)*/)
                {
                    direction = 5;
                }
                else if(direction == 5 /*&& (rooms[currentIndex].GetComponent<RoomType>().type != 2 || rooms[currentIndex].GetComponent<RoomType>().type != 4)*/)
                {
                    direction = 2;
                }
            }
            else
            {
                
                direction = 5;

            }

        }
      
        else if(direction == 3 || direction == 4) //Spawns to the left
        {
            if (transform.position.x > minX)
            {
                Vector3 newPos = new Vector3(transform.position.x - moveAmount, 0, transform.position.z);
                transform.position = newPos;

                int randomRoomIndex = Random.Range(1, rooms.Length);
                currentIndex = randomRoomIndex;
                Instantiate(rooms[randomRoomIndex], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);

                if(direction == 5 /*&& (rooms[currentIndex].GetComponent<RoomType>().type != 2 || rooms[currentIndex].GetComponent<RoomType>().type != 4)*/)
                {
                    direction = 4;
                    
                }

            }
            else
            {
                direction = 5;
              
            }


        }
        else if(direction == 5) //Spawns down
        {
            if(transform.position.z > minZ)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);

               /* if(roomDetection.GetComponent<RoomType>().type != 3 && roomDetection.GetComponent<RoomType>().type != 4)
                {
                    roomDetection.GetComponent<RoomType>().DestroyRoom();
                    int randBottomRoom = Random.Range(3, 5);
                    Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                }*/

                //if (rooms[currentIndex].GetComponent<RoomType>().type == 2 || rooms[currentIndex].GetComponent<RoomType>().type == 4)
                //{
                
                Vector3 newPos = new Vector3(transform.position.x, 0, transform.position.z - moveAmount);
                    transform.position = newPos;

                    int randomRoomIndex = Random.Range(3, 5);
                    currentIndex = randomRoomIndex; 
                    Instantiate(rooms[randomRoomIndex], transform.position, Quaternion.identity);

                    direction = Random.Range(1, 6);

                    /*if (direction == 5 && (rooms[currentIndex].GetComponent<RoomType>().type != 2 || rooms[currentIndex].GetComponent<RoomType>().type != 4))
                    {
                        direction = Random.Range(1, 5);
                    }*/

                //}
               
            }
            else
            {
                //Vector3 outOfBoundsRoomPos = new Vector3(transform.position.x, 0, transform.position.z - moveAmount);
                //Instantiate(rooms[0], outOfBoundsRoomPos, Quaternion.identity);
                stopSpawn = true;
            }
           
        }

        
       // Instantiate(rooms[0], transform.position, Quaternion.identity);
        
    }
    public void CreateBoundaries()
    {
        float xIncrease = -6.64f - moveAmount;
        float zIncrease = minZ - moveAmount;
        for (int i = 0; i < 6; i++)
        {
            
            Vector3 outOfBoundsRoomPos = new Vector3(xIncrease, 0, transform.position.z + moveAmount);
            Instantiate(rooms[0], outOfBoundsRoomPos, Quaternion.identity);
            xIncrease += moveAmount;
        }
        for (int i = 0; i < 5; i++)
        {

            Vector3 outOfBoundsRoomPos = new Vector3(minX - moveAmount, 0, zIncrease);
            Instantiate(rooms[0], outOfBoundsRoomPos, Quaternion.identity);
            zIncrease += moveAmount;
        }
        for (int i = 0; i < 5; i++)
        {

            Vector3 outOfBoundsRoomPos = new Vector3(xIncrease-moveAmount, 0, zIncrease - moveAmount);
            Instantiate(rooms[0], outOfBoundsRoomPos, Quaternion.identity);
            zIncrease -= moveAmount;
        }

        xIncrease -= (2 * moveAmount);
        for (int i = 0; i < 4; i++)
        {

            Vector3 outOfBoundsRoomPos = new Vector3(xIncrease, 0, zIncrease);
            Instantiate(rooms[0], outOfBoundsRoomPos, Quaternion.identity);
            xIncrease -= moveAmount;
        }

        //Vector3 outOfBoundsRoomPos2 = new Vector3(transform.position.x - moveAmount, 0, transform.position.z - moveAmount);

        //Instantiate(rooms[0], outOfBoundsRoomPos, Quaternion.identity);
        //Instantiate(rooms[0], outOfBoundsRoomPos2, Quaternion.identity);
    }
    /*public void AddRoom(int row, int column, int roomType)
    {
        Vector3 spawnPos = Vector3.zero;

         if (direction == 1 || direction == 2) //Spawns to the right
        {
            column++;
            spawnPos = roomSpawnersRow0[column].position;
        }
        else if (direction == 3 || direction == 4) //Spawns to the left
        {
            column--;
            spawnPos = roomSpawnersRow0[column].position;

        }


            // figure out position to spawn at
            /*switch(row)
            {
                case 0:
                    spawnPos = roomSpawnersRow0[column].position;
                    break;
                case 1:
                    spawnPos = roomSpawnersRow1[column].position;
                    break;
                case 2:
                    spawnPos = roomSpawnersRow2[column].position;
                    break;
                case 3:
                    spawnPos = roomSpawnersRow3[column].position;
                    break;
            }


            // actually spawn it
            Instantiate(rooms[roomType], spawnPos, transform.rotation);
    }*/
}
