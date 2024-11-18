using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace LivelyController
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (GeraInterface(out var nomepasta, out var tempo, out var main)) return main;
            var combine = IniciaLively();
            if (ExecutaLively(nomepasta, combine, tempo, out var i)) return i;

            return 0;
        }

        private static bool ExecutaLively(string nomepasta, string combine, int tempo, out int i)
        {
            var arquivos = Directory.GetFiles(nomepasta);
            i = 0;
            foreach (var arquivo in arquivos)
            {

                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c {combine} setwp --file {arquivo}";
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Console.WriteLine("Ocorreu algum erro para executar o programa. Verifique se o Lively está em execução.");
                    i = 2;
                    return true;
                }
                
                Thread.Sleep(tempo * 1000); 
            }

            return false;
        }

        private static string IniciaLively()
        {
            var assembly = typeof(Program).Assembly;
            var livelyStream = assembly.GetManifestResourceStream("LivelyController.Livelycu.exe");
            var combine = Path.Combine(Path.GetTempPath(), "Livelycu.exe");
            var lively = File.Create(combine);
            livelyStream.Seek(0, SeekOrigin.Begin);
            livelyStream.CopyTo(lively);
            livelyStream.Close();
            lively.Close();
            return combine;
        }

        private static bool GeraInterface(out string nomepasta, out int tempo, out int main)
        {
            main = 0;  
            Console.WriteLine("Este programa seleciona uma pasta com imagens ou videos e itera entre elas para definir como wallpaper pelo Lively");
            Console.WriteLine("Selecione o nome da pasta:");
            nomepasta = Console.ReadLine();
            Console.WriteLine("Selecione o tempo para alternar entre os wallpapers em segundos:");
            tempo = Convert.ToInt32(Console.ReadLine());
            if (!Directory.Exists(nomepasta))
            {
                Console.WriteLine("Este diretório não foi encontrado, por favor tente novamente.");
                main = 1;
                return true;
            }

            return false;
        }
    }
}