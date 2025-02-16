using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FieldOfView : MonoBehaviour
{
   public float radius;
   [Range(0, 360)]
   public float detectionAngle;
   [Range(0, 10)]
   public float detectionTime = 2f;
   public float detectionLevel;

   public GameObject playerRef;

   public LayerMask targetMask;
   public LayerMask obstructionMask;

   public bool canSeePlayer;
   public event Action OnDetectionAction;

   public Transform DamageImagePivot;
   public UnityEngine.UI.Image detectionImg;

   private AudioSource audioData;

   private void Start()
   {
      playerRef = GameObject.FindGameObjectWithTag("Player");
      detectionImg = DamageImagePivot.GetComponentInChildren<UnityEngine.UI.Image>();

      StartCoroutine(FOVRoutine());
   }

   private void Update()
   {
      SetDetectionIndicator();
   }

   private IEnumerator FOVRoutine()
   {
      WaitForSeconds wait = new WaitForSeconds(0.2f);

      while (true)
      {
         yield return wait;
         FieldOfViewCheck();
      }
   }

   private void FieldOfViewCheck()
   {
      var viewingPosition = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);

      // find player in targetMask inside radius
      Collider[] rangeChecks = Physics.OverlapSphere(viewingPosition, radius, targetMask);

      if (rangeChecks.Length != 0)
      {
         Transform target = rangeChecks[0].transform;
         var targetViewingPosition = new Vector3(target.transform.position.x, target.transform.position.y + 1.3f, target.transform.position.z);

         // get direction to target inside radius
         Vector3 directionToTarget = (targetViewingPosition - viewingPosition).normalized;
         float distanceToTarget = Vector3.Distance(viewingPosition, targetViewingPosition);

         // check if target is inside specified viewing angle
         if (Vector3.Angle(transform.forward, directionToTarget) < detectionAngle / 2)
         {
            // check if in between player and viewer there is an obstacle
            if (!Physics.Raycast(viewingPosition, directionToTarget, distanceToTarget, obstructionMask))
            {
               detectionLevel += 0.2f + (radius / distanceToTarget / 8);
               canSeePlayer = true;

               if (detectionLevel >= detectionTime)
               {
                  OnDetectionAction.Invoke();
               }
            }
            else
            {
               canSeePlayer = false;
            }
         }
         else
         {
            canSeePlayer = false;
         }
      }
      else if (canSeePlayer)
      {
         canSeePlayer = false;
      }

      if (!canSeePlayer && detectionLevel > 0)
      {
         detectionLevel -= 0.2f;
      }
   }

   // Control detection indicator
   private void SetDetectionIndicator()
   {
      if (detectionLevel > 0)
      {
         if (!DamageImagePivot.gameObject.activeSelf)
         {
            // Activate detectionIndicator if detectionLevel present
            DamageImagePivot.gameObject.SetActive(true);
         }

         // Calculate rotation of indicator relative to detecting patrols position
         float angle = Vector3.SignedAngle((new Vector3(transform.position.x, 1.3f, transform.position.z) - new Vector3(playerRef.transform.position.x, 1.3f, playerRef.transform.position.z)).normalized, playerRef.transform.forward, Vector3.up);
         DamageImagePivot.transform.localEulerAngles = new Vector3(0, 0, angle);

         // Change transparency of detection indicator relative to detectionLevel
         var tempColor = detectionImg.color;
         tempColor.a = detectionLevel / detectionTime;
         detectionImg.color = tempColor;
      }
      else if (DamageImagePivot.gameObject.activeSelf)
      {
         // Deactivate detectionIndicator if no detectionLevel present
         DamageImagePivot.gameObject.SetActive(false);
      }
   }
}