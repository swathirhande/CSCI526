
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class GoogleFormSubmit : MonoBehaviour
{
    private string formURL = "https://docs.google.com/forms/d/e/1FAIpQLSdQe96I6vwuUjWrW77nIZjvqpjlDf3ZkI5GtlnVV9qxmVMqfw/formResponse";

    // Make the method public so it can be accessed from another script
    public void SubmitData(string sessionId, string winner, int[] attacker, int[] tower, string time, int[] attackerMoney, int[] defenderMoney, float avgEnemy1, float avgEnemy2)
    {
        StartCoroutine(PostToGoogleForm(sessionId, winner, attacker, tower, time, attackerMoney, defenderMoney, avgEnemy1, avgEnemy2));
    }

    private IEnumerator PostToGoogleForm(string sessionId, string winner, int[] attacker,int[] tower, string time, int[] attackerMoney, int[] defenderMoney, float avgEnemy1, float avgEnemy2)
    {
        WWWForm form = new WWWForm();
        string AMoney = string.Join(", ", attackerMoney);
        string DMoney = string.Join(", ", defenderMoney);
        form.AddField("entry.2040210924", sessionId);
        form.AddField("entry.1013643412", winner);
        form.AddField("entry.1293289384", time);
        form.AddField("entry.36132492", attacker[0].ToString());
        form.AddField("entry.1368036508", attacker[1].ToString());
        form.AddField("entry.1838745211", attacker[2].ToString());
        form.AddField("entry.643939521", attacker[3].ToString());
        form.AddField("entry.1054348219", attacker[4].ToString());
        form.AddField("entry.1244488099", attacker[5].ToString());
        form.AddField("entry.1900819260", tower[0].ToString());
        form.AddField("entry.1102537106", tower[1].ToString());
        form.AddField("entry.81736501", tower[2].ToString());
        form.AddField("entry.2109547119", tower[3].ToString());
        form.AddField("entry.365558643", tower[4].ToString());
        form.AddField("entry.1238349847", tower[5].ToString());
        form.AddField("entry.458595264", AMoney);
        form.AddField("entry.1963508772", DMoney);
        form.AddField("entry.329402248", avgEnemy1.ToString());
        form.AddField("entry.1554310974", avgEnemy2.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(formURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Form submission failed: " + www.error);
            }
            else
            {
                Debug.Log("Form submission complete!");
            }
        }
    }
}

//public class GoogleFormSubmit : MonoBehaviour
//{
//    // Replace <form_id> with your Google Form's ID
//    private string formURL = "https://docs.google.com/forms/d/e/1FAIpQLSdQe96I6vwuUjWrW77nIZjvqpjlDf3ZkI5GtlnVV9qxmVMqfw/formResponse";

//    // This method is called when the game ends
//    public void OnGameEnd(string sessionId, string winner, int numAttackers, int numTurrets)
//    {
//        StartCoroutine(PostToGoogleForm(sessionId, winner, numAttackers, numTurrets));
//    }

//    private IEnumerator PostToGoogleForm(string sessionId, string winner, int numAttackers, int numTurrets)
//    {

//    }
//}
