

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Assets.MultiAudioListener
{
    [RequireComponent(typeof(AudioListener))]
    public class MainMultiAudioListener : MonoBehaviour {
    
        private static List<VirtualMultiAudioListener> _virtualAudioListeners=new List<VirtualMultiAudioListener>();
        public static event Action<VirtualMultiAudioListener> OnVirtualAudioListenerAdded;
        public static event Action<VirtualMultiAudioListener> OnVirtualAudioListenerRemoved;
    
        private static MainMultiAudioListener _main = null;
    
        public static MainMultiAudioListener Main
        {
            get
            {
                if(_main==null)CreateMainMultiAudioListener();
                return _main;
            }
        }
    
        private bool _createdByManager = false;

        //AudioSource pool
            //We limit the amount of items in the audio source pool. This number can be changed
        private const int MaximumItemsAudioSourcePool = 256;
        private static Queue<AudioSource> _audioSourcePool=new Queue<AudioSource>();

        //We add the audiosource in the pool, so that it can be reused.
        /// <summary>
        /// With this function you can add an audiosource to a pool to be reused inplace of destroying it
        /// </summary>
        /// <param name="audioSource"></param>
        public static void EnquequeAudioSourceInPool(AudioSource audioSource)
        {
            if (audioSource == null) return;
            if (_audioSourcePool.Count >= MaximumItemsAudioSourcePool)
            {
                //We reached the limit so destroy this one
                Destroy(audioSource.gameObject);
                return;
            }
            audioSource.Stop();
            audioSource.mute = true;
            audioSource.priority = 0;
            audioSource.gameObject.SetActive(false);
            _audioSourcePool.Enqueue(audioSource);
        }

        /// <summary>
        /// This functions gives you an audiosource out of the pool
        /// </summary>
        /// <returns>Audio source object. Can be null if pool is empty</returns>
        //Will be null if no valid audiosource is in pool
        public static AudioSource GetAudioSourceFromPool()
        {
            while (_audioSourcePool.Count>0)
            {
                var audioSource = _audioSourcePool.Dequeue();
                if (audioSource != null)
                {
                    audioSource.gameObject.SetActive(true);
                    return audioSource;
                }
            }
            //No valid audio sources in queque anymore
            return null;
        }

        /// <summary>
        /// With this function you can destroy the entire contents of the audiosource pool
        /// </summary>
        public static void DeleteContentsAudioSourcePool()
        {
            while (_audioSourcePool.Count > 0)
            {
                var audioSource = _audioSourcePool.Dequeue();
                if (audioSource != null)
                {
                    Destroy(audioSource.gameObject);
                }
            }
        }
    
        public static ReadOnlyCollection<VirtualMultiAudioListener> VirtualAudioListeners
        {
            get { return _virtualAudioListeners.AsReadOnly(); }
        } 
    
        public static void AddVirtualAudioListener(VirtualMultiAudioListener virtualAudioListener)
        {
            if(_virtualAudioListeners.Contains(virtualAudioListener))return;
            _virtualAudioListeners.Add(virtualAudioListener);
            if (OnVirtualAudioListenerAdded != null) OnVirtualAudioListenerAdded(virtualAudioListener);
        }
    
        public static void RemoveVirtualAudioListener(VirtualMultiAudioListener virtualAudioListener)
        {
            _virtualAudioListeners.Remove(virtualAudioListener);
            if (OnVirtualAudioListenerRemoved != null) OnVirtualAudioListenerRemoved(virtualAudioListener);
        }
    
        private static void CreateMainMultiAudioListener()
        {
            GameObject mainMultiAudioListener=new GameObject("MainMultiAudioListener");
            _main=mainMultiAudioListener.AddComponent<MainMultiAudioListener>();
    #if !ShowMultiAudioListenerInHierachy
            //We hide the sub audio source in hierarchy so that it doesn't flood it
            _main.gameObject.hideFlags = HideFlags.HideInHierarchy;
    #endif
    
            _main._createdByManager = true;
        }
    
    #if UNITY_EDITOR
        private void Start()
        {
            if (!_createdByManager)
            {
                //If manually placed in the scene. Remove the manager spawned one
                if(_main!=null)Destroy(_main.gameObject);
                _main = this;
            }
        }
    #endif
    }
}
