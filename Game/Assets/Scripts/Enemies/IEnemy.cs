using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    /// <summary>
    /// Attack the eneny and deduct 'damage' from their current health
    /// </summary>
    /// <param name="damage"></param>
    void AttackPlayer();

    /// <summary>
    /// Take damage, deduct 'amount' from current health
    /// </summary>
    /// <param name="amount"></param>
    void TakeDamage(float amount);
    
    /// <summary>
    /// When health reaches or falls below 0, call this. Destory object and reward player etc
    /// </summary>
    void Die();
}
