using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money_FxController : MonoBehaviour
{
    public static System.Action<GameObject, IdleNumber> OnFxSpawn;

    [SerializeField]
    private GameObject m_moneyGainFxPrefab = null;

    [SerializeField]
    private Vector3 m_moneyGainFxSpawnOffset = Vector3.up;


    private void OnEnable()
    {
        Manager_Money.OnMoneyGainCreateFx += SpawnFx;
    }

    private void OnDisable()
    {
        Manager_Money.OnMoneyGainCreateFx -= SpawnFx;
    }

    private void SpawnFx(Vector3 fxSpawnPosition, IdleNumber gains_IdleNumber)
    {
        GameObject fx = Instantiate(m_moneyGainFxPrefab);

        fx.transform.position = fxSpawnPosition + m_moneyGainFxSpawnOffset;

        OnFxSpawn?.Invoke(fx, gains_IdleNumber);
    }
}
