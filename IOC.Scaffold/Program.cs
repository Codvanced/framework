using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IOC.Scaffold
{
    class Program
    {
        static string replaceWord = "Wunderman.LogManager";

        static void Main(string[] args)
        {
            LoadMenuDecisions();

            if (args != null && args.Length > 0 && dctMenu.Keys.Contains(args[0].Trim().ToLowerInvariant()))
            {
                dctMenu[args[0].Trim().ToLowerInvariant()](args);
            }
            else
            {
                string itemMenu = "help";
                while (!itemMenu.Trim().ToLowerInvariant().Equals("exit"))
                {
                    if (itemMenu.Trim().ToLowerInvariant().Equals("help"))
                    {
                        ShowMenu();
                    }
                    else if (dctMenu.Keys.Contains(itemMenu.Trim().ToLowerInvariant()))
                    {
                        dctMenu[itemMenu.Trim().ToLowerInvariant()](args);
                    }
                    else
                    {
                        Console.WriteLine("Comando inexistente, para consultar a lista de comandos utilize 'help'");
                    }

                    Console.WriteLine();
                    Console.Write("Comando: ");
                    itemMenu = Console.ReadLine();

                    if (itemMenu.Contains(" "))
                    {
                        args = itemMenu.Split(' ');
                        itemMenu = args[0];
                    }

                    Console.Clear();
                }
            }
        }

        static void ScaffoldProcess(
            string destinationPath,
            string projectName,
            string customer
        )
        {
            string templatePath = "TEMPLATE_MVC_4";
            string actualDir = Path.Combine(Directory.GetCurrentDirectory(), templatePath);
            if (Directory.Exists(actualDir))
            {
                if (!string.IsNullOrEmpty(projectName))
                {
                    CopyDirectory(
                        new DirectoryInfo(actualDir),
                        new DirectoryInfo(destinationPath)
                    );

                    if (!string.IsNullOrWhiteSpace(customer))
                    {
                        projectName = string.Concat(customer, ".", projectName);
                    }

                    ProjectChanger(destinationPath, projectName);
                }
                else
                {
                    Console.WriteLine("É necessário informar o nome do projeto...", projectName);
                }
            }
            else
            {
                Console.WriteLine("Pasta {0} não encontrada...", templatePath);
            }
        }

        static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            if (!destination.Exists)
            {
                destination.Create();
            }

            FileInfo[] files = source.GetFiles();
            foreach (FileInfo file in files)
            {
                string destDir = Path.Combine(destination.FullName, file.Name);
                file.CopyTo(destDir);
            }

            DirectoryInfo[] dirs = source.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                string destinationDir = Path.Combine(destination.FullName, dir.Name);
                CopyDirectory(dir, new DirectoryInfo(destinationDir));
            }
        }

        static void ProjectChanger(string path, string projectName)
        {
            string[] folders = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);

            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    FileInfo info = new FileInfo(file);

                    if (info.Exists
                        && !info.Extension.Equals("dll")
                        && !info.Extension.Equals("pdb")
                        && !info.Extension.Equals("suo")
                        && !info.Extension.Equals("exe"))
                    {
                        string content = string.Empty;
                        using (var reader = new StreamReader(info.FullName))
                        {
                            content = reader.ReadToEnd();
                        }

                        if (content.Contains(replaceWord))
                        {
                            content = content.Replace(replaceWord, projectName);
                            using (var writer = new StreamWriter(info.FullName))
                            {
                                writer.Write(content);
                            }
                        }

                        if (info.Name.Contains(replaceWord))
                        {
                            string newFilename = Path.Combine(
                                info.DirectoryName,
                                info.Name.Replace(replaceWord, projectName)
                            );
                            File.Move(file, newFilename);
                        }
                    }
                }
            }

            if (folders != null && folders.Length > 0)
            {
                foreach (var folder in folders)
                {
                    string newFoldername = folder;
                    if (folder.Contains(replaceWord))
                    {
                        newFoldername = folder.Replace(replaceWord, projectName);
                        Directory.Move(folder, newFoldername);
                    }

                    if (!folder.Equals("packages"))
                    {
                        ProjectChanger(newFoldername, projectName);
                    }
                }
            }
        }

        #region CONSOLE_INFO
        static Dictionary<string, Action<string[]>> dctMenu;

        static void Log(string msg, bool isRelevant)
        {
            if (isRelevant)
            {
                Console.WriteLine();

                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }

            Console.WriteLine(msg.PadRight(Console.WindowWidth - 1)); // <-- see note
            Console.ResetColor();
        }

        static void ShowMenu()
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
Responsável por criar aplicações utilizando o framework IOC-FW.
IOC.Scaffold [Opções] ");
            Console.ResetColor();

            Console.Write("Opções: ");


            Console.WriteLine();
            Console.WriteLine();

            PrintItem("create-project", "Cria um projeto MVC 4 já configurado com o ioc-fw (.net 4.0). \r\n\t  ex: [destino] [nome_do_projeto] [nome_do_cliente]");

            PrintItem("help", "Exibe Menu.");
            PrintItem("exit", "Finaliza a execução.");

            Console.WindowWidth = 110;
            Console.WindowHeight = 30;
        }

        static void PrintItem(string item, string description)
        {
            Console.Write("\t");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(item);
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("\t  ");
            Console.Write(description);
            Console.WriteLine();
            Console.WriteLine();
        }

        static void LoadMenuDecisions()
        {
            dctMenu = new Dictionary<string, Action<string[]>>() {
                { "create-project", 
                    prms => 
                        ScaffoldProcess(prms[1], prms[2],  prms.Length >= 3 ? prms[3] : string.Empty) 
                }
            };
        } 
        #endregion
    }
}
