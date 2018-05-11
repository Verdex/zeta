
namespace Zeta

module ConsoleTypes =

    open Microsoft.FSharp.Control

    type KeyPressEvent = KeyPressEvent of char * int64
    type ConsoleMessage = PostKey of KeyPressEvent 
                        | GetNKeys of AsyncReplyChannel<list<KeyPressEvent>> * uint32

module ConsoleInterface =

    open ConsoleTypes
    open Microsoft.FSharp.Control

    // TODO will need some sort of way to clean out the characters without
    // removing them before all the processes have used them
    let mutable private cs = [] 

    let consoleMailbox = new MailboxProcessor<ConsoleMessage>( fun inbox -> 
        let rec loop () = 
            async { let! msg = inbox.Receive()
                    match msg with
                        | PostKey kpe -> cs <- kpe :: cs
                        | GetNKeys( reply, count ) -> reply.Reply( cs ) // TODO only return count character 

                    return! loop() }
        loop() )

    let initConsoleInterface () = consoleMailbox.Start()


module ConsoleReader =

    open ConsoleTypes
    open Microsoft.FSharp.Control
    open System.Threading
    open System

    let initConsoleReader (consoleMailBox : MailboxProcessor<ConsoleMessage> ) =  
        let thread = new Thread( fun () -> 
            let rec loop () = // TODO need to have a way to kill the loop (maybe use while instead) 
                let c = Console.ReadKey() // TODO need to hide keys pressed
                let tick = DateTime.Now.Ticks
                consoleMailBox.Post( PostKey( KeyPressEvent( c.KeyChar, tick ) ))
                loop() 
            loop() )
                                           
        thread.Start()

    // TODO need a kill console reader function

// TODO need a Console init module to setup all of the console stuff



