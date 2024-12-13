using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SkyMatrixNamespace
{
    /// <summary>
    /// Provides utility functions for common multiplayer programming tasks, including RPC determination, network object validation, and input action toggling.
    /// </summary>
    public static class GenericMultiplayer
    {
        /// <summary>
        /// Determines whether to invoke an action locally or as an RPC (Remote Procedure Call) based on the current network behavior's role (owner or server).
        /// </summary>
        /// <typeparam name="T">The type of the data to be sent with the RPC.</typeparam>
        /// <param name="behavior">The NetworkBehaviour instance.</param>
        /// <param name="actionToPerform">The action to perform locally if the behavior is the owner and server.</param>
        /// <param name="ActionRPC">The RPC action to invoke if the behavior is the owner but not the server.</param>
        public static void RPCDeterminationCall<T>(NetworkBehaviour behavior, Action actionToPerform, Action ActionRPC)
        {
            if (!behavior.IsOwner)
            {
                // If the current behavior is not the owner, do nothing.
                return;
            }

            if (behavior.IsServer)
            {
                // If the behavior is the owner and server, invoke the local action.
                actionToPerform.Invoke();
            }
            else
            {
                // If the behavior is the owner but not the server, invoke the RPC action.
                ActionRPC.Invoke();
            }
        }

        /// <summary>
        /// Checks if a given NetworkObjectReference is valid and points to an existing NetworkObject.
        /// </summary>
        /// <param name="netObjectRef">The NetworkObjectReference to check.</param>
        /// <returns>True if the NetworkObjectReference is valid, false otherwise.</returns>
        public static bool CheckForUpdateNetworkObject(NetworkObjectReference netObjectRef)
        {
            netObjectRef.TryGet(out NetworkObject foundObject);
            return foundObject != null;
        }

        /// <summary>
        /// Enables or disables a set of InputActions based on the given boolean value.
        /// </summary>
        /// <param name="value">True to enable the actions, false to disable them.</param>
        /// <param name="actions">The InputActions to be enabled or disabled.</param>
        public static void ToggleInputActions(bool value, params InputAction[] actions)
        {
            if (value)
            {
                // Enable each input action.
                foreach (var action in actions)
                {
                    action.Enable();
                }
            }
            else
            {
                // Disable each input action.
                foreach (var action in actions)
                {
                    action.Disable();
                }
            }
        }

    }

    [System.Serializable]
    public class ServerAndClientUnityEvent : UnityEvent
    {
        public void InvokeServerAndClientEvent(NetworkBehaviour behavior)
        {
            // Send a RPC to all clients to invoke the event
            RpcInvokeClientEvent();

            if (behavior.IsServer)
            {
                // Invoke the event on the server
                Invoke();
            }
        }

        [Rpc(SendTo.Everyone)]
        internal virtual void RpcInvokeClientEvent()
        {
            // Invoke the event on all clients
            Invoke();
        }
    }



}
