using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyAI.Utility
{
    /// <summary>
    /// Base component for all types which have message logging 
    /// </summary>
    public class MessageComponent : MonoBehaviour
    {
        /// <summary>
        /// If this component has any messages or not.
        /// </summary>
        public bool HasMessages => Messages.Count > 0;

        /// <summary>
        /// The number of messages this component has.
        /// </summary>
        public int MessageCount => Messages.Count;
        
        /// <summary>
        /// The messages of this component.
        /// </summary>
        public List<string> Messages { get; private set; } = new();

        /// <summary>
        /// Override for custom detail rendering on the automatic GUI.
        /// </summary>
        /// <param name="x">X rendering position. In most cases this should remain unchanged.</param>
        /// <param name="y">Y rendering position. Update this with every component added and return it.</param>
        /// <param name="w">Width of components. In most cases this should remain unchanged.</param>
        /// <param name="h">Height of components. In most cases this should remain unchanged.</param>
        /// <param name="p">Padding of components. In most cases this should remain unchanged.</param>
        /// <returns>The updated Y position after all custom rendering has been done.</returns>
        public virtual float DisplayDetails(float x, float y, float w, float h, float p)
        {
            return y;
        }

        /// <summary>
        /// Add a message to this component.
        /// </summary>
        /// <param name="message">The message to add.</param>
        public void Log(string message)
        {
            Manager.GlobalLog($"{name} - {message}");
            
            switch (Manager.MessageMode)
            {
                case Manager.MessagingMode.Compact when Messages.Count > 0 && Messages[0] == message:
                    return;
                case Manager.MessagingMode.Unique:
                    Messages = Messages.Where(m => m != message).ToList();
                    break;
                case Manager.MessagingMode.All:
                default:
                    break;
            }

            Messages.Insert(0, message);
            if (Messages.Count > Manager.MaxMessages)
            {
                Messages.RemoveAt(Messages.Count - 1);
            }
        }

        /// <summary>
        /// Clear all messages of this component.
        /// </summary>
        public void ClearMessages()
        {
            Messages.Clear();
        }

        /// <summary>
        /// Override to easily display the type of the component for easy usage in messages.
        /// </summary>
        /// <returns>Name of this type.</returns>
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}