using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Core Factory to create Instance of prefab.
/// You should really not use it unless you know what you do....
/// </summary>
public static class Factory
{
    /// <summary>
    /// Those folder must be children of a Resources folder.
    /// They can only contain gameobject which will be created during the game
    /// There is no recursivity here, so any GO in a sub-subFolder will be ignorated
    /// Feel free to add some useful folder, BUT add cleverly.
    /// </summary>
    private static string[] _subFolder = 
    {
        "SpellInfo",
        "SpellInfo/Minions",
        "SpellInfo/Strategist",
        "SpellInfo/Heroes",
        "Strategist",
        "Strategist/UI",
        "HeroIcons",
        "Minions",
        "Heroes",
        "SkillNodes"
    };

    /// <summary>
    /// All loaded pefabs, accessibles through there name 
    /// </summary>
    private static Dictionary<string, Object> _prefabs = new Dictionary<string, Object>();

    static Factory()
    {
        foreach (string path in _subFolder)
        {
            foreach (Object prefab in Resources.LoadAll<GameObject>(path + "/"))
            {
                _prefabs[prefab.name] = prefab;
            }
        }
    }

    /// <summary>
    /// Return a new instance of the prefab identified by the name prefabName.
    /// If position & rotation are provided, the prefab is created at the position "position" and rotated according to the rotation "rotation"
    /// 
    /// Be aware that you get a copy of the prefab, not the prefab itself.
    /// Any modification applied to the prefab you get will only modifiy this prefab.
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public static GameObject CreateInstanceOf(string prefabName, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        //Debug.Log(prefabName);

        GameObject ret = GameObject.Instantiate(_prefabs[prefabName], position, rotation) as GameObject;

        return ret;
    }

    /// <summary>
    /// Since you can do a lot of shit with this function, double check what you're doing.
    /// use CreateInstanceOf for creating a GameObject/prefab
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public static T CreateResource<T>(string prefabName)
        where T : class
    {
        return GameObject.Instantiate(_prefabs[prefabName]) as T;
    }
}
