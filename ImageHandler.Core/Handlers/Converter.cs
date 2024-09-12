using System.Diagnostics;
using ImageHandler.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageHandler.Core.Handlers;

public static class Converter
{
    private const string OutputDirectory = "../../../../Assets/Webp_Images/";
    public static BenchmarkResult BenchmarkResults { get; } = new();

    public static void ConvertImagesToWebp()
    {
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

                var outputFileName = Path.Combine(OutputDirectory, Path.GetFileNameWithoutExtension(imageFile) + ".webp");

                image.Save(outputFileName, new WebpEncoder());
                Console.WriteLine($"Imagem convertida com sucesso: {outputFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao converter imagem {imageFile}: {ex.Message}");
            }
        }

        stopwatch.Stop();
        BenchmarkResults.SequentialTime = stopwatch.ElapsedMilliseconds;
    }

    public static void ConvertImagesToWebpMultithreaded(int numThreads)
    {
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

                        var outputFileName = Path.Combine(OutputDirectory, Path.GetFileNameWithoutExtension(imageFile) + ".webp");

                        image.Save(outputFileName, new WebpEncoder());
                        Console.WriteLine($"Imagem convertida com sucesso: {outputFileName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao converter imagem {imageFile}: {ex.Message}");
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

        Console.WriteLine("Conversão com múltiplas threads concluída.");
    }


    public static void DeleteConvertedImagesFolder()
    {
        
        if (Directory.Exists(OutputDirectory))
        {
            try
            {
                Console.WriteLine("Apagando a pasta das imagens convertidas...");
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