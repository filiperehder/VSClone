using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RewardCard : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button cardButton;
    
    [Header("Visual Feedback")]
    [SerializeField] private Color hoverColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private float hoverScale = 1.1f;
    
    private Reward reward;
    private RewardUIManager uiManager;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        if (cardButton == null) cardButton = GetComponent<Button>();
    }

    public void Setup(Reward reward, RewardUIManager manager)
    {
        this.reward = reward;
        this.uiManager = manager;

        titleText.text = reward.title;
        descriptionText.text = reward.description;
        
        // Se a recompensa tiver um ícone
        if (reward.icon != null)
        {
            iconImage.sprite = reward.icon;
            iconImage.gameObject.SetActive(true);
        }
        else
        {
            iconImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        uiManager.OnRewardSelected(reward);
    }

    // Animações e feedback visual
    public void OnHoverEnter()
    {
        transform.localScale = originalScale * hoverScale;
        cardButton.targetGraphic.color = hoverColor;
    }

    public void OnHoverExit()
    {
        transform.localScale = originalScale;
        cardButton.targetGraphic.color = Color.white;
    }
}