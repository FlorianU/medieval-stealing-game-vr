using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PatrolMovement : MonoBehaviour
{
   public GameObject[] waypoints;
   int currentWp = 0;

   public float gravity = 9.8f;
   [Range(0, 3)]
   public float moveSpeed = 1f;
   [Range(1, 5)]
   public float sprintMultiplier = 1.8f;
   public float noiseDistractionTime = 5f;
   private Vector3 _moveDirection = Vector3.zero;

   private CharacterController _charCont;
   private Animator animator;
   private FieldOfView fov;
   private GameObject player;
   private AudioSource audioData;
   private Vector3 initialPosition;
   private Quaternion initialRotation;

   private bool hasDetected = false;
   private bool hasDetectedNoise = false;
   private Vector3 noisePosition;
   private bool isMoving = true;

   // Start is called before the first frame update
   void Start()
   {
      _charCont = GetComponent<CharacterController>();
      animator = GetComponent<Animator>();
      fov = GetComponent<FieldOfView>();
      audioData = GetComponent<AudioSource>();

      fov.OnDetectionAction += Fov_OnDetectionAction;
      initialPosition = transform.position;
      initialRotation = transform.rotation;
   }

   private void Fov_OnDetectionAction()
   {
      if (!hasDetected)
      {
         player = GameObject.FindGameObjectWithTag("Player");
         hasDetected = true;

         audioData.Play(0);
      }
   }

   public void AlertNoise(Vector3 position)
   {
      hasDetectedNoise = true;
      noisePosition = position;
      StartCoroutine(NoiseAlertCountdown());
   }

   private IEnumerator NoiseAlertCountdown()
   {
      yield return new WaitForSeconds(noiseDistractionTime);

      hasDetectedNoise = false;
   }

   // Update is called once per frame
   void Update()
   {
      if (GameManager.Instance.isPaused)
      {
         return;
      }

      if (!hasDetected)
      {
         if (!hasDetectedNoise)
         {
            if (waypoints.Length > 0)
            {
               // Get the next waypoint if distance is < 1
               if (Vector3.Distance(this.transform.position, waypoints[currentWp].transform.position) < 1)
               {
                  currentWp++;
               }
               if (currentWp >= waypoints.Length)
               {
                  currentWp = 0;
               }

               // Move to current waypoint
               MoveToWaypoint(new Vector3(waypoints[currentWp].transform.position.x, transform.position.y, waypoints[currentWp].transform.position.z));
            }
            else
            {
               if (Vector3.Distance(this.transform.position, initialPosition) < 1)
               {
                  animator.SetBool("isMoving", false);
                  animator.SetBool("isRunning", false);

                  transform.rotation = initialRotation;
               }
               else
               {
                  MoveToWaypoint(initialPosition);
               }
            }
         }
         else
         {
            // walk to noiseDistraction
            var noiseWalkingPosition = new Vector3(noisePosition.x, transform.position.y, noisePosition.z);
            if (Vector3.Distance(this.transform.position, noiseWalkingPosition) < 1)
            {
               animator.SetBool("isMoving", false);
               animator.SetBool("isRunning", false);
            }
            else
            {
               MoveToWaypoint(noiseWalkingPosition);
            }
         }
      }
      else
      {
         // Disable picking up objects and loot
         GameManager.Instance.DisableInteraction();

         if (Vector3.Distance(this.transform.position, player.transform.position) < 1)
         {
            GameManager.Instance.EndGame();
         }

         animator.SetBool("isRunning", true);

         this.transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

         _moveDirection = transform.forward;
         _moveDirection.y -= this.gravity * Time.deltaTime;

         // Move the controller
         _charCont.Move(_moveDirection * Time.deltaTime * moveSpeed * sprintMultiplier);
      }
   }

   private void MoveToWaypoint(Vector3 position)
   {
      // Look at new waypoint
      this.transform.LookAt(position);

      _moveDirection = transform.forward;
      _moveDirection.y -= this.gravity * Time.deltaTime;

      animator.SetBool("isMoving", true);
      animator.SetBool("isRunning", false);

      // Move the controller
      _charCont.Move(_moveDirection * Time.deltaTime * moveSpeed);
   }
}
