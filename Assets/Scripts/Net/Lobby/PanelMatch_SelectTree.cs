using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PanelMatch_SelectTree : MonoBehaviour
{
    private Dropdown _dd;

    void Awake()
    {
        _dd = this.GetComponent<Dropdown>();
    }

    public void OnEnable()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//RustInPieces";

        if (Directory.Exists(path))
        {
            _dd.options.Clear();
            foreach (var p in Directory.GetFiles(path))
            {
                var f = p.Split('\\');
                var file = f[f.Length - 1];
                _dd.options.Add(new Dropdown.OptionData(file.Split('.')[0]));
            }
            if (_dd.options.Count != 0)
            {
                _dd.value = 0;
                OnValueChanged(0);
            }
            else
            {
                PhotonNetwork.player.customProperties["Tree"] = _DefaultTree();
            }
        }
        else
        {
            PhotonNetwork.player.customProperties["Tree"] = _DefaultTree();
        }
    }

    public void OnValueChanged(int newIdx)
    {
        string treeName = _dd.options[newIdx].text;

        List<NodeData_Serializable> serializedNodes = new List<NodeData_Serializable>();

        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//RustInPieces" + "//" + treeName + ".tree";
        if (!File.Exists(path))
        {
            serializedNodes = _DefaultTree();
        }
        else
        {

            FileStream file;
            try
            {
                file = File.Open(path, FileMode.Open);
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(List<NodeData_Serializable>));
                try
                {
                    serializedNodes = reader.Deserialize(file) as List<NodeData_Serializable>;
                    file.Close();
                }
                catch
                {
                    serializedNodes = _DefaultTree();
                    file.Close();
                }
            }
            catch
            {
                serializedNodes = _DefaultTree();
            }
        }

        Hashtable props = new Hashtable();
        props.Add("TreeSize", serializedNodes.Count);
        for (int i = 0; i < serializedNodes.Count; i++)
        {
            props.Add("Tree" + i.ToString() + "id", serializedNodes[i].id);
            props.Add("Tree" + i.ToString() + "rank", serializedNodes[i].rank);
        }
        PhotonNetwork.player.SetCustomProperties(props);
    }

    private List<NodeData_Serializable> _DefaultTree()
    {
        return new List<NodeData_Serializable>()
        {
                new NodeData_Serializable(0, 1011),
                new NodeData_Serializable(0, 1110),
                new NodeData_Serializable(0, 1214),
                new NodeData_Serializable(0, 1213),

                new NodeData_Serializable(1, 1007),
                new NodeData_Serializable(1, 1106),
                new NodeData_Serializable(1, 1107),

                new NodeData_Serializable(2, 1103),
                new NodeData_Serializable(2, 1004),

                new NodeData_Serializable(3, 1200)
        };
    }
}
