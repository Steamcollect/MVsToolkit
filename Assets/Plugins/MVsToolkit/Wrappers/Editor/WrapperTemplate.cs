using UnityEngine;

public static class WrapperTemplate
{
    public static string rsoTemplate =
        "using UnityEngine;\r\n" +
        "using MVsToolkit.Wrappers;\r\n" +
        "\r\n" +
        "[CreateAssetMenu(fileName = \"#SCRIPTNAME#\", menuName = \"#FILE_PATH##SCRIPTNAME#\")]\r\n" +
        "public class #SCRIPTNAME# : RuntimeScriptableObject<>{}\r\n";
}
