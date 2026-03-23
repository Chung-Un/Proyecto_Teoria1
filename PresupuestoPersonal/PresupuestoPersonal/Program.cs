using PresupuestoPersonal.Models;
using PresupuestoPersonal.Validaciones;
using PresupuestoPersonal.Menus;

Usuario usuarioActual = null;

while(usuarioActual == null)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════╗");
    Console.WriteLine("║  SISTEMA DE PRESUPUESTO PERSONAL ║");
    Console.WriteLine("║    Chung Un Yum - 22441080       ║");
    Console.WriteLine("╚══════════════════════════════════╝");

    Console.WriteLine();
    Console.Write("Correo: ");
    string correo = Console.ReadLine();
    Console.Write("Contrasena: ");
    string password = Console.ReadLine();

    try
    {
        usuarioActual = UsuarioValidaciones.Login(correo, password);
        Console.WriteLine($"\nBienvenido, {usuarioActual.PrimerNombre}{usuarioActual.PrimerApellido}.");
        Console.WriteLine("Presionar cualquier tecla para continuar...");
        Console.ReadKey();
        MenuPrincipal.Mostrar(usuarioActual);
    }
    catch(Exception ex)
    {
        Console.WriteLine($"\nError: {ex.Message}");
        Console.WriteLine("Presione cualquier tecla para intentar de nuevo...");
        Console.ReadKey();

    }
}
