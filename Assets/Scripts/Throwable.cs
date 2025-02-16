using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwable : MonoBehaviour
{
   //public GameObject crosshair1, crosshair2;
   public bool interactable, pickedup;
   public float throwAmount;
   public float noiseRadius;
   public LayerMask guardLayer;

   private Transform objTransform, cameraTrans;
   private Rigidbody objRigidbody;
   private Outline outline;

   private AudioSource audioData;

   private bool isThrown;

   private void Start()
   {
      objRigidbody = GetComponent<Rigidbody>();
      outline = gameObject.GetComponent<Outline>();
      audioData = GetComponent<AudioSource>();
      cameraTrans = GameObject.Find("PlayerCamera").transform;
      objTransform = gameObject.transform;
      
      outline.enabled = false;
   }

   private void OnTriggerEnter(Collider other)
   {
      // Check if item falls on floor (NoiseGenerating Layer)
      if (isThrown && other.gameObject.layer == 9)
      {
         audioData.Play(0);

         isThrown = false;

         Debug.Log("Throwable collided with NoiseGenerating Layer");

         // Check if guards are in range of the noise
         Collider[] rangeChecks = Physics.OverlapSphere(new Vector3(transform.position.x, 1.3f, transform.position.z), noiseRadius, guardLayer);

         // Alert all guards in range to position of noise
         if (rangeChecks.Length > 0)
         {
            foreach (var guard in rangeChecks)
            {
               guard.GetComponent<PatrolMovement>().AlertNoise(transform.position);
            }
         }
      }
   }

   void OnTriggerStay(Collider other)
   {
      if (GameManager.Instance.canInteract)
      {
         // Check collision with camera collider
         if (other.CompareTag("MainCamera"))
         {
            // Enable outline and interaction
            outline.enabled = true;
            interactable = true;
         }
      }
   }

   void OnTriggerExit(Collider other)
   {
      // Check collision with camera collider
      if (other.CompareTag("MainCamera"))
      {
         // Undo interaction mode when leaving element
         if (pickedup == false)
         {
            outline.enabled = false;
            interactable = false;
         }
         if (pickedup == true)
         {
            objTransform.parent = null;
            objRigidbody.useGravity = true;
            interactable = false;
            pickedup = false;
         }
      }
   }

   void Update()
   {
      if (interactable == true)
      {
         // Let player hold items
         if (Input.GetMouseButtonDown(0))
         {
            objTransform.parent = cameraTrans;
            objRigidbody.useGravity = false;
            pickedup = true;
         }
         // Let items fall if not clicked
         if (Input.GetMouseButtonUp(0))
         {
            objTransform.parent = null;
            objRigidbody.useGravity = true;
            pickedup = false;
         }
         // Let player throw items with right click
         if (pickedup == true)
         {
            if (Input.GetMouseButtonDown(1))
            {
               objTransform.parent = null;
               objRigidbody.useGravity = true;
               objRigidbody.velocity = cameraTrans.forward * throwAmount * Time.deltaTime;
               pickedup = false;
               isThrown = true;
            }
         }
      }
   }
}