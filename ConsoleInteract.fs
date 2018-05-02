
namespace Zeta

module ConsoleTypes =

    open Microsoft.FSharp.Control

    type KeyPressEvent = KeyPressEvent of char * long
    type InterfaceMessage = PostKey of KeyPressEvent 
                          | GetNKeys of AsyncReplyChannel

module ConsoleInterface =

    open Microsoft.FSharp.Control

    // TODO will need some sort of way to clean out the characters without
    // removing them before all the processes have used them
    let mutable private x = 0 


module ConsoleReader =

    open System.Threading
    open System


    let initConsoleReader () =  // TODO init function takes console interface channel as param?
        let thread = new Thread( fun () -> let c = Console.ReadKey()
                                           Console.WriteLine( "blah " + new string([|c.KeyChar|])))
        thread.Start()




