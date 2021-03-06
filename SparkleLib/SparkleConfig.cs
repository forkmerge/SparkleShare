//   SparkleShare, an instant update workflow to Git.
//   Copyright (C) 2010  Hylke Bons <hylkebons@gmail.com>
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.


using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace SparkleLib {

    public class SparkleConfig : XmlDocument {

        public static SparkleConfig DefaultConfig = new SparkleConfig (
            System.IO.Path.Combine (SparklePaths.SparkleConfigPath, "config.xml"));

        public string Path;


        public SparkleConfig (string path)
        {
            if (!File.Exists (path))
                throw new ConfigFileNotFoundException (path + " does not exist");

            Path = path;
            Load (Path);
        }


        public string UserName {
            get {
                XmlNode node = SelectSingleNode ("/sparkleshare/user/name/text()");
                return node.Value;
            }

            set {
                XmlNode node = SelectSingleNode ("/sparkleshare/user/name/text()");
                node.InnerText = value;

                Save ();
            }
        }


        public string UserEmail {
            get {
                XmlNode node = SelectSingleNode ("/sparkleshare/user/name/email()");
                return node.Value;
            }

            set {
                XmlNode node = SelectSingleNode ("/sparkleshare/user/name/email()");
                node.InnerText = value;

                Save ();
            }
        }


        public string [] Folders {
            get {
                List<string> folders = new List<string> ();

                foreach (XmlNode node_folder in SelectNodes ("/sparkleshare/folder"))
                    folders.Add (node_folder ["name"].InnerText);

                return folders.ToArray ();
            }
        }


        public void AddFolder (string name, string backend)
        {
            XmlNode node_folder = CreateNode (XmlNodeType.Element, "folder", null);

            XmlNode node_name = CreateElement ("name");
            node_name.InnerText = name;

            XmlNode node_backend = CreateElement ("backend");
            node_backend.InnerText = backend;

            node_folder.AppendChild (node_name);
            node_folder.AppendChild (node_backend);

            XmlNode node_root = SelectSingleNode ("/sparkleshare");
            node_root.AppendChild (node_folder);

            Save ();
        }


        public void RemoveFolder (string name)
        {
            foreach (XmlNode node_folder in SelectNodes ("/sparkleshare/folder")) {
                if (node_folder ["name"].InnerText.Equals (name))
                    SelectSingleNode ("/sparkleshare").RemoveChild (node_folder);
            }

            Save ();
        }


        public bool FolderExists (string name)
        {
            foreach (XmlNode node_folder in SelectNodes ("/sparkleshare/folder")) {
                if (node_folder ["name"].InnerText.Equals (name))
                    return true;
            }

            return false;
        }


        public string GetBackendForFolder (string name)
        {
            foreach (XmlNode node_folder in SelectNodes ("/sparkleshare/folder")) {
                if (node_folder ["name"].InnerText.Equals (name))
                    return node_folder ["backend"].InnerText;
            }

            return null;
        }


        public void Save ()
        {
            if (!File.Exists (Path))
                throw new ConfigFileNotFoundException (Path + " does not exist");

            Save (Path);
        }
    }


    public class ConfigFileNotFoundException : Exception {

        public ConfigFileNotFoundException (string message) :
            base (message) { }
    }
}