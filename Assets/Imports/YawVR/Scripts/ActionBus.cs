using System;
using UnityEngine;
using System.Collections.Generic;

namespace YawVR
{
    /// <summary>
    /// Runs the ActionQueue functions on main thread
    /// </summary>
    public class ActionBus : MonoBehaviour
    {
        private static readonly Queue<Action> actionQueue = new Queue<Action>();
        private static ActionBus instance;

        public static ActionBus Instance()
        {
            if (instance == null)
            {
                throw new Exception("ActionBus' parent gameObject needs to be on scene, please add it to your scene.");
            }
            return instance;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                //DontDestroyOnLoad(this.gameObject); - Only uncomment if none of its parents already are DontDestroyOnLoad.
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public void Update()
        {
            lock (actionQueue)
            {
                while (actionQueue.Count > 0)
                {
                    Action action = actionQueue.Dequeue();
                    action();
                }
            }
        }

        /// <summary>
        /// Adds an action to the execution queue.
        /// </summary>
        /// <param name="action">
        /// The action to be executed on the Unity main thread during the next <see cref="Update"/> cycle.
        /// </param>
        public void Add(Action action)
        {
            lock (actionQueue)
            {
                actionQueue.Enqueue(action);
            }
        }
    }
}