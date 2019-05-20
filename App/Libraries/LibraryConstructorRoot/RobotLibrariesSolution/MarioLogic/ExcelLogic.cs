using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace MarioLogic
{
    public static class ExcelLogic
    {
        private static Application ExcelApp = new Application();

        public static Workbook OpenWorkbook(string PathWorkbook)
        {
            bool IsNotOpen = true;
            Workbook OpeningWorkbook = null;

            foreach (Workbook W in ExcelApp.Workbooks)
            {
                if (W.FullName == PathWorkbook)
                {
                    OpeningWorkbook = W;
                    IsNotOpen = false;
                }

                if (W.Name == Path.GetFileName(PathWorkbook))
                    throw new Exception("Ya hay un libro abierto con el nombre " + W.Name);
            }

            if (IsNotOpen)
                OpeningWorkbook = ExcelApp.Workbooks.Open(PathWorkbook);

            return OpeningWorkbook;
        }/*Opens a Workbook if and returns it, in case that this workbook is already opened, returns the opened workbook*/

        private static void CopyTheSheet(Workbook FromWorkbook, Workbook ToWorkbook, string SheetName)
        {
            if (!SheetExists(FromWorkbook.FullName, SheetName))
                throw new Exception("No se encontro la planilla especificada");

            if (SheetExists(ToWorkbook.FullName, SheetName))
                DeleteSheet(FromWorkbook.FullName, SheetName);

            FromWorkbook.Sheets[SheetName].Copy
                (
                After: ToWorkbook.Sheets[ToWorkbook.Sheets.Count]
                );
        }/*Does the copying action for all the other functions, it validates the existance of the sheet and deletes an existing sheet with the same name before copying*/
        public static void CopySheet(string PathFromWorkbook, string PathToWorkbook, string SheetName)
        {
            Workbook FromWorkbook = OpenWorkbook(PathFromWorkbook);
            Workbook ToWorkbook = OpenWorkbook(PathToWorkbook);

            CopySheet(PathFromWorkbook, PathToWorkbook, SheetName);
        }/*Copies a sheet into other workbook*/
        public static void CopySheet(string PathFromWorkbook, string PathToWorkbook, string OldSheetName, string NewSheetName)
        {
            Workbook FromWorkbook = OpenWorkbook(PathFromWorkbook);
            Workbook ToWorkbook = OpenWorkbook(PathToWorkbook);

            CopySheet(PathFromWorkbook, PathToWorkbook, OldSheetName);
            RenameSheet(ToWorkbook.FullName, OldSheetName, NewSheetName);
        }/*Copies and renames a sheet into other workbook*/
        public static void CopySheet(string PathFromWorkbook, string PathToWorkbook, string[] SheetNames)
        {
            Workbook FromWorkbook = OpenWorkbook(PathFromWorkbook);
            Workbook ToWorkbook = OpenWorkbook(PathToWorkbook);

            foreach (string Name in SheetNames)
            {
                CopySheet(PathFromWorkbook, PathToWorkbook, Name);
            }
        }/*Copies a list of sheets into other workbook*/
        public static void CopySheet(string PathFromWorkbook, string PathToWorkbook, Dictionary<string, string> SheetNames)
        {
            Workbook FromWorkbook = OpenWorkbook(PathFromWorkbook);
            Workbook ToWorkbook = OpenWorkbook(PathToWorkbook);

            foreach (string Name in SheetNames.Keys.ToList())
            {
                CopySheet(PathFromWorkbook, PathToWorkbook, Name);
                RenameSheet(ToWorkbook.FullName, Name, SheetNames[Name]);
            }
        }/*Copies a list of sheets into other workbook and renames them all with a specific key-value pair (key=old name, value=new name) */
        public static void CopySheet(string PathFromWorkbook, string PathToWorkbook, string SheetName, int IndexDestination)
        {
            Workbook FromWorkbook = OpenWorkbook(PathFromWorkbook);
            Workbook ToWorkbook = OpenWorkbook(PathToWorkbook);

            if (IndexDestination > ToWorkbook.Sheets.Count)
                IndexDestination = ToWorkbook.Sheets.Count;
            else if (IndexDestination < 1)
                IndexDestination = 1;

            if (!SheetExists(FromWorkbook.FullName, SheetName))
                throw new Exception("No se encontro la planilla especificada");

            CopySheet(PathFromWorkbook, PathToWorkbook, SheetName);
        }/*Copies a sheet into other workbooko in a specific index*/
        public static void CopySheet(string PathFromWorkbook, string PathToWorkbook, string OldSheetName, string NewSheetName, int IndexDestination)
        {
            Workbook FromWorkbook = OpenWorkbook(PathFromWorkbook);
            Workbook ToWorkbook = OpenWorkbook(PathToWorkbook);

            if (IndexDestination > ToWorkbook.Sheets.Count)
                IndexDestination = ToWorkbook.Sheets.Count;
            else if (IndexDestination < 1)
                IndexDestination = 1;

            CopyTheSheet(FromWorkbook, ToWorkbook, OldSheetName);
            RenameSheet(PathToWorkbook, OldSheetName, NewSheetName);
        }/*Copies and renames a sheet into other workbook in a specific index*/
        public static void DeleteSheet(string PathWorkbook, string SheetName)
        {
            if (!SheetExists(PathWorkbook, SheetName))
                throw new Exception("No se encontro la planilla especificada");

            OpenWorkbook(PathWorkbook).Sheets[SheetName].Delete();
        }/*Deletes a sheet in a workbook with a specific name*/
        public static void RenameSheet(string PathWorkbook, string OldSheetName, string NewSheetName)
        {
            if (SheetExists(PathWorkbook, OldSheetName))
                if (!SheetExists(PathWorkbook, NewSheetName))
                    OpenWorkbook(PathWorkbook).Sheets[OldSheetName].Name = NewSheetName;
                else
                    throw new Exception("No se puede cambiar el nombre de la planilla porque ya existe una planilla con ese nombre");
            else
                throw new Exception("No se encontro la planilla especificada");
        }/*Renames a sheet with a specific name to a new one*/
        public static void RenameSheet(string PathWorkbook, Dictionary<string, string> SheetNames)
        {
            foreach (string Name in SheetNames.Keys.ToList())
                if (SheetExists(PathWorkbook, Name))
                    if (!SheetExists(PathWorkbook, SheetNames[Name]))
                        OpenWorkbook(PathWorkbook).Sheets[Name].Name = SheetNames[Name];
                    else
                        throw new Exception("No se puede cambiar el nombre de la planilla porque ya existe una planilla con ese nombre");
                else
                    throw new Exception("No se encontro la planilla especificada");
        }/*Renames all the sheets with the name in the dictionary keys and renames them to their pair*/
        public static bool SheetExists(string PathWorkbook, string SheetName)/*Searches for a sheet into a Workbook*/
        {
            bool Exists = false;
            foreach (Worksheet W in OpenWorkbook(PathWorkbook).Sheets)
                if (W.Name == SheetName)
                    Exists = true;

            return Exists;
        }
    }
}
