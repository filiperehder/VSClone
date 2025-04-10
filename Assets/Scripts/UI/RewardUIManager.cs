using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class RewardUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RewardManager rewardManager;
    [SerializeField] private GameObject rewardScreenPanel;
    [SerializeField] private RewardCard rewardCardPrefab;
    [SerializeField] private Transform cardsContainer;
    
    [Header("Animation")]
    [SerializeField] private float cardRevealDelay = 0.2f;
    
    private List<RewardCard> activeCards = new List<RewardCard>();

    private void Start()
    {
        if (rewardManager != null)
        {
            rewardManager.onRewardsGenerated.AddListener(ShowRewards);
        }
        
        // Garante que o painel começa escondido
        rewardScreenPanel.SetActive(false);
    }

    public void ShowRewards(List<Reward> rewards)
    {
        // Limpa cartas antigas
        ClearCards();
        
        // Ativa o painel
        rewardScreenPanel.SetActive(true);
        
        // Cria as novas cartas
        StartCoroutine(RevealCards(rewards));
    }

    private IEnumerator RevealCards(List<Reward> rewards)
    {
        foreach (var reward in rewards)
        {
            // Instancia a carta
            RewardCard card = Instantiate(rewardCardPrefab, cardsContainer);
            card.Setup(reward, this);
            activeCards.Add(card);
            
            // Efeito de reveal
            card.transform.localScale = Vector3.zero;
            // LeanTween.scale(card.gameObject, Vector3.one, 0.3f)
            //     .setEaseOutBack();
            
            yield return new WaitForSeconds(cardRevealDelay);
        }
    }

    public void OnRewardSelected(Reward reward)
    {
        // Aplica a recompensa
        rewardManager.ApplyReward(reward);
        
        // Anima as cartas saindo
        StartCoroutine(HideCards());
    }

    private IEnumerator HideCards()
    {
        foreach (var card in activeCards)
        {
            // LeanTween.scale(card.gameObject, Vector3.zero, 0.2f)
            //     .setEaseInBack();
            //
            yield return new WaitForSeconds(0.1f);
        }
        
        yield return new WaitForSeconds(0.2f);
        
        // Limpa e esconde o painel
        ClearCards();
        rewardScreenPanel.SetActive(false);
        
        // Notifica o WaveManager
        rewardManager.GetComponent<WaveManager>().OnRewardSelected();
    }

    private void ClearCards()
    {
        foreach (var card in activeCards)
        {
            Destroy(card.gameObject);
        }
        activeCards.Clear();
    }
}