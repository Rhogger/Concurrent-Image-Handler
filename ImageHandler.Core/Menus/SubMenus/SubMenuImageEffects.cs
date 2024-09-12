using ImageHandler.Core.Handlers;

namespace ImageHandler.Core.Menus.SubMenus;
public static class SubMenuImageEffects
{
    public static void DisplayImageEffectsOptions()
    {
        var backToMain = false;
        while (!backToMain)
        {
            Console.Clear();
            Console.WriteLine("Opções:");
            Console.WriteLine("1. Aplicar Efeitos nas Imagens");
            Console.WriteLine("2. Apagar Pasta das Imagens com Efeitos");
            Console.WriteLine("3. Mostrar Resultados");
            Console.WriteLine("4. Voltar ao Menu Principal");

            Console.Write("Digite sua escolha: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayEffectsOptions();
                    break;

                case "2":
                    ImageEffects.DeleteImagesEffectsFolder();
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

    private static void DisplayEffectsOptions()
    {
        var backToMain = false;
        while (!backToMain)
        {
            Console.Clear();
            Console.WriteLine("Opções de Efeitos de Imagem:");
            Console.WriteLine("1. Aplicar Gaussian Blur");
            Console.WriteLine("2. Aplicar Grayscale");
            Console.WriteLine("3. Inverter Cores");
            Console.WriteLine("4. Ajustar Contraste");
            Console.WriteLine("5. Pixelar Imagem");
            Console.WriteLine("6. Voltar ao Menu Principal");

            Console.Write("Digite sua escolha: ");
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    DisplayImageEffectsSubmenu(1);
                    break;

                case "2":
                    DisplayImageEffectsSubmenu(2);
                    break;

                case "3":
                    DisplayImageEffectsSubmenu(3);
                    break;

                case "4":
                    DisplayImageEffectsSubmenu(4);
                    break;

                case "5":
                    DisplayImageEffectsSubmenu(5);
                    break;

                case "6":
                    backToMain = true;
                    break;

                default:
                    Console.WriteLine("Escolha inválida. Tente novamente.");
                    break;
            }

            if (backToMain) continue;
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }

    private static void SwitchEffect(int effectSelected, int numThreads = 0)
    {
        switch (effectSelected)
        {
            case 1:
                ApplyGaussianBlur(numThreads);
                break;

            case 2:
                ApplyGrayscale(numThreads);
                break;

            case 3:
                ApplyInvertColors(numThreads);
                break;

            case 4:
                ApplyAdjustContrast(numThreads);
                break;

            case 5:
                ApplyPixelate(numThreads);
                break;

            default:
                Console.WriteLine("Escolha inválida. Tente novamente.");
                break;
        }
    }

    private static void DisplayImageEffectsSubmenu(int effectSelected)
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
                Console.WriteLine("Iniciando aplicação de efeito sequencial...");
                SwitchEffect(effectSelected);
                break;

            case "2":
                DisplayThreadOptions(effectSelected);
                break;
            
            case "3":
                Console.WriteLine("Voltando ao menu principal.");
                break;

            default:
                Console.WriteLine("Escolha inválida. Voltando ao menu principal.");
                break;
        }
    }
    
    private static void DisplayThreadOptions(int effectSelected)
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
                Console.WriteLine($"Iniciando a aplicação de efeitos com {numThreads} threads...");
                SwitchEffect(effectSelected, numThreads);
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
                Console.WriteLine($"Iniciando a aplicação de efeitos com {numThreads} threads...");
                SwitchEffect(effectSelected, numThreads);
            }
            else
            {
                Console.WriteLine("Escolha inválida. Voltando ao menu de conversão.");
            }
        }
    }
        
    private static void ShowBenchmarkResults()
    {
        var results = ImageEffects.BenchmarkResults;
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

    private static void ApplyGaussianBlur(int numThreads)
    {
        Console.Clear();
        Console.Write("Digite o valor de sigma para o blur (default 3): ");
        if (float.TryParse(Console.ReadLine(), out float sigma))
        {
            ImageEffects.ApplyGaussianBlur(sigma, numThreads);
        }
        else
        {
            Console.WriteLine("Valor inválido. Usando sigma padrão (3).");
            ImageEffects.ApplyGaussianBlur(numThreads: numThreads);
        }
    }
    
    private static void ApplyGrayscale(int numThreads)
    {
        Console.Clear();
        Console.WriteLine("Aplicando efeito de Grayscale...");
        ImageEffects.ApplyGrayscale(numThreads);
    }

    private static void ApplyInvertColors(int numThreads)
    {
        Console.Clear();
        Console.WriteLine("Invertendo cores da imagem...");
        ImageEffects.ApplyInvertColors(numThreads);
    }

    private static void ApplyAdjustContrast(int numThreads)
    {
        Console.Clear();
        Console.Write("Digite o valor de contraste (default 1.2): ");
        if (float.TryParse(Console.ReadLine(), out float contrast))
        {
            ImageEffects.ApplyContrast(contrast, numThreads);
        }
        else
        {
            Console.WriteLine("Valor inválido. Usando contraste padrão (1.2).");
            ImageEffects.ApplyContrast(numThreads: numThreads);
        }
    }

    private static void ApplyPixelate(int numThreads)
    {
        Console.Clear();
        Console.Write("Digite o tamanho do pixel para o efeito (default 8): ");
        if (int.TryParse(Console.ReadLine(), out int pixelSize))
        {
            ImageEffects.ApplyPixelate(pixelSize, numThreads);
        }
        else
        {
            Console.WriteLine("Valor inválido. Usando tamanho padrão (8).");
            ImageEffects.ApplyPixelate(numThreads: numThreads);
        }
    }
}