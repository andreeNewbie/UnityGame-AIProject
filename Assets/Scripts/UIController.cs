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

    public GameObject pausePanel;
    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
        } else{
            Instance = this;
        }
    }
    
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
}
