using UnityEngine;

public class HapticFeedback
{

    public static void Vibrate()
    {
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }

}
