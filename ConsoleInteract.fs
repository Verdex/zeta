
namespace Zeta

module ConsoleInterface =

    open Microsoft.FSharp.Control

    // TODO will need some sort of way to clean out the characters without
    // removing them before all the processes have used them
    let private x = 0 


module ConsoleReader =

    open System.Threading
    open System


    let initConsoleReader () = 
        let thread = new Thread( fun () -> let c = Console.ReadKey()
                                           Console.WriteLine( "blah " + new string([|c.KeyChar|])))
        thread.Start()




