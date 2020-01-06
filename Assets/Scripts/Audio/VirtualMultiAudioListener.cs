

using UnityEngine;
using System.Collections;

namespace Assets.MultiAudioListener
{
    public class VirtualMultiAudioListener : MonoBehaviour
    {
        [Range(0.0f, 20.0f)]public float Volume = 10.0f;

        private void OnEnable()
        {
            MainMultiAudioListener.AddVirtualAudioListener(this);
        }

        private void OnDisable()
        {
            MainMultiAudioListener.RemoveVirtualAudioListener(this);
        }
    }
}
