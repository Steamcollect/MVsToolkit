using UnityEngine;

public static class WrapperTemplate
{
    public static string rso =
        "using UnityEngine;\r\n" +
        "using MVsToolkit.Wrappers;\r\n" +
        "\r\n" +
        "[CreateAssetMenu(fileName = \"#SCRIPTNAME#\", menuName = \"#FILE_PATH#\")]\r\n" +
        "public class #SCRIPTNAME# : RuntimeScriptableObject<>{}\r\n";

    public static string rse =
        "using UnityEngine;\r\n" +
        "using MVsToolkit.Wrappers;\r\n" +
        "\r\n" +
        "[CreateAssetMenu(fileName = \"#SCRIPTNAME#\", menuName = \"#FILE_PATH#\")]\r\n" +
        "public class #SCRIPTNAME# : RuntimeScriptableEvent<>{}\r\n";
}
