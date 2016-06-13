using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
[assembly: CommandClass(typeof(CrxApp.Commands))]
[assembly: ExtensionApplication(null)]
namespace CrxApp
{
    public class Commands
    {
        [CommandMethod("MyTestCommands", "test", CommandFlags.Modal)]
        static public void Test()
        {
            //prompt for input json and output folder
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            try
            {
                //extract layer names and save them to layers.txt
                var db = doc.Database;
                using (var writer = File.CreateText("layers.txt"))
                {

                    dynamic layers = db.LayerTableId;
                    foreach (dynamic layer in layers)
                        writer.WriteLine(layer.Name);
                }
            }
            catch (System.Exception e)
            {
                ed.WriteMessage("Error: {0}", e);
            }
        }
    }
}

