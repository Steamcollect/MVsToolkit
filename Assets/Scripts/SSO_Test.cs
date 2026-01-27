using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Test", menuName = "SSO/_/SSO_Test")]
public class SSO_Test : ScriptableObject
{
    public int id;
    public string _name;
    [TextArea] public string _description;
}