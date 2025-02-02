using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this to use TextMeshPro

public class DisplayPlayerNames : MonoBehaviour
{
    public TMP_Text attackerNameText;
    public TMP_Text defenderNameText;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the names stored in GameVariables and set them as text
        attackerNameText.text = GameVariables.attackerName.ToUpper();
        defenderNameText.text = GameVariables.defenderName.ToUpper();
    }
}
