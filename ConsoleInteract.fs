
namespace Zeta

module ConsoleTypes =

    open Microsoft.FSharp.Control
    open System

    type KeyPressEvent = KeyPressEvent of char * DateTime 
    type ConsoleChar = { Char : char
                         ForeColor : ConsoleColor
                         BackColor : ConsoleColor
                       }
    type ConsolePoint = { X : int
                          Y : int
                        }

    type ConsoleMessage = PostKey of KeyPressEvent 
                        | GetLastNKeys of AsyncReplyChannel<list<KeyPressEvent>> * int32
                        | WriteToConsole of list<ConsoleChar * ConsolePoint> 

module ConsoleInterface =

    open ConsoleTypes
    open Microsoft.FSharp.Control

    // TODO replace list with circular buffer
    let mutable private cs = [] 

    let private AcceptPostAndTrim kpe = 
        if cs.Length > 1000 then
            cs <- List.take 1000 cs 
            cs <- kpe :: cs 
        else 
            cs <- kpe :: cs 

    let private TakeAtLeast n ( l : list<KeyPressEvent> ) = 
        let total = if n < l.Length then n else l.Length in
            List.take total l 

    let consoleMailbox = new MailboxProcessor<ConsoleMessage>( fun inbox -> 
        let rec loop () = 
            async { let! msg = inbox.Receive()
                    match msg with
                        | PostKey kpe -> AcceptPostAndTrim kpe
                        | GetLastNKeys( reply, count ) -> reply.Reply( TakeAtLeast count cs ) 
                        // TODO get height and width
                        // TODO set console chars
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
                consoleMailBox.Post( PostKey( KeyPressEvent( c.KeyChar, DateTime.Now ) ))
            )
                                           
        thread.Start()

    let killConsoleReader () = lock l ( fun () -> shutdownFlag <- true )

// TODO need a Console init module to setup all of the console stuff



