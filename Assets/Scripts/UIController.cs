using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private Slider energySlider;
    [SerializeField] private TMP_Text energyText; 

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText; 
    [SerializeField] private Slider goldfishSlider;
    [SerializeField] private TMP_Text goldfishText; 
    public GameObject pausePanel;
    
    public void UpdateEnergySlider(float current, float max){
        energySlider.value = Mathf.RoundToInt(current); // Round the current energy value to the nearest integer
        energySlider.maxValue = max;
        energyText.text = energySlider.value + "/" + energySlider.maxValue; // Update the text to show current and max energy
    }

    public void UpdateHealthSlider(float current, float max){
        healthSlider.value = Mathf.RoundToInt(current); // Round the current energy value to the nearest integer
        healthSlider.maxValue = max;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue; // Update the text to show current and max energy
    }

    public void UpdateGoldfishSlider(int current, int max){
        goldfishSlider.value = Mathf.RoundToInt(current); // Round the current energy value to the nearest integer
        goldfishSlider.maxValue = max;
        if ( goldfishSlider.value == goldfishSlider.maxValue && healthSlider.value < healthSlider.maxValue){
            PlayerController.Instance.UpdateGoldfish(-5); // Update the goldfish count in GameManager
            PlayerController.Instance.Heal();
            GameManager.Instance.goldfishCounter -= 5; // Update the goldfish counter in GameManager
        }
        else
            goldfishText.text = goldfishSlider.value + "/" + goldfishSlider.maxValue; // Update the text to show current and max energy
    }

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
        } else{
            Instance = this;
        }
    }

    public bool IsFull()
    {
        return goldfishSlider.value == goldfishSlider.maxValue; 
    }
}
