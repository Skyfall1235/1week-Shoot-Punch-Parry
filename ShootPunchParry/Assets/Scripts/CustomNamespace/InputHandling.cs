using UnityEngine;
using UnityEngine.InputSystem;

namespace SkyMatrixNamespace
{
    public static class InputHandling
    {
        /// <summary>
        /// Retrieves a value of type T from a Unity InputActionProperty.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve. Must be a struct.</typeparam>
        /// <param name="actionProperty">The InputActionProperty object containing the value.</param>
        /// <returns>The value of type T retrieved from the InputActionProperty.</returns>
        static public T RetrieveValueFromAction<T>(InputActionProperty actionProperty) where T : struct
        {
            // retrive the value from the action map
            T actionValue = actionProperty.action.ReadValue<T>();
            //return the value
            return actionValue;
        }
    }

}
