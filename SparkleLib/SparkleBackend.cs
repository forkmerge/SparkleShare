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

namespace SparkleLib {

    public class SparkleBackend {

        public static SparkleBackend DefaultBackend =
            new SparkleBackend ("Git",
                new string [4] {
                    "/opt/local/bin/git",
                    "/usr/bin/git",
                    "/usr/local/bin/git",
                    "/usr/local/git/bin/git"
                }
            );

        public string Name;
        public string Path;


        public SparkleBackend (string name, string [] paths)
        {
            Name = name;

            foreach (string path in paths) {
                if (File.Exists (path)) {
                    Path = path;
                    break;
                }
            }
        }


        public bool IsPresent {
            get {
               return (Path != null);
            }
        }


        public bool IsUsablePath (string path)
        {
            return (path.Length > 0);
        }
    }
}
