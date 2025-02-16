using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.UI;

public class TreasureInteraction : MonoBehaviour
{
   public ParticleSystem vanishingParticles;
   public AudioSource audioSource;
   public float value;
   private HandGrabInteractable handGrab;

   private GameManager gameManager;

   // Start is called before the first frame update
   void Start()
   {
      gameObject.GetComponent<Outline>().enabled = false;
      handGrab = gameObject.GetComponentInChildren<HandGrabInteractable>();
      gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
   }

   // Update is called once per frame
   void Update()
   {
      if (gameManager.canInteract)
      {
         if (InteractableState.Select == handGrab.State)
         {
            // Make loot disappear on click
            audioSource.Play(0);
            vanishingParticles.Play();
            gameObject.SetActive(false);
            gameManager.IncreaseScore(value);
         }
         else if (InteractableState.Hover == handGrab.State)
         {
            gameObject.GetComponent<Outline>().enabled = true;
         }
         else
         {
            gameObject.GetComponent<Outline>().enabled = false;
         }
      }
   }
}
