using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;

namespace SkillTreeBuilder
{
	public class SaveAndLoad : MonoBehaviour
	{
		public GameObject pool;
		public GameObject tree;

		public InputField saveName;
		public Dropdown dropDown;

		private SkillTreeBuilderManager _manager;

        void Awake()
        {
            _manager = this.GetComponent<SkillTreeBuilderManager>();
        }

        void OnEnable()
        {
            foreach (var path in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//RustInPieces"))
            {
                var f = path.Split('\\');
                var file = f[f.Length - 1];
                dropDown.options.Add(new Dropdown.OptionData(file.Split('.')[0]));
            }
        }

        public void SaveTree()
		{
			if (saveName.text == "")
				return;

			var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//RustInPieces";
			if (!Directory.Exists(path))
			{
				try
				{
					Directory.CreateDirectory(path);
				}
				catch
				{
					_manager._popup.SpawnPopUp("Unable to create the directory at : " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//RustInPieces");
					return;
				}
			}

			FileStream file;
			try
			{
				file = File.Create(path + "//" + saveName.text + ".tree");
			}
			catch
			{
				_manager._popup.SpawnPopUp("Unable to create the template at : " + path + "//" + saveName.text + ".tree");
				return;
			}

			System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<NodeData_Serializable>));

			List<NodeData_Serializable> dataToSerialize = new List<NodeData_Serializable>();
			foreach (NodeData nodeData in tree.GetComponentsInChildren<NodeData>())
			{
				dataToSerialize.Add(new NodeData_Serializable(nodeData.rank, nodeData.nodeId));
			}

			try
			{
				writer.Serialize(file, dataToSerialize);
			}
			catch
			{
				_manager._popup.SpawnPopUp("Unable to save the file");
				return;
			}
			dropDown.options.Add(new Dropdown.OptionData(saveName.text));
            file.Close();
		}

		public void LoadTree()
		{
			System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(List<NodeData_Serializable>));

			string path;
		    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//RustInPieces" + "//" + dropDown.options[dropDown.value].text + ".tree";
			if (!File.Exists(path))
			{
				_manager._popup.SpawnPopUp("File not found");
				return;
			}

			FileStream file;
			try
			{
				file = File.Open(path, FileMode.Open);
			}
			catch
			{
				_manager._popup.SpawnPopUp("Unable to open this template");
				return;
			}

			List<NodeData_Serializable> deserializedData = new List<NodeData_Serializable>();

			try
			{
				deserializedData = reader.Deserialize(file) as List<NodeData_Serializable>;
			}
			catch
			{
				file.Close();
				_manager._popup.SpawnPopUp("File corrupted");
				return;
			}
			
            file.Close();

			_manager.ResetSkillTreeBuilder();

            foreach (NodeData_Serializable data in deserializedData)
			{
				Transform node = _manager.GetSpecificNode(data.rank, data.id);
				if (node == null)
				{
					_manager.ResetSkillTreeBuilder();
					file.Close();
					_manager._popup.SpawnPopUp("File corrupted");
					return;
				}

                //node.GetComponentInChildren<Image>().enabled = true;
                //node.SetParent(_manager.treeSkillHolders[data.rank].transform);

                node.SendMessage("_OnDoubleClick");
			}
		}
	}
}