
namespace Zeta

module ConsoleTypes =

    open Microsoft.FSharp.Control

    type KeyPressEvent = KeyPressEvent of char * int64
    type ConsoleMessage = PostKey of KeyPressEvent 
                        | GetLastNKeys of AsyncReplyChannel<list<KeyPressEvent>> * int32
                        // TODO Get Keys From so many seconds ago

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
                        | GetLastNKeys( reply, count ) -> reply.Reply( List.take count cs ) 
                    return! loop() }
        loop() )

    let initConsoleInterface () = consoleMailbox.Start()


module ConsoleReader =

    open ConsoleTypes
    open Microsoft.FSharp.Control
    open System.Threading
    open System

    let private l = new Object()
    let mutable private shutdownFlag = false
    let private initShutdownFlag () = lock l ( fun () -> shutdownFlag <- false )
    let private shutdown () = lock l ( fun () -> shutdownFlag ) 

    let initConsoleReader (consoleMailBox : MailboxProcessor<ConsoleMessage> ) =  
        initShutdownFlag()
        let thread = new Thread( fun () -> 
            while shutdown() = false do
                let c = Console.ReadKey( true ) 
                let tick = DateTime.Now.Ticks
                consoleMailBox.Post( PostKey( KeyPressEvent( c.KeyChar, tick ) ))
            )
                                           
        thread.Start()

    let killConsoleReader () = lock l ( fun () -> shutdownFlag <- true )


// TODO need a Console init module to setup all of the console stuff



