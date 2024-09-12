using ImageHandler.Core.Menus.SubMenus;

namespace ImageHandler.Core.Menus;

public static class MainMenu
{
    public static void Display()
    {
        var keepRunning = true;

        while (keepRunning)
        {
            Console.Clear();
            Console.WriteLine("Escolha uma operação:");
            Console.WriteLine("1. Converter Imagens para WebP");
            Console.WriteLine("2. Redimensionar Imagens");
            Console.WriteLine("3. Aplicar Filtros em Imagens");
            Console.WriteLine("4. Sair");

            Console.Write("Digite sua escolha: ");
            var mainChoice = Console.ReadLine();

            switch (mainChoice)
            {
                case "1":
                    SubMenuConverter.DisplayConversionOptions();
                    break;

                case "2":
                    SubMenuResizer.DisplayResizingOptions();
                    break;
                
                case "3":
                    SubMenuImageEffects.DisplayImageEffectsOptions();
                    break;

                case "4":
                    Console.WriteLine("Encerrando o programa.");
                    keepRunning = false;
                    break;

                default:
                    Console.WriteLine("Escolha inválida. Tente novamente.");
                    break;
            }
        }
    }
}