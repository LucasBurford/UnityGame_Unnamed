using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    /// <summary>
    /// Reduce damage dealt to player
    /// </summary>
    void ReduceAttackDamage();

    /// <summary>
    /// Send enemy backwards
    /// </summary>
    void Knockback();

    /// <summary>
    /// Slow enemy move speed
    /// </summary>
    void SlowDown();

    /// <summary>
    /// Set enemy move speed back to normal
    /// </summary>
    void SpeedBackup();

    /// <summary>
    /// Change enemy move speed to certain value
    /// </summary>
    void ChangeMoveSpeed();
}
