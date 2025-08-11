using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


//It is common to create a class to contain all of your
//extension methods. This class must be static.
namespace Game.Architecture {
  public static class AssertComponents {
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static T GetAssertComponent<T>(this MonoBehaviour target) {
      if (target.TryGetComponent(out T component)) {
        return component;
      }
      throw new NoComponentException($"No component ${typeof(T)}");
    }
  
    public class NoComponentException : System.Exception {
      public NoComponentException() { }
      public NoComponentException(string message) : base(message) { }
      public NoComponentException(string message, System.Exception inner) : base(message, inner) { }
      protected NoComponentException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public static T GetAssertComponentInParent<T>(this MonoBehaviour target) {
      var component = target.GetComponentInParent<T>() ?? throw new NoComponentException($"Error in .GetAssertComponentInParent: No component ${typeof(T)}");
      return component;
    }

    public static T GetAssertComponentInChildren<T>(this MonoBehaviour target) {
      var component = target.GetComponentInChildren<T>() ?? throw new NoComponentException($"Error in .GetAssertComponentInChildren: No component ${typeof(T)}");
      return component;
    }
  
    public static T AssertFind<T>(this MonoBehaviour target) where T : Object {
      var component = Object.FindFirstObjectByType<T>() ?? throw new NoComponentException($"Error in .FindFirstObjectByType: No component ${typeof(T)}");
      return component;
    }
  
    public static void ThrowWhenNull<T>(this MonoBehaviour target, [NotNull] T? value) {
#if (UNITY_EDITOR)
      if (value is null) throw new ArgumentNullException(nameof(value), $"{typeof(T)} is null in {AssetDatabase.GetAssetPath(target)}");
#endif
    }
  
    public static void ThrowWhenNull<T>(this ScriptableObject target, [NotNull] T? value) {
#if (UNITY_EDITOR)
      if (value is null) throw new ArgumentNullException(nameof(value), $"{typeof(T)} is null in {AssetDatabase.GetAssetPath(target)}");
#endif
    }
  }
}
