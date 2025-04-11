using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public GameObject pausePanel;

    [SerializeField] private Slider energySlider;
    [SerializeField] private TMP_Text energyText; 
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
}
