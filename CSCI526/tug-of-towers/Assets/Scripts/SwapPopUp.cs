// using UnityEngine;
// using UnityEngine.UI;

// public class PopupManager : MonoBehaviour
// {
//     public GameObject swapPopUp;
//     private bool isPaused = false;

//     private void Start()
//     {
//         TryTriggerPopupWithProbability(0.7f); 
//     }
//     private void TryTriggerPopupWithProbability(float probability)
//     {
//         float randomValue = Random.value; 
//         Debug.Log("Random Value: " + randomValue);

//         if (randomValue <= probability) 
//         {
//             swapPopUp.SetActive(false);
//             Invoke("TriggerRandomPopup", Random.Range(10, 30));
//             Debug.Log("Popup triggered!");
//         }
//         else
//         {
//             Debug.Log("Popup not triggered.");
//         }
//     }

//     private void TriggerRandomPopup()
//     {
//         if (!isPaused)
//         {
//             swapPopUp.SetActive(true);
//             Time.timeScale = 0;
//             isPaused = true;
//         }
//     }

//     public void ClosePopup()
//     {
//         swapPopUp.SetActive(false);
//         Time.timeScale = 1;
//         isPaused = false;

//         //Invoke("TriggerRandomPopup", Random.Range(10, 30));
//     }
// }
