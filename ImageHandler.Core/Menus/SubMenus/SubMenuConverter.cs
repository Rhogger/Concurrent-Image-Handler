using ImageHandler.Core.Handlers;

namespace ImageHandler.Core.Menus.SubMenus;

public static class SubMenuConverter
{
    public static void DisplayConversionOptions()
    {
        var backToMain = false;
        while (!backToMain)
        {
            Console.Clear();
            Console.WriteLine("Opções:");
            Console.WriteLine("1. Converter Imagens");
            Console.WriteLine("2. Apagar Pasta das Imagens Convertidas");
            Console.WriteLine("3. Mostrar Resultados");
            Console.WriteLine("4. Voltar ao Menu Principal");

            Console.Write("Digite sua escolha: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayConversionSubmenu();
                    break;

                case "2":
                    Converter.DeleteConvertedImagesFolder();
                    break;

                case "3":
                    ShowBenchmarkResults();
                    break;

                case "4":
                    backToMain = true;
                    break;

                default:
                    Console.WriteLine("Escolha inválida. Tente novamente.");
                    break;
            }
        }
    }

    private static void DisplayConversionSubmenu()
    {
        Console.Clear();
        Console.WriteLine("Escolha o tipo de conversão:");
        Console.WriteLine("1. Sequencial");
        Console.WriteLine("2. Multithreaded");
        Console.WriteLine("3. Voltar");

        Console.Write("Digite sua escolha: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.WriteLine("Iniciando conversão sequencial...");
                Converter.ConvertImagesToWebp();
                break;

            case "2":
                DisplayThreadOptions();
                break;
            
            case "3":
                Console.WriteLine("Voltando ao menu principal.");
                break;

            default:
                Console.WriteLine("Escolha inválida. Voltando ao menu principal.");
                break;
        }
        
        Console.WriteLine("\nPressione qualquer tecla para continuar.");
        Console.ReadKey();
    }

    private static void DisplayThreadOptions()
    {
        Console.Clear();
        Console.WriteLine("Escolha a quantidade de threads para a execução:");
        Console.WriteLine("1. 1 Thread");
        Console.WriteLine("2. 2 Threads");
        Console.WriteLine("3. 3 Threads");
        Console.WriteLine("4. 4 Threads");
        Console.WriteLine("5. 6 Threads");
        Console.WriteLine("6. 8 Threads");
        Console.WriteLine("7. 10 Threads");
        Console.WriteLine("8. Executar todos os números de threads");

        Console.Write("Digite sua escolha: ");
        var threadChoice = Console.ReadLine();

        if (threadChoice == "8")
        {
            foreach (var numThreads in new[] { 1, 2, 3, 4, 6, 8, 10 })
            {
                Console.WriteLine($"Iniciando conversão com {numThreads} threads...");
                Converter.ConvertImagesToWebpMultithreaded(numThreads);
            }
        }
        else
        {
            var numThreads = threadChoice switch
            {
                "1" => 1,
                "2" => 2,
                "3" => 3,
                "4" => 4,
                "5" => 6,
                "6" => 8,
                "7" => 10,
                _ => 0
            };

            if (numThreads > 0)
            {
                Console.WriteLine($"Iniciando conversão com {numThreads} threads...");
                Converter.ConvertImagesToWebpMultithreaded(numThreads);
            }
            else
            {
                Console.WriteLine("Escolha inválida. Voltando ao menu de conversão.");
            }
        }
    }
    
    private static void ShowBenchmarkResults()
    {
        var results = Converter.BenchmarkResults;
        Console.Clear();
        Console.WriteLine("Resultados do Benchmark:");
        Console.WriteLine($"Sequencial: {results.SequentialTime} ms");
        Console.WriteLine($"1 Thread: {results.Thread1Time} ms");
        Console.WriteLine($"2 Threads: {results.Thread2Time} ms");
        Console.WriteLine($"3 Threads: {results.Thread3Time} ms");
        Console.WriteLine($"4 Threads: {results.Thread4Time} ms");
        Console.WriteLine($"6 Threads: {results.Thread6Time} ms");
        Console.WriteLine($"8 Threads: {results.Thread8Time} ms");
        Console.WriteLine($"10 Threads: {results.Thread10Time} ms");

        Console.WriteLine("\nPressione qualquer tecla para voltar.");
        Console.ReadKey();
    }
}