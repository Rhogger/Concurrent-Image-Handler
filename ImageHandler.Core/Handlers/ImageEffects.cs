using System.Diagnostics;
using ImageHandler.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageHandler.Core.Handlers
{
    public static class ImageEffects
    {
        private const string OutputDirectory = "../../../../Assets/Effects_Images/";
        public static BenchmarkResult BenchmarkResults { get; } = new();
        
        public static void ApplyGaussianBlur(float sigma = 3, int numThreads = 0) 
        {
            if(numThreads == 0)
                ApplyEffect(image => image.Mutate(x => x.GaussianBlur(sigma)));
                
            ApplyEffectMultithreaded(image => image.Mutate(x => x.GaussianBlur(sigma)), numThreads);
        }

        public static void ApplyGrayscale(int numThreads = 0)
        {
            if(numThreads == 0)
                ApplyEffect(image => image.Mutate(x => x.Grayscale()));
                
            ApplyEffectMultithreaded(image => image.Mutate(x => x.Grayscale()), numThreads);
        }

        public static void ApplyInvertColors(int numThreads = 0)
        {
            if(numThreads == 0)
                ApplyEffect(image => image.Mutate(x => x.Invert()));
                
            ApplyEffectMultithreaded(image => image.Mutate(x => x.Invert()), numThreads);
        }

        public static void ApplyContrast(float contrast = 1.2f, int numThreads = 0)
        {
            if(numThreads == 0)
                ApplyEffect(image => image.Mutate(x => x.Contrast(contrast)));
                
            ApplyEffectMultithreaded(image => image.Mutate(x => x.Contrast(contrast)), numThreads);
        }

        public static void ApplyPixelate(int pixelSize = 8, int numThreads = 0)
        {
            if(numThreads == 0)
                ApplyEffect(image => image.Mutate(x => x.Pixelate(pixelSize)));
                
            ApplyEffectMultithreaded(image => image.Mutate(x => x.Pixelate(pixelSize)), numThreads);
        }

        private static void ApplyEffect(Action<Image<Rgba32>> effect)
        {
            var stopwatch = Stopwatch.StartNew();

            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            string[] allowedExtensions = { ".jpeg", ".jpg", ".png"};
            var imageFiles = Directory.GetFiles(Configurations.InputDirectory)
                .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                .ToList();

            foreach (var imageFile in imageFiles)
            {
                try
                {
                    using var image = Image.Load<Rgba32>(imageFile);
                    effect(image);
                    var outputFileName = Path.Combine(OutputDirectory, Path.GetFileName(imageFile));
                    image.Save(outputFileName);
                    Console.WriteLine($"Aplicação de efeitos feita com sucesso: {outputFileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar imagem {imageFile}: {ex.Message}");
                }
            }

            stopwatch.Stop();
            BenchmarkResults.SequentialTime = stopwatch.ElapsedMilliseconds;
        }

        private static void ApplyEffectMultithreaded(Action<Image<Rgba32>> effect, int numThreads)
        {
            var stopwatch = Stopwatch.StartNew();

            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            string[] allowedExtensions = { ".jpeg", ".jpg", ".png"};
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
                            effect(image);
                            var outputFileName = Path.Combine(OutputDirectory, Path.GetFileName(imageFile));
                            image.Save(outputFileName);
                            Console.WriteLine($"Imagem processada com sucesso: {outputFileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao processar imagem {imageFile}: {ex.Message}");
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

            Console.WriteLine("Aplicação de efeitos com múltiplas threads concluída.");
        }

        public static void DeleteImagesEffectsFolder()
        {

            if (Directory.Exists(OutputDirectory))
            {
                try
                {
                    Console.WriteLine("Apagando a pasta das imagens com efeitos...");
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