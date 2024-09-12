using System.Diagnostics;
using ImageHandler.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageHandler.Core.Handlers
{
    public static class Resizer
    {
        private const string OutputDirectory = "../../../../Assets/Resized_Images/";
        public static BenchmarkResult BenchmarkResults { get; } = new();

        public static void ResizeImages(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                Console.WriteLine("Largura e altura devem ser maiores que zero.");
                return;
            }

            if (width > 5000 || height > 5000)
            {
                Console.WriteLine("Largura e altura são muito grandes. Escolha valores menores que 10.000.");
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            string[] allowedExtensions = { ".jpeg", ".jpg", ".png" };

            var imageFiles = Directory.GetFiles(Configurations.InputDirectory)
                .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                .ToList();

            foreach (var imageFile in imageFiles)
            {
                try
                {
                    using var image = Image.Load<Rgba32>(imageFile);

                    image.Mutate(x => x.Resize(width, height));

                    var outputFileName = Path.Combine(OutputDirectory, Path.GetFileNameWithoutExtension(imageFile) + "_resized.png");

                    image.Save(outputFileName);
                    Console.WriteLine($"Imagem redimensionada com sucesso: {outputFileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao redimensionar imagem {imageFile}: {ex.Message}");
                }
            }

            stopwatch.Stop();
            BenchmarkResults.SequentialTime = stopwatch.ElapsedMilliseconds;
        }

        public static void ResizeImagesMultithreaded(int numThreads, int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                Console.WriteLine("Largura e altura devem ser maiores que zero.");
                return;
            }

            if (width > 10000 || height > 10000)
            {
                Console.WriteLine("Largura e altura são muito grandes. Escolha valores menores que 10.000.");
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            var allowedExtensions = new[] { ".jpeg", ".jpg", ".png" };

            var imageFiles = Directory.GetFiles(Configurations.InputDirectory)
                .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                .ToList();

            var partitionSize = (int)Math.Ceiling((double)imageFiles.Count / numThreads);
            var tasks = new Task[numThreads];

            for (var i = 0; i < numThreads; i++)
            {
                var partition = imageFiles.Skip(i * partitionSize).Take(partitionSize).ToList();
                tasks[i] = Task.Run(() =>
                {
                    foreach (var imageFile in partition)
                    {
                        try
                        {
                            using var image = Image.Load<Rgba32>(imageFile);

                            image.Mutate(x => x.Resize(width, height));

                            var outputFileName = Path.Combine(OutputDirectory, Path.GetFileNameWithoutExtension(imageFile) + "_resized.png");

                            image.Save(outputFileName);
                            Console.WriteLine($"Imagem redimensionada com sucesso: {outputFileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao redimensionar imagem {imageFile}: {ex.Message}");
                        }
                    }
                });
            }

            Task.WaitAll(tasks);
            stopwatch.Stop();

            switch (numThreads)
            {
                case 1:
                    BenchmarkResults.Thread1Time = stopwatch.ElapsedMilliseconds;
                    break;
                case 2:
                    BenchmarkResults.Thread2Time = stopwatch.ElapsedMilliseconds;
                    break;
                case 3:
                    BenchmarkResults.Thread3Time = stopwatch.ElapsedMilliseconds;
                    break;
                case 4:
                    BenchmarkResults.Thread4Time = stopwatch.ElapsedMilliseconds;
                    break;
                case 6:
                    BenchmarkResults.Thread6Time = stopwatch.ElapsedMilliseconds;
                    break;
                case 8:
                    BenchmarkResults.Thread8Time = stopwatch.ElapsedMilliseconds;
                    break;
                case 10:
                    BenchmarkResults.Thread10Time = stopwatch.ElapsedMilliseconds;
                    break;
            }

            Console.WriteLine("Redimensionamento com múltiplas threads concluído.");
        }

        public static void DeleteResizedImagesFolder()
        {
            if (Directory.Exists(OutputDirectory))
            {
                try
                {
                    Console.WriteLine("Apagando a pasta das imagens redimensionadas...");
                    Directory.Delete(OutputDirectory, true);
                    Console.WriteLine("Pasta excluída com sucesso.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao excluir pasta: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("A pasta não existe.");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar.");
            Console.ReadKey();
        }
    }
}