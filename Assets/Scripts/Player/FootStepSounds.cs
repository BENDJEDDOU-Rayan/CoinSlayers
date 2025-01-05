using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSounds : MonoBehaviour
{
    public void playSound()
    {
        SoundManager.PlaySound(SoundType.PLAYER_FOOTSTEP_GRASS, 0.2f);
    }
}
